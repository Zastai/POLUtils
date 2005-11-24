namespace PlayOnline.FFXI.Utils.DataBrowser {

  internal partial class FileScanDialog {

    #region Controls

    private System.Windows.Forms.ProgressBar prbScanProgress;
    private System.Windows.Forms.Label lblScanProgress;

    private System.ComponentModel.Container components = null;

    #endregion

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileScanDialog));
      this.prbScanProgress = new System.Windows.Forms.ProgressBar();
      this.lblScanProgress = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // prbScanProgress
      // 
      resources.ApplyResources(this.prbScanProgress, "prbScanProgress");
      this.prbScanProgress.Maximum = 1000;
      this.prbScanProgress.Name = "prbScanProgress";
      // 
      // lblScanProgress
      // 
      resources.ApplyResources(this.lblScanProgress, "lblScanProgress");
      this.lblScanProgress.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblScanProgress.Name = "lblScanProgress";
      // 
      // FileScanDialog
      // 
      resources.ApplyResources(this, "$this");
      this.Controls.Add(this.prbScanProgress);
      this.Controls.Add(this.lblScanProgress);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "FileScanDialog";
      this.ShowInTaskbar = false;
      this.Activated += new System.EventHandler(this.FileScanDialog_Activated);
      this.Closing += new System.ComponentModel.CancelEventHandler(this.FileScanDialog_Closing);
      this.ResumeLayout(false);

    }

    #endregion

  }

}
