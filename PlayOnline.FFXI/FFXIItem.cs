using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;

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

    #region Property Base Classes

    public interface IItemInfo {

      ItemField[] GetFields();
      string      GetFieldText (ItemField F);
      object      GetFieldValue(ItemField F);

    }

    public abstract class BasicItemInfo : IItemInfo {

      #region Private Data

      protected uint      ID_;
      protected ItemFlags Flags_;
      protected ushort    StackSize_;
      protected ItemType  Type_;

      #endregion

      #region Public Properties

      public uint      ID        { get { return this.ID_;        } }
      public ItemFlags Flags     { get { return this.Flags_;     } }
      public ushort    StackSize { get { return this.StackSize_; } }
      public ItemType  Type      { get { return this.Type_;      } }

      #endregion

      #region Methods

      public virtual ItemField[] GetFields() {
	return new ItemField[] {
	  ItemField.ID,
	  ItemField.Flags,
	  ItemField.StackSize,
	  ItemField.Type
	};
      }

      public virtual string GetFieldText(ItemField F) {
	switch (F) {
	  case ItemField.ID:        return String.Format("{0:X8}",      this.ID_);
	  case ItemField.Flags:     return String.Format("<{0:X}> {0}", this.Flags_);
	  case ItemField.StackSize: return String.Format("{0}",         this.StackSize_);
	  case ItemField.Type:      return String.Format("<{0:X}> {0}", this.Type_);
	}
	return null;
      }

      public virtual object GetFieldValue(ItemField F) {
	return null;
      }

      protected void ReadBasicFields(BinaryReader BR) {
	this.ID_        =             BR.ReadUInt32();
	this.Flags_     = (ItemFlags) BR.ReadUInt16();
	this.StackSize_ =             BR.ReadUInt16();
	this.Type_      = (ItemType)  BR.ReadUInt16();
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

      #endregion

      #region Public Properties

      #endregion

      #region Methods

      public override string GetFieldText(ItemField F) {
	return base.GetFieldText(F);
      }

      #endregion

    }

    public abstract class ArmorOrWeaponInfo : SpecificItemInfo {

      #region Private Data

      protected uint          ResourceID_;
      protected ushort        Level_;
      protected EquipmentSlot Slots_;
      protected Race          Races_;
      protected Job           Jobs_;
      protected byte          MaxCharges_;
      protected ushort        EquipDelay_;
      protected uint          ReuseTimer_;

      #endregion

      #region Public Properties

      public uint          ResourceID { get { return this.ResourceID_; } }
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
	  ItemField.ResourceID,
	  ItemField.Level,
	  ItemField.Slots,
	  ItemField.Races,
	  ItemField.Jobs,
	  ItemField.MaxCharges,
	  ItemField.EquipDelay,
	  ItemField.ReuseTimer
	});
	return (ItemField[]) Fields.ToArray(typeof(ItemField));
      }

      public override string GetFieldText(ItemField F) {
	switch (F) {
	  case ItemField.ResourceID: return String.Format("{0:X8}",      this.ResourceID_);
	  case ItemField.Level:      return String.Format("{0}",         this.Level_);
	  case ItemField.Slots:      return String.Format("<{0:X}> {0}", this.Slots_);
	  case ItemField.Races:      return String.Format("<{0:X}> {0}", this.Races_);
	  case ItemField.Jobs:       return String.Format("<{0:X}> {0}", this.Jobs_);
	  case ItemField.MaxCharges: return String.Format("{0}",         this.MaxCharges_);
	  case ItemField.EquipDelay: return String.Format("{0}", TimeSpan.FromSeconds(this.EquipDelay_));
	  case ItemField.ReuseTimer: return String.Format("{0}", TimeSpan.FromSeconds(this.ReuseTimer_));
	}
	return base.GetFieldText(F);
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

      #endregion

    }

    public abstract class WeaponInfo : ArmorOrWeaponInfo {

      #region Private Data

      protected ushort    Damage_;
      protected ushort    Delay_;
      protected ItemSkill Skill_;

      #endregion

      #region Public Properties

      public ushort    Damage     { get { return this.Damage_; } }
      public ushort    Delay      { get { return this.Delay_;  } }
      public ItemSkill Skill      { get { return this.Skill_;  } }

      #endregion

      #region Methods

      public override ItemField[] GetFields() {
      ArrayList Fields = new ArrayList();
	Fields.AddRange(base.GetFields());
	Fields.AddRange(new ItemField[] {
	  ItemField.Damage,
	  ItemField.Delay,
	  ItemField.Skill
	});
	return (ItemField[]) Fields.ToArray(typeof(ItemField));
      }

      public override string GetFieldText(ItemField F) {
	switch (F) {
	  case ItemField.Damage: return String.Format("{0}",                   this.Damage_);
	  case ItemField.Skill:  return String.Format("<{0:X}> {0}",           this.Skill_);
	  case ItemField.Delay:  return String.Format("{0} ({1:+###0;-###0})", this.Delay_, this.Delay_ - 240);
	}
	return base.GetFieldText(F);
      }

      #endregion

    }

    #endregion

    #region The Property Handlers

    #region Common Properties

    private class CommonInfo : BasicItemInfo {

      public CommonInfo(Stream S) {
      BinaryReader BR = new BinaryReader(S);
	this.ReadBasicFields(BR);
	BR.Close();
      }

    }

    private CommonInfo Common_ = null;
    public BasicItemInfo Common {
      get {
	if (this.Common_ == null)
	  this.Common_ = new CommonInfo(this.RawData);
	return this.Common_;
      }
    }

    #endregion

    #region Object Properties

    private class LangSpecificObjectInfo : ObjectInfo {

      public LangSpecificObjectInfo(Stream S, ItemDataLanguage L) {
      BinaryReader BR = new BinaryReader(S);
	this.ReadBasicFields(BR);
	/* Unknown */ BR.ReadUInt16();
	/* Unknown */ BR.ReadUInt16();
	this.ReadTextFields(BR, L);
	/* Unknown */ BR.ReadUInt16();
	/* Unknown */ BR.ReadUInt16();
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

    private LangSpecificObjectInfo ENObject_ = null;
    public ObjectInfo ENObject {
      get {
	if (this.ENObject_ == null)
	  this.ENObject_ = new LangSpecificObjectInfo(this.RawData, ItemDataLanguage.English);
	return this.ENObject_;
      }
    }

    private LangSpecificObjectInfo JPObject_ = null;
    public ObjectInfo JPObject {
      get {
	if (this.JPObject_ == null)
	  this.JPObject_ = new LangSpecificObjectInfo(this.RawData, ItemDataLanguage.Japanese);
	return this.JPObject_;
      }
    }

    #endregion

    #region Armor Properties

    private class LangSpecificArmorInfo : ArmorInfo {

      public LangSpecificArmorInfo(Stream S, ItemDataLanguage L) {
      BinaryReader BR = new BinaryReader(S);
	this.ReadBasicFields(BR);
	this.ResourceID_ =                 BR.ReadUInt32();
	this.Level_      =                 BR.ReadUInt16();
	this.Slots_      = (EquipmentSlot) BR.ReadUInt16();
	this.Races_      = (Race)          BR.ReadUInt16();
	this.Jobs_       = (Job)           BR.ReadUInt16();
	this.ShieldSize_ =                 BR.ReadUInt16();
	this.ReadTextFields(BR, L);
	this.MaxCharges_ =                 BR.ReadByte();
	/* Unknown */                      BR.ReadByte();
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

    private LangSpecificArmorInfo ENArmor_ = null;
    public ArmorInfo ENArmor {
      get {
	if (this.ENArmor_ == null)
	  this.ENArmor_ = new LangSpecificArmorInfo(this.RawData, ItemDataLanguage.English);
	return this.ENArmor_;
      }
    }

    private LangSpecificArmorInfo JPArmor_ = null;
    public ArmorInfo JPArmor {
      get {
	if (this.JPArmor_ == null)
	  this.JPArmor_ = new LangSpecificArmorInfo(this.RawData, ItemDataLanguage.Japanese);
	return this.JPArmor_;
      }
    }

    #endregion

    #region Weapon Properties

    private class LangSpecificWeaponInfo : WeaponInfo {

      public LangSpecificWeaponInfo(Stream S, ItemDataLanguage L) {
      BinaryReader BR = new BinaryReader(S);
	this.ReadBasicFields(BR);
	this.ResourceID_ =                 BR.ReadUInt32();
	this.Level_      =                 BR.ReadUInt16();
	this.Slots_      = (EquipmentSlot) BR.ReadUInt16();
	this.Races_      = (Race)          BR.ReadUInt16();
	this.Jobs_       = (Job)           BR.ReadUInt16();
	this.Damage_     =                 BR.ReadUInt16();
	this.Delay_      =                 BR.ReadUInt16();
	/* Unknown */                      BR.ReadUInt16();
	this.Skill_      = (ItemSkill)     BR.ReadByte();
	/* Unknown */                      BR.ReadByte();
	this.ReadTextFields(BR, L);
	/* Unknown */                      BR.ReadUInt16();
	/* Unknown */                      BR.ReadUInt16();
	/* Unknown */                      BR.ReadUInt16();
	this.MaxCharges_ =                 BR.ReadByte();
	/* Unknown */                      BR.ReadByte();
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

    private LangSpecificWeaponInfo ENWeapon_ = null;
    public WeaponInfo ENWeapon {
      get {
	if (this.ENWeapon_ == null)
	  this.ENWeapon_ = new LangSpecificWeaponInfo(this.RawData, ItemDataLanguage.English);
	return this.ENWeapon_;
      }
    }

    private LangSpecificWeaponInfo JPWeapon_ = null;
    public WeaponInfo JPWeapon {
      get {
	if (this.JPWeapon_ == null)
	  this.JPWeapon_ = new LangSpecificWeaponInfo(this.RawData, ItemDataLanguage.Japanese);
	return this.JPWeapon_;
      }
    }

    #endregion

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

  }

}
