// $Id$

using System;
using System.Collections.Generic;
using System.IO;

using PlayOnline.Core;

namespace PlayOnline.FFXI {

  public class DialogTableEntry : Thing {

    public DialogTableEntry() {
      // Clear fields
      this.Clear();
    }

    public override string ToString() {
      return this.Text_;
    }

    public override List<PropertyPages.IThing> GetPropertyPages() {
      return base.GetPropertyPages();
    }

    #region Fields

    public static List<string> AllFields {
      get {
	return new List<string>(new string[] {
	  "index",
	  "text",
	});
      }
    }

    public override List<string> GetAllFields() {
      return DialogTableEntry.AllFields;
    }

    #region Data Fields

    private Nullable<int> Index_;
    private string        Text_;
    
    #endregion

    public override void Clear() {
      this.Index_ = null;
      this.Text_  = null;
    }

    #endregion

    #region Field Access

    public override bool HasField(string Field) {
      switch (Field) {
	case "index": return this.Index_.HasValue;
	case "text":  return (this.Text_ != null);
	default:      return false;
      }
    }

    public override string GetFieldText(string Field) {
      switch (Field) {
	case "index": return (!this.Index_.HasValue ? String.Empty : String.Format("{0:00000}", this.Index_.Value));
	case "text":  return this.Text_;
	default:      return null;
      }
    }

    public override object GetFieldValue(string Field) {
      switch (Field) {
	case "index": return (!this.Index_.HasValue ? null : (object) this.Index_.Value);
	case "text":  return this.Text_;
	default:      return null;
      }
    }

    protected override void LoadField(string Field, System.Xml.XmlElement Node) {
      switch (Field) {
	case "index": this.Index_ = (int) this.LoadUnsignedIntegerField(Node); break;
	case "text":  this.Text_  =       this.LoadTextField           (Node); break;
      }
    }

    #endregion

    #region ROM File Reading

    public bool Read(BinaryReader BR, long EntryStart, long EntryEnd) {
      return this.Read(BR, null, EntryStart, EntryEnd);
    }

