// $Id$

#if DEBUG
# define IncludeUnknownFields
#endif

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
	  "index",
	  "text",
#if IncludeUnknownFields
	  "unknown-1",
	  "unknown-2",
	  "unknown-3",
#endif
	});
      }
    }

    public override List<string> GetAllFields() {
      return XIStringTableEntry.AllFields;
    }

    #region Data Fields

    private Nullable<int>    Index_;
    private string           Text_;
#if IncludeUnknownFields
    private Nullable<ushort> Unknown1_;
    private Nullable<ushort> Unknown2_;
    private Nullable<ushort> Unknown3_;
#endif
    
    #endregion

    public override void Clear() {
      this.Index_    = null;
      this.Text_     = null;
#if IncludeUnknownFields
      this.Unknown1_ = null;
      this.Unknown2_ = null;
      this.Unknown3_ = null;
#endif
    }

    #endregion

    #region Field Access

    public override bool HasField(string Field) {
      switch (Field) {
	case "index":     return this.Index_.HasValue;
	case "text":      return (this.Text_ != null);
#if IncludeUnknownFields
	case "unknown-1": return this.Unknown1_.HasValue;
	case "unknown-2": return this.Unknown2_.HasValue;
	case "unknown-3": return this.Unknown3_.HasValue;
#endif
	default:          return false;
      }
    }

    public override string GetFieldText(string Field) {
      switch (Field) {
	case "index":     return (!this.Index_.HasValue    ? String.Empty : String.Format("{0:00000}", this.Index_.Value));
	case "text":      return this.Text_;
#if IncludeUnknownFields
	case "unknown-1": return (!this.Unknown1_.HasValue ? String.Empty : String.Format("{0:X4} ({0})", this.Unknown1_.Value));
	case "unknown-2": return (!this.Unknown2_.HasValue ? String.Empty : String.Format("{0:X4} ({0})", this.Unknown2_.Value));
	case "unknown-3": return (!this.Unknown3_.HasValue ? String.Empty : String.Format("{0:X4} ({0})", this.Unknown3_.Value));
#endif
	default:          return null;
      }
    }

    public override object GetFieldValue(string Field) {
      switch (Field) {
	case "index":     return (!this.Index_.HasValue    ? null : (object) this.Index_.Value);
	case "text":      return this.Text_;
#if IncludeUnknownFields
	case "unknown-1": return (!this.Unknown1_.HasValue ? null : (object) this.Unknown1_.Value);
	case "unknown-2": return (!this.Unknown2_.HasValue ? null : (object) this.Unknown2_.Value);
	case "unknown-3": return (!this.Unknown3_.HasValue ? null : (object) this.Unknown3_.Value);
#endif
	default:          return null;
      }
    }

    protected override void LoadField(string Field, System.Xml.XmlElement Node) {
      switch (Field) {
	case "index":     this.Index_    = (int)    this.LoadUnsignedIntegerField(Node); break;
	case "text":      this.Text_     =          this.LoadTextField           (Node); break;
#if IncludeUnknownFields
	case "unknown-1": this.Unknown1_ = (ushort) this.LoadUnsignedIntegerField(Node); break;
	case "unknown-2": this.Unknown2_ = (ushort) this.LoadUnsignedIntegerField(Node); break;
	case "unknown-3": this.Unknown3_ = (ushort) this.LoadUnsignedIntegerField(Node); break;
#endif
      }
    }

    #endregion

    #region ROM File Reading

    public bool Read(BinaryReader BR, Encoding E, uint EntryBytes, uint DataBytes) {
      return this.Read(BR, E, null, EntryBytes, DataBytes);
    }

    public bool Read(BinaryReader BR, Encoding E, Nullable<int> Index, uint EntryBytes, uint DataBytes) {
      this.Clear();
      this.Index_ = Index;
    long IndexPos = -1;
      try {
      uint  Offset = BR.ReadUInt32();
      short Size   = BR.ReadInt16();
#if IncludeUnknownFields
	this.Unknown1_ = BR.ReadUInt16(); // 1 may indicate to use different "encoding"
	this.Unknown2_ = BR.ReadUInt16(); // almost always 0; meaning unknown
	this.Unknown3_ = BR.ReadUInt16(); // almost always 0
#else
	BR.ReadUInt16(); // 1 may indicate a different "encoding"
	BR.ReadUInt16(); // almost always 0; value unknown
	BR.ReadUInt16(); // almost always 0
#endif
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
