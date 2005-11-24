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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChooseRegionDialog));
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
      resources.ApplyResources(this.lblExplanation, "lblExplanation");
      this.lblExplanation.BackColor = System.Drawing.SystemColors.Control;
      this.lblExplanation.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblExplanation.Font = ((System.Drawing.Font) (resources.GetObject("lblExplanation.Font")));
      this.lblExplanation.Name = "lblExplanation";
      this.lblExplanation.RightToLeft = ((System.Windows.Forms.RightToLeft) (resources.GetObject("lblExplanation.RightToLeft")));
      // 
      // radJapan
      // 
      this.radJapan.AccessibleDescription = resources.GetString("radJapan.AccessibleDescription");
      this.radJapan.AccessibleName = resources.GetString("radJapan.AccessibleName");
      resources.ApplyResources(this.radJapan, "radJapan");
      this.radJapan.BackgroundImage = ((System.Drawing.Image) (resources.GetObject("radJapan.BackgroundImage")));
      this.radJapan.Font = ((System.Drawing.Font) (resources.GetObject("radJapan.Font")));
      this.radJapan.Name = "radJapan";
      this.radJapan.RightToLeft = ((System.Windows.Forms.RightToLeft) (resources.GetObject("radJapan.RightToLeft")));
      // 
      // radNorthAmerica
      // 
      this.radNorthAmerica.AccessibleDescription = resources.GetString("radNorthAmerica.AccessibleDescription");
      this.radNorthAmerica.AccessibleName = resources.GetString("radNorthAmerica.AccessibleName");
      resources.ApplyResources(this.radNorthAmerica, "radNorthAmerica");
      this.radNorthAmerica.BackgroundImage = ((System.Drawing.Image) (resources.GetObject("radNorthAmerica.BackgroundImage")));
      this.radNorthAmerica.Font = ((System.Drawing.Font) (resources.GetObject("radNorthAmerica.Font")));
      this.radNorthAmerica.Name = "radNorthAmerica";
      this.radNorthAmerica.RightToLeft = ((System.Windows.Forms.RightToLeft) (resources.GetObject("radNorthAmerica.RightToLeft")));
      // 
      // radEurope
      // 
      this.radEurope.AccessibleDescription = resources.GetString("radEurope.AccessibleDescription");
      this.radEurope.AccessibleName = resources.GetString("radEurope.AccessibleName");
      resources.ApplyResources(this.radEurope, "radEurope");
      this.radEurope.BackgroundImage = ((System.Drawing.Image) (resources.GetObject("radEurope.BackgroundImage")));
      this.radEurope.Font = ((System.Drawing.Font) (resources.GetObject("radEurope.Font")));
      this.radEurope.Name = "radEurope";
      this.radEurope.RightToLeft = ((System.Windows.Forms.RightToLeft) (resources.GetObject("radEurope.RightToLeft")));
      // 
      // btnOK
      // 
      this.btnOK.AccessibleDescription = resources.GetString("btnOK.AccessibleDescription");
      this.btnOK.AccessibleName = resources.GetString("btnOK.AccessibleName");
      resources.ApplyResources(this.btnOK, "btnOK");
      this.btnOK.BackgroundImage = ((System.Drawing.Image) (resources.GetObject("btnOK.BackgroundImage")));
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOK.Font = ((System.Drawing.Font) (resources.GetObject("btnOK.Font")));
      this.btnOK.Name = "btnOK";
      this.btnOK.RightToLeft = ((System.Windows.Forms.RightToLeft) (resources.GetObject("btnOK.RightToLeft")));
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // ChooseRegionDialog
      // 
      this.AcceptButton = this.btnOK;
      this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
      this.AccessibleName = resources.GetString("$this.AccessibleName");
      resources.ApplyResources(this, "$this");
      this.BackgroundImage = ((System.Drawing.Image) (resources.GetObject("$this.BackgroundImage")));
      this.ControlBox = false;
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.radEurope);
      this.Controls.Add(this.radNorthAmerica);
      this.Controls.Add(this.radJapan);
      this.Controls.Add(this.lblExplanation);
      this.Font = ((System.Drawing.Font) (resources.GetObject("$this.Font")));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = ((System.Drawing.Icon) (resources.GetObject("$this.Icon")));
      this.Name = "ChooseRegionDialog";
      this.RightToLeft = ((System.Windows.Forms.RightToLeft) (resources.GetObject("$this.RightToLeft")));
      this.ResumeLayout(false);

    }

    #endregion

  }

}
