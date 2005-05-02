using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using PlayOnline.Core;

namespace PlayOnline.FFXI {

  public class FFXIItemEditor : System.Windows.Forms.UserControl {

    #region Controls

    private System.Windows.Forms.GroupBox grpItemViewMode;
    private System.Windows.Forms.CheckBox chkViewItemAsJWeapon;
    private System.Windows.Forms.CheckBox chkViewItemAsJObject;
    private System.Windows.Forms.CheckBox chkViewItemAsJArmor;
    private System.Windows.Forms.CheckBox chkViewItemAsEWeapon;
    private System.Windows.Forms.CheckBox chkViewItemAsEObject;
    private System.Windows.Forms.CheckBox chkViewItemAsEArmor;
    private System.Windows.Forms.GroupBox grpSpecializedItemInfo;
    private System.Windows.Forms.Label lblItemRaces;
    private System.Windows.Forms.TextBox txtItemRaces;
    private System.Windows.Forms.Label lblItemSlots;
    private System.Windows.Forms.TextBox txtItemSlots;
    private System.Windows.Forms.Label lblItemJobs;
    private System.Windows.Forms.TextBox txtItemJobs;
    private System.Windows.Forms.Label lblItemLevel;
    private System.Windows.Forms.TextBox txtItemLevel;
    private System.Windows.Forms.Label lblItemEquipDelay;
    private System.Windows.Forms.Label lblItemReuseTimer;
    private System.Windows.Forms.Label lblItemMaxCharges;
    private System.Windows.Forms.Label lblItemResourceID;
    private System.Windows.Forms.TextBox txtItemReuseTimer;
    private System.Windows.Forms.TextBox txtItemEquipDelay;
    private System.Windows.Forms.TextBox txtItemMaxCharges;
    private System.Windows.Forms.TextBox txtItemResourceID;
    private System.Windows.Forms.Label lblItemSkill;
    private System.Windows.Forms.Label lblItemDelay;
    private System.Windows.Forms.Label lblItemDamage;
    private System.Windows.Forms.TextBox txtItemSkill;
    private System.Windows.Forms.TextBox txtItemDelay;
    private System.Windows.Forms.TextBox txtItemDamage;
    private System.Windows.Forms.Label lblItemShieldSize;
    private System.Windows.Forms.TextBox txtItemShieldSize;
    private System.Windows.Forms.GroupBox grpCommonItemInfo;
    private System.Windows.Forms.Label lblItemDescription;
    private System.Windows.Forms.Label lblItemPlural;
    private System.Windows.Forms.Label lblItemSingular;
    private System.Windows.Forms.Label lblItemJName;
    private System.Windows.Forms.Label lblItemEName;
    private System.Windows.Forms.TextBox txtItemPlural;
    private System.Windows.Forms.TextBox txtItemSingular;
    private System.Windows.Forms.TextBox txtItemJName;
    private System.Windows.Forms.TextBox txtItemEName;
    private System.Windows.Forms.TextBox txtItemDescription;
    private System.Windows.Forms.Label lblItemStackSize;
    private System.Windows.Forms.Label lblItemFlags;
    private System.Windows.Forms.Label lblItemType;
    private System.Windows.Forms.Label lblItemID;
    private System.Windows.Forms.TextBox txtItemStackSize;
    private System.Windows.Forms.TextBox txtItemFlags;
    private System.Windows.Forms.TextBox txtItemType;
    private System.Windows.Forms.TextBox txtItemID;
    private System.Windows.Forms.PictureBox picItemIcon;
    private System.Windows.Forms.ToolTip ttToolTip;
    private System.ComponentModel.IContainer components;

    #endregion

    public FFXIItemEditor() {
      this.InitializeComponent();
      this.ShowItem();
    }

    private FFXIItem ItemToShow_ = null;

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
    public ItemDataLanguage ChosenItemLanguage {
      get {
	return ((this.chkViewItemAsEArmor.Checked || this.chkViewItemAsEObject.Checked || this.chkViewItemAsEWeapon.Checked) ? ItemDataLanguage.English : ItemDataLanguage.Japanese);
      }
    }

