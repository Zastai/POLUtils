using System;
using System.Collections;

namespace PlayOnline.FFXI {

  public class CharacterCollection : ReadOnlyCollectionBase {

    public void Add     (Character C) {        this.InnerList.Add     (C); }
    public bool Contains(Character C) { return this.InnerList.Contains(C); }
    public int  IndexOf (Character C) { return this.InnerList.IndexOf (C); }
    public void Remove  (Character C) {        this.InnerList.Remove  (C); }

    public Character this[int Index] {
      get { return this.InnerList[Index] as Character; }
      set { this.InnerList[Index] = value; }
    }

    public Character this[string ID] {
      get {
	foreach (Character C in this.InnerList) {
	  if (C.ID == ID)
	    return C;
	}
	return null;
      }
    }

  }

  public class MacroFolderCollection : CollectionBase {

    public void Add     (MacroFolder MF) {        this.InnerList.Add     (MF); }
    public bool Contains(MacroFolder MF) { return this.InnerList.Contains(MF); }
    public int  IndexOf (MacroFolder MF) { return this.InnerList.IndexOf (MF); }
    public void Remove  (MacroFolder MF) {        this.InnerList.Remove  (MF); }

    public MacroFolder this[int Index] {
      get { return this.InnerList[Index] as MacroFolder; }
      set { this.InnerList[Index] = value; }
    }

  }

  public class MacroCollection : CollectionBase {

    public void Add     (Macro M) {        this.InnerList.Add     (M); }
    public bool Contains(Macro M) { return this.InnerList.Contains(M); }
    public int  IndexOf (Macro M) { return this.InnerList.IndexOf (M); }
    public void Remove  (Macro M) {        this.InnerList.Remove  (M); }

    public Macro this[int Index] {
      get { return this.InnerList[Index] as Macro; }
      set { this.InnerList[Index] = value; }
    }

  }

  public class MessageGroupCollection : ReadOnlyCollectionBase {

    public void Add     (AutoTranslator.MessageGroup MG) {        this.InnerList.Add     (MG); }
    public bool Contains(AutoTranslator.MessageGroup MG) { return this.InnerList.Contains(MG); }
    public int  IndexOf (AutoTranslator.MessageGroup MG) { return this.InnerList.IndexOf (MG); }
    public void Remove  (AutoTranslator.MessageGroup MG) {        this.InnerList.Remove  (MG); }

  }

  public class MessageCollection : ReadOnlyCollectionBase {

    public void Add     (AutoTranslator.Message M) {        this.InnerList.Add     (M); }
    public bool Contains(AutoTranslator.Message M) { return this.InnerList.Contains(M); }
    public int  IndexOf (AutoTranslator.Message M) { return this.InnerList.IndexOf (M); }
    public void Remove  (AutoTranslator.Message M) {        this.InnerList.Remove  (M); }

  }

}
