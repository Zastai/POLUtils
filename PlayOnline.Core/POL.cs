using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace PlayOnline.Core {

  public struct AppID {
    public const string POLViewer   = "1000";
    public const string FFXI        = "0001";
    public const string TetraMaster = "0002";
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

    public static string RegionText(Region R) {
      switch (R) {
	case POL.Region.Europe:       return I18N.GetText("RegionEU");
	case POL.Region.Japan:        return I18N.GetText("RegionJP");
	case POL.Region.NorthAmerica: return I18N.GetText("RegionUS");
	case POL.Region.None:         return I18N.GetText("NoRegion");
	default:                      return I18N.GetText("BadRegion");
      }
    }

    private static Region AvailableRegions_ = Region.None;
    private static Region SelectedRegion_   = Region.None;

    public static void DetectRegions() {
    RegistryKey POLKey;
      POLKey = Registry.LocalMachine.OpenSubKey(@"Software\PlayOnline\InstallFolder");
      if (POLKey != null) {
	POLKey.Close();
	POL.AvailableRegions_ |= Region.Japan;
      }
      POLKey = Registry.LocalMachine.OpenSubKey(@"Software\PlayOnlineUS\InstallFolder");
      if (POLKey != null) {
	POLKey.Close();
	POL.AvailableRegions_ |= Region.NorthAmerica;
      }
      POLKey = Registry.LocalMachine.OpenSubKey(@"Software\PlayOnlineEU\InstallFolder");
      if (POLKey != null) {
	POLKey.Close();
	POL.AvailableRegions_ |= Region.Europe;
      }
      POLKey = Registry.LocalMachine.OpenSubKey(@"Software\Pebbles\POLUtils");
      if (POLKey != null) {
      string UserRegion = POLKey.GetValue("Region", "None") as string;
	try {
	  POL.SelectedRegion_ = (Region) Enum.Parse(typeof(Region), UserRegion, true);
	}
	catch {
	  POL.SelectedRegion_ = Region.None;
	}
	POLKey.Close();
      }
      if ((POL.AvailableRegions_ & POL.SelectedRegion_) != POL.SelectedRegion_)
	POL.SelectedRegion_ = Region.None;
      if (POL.SelectedRegion_ == Region.None) {
	if ((POL.AvailableRegions_ & Region.Japan) != 0)
	  POL.SelectedRegion_ = Region.Japan;
	else if ((POL.AvailableRegions_ & Region.NorthAmerica) != 0)
	  POL.SelectedRegion_ = Region.NorthAmerica;
	else if ((POL.AvailableRegions_ & Region.Europe) != 0)
	  POL.SelectedRegion_ = Region.Europe;
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
    RegistryKey POLKey = Registry.LocalMachine.CreateSubKey(@"Software\Pebbles\POLUtils");
      if (POLKey != null) {
	POLKey.SetValue("Region", POL.SelectedRegion_.ToString());
	POLKey.Close();
      }
    }

    public static string GetApplicationPath(string ID) {
      if (POL.SelectedRegion_ == Region.None)
	return null;
    RegistryKey POLKey = null;
      switch (POL.SelectedRegion_) {
	case Region.Japan:
	  POLKey = Registry.LocalMachine.OpenSubKey(@"Software\PlayOnline\InstallFolder");
	  break;
	case Region.NorthAmerica:
	  POLKey = Registry.LocalMachine.OpenSubKey(@"Software\PlayOnlineUS\InstallFolder");
	  break;
	case Region.Europe:
	  POLKey = Registry.LocalMachine.OpenSubKey(@"Software\PlayOnlineEU\InstallFolder");
	  break;
      }
      if (POLKey == null)
	return null;
    string InstallPath = POLKey.GetValue(ID, null) as string;
      POLKey.Close();
      return InstallPath;
    }

    public static bool IsAppInstalled(string ID) {
      return (POL.GetApplicationPath(ID) != null);
    }

  }

}