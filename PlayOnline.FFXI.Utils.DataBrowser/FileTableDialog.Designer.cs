namespace PlayOnline.FFXI.Utils.DataBrowser {

  internal partial class FileTableDialog {

    #region Controls

    private System.Windows.Forms.ListView lstFileTable;
    private System.Windows.Forms.ColumnHeader colFileID;
    private System.Windows.Forms.ColumnHeader colLocation;
    private System.Windows.Forms.ColumnHeader colProvider;
    private System.Windows.Forms.StatusBar stbStatus;

    private System.ComponentModel.IContainer components = null;

    #endregion

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileTableDialog));
      this.lstFileTable = new System.Windows.Forms.ListView();
      this.colFileID = new System.Windows.Forms.ColumnHeader();
      this.colProvider = new System.Windows.Forms.ColumnHeader();
      this.colLocation = new System.Windows.Forms.ColumnHeader();
      this.stbStatus = new System.Windows.Forms.StatusBar();
      this.SuspendLayout();
      // 
      // lstFileTable
      // 
      this.lstFileTable.AllowColumnReorder = true;
      this.lstFileTable.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colFileID,
            this.colProvider,
            this.colLocation});
      resources.ApplyResources(this.lstFileTable, "lstFileTable");
      this.lstFileTable.FullRowSelect = true;
      this.lstFileTable.GridLines = true;
      this.lstFileTable.HideSelection = false;
      this.lstFileTable.Name = "lstFileTable";
      this.lstFileTable.View = System.Windows.Forms.View.Details;
      // 
      // colFileID
      // 
      resources.ApplyResources(this.colFileID, "colFileID");
      // 
      // colProvider
      // 
      resources.ApplyResources(this.colProvider, "colProvider");
      // 
      // colLocation
      // 
      resources.ApplyResources(this.colLocation, "colLocation");
      // 
      // stbStatus
      // 
      resources.ApplyResources(this.stbStatus, "stbStatus");
      this.stbStatus.Name = "stbStatus";
      // 
      // FileTableDialog
      // 
      resources.ApplyResources(this, "$this");
      this.Controls.Add(this.lstFileTable);
      this.Controls.Add(this.stbStatus);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
      this.Name = "FileTableDialog";
      this.ShowInTaskbar = false;
      this.Activated += new System.EventHandler(this.FileTableDialog_Activated);
      this.ResumeLayout(false);

    }

    #endregion

  }

}
