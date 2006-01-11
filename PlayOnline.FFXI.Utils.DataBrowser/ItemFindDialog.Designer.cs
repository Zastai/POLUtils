namespace PlayOnline.FFXI.Utils.DataBrowser {

  internal partial class ItemFindDialog {

    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing) {
      if (disposing && this.components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent() {
      this.components = new System.ComponentModel.Container();
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemFindDialog));
      this.lstItems = new System.Windows.Forms.ListView();
      this.mnuItemListContext = new System.Windows.Forms.ContextMenu();
      this.mnuILCProperties = new System.Windows.Forms.MenuItem();
      this.mnuILCCopy = new System.Windows.Forms.MenuItem();
      this.mnuILCExport = new System.Windows.Forms.MenuItem();
      this.mnuILCEAll = new System.Windows.Forms.MenuItem();
      this.mnuILCEResults = new System.Windows.Forms.MenuItem();
      this.mnuILCESelected = new System.Windows.Forms.MenuItem();
      this.ilItemIcons = new System.Windows.Forms.ImageList(this.components);
      this.pnlSearchOptions = new System.Windows.Forms.Panel();
      this.chkShowIcons = new System.Windows.Forms.CheckBox();
      this.btnClose = new System.Windows.Forms.Button();
      this.btnRunQuery = new System.Windows.Forms.Button();
      this.stbStatus = new System.Windows.Forms.StatusBar();
      this.dlgExportFile = new System.Windows.Forms.SaveFileDialog();
      this.pnlSearchOptions.SuspendLayout();
      this.SuspendLayout();
      // 
      // lstItems
      // 
      this.lstItems.AllowColumnReorder = true;
      this.lstItems.ContextMenu = this.mnuItemListContext;
      resources.ApplyResources(this.lstItems, "lstItems");
      this.lstItems.FullRowSelect = true;
      this.lstItems.GridLines = true;
      this.lstItems.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
      this.lstItems.HideSelection = false;
      this.lstItems.Name = "lstItems";
      this.lstItems.UseCompatibleStateImageBehavior = false;
      this.lstItems.View = System.Windows.Forms.View.Details;
      this.lstItems.DoubleClick += new System.EventHandler(this.lstItems_DoubleClick);
      this.lstItems.SelectedIndexChanged += new System.EventHandler(this.lstItems_SelectedIndexChanged);
      // 
      // mnuItemListContext
      // 
      this.mnuItemListContext.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuILCProperties,
            this.mnuILCCopy,
            this.mnuILCExport});
      // 
      // mnuILCProperties
      // 
      this.mnuILCProperties.Index = 0;
      resources.ApplyResources(this.mnuILCProperties, "mnuILCProperties");
      this.mnuILCProperties.Click += new System.EventHandler(this.mnuILCProperties_Click);
      // 
      // mnuILCCopy
      // 
      this.mnuILCCopy.Index = 1;
      resources.ApplyResources(this.mnuILCCopy, "mnuILCCopy");
      // 
      // mnuILCExport
      // 
      this.mnuILCExport.Index = 2;
      this.mnuILCExport.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuILCEAll,
            this.mnuILCEResults,
            this.mnuILCESelected});
      resources.ApplyResources(this.mnuILCExport, "mnuILCExport");
      // 
      // mnuILCEAll
      // 
      this.mnuILCEAll.Index = 0;
      resources.ApplyResources(this.mnuILCEAll, "mnuILCEAll");
      this.mnuILCEAll.Click += new System.EventHandler(this.mnuILCECAll_Click);
      // 
      // mnuILCEResults
      // 
      resources.ApplyResources(this.mnuILCEResults, "mnuILCEResults");
      this.mnuILCEResults.Index = 1;
      // 
      // mnuILCESelected
      // 
      resources.ApplyResources(this.mnuILCESelected, "mnuILCESelected");
      this.mnuILCESelected.Index = 2;
      // 
      // ilItemIcons
      // 
      this.ilItemIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
      resources.ApplyResources(this.ilItemIcons, "ilItemIcons");
      this.ilItemIcons.TransparentColor = System.Drawing.Color.Transparent;
      // 
      // pnlSearchOptions
      // 
      this.pnlSearchOptions.Controls.Add(this.chkShowIcons);
      this.pnlSearchOptions.Controls.Add(this.btnClose);
      this.pnlSearchOptions.Controls.Add(this.btnRunQuery);
      resources.ApplyResources(this.pnlSearchOptions, "pnlSearchOptions");
      this.pnlSearchOptions.Name = "pnlSearchOptions";
      // 
      // chkShowIcons
      // 
      resources.ApplyResources(this.chkShowIcons, "chkShowIcons");
      this.chkShowIcons.Name = "chkShowIcons";
      this.chkShowIcons.CheckedChanged += new System.EventHandler(this.chkShowIcons_CheckedChanged);
      // 
      // btnClose
      // 
      resources.ApplyResources(this.btnClose, "btnClose");
      this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnClose.Name = "btnClose";
      this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
      // 
      // btnRunQuery
      // 
      resources.ApplyResources(this.btnRunQuery, "btnRunQuery");
      this.btnRunQuery.Name = "btnRunQuery";
      this.btnRunQuery.Click += new System.EventHandler(this.btnRunQuery_Click);
      // 
      // stbStatus
      // 
      resources.ApplyResources(this.stbStatus, "stbStatus");
      this.stbStatus.Name = "stbStatus";
      // 
      // dlgExportFile
      // 
      this.dlgExportFile.DefaultExt = "xml";
      resources.ApplyResources(this.dlgExportFile, "dlgExportFile");
      // 
      // ItemFindDialog
      // 
      this.AcceptButton = this.btnRunQuery;
      this.CancelButton = this.btnClose;
      resources.ApplyResources(this, "$this");
      this.Controls.Add(this.stbStatus);
      this.Controls.Add(this.lstItems);
      this.Controls.Add(this.pnlSearchOptions);
      this.Name = "ItemFindDialog";
      this.ShowInTaskbar = false;
      this.pnlSearchOptions.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListView lstItems;
    private System.Windows.Forms.ImageList ilItemIcons;
    private System.Windows.Forms.Panel pnlSearchOptions;
    private System.Windows.Forms.Button btnRunQuery;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.StatusBar stbStatus;
    private System.Windows.Forms.CheckBox chkShowIcons;
    private System.Windows.Forms.SaveFileDialog dlgExportFile;
    private System.Windows.Forms.ContextMenu mnuItemListContext;
    private System.Windows.Forms.MenuItem mnuILCProperties;
    private System.Windows.Forms.MenuItem mnuILCCopy;
    private System.Windows.Forms.MenuItem mnuILCExport;
    private System.Windows.Forms.MenuItem mnuILCEAll;
    private System.Windows.Forms.MenuItem mnuILCEResults;
    private System.Windows.Forms.MenuItem mnuILCESelected;
 
  }

}
