// $Id$

// Copyright © 2004-2010 Tim Van Holder
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

namespace PlayOnline.Core {

  internal partial class ChooseRegionDialog {

    #region Controls

    private System.Windows.Forms.Label lblExplanation;
    private System.Windows.Forms.RadioButton radJapan;
    private System.Windows.Forms.RadioButton radNorthAmerica;
    private System.Windows.Forms.RadioButton radEurope;
    private System.Windows.Forms.Button btnOK;

    private System.ComponentModel.Container components = null;

    #endregion

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      this.lblExplanation = new System.Windows.Forms.Label();
      this.radJapan = new System.Windows.Forms.RadioButton();
      this.radNorthAmerica = new System.Windows.Forms.RadioButton();
      this.radEurope = new System.Windows.Forms.RadioButton();
      this.btnOK = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // lblExplanation
      // 
      this.lblExplanation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lblExplanation.BackColor = System.Drawing.SystemColors.Control;
      this.lblExplanation.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblExplanation.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblExplanation.Location = new System.Drawing.Point(6, 6);
      this.lblExplanation.Name = "lblExplanation";
      this.lblExplanation.Size = new System.Drawing.Size(271, 89);
      this.lblExplanation.TabIndex = 0;
      this.lblExplanation.Text = "Multiple versions of the PlayOnline client software are installed on this machine.\nDifferent applications may be installed under the different clients, and this program needs to know where to find those applications (and their data files).\nPlease select the region you wish to use:";
      // 
      // radJapan
      // 
      this.radJapan.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
      this.radJapan.AutoSize = true;
      this.radJapan.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.radJapan.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.radJapan.Location = new System.Drawing.Point(83, 98);
      this.radJapan.Name = "radJapan";
      this.radJapan.Size = new System.Drawing.Size(60, 18);
      this.radJapan.TabIndex = 1;
      this.radJapan.Text = "&Japan";
      // 
      // radNorthAmerica
      // 
      this.radNorthAmerica.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
      this.radNorthAmerica.AutoSize = true;
      this.radNorthAmerica.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.radNorthAmerica.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.radNorthAmerica.Location = new System.Drawing.Point(83, 122);
      this.radNorthAmerica.Name = "radNorthAmerica";
      this.radNorthAmerica.Size = new System.Drawing.Size(98, 18);
      this.radNorthAmerica.TabIndex = 2;
      this.radNorthAmerica.Text = "North &America";
      // 
      // radEurope
      // 
      this.radEurope.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
      this.radEurope.AutoSize = true;
      this.radEurope.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.radEurope.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.radEurope.Location = new System.Drawing.Point(83, 146);
      this.radEurope.Name = "radEurope";
      this.radEurope.Size = new System.Drawing.Size(117, 18);
      this.radEurope.TabIndex = 3;
      this.radEurope.Text = "&Europe && Australia";
      // 
      // btnOK
      // 
      this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.btnOK.Location = new System.Drawing.Point(109, 172);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(65, 24);
      this.btnOK.TabIndex = 4;
      this.btnOK.Text = "&OK";
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // ChooseRegionDialog
      // 
      this.ClientSize = new System.Drawing.Size(282, 204);
      this.ControlBox = false;
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.radEurope);
      this.Controls.Add(this.radNorthAmerica);
      this.Controls.Add(this.radJapan);
      this.Controls.Add(this.lblExplanation);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Name = "ChooseRegionDialog";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Select Region";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

  }

}
