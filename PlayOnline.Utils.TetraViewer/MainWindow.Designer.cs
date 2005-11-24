namespace PlayOnline.Utils.TetraViewer {

  public partial class MainWindow {

    #region Controls

    private System.Windows.Forms.FolderBrowserDialog dlgBrowseFolder;
    private System.Windows.Forms.TreeView tvDataFiles;
    private System.Windows.Forms.StatusBar sbrStatus;
    private System.Windows.Forms.ContextMenu mnuTreeContext;
    private System.Windows.Forms.ContextMenu mnuPictureContext;
    private System.Windows.Forms.MenuItem mnuStretchImage;
    private System.Windows.Forms.MenuItem mnuExportAll;
    private System.Windows.Forms.MenuItem mnuExport;
    private System.Windows.Forms.PictureBox picViewer;
    private System.Windows.Forms.MenuItem mnuNormalImage;
    private System.Windows.Forms.MenuItem mnuTiledImage;

    private System.ComponentModel.Container components = null;

    #endregion

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
      this.dlgBrowseFolder = new System.Windows.Forms.FolderBrowserDialog();
      this.tvDataFiles = new System.Windows.Forms.TreeView();
      this.mnuTreeContext = new System.Windows.Forms.ContextMenu();
      this.mnuExportAll = new System.Windows.Forms.MenuItem();
      this.mnuExport = new System.Windows.Forms.MenuItem();
      this.sbrStatus = new System.Windows.Forms.StatusBar();
      this.picViewer = new System.Windows.Forms.PictureBox();
      this.mnuPictureContext = new System.Windows.Forms.ContextMenu();
      this.mnuNormalImage = new System.Windows.Forms.MenuItem();
      this.mnuStretchImage = new System.Windows.Forms.MenuItem();
      this.mnuTiledImage = new System.Windows.Forms.MenuItem();
      ((System.ComponentModel.ISupportInitialize) (this.picViewer)).BeginInit();
      this.SuspendLayout();
      // 
      // dlgBrowseFolder
      // 
      resources.ApplyResources(this.dlgBrowseFolder, "dlgBrowseFolder");
      // 
      // tvDataFiles
      // 
      this.tvDataFiles.ContextMenu = this.mnuTreeContext;
      resources.ApplyResources(this.tvDataFiles, "tvDataFiles");
      this.tvDataFiles.HideSelection = false;
      this.tvDataFiles.Name = "tvDataFiles";
      this.tvDataFiles.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvDataFiles_AfterSelect);
      // 
      // mnuTreeContext
      // 
      this.mnuTreeContext.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuExportAll,
            this.mnuExport});
      // 
      // mnuExportAll
      // 
      this.mnuExportAll.Index = 0;
      resources.ApplyResources(this.mnuExportAll, "mnuExportAll");
      // 
      // mnuExport
      // 
      this.mnuExport.Index = 1;
      resources.ApplyResources(this.mnuExport, "mnuExport");
      // 
      // sbrStatus
      // 
      resources.ApplyResources(this.sbrStatus, "sbrStatus");
      this.sbrStatus.Name = "sbrStatus";
      // 
      // picViewer
      // 
      this.picViewer.ContextMenu = this.mnuPictureContext;
      resources.ApplyResources(this.picViewer, "picViewer");
      this.picViewer.Name = "picViewer";
      this.picViewer.TabStop = false;
      // 
      // mnuPictureContext
      // 
      this.mnuPictureContext.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuNormalImage,
            this.mnuStretchImage,
            this.mnuTiledImage});
      // 
      // mnuNormalImage
      // 
      this.mnuNormalImage.Checked = true;
      this.mnuNormalImage.Index = 0;
      this.mnuNormalImage.RadioCheck = true;
      resources.ApplyResources(this.mnuNormalImage, "mnuNormalImage");
      this.mnuNormalImage.Click += new System.EventHandler(this.ImageOption_Click);
      // 
      // mnuStretchImage
      // 
      this.mnuStretchImage.Index = 1;
      this.mnuStretchImage.RadioCheck = true;
      resources.ApplyResources(this.mnuStretchImage, "mnuStretchImage");
      this.mnuStretchImage.Click += new System.EventHandler(this.ImageOption_Click);
      // 
      // mnuTiledImage
      // 
      resources.ApplyResources(this.mnuTiledImage, "mnuTiledImage");
      this.mnuTiledImage.Index = 2;
      this.mnuTiledImage.RadioCheck = true;
      this.mnuTiledImage.Click += new System.EventHandler(this.ImageOption_Click);
      // 
      // MainWindow
      // 
      resources.ApplyResources(this, "$this");
      this.Controls.Add(this.picViewer);
      this.Controls.Add(this.tvDataFiles);
      this.Controls.Add(this.sbrStatus);
      this.Name = "MainWindow";
      ((System.ComponentModel.ISupportInitialize) (this.picViewer)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

  }

}
