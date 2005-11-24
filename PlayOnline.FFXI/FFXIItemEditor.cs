// $Id$

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using PlayOnline.Core;

namespace PlayOnline.FFXI {

  public partial class FFXIItemEditor : UserControl {

    private FFXIItem ItemToShow_     = null;
    private CheckBox LockedViewMode_ = null;
    private int      LogicalHeight;

    #region Properties

    [Browsable(false)]
    public FFXIItem Item {
      get {
	return this.ItemToShow_;
      }
      set {
	this.ItemToShow_ = value;
	this.ShowItem();
      }
    }

    [Browsable(false)]
    public FFXIItem.IItemInfo ItemInfo {
      get {
	if (this.ItemToShow_ == null)
	  return null;
	return this.ItemToShow_.GetInfo(this.ChosenItemLanguage, this.ChosenItemType);
      }
    }

    [Browsable(false)]
    public ItemDataLanguage ChosenItemLanguage {
      get {
	return ((this.chkViewAsEArmor.Checked || this.chkViewAsEObject.Checked || this.chkViewAsEWeapon.Checked) ? ItemDataLanguage.English : ItemDataLanguage.Japanese);
      }
    }

    [Browsable(false)]
    public ItemDataType ChosenItemType {
      get {
	if (this.chkViewAsEArmor.Checked || this.chkViewAsJArmor.Checked)
	  return ItemDataType.Armor;
	if (this.chkViewAsEWeapon.Checked || this.chkViewAsJWeapon.Checked)
	  return ItemDataType.Weapon;
	return ItemDataType.Object;
      }
    }

    // This is currently a no-op, as actual item editing is not supported (yet)
    [Browsable(true), Category("Behavior"), Description("Controls whether or not the item information can be edited."), DefaultValue(true)]
    public bool ReadOnly {
      get { return true; }
      set { }
    }

    #endregion

    #region Field Marks

    public enum Mark { None, New, Changed, Removed };

    private Control GetFieldControl(ItemField IF) {
      switch (IF) {
	case ItemField.CastTime:        return this.txtCastTime;
	case ItemField.DPS:             return this.txtDPS;
	case ItemField.Damage:          return this.txtDamage;
	case ItemField.Delay:           return this.txtDelay;
	case ItemField.Description:     return this.txtDescription;
	case ItemField.Element:         return this.txtElement;
	case ItemField.EnglishName:     return this.txtEName;
	case ItemField.EquipDelay:      return this.txtEquipDelay;
	case ItemField.Flags:           return this.txtFlags;
	case ItemField.ID:              return this.txtID;
	case ItemField.JapaneseName:    return this.txtJName;
	case ItemField.Jobs:            return this.txtJobs;
	case ItemField.JugSize:         return this.txtJugSize;
	case ItemField.Level:           return this.txtLevel;
	case ItemField.LogNamePlural:   return this.txtPlural;
	case ItemField.LogNameSingular: return this.txtSingular;
	case ItemField.MaxCharges:      return this.txtMaxCharges;
	case ItemField.MysteryField:    return this.txtID;
	case ItemField.Races:           return this.txtRaces;
	case ItemField.ReuseTimer:      return this.txtReuseTimer;
	case ItemField.ShieldSize:      return this.txtShieldSize;
	case ItemField.Skill:           return this.txtSkill;
	case ItemField.Slots:           return this.txtSlots;
	case ItemField.StackSize:       return this.txtStackSize;
	case ItemField.Storage:         return this.txtStorage;
	case ItemField.Type:            return this.txtType;
	case ItemField.ValidTargets:    return this.txtValidTargets;
      }
      return null;
    }

    public void UnmarkAll() {
      this.MarkIcon(Mark.None);
      foreach (ItemField IF in Enum.GetValues(typeof(ItemField)))
	this.MarkField(IF, Mark.None);
    }

    private void MarkControl(Control C, Mark M) {
      if (C != null) {
	switch (M) {
	  case Mark.None:    C.BackColor = SystemColors.Control;       break;
	  case Mark.New:     C.BackColor = Color.LightGreen;           break;
	  case Mark.Changed: C.BackColor = Color.LightGoldenrodYellow; break;
	  case Mark.Removed: C.BackColor = Color.LightPink;            break;
	}
	C.Font = new Font(C.Font, ((M == Mark.None) ? FontStyle.Regular : FontStyle.Bold));
      }
    }

    public void MarkField(ItemField IF, Mark M) {
      this.MarkControl(this.GetFieldControl(IF), M);
    }

    public void MarkIcon(Mark M) {
      this.MarkControl(this.picIcon, M);
    }

