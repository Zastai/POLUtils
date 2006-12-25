// $Id$

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using PlayOnline.Core;

namespace PlayOnline.FFXI.Things {

  public class DMSGStringTableEntry2 : Thing {

    public DMSGStringTableEntry2() {
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
      return DMSGStringTableEntry2.AllFields;
    }

    #region Data Fields

    private uint?  Index_;
    private string Text_;
    
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
	case "index": this.Index_ = (uint) this.LoadUnsignedIntegerField(Node); break;
	case "text":  this.Text_  =        this.LoadTextField           (Node); break;
      }
    }

    #endregion

    #region ROM File Reading

    public bool Read(BinaryReader BR, Encoding E, uint HeaderBytes, uint EntryBytes, uint DataBytes) {
      return this.Read(BR, E, null, HeaderBytes, EntryBytes, DataBytes);
    }

    public bool Read(BinaryReader BR, Encoding E, uint? Index, uint HeaderBytes, uint EntryBytes, uint DataBytes) {
      this.Clear();
      this.Index_ = Index;
      BR.BaseStream.Position = HeaderBytes + 0x8 * Index.Value;
      try {
      uint Offset = ~BR.ReadUInt32() + 40;
      uint Length = ~BR.ReadUInt32() - 40;
	if (Length < 0 || Offset + Length > DataBytes)
	  return false;
	BR.BaseStream.Position = HeaderBytes + EntryBytes + Offset;
      byte[] TextBytes = BR.ReadBytes((int) Length);
	for (uint i = 0; i < TextBytes.Length; ++i)
	  TextBytes[i] ^= 0xff;
	this.Text_ = E.GetString(TextBytes).TrimEnd('\0');
	return true;
      } catch { }
      return false;
    }

    #endregion

  }

}
