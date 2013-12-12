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

  public partial class PleaseWaitDialog {

    #region Controls

    private System.Windows.Forms.Label lblMessage;

    private System.ComponentModel.Container components = null;

    #endregion

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      this.lblMessage = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // lblMessage
      // 
      this.lblMessage.BackColor = System.Drawing.Color.Transparent;
      this.lblMessage.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
      this.lblMessage.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblMessage.Location = new System.Drawing.Point(0, 0);
      this.lblMessage.Name = "lblMessage";
      this.lblMessage.Size = new System.Drawing.Size(294, 76);
      this.lblMessage.TabIndex = 0;
      this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.lblMessage.UseMnemonic = false;
      // 
      // PleaseWaitDialog
      // 
      this.ClientSize = new System.Drawing.Size(294, 76);
      this.ControlBox = false;
      this.Controls.Add(this.lblMessage);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.Name = "PleaseWaitDialog";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Please Wait...";
      this.ResumeLayout(false);

    }

    #endregion

  }

}
