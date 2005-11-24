namespace POLUtils {

  public partial class POLUtilsUI {

    #region Controls

    private System.Windows.Forms.GroupBox grpRegion;
    private System.Windows.Forms.Label lblSelectedRegion;
    private System.Windows.Forms.TextBox txtSelectedRegion;
    private System.Windows.Forms.Button btnChooseRegion;
    private System.Windows.Forms.ComboBox cmbCultures;

    private System.Windows.Forms.Button btnAudioManager;
    private System.Windows.Forms.Button btnFFXIMacroManager;
    private System.Windows.Forms.Button btnFFXIDataBrowser;
    private System.Windows.Forms.Button btnTetraViewer;
    private System.Windows.Forms.Label lblToolLanguage;
    private System.Windows.Forms.Button btnFFXIConfigEditor;
    private System.Windows.Forms.Button btnFFXIItemComparison;

    private System.ComponentModel.Container components = null;

    #endregion

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(POLUtilsUI));
      this.btnTetraViewer = new System.Windows.Forms.Button();
      this.grpRegion = new System.Windows.Forms.GroupBox();
      this.cmbCultures = new System.Windows.Forms.ComboBox();
      this.btnChooseRegion = new System.Windows.Forms.Button();
      this.txtSelectedRegion = new System.Windows.Forms.TextBox();
      this.lblSelectedRegion = new System.Windows.Forms.Label();
      this.lblToolLanguage = new System.Windows.Forms.Label();
      this.btnAudioManager = new System.Windows.Forms.Button();
      this.btnFFXIMacroManager = new System.Windows.Forms.Button();
      this.btnFFXIDataBrowser = new System.Windows.Forms.Button();
      this.btnFFXIConfigEditor = new System.Windows.Forms.Button();
      this.btnFFXIItemComparison = new System.Windows.Forms.Button();
      this.grpRegion.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnTetraViewer
      // 
      resources.ApplyResources(this.btnTetraViewer, "btnTetraViewer");
      this.btnTetraViewer.Name = "btnTetraViewer";
      this.btnTetraViewer.Click += new System.EventHandler(this.btnTetraViewer_Click);
      // 
      // grpRegion
      // 
      resources.ApplyResources(this.grpRegion, "grpRegion");
      this.grpRegion.Controls.Add(this.cmbCultures);
      this.grpRegion.Controls.Add(this.btnChooseRegion);
      this.grpRegion.Controls.Add(this.txtSelectedRegion);
      this.grpRegion.Controls.Add(this.lblSelectedRegion);
      this.grpRegion.Controls.Add(this.lblToolLanguage);
      this.grpRegion.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.grpRegion.Name = "grpRegion";
      this.grpRegion.TabStop = false;
      // 
      // cmbCultures
      // 
      resources.ApplyResources(this.cmbCultures, "cmbCultures");
      this.cmbCultures.DisplayMember = "Name";
      this.cmbCultures.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbCultures.FormattingEnabled = true;
      this.cmbCultures.Name = "cmbCultures";
      this.cmbCultures.SelectedIndexChanged += new System.EventHandler(this.cmbCultures_SelectedIndexChanged);
      // 
      // btnChooseRegion
      // 
      resources.ApplyResources(this.btnChooseRegion, "btnChooseRegion");
      this.btnChooseRegion.Name = "btnChooseRegion";
      this.btnChooseRegion.Click += new System.EventHandler(this.btnChooseRegion_Click);
      // 
      // txtSelectedRegion
      // 
      resources.ApplyResources(this.txtSelectedRegion, "txtSelectedRegion");
      this.txtSelectedRegion.Name = "txtSelectedRegion";
      this.txtSelectedRegion.ReadOnly = true;
      this.txtSelectedRegion.TabStop = false;
      // 
      // lblSelectedRegion
      // 
      this.lblSelectedRegion.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblSelectedRegion, "lblSelectedRegion");
      this.lblSelectedRegion.Name = "lblSelectedRegion";
      // 
      // lblToolLanguage
      // 
      this.lblToolLanguage.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.lblToolLanguage, "lblToolLanguage");
      this.lblToolLanguage.Name = "lblToolLanguage";
      // 
      // btnAudioManager
      // 
      resources.ApplyResources(this.btnAudioManager, "btnAudioManager");
      this.btnAudioManager.Name = "btnAudioManager";
      this.btnAudioManager.Click += new System.EventHandler(this.btnAudioManager_Click);
      // 
      // btnFFXIMacroManager
      // 
      resources.ApplyResources(this.btnFFXIMacroManager, "btnFFXIMacroManager");
      this.btnFFXIMacroManager.Name = "btnFFXIMacroManager";
      this.btnFFXIMacroManager.Click += new System.EventHandler(this.btnFFXIMacroManager_Click);
      // 
      // btnFFXIDataBrowser
      // 
      resources.ApplyResources(this.btnFFXIDataBrowser, "btnFFXIDataBrowser");
      this.btnFFXIDataBrowser.Name = "btnFFXIDataBrowser";
      this.btnFFXIDataBrowser.Click += new System.EventHandler(this.btnFFXIDataBrowser_Click);
      // 
      // btnFFXIConfigEditor
      // 
      resources.ApplyResources(this.btnFFXIConfigEditor, "btnFFXIConfigEditor");
      this.btnFFXIConfigEditor.Name = "btnFFXIConfigEditor";
      this.btnFFXIConfigEditor.Click += new System.EventHandler(this.btnFFXIConfigEditor_Click);
      // 
      // btnFFXIItemComparison
      // 
      resources.ApplyResources(this.btnFFXIItemComparison, "btnFFXIItemComparison");
      this.btnFFXIItemComparison.Name = "btnFFXIItemComparison";
      this.btnFFXIItemComparison.Click += new System.EventHandler(this.btnFFXIItemComparison_Click);
      // 
      // POLUtilsUI
      // 
      resources.ApplyResources(this, "$this");
      this.Controls.Add(this.btnFFXIItemComparison);
      this.Controls.Add(this.btnFFXIConfigEditor);
      this.Controls.Add(this.btnFFXIDataBrowser);
      this.Controls.Add(this.btnFFXIMacroManager);
      this.Controls.Add(this.btnAudioManager);
      this.Controls.Add(this.grpRegion);
      this.Controls.Add(this.btnTetraViewer);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.Name = "POLUtilsUI";
      this.grpRegion.ResumeLayout(false);
      this.grpRegion.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

  }

}
