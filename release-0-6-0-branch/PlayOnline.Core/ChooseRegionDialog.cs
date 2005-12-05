// $Id$

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace PlayOnline.Core {

  internal class ChooseRegionDialog : System.Windows.Forms.Form {

    #region Controls

    private System.Windows.Forms.Label lblExplanation;
    private System.Windows.Forms.RadioButton radJapan;
    private System.Windows.Forms.RadioButton radNorthAmerica;
    private System.Windows.Forms.RadioButton radEurope;
    private System.Windows.Forms.Button btnOK;

    private System.ComponentModel.Container components = null;

    #endregion

    public ChooseRegionDialog() {
      this.InitializeComponent();
      this.radJapan.Checked        = (POL.SelectedRegion == POL.Region.Japan);
      this.radJapan.Enabled        = ((POL.AvailableRegions & POL.Region.Japan)        != 0);
      this.radNorthAmerica.Checked = (POL.SelectedRegion == POL.Region.NorthAmerica);
      this.radNorthAmerica.Enabled = ((POL.AvailableRegions & POL.Region.NorthAmerica) != 0);
      this.radEurope.Checked       = (POL.SelectedRegion == POL.Region.Europe);
      this.radEurope.Enabled       = ((POL.AvailableRegions & POL.Region.Europe)       != 0);
    }

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if(disposing && components != null)
	components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ChooseRegionDialog));
      this.lblExplanation = new System.Windows.Forms.Label();
      this.radJapan = new System.Windows.Forms.RadioButton();
      this.radNorthAmerica = new System.Windows.Forms.RadioButton();
      this.radEurope = new System.Windows.Forms.RadioButton();
      this.btnOK = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // lblExplanation
      // 
      this.lblExplanation.AccessibleDescription = resources.GetString("lblExplanation.AccessibleDescription");
      this.lblExplanation.AccessibleName = resources.GetString("lblExplanation.AccessibleName");
      this.lblExplanation.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblExplanation.Anchor")));
      this.lblExplanation.AutoSize = ((bool)(resources.GetObject("lblExplanation.AutoSize")));
      this.lblExplanation.BackColor = System.Drawing.SystemColors.Control;
      this.lblExplanation.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblExplanation.Dock")));
      this.lblExplanation.Enabled = ((bool)(resources.GetObject("lblExplanation.Enabled")));
      this.lblExplanation.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblExplanation.Font = ((System.Drawing.Font)(resources.GetObject("lblExplanation.Font")));
      this.lblExplanation.Image = ((System.Drawing.Image)(resources.GetObject("lblExplanation.Image")));
      this.lblExplanation.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblExplanation.ImageAlign")));
      this.lblExplanation.ImageIndex = ((int)(resources.GetObject("lblExplanation.ImageIndex")));
      this.lblExplanation.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblExplanation.ImeMode")));
      this.lblExplanation.Location = ((System.Drawing.Point)(resources.GetObject("lblExplanation.Location")));
      this.lblExplanation.Name = "lblExplanation";
      this.lblExplanation.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblExplanation.RightToLeft")));
      this.lblExplanation.Size = ((System.Drawing.Size)(resources.GetObject("lblExplanation.Size")));
      this.lblExplanation.TabIndex = ((int)(resources.GetObject("lblExplanation.TabIndex")));
      this.lblExplanation.Text = resources.GetString("lblExplanation.Text");
      this.lblExplanation.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblExplanation.TextAlign")));
      this.lblExplanation.Visible = ((bool)(resources.GetObject("lblExplanation.Visible")));
      // 
      // radJapan
      // 
      this.radJapan.AccessibleDescription = resources.GetString("radJapan.AccessibleDescription");
      this.radJapan.AccessibleName = resources.GetString("radJapan.AccessibleName");
      this.radJapan.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("radJapan.Anchor")));
      this.radJapan.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("radJapan.Appearance")));
      this.radJapan.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radJapan.BackgroundImage")));
      this.radJapan.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radJapan.CheckAlign")));
      this.radJapan.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("radJapan.Dock")));
      this.radJapan.Enabled = ((bool)(resources.GetObject("radJapan.Enabled")));
      this.radJapan.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("radJapan.FlatStyle")));
      this.radJapan.Font = ((System.Drawing.Font)(resources.GetObject("radJapan.Font")));
      this.radJapan.Image = ((System.Drawing.Image)(resources.GetObject("radJapan.Image")));
      this.radJapan.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radJapan.ImageAlign")));
      this.radJapan.ImageIndex = ((int)(resources.GetObject("radJapan.ImageIndex")));
      this.radJapan.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("radJapan.ImeMode")));
      this.radJapan.Location = ((System.Drawing.Point)(resources.GetObject("radJapan.Location")));
      this.radJapan.Name = "radJapan";
      this.radJapan.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("radJapan.RightToLeft")));
      this.radJapan.Size = ((System.Drawing.Size)(resources.GetObject("radJapan.Size")));
      this.radJapan.TabIndex = ((int)(resources.GetObject("radJapan.TabIndex")));
      this.radJapan.Text = resources.GetString("radJapan.Text");
      this.radJapan.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radJapan.TextAlign")));
      this.radJapan.Visible = ((bool)(resources.GetObject("radJapan.Visible")));
      // 
      // radNorthAmerica
      // 
      this.radNorthAmerica.AccessibleDescription = resources.GetString("radNorthAmerica.AccessibleDescription");
      this.radNorthAmerica.AccessibleName = resources.GetString("radNorthAmerica.AccessibleName");
      this.radNorthAmerica.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("radNorthAmerica.Anchor")));
      this.radNorthAmerica.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("radNorthAmerica.Appearance")));
      this.radNorthAmerica.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radNorthAmerica.BackgroundImage")));
      this.radNorthAmerica.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radNorthAmerica.CheckAlign")));
      this.radNorthAmerica.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("radNorthAmerica.Dock")));
      this.radNorthAmerica.Enabled = ((bool)(resources.GetObject("radNorthAmerica.Enabled")));
      this.radNorthAmerica.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("radNorthAmerica.FlatStyle")));
      this.radNorthAmerica.Font = ((System.Drawing.Font)(resources.GetObject("radNorthAmerica.Font")));
      this.radNorthAmerica.Image = ((System.Drawing.Image)(resources.GetObject("radNorthAmerica.Image")));
      this.radNorthAmerica.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radNorthAmerica.ImageAlign")));
      this.radNorthAmerica.ImageIndex = ((int)(resources.GetObject("radNorthAmerica.ImageIndex")));
      this.radNorthAmerica.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("radNorthAmerica.ImeMode")));
      this.radNorthAmerica.Location = ((System.Drawing.Point)(resources.GetObject("radNorthAmerica.Location")));
      this.radNorthAmerica.Name = "radNorthAmerica";
      this.radNorthAmerica.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("radNorthAmerica.RightToLeft")));
      this.radNorthAmerica.Size = ((System.Drawing.Size)(resources.GetObject("radNorthAmerica.Size")));
      this.radNorthAmerica.TabIndex = ((int)(resources.GetObject("radNorthAmerica.TabIndex")));
      this.radNorthAmerica.Text = resources.GetString("radNorthAmerica.Text");
      this.radNorthAmerica.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radNorthAmerica.TextAlign")));
      this.radNorthAmerica.Visible = ((bool)(resources.GetObject("radNorthAmerica.Visible")));
      // 
      // radEurope
      // 
      this.radEurope.AccessibleDescription = resources.GetString("radEurope.AccessibleDescription");
      this.radEurope.AccessibleName = resources.GetString("radEurope.AccessibleName");
      this.radEurope.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("radEurope.Anchor")));
      this.radEurope.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("radEurope.Appearance")));
      this.radEurope.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("radEurope.BackgroundImage")));
      this.radEurope.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radEurope.CheckAlign")));
      this.radEurope.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("radEurope.Dock")));
      this.radEurope.Enabled = ((bool)(resources.GetObject("radEurope.Enabled")));
      this.radEurope.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("radEurope.FlatStyle")));
      this.radEurope.Font = ((System.Drawing.Font)(resources.GetObject("radEurope.Font")));
      this.radEurope.Image = ((System.Drawing.Image)(resources.GetObject("radEurope.Image")));
      this.radEurope.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radEurope.ImageAlign")));
      this.radEurope.ImageIndex = ((int)(resources.GetObject("radEurope.ImageIndex")));
      this.radEurope.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("radEurope.ImeMode")));
      this.radEurope.Location = ((System.Drawing.Point)(resources.GetObject("radEurope.Location")));
      this.radEurope.Name = "radEurope";
      this.radEurope.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("radEurope.RightToLeft")));
      this.radEurope.Size = ((System.Drawing.Size)(resources.GetObject("radEurope.Size")));
      this.radEurope.TabIndex = ((int)(resources.GetObject("radEurope.TabIndex")));
      this.radEurope.Text = resources.GetString("radEurope.Text");
      this.radEurope.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("radEurope.TextAlign")));
      this.radEurope.Visible = ((bool)(resources.GetObject("radEurope.Visible")));
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
      // ChooseRegionDialog
      // 
      this.AcceptButton = this.btnOK;
      this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
      this.AccessibleName = resources.GetString("$this.AccessibleName");
      this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
      this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
      this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
      this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
      this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
      this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
      this.ControlBox = false;
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.radEurope);
      this.Controls.Add(this.radNorthAmerica);
      this.Controls.Add(this.radJapan);
      this.Controls.Add(this.lblExplanation);
      this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
      this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
      this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
      this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
      this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
      this.Name = "ChooseRegionDialog";
      this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
      this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
      this.Text = resources.GetString("$this.Text");
      this.ResumeLayout(false);

    }

    #endregion

    #region Events

    private void btnOK_Click(object sender, System.EventArgs e) {
      if (this.radJapan.Checked)        POL.SelectedRegion = POL.Region.Japan;
      if (this.radNorthAmerica.Checked) POL.SelectedRegion = POL.Region.NorthAmerica;
      if (this.radEurope.Checked)       POL.SelectedRegion = POL.Region.Europe;
    }

    #endregion

  }

}