    #endregion

    #region Public Methods

    public FFXIItemEditor() {
      this.InitializeComponent();
      this.ShowItem();
    }

    public void Reset() {
      this.UnmarkAll();
      this.UnlockViewMode();
    }

    public void UnlockViewMode() {
      this.chkViewAsEArmor.Enabled  = true;
      this.chkViewAsEObject.Enabled = true;
      this.chkViewAsEWeapon.Enabled = true;
      this.chkViewAsJArmor.Enabled  = true;
      this.chkViewAsJObject.Enabled = true;
      this.chkViewAsJWeapon.Enabled = true;
      this.LockedViewMode_ = null;
    }

    public void LockViewMode() { // Locks selection, but leaves autodetection active
      this.chkViewAsEArmor.Enabled  = false;
      this.chkViewAsEObject.Enabled = false;
      this.chkViewAsEWeapon.Enabled = false;
      this.chkViewAsJArmor.Enabled  = false;
      this.chkViewAsJObject.Enabled = false;
      this.chkViewAsJWeapon.Enabled = false;
      this.LockedViewMode_ = null;
    }

    public void LockViewMode(ItemDataLanguage L, ItemDataType T) { // Locks view to the specified mode
      this.LockedViewMode_ = null;
      this.chkViewAsEArmor.Enabled  = (L == ItemDataLanguage.English  && T == ItemDataType.Armor);
      this.chkViewAsEObject.Enabled = (L == ItemDataLanguage.English  && T == ItemDataType.Object);
      this.chkViewAsEWeapon.Enabled = (L == ItemDataLanguage.English  && T == ItemDataType.Weapon);
      this.chkViewAsJArmor.Enabled  = (L == ItemDataLanguage.Japanese && T == ItemDataType.Armor);
      this.chkViewAsJObject.Enabled = (L == ItemDataLanguage.Japanese && T == ItemDataType.Object);
      this.chkViewAsJWeapon.Enabled = (L == ItemDataLanguage.Japanese && T == ItemDataType.Weapon);
      if (this.chkViewAsEArmor.Enabled)  this.LockedViewMode_ = this.chkViewAsEArmor;
      if (this.chkViewAsEObject.Enabled) this.LockedViewMode_ = this.chkViewAsEObject;
      if (this.chkViewAsEWeapon.Enabled) this.LockedViewMode_ = this.chkViewAsEWeapon;
      if (this.chkViewAsJArmor.Enabled)  this.LockedViewMode_ = this.chkViewAsJArmor;
      if (this.chkViewAsJObject.Enabled) this.LockedViewMode_ = this.chkViewAsJObject;
      if (this.chkViewAsJWeapon.Enabled) this.LockedViewMode_ = this.chkViewAsJWeapon;
    }

    #endregion

    #region Private Methods

    private CheckBox DetectLanguage(FFXIItem.IItemInfo II1, CheckBox CB1, FFXIItem.IItemInfo II2, CheckBox CB2) {
      if (II1.GetFieldText(ItemField.Description) != String.Empty && II1.GetFieldText(ItemField.Description) == String.Empty)
	return CB1;
      if (II1.GetFieldText(ItemField.Description) == String.Empty && II2.GetFieldText(ItemField.Description) != String.Empty)
	return CB2;
    string LogNames = II1.GetFieldText(ItemField.LogNameSingular) + II1.GetFieldText(ItemField.LogNamePlural);
      if (LogNames == String.Empty)
	return CB2;
      else {
      bool ExtendedCharSeen = false;
	foreach (char C in LogNames) {
	  if ((int) C > 0x100 && C != '♂' && C != '♀') { // Mannequins use male/female sign, so allow those
	    ExtendedCharSeen = true;
	    break;
	  }
	}
	return (!ExtendedCharSeen ? CB1 : CB2);
      }
    }

    private void DetectViewMode(FFXIItem I) {
    CheckBox SelectedMode = this.chkViewAsEObject;
      if (I.Common.MysteryField >= 10000 && I.Common.MysteryField < 20000) // Weapon
	SelectedMode = this.DetectLanguage(I.ENWeapon, this.chkViewAsEWeapon, I.JPWeapon, this.chkViewAsJWeapon);
      else if (I.Common.MysteryField >= 50000 && I.Common.MysteryField < 60000) // Armor
	SelectedMode = this.DetectLanguage(I.ENArmor, this.chkViewAsEArmor, I.JPArmor, this.chkViewAsJArmor);
      else // Other Items
	SelectedMode = this.DetectLanguage(I.ENObject, this.chkViewAsEObject, I.JPObject, this.chkViewAsJObject);
      SelectedMode.Checked = true;
    }

