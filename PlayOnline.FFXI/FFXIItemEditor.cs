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

    #region Controls

    private System.Windows.Forms.GroupBox grpViewMode;
    private System.Windows.Forms.CheckBox chkViewAsJWeapon;
    private System.Windows.Forms.CheckBox chkViewAsJObject;
    private System.Windows.Forms.CheckBox chkViewAsJArmor;
    private System.Windows.Forms.CheckBox chkViewAsEWeapon;
    private System.Windows.Forms.CheckBox chkViewAsEObject;
    private System.Windows.Forms.CheckBox chkViewAsEArmor;
    private System.Windows.Forms.GroupBox grpSpecializedInfo;
    private System.Windows.Forms.Label lblRaces;
    private System.Windows.Forms.TextBox txtRaces;
    private System.Windows.Forms.Label lblSlots;
    private System.Windows.Forms.TextBox txtSlots;
    private System.Windows.Forms.Label lblJobs;
    private System.Windows.Forms.TextBox txtJobs;
    private System.Windows.Forms.Label lblLevel;
    private System.Windows.Forms.TextBox txtLevel;
    private System.Windows.Forms.Label lblEquipDelay;
    private System.Windows.Forms.Label lblReuseTimer;
    private System.Windows.Forms.Label lblMaxCharges;
    private System.Windows.Forms.Label lblResourceID;
    private System.Windows.Forms.TextBox txtReuseTimer;
    private System.Windows.Forms.TextBox txtEquipDelay;
    private System.Windows.Forms.TextBox txtMaxCharges;
    private System.Windows.Forms.TextBox txtResourceID;
    private System.Windows.Forms.Label lblSkill;
    private System.Windows.Forms.Label lblDelay;
    private System.Windows.Forms.Label lblDamage;
    private System.Windows.Forms.TextBox txtSkill;
    private System.Windows.Forms.TextBox txtDelay;
    private System.Windows.Forms.TextBox txtDamage;
    private System.Windows.Forms.Label lblShieldSize;
    private System.Windows.Forms.TextBox txtShieldSize;
    private System.Windows.Forms.GroupBox grpCommonInfo;
    private System.Windows.Forms.Label lblDescription;
    private System.Windows.Forms.Label lblPlural;
    private System.Windows.Forms.Label lblSingular;
    private System.Windows.Forms.Label lblJName;
    private System.Windows.Forms.Label lblEName;
    private System.Windows.Forms.TextBox txtPlural;
    private System.Windows.Forms.TextBox txtSingular;
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
    private System.Windows.Forms.Label lblCastTime;
    private System.Windows.Forms.TextBox txtCastTime;
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
	case ItemField.Damage:          return this.txtDamage;
	case ItemField.Delay:           return this.txtDelay;
	case ItemField.Description:     return this.txtDescription;
	case ItemField.EnglishName:     return this.txtEName;
	case ItemField.EquipDelay:      return this.txtEquipDelay;
	case ItemField.Flags:           return this.txtFlags;
	case ItemField.ID:              return this.txtID;
	case ItemField.JapaneseName:    return this.txtJName;
	case ItemField.Jobs:            return this.txtJobs;
	case ItemField.Level:           return this.txtLevel;
	case ItemField.LogNamePlural:   return this.txtPlural;
	case ItemField.LogNameSingular: return this.txtSingular;
	case ItemField.MaxCharges:      return this.txtMaxCharges;
	case ItemField.Races:           return this.txtRaces;
	case ItemField.ResourceID:      return this.txtResourceID;
	case ItemField.ReuseTimer:      return this.txtReuseTimer;
	case ItemField.ShieldSize:      return this.txtShieldSize;
	case ItemField.Skill:           return this.txtSkill;
	case ItemField.Slots:           return this.txtSlots;
	case ItemField.StackSize:       return this.txtStackSize;
	case ItemField.Type:            return this.txtType;
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

    private void DetectViewMode(FFXIItem I) {
    CheckBox SelectedMode = this.chkViewAsEObject;
      switch (I.Common.Type) {
	case ItemType.Armor: {
	  if (I.ENArmor.Description != String.Empty && I.JPArmor.Description == String.Empty)
	    SelectedMode = this.chkViewAsEArmor;
	  else if (I.JPArmor.Description != String.Empty && I.ENArmor.Description == String.Empty)
	    SelectedMode = this.chkViewAsJArmor;
	  else {
	  string LogNames = I.ENArmor.LogNameSingular + I.ENArmor.LogNamePlural;
	    if (LogNames == String.Empty)
	      SelectedMode = this.chkViewAsJArmor;
	    else {
	    bool ExtendedCharSeen = false;
	      foreach (char C in LogNames) {
		if ((int) C > 0x100) {
		  ExtendedCharSeen = true;
		  break;
		}
	      }
	      if (ExtendedCharSeen)
		SelectedMode = this.chkViewAsJArmor;
	      else
		SelectedMode = this.chkViewAsEArmor;
	    }
	  }
	  break;
	}
	case ItemType.Weapon: {
	  if (I.ENWeapon.Description != String.Empty && I.JPWeapon.Description == String.Empty)
	    SelectedMode = this.chkViewAsEWeapon;
	  else if (I.JPWeapon.Description != String.Empty && I.ENWeapon.Description == String.Empty)
	    SelectedMode = this.chkViewAsJWeapon;
	  else {
	  string LogNames = I.ENWeapon.LogNameSingular + I.ENWeapon.LogNamePlural;
	    if (LogNames == String.Empty)
	      SelectedMode = this.chkViewAsJWeapon;
	    else {
	    bool ExtendedCharSeen = false;
	      foreach (char C in LogNames) {
		if ((int) C > 0x100) {
		  ExtendedCharSeen = true;
		  break;
		}
	      }
	      if (ExtendedCharSeen)
		SelectedMode = this.chkViewAsJWeapon;
	      else
		SelectedMode = this.chkViewAsEWeapon;
	    }
	  }
	  break;
	}
	default: {
	  if (I.ENObject.Description != String.Empty && I.JPObject.Description == String.Empty)
	    SelectedMode = this.chkViewAsEObject;
	  else if (I.JPObject.Description != String.Empty && I.ENObject.Description == String.Empty)
	    SelectedMode = this.chkViewAsJObject;
	  else {
	  string LogNames = I.ENObject.LogNameSingular + I.ENObject.LogNamePlural;
	    if (LogNames == String.Empty)
	      SelectedMode = this.chkViewAsJObject;
	    else {
	    bool ExtendedCharSeen = false;
	      foreach (char C in LogNames) {
		if ((int) C > 0x100) {
		  ExtendedCharSeen = true;
		  break;
		}
	      }
	      if (ExtendedCharSeen)
		SelectedMode = this.chkViewAsJObject;
	      else
		SelectedMode = this.chkViewAsEObject;
	    }
	  }
	  break;
	}
      }
      SelectedMode.Checked = true;
    }

    private void ShowItem() {
    FFXIItem I = this.ItemToShow_;
      this.chkViewAsEArmor.Checked = this.chkViewAsEObject.Checked = this.chkViewAsEWeapon.Checked = false;
      this.chkViewAsJArmor.Checked = this.chkViewAsJObject.Checked = this.chkViewAsJWeapon.Checked = false;
      if (I != null) {
	// Common Info
	this.picIcon.Image     = I.IconGraphic.Bitmap;
	this.ttToolTip.SetToolTip(this.picIcon, I.IconGraphic.ToString());
	this.txtID.Text        = String.Format("{0:X4} ({0})", I.Common.ID);
	this.txtType.Text      = String.Format("{0:X} ({0})",  I.Common.Type);
	this.txtFlags.Text     = String.Format("{0:X} ({0})",  I.Common.Flags);
	this.txtStackSize.Text = String.Format("{0}",          I.Common.StackSize);
	if (this.LockedViewMode_ != null)
	  this.LockedViewMode_.Checked = true;
	else
	  this.DetectViewMode(I);
      }
      else { // Clear all fields
	this.picIcon.Image = null;
	this.ttToolTip.SetToolTip(this.picIcon, null);
	this.txtID.Text          = this.txtType.Text      = String.Empty;
	this.txtEName.Text       = this.txtJName.Text     = String.Empty;
	this.txtSingular.Text    = this.txtPlural.Text    = String.Empty;
	this.txtFlags.Text       = this.txtStackSize.Text = String.Empty;
	this.txtDescription.Text = String.Empty;
	this.grpSpecializedInfo.Visible = false;
      }
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
      this.grpViewMode = new System.Windows.Forms.GroupBox();
      this.chkViewAsJWeapon = new System.Windows.Forms.CheckBox();
      this.chkViewAsJObject = new System.Windows.Forms.CheckBox();
      this.chkViewAsJArmor = new System.Windows.Forms.CheckBox();
      this.chkViewAsEWeapon = new System.Windows.Forms.CheckBox();
      this.chkViewAsEObject = new System.Windows.Forms.CheckBox();
      this.chkViewAsEArmor = new System.Windows.Forms.CheckBox();
      this.grpSpecializedInfo = new System.Windows.Forms.GroupBox();
      this.lblCastTime = new System.Windows.Forms.Label();
      this.txtCastTime = new System.Windows.Forms.TextBox();
      this.lblRaces = new System.Windows.Forms.Label();
      this.txtRaces = new System.Windows.Forms.TextBox();
      this.lblSlots = new System.Windows.Forms.Label();
      this.txtSlots = new System.Windows.Forms.TextBox();
      this.lblJobs = new System.Windows.Forms.Label();
      this.txtJobs = new System.Windows.Forms.TextBox();
      this.lblLevel = new System.Windows.Forms.Label();
      this.txtLevel = new System.Windows.Forms.TextBox();
      this.lblEquipDelay = new System.Windows.Forms.Label();
      this.lblReuseTimer = new System.Windows.Forms.Label();
      this.lblMaxCharges = new System.Windows.Forms.Label();
      this.lblResourceID = new System.Windows.Forms.Label();
      this.txtReuseTimer = new System.Windows.Forms.TextBox();
      this.txtEquipDelay = new System.Windows.Forms.TextBox();
      this.txtMaxCharges = new System.Windows.Forms.TextBox();
      this.txtResourceID = new System.Windows.Forms.TextBox();
      this.lblSkill = new System.Windows.Forms.Label();
      this.lblDelay = new System.Windows.Forms.Label();
      this.lblDamage = new System.Windows.Forms.Label();
      this.txtSkill = new System.Windows.Forms.TextBox();
      this.txtDelay = new System.Windows.Forms.TextBox();
      this.txtDamage = new System.Windows.Forms.TextBox();
      this.lblShieldSize = new System.Windows.Forms.Label();
      this.txtShieldSize = new System.Windows.Forms.TextBox();
      this.grpCommonInfo = new System.Windows.Forms.GroupBox();
      this.lblDescription = new System.Windows.Forms.Label();
      this.lblPlural = new System.Windows.Forms.Label();
      this.lblSingular = new System.Windows.Forms.Label();
      this.lblJName = new System.Windows.Forms.Label();
      this.lblEName = new System.Windows.Forms.Label();
      this.txtPlural = new System.Windows.Forms.TextBox();
      this.txtSingular = new System.Windows.Forms.TextBox();
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
      this.grpViewMode.SuspendLayout();
      this.grpSpecializedInfo.SuspendLayout();
      this.grpCommonInfo.SuspendLayout();
      this.SuspendLayout();
      // 
      // grpViewMode
      // 
      this.grpViewMode.Controls.Add(this.chkViewAsJWeapon);
      this.grpViewMode.Controls.Add(this.chkViewAsJObject);
      this.grpViewMode.Controls.Add(this.chkViewAsJArmor);
      this.grpViewMode.Controls.Add(this.chkViewAsEWeapon);
      this.grpViewMode.Controls.Add(this.chkViewAsEObject);
      this.grpViewMode.Controls.Add(this.chkViewAsEArmor);
      this.grpViewMode.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.grpViewMode.Location = new System.Drawing.Point(0, 0);
      this.grpViewMode.Name = "grpViewMode";
      this.grpViewMode.Size = new System.Drawing.Size(424, 76);
      this.grpViewMode.TabIndex = 4;
      this.grpViewMode.TabStop = false;
      this.grpViewMode.Text = "View Item Info As";
      // 
      // chkViewAsJWeapon
      // 
      this.chkViewAsJWeapon.Appearance = System.Windows.Forms.Appearance.Button;
      this.chkViewAsJWeapon.AutoCheck = false;
      this.chkViewAsJWeapon.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.chkViewAsJWeapon.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.chkViewAsJWeapon.Location = new System.Drawing.Point(276, 44);
      this.chkViewAsJWeapon.Name = "chkViewAsJWeapon";
      this.chkViewAsJWeapon.Size = new System.Drawing.Size(140, 24);
      this.chkViewAsJWeapon.TabIndex = 6;
      this.chkViewAsJWeapon.Text = "Weapon Information - JP";
      this.chkViewAsJWeapon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.chkViewAsJWeapon.Click += new System.EventHandler(this.chkView_Click);
      this.chkViewAsJWeapon.CheckedChanged += new System.EventHandler(this.chkViewAsJWeapon_CheckedChanged);
      // 
      // chkViewAsJObject
      // 
      this.chkViewAsJObject.Appearance = System.Windows.Forms.Appearance.Button;
      this.chkViewAsJObject.AutoCheck = false;
      this.chkViewAsJObject.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.chkViewAsJObject.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.chkViewAsJObject.Location = new System.Drawing.Point(140, 44);
      this.chkViewAsJObject.Name = "chkViewAsJObject";
      this.chkViewAsJObject.Size = new System.Drawing.Size(132, 24);
      this.chkViewAsJObject.TabIndex = 5;
      this.chkViewAsJObject.Text = "Object Information - JP";
      this.chkViewAsJObject.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.chkViewAsJObject.Click += new System.EventHandler(this.chkView_Click);
      this.chkViewAsJObject.CheckedChanged += new System.EventHandler(this.chkViewAsJObject_CheckedChanged);
      // 
      // chkViewAsJArmor
      // 
      this.chkViewAsJArmor.Appearance = System.Windows.Forms.Appearance.Button;
      this.chkViewAsJArmor.AutoCheck = false;
      this.chkViewAsJArmor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.chkViewAsJArmor.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.chkViewAsJArmor.Location = new System.Drawing.Point(8, 44);
      this.chkViewAsJArmor.Name = "chkViewAsJArmor";
      this.chkViewAsJArmor.Size = new System.Drawing.Size(128, 24);
      this.chkViewAsJArmor.TabIndex = 4;
      this.chkViewAsJArmor.Text = "Armor Information - JP";
      this.chkViewAsJArmor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.chkViewAsJArmor.Click += new System.EventHandler(this.chkView_Click);
      this.chkViewAsJArmor.CheckedChanged += new System.EventHandler(this.chkViewAsJArmor_CheckedChanged);
      // 
      // chkViewAsEWeapon
      // 
      this.chkViewAsEWeapon.Appearance = System.Windows.Forms.Appearance.Button;
      this.chkViewAsEWeapon.AutoCheck = false;
      this.chkViewAsEWeapon.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.chkViewAsEWeapon.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.chkViewAsEWeapon.Location = new System.Drawing.Point(276, 16);
      this.chkViewAsEWeapon.Name = "chkViewAsEWeapon";
      this.chkViewAsEWeapon.Size = new System.Drawing.Size(140, 24);
      this.chkViewAsEWeapon.TabIndex = 3;
      this.chkViewAsEWeapon.Text = "Weapon Information - EN";
      this.chkViewAsEWeapon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.chkViewAsEWeapon.Click += new System.EventHandler(this.chkView_Click);
      this.chkViewAsEWeapon.CheckedChanged += new System.EventHandler(this.chkViewAsEWeapon_CheckedChanged);
      // 
      // chkViewAsEObject
      // 
      this.chkViewAsEObject.Appearance = System.Windows.Forms.Appearance.Button;
      this.chkViewAsEObject.AutoCheck = false;
      this.chkViewAsEObject.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.chkViewAsEObject.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.chkViewAsEObject.Location = new System.Drawing.Point(140, 16);
      this.chkViewAsEObject.Name = "chkViewAsEObject";
      this.chkViewAsEObject.Size = new System.Drawing.Size(132, 24);
      this.chkViewAsEObject.TabIndex = 2;
      this.chkViewAsEObject.Text = "Object Information - EN";
      this.chkViewAsEObject.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.chkViewAsEObject.Click += new System.EventHandler(this.chkView_Click);
      this.chkViewAsEObject.CheckedChanged += new System.EventHandler(this.chkViewAsEObject_CheckedChanged);
      // 
      // chkViewAsEArmor
      // 
      this.chkViewAsEArmor.Appearance = System.Windows.Forms.Appearance.Button;
      this.chkViewAsEArmor.AutoCheck = false;
      this.chkViewAsEArmor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.chkViewAsEArmor.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.chkViewAsEArmor.Location = new System.Drawing.Point(8, 16);
      this.chkViewAsEArmor.Name = "chkViewAsEArmor";
      this.chkViewAsEArmor.Size = new System.Drawing.Size(128, 24);
      this.chkViewAsEArmor.TabIndex = 1;
      this.chkViewAsEArmor.Text = "Armor Information - EN";
      this.chkViewAsEArmor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.chkViewAsEArmor.Click += new System.EventHandler(this.chkView_Click);
      this.chkViewAsEArmor.CheckedChanged += new System.EventHandler(this.chkViewAsEArmor_CheckedChanged);
      // 
      // grpSpecializedInfo
      // 
      this.grpSpecializedInfo.Controls.Add(this.lblCastTime);
      this.grpSpecializedInfo.Controls.Add(this.txtCastTime);
      this.grpSpecializedInfo.Controls.Add(this.lblRaces);
      this.grpSpecializedInfo.Controls.Add(this.txtRaces);
      this.grpSpecializedInfo.Controls.Add(this.lblSlots);
      this.grpSpecializedInfo.Controls.Add(this.txtSlots);
      this.grpSpecializedInfo.Controls.Add(this.lblJobs);
      this.grpSpecializedInfo.Controls.Add(this.txtJobs);
      this.grpSpecializedInfo.Controls.Add(this.lblLevel);
      this.grpSpecializedInfo.Controls.Add(this.txtLevel);
      this.grpSpecializedInfo.Controls.Add(this.lblEquipDelay);
      this.grpSpecializedInfo.Controls.Add(this.lblReuseTimer);
      this.grpSpecializedInfo.Controls.Add(this.lblMaxCharges);
      this.grpSpecializedInfo.Controls.Add(this.lblResourceID);
      this.grpSpecializedInfo.Controls.Add(this.txtReuseTimer);
      this.grpSpecializedInfo.Controls.Add(this.txtEquipDelay);
      this.grpSpecializedInfo.Controls.Add(this.txtMaxCharges);
      this.grpSpecializedInfo.Controls.Add(this.txtResourceID);
      this.grpSpecializedInfo.Controls.Add(this.lblSkill);
      this.grpSpecializedInfo.Controls.Add(this.lblDelay);
      this.grpSpecializedInfo.Controls.Add(this.lblDamage);
      this.grpSpecializedInfo.Controls.Add(this.txtSkill);
      this.grpSpecializedInfo.Controls.Add(this.txtDelay);
      this.grpSpecializedInfo.Controls.Add(this.txtDamage);
      this.grpSpecializedInfo.Controls.Add(this.lblShieldSize);
      this.grpSpecializedInfo.Controls.Add(this.txtShieldSize);
      this.grpSpecializedInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.grpSpecializedInfo.Location = new System.Drawing.Point(0, 364);
      this.grpSpecializedInfo.Name = "grpSpecializedInfo";
      this.grpSpecializedInfo.Size = new System.Drawing.Size(424, 122);
      this.grpSpecializedInfo.TabIndex = 6;
      this.grpSpecializedInfo.TabStop = false;
      this.grpSpecializedInfo.Text = "Specialized Information";
      this.grpSpecializedInfo.Visible = false;
      // 
      // lblCastTime
      // 
      this.lblCastTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblCastTime.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblCastTime.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblCastTime.Location = new System.Drawing.Point(84, 96);
      this.lblCastTime.Name = "lblCastTime";
      this.lblCastTime.Size = new System.Drawing.Size(64, 16);
      this.lblCastTime.TabIndex = 40;
      this.lblCastTime.Text = "Casting Time:";
      // 
      // txtCastTime
      // 
      this.txtCastTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.txtCastTime.Location = new System.Drawing.Point(152, 94);
      this.txtCastTime.Name = "txtCastTime";
      this.txtCastTime.ReadOnly = true;
      this.txtCastTime.Size = new System.Drawing.Size(34, 20);
      this.txtCastTime.TabIndex = 39;
      this.txtCastTime.Text = "";
      // 
      // lblRaces
      // 
      this.lblRaces.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblRaces.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblRaces.Location = new System.Drawing.Point(264, 48);
      this.lblRaces.Name = "lblRaces";
      this.lblRaces.Size = new System.Drawing.Size(36, 16);
      this.lblRaces.TabIndex = 38;
      this.lblRaces.Text = "Races:";
      // 
      // txtRaces
      // 
      this.txtRaces.Location = new System.Drawing.Point(304, 44);
      this.txtRaces.Name = "txtRaces";
      this.txtRaces.ReadOnly = true;
      this.txtRaces.Size = new System.Drawing.Size(112, 20);
      this.txtRaces.TabIndex = 37;
      this.txtRaces.Text = "";
      // 
      // lblSlots
      // 
      this.lblSlots.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblSlots.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblSlots.Location = new System.Drawing.Point(228, 24);
      this.lblSlots.Name = "lblSlots";
      this.lblSlots.Size = new System.Drawing.Size(32, 16);
      this.lblSlots.TabIndex = 36;
      this.lblSlots.Text = "Slots:";
      // 
      // txtSlots
      // 
      this.txtSlots.Location = new System.Drawing.Point(260, 20);
      this.txtSlots.Name = "txtSlots";
      this.txtSlots.ReadOnly = true;
      this.txtSlots.Size = new System.Drawing.Size(156, 20);
      this.txtSlots.TabIndex = 35;
      this.txtSlots.Text = "";
      // 
      // lblJobs
      // 
      this.lblJobs.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblJobs.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblJobs.Location = new System.Drawing.Point(8, 48);
      this.lblJobs.Name = "lblJobs";
      this.lblJobs.Size = new System.Drawing.Size(32, 16);
      this.lblJobs.TabIndex = 34;
      this.lblJobs.Text = "Jobs:";
      // 
      // txtJobs
      // 
      this.txtJobs.Location = new System.Drawing.Point(40, 44);
      this.txtJobs.Name = "txtJobs";
      this.txtJobs.ReadOnly = true;
      this.txtJobs.Size = new System.Drawing.Size(216, 20);
      this.txtJobs.TabIndex = 33;
      this.txtJobs.Text = "";
      // 
      // lblLevel
      // 
      this.lblLevel.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblLevel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblLevel.Location = new System.Drawing.Point(156, 24);
      this.lblLevel.Name = "lblLevel";
      this.lblLevel.Size = new System.Drawing.Size(32, 16);
      this.lblLevel.TabIndex = 32;
      this.lblLevel.Text = "Level:";
      // 
      // txtLevel
      // 
      this.txtLevel.Location = new System.Drawing.Point(192, 20);
      this.txtLevel.Name = "txtLevel";
      this.txtLevel.ReadOnly = true;
      this.txtLevel.Size = new System.Drawing.Size(28, 20);
      this.txtLevel.TabIndex = 31;
      this.txtLevel.Text = "";
      // 
      // lblEquipDelay
      // 
      this.lblEquipDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblEquipDelay.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblEquipDelay.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblEquipDelay.Location = new System.Drawing.Point(192, 96);
      this.lblEquipDelay.Name = "lblEquipDelay";
      this.lblEquipDelay.Size = new System.Drawing.Size(60, 16);
      this.lblEquipDelay.TabIndex = 25;
      this.lblEquipDelay.Text = "Equip Delay:";
      // 
      // lblReuseTimer
      // 
      this.lblReuseTimer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblReuseTimer.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblReuseTimer.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblReuseTimer.Location = new System.Drawing.Point(312, 96);
      this.lblReuseTimer.Name = "lblReuseTimer";
      this.lblReuseTimer.Size = new System.Drawing.Size(40, 16);
      this.lblReuseTimer.TabIndex = 24;
      this.lblReuseTimer.Text = "Re-Use:";
      // 
      // lblMaxCharges
      // 
      this.lblMaxCharges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblMaxCharges.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblMaxCharges.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblMaxCharges.Location = new System.Drawing.Point(8, 96);
      this.lblMaxCharges.Name = "lblMaxCharges";
      this.lblMaxCharges.Size = new System.Drawing.Size(48, 16);
      this.lblMaxCharges.TabIndex = 23;
      this.lblMaxCharges.Text = "Charges:";
      // 
      // lblResourceID
      // 
      this.lblResourceID.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblResourceID.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblResourceID.Location = new System.Drawing.Point(8, 24);
      this.lblResourceID.Name = "lblResourceID";
      this.lblResourceID.Size = new System.Drawing.Size(68, 16);
      this.lblResourceID.TabIndex = 22;
      this.lblResourceID.Text = "Resource ID:";
      // 
      // txtReuseTimer
      // 
      this.txtReuseTimer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.txtReuseTimer.Location = new System.Drawing.Point(356, 94);
      this.txtReuseTimer.Name = "txtReuseTimer";
      this.txtReuseTimer.ReadOnly = true;
      this.txtReuseTimer.Size = new System.Drawing.Size(60, 20);
      this.txtReuseTimer.TabIndex = 21;
      this.txtReuseTimer.Text = "";
      // 
      // txtEquipDelay
      // 
      this.txtEquipDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.txtEquipDelay.Location = new System.Drawing.Point(256, 94);
      this.txtEquipDelay.Name = "txtEquipDelay";
      this.txtEquipDelay.ReadOnly = true;
      this.txtEquipDelay.Size = new System.Drawing.Size(48, 20);
      this.txtEquipDelay.TabIndex = 20;
      this.txtEquipDelay.Text = "";
      // 
      // txtMaxCharges
      // 
      this.txtMaxCharges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.txtMaxCharges.Location = new System.Drawing.Point(56, 94);
      this.txtMaxCharges.Name = "txtMaxCharges";
      this.txtMaxCharges.ReadOnly = true;
      this.txtMaxCharges.Size = new System.Drawing.Size(24, 20);
      this.txtMaxCharges.TabIndex = 19;
      this.txtMaxCharges.Text = "";
      // 
      // txtResourceID
      // 
      this.txtResourceID.Location = new System.Drawing.Point(76, 20);
      this.txtResourceID.Name = "txtResourceID";
      this.txtResourceID.ReadOnly = true;
      this.txtResourceID.Size = new System.Drawing.Size(72, 20);
      this.txtResourceID.TabIndex = 15;
      this.txtResourceID.Text = "";
      // 
      // lblSkill
      // 
      this.lblSkill.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblSkill.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblSkill.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblSkill.Location = new System.Drawing.Point(190, 72);
      this.lblSkill.Name = "lblSkill";
      this.lblSkill.Size = new System.Drawing.Size(68, 16);
      this.lblSkill.TabIndex = 28;
      this.lblSkill.Text = "Weapon Skill:";
      this.lblSkill.Visible = false;
      // 
      // lblDelay
      // 
      this.lblDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblDelay.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblDelay.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblDelay.Location = new System.Drawing.Point(90, 72);
      this.lblDelay.Name = "lblDelay";
      this.lblDelay.Size = new System.Drawing.Size(32, 16);
      this.lblDelay.TabIndex = 27;
      this.lblDelay.Text = "Delay:";
      this.lblDelay.Visible = false;
      // 
      // lblDamage
      // 
      this.lblDamage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblDamage.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblDamage.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblDamage.Location = new System.Drawing.Point(8, 72);
      this.lblDamage.Name = "lblDamage";
      this.lblDamage.Size = new System.Drawing.Size(46, 16);
      this.lblDamage.TabIndex = 26;
      this.lblDamage.Text = "Damage:";
      this.lblDamage.Visible = false;
      // 
      // txtSkill
      // 
      this.txtSkill.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.txtSkill.Location = new System.Drawing.Point(260, 68);
      this.txtSkill.Name = "txtSkill";
      this.txtSkill.ReadOnly = true;
      this.txtSkill.Size = new System.Drawing.Size(156, 20);
      this.txtSkill.TabIndex = 18;
      this.txtSkill.Text = "";
      this.txtSkill.Visible = false;
      // 
      // txtDelay
      // 
      this.txtDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.txtDelay.Location = new System.Drawing.Point(124, 68);
      this.txtDelay.Name = "txtDelay";
      this.txtDelay.ReadOnly = true;
      this.txtDelay.Size = new System.Drawing.Size(60, 20);
      this.txtDelay.TabIndex = 17;
      this.txtDelay.Text = "";
      this.txtDelay.Visible = false;
      // 
      // txtDamage
      // 
      this.txtDamage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.txtDamage.Location = new System.Drawing.Point(56, 68);
      this.txtDamage.Name = "txtDamage";
      this.txtDamage.ReadOnly = true;
      this.txtDamage.Size = new System.Drawing.Size(28, 20);
      this.txtDamage.TabIndex = 16;
      this.txtDamage.Text = "";
      this.txtDamage.Visible = false;
      // 
      // lblShieldSize
      // 
      this.lblShieldSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblShieldSize.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblShieldSize.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblShieldSize.Location = new System.Drawing.Point(8, 72);
      this.lblShieldSize.Name = "lblShieldSize";
      this.lblShieldSize.Size = new System.Drawing.Size(56, 16);
      this.lblShieldSize.TabIndex = 30;
      this.lblShieldSize.Text = "Shield Size:";
      this.lblShieldSize.Visible = false;
      // 
      // txtShieldSize
      // 
      this.txtShieldSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.txtShieldSize.Location = new System.Drawing.Point(68, 68);
      this.txtShieldSize.Name = "txtShieldSize";
      this.txtShieldSize.ReadOnly = true;
      this.txtShieldSize.Size = new System.Drawing.Size(32, 20);
      this.txtShieldSize.TabIndex = 29;
      this.txtShieldSize.Text = "";
      this.txtShieldSize.Visible = false;
      // 
      // grpCommonInfo
      // 
      this.grpCommonInfo.Controls.Add(this.lblDescription);
      this.grpCommonInfo.Controls.Add(this.lblPlural);
      this.grpCommonInfo.Controls.Add(this.lblSingular);
      this.grpCommonInfo.Controls.Add(this.lblJName);
      this.grpCommonInfo.Controls.Add(this.lblEName);
      this.grpCommonInfo.Controls.Add(this.txtPlural);
      this.grpCommonInfo.Controls.Add(this.txtSingular);
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
      this.grpCommonInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.grpCommonInfo.Location = new System.Drawing.Point(0, 76);
      this.grpCommonInfo.Name = "grpCommonInfo";
      this.grpCommonInfo.Size = new System.Drawing.Size(424, 284);
      this.grpCommonInfo.TabIndex = 5;
      this.grpCommonInfo.TabStop = false;
      this.grpCommonInfo.Text = "Common Information";
      // 
      // lblDescription
      // 
      this.lblDescription.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblDescription.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblDescription.Location = new System.Drawing.Point(8, 144);
      this.lblDescription.Name = "lblDescription";
      this.lblDescription.Size = new System.Drawing.Size(60, 16);
      this.lblDescription.TabIndex = 0;
      this.lblDescription.Text = "Description:";
      // 
      // lblPlural
      // 
      this.lblPlural.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblPlural.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblPlural.Location = new System.Drawing.Point(8, 120);
      this.lblPlural.Name = "lblPlural";
      this.lblPlural.Size = new System.Drawing.Size(100, 16);
      this.lblPlural.TabIndex = 0;
      this.lblPlural.Text = "Log Name (Multiple):";
      // 
      // lblSingular
      // 
      this.lblSingular.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblSingular.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblSingular.Location = new System.Drawing.Point(8, 96);
      this.lblSingular.Name = "lblSingular";
      this.lblSingular.Size = new System.Drawing.Size(100, 16);
      this.lblSingular.TabIndex = 0;
      this.lblSingular.Text = "Log Name (Single):";
      // 
      // lblJName
      // 
      this.lblJName.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblJName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblJName.Location = new System.Drawing.Point(80, 72);
      this.lblJName.Name = "lblJName";
      this.lblJName.Size = new System.Drawing.Size(80, 16);
      this.lblJName.TabIndex = 0;
      this.lblJName.Text = "Japanese Name:";
      // 
      // lblEName
      // 
      this.lblEName.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblEName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblEName.Location = new System.Drawing.Point(80, 48);
      this.lblEName.Name = "lblEName";
      this.lblEName.Size = new System.Drawing.Size(80, 16);
      this.lblEName.TabIndex = 0;
      this.lblEName.Text = "English Name:";
      // 
      // txtPlural
      // 
      this.txtPlural.Location = new System.Drawing.Point(112, 116);
      this.txtPlural.Name = "txtPlural";
      this.txtPlural.ReadOnly = true;
      this.txtPlural.Size = new System.Drawing.Size(304, 20);
      this.txtPlural.TabIndex = 6;
      this.txtPlural.Text = "";
      // 
      // txtSingular
      // 
      this.txtSingular.Location = new System.Drawing.Point(112, 92);
      this.txtSingular.Name = "txtSingular";
      this.txtSingular.ReadOnly = true;
      this.txtSingular.Size = new System.Drawing.Size(304, 20);
      this.txtSingular.TabIndex = 5;
      this.txtSingular.Text = "";
      // 
      // txtJName
      // 
      this.txtJName.Location = new System.Drawing.Point(164, 68);
      this.txtJName.Name = "txtJName";
      this.txtJName.ReadOnly = true;
      this.txtJName.Size = new System.Drawing.Size(252, 20);
      this.txtJName.TabIndex = 4;
      this.txtJName.Text = "";
      // 
      // txtEName
      // 
      this.txtEName.Location = new System.Drawing.Point(164, 44);
      this.txtEName.Name = "txtEName";
      this.txtEName.ReadOnly = true;
      this.txtEName.Size = new System.Drawing.Size(252, 20);
      this.txtEName.TabIndex = 3;
      this.txtEName.Text = "";
      // 
      // txtDescription
      // 
      this.txtDescription.Location = new System.Drawing.Point(68, 140);
      this.txtDescription.Multiline = true;
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.ReadOnly = true;
      this.txtDescription.Size = new System.Drawing.Size(348, 112);
      this.txtDescription.TabIndex = 7;
      this.txtDescription.Text = "";
      // 
      // lblStackSize
      // 
      this.lblStackSize.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblStackSize.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblStackSize.Location = new System.Drawing.Point(324, 260);
      this.lblStackSize.Name = "lblStackSize";
      this.lblStackSize.Size = new System.Drawing.Size(56, 16);
      this.lblStackSize.TabIndex = 0;
      this.lblStackSize.Text = "Stack Size:";
      // 
      // lblFlags
      // 
      this.lblFlags.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblFlags.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblFlags.Location = new System.Drawing.Point(8, 260);
      this.lblFlags.Name = "lblFlags";
      this.lblFlags.Size = new System.Drawing.Size(28, 16);
      this.lblFlags.TabIndex = 0;
      this.lblFlags.Text = "Flags:";
      // 
      // lblType
      // 
      this.lblType.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblType.Location = new System.Drawing.Point(196, 24);
      this.lblType.Name = "lblType";
      this.lblType.Size = new System.Drawing.Size(28, 16);
      this.lblType.TabIndex = 0;
      this.lblType.Text = "Type:";
      // 
      // lblID
      // 
      this.lblID.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblID.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblID.Location = new System.Drawing.Point(80, 24);
      this.lblID.Name = "lblID";
      this.lblID.Size = new System.Drawing.Size(20, 16);
      this.lblID.TabIndex = 0;
      this.lblID.Text = "ID:";
      // 
      // txtStackSize
      // 
      this.txtStackSize.Location = new System.Drawing.Point(384, 256);
      this.txtStackSize.Name = "txtStackSize";
      this.txtStackSize.ReadOnly = true;
      this.txtStackSize.Size = new System.Drawing.Size(32, 20);
      this.txtStackSize.TabIndex = 9;
      this.txtStackSize.Text = "";
      // 
      // txtFlags
      // 
      this.txtFlags.Location = new System.Drawing.Point(40, 256);
      this.txtFlags.Name = "txtFlags";
      this.txtFlags.ReadOnly = true;
      this.txtFlags.Size = new System.Drawing.Size(276, 20);
      this.txtFlags.TabIndex = 8;
      this.txtFlags.Text = "";
      // 
      // txtType
      // 
      this.txtType.Location = new System.Drawing.Point(228, 20);
      this.txtType.Name = "txtType";
      this.txtType.ReadOnly = true;
      this.txtType.Size = new System.Drawing.Size(188, 20);
      this.txtType.TabIndex = 2;
      this.txtType.Text = "";
      // 
      // txtID
      // 
      this.txtID.Location = new System.Drawing.Point(100, 20);
      this.txtID.Name = "txtID";
      this.txtID.ReadOnly = true;
      this.txtID.Size = new System.Drawing.Size(88, 20);
      this.txtID.TabIndex = 1;
      this.txtID.Text = "";
      // 
      // picIcon
      // 
      this.picIcon.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.picIcon.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.picIcon.Location = new System.Drawing.Point(8, 20);
      this.picIcon.Name = "picIcon";
      this.picIcon.Size = new System.Drawing.Size(64, 64);
      this.picIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.picIcon.TabIndex = 3;
      this.picIcon.TabStop = false;
      // 
      // FFXIItemEditor
      // 
      this.BackColor = System.Drawing.SystemColors.Control;
      this.Controls.Add(this.grpViewMode);
      this.Controls.Add(this.grpSpecializedInfo);
      this.Controls.Add(this.grpCommonInfo);
      this.Name = "FFXIItemEditor";
      this.Size = new System.Drawing.Size(424, 486);
      this.grpViewMode.ResumeLayout(false);
      this.grpSpecializedInfo.ResumeLayout(false);
      this.grpCommonInfo.ResumeLayout(false);
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

    private void FillCommonFields(FFXIItem.IItemInfo II) {
      this.txtEName.Text       = II.GetFieldText(ItemField.EnglishName);
      this.txtJName.Text       = II.GetFieldText(ItemField.JapaneseName);
      this.txtSingular.Text    = II.GetFieldText(ItemField.LogNameSingular);
      this.txtPlural.Text      = II.GetFieldText(ItemField.LogNamePlural);
      this.txtDescription.Text = II.GetFieldText(ItemField.Description).Replace("\n", "\r\n");
    }

    private void FillCommonExtraFields(FFXIItem.IItemInfo II) {
      this.txtResourceID.Text = II.GetFieldText(ItemField.ResourceID);
      this.txtLevel.Text      = II.GetFieldText(ItemField.Level);
      this.txtSlots.Text      = II.GetFieldText(ItemField.Slots);
      this.txtJobs.Text       = II.GetFieldText(ItemField.Jobs);
      this.txtRaces.Text      = II.GetFieldText(ItemField.Races);
      this.txtMaxCharges.Text = II.GetFieldText(ItemField.MaxCharges);
      this.txtCastTime.Text   = II.GetFieldText(ItemField.CastTime);
      this.txtEquipDelay.Text = II.GetFieldText(ItemField.EquipDelay);
      this.txtReuseTimer.Text = II.GetFieldText(ItemField.ReuseTimer);
    }

    private void FillObjectFields(FFXIItem.IItemInfo II) {
      this.FillCommonFields(II);
      this.grpSpecializedInfo.Visible = false;
    }

    private void FillArmorFields(FFXIItem.IItemInfo II) {
      this.FillCommonFields(II);
      this.grpSpecializedInfo.Visible = true;
      this.lblDamage.Visible = this.lblDelay.Visible = this.lblSkill.Visible = false;
      this.txtDamage.Visible = this.txtDelay.Visible = this.txtSkill.Visible = false;
      this.lblShieldSize.Visible = this.txtShieldSize.Visible = true;
      this.txtShieldSize.Text = II.GetFieldText(ItemField.ShieldSize);
      this.FillCommonExtraFields(II);
    }

    private void FillWeaponFields(FFXIItem.IItemInfo II) {
      this.FillCommonFields(II);
      this.grpSpecializedInfo.Visible = true;
      this.lblDamage.Visible = this.lblDelay.Visible = this.lblSkill.Visible = true;
      this.txtDamage.Visible = this.txtDelay.Visible = this.txtSkill.Visible = true;
      this.lblShieldSize.Visible = this.txtShieldSize.Visible = false;
      this.txtDamage.Text = II.GetFieldText(ItemField.Damage);
      this.txtDelay.Text  = II.GetFieldText(ItemField.Delay);
      this.txtSkill.Text  = II.GetFieldText(ItemField.Skill);
      this.FillCommonExtraFields(II);
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
