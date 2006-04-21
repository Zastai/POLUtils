// $Id$

using System;
using System.Collections.Generic;
using System.IO;

using PlayOnline.Core;

namespace PlayOnline.FFXI {

  public class SpellInfo : Thing {

    public SpellInfo() {
      // Clear fields
      this.Clear();
    }

    public override string ToString() {
      return String.Format("{0} / {1}", this.EnglishName_, this.JapaneseName_);
    }

    public override List<PropertyPages.IThing> GetPropertyPages() {
      return base.GetPropertyPages();
    }

    #region Fields

    public static List<string> AllFields {
      get {
	return new List<string>(new string[] {
	  "index",
	  "id",
	  "english-name",
	  "japanese-name",
	  "magic-type",
	  "element",
	  "valid-targets",
	  "skill",
	  "level-required",
	  "mp-cost",
	  "casting-time",
	  "recast-delay",
	  "english-description",
	  "japanese-description",
	  "unknown-1",
	});
      }
    }

    public override List<string> GetAllFields() {
      return SpellInfo.AllFields;
    }

    #region Data Fields

    private Nullable<ushort>      Index_;
    private Nullable<MagicType>   MagicType_;
    private Nullable<Element>     Element_;
    private Nullable<ValidTarget> ValidTargets_;
    private Nullable<Skill>       Skill_;
    private Nullable<ushort>      MPCost_;
    private Nullable<byte>        CastingTime_;
    private Nullable<ushort>      RecastDelay_;
    private byte[]                LevelRequired_;
    private Nullable<ushort>      ID_;
    private Nullable<byte>        Unknown1_;
    private string                JapaneseName_;
    private string                EnglishName_;
    private string                JapaneseDescription_;
    private string                EnglishDescription_;
    
    #endregion

    public override void Clear() {
      this.Index_               = null;
      this.MagicType_           = null;
      this.Element_             = null;
      this.ValidTargets_        = null;
      this.Skill_               = null;
      this.MPCost_              = null;
      this.CastingTime_         = null;
      this.RecastDelay_         = null;
      this.LevelRequired_       = null;
      this.ID_                  = null;
      this.Unknown1_            = null;
      this.JapaneseName_        = null;
      this.EnglishName_         = null;
      this.JapaneseDescription_ = null;
      this.EnglishDescription_  = null;
    }

    #endregion

    #region Field Access

    public override bool HasField(string Field) {
      switch (Field) {
	// Objects
	case "english-description":   return (this.EnglishDescription_  != null);
	case "english-name":          return (this.EnglishName_         != null);
	case "japanese-description":  return (this.JapaneseDescription_ != null);
	case "japanese-name":         return (this.JapaneseName_        != null);
	case "level-required":        return (this.LevelRequired_       != null);
	// Nullables
	case "casting-time":          return this.CastingTime_.HasValue;
	case "element":               return this.Element_.HasValue;
	case "id":                    return this.ID_.HasValue;
	case "index":                 return this.Index_.HasValue;
	case "magic-type":            return this.MagicType_.HasValue;
	case "mp-cost":               return this.MPCost_.HasValue;
	case "recast-delay":          return this.RecastDelay_.HasValue;
	case "skill":                 return this.Skill_.HasValue;
	case "unknown-1":             return this.Unknown1_.HasValue;
	case "valid-targets":         return this.ValidTargets_.HasValue;
	default:                      return false;
      }
    }

