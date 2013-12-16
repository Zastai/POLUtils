// $Id$

// Copyright © 2004-2012 Tim Van Holder, Nevin Stepan
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace PlayOnline.Core {

  // This probably should have a more fitting location.
  public delegate void AnonymousMethod();

  public struct AppID {
    public const string POLViewer   = "1000";
    public const string FFXI        = "0001";
    public const string TetraMaster = "0002";
    public const string FFXITC      = "0015";
    // FrontMission Online = ?
    // That Mahjongg thing = ?
  }

  public class POL {

    [Flags]
    public enum Region {
      None         = 0x00,
      Japan        = 0x01,
      NorthAmerica = 0x02,
      Europe       = 0x04,
    }

    private static Region AvailableRegions_ = Region.None;
    private static Region SelectedRegion_   = Region.None;

    public static void DetectRegions() {
      // Get configured region
      using (RegistryKey SettingsKey = POL.OpenPOLUtilsConfigKey()) {
        if (SettingsKey != null) {
        string UserRegion = SettingsKey.GetValue("Region", "None") as string;
          try {
            POL.SelectedRegion_ = (Region) Enum.Parse(typeof(Region), UserRegion, true);
          }
          catch {
            POL.SelectedRegion_ = Region.None;
          }
        }
      }
      // Check for installed POL software
      foreach (Region R in Enum.GetValues(typeof(Region))) {
        using (RegistryKey POLKey = POL.OpenRegistryKey(R, "InstallFolder")) {
          if (POLKey != null)
            POL.AvailableRegions_ |= R;
        }
      }
      // If user's choice is not available, clear that selection
      if ((POL.AvailableRegions_ & POL.SelectedRegion_) != POL.SelectedRegion_)
        POL.SelectedRegion_ = Region.None;
      // Select a region based on what's available
      if (POL.SelectedRegion_ == Region.None) {
        if ((POL.AvailableRegions_ & Region.NorthAmerica) != 0)
          POL.SelectedRegion_ = Region.NorthAmerica;
        else if ((POL.AvailableRegions_ & Region.Europe) != 0)
          POL.SelectedRegion_ = Region.Europe;
        else if ((POL.AvailableRegions_ & Region.Japan) != 0)
          POL.SelectedRegion_ = Region.Japan;
      }
    }

    public static Region AvailableRegions {
      get {
        POL.DetectRegions();
        return POL.AvailableRegions_;
      }
    }

    public static Region SelectedRegion {
      get {
        return POL.SelectedRegion_;
      }
      set {
        if ((POL.AvailableRegions & value) == value)
          POL.SelectedRegion_ = value;
        else
          throw new ArgumentOutOfRangeException("SelectedRegion", value, I18N.GetText("POLRegionNotInstalled"));
      }
    }

    public static bool MultipleRegionsAvailable {
      get {
        return (POL.AvailableRegions_ != Region.None         &&
                POL.AvailableRegions_ != Region.Japan        &&
                POL.AvailableRegions_ != Region.NorthAmerica &&
                POL.AvailableRegions_ != Region.Europe);
      }
    }

    public static void ChooseRegion(Form Parent) {
      POL.DetectRegions();
      if (POL.MultipleRegionsAvailable) {
        using (ChooseRegionDialog CRD = new ChooseRegionDialog())
          CRD.ShowDialog(Parent);
      }
      else // No multiple regions installed? No choice to be made then!
        POL.SelectedRegion_ = POL.AvailableRegions_;
      using (RegistryKey POLKey = POL.OpenPOLUtilsConfigKey()) {
        if (POLKey != null)
          POLKey.SetValue("Region", POL.SelectedRegion_.ToString());
      }
    }

    public static string GetApplicationPath(string ID) {
      return POL.GetApplicationPath(ID, POL.SelectedRegion_);
    }

    public static string GetApplicationPath(string ID, Region Region) {
      if ((POL.AvailableRegions & Region) == 0)
        return null;
    RegistryKey POLKey = POL.OpenRegistryKey(Region, "InstallFolder");
      if (POLKey == null)
        return null;
    string InstallPath = POLKey.GetValue(ID, null) as string;
      POLKey.Close();
      return InstallPath;
    }

    public static bool IsAppInstalled(string ID) {
      return POL.IsAppInstalled(ID, POL.SelectedRegion_);
    }

    public static bool IsAppInstalled(string ID, Region Region) {
      return (POL.GetApplicationPath(ID, Region) != null);
    }

    public static RegistryKey OpenAppConfigKey(string ID, bool writable = false) {
      return POL.OpenAppConfigKey(ID, POL.SelectedRegion_, writable);
    }

    public static RegistryKey OpenAppConfigKey(string ID, Region Region, bool writable = false) {
    string BaseKey;
      switch (Region) {
        case Region.Europe:       BaseKey = "SquareEnix"; break;
        case Region.Japan:        BaseKey = "SQUARE";     break;
        case Region.NorthAmerica: BaseKey = "SquareEnix"; break;
        default: return null;
      }
    string AppKey;
           if (ID == AppID.FFXI)        AppKey = "FinalFantasyXI";
      else if (ID == AppID.TetraMaster) AppKey = "TetraMaster";
      else if (ID == AppID.POLViewer)   AppKey = "PlayOnlineViewer";
      else return null;
      return POL.OpenRegistryKey(Region, Path.Combine(BaseKey, AppKey), writable);
    }

    private static RegistryKey OpenRegistryKey(string KeyName, bool writable = false) {
      return POL.OpenRegistryKey(POL.SelectedRegion_, KeyName, writable);
    }

    private static RegistryKey OpenRegistryKey(Region region, string name, bool writable = false) {
    string subKey;
      {
        var polKey = "PlayOnline";
        switch (region) {
          case Region.Europe:       polKey += "EU"; break;
          case Region.Japan:                        break;
          case Region.NorthAmerica: polKey += "US"; break;
          default:
            return null;
        }
        subKey = Path.Combine(polKey, name);
      }
      try {
        using (var w64Root = Registry.LocalMachine.OpenSubKey(@"Software\WOW6432Node")) {
          if (w64Root != null) {
            var w64Key = w64Root.OpenSubKey(subKey, writable);
            if (w64Key != null)
              return w64Key;
          }
        }
      } catch { }
      try {
        return Registry.LocalMachine.OpenSubKey(Path.Combine("Software", subKey), writable);
      } catch {
        return null;
      }
    }

    public static RegistryKey OpenPOLUtilsConfigKey(string name = null) {
      try {
        var fullname = @"Software\Pebbles\POLUtils\";
        if (name != null)
          fullname += name;
        return Registry.CurrentUser.CreateSubKey(fullname);
      } catch { }
      return null;
    }

  }

}