    private void ShowItem() {
    FFXIItem I = this.ItemToShow_;
      this.chkViewAsEArmor.Checked = this.chkViewAsEObject.Checked = this.chkViewAsEWeapon.Checked = false;
      this.chkViewAsJArmor.Checked = this.chkViewAsJObject.Checked = this.chkViewAsJWeapon.Checked = false;
      this.ResetInfoGroups();
      this.picIcon.Image = null;
      this.ttToolTip.SetToolTip(this.picIcon, null);
      if (I != null) {
	if (I.IconGraphic != null) {
	  this.picIcon.Image = I.IconGraphic.Bitmap;
	  this.ttToolTip.SetToolTip(this.picIcon, I.IconGraphic.ToString());
	}
	if (this.LockedViewMode_ != null)
	  this.LockedViewMode_.Checked = true;
	else
	  this.DetectViewMode(I);
      }
      else {
	this.txtID.Text           = this.txtType.Text      = String.Empty;
	this.txtEName.Text        = this.txtJName.Text     = String.Empty;
	this.txtFlags.Text        = this.txtStackSize.Text = String.Empty;
	this.txtDescription.Text  = String.Empty;
	this.txtValidTargets.Text = String.Empty;
      }
    }

    private void FillCommonFields(FFXIItem.IItemInfo II) {
      this.txtID.Text           = II.GetFieldText(ItemField.ID) + " / " + II.GetFieldText(ItemField.MysteryField);
      this.txtType.Text         = II.GetFieldText(ItemField.Type);
      this.txtFlags.Text        = II.GetFieldText(ItemField.Flags);
      this.txtStackSize.Text    = II.GetFieldText(ItemField.StackSize);
      this.txtValidTargets.Text = II.GetFieldText(ItemField.ValidTargets);
      this.txtEName.Text        = II.GetFieldText(ItemField.EnglishName);
      this.txtJName.Text        = II.GetFieldText(ItemField.JapaneseName);
      this.txtDescription.Text  = II.GetFieldText(ItemField.Description).Replace("\n", "\r\n");
      if (this.ChosenItemLanguage == ItemDataLanguage.English) {
	this.txtSingular.Text   = II.GetFieldText(ItemField.LogNameSingular);
	this.txtPlural.Text     = II.GetFieldText(ItemField.LogNamePlural);
	this.ShowInfoGroup(this.grpLogStrings);
      }
    }

    private void FillEquipmentFields(FFXIItem.IItemInfo II) {
      this.txtLevel.Text      = II.GetFieldText(ItemField.Level);
      this.txtJobs.Text       = II.GetFieldText(ItemField.Jobs);
      this.txtSlots.Text      = II.GetFieldText(ItemField.Slots);
      this.txtRaces.Text      = II.GetFieldText(ItemField.Races);
      this.ShowInfoGroup(this.grpEquipmentInfo);
      if ((byte) II.GetFieldValue(ItemField.MaxCharges) > 0) {
	this.txtMaxCharges.Text = II.GetFieldText(ItemField.MaxCharges);
	this.txtCastTime.Text   = II.GetFieldText(ItemField.CastTime);
	this.txtEquipDelay.Text = II.GetFieldText(ItemField.EquipDelay);
	this.txtReuseTimer.Text = II.GetFieldText(ItemField.ReuseTimer);
	this.ShowInfoGroup(this.grpEnchantmentInfo);
      }
    }

    private void FillEnchantmentFields(FFXIItem.IItemInfo II) {
      if ((byte) II.GetFieldValue(ItemField.MaxCharges) > 0) {
	this.txtMaxCharges.Text = II.GetFieldText(ItemField.MaxCharges);
	this.txtCastTime.Text   = II.GetFieldText(ItemField.CastTime);
	this.txtEquipDelay.Text = II.GetFieldText(ItemField.EquipDelay);
	this.txtReuseTimer.Text = II.GetFieldText(ItemField.ReuseTimer);
	this.ShowInfoGroup(this.grpEnchantmentInfo);
      }
    }

    private void FillObjectFields(FFXIItem.IItemInfo II) {
      this.FillCommonFields(II);
    ItemType IT = (ItemType) II.GetFieldValue(ItemField.Type);
      if (IT == ItemType.Flowerpot || IT == ItemType.Furnishing || IT == ItemType.Mannequin) {
	this.txtElement.Text = II.GetFieldText(ItemField.Element);
	this.txtStorage.Text = II.GetFieldText(ItemField.Storage);
	this.ShowInfoGroup(this.grpFurnitureInfo);
      }
    }

