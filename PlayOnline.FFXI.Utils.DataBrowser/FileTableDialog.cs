// $Id$

using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using PlayOnline.Core;

namespace PlayOnline.FFXI.Utils.DataBrowser {

  internal class FileTableDialog : System.Windows.Forms.Form {

    #region Controls

    private System.Windows.Forms.ListView lstFileTable;
    private System.Windows.Forms.ColumnHeader colFileID;
    private System.Windows.Forms.ColumnHeader colLocation;
    private System.Windows.Forms.ColumnHeader colProvider;
    private System.Windows.Forms.StatusBar stbStatus;

    private System.ComponentModel.IContainer components = null;

    #endregion

    public FileTableDialog() {
      this.InitializeComponent();
      this.lstFileTable.ColumnClick += new ColumnClickEventHandler(ListViewColumnSorter.ListView_ColumnClick);
    }

    private void PopulateList() {
      this.lstFileTable.HeaderStyle = ColumnHeaderStyle.Nonclickable;
    string DataRoot = POL.GetApplicationPath(AppID.FFXI);
      for (int i = 1; i < 10; ++i) {
      string AppName = I18N.GetText(String.Format("FFXI{0}", i));
      string Suffix = "";
      string DataDir = DataRoot;
	if (i > 1) {
	  Suffix = i.ToString();
	  DataDir = Path.Combine(DataRoot, "Rom" + Suffix);
	}
      string VTableFile = Path.Combine(DataDir, String.Format("VTABLE{0}.DAT", Suffix));
      string FTableFile = Path.Combine(DataDir, String.Format("FTABLE{0}.DAT", Suffix));
	if (File.Exists(VTableFile) && File.Exists(FTableFile)) {
	  try {
	  BinaryReader VBR = new BinaryReader(new FileStream(VTableFile, FileMode.Open, FileAccess.Read, FileShare.Read));
	  BinaryReader FBR = new BinaryReader(new FileStream(FTableFile, FileMode.Open, FileAccess.Read, FileShare.Read));
	  long MaxFileNo = VBR.BaseStream.Length;
	    this.stbStatus.Text = AppName;
	  long FileCount = 0;
	    for (long FileNo = 0; FileNo < MaxFileNo; ++FileNo) {
	      if (VBR.ReadByte() == i) {
	      ListViewItem LVI = this.lstFileTable.Items.Add(String.Format("{0:D6}", FileNo));
		LVI.SubItems.Add(AppName);
		FBR.BaseStream.Seek(2 * FileNo, SeekOrigin.Begin);
	      ushort Location = FBR.ReadUInt16();
		LVI.SubItems.Add(Path.Combine(DataDir, String.Format("{0}{1}{2}.DAT", Location / 0x80, Path.DirectorySeparatorChar, Location % 0x80)));
		++FileCount;
		if ((FileCount % 100) == 0) {
		  this.stbStatus.Text = String.Format(I18N.GetText("FileTableDialog:StatusFormat"), AppName, FileCount);
		  Application.DoEvents();
		}
	      }
	    }
	    VBR.Close();
	    FBR.Close();
	  }
	  catch (Exception E) { Console.WriteLine("{0}", E.ToString()); }
	}
      }
      Application.DoEvents();
      this.lstFileTable.HeaderStyle = ColumnHeaderStyle.Clickable;
      this.stbStatus.Text = String.Empty;
      Application.DoEvents();
    }

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
	components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FileTableDialog));
      this.lstFileTable = new System.Windows.Forms.ListView();
      this.colFileID = new System.Windows.Forms.ColumnHeader();
      this.colProvider = new System.Windows.Forms.ColumnHeader();
      this.colLocation = new System.Windows.Forms.ColumnHeader();
      this.stbStatus = new System.Windows.Forms.StatusBar();
      this.SuspendLayout();
      // 
      // lstFileTable
      // 
      this.lstFileTable.AccessibleDescription = resources.GetString("lstFileTable.AccessibleDescription");
      this.lstFileTable.AccessibleName = resources.GetString("lstFileTable.AccessibleName");
      this.lstFileTable.Alignment = ((System.Windows.Forms.ListViewAlignment)(resources.GetObject("lstFileTable.Alignment")));
      this.lstFileTable.AllowColumnReorder = true;
      this.lstFileTable.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lstFileTable.Anchor")));
      this.lstFileTable.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("lstFileTable.BackgroundImage")));
      this.lstFileTable.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
										   this.colFileID,
										   this.colProvider,
										   this.colLocation});
      this.lstFileTable.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lstFileTable.Dock")));
      this.lstFileTable.Enabled = ((bool)(resources.GetObject("lstFileTable.Enabled")));
      this.lstFileTable.Font = ((System.Drawing.Font)(resources.GetObject("lstFileTable.Font")));
      this.lstFileTable.FullRowSelect = true;
      this.lstFileTable.GridLines = true;
      this.lstFileTable.HideSelection = false;
      this.lstFileTable.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lstFileTable.ImeMode")));
      this.lstFileTable.LabelWrap = ((bool)(resources.GetObject("lstFileTable.LabelWrap")));
      this.lstFileTable.Location = ((System.Drawing.Point)(resources.GetObject("lstFileTable.Location")));
      this.lstFileTable.Name = "lstFileTable";
      this.lstFileTable.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lstFileTable.RightToLeft")));
      this.lstFileTable.Size = ((System.Drawing.Size)(resources.GetObject("lstFileTable.Size")));
      this.lstFileTable.TabIndex = ((int)(resources.GetObject("lstFileTable.TabIndex")));
      this.lstFileTable.Text = resources.GetString("lstFileTable.Text");
      this.lstFileTable.View = System.Windows.Forms.View.Details;
      this.lstFileTable.Visible = ((bool)(resources.GetObject("lstFileTable.Visible")));
      // 
      // colFileID
      // 
      this.colFileID.Text = resources.GetString("colFileID.Text");
      this.colFileID.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("colFileID.TextAlign")));
      this.colFileID.Width = ((int)(resources.GetObject("colFileID.Width")));
      // 
      // colProvider
      // 
      this.colProvider.Text = resources.GetString("colProvider.Text");
      this.colProvider.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("colProvider.TextAlign")));
      this.colProvider.Width = ((int)(resources.GetObject("colProvider.Width")));
      // 
      // colLocation
      // 
      this.colLocation.Text = resources.GetString("colLocation.Text");
      this.colLocation.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("colLocation.TextAlign")));
      this.colLocation.Width = ((int)(resources.GetObject("colLocation.Width")));
      // 
      // stbStatus
      // 
      this.stbStatus.AccessibleDescription = resources.GetString("stbStatus.AccessibleDescription");
      this.stbStatus.AccessibleName = resources.GetString("stbStatus.AccessibleName");
      this.stbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("stbStatus.Anchor")));
      this.stbStatus.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("stbStatus.BackgroundImage")));
      this.stbStatus.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("stbStatus.Dock")));
      this.stbStatus.Enabled = ((bool)(resources.GetObject("stbStatus.Enabled")));
      this.stbStatus.Font = ((System.Drawing.Font)(resources.GetObject("stbStatus.Font")));
      this.stbStatus.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("stbStatus.ImeMode")));
      this.stbStatus.Location = ((System.Drawing.Point)(resources.GetObject("stbStatus.Location")));
      this.stbStatus.Name = "stbStatus";
      this.stbStatus.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("stbStatus.RightToLeft")));
      this.stbStatus.Size = ((System.Drawing.Size)(resources.GetObject("stbStatus.Size")));
      this.stbStatus.TabIndex = ((int)(resources.GetObject("stbStatus.TabIndex")));
      this.stbStatus.Text = resources.GetString("stbStatus.Text");
      this.stbStatus.Visible = ((bool)(resources.GetObject("stbStatus.Visible")));
      // 
      // FileTableDialog
      // 
      this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
      this.AccessibleName = resources.GetString("$this.AccessibleName");
      this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
      this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
      this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
      this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
      this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
      this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
      this.Controls.Add(this.stbStatus);
      this.Controls.Add(this.lstFileTable);
      this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
      this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
      this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
      this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
      this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
      this.Name = "FileTableDialog";
      this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
      this.ShowInTaskbar = false;
      this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
      this.Text = resources.GetString("$this.Text");
      this.Activated += new System.EventHandler(this.FileTableDialog_Activated);
      this.ResumeLayout(false);

    }

    #endregion

    private void FileTableDialog_Activated(object sender, System.EventArgs e) {
      if (this.Visible && this.lstFileTable.Items.Count == 0)
	this.PopulateList();
    }


  }

}
