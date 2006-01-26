// $Id$

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

using PlayOnline.Core;

namespace PlayOnline.FFXI {

  public class Item : Thing {

    public Item() {
      // Fill Thing helpers
      this.IconField_ = "icon";
      // Clear fields
      this.Clear();
    }

    public override string ToString() {
      return String.Format("[{0:X4}] {1} / {2}", this.ID_, this.EnglishName_, this.JapaneseName_);
    }

    public override List<PropertyPages.IThing> GetPropertyPages() {
    List<PropertyPages.IThing> Pages = base.GetPropertyPages();
      Pages.Add(new PropertyPages.Item(this));
      return Pages;
    }

    #region Fields

    public static List<string> AllFields {
      get {
	return new List<string>(new string[] {
	  // General
	  "id",
	  "flags",
	  "stack-size",
	  "type",
	  "resource-id",
	  "valid-targets",
	  "english-name",
	  "japanese-name",
	  "description",
	  // English-Specific
	  "log-name-singular",
	  "log-name-plural",
	  // Furniture-Specific
	  "element",
	  "storage-slots",
	  // UsableItem-Specific
	  "activation-time",
	  // Equipment-Specific
	  "level",
	  "slots",
	  "races",
	  "jobs",
	  // Armor-Specific
	  "shield-size",
	  // Weapon-Specific
	  "damage",
	  "delay",
	  "dps",
	  "skill",
	  "jug-size",
	  // Enchantment Info
	  "max-charges",
	  "casting-time",
	  "use-delay",
	  "reuse-delay",
	  // Special Stuff
	  "icon",
	  "unknown-1",
	  "unknown-2",
	  "unknown-3",
	  "unknown-4",
	  "unknown-5",
	});
      }
    }

    public override List<string> GetAllFields() {
      return Item.AllFields;
    }

    #region Data Fields

    // General
    private Nullable<uint>          ID_;
    private Nullable<ItemFlags>     Flags_;
    private Nullable<ushort>        StackSize_;
    private Nullable<ItemType>      Type_;
    private Nullable<ushort>        ResourceID_;
    private Nullable<ValidTarget>   ValidTargets_;
    private string                  EnglishName_;
    private string                  JapaneseName_;
    private string                  Description_;
    // English-Specific
    private string                  LogNameSingular_;
    private string                  LogNamePlural_;
    // Furniture-Specific
    private Nullable<Element>       Element_;
    private Nullable<short>         StorageSlots_;
    // UsableItem-Specific
    private Nullable<byte>          ActivationTime_;
    // Equipment-Specific
    private Nullable<ushort>        Level_;
    private Nullable<EquipmentSlot> Slots_;
    private Nullable<Race>          Races_;
    private Nullable<Job>           Jobs_;
    // Armor-Specific
    private Nullable<ushort>        ShieldSize_;
    // Weapon-Specific
    private Nullable<ushort>        Damage_;
    private Nullable<ushort>        Delay_;
    private Nullable<ushort>        DPS_;
    private Nullable<Skill>         Skill_;
    private Nullable<byte>          JugSize_;
    // Enchantment Info
    private Nullable<byte>          MaxCharges_;
    private Nullable<byte>          CastingTime_;
    private Nullable<ushort>        UseDelay_;
    private Nullable<uint>          ReuseDelay_;
    // Special
    private Graphic                 Icon_;
    private Nullable<ushort>        Unknown1_;
    private Nullable<ushort>        Unknown2_;
    private Nullable<ushort>        Unknown3_;
    private Nullable<ushort>        Unknown4_;
    private Nullable<ushort>        Unknown5_;
    
    #endregion

