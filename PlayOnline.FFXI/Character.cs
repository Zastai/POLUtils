using System;
using System.IO;
using Microsoft.Win32;

using PlayOnline.Core;

namespace PlayOnline.FFXI {

  public class Character {

    #region Data Members

    public string ID { get { return this.ID_; } }

    public string Name {
      get {
      string value = String.Format("Unknown Character ({0})", this.ID_);
      RegistryKey NameMappings = Registry.LocalMachine.OpenSubKey(@"Software\Pebbles\POLUtils\Character Names");
	if (NameMappings != null) {
	  value = NameMappings.GetValue(this.ID_, value) as string;
	  NameMappings.Close();
	}
	return value;
      }
      set {
      RegistryKey NameMappings = Registry.LocalMachine.CreateSubKey(@"Software\Pebbles\POLUtils\Character Names");
	if (NameMappings != null) {
	  if (value == null)
	    NameMappings.DeleteValue(this.ID_, false);
	  else
	    NameMappings.SetValue(this.ID_, value);
	  NameMappings.Close();
	}
      }
    }

    public MacroFolderCollection MacroBars {
      get { return this.MacroBars_; }
    }

    #region Private Fields

    private string                ID_;
    private string                DataDir_;
    private MacroFolderCollection MacroBars_;

    #endregion

    #endregion

    internal Character(string ContentID) {
      this.ID_ = ContentID;
      this.DataDir_ = Path.Combine(POL.GetApplicationPath(AppID.FFXI), Path.Combine("User", ContentID));
      this.MacroBars_ = new MacroFolderCollection();
      for (int i = 0; i < 10; ++i) {
      MacroFolder MF = MacroFolder.LoadFromMacroBar(Path.Combine(this.DataDir_, String.Format("mcr{0:#}.dat", i)));
	MF.Name = String.Format("Macro Bar #{0}", i + 1);
	this.MacroBars_.Add(MF);
      }
    }

    public void SaveMacroBar(int Index) {
      this.MacroBars_[Index].WriteToMacroBar(Path.Combine(this.DataDir_, String.Format("mcr{0:#}.dat", Index)));
    }

  }

}