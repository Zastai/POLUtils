// $Id$

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using PlayOnline.Core;

namespace PlayOnline.FFXI {

  public class FFXIItemEditor : System.Windows.Forms.UserControl {

    private FFXIItem ItemToShow_     = null;
    private CheckBox LockedViewMode_ = null;
    private int      LogicalHeight;

    #region Controls

    private System.Windows.Forms.GroupBox grpViewMode;
    private System.Windows.Forms.CheckBox chkViewAsJWeapon;
    private System.Windows.Forms.CheckBox chkViewAsJObject;
    private System.Windows.Forms.CheckBox chkViewAsJArmor;
    private System.Windows.Forms.CheckBox chkViewAsEWeapon;
    private System.Windows.Forms.CheckBox chkViewAsEObject;
    private System.Windows.Forms.CheckBox chkViewAsEArmor;
    private System.Windows.Forms.Label lblRaces;
    private System.Windows.Forms.TextBox txtRaces;
    private System.Windows.Forms.Label lblSlots;
    private System.Windows.Forms.TextBox txtSlots;
    private System.Windows.Forms.Label lblJobs;
    private System.Windows.Forms.TextBox txtJobs;
    private System.Windows.Forms.Label lblLevel;
    private System.Windows.Forms.TextBox txtLevel;
    private System.Windows.Forms.GroupBox grpCommonInfo;
    private System.Windows.Forms.Label lblDescription;
    private System.Windows.Forms.Label lblJName;
    private System.Windows.Forms.Label lblEName;
    private System.Windows.Forms.TextBox txtJName;
    private System.Windows.Forms.TextBox txtEName;
    private System.Windows.Forms.TextBox txtDescription;
    private System.Windows.Forms.Label lblStackSize;
    private System.Windows.Forms.Label lblFlags;
    private System.Windows.Forms.Label lblType;
    private System.Windows.Forms.Label lblID;
    private System.Windows.Forms.TextBox txtStackSize;
    private System.Windows.Forms.TextBox txtFlags;
    private System.Windows.Forms.TextBox txtType;
    private System.Windows.Forms.TextBox txtID;
    private System.Windows.Forms.PictureBox picIcon;
    private System.Windows.Forms.ToolTip ttToolTip;
    private System.Windows.Forms.GroupBox grpEquipmentInfo;
    private System.Windows.Forms.GroupBox grpWeaponInfo;
    private System.Windows.Forms.GroupBox grpFurnitureInfo;
    private System.Windows.Forms.Label lblStorage;
    private System.Windows.Forms.TextBox txtStorage;
    private System.Windows.Forms.Label lblElement;
    private System.Windows.Forms.TextBox txtElement;
    private System.Windows.Forms.Label lblShieldSize;
    private System.Windows.Forms.TextBox txtShieldSize;
    private System.Windows.Forms.Label lblSkill;
    private System.Windows.Forms.TextBox txtSkill;
    private System.Windows.Forms.Label lblDelay;
    private System.Windows.Forms.Label lblDamage;
    private System.Windows.Forms.TextBox txtDelay;
    private System.Windows.Forms.TextBox txtDamage;
    private System.Windows.Forms.Label lblDPS;
    private System.Windows.Forms.TextBox txtDPS;
    private System.Windows.Forms.GroupBox grpJugInfo;
    private System.Windows.Forms.Label lblJugSize;
    private System.Windows.Forms.TextBox txtJugSize;
    private System.Windows.Forms.GroupBox grpShieldInfo;
    private System.Windows.Forms.GroupBox grpEnchantmentInfo;
    private System.Windows.Forms.Label lblCastTime;
    private System.Windows.Forms.TextBox txtCastTime;
    private System.Windows.Forms.Label lblEquipDelay;
    private System.Windows.Forms.Label lblReuseTimer;
    private System.Windows.Forms.Label lblMaxCharges;
    private System.Windows.Forms.TextBox txtReuseTimer;
    private System.Windows.Forms.TextBox txtEquipDelay;
    private System.Windows.Forms.TextBox txtMaxCharges;
    private System.Windows.Forms.Label lblValidTargets;
    private System.Windows.Forms.TextBox txtValidTargets;
    private System.Windows.Forms.GroupBox grpLogStrings;
    private System.Windows.Forms.Label lblPlural;
    private System.Windows.Forms.Label lblSingular;
    private System.Windows.Forms.TextBox txtPlural;
    private System.Windows.Forms.TextBox txtSingular;
    private System.ComponentModel.IContainer components;

    #endregion

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

