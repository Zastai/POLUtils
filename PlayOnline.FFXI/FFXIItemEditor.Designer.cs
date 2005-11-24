namespace PlayOnline.FFXI {

  public partial class FFXIItemEditor {

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

    #region Component Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FFXIItemEditor));
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
      ((System.ComponentModel.ISupportInitialize) (this.picIcon)).BeginInit();
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
      this.grpViewMode.Controls.Add(this.chkViewAsJWeapon);
      this.grpViewMode.Controls.Add(this.chkViewAsJObject);
      this.grpViewMode.Controls.Add(this.chkViewAsJArmor);
      this.grpViewMode.Controls.Add(this.chkViewAsEWeapon);
      this.grpViewMode.Controls.Add(this.chkViewAsEObject);
      this.grpViewMode.Controls.Add(this.chkViewAsEArmor);
      this.grpViewMode.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.grpViewMode, "grpViewMode");
      this.grpViewMode.Name = "grpViewMode";
      this.grpViewMode.TabStop = false;
      // 
      // chkViewAsJWeapon
      // 
      resources.ApplyResources(this.chkViewAsJWeapon, "chkViewAsJWeapon");
      this.chkViewAsJWeapon.AutoCheck = false;
      this.chkViewAsJWeapon.Name = "chkViewAsJWeapon";
      this.chkViewAsJWeapon.CheckedChanged += new System.EventHandler(this.chkViewAsJWeapon_CheckedChanged);
      this.chkViewAsJWeapon.Click += new System.EventHandler(this.chkView_Click);
      // 
      // chkViewAsJObject
      // 
      resources.ApplyResources(this.chkViewAsJObject, "chkViewAsJObject");
      this.chkViewAsJObject.AutoCheck = false;
      this.chkViewAsJObject.Name = "chkViewAsJObject";
      this.chkViewAsJObject.CheckedChanged += new System.EventHandler(this.chkViewAsJObject_CheckedChanged);
      this.chkViewAsJObject.Click += new System.EventHandler(this.chkView_Click);
      // 
      // chkViewAsJArmor
      // 
      resources.ApplyResources(this.chkViewAsJArmor, "chkViewAsJArmor");
      this.chkViewAsJArmor.AutoCheck = false;
      this.chkViewAsJArmor.Name = "chkViewAsJArmor";
      this.chkViewAsJArmor.CheckedChanged += new System.EventHandler(this.chkViewAsJArmor_CheckedChanged);
      this.chkViewAsJArmor.Click += new System.EventHandler(this.chkView_Click);
      // 
      // chkViewAsEWeapon
      // 
      resources.ApplyResources(this.chkViewAsEWeapon, "chkViewAsEWeapon");
      this.chkViewAsEWeapon.AutoCheck = false;
      this.chkViewAsEWeapon.Name = "chkViewAsEWeapon";
      this.chkViewAsEWeapon.CheckedChanged += new System.EventHandler(this.chkViewAsEWeapon_CheckedChanged);
      this.chkViewAsEWeapon.Click += new System.EventHandler(this.chkView_Click);
      // 
      // chkViewAsEObject
      // 
      resources.ApplyResources(this.chkViewAsEObject, "chkViewAsEObject");
      this.chkViewAsEObject.AutoCheck = false;
      this.chkViewAsEObject.Name = "chkViewAsEObject";
      this.chkViewAsEObject.CheckedChanged += new System.EventHandler(this.chkViewAsEObject_CheckedChanged);
      this.chkViewAsEObject.Click += new System.EventHandler(this.chkView_Click);
      // 
      // chkViewAsEArmor
      // 
      resources.ApplyResources(this.chkViewAsEArmor, "chkViewAsEArmor");
      this.chkViewAsEArmor.AutoCheck = false;
      this.chkViewAsEArmor.Name = "chkViewAsEArmor";
      this.chkViewAsEArmor.CheckedChanged += new System.EventHandler(this.chkViewAsEArmor_CheckedChanged);
      this.chkViewAsEArmor.Click += new System.EventHandler(this.chkView_Click);
      // 
      // grpEquipmentInfo
      // 
      this.grpEquipmentInfo.Controls.Add(this.lblRaces);
      this.grpEquipmentInfo.Controls.Add(this.txtRaces);
      this.grpEquipmentInfo.Controls.Add(this.lblSlots);
      this.grpEquipmentInfo.Controls.Add(this.txtSlots);
      this.grpEquipmentInfo.Controls.Add(this.lblJobs);
      this.grpEquipmentInfo.Controls.Add(this.txtJobs);
      this.grpEquipmentInfo.Controls.Add(this.lblLevel);
      this.grpEquipmentInfo.Controls.Add(this.txtLevel);
      this.grpEquipmentInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.grpEquipmentInfo, "grpEquipmentInfo");
      this.grpEquipmentInfo.Name = "grpEquipmentInfo";
      this.grpEquipmentInfo.TabStop = false;
      // 
      // lblRaces
      // 
      this.lblRaces.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblRaces, "lblRaces");
      this.lblRaces.Name = "lblRaces";
      // 
      // txtRaces
      // 
      resources.ApplyResources(this.txtRaces, "txtRaces");
      this.txtRaces.Name = "txtRaces";
      this.txtRaces.ReadOnly = true;
      // 
      // lblSlots
      // 
      this.lblSlots.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblSlots, "lblSlots");
      this.lblSlots.Name = "lblSlots";
      // 
      // txtSlots
      // 
      resources.ApplyResources(this.txtSlots, "txtSlots");
      this.txtSlots.Name = "txtSlots";
      this.txtSlots.ReadOnly = true;
      // 
      // lblJobs
      // 
      this.lblJobs.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblJobs, "lblJobs");
      this.lblJobs.Name = "lblJobs";
      // 
      // txtJobs
      // 
      resources.ApplyResources(this.txtJobs, "txtJobs");
      this.txtJobs.Name = "txtJobs";
      this.txtJobs.ReadOnly = true;
      // 
      // lblLevel
      // 
      this.lblLevel.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblLevel, "lblLevel");
      this.lblLevel.Name = "lblLevel";
      // 
      // txtLevel
      // 
      resources.ApplyResources(this.txtLevel, "txtLevel");
      this.txtLevel.Name = "txtLevel";
      this.txtLevel.ReadOnly = true;
      // 
      // grpCommonInfo
      // 
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
      this.grpCommonInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.grpCommonInfo, "grpCommonInfo");
      this.grpCommonInfo.Name = "grpCommonInfo";
      this.grpCommonInfo.TabStop = false;
      // 
      // lblValidTargets
      // 
      this.lblValidTargets.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblValidTargets, "lblValidTargets");
      this.lblValidTargets.Name = "lblValidTargets";
      // 
      // txtValidTargets
      // 
      resources.ApplyResources(this.txtValidTargets, "txtValidTargets");
      this.txtValidTargets.Name = "txtValidTargets";
      this.txtValidTargets.ReadOnly = true;
      // 
      // lblDescription
      // 
      this.lblDescription.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblDescription, "lblDescription");
      this.lblDescription.Name = "lblDescription";
      // 
      // lblJName
      // 
      this.lblJName.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblJName, "lblJName");
      this.lblJName.Name = "lblJName";
      // 
      // lblEName
      // 
      this.lblEName.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblEName, "lblEName");
      this.lblEName.Name = "lblEName";
      // 
      // txtJName
      // 
      resources.ApplyResources(this.txtJName, "txtJName");
      this.txtJName.Name = "txtJName";
      this.txtJName.ReadOnly = true;
      // 
      // txtEName
      // 
      resources.ApplyResources(this.txtEName, "txtEName");
      this.txtEName.Name = "txtEName";
      this.txtEName.ReadOnly = true;
      // 
      // txtDescription
      // 
      resources.ApplyResources(this.txtDescription, "txtDescription");
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.ReadOnly = true;
      // 
      // lblStackSize
      // 
      this.lblStackSize.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblStackSize, "lblStackSize");
      this.lblStackSize.Name = "lblStackSize";
      // 
      // lblFlags
      // 
      this.lblFlags.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblFlags, "lblFlags");
      this.lblFlags.Name = "lblFlags";
      // 
      // lblType
      // 
      this.lblType.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblType, "lblType");
      this.lblType.Name = "lblType";
      // 
      // lblID
      // 
      this.lblID.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblID, "lblID");
      this.lblID.Name = "lblID";
      // 
      // txtStackSize
      // 
      resources.ApplyResources(this.txtStackSize, "txtStackSize");
      this.txtStackSize.Name = "txtStackSize";
      this.txtStackSize.ReadOnly = true;
      // 
      // txtFlags
      // 
      resources.ApplyResources(this.txtFlags, "txtFlags");
      this.txtFlags.Name = "txtFlags";
      this.txtFlags.ReadOnly = true;
      // 
      // txtType
      // 
      resources.ApplyResources(this.txtType, "txtType");
      this.txtType.Name = "txtType";
      this.txtType.ReadOnly = true;
      // 
      // txtID
      // 
      resources.ApplyResources(this.txtID, "txtID");
      this.txtID.Name = "txtID";
      this.txtID.ReadOnly = true;
      // 
      // picIcon
      // 
      this.picIcon.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      resources.ApplyResources(this.picIcon, "picIcon");
      this.picIcon.Name = "picIcon";
      this.picIcon.TabStop = false;
      // 
      // grpWeaponInfo
      // 
      this.grpWeaponInfo.Controls.Add(this.lblDPS);
      this.grpWeaponInfo.Controls.Add(this.txtDPS);
      this.grpWeaponInfo.Controls.Add(this.lblDelay);
      this.grpWeaponInfo.Controls.Add(this.lblDamage);
      this.grpWeaponInfo.Controls.Add(this.txtDelay);
      this.grpWeaponInfo.Controls.Add(this.txtDamage);
      this.grpWeaponInfo.Controls.Add(this.lblSkill);
      this.grpWeaponInfo.Controls.Add(this.txtSkill);
      this.grpWeaponInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.grpWeaponInfo, "grpWeaponInfo");
      this.grpWeaponInfo.Name = "grpWeaponInfo";
      this.grpWeaponInfo.TabStop = false;
      // 
      // lblDPS
      // 
      this.lblDPS.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblDPS, "lblDPS");
      this.lblDPS.Name = "lblDPS";
      // 
      // txtDPS
      // 
      resources.ApplyResources(this.txtDPS, "txtDPS");
      this.txtDPS.Name = "txtDPS";
      this.txtDPS.ReadOnly = true;
      // 
      // lblDelay
      // 
      this.lblDelay.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblDelay, "lblDelay");
      this.lblDelay.Name = "lblDelay";
      // 
      // lblDamage
      // 
      this.lblDamage.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblDamage, "lblDamage");
      this.lblDamage.Name = "lblDamage";
      // 
      // txtDelay
      // 
      resources.ApplyResources(this.txtDelay, "txtDelay");
      this.txtDelay.Name = "txtDelay";
      this.txtDelay.ReadOnly = true;
      // 
      // txtDamage
      // 
      resources.ApplyResources(this.txtDamage, "txtDamage");
      this.txtDamage.Name = "txtDamage";
      this.txtDamage.ReadOnly = true;
      // 
      // lblSkill
      // 
      this.lblSkill.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblSkill, "lblSkill");
      this.lblSkill.Name = "lblSkill";
      // 
      // txtSkill
      // 
      resources.ApplyResources(this.txtSkill, "txtSkill");
      this.txtSkill.Name = "txtSkill";
      this.txtSkill.ReadOnly = true;
      // 
      // grpFurnitureInfo
      // 
      this.grpFurnitureInfo.Controls.Add(this.lblStorage);
      this.grpFurnitureInfo.Controls.Add(this.txtStorage);
      this.grpFurnitureInfo.Controls.Add(this.lblElement);
      this.grpFurnitureInfo.Controls.Add(this.txtElement);
      this.grpFurnitureInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.grpFurnitureInfo, "grpFurnitureInfo");
      this.grpFurnitureInfo.Name = "grpFurnitureInfo";
      this.grpFurnitureInfo.TabStop = false;
      // 
      // lblStorage
      // 
      this.lblStorage.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblStorage, "lblStorage");
      this.lblStorage.Name = "lblStorage";
      // 
      // txtStorage
      // 
      resources.ApplyResources(this.txtStorage, "txtStorage");
      this.txtStorage.Name = "txtStorage";
      this.txtStorage.ReadOnly = true;
      // 
      // lblElement
      // 
      this.lblElement.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblElement, "lblElement");
      this.lblElement.Name = "lblElement";
      // 
      // txtElement
      // 
      resources.ApplyResources(this.txtElement, "txtElement");
      this.txtElement.Name = "txtElement";
      this.txtElement.ReadOnly = true;
      // 
      // grpShieldInfo
      // 
      this.grpShieldInfo.Controls.Add(this.lblShieldSize);
      this.grpShieldInfo.Controls.Add(this.txtShieldSize);
      this.grpShieldInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.grpShieldInfo, "grpShieldInfo");
      this.grpShieldInfo.Name = "grpShieldInfo";
      this.grpShieldInfo.TabStop = false;
      // 
      // lblShieldSize
      // 
      this.lblShieldSize.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblShieldSize, "lblShieldSize");
      this.lblShieldSize.Name = "lblShieldSize";
      // 
      // txtShieldSize
      // 
      resources.ApplyResources(this.txtShieldSize, "txtShieldSize");
      this.txtShieldSize.Name = "txtShieldSize";
      this.txtShieldSize.ReadOnly = true;
      // 
      // grpJugInfo
      // 
      this.grpJugInfo.Controls.Add(this.lblJugSize);
      this.grpJugInfo.Controls.Add(this.txtJugSize);
      this.grpJugInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.grpJugInfo, "grpJugInfo");
      this.grpJugInfo.Name = "grpJugInfo";
      this.grpJugInfo.TabStop = false;
      // 
      // lblJugSize
      // 
      this.lblJugSize.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblJugSize, "lblJugSize");
      this.lblJugSize.Name = "lblJugSize";
      // 
      // txtJugSize
      // 
      resources.ApplyResources(this.txtJugSize, "txtJugSize");
      this.txtJugSize.Name = "txtJugSize";
      this.txtJugSize.ReadOnly = true;
      // 
      // grpEnchantmentInfo
      // 
      this.grpEnchantmentInfo.Controls.Add(this.lblCastTime);
      this.grpEnchantmentInfo.Controls.Add(this.txtCastTime);
      this.grpEnchantmentInfo.Controls.Add(this.lblEquipDelay);
      this.grpEnchantmentInfo.Controls.Add(this.lblReuseTimer);
      this.grpEnchantmentInfo.Controls.Add(this.lblMaxCharges);
      this.grpEnchantmentInfo.Controls.Add(this.txtReuseTimer);
      this.grpEnchantmentInfo.Controls.Add(this.txtEquipDelay);
      this.grpEnchantmentInfo.Controls.Add(this.txtMaxCharges);
      this.grpEnchantmentInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.grpEnchantmentInfo, "grpEnchantmentInfo");
      this.grpEnchantmentInfo.Name = "grpEnchantmentInfo";
      this.grpEnchantmentInfo.TabStop = false;
      // 
      // lblCastTime
      // 
      this.lblCastTime.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblCastTime, "lblCastTime");
      this.lblCastTime.Name = "lblCastTime";
      // 
      // txtCastTime
      // 
      resources.ApplyResources(this.txtCastTime, "txtCastTime");
      this.txtCastTime.Name = "txtCastTime";
      this.txtCastTime.ReadOnly = true;
      // 
      // lblEquipDelay
      // 
      this.lblEquipDelay.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblEquipDelay, "lblEquipDelay");
      this.lblEquipDelay.Name = "lblEquipDelay";
      // 
      // lblReuseTimer
      // 
      this.lblReuseTimer.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblReuseTimer, "lblReuseTimer");
      this.lblReuseTimer.Name = "lblReuseTimer";
      // 
      // lblMaxCharges
      // 
      this.lblMaxCharges.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblMaxCharges, "lblMaxCharges");
      this.lblMaxCharges.Name = "lblMaxCharges";
      // 
      // txtReuseTimer
      // 
      resources.ApplyResources(this.txtReuseTimer, "txtReuseTimer");
      this.txtReuseTimer.Name = "txtReuseTimer";
      this.txtReuseTimer.ReadOnly = true;
      // 
      // txtEquipDelay
      // 
      resources.ApplyResources(this.txtEquipDelay, "txtEquipDelay");
      this.txtEquipDelay.Name = "txtEquipDelay";
      this.txtEquipDelay.ReadOnly = true;
      // 
      // txtMaxCharges
      // 
      resources.ApplyResources(this.txtMaxCharges, "txtMaxCharges");
      this.txtMaxCharges.Name = "txtMaxCharges";
      this.txtMaxCharges.ReadOnly = true;
      // 
      // grpLogStrings
      // 
      this.grpLogStrings.Controls.Add(this.lblPlural);
      this.grpLogStrings.Controls.Add(this.lblSingular);
      this.grpLogStrings.Controls.Add(this.txtPlural);
      this.grpLogStrings.Controls.Add(this.txtSingular);
      this.grpLogStrings.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.grpLogStrings, "grpLogStrings");
      this.grpLogStrings.Name = "grpLogStrings";
      this.grpLogStrings.TabStop = false;
      // 
      // lblPlural
      // 
      this.lblPlural.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblPlural, "lblPlural");
      this.lblPlural.Name = "lblPlural";
      // 
      // lblSingular
      // 
      this.lblSingular.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblSingular, "lblSingular");
      this.lblSingular.Name = "lblSingular";
      // 
      // txtPlural
      // 
      resources.ApplyResources(this.txtPlural, "txtPlural");
      this.txtPlural.Name = "txtPlural";
      this.txtPlural.ReadOnly = true;
      // 
      // txtSingular
      // 
      resources.ApplyResources(this.txtSingular, "txtSingular");
      this.txtSingular.Name = "txtSingular";
      this.txtSingular.ReadOnly = true;
      // 
      // FFXIItemEditor
      // 
      this.BackColor = System.Drawing.SystemColors.Control;
      this.Controls.Add(this.grpLogStrings);
      this.Controls.Add(this.grpEquipmentInfo);
      this.Controls.Add(this.grpEnchantmentInfo);
      this.Controls.Add(this.grpJugInfo);
      this.Controls.Add(this.grpShieldInfo);
      this.Controls.Add(this.grpFurnitureInfo);
      this.Controls.Add(this.grpWeaponInfo);
      this.Controls.Add(this.grpViewMode);
      this.Controls.Add(this.grpCommonInfo);
      this.Name = "FFXIItemEditor";
      resources.ApplyResources(this, "$this");
      this.grpViewMode.ResumeLayout(false);
      this.grpEquipmentInfo.ResumeLayout(false);
      this.grpEquipmentInfo.PerformLayout();
      this.grpCommonInfo.ResumeLayout(false);
      this.grpCommonInfo.PerformLayout();
      ((System.ComponentModel.ISupportInitialize) (this.picIcon)).EndInit();
      this.grpWeaponInfo.ResumeLayout(false);
      this.grpWeaponInfo.PerformLayout();
      this.grpFurnitureInfo.ResumeLayout(false);
      this.grpFurnitureInfo.PerformLayout();
      this.grpShieldInfo.ResumeLayout(false);
      this.grpShieldInfo.PerformLayout();
      this.grpJugInfo.ResumeLayout(false);
      this.grpJugInfo.PerformLayout();
      this.grpEnchantmentInfo.ResumeLayout(false);
      this.grpEnchantmentInfo.PerformLayout();
      this.grpLogStrings.ResumeLayout(false);
      this.grpLogStrings.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

  }

}