    public override void Clear() {
      if (this.Icon_ != null)
	this.Icon_.Clear();
      this.ID_              = null;
      this.Flags_           = null;
      this.StackSize_       = null;
      this.Type_            = null;
      this.ResourceID_      = null;
      this.ValidTargets_    = null;
      this.EnglishName_     = null;
      this.JapaneseName_    = null;
      this.Description_     = null;
      this.LogNameSingular_ = null;
      this.LogNamePlural_   = null;
      this.Element_         = null;
      this.StorageSlots_    = null;
      this.ActivationTime_  = null;
      this.Level_           = null;
      this.Slots_           = null;
      this.Races_           = null;
      this.Jobs_            = null;
      this.ShieldSize_      = null;
      this.Damage_          = null;
      this.Delay_           = null;
      this.DPS_             = null;
      this.Skill_           = null;
      this.JugSize_         = null;
      this.MaxCharges_      = null;
      this.CastingTime_     = null;
      this.UseDelay_        = null;
      this.ReuseDelay_      = null;
      this.Icon_            = null;
      this.Unknown1_        = null;
      this.Unknown2_        = null;
      this.Unknown3_        = null;
      this.Unknown4_        = null;
      this.Unknown5_        = null;
    }

    #endregion

    #region Field Access

    public override bool HasField(string Field) {
      switch (Field) {
	// Objects
	case "description":       return (this.Description_     != null);
	case "english-name":      return (this.EnglishName_     != null);
	case "icon":              return (this.Icon_            != null);
	case "japanese-name":     return (this.JapaneseName_    != null);
	case "log-name-plural":   return (this.LogNamePlural_   != null);
	case "log-name-singular": return (this.LogNameSingular_ != null);
	// Nullables
	case "activation-time":   return this.ActivationTime_.HasValue;
	case "casting-time":      return this.CastingTime_.HasValue;
	case "damage":            return this.Damage_.HasValue;
	case "delay":             return this.Delay_.HasValue;
	case "dps":               return this.DPS_.HasValue;
	case "element":           return this.Element_.HasValue;
	case "flags":             return this.Flags_.HasValue;
	case "id":                return this.ID_.HasValue;
	case "jobs":              return this.Jobs_.HasValue;
	case "jug-size":          return this.JugSize_.HasValue;
	case "level":             return this.Level_.HasValue;
	case "max-charges":       return this.MaxCharges_.HasValue;
	case "races":             return this.Races_.HasValue;
	case "resource-id":       return this.ResourceID_.HasValue;
	case "reuse-delay":       return this.ReuseDelay_.HasValue;
	case "shield-size":       return this.ShieldSize_.HasValue;
	case "skill":             return this.Skill_.HasValue;
	case "slots":             return this.Slots_.HasValue;
	case "stack-size":        return this.StackSize_.HasValue;
	case "storage-slots":     return this.StorageSlots_.HasValue;
	case "type":              return this.Type_.HasValue;
	case "unknown-1":         return this.Unknown1_.HasValue;
	case "unknown-2":         return this.Unknown2_.HasValue;
	case "unknown-3":         return this.Unknown3_.HasValue;
	case "unknown-4":         return this.Unknown4_.HasValue;
	case "unknown-5":         return this.Unknown5_.HasValue;
	case "use-delay":         return this.UseDelay_.HasValue;
	case "valid-targets":     return this.ValidTargets_.HasValue;
	default:                  return false;
      }
    }

