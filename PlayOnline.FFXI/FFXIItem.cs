// $Id$

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

using PlayOnline.Core;
using PlayOnline.FFXI;

namespace PlayOnline.FFXI {

  public class FFXIItem {

    private long        Index_;
    private byte[]      Data_;
    private FFXIGraphic IconGraphic_;

    public FFXIItem(long Index, byte[] Data, FFXIGraphic IconGraphic) {
      this.Index_       = Index;
      this.Data_        = new byte[0x200];
      Array.Copy(Data, 0, this.Data_, 0, 0x200);
      this.IconGraphic_ = IconGraphic;
    }

    public FFXIItem(long Index, XmlElement DumpedItem) {
      this.Index_ = Index;
      this.UndumpItem(DumpedItem);
    }

    public long Index {
      get {
	return this.Index_;
      }
    }

    public MemoryStream RawData {
      get {
	return new MemoryStream(this.Data_, false);
      }
    }

    public FFXIGraphic IconGraphic {
      get {
	return this.IconGraphic_;
      }
    }

    public override string ToString() {
      return String.Format("{0:0000} - Item #{1:X4} ({2})", this.Index_, this.Common.ID, this.Common.Type);
    }

    private void UndumpItem(XmlElement DumpedItem) {
      this.Common_   = new DumpedItemInfo(DumpedItem);
      this.ENObject_ = new DumpedObjectInfo(DumpedItem);
      this.JPObject_ = this.ENObject_;
      this.ENArmor_  = new DumpedArmorInfo(DumpedItem);
      this.JPArmor_  = this.ENArmor_;
      this.ENWeapon_ = new DumpedWeaponInfo(DumpedItem);
      this.JPWeapon_ = this.ENWeapon_;
      this.IconGraphic_ = FFXIGraphic.UnDump(DumpedItem);
    }

    private static string UndumpStringField(XmlElement DumpedItem, ItemField IF) {
      try   { return DumpedItem.SelectSingleNode(String.Format("./child::*[name(.) = '{0}']", IF)).InnerText; }
      catch { return String.Empty; }
    }

    private static long UndumpIntegerField(XmlElement DumpedItem, ItemField IF) {
      try   { return long.Parse(FFXIItem.UndumpStringField(DumpedItem, IF), NumberStyles.Integer); }
      catch { return 0; }
    }

    private static uint UndumpEnumField(XmlElement DumpedItem, ItemField IF) {
      try   { return uint.Parse(FFXIItem.UndumpStringField(DumpedItem, IF), NumberStyles.HexNumber); }
      catch { return 0; }
    }

    #region Property Base Classes

    public interface IItemInfo {

      ItemField[] GetFields();
      string      GetFieldText (ItemField F);
      object      GetFieldValue(ItemField F);

    }

    public abstract class BasicItemInfo : IItemInfo {

      #region Private Data

      protected uint        ID_;
      protected ItemFlags   Flags_;
      protected ushort      StackSize_;
      protected ItemType    Type_;
      protected ushort      MysteryField_;
      protected ValidTarget ValidTargets_;

      #endregion

      #region Public Properties

      public uint        ID           { get { return this.ID_;           } }
      public ItemFlags   Flags        { get { return this.Flags_;        } }
      public ushort      StackSize    { get { return this.StackSize_;    } }
      public ItemType    Type         { get { return this.Type_;         } }
      public ushort      MysteryField { get { return this.MysteryField_; } }
      public ValidTarget ValidTargets { get { return this.ValidTargets_; } }

      #endregion

      #region Methods

      public virtual ItemField[] GetFields() {
	return new ItemField[] {
	  ItemField.ID,
	  ItemField.Flags,
	  ItemField.StackSize,
	  ItemField.Type,
	  ItemField.MysteryField,
	  ItemField.ValidTargets
	};
      }

