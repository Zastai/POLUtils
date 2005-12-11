namespace ItemListUpgrade {

  partial class MainWindow {

    #region Windows Form Designer generated code

    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
	components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
      this.btnOldFile = new System.Windows.Forms.Button();
      this.btnNewFile = new System.Windows.Forms.Button();
      this.btnPerformUpgrade = new System.Windows.Forms.Button();
      this.txtOldFile = new System.Windows.Forms.TextBox();
      this.txtNewFile = new System.Windows.Forms.TextBox();
      this.lblOldFile = new System.Windows.Forms.Label();
      this.lblNewFile = new System.Windows.Forms.Label();
      this.dlgOldFile = new System.Windows.Forms.OpenFileDialog();
      this.dlgNewFile = new System.Windows.Forms.SaveFileDialog();
      this.SuspendLayout();
      // 
      // btnOldFile
      // 
      resources.ApplyResources(this.btnOldFile, "btnOldFile");
      this.btnOldFile.Name = "btnOldFile";
      this.btnOldFile.UseMnemonic = false;
      this.btnOldFile.UseVisualStyleBackColor = true;
      this.btnOldFile.Click += new System.EventHandler(this.btnOldFile_Click);
      // 
      // btnNewFile
      // 
      resources.ApplyResources(this.btnNewFile, "btnNewFile");
      this.btnNewFile.Name = "btnNewFile";
      this.btnNewFile.UseMnemonic = false;
      this.btnNewFile.UseVisualStyleBackColor = true;
      this.btnNewFile.Click += new System.EventHandler(this.btnNewFile_Click);
      // 
      // btnPerformUpgrade
      // 
      resources.ApplyResources(this.btnPerformUpgrade, "btnPerformUpgrade");
      this.btnPerformUpgrade.Name = "btnPerformUpgrade";
      this.btnPerformUpgrade.UseVisualStyleBackColor = true;
      this.btnPerformUpgrade.Click += new System.EventHandler(this.btnPerformUpgrade_Click);
      // 
      // txtOldFile
      // 
      resources.ApplyResources(this.txtOldFile, "txtOldFile");
      this.txtOldFile.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.txtOldFile.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
      this.txtOldFile.Name = "txtOldFile";
      this.txtOldFile.TextChanged += new System.EventHandler(this.txtOldFile_TextChanged);
      // 
      // txtNewFile
      // 
      resources.ApplyResources(this.txtNewFile, "txtNewFile");
      this.txtNewFile.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.txtNewFile.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
      this.txtNewFile.Name = "txtNewFile";
      this.txtNewFile.TextChanged += new System.EventHandler(this.txtNewFile_TextChanged);
      // 
      // lblOldFile
      // 
      resources.ApplyResources(this.lblOldFile, "lblOldFile");
      this.lblOldFile.BackColor = System.Drawing.Color.Transparent;
      this.lblOldFile.Name = "lblOldFile";
      this.lblOldFile.UseMnemonic = false;
      // 
      // lblNewFile
      // 
      resources.ApplyResources(this.lblNewFile, "lblNewFile");
      this.lblNewFile.BackColor = System.Drawing.Color.Transparent;
      this.lblNewFile.Name = "lblNewFile";
      // 
      // dlgOldFile
      // 
      this.dlgOldFile.DefaultExt = "xml";
      resources.ApplyResources(this.dlgOldFile, "dlgOldFile");
      // 
      // dlgNewFile
      // 
      this.dlgNewFile.DefaultExt = "xml";
      resources.ApplyResources(this.dlgNewFile, "dlgNewFile");
      // 
      // MainWindow
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.lblNewFile);
      this.Controls.Add(this.lblOldFile);
      this.Controls.Add(this.txtNewFile);
      this.Controls.Add(this.txtOldFile);
      this.Controls.Add(this.btnPerformUpgrade);
      this.Controls.Add(this.btnNewFile);
      this.Controls.Add(this.btnOldFile);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "MainWindow";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnOldFile;
    private System.Windows.Forms.Button btnNewFile;
    private System.Windows.Forms.Button btnPerformUpgrade;
    private System.Windows.Forms.TextBox txtOldFile;
    private System.Windows.Forms.TextBox txtNewFile;
    private System.Windows.Forms.Label lblOldFile;
    private System.Windows.Forms.Label lblNewFile;
    private System.Windows.Forms.OpenFileDialog dlgOldFile;
    private System.Windows.Forms.SaveFileDialog dlgNewFile;

  }

}