    public override string GetFieldText(string Field) {
      switch (Field) {
	// Objects
	case "description":       return this.Description_;
	case "english-name":      return this.EnglishName_;
	case "icon":              return this.Icon_.ToString();
	case "japanese-name":     return this.JapaneseName_;
	case "log-name-plural":   return this.LogNamePlural_;
	case "log-name-singular": return this.LogNameSingular_;
	// Nullables - Simple Values
	case "damage":            return (!this.Damage_.HasValue         ? String.Empty : String.Format("{0}", this.Damage_.Value));
	case "dps":               return (!this.DPS_.HasValue            ? String.Empty : String.Format("{0}", this.DPS_.Value / 100.0));
	case "element":           return (!this.Element_.HasValue        ? String.Empty : String.Format("{0}", this.Element_.Value));
	case "flags":             return (!this.Flags_.HasValue          ? String.Empty : String.Format("{0}", this.Flags_.Value));
	case "jobs":              return (!this.Jobs_.HasValue           ? String.Empty : String.Format("{0}", this.Jobs_.Value));
	case "jug-size":          return (!this.JugSize_.HasValue        ? String.Empty : String.Format("{0}", this.JugSize_.Value));
	case "level":             return (!this.Level_.HasValue          ? String.Empty : String.Format("{0}", this.Level_.Value));
	case "max-charges":       return (!this.MaxCharges_.HasValue     ? String.Empty : String.Format("{0}", this.MaxCharges_.Value));
	case "races":             return (!this.Races_.HasValue          ? String.Empty : String.Format("{0}", this.Races_.Value));
	case "shield-size":       return (!this.ShieldSize_.HasValue     ? String.Empty : String.Format("{0}", this.ShieldSize_.Value));
	case "skill":             return (!this.Skill_.HasValue          ? String.Empty : String.Format("{0}", this.Skill_.Value));
	case "slots":             return (!this.Slots_.HasValue          ? String.Empty : String.Format("{0}", this.Slots_.Value));
	case "stack-size":        return (!this.StackSize_.HasValue      ? String.Empty : String.Format("{0}", this.StackSize_.Value));
	case "storage-slots":     return (!this.StorageSlots_.HasValue   ? String.Empty : String.Format("{0}", this.StorageSlots_.Value));
	case "type":              return (!this.Type_.HasValue           ? String.Empty : String.Format("{0}", this.Type_.Value));
	case "valid-targets":     return (!this.ValidTargets_.HasValue   ? String.Empty : String.Format("{0}", this.ValidTargets_.Value));
	// Nullables - Hex Values
	case "id":                return (!this.ID_.HasValue             ? String.Empty : String.Format("{0:X8}", this.ID_.Value));
	case "resource-id":       return (!this.ResourceID_.HasValue     ? String.Empty : String.Format("{0:X4}", this.ResourceID_.Value));
	case "unknown-1":         return (!this.Unknown1_.HasValue       ? String.Empty : String.Format("{0:X4} ({0})", this.Unknown1_.Value));
	case "unknown-2":         return (!this.Unknown2_.HasValue       ? String.Empty : String.Format("{0:X4} ({0})", this.Unknown2_.Value));
	case "unknown-3":         return (!this.Unknown3_.HasValue       ? String.Empty : String.Format("{0:X4} ({0})", this.Unknown3_.Value));
	case "unknown-4":         return (!this.Unknown4_.HasValue       ? String.Empty : String.Format("{0:X4} ({0})", this.Unknown4_.Value));
	case "unknown-5":         return (!this.Unknown5_.HasValue       ? String.Empty : String.Format("{0:X4} ({0})", this.Unknown5_.Value));
	// Nullables - Time Values
	case "activation-time":   return (!this.ActivationTime_.HasValue ? String.Empty : this.FormatTime(this.ActivationTime_.Value / 4.0));
	case "casting-time":      return (!this.CastingTime_.HasValue    ? String.Empty : this.FormatTime(this.CastingTime_.Value / 4.0));
	case "reuse-delay":       return (!this.ReuseDelay_.HasValue     ? String.Empty : this.FormatTime(this.ReuseDelay_.Value));
	case "use-delay":         return (!this.UseDelay_.HasValue       ? String.Empty : this.FormatTime(this.UseDelay_.Value));
	// Nullables - Special/Complex Values
	case "delay":             return (!this.Delay_.HasValue          ? String.Empty : String.Format("{0} ({1:+###0;-###0})", this.Delay_.Value, this.Delay_.Value - 240));
	default:                  return null;
      }
    }

