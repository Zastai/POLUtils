// $Id$

using System;
using System.Collections.Generic;
using System.IO;

using PlayOnline.Core;

namespace PlayOnline.FFXI {

  public class SimpleStringTableEntry : Thing {

    public SimpleStringTableEntry() {
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
	  "id",
	  "text",
	});
      }
    }

    public override List<string> GetAllFields() {
      return SimpleStringTableEntry.AllFields;
    }

    #region Data Fields

    private Nullable<uint> ID_;
    private string         Text_;
    
    #endregion

    public override void Clear() {
      this.ID_   = null;
      this.Text_ = null;
    }

    #endregion

    #region Field Access

    public override bool HasField(string Field) {
      switch (Field) {
	case "id":   return this.ID_.HasValue;
	case "text": return (this.Text_ != null);
	default:     return false;
      }
    }

    public override string GetFieldText(string Field) {
      switch (Field) {
	case "id":   return (!this.ID_.HasValue ? String.Empty : String.Format("{0}", this.ID_.Value));
	case "text": return this.Text_;
	default:     return null;
      }
    }

    public override object GetFieldValue(string Field) {
      switch (Field) {
	case "id":   return (!this.ID_.HasValue ? null : (object) this.ID_.Value);
	case "text": return this.Text_;
	default:     return null;
      }
    }

    protected override void LoadField(string Field, System.Xml.XmlElement Node) {
      switch (Field) {
	case "id":   this.ID_   = (uint) this.LoadUnsignedIntegerField(Node); break;
	case "text": this.Text_ =        this.LoadTextField           (Node); break;
      }
    }

    #endregion

    #region ROM File Reading

    public bool Read(BinaryReader BR) {
      this.Clear();
      try {
	this.ID_ = BR.ReadUInt32();
      FFXIEncoding E = new FFXIEncoding();
#if false
	this.Text_ = E.GetString(BR.ReadBytes(0x3b)).TrimEnd('\0');
#else
      byte[] TextBytes = BR.ReadBytes(0x3b);
	this.Text_ = String.Empty;
      int LastPos = 0;
	for (int i = 0; i < TextBytes.Length; ++i) {
	  if (TextBytes[i] == 0x07) { // Line Break
	    if (LastPos < i)
	      this.Text_ += E.GetString(TextBytes, LastPos, i - LastPos);
#if DEBUG
	    this.Text_ += "\r\n";
#else
	    this.Text_ += String.Format("{0}NewLine{1}", FFXIEncoding.SpecialMarkerStart, FFXIEncoding.SpecialMarkerEnd);
#endif
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
#endif
	return (BR.ReadByte() == 0xff);
      } catch { return false; }
    }

    #endregion

  }

}
