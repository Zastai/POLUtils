using System;
using System.IO;
using Microsoft.Win32;

using PlayOnline.Core;

namespace PlayOnline.FFXI {

  public class Character {

    internal Character(string ContentID) {
      this.ID_ = ContentID;
      this.DataDir_ = Path.Combine(POL.GetApplicationPath(AppID.FFXI), Path.Combine("User", ContentID));
    }

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
      get {
	if (this.MacroBars_ == null)
	  this.LoadMacroBars();
	return this.MacroBars_;
      }
    }

    #region Private Fields

    private string                ID_;
    private string                DataDir_;
    private MacroFolderCollection MacroBars_;

    #endregion

    #endregion

    public override string ToString() { return this.Name; }

    public FileStream OpenUserFile(string FileName, FileMode Mode, FileAccess Access) {
    FileStream Result = null;
      try {
	Result = new FileStream(Path.Combine(this.DataDir_, FileName), Mode, Access, FileShare.Read);
      } catch (Exception E) { Console.WriteLine("{0}", E.ToString()); }
      return Result;
    }

    public void SaveMacroBar(int Index) {
      this.MacroBars_[Index].WriteToMacroBar(Path.Combine(this.DataDir_, String.Format("mcr{0:#}.dat", Index)));
    }

    private void LoadMacroBars() {
      this.MacroBars_ = new MacroFolderCollection();
      for (int i = 0; i < 10; ++i) {
      MacroFolder MF = MacroFolder.LoadFromMacroBar(Path.Combine(this.DataDir_, String.Format("mcr{0:#}.dat", i)));
	MF.Name = String.Format(I18N.GetText("MacroBarLabel"), i + 1);
	this.MacroBars_.Add(MF);
      }
    }

  }

}