// $Id$

namespace PlayOnline.FFXI {

  public partial class ItemEditor {

    private System.ComponentModel.IContainer components;

    protected override void Dispose(bool disposing) {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    private void InitializeComponent() {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemEditor));
      this.grpEquipmentInfo = new System.Windows.Forms.GroupBox();
      this.txtRaces = new System.Windows.Forms.TextBox();
      this.txtSlots = new System.Windows.Forms.TextBox();
      this.txtJobs = new System.Windows.Forms.TextBox();
      this.txtLevel = new System.Windows.Forms.TextBox();
      this.lblRaces = new System.Windows.Forms.Label();
      this.lblSlots = new System.Windows.Forms.Label();
      this.lblJobs = new System.Windows.Forms.Label();
      this.lblLevel = new System.Windows.Forms.Label();
      this.grpCommonInfo = new System.Windows.Forms.GroupBox();
      this.txtValidTargets = new System.Windows.Forms.TextBox();
      this.txtJName = new System.Windows.Forms.TextBox();
      this.txtEName = new System.Windows.Forms.TextBox();
      this.txtDescription = new System.Windows.Forms.TextBox();
      this.txtStackSize = new System.Windows.Forms.TextBox();
      this.txtFlags = new System.Windows.Forms.TextBox();
      this.txtType = new System.Windows.Forms.TextBox();
      this.txtID = new System.Windows.Forms.TextBox();
      this.picIcon = new System.Windows.Forms.PictureBox();
      this.lblValidTargets = new System.Windows.Forms.Label();
      this.lblDescription = new System.Windows.Forms.Label();
      this.lblJName = new System.Windows.Forms.Label();
      this.lblEName = new System.Windows.Forms.Label();
      this.lblStackSize = new System.Windows.Forms.Label();
      this.lblFlags = new System.Windows.Forms.Label();
      this.lblType = new System.Windows.Forms.Label();
      this.lblID = new System.Windows.Forms.Label();
      this.ttToolTip = new System.Windows.Forms.ToolTip(this.components);
      this.grpWeaponInfo = new System.Windows.Forms.GroupBox();
      this.txtJugSize = new System.Windows.Forms.TextBox();
      this.lblJugSize = new System.Windows.Forms.Label();
      this.txtDPS = new System.Windows.Forms.TextBox();
      this.txtDelay = new System.Windows.Forms.TextBox();
      this.txtDamage = new System.Windows.Forms.TextBox();
      this.txtSkill = new System.Windows.Forms.TextBox();
      this.lblDPS = new System.Windows.Forms.Label();
      this.lblDelay = new System.Windows.Forms.Label();
      this.lblDamage = new System.Windows.Forms.Label();
      this.lblSkill = new System.Windows.Forms.Label();
      this.grpFurnitureInfo = new System.Windows.Forms.GroupBox();
      this.txtStorage = new System.Windows.Forms.TextBox();
      this.txtElement = new System.Windows.Forms.TextBox();
      this.lblStorage = new System.Windows.Forms.Label();
      this.lblElement = new System.Windows.Forms.Label();
      this.grpShieldInfo = new System.Windows.Forms.GroupBox();
      this.txtShieldSize = new System.Windows.Forms.TextBox();
      this.lblShieldSize = new System.Windows.Forms.Label();
      this.grpEnchantmentInfo = new System.Windows.Forms.GroupBox();
      this.txtCastTime = new System.Windows.Forms.TextBox();
      this.txtReuseTimer = new System.Windows.Forms.TextBox();
      this.txtEquipDelay = new System.Windows.Forms.TextBox();
      this.txtMaxCharges = new System.Windows.Forms.TextBox();
      this.lblCastTime = new System.Windows.Forms.Label();
      this.lblEquipDelay = new System.Windows.Forms.Label();
      this.lblReuseTimer = new System.Windows.Forms.Label();
      this.lblMaxCharges = new System.Windows.Forms.Label();
      this.grpLogStrings = new System.Windows.Forms.GroupBox();
      this.txtPlural = new System.Windows.Forms.TextBox();
      this.txtSingular = new System.Windows.Forms.TextBox();
      this.lblPlural = new System.Windows.Forms.Label();
      this.lblSingular = new System.Windows.Forms.Label();
      this.grpUsableItemInfo = new System.Windows.Forms.GroupBox();
      this.txtActivationTime = new System.Windows.Forms.TextBox();
      this.lblActivationTime = new System.Windows.Forms.Label();
      this.grpEquipmentInfo.SuspendLayout();
      this.grpCommonInfo.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize) (this.picIcon)).BeginInit();
      this.grpWeaponInfo.SuspendLayout();
      this.grpFurnitureInfo.SuspendLayout();
      this.grpShieldInfo.SuspendLayout();
      this.grpEnchantmentInfo.SuspendLayout();
      this.grpLogStrings.SuspendLayout();
      this.grpUsableItemInfo.SuspendLayout();
      this.SuspendLayout();
      // 
      // grpEquipmentInfo
      // 
      this.grpEquipmentInfo.AccessibleDescription = null;
      this.grpEquipmentInfo.AccessibleName = null;
      resources.ApplyResources(this.grpEquipmentInfo, "grpEquipmentInfo");
      this.grpEquipmentInfo.BackColor = System.Drawing.Color.Transparent;
      this.grpEquipmentInfo.BackgroundImage = null;
      this.grpEquipmentInfo.Controls.Add(this.txtRaces);
      this.grpEquipmentInfo.Controls.Add(this.txtSlots);
      this.grpEquipmentInfo.Controls.Add(this.txtJobs);
      this.grpEquipmentInfo.Controls.Add(this.txtLevel);
      this.grpEquipmentInfo.Controls.Add(this.lblRaces);
      this.grpEquipmentInfo.Controls.Add(this.lblSlots);
      this.grpEquipmentInfo.Controls.Add(this.lblJobs);
      this.grpEquipmentInfo.Controls.Add(this.lblLevel);
      this.grpEquipmentInfo.Font = null;
      this.grpEquipmentInfo.Name = "grpEquipmentInfo";
      this.grpEquipmentInfo.TabStop = false;
      this.ttToolTip.SetToolTip(this.grpEquipmentInfo, resources.GetString("grpEquipmentInfo.ToolTip"));
      // 
      // txtRaces
      // 
      this.txtRaces.AccessibleDescription = null;
      this.txtRaces.AccessibleName = null;
      resources.ApplyResources(this.txtRaces, "txtRaces");
      this.txtRaces.BackgroundImage = null;
      this.txtRaces.Font = null;
      this.txtRaces.Name = "txtRaces";
      this.txtRaces.ReadOnly = true;
      this.ttToolTip.SetToolTip(this.txtRaces, resources.GetString("txtRaces.ToolTip"));
      // 
      // txtSlots
      // 
      this.txtSlots.AccessibleDescription = null;
      this.txtSlots.AccessibleName = null;
      resources.ApplyResources(this.txtSlots, "txtSlots");
      this.txtSlots.BackgroundImage = null;
      this.txtSlots.Font = null;
      this.txtSlots.Name = "txtSlots";
      this.txtSlots.ReadOnly = true;
      this.ttToolTip.SetToolTip(this.txtSlots, resources.GetString("txtSlots.ToolTip"));
      // 
      // txtJobs
      // 
      this.txtJobs.AccessibleDescription = null;
      this.txtJobs.AccessibleName = null;
      resources.ApplyResources(this.txtJobs, "txtJobs");
      this.txtJobs.BackgroundImage = null;
      this.txtJobs.Font = null;
      this.txtJobs.Name = "txtJobs";
      this.txtJobs.ReadOnly = true;
      this.ttToolTip.SetToolTip(this.txtJobs, resources.GetString("txtJobs.ToolTip"));
      // 
      // txtLevel
      // 
      this.txtLevel.AccessibleDescription = null;
      this.txtLevel.AccessibleName = null;
      resources.ApplyResources(this.txtLevel, "txtLevel");
      this.txtLevel.BackgroundImage = null;
      this.txtLevel.Font = null;
      this.txtLevel.Name = "txtLevel";
      this.txtLevel.ReadOnly = true;
      this.ttToolTip.SetToolTip(this.txtLevel, resources.GetString("txtLevel.ToolTip"));
      // 
      // lblRaces
      // 
      this.lblRaces.AccessibleDescription = null;
      this.lblRaces.AccessibleName = null;
      resources.ApplyResources(this.lblRaces, "lblRaces");
      this.lblRaces.Font = null;
      this.lblRaces.Name = "lblRaces";
      this.ttToolTip.SetToolTip(this.lblRaces, resources.GetString("lblRaces.ToolTip"));
      // 
      // lblSlots
      // 
      this.lblSlots.AccessibleDescription = null;
      this.lblSlots.AccessibleName = null;
      resources.ApplyResources(this.lblSlots, "lblSlots");
      this.lblSlots.Font = null;
      this.lblSlots.Name = "lblSlots";
      this.ttToolTip.SetToolTip(this.lblSlots, resources.GetString("lblSlots.ToolTip"));
      // 
      // lblJobs
      // 
      this.lblJobs.AccessibleDescription = null;
      this.lblJobs.AccessibleName = null;
      resources.ApplyResources(this.lblJobs, "lblJobs");
      this.lblJobs.Font = null;
      this.lblJobs.Name = "lblJobs";
      this.ttToolTip.SetToolTip(this.lblJobs, resources.GetString("lblJobs.ToolTip"));
      // 
      // lblLevel
      // 
      this.lblLevel.AccessibleDescription = null;
      this.lblLevel.AccessibleName = null;
      resources.ApplyResources(this.lblLevel, "lblLevel");
      this.lblLevel.Font = null;
      this.lblLevel.Name = "lblLevel";
      this.ttToolTip.SetToolTip(this.lblLevel, resources.GetString("lblLevel.ToolTip"));
      // 
      // grpCommonInfo
      // 
      this.grpCommonInfo.AccessibleDescription = null;
      this.grpCommonInfo.AccessibleName = null;
      resources.ApplyResources(this.grpCommonInfo, "grpCommonInfo");
      this.grpCommonInfo.BackColor = System.Drawing.Color.Transparent;
      this.grpCommonInfo.BackgroundImage = null;
      this.grpCommonInfo.Controls.Add(this.txtValidTargets);
      this.grpCommonInfo.Controls.Add(this.txtJName);
      this.grpCommonInfo.Controls.Add(this.txtEName);
      this.grpCommonInfo.Controls.Add(this.txtDescription);
      this.grpCommonInfo.Controls.Add(this.txtStackSize);
      this.grpCommonInfo.Controls.Add(this.txtFlags);
      this.grpCommonInfo.Controls.Add(this.txtType);
      this.grpCommonInfo.Controls.Add(this.txtID);
      this.grpCommonInfo.Controls.Add(this.picIcon);
      this.grpCommonInfo.Controls.Add(this.lblValidTargets);
      this.grpCommonInfo.Controls.Add(this.lblDescription);
      this.grpCommonInfo.Controls.Add(this.lblJName);
      this.grpCommonInfo.Controls.Add(this.lblEName);
      this.grpCommonInfo.Controls.Add(this.lblStackSize);
      this.grpCommonInfo.Controls.Add(this.lblFlags);
      this.grpCommonInfo.Controls.Add(this.lblType);
      this.grpCommonInfo.Controls.Add(this.lblID);
      this.grpCommonInfo.Font = null;
      this.grpCommonInfo.Name = "grpCommonInfo";
      this.grpCommonInfo.TabStop = false;
      this.ttToolTip.SetToolTip(this.grpCommonInfo, resources.GetString("grpCommonInfo.ToolTip"));
      // 
      // txtValidTargets
      // 
      this.txtValidTargets.AccessibleDescription = null;
      this.txtValidTargets.AccessibleName = null;
      resources.ApplyResources(this.txtValidTargets, "txtValidTargets");
      this.txtValidTargets.BackgroundImage = null;
      this.txtValidTargets.Font = null;
      this.txtValidTargets.Name = "txtValidTargets";
      this.txtValidTargets.ReadOnly = true;
      this.ttToolTip.SetToolTip(this.txtValidTargets, resources.GetString("txtValidTargets.ToolTip"));
      // 
      // txtJName
      // 
      this.txtJName.AccessibleDescription = null;
      this.txtJName.AccessibleName = null;
      resources.ApplyResources(this.txtJName, "txtJName");
      this.txtJName.BackgroundImage = null;
      this.txtJName.Font = null;
      this.txtJName.Name = "txtJName";
      this.txtJName.ReadOnly = true;
      this.ttToolTip.SetToolTip(this.txtJName, resources.GetString("txtJName.ToolTip"));
      // 
      // txtEName
      // 
      this.txtEName.AccessibleDescription = null;
      this.txtEName.AccessibleName = null;
      resources.ApplyResources(this.txtEName, "txtEName");
      this.txtEName.BackgroundImage = null;
      this.txtEName.Font = null;
      this.txtEName.Name = "txtEName";
      this.txtEName.ReadOnly = true;
      this.ttToolTip.SetToolTip(this.txtEName, resources.GetString("txtEName.ToolTip"));
      // 
      // txtDescription
      // 
      this.txtDescription.AccessibleDescription = null;
      this.txtDescription.AccessibleName = null;
      resources.ApplyResources(this.txtDescription, "txtDescription");
      this.txtDescription.BackgroundImage = null;
      this.txtDescription.Font = null;
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.ReadOnly = true;
      this.ttToolTip.SetToolTip(this.txtDescription, resources.GetString("txtDescription.ToolTip"));
      // 
      // txtStackSize
      // 
      this.txtStackSize.AccessibleDescription = null;
      this.txtStackSize.AccessibleName = null;
      resources.ApplyResources(this.txtStackSize, "txtStackSize");
      this.txtStackSize.BackgroundImage = null;
      this.txtStackSize.Font = null;
      this.txtStackSize.Name = "txtStackSize";
      this.txtStackSize.ReadOnly = true;
      this.ttToolTip.SetToolTip(this.txtStackSize, resources.GetString("txtStackSize.ToolTip"));
      // 
      // txtFlags
      // 
      this.txtFlags.AccessibleDescription = null;
      this.txtFlags.AccessibleName = null;
      resources.ApplyResources(this.txtFlags, "txtFlags");
      this.txtFlags.BackgroundImage = null;
      this.txtFlags.Font = null;
      this.txtFlags.Name = "txtFlags";
      this.txtFlags.ReadOnly = true;
      this.ttToolTip.SetToolTip(this.txtFlags, resources.GetString("txtFlags.ToolTip"));
      // 
      // txtType
      // 
      this.txtType.AccessibleDescription = null;
      this.txtType.AccessibleName = null;
      resources.ApplyResources(this.txtType, "txtType");
      this.txtType.BackgroundImage = null;
      this.txtType.Font = null;
      this.txtType.Name = "txtType";
      this.txtType.ReadOnly = true;
      this.ttToolTip.SetToolTip(this.txtType, resources.GetString("txtType.ToolTip"));
      // 
      // txtID
      // 
      this.txtID.AccessibleDescription = null;
      this.txtID.AccessibleName = null;
      resources.ApplyResources(this.txtID, "txtID");
      this.txtID.BackgroundImage = null;
      this.txtID.Font = null;
      this.txtID.Name = "txtID";
      this.txtID.ReadOnly = true;
      this.ttToolTip.SetToolTip(this.txtID, resources.GetString("txtID.ToolTip"));
      // 
      // picIcon
      // 
      this.picIcon.AccessibleDescription = null;
      this.picIcon.AccessibleName = null;
      resources.ApplyResources(this.picIcon, "picIcon");
      this.picIcon.BackgroundImage = null;
      this.picIcon.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.picIcon.Font = null;
      this.picIcon.ImageLocation = null;
      this.picIcon.Name = "picIcon";
      this.picIcon.TabStop = false;
      this.ttToolTip.SetToolTip(this.picIcon, resources.GetString("picIcon.ToolTip"));
      // 
      // lblValidTargets
      // 
      this.lblValidTargets.AccessibleDescription = null;
      this.lblValidTargets.AccessibleName = null;
      resources.ApplyResources(this.lblValidTargets, "lblValidTargets");
      this.lblValidTargets.Font = null;
      this.lblValidTargets.Name = "lblValidTargets";
      this.ttToolTip.SetToolTip(this.lblValidTargets, resources.GetString("lblValidTargets.ToolTip"));
      // 
      // lblDescription
      // 
      this.lblDescription.AccessibleDescription = null;
      this.lblDescription.AccessibleName = null;
      resources.ApplyResources(this.lblDescription, "lblDescription");
      this.lblDescription.Font = null;
      this.lblDescription.Name = "lblDescription";
      this.ttToolTip.SetToolTip(this.lblDescription, resources.GetString("lblDescription.ToolTip"));
      // 
      // lblJName
      // 
      this.lblJName.AccessibleDescription = null;
      this.lblJName.AccessibleName = null;
      resources.ApplyResources(this.lblJName, "lblJName");
      this.lblJName.Font = null;
      this.lblJName.Name = "lblJName";
      this.ttToolTip.SetToolTip(this.lblJName, resources.GetString("lblJName.ToolTip"));
      // 
      // lblEName
      // 
      this.lblEName.AccessibleDescription = null;
      this.lblEName.AccessibleName = null;
      resources.ApplyResources(this.lblEName, "lblEName");
      this.lblEName.Font = null;
      this.lblEName.Name = "lblEName";
      this.ttToolTip.SetToolTip(this.lblEName, resources.GetString("lblEName.ToolTip"));
      // 
      // lblStackSize
      // 
      this.lblStackSize.AccessibleDescription = null;
      this.lblStackSize.AccessibleName = null;
      resources.ApplyResources(this.lblStackSize, "lblStackSize");
      this.lblStackSize.Font = null;
      this.lblStackSize.Name = "lblStackSize";
      this.ttToolTip.SetToolTip(this.lblStackSize, resources.GetString("lblStackSize.ToolTip"));
      // 
      // lblFlags
      // 
      this.lblFlags.AccessibleDescription = null;
      this.lblFlags.AccessibleName = null;
      resources.ApplyResources(this.lblFlags, "lblFlags");
      this.lblFlags.Font = null;
      this.lblFlags.Name = "lblFlags";
      this.ttToolTip.SetToolTip(this.lblFlags, resources.GetString("lblFlags.ToolTip"));
      // 
      // lblType
      // 
      this.lblType.AccessibleDescription = null;
      this.lblType.AccessibleName = null;
      resources.ApplyResources(this.lblType, "lblType");
      this.lblType.Font = null;
      this.lblType.Name = "lblType";
      this.ttToolTip.SetToolTip(this.lblType, resources.GetString("lblType.ToolTip"));
      // 
      // lblID
      // 
      this.lblID.AccessibleDescription = null;
      this.lblID.AccessibleName = null;
      resources.ApplyResources(this.lblID, "lblID");
      this.lblID.Font = null;
      this.lblID.Name = "lblID";
      this.ttToolTip.SetToolTip(this.lblID, resources.GetString("lblID.ToolTip"));
      // 
      // grpWeaponInfo
      // 
      this.grpWeaponInfo.AccessibleDescription = null;
      this.grpWeaponInfo.AccessibleName = null;
      resources.ApplyResources(this.grpWeaponInfo, "grpWeaponInfo");
      this.grpWeaponInfo.BackColor = System.Drawing.Color.Transparent;
      this.grpWeaponInfo.BackgroundImage = null;
      this.grpWeaponInfo.Controls.Add(this.txtJugSize);
      this.grpWeaponInfo.Controls.Add(this.lblJugSize);
      this.grpWeaponInfo.Controls.Add(this.txtDPS);
      this.grpWeaponInfo.Controls.Add(this.txtDelay);
      this.grpWeaponInfo.Controls.Add(this.txtDamage);
      this.grpWeaponInfo.Controls.Add(this.txtSkill);
      this.grpWeaponInfo.Controls.Add(this.lblDPS);
      this.grpWeaponInfo.Controls.Add(this.lblDelay);
      this.grpWeaponInfo.Controls.Add(this.lblDamage);
      this.grpWeaponInfo.Controls.Add(this.lblSkill);
      this.grpWeaponInfo.Font = null;
      this.grpWeaponInfo.Name = "grpWeaponInfo";
      this.grpWeaponInfo.TabStop = false;
      this.ttToolTip.SetToolTip(this.grpWeaponInfo, resources.GetString("grpWeaponInfo.ToolTip"));
      // 
      // txtJugSize
      // 
      this.txtJugSize.AccessibleDescription = null;
      this.txtJugSize.AccessibleName = null;
      resources.ApplyResources(this.txtJugSize, "txtJugSize");
      this.txtJugSize.BackgroundImage = null;
      this.txtJugSize.Font = null;
      this.txtJugSize.Name = "txtJugSize";
      this.txtJugSize.ReadOnly = true;
      this.ttToolTip.SetToolTip(this.txtJugSize, resources.GetString("txtJugSize.ToolTip"));
      // 
      // lblJugSize
      // 
      this.lblJugSize.AccessibleDescription = null;
      this.lblJugSize.AccessibleName = null;
      resources.ApplyResources(this.lblJugSize, "lblJugSize");
      this.lblJugSize.Font = null;
      this.lblJugSize.Name = "lblJugSize";
      this.ttToolTip.SetToolTip(this.lblJugSize, resources.GetString("lblJugSize.ToolTip"));
      // 
      // txtDPS
      // 
      this.txtDPS.AccessibleDescription = null;
      this.txtDPS.AccessibleName = null;
      resources.ApplyResources(this.txtDPS, "txtDPS");
      this.txtDPS.BackgroundImage = null;
      this.txtDPS.Font = null;
      this.txtDPS.Name = "txtDPS";
      this.txtDPS.ReadOnly = true;
      this.ttToolTip.SetToolTip(this.txtDPS, resources.GetString("txtDPS.ToolTip"));
      // 
      // txtDelay
      // 
      this.txtDelay.AccessibleDescription = null;
      this.txtDelay.AccessibleName = null;
      resources.ApplyResources(this.txtDelay, "txtDelay");
      this.txtDelay.BackgroundImage = null;
      this.txtDelay.Font = null;
      this.txtDelay.Name = "txtDelay";
      this.txtDelay.ReadOnly = true;
      this.ttToolTip.SetToolTip(this.txtDelay, resources.GetString("txtDelay.ToolTip"));
      // 
      // txtDamage
      // 
      this.txtDamage.AccessibleDescription = null;
      this.txtDamage.AccessibleName = null;
      resources.ApplyResources(this.txtDamage, "txtDamage");
      this.txtDamage.BackgroundImage = null;
      this.txtDamage.Font = null;
      this.txtDamage.Name = "txtDamage";
      this.txtDamage.ReadOnly = true;
      this.ttToolTip.SetToolTip(this.txtDamage, resources.GetString("txtDamage.ToolTip"));
      // 
      // txtSkill
      // 
      this.txtSkill.AccessibleDescription = null;
      this.txtSkill.AccessibleName = null;
      resources.ApplyResources(this.txtSkill, "txtSkill");
      this.txtSkill.BackgroundImage = null;
      this.txtSkill.Font = null;
      this.txtSkill.Name = "txtSkill";
      this.txtSkill.ReadOnly = true;
      this.ttToolTip.SetToolTip(this.txtSkill, resources.GetString("txtSkill.ToolTip"));
      // 
      // lblDPS
      // 
      this.lblDPS.AccessibleDescription = null;
      this.lblDPS.AccessibleName = null;
      resources.ApplyResources(this.lblDPS, "lblDPS");
      this.lblDPS.Font = null;
      this.lblDPS.Name = "lblDPS";
      this.ttToolTip.SetToolTip(this.lblDPS, resources.GetString("lblDPS.ToolTip"));
      // 
      // lblDelay
      // 
      this.lblDelay.AccessibleDescription = null;
      this.lblDelay.AccessibleName = null;
      resources.ApplyResources(this.lblDelay, "lblDelay");
      this.lblDelay.Font = null;
      this.lblDelay.Name = "lblDelay";
      this.ttToolTip.SetToolTip(this.lblDelay, resources.GetString("lblDelay.ToolTip"));
      // 
      // lblDamage
      // 
      this.lblDamage.AccessibleDescription = null;
      this.lblDamage.AccessibleName = null;
      resources.ApplyResources(this.lblDamage, "lblDamage");
      this.lblDamage.Font = null;
      this.lblDamage.Name = "lblDamage";
      this.ttToolTip.SetToolTip(this.lblDamage, resources.GetString("lblDamage.ToolTip"));
      // 
      // lblSkill
      // 
      this.lblSkill.AccessibleDescription = null;
      this.lblSkill.AccessibleName = null;
      resources.ApplyResources(this.lblSkill, "lblSkill");
      this.lblSkill.Font = null;
      this.lblSkill.Name = "lblSkill";
      this.ttToolTip.SetToolTip(this.lblSkill, resources.GetString("lblSkill.ToolTip"));
      // 
      // grpFurnitureInfo
      // 
      this.grpFurnitureInfo.AccessibleDescription = null;
      this.grpFurnitureInfo.AccessibleName = null;
      resources.ApplyResources(this.grpFurnitureInfo, "grpFurnitureInfo");
      this.grpFurnitureInfo.BackColor = System.Drawing.Color.Transparent;
      this.grpFurnitureInfo.BackgroundImage = null;
      this.grpFurnitureInfo.Controls.Add(this.txtStorage);
      this.grpFurnitureInfo.Controls.Add(this.txtElement);
      this.grpFurnitureInfo.Controls.Add(this.lblStorage);
      this.grpFurnitureInfo.Controls.Add(this.lblElement);
      this.grpFurnitureInfo.Font = null;
      this.grpFurnitureInfo.Name = "grpFurnitureInfo";
      this.grpFurnitureInfo.TabStop = false;
      this.ttToolTip.SetToolTip(this.grpFurnitureInfo, resources.GetString("grpFurnitureInfo.ToolTip"));
      // 
      // txtStorage
      // 
      this.txtStorage.AccessibleDescription = null;
      this.txtStorage.AccessibleName = null;
      resources.ApplyResources(this.txtStorage, "txtStorage");
      this.txtStorage.BackgroundImage = null;
      this.txtStorage.Font = null;
      this.txtStorage.Name = "txtStorage";
      this.txtStorage.ReadOnly = true;
      this.ttToolTip.SetToolTip(this.txtStorage, resources.GetString("txtStorage.ToolTip"));
      // 
      // txtElement
      // 
      this.txtElement.AccessibleDescription = null;
      this.txtElement.AccessibleName = null;
      resources.ApplyResources(this.txtElement, "txtElement");
      this.txtElement.BackgroundImage = null;
      this.txtElement.Font = null;
      this.txtElement.Name = "txtElement";
      this.txtElement.ReadOnly = true;
      this.ttToolTip.SetToolTip(this.txtElement, resources.GetString("txtElement.ToolTip"));
      // 
      // lblStorage
      // 
      this.lblStorage.AccessibleDescription = null;
      this.lblStorage.AccessibleName = null;
      resources.ApplyResources(this.lblStorage, "lblStorage");
      this.lblStorage.Font = null;
      this.lblStorage.Name = "lblStorage";
      this.ttToolTip.SetToolTip(this.lblStorage, resources.GetString("lblStorage.ToolTip"));
      // 
      // lblElement
      // 
      this.lblElement.AccessibleDescription = null;
      this.lblElement.AccessibleName = null;
      resources.ApplyResources(this.lblElement, "lblElement");
      this.lblElement.Font = null;
      this.lblElement.Name = "lblElement";
      this.ttToolTip.SetToolTip(this.lblElement, resources.GetString("lblElement.ToolTip"));
      // 
      // grpShieldInfo
      // 
      this.grpShieldInfo.AccessibleDescription = null;
      this.grpShieldInfo.AccessibleName = null;
      resources.ApplyResources(this.grpShieldInfo, "grpShieldInfo");
      this.grpShieldInfo.BackColor = System.Drawing.Color.Transparent;
      this.grpShieldInfo.BackgroundImage = null;
      this.grpShieldInfo.Controls.Add(this.txtShieldSize);
      this.grpShieldInfo.Controls.Add(this.lblShieldSize);
      this.grpShieldInfo.Font = null;
      this.grpShieldInfo.Name = "grpShieldInfo";
      this.grpShieldInfo.TabStop = false;
      this.ttToolTip.SetToolTip(this.grpShieldInfo, resources.GetString("grpShieldInfo.ToolTip"));
      // 
      // txtShieldSize
      // 
      this.txtShieldSize.AccessibleDescription = null;
      this.txtShieldSize.AccessibleName = null;
      resources.ApplyResources(this.txtShieldSize, "txtShieldSize");
      this.txtShieldSize.BackgroundImage = null;
      this.txtShieldSize.Font = null;
      this.txtShieldSize.Name = "txtShieldSize";
      this.txtShieldSize.ReadOnly = true;
      this.ttToolTip.SetToolTip(this.txtShieldSize, resources.GetString("txtShieldSize.ToolTip"));
      // 
      // lblShieldSize
      // 
      this.lblShieldSize.AccessibleDescription = null;
      this.lblShieldSize.AccessibleName = null;
      resources.ApplyResources(this.lblShieldSize, "lblShieldSize");
      this.lblShieldSize.Font = null;
      this.lblShieldSize.Name = "lblShieldSize";
      this.ttToolTip.SetToolTip(this.lblShieldSize, resources.GetString("lblShieldSize.ToolTip"));
      // 
      // grpEnchantmentInfo
      // 
      this.grpEnchantmentInfo.AccessibleDescription = null;
      this.grpEnchantmentInfo.AccessibleName = null;
      resources.ApplyResources(this.grpEnchantmentInfo, "grpEnchantmentInfo");
      this.grpEnchantmentInfo.BackColor = System.Drawing.Color.Transparent;
      this.grpEnchantmentInfo.BackgroundImage = null;
      this.grpEnchantmentInfo.Controls.Add(this.txtCastTime);
      this.grpEnchantmentInfo.Controls.Add(this.txtReuseTimer);
      this.grpEnchantmentInfo.Controls.Add(this.txtEquipDelay);
      this.grpEnchantmentInfo.Controls.Add(this.txtMaxCharges);
      this.grpEnchantmentInfo.Controls.Add(this.lblCastTime);
      this.grpEnchantmentInfo.Controls.Add(this.lblEquipDelay);
      this.grpEnchantmentInfo.Controls.Add(this.lblReuseTimer);
      this.grpEnchantmentInfo.Controls.Add(this.lblMaxCharges);
      this.grpEnchantmentInfo.Font = null;
      this.grpEnchantmentInfo.Name = "grpEnchantmentInfo";
      this.grpEnchantmentInfo.TabStop = false;
      this.ttToolTip.SetToolTip(this.grpEnchantmentInfo, resources.GetString("grpEnchantmentInfo.ToolTip"));
      // 
      // txtCastTime
      // 
      this.txtCastTime.AccessibleDescription = null;
      this.txtCastTime.AccessibleName = null;
      resources.ApplyResources(this.txtCastTime, "txtCastTime");
      this.txtCastTime.BackgroundImage = null;
      this.txtCastTime.Font = null;
      this.txtCastTime.Name = "txtCastTime";
      this.txtCastTime.ReadOnly = true;
      this.ttToolTip.SetToolTip(this.txtCastTime, resources.GetString("txtCastTime.ToolTip"));
      // 
      // txtReuseTimer
      // 
      this.txtReuseTimer.AccessibleDescription = null;
      this.txtReuseTimer.AccessibleName = null;
      resources.ApplyResources(this.txtReuseTimer, "txtReuseTimer");
      this.txtReuseTimer.BackgroundImage = null;
      this.txtReuseTimer.Font = null;
      this.txtReuseTimer.Name = "txtReuseTimer";
      this.txtReuseTimer.ReadOnly = true;
      this.ttToolTip.SetToolTip(this.txtReuseTimer, resources.GetString("txtReuseTimer.ToolTip"));
      // 
      // txtEquipDelay
      // 
      this.txtEquipDelay.AccessibleDescription = null;
      this.txtEquipDelay.AccessibleName = null;
      resources.ApplyResources(this.txtEquipDelay, "txtEquipDelay");
      this.txtEquipDelay.BackgroundImage = null;
      this.txtEquipDelay.Font = null;
      this.txtEquipDelay.Name = "txtEquipDelay";
      this.txtEquipDelay.ReadOnly = true;
      this.ttToolTip.SetToolTip(this.txtEquipDelay, resources.GetString("txtEquipDelay.ToolTip"));
      // 
      // txtMaxCharges
      // 
      this.txtMaxCharges.AccessibleDescription = null;
      this.txtMaxCharges.AccessibleName = null;
      resources.ApplyResources(this.txtMaxCharges, "txtMaxCharges");
      this.txtMaxCharges.BackgroundImage = null;
      this.txtMaxCharges.Font = null;
      this.txtMaxCharges.Name = "txtMaxCharges";
      this.txtMaxCharges.ReadOnly = true;
      this.ttToolTip.SetToolTip(this.txtMaxCharges, resources.GetString("txtMaxCharges.ToolTip"));
      // 
      // lblCastTime
      // 
      this.lblCastTime.AccessibleDescription = null;
      this.lblCastTime.AccessibleName = null;
      resources.ApplyResources(this.lblCastTime, "lblCastTime");
      this.lblCastTime.Font = null;
      this.lblCastTime.Name = "lblCastTime";
      this.ttToolTip.SetToolTip(this.lblCastTime, resources.GetString("lblCastTime.ToolTip"));
      // 
      // lblEquipDelay
      // 
      this.lblEquipDelay.AccessibleDescription = null;
      this.lblEquipDelay.AccessibleName = null;
      resources.ApplyResources(this.lblEquipDelay, "lblEquipDelay");
      this.lblEquipDelay.Font = null;
      this.lblEquipDelay.Name = "lblEquipDelay";
      this.ttToolTip.SetToolTip(this.lblEquipDelay, resources.GetString("lblEquipDelay.ToolTip"));
      // 
      // lblReuseTimer
      // 
      this.lblReuseTimer.AccessibleDescription = null;
      this.lblReuseTimer.AccessibleName = null;
      resources.ApplyResources(this.lblReuseTimer, "lblReuseTimer");
      this.lblReuseTimer.Font = null;
      this.lblReuseTimer.Name = "lblReuseTimer";
      this.ttToolTip.SetToolTip(this.lblReuseTimer, resources.GetString("lblReuseTimer.ToolTip"));
      // 
      // lblMaxCharges
      // 
      this.lblMaxCharges.AccessibleDescription = null;
      this.lblMaxCharges.AccessibleName = null;
      resources.ApplyResources(this.lblMaxCharges, "lblMaxCharges");
      this.lblMaxCharges.Font = null;
      this.lblMaxCharges.Name = "lblMaxCharges";
      this.ttToolTip.SetToolTip(this.lblMaxCharges, resources.GetString("lblMaxCharges.ToolTip"));
      // 
      // grpLogStrings
      // 
      this.grpLogStrings.AccessibleDescription = null;
      this.grpLogStrings.AccessibleName = null;
      resources.ApplyResources(this.grpLogStrings, "grpLogStrings");
      this.grpLogStrings.BackColor = System.Drawing.Color.Transparent;
      this.grpLogStrings.BackgroundImage = null;
      this.grpLogStrings.Controls.Add(this.txtPlural);
      this.grpLogStrings.Controls.Add(this.txtSingular);
      this.grpLogStrings.Controls.Add(this.lblPlural);
      this.grpLogStrings.Controls.Add(this.lblSingular);
      this.grpLogStrings.Font = null;
      this.grpLogStrings.Name = "grpLogStrings";
      this.grpLogStrings.TabStop = false;
      this.ttToolTip.SetToolTip(this.grpLogStrings, resources.GetString("grpLogStrings.ToolTip"));
      // 
      // txtPlural
      // 
      this.txtPlural.AccessibleDescription = null;
      this.txtPlural.AccessibleName = null;
      resources.ApplyResources(this.txtPlural, "txtPlural");
      this.txtPlural.BackgroundImage = null;
      this.txtPlural.Font = null;
      this.txtPlural.Name = "txtPlural";
      this.txtPlural.ReadOnly = true;
      this.ttToolTip.SetToolTip(this.txtPlural, resources.GetString("txtPlural.ToolTip"));
      // 
      // txtSingular
      // 
      this.txtSingular.AccessibleDescription = null;
      this.txtSingular.AccessibleName = null;
      resources.ApplyResources(this.txtSingular, "txtSingular");
      this.txtSingular.BackgroundImage = null;
      this.txtSingular.Font = null;
      this.txtSingular.Name = "txtSingular";
      this.txtSingular.ReadOnly = true;
      this.ttToolTip.SetToolTip(this.txtSingular, resources.GetString("txtSingular.ToolTip"));
      // 
      // lblPlural
      // 
      this.lblPlural.AccessibleDescription = null;
      this.lblPlural.AccessibleName = null;
      resources.ApplyResources(this.lblPlural, "lblPlural");
      this.lblPlural.Font = null;
      this.lblPlural.Name = "lblPlural";
      this.ttToolTip.SetToolTip(this.lblPlural, resources.GetString("lblPlural.ToolTip"));
      // 
      // lblSingular
      // 
      this.lblSingular.AccessibleDescription = null;
      this.lblSingular.AccessibleName = null;
      resources.ApplyResources(this.lblSingular, "lblSingular");
      this.lblSingular.Font = null;
      this.lblSingular.Name = "lblSingular";
      this.ttToolTip.SetToolTip(this.lblSingular, resources.GetString("lblSingular.ToolTip"));
      // 
      // grpUsableItemInfo
      // 
      this.grpUsableItemInfo.AccessibleDescription = null;
      this.grpUsableItemInfo.AccessibleName = null;
      resources.ApplyResources(this.grpUsableItemInfo, "grpUsableItemInfo");
      this.grpUsableItemInfo.BackColor = System.Drawing.Color.Transparent;
      this.grpUsableItemInfo.BackgroundImage = null;
      this.grpUsableItemInfo.Controls.Add(this.txtActivationTime);
      this.grpUsableItemInfo.Controls.Add(this.lblActivationTime);
      this.grpUsableItemInfo.Font = null;
      this.grpUsableItemInfo.Name = "grpUsableItemInfo";
      this.grpUsableItemInfo.TabStop = false;
      this.ttToolTip.SetToolTip(this.grpUsableItemInfo, resources.GetString("grpUsableItemInfo.ToolTip"));
      // 
      // txtActivationTime
      // 
      this.txtActivationTime.AccessibleDescription = null;
      this.txtActivationTime.AccessibleName = null;
      resources.ApplyResources(this.txtActivationTime, "txtActivationTime");
      this.txtActivationTime.BackgroundImage = null;
      this.txtActivationTime.Font = null;
      this.txtActivationTime.Name = "txtActivationTime";
      this.txtActivationTime.ReadOnly = true;
      this.ttToolTip.SetToolTip(this.txtActivationTime, resources.GetString("txtActivationTime.ToolTip"));
      // 
      // lblActivationTime
      // 
      this.lblActivationTime.AccessibleDescription = null;
      this.lblActivationTime.AccessibleName = null;
      resources.ApplyResources(this.lblActivationTime, "lblActivationTime");
      this.lblActivationTime.Font = null;
      this.lblActivationTime.Name = "lblActivationTime";
      this.ttToolTip.SetToolTip(this.lblActivationTime, resources.GetString("lblActivationTime.ToolTip"));
      // 
      // ItemEditor
      // 
      this.AccessibleDescription = null;
      this.AccessibleName = null;
      resources.ApplyResources(this, "$this");
      this.BackColor = System.Drawing.Color.Transparent;
      this.BackgroundImage = null;
      this.Controls.Add(this.grpUsableItemInfo);
      this.Controls.Add(this.grpLogStrings);
      this.Controls.Add(this.grpEquipmentInfo);
      this.Controls.Add(this.grpEnchantmentInfo);
      this.Controls.Add(this.grpShieldInfo);
      this.Controls.Add(this.grpFurnitureInfo);
      this.Controls.Add(this.grpWeaponInfo);
      this.Controls.Add(this.grpCommonInfo);
      this.Font = null;
      this.Name = "ItemEditor";
      this.ttToolTip.SetToolTip(this, resources.GetString("$this.ToolTip"));
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
      this.grpEnchantmentInfo.ResumeLayout(false);
      this.grpEnchantmentInfo.PerformLayout();
      this.grpLogStrings.ResumeLayout(false);
      this.grpLogStrings.PerformLayout();
      this.grpUsableItemInfo.ResumeLayout(false);
      this.grpUsableItemInfo.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

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
    private System.Windows.Forms.TextBox txtJugSize;
    private System.Windows.Forms.Label lblJugSize;
    private System.Windows.Forms.GroupBox grpUsableItemInfo;
    private System.Windows.Forms.TextBox txtActivationTime;
    private System.Windows.Forms.Label lblActivationTime;

  }

}