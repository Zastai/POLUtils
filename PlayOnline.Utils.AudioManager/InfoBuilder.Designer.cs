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

  public partial class InfoBuilder {

    #region Controls

    private System.Windows.Forms.ProgressBar prbApplication;
    private System.Windows.Forms.Label lblApplication;
    private System.Windows.Forms.TextBox txtApplication;
    private System.Windows.Forms.TextBox txtDirectory;
    private System.Windows.Forms.Label lblDirectory;
    private System.Windows.Forms.ProgressBar prbDirectory;
    private System.Windows.Forms.TextBox txtFile;
    private System.Windows.Forms.Label lblFile;
    private System.Windows.Forms.ProgressBar prbFile;
    private System.ComponentModel.Container components = null;

    #endregion

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      this.prbApplication = new System.Windows.Forms.ProgressBar();
      this.lblApplication = new System.Windows.Forms.Label();
      this.txtApplication = new System.Windows.Forms.TextBox();
      this.txtDirectory = new System.Windows.Forms.TextBox();
      this.lblDirectory = new System.Windows.Forms.Label();
      this.prbDirectory = new System.Windows.Forms.ProgressBar();
      this.txtFile = new System.Windows.Forms.TextBox();
      this.lblFile = new System.Windows.Forms.Label();
      this.prbFile = new System.Windows.Forms.ProgressBar();
      this.SuspendLayout();
      // 
      // prbApplication
      // 
      this.prbApplication.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.prbApplication.Location = new System.Drawing.Point(356, 12);
      this.prbApplication.Name = "prbApplication";
      this.prbApplication.Size = new System.Drawing.Size(152, 20);
      this.prbApplication.TabIndex = 2;
      // 
      // lblApplication
      // 
      this.lblApplication.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblApplication.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblApplication.Location = new System.Drawing.Point(8, 16);
      this.lblApplication.Name = "lblApplication";
      this.lblApplication.Size = new System.Drawing.Size(56, 16);
      this.lblApplication.TabIndex = 0;
      this.lblApplication.Text = "Application:";
      // 
      // txtApplication
      // 
      this.txtApplication.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtApplication.Location = new System.Drawing.Point(68, 12);
      this.txtApplication.Name = "txtApplication";
      this.txtApplication.ReadOnly = true;
      this.txtApplication.Size = new System.Drawing.Size(280, 20);
      this.txtApplication.TabIndex = 1;
      // 
      // txtDirectory
      // 
      this.txtDirectory.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtDirectory.Location = new System.Drawing.Point(68, 40);
      this.txtDirectory.Name = "txtDirectory";
      this.txtDirectory.ReadOnly = true;
      this.txtDirectory.Size = new System.Drawing.Size(280, 20);
      this.txtDirectory.TabIndex = 4;
      // 
      // lblDirectory
      // 
      this.lblDirectory.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblDirectory.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblDirectory.Location = new System.Drawing.Point(8, 44);
      this.lblDirectory.Name = "lblDirectory";
      this.lblDirectory.Size = new System.Drawing.Size(56, 16);
      this.lblDirectory.TabIndex = 3;
      this.lblDirectory.Text = "Directory:";
      // 
      // prbDirectory
      // 
      this.prbDirectory.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.prbDirectory.Location = new System.Drawing.Point(356, 40);
      this.prbDirectory.Name = "prbDirectory";
      this.prbDirectory.Size = new System.Drawing.Size(152, 20);
      this.prbDirectory.TabIndex = 5;
      // 
      // txtFile
      // 
      this.txtFile.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtFile.Location = new System.Drawing.Point(68, 68);
      this.txtFile.Name = "txtFile";
      this.txtFile.ReadOnly = true;
      this.txtFile.Size = new System.Drawing.Size(280, 20);
      this.txtFile.TabIndex = 7;
      // 
      // lblFile
      // 
      this.lblFile.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblFile.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblFile.Location = new System.Drawing.Point(8, 72);
      this.lblFile.Name = "lblFile";
      this.lblFile.Size = new System.Drawing.Size(56, 16);
      this.lblFile.TabIndex = 6;
      this.lblFile.Text = "File:";
      // 
      // prbFile
      // 
      this.prbFile.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.prbFile.Location = new System.Drawing.Point(356, 68);
      this.prbFile.Name = "prbFile";
      this.prbFile.Size = new System.Drawing.Size(152, 20);
      this.prbFile.TabIndex = 8;
      // 
      // InfoBuilder
      // 
      this.ClientSize = new System.Drawing.Size(518, 96);
      this.ControlBox = false;
      this.Controls.Add(this.txtFile);
      this.Controls.Add(this.txtDirectory);
      this.Controls.Add(this.txtApplication);
      this.Controls.Add(this.lblFile);
      this.Controls.Add(this.prbFile);
      this.Controls.Add(this.lblDirectory);
      this.Controls.Add(this.prbDirectory);
      this.Controls.Add(this.lblApplication);
      this.Controls.Add(this.prbApplication);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Name = "InfoBuilder";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Gathering Information...";
      this.VisibleChanged += new System.EventHandler(this.InfoBuilder_VisibleChanged);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

  }

}
