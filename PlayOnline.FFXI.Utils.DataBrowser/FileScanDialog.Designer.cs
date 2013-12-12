// $Id$

// Copyright © 2004-2010 Tim Van Holder
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

namespace PlayOnline.FFXI.Utils.DataBrowser {

  internal partial class FileScanDialog {

    #region Controls

    private System.Windows.Forms.ProgressBar prbScanProgress;
    private System.Windows.Forms.Label lblScanProgress;

    private System.ComponentModel.Container components = null;

    #endregion

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      this.prbScanProgress = new System.Windows.Forms.ProgressBar();
      this.lblScanProgress = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // prbScanProgress
      // 
      this.prbScanProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.prbScanProgress.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.prbScanProgress.Location = new System.Drawing.Point(5, 32);
      this.prbScanProgress.Maximum = 1000;
      this.prbScanProgress.Name = "prbScanProgress";
      this.prbScanProgress.Size = new System.Drawing.Size(436, 16);
      this.prbScanProgress.TabIndex = 5;
      this.prbScanProgress.Visible = false;
      // 
      // lblScanProgress
      // 
      this.lblScanProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lblScanProgress.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblScanProgress.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblScanProgress.Location = new System.Drawing.Point(5, 8);
      this.lblScanProgress.Name = "lblScanProgress";
      this.lblScanProgress.Size = new System.Drawing.Size(436, 16);
      this.lblScanProgress.TabIndex = 4;
      this.lblScanProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // FileScanDialog
      // 
      this.ClientSize = new System.Drawing.Size(446, 52);
      this.Controls.Add(this.prbScanProgress);
      this.Controls.Add(this.lblScanProgress);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "FileScanDialog";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Scanning File...";
      this.ResumeLayout(false);

    }

    #endregion

  }

}
