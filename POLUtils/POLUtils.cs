using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

using PlayOnline.Core;
using PlayOnline.FFXI;

namespace PlayOnline.Utils {

  internal class CultureChoice {

    public string Name {
      get {
	if (this.Culture_.LCID == CultureInfo.InvariantCulture.LCID)
	  return "Default";
	return String.Format("[{0}] {1}", this.Culture_.Name, this.Culture_.NativeName);
      }
    }

    private CultureInfo Culture_;
    public CultureInfo Culture { get { return this.Culture_; } }

    public CultureChoice(CultureInfo Culture) {
      this.Culture_ = Culture;
    }

    private static CultureChoice Current_ = null;
    public static CultureChoice Current {
      get { return CultureChoice.Current_; }
      set {
	CultureChoice.Current_ = value;
	Thread.CurrentThread.CurrentUICulture = value.Culture;
      RegistryKey SettingsKey = Registry.LocalMachine.CreateSubKey(@"Software\Pebbles\POLUtils");
	if (SettingsKey != null) {
	  SettingsKey.SetValue("UI Culture", value.Culture.Name);
	  SettingsKey.Close();
	}
      }
    }

  }

  public class POLUtils {

    internal static bool KeepGoing;

    internal static ArrayList AvailableCultures;

    [STAThread]
    public static int Main(string[] Arguments) {
      if (POL.AvailableRegions == POL.Region.None)
	MessageBox.Show(I18N.GetText("Text:NoPOL"), I18N.GetText("Caption:NoPOL"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
      else {
	POLUtils.AvailableCultures = new ArrayList();
      string LastCulture = String.Empty;
	{
	RegistryKey SettingsKey = Registry.LocalMachine.OpenSubKey(@"Software\Pebbles\POLUtils");
	  if (SettingsKey != null) {
	    LastCulture = SettingsKey.GetValue("UI Culture", "") as string;
	    SettingsKey.Close();
	  }
	}
	// Detect Available Languages
	POLUtils.AvailableCultures.Add(new CultureChoice(CultureInfo.InvariantCulture));
	foreach (CultureInfo CI in CultureInfo.GetCultures(CultureTypes.AllCultures)) {
	  if (CI.Name != String.Empty && Directory.Exists(Path.Combine(Application.StartupPath, CI.Name))) {
	  CultureChoice CC = new CultureChoice(CI);
	    POLUtils.AvailableCultures.Add(CC);
	    if (LastCulture != String.Empty && LastCulture == CC.Culture.Name)
	      CultureChoice.Current = CC;
	  }
	}
	if (CultureChoice.Current == null) // if none configured, default to invariant culture
	  CultureChoice.Current = POLUtils.AvailableCultures[0] as CultureChoice;
	// The loop is for the benefit of language change
	POLUtils.KeepGoing = true;
	while (POLUtils.KeepGoing) {
	  POLUtils.KeepGoing = false;
	  Application.Run(new POLUtilsUI());
	}
      }
      return 0;
    }
  }

}