      public virtual string GetFieldText(ItemField F) {
	switch (F) {
	  case ItemField.ID:           return String.Format("{0:X8}",      this.ID_);
	  case ItemField.Flags:        return String.Format("<{0:X}> {0}", this.Flags_);
	  case ItemField.StackSize:    return String.Format("{0}",         this.StackSize_);
	  case ItemField.Type:         return String.Format("<{0:X}> {0}", this.Type_);
	  case ItemField.MysteryField: return String.Format("{0:X4}",      this.MysteryField_);
	  case ItemField.ValidTargets: return String.Format("<{0:X}> {0}", this.ValidTargets_);
	}
	return null;
      }

      public virtual object GetFieldValue(ItemField F) {
	switch (F) {
	  case ItemField.ID:           return this.ID_;
	  case ItemField.Flags:        return this.Flags_;
	  case ItemField.StackSize:    return this.StackSize_;
	  case ItemField.Type:         return this.Type_;
	  case ItemField.MysteryField: return this.MysteryField_;
	  case ItemField.ValidTargets: return this.ValidTargets_;
	}
	return null;
      }

      protected void ReadBasicFields(BinaryReader BR) {
	this.ID_           =               BR.ReadUInt32();
	this.Flags_        = (ItemFlags)   BR.ReadUInt16();
	this.StackSize_    =               BR.ReadUInt16();
	this.Type_         = (ItemType)    BR.ReadUInt16();
	this.MysteryField_ =               BR.ReadUInt16();
	this.ValidTargets_ = (ValidTarget) BR.ReadUInt16();
      }

      #endregion

    }

    public abstract class SpecificItemInfo : BasicItemInfo {

      #region Private Data

      protected string JapaneseName_;
      protected string EnglishName_;
      protected string LogNameSingular_;
      protected string LogNamePlural_;
      protected string Description_;

      #endregion

      #region Public Properties

      public string JapaneseName    { get { return this.JapaneseName_;    } }
      public string EnglishName     { get { return this.EnglishName_;     } }
      public string LogNameSingular { get { return this.LogNameSingular_; } }
      public string LogNamePlural   { get { return this.LogNamePlural_;   } }
      public string Description     { get { return this.Description_;     } }

      #endregion

      #region Methods

      public override ItemField[] GetFields() {
      ArrayList Fields = new ArrayList();
	Fields.AddRange(base.GetFields());
	Fields.AddRange(new ItemField[] {
	  ItemField.JapaneseName,
	  ItemField.EnglishName,
	  ItemField.LogNameSingular,
	  ItemField.LogNamePlural,
	  ItemField.Description
	});
	return (ItemField[]) Fields.ToArray(typeof(ItemField));
      }

      public override string GetFieldText(ItemField F) {
	switch (F) {
	  case ItemField.JapaneseName:    return this.JapaneseName;
	  case ItemField.EnglishName:     return this.EnglishName;
	  case ItemField.LogNameSingular: return this.LogNameSingular;
	  case ItemField.LogNamePlural:   return this.LogNamePlural;
	  case ItemField.Description:     return this.Description;
	}
	return base.GetFieldText(F);
      }

      public override object GetFieldValue(ItemField F) {
	switch (F) {
	  case ItemField.JapaneseName:    return this.JapaneseName;
	  case ItemField.EnglishName:     return this.EnglishName;
	  case ItemField.LogNameSingular: return this.LogNameSingular;
	  case ItemField.LogNamePlural:   return this.LogNamePlural;
	  case ItemField.Description:     return this.Description;
	}
	return base.GetFieldValue(F);
      }

      protected void ReadTextFields(BinaryReader BR, ItemDataLanguage L) {
      FFXIEncoding E = new FFXIEncoding();
	this.JapaneseName_ = E.GetString(BR.ReadBytes( 22)).TrimEnd('\0');
	this.EnglishName_  = E.GetString(BR.ReadBytes( 22)).TrimEnd('\0');
	if (L == ItemDataLanguage.English) {
	  BR.BaseStream.Seek(12, SeekOrigin.Current); // 12 unknown bytes, apparently 10 NULs and a ushort
	  this.LogNameSingular_ = E.GetString(BR.ReadBytes( 64)).TrimEnd('\0');
	  this.LogNamePlural_   = E.GetString(BR.ReadBytes( 64)).TrimEnd('\0');
	}
	this.Description_ = E.GetString(BR.ReadBytes(188)).TrimEnd('\0').Replace('\0', '\n');
      }