    public bool Read(BinaryReader BR, Nullable<int> Index, long EntryStart, long EntryEnd) {
      this.Clear();
      this.Index_ = Index;
      try {
	BR.BaseStream.Seek(4 + EntryStart, SeekOrigin.Begin);
      byte[] TextBytes = BR.ReadBytes((int) (EntryEnd - EntryStart));
	for (int i = 0; i < TextBytes.Length; ++i)
	  TextBytes[i] ^= 0x80; // <= Evil encryption-breaking!
	this.Text_ = String.Empty;
      FFXIEncoding E = new FFXIEncoding();
      int LastPos = 0;
	for (int i = 0; i < TextBytes.Length; ++i) {
	  if (TextBytes[i] == 0x07) { // Line Break
	    if (LastPos < i)
	      this.Text_ += E.GetString(TextBytes, LastPos, i - LastPos);
	    this.Text_ += "\r\n";
	    LastPos = i + 1;
	  }
	  else if (TextBytes[i] == 0x08) { // Character Name (You)
	    if (LastPos < i)
	      this.Text_ += E.GetString(TextBytes, LastPos, i - LastPos);
	    this.Text_ += String.Format("{0}Player Name{1}", FFXIEncoding.SpecialMarkerStart, FFXIEncoding.SpecialMarkerEnd);
	    LastPos = i + 1;
	  }
	  else if (TextBytes[i] == 0x09) { // Character Name (They)
	    if (LastPos < i)
	      this.Text_ += E.GetString(TextBytes, LastPos, i - LastPos);
	    this.Text_ += String.Format("{0}Speaker Name{1}", FFXIEncoding.SpecialMarkerStart, FFXIEncoding.SpecialMarkerEnd);
	    LastPos = i + 1;
	  }
	  else if (TextBytes[i] == 0x0a && i + 1 < TextBytes.Length) {
	    if (LastPos < i)
	      this.Text_ += E.GetString(TextBytes, LastPos, i - LastPos);
	    this.Text_ += String.Format("{0}Numeric Parameter {1}{2}", FFXIEncoding.SpecialMarkerStart, TextBytes[i + 1], FFXIEncoding.SpecialMarkerEnd);
	    LastPos = i + 2;
	    ++i;
	  }
	  else if (TextBytes[i] == 0x0b) { // Indicates that the lines after this are in a prompt window
	    if (LastPos < i)
	      this.Text_ += E.GetString(TextBytes, LastPos, i - LastPos);
	    this.Text_ += String.Format("{0}Selection Dialog{1}", FFXIEncoding.SpecialMarkerStart, FFXIEncoding.SpecialMarkerEnd);
	    LastPos = i + 1;
	  }
	  else if (TextBytes[i] == 0x0c && i + 1 < TextBytes.Length) {
	    if (LastPos < i)
	      this.Text_ += E.GetString(TextBytes, LastPos, i - LastPos);
	    this.Text_ += String.Format("{0}Multiple Choice (Parameter {1}){2}", FFXIEncoding.SpecialMarkerStart, TextBytes[i + 1], FFXIEncoding.SpecialMarkerEnd);
	    LastPos = i + 2;
	    ++i;
	  }
	  else if (TextBytes[i] == 0x19 && i + 1 < TextBytes.Length) {
	    if (LastPos < i)
	      this.Text_ += E.GetString(TextBytes, LastPos, i - LastPos);
	    this.Text_ += String.Format("{0}Item Parameter {1}{2}", FFXIEncoding.SpecialMarkerStart, TextBytes[i + 1], FFXIEncoding.SpecialMarkerEnd);
	    LastPos = i + 2;
	    ++i;
	  }
	  else if (TextBytes[i] == 0x1a && i + 1 < TextBytes.Length) {
	    if (LastPos < i)
	      this.Text_ += E.GetString(TextBytes, LastPos, i - LastPos);
	    this.Text_ += String.Format("{0}Marker: {1:X2}{2:X2}{3}", FFXIEncoding.SpecialMarkerStart, TextBytes[i + 0], TextBytes[i + 1], FFXIEncoding.SpecialMarkerEnd);
	    LastPos = i + 2;
	    ++i;
	  }
	  else if (TextBytes[i] == 0x1e && i + 1 < TextBytes.Length) {
	    if (LastPos < i)
	      this.Text_ += E.GetString(TextBytes, LastPos, i - LastPos);
	    this.Text_ += String.Format("{0}Set Color #{1}{2}", FFXIEncoding.SpecialMarkerStart, TextBytes[i + 1], FFXIEncoding.SpecialMarkerEnd);
	    LastPos = i + 2;
	    ++i;
	  }
	  else if (TextBytes[i] == 0x7f && i + 2 < TextBytes.Length) { // Unknown Type of Text Substitution
	    if (LastPos < i)
	      this.Text_ += E.GetString(TextBytes, LastPos, i - LastPos);
	    this.Text_ += String.Format("{0}Marker: {1:X2}{2:X2}{3:X2}{4}", FFXIEncoding.SpecialMarkerStart, TextBytes[i + 0], TextBytes[i + 1], TextBytes[i + 2], FFXIEncoding.SpecialMarkerEnd);
	    LastPos = i + 3;
	    i += 2;
	  }
#if DEBUG
	  else if (TextBytes[i] < 0x20) {
	    if (LastPos < i)
	      this.Text_ += E.GetString(TextBytes, LastPos, i - LastPos);
	    this.Text_ += String.Format("{0}Possible Special Code: {2:X2}{1}", FFXIEncoding.SpecialMarkerStart, FFXIEncoding.SpecialMarkerEnd, TextBytes[i]);
	    LastPos = i + 1;
	  }
#endif
	}
	if (LastPos < TextBytes.Length)
	  this.Text_ += E.GetString(TextBytes, LastPos, TextBytes.Length - LastPos);
        return true;
      } catch { return false; }
    }

    #endregion

  }

}
