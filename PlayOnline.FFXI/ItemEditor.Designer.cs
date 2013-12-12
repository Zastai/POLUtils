// $Id$

// Copyright © 2004-2010 Tim Van Holder
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

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
      this.txtResourceID = new System.Windows.Forms.TextBox();
      this.lblResourceID = new System.Windows.Forms.Label();
      this.txtValidTargets = new System.Windows.Forms.TextBox();
      this.txtName = new System.Windows.Forms.TextBox();
      this.txtDescription = new System.Windows.Forms.TextBox();
      this.txtStackSize = new System.Windows.Forms.TextBox();
      this.txtFlags = new System.Windows.Forms.TextBox();
      this.txtType = new System.Windows.Forms.TextBox();
      this.txtID = new System.Windows.Forms.TextBox();
      this.picIcon = new System.Windows.Forms.PictureBox();
      this.lblValidTargets = new System.Windows.Forms.Label();
      this.lblDescription = new System.Windows.Forms.Label();
      this.lblName = new System.Windows.Forms.Label();
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
      this.grpPuppetItemInfo = new System.Windows.Forms.GroupBox();
      this.txtPuppetSlot = new System.Windows.Forms.TextBox();
      this.txtElementCharge = new System.Windows.Forms.TextBox();
      this.lblPuppetSlot = new System.Windows.Forms.Label();
      this.lblElementCharge = new System.Windows.Forms.Label();
      this.grpEquipmentInfo.SuspendLayout();
      this.grpCommonInfo.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
      this.grpWeaponInfo.SuspendLayout();
      this.grpFurnitureInfo.SuspendLayout();
      this.grpShieldInfo.SuspendLayout();
      this.grpEnchantmentInfo.SuspendLayout();
      this.grpLogStrings.SuspendLayout();
      this.grpUsableItemInfo.SuspendLayout();
      this.grpPuppetItemInfo.SuspendLayout();
      this.SuspendLayout();
      // 
      // grpEquipmentInfo
      // 
      this.grpEquipmentInfo.BackColor = System.Drawing.Color.Transparent;
      this.grpEquipmentInfo.Controls.Add(this.txtRaces);
      this.grpEquipmentInfo.Controls.Add(this.txtSlots);
      this.grpEquipmentInfo.Controls.Add(this.txtJobs);
      this.grpEquipmentInfo.Controls.Add(this.txtLevel);
      this.grpEquipmentInfo.Controls.Add(this.lblRaces);
      this.grpEquipmentInfo.Controls.Add(this.lblSlots);
      this.grpEquipmentInfo.Controls.Add(this.lblJobs);
      this.grpEquipmentInfo.Controls.Add(this.lblLevel);
      this.grpEquipmentInfo.Location = new System.Drawing.Point(0, 334);
      this.grpEquipmentInfo.Name = "grpEquipmentInfo";
      this.grpEquipmentInfo.Size = new System.Drawing.Size(424, 72);
      this.grpEquipmentInfo.TabIndex = 3;
      this.grpEquipmentInfo.TabStop = false;
      this.grpEquipmentInfo.Text = "Equipment Information";
      this.grpEquipmentInfo.Visible = false;
      // 
      // txtRaces
      // 
      this.txtRaces.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtRaces.Location = new System.Drawing.Point(253, 44);
      this.txtRaces.Name = "txtRaces";
      this.txtRaces.ReadOnly = true;
      this.txtRaces.Size = new System.Drawing.Size(163, 20);
      this.txtRaces.TabIndex = 303;
      // 
      // txtSlots
      // 
      this.txtSlots.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtSlots.Location = new System.Drawing.Point(48, 44);
      this.txtSlots.Name = "txtSlots";
      this.txtSlots.ReadOnly = true;
      this.txtSlots.Size = new System.Drawing.Size(152, 20);
      this.txtSlots.TabIndex = 302;
      // 
      // txtJobs
      // 
      this.txtJobs.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtJobs.Location = new System.Drawing.Point(115, 19);
      this.txtJobs.Name = "txtJobs";
      this.txtJobs.ReadOnly = true;
      this.txtJobs.Size = new System.Drawing.Size(301, 20);
      this.txtJobs.TabIndex = 301;
      // 
      // txtLevel
      // 
      this.txtLevel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtLevel.Location = new System.Drawing.Point(48, 19);
      this.txtLevel.Name = "txtLevel";
      this.txtLevel.ReadOnly = true;
      this.txtLevel.Size = new System.Drawing.Size(24, 20);
      this.txtLevel.TabIndex = 300;
      // 
      // lblRaces
      // 
      this.lblRaces.AutoSize = true;
      this.lblRaces.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblRaces.Location = new System.Drawing.Point(206, 48);
      this.lblRaces.Name = "lblRaces";
      this.lblRaces.Size = new System.Drawing.Size(41, 13);
      this.lblRaces.TabIndex = 0;
      this.lblRaces.Text = "Races:";
      // 
      // lblSlots
      // 
      this.lblSlots.AutoSize = true;
      this.lblSlots.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblSlots.Location = new System.Drawing.Point(6, 47);
      this.lblSlots.Name = "lblSlots";
      this.lblSlots.Size = new System.Drawing.Size(33, 13);
      this.lblSlots.TabIndex = 0;
      this.lblSlots.Text = "Slots:";
      // 
      // lblJobs
      // 
      this.lblJobs.AutoSize = true;
      this.lblJobs.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblJobs.Location = new System.Drawing.Point(77, 23);
      this.lblJobs.Name = "lblJobs";
      this.lblJobs.Size = new System.Drawing.Size(32, 13);
      this.lblJobs.TabIndex = 0;
      this.lblJobs.Text = "Jobs:";
      // 
      // lblLevel
      // 
      this.lblLevel.AutoSize = true;
      this.lblLevel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblLevel.Location = new System.Drawing.Point(6, 22);
      this.lblLevel.Name = "lblLevel";
      this.lblLevel.Size = new System.Drawing.Size(36, 13);
      this.lblLevel.TabIndex = 0;
      this.lblLevel.Text = "Level:";
      // 
      // grpCommonInfo
      // 
      this.grpCommonInfo.BackColor = System.Drawing.Color.Transparent;
      this.grpCommonInfo.Controls.Add(this.txtResourceID);
      this.grpCommonInfo.Controls.Add(this.lblResourceID);
      this.grpCommonInfo.Controls.Add(this.txtValidTargets);
      this.grpCommonInfo.Controls.Add(this.txtName);
      this.grpCommonInfo.Controls.Add(this.txtDescription);
      this.grpCommonInfo.Controls.Add(this.txtStackSize);
      this.grpCommonInfo.Controls.Add(this.txtFlags);
      this.grpCommonInfo.Controls.Add(this.txtType);
      this.grpCommonInfo.Controls.Add(this.txtID);
      this.grpCommonInfo.Controls.Add(this.picIcon);
      this.grpCommonInfo.Controls.Add(this.lblValidTargets);
      this.grpCommonInfo.Controls.Add(this.lblDescription);
      this.grpCommonInfo.Controls.Add(this.lblName);
      this.grpCommonInfo.Controls.Add(this.lblStackSize);
      this.grpCommonInfo.Controls.Add(this.lblFlags);
      this.grpCommonInfo.Controls.Add(this.lblType);
      this.grpCommonInfo.Controls.Add(this.lblID);
      this.grpCommonInfo.Location = new System.Drawing.Point(0, 0);
      this.grpCommonInfo.Name = "grpCommonInfo";
      this.grpCommonInfo.Size = new System.Drawing.Size(424, 260);
      this.grpCommonInfo.TabIndex = 1;
      this.grpCommonInfo.TabStop = false;
      this.grpCommonInfo.Text = "Common Information";
      // 
      // txtResourceID
      // 
      this.txtResourceID.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtResourceID.Location = new System.Drawing.Point(311, 20);
      this.txtResourceID.Name = "txtResourceID";
      this.txtResourceID.ReadOnly = true;
      this.txtResourceID.Size = new System.Drawing.Size(105, 20);
      this.txtResourceID.TabIndex = 101;
      // 
      // lblResourceID
      // 
      this.lblResourceID.AutoSize = true;
      this.lblResourceID.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblResourceID.Location = new System.Drawing.Point(235, 23);
      this.lblResourceID.Name = "lblResourceID";
      this.lblResourceID.Size = new System.Drawing.Size(70, 13);
      this.lblResourceID.TabIndex = 0;
      this.lblResourceID.Text = "Resource ID:";
      // 
      // txtValidTargets
      // 
      this.txtValidTargets.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtValidTargets.Location = new System.Drawing.Point(85, 232);
      this.txtValidTargets.Name = "txtValidTargets";
      this.txtValidTargets.ReadOnly = true;
      this.txtValidTargets.Size = new System.Drawing.Size(331, 20);
      this.txtValidTargets.TabIndex = 107;
      // 
      // txtName
      // 
      this.txtName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtName.Location = new System.Drawing.Point(120, 44);
      this.txtName.Name = "txtName";
      this.txtName.ReadOnly = true;
      this.txtName.Size = new System.Drawing.Size(296, 20);
      this.txtName.TabIndex = 102;
      // 
      // txtDescription
      // 
      this.txtDescription.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtDescription.Location = new System.Drawing.Point(85, 92);
      this.txtDescription.Multiline = true;
      this.txtDescription.Name = "txtDescription";
      this.txtDescription.ReadOnly = true;
      this.txtDescription.Size = new System.Drawing.Size(331, 112);
      this.txtDescription.TabIndex = 105;
      // 
      // txtStackSize
      // 
      this.txtStackSize.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtStackSize.Location = new System.Drawing.Point(363, 68);
      this.txtStackSize.Name = "txtStackSize";
      this.txtStackSize.ReadOnly = true;
      this.txtStackSize.Size = new System.Drawing.Size(53, 20);
      this.txtStackSize.TabIndex = 104;
      // 
      // txtFlags
      // 
      this.txtFlags.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtFlags.Location = new System.Drawing.Point(85, 208);
      this.txtFlags.Name = "txtFlags";
      this.txtFlags.ReadOnly = true;
      this.txtFlags.Size = new System.Drawing.Size(331, 20);
      this.txtFlags.TabIndex = 106;
      // 
      // txtType
      // 
      this.txtType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtType.Location = new System.Drawing.Point(120, 68);
      this.txtType.Name = "txtType";
      this.txtType.ReadOnly = true;
      this.txtType.Size = new System.Drawing.Size(170, 20);
      this.txtType.TabIndex = 103;
      // 
      // txtID
      // 
      this.txtID.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtID.Location = new System.Drawing.Point(120, 19);
      this.txtID.Name = "txtID";
      this.txtID.ReadOnly = true;
      this.txtID.Size = new System.Drawing.Size(105, 20);
      this.txtID.TabIndex = 100;
      // 
      // picIcon
      // 
      this.picIcon.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.picIcon.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.picIcon.Location = new System.Drawing.Point(6, 19);
      this.picIcon.Name = "picIcon";
      this.picIcon.Size = new System.Drawing.Size(64, 64);
      this.picIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
      this.picIcon.TabIndex = 3;
      this.picIcon.TabStop = false;
      // 
      // lblValidTargets
      // 
      this.lblValidTargets.AutoSize = true;
      this.lblValidTargets.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblValidTargets.Location = new System.Drawing.Point(6, 235);
      this.lblValidTargets.Name = "lblValidTargets";
      this.lblValidTargets.Size = new System.Drawing.Size(72, 13);
      this.lblValidTargets.TabIndex = 0;
      this.lblValidTargets.Text = "Valid Targets:";
      // 
      // lblDescription
      // 
      this.lblDescription.AutoSize = true;
      this.lblDescription.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblDescription.Location = new System.Drawing.Point(6, 95);
      this.lblDescription.Name = "lblDescription";
      this.lblDescription.Size = new System.Drawing.Size(63, 13);
      this.lblDescription.TabIndex = 0;
      this.lblDescription.Text = "Description:";
      // 
      // lblName
      // 
      this.lblName.AutoSize = true;
      this.lblName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblName.Location = new System.Drawing.Point(76, 47);
      this.lblName.Name = "lblName";
      this.lblName.Size = new System.Drawing.Size(38, 13);
      this.lblName.TabIndex = 0;
      this.lblName.Text = "Name:";
      // 
      // lblStackSize
      // 
      this.lblStackSize.AutoSize = true;
      this.lblStackSize.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblStackSize.Location = new System.Drawing.Point(296, 71);
      this.lblStackSize.Name = "lblStackSize";
      this.lblStackSize.Size = new System.Drawing.Size(61, 13);
      this.lblStackSize.TabIndex = 0;
      this.lblStackSize.Text = "Stack Size:";
      // 
      // lblFlags
      // 
      this.lblFlags.AutoSize = true;
      this.lblFlags.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblFlags.Location = new System.Drawing.Point(6, 211);
      this.lblFlags.Name = "lblFlags";
      this.lblFlags.Size = new System.Drawing.Size(35, 13);
      this.lblFlags.TabIndex = 0;
      this.lblFlags.Text = "Flags:";
      // 
      // lblType
      // 
      this.lblType.AutoSize = true;
      this.lblType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblType.Location = new System.Drawing.Point(76, 71);
      this.lblType.Name = "lblType";
      this.lblType.Size = new System.Drawing.Size(34, 13);
      this.lblType.TabIndex = 0;
      this.lblType.Text = "Type:";
      // 
      // lblID
      // 
      this.lblID.AutoSize = true;
      this.lblID.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblID.Location = new System.Drawing.Point(76, 23);
      this.lblID.Name = "lblID";
      this.lblID.Size = new System.Drawing.Size(21, 13);
      this.lblID.TabIndex = 0;
      this.lblID.Text = "ID:";
      // 
      // grpWeaponInfo
      // 
      this.grpWeaponInfo.BackColor = System.Drawing.Color.Transparent;
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
      this.grpWeaponInfo.Location = new System.Drawing.Point(0, 505);
      this.grpWeaponInfo.Name = "grpWeaponInfo";
      this.grpWeaponInfo.Size = new System.Drawing.Size(424, 72);
      this.grpWeaponInfo.TabIndex = 6;
      this.grpWeaponInfo.TabStop = false;
      this.grpWeaponInfo.Text = "Weapon Information";
      // 
      // txtJugSize
      // 
      this.txtJugSize.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtJugSize.Location = new System.Drawing.Point(320, 44);
      this.txtJugSize.Name = "txtJugSize";
      this.txtJugSize.ReadOnly = true;
      this.txtJugSize.Size = new System.Drawing.Size(32, 20);
      this.txtJugSize.TabIndex = 604;
      // 
      // lblJugSize
      // 
      this.lblJugSize.AutoSize = true;
      this.lblJugSize.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblJugSize.Location = new System.Drawing.Point(264, 47);
      this.lblJugSize.Name = "lblJugSize";
      this.lblJugSize.Size = new System.Drawing.Size(50, 13);
      this.lblJugSize.TabIndex = 0;
      this.lblJugSize.Text = "Jug Size:";
      // 
      // txtDPS
      // 
      this.txtDPS.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtDPS.Location = new System.Drawing.Point(320, 19);
      this.txtDPS.Name = "txtDPS";
      this.txtDPS.ReadOnly = true;
      this.txtDPS.Size = new System.Drawing.Size(40, 20);
      this.txtDPS.TabIndex = 602;
      // 
      // txtDelay
      // 
      this.txtDelay.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtDelay.Location = new System.Drawing.Point(139, 19);
      this.txtDelay.Name = "txtDelay";
      this.txtDelay.ReadOnly = true;
      this.txtDelay.Size = new System.Drawing.Size(60, 20);
      this.txtDelay.TabIndex = 601;
      // 
      // txtDamage
      // 
      this.txtDamage.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtDamage.Location = new System.Drawing.Point(62, 19);
      this.txtDamage.Name = "txtDamage";
      this.txtDamage.ReadOnly = true;
      this.txtDamage.Size = new System.Drawing.Size(28, 20);
      this.txtDamage.TabIndex = 600;
      // 
      // txtSkill
      // 
      this.txtSkill.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtSkill.Location = new System.Drawing.Point(85, 44);
      this.txtSkill.Name = "txtSkill";
      this.txtSkill.ReadOnly = true;
      this.txtSkill.Size = new System.Drawing.Size(173, 20);
      this.txtSkill.TabIndex = 603;
      // 
      // lblDPS
      // 
      this.lblDPS.AutoSize = true;
      this.lblDPS.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblDPS.Location = new System.Drawing.Point(205, 22);
      this.lblDPS.Name = "lblDPS";
      this.lblDPS.Size = new System.Drawing.Size(109, 13);
      this.lblDPS.TabIndex = 0;
      this.lblDPS.Text = "Damage Per Second:";
      // 
      // lblDelay
      // 
      this.lblDelay.AutoSize = true;
      this.lblDelay.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblDelay.Location = new System.Drawing.Point(96, 22);
      this.lblDelay.Name = "lblDelay";
      this.lblDelay.Size = new System.Drawing.Size(37, 13);
      this.lblDelay.TabIndex = 0;
      this.lblDelay.Text = "Delay:";
      // 
      // lblDamage
      // 
      this.lblDamage.AutoSize = true;
      this.lblDamage.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblDamage.Location = new System.Drawing.Point(6, 22);
      this.lblDamage.Name = "lblDamage";
      this.lblDamage.Size = new System.Drawing.Size(50, 13);
      this.lblDamage.TabIndex = 0;
      this.lblDamage.Text = "Damage:";
      // 
      // lblSkill
      // 
      this.lblSkill.AutoSize = true;
      this.lblSkill.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblSkill.Location = new System.Drawing.Point(6, 47);
      this.lblSkill.Name = "lblSkill";
      this.lblSkill.Size = new System.Drawing.Size(73, 13);
      this.lblSkill.TabIndex = 0;
      this.lblSkill.Text = "Weapon Skill:";
      // 
      // grpFurnitureInfo
      // 
      this.grpFurnitureInfo.BackColor = System.Drawing.Color.Transparent;
      this.grpFurnitureInfo.Controls.Add(this.txtStorage);
      this.grpFurnitureInfo.Controls.Add(this.txtElement);
      this.grpFurnitureInfo.Controls.Add(this.lblStorage);
      this.grpFurnitureInfo.Controls.Add(this.lblElement);
      this.grpFurnitureInfo.Location = new System.Drawing.Point(0, 578);
      this.grpFurnitureInfo.Name = "grpFurnitureInfo";
      this.grpFurnitureInfo.Size = new System.Drawing.Size(424, 48);
      this.grpFurnitureInfo.TabIndex = 7;
      this.grpFurnitureInfo.TabStop = false;
      this.grpFurnitureInfo.Text = "Furniture Information";
      this.grpFurnitureInfo.Visible = false;
      // 
      // txtStorage
      // 
      this.txtStorage.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtStorage.Location = new System.Drawing.Point(237, 19);
      this.txtStorage.Name = "txtStorage";
      this.txtStorage.ReadOnly = true;
      this.txtStorage.Size = new System.Drawing.Size(56, 20);
      this.txtStorage.TabIndex = 701;
      // 
      // txtElement
      // 
      this.txtElement.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtElement.Location = new System.Drawing.Point(60, 19);
      this.txtElement.Name = "txtElement";
      this.txtElement.ReadOnly = true;
      this.txtElement.Size = new System.Drawing.Size(92, 20);
      this.txtElement.TabIndex = 700;
      // 
      // lblStorage
      // 
      this.lblStorage.AutoSize = true;
      this.lblStorage.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblStorage.Location = new System.Drawing.Point(158, 22);
      this.lblStorage.Name = "lblStorage";
      this.lblStorage.Size = new System.Drawing.Size(73, 13);
      this.lblStorage.TabIndex = 0;
      this.lblStorage.Text = "Storage Slots:";
      // 
      // lblElement
      // 
      this.lblElement.AutoSize = true;
      this.lblElement.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblElement.Location = new System.Drawing.Point(6, 22);
      this.lblElement.Name = "lblElement";
      this.lblElement.Size = new System.Drawing.Size(48, 13);
      this.lblElement.TabIndex = 0;
      this.lblElement.Text = "Element:";
      // 
      // grpShieldInfo
      // 
      this.grpShieldInfo.BackColor = System.Drawing.Color.Transparent;
      this.grpShieldInfo.Controls.Add(this.txtShieldSize);
      this.grpShieldInfo.Controls.Add(this.lblShieldSize);
      this.grpShieldInfo.Location = new System.Drawing.Point(0, 456);
      this.grpShieldInfo.Name = "grpShieldInfo";
      this.grpShieldInfo.Size = new System.Drawing.Size(424, 48);
      this.grpShieldInfo.TabIndex = 5;
      this.grpShieldInfo.TabStop = false;
      this.grpShieldInfo.Text = "Shield Information";
      this.grpShieldInfo.Visible = false;
      // 
      // txtShieldSize
      // 
      this.txtShieldSize.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtShieldSize.Location = new System.Drawing.Point(74, 19);
      this.txtShieldSize.Name = "txtShieldSize";
      this.txtShieldSize.ReadOnly = true;
      this.txtShieldSize.Size = new System.Drawing.Size(26, 20);
      this.txtShieldSize.TabIndex = 500;
      // 
      // lblShieldSize
      // 
      this.lblShieldSize.AutoSize = true;
      this.lblShieldSize.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblShieldSize.Location = new System.Drawing.Point(6, 22);
      this.lblShieldSize.Name = "lblShieldSize";
      this.lblShieldSize.Size = new System.Drawing.Size(62, 13);
      this.lblShieldSize.TabIndex = 0;
      this.lblShieldSize.Text = "Shield Size:";
      // 
      // grpEnchantmentInfo
      // 
      this.grpEnchantmentInfo.BackColor = System.Drawing.Color.Transparent;
      this.grpEnchantmentInfo.Controls.Add(this.txtCastTime);
      this.grpEnchantmentInfo.Controls.Add(this.txtReuseTimer);
      this.grpEnchantmentInfo.Controls.Add(this.txtEquipDelay);
      this.grpEnchantmentInfo.Controls.Add(this.txtMaxCharges);
      this.grpEnchantmentInfo.Controls.Add(this.lblCastTime);
      this.grpEnchantmentInfo.Controls.Add(this.lblEquipDelay);
      this.grpEnchantmentInfo.Controls.Add(this.lblReuseTimer);
      this.grpEnchantmentInfo.Controls.Add(this.lblMaxCharges);
      this.grpEnchantmentInfo.Location = new System.Drawing.Point(0, 407);
      this.grpEnchantmentInfo.Name = "grpEnchantmentInfo";
      this.grpEnchantmentInfo.Size = new System.Drawing.Size(424, 48);
      this.grpEnchantmentInfo.TabIndex = 4;
      this.grpEnchantmentInfo.TabStop = false;
      this.grpEnchantmentInfo.Text = "Enchantment Information";
      this.grpEnchantmentInfo.Visible = false;
      // 
      // txtCastTime
      // 
      this.txtCastTime.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtCastTime.Location = new System.Drawing.Point(158, 19);
      this.txtCastTime.Name = "txtCastTime";
      this.txtCastTime.ReadOnly = true;
      this.txtCastTime.Size = new System.Drawing.Size(26, 20);
      this.txtCastTime.TabIndex = 401;
      // 
      // txtReuseTimer
      // 
      this.txtReuseTimer.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtReuseTimer.Location = new System.Drawing.Point(355, 19);
      this.txtReuseTimer.Name = "txtReuseTimer";
      this.txtReuseTimer.ReadOnly = true;
      this.txtReuseTimer.Size = new System.Drawing.Size(61, 20);
      this.txtReuseTimer.TabIndex = 403;
      // 
      // txtEquipDelay
      // 
      this.txtEquipDelay.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtEquipDelay.Location = new System.Drawing.Point(263, 19);
      this.txtEquipDelay.Name = "txtEquipDelay";
      this.txtEquipDelay.ReadOnly = true;
      this.txtEquipDelay.Size = new System.Drawing.Size(35, 20);
      this.txtEquipDelay.TabIndex = 402;
      // 
      // txtMaxCharges
      // 
      this.txtMaxCharges.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtMaxCharges.Location = new System.Drawing.Point(56, 19);
      this.txtMaxCharges.Name = "txtMaxCharges";
      this.txtMaxCharges.ReadOnly = true;
      this.txtMaxCharges.Size = new System.Drawing.Size(19, 20);
      this.txtMaxCharges.TabIndex = 400;
      // 
      // lblCastTime
      // 
      this.lblCastTime.AutoSize = true;
      this.lblCastTime.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblCastTime.Location = new System.Drawing.Point(81, 22);
      this.lblCastTime.Name = "lblCastTime";
      this.lblCastTime.Size = new System.Drawing.Size(71, 13);
      this.lblCastTime.TabIndex = 0;
      this.lblCastTime.Text = "Casting Time:";
      // 
      // lblEquipDelay
      // 
      this.lblEquipDelay.AutoSize = true;
      this.lblEquipDelay.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblEquipDelay.Location = new System.Drawing.Point(190, 22);
      this.lblEquipDelay.Name = "lblEquipDelay";
      this.lblEquipDelay.Size = new System.Drawing.Size(67, 13);
      this.lblEquipDelay.TabIndex = 0;
      this.lblEquipDelay.Text = "Equip Delay:";
      // 
      // lblReuseTimer
      // 
      this.lblReuseTimer.AutoSize = true;
      this.lblReuseTimer.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblReuseTimer.Location = new System.Drawing.Point(304, 22);
      this.lblReuseTimer.Name = "lblReuseTimer";
      this.lblReuseTimer.Size = new System.Drawing.Size(46, 13);
      this.lblReuseTimer.TabIndex = 0;
      this.lblReuseTimer.Text = "Re-Use:";
      // 
      // lblMaxCharges
      // 
      this.lblMaxCharges.AutoSize = true;
      this.lblMaxCharges.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblMaxCharges.Location = new System.Drawing.Point(6, 22);
      this.lblMaxCharges.Name = "lblMaxCharges";
      this.lblMaxCharges.Size = new System.Drawing.Size(49, 13);
      this.lblMaxCharges.TabIndex = 0;
      this.lblMaxCharges.Text = "Charges:";
      // 
      // grpLogStrings
      // 
      this.grpLogStrings.BackColor = System.Drawing.Color.Transparent;
      this.grpLogStrings.Controls.Add(this.txtPlural);
      this.grpLogStrings.Controls.Add(this.txtSingular);
      this.grpLogStrings.Controls.Add(this.lblPlural);
      this.grpLogStrings.Controls.Add(this.lblSingular);
      this.grpLogStrings.Location = new System.Drawing.Point(0, 261);
      this.grpLogStrings.Name = "grpLogStrings";
      this.grpLogStrings.Size = new System.Drawing.Size(424, 72);
      this.grpLogStrings.TabIndex = 2;
      this.grpLogStrings.TabStop = false;
      this.grpLogStrings.Text = "English Log Strings";
      this.grpLogStrings.Visible = false;
      // 
      // txtPlural
      // 
      this.txtPlural.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtPlural.Location = new System.Drawing.Point(116, 44);
      this.txtPlural.Name = "txtPlural";
      this.txtPlural.ReadOnly = true;
      this.txtPlural.Size = new System.Drawing.Size(300, 20);
      this.txtPlural.TabIndex = 201;
      // 
      // txtSingular
      // 
      this.txtSingular.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtSingular.Location = new System.Drawing.Point(116, 20);
      this.txtSingular.Name = "txtSingular";
      this.txtSingular.ReadOnly = true;
      this.txtSingular.Size = new System.Drawing.Size(300, 20);
      this.txtSingular.TabIndex = 200;
      // 
      // lblPlural
      // 
      this.lblPlural.AutoSize = true;
      this.lblPlural.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblPlural.Location = new System.Drawing.Point(6, 47);
      this.lblPlural.Name = "lblPlural";
      this.lblPlural.Size = new System.Drawing.Size(104, 13);
      this.lblPlural.TabIndex = 0;
      this.lblPlural.Text = "Log Name (Multiple):";
      // 
      // lblSingular
      // 
      this.lblSingular.AutoSize = true;
      this.lblSingular.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblSingular.Location = new System.Drawing.Point(13, 23);
      this.lblSingular.Name = "lblSingular";
      this.lblSingular.Size = new System.Drawing.Size(97, 13);
      this.lblSingular.TabIndex = 0;
      this.lblSingular.Text = "Log Name (Single):";
      // 
      // grpUsableItemInfo
      // 
      this.grpUsableItemInfo.BackColor = System.Drawing.Color.Transparent;
      this.grpUsableItemInfo.Controls.Add(this.txtActivationTime);
      this.grpUsableItemInfo.Controls.Add(this.lblActivationTime);
      this.grpUsableItemInfo.Location = new System.Drawing.Point(0, 627);
      this.grpUsableItemInfo.Name = "grpUsableItemInfo";
      this.grpUsableItemInfo.Size = new System.Drawing.Size(424, 48);
      this.grpUsableItemInfo.TabIndex = 8;
      this.grpUsableItemInfo.TabStop = false;
      this.grpUsableItemInfo.Text = "Item Effect Information";
      this.grpUsableItemInfo.Visible = false;
      // 
      // txtActivationTime
      // 
      this.txtActivationTime.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtActivationTime.Location = new System.Drawing.Point(126, 19);
      this.txtActivationTime.Name = "txtActivationTime";
      this.txtActivationTime.ReadOnly = true;
      this.txtActivationTime.Size = new System.Drawing.Size(37, 20);
      this.txtActivationTime.TabIndex = 800;
      // 
      // lblActivationTime
      // 
      this.lblActivationTime.AutoSize = true;
      this.lblActivationTime.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblActivationTime.Location = new System.Drawing.Point(6, 22);
      this.lblActivationTime.Name = "lblActivationTime";
      this.lblActivationTime.Size = new System.Drawing.Size(114, 13);
      this.lblActivationTime.TabIndex = 0;
      this.lblActivationTime.Text = "Effect Activation Time:";
      // 
      // grpPuppetItemInfo
      // 
      this.grpPuppetItemInfo.BackColor = System.Drawing.Color.Transparent;
      this.grpPuppetItemInfo.Controls.Add(this.txtPuppetSlot);
      this.grpPuppetItemInfo.Controls.Add(this.txtElementCharge);
      this.grpPuppetItemInfo.Controls.Add(this.lblPuppetSlot);
      this.grpPuppetItemInfo.Controls.Add(this.lblElementCharge);
      this.grpPuppetItemInfo.Location = new System.Drawing.Point(0, 676);
      this.grpPuppetItemInfo.Name = "grpPuppetItemInfo";
      this.grpPuppetItemInfo.Size = new System.Drawing.Size(424, 72);
      this.grpPuppetItemInfo.TabIndex = 9;
      this.grpPuppetItemInfo.TabStop = false;
      this.grpPuppetItemInfo.Text = "Puppet Item Information";
      this.grpPuppetItemInfo.Visible = false;
      // 
      // txtPuppetSlot
      // 
      this.txtPuppetSlot.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtPuppetSlot.Location = new System.Drawing.Point(77, 19);
      this.txtPuppetSlot.Name = "txtPuppetSlot";
      this.txtPuppetSlot.ReadOnly = true;
      this.txtPuppetSlot.Size = new System.Drawing.Size(86, 20);
      this.txtPuppetSlot.TabIndex = 900;
      // 
      // txtElementCharge
      // 
      this.txtElementCharge.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtElementCharge.Location = new System.Drawing.Point(97, 45);
      this.txtElementCharge.Name = "txtElementCharge";
      this.txtElementCharge.ReadOnly = true;
      this.txtElementCharge.Size = new System.Drawing.Size(319, 20);
      this.txtElementCharge.TabIndex = 901;
      // 
      // lblPuppetSlot
      // 
      this.lblPuppetSlot.AutoSize = true;
      this.lblPuppetSlot.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblPuppetSlot.Location = new System.Drawing.Point(6, 22);
      this.lblPuppetSlot.Name = "lblPuppetSlot";
      this.lblPuppetSlot.Size = new System.Drawing.Size(65, 13);
      this.lblPuppetSlot.TabIndex = 0;
      this.lblPuppetSlot.Text = "Puppet Slot:";
      // 
      // lblElementCharge
      // 
      this.lblElementCharge.AutoSize = true;
      this.lblElementCharge.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblElementCharge.Location = new System.Drawing.Point(6, 48);
      this.lblElementCharge.Name = "lblElementCharge";
      this.lblElementCharge.Size = new System.Drawing.Size(85, 13);
      this.lblElementCharge.TabIndex = 0;
      this.lblElementCharge.Text = "Element Charge:";
      // 
      // ItemEditor
      // 
      this.BackColor = System.Drawing.Color.Transparent;
      this.Controls.Add(this.grpPuppetItemInfo);
      this.Controls.Add(this.grpUsableItemInfo);
      this.Controls.Add(this.grpLogStrings);
      this.Controls.Add(this.grpEquipmentInfo);
      this.Controls.Add(this.grpEnchantmentInfo);
      this.Controls.Add(this.grpShieldInfo);
      this.Controls.Add(this.grpFurnitureInfo);
      this.Controls.Add(this.grpWeaponInfo);
      this.Controls.Add(this.grpCommonInfo);
      this.Name = "ItemEditor";
      this.Size = new System.Drawing.Size(424, 781);
      this.grpEquipmentInfo.ResumeLayout(false);
      this.grpEquipmentInfo.PerformLayout();
      this.grpCommonInfo.ResumeLayout(false);
      this.grpCommonInfo.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
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
      this.grpPuppetItemInfo.ResumeLayout(false);
      this.grpPuppetItemInfo.PerformLayout();
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
    private System.Windows.Forms.Label lblName;
    private System.Windows.Forms.TextBox txtName;
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
    private System.Windows.Forms.GroupBox grpPuppetItemInfo;
    private System.Windows.Forms.TextBox txtPuppetSlot;
    private System.Windows.Forms.TextBox txtElementCharge;
    private System.Windows.Forms.Label lblPuppetSlot;
    private System.Windows.Forms.Label lblElementCharge;
    private System.Windows.Forms.TextBox txtResourceID;
    private System.Windows.Forms.Label lblResourceID;

  }

}