namespace PlayOnline.FFXI.Utils.DataBrowser {

  internal partial class XMLOptionDialog {

    #region Controls

    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.GroupBox grpDisplayMode;
    private System.Windows.Forms.ComboBox cmbItemType;
    private System.Windows.Forms.ComboBox cmbLanguage;
    private System.Windows.Forms.GroupBox grpFolder;
    private System.Windows.Forms.TextBox txtFolder;
    private System.Windows.Forms.Button btnBrowseFolder;

    private System.ComponentModel.Container components = null;

    #endregion

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XMLOptionDialog));
      this.btnOK = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.grpDisplayMode = new System.Windows.Forms.GroupBox();
      this.cmbItemType = new System.Windows.Forms.ComboBox();
      this.cmbLanguage = new System.Windows.Forms.ComboBox();
      this.grpFolder = new System.Windows.Forms.GroupBox();
      this.txtFolder = new System.Windows.Forms.TextBox();
      this.btnBrowseFolder = new System.Windows.Forms.Button();
      this.grpDisplayMode.SuspendLayout();
      this.grpFolder.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnOK
      // 
      resources.ApplyResources(this.btnOK, "btnOK");
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOK.Name = "btnOK";
      // 
      // btnCancel
      // 
      resources.ApplyResources(this.btnCancel, "btnCancel");
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Name = "btnCancel";
      // 
      // grpDisplayMode
      // 
      this.grpDisplayMode.Controls.Add(this.cmbItemType);
      this.grpDisplayMode.Controls.Add(this.cmbLanguage);
      this.grpDisplayMode.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.grpDisplayMode, "grpDisplayMode");
      this.grpDisplayMode.Name = "grpDisplayMode";
      this.grpDisplayMode.TabStop = false;
      // 
      // cmbItemType
      // 
      this.cmbItemType.DisplayMember = "Name";
      this.cmbItemType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbItemType.FormattingEnabled = true;
      resources.ApplyResources(this.cmbItemType, "cmbItemType");
      this.cmbItemType.Name = "cmbItemType";
      this.cmbItemType.Sorted = true;
      this.cmbItemType.SelectedIndexChanged += new System.EventHandler(this.cmbItemType_SelectedIndexChanged);
      // 
      // cmbLanguage
      // 
      this.cmbLanguage.DisplayMember = "Name";
      this.cmbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbLanguage.FormattingEnabled = true;
      resources.ApplyResources(this.cmbLanguage, "cmbLanguage");
      this.cmbLanguage.Name = "cmbLanguage";
      this.cmbLanguage.Sorted = true;
      this.cmbLanguage.SelectedIndexChanged += new System.EventHandler(this.cmbLanguage_SelectedIndexChanged);
      // 
      // grpFolder
      // 
      this.grpFolder.Controls.Add(this.txtFolder);
      this.grpFolder.Controls.Add(this.btnBrowseFolder);
      this.grpFolder.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.grpFolder, "grpFolder");
      this.grpFolder.Name = "grpFolder";
      this.grpFolder.TabStop = false;
      // 
      // txtFolder
      // 
      resources.ApplyResources(this.txtFolder, "txtFolder");
      this.txtFolder.Name = "txtFolder";
      this.txtFolder.ReadOnly = true;
      // 
      // btnBrowseFolder
      // 
      resources.ApplyResources(this.btnBrowseFolder, "btnBrowseFolder");
      this.btnBrowseFolder.Name = "btnBrowseFolder";
      this.btnBrowseFolder.Click += new System.EventHandler(this.btnBrowseFolder_Click);
      // 
      // XMLOptionDialog
      // 
      this.AcceptButton = this.btnOK;
      this.CancelButton = this.btnCancel;
      resources.ApplyResources(this, "$this");
      this.Controls.Add(this.grpFolder);
      this.Controls.Add(this.grpDisplayMode);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnOK);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "XMLOptionDialog";
      this.ShowInTaskbar = false;
      this.grpDisplayMode.ResumeLayout(false);
      this.grpFolder.ResumeLayout(false);
      this.grpFolder.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

  }

}