      #endregion

    }

    public abstract class ObjectInfo : SpecificItemInfo {

      #region Private Data

      protected Element Element_;
      protected short   Storage_;

      #endregion

      #region Public Properties

      public Element Element { get { return this.Element_; } }
      public short   Storage { get { return this.Storage_; } }

      #endregion

      #region Methods

      public override ItemField[] GetFields() {
      ArrayList Fields = new ArrayList();
	Fields.AddRange(base.GetFields());
	Fields.AddRange(new ItemField[] {
	  ItemField.Element,
	  ItemField.Storage
	});
	return (ItemField[]) Fields.ToArray(typeof(ItemField));
      }

      public override string GetFieldText(ItemField F) {
	switch (F) {
	  case ItemField.Element: return String.Format("<{0:X}> {0}", this.Element_);
	  case ItemField.Storage: return String.Format("{0}",         this.Storage_);
	}
	return base.GetFieldText(F);
      }

      public override object GetFieldValue(ItemField F) {
	switch (F) {
	  case ItemField.Element: return this.Element_;
	  case ItemField.Storage: return this.Storage_;
	}
	return base.GetFieldValue(F);
      }

      #endregion

    }

    public abstract class ArmorOrWeaponInfo : SpecificItemInfo {

      #region Private Data

      protected ushort        Level_;
      protected EquipmentSlot Slots_;
      protected Race          Races_;
      protected Job           Jobs_;
      protected byte          MaxCharges_;
      protected byte          CastTime_;
      protected ushort        EquipDelay_;
      protected uint          ReuseTimer_;

      #endregion

      #region Public Properties

      public ushort        Level      { get { return this.Level_;      } }
      public EquipmentSlot Slots      { get { return this.Slots_;      } }
      public Race          Races      { get { return this.Races_;      } }
      public Job           Jobs       { get { return this.Jobs_;       } }
      public byte          MaxCharges { get { return this.MaxCharges_; } }
      public ushort        EquipDelay { get { return this.EquipDelay_; } }
      public uint          ReuseTimer { get { return this.ReuseTimer_; } }

      #endregion

      #region Methods

      public override ItemField[] GetFields() {
      ArrayList Fields = new ArrayList();
	Fields.AddRange(base.GetFields());
	Fields.AddRange(new ItemField[] {
	  ItemField.Level,
	  ItemField.Slots,
	  ItemField.Races,
	  ItemField.Jobs,
	  ItemField.MaxCharges,
	  ItemField.CastTime,
	  ItemField.EquipDelay,
	  ItemField.ReuseTimer
	});
	return (ItemField[]) Fields.ToArray(typeof(ItemField));
      }

      public override string GetFieldText(ItemField F) {
	switch (F) {
	  case ItemField.Level:      return String.Format("{0}",         this.Level_);
	  case ItemField.Slots:      return String.Format("<{0:X}> {0}", this.Slots_);
	  case ItemField.Races:      return String.Format("<{0:X}> {0}", this.Races_);
	  case ItemField.Jobs:       return String.Format("<{0:X}> {0}", this.Jobs_);
	  case ItemField.MaxCharges: return String.Format("{0}",         this.MaxCharges_);
	  case ItemField.CastTime:   return String.Format("{0}s",        this.CastTime_ / 4.0);
	  case ItemField.EquipDelay: return String.Format("{0}", TimeSpan.FromSeconds(this.EquipDelay_));
	  case ItemField.ReuseTimer: return String.Format("{0}", TimeSpan.FromSeconds(this.ReuseTimer_));
	}
	return base.GetFieldText(F);
      }