    private void FillArmorFields(FFXIItem.IItemInfo II) {
      this.ResetInfoGroups();
      this.FillCommonFields(II);
      this.FillEquipmentFields(II);
      if ((ushort) II.GetFieldValue(ItemField.ShieldSize) > 0) { // Shield
	this.txtShieldSize.Text = II.GetFieldText(ItemField.ShieldSize);
	this.ShowInfoGroup(this.grpShieldInfo);
      }
      this.FillEnchantmentFields(II);
    }

    private void FillWeaponFields(FFXIItem.IItemInfo II) {
      this.ResetInfoGroups();
      this.FillCommonFields(II);
      this.FillEquipmentFields(II);
      this.txtDamage.Text  = II.GetFieldText(ItemField.Damage);
      this.txtDelay.Text   = II.GetFieldText(ItemField.Delay);
      this.txtDPS.Text     = II.GetFieldText(ItemField.DPS);
      this.txtSkill.Text   = II.GetFieldText(ItemField.Skill);
      this.ShowInfoGroup(this.grpWeaponInfo);
      if ((byte) II.GetFieldValue(ItemField.JugSize) > 0) { // BST Jug
	this.txtJugSize.Text = II.GetFieldText(ItemField.JugSize);
	this.ShowInfoGroup(this.grpJugInfo);
      }
      this.FillEnchantmentFields(II);
    }

    private void ResetInfoGroups() {
      this.grpEnchantmentInfo.Visible = false;
      this.grpEquipmentInfo.Visible   = false;
      this.grpFurnitureInfo.Visible   = false;
      this.grpJugInfo.Visible         = false;
      this.grpLogStrings.Visible      = false;
      this.grpShieldInfo.Visible      = false;
      this.grpWeaponInfo.Visible      = false;
      this.LogicalHeight = this.grpViewMode.Top + this.grpViewMode.Height + this.grpCommonInfo.Height;
      this.Height = this.LogicalHeight;
    }

    private void ShowInfoGroup(GroupBox GB) {
      GB.Top              = this.LogicalHeight;
      this.LogicalHeight += GB.Height;
      GB.Visible          = true;
      this.Height         = this.LogicalHeight;
    }

    #endregion

    #region Events - View Mode

    private void chkView_Click(object sender, System.EventArgs e) {
      this.chkViewAsEArmor.Checked  = sender.Equals(this.chkViewAsEArmor);
      this.chkViewAsEObject.Checked = sender.Equals(this.chkViewAsEObject);
      this.chkViewAsEWeapon.Checked = sender.Equals(this.chkViewAsEWeapon);
      this.chkViewAsJArmor.Checked  = sender.Equals(this.chkViewAsJArmor);
      this.chkViewAsJObject.Checked = sender.Equals(this.chkViewAsJObject);
      this.chkViewAsJWeapon.Checked = sender.Equals(this.chkViewAsJWeapon);
    }

    private void chkViewAsEArmor_CheckedChanged(object sender, System.EventArgs e) {
      if (this.ItemToShow_ != null && this.chkViewAsEArmor.Checked)
	this.FillArmorFields(this.ItemToShow_.ENArmor);
    }

    private void chkViewAsEObject_CheckedChanged(object sender, System.EventArgs e) {
      if (this.ItemToShow_ != null && this.chkViewAsEObject.Checked)
	this.FillObjectFields(this.ItemToShow_.ENObject);
    }

    private void chkViewAsEWeapon_CheckedChanged(object sender, System.EventArgs e) {
      if (this.ItemToShow_ != null && this.chkViewAsEWeapon.Checked)
	this.FillWeaponFields(this.ItemToShow_.ENWeapon);
    }

    private void chkViewAsJArmor_CheckedChanged(object sender, System.EventArgs e) {
      if (this.ItemToShow_ != null && this.chkViewAsJArmor.Checked)
	this.FillArmorFields(this.ItemToShow_.JPArmor);
    }

    private void chkViewAsJObject_CheckedChanged(object sender, System.EventArgs e) {
      if (this.ItemToShow_ != null && this.chkViewAsJObject.Checked)
	this.FillObjectFields(this.ItemToShow_.JPObject);
    }

    private void chkViewAsJWeapon_CheckedChanged(object sender, System.EventArgs e) {
      if (this.ItemToShow_ != null && this.chkViewAsJWeapon.Checked)
	this.FillWeaponFields(this.ItemToShow_.JPWeapon);
    }

    #endregion

  }

}
