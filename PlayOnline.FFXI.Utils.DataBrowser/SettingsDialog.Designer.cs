namespace PlayOnline.FFXI.Utils.DataBrowser {

  internal partial class SettingsDialog {

    #region Controls

    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.GroupBox grpScanSettings;
    private System.Windows.Forms.CheckBox chkFSShowProgressDetails;
    private System.Windows.Forms.CheckBox chkFSAllowAbort;

    private System.ComponentModel.Container components = null;

    #endregion

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsDialog));
      this.btnCancel = new System.Windows.Forms.Button();
      this.btnOK = new System.Windows.Forms.Button();
      this.grpScanSettings = new System.Windows.Forms.GroupBox();
      this.chkFSAllowAbort = new System.Windows.Forms.CheckBox();
      this.chkFSShowProgressDetails = new System.Windows.Forms.CheckBox();
      this.grpScanSettings.SuspendLayout();
      this.SuspendLayout();
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
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // grpScanSettings
      // 
      resources.ApplyResources(this.grpScanSettings, "grpScanSettings");
      this.grpScanSettings.Controls.Add(this.chkFSAllowAbort);
      this.grpScanSettings.Controls.Add(this.chkFSShowProgressDetails);
      this.grpScanSettings.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.grpScanSettings.Name = "grpScanSettings";
      this.grpScanSettings.TabStop = false;
      // 
      // chkFSAllowAbort
      // 
      resources.ApplyResources(this.chkFSAllowAbort, "chkFSAllowAbort");
      this.chkFSAllowAbort.Name = "chkFSAllowAbort";
      // 
      // chkFSShowProgressDetails
      // 
      resources.ApplyResources(this.chkFSShowProgressDetails, "chkFSShowProgressDetails");
      this.chkFSShowProgressDetails.Name = "chkFSShowProgressDetails";
      // 
      // SettingsDialog
      // 
      this.AcceptButton = this.btnOK;
      this.CancelButton = this.btnCancel;
      resources.ApplyResources(this, "$this");
      this.Controls.Add(this.grpScanSettings);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.btnCancel);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "SettingsDialog";
      this.ShowInTaskbar = false;
      this.grpScanSettings.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

  }

}