    [Browsable(false)]
    public ItemDataType ChosenItemType {
      get {
	if (this.chkViewItemAsEArmor.Checked || this.chkViewItemAsJArmor.Checked)
	  return ItemDataType.Armor;
	if (this.chkViewItemAsEWeapon.Checked || this.chkViewItemAsJWeapon.Checked)
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

    private void ShowItem() {
      this.chkViewItemAsEArmor.Checked = this.chkViewItemAsEObject.Checked = this.chkViewItemAsEWeapon.Checked = false;
      this.chkViewItemAsJArmor.Checked = this.chkViewItemAsJObject.Checked = this.chkViewItemAsJWeapon.Checked = false;
    FFXIItem I = this.ItemToShow_;
      if (I != null) {
	// Common Info
	this.picItemIcon.Image     = I.IconGraphic.Bitmap;
	this.ttToolTip.SetToolTip(this.picItemIcon, I.IconGraphic.ToString());
	this.txtItemID.Text        = String.Format("{0:X4} ({0})", I.Common.ID);
	this.txtItemType.Text      = String.Format("{0:X} ({0})",  I.Common.Type);
	this.txtItemFlags.Text     = String.Format("{0:X} ({0})",  I.Common.Flags);
	this.txtItemStackSize.Text = String.Format("{0}",          I.Common.StackSize);
	switch (I.Common.Type) {
	  case ItemType.Armor: {
	  string LogNames = I.ENArmor.LogNameSingular + I.ENArmor.LogNamePlural;
	    if (LogNames == String.Empty)
	      this.chkViewItemAsJArmor.Checked = true;
	    else {
	    bool ExtendedCharSeen = false;
	      foreach (char C in LogNames) {
		if ((int) C > 0x100) {
		  ExtendedCharSeen = true;
		  break;
		}
	      }
	      this.chkViewItemAsEArmor.Checked = !ExtendedCharSeen;
	      this.chkViewItemAsJArmor.Checked =  ExtendedCharSeen;
	    }
	    break;
	  }
	  case ItemType.Weapon: {
	  string LogNames = I.ENWeapon.LogNameSingular + I.ENArmor.LogNamePlural;
	    if (LogNames == String.Empty)
	      this.chkViewItemAsJWeapon.Checked = true;
	    else {
	    bool ExtendedCharSeen = false;
	      foreach (char C in LogNames) {
		if ((int) C > 0x100) {
		  ExtendedCharSeen = true;
		  break;
		}
	      }
	      this.chkViewItemAsEWeapon.Checked = !ExtendedCharSeen;
	      this.chkViewItemAsJWeapon.Checked =  ExtendedCharSeen;
	    }
	    break;
	  }
	  default: {
	  string LogNames = I.ENObject.LogNameSingular + I.ENArmor.LogNamePlural;
	    if (LogNames == String.Empty)
	      this.chkViewItemAsJObject.Checked = true;
	    else {
	    bool ExtendedCharSeen = false;
	      foreach (char C in LogNames) {
		if ((int) C > 0x100) {
		  ExtendedCharSeen = true;
		  break;
		}
	      }
	      this.chkViewItemAsEObject.Checked = !ExtendedCharSeen;
	      this.chkViewItemAsJObject.Checked =  ExtendedCharSeen;
	    }
	    break;
	  }
	}
      }
      else { // Clear all fields
	this.picItemIcon.Image = null;
	this.ttToolTip.SetToolTip(this.picItemIcon, null);
	this.txtItemID.Text          = this.txtItemType.Text      = String.Empty;
	this.txtItemEName.Text       = this.txtItemJName.Text     = String.Empty;
	this.txtItemSingular.Text    = this.txtItemPlural.Text    = String.Empty;
	this.txtItemFlags.Text       = this.txtItemStackSize.Text = String.Empty;
	this.txtItemDescription.Text = String.Empty;
	this.grpSpecializedItemInfo.Visible = false;
      }
    }

    #region Component Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && this.components != null)
	this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      this.components = new System.ComponentModel.Container();
      this.grpItemViewMode = new System.Windows.Forms.GroupBox();
      this.chkViewItemAsJWeapon = new System.Windows.Forms.CheckBox();
      this.chkViewItemAsJObject = new System.Windows.Forms.CheckBox();
      this.chkViewItemAsJArmor = new System.Windows.Forms.CheckBox();
      this.chkViewItemAsEWeapon = new System.Windows.Forms.CheckBox();
      this.chkViewItemAsEObject = new System.Windows.Forms.CheckBox();
      this.chkViewItemAsEArmor = new System.Windows.Forms.CheckBox();
      this.grpSpecializedItemInfo = new System.Windows.Forms.GroupBox();
      this.lblItemRaces = new System.Windows.Forms.Label();
      this.txtItemRaces = new System.Windows.Forms.TextBox();
      this.lblItemSlots = new System.Windows.Forms.Label();
      this.txtItemSlots = new System.Windows.Forms.TextBox();
      this.lblItemJobs = new System.Windows.Forms.Label();
      this.txtItemJobs = new System.Windows.Forms.TextBox();
      this.lblItemLevel = new System.Windows.Forms.Label();
      this.txtItemLevel = new System.Windows.Forms.TextBox();
      this.lblItemEquipDelay = new System.Windows.Forms.Label();
      this.lblItemReuseTimer = new System.Windows.Forms.Label();
      this.lblItemMaxCharges = new System.Windows.Forms.Label();
      this.lblItemResourceID = new System.Windows.Forms.Label();
      this.txtItemReuseTimer = new System.Windows.Forms.TextBox();
      this.txtItemEquipDelay = new System.Windows.Forms.TextBox();
      this.txtItemMaxCharges = new System.Windows.Forms.TextBox();
      this.txtItemResourceID = new System.Windows.Forms.TextBox();
      this.lblItemSkill = new System.Windows.Forms.Label();
      this.lblItemDelay = new System.Windows.Forms.Label();
      this.lblItemDamage = new System.Windows.Forms.Label();
      this.txtItemSkill = new System.Windows.Forms.TextBox();
      this.txtItemDelay = new System.Windows.Forms.TextBox();
      this.txtItemDamage = new System.Windows.Forms.TextBox();
      this.lblItemShieldSize = new System.Windows.Forms.Label();
      this.txtItemShieldSize = new System.Windows.Forms.TextBox();
      this.grpCommonItemInfo = new System.Windows.Forms.GroupBox();
      this.lblItemDescription = new System.Windows.Forms.Label();
      this.lblItemPlural = new System.Windows.Forms.Label();
      this.lblItemSingular = new System.Windows.Forms.Label();
      this.lblItemJName = new System.Windows.Forms.Label();
      this.lblItemEName = new System.Windows.Forms.Label();
      this.txtItemPlural = new System.Windows.Forms.TextBox();
      this.txtItemSingular = new System.Windows.Forms.TextBox();
      this.txtItemJName = new System.Windows.Forms.TextBox();
      this.txtItemEName = new System.Windows.Forms.TextBox();
      this.txtItemDescription = new System.Windows.Forms.TextBox();
      this.lblItemStackSize = new System.Windows.Forms.Label();
      this.lblItemFlags = new System.Windows.Forms.Label();
      this.lblItemType = new System.Windows.Forms.Label();
      this.lblItemID = new System.Windows.Forms.Label();
      this.txtItemStackSize = new System.Windows.Forms.TextBox();
      this.txtItemFlags = new System.Windows.Forms.TextBox();
      this.txtItemType = new System.Windows.Forms.TextBox();
      this.txtItemID = new System.Windows.Forms.TextBox();
      this.picItemIcon = new System.Windows.Forms.PictureBox();
      this.ttToolTip = new System.Windows.Forms.ToolTip(this.components);
      this.grpItemViewMode.SuspendLayout();
      this.grpSpecializedItemInfo.SuspendLayout();
      this.grpCommonItemInfo.SuspendLayout();
      this.SuspendLayout();
      // 
      // grpItemViewMode
      // 
      this.grpItemViewMode.Controls.Add(this.chkViewItemAsJWeapon);
      this.grpItemViewMode.Controls.Add(this.chkViewItemAsJObject);
      this.grpItemViewMode.Controls.Add(this.chkViewItemAsJArmor);
      this.grpItemViewMode.Controls.Add(this.chkViewItemAsEWeapon);
      this.grpItemViewMode.Controls.Add(this.chkViewItemAsEObject);
      this.grpItemViewMode.Controls.Add(this.chkViewItemAsEArmor);
      this.grpItemViewMode.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.grpItemViewMode.Location = new System.Drawing.Point(0, 0);
      this.grpItemViewMode.Name = "grpItemViewMode";
      this.grpItemViewMode.Size = new System.Drawing.Size(424, 76);
      this.grpItemViewMode.TabIndex = 4;
      this.grpItemViewMode.TabStop = false;
      this.grpItemViewMode.Text = "View Item Info As";
      // 
      // chkViewItemAsJWeapon
      // 
      this.chkViewItemAsJWeapon.Appearance = System.Windows.Forms.Appearance.Button;
      this.chkViewItemAsJWeapon.AutoCheck = false;
      this.chkViewItemAsJWeapon.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.chkViewItemAsJWeapon.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.chkViewItemAsJWeapon.Location = new System.Drawing.Point(276, 44);
      this.chkViewItemAsJWeapon.Name = "chkViewItemAsJWeapon";
      this.chkViewItemAsJWeapon.Size = new System.Drawing.Size(140, 24);
      this.chkViewItemAsJWeapon.TabIndex = 6;
      this.chkViewItemAsJWeapon.Text = "Weapon Information - JP";
      this.chkViewItemAsJWeapon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.chkViewItemAsJWeapon.Click += new System.EventHandler(this.chkViewItem_Click);
      this.chkViewItemAsJWeapon.CheckedChanged += new System.EventHandler(this.chkViewItemAsJWeapon_CheckedChanged);
      // 
      // chkViewItemAsJObject
      // 
      this.chkViewItemAsJObject.Appearance = System.Windows.Forms.Appearance.Button;
      this.chkViewItemAsJObject.AutoCheck = false;
      this.chkViewItemAsJObject.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.chkViewItemAsJObject.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.chkViewItemAsJObject.Location = new System.Drawing.Point(140, 44);
      this.chkViewItemAsJObject.Name = "chkViewItemAsJObject";
      this.chkViewItemAsJObject.Size = new System.Drawing.Size(132, 24);
      this.chkViewItemAsJObject.TabIndex = 5;
      this.chkViewItemAsJObject.Text = "Object Information - JP";
      this.chkViewItemAsJObject.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.chkViewItemAsJObject.Click += new System.EventHandler(this.chkViewItem_Click);
      this.chkViewItemAsJObject.CheckedChanged += new System.EventHandler(this.chkViewItemAsJObject_CheckedChanged);
      // 
      // chkViewItemAsJArmor
      // 
      this.chkViewItemAsJArmor.Appearance = System.Windows.Forms.Appearance.Button;
      this.chkViewItemAsJArmor.AutoCheck = false;
      this.chkViewItemAsJArmor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.chkViewItemAsJArmor.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.chkViewItemAsJArmor.Location = new System.Drawing.Point(8, 44);
      this.chkViewItemAsJArmor.Name = "chkViewItemAsJArmor";
      this.chkViewItemAsJArmor.Size = new System.Drawing.Size(128, 24);
      this.chkViewItemAsJArmor.TabIndex = 4;
      this.chkViewItemAsJArmor.Text = "Armor Information - JP";
      this.chkViewItemAsJArmor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.chkViewItemAsJArmor.Click += new System.EventHandler(this.chkViewItem_Click);
      this.chkViewItemAsJArmor.CheckedChanged += new System.EventHandler(this.chkViewItemAsJArmor_CheckedChanged);
      // 
      // chkViewItemAsEWeapon
      // 
      this.chkViewItemAsEWeapon.Appearance = System.Windows.Forms.Appearance.Button;
      this.chkViewItemAsEWeapon.AutoCheck = false;
      this.chkViewItemAsEWeapon.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.chkViewItemAsEWeapon.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.chkViewItemAsEWeapon.Location = new System.Drawing.Point(276, 16);
      this.chkViewItemAsEWeapon.Name = "chkViewItemAsEWeapon";
      this.chkViewItemAsEWeapon.Size = new System.Drawing.Size(140, 24);
      this.chkViewItemAsEWeapon.TabIndex = 3;
      this.chkViewItemAsEWeapon.Text = "Weapon Information - EN";
      this.chkViewItemAsEWeapon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.chkViewItemAsEWeapon.Click += new System.EventHandler(this.chkViewItem_Click);
      this.chkViewItemAsEWeapon.CheckedChanged += new System.EventHandler(this.chkViewItemAsEWeapon_CheckedChanged);
      // 
      // chkViewItemAsEObject
      // 
      this.chkViewItemAsEObject.Appearance = System.Windows.Forms.Appearance.Button;
      this.chkViewItemAsEObject.AutoCheck = false;
      this.chkViewItemAsEObject.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.chkViewItemAsEObject.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.chkViewItemAsEObject.Location = new System.Drawing.Point(140, 16);
      this.chkViewItemAsEObject.Name = "chkViewItemAsEObject";
      this.chkViewItemAsEObject.Size = new System.Drawing.Size(132, 24);
      this.chkViewItemAsEObject.TabIndex = 2;
      this.chkViewItemAsEObject.Text = "Object Information - EN";
      this.chkViewItemAsEObject.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.chkViewItemAsEObject.Click += new System.EventHandler(this.chkViewItem_Click);
      this.chkViewItemAsEObject.CheckedChanged += new System.EventHandler(this.chkViewItemAsEObject_CheckedChanged);
      // 
      // chkViewItemAsEArmor
      // 
      this.chkViewItemAsEArmor.Appearance = System.Windows.Forms.Appearance.Button;
      this.chkViewItemAsEArmor.AutoCheck = false;
      this.chkViewItemAsEArmor.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.chkViewItemAsEArmor.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.chkViewItemAsEArmor.Location = new System.Drawing.Point(8, 16);
      this.chkViewItemAsEArmor.Name = "chkViewItemAsEArmor";
      this.chkViewItemAsEArmor.Size = new System.Drawing.Size(128, 24);
      this.chkViewItemAsEArmor.TabIndex = 1;
      this.chkViewItemAsEArmor.Text = "Armor Information - EN";
      this.chkViewItemAsEArmor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.chkViewItemAsEArmor.Click += new System.EventHandler(this.chkViewItem_Click);
      this.chkViewItemAsEArmor.CheckedChanged += new System.EventHandler(this.chkViewItemAsEArmor_CheckedChanged);
      // 
      // grpSpecializedItemInfo
      // 
      this.grpSpecializedItemInfo.Controls.Add(this.lblItemRaces);
      this.grpSpecializedItemInfo.Controls.Add(this.txtItemRaces);
      this.grpSpecializedItemInfo.Controls.Add(this.lblItemSlots);
      this.grpSpecializedItemInfo.Controls.Add(this.txtItemSlots);
      this.grpSpecializedItemInfo.Controls.Add(this.lblItemJobs);
      this.grpSpecializedItemInfo.Controls.Add(this.txtItemJobs);
      this.grpSpecializedItemInfo.Controls.Add(this.lblItemLevel);
      this.grpSpecializedItemInfo.Controls.Add(this.txtItemLevel);
      this.grpSpecializedItemInfo.Controls.Add(this.lblItemEquipDelay);
      this.grpSpecializedItemInfo.Controls.Add(this.lblItemReuseTimer);
      this.grpSpecializedItemInfo.Controls.Add(this.lblItemMaxCharges);
      this.grpSpecializedItemInfo.Controls.Add(this.lblItemResourceID);
      this.grpSpecializedItemInfo.Controls.Add(this.txtItemReuseTimer);
      this.grpSpecializedItemInfo.Controls.Add(this.txtItemEquipDelay);
      this.grpSpecializedItemInfo.Controls.Add(this.txtItemMaxCharges);
      this.grpSpecializedItemInfo.Controls.Add(this.txtItemResourceID);
      this.grpSpecializedItemInfo.Controls.Add(this.lblItemSkill);
      this.grpSpecializedItemInfo.Controls.Add(this.lblItemDelay);
      this.grpSpecializedItemInfo.Controls.Add(this.lblItemDamage);
      this.grpSpecializedItemInfo.Controls.Add(this.txtItemSkill);
      this.grpSpecializedItemInfo.Controls.Add(this.txtItemDelay);
      this.grpSpecializedItemInfo.Controls.Add(this.txtItemDamage);
      this.grpSpecializedItemInfo.Controls.Add(this.lblItemShieldSize);
      this.grpSpecializedItemInfo.Controls.Add(this.txtItemShieldSize);
      this.grpSpecializedItemInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.grpSpecializedItemInfo.Location = new System.Drawing.Point(0, 364);
      this.grpSpecializedItemInfo.Name = "grpSpecializedItemInfo";
      this.grpSpecializedItemInfo.Size = new System.Drawing.Size(424, 122);
      this.grpSpecializedItemInfo.TabIndex = 6;
      this.grpSpecializedItemInfo.TabStop = false;
      this.grpSpecializedItemInfo.Text = "Specialized Information";
      this.grpSpecializedItemInfo.Visible = false;
      // 
      // lblItemRaces
      // 
      this.lblItemRaces.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemRaces.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblItemRaces.Location = new System.Drawing.Point(264, 48);
      this.lblItemRaces.Name = "lblItemRaces";
      this.lblItemRaces.Size = new System.Drawing.Size(36, 16);
      this.lblItemRaces.TabIndex = 38;
      this.lblItemRaces.Text = "Races:";
      // 
      // txtItemRaces
      // 
      this.txtItemRaces.Location = new System.Drawing.Point(304, 44);
      this.txtItemRaces.Name = "txtItemRaces";
      this.txtItemRaces.ReadOnly = true;
      this.txtItemRaces.Size = new System.Drawing.Size(112, 20);
      this.txtItemRaces.TabIndex = 37;
      this.txtItemRaces.Text = "";
      // 
      // lblItemSlots
      // 
      this.lblItemSlots.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemSlots.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblItemSlots.Location = new System.Drawing.Point(228, 24);
      this.lblItemSlots.Name = "lblItemSlots";
      this.lblItemSlots.Size = new System.Drawing.Size(32, 16);
      this.lblItemSlots.TabIndex = 36;
      this.lblItemSlots.Text = "Slots:";
      // 
      // txtItemSlots
      // 
      this.txtItemSlots.Location = new System.Drawing.Point(260, 20);
      this.txtItemSlots.Name = "txtItemSlots";
      this.txtItemSlots.ReadOnly = true;
      this.txtItemSlots.Size = new System.Drawing.Size(156, 20);
      this.txtItemSlots.TabIndex = 35;
      this.txtItemSlots.Text = "";
      // 
      // lblItemJobs
      // 
      this.lblItemJobs.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemJobs.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblItemJobs.Location = new System.Drawing.Point(8, 48);
      this.lblItemJobs.Name = "lblItemJobs";
      this.lblItemJobs.Size = new System.Drawing.Size(32, 16);
      this.lblItemJobs.TabIndex = 34;
      this.lblItemJobs.Text = "Jobs:";
      // 
      // txtItemJobs
      // 
      this.txtItemJobs.Location = new System.Drawing.Point(40, 44);
      this.txtItemJobs.Name = "txtItemJobs";
      this.txtItemJobs.ReadOnly = true;
      this.txtItemJobs.Size = new System.Drawing.Size(216, 20);
      this.txtItemJobs.TabIndex = 33;
      this.txtItemJobs.Text = "";
      // 
      // lblItemLevel
      // 
      this.lblItemLevel.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemLevel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblItemLevel.Location = new System.Drawing.Point(156, 24);
      this.lblItemLevel.Name = "lblItemLevel";
      this.lblItemLevel.Size = new System.Drawing.Size(32, 16);
      this.lblItemLevel.TabIndex = 32;
      this.lblItemLevel.Text = "Level:";
      // 
      // txtItemLevel
      // 
      this.txtItemLevel.Location = new System.Drawing.Point(192, 20);
      this.txtItemLevel.Name = "txtItemLevel";
      this.txtItemLevel.ReadOnly = true;
      this.txtItemLevel.Size = new System.Drawing.Size(28, 20);
      this.txtItemLevel.TabIndex = 31;
      this.txtItemLevel.Text = "";
      // 
      // lblItemEquipDelay
      // 
      this.lblItemEquipDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblItemEquipDelay.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemEquipDelay.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblItemEquipDelay.Location = new System.Drawing.Point(108, 98);
      this.lblItemEquipDelay.Name = "lblItemEquipDelay";
      this.lblItemEquipDelay.Size = new System.Drawing.Size(60, 16);
      this.lblItemEquipDelay.TabIndex = 25;
      this.lblItemEquipDelay.Text = "Equip Delay:";
      // 
      // lblItemReuseTimer
      // 
      this.lblItemReuseTimer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblItemReuseTimer.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemReuseTimer.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblItemReuseTimer.Location = new System.Drawing.Point(264, 98);
      this.lblItemReuseTimer.Name = "lblItemReuseTimer";
      this.lblItemReuseTimer.Size = new System.Drawing.Size(64, 16);
      this.lblItemReuseTimer.TabIndex = 24;
      this.lblItemReuseTimer.Text = "Reuse Timer:";
      // 
      // lblItemMaxCharges
      // 
      this.lblItemMaxCharges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblItemMaxCharges.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemMaxCharges.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblItemMaxCharges.Location = new System.Drawing.Point(8, 98);
      this.lblItemMaxCharges.Name = "lblItemMaxCharges";
      this.lblItemMaxCharges.Size = new System.Drawing.Size(48, 16);
      this.lblItemMaxCharges.TabIndex = 23;
      this.lblItemMaxCharges.Text = "Charges:";
      // 
      // lblItemResourceID
      // 
      this.lblItemResourceID.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemResourceID.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblItemResourceID.Location = new System.Drawing.Point(8, 24);
      this.lblItemResourceID.Name = "lblItemResourceID";
      this.lblItemResourceID.Size = new System.Drawing.Size(68, 16);
      this.lblItemResourceID.TabIndex = 22;
      this.lblItemResourceID.Text = "Resource ID:";
      // 
      // txtItemReuseTimer
      // 
      this.txtItemReuseTimer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.txtItemReuseTimer.Location = new System.Drawing.Point(332, 94);
      this.txtItemReuseTimer.Name = "txtItemReuseTimer";
      this.txtItemReuseTimer.ReadOnly = true;
      this.txtItemReuseTimer.Size = new System.Drawing.Size(84, 20);
      this.txtItemReuseTimer.TabIndex = 21;
      this.txtItemReuseTimer.Text = "";
      // 
      // txtItemEquipDelay
      // 
      this.txtItemEquipDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.txtItemEquipDelay.Location = new System.Drawing.Point(172, 94);
      this.txtItemEquipDelay.Name = "txtItemEquipDelay";
      this.txtItemEquipDelay.ReadOnly = true;
      this.txtItemEquipDelay.Size = new System.Drawing.Size(84, 20);
      this.txtItemEquipDelay.TabIndex = 20;
      this.txtItemEquipDelay.Text = "";
      // 
      // txtItemMaxCharges
      // 
      this.txtItemMaxCharges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.txtItemMaxCharges.Location = new System.Drawing.Point(56, 94);
      this.txtItemMaxCharges.Name = "txtItemMaxCharges";
      this.txtItemMaxCharges.ReadOnly = true;
      this.txtItemMaxCharges.Size = new System.Drawing.Size(44, 20);
      this.txtItemMaxCharges.TabIndex = 19;
      this.txtItemMaxCharges.Text = "";
      // 
      // txtItemResourceID
      // 
      this.txtItemResourceID.Location = new System.Drawing.Point(76, 20);
      this.txtItemResourceID.Name = "txtItemResourceID";
      this.txtItemResourceID.ReadOnly = true;
      this.txtItemResourceID.Size = new System.Drawing.Size(72, 20);
      this.txtItemResourceID.TabIndex = 15;
      this.txtItemResourceID.Text = "";
      // 
      // lblItemSkill
      // 
      this.lblItemSkill.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblItemSkill.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemSkill.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblItemSkill.Location = new System.Drawing.Point(190, 72);
      this.lblItemSkill.Name = "lblItemSkill";
      this.lblItemSkill.Size = new System.Drawing.Size(68, 16);
      this.lblItemSkill.TabIndex = 28;
      this.lblItemSkill.Text = "Weapon Skill:";
      this.lblItemSkill.Visible = false;
      // 
      // lblItemDelay
      // 
      this.lblItemDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblItemDelay.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemDelay.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblItemDelay.Location = new System.Drawing.Point(90, 72);
      this.lblItemDelay.Name = "lblItemDelay";
      this.lblItemDelay.Size = new System.Drawing.Size(32, 16);
      this.lblItemDelay.TabIndex = 27;
      this.lblItemDelay.Text = "Delay:";
      this.lblItemDelay.Visible = false;
      // 
      // lblItemDamage
      // 
      this.lblItemDamage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblItemDamage.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemDamage.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblItemDamage.Location = new System.Drawing.Point(8, 72);
      this.lblItemDamage.Name = "lblItemDamage";
      this.lblItemDamage.Size = new System.Drawing.Size(46, 16);
      this.lblItemDamage.TabIndex = 26;
      this.lblItemDamage.Text = "Damage:";
      this.lblItemDamage.Visible = false;
      // 
      // txtItemSkill
      // 
      this.txtItemSkill.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.txtItemSkill.Location = new System.Drawing.Point(260, 68);
      this.txtItemSkill.Name = "txtItemSkill";
      this.txtItemSkill.ReadOnly = true;
      this.txtItemSkill.Size = new System.Drawing.Size(156, 20);
      this.txtItemSkill.TabIndex = 18;
      this.txtItemSkill.Text = "";
      this.txtItemSkill.Visible = false;
      // 
      // txtItemDelay
      // 
      this.txtItemDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.txtItemDelay.Location = new System.Drawing.Point(124, 68);
      this.txtItemDelay.Name = "txtItemDelay";
      this.txtItemDelay.ReadOnly = true;
      this.txtItemDelay.Size = new System.Drawing.Size(60, 20);
      this.txtItemDelay.TabIndex = 17;
      this.txtItemDelay.Text = "";
      this.txtItemDelay.Visible = false;
      // 
      // txtItemDamage
      // 
      this.txtItemDamage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.txtItemDamage.Location = new System.Drawing.Point(56, 68);
      this.txtItemDamage.Name = "txtItemDamage";
      this.txtItemDamage.ReadOnly = true;
      this.txtItemDamage.Size = new System.Drawing.Size(28, 20);
      this.txtItemDamage.TabIndex = 16;
      this.txtItemDamage.Text = "";
      this.txtItemDamage.Visible = false;
      // 
      // lblItemShieldSize
      // 
      this.lblItemShieldSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblItemShieldSize.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemShieldSize.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblItemShieldSize.Location = new System.Drawing.Point(8, 72);
      this.lblItemShieldSize.Name = "lblItemShieldSize";
      this.lblItemShieldSize.Size = new System.Drawing.Size(56, 16);
      this.lblItemShieldSize.TabIndex = 30;
      this.lblItemShieldSize.Text = "Shield Size:";
      this.lblItemShieldSize.Visible = false;
      // 
      // txtItemShieldSize
      // 
      this.txtItemShieldSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.txtItemShieldSize.Location = new System.Drawing.Point(68, 68);
      this.txtItemShieldSize.Name = "txtItemShieldSize";
      this.txtItemShieldSize.ReadOnly = true;
      this.txtItemShieldSize.Size = new System.Drawing.Size(32, 20);
      this.txtItemShieldSize.TabIndex = 29;
      this.txtItemShieldSize.Text = "";
      this.txtItemShieldSize.Visible = false;
      // 
      // grpCommonItemInfo
      // 
      this.grpCommonItemInfo.Controls.Add(this.lblItemDescription);
      this.grpCommonItemInfo.Controls.Add(this.lblItemPlural);
      this.grpCommonItemInfo.Controls.Add(this.lblItemSingular);
      this.grpCommonItemInfo.Controls.Add(this.lblItemJName);
      this.grpCommonItemInfo.Controls.Add(this.lblItemEName);
      this.grpCommonItemInfo.Controls.Add(this.txtItemPlural);
      this.grpCommonItemInfo.Controls.Add(this.txtItemSingular);
      this.grpCommonItemInfo.Controls.Add(this.txtItemJName);
      this.grpCommonItemInfo.Controls.Add(this.txtItemEName);
      this.grpCommonItemInfo.Controls.Add(this.txtItemDescription);
      this.grpCommonItemInfo.Controls.Add(this.lblItemStackSize);
      this.grpCommonItemInfo.Controls.Add(this.lblItemFlags);
      this.grpCommonItemInfo.Controls.Add(this.lblItemType);
      this.grpCommonItemInfo.Controls.Add(this.lblItemID);
      this.grpCommonItemInfo.Controls.Add(this.txtItemStackSize);
      this.grpCommonItemInfo.Controls.Add(this.txtItemFlags);
      this.grpCommonItemInfo.Controls.Add(this.txtItemType);
      this.grpCommonItemInfo.Controls.Add(this.txtItemID);
      this.grpCommonItemInfo.Controls.Add(this.picItemIcon);
      this.grpCommonItemInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.grpCommonItemInfo.Location = new System.Drawing.Point(0, 76);
      this.grpCommonItemInfo.Name = "grpCommonItemInfo";
      this.grpCommonItemInfo.Size = new System.Drawing.Size(424, 284);
      this.grpCommonItemInfo.TabIndex = 5;
      this.grpCommonItemInfo.TabStop = false;
      this.grpCommonItemInfo.Text = "Common Information";
      // 
      // lblItemDescription
      // 
      this.lblItemDescription.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemDescription.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblItemDescription.Location = new System.Drawing.Point(8, 144);
      this.lblItemDescription.Name = "lblItemDescription";
      this.lblItemDescription.Size = new System.Drawing.Size(60, 16);
      this.lblItemDescription.TabIndex = 0;
      this.lblItemDescription.Text = "Description:";
      // 
      // lblItemPlural
      // 
      this.lblItemPlural.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemPlural.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblItemPlural.Location = new System.Drawing.Point(8, 120);
      this.lblItemPlural.Name = "lblItemPlural";
      this.lblItemPlural.Size = new System.Drawing.Size(100, 16);
      this.lblItemPlural.TabIndex = 0;
      this.lblItemPlural.Text = "Log Name (Multiple):";
      // 
      // lblItemSingular
      // 
      this.lblItemSingular.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemSingular.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblItemSingular.Location = new System.Drawing.Point(8, 96);
      this.lblItemSingular.Name = "lblItemSingular";
      this.lblItemSingular.Size = new System.Drawing.Size(100, 16);
      this.lblItemSingular.TabIndex = 0;
      this.lblItemSingular.Text = "Log Name (Single):";
      // 
      // lblItemJName
      // 
      this.lblItemJName.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemJName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblItemJName.Location = new System.Drawing.Point(80, 72);
      this.lblItemJName.Name = "lblItemJName";
      this.lblItemJName.Size = new System.Drawing.Size(80, 16);
      this.lblItemJName.TabIndex = 0;
      this.lblItemJName.Text = "Japanese Name:";
      // 
      // lblItemEName
      // 
      this.lblItemEName.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemEName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblItemEName.Location = new System.Drawing.Point(80, 48);
      this.lblItemEName.Name = "lblItemEName";
      this.lblItemEName.Size = new System.Drawing.Size(80, 16);
      this.lblItemEName.TabIndex = 0;
      this.lblItemEName.Text = "English Name:";
      // 
      // txtItemPlural
      // 
      this.txtItemPlural.Location = new System.Drawing.Point(112, 116);
      this.txtItemPlural.Name = "txtItemPlural";
      this.txtItemPlural.ReadOnly = true;
      this.txtItemPlural.Size = new System.Drawing.Size(304, 20);
      this.txtItemPlural.TabIndex = 6;
      this.txtItemPlural.Text = "";
      // 
      // txtItemSingular
      // 
      this.txtItemSingular.Location = new System.Drawing.Point(112, 92);
      this.txtItemSingular.Name = "txtItemSingular";
      this.txtItemSingular.ReadOnly = true;
      this.txtItemSingular.Size = new System.Drawing.Size(304, 20);
      this.txtItemSingular.TabIndex = 5;
      this.txtItemSingular.Text = "";
      // 
      // txtItemJName
      // 
      this.txtItemJName.Location = new System.Drawing.Point(164, 68);
      this.txtItemJName.Name = "txtItemJName";
      this.txtItemJName.ReadOnly = true;
      this.txtItemJName.Size = new System.Drawing.Size(252, 20);
      this.txtItemJName.TabIndex = 4;
      this.txtItemJName.Text = "";
      // 
      // txtItemEName
      // 
      this.txtItemEName.Location = new System.Drawing.Point(164, 44);
      this.txtItemEName.Name = "txtItemEName";
      this.txtItemEName.ReadOnly = true;
      this.txtItemEName.Size = new System.Drawing.Size(252, 20);
      this.txtItemEName.TabIndex = 3;
      this.txtItemEName.Text = "";
      // 
      // txtItemDescription
      // 
      this.txtItemDescription.Location = new System.Drawing.Point(68, 140);
      this.txtItemDescription.Multiline = true;
      this.txtItemDescription.Name = "txtItemDescription";
      this.txtItemDescription.ReadOnly = true;
      this.txtItemDescription.Size = new System.Drawing.Size(348, 112);
      this.txtItemDescription.TabIndex = 7;
      this.txtItemDescription.Text = "";
      // 
      // lblItemStackSize
      // 
      this.lblItemStackSize.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemStackSize.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblItemStackSize.Location = new System.Drawing.Point(324, 260);
      this.lblItemStackSize.Name = "lblItemStackSize";
      this.lblItemStackSize.Size = new System.Drawing.Size(56, 16);
      this.lblItemStackSize.TabIndex = 0;
      this.lblItemStackSize.Text = "Stack Size:";
      // 
      // lblItemFlags
      // 
      this.lblItemFlags.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemFlags.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblItemFlags.Location = new System.Drawing.Point(8, 260);
      this.lblItemFlags.Name = "lblItemFlags";
      this.lblItemFlags.Size = new System.Drawing.Size(28, 16);
      this.lblItemFlags.TabIndex = 0;
      this.lblItemFlags.Text = "Flags:";
      // 
      // lblItemType
      // 
      this.lblItemType.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblItemType.Location = new System.Drawing.Point(196, 24);
      this.lblItemType.Name = "lblItemType";
      this.lblItemType.Size = new System.Drawing.Size(28, 16);
      this.lblItemType.TabIndex = 0;
      this.lblItemType.Text = "Type:";
      // 
      // lblItemID
      // 
      this.lblItemID.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemID.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblItemID.Location = new System.Drawing.Point(80, 24);
      this.lblItemID.Name = "lblItemID";
      this.lblItemID.Size = new System.Drawing.Size(20, 16);
      this.lblItemID.TabIndex = 0;
      this.lblItemID.Text = "ID:";
      // 
      // txtItemStackSize
      // 
      this.txtItemStackSize.Location = new System.Drawing.Point(384, 256);
      this.txtItemStackSize.Name = "txtItemStackSize";
      this.txtItemStackSize.ReadOnly = true;
      this.txtItemStackSize.Size = new System.Drawing.Size(32, 20);
      this.txtItemStackSize.TabIndex = 9;
      this.txtItemStackSize.Text = "";
      // 
      // txtItemFlags
      // 
      this.txtItemFlags.Location = new System.Drawing.Point(40, 256);
      this.txtItemFlags.Name = "txtItemFlags";
      this.txtItemFlags.ReadOnly = true;
      this.txtItemFlags.Size = new System.Drawing.Size(276, 20);
      this.txtItemFlags.TabIndex = 8;
      this.txtItemFlags.Text = "";
      // 
      // txtItemType
      // 
      this.txtItemType.Location = new System.Drawing.Point(228, 20);
      this.txtItemType.Name = "txtItemType";
      this.txtItemType.ReadOnly = true;
      this.txtItemType.Size = new System.Drawing.Size(188, 20);
      this.txtItemType.TabIndex = 2;
      this.txtItemType.Text = "";
      // 
      // txtItemID
      // 
      this.txtItemID.Location = new System.Drawing.Point(100, 20);
      this.txtItemID.Name = "txtItemID";
      this.txtItemID.ReadOnly = true;
      this.txtItemID.Size = new System.Drawing.Size(88, 20);
      this.txtItemID.TabIndex = 1;
      this.txtItemID.Text = "";
      // 
      // picItemIcon
      // 
      this.picItemIcon.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.picItemIcon.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.picItemIcon.Location = new System.Drawing.Point(8, 20);
      this.picItemIcon.Name = "picItemIcon";
      this.picItemIcon.Size = new System.Drawing.Size(64, 64);
      this.picItemIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.picItemIcon.TabIndex = 3;
      this.picItemIcon.TabStop = false;
      // 
      // FFXIItemEditor
      // 
      this.BackColor = System.Drawing.SystemColors.Control;
      this.Controls.Add(this.grpItemViewMode);
      this.Controls.Add(this.grpSpecializedItemInfo);
      this.Controls.Add(this.grpCommonItemInfo);
      this.Name = "FFXIItemEditor";
      this.Size = new System.Drawing.Size(424, 486);
      this.grpItemViewMode.ResumeLayout(false);
      this.grpSpecializedItemInfo.ResumeLayout(false);
      this.grpCommonItemInfo.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    #region Events - View Mode

    private void chkViewItem_Click(object sender, System.EventArgs e) {
      this.chkViewItemAsEArmor.Checked  = sender.Equals(this.chkViewItemAsEArmor);
      this.chkViewItemAsEObject.Checked = sender.Equals(this.chkViewItemAsEObject);
      this.chkViewItemAsEWeapon.Checked = sender.Equals(this.chkViewItemAsEWeapon);
      this.chkViewItemAsJArmor.Checked  = sender.Equals(this.chkViewItemAsJArmor);
      this.chkViewItemAsJObject.Checked = sender.Equals(this.chkViewItemAsJObject);
      this.chkViewItemAsJWeapon.Checked = sender.Equals(this.chkViewItemAsJWeapon);
    }

    private void FillCommonFields(FFXIItem.IItemInfo II) {
      this.txtItemEName.Text       = II.GetFieldText(ItemField.EnglishName);
      this.txtItemJName.Text       = II.GetFieldText(ItemField.JapaneseName);
      this.txtItemSingular.Text    = II.GetFieldText(ItemField.LogNameSingular);
      this.txtItemPlural.Text      = II.GetFieldText(ItemField.LogNamePlural);
      this.txtItemDescription.Text = II.GetFieldText(ItemField.Description).Replace("\n", "\r\n");
    }

    private void FillCommonExtraFields(FFXIItem.IItemInfo II) {
      this.txtItemResourceID.Text = II.GetFieldText(ItemField.ResourceID);
      this.txtItemLevel.Text      = II.GetFieldText(ItemField.Level);
      this.txtItemSlots.Text      = II.GetFieldText(ItemField.Slots);
      this.txtItemJobs.Text       = II.GetFieldText(ItemField.Jobs);
      this.txtItemRaces.Text      = II.GetFieldText(ItemField.Races);
      this.txtItemMaxCharges.Text = II.GetFieldText(ItemField.MaxCharges);
      this.txtItemEquipDelay.Text = II.GetFieldText(ItemField.EquipDelay);
      this.txtItemReuseTimer.Text = II.GetFieldText(ItemField.ReuseTimer);
    }

    private void FillObjectFields(FFXIItem.IItemInfo II) {
      this.FillCommonFields(II);
      this.grpSpecializedItemInfo.Visible = false;
    }

    private void FillArmorFields(FFXIItem.IItemInfo II) {
      this.FillCommonFields(II);
      this.grpSpecializedItemInfo.Visible = true;
      this.lblItemDamage.Visible = this.lblItemDelay.Visible = this.lblItemSkill.Visible = false;
      this.txtItemDamage.Visible = this.txtItemDelay.Visible = this.txtItemSkill.Visible = false;
      this.lblItemShieldSize.Visible = this.txtItemShieldSize.Visible = true;
      this.txtItemShieldSize.Text = II.GetFieldText(ItemField.ShieldSize);
      this.FillCommonExtraFields(II);
    }

    private void FillWeaponFields(FFXIItem.IItemInfo II) {
      this.FillCommonFields(II);
      this.grpSpecializedItemInfo.Visible = true;
      this.lblItemDamage.Visible = this.lblItemDelay.Visible = this.lblItemSkill.Visible = true;
      this.txtItemDamage.Visible = this.txtItemDelay.Visible = this.txtItemSkill.Visible = true;
      this.lblItemShieldSize.Visible = this.txtItemShieldSize.Visible = false;
      this.txtItemDamage.Text = II.GetFieldText(ItemField.Damage);
      this.txtItemDelay.Text  = II.GetFieldText(ItemField.Delay);
      this.txtItemSkill.Text  = II.GetFieldText(ItemField.Skill);
      this.FillCommonExtraFields(II);
    }

    private void chkViewItemAsEArmor_CheckedChanged(object sender, System.EventArgs e) {
      if (this.ItemToShow_ != null && this.chkViewItemAsEArmor.Checked)
	this.FillArmorFields(this.ItemToShow_.ENArmor);
    }

    private void chkViewItemAsEObject_CheckedChanged(object sender, System.EventArgs e) {
      if (this.ItemToShow_ != null && this.chkViewItemAsEObject.Checked)
	this.FillObjectFields(this.ItemToShow_.ENObject);
    }

    private void chkViewItemAsEWeapon_CheckedChanged(object sender, System.EventArgs e) {
      if (this.ItemToShow_ != null && this.chkViewItemAsEWeapon.Checked)
	this.FillWeaponFields(this.ItemToShow_.ENWeapon);
    }

    private void chkViewItemAsJArmor_CheckedChanged(object sender, System.EventArgs e) {
      if (this.ItemToShow_ != null && this.chkViewItemAsJArmor.Checked)
	this.FillArmorFields(this.ItemToShow_.JPArmor);
    }

    private void chkViewItemAsJObject_CheckedChanged(object sender, System.EventArgs e) {
      if (this.ItemToShow_ != null && this.chkViewItemAsJObject.Checked)
	this.FillObjectFields(this.ItemToShow_.JPObject);
    }

    private void chkViewItemAsJWeapon_CheckedChanged(object sender, System.EventArgs e) {
      if (this.ItemToShow_ != null && this.chkViewItemAsJWeapon.Checked)
	this.FillWeaponFields(this.ItemToShow_.JPWeapon);
    }

    #endregion

  }

}