    #region Component Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && this.components != null)
	this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      this.components = new System.ComponentModel.Container();
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FFXIItemEditor));
      this.grpViewMode = new System.Windows.Forms.GroupBox();
      this.chkViewAsJWeapon = new System.Windows.Forms.CheckBox();
      this.chkViewAsJObject = new System.Windows.Forms.CheckBox();
      this.chkViewAsJArmor = new System.Windows.Forms.CheckBox();
      this.chkViewAsEWeapon = new System.Windows.Forms.CheckBox();
      this.chkViewAsEObject = new System.Windows.Forms.CheckBox();
      this.chkViewAsEArmor = new System.Windows.Forms.CheckBox();
      this.grpEquipmentInfo = new System.Windows.Forms.GroupBox();
      this.lblRaces = new System.Windows.Forms.Label();
      this.txtRaces = new System.Windows.Forms.TextBox();
      this.lblSlots = new System.Windows.Forms.Label();
      this.txtSlots = new System.Windows.Forms.TextBox();
      this.lblJobs = new System.Windows.Forms.Label();
      this.txtJobs = new System.Windows.Forms.TextBox();
      this.lblLevel = new System.Windows.Forms.Label();
      this.txtLevel = new System.Windows.Forms.TextBox();
      this.grpCommonInfo = new System.Windows.Forms.GroupBox();
      this.lblValidTargets = new System.Windows.Forms.Label();
      this.txtValidTargets = new System.Windows.Forms.TextBox();
      this.lblDescription = new System.Windows.Forms.Label();
      this.lblJName = new System.Windows.Forms.Label();
      this.lblEName = new System.Windows.Forms.Label();
      this.txtJName = new System.Windows.Forms.TextBox();
      this.txtEName = new System.Windows.Forms.TextBox();
      this.txtDescription = new System.Windows.Forms.TextBox();
      this.lblStackSize = new System.Windows.Forms.Label();
      this.lblFlags = new System.Windows.Forms.Label();
      this.lblType = new System.Windows.Forms.Label();
      this.lblID = new System.Windows.Forms.Label();
      this.txtStackSize = new System.Windows.Forms.TextBox();
      this.txtFlags = new System.Windows.Forms.TextBox();
      this.txtType = new System.Windows.Forms.TextBox();
      this.txtID = new System.Windows.Forms.TextBox();
      this.picIcon = new System.Windows.Forms.PictureBox();
      this.ttToolTip = new System.Windows.Forms.ToolTip(this.components);
      this.grpWeaponInfo = new System.Windows.Forms.GroupBox();
      this.lblDPS = new System.Windows.Forms.Label();
      this.txtDPS = new System.Windows.Forms.TextBox();
      this.lblDelay = new System.Windows.Forms.Label();
      this.lblDamage = new System.Windows.Forms.Label();
      this.txtDelay = new System.Windows.Forms.TextBox();
      this.txtDamage = new System.Windows.Forms.TextBox();
      this.lblSkill = new System.Windows.Forms.Label();
      this.txtSkill = new System.Windows.Forms.TextBox();
      this.grpFurnitureInfo = new System.Windows.Forms.GroupBox();
      this.lblStorage = new System.Windows.Forms.Label();
      this.txtStorage = new System.Windows.Forms.TextBox();
      this.lblElement = new System.Windows.Forms.Label();
      this.txtElement = new System.Windows.Forms.TextBox();
      this.grpShieldInfo = new System.Windows.Forms.GroupBox();
      this.lblShieldSize = new System.Windows.Forms.Label();
      this.txtShieldSize = new System.Windows.Forms.TextBox();
      this.grpJugInfo = new System.Windows.Forms.GroupBox();
      this.lblJugSize = new System.Windows.Forms.Label();
      this.txtJugSize = new System.Windows.Forms.TextBox();
      this.grpEnchantmentInfo = new System.Windows.Forms.GroupBox();
      this.lblCastTime = new System.Windows.Forms.Label();
      this.txtCastTime = new System.Windows.Forms.TextBox();
      this.lblEquipDelay = new System.Windows.Forms.Label();
      this.lblReuseTimer = new System.Windows.Forms.Label();
      this.lblMaxCharges = new System.Windows.Forms.Label();
      this.txtReuseTimer = new System.Windows.Forms.TextBox();
      this.txtEquipDelay = new System.Windows.Forms.TextBox();
      this.txtMaxCharges = new System.Windows.Forms.TextBox();
      this.grpLogStrings = new System.Windows.Forms.GroupBox();
      this.lblPlural = new System.Windows.Forms.Label();
      this.lblSingular = new System.Windows.Forms.Label();
      this.txtPlural = new System.Windows.Forms.TextBox();
      this.txtSingular = new System.Windows.Forms.TextBox();
      this.grpViewMode.SuspendLayout();
      this.grpEquipmentInfo.SuspendLayout();
      this.grpCommonInfo.SuspendLayout();
      this.grpWeaponInfo.SuspendLayout();
      this.grpFurnitureInfo.SuspendLayout();
      this.grpShieldInfo.SuspendLayout();
      this.grpJugInfo.SuspendLayout();
      this.grpEnchantmentInfo.SuspendLayout();
      this.grpLogStrings.SuspendLayout();
      this.SuspendLayout();
      // 
      // grpViewMode
      // 
      this.grpViewMode.AccessibleDescription = resources.GetString("grpViewMode.AccessibleDescription");
      this.grpViewMode.AccessibleName = resources.GetString("grpViewMode.AccessibleName");
      this.grpViewMode.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("grpViewMode.Anchor")));
      this.grpViewMode.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpViewMode.BackgroundImage")));
      this.grpViewMode.Controls.Add(this.chkViewAsJWeapon);
      this.grpViewMode.Controls.Add(this.chkViewAsJObject);
      this.grpViewMode.Controls.Add(this.chkViewAsJArmor);
      this.grpViewMode.Controls.Add(this.chkViewAsEWeapon);
      this.grpViewMode.Controls.Add(this.chkViewAsEObject);
      this.grpViewMode.Controls.Add(this.chkViewAsEArmor);
      this.grpViewMode.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("grpViewMode.Dock")));
      this.grpViewMode.Enabled = ((bool)(resources.GetObject("grpViewMode.Enabled")));
      this.grpViewMode.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.grpViewMode.Font = ((System.Drawing.Font)(resources.GetObject("grpViewMode.Font")));
      this.grpViewMode.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("grpViewMode.ImeMode")));
      this.grpViewMode.Location = ((System.Drawing.Point)(resources.GetObject("grpViewMode.Location")));
      this.grpViewMode.Name = "grpViewMode";
      this.grpViewMode.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("grpViewMode.RightToLeft")));
      this.grpViewMode.Size = ((System.Drawing.Size)(resources.GetObject("grpViewMode.Size")));
      this.grpViewMode.TabIndex = ((int)(resources.GetObject("grpViewMode.TabIndex")));
      this.grpViewMode.TabStop = false;
      this.grpViewMode.Text = resources.GetString("grpViewMode.Text");
      this.ttToolTip.SetToolTip(this.grpViewMode, resources.GetString("grpViewMode.ToolTip"));
      this.grpViewMode.Visible = ((bool)(resources.GetObject("grpViewMode.Visible")));
      // 
      // chkViewAsJWeapon
      // 
      this.chkViewAsJWeapon.AccessibleDescription = resources.GetString("chkViewAsJWeapon.AccessibleDescription");
      this.chkViewAsJWeapon.AccessibleName = resources.GetString("chkViewAsJWeapon.AccessibleName");
      this.chkViewAsJWeapon.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkViewAsJWeapon.Anchor")));
      this.chkViewAsJWeapon.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkViewAsJWeapon.Appearance")));
      this.chkViewAsJWeapon.AutoCheck = false;
      this.chkViewAsJWeapon.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkViewAsJWeapon.BackgroundImage")));
      this.chkViewAsJWeapon.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkViewAsJWeapon.CheckAlign")));
      this.chkViewAsJWeapon.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkViewAsJWeapon.Dock")));
      this.chkViewAsJWeapon.Enabled = ((bool)(resources.GetObject("chkViewAsJWeapon.Enabled")));
      this.chkViewAsJWeapon.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkViewAsJWeapon.FlatStyle")));
      this.chkViewAsJWeapon.Font = ((System.Drawing.Font)(resources.GetObject("chkViewAsJWeapon.Font")));
      this.chkViewAsJWeapon.Image = ((System.Drawing.Image)(resources.GetObject("chkViewAsJWeapon.Image")));
      this.chkViewAsJWeapon.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkViewAsJWeapon.ImageAlign")));
      this.chkViewAsJWeapon.ImageIndex = ((int)(resources.GetObject("chkViewAsJWeapon.ImageIndex")));
      this.chkViewAsJWeapon.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkViewAsJWeapon.ImeMode")));
      this.chkViewAsJWeapon.Location = ((System.Drawing.Point)(resources.GetObject("chkViewAsJWeapon.Location")));
      this.chkViewAsJWeapon.Name = "chkViewAsJWeapon";
      this.chkViewAsJWeapon.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkViewAsJWeapon.RightToLeft")));
      this.chkViewAsJWeapon.Size = ((System.Drawing.Size)(resources.GetObject("chkViewAsJWeapon.Size")));
      this.chkViewAsJWeapon.TabIndex = ((int)(resources.GetObject("chkViewAsJWeapon.TabIndex")));
      this.chkViewAsJWeapon.Text = resources.GetString("chkViewAsJWeapon.Text");
      this.chkViewAsJWeapon.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkViewAsJWeapon.TextAlign")));
      this.ttToolTip.SetToolTip(this.chkViewAsJWeapon, resources.GetString("chkViewAsJWeapon.ToolTip"));
      this.chkViewAsJWeapon.Visible = ((bool)(resources.GetObject("chkViewAsJWeapon.Visible")));
      this.chkViewAsJWeapon.Click += new System.EventHandler(this.chkView_Click);
      this.chkViewAsJWeapon.CheckedChanged += new System.EventHandler(this.chkViewAsJWeapon_CheckedChanged);
      // 
      // chkViewAsJObject
      // 
      this.chkViewAsJObject.AccessibleDescription = resources.GetString("chkViewAsJObject.AccessibleDescription");
      this.chkViewAsJObject.AccessibleName = resources.GetString("chkViewAsJObject.AccessibleName");
      this.chkViewAsJObject.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkViewAsJObject.Anchor")));
      this.chkViewAsJObject.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkViewAsJObject.Appearance")));
      this.chkViewAsJObject.AutoCheck = false;
      this.chkViewAsJObject.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkViewAsJObject.BackgroundImage")));
      this.chkViewAsJObject.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkViewAsJObject.CheckAlign")));
      this.chkViewAsJObject.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkViewAsJObject.Dock")));
      this.chkViewAsJObject.Enabled = ((bool)(resources.GetObject("chkViewAsJObject.Enabled")));
      this.chkViewAsJObject.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkViewAsJObject.FlatStyle")));
      this.chkViewAsJObject.Font = ((System.Drawing.Font)(resources.GetObject("chkViewAsJObject.Font")));
      this.chkViewAsJObject.Image = ((System.Drawing.Image)(resources.GetObject("chkViewAsJObject.Image")));
      this.chkViewAsJObject.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkViewAsJObject.ImageAlign")));
      this.chkViewAsJObject.ImageIndex = ((int)(resources.GetObject("chkViewAsJObject.ImageIndex")));
      this.chkViewAsJObject.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkViewAsJObject.ImeMode")));
      this.chkViewAsJObject.Location = ((System.Drawing.Point)(resources.GetObject("chkViewAsJObject.Location")));
      this.chkViewAsJObject.Name = "chkViewAsJObject";
      this.chkViewAsJObject.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkViewAsJObject.RightToLeft")));
      this.chkViewAsJObject.Size = ((System.Drawing.Size)(resources.GetObject("chkViewAsJObject.Size")));
      this.chkViewAsJObject.TabIndex = ((int)(resources.GetObject("chkViewAsJObject.TabIndex")));
      this.chkViewAsJObject.Text = resources.GetString("chkViewAsJObject.Text");
      this.chkViewAsJObject.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkViewAsJObject.TextAlign")));
      this.ttToolTip.SetToolTip(this.chkViewAsJObject, resources.GetString("chkViewAsJObject.ToolTip"));
      this.chkViewAsJObject.Visible = ((bool)(resources.GetObject("chkViewAsJObject.Visible")));
      this.chkViewAsJObject.Click += new System.EventHandler(this.chkView_Click);
      this.chkViewAsJObject.CheckedChanged += new System.EventHandler(this.chkViewAsJObject_CheckedChanged);
      // 
      // chkViewAsJArmor
      // 
      this.chkViewAsJArmor.AccessibleDescription = resources.GetString("chkViewAsJArmor.AccessibleDescription");
      this.chkViewAsJArmor.AccessibleName = resources.GetString("chkViewAsJArmor.AccessibleName");
      this.chkViewAsJArmor.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkViewAsJArmor.Anchor")));
      this.chkViewAsJArmor.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkViewAsJArmor.Appearance")));
      this.chkViewAsJArmor.AutoCheck = false;
      this.chkViewAsJArmor.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkViewAsJArmor.BackgroundImage")));
      this.chkViewAsJArmor.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkViewAsJArmor.CheckAlign")));
      this.chkViewAsJArmor.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkViewAsJArmor.Dock")));
      this.chkViewAsJArmor.Enabled = ((bool)(resources.GetObject("chkViewAsJArmor.Enabled")));
      this.chkViewAsJArmor.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkViewAsJArmor.FlatStyle")));
      this.chkViewAsJArmor.Font = ((System.Drawing.Font)(resources.GetObject("chkViewAsJArmor.Font")));
      this.chkViewAsJArmor.Image = ((System.Drawing.Image)(resources.GetObject("chkViewAsJArmor.Image")));
      this.chkViewAsJArmor.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkViewAsJArmor.ImageAlign")));
      this.chkViewAsJArmor.ImageIndex = ((int)(resources.GetObject("chkViewAsJArmor.ImageIndex")));
      this.chkViewAsJArmor.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkViewAsJArmor.ImeMode")));
      this.chkViewAsJArmor.Location = ((System.Drawing.Point)(resources.GetObject("chkViewAsJArmor.Location")));
      this.chkViewAsJArmor.Name = "chkViewAsJArmor";
      this.chkViewAsJArmor.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkViewAsJArmor.RightToLeft")));
      this.chkViewAsJArmor.Size = ((System.Drawing.Size)(resources.GetObject("chkViewAsJArmor.Size")));
      this.chkViewAsJArmor.TabIndex = ((int)(resources.GetObject("chkViewAsJArmor.TabIndex")));
      this.chkViewAsJArmor.Text = resources.GetString("chkViewAsJArmor.Text");
      this.chkViewAsJArmor.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkViewAsJArmor.TextAlign")));
      this.ttToolTip.SetToolTip(this.chkViewAsJArmor, resources.GetString("chkViewAsJArmor.ToolTip"));
      this.chkViewAsJArmor.Visible = ((bool)(resources.GetObject("chkViewAsJArmor.Visible")));
      this.chkViewAsJArmor.Click += new System.EventHandler(this.chkView_Click);
      this.chkViewAsJArmor.CheckedChanged += new System.EventHandler(this.chkViewAsJArmor_CheckedChanged);
      // 
      // chkViewAsEWeapon
      // 
      this.chkViewAsEWeapon.AccessibleDescription = resources.GetString("chkViewAsEWeapon.AccessibleDescription");
      this.chkViewAsEWeapon.AccessibleName = resources.GetString("chkViewAsEWeapon.AccessibleName");
      this.chkViewAsEWeapon.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkViewAsEWeapon.Anchor")));
      this.chkViewAsEWeapon.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkViewAsEWeapon.Appearance")));
      this.chkViewAsEWeapon.AutoCheck = false;
      this.chkViewAsEWeapon.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkViewAsEWeapon.BackgroundImage")));
      this.chkViewAsEWeapon.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkViewAsEWeapon.CheckAlign")));
      this.chkViewAsEWeapon.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkViewAsEWeapon.Dock")));
      this.chkViewAsEWeapon.Enabled = ((bool)(resources.GetObject("chkViewAsEWeapon.Enabled")));
      this.chkViewAsEWeapon.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkViewAsEWeapon.FlatStyle")));
      this.chkViewAsEWeapon.Font = ((System.Drawing.Font)(resources.GetObject("chkViewAsEWeapon.Font")));
      this.chkViewAsEWeapon.Image = ((System.Drawing.Image)(resources.GetObject("chkViewAsEWeapon.Image")));
      this.chkViewAsEWeapon.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkViewAsEWeapon.ImageAlign")));
      this.chkViewAsEWeapon.ImageIndex = ((int)(resources.GetObject("chkViewAsEWeapon.ImageIndex")));
      this.chkViewAsEWeapon.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkViewAsEWeapon.ImeMode")));
      this.chkViewAsEWeapon.Location = ((System.Drawing.Point)(resources.GetObject("chkViewAsEWeapon.Location")));
      this.chkViewAsEWeapon.Name = "chkViewAsEWeapon";
      this.chkViewAsEWeapon.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkViewAsEWeapon.RightToLeft")));
      this.chkViewAsEWeapon.Size = ((System.Drawing.Size)(resources.GetObject("chkViewAsEWeapon.Size")));
      this.chkViewAsEWeapon.TabIndex = ((int)(resources.GetObject("chkViewAsEWeapon.TabIndex")));
      this.chkViewAsEWeapon.Text = resources.GetString("chkViewAsEWeapon.Text");
      this.chkViewAsEWeapon.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkViewAsEWeapon.TextAlign")));
      this.ttToolTip.SetToolTip(this.chkViewAsEWeapon, resources.GetString("chkViewAsEWeapon.ToolTip"));
      this.chkViewAsEWeapon.Visible = ((bool)(resources.GetObject("chkViewAsEWeapon.Visible")));
      this.chkViewAsEWeapon.Click += new System.EventHandler(this.chkView_Click);
      this.chkViewAsEWeapon.CheckedChanged += new System.EventHandler(this.chkViewAsEWeapon_CheckedChanged);
      // 
      // chkViewAsEObject
      // 
      this.chkViewAsEObject.AccessibleDescription = resources.GetString("chkViewAsEObject.AccessibleDescription");
      this.chkViewAsEObject.AccessibleName = resources.GetString("chkViewAsEObject.AccessibleName");
      this.chkViewAsEObject.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkViewAsEObject.Anchor")));
      this.chkViewAsEObject.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkViewAsEObject.Appearance")));
      this.chkViewAsEObject.AutoCheck = false;
      this.chkViewAsEObject.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkViewAsEObject.BackgroundImage")));
      this.chkViewAsEObject.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkViewAsEObject.CheckAlign")));
      this.chkViewAsEObject.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkViewAsEObject.Dock")));
      this.chkViewAsEObject.Enabled = ((bool)(resources.GetObject("chkViewAsEObject.Enabled")));
      this.chkViewAsEObject.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkViewAsEObject.FlatStyle")));
      this.chkViewAsEObject.Font = ((System.Drawing.Font)(resources.GetObject("chkViewAsEObject.Font")));
      this.chkViewAsEObject.Image = ((System.Drawing.Image)(resources.GetObject("chkViewAsEObject.Image")));
      this.chkViewAsEObject.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkViewAsEObject.ImageAlign")));
      this.chkViewAsEObject.ImageIndex = ((int)(resources.GetObject("chkViewAsEObject.ImageIndex")));
      this.chkViewAsEObject.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkViewAsEObject.ImeMode")));
      this.chkViewAsEObject.Location = ((System.Drawing.Point)(resources.GetObject("chkViewAsEObject.Location")));
      this.chkViewAsEObject.Name = "chkViewAsEObject";
      this.chkViewAsEObject.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkViewAsEObject.RightToLeft")));
      this.chkViewAsEObject.Size = ((System.Drawing.Size)(resources.GetObject("chkViewAsEObject.Size")));
      this.chkViewAsEObject.TabIndex = ((int)(resources.GetObject("chkViewAsEObject.TabIndex")));
      this.chkViewAsEObject.Text = resources.GetString("chkViewAsEObject.Text");
      this.chkViewAsEObject.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkViewAsEObject.TextAlign")));
      this.ttToolTip.SetToolTip(this.chkViewAsEObject, resources.GetString("chkViewAsEObject.ToolTip"));
      this.chkViewAsEObject.Visible = ((bool)(resources.GetObject("chkViewAsEObject.Visible")));
      this.chkViewAsEObject.Click += new System.EventHandler(this.chkView_Click);
      this.chkViewAsEObject.CheckedChanged += new System.EventHandler(this.chkViewAsEObject_CheckedChanged);
      // 
      // chkViewAsEArmor
      // 
      this.chkViewAsEArmor.AccessibleDescription = resources.GetString("chkViewAsEArmor.AccessibleDescription");
      this.chkViewAsEArmor.AccessibleName = resources.GetString("chkViewAsEArmor.AccessibleName");
      this.chkViewAsEArmor.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkViewAsEArmor.Anchor")));
      this.chkViewAsEArmor.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkViewAsEArmor.Appearance")));
      this.chkViewAsEArmor.AutoCheck = false;
      this.chkViewAsEArmor.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkViewAsEArmor.BackgroundImage")));
      this.chkViewAsEArmor.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkViewAsEArmor.CheckAlign")));
      this.chkViewAsEArmor.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkViewAsEArmor.Dock")));
      this.chkViewAsEArmor.Enabled = ((bool)(resources.GetObject("chkViewAsEArmor.Enabled")));
      this.chkViewAsEArmor.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkViewAsEArmor.FlatStyle")));
      this.chkViewAsEArmor.Font = ((System.Drawing.Font)(resources.GetObject("chkViewAsEArmor.Font")));
      this.chkViewAsEArmor.Image = ((System.Drawing.Image)(resources.GetObject("chkViewAsEArmor.Image")));
      this.chkViewAsEArmor.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkViewAsEArmor.ImageAlign")));
      this.chkViewAsEArmor.ImageIndex = ((int)(resources.GetObject("chkViewAsEArmor.ImageIndex")));
      this.chkViewAsEArmor.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkViewAsEArmor.ImeMode")));
      this.chkViewAsEArmor.Location = ((System.Drawing.Point)(resources.GetObject("chkViewAsEArmor.Location")));
      this.chkViewAsEArmor.Name = "chkViewAsEArmor";
      this.chkViewAsEArmor.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkViewAsEArmor.RightToLeft")));
      this.chkViewAsEArmor.Size = ((System.Drawing.Size)(resources.GetObject("chkViewAsEArmor.Size")));
      this.chkViewAsEArmor.TabIndex = ((int)(resources.GetObject("chkViewAsEArmor.TabIndex")));
      this.chkViewAsEArmor.Text = resources.GetString("chkViewAsEArmor.Text");
      this.chkViewAsEArmor.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkViewAsEArmor.TextAlign")));
      this.ttToolTip.SetToolTip(this.chkViewAsEArmor, resources.GetString("chkViewAsEArmor.ToolTip"));
      this.chkViewAsEArmor.Visible = ((bool)(resources.GetObject("chkViewAsEArmor.Visible")));
      this.chkViewAsEArmor.Click += new System.EventHandler(this.chkView_Click);
      this.chkViewAsEArmor.CheckedChanged += new System.EventHandler(this.chkViewAsEArmor_CheckedChanged);
      // 
      // grpEquipmentInfo
      // 
      this.grpEquipmentInfo.AccessibleDescription = resources.GetString("grpEquipmentInfo.AccessibleDescription");
      this.grpEquipmentInfo.AccessibleName = resources.GetString("grpEquipmentInfo.AccessibleName");
      this.grpEquipmentInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("grpEquipmentInfo.Anchor")));
      this.grpEquipmentInfo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpEquipmentInfo.BackgroundImage")));
      this.grpEquipmentInfo.Controls.Add(this.lblRaces);
      this.grpEquipmentInfo.Controls.Add(this.txtRaces);
      this.grpEquipmentInfo.Controls.Add(this.lblSlots);
      this.grpEquipmentInfo.Controls.Add(this.txtSlots);
      this.grpEquipmentInfo.Controls.Add(this.lblJobs);
      this.grpEquipmentInfo.Controls.Add(this.txtJobs);
      this.grpEquipmentInfo.Controls.Add(this.lblLevel);
      this.grpEquipmentInfo.Controls.Add(this.txtLevel);
      this.grpEquipmentInfo.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("grpEquipmentInfo.Dock")));
      this.grpEquipmentInfo.Enabled = ((bool)(resources.GetObject("grpEquipmentInfo.Enabled")));
      this.grpEquipmentInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.grpEquipmentInfo.Font = ((System.Drawing.Font)(resources.GetObject("grpEquipmentInfo.Font")));
      this.grpEquipmentInfo.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("grpEquipmentInfo.ImeMode")));
      this.grpEquipmentInfo.Location = ((System.Drawing.Point)(resources.GetObject("grpEquipmentInfo.Location")));
      this.grpEquipmentInfo.Name = "grpEquipmentInfo";
      this.grpEquipmentInfo.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("grpEquipmentInfo.RightToLeft")));
      this.grpEquipmentInfo.Size = ((System.Drawing.Size)(resources.GetObject("grpEquipmentInfo.Size")));
      this.grpEquipmentInfo.TabIndex = ((int)(resources.GetObject("grpEquipmentInfo.TabIndex")));
      this.grpEquipmentInfo.TabStop = false;
      this.grpEquipmentInfo.Text = resources.GetString("grpEquipmentInfo.Text");
      this.ttToolTip.SetToolTip(this.grpEquipmentInfo, resources.GetString("grpEquipmentInfo.ToolTip"));
      this.grpEquipmentInfo.Visible = ((bool)(resources.GetObject("grpEquipmentInfo.Visible")));
      // 
      // lblRaces
      // 
      this.lblRaces.AccessibleDescription = resources.GetString("lblRaces.AccessibleDescription");
      this.lblRaces.AccessibleName = resources.GetString("lblRaces.AccessibleName");
      this.lblRaces.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblRaces.Anchor")));
      this.lblRaces.AutoSize = ((bool)(resources.GetObject("lblRaces.AutoSize")));
      this.lblRaces.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblRaces.Dock")));
      this.lblRaces.Enabled = ((bool)(resources.GetObject("lblRaces.Enabled")));
      this.lblRaces.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblRaces.Font = ((System.Drawing.Font)(resources.GetObject("lblRaces.Font")));
      this.lblRaces.Image = ((System.Drawing.Image)(resources.GetObject("lblRaces.Image")));
      this.lblRaces.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblRaces.ImageAlign")));
      this.lblRaces.ImageIndex = ((int)(resources.GetObject("lblRaces.ImageIndex")));
      this.lblRaces.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblRaces.ImeMode")));
      this.lblRaces.Location = ((System.Drawing.Point)(resources.GetObject("lblRaces.Location")));
      this.lblRaces.Name = "lblRaces";
      this.lblRaces.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblRaces.RightToLeft")));
      this.lblRaces.Size = ((System.Drawing.Size)(resources.GetObject("lblRaces.Size")));
      this.lblRaces.TabIndex = ((int)(resources.GetObject("lblRaces.TabIndex")));
      this.lblRaces.Text = resources.GetString("lblRaces.Text");
      this.lblRaces.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblRaces.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblRaces, resources.GetString("lblRaces.ToolTip"));
      this.lblRaces.Visible = ((bool)(resources.GetObject("lblRaces.Visible")));
      // 
      // txtRaces
      // 
      this.txtRaces.AccessibleDescription = resources.GetString("txtRaces.AccessibleDescription");
      this.txtRaces.AccessibleName = resources.GetString("txtRaces.AccessibleName");
      this.txtRaces.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtRaces.Anchor")));
      this.txtRaces.AutoSize = ((bool)(resources.GetObject("txtRaces.AutoSize")));
      this.txtRaces.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtRaces.BackgroundImage")));
      this.txtRaces.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtRaces.Dock")));
      this.txtRaces.Enabled = ((bool)(resources.GetObject("txtRaces.Enabled")));
      this.txtRaces.Font = ((System.Drawing.Font)(resources.GetObject("txtRaces.Font")));
      this.txtRaces.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtRaces.ImeMode")));
      this.txtRaces.Location = ((System.Drawing.Point)(resources.GetObject("txtRaces.Location")));
      this.txtRaces.MaxLength = ((int)(resources.GetObject("txtRaces.MaxLength")));
      this.txtRaces.Multiline = ((bool)(resources.GetObject("txtRaces.Multiline")));
      this.txtRaces.Name = "txtRaces";
      this.txtRaces.PasswordChar = ((char)(resources.GetObject("txtRaces.PasswordChar")));
      this.txtRaces.ReadOnly = true;
      this.txtRaces.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtRaces.RightToLeft")));
      this.txtRaces.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtRaces.ScrollBars")));
      this.txtRaces.Size = ((System.Drawing.Size)(resources.GetObject("txtRaces.Size")));
      this.txtRaces.TabIndex = ((int)(resources.GetObject("txtRaces.TabIndex")));
      this.txtRaces.Text = resources.GetString("txtRaces.Text");
      this.txtRaces.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtRaces.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtRaces, resources.GetString("txtRaces.ToolTip"));
      this.txtRaces.Visible = ((bool)(resources.GetObject("txtRaces.Visible")));
      this.txtRaces.WordWrap = ((bool)(resources.GetObject("txtRaces.WordWrap")));
      // 
      // lblSlots
      // 
      this.lblSlots.AccessibleDescription = resources.GetString("lblSlots.AccessibleDescription");
      this.lblSlots.AccessibleName = resources.GetString("lblSlots.AccessibleName");
      this.lblSlots.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblSlots.Anchor")));
      this.lblSlots.AutoSize = ((bool)(resources.GetObject("lblSlots.AutoSize")));
      this.lblSlots.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblSlots.Dock")));
      this.lblSlots.Enabled = ((bool)(resources.GetObject("lblSlots.Enabled")));
      this.lblSlots.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblSlots.Font = ((System.Drawing.Font)(resources.GetObject("lblSlots.Font")));
      this.lblSlots.Image = ((System.Drawing.Image)(resources.GetObject("lblSlots.Image")));
      this.lblSlots.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblSlots.ImageAlign")));
      this.lblSlots.ImageIndex = ((int)(resources.GetObject("lblSlots.ImageIndex")));
      this.lblSlots.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblSlots.ImeMode")));
      this.lblSlots.Location = ((System.Drawing.Point)(resources.GetObject("lblSlots.Location")));
      this.lblSlots.Name = "lblSlots";
      this.lblSlots.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblSlots.RightToLeft")));
      this.lblSlots.Size = ((System.Drawing.Size)(resources.GetObject("lblSlots.Size")));
      this.lblSlots.TabIndex = ((int)(resources.GetObject("lblSlots.TabIndex")));
      this.lblSlots.Text = resources.GetString("lblSlots.Text");
      this.lblSlots.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblSlots.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblSlots, resources.GetString("lblSlots.ToolTip"));
      this.lblSlots.Visible = ((bool)(resources.GetObject("lblSlots.Visible")));
      // 
      // txtSlots
      // 
      this.txtSlots.AccessibleDescription = resources.GetString("txtSlots.AccessibleDescription");
      this.txtSlots.AccessibleName = resources.GetString("txtSlots.AccessibleName");
      this.txtSlots.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtSlots.Anchor")));
      this.txtSlots.AutoSize = ((bool)(resources.GetObject("txtSlots.AutoSize")));
      this.txtSlots.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtSlots.BackgroundImage")));
      this.txtSlots.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtSlots.Dock")));
      this.txtSlots.Enabled = ((bool)(resources.GetObject("txtSlots.Enabled")));
      this.txtSlots.Font = ((System.Drawing.Font)(resources.GetObject("txtSlots.Font")));
      this.txtSlots.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtSlots.ImeMode")));
      this.txtSlots.Location = ((System.Drawing.Point)(resources.GetObject("txtSlots.Location")));
      this.txtSlots.MaxLength = ((int)(resources.GetObject("txtSlots.MaxLength")));
      this.txtSlots.Multiline = ((bool)(resources.GetObject("txtSlots.Multiline")));
      this.txtSlots.Name = "txtSlots";
      this.txtSlots.PasswordChar = ((char)(resources.GetObject("txtSlots.PasswordChar")));
      this.txtSlots.ReadOnly = true;
      this.txtSlots.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtSlots.RightToLeft")));
      this.txtSlots.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtSlots.ScrollBars")));
      this.txtSlots.Size = ((System.Drawing.Size)(resources.GetObject("txtSlots.Size")));
      this.txtSlots.TabIndex = ((int)(resources.GetObject("txtSlots.TabIndex")));
      this.txtSlots.Text = resources.GetString("txtSlots.Text");
      this.txtSlots.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtSlots.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtSlots, resources.GetString("txtSlots.ToolTip"));
      this.txtSlots.Visible = ((bool)(resources.GetObject("txtSlots.Visible")));
      this.txtSlots.WordWrap = ((bool)(resources.GetObject("txtSlots.WordWrap")));
      // 
      // lblJobs
      // 
      this.lblJobs.AccessibleDescription = resources.GetString("lblJobs.AccessibleDescription");
      this.lblJobs.AccessibleName = resources.GetString("lblJobs.AccessibleName");
      this.lblJobs.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblJobs.Anchor")));
      this.lblJobs.AutoSize = ((bool)(resources.GetObject("lblJobs.AutoSize")));
      this.lblJobs.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblJobs.Dock")));
      this.lblJobs.Enabled = ((bool)(resources.GetObject("lblJobs.Enabled")));
      this.lblJobs.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblJobs.Font = ((System.Drawing.Font)(resources.GetObject("lblJobs.Font")));
      this.lblJobs.Image = ((System.Drawing.Image)(resources.GetObject("lblJobs.Image")));
      this.lblJobs.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblJobs.ImageAlign")));
      this.lblJobs.ImageIndex = ((int)(resources.GetObject("lblJobs.ImageIndex")));
      this.lblJobs.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblJobs.ImeMode")));
      this.lblJobs.Location = ((System.Drawing.Point)(resources.GetObject("lblJobs.Location")));
      this.lblJobs.Name = "lblJobs";
      this.lblJobs.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblJobs.RightToLeft")));
      this.lblJobs.Size = ((System.Drawing.Size)(resources.GetObject("lblJobs.Size")));
      this.lblJobs.TabIndex = ((int)(resources.GetObject("lblJobs.TabIndex")));
      this.lblJobs.Text = resources.GetString("lblJobs.Text");
      this.lblJobs.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblJobs.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblJobs, resources.GetString("lblJobs.ToolTip"));
      this.lblJobs.Visible = ((bool)(resources.GetObject("lblJobs.Visible")));
      // 
      // txtJobs
      // 
      this.txtJobs.AccessibleDescription = resources.GetString("txtJobs.AccessibleDescription");
      this.txtJobs.AccessibleName = resources.GetString("txtJobs.AccessibleName");
      this.txtJobs.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtJobs.Anchor")));
      this.txtJobs.AutoSize = ((bool)(resources.GetObject("txtJobs.AutoSize")));
      this.txtJobs.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtJobs.BackgroundImage")));
      this.txtJobs.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtJobs.Dock")));
      this.txtJobs.Enabled = ((bool)(resources.GetObject("txtJobs.Enabled")));
      this.txtJobs.Font = ((System.Drawing.Font)(resources.GetObject("txtJobs.Font")));
      this.txtJobs.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtJobs.ImeMode")));
      this.txtJobs.Location = ((System.Drawing.Point)(resources.GetObject("txtJobs.Location")));
      this.txtJobs.MaxLength = ((int)(resources.GetObject("txtJobs.MaxLength")));
      this.txtJobs.Multiline = ((bool)(resources.GetObject("txtJobs.Multiline")));
      this.txtJobs.Name = "txtJobs";
      this.txtJobs.PasswordChar = ((char)(resources.GetObject("txtJobs.PasswordChar")));
      this.txtJobs.ReadOnly = true;
      this.txtJobs.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtJobs.RightToLeft")));
      this.txtJobs.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtJobs.ScrollBars")));
      this.txtJobs.Size = ((System.Drawing.Size)(resources.GetObject("txtJobs.Size")));
      this.txtJobs.TabIndex = ((int)(resources.GetObject("txtJobs.TabIndex")));
      this.txtJobs.Text = resources.GetString("txtJobs.Text");
      this.txtJobs.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtJobs.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtJobs, resources.GetString("txtJobs.ToolTip"));
      this.txtJobs.Visible = ((bool)(resources.GetObject("txtJobs.Visible")));
      this.txtJobs.WordWrap = ((bool)(resources.GetObject("txtJobs.WordWrap")));
      // 
      // lblLevel
      // 
      this.lblLevel.AccessibleDescription = resources.GetString("lblLevel.AccessibleDescription");
      this.lblLevel.AccessibleName = resources.GetString("lblLevel.AccessibleName");
      this.lblLevel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblLevel.Anchor")));
      this.lblLevel.AutoSize = ((bool)(resources.GetObject("lblLevel.AutoSize")));
      this.lblLevel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblLevel.Dock")));
      this.lblLevel.Enabled = ((bool)(resources.GetObject("lblLevel.Enabled")));
      this.lblLevel.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblLevel.Font = ((System.Drawing.Font)(resources.GetObject("lblLevel.Font")));
      this.lblLevel.Image = ((System.Drawing.Image)(resources.GetObject("lblLevel.Image")));
      this.lblLevel.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblLevel.ImageAlign")));
      this.lblLevel.ImageIndex = ((int)(resources.GetObject("lblLevel.ImageIndex")));
      this.lblLevel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblLevel.ImeMode")));
      this.lblLevel.Location = ((System.Drawing.Point)(resources.GetObject("lblLevel.Location")));
      this.lblLevel.Name = "lblLevel";
      this.lblLevel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblLevel.RightToLeft")));
      this.lblLevel.Size = ((System.Drawing.Size)(resources.GetObject("lblLevel.Size")));
      this.lblLevel.TabIndex = ((int)(resources.GetObject("lblLevel.TabIndex")));
      this.lblLevel.Text = resources.GetString("lblLevel.Text");
      this.lblLevel.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblLevel.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblLevel, resources.GetString("lblLevel.ToolTip"));
      this.lblLevel.Visible = ((bool)(resources.GetObject("lblLevel.Visible")));
      // 
      // txtLevel
      // 
      this.txtLevel.AccessibleDescription = resources.GetString("txtLevel.AccessibleDescription");
      this.txtLevel.AccessibleName = resources.GetString("txtLevel.AccessibleName");
      this.txtLevel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtLevel.Anchor")));
      this.txtLevel.AutoSize = ((bool)(resources.GetObject("txtLevel.AutoSize")));
      this.txtLevel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtLevel.BackgroundImage")));
      this.txtLevel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtLevel.Dock")));
      this.txtLevel.Enabled = ((bool)(resources.GetObject("txtLevel.Enabled")));
      this.txtLevel.Font = ((System.Drawing.Font)(resources.GetObject("txtLevel.Font")));
      this.txtLevel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtLevel.ImeMode")));
      this.txtLevel.Location = ((System.Drawing.Point)(resources.GetObject("txtLevel.Location")));
      this.txtLevel.MaxLength = ((int)(resources.GetObject("txtLevel.MaxLength")));
      this.txtLevel.Multiline = ((bool)(resources.GetObject("txtLevel.Multiline")));
      this.txtLevel.Name = "txtLevel";
      this.txtLevel.PasswordChar = ((char)(resources.GetObject("txtLevel.PasswordChar")));
      this.txtLevel.ReadOnly = true;
      this.txtLevel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtLevel.RightToLeft")));
      this.txtLevel.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtLevel.ScrollBars")));
      this.txtLevel.Size = ((System.Drawing.Size)(resources.GetObject("txtLevel.Size")));
      this.txtLevel.TabIndex = ((int)(resources.GetObject("txtLevel.TabIndex")));
      this.txtLevel.Text = resources.GetString("txtLevel.Text");
      this.txtLevel.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtLevel.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtLevel, resources.GetString("txtLevel.ToolTip"));
      this.txtLevel.Visible = ((bool)(resources.GetObject("txtLevel.Visible")));
      this.txtLevel.WordWrap = ((bool)(resources.GetObject("txtLevel.WordWrap")));
      // 
      // grpCommonInfo
      // 
      this.grpCommonInfo.AccessibleDescription = resources.GetString("grpCommonInfo.AccessibleDescription");
      this.grpCommonInfo.AccessibleName = resources.GetString("grpCommonInfo.AccessibleName");
      this.grpCommonInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("grpCommonInfo.Anchor")));
      this.grpCommonInfo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpCommonInfo.BackgroundImage")));
      this.grpCommonInfo.Controls.Add(this.lblValidTargets);
      this.grpCommonInfo.Controls.Add(this.txtValidTargets);
      this.grpCommonInfo.Controls.Add(this.lblDescription);
      this.grpCommonInfo.Controls.Add(this.lblJName);
      this.grpCommonInfo.Controls.Add(this.lblEName);
      this.grpCommonInfo.Controls.Add(this.txtJName);
      this.grpCommonInfo.Controls.Add(this.txtEName);
      this.grpCommonInfo.Controls.Add(this.txtDescription);
      this.grpCommonInfo.Controls.Add(this.lblStackSize);
      this.grpCommonInfo.Controls.Add(this.lblFlags);
      this.grpCommonInfo.Controls.Add(this.lblType);
      this.grpCommonInfo.Controls.Add(this.lblID);
      this.grpCommonInfo.Controls.Add(this.txtStackSize);
      this.grpCommonInfo.Controls.Add(this.txtFlags);
      this.grpCommonInfo.Controls.Add(this.txtType);
      this.grpCommonInfo.Controls.Add(this.txtID);
      this.grpCommonInfo.Controls.Add(this.picIcon);
      this.grpCommonInfo.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("grpCommonInfo.Dock")));
      this.grpCommonInfo.Enabled = ((bool)(resources.GetObject("grpCommonInfo.Enabled")));
      this.grpCommonInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.grpCommonInfo.Font = ((System.Drawing.Font)(resources.GetObject("grpCommonInfo.Font")));
      this.grpCommonInfo.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("grpCommonInfo.ImeMode")));
      this.grpCommonInfo.Location = ((System.Drawing.Point)(resources.GetObject("grpCommonInfo.Location")));
      this.grpCommonInfo.Name = "grpCommonInfo";
      this.grpCommonInfo.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("grpCommonInfo.RightToLeft")));
      this.grpCommonInfo.Size = ((System.Drawing.Size)(resources.GetObject("grpCommonInfo.Size")));
      this.grpCommonInfo.TabIndex = ((int)(resources.GetObject("grpCommonInfo.TabIndex")));
      this.grpCommonInfo.TabStop = false;
      this.grpCommonInfo.Text = resources.GetString("grpCommonInfo.Text");
      this.ttToolTip.SetToolTip(this.grpCommonInfo, resources.GetString("grpCommonInfo.ToolTip"));
      this.grpCommonInfo.Visible = ((bool)(resources.GetObject("grpCommonInfo.Visible")));
      // 
      // lblValidTargets
      // 
      this.lblValidTargets.AccessibleDescription = resources.GetString("lblValidTargets.AccessibleDescription");
      this.lblValidTargets.AccessibleName = resources.GetString("lblValidTargets.AccessibleName");
      this.lblValidTargets.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblValidTargets.Anchor")));
      this.lblValidTargets.AutoSize = ((bool)(resources.GetObject("lblValidTargets.AutoSize")));
      this.lblValidTargets.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblValidTargets.Dock")));
      this.lblValidTargets.Enabled = ((bool)(resources.GetObject("lblValidTargets.Enabled")));
      this.lblValidTargets.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblValidTargets.Font = ((System.Drawing.Font)(resources.GetObject("lblValidTargets.Font")));
      this.lblValidTargets.Image = ((System.Drawing.Image)(resources.GetObject("lblValidTargets.Image")));
      this.lblValidTargets.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblValidTargets.ImageAlign")));
      this.lblValidTargets.ImageIndex = ((int)(resources.GetObject("lblValidTargets.ImageIndex")));
      this.lblValidTargets.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblValidTargets.ImeMode")));
      this.lblValidTargets.Location = ((System.Drawing.Point)(resources.GetObject("lblValidTargets.Location")));
      this.lblValidTargets.Name = "lblValidTargets";
      this.lblValidTargets.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblValidTargets.RightToLeft")));
      this.lblValidTargets.Size = ((System.Drawing.Size)(resources.GetObject("lblValidTargets.Size")));
      this.lblValidTargets.TabIndex = ((int)(resources.GetObject("lblValidTargets.TabIndex")));
      this.lblValidTargets.Text = resources.GetString("lblValidTargets.Text");
      this.lblValidTargets.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblValidTargets.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblValidTargets, resources.GetString("lblValidTargets.ToolTip"));
      this.lblValidTargets.Visible = ((bool)(resources.GetObject("lblValidTargets.Visible")));
      // 
      // txtValidTargets
      // 
      this.txtValidTargets.AccessibleDescription = resources.GetString("txtValidTargets.AccessibleDescription");
      this.txtValidTargets.AccessibleName = resources.GetString("txtValidTargets.AccessibleName");
      this.txtValidTargets.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtValidTargets.Anchor")));
      this.txtValidTargets.AutoSize = ((bool)(resources.GetObject("txtValidTargets.AutoSize")));
      this.txtValidTargets.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtValidTargets.BackgroundImage")));
      this.txtValidTargets.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtValidTargets.Dock")));
      this.txtValidTargets.Enabled = ((bool)(resources.GetObject("txtValidTargets.Enabled")));
      this.txtValidTargets.Font = ((System.Drawing.Font)(resources.GetObject("txtValidTargets.Font")));
      this.txtValidTargets.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtValidTargets.ImeMode")));
      this.txtValidTargets.Location = ((System.Drawing.Point)(resources.GetObject("txtValidTargets.Location")));
      this.txtValidTargets.MaxLength = ((int)(resources.GetObject("txtValidTargets.MaxLength")));
      this.txtValidTargets.Multiline = ((bool)(resources.GetObject("txtValidTargets.Multiline")));
      this.txtValidTargets.Name = "txtValidTargets";
      this.txtValidTargets.PasswordChar = ((char)(resources.GetObject("txtValidTargets.PasswordChar")));
      this.txtValidTargets.ReadOnly = true;
      this.txtValidTargets.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtValidTargets.RightToLeft")));
      this.txtValidTargets.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtValidTargets.ScrollBars")));
      this.txtValidTargets.Size = ((System.Drawing.Size)(resources.GetObject("txtValidTargets.Size")));
      this.txtValidTargets.TabIndex = ((int)(resources.GetObject("txtValidTargets.TabIndex")));
      this.txtValidTargets.Text = resources.GetString("txtValidTargets.Text");
      this.txtValidTargets.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtValidTargets.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtValidTargets, resources.GetString("txtValidTargets.ToolTip"));
      this.txtValidTargets.Visible = ((bool)(resources.GetObject("txtValidTargets.Visible")));
      this.txtValidTargets.WordWrap = ((bool)(resources.GetObject("txtValidTargets.WordWrap")));
      // 
      // lblDescription
      // 
      this.lblDescription.AccessibleDescription = resources.GetString("lblDescription.AccessibleDescription");
      this.lblDescription.AccessibleName = resources.GetString("lblDescription.AccessibleName");
      this.lblDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblDescription.Anchor")));
      this.lblDescription.AutoSize = ((bool)(resources.GetObject("lblDescription.AutoSize")));
      this.lblDescription.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblDescription.Dock")));
      this.lblDescription.Enabled = ((bool)(resources.GetObject("lblDescription.Enabled")));
      this.lblDescription.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblDescription.Font = ((System.Drawing.Font)(resources.GetObject("lblDescription.Font")));
      this.lblDescription.Image = ((System.Drawing.Image)(resources.GetObject("lblDescription.Image")));
      this.lblDescription.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblDescription.ImageAlign")));
      this.lblDescription.ImageIndex = ((int)(resources.GetObject("lblDescription.ImageIndex")));
      this.lblDescription.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblDescription.ImeMode")));
      this.lblDescription.Location = ((System.Drawing.Point)(resources.GetObject("lblDescription.Location")));
      this.lblDescription.Name = "lblDescription";
      this.lblDescription.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblDescription.RightToLeft")));
      this.lblDescription.Size = ((System.Drawing.Size)(resources.GetObject("lblDescription.Size")));
      this.lblDescription.TabIndex = ((int)(resources.GetObject("lblDescription.TabIndex")));
      this.lblDescription.Text = resources.GetString("lblDescription.Text");
      this.lblDescription.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblDescription.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblDescription, resources.GetString("lblDescription.ToolTip"));
      this.lblDescription.Visible = ((bool)(resources.GetObject("lblDescription.Visible")));
      // 
      // lblJName
      // 
      this.lblJName.AccessibleDescription = resources.GetString("lblJName.AccessibleDescription");
      this.lblJName.AccessibleName = resources.GetString("lblJName.AccessibleName");
      this.lblJName.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblJName.Anchor")));
      this.lblJName.AutoSize = ((bool)(resources.GetObject("lblJName.AutoSize")));
      this.lblJName.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblJName.Dock")));
      this.lblJName.Enabled = ((bool)(resources.GetObject("lblJName.Enabled")));
      this.lblJName.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblJName.Font = ((System.Drawing.Font)(resources.GetObject("lblJName.Font")));
      this.lblJName.Image = ((System.Drawing.Image)(resources.GetObject("lblJName.Image")));
      this.lblJName.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblJName.ImageAlign")));
      this.lblJName.ImageIndex = ((int)(resources.GetObject("lblJName.ImageIndex")));
      this.lblJName.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblJName.ImeMode")));
      this.lblJName.Location = ((System.Drawing.Point)(resources.GetObject("lblJName.Location")));
      this.lblJName.Name = "lblJName";
      this.lblJName.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblJName.RightToLeft")));
      this.lblJName.Size = ((System.Drawing.Size)(resources.GetObject("lblJName.Size")));
      this.lblJName.TabIndex = ((int)(resources.GetObject("lblJName.TabIndex")));
      this.lblJName.Text = resources.GetString("lblJName.Text");
      this.lblJName.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblJName.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblJName, resources.GetString("lblJName.ToolTip"));
      this.lblJName.Visible = ((bool)(resources.GetObject("lblJName.Visible")));
      // 
      // lblEName
      // 
      this.lblEName.AccessibleDescription = resources.GetString("lblEName.AccessibleDescription");
      this.lblEName.AccessibleName = resources.GetString("lblEName.AccessibleName");
      this.lblEName.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblEName.Anchor")));
      this.lblEName.AutoSize = ((bool)(resources.GetObject("lblEName.AutoSize")));
      this.lblEName.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblEName.Dock")));
      this.lblEName.Enabled = ((bool)(resources.GetObject("lblEName.Enabled")));
      this.lblEName.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblEName.Font = ((System.Drawing.Font)(resources.GetObject("lblEName.Font")));
      this.lblEName.Image = ((System.Drawing.Image)(resources.GetObject("lblEName.Image")));
      this.lblEName.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblEName.ImageAlign")));
      this.lblEName.ImageIndex = ((int)(resources.GetObject("lblEName.ImageIndex")));
      this.lblEName.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblEName.ImeMode")));
      this.lblEName.Location = ((System.Drawing.Point)(resources.GetObject("lblEName.Location")));
      this.lblEName.Name = "lblEName";
      this.lblEName.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblEName.RightToLeft")));
      this.lblEName.Size = ((System.Drawing.Size)(resources.GetObject("lblEName.Size")));
      this.lblEName.TabIndex = ((int)(resources.GetObject("lblEName.TabIndex")));
      this.lblEName.Text = resources.GetString("lblEName.Text");
      this.lblEName.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblEName.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblEName, resources.GetString("lblEName.ToolTip"));
      this.lblEName.Visible = ((bool)(resources.GetObject("lblEName.Visible")));
      // 
      // txtJName
      // 
      this.txtJName.AccessibleDescription = resources.GetString("txtJName.AccessibleDescription");
      this.txtJName.AccessibleName = resources.GetString("txtJName.AccessibleName");
      this.txtJName.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtJName.Anchor")));
      this.txtJName.AutoSize = ((bool)(resources.GetObject("txtJName.AutoSize")));
      this.txtJName.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtJName.BackgroundImage")));
      this.txtJName.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtJName.Dock")));
      this.txtJName.Enabled = ((bool)(resources.GetObject("txtJName.Enabled")));
      this.txtJName.Font = ((System.Drawing.Font)(resources.GetObject("txtJName.Font")));
      this.txtJName.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtJName.ImeMode")));
      this.txtJName.Location = ((System.Drawing.Point)(resources.GetObject("txtJName.Location")));
      this.txtJName.MaxLength = ((int)(resources.GetObject("txtJName.MaxLength")));
      this.txtJName.Multiline = ((bool)(resources.GetObject("txtJName.Multiline")));
      this.txtJName.Name = "txtJName";
      this.txtJName.PasswordChar = ((char)(resources.GetObject("txtJName.PasswordChar")));
      this.txtJName.ReadOnly = true;
      this.txtJName.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtJName.RightToLeft")));
      this.txtJName.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtJName.ScrollBars")));
      this.txtJName.Size = ((System.Drawing.Size)(resources.GetObject("txtJName.Size")));
      this.txtJName.TabIndex = ((int)(resources.GetObject("txtJName.TabIndex")));
      this.txtJName.Text = resources.GetString("txtJName.Text");
      this.txtJName.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtJName.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtJName, resources.GetString("txtJName.ToolTip"));
      this.txtJName.Visible = ((bool)(resources.GetObject("txtJName.Visible")));
      this.txtJName.WordWrap = ((bool)(resources.GetObject("txtJName.WordWrap")));
      // 
      // txtEName
      // 
      this.txtEName.AccessibleDescription = resources.GetString("txtEName.AccessibleDescription");
      this.txtEName.AccessibleName = resources.GetString("txtEName.AccessibleName");
      this.txtEName.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtEName.Anchor")));
      this.txtEName.AutoSize = ((bool)(resources.GetObject("txtEName.AutoSize")));
      this.txtEName.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtEName.BackgroundImage")));
      this.txtEName.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtEName.Dock")));
      this.txtEName.Enabled = ((bool)(resources.GetObject("txtEName.Enabled")));
      this.txtEName.Font = ((System.Drawing.Font)(resources.GetObject("txtEName.Font")));
      this.txtEName.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtEName.ImeMode")));
      this.txtEName.Location = ((System.Drawing.Point)(resources.GetObject("txtEName.Location")));
      this.txtEName.MaxLength = ((int)(resources.GetObject("txtEName.MaxLength")));
      this.txtEName.Multiline = ((bool)(resources.GetObject("txtEName.Multiline")));
      this.txtEName.Name = "txtEName";
      this.txtEName.PasswordChar = ((char)(resources.GetObject("txtEName.PasswordChar")));
      this.txtEName.ReadOnly = true;
      this.txtEName.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtEName.RightToLeft")));
      this.txtEName.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtEName.ScrollBars")));
      this.txtEName.Size = ((System.Drawing.Size)(resources.GetObject("txtEName.Size")));
      this.txtEName.TabIndex = ((int)(resources.GetObject("txtEName.TabIndex")));
      this.txtEName.Text = resources.GetString("txtEName.Text");
      this.txtEName.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtEName.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtEName, resources.GetString("txtEName.ToolTip"));
      this.txtEName.Visible = ((bool)(resources.GetObject("txtEName.Visible")));
      this.txtEName.WordWrap = ((bool)(resources.GetObject("txtEName.WordWrap")));
      // 
      // txtDescription
      // 
      this.txtDescription.AccessibleDescription = resources.GetString("txtDescription.AccessibleDescription");
      this.txtDescription.AccessibleName = resources.GetString("txtDescription.AccessibleName");
      this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtDescription.Anchor")));
      this.txtDescription.AutoSize = ((bool)(resources.GetObject("txtDescription.AutoSize")));
      this.txtDescription.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtDescription.BackgroundImage")));
      this.txtDescription.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtDescription.Dock")));
      this.txtDescription.Enabled = ((bool)(resources.GetObject("txtDescription.Enabled")));
      this.txtDescription.Font = ((System.Drawing.Font)(resources.GetObject("txtDescription.Font")));
      this.txtDescription.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtDescription.ImeMode")));
      this.txtDescription.Location = ((System.Drawing.Point)(resources.GetObject("txtDescription.Location")));
      this.txtDescription.MaxLength = ((int)(resources.GetObject("txtDescription.MaxLength")));
      this.txtDescription.Multiline = ((bool)(resources.GetObject("txtDescription.Multiline")));
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.PasswordChar = ((char)(resources.GetObject("txtDescription.PasswordChar")));
      this.txtDescription.ReadOnly = true;
      this.txtDescription.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtDescription.RightToLeft")));
      this.txtDescription.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtDescription.ScrollBars")));
      this.txtDescription.Size = ((System.Drawing.Size)(resources.GetObject("txtDescription.Size")));
      this.txtDescription.TabIndex = ((int)(resources.GetObject("txtDescription.TabIndex")));
      this.txtDescription.Text = resources.GetString("txtDescription.Text");
      this.txtDescription.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtDescription.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtDescription, resources.GetString("txtDescription.ToolTip"));
      this.txtDescription.Visible = ((bool)(resources.GetObject("txtDescription.Visible")));
      this.txtDescription.WordWrap = ((bool)(resources.GetObject("txtDescription.WordWrap")));
      // 
      // lblStackSize
      // 
      this.lblStackSize.AccessibleDescription = resources.GetString("lblStackSize.AccessibleDescription");
      this.lblStackSize.AccessibleName = resources.GetString("lblStackSize.AccessibleName");
      this.lblStackSize.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblStackSize.Anchor")));
      this.lblStackSize.AutoSize = ((bool)(resources.GetObject("lblStackSize.AutoSize")));
      this.lblStackSize.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblStackSize.Dock")));
      this.lblStackSize.Enabled = ((bool)(resources.GetObject("lblStackSize.Enabled")));
      this.lblStackSize.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblStackSize.Font = ((System.Drawing.Font)(resources.GetObject("lblStackSize.Font")));
      this.lblStackSize.Image = ((System.Drawing.Image)(resources.GetObject("lblStackSize.Image")));
      this.lblStackSize.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblStackSize.ImageAlign")));
      this.lblStackSize.ImageIndex = ((int)(resources.GetObject("lblStackSize.ImageIndex")));
      this.lblStackSize.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblStackSize.ImeMode")));
      this.lblStackSize.Location = ((System.Drawing.Point)(resources.GetObject("lblStackSize.Location")));
      this.lblStackSize.Name = "lblStackSize";
      this.lblStackSize.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblStackSize.RightToLeft")));
      this.lblStackSize.Size = ((System.Drawing.Size)(resources.GetObject("lblStackSize.Size")));
      this.lblStackSize.TabIndex = ((int)(resources.GetObject("lblStackSize.TabIndex")));
      this.lblStackSize.Text = resources.GetString("lblStackSize.Text");
      this.lblStackSize.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblStackSize.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblStackSize, resources.GetString("lblStackSize.ToolTip"));
      this.lblStackSize.Visible = ((bool)(resources.GetObject("lblStackSize.Visible")));
      // 
      // lblFlags
      // 
      this.lblFlags.AccessibleDescription = resources.GetString("lblFlags.AccessibleDescription");
      this.lblFlags.AccessibleName = resources.GetString("lblFlags.AccessibleName");
      this.lblFlags.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblFlags.Anchor")));
      this.lblFlags.AutoSize = ((bool)(resources.GetObject("lblFlags.AutoSize")));
      this.lblFlags.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblFlags.Dock")));
      this.lblFlags.Enabled = ((bool)(resources.GetObject("lblFlags.Enabled")));
      this.lblFlags.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblFlags.Font = ((System.Drawing.Font)(resources.GetObject("lblFlags.Font")));
      this.lblFlags.Image = ((System.Drawing.Image)(resources.GetObject("lblFlags.Image")));
      this.lblFlags.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblFlags.ImageAlign")));
      this.lblFlags.ImageIndex = ((int)(resources.GetObject("lblFlags.ImageIndex")));
      this.lblFlags.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblFlags.ImeMode")));
      this.lblFlags.Location = ((System.Drawing.Point)(resources.GetObject("lblFlags.Location")));
      this.lblFlags.Name = "lblFlags";
      this.lblFlags.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblFlags.RightToLeft")));
      this.lblFlags.Size = ((System.Drawing.Size)(resources.GetObject("lblFlags.Size")));
      this.lblFlags.TabIndex = ((int)(resources.GetObject("lblFlags.TabIndex")));
      this.lblFlags.Text = resources.GetString("lblFlags.Text");
      this.lblFlags.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblFlags.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblFlags, resources.GetString("lblFlags.ToolTip"));
      this.lblFlags.Visible = ((bool)(resources.GetObject("lblFlags.Visible")));
      // 
      // lblType
      // 
      this.lblType.AccessibleDescription = resources.GetString("lblType.AccessibleDescription");
      this.lblType.AccessibleName = resources.GetString("lblType.AccessibleName");
      this.lblType.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblType.Anchor")));
      this.lblType.AutoSize = ((bool)(resources.GetObject("lblType.AutoSize")));
      this.lblType.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblType.Dock")));
      this.lblType.Enabled = ((bool)(resources.GetObject("lblType.Enabled")));
      this.lblType.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblType.Font = ((System.Drawing.Font)(resources.GetObject("lblType.Font")));
      this.lblType.Image = ((System.Drawing.Image)(resources.GetObject("lblType.Image")));
      this.lblType.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblType.ImageAlign")));
      this.lblType.ImageIndex = ((int)(resources.GetObject("lblType.ImageIndex")));
      this.lblType.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblType.ImeMode")));
      this.lblType.Location = ((System.Drawing.Point)(resources.GetObject("lblType.Location")));
      this.lblType.Name = "lblType";
      this.lblType.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblType.RightToLeft")));
      this.lblType.Size = ((System.Drawing.Size)(resources.GetObject("lblType.Size")));
      this.lblType.TabIndex = ((int)(resources.GetObject("lblType.TabIndex")));
      this.lblType.Text = resources.GetString("lblType.Text");
      this.lblType.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblType.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblType, resources.GetString("lblType.ToolTip"));
      this.lblType.Visible = ((bool)(resources.GetObject("lblType.Visible")));
      // 
      // lblID
      // 
      this.lblID.AccessibleDescription = resources.GetString("lblID.AccessibleDescription");
      this.lblID.AccessibleName = resources.GetString("lblID.AccessibleName");
      this.lblID.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblID.Anchor")));
      this.lblID.AutoSize = ((bool)(resources.GetObject("lblID.AutoSize")));
      this.lblID.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblID.Dock")));
      this.lblID.Enabled = ((bool)(resources.GetObject("lblID.Enabled")));
      this.lblID.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblID.Font = ((System.Drawing.Font)(resources.GetObject("lblID.Font")));
      this.lblID.Image = ((System.Drawing.Image)(resources.GetObject("lblID.Image")));
      this.lblID.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblID.ImageAlign")));
      this.lblID.ImageIndex = ((int)(resources.GetObject("lblID.ImageIndex")));
      this.lblID.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblID.ImeMode")));
      this.lblID.Location = ((System.Drawing.Point)(resources.GetObject("lblID.Location")));
      this.lblID.Name = "lblID";
      this.lblID.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblID.RightToLeft")));
      this.lblID.Size = ((System.Drawing.Size)(resources.GetObject("lblID.Size")));
      this.lblID.TabIndex = ((int)(resources.GetObject("lblID.TabIndex")));
      this.lblID.Text = resources.GetString("lblID.Text");
      this.lblID.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblID.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblID, resources.GetString("lblID.ToolTip"));
      this.lblID.Visible = ((bool)(resources.GetObject("lblID.Visible")));
      // 
      // txtStackSize
      // 
      this.txtStackSize.AccessibleDescription = resources.GetString("txtStackSize.AccessibleDescription");
      this.txtStackSize.AccessibleName = resources.GetString("txtStackSize.AccessibleName");
      this.txtStackSize.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtStackSize.Anchor")));
      this.txtStackSize.AutoSize = ((bool)(resources.GetObject("txtStackSize.AutoSize")));
      this.txtStackSize.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtStackSize.BackgroundImage")));
      this.txtStackSize.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtStackSize.Dock")));
      this.txtStackSize.Enabled = ((bool)(resources.GetObject("txtStackSize.Enabled")));
      this.txtStackSize.Font = ((System.Drawing.Font)(resources.GetObject("txtStackSize.Font")));
      this.txtStackSize.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtStackSize.ImeMode")));
      this.txtStackSize.Location = ((System.Drawing.Point)(resources.GetObject("txtStackSize.Location")));
      this.txtStackSize.MaxLength = ((int)(resources.GetObject("txtStackSize.MaxLength")));
      this.txtStackSize.Multiline = ((bool)(resources.GetObject("txtStackSize.Multiline")));
      this.txtStackSize.Name = "txtStackSize";
      this.txtStackSize.PasswordChar = ((char)(resources.GetObject("txtStackSize.PasswordChar")));
      this.txtStackSize.ReadOnly = true;
      this.txtStackSize.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtStackSize.RightToLeft")));
      this.txtStackSize.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtStackSize.ScrollBars")));
      this.txtStackSize.Size = ((System.Drawing.Size)(resources.GetObject("txtStackSize.Size")));
      this.txtStackSize.TabIndex = ((int)(resources.GetObject("txtStackSize.TabIndex")));
      this.txtStackSize.Text = resources.GetString("txtStackSize.Text");
      this.txtStackSize.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtStackSize.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtStackSize, resources.GetString("txtStackSize.ToolTip"));
      this.txtStackSize.Visible = ((bool)(resources.GetObject("txtStackSize.Visible")));
      this.txtStackSize.WordWrap = ((bool)(resources.GetObject("txtStackSize.WordWrap")));
      // 
      // txtFlags
      // 
      this.txtFlags.AccessibleDescription = resources.GetString("txtFlags.AccessibleDescription");
      this.txtFlags.AccessibleName = resources.GetString("txtFlags.AccessibleName");
      this.txtFlags.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtFlags.Anchor")));
      this.txtFlags.AutoSize = ((bool)(resources.GetObject("txtFlags.AutoSize")));
      this.txtFlags.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtFlags.BackgroundImage")));
      this.txtFlags.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtFlags.Dock")));
      this.txtFlags.Enabled = ((bool)(resources.GetObject("txtFlags.Enabled")));
      this.txtFlags.Font = ((System.Drawing.Font)(resources.GetObject("txtFlags.Font")));
      this.txtFlags.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtFlags.ImeMode")));
      this.txtFlags.Location = ((System.Drawing.Point)(resources.GetObject("txtFlags.Location")));
      this.txtFlags.MaxLength = ((int)(resources.GetObject("txtFlags.MaxLength")));
      this.txtFlags.Multiline = ((bool)(resources.GetObject("txtFlags.Multiline")));
      this.txtFlags.Name = "txtFlags";
      this.txtFlags.PasswordChar = ((char)(resources.GetObject("txtFlags.PasswordChar")));
      this.txtFlags.ReadOnly = true;
      this.txtFlags.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtFlags.RightToLeft")));
      this.txtFlags.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtFlags.ScrollBars")));
      this.txtFlags.Size = ((System.Drawing.Size)(resources.GetObject("txtFlags.Size")));
      this.txtFlags.TabIndex = ((int)(resources.GetObject("txtFlags.TabIndex")));
      this.txtFlags.Text = resources.GetString("txtFlags.Text");
      this.txtFlags.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtFlags.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtFlags, resources.GetString("txtFlags.ToolTip"));
      this.txtFlags.Visible = ((bool)(resources.GetObject("txtFlags.Visible")));
      this.txtFlags.WordWrap = ((bool)(resources.GetObject("txtFlags.WordWrap")));
      // 
      // txtType
      // 
      this.txtType.AccessibleDescription = resources.GetString("txtType.AccessibleDescription");
      this.txtType.AccessibleName = resources.GetString("txtType.AccessibleName");
      this.txtType.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtType.Anchor")));
      this.txtType.AutoSize = ((bool)(resources.GetObject("txtType.AutoSize")));
      this.txtType.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtType.BackgroundImage")));
      this.txtType.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtType.Dock")));
      this.txtType.Enabled = ((bool)(resources.GetObject("txtType.Enabled")));
      this.txtType.Font = ((System.Drawing.Font)(resources.GetObject("txtType.Font")));
      this.txtType.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtType.ImeMode")));
      this.txtType.Location = ((System.Drawing.Point)(resources.GetObject("txtType.Location")));
      this.txtType.MaxLength = ((int)(resources.GetObject("txtType.MaxLength")));
      this.txtType.Multiline = ((bool)(resources.GetObject("txtType.Multiline")));
      this.txtType.Name = "txtType";
      this.txtType.PasswordChar = ((char)(resources.GetObject("txtType.PasswordChar")));
      this.txtType.ReadOnly = true;
      this.txtType.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtType.RightToLeft")));
      this.txtType.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtType.ScrollBars")));
      this.txtType.Size = ((System.Drawing.Size)(resources.GetObject("txtType.Size")));
      this.txtType.TabIndex = ((int)(resources.GetObject("txtType.TabIndex")));
      this.txtType.Text = resources.GetString("txtType.Text");
      this.txtType.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtType.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtType, resources.GetString("txtType.ToolTip"));
      this.txtType.Visible = ((bool)(resources.GetObject("txtType.Visible")));
      this.txtType.WordWrap = ((bool)(resources.GetObject("txtType.WordWrap")));
      // 
      // txtID
      // 
      this.txtID.AccessibleDescription = resources.GetString("txtID.AccessibleDescription");
      this.txtID.AccessibleName = resources.GetString("txtID.AccessibleName");
      this.txtID.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtID.Anchor")));
      this.txtID.AutoSize = ((bool)(resources.GetObject("txtID.AutoSize")));
      this.txtID.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtID.BackgroundImage")));
      this.txtID.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtID.Dock")));
      this.txtID.Enabled = ((bool)(resources.GetObject("txtID.Enabled")));
      this.txtID.Font = ((System.Drawing.Font)(resources.GetObject("txtID.Font")));
      this.txtID.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtID.ImeMode")));
      this.txtID.Location = ((System.Drawing.Point)(resources.GetObject("txtID.Location")));
      this.txtID.MaxLength = ((int)(resources.GetObject("txtID.MaxLength")));
      this.txtID.Multiline = ((bool)(resources.GetObject("txtID.Multiline")));
      this.txtID.Name = "txtID";
      this.txtID.PasswordChar = ((char)(resources.GetObject("txtID.PasswordChar")));
      this.txtID.ReadOnly = true;
      this.txtID.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtID.RightToLeft")));
      this.txtID.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtID.ScrollBars")));
      this.txtID.Size = ((System.Drawing.Size)(resources.GetObject("txtID.Size")));
      this.txtID.TabIndex = ((int)(resources.GetObject("txtID.TabIndex")));
      this.txtID.Text = resources.GetString("txtID.Text");
      this.txtID.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtID.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtID, resources.GetString("txtID.ToolTip"));
      this.txtID.Visible = ((bool)(resources.GetObject("txtID.Visible")));
      this.txtID.WordWrap = ((bool)(resources.GetObject("txtID.WordWrap")));
      // 
      // picIcon
      // 
      this.picIcon.AccessibleDescription = resources.GetString("picIcon.AccessibleDescription");
      this.picIcon.AccessibleName = resources.GetString("picIcon.AccessibleName");
      this.picIcon.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("picIcon.Anchor")));
      this.picIcon.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picIcon.BackgroundImage")));
      this.picIcon.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.picIcon.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("picIcon.Dock")));
      this.picIcon.Enabled = ((bool)(resources.GetObject("picIcon.Enabled")));
      this.picIcon.Font = ((System.Drawing.Font)(resources.GetObject("picIcon.Font")));
      this.picIcon.Image = ((System.Drawing.Image)(resources.GetObject("picIcon.Image")));
      this.picIcon.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("picIcon.ImeMode")));
      this.picIcon.Location = ((System.Drawing.Point)(resources.GetObject("picIcon.Location")));
      this.picIcon.Name = "picIcon";
      this.picIcon.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("picIcon.RightToLeft")));
      this.picIcon.Size = ((System.Drawing.Size)(resources.GetObject("picIcon.Size")));
      this.picIcon.SizeMode = ((System.Windows.Forms.PictureBoxSizeMode)(resources.GetObject("picIcon.SizeMode")));
      this.picIcon.TabIndex = ((int)(resources.GetObject("picIcon.TabIndex")));
      this.picIcon.TabStop = false;
      this.picIcon.Text = resources.GetString("picIcon.Text");
      this.ttToolTip.SetToolTip(this.picIcon, resources.GetString("picIcon.ToolTip"));
      this.picIcon.Visible = ((bool)(resources.GetObject("picIcon.Visible")));
      // 
      // grpWeaponInfo
      // 
      this.grpWeaponInfo.AccessibleDescription = resources.GetString("grpWeaponInfo.AccessibleDescription");
      this.grpWeaponInfo.AccessibleName = resources.GetString("grpWeaponInfo.AccessibleName");
      this.grpWeaponInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("grpWeaponInfo.Anchor")));
      this.grpWeaponInfo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpWeaponInfo.BackgroundImage")));
      this.grpWeaponInfo.Controls.Add(this.lblDPS);
      this.grpWeaponInfo.Controls.Add(this.txtDPS);
      this.grpWeaponInfo.Controls.Add(this.lblDelay);
      this.grpWeaponInfo.Controls.Add(this.lblDamage);
      this.grpWeaponInfo.Controls.Add(this.txtDelay);
      this.grpWeaponInfo.Controls.Add(this.txtDamage);
      this.grpWeaponInfo.Controls.Add(this.lblSkill);
      this.grpWeaponInfo.Controls.Add(this.txtSkill);
      this.grpWeaponInfo.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("grpWeaponInfo.Dock")));
      this.grpWeaponInfo.Enabled = ((bool)(resources.GetObject("grpWeaponInfo.Enabled")));
      this.grpWeaponInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.grpWeaponInfo.Font = ((System.Drawing.Font)(resources.GetObject("grpWeaponInfo.Font")));
      this.grpWeaponInfo.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("grpWeaponInfo.ImeMode")));
      this.grpWeaponInfo.Location = ((System.Drawing.Point)(resources.GetObject("grpWeaponInfo.Location")));
      this.grpWeaponInfo.Name = "grpWeaponInfo";
      this.grpWeaponInfo.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("grpWeaponInfo.RightToLeft")));
      this.grpWeaponInfo.Size = ((System.Drawing.Size)(resources.GetObject("grpWeaponInfo.Size")));
      this.grpWeaponInfo.TabIndex = ((int)(resources.GetObject("grpWeaponInfo.TabIndex")));
      this.grpWeaponInfo.TabStop = false;
      this.grpWeaponInfo.Text = resources.GetString("grpWeaponInfo.Text");
      this.ttToolTip.SetToolTip(this.grpWeaponInfo, resources.GetString("grpWeaponInfo.ToolTip"));
      this.grpWeaponInfo.Visible = ((bool)(resources.GetObject("grpWeaponInfo.Visible")));
      // 
      // lblDPS
      // 
      this.lblDPS.AccessibleDescription = resources.GetString("lblDPS.AccessibleDescription");
      this.lblDPS.AccessibleName = resources.GetString("lblDPS.AccessibleName");
      this.lblDPS.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblDPS.Anchor")));
      this.lblDPS.AutoSize = ((bool)(resources.GetObject("lblDPS.AutoSize")));
      this.lblDPS.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblDPS.Dock")));
      this.lblDPS.Enabled = ((bool)(resources.GetObject("lblDPS.Enabled")));
      this.lblDPS.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblDPS.Font = ((System.Drawing.Font)(resources.GetObject("lblDPS.Font")));
      this.lblDPS.Image = ((System.Drawing.Image)(resources.GetObject("lblDPS.Image")));
      this.lblDPS.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblDPS.ImageAlign")));
      this.lblDPS.ImageIndex = ((int)(resources.GetObject("lblDPS.ImageIndex")));
      this.lblDPS.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblDPS.ImeMode")));
      this.lblDPS.Location = ((System.Drawing.Point)(resources.GetObject("lblDPS.Location")));
      this.lblDPS.Name = "lblDPS";
      this.lblDPS.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblDPS.RightToLeft")));
      this.lblDPS.Size = ((System.Drawing.Size)(resources.GetObject("lblDPS.Size")));
      this.lblDPS.TabIndex = ((int)(resources.GetObject("lblDPS.TabIndex")));
      this.lblDPS.Text = resources.GetString("lblDPS.Text");
      this.lblDPS.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblDPS.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblDPS, resources.GetString("lblDPS.ToolTip"));
      this.lblDPS.Visible = ((bool)(resources.GetObject("lblDPS.Visible")));
      // 
      // txtDPS
      // 
      this.txtDPS.AccessibleDescription = resources.GetString("txtDPS.AccessibleDescription");
      this.txtDPS.AccessibleName = resources.GetString("txtDPS.AccessibleName");
      this.txtDPS.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtDPS.Anchor")));
      this.txtDPS.AutoSize = ((bool)(resources.GetObject("txtDPS.AutoSize")));
      this.txtDPS.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtDPS.BackgroundImage")));
      this.txtDPS.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtDPS.Dock")));
      this.txtDPS.Enabled = ((bool)(resources.GetObject("txtDPS.Enabled")));
      this.txtDPS.Font = ((System.Drawing.Font)(resources.GetObject("txtDPS.Font")));
      this.txtDPS.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtDPS.ImeMode")));
      this.txtDPS.Location = ((System.Drawing.Point)(resources.GetObject("txtDPS.Location")));
      this.txtDPS.MaxLength = ((int)(resources.GetObject("txtDPS.MaxLength")));
      this.txtDPS.Multiline = ((bool)(resources.GetObject("txtDPS.Multiline")));
      this.txtDPS.Name = "txtDPS";
      this.txtDPS.PasswordChar = ((char)(resources.GetObject("txtDPS.PasswordChar")));
      this.txtDPS.ReadOnly = true;
      this.txtDPS.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtDPS.RightToLeft")));
      this.txtDPS.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtDPS.ScrollBars")));
      this.txtDPS.Size = ((System.Drawing.Size)(resources.GetObject("txtDPS.Size")));
      this.txtDPS.TabIndex = ((int)(resources.GetObject("txtDPS.TabIndex")));
      this.txtDPS.Text = resources.GetString("txtDPS.Text");
      this.txtDPS.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtDPS.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtDPS, resources.GetString("txtDPS.ToolTip"));
      this.txtDPS.Visible = ((bool)(resources.GetObject("txtDPS.Visible")));
      this.txtDPS.WordWrap = ((bool)(resources.GetObject("txtDPS.WordWrap")));
      // 
      // lblDelay
      // 
      this.lblDelay.AccessibleDescription = resources.GetString("lblDelay.AccessibleDescription");
      this.lblDelay.AccessibleName = resources.GetString("lblDelay.AccessibleName");
      this.lblDelay.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblDelay.Anchor")));
      this.lblDelay.AutoSize = ((bool)(resources.GetObject("lblDelay.AutoSize")));
      this.lblDelay.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblDelay.Dock")));
      this.lblDelay.Enabled = ((bool)(resources.GetObject("lblDelay.Enabled")));
      this.lblDelay.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblDelay.Font = ((System.Drawing.Font)(resources.GetObject("lblDelay.Font")));
      this.lblDelay.Image = ((System.Drawing.Image)(resources.GetObject("lblDelay.Image")));
      this.lblDelay.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblDelay.ImageAlign")));
      this.lblDelay.ImageIndex = ((int)(resources.GetObject("lblDelay.ImageIndex")));
      this.lblDelay.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblDelay.ImeMode")));
      this.lblDelay.Location = ((System.Drawing.Point)(resources.GetObject("lblDelay.Location")));
      this.lblDelay.Name = "lblDelay";
      this.lblDelay.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblDelay.RightToLeft")));
      this.lblDelay.Size = ((System.Drawing.Size)(resources.GetObject("lblDelay.Size")));
      this.lblDelay.TabIndex = ((int)(resources.GetObject("lblDelay.TabIndex")));
      this.lblDelay.Text = resources.GetString("lblDelay.Text");
      this.lblDelay.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblDelay.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblDelay, resources.GetString("lblDelay.ToolTip"));
      this.lblDelay.Visible = ((bool)(resources.GetObject("lblDelay.Visible")));
      // 
      // lblDamage
      // 
      this.lblDamage.AccessibleDescription = resources.GetString("lblDamage.AccessibleDescription");
      this.lblDamage.AccessibleName = resources.GetString("lblDamage.AccessibleName");
      this.lblDamage.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblDamage.Anchor")));
      this.lblDamage.AutoSize = ((bool)(resources.GetObject("lblDamage.AutoSize")));
      this.lblDamage.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblDamage.Dock")));
      this.lblDamage.Enabled = ((bool)(resources.GetObject("lblDamage.Enabled")));
      this.lblDamage.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblDamage.Font = ((System.Drawing.Font)(resources.GetObject("lblDamage.Font")));
      this.lblDamage.Image = ((System.Drawing.Image)(resources.GetObject("lblDamage.Image")));
      this.lblDamage.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblDamage.ImageAlign")));
      this.lblDamage.ImageIndex = ((int)(resources.GetObject("lblDamage.ImageIndex")));
      this.lblDamage.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblDamage.ImeMode")));
      this.lblDamage.Location = ((System.Drawing.Point)(resources.GetObject("lblDamage.Location")));
      this.lblDamage.Name = "lblDamage";
      this.lblDamage.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblDamage.RightToLeft")));
      this.lblDamage.Size = ((System.Drawing.Size)(resources.GetObject("lblDamage.Size")));
      this.lblDamage.TabIndex = ((int)(resources.GetObject("lblDamage.TabIndex")));
      this.lblDamage.Text = resources.GetString("lblDamage.Text");
      this.lblDamage.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblDamage.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblDamage, resources.GetString("lblDamage.ToolTip"));
      this.lblDamage.Visible = ((bool)(resources.GetObject("lblDamage.Visible")));
      // 
      // txtDelay
      // 
      this.txtDelay.AccessibleDescription = resources.GetString("txtDelay.AccessibleDescription");
      this.txtDelay.AccessibleName = resources.GetString("txtDelay.AccessibleName");
      this.txtDelay.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtDelay.Anchor")));
      this.txtDelay.AutoSize = ((bool)(resources.GetObject("txtDelay.AutoSize")));
      this.txtDelay.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtDelay.BackgroundImage")));
      this.txtDelay.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtDelay.Dock")));
      this.txtDelay.Enabled = ((bool)(resources.GetObject("txtDelay.Enabled")));
      this.txtDelay.Font = ((System.Drawing.Font)(resources.GetObject("txtDelay.Font")));
      this.txtDelay.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtDelay.ImeMode")));
      this.txtDelay.Location = ((System.Drawing.Point)(resources.GetObject("txtDelay.Location")));
      this.txtDelay.MaxLength = ((int)(resources.GetObject("txtDelay.MaxLength")));
      this.txtDelay.Multiline = ((bool)(resources.GetObject("txtDelay.Multiline")));
      this.txtDelay.Name = "txtDelay";
      this.txtDelay.PasswordChar = ((char)(resources.GetObject("txtDelay.PasswordChar")));
      this.txtDelay.ReadOnly = true;
      this.txtDelay.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtDelay.RightToLeft")));
      this.txtDelay.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtDelay.ScrollBars")));
      this.txtDelay.Size = ((System.Drawing.Size)(resources.GetObject("txtDelay.Size")));
      this.txtDelay.TabIndex = ((int)(resources.GetObject("txtDelay.TabIndex")));
      this.txtDelay.Text = resources.GetString("txtDelay.Text");
      this.txtDelay.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtDelay.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtDelay, resources.GetString("txtDelay.ToolTip"));
      this.txtDelay.Visible = ((bool)(resources.GetObject("txtDelay.Visible")));
      this.txtDelay.WordWrap = ((bool)(resources.GetObject("txtDelay.WordWrap")));
      // 
      // txtDamage
      // 
      this.txtDamage.AccessibleDescription = resources.GetString("txtDamage.AccessibleDescription");
      this.txtDamage.AccessibleName = resources.GetString("txtDamage.AccessibleName");
      this.txtDamage.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtDamage.Anchor")));
      this.txtDamage.AutoSize = ((bool)(resources.GetObject("txtDamage.AutoSize")));
      this.txtDamage.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtDamage.BackgroundImage")));
      this.txtDamage.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtDamage.Dock")));
      this.txtDamage.Enabled = ((bool)(resources.GetObject("txtDamage.Enabled")));
      this.txtDamage.Font = ((System.Drawing.Font)(resources.GetObject("txtDamage.Font")));
      this.txtDamage.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtDamage.ImeMode")));
      this.txtDamage.Location = ((System.Drawing.Point)(resources.GetObject("txtDamage.Location")));
      this.txtDamage.MaxLength = ((int)(resources.GetObject("txtDamage.MaxLength")));
      this.txtDamage.Multiline = ((bool)(resources.GetObject("txtDamage.Multiline")));
      this.txtDamage.Name = "txtDamage";
      this.txtDamage.PasswordChar = ((char)(resources.GetObject("txtDamage.PasswordChar")));
      this.txtDamage.ReadOnly = true;
      this.txtDamage.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtDamage.RightToLeft")));
      this.txtDamage.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtDamage.ScrollBars")));
      this.txtDamage.Size = ((System.Drawing.Size)(resources.GetObject("txtDamage.Size")));
      this.txtDamage.TabIndex = ((int)(resources.GetObject("txtDamage.TabIndex")));
      this.txtDamage.Text = resources.GetString("txtDamage.Text");
      this.txtDamage.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtDamage.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtDamage, resources.GetString("txtDamage.ToolTip"));
      this.txtDamage.Visible = ((bool)(resources.GetObject("txtDamage.Visible")));
      this.txtDamage.WordWrap = ((bool)(resources.GetObject("txtDamage.WordWrap")));
      // 
      // lblSkill
      // 
      this.lblSkill.AccessibleDescription = resources.GetString("lblSkill.AccessibleDescription");
      this.lblSkill.AccessibleName = resources.GetString("lblSkill.AccessibleName");
      this.lblSkill.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblSkill.Anchor")));
      this.lblSkill.AutoSize = ((bool)(resources.GetObject("lblSkill.AutoSize")));
      this.lblSkill.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblSkill.Dock")));
      this.lblSkill.Enabled = ((bool)(resources.GetObject("lblSkill.Enabled")));
      this.lblSkill.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblSkill.Font = ((System.Drawing.Font)(resources.GetObject("lblSkill.Font")));
      this.lblSkill.Image = ((System.Drawing.Image)(resources.GetObject("lblSkill.Image")));
      this.lblSkill.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblSkill.ImageAlign")));
      this.lblSkill.ImageIndex = ((int)(resources.GetObject("lblSkill.ImageIndex")));
      this.lblSkill.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblSkill.ImeMode")));
      this.lblSkill.Location = ((System.Drawing.Point)(resources.GetObject("lblSkill.Location")));
      this.lblSkill.Name = "lblSkill";
      this.lblSkill.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblSkill.RightToLeft")));
      this.lblSkill.Size = ((System.Drawing.Size)(resources.GetObject("lblSkill.Size")));
      this.lblSkill.TabIndex = ((int)(resources.GetObject("lblSkill.TabIndex")));
      this.lblSkill.Text = resources.GetString("lblSkill.Text");
      this.lblSkill.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblSkill.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblSkill, resources.GetString("lblSkill.ToolTip"));
      this.lblSkill.Visible = ((bool)(resources.GetObject("lblSkill.Visible")));
      // 
      // txtSkill
      // 
      this.txtSkill.AccessibleDescription = resources.GetString("txtSkill.AccessibleDescription");
      this.txtSkill.AccessibleName = resources.GetString("txtSkill.AccessibleName");
      this.txtSkill.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtSkill.Anchor")));
      this.txtSkill.AutoSize = ((bool)(resources.GetObject("txtSkill.AutoSize")));
      this.txtSkill.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtSkill.BackgroundImage")));
      this.txtSkill.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtSkill.Dock")));
      this.txtSkill.Enabled = ((bool)(resources.GetObject("txtSkill.Enabled")));
      this.txtSkill.Font = ((System.Drawing.Font)(resources.GetObject("txtSkill.Font")));
      this.txtSkill.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtSkill.ImeMode")));
      this.txtSkill.Location = ((System.Drawing.Point)(resources.GetObject("txtSkill.Location")));
      this.txtSkill.MaxLength = ((int)(resources.GetObject("txtSkill.MaxLength")));
      this.txtSkill.Multiline = ((bool)(resources.GetObject("txtSkill.Multiline")));
      this.txtSkill.Name = "txtSkill";
      this.txtSkill.PasswordChar = ((char)(resources.GetObject("txtSkill.PasswordChar")));
      this.txtSkill.ReadOnly = true;
      this.txtSkill.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtSkill.RightToLeft")));
      this.txtSkill.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtSkill.ScrollBars")));
      this.txtSkill.Size = ((System.Drawing.Size)(resources.GetObject("txtSkill.Size")));
      this.txtSkill.TabIndex = ((int)(resources.GetObject("txtSkill.TabIndex")));
      this.txtSkill.Text = resources.GetString("txtSkill.Text");
      this.txtSkill.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtSkill.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtSkill, resources.GetString("txtSkill.ToolTip"));
      this.txtSkill.Visible = ((bool)(resources.GetObject("txtSkill.Visible")));
      this.txtSkill.WordWrap = ((bool)(resources.GetObject("txtSkill.WordWrap")));
      // 
      // grpFurnitureInfo
      // 
      this.grpFurnitureInfo.AccessibleDescription = resources.GetString("grpFurnitureInfo.AccessibleDescription");
      this.grpFurnitureInfo.AccessibleName = resources.GetString("grpFurnitureInfo.AccessibleName");
      this.grpFurnitureInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("grpFurnitureInfo.Anchor")));
      this.grpFurnitureInfo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpFurnitureInfo.BackgroundImage")));
      this.grpFurnitureInfo.Controls.Add(this.lblStorage);
      this.grpFurnitureInfo.Controls.Add(this.txtStorage);
      this.grpFurnitureInfo.Controls.Add(this.lblElement);
      this.grpFurnitureInfo.Controls.Add(this.txtElement);
      this.grpFurnitureInfo.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("grpFurnitureInfo.Dock")));
      this.grpFurnitureInfo.Enabled = ((bool)(resources.GetObject("grpFurnitureInfo.Enabled")));
      this.grpFurnitureInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.grpFurnitureInfo.Font = ((System.Drawing.Font)(resources.GetObject("grpFurnitureInfo.Font")));
      this.grpFurnitureInfo.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("grpFurnitureInfo.ImeMode")));
      this.grpFurnitureInfo.Location = ((System.Drawing.Point)(resources.GetObject("grpFurnitureInfo.Location")));
      this.grpFurnitureInfo.Name = "grpFurnitureInfo";
      this.grpFurnitureInfo.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("grpFurnitureInfo.RightToLeft")));
      this.grpFurnitureInfo.Size = ((System.Drawing.Size)(resources.GetObject("grpFurnitureInfo.Size")));
      this.grpFurnitureInfo.TabIndex = ((int)(resources.GetObject("grpFurnitureInfo.TabIndex")));
      this.grpFurnitureInfo.TabStop = false;
      this.grpFurnitureInfo.Text = resources.GetString("grpFurnitureInfo.Text");
      this.ttToolTip.SetToolTip(this.grpFurnitureInfo, resources.GetString("grpFurnitureInfo.ToolTip"));
      this.grpFurnitureInfo.Visible = ((bool)(resources.GetObject("grpFurnitureInfo.Visible")));
      // 
      // lblStorage
      // 
      this.lblStorage.AccessibleDescription = resources.GetString("lblStorage.AccessibleDescription");
      this.lblStorage.AccessibleName = resources.GetString("lblStorage.AccessibleName");
      this.lblStorage.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblStorage.Anchor")));
      this.lblStorage.AutoSize = ((bool)(resources.GetObject("lblStorage.AutoSize")));
      this.lblStorage.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblStorage.Dock")));
      this.lblStorage.Enabled = ((bool)(resources.GetObject("lblStorage.Enabled")));
      this.lblStorage.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblStorage.Font = ((System.Drawing.Font)(resources.GetObject("lblStorage.Font")));
      this.lblStorage.Image = ((System.Drawing.Image)(resources.GetObject("lblStorage.Image")));
      this.lblStorage.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblStorage.ImageAlign")));
      this.lblStorage.ImageIndex = ((int)(resources.GetObject("lblStorage.ImageIndex")));
      this.lblStorage.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblStorage.ImeMode")));
      this.lblStorage.Location = ((System.Drawing.Point)(resources.GetObject("lblStorage.Location")));
      this.lblStorage.Name = "lblStorage";
      this.lblStorage.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblStorage.RightToLeft")));
      this.lblStorage.Size = ((System.Drawing.Size)(resources.GetObject("lblStorage.Size")));
      this.lblStorage.TabIndex = ((int)(resources.GetObject("lblStorage.TabIndex")));
      this.lblStorage.Text = resources.GetString("lblStorage.Text");
      this.lblStorage.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblStorage.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblStorage, resources.GetString("lblStorage.ToolTip"));
      this.lblStorage.Visible = ((bool)(resources.GetObject("lblStorage.Visible")));
      // 
      // txtStorage
      // 
      this.txtStorage.AccessibleDescription = resources.GetString("txtStorage.AccessibleDescription");
      this.txtStorage.AccessibleName = resources.GetString("txtStorage.AccessibleName");
      this.txtStorage.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtStorage.Anchor")));
      this.txtStorage.AutoSize = ((bool)(resources.GetObject("txtStorage.AutoSize")));
      this.txtStorage.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtStorage.BackgroundImage")));
      this.txtStorage.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtStorage.Dock")));
      this.txtStorage.Enabled = ((bool)(resources.GetObject("txtStorage.Enabled")));
      this.txtStorage.Font = ((System.Drawing.Font)(resources.GetObject("txtStorage.Font")));
      this.txtStorage.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtStorage.ImeMode")));
      this.txtStorage.Location = ((System.Drawing.Point)(resources.GetObject("txtStorage.Location")));
      this.txtStorage.MaxLength = ((int)(resources.GetObject("txtStorage.MaxLength")));
      this.txtStorage.Multiline = ((bool)(resources.GetObject("txtStorage.Multiline")));
      this.txtStorage.Name = "txtStorage";
      this.txtStorage.PasswordChar = ((char)(resources.GetObject("txtStorage.PasswordChar")));
      this.txtStorage.ReadOnly = true;
      this.txtStorage.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtStorage.RightToLeft")));
      this.txtStorage.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtStorage.ScrollBars")));
      this.txtStorage.Size = ((System.Drawing.Size)(resources.GetObject("txtStorage.Size")));
      this.txtStorage.TabIndex = ((int)(resources.GetObject("txtStorage.TabIndex")));
      this.txtStorage.Text = resources.GetString("txtStorage.Text");
      this.txtStorage.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtStorage.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtStorage, resources.GetString("txtStorage.ToolTip"));
      this.txtStorage.Visible = ((bool)(resources.GetObject("txtStorage.Visible")));
      this.txtStorage.WordWrap = ((bool)(resources.GetObject("txtStorage.WordWrap")));
      // 
      // lblElement
      // 
      this.lblElement.AccessibleDescription = resources.GetString("lblElement.AccessibleDescription");
      this.lblElement.AccessibleName = resources.GetString("lblElement.AccessibleName");
      this.lblElement.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblElement.Anchor")));
      this.lblElement.AutoSize = ((bool)(resources.GetObject("lblElement.AutoSize")));
      this.lblElement.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblElement.Dock")));
      this.lblElement.Enabled = ((bool)(resources.GetObject("lblElement.Enabled")));
      this.lblElement.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblElement.Font = ((System.Drawing.Font)(resources.GetObject("lblElement.Font")));
      this.lblElement.Image = ((System.Drawing.Image)(resources.GetObject("lblElement.Image")));
      this.lblElement.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblElement.ImageAlign")));
      this.lblElement.ImageIndex = ((int)(resources.GetObject("lblElement.ImageIndex")));
      this.lblElement.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblElement.ImeMode")));
      this.lblElement.Location = ((System.Drawing.Point)(resources.GetObject("lblElement.Location")));
      this.lblElement.Name = "lblElement";
      this.lblElement.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblElement.RightToLeft")));
      this.lblElement.Size = ((System.Drawing.Size)(resources.GetObject("lblElement.Size")));
      this.lblElement.TabIndex = ((int)(resources.GetObject("lblElement.TabIndex")));
      this.lblElement.Text = resources.GetString("lblElement.Text");
      this.lblElement.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblElement.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblElement, resources.GetString("lblElement.ToolTip"));
      this.lblElement.Visible = ((bool)(resources.GetObject("lblElement.Visible")));
      // 
      // txtElement
      // 
      this.txtElement.AccessibleDescription = resources.GetString("txtElement.AccessibleDescription");
      this.txtElement.AccessibleName = resources.GetString("txtElement.AccessibleName");
      this.txtElement.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtElement.Anchor")));
      this.txtElement.AutoSize = ((bool)(resources.GetObject("txtElement.AutoSize")));
      this.txtElement.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtElement.BackgroundImage")));
      this.txtElement.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtElement.Dock")));
      this.txtElement.Enabled = ((bool)(resources.GetObject("txtElement.Enabled")));
      this.txtElement.Font = ((System.Drawing.Font)(resources.GetObject("txtElement.Font")));
      this.txtElement.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtElement.ImeMode")));
      this.txtElement.Location = ((System.Drawing.Point)(resources.GetObject("txtElement.Location")));
      this.txtElement.MaxLength = ((int)(resources.GetObject("txtElement.MaxLength")));
      this.txtElement.Multiline = ((bool)(resources.GetObject("txtElement.Multiline")));
      this.txtElement.Name = "txtElement";
      this.txtElement.PasswordChar = ((char)(resources.GetObject("txtElement.PasswordChar")));
      this.txtElement.ReadOnly = true;
      this.txtElement.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtElement.RightToLeft")));
      this.txtElement.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtElement.ScrollBars")));
      this.txtElement.Size = ((System.Drawing.Size)(resources.GetObject("txtElement.Size")));
      this.txtElement.TabIndex = ((int)(resources.GetObject("txtElement.TabIndex")));
      this.txtElement.Text = resources.GetString("txtElement.Text");
      this.txtElement.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtElement.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtElement, resources.GetString("txtElement.ToolTip"));
      this.txtElement.Visible = ((bool)(resources.GetObject("txtElement.Visible")));
      this.txtElement.WordWrap = ((bool)(resources.GetObject("txtElement.WordWrap")));
      // 
      // grpShieldInfo
      // 
      this.grpShieldInfo.AccessibleDescription = resources.GetString("grpShieldInfo.AccessibleDescription");
      this.grpShieldInfo.AccessibleName = resources.GetString("grpShieldInfo.AccessibleName");
      this.grpShieldInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("grpShieldInfo.Anchor")));
      this.grpShieldInfo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpShieldInfo.BackgroundImage")));
      this.grpShieldInfo.Controls.Add(this.lblShieldSize);
      this.grpShieldInfo.Controls.Add(this.txtShieldSize);
      this.grpShieldInfo.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("grpShieldInfo.Dock")));
      this.grpShieldInfo.Enabled = ((bool)(resources.GetObject("grpShieldInfo.Enabled")));
      this.grpShieldInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.grpShieldInfo.Font = ((System.Drawing.Font)(resources.GetObject("grpShieldInfo.Font")));
      this.grpShieldInfo.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("grpShieldInfo.ImeMode")));
      this.grpShieldInfo.Location = ((System.Drawing.Point)(resources.GetObject("grpShieldInfo.Location")));
      this.grpShieldInfo.Name = "grpShieldInfo";
      this.grpShieldInfo.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("grpShieldInfo.RightToLeft")));
      this.grpShieldInfo.Size = ((System.Drawing.Size)(resources.GetObject("grpShieldInfo.Size")));
      this.grpShieldInfo.TabIndex = ((int)(resources.GetObject("grpShieldInfo.TabIndex")));
      this.grpShieldInfo.TabStop = false;
      this.grpShieldInfo.Text = resources.GetString("grpShieldInfo.Text");
      this.ttToolTip.SetToolTip(this.grpShieldInfo, resources.GetString("grpShieldInfo.ToolTip"));
      this.grpShieldInfo.Visible = ((bool)(resources.GetObject("grpShieldInfo.Visible")));
      // 
      // lblShieldSize
      // 
      this.lblShieldSize.AccessibleDescription = resources.GetString("lblShieldSize.AccessibleDescription");
      this.lblShieldSize.AccessibleName = resources.GetString("lblShieldSize.AccessibleName");
      this.lblShieldSize.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblShieldSize.Anchor")));
      this.lblShieldSize.AutoSize = ((bool)(resources.GetObject("lblShieldSize.AutoSize")));
      this.lblShieldSize.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblShieldSize.Dock")));
      this.lblShieldSize.Enabled = ((bool)(resources.GetObject("lblShieldSize.Enabled")));
      this.lblShieldSize.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblShieldSize.Font = ((System.Drawing.Font)(resources.GetObject("lblShieldSize.Font")));
      this.lblShieldSize.Image = ((System.Drawing.Image)(resources.GetObject("lblShieldSize.Image")));
      this.lblShieldSize.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblShieldSize.ImageAlign")));
      this.lblShieldSize.ImageIndex = ((int)(resources.GetObject("lblShieldSize.ImageIndex")));
      this.lblShieldSize.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblShieldSize.ImeMode")));
      this.lblShieldSize.Location = ((System.Drawing.Point)(resources.GetObject("lblShieldSize.Location")));
      this.lblShieldSize.Name = "lblShieldSize";
      this.lblShieldSize.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblShieldSize.RightToLeft")));
      this.lblShieldSize.Size = ((System.Drawing.Size)(resources.GetObject("lblShieldSize.Size")));
      this.lblShieldSize.TabIndex = ((int)(resources.GetObject("lblShieldSize.TabIndex")));
      this.lblShieldSize.Text = resources.GetString("lblShieldSize.Text");
      this.lblShieldSize.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblShieldSize.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblShieldSize, resources.GetString("lblShieldSize.ToolTip"));
      this.lblShieldSize.Visible = ((bool)(resources.GetObject("lblShieldSize.Visible")));
      // 
      // txtShieldSize
      // 
      this.txtShieldSize.AccessibleDescription = resources.GetString("txtShieldSize.AccessibleDescription");
      this.txtShieldSize.AccessibleName = resources.GetString("txtShieldSize.AccessibleName");
      this.txtShieldSize.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtShieldSize.Anchor")));
      this.txtShieldSize.AutoSize = ((bool)(resources.GetObject("txtShieldSize.AutoSize")));
      this.txtShieldSize.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtShieldSize.BackgroundImage")));
      this.txtShieldSize.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtShieldSize.Dock")));
      this.txtShieldSize.Enabled = ((bool)(resources.GetObject("txtShieldSize.Enabled")));
      this.txtShieldSize.Font = ((System.Drawing.Font)(resources.GetObject("txtShieldSize.Font")));
      this.txtShieldSize.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtShieldSize.ImeMode")));
      this.txtShieldSize.Location = ((System.Drawing.Point)(resources.GetObject("txtShieldSize.Location")));
      this.txtShieldSize.MaxLength = ((int)(resources.GetObject("txtShieldSize.MaxLength")));
      this.txtShieldSize.Multiline = ((bool)(resources.GetObject("txtShieldSize.Multiline")));
      this.txtShieldSize.Name = "txtShieldSize";
      this.txtShieldSize.PasswordChar = ((char)(resources.GetObject("txtShieldSize.PasswordChar")));
      this.txtShieldSize.ReadOnly = true;
      this.txtShieldSize.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtShieldSize.RightToLeft")));
      this.txtShieldSize.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtShieldSize.ScrollBars")));
      this.txtShieldSize.Size = ((System.Drawing.Size)(resources.GetObject("txtShieldSize.Size")));
      this.txtShieldSize.TabIndex = ((int)(resources.GetObject("txtShieldSize.TabIndex")));
      this.txtShieldSize.Text = resources.GetString("txtShieldSize.Text");
      this.txtShieldSize.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtShieldSize.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtShieldSize, resources.GetString("txtShieldSize.ToolTip"));
      this.txtShieldSize.Visible = ((bool)(resources.GetObject("txtShieldSize.Visible")));
      this.txtShieldSize.WordWrap = ((bool)(resources.GetObject("txtShieldSize.WordWrap")));
      // 
      // grpJugInfo
      // 
      this.grpJugInfo.AccessibleDescription = resources.GetString("grpJugInfo.AccessibleDescription");
      this.grpJugInfo.AccessibleName = resources.GetString("grpJugInfo.AccessibleName");
      this.grpJugInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("grpJugInfo.Anchor")));
      this.grpJugInfo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpJugInfo.BackgroundImage")));
      this.grpJugInfo.Controls.Add(this.lblJugSize);
      this.grpJugInfo.Controls.Add(this.txtJugSize);
      this.grpJugInfo.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("grpJugInfo.Dock")));
      this.grpJugInfo.Enabled = ((bool)(resources.GetObject("grpJugInfo.Enabled")));
      this.grpJugInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.grpJugInfo.Font = ((System.Drawing.Font)(resources.GetObject("grpJugInfo.Font")));
      this.grpJugInfo.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("grpJugInfo.ImeMode")));
      this.grpJugInfo.Location = ((System.Drawing.Point)(resources.GetObject("grpJugInfo.Location")));
      this.grpJugInfo.Name = "grpJugInfo";
      this.grpJugInfo.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("grpJugInfo.RightToLeft")));
      this.grpJugInfo.Size = ((System.Drawing.Size)(resources.GetObject("grpJugInfo.Size")));
      this.grpJugInfo.TabIndex = ((int)(resources.GetObject("grpJugInfo.TabIndex")));
      this.grpJugInfo.TabStop = false;
      this.grpJugInfo.Text = resources.GetString("grpJugInfo.Text");
      this.ttToolTip.SetToolTip(this.grpJugInfo, resources.GetString("grpJugInfo.ToolTip"));
      this.grpJugInfo.Visible = ((bool)(resources.GetObject("grpJugInfo.Visible")));
      // 
      // lblJugSize
      // 
      this.lblJugSize.AccessibleDescription = resources.GetString("lblJugSize.AccessibleDescription");
      this.lblJugSize.AccessibleName = resources.GetString("lblJugSize.AccessibleName");
      this.lblJugSize.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblJugSize.Anchor")));
      this.lblJugSize.AutoSize = ((bool)(resources.GetObject("lblJugSize.AutoSize")));
      this.lblJugSize.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblJugSize.Dock")));
      this.lblJugSize.Enabled = ((bool)(resources.GetObject("lblJugSize.Enabled")));
      this.lblJugSize.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblJugSize.Font = ((System.Drawing.Font)(resources.GetObject("lblJugSize.Font")));
      this.lblJugSize.Image = ((System.Drawing.Image)(resources.GetObject("lblJugSize.Image")));
      this.lblJugSize.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblJugSize.ImageAlign")));
      this.lblJugSize.ImageIndex = ((int)(resources.GetObject("lblJugSize.ImageIndex")));
      this.lblJugSize.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblJugSize.ImeMode")));
      this.lblJugSize.Location = ((System.Drawing.Point)(resources.GetObject("lblJugSize.Location")));
      this.lblJugSize.Name = "lblJugSize";
      this.lblJugSize.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblJugSize.RightToLeft")));
      this.lblJugSize.Size = ((System.Drawing.Size)(resources.GetObject("lblJugSize.Size")));
      this.lblJugSize.TabIndex = ((int)(resources.GetObject("lblJugSize.TabIndex")));
      this.lblJugSize.Text = resources.GetString("lblJugSize.Text");
      this.lblJugSize.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblJugSize.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblJugSize, resources.GetString("lblJugSize.ToolTip"));
      this.lblJugSize.Visible = ((bool)(resources.GetObject("lblJugSize.Visible")));
      // 
      // txtJugSize
      // 
      this.txtJugSize.AccessibleDescription = resources.GetString("txtJugSize.AccessibleDescription");
      this.txtJugSize.AccessibleName = resources.GetString("txtJugSize.AccessibleName");
      this.txtJugSize.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtJugSize.Anchor")));
      this.txtJugSize.AutoSize = ((bool)(resources.GetObject("txtJugSize.AutoSize")));
      this.txtJugSize.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtJugSize.BackgroundImage")));
      this.txtJugSize.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtJugSize.Dock")));
      this.txtJugSize.Enabled = ((bool)(resources.GetObject("txtJugSize.Enabled")));
      this.txtJugSize.Font = ((System.Drawing.Font)(resources.GetObject("txtJugSize.Font")));
      this.txtJugSize.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtJugSize.ImeMode")));
      this.txtJugSize.Location = ((System.Drawing.Point)(resources.GetObject("txtJugSize.Location")));
      this.txtJugSize.MaxLength = ((int)(resources.GetObject("txtJugSize.MaxLength")));
      this.txtJugSize.Multiline = ((bool)(resources.GetObject("txtJugSize.Multiline")));
      this.txtJugSize.Name = "txtJugSize";
      this.txtJugSize.PasswordChar = ((char)(resources.GetObject("txtJugSize.PasswordChar")));
      this.txtJugSize.ReadOnly = true;
      this.txtJugSize.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtJugSize.RightToLeft")));
      this.txtJugSize.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtJugSize.ScrollBars")));
      this.txtJugSize.Size = ((System.Drawing.Size)(resources.GetObject("txtJugSize.Size")));
      this.txtJugSize.TabIndex = ((int)(resources.GetObject("txtJugSize.TabIndex")));
      this.txtJugSize.Text = resources.GetString("txtJugSize.Text");
      this.txtJugSize.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtJugSize.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtJugSize, resources.GetString("txtJugSize.ToolTip"));
      this.txtJugSize.Visible = ((bool)(resources.GetObject("txtJugSize.Visible")));
      this.txtJugSize.WordWrap = ((bool)(resources.GetObject("txtJugSize.WordWrap")));
      // 
      // grpEnchantmentInfo
      // 
      this.grpEnchantmentInfo.AccessibleDescription = resources.GetString("grpEnchantmentInfo.AccessibleDescription");
      this.grpEnchantmentInfo.AccessibleName = resources.GetString("grpEnchantmentInfo.AccessibleName");
      this.grpEnchantmentInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("grpEnchantmentInfo.Anchor")));
      this.grpEnchantmentInfo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpEnchantmentInfo.BackgroundImage")));
      this.grpEnchantmentInfo.Controls.Add(this.lblCastTime);
      this.grpEnchantmentInfo.Controls.Add(this.txtCastTime);
      this.grpEnchantmentInfo.Controls.Add(this.lblEquipDelay);
      this.grpEnchantmentInfo.Controls.Add(this.lblReuseTimer);
      this.grpEnchantmentInfo.Controls.Add(this.lblMaxCharges);
      this.grpEnchantmentInfo.Controls.Add(this.txtReuseTimer);
      this.grpEnchantmentInfo.Controls.Add(this.txtEquipDelay);
      this.grpEnchantmentInfo.Controls.Add(this.txtMaxCharges);
      this.grpEnchantmentInfo.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("grpEnchantmentInfo.Dock")));
      this.grpEnchantmentInfo.Enabled = ((bool)(resources.GetObject("grpEnchantmentInfo.Enabled")));
      this.grpEnchantmentInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.grpEnchantmentInfo.Font = ((System.Drawing.Font)(resources.GetObject("grpEnchantmentInfo.Font")));
      this.grpEnchantmentInfo.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("grpEnchantmentInfo.ImeMode")));
      this.grpEnchantmentInfo.Location = ((System.Drawing.Point)(resources.GetObject("grpEnchantmentInfo.Location")));
      this.grpEnchantmentInfo.Name = "grpEnchantmentInfo";
      this.grpEnchantmentInfo.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("grpEnchantmentInfo.RightToLeft")));
      this.grpEnchantmentInfo.Size = ((System.Drawing.Size)(resources.GetObject("grpEnchantmentInfo.Size")));
      this.grpEnchantmentInfo.TabIndex = ((int)(resources.GetObject("grpEnchantmentInfo.TabIndex")));
      this.grpEnchantmentInfo.TabStop = false;
      this.grpEnchantmentInfo.Text = resources.GetString("grpEnchantmentInfo.Text");
      this.ttToolTip.SetToolTip(this.grpEnchantmentInfo, resources.GetString("grpEnchantmentInfo.ToolTip"));
      this.grpEnchantmentInfo.Visible = ((bool)(resources.GetObject("grpEnchantmentInfo.Visible")));
      // 
      // lblCastTime
      // 
      this.lblCastTime.AccessibleDescription = resources.GetString("lblCastTime.AccessibleDescription");
      this.lblCastTime.AccessibleName = resources.GetString("lblCastTime.AccessibleName");
      this.lblCastTime.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblCastTime.Anchor")));
      this.lblCastTime.AutoSize = ((bool)(resources.GetObject("lblCastTime.AutoSize")));
      this.lblCastTime.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblCastTime.Dock")));
      this.lblCastTime.Enabled = ((bool)(resources.GetObject("lblCastTime.Enabled")));
      this.lblCastTime.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblCastTime.Font = ((System.Drawing.Font)(resources.GetObject("lblCastTime.Font")));
      this.lblCastTime.Image = ((System.Drawing.Image)(resources.GetObject("lblCastTime.Image")));
      this.lblCastTime.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblCastTime.ImageAlign")));
      this.lblCastTime.ImageIndex = ((int)(resources.GetObject("lblCastTime.ImageIndex")));
      this.lblCastTime.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblCastTime.ImeMode")));
      this.lblCastTime.Location = ((System.Drawing.Point)(resources.GetObject("lblCastTime.Location")));
      this.lblCastTime.Name = "lblCastTime";
      this.lblCastTime.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblCastTime.RightToLeft")));
      this.lblCastTime.Size = ((System.Drawing.Size)(resources.GetObject("lblCastTime.Size")));
      this.lblCastTime.TabIndex = ((int)(resources.GetObject("lblCastTime.TabIndex")));
      this.lblCastTime.Text = resources.GetString("lblCastTime.Text");
      this.lblCastTime.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblCastTime.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblCastTime, resources.GetString("lblCastTime.ToolTip"));
      this.lblCastTime.Visible = ((bool)(resources.GetObject("lblCastTime.Visible")));
      // 
      // txtCastTime
      // 
      this.txtCastTime.AccessibleDescription = resources.GetString("txtCastTime.AccessibleDescription");
      this.txtCastTime.AccessibleName = resources.GetString("txtCastTime.AccessibleName");
      this.txtCastTime.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtCastTime.Anchor")));
      this.txtCastTime.AutoSize = ((bool)(resources.GetObject("txtCastTime.AutoSize")));
      this.txtCastTime.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtCastTime.BackgroundImage")));
      this.txtCastTime.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtCastTime.Dock")));
      this.txtCastTime.Enabled = ((bool)(resources.GetObject("txtCastTime.Enabled")));
      this.txtCastTime.Font = ((System.Drawing.Font)(resources.GetObject("txtCastTime.Font")));
      this.txtCastTime.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtCastTime.ImeMode")));
      this.txtCastTime.Location = ((System.Drawing.Point)(resources.GetObject("txtCastTime.Location")));
      this.txtCastTime.MaxLength = ((int)(resources.GetObject("txtCastTime.MaxLength")));
      this.txtCastTime.Multiline = ((bool)(resources.GetObject("txtCastTime.Multiline")));
      this.txtCastTime.Name = "txtCastTime";
      this.txtCastTime.PasswordChar = ((char)(resources.GetObject("txtCastTime.PasswordChar")));
      this.txtCastTime.ReadOnly = true;
      this.txtCastTime.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtCastTime.RightToLeft")));
      this.txtCastTime.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtCastTime.ScrollBars")));
      this.txtCastTime.Size = ((System.Drawing.Size)(resources.GetObject("txtCastTime.Size")));
      this.txtCastTime.TabIndex = ((int)(resources.GetObject("txtCastTime.TabIndex")));
      this.txtCastTime.Text = resources.GetString("txtCastTime.Text");
      this.txtCastTime.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtCastTime.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtCastTime, resources.GetString("txtCastTime.ToolTip"));
      this.txtCastTime.Visible = ((bool)(resources.GetObject("txtCastTime.Visible")));
      this.txtCastTime.WordWrap = ((bool)(resources.GetObject("txtCastTime.WordWrap")));
      // 
      // lblEquipDelay
      // 
      this.lblEquipDelay.AccessibleDescription = resources.GetString("lblEquipDelay.AccessibleDescription");
      this.lblEquipDelay.AccessibleName = resources.GetString("lblEquipDelay.AccessibleName");
      this.lblEquipDelay.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblEquipDelay.Anchor")));
      this.lblEquipDelay.AutoSize = ((bool)(resources.GetObject("lblEquipDelay.AutoSize")));
      this.lblEquipDelay.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblEquipDelay.Dock")));
      this.lblEquipDelay.Enabled = ((bool)(resources.GetObject("lblEquipDelay.Enabled")));
      this.lblEquipDelay.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblEquipDelay.Font = ((System.Drawing.Font)(resources.GetObject("lblEquipDelay.Font")));
      this.lblEquipDelay.Image = ((System.Drawing.Image)(resources.GetObject("lblEquipDelay.Image")));
      this.lblEquipDelay.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblEquipDelay.ImageAlign")));
      this.lblEquipDelay.ImageIndex = ((int)(resources.GetObject("lblEquipDelay.ImageIndex")));
      this.lblEquipDelay.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblEquipDelay.ImeMode")));
      this.lblEquipDelay.Location = ((System.Drawing.Point)(resources.GetObject("lblEquipDelay.Location")));
      this.lblEquipDelay.Name = "lblEquipDelay";
      this.lblEquipDelay.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblEquipDelay.RightToLeft")));
      this.lblEquipDelay.Size = ((System.Drawing.Size)(resources.GetObject("lblEquipDelay.Size")));
      this.lblEquipDelay.TabIndex = ((int)(resources.GetObject("lblEquipDelay.TabIndex")));
      this.lblEquipDelay.Text = resources.GetString("lblEquipDelay.Text");
      this.lblEquipDelay.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblEquipDelay.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblEquipDelay, resources.GetString("lblEquipDelay.ToolTip"));
      this.lblEquipDelay.Visible = ((bool)(resources.GetObject("lblEquipDelay.Visible")));
      // 
      // lblReuseTimer
      // 
      this.lblReuseTimer.AccessibleDescription = resources.GetString("lblReuseTimer.AccessibleDescription");
      this.lblReuseTimer.AccessibleName = resources.GetString("lblReuseTimer.AccessibleName");
      this.lblReuseTimer.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblReuseTimer.Anchor")));
      this.lblReuseTimer.AutoSize = ((bool)(resources.GetObject("lblReuseTimer.AutoSize")));
      this.lblReuseTimer.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblReuseTimer.Dock")));
      this.lblReuseTimer.Enabled = ((bool)(resources.GetObject("lblReuseTimer.Enabled")));
      this.lblReuseTimer.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblReuseTimer.Font = ((System.Drawing.Font)(resources.GetObject("lblReuseTimer.Font")));
      this.lblReuseTimer.Image = ((System.Drawing.Image)(resources.GetObject("lblReuseTimer.Image")));
      this.lblReuseTimer.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblReuseTimer.ImageAlign")));
      this.lblReuseTimer.ImageIndex = ((int)(resources.GetObject("lblReuseTimer.ImageIndex")));
      this.lblReuseTimer.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblReuseTimer.ImeMode")));
      this.lblReuseTimer.Location = ((System.Drawing.Point)(resources.GetObject("lblReuseTimer.Location")));
      this.lblReuseTimer.Name = "lblReuseTimer";
      this.lblReuseTimer.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblReuseTimer.RightToLeft")));
      this.lblReuseTimer.Size = ((System.Drawing.Size)(resources.GetObject("lblReuseTimer.Size")));
      this.lblReuseTimer.TabIndex = ((int)(resources.GetObject("lblReuseTimer.TabIndex")));
      this.lblReuseTimer.Text = resources.GetString("lblReuseTimer.Text");
      this.lblReuseTimer.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblReuseTimer.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblReuseTimer, resources.GetString("lblReuseTimer.ToolTip"));
      this.lblReuseTimer.Visible = ((bool)(resources.GetObject("lblReuseTimer.Visible")));
      // 
      // lblMaxCharges
      // 
      this.lblMaxCharges.AccessibleDescription = resources.GetString("lblMaxCharges.AccessibleDescription");
      this.lblMaxCharges.AccessibleName = resources.GetString("lblMaxCharges.AccessibleName");
      this.lblMaxCharges.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblMaxCharges.Anchor")));
      this.lblMaxCharges.AutoSize = ((bool)(resources.GetObject("lblMaxCharges.AutoSize")));
      this.lblMaxCharges.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblMaxCharges.Dock")));
      this.lblMaxCharges.Enabled = ((bool)(resources.GetObject("lblMaxCharges.Enabled")));
      this.lblMaxCharges.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblMaxCharges.Font = ((System.Drawing.Font)(resources.GetObject("lblMaxCharges.Font")));
      this.lblMaxCharges.Image = ((System.Drawing.Image)(resources.GetObject("lblMaxCharges.Image")));
      this.lblMaxCharges.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblMaxCharges.ImageAlign")));
      this.lblMaxCharges.ImageIndex = ((int)(resources.GetObject("lblMaxCharges.ImageIndex")));
      this.lblMaxCharges.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblMaxCharges.ImeMode")));
      this.lblMaxCharges.Location = ((System.Drawing.Point)(resources.GetObject("lblMaxCharges.Location")));
      this.lblMaxCharges.Name = "lblMaxCharges";
      this.lblMaxCharges.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblMaxCharges.RightToLeft")));
      this.lblMaxCharges.Size = ((System.Drawing.Size)(resources.GetObject("lblMaxCharges.Size")));
      this.lblMaxCharges.TabIndex = ((int)(resources.GetObject("lblMaxCharges.TabIndex")));
      this.lblMaxCharges.Text = resources.GetString("lblMaxCharges.Text");
      this.lblMaxCharges.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblMaxCharges.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblMaxCharges, resources.GetString("lblMaxCharges.ToolTip"));
      this.lblMaxCharges.Visible = ((bool)(resources.GetObject("lblMaxCharges.Visible")));
      // 
      // txtReuseTimer
      // 
      this.txtReuseTimer.AccessibleDescription = resources.GetString("txtReuseTimer.AccessibleDescription");
      this.txtReuseTimer.AccessibleName = resources.GetString("txtReuseTimer.AccessibleName");
      this.txtReuseTimer.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtReuseTimer.Anchor")));
      this.txtReuseTimer.AutoSize = ((bool)(resources.GetObject("txtReuseTimer.AutoSize")));
      this.txtReuseTimer.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtReuseTimer.BackgroundImage")));
      this.txtReuseTimer.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtReuseTimer.Dock")));
      this.txtReuseTimer.Enabled = ((bool)(resources.GetObject("txtReuseTimer.Enabled")));
      this.txtReuseTimer.Font = ((System.Drawing.Font)(resources.GetObject("txtReuseTimer.Font")));
      this.txtReuseTimer.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtReuseTimer.ImeMode")));
      this.txtReuseTimer.Location = ((System.Drawing.Point)(resources.GetObject("txtReuseTimer.Location")));
      this.txtReuseTimer.MaxLength = ((int)(resources.GetObject("txtReuseTimer.MaxLength")));
      this.txtReuseTimer.Multiline = ((bool)(resources.GetObject("txtReuseTimer.Multiline")));
      this.txtReuseTimer.Name = "txtReuseTimer";
      this.txtReuseTimer.PasswordChar = ((char)(resources.GetObject("txtReuseTimer.PasswordChar")));
      this.txtReuseTimer.ReadOnly = true;
      this.txtReuseTimer.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtReuseTimer.RightToLeft")));
      this.txtReuseTimer.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtReuseTimer.ScrollBars")));
      this.txtReuseTimer.Size = ((System.Drawing.Size)(resources.GetObject("txtReuseTimer.Size")));
      this.txtReuseTimer.TabIndex = ((int)(resources.GetObject("txtReuseTimer.TabIndex")));
      this.txtReuseTimer.Text = resources.GetString("txtReuseTimer.Text");
      this.txtReuseTimer.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtReuseTimer.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtReuseTimer, resources.GetString("txtReuseTimer.ToolTip"));
      this.txtReuseTimer.Visible = ((bool)(resources.GetObject("txtReuseTimer.Visible")));
      this.txtReuseTimer.WordWrap = ((bool)(resources.GetObject("txtReuseTimer.WordWrap")));
      // 
      // txtEquipDelay
      // 
      this.txtEquipDelay.AccessibleDescription = resources.GetString("txtEquipDelay.AccessibleDescription");
      this.txtEquipDelay.AccessibleName = resources.GetString("txtEquipDelay.AccessibleName");
      this.txtEquipDelay.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtEquipDelay.Anchor")));
      this.txtEquipDelay.AutoSize = ((bool)(resources.GetObject("txtEquipDelay.AutoSize")));
      this.txtEquipDelay.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtEquipDelay.BackgroundImage")));
      this.txtEquipDelay.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtEquipDelay.Dock")));
      this.txtEquipDelay.Enabled = ((bool)(resources.GetObject("txtEquipDelay.Enabled")));
      this.txtEquipDelay.Font = ((System.Drawing.Font)(resources.GetObject("txtEquipDelay.Font")));
      this.txtEquipDelay.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtEquipDelay.ImeMode")));
      this.txtEquipDelay.Location = ((System.Drawing.Point)(resources.GetObject("txtEquipDelay.Location")));
      this.txtEquipDelay.MaxLength = ((int)(resources.GetObject("txtEquipDelay.MaxLength")));
      this.txtEquipDelay.Multiline = ((bool)(resources.GetObject("txtEquipDelay.Multiline")));
      this.txtEquipDelay.Name = "txtEquipDelay";
      this.txtEquipDelay.PasswordChar = ((char)(resources.GetObject("txtEquipDelay.PasswordChar")));
      this.txtEquipDelay.ReadOnly = true;
      this.txtEquipDelay.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtEquipDelay.RightToLeft")));
      this.txtEquipDelay.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtEquipDelay.ScrollBars")));
      this.txtEquipDelay.Size = ((System.Drawing.Size)(resources.GetObject("txtEquipDelay.Size")));
      this.txtEquipDelay.TabIndex = ((int)(resources.GetObject("txtEquipDelay.TabIndex")));
      this.txtEquipDelay.Text = resources.GetString("txtEquipDelay.Text");
      this.txtEquipDelay.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtEquipDelay.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtEquipDelay, resources.GetString("txtEquipDelay.ToolTip"));
      this.txtEquipDelay.Visible = ((bool)(resources.GetObject("txtEquipDelay.Visible")));
      this.txtEquipDelay.WordWrap = ((bool)(resources.GetObject("txtEquipDelay.WordWrap")));
      // 
      // txtMaxCharges
      // 
      this.txtMaxCharges.AccessibleDescription = resources.GetString("txtMaxCharges.AccessibleDescription");
      this.txtMaxCharges.AccessibleName = resources.GetString("txtMaxCharges.AccessibleName");
      this.txtMaxCharges.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtMaxCharges.Anchor")));
      this.txtMaxCharges.AutoSize = ((bool)(resources.GetObject("txtMaxCharges.AutoSize")));
      this.txtMaxCharges.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtMaxCharges.BackgroundImage")));
      this.txtMaxCharges.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtMaxCharges.Dock")));
      this.txtMaxCharges.Enabled = ((bool)(resources.GetObject("txtMaxCharges.Enabled")));
      this.txtMaxCharges.Font = ((System.Drawing.Font)(resources.GetObject("txtMaxCharges.Font")));
      this.txtMaxCharges.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtMaxCharges.ImeMode")));
      this.txtMaxCharges.Location = ((System.Drawing.Point)(resources.GetObject("txtMaxCharges.Location")));
      this.txtMaxCharges.MaxLength = ((int)(resources.GetObject("txtMaxCharges.MaxLength")));
      this.txtMaxCharges.Multiline = ((bool)(resources.GetObject("txtMaxCharges.Multiline")));
      this.txtMaxCharges.Name = "txtMaxCharges";
      this.txtMaxCharges.PasswordChar = ((char)(resources.GetObject("txtMaxCharges.PasswordChar")));
      this.txtMaxCharges.ReadOnly = true;
      this.txtMaxCharges.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtMaxCharges.RightToLeft")));
      this.txtMaxCharges.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtMaxCharges.ScrollBars")));
      this.txtMaxCharges.Size = ((System.Drawing.Size)(resources.GetObject("txtMaxCharges.Size")));
      this.txtMaxCharges.TabIndex = ((int)(resources.GetObject("txtMaxCharges.TabIndex")));
      this.txtMaxCharges.Text = resources.GetString("txtMaxCharges.Text");
      this.txtMaxCharges.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtMaxCharges.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtMaxCharges, resources.GetString("txtMaxCharges.ToolTip"));
      this.txtMaxCharges.Visible = ((bool)(resources.GetObject("txtMaxCharges.Visible")));
      this.txtMaxCharges.WordWrap = ((bool)(resources.GetObject("txtMaxCharges.WordWrap")));
      // 
      // grpLogStrings
      // 
      this.grpLogStrings.AccessibleDescription = resources.GetString("grpLogStrings.AccessibleDescription");
      this.grpLogStrings.AccessibleName = resources.GetString("grpLogStrings.AccessibleName");
      this.grpLogStrings.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("grpLogStrings.Anchor")));
      this.grpLogStrings.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpLogStrings.BackgroundImage")));
      this.grpLogStrings.Controls.Add(this.lblPlural);
      this.grpLogStrings.Controls.Add(this.lblSingular);
      this.grpLogStrings.Controls.Add(this.txtPlural);
      this.grpLogStrings.Controls.Add(this.txtSingular);
      this.grpLogStrings.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("grpLogStrings.Dock")));
      this.grpLogStrings.Enabled = ((bool)(resources.GetObject("grpLogStrings.Enabled")));
      this.grpLogStrings.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.grpLogStrings.Font = ((System.Drawing.Font)(resources.GetObject("grpLogStrings.Font")));
      this.grpLogStrings.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("grpLogStrings.ImeMode")));
      this.grpLogStrings.Location = ((System.Drawing.Point)(resources.GetObject("grpLogStrings.Location")));
      this.grpLogStrings.Name = "grpLogStrings";
      this.grpLogStrings.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("grpLogStrings.RightToLeft")));
      this.grpLogStrings.Size = ((System.Drawing.Size)(resources.GetObject("grpLogStrings.Size")));
      this.grpLogStrings.TabIndex = ((int)(resources.GetObject("grpLogStrings.TabIndex")));
      this.grpLogStrings.TabStop = false;
      this.grpLogStrings.Text = resources.GetString("grpLogStrings.Text");
      this.ttToolTip.SetToolTip(this.grpLogStrings, resources.GetString("grpLogStrings.ToolTip"));
      this.grpLogStrings.Visible = ((bool)(resources.GetObject("grpLogStrings.Visible")));
      // 
      // lblPlural
      // 
      this.lblPlural.AccessibleDescription = resources.GetString("lblPlural.AccessibleDescription");
      this.lblPlural.AccessibleName = resources.GetString("lblPlural.AccessibleName");
      this.lblPlural.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblPlural.Anchor")));
      this.lblPlural.AutoSize = ((bool)(resources.GetObject("lblPlural.AutoSize")));
      this.lblPlural.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblPlural.Dock")));
      this.lblPlural.Enabled = ((bool)(resources.GetObject("lblPlural.Enabled")));
      this.lblPlural.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblPlural.Font = ((System.Drawing.Font)(resources.GetObject("lblPlural.Font")));
      this.lblPlural.Image = ((System.Drawing.Image)(resources.GetObject("lblPlural.Image")));
      this.lblPlural.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblPlural.ImageAlign")));
      this.lblPlural.ImageIndex = ((int)(resources.GetObject("lblPlural.ImageIndex")));
      this.lblPlural.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblPlural.ImeMode")));
      this.lblPlural.Location = ((System.Drawing.Point)(resources.GetObject("lblPlural.Location")));
      this.lblPlural.Name = "lblPlural";
      this.lblPlural.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblPlural.RightToLeft")));
      this.lblPlural.Size = ((System.Drawing.Size)(resources.GetObject("lblPlural.Size")));
      this.lblPlural.TabIndex = ((int)(resources.GetObject("lblPlural.TabIndex")));
      this.lblPlural.Text = resources.GetString("lblPlural.Text");
      this.lblPlural.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblPlural.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblPlural, resources.GetString("lblPlural.ToolTip"));
      this.lblPlural.Visible = ((bool)(resources.GetObject("lblPlural.Visible")));
      // 
      // lblSingular
      // 
      this.lblSingular.AccessibleDescription = resources.GetString("lblSingular.AccessibleDescription");
      this.lblSingular.AccessibleName = resources.GetString("lblSingular.AccessibleName");
      this.lblSingular.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblSingular.Anchor")));
      this.lblSingular.AutoSize = ((bool)(resources.GetObject("lblSingular.AutoSize")));
      this.lblSingular.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblSingular.Dock")));
      this.lblSingular.Enabled = ((bool)(resources.GetObject("lblSingular.Enabled")));
      this.lblSingular.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblSingular.Font = ((System.Drawing.Font)(resources.GetObject("lblSingular.Font")));
      this.lblSingular.Image = ((System.Drawing.Image)(resources.GetObject("lblSingular.Image")));
      this.lblSingular.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblSingular.ImageAlign")));
      this.lblSingular.ImageIndex = ((int)(resources.GetObject("lblSingular.ImageIndex")));
      this.lblSingular.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblSingular.ImeMode")));
      this.lblSingular.Location = ((System.Drawing.Point)(resources.GetObject("lblSingular.Location")));
      this.lblSingular.Name = "lblSingular";
      this.lblSingular.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblSingular.RightToLeft")));
      this.lblSingular.Size = ((System.Drawing.Size)(resources.GetObject("lblSingular.Size")));
      this.lblSingular.TabIndex = ((int)(resources.GetObject("lblSingular.TabIndex")));
      this.lblSingular.Text = resources.GetString("lblSingular.Text");
      this.lblSingular.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblSingular.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblSingular, resources.GetString("lblSingular.ToolTip"));
      this.lblSingular.Visible = ((bool)(resources.GetObject("lblSingular.Visible")));
      // 
      // txtPlural
      // 
      this.txtPlural.AccessibleDescription = resources.GetString("txtPlural.AccessibleDescription");
      this.txtPlural.AccessibleName = resources.GetString("txtPlural.AccessibleName");
      this.txtPlural.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtPlural.Anchor")));
      this.txtPlural.AutoSize = ((bool)(resources.GetObject("txtPlural.AutoSize")));
      this.txtPlural.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtPlural.BackgroundImage")));
      this.txtPlural.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtPlural.Dock")));
      this.txtPlural.Enabled = ((bool)(resources.GetObject("txtPlural.Enabled")));
      this.txtPlural.Font = ((System.Drawing.Font)(resources.GetObject("txtPlural.Font")));
      this.txtPlural.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtPlural.ImeMode")));
      this.txtPlural.Location = ((System.Drawing.Point)(resources.GetObject("txtPlural.Location")));
      this.txtPlural.MaxLength = ((int)(resources.GetObject("txtPlural.MaxLength")));
      this.txtPlural.Multiline = ((bool)(resources.GetObject("txtPlural.Multiline")));
      this.txtPlural.Name = "txtPlural";
      this.txtPlural.PasswordChar = ((char)(resources.GetObject("txtPlural.PasswordChar")));
      this.txtPlural.ReadOnly = true;
      this.txtPlural.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtPlural.RightToLeft")));
      this.txtPlural.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtPlural.ScrollBars")));
      this.txtPlural.Size = ((System.Drawing.Size)(resources.GetObject("txtPlural.Size")));
      this.txtPlural.TabIndex = ((int)(resources.GetObject("txtPlural.TabIndex")));
      this.txtPlural.Text = resources.GetString("txtPlural.Text");
      this.txtPlural.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtPlural.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtPlural, resources.GetString("txtPlural.ToolTip"));
      this.txtPlural.Visible = ((bool)(resources.GetObject("txtPlural.Visible")));
      this.txtPlural.WordWrap = ((bool)(resources.GetObject("txtPlural.WordWrap")));
      // 
      // txtSingular
      // 
      this.txtSingular.AccessibleDescription = resources.GetString("txtSingular.AccessibleDescription");
      this.txtSingular.AccessibleName = resources.GetString("txtSingular.AccessibleName");
      this.txtSingular.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtSingular.Anchor")));
      this.txtSingular.AutoSize = ((bool)(resources.GetObject("txtSingular.AutoSize")));
      this.txtSingular.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtSingular.BackgroundImage")));
      this.txtSingular.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtSingular.Dock")));
      this.txtSingular.Enabled = ((bool)(resources.GetObject("txtSingular.Enabled")));
      this.txtSingular.Font = ((System.Drawing.Font)(resources.GetObject("txtSingular.Font")));
      this.txtSingular.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtSingular.ImeMode")));
      this.txtSingular.Location = ((System.Drawing.Point)(resources.GetObject("txtSingular.Location")));
      this.txtSingular.MaxLength = ((int)(resources.GetObject("txtSingular.MaxLength")));
      this.txtSingular.Multiline = ((bool)(resources.GetObject("txtSingular.Multiline")));
      this.txtSingular.Name = "txtSingular";
      this.txtSingular.PasswordChar = ((char)(resources.GetObject("txtSingular.PasswordChar")));
      this.txtSingular.ReadOnly = true;
      this.txtSingular.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtSingular.RightToLeft")));
      this.txtSingular.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtSingular.ScrollBars")));
      this.txtSingular.Size = ((System.Drawing.Size)(resources.GetObject("txtSingular.Size")));
      this.txtSingular.TabIndex = ((int)(resources.GetObject("txtSingular.TabIndex")));
      this.txtSingular.Text = resources.GetString("txtSingular.Text");
      this.txtSingular.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtSingular.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtSingular, resources.GetString("txtSingular.ToolTip"));
      this.txtSingular.Visible = ((bool)(resources.GetObject("txtSingular.Visible")));
      this.txtSingular.WordWrap = ((bool)(resources.GetObject("txtSingular.WordWrap")));
      // 
      // FFXIItemEditor
      // 
      this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
      this.AccessibleName = resources.GetString("$this.AccessibleName");
      this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
      this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
      this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
      this.BackColor = System.Drawing.SystemColors.Control;
      this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
      this.Controls.Add(this.grpLogStrings);
      this.Controls.Add(this.grpEquipmentInfo);
      this.Controls.Add(this.grpEnchantmentInfo);
      this.Controls.Add(this.grpJugInfo);
      this.Controls.Add(this.grpShieldInfo);
      this.Controls.Add(this.grpFurnitureInfo);
      this.Controls.Add(this.grpWeaponInfo);
      this.Controls.Add(this.grpViewMode);
      this.Controls.Add(this.grpCommonInfo);
      this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
      this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
      this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
      this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
      this.Name = "FFXIItemEditor";
      this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
      this.Size = ((System.Drawing.Size)(resources.GetObject("$this.Size")));
      this.ttToolTip.SetToolTip(this, resources.GetString("$this.ToolTip"));
      this.grpViewMode.ResumeLayout(false);
      this.grpEquipmentInfo.ResumeLayout(false);
      this.grpCommonInfo.ResumeLayout(false);
      this.grpWeaponInfo.ResumeLayout(false);
      this.grpFurnitureInfo.ResumeLayout(false);
      this.grpShieldInfo.ResumeLayout(false);
      this.grpJugInfo.ResumeLayout(false);
      this.grpEnchantmentInfo.ResumeLayout(false);
      this.grpLogStrings.ResumeLayout(false);
      this.ResumeLayout(false);

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
