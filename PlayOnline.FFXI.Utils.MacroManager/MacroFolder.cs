// $Id$

using System;
using System.Collections;
using System.Xml;

namespace PlayOnline.FFXI {

  public class MacroFolder {

    // Folder Creation

    public MacroFolder() : this(null) {
    }

    public MacroFolder(string Name) {
      this.Name_    = Name;
      this.Folders_ = new MacroFolderCollection();
      this.Macros_  = new MacroCollection();
    }

    public MacroFolder Clone() {
    MacroFolder MF = new MacroFolder(this.Name_);
      foreach (MacroFolder SubFolder in this.Folders_)
	MF.Folders_.Add(SubFolder.Clone());
      foreach (Macro M in this.Macros_)
	MF.Macros_.Add(M.Clone());
      return MF;
    }

    public void Lock() {
      this.Locked_ = true;
    }

    public void LockTree() {
      this.Lock();
      foreach (MacroFolder MF in this.Folders_)
	MF.LockTree();
    }

    public void Unlock() {
      this.Locked_ = false;
    }

    public void UnlockTree() {
      this.Unlock();
      foreach (MacroFolder MF in this.Folders_)
	MF.LockTree();
    }

    public virtual bool CanSave { get { return false; } }

    public virtual bool Save()  {
      return false;
    }

    #region Data Members

    public string Name {
      get { return this.Name_;  }
      set { this.Name_ = value; }
    }

    public MacroFolderCollection Folders { get { return this.Folders_; } }
    public MacroCollection       Macros  { get { return this.Macros_;  } }
    public bool                  Locked  { get { return this.Locked_;  } }

    #region Private Fields

    private string                         Name_;
    private readonly MacroFolderCollection Folders_;
    private readonly MacroCollection       Macros_;
    private bool                           Locked_;

    #endregion

    #endregion

  }

}
