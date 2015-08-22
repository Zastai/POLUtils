// $Id$

// Copyright © 2004-2010 Tim Van Holder
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

namespace PlayOnline.Utils.AudioManager {

  public partial class WaveWriter {

    #region Controls

    private System.Windows.Forms.Label lblSource;
    private System.Windows.Forms.Label lblTarget;
    private System.Windows.Forms.TextBox txtSource;
    private System.Windows.Forms.TextBox txtTarget;
    private System.Windows.Forms.ProgressBar prbBytesWritten;

    private System.ComponentModel.Container components = null;

    #endregion

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      this.prbBytesWritten = new System.Windows.Forms.ProgressBar();
      this.lblSource = new System.Windows.Forms.Label();
      this.lblTarget = new System.Windows.Forms.Label();
      this.txtSource = new System.Windows.Forms.TextBox();
      this.txtTarget = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // prbBytesWritten
      // 
      this.prbBytesWritten.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.prbBytesWritten.Location = new System.Drawing.Point(8, 64);
      this.prbBytesWritten.Name = "prbBytesWritten";
      this.prbBytesWritten.Size = new System.Drawing.Size(468, 24);
      this.prbBytesWritten.TabIndex = 0;
      // 
      // lblSource
      // 
      this.lblSource.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblSource.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblSource.Location = new System.Drawing.Point(8, 12);
      this.lblSource.Name = "lblSource";
      this.lblSource.Size = new System.Drawing.Size(40, 16);
      this.lblSource.TabIndex = 1;
      this.lblSource.Text = "Source:";
      // 
      // lblTarget
      // 
      this.lblTarget.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblTarget.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblTarget.Location = new System.Drawing.Point(8, 40);
      this.lblTarget.Name = "lblTarget";
      this.lblTarget.Size = new System.Drawing.Size(40, 16);
      this.lblTarget.TabIndex = 2;
      this.lblTarget.Text = "Target:";
      // 
      // txtSource
      // 
      this.txtSource.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtSource.Location = new System.Drawing.Point(52, 8);
      this.txtSource.Name = "txtSource";
      this.txtSource.ReadOnly = true;
      this.txtSource.Size = new System.Drawing.Size(424, 20);
      this.txtSource.TabIndex = 3;
      // 
      // txtTarget
      // 
      this.txtTarget.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtTarget.Location = new System.Drawing.Point(52, 36);
      this.txtTarget.Name = "txtTarget";
      this.txtTarget.ReadOnly = true;
      this.txtTarget.Size = new System.Drawing.Size(424, 20);
      this.txtTarget.TabIndex = 4;
      // 
      // WaveWriter
      // 
      this.ClientSize = new System.Drawing.Size(484, 96);
      this.ControlBox = false;
      this.Controls.Add(this.txtTarget);
      this.Controls.Add(this.txtSource);
      this.Controls.Add(this.lblTarget);
      this.Controls.Add(this.lblSource);
      this.Controls.Add(this.prbBytesWritten);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Name = "WaveWriter";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Decoding to Wave...";
      this.Activated += new System.EventHandler(this.WaveWriter_Activated);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

  }

}
