// $Id$

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace PlayOnline.FFXI.Utils.DataBrowser {

  internal class SettingsDialog : System.Windows.Forms.Form {
  
    #region Controls

    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.GroupBox grpScanSettings;
    private System.Windows.Forms.CheckBox chkFSShowProgressDetails;
    private System.Windows.Forms.CheckBox chkFSAllowAbort;

    private System.ComponentModel.Container components = null;

    #endregion

    public SettingsDialog() {
      this.InitializeComponent();
      this.RetrieveSettings();
    }

    private void RetrieveSettings() {
      this.chkFSShowProgressDetails.Checked = FileScanDialog.ShowProgressDetails;
      this.chkFSAllowAbort.Checked          = FileScanDialog.AllowAbort;
    }

    private void StoreSettings() {
      FileScanDialog.ShowProgressDetails = this.chkFSShowProgressDetails.Checked;
      FileScanDialog.AllowAbort          = this.chkFSAllowAbort.Checked;
    }

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
	components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SettingsDialog));
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnOK = new System.Windows.Forms.Button();
      this.grpScanSettings = new System.Windows.Forms.GroupBox();
      this.chkFSShowProgressDetails = new System.Windows.Forms.CheckBox();
      this.chkFSAllowAbort = new System.Windows.Forms.CheckBox();
      this.grpScanSettings.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnCancel
      // 
      this.btnCancel.AccessibleDescription = resources.GetString("btnCancel.AccessibleDescription");
      this.btnCancel.AccessibleName = resources.GetString("btnCancel.AccessibleName");
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnCancel.Anchor")));
      this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnCancel.Dock")));
      this.btnCancel.Enabled = ((bool)(resources.GetObject("btnCancel.Enabled")));
      this.btnCancel.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnCancel.FlatStyle")));
      this.btnCancel.Font = ((System.Drawing.Font)(resources.GetObject("btnCancel.Font")));
      this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
      this.btnCancel.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnCancel.ImageAlign")));
      this.btnCancel.ImageIndex = ((int)(resources.GetObject("btnCancel.ImageIndex")));
      this.btnCancel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnCancel.ImeMode")));
      this.btnCancel.Location = ((System.Drawing.Point)(resources.GetObject("btnCancel.Location")));
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnCancel.RightToLeft")));
      this.btnCancel.Size = ((System.Drawing.Size)(resources.GetObject("btnCancel.Size")));
      this.btnCancel.TabIndex = ((int)(resources.GetObject("btnCancel.TabIndex")));
      this.btnCancel.Text = resources.GetString("btnCancel.Text");
      this.btnCancel.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnCancel.TextAlign")));
      this.btnCancel.Visible = ((bool)(resources.GetObject("btnCancel.Visible")));
      // 
      // btnOK
      // 
      this.btnOK.AccessibleDescription = resources.GetString("btnOK.AccessibleDescription");
      this.btnOK.AccessibleName = resources.GetString("btnOK.AccessibleName");
      this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnOK.Anchor")));
      this.btnOK.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnOK.BackgroundImage")));
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOK.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnOK.Dock")));
      this.btnOK.Enabled = ((bool)(resources.GetObject("btnOK.Enabled")));
      this.btnOK.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnOK.FlatStyle")));
      this.btnOK.Font = ((System.Drawing.Font)(resources.GetObject("btnOK.Font")));
      this.btnOK.Image = ((System.Drawing.Image)(resources.GetObject("btnOK.Image")));
      this.btnOK.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnOK.ImageAlign")));
      this.btnOK.ImageIndex = ((int)(resources.GetObject("btnOK.ImageIndex")));
      this.btnOK.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnOK.ImeMode")));
      this.btnOK.Location = ((System.Drawing.Point)(resources.GetObject("btnOK.Location")));
      this.btnOK.Name = "btnOK";
      this.btnOK.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnOK.RightToLeft")));
      this.btnOK.Size = ((System.Drawing.Size)(resources.GetObject("btnOK.Size")));
      this.btnOK.TabIndex = ((int)(resources.GetObject("btnOK.TabIndex")));
      this.btnOK.Text = resources.GetString("btnOK.Text");
      this.btnOK.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnOK.TextAlign")));
      this.btnOK.Visible = ((bool)(resources.GetObject("btnOK.Visible")));
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // grpScanSettings
      // 
      this.grpScanSettings.AccessibleDescription = resources.GetString("grpScanSettings.AccessibleDescription");
      this.grpScanSettings.AccessibleName = resources.GetString("grpScanSettings.AccessibleName");
      this.grpScanSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("grpScanSettings.Anchor")));
      this.grpScanSettings.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpScanSettings.BackgroundImage")));
      this.grpScanSettings.Controls.Add(this.chkFSAllowAbort);
      this.grpScanSettings.Controls.Add(this.chkFSShowProgressDetails);
      this.grpScanSettings.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("grpScanSettings.Dock")));
      this.grpScanSettings.Enabled = ((bool)(resources.GetObject("grpScanSettings.Enabled")));
      this.grpScanSettings.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.grpScanSettings.Font = ((System.Drawing.Font)(resources.GetObject("grpScanSettings.Font")));
      this.grpScanSettings.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("grpScanSettings.ImeMode")));
      this.grpScanSettings.Location = ((System.Drawing.Point)(resources.GetObject("grpScanSettings.Location")));
      this.grpScanSettings.Name = "grpScanSettings";
      this.grpScanSettings.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("grpScanSettings.RightToLeft")));
      this.grpScanSettings.Size = ((System.Drawing.Size)(resources.GetObject("grpScanSettings.Size")));
      this.grpScanSettings.TabIndex = ((int)(resources.GetObject("grpScanSettings.TabIndex")));
      this.grpScanSettings.TabStop = false;
      this.grpScanSettings.Text = resources.GetString("grpScanSettings.Text");
      this.grpScanSettings.Visible = ((bool)(resources.GetObject("grpScanSettings.Visible")));
      // 
      // chkFSShowProgressDetails
      // 
      this.chkFSShowProgressDetails.AccessibleDescription = resources.GetString("chkFSShowProgressDetails.AccessibleDescription");
      this.chkFSShowProgressDetails.AccessibleName = resources.GetString("chkFSShowProgressDetails.AccessibleName");
      this.chkFSShowProgressDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkFSShowProgressDetails.Anchor")));
      this.chkFSShowProgressDetails.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkFSShowProgressDetails.Appearance")));
      this.chkFSShowProgressDetails.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkFSShowProgressDetails.BackgroundImage")));
      this.chkFSShowProgressDetails.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkFSShowProgressDetails.CheckAlign")));
      this.chkFSShowProgressDetails.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkFSShowProgressDetails.Dock")));
      this.chkFSShowProgressDetails.Enabled = ((bool)(resources.GetObject("chkFSShowProgressDetails.Enabled")));
      this.chkFSShowProgressDetails.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkFSShowProgressDetails.FlatStyle")));
      this.chkFSShowProgressDetails.Font = ((System.Drawing.Font)(resources.GetObject("chkFSShowProgressDetails.Font")));
      this.chkFSShowProgressDetails.Image = ((System.Drawing.Image)(resources.GetObject("chkFSShowProgressDetails.Image")));
      this.chkFSShowProgressDetails.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkFSShowProgressDetails.ImageAlign")));
      this.chkFSShowProgressDetails.ImageIndex = ((int)(resources.GetObject("chkFSShowProgressDetails.ImageIndex")));
      this.chkFSShowProgressDetails.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkFSShowProgressDetails.ImeMode")));
      this.chkFSShowProgressDetails.Location = ((System.Drawing.Point)(resources.GetObject("chkFSShowProgressDetails.Location")));
      this.chkFSShowProgressDetails.Name = "chkFSShowProgressDetails";
      this.chkFSShowProgressDetails.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkFSShowProgressDetails.RightToLeft")));
      this.chkFSShowProgressDetails.Size = ((System.Drawing.Size)(resources.GetObject("chkFSShowProgressDetails.Size")));
      this.chkFSShowProgressDetails.TabIndex = ((int)(resources.GetObject("chkFSShowProgressDetails.TabIndex")));
      this.chkFSShowProgressDetails.Text = resources.GetString("chkFSShowProgressDetails.Text");
      this.chkFSShowProgressDetails.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkFSShowProgressDetails.TextAlign")));
      this.chkFSShowProgressDetails.Visible = ((bool)(resources.GetObject("chkFSShowProgressDetails.Visible")));
      // 
      // chkFSAllowAbort
      // 
      this.chkFSAllowAbort.AccessibleDescription = resources.GetString("chkFSAllowAbort.AccessibleDescription");
      this.chkFSAllowAbort.AccessibleName = resources.GetString("chkFSAllowAbort.AccessibleName");
      this.chkFSAllowAbort.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkFSAllowAbort.Anchor")));
      this.chkFSAllowAbort.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkFSAllowAbort.Appearance")));
      this.chkFSAllowAbort.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkFSAllowAbort.BackgroundImage")));
      this.chkFSAllowAbort.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkFSAllowAbort.CheckAlign")));
      this.chkFSAllowAbort.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkFSAllowAbort.Dock")));
      this.chkFSAllowAbort.Enabled = ((bool)(resources.GetObject("chkFSAllowAbort.Enabled")));
      this.chkFSAllowAbort.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkFSAllowAbort.FlatStyle")));
      this.chkFSAllowAbort.Font = ((System.Drawing.Font)(resources.GetObject("chkFSAllowAbort.Font")));
      this.chkFSAllowAbort.Image = ((System.Drawing.Image)(resources.GetObject("chkFSAllowAbort.Image")));
      this.chkFSAllowAbort.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkFSAllowAbort.ImageAlign")));
      this.chkFSAllowAbort.ImageIndex = ((int)(resources.GetObject("chkFSAllowAbort.ImageIndex")));
      this.chkFSAllowAbort.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkFSAllowAbort.ImeMode")));
      this.chkFSAllowAbort.Location = ((System.Drawing.Point)(resources.GetObject("chkFSAllowAbort.Location")));
      this.chkFSAllowAbort.Name = "chkFSAllowAbort";
      this.chkFSAllowAbort.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkFSAllowAbort.RightToLeft")));
      this.chkFSAllowAbort.Size = ((System.Drawing.Size)(resources.GetObject("chkFSAllowAbort.Size")));
      this.chkFSAllowAbort.TabIndex = ((int)(resources.GetObject("chkFSAllowAbort.TabIndex")));
      this.chkFSAllowAbort.Text = resources.GetString("chkFSAllowAbort.Text");
      this.chkFSAllowAbort.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkFSAllowAbort.TextAlign")));
      this.chkFSAllowAbort.Visible = ((bool)(resources.GetObject("chkFSAllowAbort.Visible")));
      // 
      // SettingsDialog
      // 
      this.AcceptButton = this.btnOK;
      this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
      this.AccessibleName = resources.GetString("$this.AccessibleName");
      this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
      this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
      this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
      this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
      this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
      this.CancelButton = this.btnCancel;
      this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
      this.Controls.Add(this.grpScanSettings);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.btnCancel);
      this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
      this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
      this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
      this.MaximizeBox = false;
      this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
      this.MinimizeBox = false;
      this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
      this.Name = "SettingsDialog";
      this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
      this.ShowInTaskbar = false;
      this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
      this.Text = resources.GetString("$this.Text");
      this.grpScanSettings.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private void btnOK_Click(object sender, System.EventArgs e) {
      this.StoreSettings();
    }

  }

}
