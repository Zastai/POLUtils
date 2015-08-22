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

  partial class ThingPropertyPages {

    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing) {
      if (disposing && this.components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent() {
      this.tabPages = new System.Windows.Forms.TabControl();
      this.tabDummy = new System.Windows.Forms.TabPage();
      this.pnlButtons = new System.Windows.Forms.Panel();
      this.btnClose = new System.Windows.Forms.Button();
      this.tabPages.SuspendLayout();
      this.pnlButtons.SuspendLayout();
      this.SuspendLayout();
      // 
      // tabPages
      // 
      this.tabPages.Controls.Add(this.tabDummy);
      this.tabPages.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabPages.HotTrack = true;
      this.tabPages.Location = new System.Drawing.Point(0, 0);
      this.tabPages.Multiline = true;
      this.tabPages.Name = "tabPages";
      this.tabPages.SelectedIndex = 0;
      this.tabPages.Size = new System.Drawing.Size(344, 368);
      this.tabPages.TabIndex = 0;
      this.tabPages.SelectedIndexChanged += new System.EventHandler(this.tabPages_SelectedIndexChanged);
      // 
      // tabDummy
      // 
      this.tabDummy.Location = new System.Drawing.Point(4, 22);
      this.tabDummy.Margin = new System.Windows.Forms.Padding(0);
      this.tabDummy.Name = "tabDummy";
      this.tabDummy.Size = new System.Drawing.Size(336, 342);
      this.tabDummy.TabIndex = 0;
      this.tabDummy.Text = "Dummy";
      this.tabDummy.UseVisualStyleBackColor = true;
      // 
      // pnlButtons
      // 
      this.pnlButtons.BackColor = System.Drawing.Color.Transparent;
      this.pnlButtons.Controls.Add(this.btnClose);
      this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.pnlButtons.Location = new System.Drawing.Point(0, 368);
      this.pnlButtons.Name = "pnlButtons";
      this.pnlButtons.Size = new System.Drawing.Size(344, 25);
      this.pnlButtons.TabIndex = 1;
      // 
      // btnClose
      // 
      this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnClose.Location = new System.Drawing.Point(286, 1);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new System.Drawing.Size(55, 22);
      this.btnClose.TabIndex = 1;
      this.btnClose.Text = "&Close";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
      // 
      // ThingPropertyPages
      // 
      this.AcceptButton = this.btnClose;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnClose;
      this.ClientSize = new System.Drawing.Size(344, 393);
      this.Controls.Add(this.tabPages);
      this.Controls.Add(this.pnlButtons);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ThingPropertyPages";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.Text = "Properties";
      this.tabPages.ResumeLayout(false);
      this.pnlButtons.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TabControl tabPages;
    private System.Windows.Forms.Panel pnlButtons;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.TabPage tabDummy;

  }

}