namespace PlayOnline.FFXI.Utils.DataBrowser {

  internal partial class ItemFindDialog {

    #region Controls

    private System.Windows.Forms.ListView lstItems;
    private System.Windows.Forms.ImageList ilItemIcons;
    private System.Windows.Forms.Panel pnlSearchOptions;
    private System.Windows.Forms.Button btnRunQuery;
    private System.Windows.Forms.GroupBox grpDisplayMode;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.ComboBox cmbItemType;
    private System.Windows.Forms.ComboBox cmbLanguage;
    private System.Windows.Forms.CheckBox chkShowIcons;
    private System.Windows.Forms.ContextMenu mnuItemListContext;
    private System.Windows.Forms.MenuItem mnuILCCopy;
    private System.Windows.Forms.StatusBar stbStatus;
    private System.Windows.Forms.MenuItem mnuILCExport;
    private System.Windows.Forms.MenuItem mnuILCEAll;
    private System.Windows.Forms.MenuItem mnuILCEResults;
    private System.Windows.Forms.MenuItem mnuILCESelected;
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemFindDialog));
      this.lstItems = new System.Windows.Forms.ListView();
      this.mnuItemListContext = new System.Windows.Forms.ContextMenu();
      this.mnuILCCopy = new System.Windows.Forms.MenuItem();
      this.mnuILCExport = new System.Windows.Forms.MenuItem();
      this.mnuILCEAll = new System.Windows.Forms.MenuItem();
      this.mnuILCEResults = new System.Windows.Forms.MenuItem();
      this.mnuILCESelected = new System.Windows.Forms.MenuItem();
      this.ilItemIcons = new System.Windows.Forms.ImageList(this.components);
      this.pnlSearchOptions = new System.Windows.Forms.Panel();
      this.btnClose = new System.Windows.Forms.Button();
      this.grpDisplayMode = new System.Windows.Forms.GroupBox();
      this.chkShowIcons = new System.Windows.Forms.CheckBox();
      this.cmbItemType = new System.Windows.Forms.ComboBox();
      this.cmbLanguage = new System.Windows.Forms.ComboBox();
      this.btnRunQuery = new System.Windows.Forms.Button();
      this.stbStatus = new System.Windows.Forms.StatusBar();
      this.pnlSearchOptions.SuspendLayout();
      this.grpDisplayMode.SuspendLayout();
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
      this.lstItems.View = System.Windows.Forms.View.Details;
      this.lstItems.SelectedIndexChanged += new System.EventHandler(this.lstItems_SelectedIndexChanged);
      this.lstItems.DoubleClick += new System.EventHandler(this.lstItems_DoubleClick);
      // 
      // mnuItemListContext
      // 
      this.mnuItemListContext.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuILCCopy,
            this.mnuILCExport});
      this.mnuItemListContext.Popup += new System.EventHandler(this.mnuItemListContext_Popup);
      // 
      // mnuILCCopy
      // 
      this.mnuILCCopy.Index = 0;
      resources.ApplyResources(this.mnuILCCopy, "mnuILCCopy");
      // 
      // mnuILCExport
      // 
      this.mnuILCExport.Index = 1;
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
      this.mnuILCEResults.Click += new System.EventHandler(this.mnuILCECResults_Click);
      // 
      // mnuILCESelected
      // 
      resources.ApplyResources(this.mnuILCESelected, "mnuILCESelected");
      this.mnuILCESelected.Index = 2;
      this.mnuILCESelected.Click += new System.EventHandler(this.mnuILCECSelected_Click);
      // 
      // ilItemIcons
      // 
      this.ilItemIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
      resources.ApplyResources(this.ilItemIcons, "ilItemIcons");
      this.ilItemIcons.TransparentColor = System.Drawing.Color.Transparent;
      // 
      // pnlSearchOptions
      // 
      this.pnlSearchOptions.Controls.Add(this.btnClose);
      this.pnlSearchOptions.Controls.Add(this.grpDisplayMode);
      this.pnlSearchOptions.Controls.Add(this.btnRunQuery);
      resources.ApplyResources(this.pnlSearchOptions, "pnlSearchOptions");
      this.pnlSearchOptions.Name = "pnlSearchOptions";
      // 
      // btnClose
      // 
      this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      resources.ApplyResources(this.btnClose, "btnClose");
      this.btnClose.Name = "btnClose";
      // 
      // grpDisplayMode
      // 
      this.grpDisplayMode.Controls.Add(this.chkShowIcons);
      this.grpDisplayMode.Controls.Add(this.cmbItemType);
      this.grpDisplayMode.Controls.Add(this.cmbLanguage);
      this.grpDisplayMode.FlatStyle = System.Windows.Forms.FlatStyle.System;
      resources.ApplyResources(this.grpDisplayMode, "grpDisplayMode");
      this.grpDisplayMode.Name = "grpDisplayMode";
      this.grpDisplayMode.TabStop = false;
      // 
      // chkShowIcons
      // 
      resources.ApplyResources(this.chkShowIcons, "chkShowIcons");
      this.chkShowIcons.Name = "chkShowIcons";
      this.chkShowIcons.CheckedChanged += new System.EventHandler(this.chkShowIcons_CheckedChanged);
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
      this.grpDisplayMode.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

  }

}
