// $Id$

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace PlayOnline.FFXI.Utils.DataBrowser {

  internal class ExportMethodDialog : System.Windows.Forms.Form {
  
    #region Controls

    private System.Windows.Forms.Label lblPrompt;
    private System.Windows.Forms.RadioButton optCSV;
    private System.Windows.Forms.RadioButton optXML;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Button btnCancel;

    private System.ComponentModel.Container components = null;

    #endregion

    public ExportMethodDialog() {
      this.InitializeComponent();
    }

    public ItemExportMethod SelectedMethod {
      get {
	if (this.optCSV.Checked)
	  return ItemExportMethod.CSV;
	if (this.optXML.Checked)
	  return ItemExportMethod.XML;
	return ItemExportMethod.UserSelect;
      }
    }

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
	components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ExportMethodDialog));
      this.lblPrompt = new System.Windows.Forms.Label();
      this.optCSV = new System.Windows.Forms.RadioButton();
      this.optXML = new System.Windows.Forms.RadioButton();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnOK = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // lblPrompt
      // 
      this.lblPrompt.AccessibleDescription = resources.GetString("lblPrompt.AccessibleDescription");
      this.lblPrompt.AccessibleName = resources.GetString("lblPrompt.AccessibleName");
      this.lblPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblPrompt.Anchor")));
      this.lblPrompt.AutoSize = ((bool)(resources.GetObject("lblPrompt.AutoSize")));
      this.lblPrompt.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblPrompt.Dock")));
      this.lblPrompt.Enabled = ((bool)(resources.GetObject("lblPrompt.Enabled")));
      this.lblPrompt.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblPrompt.Font = ((System.Drawing.Font)(resources.GetObject("lblPrompt.Font")));
      this.lblPrompt.Image = ((System.Drawing.Image)(resources.GetObject("lblPrompt.Image")));
      this.lblPrompt.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblPrompt.ImageAlign")));
      this.lblPrompt.ImageIndex = ((int)(resources.GetObject("lblPrompt.ImageIndex")));
      this.lblPrompt.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblPrompt.ImeMode")));
      this.lblPrompt.Location = ((System.Drawing.Point)(resources.GetObject("lblPrompt.Location")));
      this.lblPrompt.Name = "lblPrompt";
      this.lblPrompt.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblPrompt.RightToLeft")));
      this.lblPrompt.Size = ((System.Drawing.Size)(resources.GetObject("lblPrompt.Size")));
      this.lblPrompt.TabIndex = ((int)(resources.GetObject("lblPrompt.TabIndex")));
      this.lblPrompt.Text = resources.GetString("lblPrompt.Text");
      this.lblPrompt.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblPrompt.TextAlign")));
      this.lblPrompt.Visible = ((bool)(resources.GetObject("lblPrompt.Visible")));
      // 
      // optCSV
      // 
      this.optCSV.AccessibleDescription = resources.GetString("optCSV.AccessibleDescription");
      this.optCSV.AccessibleName = resources.GetString("optCSV.AccessibleName");
      this.optCSV.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("optCSV.Anchor")));
      this.optCSV.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("optCSV.Appearance")));
      this.optCSV.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("optCSV.BackgroundImage")));
      this.optCSV.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("optCSV.CheckAlign")));
      this.optCSV.Checked = true;
      this.optCSV.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("optCSV.Dock")));
      this.optCSV.Enabled = ((bool)(resources.GetObject("optCSV.Enabled")));
      this.optCSV.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("optCSV.FlatStyle")));
      this.optCSV.Font = ((System.Drawing.Font)(resources.GetObject("optCSV.Font")));
      this.optCSV.Image = ((System.Drawing.Image)(resources.GetObject("optCSV.Image")));
      this.optCSV.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("optCSV.ImageAlign")));
      this.optCSV.ImageIndex = ((int)(resources.GetObject("optCSV.ImageIndex")));
      this.optCSV.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("optCSV.ImeMode")));
      this.optCSV.Location = ((System.Drawing.Point)(resources.GetObject("optCSV.Location")));
      this.optCSV.Name = "optCSV";
      this.optCSV.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("optCSV.RightToLeft")));
      this.optCSV.Size = ((System.Drawing.Size)(resources.GetObject("optCSV.Size")));
      this.optCSV.TabIndex = ((int)(resources.GetObject("optCSV.TabIndex")));
      this.optCSV.TabStop = true;
      this.optCSV.Text = resources.GetString("optCSV.Text");
      this.optCSV.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("optCSV.TextAlign")));
      this.optCSV.Visible = ((bool)(resources.GetObject("optCSV.Visible")));
      // 
      // optXML
      // 
      this.optXML.AccessibleDescription = resources.GetString("optXML.AccessibleDescription");
      this.optXML.AccessibleName = resources.GetString("optXML.AccessibleName");
      this.optXML.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("optXML.Anchor")));
      this.optXML.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("optXML.Appearance")));
      this.optXML.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("optXML.BackgroundImage")));
      this.optXML.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("optXML.CheckAlign")));
      this.optXML.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("optXML.Dock")));
      this.optXML.Enabled = ((bool)(resources.GetObject("optXML.Enabled")));
      this.optXML.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("optXML.FlatStyle")));
      this.optXML.Font = ((System.Drawing.Font)(resources.GetObject("optXML.Font")));
      this.optXML.Image = ((System.Drawing.Image)(resources.GetObject("optXML.Image")));
      this.optXML.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("optXML.ImageAlign")));
      this.optXML.ImageIndex = ((int)(resources.GetObject("optXML.ImageIndex")));
      this.optXML.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("optXML.ImeMode")));
      this.optXML.Location = ((System.Drawing.Point)(resources.GetObject("optXML.Location")));
      this.optXML.Name = "optXML";
      this.optXML.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("optXML.RightToLeft")));
      this.optXML.Size = ((System.Drawing.Size)(resources.GetObject("optXML.Size")));
      this.optXML.TabIndex = ((int)(resources.GetObject("optXML.TabIndex")));
      this.optXML.Text = resources.GetString("optXML.Text");
      this.optXML.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("optXML.TextAlign")));
      this.optXML.Visible = ((bool)(resources.GetObject("optXML.Visible")));
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
      // 
      // ExportMethodDialog
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
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.optXML);
      this.Controls.Add(this.optCSV);
      this.Controls.Add(this.lblPrompt);
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
      this.Name = "ExportMethodDialog";
      this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
      this.ShowInTaskbar = false;
      this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
      this.Text = resources.GetString("$this.Text");
      this.ResumeLayout(false);

    }

    #endregion

  }

}
