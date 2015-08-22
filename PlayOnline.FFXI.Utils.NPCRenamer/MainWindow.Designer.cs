// $Id$

// Copyright © 2004-2010 Tim Van Holder
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

namespace PlayOnline.FFXI.Utils.NPCRenamer {

  partial class MainWindow {

    private System.ComponentModel.Container components = null;

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      System.Windows.Forms.ColumnHeader colNPCName;
      this.grpArea = new System.Windows.Forms.GroupBox();
      this.cmbArea = new System.Windows.Forms.ComboBox();
      this.grpNames = new System.Windows.Forms.GroupBox();
      this.lstNPCNames = new System.Windows.Forms.ListView();
      this.btnClose = new System.Windows.Forms.Button();
      this.btnShowChanges = new System.Windows.Forms.Button();
      colNPCName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.grpArea.SuspendLayout();
      this.grpNames.SuspendLayout();
      this.SuspendLayout();
      // 
      // colNPCName
      // 
      colNPCName.Text = "NPC Name";
      colNPCName.Width = 200;
      // 
      // grpArea
      // 
      this.grpArea.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.grpArea.Controls.Add(this.cmbArea);
      this.grpArea.Location = new System.Drawing.Point(12, 12);
      this.grpArea.Name = "grpArea";
      this.grpArea.Size = new System.Drawing.Size(243, 51);
      this.grpArea.TabIndex = 1;
      this.grpArea.TabStop = false;
      this.grpArea.Text = "Area";
      // 
      // cmbArea
      // 
      this.cmbArea.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.cmbArea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbArea.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.cmbArea.FormattingEnabled = true;
      this.cmbArea.Location = new System.Drawing.Point(6, 19);
      this.cmbArea.Name = "cmbArea";
      this.cmbArea.Size = new System.Drawing.Size(231, 21);
      this.cmbArea.Sorted = true;
      this.cmbArea.TabIndex = 2;
      this.cmbArea.SelectedIndexChanged += new System.EventHandler(this.cmbArea_SelectedIndexChanged);
      // 
      // grpNames
      // 
      this.grpNames.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.grpNames.Controls.Add(this.lstNPCNames);
      this.grpNames.Location = new System.Drawing.Point(12, 69);
      this.grpNames.Name = "grpNames";
      this.grpNames.Size = new System.Drawing.Size(243, 211);
      this.grpNames.TabIndex = 3;
      this.grpNames.TabStop = false;
      this.grpNames.Text = "NPC Names";
      // 
      // lstNPCNames
      // 
      this.lstNPCNames.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lstNPCNames.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            colNPCName});
      this.lstNPCNames.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
      this.lstNPCNames.LabelEdit = true;
      this.lstNPCNames.Location = new System.Drawing.Point(6, 19);
      this.lstNPCNames.MultiSelect = false;
      this.lstNPCNames.Name = "lstNPCNames";
      this.lstNPCNames.ShowGroups = false;
      this.lstNPCNames.Size = new System.Drawing.Size(231, 186);
      this.lstNPCNames.TabIndex = 4;
      this.lstNPCNames.UseCompatibleStateImageBehavior = false;
      this.lstNPCNames.View = System.Windows.Forms.View.Details;
      this.lstNPCNames.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.lstNPCNames_AfterLabelEdit);
      this.lstNPCNames.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstNPCNames_KeyDown);
      // 
      // btnClose
      // 
      this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnClose.Location = new System.Drawing.Point(180, 286);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new System.Drawing.Size(75, 23);
      this.btnClose.TabIndex = 100;
      this.btnClose.Text = "&Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
      // 
      // btnShowChanges
      // 
      this.btnShowChanges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnShowChanges.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnShowChanges.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.btnShowChanges.Location = new System.Drawing.Point(79, 286);
      this.btnShowChanges.Name = "btnShowChanges";
      this.btnShowChanges.Size = new System.Drawing.Size(95, 23);
      this.btnShowChanges.TabIndex = 99;
      this.btnShowChanges.Text = "&Show Changes...";
      this.btnShowChanges.UseVisualStyleBackColor = true;
      this.btnShowChanges.Click += new System.EventHandler(this.btnShowChanges_Click);
      // 
      // MainWindow
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnClose;
      this.ClientSize = new System.Drawing.Size(267, 321);
      this.Controls.Add(this.btnShowChanges);
      this.Controls.Add(this.btnClose);
      this.Controls.Add(this.grpNames);
      this.Controls.Add(this.grpArea);
      this.Name = "MainWindow";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "FFXI NPC Renamer";
      this.Shown += new System.EventHandler(this.MainWindow_Shown);
      this.grpArea.ResumeLayout(false);
      this.grpNames.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox grpArea;
    private System.Windows.Forms.ComboBox cmbArea;
    private System.Windows.Forms.GroupBox grpNames;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.Button btnShowChanges;
    private System.Windows.Forms.ListView lstNPCNames;

  }

}