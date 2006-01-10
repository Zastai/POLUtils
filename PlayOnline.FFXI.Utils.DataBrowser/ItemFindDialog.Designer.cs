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
      this.cmsItemList = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.cmiILCopy = new System.Windows.Forms.ToolStripMenuItem();
      this.cmiILExport = new System.Windows.Forms.ToolStripMenuItem();
      this.cmiILEAll = new System.Windows.Forms.ToolStripMenuItem();
      this.cmiILEResults = new System.Windows.Forms.ToolStripMenuItem();
      this.cmiILESelected = new System.Windows.Forms.ToolStripMenuItem();
      this.ilItemIcons = new System.Windows.Forms.ImageList(this.components);
      this.pnlSearchOptions = new System.Windows.Forms.Panel();
      this.chkShowIcons = new System.Windows.Forms.CheckBox();
      this.btnClose = new System.Windows.Forms.Button();
      this.btnRunQuery = new System.Windows.Forms.Button();
      this.stbStatus = new System.Windows.Forms.StatusBar();
      this.dlgExportFile = new System.Windows.Forms.SaveFileDialog();
      this.cmiILProperties = new System.Windows.Forms.ToolStripMenuItem();
      this.cmsItemList.SuspendLayout();
      this.pnlSearchOptions.SuspendLayout();
      this.SuspendLayout();
      // 
      // lstItems
      // 
      this.lstItems.AllowColumnReorder = true;
      this.lstItems.ContextMenuStrip = this.cmsItemList;
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
      // cmsItemList
      // 
      this.cmsItemList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiILProperties,
            this.cmiILCopy,
            this.cmiILExport});
      this.cmsItemList.Name = "cmsItemList";
      this.cmsItemList.ShowItemToolTips = false;
      resources.ApplyResources(this.cmsItemList, "cmsItemList");
      // 
      // cmiILCopy
      // 
      this.cmiILCopy.Name = "cmiILCopy";
      resources.ApplyResources(this.cmiILCopy, "cmiILCopy");
      // 
      // cmiILExport
      // 
      this.cmiILExport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmiILEAll,
            this.cmiILEResults,
            this.cmiILESelected});
      this.cmiILExport.Name = "cmiILExport";
      resources.ApplyResources(this.cmiILExport, "cmiILExport");
      // 
      // cmiILEAll
      // 
      this.cmiILEAll.Name = "cmiILEAll";
      resources.ApplyResources(this.cmiILEAll, "cmiILEAll");
      this.cmiILEAll.Click += new System.EventHandler(this.cmiILEAll_Click);
      // 
      // cmiILEResults
      // 
      resources.ApplyResources(this.cmiILEResults, "cmiILEResults");
      this.cmiILEResults.Name = "cmiILEResults";
      this.cmiILEResults.Click += new System.EventHandler(this.cmiILEResults_Click);
      // 
      // cmiILESelected
      // 
      resources.ApplyResources(this.cmiILESelected, "cmiILESelected");
      this.cmiILESelected.Name = "cmiILESelected";
      this.cmiILESelected.Click += new System.EventHandler(this.cmiILESelected_Click);
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
      // cmiILProperties
      // 
      this.cmiILProperties.Name = "cmiILProperties";
      resources.ApplyResources(this.cmiILProperties, "cmiILProperties");
      this.cmiILProperties.Click += new System.EventHandler(this.cmiILProperties_Click);
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
      this.cmsItemList.ResumeLayout(false);
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
    private System.Windows.Forms.ContextMenuStrip cmsItemList;
    private System.Windows.Forms.SaveFileDialog dlgExportFile;
    private System.Windows.Forms.ToolStripMenuItem cmiILCopy;
    private System.Windows.Forms.ToolStripMenuItem cmiILExport;
    private System.Windows.Forms.ToolStripMenuItem cmiILEAll;
    private System.Windows.Forms.ToolStripMenuItem cmiILEResults;
    private System.Windows.Forms.ToolStripMenuItem cmiILESelected;
    private System.Windows.Forms.ToolStripMenuItem cmiILProperties;

  }

}