    public override string GetFieldText(string Field) {
      switch (Field) {
	// Special Values
	case "level-required": {
	string LevelInfo = String.Empty;
	  if (this.LevelRequired_ == null || this.LevelRequired_.Length > sizeof(Job) * 8)
	    return LevelInfo;
	  for (int i = 0; i < this.LevelRequired_.Length; ++i) {
	    if (this.LevelRequired_[i] != 0xff) {
	      if (LevelInfo != String.Empty)
		LevelInfo += '/';
	      LevelInfo += String.Format("{0:00}{1}", this.LevelRequired_[i], (Job) (1 << i));
	    }
	  }
	  return LevelInfo;
	}
	// Objects
	case "english-description":  return this.EnglishDescription_;
	case "english-name":         return this.EnglishName_;
	case "japanese-description": return this.JapaneseDescription_;
	case "japanese-name":        return this.JapaneseName_;
	// Nullables - Simple Values
	case "element":              return (!this.Element_.HasValue      ? String.Empty : String.Format("{0}", this.Element_.Value));
	case "id":                   return (!this.ID_.HasValue           ? String.Empty : String.Format("{0:000}", this.ID_.Value));
	case "index":                return (!this.ID_.HasValue           ? String.Empty : String.Format("{0:000}", this.Index_.Value));
	case "magic-type":           return (!this.MagicType_.HasValue    ? String.Empty : String.Format("{0}", this.MagicType_.Value));
	case "mp-cost":              return (!this.MPCost_.HasValue       ? String.Empty : String.Format("{0}", this.MPCost_.Value));
	case "skill":                return (!this.Skill_.HasValue        ? String.Empty : String.Format("{0}", this.Skill_.Value));
	case "valid-targets":        return (!this.ValidTargets_.HasValue ? String.Empty : String.Format("{0}", this.ValidTargets_.Value));
	// Nullables - Hex Values
	case "unknown-1":            return (!this.Unknown1_.HasValue     ? String.Empty : String.Format("{0:X2} ({0})", this.Unknown1_.Value));
	// Nullables - Time Values
	case "casting-time":         return (!this.CastingTime_.HasValue  ? String.Empty : this.FormatTime(this.CastingTime_.Value / 4.0));
	case "recast-delay":         return (!this.RecastDelay_.HasValue  ? String.Empty : this.FormatTime(this.RecastDelay_.Value / 4.0));
	default:                     return null;
      }
    }

    public override object GetFieldValue(string Field) {
      switch (Field) {
	// Objects
	case "english-description":  return this.EnglishDescription_;
	case "english-name":         return this.EnglishName_;
	case "japanese-description": return this.JapaneseDescription_;
	case "japanese-name":        return this.JapaneseName_;
	case "level-required":       return this.LevelRequired_;
	// Nullables
	case "casting-time":         return (!this.CastingTime_.HasValue  ? null : (object) this.CastingTime_.Value);
	case "element":              return (!this.Element_.HasValue      ? null : (object) this.Element_.Value);
	case "id":                   return (!this.ID_.HasValue           ? null : (object) this.ID_.Value);
	case "index":                return (!this.Index_.HasValue        ? null : (object) this.Index_.Value);
	case "magic-type":           return (!this.MagicType_.HasValue    ? null : (object) this.MagicType_.Value);
	case "mp-cost":              return (!this.MPCost_.HasValue       ? null : (object) this.MPCost_.Value);
	case "recast-delay":         return (!this.RecastDelay_.HasValue  ? null : (object) this.RecastDelay_.Value);
	case "skill":                return (!this.Skill_.HasValue        ? null : (object) this.Skill_.Value);
	case "unknown-1":            return (!this.Unknown1_.HasValue     ? null : (object) this.Unknown1_.Value);
	case "valid-targets":        return (!this.ValidTargets_.HasValue ? null : (object) this.ValidTargets_.Value);
	default:                     return null;
      }
    }

    protected override void LoadField(string Field, System.Xml.XmlElement Node) {
      switch (Field) {
	// "Simple" Fields
	case "casting-time":         this.CastingTime_         = (byte)        this.LoadUnsignedIntegerField(Node); break;
	case "element":              this.Element_             = (Element)     this.LoadHexField            (Node); break;
	case "english-description":  this.EnglishDescription_  =               this.LoadTextField           (Node); break;
	case "english-name":         this.EnglishName_         =               this.LoadTextField           (Node); break;
	case "id":                   this.ID_                  = (ushort)      this.LoadUnsignedIntegerField(Node); break;
	case "index":                this.Index_               = (ushort)      this.LoadUnsignedIntegerField(Node); break;
	case "japanese-description": this.JapaneseDescription_ =               this.LoadTextField           (Node); break;
	case "japanese-name":        this.JapaneseName_        =               this.LoadTextField           (Node); break;
	case "level-required":       this.LevelRequired_       =               this.LoadByteArray           (Node); break;
	case "magic-type":           this.MagicType_           = (MagicType)   this.LoadHexField            (Node); break;
	case "mp-cost":              this.MPCost_              = (ushort)      this.LoadUnsignedIntegerField(Node); break;
	case "recast-delay":         this.RecastDelay_         = (ushort)      this.LoadUnsignedIntegerField(Node); break;
	case "skill":                this.Skill_               = (Skill)       this.LoadHexField            (Node); break;
	case "unknown-1":            this.Unknown1_            = (byte)        this.LoadUnsignedIntegerField(Node); break;
	case "valid-targets":        this.ValidTargets_        = (ValidTarget) this.LoadHexField            (Node); break;
      }
    }

