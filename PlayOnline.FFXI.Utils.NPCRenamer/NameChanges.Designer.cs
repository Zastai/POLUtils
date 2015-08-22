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

  partial class NameChanges {

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
      this.components = new System.ComponentModel.Container();
      this.lstNameChanges = new System.Windows.Forms.ListView();
      this.chArea = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.chOldName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.chNewName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.mnuNameChangeContext = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.mnuWriteSelected = new System.Windows.Forms.ToolStripMenuItem();
      this.mnuRevertSelected = new System.Windows.Forms.ToolStripMenuItem();
      this.mnuSaveSelected = new System.Windows.Forms.ToolStripMenuItem();
      this.btnDiscardPending = new System.Windows.Forms.Button();
      this.btnWritePending = new System.Windows.Forms.Button();
      this.btnClose = new System.Windows.Forms.Button();
      this.btnRevertAll = new System.Windows.Forms.Button();
      this.btnForgetAll = new System.Windows.Forms.Button();
      this.btnApplySet = new System.Windows.Forms.Button();
      this.btnUnapplySet = new System.Windows.Forms.Button();
      this.dlgLoadChangeset = new System.Windows.Forms.OpenFileDialog();
      this.dlgSaveChangeset = new System.Windows.Forms.SaveFileDialog();
      this.prbWriteChanges = new System.Windows.Forms.ProgressBar();
      this.mnuNameChangeContext.SuspendLayout();
      this.SuspendLayout();
      // 
      // lstNameChanges
      // 
      this.lstNameChanges.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lstNameChanges.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chArea,
            this.chOldName,
            this.chNewName});
      this.lstNameChanges.ContextMenuStrip = this.mnuNameChangeContext;
      this.lstNameChanges.FullRowSelect = true;
      this.lstNameChanges.Location = new System.Drawing.Point(12, 12);
      this.lstNameChanges.Name = "lstNameChanges";
      this.lstNameChanges.Size = new System.Drawing.Size(888, 521);
      this.lstNameChanges.TabIndex = 0;
      this.lstNameChanges.UseCompatibleStateImageBehavior = false;
      this.lstNameChanges.View = System.Windows.Forms.View.Details;
      this.lstNameChanges.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstNameChanges_KeyDown);
      // 
      // chArea
      // 
      this.chArea.Text = "Area";
      this.chArea.Width = 280;
      // 
      // chOldName
      // 
      this.chOldName.Text = "Old Name";
      this.chOldName.Width = 280;
      // 
      // chNewName
      // 
      this.chNewName.Text = "New Name";
      this.chNewName.Width = 280;
      // 
      // mnuNameChangeContext
      // 
      this.mnuNameChangeContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuWriteSelected,
            this.mnuRevertSelected,
            this.mnuSaveSelected});
      this.mnuNameChangeContext.Name = "mnuNameChangeContext";
      this.mnuNameChangeContext.ShowImageMargin = false;
      this.mnuNameChangeContext.Size = new System.Drawing.Size(179, 70);
      this.mnuNameChangeContext.Opening += new System.ComponentModel.CancelEventHandler(this.mnuNameChangeContext_Opening);
      // 
      // mnuWriteSelected
      // 
      this.mnuWriteSelected.Name = "mnuWriteSelected";
      this.mnuWriteSelected.Size = new System.Drawing.Size(178, 22);
      this.mnuWriteSelected.Text = "&Write Selected Changes";
      this.mnuWriteSelected.Click += new System.EventHandler(this.mnuWriteSelected_Click);
      // 
      // mnuRevertSelected
      // 
      this.mnuRevertSelected.Name = "mnuRevertSelected";
      this.mnuRevertSelected.Size = new System.Drawing.Size(178, 22);
      this.mnuRevertSelected.Text = "&Revert Selected Changes";
      this.mnuRevertSelected.Click += new System.EventHandler(this.mnuRevertSelected_Click);
      // 
      // mnuSaveSelected
      // 
      this.mnuSaveSelected.Name = "mnuSaveSelected";
      this.mnuSaveSelected.Size = new System.Drawing.Size(178, 22);
      this.mnuSaveSelected.Text = "&Save Selected Changes...";
      this.mnuSaveSelected.Click += new System.EventHandler(this.mnuSaveSelected_Click);
      // 
      // btnDiscardPending
      // 
      this.btnDiscardPending.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnDiscardPending.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnDiscardPending.Location = new System.Drawing.Point(525, 539);
      this.btnDiscardPending.Name = "btnDiscardPending";
      this.btnDiscardPending.Size = new System.Drawing.Size(146, 23);
      this.btnDiscardPending.TabIndex = 2;
      this.btnDiscardPending.Text = "&Discard Pending Changes";
      this.btnDiscardPending.UseVisualStyleBackColor = true;
      this.btnDiscardPending.Click += new System.EventHandler(this.btnDiscardPending_Click);
      // 
      // btnWritePending
      // 
      this.btnWritePending.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnWritePending.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnWritePending.Location = new System.Drawing.Point(677, 539);
      this.btnWritePending.Name = "btnWritePending";
      this.btnWritePending.Size = new System.Drawing.Size(142, 23);
      this.btnWritePending.TabIndex = 3;
      this.btnWritePending.Text = "&Write Pending Changes";
      this.btnWritePending.UseVisualStyleBackColor = true;
      this.btnWritePending.Click += new System.EventHandler(this.btnWritePending_Click);
      // 
      // btnClose
      // 
      this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnClose.Location = new System.Drawing.Point(825, 539);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new System.Drawing.Size(75, 23);
      this.btnClose.TabIndex = 4;
      this.btnClose.Text = "&Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
      // 
      // btnRevertAll
      // 
      this.btnRevertAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnRevertAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnRevertAll.Location = new System.Drawing.Point(403, 539);
      this.btnRevertAll.Name = "btnRevertAll";
      this.btnRevertAll.Size = new System.Drawing.Size(116, 23);
      this.btnRevertAll.TabIndex = 5;
      this.btnRevertAll.Text = "&Revert All Changes";
      this.btnRevertAll.UseVisualStyleBackColor = true;
      this.btnRevertAll.Click += new System.EventHandler(this.btnRevertAll_Click);
      // 
      // btnForgetAll
      // 
      this.btnForgetAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnForgetAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnForgetAll.Location = new System.Drawing.Point(283, 539);
      this.btnForgetAll.Name = "btnForgetAll";
      this.btnForgetAll.Size = new System.Drawing.Size(114, 23);
      this.btnForgetAll.TabIndex = 6;
      this.btnForgetAll.Text = "&Forget All Changes";
      this.btnForgetAll.UseVisualStyleBackColor = true;
      this.btnForgetAll.Click += new System.EventHandler(this.btnForgetAll_Click);
      // 
      // btnApplySet
      // 
      this.btnApplySet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnApplySet.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnApplySet.Location = new System.Drawing.Point(12, 539);
      this.btnApplySet.Name = "btnApplySet";
      this.btnApplySet.Size = new System.Drawing.Size(106, 23);
      this.btnApplySet.TabIndex = 7;
      this.btnApplySet.Text = "&Apply Changeset..";
      this.btnApplySet.UseVisualStyleBackColor = true;
      this.btnApplySet.Click += new System.EventHandler(this.btnApplySet_Click);
      // 
      // btnUnapplySet
      // 
      this.btnUnapplySet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnUnapplySet.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnUnapplySet.Location = new System.Drawing.Point(124, 539);
      this.btnUnapplySet.Name = "btnUnapplySet";
      this.btnUnapplySet.Size = new System.Drawing.Size(114, 23);
      this.btnUnapplySet.TabIndex = 8;
      this.btnUnapplySet.Text = "&Unapply Changeset...";
      this.btnUnapplySet.UseVisualStyleBackColor = true;
      this.btnUnapplySet.Click += new System.EventHandler(this.btnUnapplySet_Click);
      // 
      // dlgLoadChangeset
      // 
      this.dlgLoadChangeset.DefaultExt = "xml";
      this.dlgLoadChangeset.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
      this.dlgLoadChangeset.SupportMultiDottedExtensions = true;
      this.dlgLoadChangeset.Title = "Load Changeset";
      // 
      // dlgSaveChangeset
      // 
      this.dlgSaveChangeset.DefaultExt = "xml";
      this.dlgSaveChangeset.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
      this.dlgSaveChangeset.SupportMultiDottedExtensions = true;
      this.dlgSaveChangeset.Title = "Save Changeset";
      // 
      // prbWriteChanges
      // 
      this.prbWriteChanges.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.prbWriteChanges.Location = new System.Drawing.Point(12, 513);
      this.prbWriteChanges.Name = "prbWriteChanges";
      this.prbWriteChanges.Size = new System.Drawing.Size(888, 20);
      this.prbWriteChanges.Step = 1;
      this.prbWriteChanges.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
      this.prbWriteChanges.TabIndex = 9;
      this.prbWriteChanges.Visible = false;
      // 
      // NameChanges
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnClose;
      this.ClientSize = new System.Drawing.Size(912, 574);
      this.Controls.Add(this.prbWriteChanges);
      this.Controls.Add(this.btnWritePending);
      this.Controls.Add(this.btnForgetAll);
      this.Controls.Add(this.btnApplySet);
      this.Controls.Add(this.btnUnapplySet);
      this.Controls.Add(this.btnRevertAll);
      this.Controls.Add(this.lstNameChanges);
      this.Controls.Add(this.btnClose);
      this.Controls.Add(this.btnDiscardPending);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
      this.MinimumSize = new System.Drawing.Size(880, 230);
      this.Name = "NameChanges";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Name Changes";
      this.mnuNameChangeContext.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListView lstNameChanges;
    private System.Windows.Forms.ColumnHeader chArea;
    private System.Windows.Forms.ColumnHeader chOldName;
    private System.Windows.Forms.ColumnHeader chNewName;
    private System.Windows.Forms.ContextMenuStrip mnuNameChangeContext;
    private System.Windows.Forms.ToolStripMenuItem mnuRevertSelected;
    private System.Windows.Forms.ToolStripMenuItem mnuWriteSelected;
    private System.Windows.Forms.ToolStripMenuItem mnuSaveSelected;
    private System.Windows.Forms.Button btnDiscardPending;
    private System.Windows.Forms.Button btnWritePending;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.Button btnRevertAll;
    private System.Windows.Forms.Button btnForgetAll;
    private System.Windows.Forms.Button btnApplySet;
    private System.Windows.Forms.Button btnUnapplySet;
    private System.ComponentModel.IContainer components;
    private System.Windows.Forms.OpenFileDialog dlgLoadChangeset;
    private System.Windows.Forms.SaveFileDialog dlgSaveChangeset;
    private System.Windows.Forms.ProgressBar prbWriteChanges;

  }

}