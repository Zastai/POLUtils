// $Id$

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using PlayOnline.Core;

namespace PlayOnline.FFXI {

  public class XIStringTableEntry : Thing {

    public XIStringTableEntry() {
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
	  "unknown-1",
	  "unknown-2",
	  "text",
	});
      }
    }

    public override List<string> GetAllFields() {
      return SimpleStringTableEntry.AllFields;
    }

    #region Data Fields

    private Nullable<ushort> Unknown1_;
    private Nullable<uint>   Unknown2_;
    private string           Text_;
    
    #endregion

    public override void Clear() {
      this.Unknown1_ = null;
      this.Unknown2_ = null;
      this.Text_     = null;
    }

    #endregion

    #region Field Access

    public override bool HasField(string Field) {
      switch (Field) {
	case "unknown-1": return this.Unknown1_.HasValue;
	case "unknown-2": return this.Unknown2_.HasValue;
	case "text":      return (this.Text_ != null);
	default:          return false;
      }
    }

    public override string GetFieldText(string Field) {
      switch (Field) {
	case "unknown-1": return (!this.Unknown1_.HasValue ? String.Empty : String.Format("{0:X4}", this.Unknown1_.Value));
	case "unknown-2": return (!this.Unknown2_.HasValue ? String.Empty : String.Format("{0:X8}", this.Unknown2_.Value));
	case "text":      return this.Text_;
	default:          return null;
      }
    }

    public override object GetFieldValue(string Field) {
      switch (Field) {
	case "unknown-1": return (!this.Unknown1_.HasValue ? null : (object) this.Unknown1_.Value);
	case "unknown-2": return (!this.Unknown2_.HasValue ? null : (object) this.Unknown2_.Value);
	case "text":      return this.Text_;
	default:          return null;
      }
    }

    protected override void LoadField(string Field, System.Xml.XmlElement Node) {
      switch (Field) {
	case "unknown-1": this.Unknown1_ = (ushort) this.LoadUnsignedIntegerField(Node); break;
	case "unknown-2": this.Unknown2_ = (uint)   this.LoadUnsignedIntegerField(Node); break;
	case "text":      this.Text_     =          this.LoadTextField           (Node); break;
      }
    }

    #endregion

    #region ROM File Reading

    public bool Read(BinaryReader BR, Encoding E, uint EntryBytes, uint DataBytes) {
      this.Clear();
    long IndexPos = -1;
      try {
      uint  Offset = BR.ReadUInt32();
      short Size   = BR.ReadInt16();
        this.Unknown1_ = BR.ReadUInt16(); // Unknown (0 or 1; so probably a flag of some sort)
	this.Unknown2_ = BR.ReadUInt32(); // Unknown
	if (Size < 0 || Offset + Size > DataBytes)
	  return false;
	IndexPos = BR.BaseStream.Position;
	BR.BaseStream.Seek(0x38 + EntryBytes + Offset, SeekOrigin.Begin);
	this.Text_ = E.GetString(BR.ReadBytes(Size)).TrimEnd('\0');
	BR.BaseStream.Seek(IndexPos, SeekOrigin.Begin);
	return true;
      } catch {
	if (IndexPos >= 0)
	  BR.BaseStream.Seek(IndexPos, SeekOrigin.Begin);
	return false;
      }
    }

    #endregion

  }

}
