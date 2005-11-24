namespace PlayOnline.FFXI.Utils.DataBrowser {

  public partial class MainWindow {

    #region Controls

    private System.Windows.Forms.TreeView tvDataFiles;
    private System.Windows.Forms.ImageList ilBrowserIcons;
    private System.Windows.Forms.Splitter splSplitter;
    private System.Windows.Forms.ContextMenu mnuPictureContext;
    private System.Windows.Forms.MenuItem mnuPCBackgroundBlack;
    private System.Windows.Forms.MenuItem mnuPCBackgroundWhite;
    private System.Windows.Forms.MenuItem mnuPCBackgroundTransparent;
    private System.Windows.Forms.SaveFileDialog dlgSavePicture;
    private System.Windows.Forms.MainMenu mnuMain;
    private System.Windows.Forms.MenuItem mnuPCMode;
    private System.Windows.Forms.MenuItem mnuPCModeNormal;
    private System.Windows.Forms.MenuItem mnuPCModeCentered;
    private System.Windows.Forms.MenuItem mnuPCModeStretched;
    private System.Windows.Forms.MenuItem mnuPCBackground;
    private System.Windows.Forms.MenuItem mnuPCSaveAs;
    private System.Windows.Forms.ContextMenu mnuStringTableContext;
    private System.Windows.Forms.Panel pnlViewerArea;
    private System.Windows.Forms.MenuItem mnuWindows;
    private System.Windows.Forms.TabControl tabViewers;
    private System.Windows.Forms.ListView lstEntries;
    private System.Windows.Forms.TabPage tabViewerImages;
    private System.Windows.Forms.PictureBox picImageViewer;
    private System.Windows.Forms.Panel pnlImageChooser;
    private System.Windows.Forms.Label lblImageChooser;
    private System.Windows.Forms.ComboBox cmbImageChooser;
    private System.Windows.Forms.Panel pnlNoViewers;
    private System.Windows.Forms.Label lblNoViewers;
    private System.Windows.Forms.TabPage tabViewerItems;
    private System.Windows.Forms.TabPage tabViewerStringTable;
    private System.Windows.Forms.Button btnExportItems;
    private System.Windows.Forms.Button btnFindItems;
    private System.Windows.Forms.GroupBox grpMainItemActions;
    private System.Windows.Forms.ComboBox cmbItems;
    private System.Windows.Forms.MenuItem mnuOFileTable;
    private System.Windows.Forms.MenuItem mnuOSettings;
    private PlayOnline.FFXI.FFXIItemEditor ieItemViewer;
    private System.Windows.Forms.Button btnImageSaveAll;
    private System.Windows.Forms.MenuItem mnuSTCCopyRow;
    private System.Windows.Forms.MenuItem mnuSTCCopyField;

    private System.ComponentModel.IContainer components;

    #endregion

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
      this.tvDataFiles = new System.Windows.Forms.TreeView();
      this.ilBrowserIcons = new System.Windows.Forms.ImageList(this.components);
      this.splSplitter = new System.Windows.Forms.Splitter();
      this.mnuPictureContext = new System.Windows.Forms.ContextMenu();
      this.mnuPCMode = new System.Windows.Forms.MenuItem();
      this.mnuPCModeNormal = new System.Windows.Forms.MenuItem();
      this.mnuPCModeCentered = new System.Windows.Forms.MenuItem();
      this.mnuPCModeStretched = new System.Windows.Forms.MenuItem();
      this.mnuPCBackground = new System.Windows.Forms.MenuItem();
      this.mnuPCBackgroundBlack = new System.Windows.Forms.MenuItem();
      this.mnuPCBackgroundWhite = new System.Windows.Forms.MenuItem();
      this.mnuPCBackgroundTransparent = new System.Windows.Forms.MenuItem();
      this.mnuPCSaveAs = new System.Windows.Forms.MenuItem();
      this.dlgSavePicture = new System.Windows.Forms.SaveFileDialog();
      this.mnuStringTableContext = new System.Windows.Forms.ContextMenu();
      this.mnuSTCCopyRow = new System.Windows.Forms.MenuItem();
      this.mnuSTCCopyField = new System.Windows.Forms.MenuItem();
      this.mnuMain = new System.Windows.Forms.MainMenu(this.components);
      this.mnuWindows = new System.Windows.Forms.MenuItem();
      this.mnuOFileTable = new System.Windows.Forms.MenuItem();
      this.mnuOSettings = new System.Windows.Forms.MenuItem();
      this.pnlViewerArea = new System.Windows.Forms.Panel();
      this.tabViewers = new System.Windows.Forms.TabControl();
      this.tabViewerItems = new System.Windows.Forms.TabPage();
      this.ieItemViewer = new PlayOnline.FFXI.FFXIItemEditor();
      this.grpMainItemActions = new System.Windows.Forms.GroupBox();
      this.cmbItems = new System.Windows.Forms.ComboBox();
      this.btnFindItems = new System.Windows.Forms.Button();
      this.btnExportItems = new System.Windows.Forms.Button();
      this.tabViewerImages = new System.Windows.Forms.TabPage();
      this.picImageViewer = new System.Windows.Forms.PictureBox();
      this.pnlImageChooser = new System.Windows.Forms.Panel();
      this.btnImageSaveAll = new System.Windows.Forms.Button();
      this.cmbImageChooser = new System.Windows.Forms.ComboBox();
      this.lblImageChooser = new System.Windows.Forms.Label();
      this.tabViewerStringTable = new System.Windows.Forms.TabPage();
      this.lstEntries = new System.Windows.Forms.ListView();
      this.pnlNoViewers = new System.Windows.Forms.Panel();
      this.lblNoViewers = new System.Windows.Forms.Label();
      this.pnlViewerArea.SuspendLayout();
      this.tabViewers.SuspendLayout();
      this.tabViewerItems.SuspendLayout();
      this.grpMainItemActions.SuspendLayout();
      this.tabViewerImages.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize) (this.picImageViewer)).BeginInit();
      this.pnlImageChooser.SuspendLayout();
      this.tabViewerStringTable.SuspendLayout();
      this.pnlNoViewers.SuspendLayout();
      this.SuspendLayout();
      // 
      // tvDataFiles
      // 
      resources.ApplyResources(this.tvDataFiles, "tvDataFiles");
      this.tvDataFiles.HideSelection = false;
      this.tvDataFiles.ImageList = this.ilBrowserIcons;
      this.tvDataFiles.Name = "tvDataFiles";
      this.tvDataFiles.PathSeparator = "/";
      this.tvDataFiles.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvDataFiles_AfterExpand);
      this.tvDataFiles.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvDataFiles_BeforeExpand);
      this.tvDataFiles.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvDataFiles_AfterSelect);
      this.tvDataFiles.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tvDataFiles_AfterCollapse);
      // 
      // ilBrowserIcons
      // 
      this.ilBrowserIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
      resources.ApplyResources(this.ilBrowserIcons, "ilBrowserIcons");
      this.ilBrowserIcons.TransparentColor = System.Drawing.Color.Transparent;
      // 
      // splSplitter
      // 
      resources.ApplyResources(this.splSplitter, "splSplitter");
      this.splSplitter.Name = "splSplitter";
      this.splSplitter.TabStop = false;
      // 
      // mnuPictureContext
      // 
      this.mnuPictureContext.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuPCMode,
            this.mnuPCBackground,
            this.mnuPCSaveAs});
      // 
      // mnuPCMode
      // 
      this.mnuPCMode.Index = 0;
      this.mnuPCMode.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuPCModeNormal,
            this.mnuPCModeCentered,
            this.mnuPCModeStretched});
      resources.ApplyResources(this.mnuPCMode, "mnuPCMode");
      // 
      // mnuPCModeNormal
      // 
      this.mnuPCModeNormal.Checked = true;
      this.mnuPCModeNormal.Index = 0;
      this.mnuPCModeNormal.RadioCheck = true;
      resources.ApplyResources(this.mnuPCModeNormal, "mnuPCModeNormal");
      this.mnuPCModeNormal.Click += new System.EventHandler(this.mnuPCModeNormal_Click);
      // 
      // mnuPCModeCentered
      // 
      this.mnuPCModeCentered.Index = 1;
      this.mnuPCModeCentered.RadioCheck = true;
      resources.ApplyResources(this.mnuPCModeCentered, "mnuPCModeCentered");
      this.mnuPCModeCentered.Click += new System.EventHandler(this.mnuPCModeCentered_Click);
      // 
      // mnuPCModeStretched
      // 
      this.mnuPCModeStretched.Index = 2;
      this.mnuPCModeStretched.RadioCheck = true;
      resources.ApplyResources(this.mnuPCModeStretched, "mnuPCModeStretched");
      this.mnuPCModeStretched.Click += new System.EventHandler(this.mnuPCModeStretched_Click);
      // 
      // mnuPCBackground
      // 
      this.mnuPCBackground.Index = 1;
      this.mnuPCBackground.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuPCBackgroundBlack,
            this.mnuPCBackgroundWhite,
            this.mnuPCBackgroundTransparent});
      resources.ApplyResources(this.mnuPCBackground, "mnuPCBackground");
      // 
      // mnuPCBackgroundBlack
      // 
      this.mnuPCBackgroundBlack.Index = 0;
      this.mnuPCBackgroundBlack.RadioCheck = true;
      resources.ApplyResources(this.mnuPCBackgroundBlack, "mnuPCBackgroundBlack");
      this.mnuPCBackgroundBlack.Click += new System.EventHandler(this.mnuPCBackgroundBlack_Click);
      // 
      // mnuPCBackgroundWhite
      // 
      this.mnuPCBackgroundWhite.Index = 1;
      this.mnuPCBackgroundWhite.RadioCheck = true;
      resources.ApplyResources(this.mnuPCBackgroundWhite, "mnuPCBackgroundWhite");
      this.mnuPCBackgroundWhite.Click += new System.EventHandler(this.mnuPCBackgroundWhite_Click);
      // 
      // mnuPCBackgroundTransparent
      // 
      this.mnuPCBackgroundTransparent.Checked = true;
      this.mnuPCBackgroundTransparent.Index = 2;
      this.mnuPCBackgroundTransparent.RadioCheck = true;
      resources.ApplyResources(this.mnuPCBackgroundTransparent, "mnuPCBackgroundTransparent");
      this.mnuPCBackgroundTransparent.Click += new System.EventHandler(this.mnuPCBackgroundTransparent_Click);
      // 
      // mnuPCSaveAs
      // 
      this.mnuPCSaveAs.Index = 2;
      resources.ApplyResources(this.mnuPCSaveAs, "mnuPCSaveAs");
      this.mnuPCSaveAs.Click += new System.EventHandler(this.mnuPCSaveAs_Click);
      // 
      // dlgSavePicture
      // 
      resources.ApplyResources(this.dlgSavePicture, "dlgSavePicture");
      // 
      // mnuStringTableContext
      // 
      this.mnuStringTableContext.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuSTCCopyRow,
            this.mnuSTCCopyField});
      this.mnuStringTableContext.Popup += new System.EventHandler(this.mnuStringTableContext_Popup);
      // 
      // mnuSTCCopyRow
      // 
      this.mnuSTCCopyRow.Index = 0;
      resources.ApplyResources(this.mnuSTCCopyRow, "mnuSTCCopyRow");
      this.mnuSTCCopyRow.Click += new System.EventHandler(this.mnuSTCCopyRow_Click);
      // 
      // mnuSTCCopyField
      // 
      this.mnuSTCCopyField.Index = 1;
      resources.ApplyResources(this.mnuSTCCopyField, "mnuSTCCopyField");
      // 
      // mnuMain
      // 
      this.mnuMain.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuWindows});
      // 
      // mnuWindows
      // 
      this.mnuWindows.Index = 0;
      this.mnuWindows.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuOFileTable,
            this.mnuOSettings});
      resources.ApplyResources(this.mnuWindows, "mnuWindows");
      // 
      // mnuOFileTable
      // 
      this.mnuOFileTable.Index = 0;
      resources.ApplyResources(this.mnuOFileTable, "mnuOFileTable");
      this.mnuOFileTable.Click += new System.EventHandler(this.mnuOFileTable_Click);
      // 
      // mnuOSettings
      // 
      this.mnuOSettings.Index = 1;
      resources.ApplyResources(this.mnuOSettings, "mnuOSettings");
      this.mnuOSettings.Click += new System.EventHandler(this.mnuOSettings_Click);
      // 
      // pnlViewerArea
      // 
      this.pnlViewerArea.Controls.Add(this.tabViewers);
      this.pnlViewerArea.Controls.Add(this.pnlNoViewers);
      resources.ApplyResources(this.pnlViewerArea, "pnlViewerArea");
      this.pnlViewerArea.Name = "pnlViewerArea";
      // 
      // tabViewers
      // 
      this.tabViewers.Controls.Add(this.tabViewerItems);
      this.tabViewers.Controls.Add(this.tabViewerImages);
      this.tabViewers.Controls.Add(this.tabViewerStringTable);
      resources.ApplyResources(this.tabViewers, "tabViewers");
      this.tabViewers.Name = "tabViewers";
      this.tabViewers.SelectedIndex = 0;
      // 
      // tabViewerItems
      // 
      this.tabViewerItems.Controls.Add(this.ieItemViewer);
      this.tabViewerItems.Controls.Add(this.grpMainItemActions);
      resources.ApplyResources(this.tabViewerItems, "tabViewerItems");
      this.tabViewerItems.Name = "tabViewerItems";
      // 
      // ieItemViewer
      // 
      this.ieItemViewer.BackColor = System.Drawing.SystemColors.Control;
      this.ieItemViewer.Item = null;
      resources.ApplyResources(this.ieItemViewer, "ieItemViewer");
      this.ieItemViewer.Name = "ieItemViewer";
      this.ieItemViewer.SizeChanged += new System.EventHandler(this.ieItemViewer_SizeChanged);
      // 
      // grpMainItemActions
      // 
      this.grpMainItemActions.Controls.Add(this.cmbItems);
      this.grpMainItemActions.Controls.Add(this.btnFindItems);
      this.grpMainItemActions.Controls.Add(this.btnExportItems);
      this.grpMainItemActions.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.grpMainItemActions, "grpMainItemActions");
      this.grpMainItemActions.Name = "grpMainItemActions";
      this.grpMainItemActions.TabStop = false;
      // 
      // cmbItems
      // 
      resources.ApplyResources(this.cmbItems, "cmbItems");
      this.cmbItems.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbItems.FormattingEnabled = true;
      this.cmbItems.Name = "cmbItems";
      this.cmbItems.SelectedIndexChanged += new System.EventHandler(this.cmbItems_SelectedIndexChanged);
      // 
      // btnFindItems
      // 
      resources.ApplyResources(this.btnFindItems, "btnFindItems");
      this.btnFindItems.Name = "btnFindItems";
      this.btnFindItems.Click += new System.EventHandler(this.btnFindItems_Click);
      // 
      // btnExportItems
      // 
      resources.ApplyResources(this.btnExportItems, "btnExportItems");
      this.btnExportItems.Name = "btnExportItems";
      this.btnExportItems.Click += new System.EventHandler(this.btnExportItems_Click);
      // 
      // tabViewerImages
      // 
      this.tabViewerImages.Controls.Add(this.picImageViewer);
      this.tabViewerImages.Controls.Add(this.pnlImageChooser);
      resources.ApplyResources(this.tabViewerImages, "tabViewerImages");
      this.tabViewerImages.Name = "tabViewerImages";
      // 
      // picImageViewer
      // 
      this.picImageViewer.BackColor = System.Drawing.Color.Transparent;
      this.picImageViewer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.picImageViewer.ContextMenu = this.mnuPictureContext;
      resources.ApplyResources(this.picImageViewer, "picImageViewer");
      this.picImageViewer.Name = "picImageViewer";
      this.picImageViewer.TabStop = false;
      // 
      // pnlImageChooser
      // 
      this.pnlImageChooser.Controls.Add(this.btnImageSaveAll);
      this.pnlImageChooser.Controls.Add(this.cmbImageChooser);
      this.pnlImageChooser.Controls.Add(this.lblImageChooser);
      resources.ApplyResources(this.pnlImageChooser, "pnlImageChooser");
      this.pnlImageChooser.Name = "pnlImageChooser";
      // 
      // btnImageSaveAll
      // 
      resources.ApplyResources(this.btnImageSaveAll, "btnImageSaveAll");
      this.btnImageSaveAll.Name = "btnImageSaveAll";
      this.btnImageSaveAll.Click += new System.EventHandler(this.btnImageSaveAll_Click);
      // 
      // cmbImageChooser
      // 
      resources.ApplyResources(this.cmbImageChooser, "cmbImageChooser");
      this.cmbImageChooser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbImageChooser.FormattingEnabled = true;
      this.cmbImageChooser.Name = "cmbImageChooser";
      this.cmbImageChooser.SelectedIndexChanged += new System.EventHandler(this.cmbImageChooser_SelectedIndexChanged);
      // 
      // lblImageChooser
      // 
      this.lblImageChooser.BackColor = System.Drawing.Color.Transparent;
      resources.ApplyResources(this.lblImageChooser, "lblImageChooser");
      this.lblImageChooser.Name = "lblImageChooser";
      // 
      // tabViewerStringTable
      // 
      this.tabViewerStringTable.Controls.Add(this.lstEntries);
      resources.ApplyResources(this.tabViewerStringTable, "tabViewerStringTable");
      this.tabViewerStringTable.Name = "tabViewerStringTable";
      // 
      // lstEntries
      // 
      this.lstEntries.AllowColumnReorder = true;
      this.lstEntries.ContextMenu = this.mnuStringTableContext;
      resources.ApplyResources(this.lstEntries, "lstEntries");
      this.lstEntries.FullRowSelect = true;
      this.lstEntries.GridLines = true;
      this.lstEntries.Name = "lstEntries";
      this.lstEntries.View = System.Windows.Forms.View.Details;
      this.lstEntries.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstEntries_KeyDown);
      // 
      // pnlNoViewers
      // 
      this.pnlNoViewers.Controls.Add(this.lblNoViewers);
      resources.ApplyResources(this.pnlNoViewers, "pnlNoViewers");
      this.pnlNoViewers.Name = "pnlNoViewers";
      // 
      // lblNoViewers
      // 
      resources.ApplyResources(this.lblNoViewers, "lblNoViewers");
      this.lblNoViewers.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblNoViewers.Name = "lblNoViewers";
      // 
      // MainWindow
      // 
      resources.ApplyResources(this, "$this");
      this.Controls.Add(this.pnlViewerArea);
      this.Controls.Add(this.splSplitter);
      this.Controls.Add(this.tvDataFiles);
      this.Menu = this.mnuMain;
      this.Name = "MainWindow";
      this.pnlViewerArea.ResumeLayout(false);
      this.tabViewers.ResumeLayout(false);
      this.tabViewerItems.ResumeLayout(false);
      this.grpMainItemActions.ResumeLayout(false);
      this.tabViewerImages.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize) (this.picImageViewer)).EndInit();
      this.pnlImageChooser.ResumeLayout(false);
      this.tabViewerStringTable.ResumeLayout(false);
      this.pnlNoViewers.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

  }

}