    public override object GetFieldValue(string Field) {
      switch (Field) {
	// Objects
	case "description":       return this.Description_;
	case "english-name":      return this.EnglishName_;
	case "icon":              return this.Icon_;
	case "japanese-name":     return this.JapaneseName_;
	case "log-name-plural":   return this.LogNamePlural_;
	case "log-name-singular": return this.LogNameSingular_;
	// Nullables
	case "activation-time":   return (!this.ActivationTime_.HasValue ? null : (object) this.ActivationTime_.Value);
	case "casting-time":      return (!this.CastingTime_.HasValue    ? null : (object) this.CastingTime_.Value);
	case "damage":            return (!this.Damage_.HasValue         ? null : (object) this.Damage_.Value);
	case "delay":             return (!this.Delay_.HasValue          ? null : (object) this.Delay_.Value);
	case "dps":               return (!this.DPS_.HasValue            ? null : (object) this.DPS_.Value);
	case "element":           return (!this.Element_.HasValue        ? null : (object) this.Element_.Value);
	case "flags":             return (!this.Flags_.HasValue          ? null : (object) this.Flags_.Value);
	case "id":                return (!this.ID_.HasValue             ? null : (object) this.ID_.Value);
	case "jobs":              return (!this.Jobs_.HasValue           ? null : (object) this.Jobs_.Value);
	case "jug-size":          return (!this.JugSize_.HasValue        ? null : (object) this.JugSize_.Value);
	case "level":             return (!this.Level_.HasValue          ? null : (object) this.Level_.Value);
	case "max-charges":       return (!this.MaxCharges_.HasValue     ? null : (object) this.MaxCharges_.Value);
	case "races":             return (!this.Races_.HasValue          ? null : (object) this.Races_.Value);
	case "resource-id":       return (!this.ResourceID_.HasValue     ? null : (object) this.ResourceID_.Value);
	case "reuse-delay":       return (!this.ReuseDelay_.HasValue     ? null : (object) this.ReuseDelay_.Value);
	case "shield-size":       return (!this.ShieldSize_.HasValue     ? null : (object) this.ShieldSize_.Value);
	case "skill":             return (!this.Skill_.HasValue          ? null : (object) this.Skill_.Value);
	case "slots":             return (!this.Slots_.HasValue          ? null : (object) this.Slots_.Value);
	case "stack-size":        return (!this.StackSize_.HasValue      ? null : (object) this.StackSize_.Value);
	case "storage-slots":     return (!this.StorageSlots_.HasValue   ? null : (object) this.StorageSlots_.Value);
	case "type":              return (!this.Type_.HasValue           ? null : (object) this.Type_.Value);
	case "unknown-1":         return (!this.Unknown1_.HasValue       ? null : (object) this.Unknown1_.Value);
	case "unknown-2":         return (!this.Unknown2_.HasValue       ? null : (object) this.Unknown2_.Value);
	case "unknown-3":         return (!this.Unknown3_.HasValue       ? null : (object) this.Unknown3_.Value);
	case "unknown-4":         return (!this.Unknown4_.HasValue       ? null : (object) this.Unknown4_.Value);
	case "unknown-5":         return (!this.Unknown5_.HasValue       ? null : (object) this.Unknown5_.Value);
	case "use-delay":         return (!this.UseDelay_.HasValue       ? null : (object) this.UseDelay_.Value);
	case "valid-targets":     return (!this.ValidTargets_.HasValue   ? null : (object) this.ValidTargets_.Value);
	default:                  return null;
      }
    }

