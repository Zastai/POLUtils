namespace PlayOnline.FFXI.Utils.DataBrowser {

  internal partial class ExportMethodDialog {

    #region Controls

    private System.Windows.Forms.Label lblPrompt;
    private System.Windows.Forms.RadioButton optCSV;
    private System.Windows.Forms.RadioButton optXML;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Button btnCancel;

    private System.ComponentModel.Container components = null;

    #endregion

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportMethodDialog));
      this.lblPrompt = new System.Windows.Forms.Label();
      this.optCSV = new System.Windows.Forms.RadioButton();
      this.optXML = new System.Windows.Forms.RadioButton();
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnOK = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // lblPrompt
      // 
      resources.ApplyResources(this.lblPrompt, "lblPrompt");
      this.lblPrompt.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblPrompt.Name = "lblPrompt";
      // 
      // optCSV
      // 
      resources.ApplyResources(this.optCSV, "optCSV");
      this.optCSV.Checked = true;
      this.optCSV.Name = "optCSV";
      // 
      // optXML
      // 
      resources.ApplyResources(this.optXML, "optXML");
      this.optXML.Name = "optXML";
      // 
      // btnCancel
      // 
      resources.ApplyResources(this.btnCancel, "btnCancel");
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Name = "btnCancel";
      // 
      // btnOK
      // 
      resources.ApplyResources(this.btnOK, "btnOK");
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOK.Name = "btnOK";
      // 
      // ExportMethodDialog
      // 
      this.AcceptButton = this.btnOK;
      this.CancelButton = this.btnCancel;
      resources.ApplyResources(this, "$this");
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.optXML);
      this.Controls.Add(this.optCSV);
      this.Controls.Add(this.lblPrompt);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ExportMethodDialog";
      this.ShowInTaskbar = false;
      this.ResumeLayout(false);

    }

    #endregion

  }

}
