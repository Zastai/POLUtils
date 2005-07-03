// $Id$

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace PlayOnline.FFXI {

  public class PleaseWaitDialog : System.Windows.Forms.Form {

    #region Controls

    private System.Windows.Forms.Label lblMessage;

    private System.ComponentModel.Container components = null;

    #endregion

    public PleaseWaitDialog(string Message) {
      this.InitializeComponent();
      this.lblMessage.Text = Message;
    }

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
	components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(PleaseWaitDialog));
      this.lblMessage = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // lblMessage
      // 
      this.lblMessage.AccessibleDescription = resources.GetString("lblMessage.AccessibleDescription");
      this.lblMessage.AccessibleName = resources.GetString("lblMessage.AccessibleName");
      this.lblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblMessage.Anchor")));
      this.lblMessage.AutoSize = ((bool)(resources.GetObject("lblMessage.AutoSize")));
      this.lblMessage.BackColor = System.Drawing.Color.Transparent;
      this.lblMessage.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblMessage.Dock")));
      this.lblMessage.Enabled = ((bool)(resources.GetObject("lblMessage.Enabled")));
      this.lblMessage.Font = ((System.Drawing.Font)(resources.GetObject("lblMessage.Font")));
      this.lblMessage.Image = ((System.Drawing.Image)(resources.GetObject("lblMessage.Image")));
      this.lblMessage.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblMessage.ImageAlign")));
      this.lblMessage.ImageIndex = ((int)(resources.GetObject("lblMessage.ImageIndex")));
      this.lblMessage.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblMessage.ImeMode")));
      this.lblMessage.Location = ((System.Drawing.Point)(resources.GetObject("lblMessage.Location")));
      this.lblMessage.Name = "lblMessage";
      this.lblMessage.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblMessage.RightToLeft")));
      this.lblMessage.Size = ((System.Drawing.Size)(resources.GetObject("lblMessage.Size")));
      this.lblMessage.TabIndex = ((int)(resources.GetObject("lblMessage.TabIndex")));
      this.lblMessage.Text = resources.GetString("lblMessage.Text");
      this.lblMessage.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblMessage.TextAlign")));
      this.lblMessage.UseMnemonic = false;
      this.lblMessage.Visible = ((bool)(resources.GetObject("lblMessage.Visible")));
      // 
      // PleaseWaitDialog
      // 
      this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
      this.AccessibleName = resources.GetString("$this.AccessibleName");
      this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
      this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
      this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
      this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
      this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
      this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
      this.ControlBox = false;
      this.Controls.Add(this.lblMessage);
      this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
      this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
      this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
      this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
      this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
      this.Name = "PleaseWaitDialog";
      this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
      this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
      this.Text = resources.GetString("$this.Text");
      this.ResumeLayout(false);

    }

    #endregion

  }

}