      public override object GetFieldValue(ItemField F) {
	switch (F) {
	  case ItemField.Level:      return this.Level_;
	  case ItemField.Slots:      return this.Slots_;
	  case ItemField.Races:      return this.Races_;
	  case ItemField.Jobs:       return this.Jobs_;
	  case ItemField.MaxCharges: return this.MaxCharges_;
	  case ItemField.CastTime:   return this.CastTime_;
	  case ItemField.EquipDelay: return this.EquipDelay_;
	  case ItemField.ReuseTimer: return this.ReuseTimer_;
	}
	return base.GetFieldValue(F);
      }

      #endregion

    }

    public abstract class ArmorInfo : ArmorOrWeaponInfo {

      #region Private Data

      protected ushort ShieldSize_;

      #endregion

      #region Public Properties

      public ushort ShieldSize { get { return this.ShieldSize_; } }

      #endregion

      #region Methods

      public override ItemField[] GetFields() {
      ArrayList Fields = new ArrayList();
	Fields.AddRange(base.GetFields());
	Fields.Add(ItemField.ShieldSize);
	return (ItemField[]) Fields.ToArray(typeof(ItemField));
      }

      public override string GetFieldText(ItemField F) {
	switch (F) {
	  case ItemField.ShieldSize: return String.Format("{0}", this.ShieldSize_);
	}
	return base.GetFieldText(F);
      }

      public override object GetFieldValue(ItemField F) {
	switch (F) {
	  case ItemField.ShieldSize: return this.ShieldSize_;
	}
	return base.GetFieldValue(F);
      }

      #endregion

    }

    public abstract class WeaponInfo : ArmorOrWeaponInfo {

      #region Private Data

      protected ushort Damage_;
      protected ushort Delay_;
      protected ushort DPS_;
      protected Skill  Skill_;
      protected byte   JugSize_;

      #endregion

      #region Public Properties

      public ushort Damage  { get { return this.Damage_;  } }
      public ushort Delay   { get { return this.Delay_;   } }
      public ushort DPS     { get { return this.DPS_;     } }
      public Skill  Skill   { get { return this.Skill_;   } }
      public byte   JugSize { get { return this.JugSize_; } }

      #endregion

      #region Methods

      public override ItemField[] GetFields() {
      ArrayList Fields = new ArrayList();
	Fields.AddRange(base.GetFields());
	Fields.AddRange(new ItemField[] {
	  ItemField.Damage,
	  ItemField.Delay,
	  ItemField.DPS,
	  ItemField.Skill,
	  ItemField.JugSize
	});
	return (ItemField[]) Fields.ToArray(typeof(ItemField));
      }

      public override string GetFieldText(ItemField F) {
	switch (F) {
	  case ItemField.Damage:  return String.Format("{0}",                   this.Damage_);
	  case ItemField.Delay:   return String.Format("{0} ({1:+###0;-###0})", this.Delay_, this.Delay_ - 240);
	  case ItemField.DPS:     return String.Format("{0}",                   this.DPS_ / 100.0);
	  case ItemField.Skill:   return String.Format("<{0:X}> {0}",           this.Skill_);
	  case ItemField.JugSize: return String.Format("{0}",                   this.JugSize_);
	}
	return base.GetFieldText(F);
      }

      public override object GetFieldValue(ItemField F) {
	switch (F) {
	  case ItemField.Damage:  return this.Damage_;
	  case ItemField.Delay:   return this.Delay_;
	  case ItemField.DPS:     return this.DPS_;
	  case ItemField.Skill:   return this.Skill_;
	  case ItemField.JugSize: return this.JugSize_;
	}
	return base.GetFieldValue(F);
      }

      #endregion

    }

    #endregion

    #region The Property Handlers

    #region ByteStream-Based

    #region Common Property Handler

    private class CommonInfo : BasicItemInfo {

      public CommonInfo(Stream S) {
      BinaryReader BR = new BinaryReader(S);
	this.ReadBasicFields(BR);
	BR.Close();
      }

    }

    #endregion

    #region Object Property Handler

    private class LangSpecificObjectInfo : ObjectInfo {

