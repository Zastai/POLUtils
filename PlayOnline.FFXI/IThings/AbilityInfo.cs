// $Id$

using System;
using System.Collections.Generic;
using System.IO;

using PlayOnline.Core;

namespace PlayOnline.FFXI {

  public class AbilityInfo : Thing {

    public AbilityInfo() {
      // Clear fields
      this.Clear();
    }

    public override string ToString() {
      return this.Name_;
    }

    public override List<PropertyPages.IThing> GetPropertyPages() {
      return base.GetPropertyPages();
    }

    #region Fields

    public static List<string> AllFields {
      get {
	return new List<string>(new string[] {
	  "id",
	  "type",
	  "unknown-1",
	  "mp-cost",
	  "reuse-delay",
	  "valid-targets",
	  "name",
	  "description",
	});
      }
    }

    public override List<string> GetAllFields() {
      return AbilityInfo.AllFields;
    }

    #region Data Fields

    private Nullable<ushort>      ID_;
    private Nullable<AbilityType> Type_;
    private Nullable<byte>        Unknown1_;
    private Nullable<ushort>      MPCost_;
    private Nullable<ushort>      ReuseDelay_;
    private Nullable<ValidTarget> ValidTargets_;
    private string                Name_;
    private string                Description_;
    
    #endregion

    public override void Clear() {
      this.ID_           = null;
      this.Type_         = null;
      this.Unknown1_     = null;
      this.MPCost_       = null;
      this.ReuseDelay_   = null;
      this.ValidTargets_ = null;
      this.Name_         = null;
      this.Description_  = null;
    }

    #endregion

    #region Field Access

    public override bool HasField(string Field) {
      switch (Field) {
	// Objects
	case "description":   return (this.Description_ != null);
	case "name":          return (this.Name_        != null);
	// Nullables
	case "id":            return this.ID_.HasValue;
	case "mp-cost":       return this.MPCost_.HasValue;
	case "reuse-delay":   return this.ReuseDelay_.HasValue;
	case "type":          return this.Type_.HasValue;
	case "unknown-1":     return this.Unknown1_.HasValue;
	case "valid-targets": return this.ValidTargets_.HasValue;
	default:              return false;
      }
    }

    public override string GetFieldText(string Field) {
      switch (Field) {
	// Objects
	case "description":   return this.Description_;
	case "name":          return this.Name_;
	// Nullables - Simple Values
	case "id":            return (!this.ID_.HasValue           ? String.Empty : String.Format("{0}", this.ID_.Value));
	case "mp-cost":       return (!this.MPCost_.HasValue       ? String.Empty : String.Format("{0}", this.MPCost_.Value));
	case "type":          return (!this.Type_.HasValue         ? String.Empty : String.Format("{0}", this.Type_.Value));
	case "valid-targets": return (!this.ValidTargets_.HasValue ? String.Empty : String.Format("{0}", this.ValidTargets_.Value));
	// Nullables - Hex Values
	case "unknown-1":     return (!this.Unknown1_.HasValue     ? String.Empty : String.Format("{0:X2} ({0})", this.Unknown1_.Value));
	// Nullables - Time Values
	case "reuse-delay":   return (!this.ReuseDelay_.HasValue   ? String.Empty : this.FormatTime(this.ReuseDelay_.Value));
	default:              return null;
      }
    }

    public override object GetFieldValue(string Field) {
      switch (Field) {
	// Objects
	case "description":   return this.Description_;
	case "name":          return this.Name_;
	// Nullables
	case "id":            return (!this.ID_.HasValue           ? null : (object) this.ID_.Value);
	case "mp-cost":       return (!this.MPCost_.HasValue       ? null : (object) this.MPCost_.Value);
	case "reuse-delay":   return (!this.ReuseDelay_.HasValue   ? null : (object) this.ReuseDelay_.Value);
	case "type":          return (!this.Type_.HasValue         ? null : (object) this.Type_.Value);
	case "unknown-1":     return (!this.Unknown1_.HasValue     ? null : (object) this.Unknown1_.Value);
	case "valid-targets": return (!this.ValidTargets_.HasValue ? null : (object) this.ValidTargets_.Value);
	default:              return null;
      }
    }

    protected override void LoadField(string Field, System.Xml.XmlElement Node) {
      switch (Field) {
	// "Simple" Fields
	case "description":   this.Description_  =               this.LoadTextField           (Node); break;
	case "id":            this.ID_           = (ushort)      this.LoadUnsignedIntegerField(Node); break;
	case "mp-cost":       this.MPCost_       = (ushort)      this.LoadUnsignedIntegerField(Node); break;
	case "name":          this.Name_         =               this.LoadTextField           (Node); break;
	case "reuse-delay":   this.ReuseDelay_   = (ushort)      this.LoadUnsignedIntegerField(Node); break;
	case "type":          this.Type_         = (AbilityType) this.LoadHexField            (Node); break;
	case "unknown-1":     this.Unknown1_     = (byte)        this.LoadUnsignedIntegerField(Node); break;
	case "valid-targets": this.ValidTargets_ = (ValidTarget) this.LoadHexField            (Node); break;
      }
    }

    #endregion

    #region ROM File Reading

    // Block Layout:
    // 000-001 U16 Index
    // 002-003 U16 Unknown
    // 004-005 U16 MP Cost
    // 006-007 U16 Cooldown
    // 008-009 U16 Valid Targets
    // 00a-029 TXT Name
    // 02a-129 TXT Description (exact length unknown)
    // 12a-3fe U8  Padding (NULs)
    // 3ff-3ff U8  End marker (0xff)
    public bool Read(BinaryReader BR) {
      this.Clear();
      try {
      byte[] Bytes = BR.ReadBytes(0x400);
	if (Bytes[0x3ff] != 0xff || Bytes[9] != 0x00 || !FFXIEncryption.DecodeDataBlock(Bytes))
	  return false;
      FFXIEncoding E = new FFXIEncoding();
	BR = new BinaryReader(new MemoryStream(Bytes, false));
	this.ID_           = BR.ReadUInt16();
	this.Type_         = (AbilityType) BR.ReadByte();
	this.Unknown1_     = BR.ReadByte();
	this.MPCost_       = BR.ReadUInt16();
	this.ReuseDelay_   = BR.ReadUInt16();
	this.ValidTargets_ = (ValidTarget) BR.ReadUInt16();
	this.Name_         = E.GetString(BR.ReadBytes(32)).TrimEnd('\0');
	this.Description_  = E.GetString(BR.ReadBytes(256)).TrimEnd('\0');
	BR.Close();
	return true;
      } catch { return false; }
    }

    #endregion

  }

}