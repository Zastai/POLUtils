// $Id$

using System.IO;

namespace PlayOnline.FFXI {

  public class CharacterMacros : SpecialMacroFolder {
  
    public CharacterMacros(Character C) : base(C.Name) {
    MacroBook[] Books = MacroBook.Load(C);
      if (Books != null)
	this.Folders.AddRange(Books);
      else {
	for (int j = 0; j < 10; ++j)
	  this.Folders.Add(MacroSet.Load(C.GetUserFileName(string.Format("mcr{0:#####}.dat", j)), string.Format("Macro Set {0}", j + 1)));
      }
    }

    public override bool CanSave { get { return true; } }

    public override bool Save() {
    bool OK = true;
      foreach (MacroFolder MF in this.Folders) {
	if (MF.CanSave)
	  OK = OK && MF.Save();
      }
      return OK;
    }

  }

}