    protected override void LoadField(string Field, System.Xml.XmlElement Node) {
      switch (Field) {
	// "Simple" Fields
	case "activation-time":   this.ActivationTime_  = (byte)          this.LoadUnsignedIntegerField(Node);             break;
	case "casting-time":      this.CastingTime_     = (byte)          this.LoadUnsignedIntegerField(Node);             break;
	case "damage":            this.Damage_          = (ushort)        this.LoadUnsignedIntegerField(Node);             break;
	case "delay":             this.Delay_           = (ushort)        this.LoadUnsignedIntegerField(Node);             break;
	case "description":       this.Description_     =                 this.LoadTextField           (Node);             break;
	case "dps":               this.DPS_             = (ushort)        this.LoadUnsignedIntegerField(Node);             break;
	case "element":           this.Element_         = (Element)       this.LoadHexField            (Node);             break;
	case "english-name":      this.EnglishName_     =                 this.LoadTextField           (Node);             break;
	case "flags":             this.Flags_           = (ItemFlags)     this.LoadHexField            (Node);             break;
	case "id":                this.ID_              = (uint)          this.LoadUnsignedIntegerField(Node);             break;
	case "japanese-name":     this.JapaneseName_    =                 this.LoadTextField           (Node);             break;
	case "jobs":              this.Jobs_            = (Job)           this.LoadHexField            (Node);             break;
	case "jug-size":          this.JugSize_         = (byte)          this.LoadUnsignedIntegerField(Node);             break;
	case "level":             this.Level_           = (ushort)        this.LoadUnsignedIntegerField(Node);             break;
	case "log-name-plural":   this.LogNamePlural_   =                 this.LoadTextField           (Node);             break;
	case "log-name-singular": this.LogNameSingular_ =                 this.LoadTextField           (Node);             break;
	case "max-charges":       this.MaxCharges_      = (byte)          this.LoadUnsignedIntegerField(Node);             break;
	case "races":             this.Races_           = (Race)          this.LoadHexField            (Node);             break;
	case "resource-id":       this.ResourceID_      = (ushort)        this.LoadUnsignedIntegerField(Node);             break;
	case "reuse-delay":       this.ReuseDelay_      = (uint)          this.LoadUnsignedIntegerField(Node);             break;
	case "shield-size":       this.ShieldSize_      = (ushort)        this.LoadUnsignedIntegerField(Node);             break;
	case "skill":             this.Skill_           = (Skill)         this.LoadHexField            (Node);             break;
	case "slots":             this.Slots_           = (EquipmentSlot) this.LoadHexField            (Node);             break;
	case "stack-size":        this.StackSize_       = (ushort)        this.LoadUnsignedIntegerField(Node);             break;
	case "storage-slots":     this.StorageSlots_    = (short)         this.LoadSignedIntegerField  (Node);             break;
	case "type":              this.Type_            = (ItemType)      this.LoadHexField            (Node);             break;
	case "unknown-1":         this.Unknown1_        = (ushort)        this.LoadUnsignedIntegerField(Node);             break;
	case "unknown-2":         this.Unknown2_        = (ushort)        this.LoadUnsignedIntegerField(Node);             break;
	case "unknown-3":         this.Unknown3_        = (ushort)        this.LoadUnsignedIntegerField(Node);             break;
	case "unknown-4":         this.Unknown4_        = (ushort)        this.LoadUnsignedIntegerField(Node);             break;
	case "unknown-5":         this.Unknown5_        = (ushort)        this.LoadUnsignedIntegerField(Node);             break;
	case "use-delay":         this.UseDelay_        = (ushort)        this.LoadUnsignedIntegerField(Node);             break;
	case "valid-targets":     this.ValidTargets_    = (ValidTarget)   this.LoadHexField            (Node);             break;
	// Sub-Things
	case "icon":
	  if (this.Icon_ == null)
	    this.Icon_ = new Graphic();
	  this.LoadThingField(Node, this.Icon_); break;
      }
    }

    #endregion

    #region ROM File Reading

    public enum Language { English, Japanese };
    public enum Type     { Armor, Object, Weapon };

    public static void DeduceLanguageAndType(BinaryReader BR, out Language L, out Type T) {
    byte[] FirstItem = BR.ReadBytes(0x200);
      BR.BaseStream.Seek(-0x200, SeekOrigin.Current);
      FFXIEncryption.Rotate(FirstItem, 5);
      { // Type -> Based on resource ID
      ushort ResourceID = (ushort) (FirstItem[10] + 256 * FirstItem[11]);
	if (ResourceID >= 50000 && ResourceID < 60000)
	  T = Type.Armor;
	else if (ResourceID >= 10000 && ResourceID < 20000)
	  T = Type.Weapon;
	else
	  T = Type.Object;
      }
      { // Language -> based on the 10 NUL bytes before the log names in english data + the log names
      int Offset = 14 + 22 + 22; // common info + english name + japanese name
	if (T == Type.Armor)
	  Offset += 10 + 2; // equipment info + armor info
	else if (T == Type.Weapon)
	  Offset += 10 + 8; // equipment info + weapon info
	L = Language.Japanese;
	if (FirstItem[Offset + 12] != 0 && FirstItem[Offset + 76] != 0) {
	  L = Language.English;
	  for (int i = 0; i < 10; ++i) {
	    if (FirstItem[Offset + i] != 0) {
	      L = Language.Japanese;
	      break;
	    }
	  }
	}
      }
    }