      public LangSpecificObjectInfo(Stream S, ItemDataLanguage L) {
      BinaryReader BR = new BinaryReader(S);
	this.ReadBasicFields(BR);
	this.ReadTextFields(BR, L);
	this.Element_ = (Element) BR.ReadUInt16();
	this.Storage_ =           BR.ReadInt16();
	BR.Close();
	this.L = L;
      }

      private ItemDataLanguage L;

      public override ItemField[] GetFields() {
	if (this.L == ItemDataLanguage.English)
	  return base.GetFields();
	else {
	ArrayList Fields = new ArrayList(base.GetFields());
	  Fields.Remove(ItemField.LogNameSingular);
	  Fields.Remove(ItemField.LogNamePlural);
	  return (ItemField[]) Fields.ToArray(typeof(ItemField));
	}
      }

    }

    #endregion

    #region Armor Property Handler

    private class LangSpecificArmorInfo : ArmorInfo {

      public LangSpecificArmorInfo(Stream S, ItemDataLanguage L) {
      BinaryReader BR = new BinaryReader(S);
	this.ReadBasicFields(BR);
	this.Level_      =                 BR.ReadUInt16();
	this.Slots_      = (EquipmentSlot) BR.ReadUInt16();
	this.Races_      = (Race)          BR.ReadUInt16();
	this.Jobs_       = (Job)           BR.ReadUInt16();
	this.ShieldSize_ =                 BR.ReadUInt16();
	this.ReadTextFields(BR, L);
	this.MaxCharges_ =                 BR.ReadByte();
	this.CastTime_   =                 BR.ReadByte();
	this.EquipDelay_ =                 BR.ReadUInt16();
	this.ReuseTimer_ =                 BR.ReadUInt32();
	BR.Close();
	this.L = L;
      }

      private ItemDataLanguage L;

      public override ItemField[] GetFields() {
	if (this.L == ItemDataLanguage.English)
	  return base.GetFields();
	else {
	ArrayList Fields = new ArrayList(base.GetFields());
	  Fields.Remove(ItemField.LogNameSingular);
	  Fields.Remove(ItemField.LogNamePlural);
	  return (ItemField[]) Fields.ToArray(typeof(ItemField));
	}
      }

    }

    #endregion

    #region Weapon Property Handler

    private class LangSpecificWeaponInfo : WeaponInfo {

      public LangSpecificWeaponInfo(Stream S, ItemDataLanguage L) {
      BinaryReader BR = new BinaryReader(S);
	this.ReadBasicFields(BR);
	this.Level_      =                 BR.ReadUInt16();
	this.Slots_      = (EquipmentSlot) BR.ReadUInt16();
	this.Races_      = (Race)          BR.ReadUInt16();
	this.Jobs_       = (Job)           BR.ReadUInt16();
	this.Damage_     =                 BR.ReadUInt16();
	this.Delay_      =                 BR.ReadUInt16();
	this.DPS_        =                 BR.ReadUInt16();
	this.Skill_      = (Skill)         BR.ReadByte();
	this.JugSize_    =                 BR.ReadByte();
	this.ReadTextFields(BR, L);
	/* Unknown */                      BR.ReadUInt16();
	/* Unknown */                      BR.ReadUInt16();
	/* Unknown */                      BR.ReadUInt16();
	this.MaxCharges_ =                 BR.ReadByte();
	this.CastTime_   =                 BR.ReadByte();
	this.EquipDelay_ =                 BR.ReadUInt16();
	this.ReuseTimer_ =                 BR.ReadUInt32();
	BR.Close();
	this.L = L;
      }

      private ItemDataLanguage L;

      public override ItemField[] GetFields() {
	if (this.L == ItemDataLanguage.English)
	  return base.GetFields();
	else {
	ArrayList Fields = new ArrayList(base.GetFields());
	  Fields.Remove(ItemField.LogNameSingular);
	  Fields.Remove(ItemField.LogNamePlural);
	  return (ItemField[]) Fields.ToArray(typeof(ItemField));
	}
      }

    }

    #endregion