    #endregion

    #region ROM File Reading

    // Block Layout:
    // 000-001 U16 Index
    // 002-003 U16 Spell Type (1/2/3/4/5 - White/Black/Summon/Ninja/Bard)
    // 004-005 U16 Element
    // 006-007 U16 Valid Targets
    // 008-009 U16 Skill
    // 00a-00b U16 MP Cost
    // 00c-00c U8  Cast Time (1/4 second)
    // 00d-00d U8  Recast Delay (1/4 second)
    // 00e-025 U8  Level required (1 byte per job, 0xff if not learnable; first is for the NUL job, so always 0xff; only 24 slots despite 32 possible job flags)
    // 026-027 U16 ID (0 for "unused" spells; starts out equal to the index, but doesn't stay that way)
    // 028-028 U8  Unknown
    // 029-03c TXT Japanese Name
    // 03d-050 TXT English Name
    // 051-0D0 TXT Japanese Description
    // 0D1-150 TXT English Description
    // 151-3fe U8  Padding (NULs)
    // 3ff-3ff U8  End marker (0xff)
    public bool Read(BinaryReader BR) {
      this.Clear();
      try {
      byte[] Bytes = BR.ReadBytes(0x400);
	if (Bytes[0x3] != 0x00 || Bytes[0x5] != 0x00 || Bytes[0x7] != 0x00 || Bytes[0x9] != 0x00 || Bytes[0xf] != 0xff || Bytes[0x3ff] != 0xff)
	  return false;
	if (!FFXIEncryption.DecodeDataBlock(Bytes))
	  return false;
	BR = new BinaryReader(new MemoryStream(Bytes, false));
      } catch { return false; }
      this.Index_         = BR.ReadUInt16();
      this.MagicType_     = (MagicType)   BR.ReadUInt16();
      this.Element_       = (Element)     BR.ReadUInt16();
      this.ValidTargets_  = (ValidTarget) BR.ReadUInt16();
      this.Skill_         = (Skill)       BR.ReadUInt16();
      this.MPCost_        = BR.ReadUInt16();
      this.CastingTime_   = BR.ReadByte();
      this.RecastDelay_   = BR.ReadByte();
      this.LevelRequired_ = BR.ReadBytes(24);
      this.ID_            = BR.ReadUInt16();
      this.Unknown1_      = BR.ReadByte();
    FFXIEncoding E = new FFXIEncoding();
      this.JapaneseName_        = E.GetString(BR.ReadBytes( 20)).TrimEnd('\0');
      this.EnglishName_         = E.GetString(BR.ReadBytes( 20)).TrimEnd('\0');
      this.JapaneseDescription_ = E.GetString(BR.ReadBytes(128)).TrimEnd('\0');
      this.EnglishDescription_  = E.GetString(BR.ReadBytes(128)).TrimEnd('\0');
#if DEBUG
      { // Read the next 64 bits, and report if it's not 0 (means there's new data to identify)
      ulong Next64 = BR.ReadUInt64();
	if (Next64 != 0) {
	  Console.ForegroundColor = ConsoleColor.Red;
	  Console.WriteLine("Nonzero data after item ({0}): {1:X16}", this.EnglishName_, Next64);
	  Console.ResetColor();
	}
      }
#endif
      BR.Close();
      return true;
    }

    #endregion

  }

}