    public bool Read(BinaryReader BR, Language L, Type T) {
      this.Clear();
      try {
      byte[] ItemBytes = BR.ReadBytes(0xC00);
	FFXIEncryption.Rotate(ItemBytes, 5);
	BR = new BinaryReader(new MemoryStream(ItemBytes, false));
	BR.BaseStream.Seek(0x200, SeekOrigin.Begin);
      Graphic G = new Graphic();
      int GraphicSize = BR.ReadInt32();
	if (GraphicSize < 0 || !G.Read(BR) || BR.BaseStream.Position != 0x200 + 4 + GraphicSize) {
	  BR.Close();
	  return false;
	}
	this.Icon_ = G;
	BR.BaseStream.Seek(0, SeekOrigin.Begin);
      } catch { return false; }
      // Common Fields
      this.ID_           =               BR.ReadUInt32();
      this.Flags_        = (ItemFlags)   BR.ReadUInt16();
      this.StackSize_    =               BR.ReadUInt16();
      this.Type_         = (ItemType)    BR.ReadUInt16();
      this.ResourceID_   =               BR.ReadUInt16();
      this.ValidTargets_ = (ValidTarget) BR.ReadUInt16();
      // Extra Fields (Equipment Only)
      if (T == Type.Armor || T == Type.Weapon) {
	this.Level_ =                 BR.ReadUInt16();
	this.Slots_ = (EquipmentSlot) BR.ReadUInt16();
	this.Races_ = (Race)          BR.ReadUInt16();
	this.Jobs_  = (Job)           BR.ReadUInt32();
	if (T == Type.Armor)
	  this.ShieldSize_ = BR.ReadUInt16();
	else { // Weapon
	  this.Damage_  =         BR.ReadUInt16();
	  this.Delay_   =         BR.ReadUInt16();
	  this.DPS_     =         BR.ReadUInt16();
	  this.Skill_   = (Skill) BR.ReadByte();
	  this.JugSize_ =         BR.ReadByte();
	}
      }
      { // Text Fields
      FFXIEncoding E = new FFXIEncoding();
	this.JapaneseName_ = E.GetString(BR.ReadBytes(22)).TrimEnd('\0');
	this.EnglishName_  = E.GetString(BR.ReadBytes(22)).TrimEnd('\0');
	if (L == Language.English) {
	  BR.BaseStream.Seek(10, SeekOrigin.Current); // 10 NULs
	  this.Unknown1_        = BR.ReadUInt16();
	  this.LogNameSingular_ = E.GetString(BR.ReadBytes(64)).TrimEnd('\0');
	  this.LogNamePlural_   = E.GetString(BR.ReadBytes(64)).TrimEnd('\0');
	}
	this.Description_ = E.GetString(BR.ReadBytes(188)).TrimEnd('\0').Replace("\0", "\r\n");
      }
      // Extra Fields
      if (T == Type.Armor || T == Type.Weapon) {
	if (T == Type.Weapon) {
	  this.Unknown2_ = BR.ReadUInt16();
	  this.Unknown3_ = BR.ReadUInt16();
	  this.Unknown4_ = BR.ReadUInt16();
	}
	this.Unknown5_    = BR.ReadUInt16();
	this.MaxCharges_  = BR.ReadByte();
	this.CastingTime_ = BR.ReadByte();
	this.UseDelay_    = BR.ReadUInt16();
	this.ReuseDelay_  = BR.ReadUInt32();
      }
      else {
	switch (this.Type_.Value) { // "Furniture" has extra fields
	  case ItemType.Flowerpot:
	  case ItemType.Furnishing:
	  case ItemType.Mannequin:
	    this.Element_      = (Element) BR.ReadUInt16();
	    this.StorageSlots_ =           BR.ReadInt16();
	    break;
	  case ItemType.Crystal:
	  case ItemType.Fish:
	  case ItemType.UsableItem:
	    this.ActivationTime_ = BR.ReadByte();
	    break;
	}
      }
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