    #endregion

    #region XML-Based

    #region Common Property Handler

    private class DumpedItemInfo : BasicItemInfo {

      public DumpedItemInfo(XmlElement DumpedItem) {
	this.Flags_        = (ItemFlags)   FFXIItem.UndumpEnumField   (DumpedItem, ItemField.Flags);
	this.ID_           = (uint)        FFXIItem.UndumpIntegerField(DumpedItem, ItemField.ID);
	this.StackSize_    = (ushort)      FFXIItem.UndumpIntegerField(DumpedItem, ItemField.StackSize);
	this.Type_         = (ItemType)    FFXIItem.UndumpEnumField   (DumpedItem, ItemField.Type);
	this.MysteryField_ = (ushort)      FFXIItem.UndumpIntegerField(DumpedItem, ItemField.MysteryField);
	this.ValidTargets_ = (ValidTarget) FFXIItem.UndumpEnumField   (DumpedItem, ItemField.ValidTargets);
      }

    }

    #endregion

    #region Object Property Handler

    private class DumpedObjectInfo : ObjectInfo {

      public DumpedObjectInfo(XmlElement DumpedItem) {
	this.Flags_           = (ItemFlags)   FFXIItem.UndumpEnumField   (DumpedItem, ItemField.Flags);
	this.ID_              = (uint)        FFXIItem.UndumpIntegerField(DumpedItem, ItemField.ID);
	this.StackSize_       = (ushort)      FFXIItem.UndumpIntegerField(DumpedItem, ItemField.StackSize);
	this.Type_            = (ItemType)    FFXIItem.UndumpEnumField   (DumpedItem, ItemField.Type);
	this.MysteryField_    = (ushort)      FFXIItem.UndumpIntegerField(DumpedItem, ItemField.MysteryField);
	this.ValidTargets_    = (ValidTarget) FFXIItem.UndumpEnumField   (DumpedItem, ItemField.ValidTargets);
	this.EnglishName_     =               FFXIItem.UndumpStringField (DumpedItem, ItemField.EnglishName);
	this.JapaneseName_    =               FFXIItem.UndumpStringField (DumpedItem, ItemField.JapaneseName);
	this.LogNameSingular_ =               FFXIItem.UndumpStringField (DumpedItem, ItemField.LogNameSingular);
	this.LogNamePlural_   =               FFXIItem.UndumpStringField (DumpedItem, ItemField.LogNamePlural);
	this.Description_     =               FFXIItem.UndumpStringField (DumpedItem, ItemField.Description);
	this.Element_         = (Element)     FFXIItem.UndumpEnumField   (DumpedItem, ItemField.Element);
	this.Storage_         = (short)       FFXIItem.UndumpIntegerField(DumpedItem, ItemField.Storage);
      }

    }

    #endregion

    #region Armor Property Handler

    private class DumpedArmorInfo : ArmorInfo {

