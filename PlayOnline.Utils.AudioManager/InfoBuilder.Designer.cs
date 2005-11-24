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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InfoBuilder));
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
      resources.ApplyResources(this.prbApplication, "prbApplication");
      this.prbApplication.Name = "prbApplication";
      // 
      // lblApplication
      // 
      this.lblApplication.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblApplication, "lblApplication");
      this.lblApplication.Name = "lblApplication";
      // 
      // txtApplication
      // 
      resources.ApplyResources(this.txtApplication, "txtApplication");
      this.txtApplication.Name = "txtApplication";
      this.txtApplication.ReadOnly = true;
      // 
      // txtDirectory
      // 
      resources.ApplyResources(this.txtDirectory, "txtDirectory");
      this.txtDirectory.Name = "txtDirectory";
      this.txtDirectory.ReadOnly = true;
      // 
      // lblDirectory
      // 
      this.lblDirectory.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblDirectory, "lblDirectory");
      this.lblDirectory.Name = "lblDirectory";
      // 
      // prbDirectory
      // 
      resources.ApplyResources(this.prbDirectory, "prbDirectory");
      this.prbDirectory.Name = "prbDirectory";
      // 
      // txtFile
      // 
      resources.ApplyResources(this.txtFile, "txtFile");
      this.txtFile.Name = "txtFile";
      this.txtFile.ReadOnly = true;
      // 
      // lblFile
      // 
      this.lblFile.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblFile, "lblFile");
      this.lblFile.Name = "lblFile";
      // 
      // prbFile
      // 
      resources.ApplyResources(this.prbFile, "prbFile");
      this.prbFile.Name = "prbFile";
      // 
      // InfoBuilder
      // 
      resources.ApplyResources(this, "$this");
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
      this.VisibleChanged += new System.EventHandler(this.InfoBuilder_VisibleChanged);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

  }

}
