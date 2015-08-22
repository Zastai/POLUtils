// $Id$

// Copyright © 2004-2010 Tim Van Holder
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

namespace PlayOnline.FFXI.Utils.EngrishOnry {

  public partial class MainWindow {

    #region Windows Form Designer generated code

    private System.ComponentModel.Container components = null;

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      this.pnlLog = new System.Windows.Forms.Panel();
      this.lblActivityLog = new System.Windows.Forms.Label();
      this.rtbActivityLog = new System.Windows.Forms.RichTextBox();
      this.mnuConfigSpellData = new System.Windows.Forms.ContextMenu();
      this.mnuTranslateSpellNames = new System.Windows.Forms.MenuItem();
      this.mnuTranslateSpellDescriptions = new System.Windows.Forms.MenuItem();
      this.mnuConfigItemData = new System.Windows.Forms.ContextMenu();
      this.mnuTranslateItemNames = new System.Windows.Forms.MenuItem();
      this.mnuTranslateItemDescriptions = new System.Windows.Forms.MenuItem();
      this.mnuConfigAutoTrans = new System.Windows.Forms.ContextMenu();
      this.mnuPreserveJapaneseATCompletion = new System.Windows.Forms.MenuItem();
      this.mnuEnglishATCompletionOnly = new System.Windows.Forms.MenuItem();
      this.pnlActions = new System.Windows.Forms.Panel();
      this.btnConfigAbilities = new System.Windows.Forms.Button();
      this.lblAbilities = new System.Windows.Forms.Label();
      this.btnRestoreAbilities = new System.Windows.Forms.Button();
      this.btnTranslateAbilities = new System.Windows.Forms.Button();
      this.btnConfigDialogTables = new System.Windows.Forms.Button();
      this.btnConfigStringTables = new System.Windows.Forms.Button();
      this.btnConfigAutoTrans = new System.Windows.Forms.Button();
      this.btnConfigItemData = new System.Windows.Forms.Button();
      this.btnConfigSpellData = new System.Windows.Forms.Button();
      this.lblItemData = new System.Windows.Forms.Label();
      this.lblAutoTranslator = new System.Windows.Forms.Label();
      this.lblSpellData = new System.Windows.Forms.Label();
      this.lblStringTables = new System.Windows.Forms.Label();
      this.lblDialogTables = new System.Windows.Forms.Label();
      this.btnRestoreSpellData = new System.Windows.Forms.Button();
      this.btnTranslateSpellData = new System.Windows.Forms.Button();
      this.btnRestoreStringTables = new System.Windows.Forms.Button();
      this.btnTranslateStringTables = new System.Windows.Forms.Button();
      this.btnRestoreDialogTables = new System.Windows.Forms.Button();
      this.btnTranslateDialogTables = new System.Windows.Forms.Button();
      this.btnRestoreAutoTrans = new System.Windows.Forms.Button();
      this.btnTranslateAutoTrans = new System.Windows.Forms.Button();
      this.btnRestoreItemData = new System.Windows.Forms.Button();
      this.btnTranslateItemData = new System.Windows.Forms.Button();
      this.mnuConfigAbilities = new System.Windows.Forms.ContextMenu();
      this.mnuTranslateAbilityNames = new System.Windows.Forms.MenuItem();
      this.mnuTranslateAbilityDescriptions = new System.Windows.Forms.MenuItem();
      this.pnlLog.SuspendLayout();
      this.pnlActions.SuspendLayout();
      this.SuspendLayout();
      // 
      // pnlLog
      // 
      this.pnlLog.Controls.Add(this.lblActivityLog);
      this.pnlLog.Controls.Add(this.rtbActivityLog);
      this.pnlLog.Dock = System.Windows.Forms.DockStyle.Fill;
      this.pnlLog.Location = new System.Drawing.Point(0, 72);
      this.pnlLog.Name = "pnlLog";
      this.pnlLog.Size = new System.Drawing.Size(519, 150);
      this.pnlLog.TabIndex = 0;
      // 
      // lblActivityLog
      // 
      this.lblActivityLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lblActivityLog.Location = new System.Drawing.Point(4, 4);
      this.lblActivityLog.Name = "lblActivityLog";
      this.lblActivityLog.Size = new System.Drawing.Size(507, 16);
      this.lblActivityLog.TabIndex = 0;
      this.lblActivityLog.Text = "Activity Log:";
      // 
      // rtbActivityLog
      // 
      this.rtbActivityLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.rtbActivityLog.Font = new System.Drawing.Font("Lucida Sans Typewriter", 8F);
      this.rtbActivityLog.Location = new System.Drawing.Point(4, 20);
      this.rtbActivityLog.Name = "rtbActivityLog";
      this.rtbActivityLog.ReadOnly = true;
      this.rtbActivityLog.Size = new System.Drawing.Size(511, 126);
      this.rtbActivityLog.TabIndex = 500;
      this.rtbActivityLog.Text = "";
      this.rtbActivityLog.WordWrap = false;
      // 
      // mnuConfigSpellData
      // 
      this.mnuConfigSpellData.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuTranslateSpellNames,
            this.mnuTranslateSpellDescriptions});
      // 
      // mnuTranslateSpellNames
      // 
      this.mnuTranslateSpellNames.Checked = true;
      this.mnuTranslateSpellNames.Index = 0;
      this.mnuTranslateSpellNames.Text = "Translate Spell &Names";
      this.mnuTranslateSpellNames.Click += new System.EventHandler(this.mnuTranslateSpellNames_Click);
      // 
      // mnuTranslateSpellDescriptions
      // 
      this.mnuTranslateSpellDescriptions.Checked = true;
      this.mnuTranslateSpellDescriptions.Index = 1;
      this.mnuTranslateSpellDescriptions.Text = "Translate Spell &Descriptions";
      this.mnuTranslateSpellDescriptions.Click += new System.EventHandler(this.mnuTranslateSpellDescriptions_Click);
      // 
      // mnuConfigItemData
      // 
      this.mnuConfigItemData.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuTranslateItemNames,
            this.mnuTranslateItemDescriptions});
      // 
      // mnuTranslateItemNames
      // 
      this.mnuTranslateItemNames.Checked = true;
      this.mnuTranslateItemNames.Index = 0;
      this.mnuTranslateItemNames.Text = "Translate Item &Names";
      this.mnuTranslateItemNames.Click += new System.EventHandler(this.mnuTranslateItemNames_Click);
      // 
      // mnuTranslateItemDescriptions
      // 
      this.mnuTranslateItemDescriptions.Checked = true;
      this.mnuTranslateItemDescriptions.Index = 1;
      this.mnuTranslateItemDescriptions.Text = "Translate Item &Descriptions";
      this.mnuTranslateItemDescriptions.Click += new System.EventHandler(this.mnuTranslateItemDescriptions_Click);
      // 
      // mnuConfigAutoTrans
      // 
      this.mnuConfigAutoTrans.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuPreserveJapaneseATCompletion,
            this.mnuEnglishATCompletionOnly});
      // 
      // mnuPreserveJapaneseATCompletion
      // 
      this.mnuPreserveJapaneseATCompletion.Checked = true;
      this.mnuPreserveJapaneseATCompletion.Index = 0;
      this.mnuPreserveJapaneseATCompletion.Text = "Preserve &Japanese Completion";
      this.mnuPreserveJapaneseATCompletion.Click += new System.EventHandler(this.mnuPreserveJapaneseATCompletion_Click);
      // 
      // mnuEnglishATCompletionOnly
      // 
      this.mnuEnglishATCompletionOnly.Index = 1;
      this.mnuEnglishATCompletionOnly.Text = "&English Completion Only";
      this.mnuEnglishATCompletionOnly.Click += new System.EventHandler(this.mnuEnglishATCompletionOnly_Click);
      // 
      // pnlActions
      // 
      this.pnlActions.Controls.Add(this.btnConfigAbilities);
      this.pnlActions.Controls.Add(this.lblAbilities);
      this.pnlActions.Controls.Add(this.btnRestoreAbilities);
      this.pnlActions.Controls.Add(this.btnTranslateAbilities);
      this.pnlActions.Controls.Add(this.btnConfigDialogTables);
      this.pnlActions.Controls.Add(this.btnConfigStringTables);
      this.pnlActions.Controls.Add(this.btnConfigAutoTrans);
      this.pnlActions.Controls.Add(this.btnConfigItemData);
      this.pnlActions.Controls.Add(this.btnConfigSpellData);
      this.pnlActions.Controls.Add(this.lblItemData);
      this.pnlActions.Controls.Add(this.lblAutoTranslator);
      this.pnlActions.Controls.Add(this.lblSpellData);
      this.pnlActions.Controls.Add(this.lblStringTables);
      this.pnlActions.Controls.Add(this.lblDialogTables);
      this.pnlActions.Controls.Add(this.btnRestoreSpellData);
      this.pnlActions.Controls.Add(this.btnTranslateSpellData);
      this.pnlActions.Controls.Add(this.btnRestoreStringTables);
      this.pnlActions.Controls.Add(this.btnTranslateStringTables);
      this.pnlActions.Controls.Add(this.btnRestoreDialogTables);
      this.pnlActions.Controls.Add(this.btnTranslateDialogTables);
      this.pnlActions.Controls.Add(this.btnRestoreAutoTrans);
      this.pnlActions.Controls.Add(this.btnTranslateAutoTrans);
      this.pnlActions.Controls.Add(this.btnRestoreItemData);
      this.pnlActions.Controls.Add(this.btnTranslateItemData);
      this.pnlActions.Dock = System.Windows.Forms.DockStyle.Top;
      this.pnlActions.Location = new System.Drawing.Point(0, 0);
      this.pnlActions.Name = "pnlActions";
      this.pnlActions.Size = new System.Drawing.Size(519, 72);
      this.pnlActions.TabIndex = 14;
      // 
      // btnConfigAbilities
      // 
      this.btnConfigAbilities.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnConfigAbilities.Location = new System.Drawing.Point(188, 48);
      this.btnConfigAbilities.Name = "btnConfigAbilities";
      this.btnConfigAbilities.Size = new System.Drawing.Size(48, 20);
      this.btnConfigAbilities.TabIndex = 37;
      this.btnConfigAbilities.Text = "Options";
      this.btnConfigAbilities.Click += new System.EventHandler(this.btnConfigAbilities_Click);
      // 
      // lblAbilities
      // 
      this.lblAbilities.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblAbilities.Location = new System.Drawing.Point(8, 52);
      this.lblAbilities.Name = "lblAbilities";
      this.lblAbilities.Size = new System.Drawing.Size(60, 16);
      this.lblAbilities.TabIndex = 34;
      this.lblAbilities.Text = "Ability Data:";
      this.lblAbilities.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // btnRestoreAbilities
      // 
      this.btnRestoreAbilities.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnRestoreAbilities.Location = new System.Drawing.Point(128, 48);
      this.btnRestoreAbilities.Name = "btnRestoreAbilities";
      this.btnRestoreAbilities.Size = new System.Drawing.Size(60, 20);
      this.btnRestoreAbilities.TabIndex = 36;
      this.btnRestoreAbilities.Text = "Restore";
      this.btnRestoreAbilities.Click += new System.EventHandler(this.btnRestoreAbilities_Click);
      // 
      // btnTranslateAbilities
      // 
      this.btnTranslateAbilities.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnTranslateAbilities.Location = new System.Drawing.Point(72, 48);
      this.btnTranslateAbilities.Name = "btnTranslateAbilities";
      this.btnTranslateAbilities.Size = new System.Drawing.Size(56, 20);
      this.btnTranslateAbilities.TabIndex = 35;
      this.btnTranslateAbilities.Text = "Translate";
      this.btnTranslateAbilities.Click += new System.EventHandler(this.btnTranslateAbilities_Click);
      // 
      // btnConfigDialogTables
      // 
      this.btnConfigDialogTables.Enabled = false;
      this.btnConfigDialogTables.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnConfigDialogTables.Location = new System.Drawing.Point(464, 8);
      this.btnConfigDialogTables.Name = "btnConfigDialogTables";
      this.btnConfigDialogTables.Size = new System.Drawing.Size(48, 20);
      this.btnConfigDialogTables.TabIndex = 33;
      this.btnConfigDialogTables.Text = "Options";
      // 
      // btnConfigStringTables
      // 
      this.btnConfigStringTables.Enabled = false;
      this.btnConfigStringTables.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnConfigStringTables.Location = new System.Drawing.Point(464, 28);
      this.btnConfigStringTables.Name = "btnConfigStringTables";
      this.btnConfigStringTables.Size = new System.Drawing.Size(48, 20);
      this.btnConfigStringTables.TabIndex = 32;
      this.btnConfigStringTables.Text = "Options";
      // 
      // btnConfigAutoTrans
      // 
      this.btnConfigAutoTrans.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnConfigAutoTrans.Location = new System.Drawing.Point(464, 48);
      this.btnConfigAutoTrans.Name = "btnConfigAutoTrans";
      this.btnConfigAutoTrans.Size = new System.Drawing.Size(48, 20);
      this.btnConfigAutoTrans.TabIndex = 27;
      this.btnConfigAutoTrans.Text = "Options";
      this.btnConfigAutoTrans.Click += new System.EventHandler(this.btnConfigAutoTrans_Click);
      // 
      // btnConfigItemData
      // 
      this.btnConfigItemData.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnConfigItemData.Location = new System.Drawing.Point(188, 28);
      this.btnConfigItemData.Name = "btnConfigItemData";
      this.btnConfigItemData.Size = new System.Drawing.Size(48, 20);
      this.btnConfigItemData.TabIndex = 24;
      this.btnConfigItemData.Text = "Options";
      this.btnConfigItemData.Click += new System.EventHandler(this.btnConfigItemData_Click);
      // 
      // btnConfigSpellData
      // 
      this.btnConfigSpellData.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnConfigSpellData.Location = new System.Drawing.Point(188, 8);
      this.btnConfigSpellData.Name = "btnConfigSpellData";
      this.btnConfigSpellData.Size = new System.Drawing.Size(48, 20);
      this.btnConfigSpellData.TabIndex = 21;
      this.btnConfigSpellData.Text = "Options";
      this.btnConfigSpellData.Click += new System.EventHandler(this.btnConfigSpellData_Click);
      // 
      // lblItemData
      // 
      this.lblItemData.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemData.Location = new System.Drawing.Point(8, 32);
      this.lblItemData.Name = "lblItemData";
      this.lblItemData.Size = new System.Drawing.Size(60, 16);
      this.lblItemData.TabIndex = 17;
      this.lblItemData.Text = "Item Data:";
      this.lblItemData.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // lblAutoTranslator
      // 
      this.lblAutoTranslator.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblAutoTranslator.Location = new System.Drawing.Point(260, 52);
      this.lblAutoTranslator.Name = "lblAutoTranslator";
      this.lblAutoTranslator.Size = new System.Drawing.Size(82, 16);
      this.lblAutoTranslator.TabIndex = 14;
      this.lblAutoTranslator.Text = "Auto-Translator:";
      this.lblAutoTranslator.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // lblSpellData
      // 
      this.lblSpellData.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblSpellData.Location = new System.Drawing.Point(8, 12);
      this.lblSpellData.Name = "lblSpellData";
      this.lblSpellData.Size = new System.Drawing.Size(60, 16);
      this.lblSpellData.TabIndex = 15;
      this.lblSpellData.Text = "Spell Data:";
      this.lblSpellData.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // lblStringTables
      // 
      this.lblStringTables.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblStringTables.Location = new System.Drawing.Point(240, 32);
      this.lblStringTables.Name = "lblStringTables";
      this.lblStringTables.Size = new System.Drawing.Size(104, 16);
      this.lblStringTables.TabIndex = 16;
      this.lblStringTables.Text = "Other String Tables:";
      this.lblStringTables.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // lblDialogTables
      // 
      this.lblDialogTables.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblDialogTables.Location = new System.Drawing.Point(240, 12);
      this.lblDialogTables.Name = "lblDialogTables";
      this.lblDialogTables.Size = new System.Drawing.Size(104, 16);
      this.lblDialogTables.TabIndex = 18;
      this.lblDialogTables.Text = "Zone Dialog Tables:";
      this.lblDialogTables.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // btnRestoreSpellData
      // 
      this.btnRestoreSpellData.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnRestoreSpellData.Location = new System.Drawing.Point(128, 8);
      this.btnRestoreSpellData.Name = "btnRestoreSpellData";
      this.btnRestoreSpellData.Size = new System.Drawing.Size(60, 20);
      this.btnRestoreSpellData.TabIndex = 20;
      this.btnRestoreSpellData.Text = "Restore";
      this.btnRestoreSpellData.Click += new System.EventHandler(this.btnRestoreSpellData_Click);
      // 
      // btnTranslateSpellData
      // 
      this.btnTranslateSpellData.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnTranslateSpellData.Location = new System.Drawing.Point(72, 8);
      this.btnTranslateSpellData.Name = "btnTranslateSpellData";
      this.btnTranslateSpellData.Size = new System.Drawing.Size(56, 20);
      this.btnTranslateSpellData.TabIndex = 19;
      this.btnTranslateSpellData.Text = "Translate";
      this.btnTranslateSpellData.Click += new System.EventHandler(this.btnTranslateSpellData_Click);
      // 
      // btnRestoreStringTables
      // 
      this.btnRestoreStringTables.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnRestoreStringTables.Location = new System.Drawing.Point(404, 28);
      this.btnRestoreStringTables.Name = "btnRestoreStringTables";
      this.btnRestoreStringTables.Size = new System.Drawing.Size(60, 20);
      this.btnRestoreStringTables.TabIndex = 31;
      this.btnRestoreStringTables.Text = "Restore";
      this.btnRestoreStringTables.Click += new System.EventHandler(this.btnRestoreStringTables_Click);
      // 
      // btnTranslateStringTables
      // 
      this.btnTranslateStringTables.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnTranslateStringTables.Location = new System.Drawing.Point(348, 28);
      this.btnTranslateStringTables.Name = "btnTranslateStringTables";
      this.btnTranslateStringTables.Size = new System.Drawing.Size(56, 20);
      this.btnTranslateStringTables.TabIndex = 30;
      this.btnTranslateStringTables.Text = "Translate";
      this.btnTranslateStringTables.Click += new System.EventHandler(this.btnTranslateStringTables_Click);
      // 
      // btnRestoreDialogTables
      // 
      this.btnRestoreDialogTables.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnRestoreDialogTables.Location = new System.Drawing.Point(404, 8);
      this.btnRestoreDialogTables.Name = "btnRestoreDialogTables";
      this.btnRestoreDialogTables.Size = new System.Drawing.Size(60, 20);
      this.btnRestoreDialogTables.TabIndex = 29;
      this.btnRestoreDialogTables.Text = "Restore";
      this.btnRestoreDialogTables.Click += new System.EventHandler(this.btnRestoreDialogTables_Click);
      // 
      // btnTranslateDialogTables
      // 
      this.btnTranslateDialogTables.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnTranslateDialogTables.Location = new System.Drawing.Point(348, 8);
      this.btnTranslateDialogTables.Name = "btnTranslateDialogTables";
      this.btnTranslateDialogTables.Size = new System.Drawing.Size(56, 20);
      this.btnTranslateDialogTables.TabIndex = 28;
      this.btnTranslateDialogTables.Text = "Translate";
      this.btnTranslateDialogTables.Click += new System.EventHandler(this.btnTranslateDialogTables_Click);
      // 
      // btnRestoreAutoTrans
      // 
      this.btnRestoreAutoTrans.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnRestoreAutoTrans.Location = new System.Drawing.Point(404, 48);
      this.btnRestoreAutoTrans.Name = "btnRestoreAutoTrans";
      this.btnRestoreAutoTrans.Size = new System.Drawing.Size(60, 20);
      this.btnRestoreAutoTrans.TabIndex = 26;
      this.btnRestoreAutoTrans.Text = "Restore";
      this.btnRestoreAutoTrans.Click += new System.EventHandler(this.btnRestoreAutoTrans_Click);
      // 
      // btnTranslateAutoTrans
      // 
      this.btnTranslateAutoTrans.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnTranslateAutoTrans.Location = new System.Drawing.Point(348, 48);
      this.btnTranslateAutoTrans.Name = "btnTranslateAutoTrans";
      this.btnTranslateAutoTrans.Size = new System.Drawing.Size(56, 20);
      this.btnTranslateAutoTrans.TabIndex = 25;
      this.btnTranslateAutoTrans.Text = "Translate";
      this.btnTranslateAutoTrans.Click += new System.EventHandler(this.btnTranslateAutoTrans_Click);
      // 
      // btnRestoreItemData
      // 
      this.btnRestoreItemData.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnRestoreItemData.Location = new System.Drawing.Point(128, 28);
      this.btnRestoreItemData.Name = "btnRestoreItemData";
      this.btnRestoreItemData.Size = new System.Drawing.Size(60, 20);
      this.btnRestoreItemData.TabIndex = 23;
      this.btnRestoreItemData.Text = "Restore";
      this.btnRestoreItemData.Click += new System.EventHandler(this.btnRestoreItemData_Click);
      // 
      // btnTranslateItemData
      // 
      this.btnTranslateItemData.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnTranslateItemData.Location = new System.Drawing.Point(72, 28);
      this.btnTranslateItemData.Name = "btnTranslateItemData";
      this.btnTranslateItemData.Size = new System.Drawing.Size(56, 20);
      this.btnTranslateItemData.TabIndex = 22;
      this.btnTranslateItemData.Text = "Translate";
      this.btnTranslateItemData.Click += new System.EventHandler(this.btnTranslateItemData_Click);
      // 
      // mnuConfigAbilities
      // 
      this.mnuConfigAbilities.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuTranslateAbilityNames,
            this.mnuTranslateAbilityDescriptions});
      // 
      // mnuTranslateAbilityNames
      // 
      this.mnuTranslateAbilityNames.Checked = true;
      this.mnuTranslateAbilityNames.Index = 0;
      this.mnuTranslateAbilityNames.Text = "Translate Ability &Names";
      // 
      // mnuTranslateAbilityDescriptions
      // 
      this.mnuTranslateAbilityDescriptions.Checked = true;
      this.mnuTranslateAbilityDescriptions.Index = 1;
      this.mnuTranslateAbilityDescriptions.Text = "Translate Ability &Descriptions";
      // 
      // MainWindow
      // 
      this.ClientSize = new System.Drawing.Size(519, 222);
      this.Controls.Add(this.pnlLog);
      this.Controls.Add(this.pnlActions);
      this.Name = "MainWindow";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Make JP FFXI Engrish!";
      this.pnlLog.ResumeLayout(false);
      this.pnlActions.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    // Controls
    private System.Windows.Forms.Label lblActivityLog;
    private System.Windows.Forms.RichTextBox rtbActivityLog;
    private System.Windows.Forms.ContextMenu mnuConfigSpellData;
    private System.Windows.Forms.ContextMenu mnuConfigItemData;
    private System.Windows.Forms.MenuItem mnuTranslateSpellNames;
    private System.Windows.Forms.MenuItem mnuTranslateSpellDescriptions;
    private System.Windows.Forms.ContextMenu mnuConfigAutoTrans;
    private System.Windows.Forms.Panel pnlLog;
    private System.Windows.Forms.Panel pnlActions;
    private System.Windows.Forms.Button btnConfigAutoTrans;
    private System.Windows.Forms.Button btnConfigItemData;
    private System.Windows.Forms.Button btnConfigSpellData;
    private System.Windows.Forms.Label lblItemData;
    private System.Windows.Forms.Label lblAutoTranslator;
    private System.Windows.Forms.Label lblSpellData;
    private System.Windows.Forms.Label lblStringTables;
    private System.Windows.Forms.Label lblDialogTables;
    private System.Windows.Forms.Button btnRestoreSpellData;
    private System.Windows.Forms.Button btnTranslateSpellData;
    private System.Windows.Forms.Button btnRestoreStringTables;
    private System.Windows.Forms.Button btnTranslateStringTables;
    private System.Windows.Forms.Button btnRestoreDialogTables;
    private System.Windows.Forms.Button btnTranslateDialogTables;
    private System.Windows.Forms.Button btnRestoreAutoTrans;
    private System.Windows.Forms.Button btnTranslateAutoTrans;
    private System.Windows.Forms.Button btnRestoreItemData;
    private System.Windows.Forms.Button btnTranslateItemData;
    private System.Windows.Forms.MenuItem mnuTranslateItemNames;
    private System.Windows.Forms.MenuItem mnuTranslateItemDescriptions;
    private System.Windows.Forms.MenuItem mnuPreserveJapaneseATCompletion;
    private System.Windows.Forms.MenuItem mnuEnglishATCompletionOnly;
    private System.Windows.Forms.Button btnConfigStringTables;
    private System.Windows.Forms.Button btnConfigDialogTables;
    private System.Windows.Forms.Button btnConfigAbilities;
    private System.Windows.Forms.Label lblAbilities;
    private System.Windows.Forms.Button btnRestoreAbilities;
    private System.Windows.Forms.Button btnTranslateAbilities;
    private System.Windows.Forms.ContextMenu mnuConfigAbilities;
    private System.Windows.Forms.MenuItem mnuTranslateAbilityNames;
    private System.Windows.Forms.MenuItem mnuTranslateAbilityDescriptions;

  }

}