      public DumpedArmorInfo(XmlElement DumpedItem) {
	this.Flags_           = (ItemFlags)     FFXIItem.UndumpEnumField   (DumpedItem, ItemField.Flags);
	this.ID_              = (uint)          FFXIItem.UndumpIntegerField(DumpedItem, ItemField.ID);
	this.StackSize_       = (ushort)        FFXIItem.UndumpIntegerField(DumpedItem, ItemField.StackSize);
	this.Type_            = (ItemType)      FFXIItem.UndumpEnumField   (DumpedItem, ItemField.Type);
	this.MysteryField_    = (ushort)        FFXIItem.UndumpIntegerField(DumpedItem, ItemField.MysteryField);
	this.ValidTargets_    = (ValidTarget)   FFXIItem.UndumpEnumField   (DumpedItem, ItemField.ValidTargets);
	this.Level_           = (ushort)        FFXIItem.UndumpIntegerField(DumpedItem, ItemField.Level);
	this.Slots_           = (EquipmentSlot) FFXIItem.UndumpEnumField   (DumpedItem, ItemField.Slots);
	this.Races_           = (Race)          FFXIItem.UndumpEnumField   (DumpedItem, ItemField.Races);
	this.Jobs_            = (Job)           FFXIItem.UndumpEnumField   (DumpedItem, ItemField.Jobs);
	this.ShieldSize_      = (ushort)        FFXIItem.UndumpIntegerField(DumpedItem, ItemField.ShieldSize);
	this.EnglishName_     =                 FFXIItem.UndumpStringField (DumpedItem, ItemField.EnglishName);
	this.JapaneseName_    =                 FFXIItem.UndumpStringField (DumpedItem, ItemField.JapaneseName);
	this.LogNameSingular_ =                 FFXIItem.UndumpStringField (DumpedItem, ItemField.LogNameSingular);
	this.LogNamePlural_   =                 FFXIItem.UndumpStringField (DumpedItem, ItemField.LogNamePlural);
	this.Description_     =                 FFXIItem.UndumpStringField (DumpedItem, ItemField.Description);
	this.MaxCharges_      = (byte)          FFXIItem.UndumpIntegerField(DumpedItem, ItemField.MaxCharges);
	this.CastTime_        = (byte)          FFXIItem.UndumpIntegerField(DumpedItem, ItemField.CastTime);
	this.EquipDelay_      = (ushort)        FFXIItem.UndumpIntegerField(DumpedItem, ItemField.EquipDelay);
	this.ReuseTimer_      = (uint)          FFXIItem.UndumpIntegerField(DumpedItem, ItemField.ReuseTimer);
      }

    }

    #endregion

    #region Weapon Property Handler

    private class DumpedWeaponInfo : WeaponInfo {

      public DumpedWeaponInfo(XmlElement DumpedItem) {
	this.Flags_           = (ItemFlags)     FFXIItem.UndumpEnumField   (DumpedItem, ItemField.Flags);
	this.ID_              = (uint)          FFXIItem.UndumpIntegerField(DumpedItem, ItemField.ID);
	this.StackSize_       = (ushort)        FFXIItem.UndumpIntegerField(DumpedItem, ItemField.StackSize);
	this.Type_            = (ItemType)      FFXIItem.UndumpEnumField   (DumpedItem, ItemField.Type);
	this.MysteryField_    = (ushort)        FFXIItem.UndumpIntegerField(DumpedItem, ItemField.MysteryField);
	this.ValidTargets_    = (ValidTarget)   FFXIItem.UndumpEnumField   (DumpedItem, ItemField.ValidTargets);
	this.Level_           = (ushort)        FFXIItem.UndumpIntegerField(DumpedItem, ItemField.Level);
	this.Slots_           = (EquipmentSlot) FFXIItem.UndumpEnumField   (DumpedItem, ItemField.Slots);
	this.Races_           = (Race)          FFXIItem.UndumpEnumField   (DumpedItem, ItemField.Races);
	this.Jobs_            = (Job)           FFXIItem.UndumpEnumField   (DumpedItem, ItemField.Jobs);
	this.Damage_          = (ushort)        FFXIItem.UndumpIntegerField(DumpedItem, ItemField.Damage);
	this.Delay_           = (ushort)        FFXIItem.UndumpIntegerField(DumpedItem, ItemField.Delay);
	this.DPS_             = (ushort)        FFXIItem.UndumpIntegerField(DumpedItem, ItemField.DPS);
	this.Skill_           = (Skill)         FFXIItem.UndumpEnumField   (DumpedItem, ItemField.Skill);
	this.JugSize_         = (byte)          FFXIItem.UndumpIntegerField(DumpedItem, ItemField.JugSize);
	this.EnglishName_     =                 FFXIItem.UndumpStringField (DumpedItem, ItemField.EnglishName);
	this.JapaneseName_    =                 FFXIItem.UndumpStringField (DumpedItem, ItemField.JapaneseName);
	this.LogNameSingular_ =                 FFXIItem.UndumpStringField (DumpedItem, ItemField.LogNameSingular);
	this.LogNamePlural_   =                 FFXIItem.UndumpStringField (DumpedItem, ItemField.LogNamePlural);
	this.Description_     =                 FFXIItem.UndumpStringField (DumpedItem, ItemField.Description);
	this.MaxCharges_      = (byte)          FFXIItem.UndumpIntegerField(DumpedItem, ItemField.MaxCharges);
	this.CastTime_        = (byte)          FFXIItem.UndumpIntegerField(DumpedItem, ItemField.CastTime);
	this.EquipDelay_      = (ushort)        FFXIItem.UndumpIntegerField(DumpedItem, ItemField.EquipDelay);
	this.ReuseTimer_      = (uint)          FFXIItem.UndumpIntegerField(DumpedItem, ItemField.ReuseTimer);
      }

    }

