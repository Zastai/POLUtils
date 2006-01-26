namespace POLUtils {

  public partial class POLUtilsUI {

    #region Windows Form Designer generated code

    private System.ComponentModel.Container components = null;

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
      this.btnFFXIEngrishOnry = new System.Windows.Forms.Button();
      this.grpRegion.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnTetraViewer
      // 
      this.btnTetraViewer.AccessibleDescription = null;
      this.btnTetraViewer.AccessibleName = null;
      resources.ApplyResources(this.btnTetraViewer, "btnTetraViewer");
      this.btnTetraViewer.BackgroundImage = null;
      this.btnTetraViewer.Font = null;
      this.btnTetraViewer.Name = "btnTetraViewer";
      this.btnTetraViewer.Click += new System.EventHandler(this.btnTetraViewer_Click);
      // 
      // grpRegion
      // 
      this.grpRegion.AccessibleDescription = null;
      this.grpRegion.AccessibleName = null;
      resources.ApplyResources(this.grpRegion, "grpRegion");
      this.grpRegion.BackgroundImage = null;
      this.grpRegion.Controls.Add(this.cmbCultures);
      this.grpRegion.Controls.Add(this.btnChooseRegion);
      this.grpRegion.Controls.Add(this.txtSelectedRegion);
      this.grpRegion.Controls.Add(this.lblSelectedRegion);
      this.grpRegion.Controls.Add(this.lblToolLanguage);
      this.grpRegion.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.grpRegion.Font = null;
      this.grpRegion.Name = "grpRegion";
      this.grpRegion.TabStop = false;
      // 
      // cmbCultures
      // 
      this.cmbCultures.AccessibleDescription = null;
      this.cmbCultures.AccessibleName = null;
      resources.ApplyResources(this.cmbCultures, "cmbCultures");
      this.cmbCultures.BackgroundImage = null;
      this.cmbCultures.DisplayMember = "Name";
      this.cmbCultures.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbCultures.Font = null;
      this.cmbCultures.FormattingEnabled = true;
      this.cmbCultures.Name = "cmbCultures";
      this.cmbCultures.SelectedIndexChanged += new System.EventHandler(this.cmbCultures_SelectedIndexChanged);
      // 
      // btnChooseRegion
      // 
      this.btnChooseRegion.AccessibleDescription = null;
      this.btnChooseRegion.AccessibleName = null;
      resources.ApplyResources(this.btnChooseRegion, "btnChooseRegion");
      this.btnChooseRegion.BackgroundImage = null;
      this.btnChooseRegion.Font = null;
      this.btnChooseRegion.Name = "btnChooseRegion";
      this.btnChooseRegion.Click += new System.EventHandler(this.btnChooseRegion_Click);
      // 
      // txtSelectedRegion
      // 
      this.txtSelectedRegion.AccessibleDescription = null;
      this.txtSelectedRegion.AccessibleName = null;
      resources.ApplyResources(this.txtSelectedRegion, "txtSelectedRegion");
      this.txtSelectedRegion.BackgroundImage = null;
      this.txtSelectedRegion.Font = null;
      this.txtSelectedRegion.Name = "txtSelectedRegion";
      this.txtSelectedRegion.ReadOnly = true;
      this.txtSelectedRegion.TabStop = false;
      // 
      // lblSelectedRegion
      // 
      this.lblSelectedRegion.AccessibleDescription = null;
      this.lblSelectedRegion.AccessibleName = null;
      resources.ApplyResources(this.lblSelectedRegion, "lblSelectedRegion");
      this.lblSelectedRegion.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblSelectedRegion.Font = null;
      this.lblSelectedRegion.Name = "lblSelectedRegion";
      // 
      // lblToolLanguage
      // 
      this.lblToolLanguage.AccessibleDescription = null;
      this.lblToolLanguage.AccessibleName = null;
      resources.ApplyResources(this.lblToolLanguage, "lblToolLanguage");
      this.lblToolLanguage.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblToolLanguage.Font = null;
      this.lblToolLanguage.Name = "lblToolLanguage";
      // 
      // btnAudioManager
      // 
      this.btnAudioManager.AccessibleDescription = null;
      this.btnAudioManager.AccessibleName = null;
      resources.ApplyResources(this.btnAudioManager, "btnAudioManager");
      this.btnAudioManager.BackgroundImage = null;
      this.btnAudioManager.Font = null;
      this.btnAudioManager.Name = "btnAudioManager";
      this.btnAudioManager.Click += new System.EventHandler(this.btnAudioManager_Click);
      // 
      // btnFFXIMacroManager
      // 
      this.btnFFXIMacroManager.AccessibleDescription = null;
      this.btnFFXIMacroManager.AccessibleName = null;
      resources.ApplyResources(this.btnFFXIMacroManager, "btnFFXIMacroManager");
      this.btnFFXIMacroManager.BackgroundImage = null;
      this.btnFFXIMacroManager.Font = null;
      this.btnFFXIMacroManager.Name = "btnFFXIMacroManager";
      this.btnFFXIMacroManager.Click += new System.EventHandler(this.btnFFXIMacroManager_Click);
      // 
      // btnFFXIDataBrowser
      // 
      this.btnFFXIDataBrowser.AccessibleDescription = null;
      this.btnFFXIDataBrowser.AccessibleName = null;
      resources.ApplyResources(this.btnFFXIDataBrowser, "btnFFXIDataBrowser");
      this.btnFFXIDataBrowser.BackgroundImage = null;
      this.btnFFXIDataBrowser.Font = null;
      this.btnFFXIDataBrowser.Name = "btnFFXIDataBrowser";
      this.btnFFXIDataBrowser.Click += new System.EventHandler(this.btnFFXIDataBrowser_Click);
      // 
      // btnFFXIConfigEditor
      // 
      this.btnFFXIConfigEditor.AccessibleDescription = null;
      this.btnFFXIConfigEditor.AccessibleName = null;
      resources.ApplyResources(this.btnFFXIConfigEditor, "btnFFXIConfigEditor");
      this.btnFFXIConfigEditor.BackgroundImage = null;
      this.btnFFXIConfigEditor.Font = null;
      this.btnFFXIConfigEditor.Name = "btnFFXIConfigEditor";
      this.btnFFXIConfigEditor.Click += new System.EventHandler(this.btnFFXIConfigEditor_Click);
      // 
      // btnFFXIItemComparison
      // 
      this.btnFFXIItemComparison.AccessibleDescription = null;
      this.btnFFXIItemComparison.AccessibleName = null;
      resources.ApplyResources(this.btnFFXIItemComparison, "btnFFXIItemComparison");
      this.btnFFXIItemComparison.BackgroundImage = null;
      this.btnFFXIItemComparison.Font = null;
      this.btnFFXIItemComparison.Name = "btnFFXIItemComparison";
      this.btnFFXIItemComparison.Click += new System.EventHandler(this.btnFFXIItemComparison_Click);
      // 
      // btnFFXIEngrishOnry
      // 
      this.btnFFXIEngrishOnry.AccessibleDescription = null;
      this.btnFFXIEngrishOnry.AccessibleName = null;
      resources.ApplyResources(this.btnFFXIEngrishOnry, "btnFFXIEngrishOnry");
      this.btnFFXIEngrishOnry.BackgroundImage = null;
      this.btnFFXIEngrishOnry.Font = null;
      this.btnFFXIEngrishOnry.Name = "btnFFXIEngrishOnry";
      this.btnFFXIEngrishOnry.Click += new System.EventHandler(this.btnFFXIEngrishOnry_Click);
      // 
      // POLUtilsUI
      // 
      this.AccessibleDescription = null;
      this.AccessibleName = null;
      resources.ApplyResources(this, "$this");
      this.BackgroundImage = null;
      this.Controls.Add(this.btnFFXIEngrishOnry);
      this.Controls.Add(this.btnFFXIItemComparison);
      this.Controls.Add(this.btnFFXIConfigEditor);
      this.Controls.Add(this.btnFFXIDataBrowser);
      this.Controls.Add(this.btnFFXIMacroManager);
      this.Controls.Add(this.btnAudioManager);
      this.Controls.Add(this.grpRegion);
      this.Controls.Add(this.btnTetraViewer);
      this.Font = null;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Icon = null;
      this.MaximizeBox = false;
      this.Name = "POLUtilsUI";
      this.grpRegion.ResumeLayout(false);
      this.grpRegion.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    // Controls
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
    private System.Windows.Forms.Button btnFFXIEngrishOnry;

  }

}