    #endregion

    #endregion

    #endregion

    #region The Main Properties

    private BasicItemInfo Common_ = null;
    public BasicItemInfo Common {
      get {
	if (this.Common_ == null)
	  this.Common_ = new CommonInfo(this.RawData);
	return this.Common_;
      }
    }

    private ObjectInfo ENObject_ = null;
    public ObjectInfo ENObject {
      get {
	if (this.ENObject_ == null)
	  this.ENObject_ = new LangSpecificObjectInfo(this.RawData, ItemDataLanguage.English);
	return this.ENObject_;
      }
    }

    private ObjectInfo JPObject_ = null;
    public ObjectInfo JPObject {
      get {
	if (this.JPObject_ == null)
	  this.JPObject_ = new LangSpecificObjectInfo(this.RawData, ItemDataLanguage.Japanese);
	return this.JPObject_;
      }
    }

    private ArmorInfo ENArmor_ = null;
    public ArmorInfo ENArmor {
      get {
	if (this.ENArmor_ == null)
	  this.ENArmor_ = new LangSpecificArmorInfo(this.RawData, ItemDataLanguage.English);
	return this.ENArmor_;
      }
    }

    private ArmorInfo JPArmor_ = null;
    public ArmorInfo JPArmor {
      get {
	if (this.JPArmor_ == null)
	  this.JPArmor_ = new LangSpecificArmorInfo(this.RawData, ItemDataLanguage.Japanese);
	return this.JPArmor_;
      }
    }

    private WeaponInfo ENWeapon_ = null;
    public WeaponInfo ENWeapon {
      get {
	if (this.ENWeapon_ == null)
	  this.ENWeapon_ = new LangSpecificWeaponInfo(this.RawData, ItemDataLanguage.English);
	return this.ENWeapon_;
      }
    }

    private WeaponInfo JPWeapon_ = null;
    public WeaponInfo JPWeapon {
      get {
	if (this.JPWeapon_ == null)
	  this.JPWeapon_ = new LangSpecificWeaponInfo(this.RawData, ItemDataLanguage.Japanese);
	return this.JPWeapon_;
      }
    }

    #endregion

    // Generic access
    public IItemInfo GetInfo(ItemDataLanguage L, ItemDataType T) {
      switch (T) {
	case ItemDataType.Armor:  return ((L == ItemDataLanguage.English) ? this.ENArmor  : this.JPArmor );
	case ItemDataType.Object: return ((L == ItemDataLanguage.English) ? this.ENObject : this.JPObject);
	case ItemDataType.Weapon: return ((L == ItemDataLanguage.English) ? this.ENWeapon : this.JPWeapon);
      }
      return null;
    }

    public static ItemField[] GetFields(ItemDataLanguage L, ItemDataType T) {
    byte[] DummyData = new byte[0x200];
    MemoryStream DummyStream = new MemoryStream(DummyData, false);
      switch (T) {
	case ItemDataType.Armor:
	  return new LangSpecificArmorInfo(DummyStream, L).GetFields();
	case ItemDataType.Object:
	  return new LangSpecificObjectInfo(DummyStream, L).GetFields();
	case ItemDataType.Weapon:
	  return new LangSpecificWeaponInfo(DummyStream, L).GetFields();
      }
      return null;
    }

  }

}
