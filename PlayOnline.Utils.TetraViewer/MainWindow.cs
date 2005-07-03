// $Id$

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
//using System.Data;
using System.Runtime.InteropServices;

using PlayOnline.Core;

namespace PlayOnline.Utils.TetraViewer {

  public class MainWindow : System.Windows.Forms.Form {

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

    public MainWindow() {
      InitializeComponent();
      this.Icon = Icons.TetraMaster;
    TreeNode Root = new TreeNode("Tetra Master Data Files");
      this.tvDataFiles.Nodes.Add(Root);
    string TetraDir = POL.GetApplicationPath(AppID.TetraMaster);
      foreach (string DataFilePath in Directory.GetFiles(Path.Combine(TetraDir, "data"), "gW*.dat")) {
      TreeNode TN = new TreeNode(Path.GetFileName(DataFilePath));
	TN.Tag = DataFilePath;
	Root.Nodes.Add(TN);
      FileStream F = new FileStream(DataFilePath, FileMode.Open, FileAccess.Read);
	this.ReadTOC(F, TN);
      }
      Root.Expand();
    }

    protected override void Dispose(bool disposing) {
      if (disposing && components != null) 
	components.Dispose();
      base.Dispose(disposing);
    }

    private class TOCEntry {
      public Stream Source;
      public int    Type;
      public string Name;
      public long   Offset;
      public int    Size;

      public TOCEntry(Stream S, long BaseOffset) {
	this.Source = S;
      BinaryReader BR = new BinaryReader(S, Encoding.ASCII);
	this.Type = BR.ReadInt32();
	this.Name = new string (BR.ReadChars(12));
	{
	int end = this.Name.IndexOf('\0');
	  if (end >= 0)
	    this.Name = this.Name.Substring(0, end);
	}
	foreach (char c in Path.InvalidPathChars)
	  this.Name = this.Name.Replace(c, '_');
	this.Offset = BaseOffset + BR.ReadInt32();
	this.Size   = BR.ReadInt32();
	BR.ReadInt32();
	BR.ReadInt32();
      }
    }

    private void ReadSubTOC(Stream S, TreeNode Root) {
    long BaseOffset = S.Position;
    TOCEntry TE = new TOCEntry(S, BaseOffset);
      while (TE.Type == 0x4000) {
      TreeNode TN = new TreeNode(TE.Name);
	TN.Tag = TE;
	Root.Nodes.Add(TN);
	TE = new TOCEntry(S, BaseOffset);
      }
    }

    private void ReadTOC(Stream S, TreeNode Root) {
    long BaseOffset = S.Position;
    TOCEntry TE = new TOCEntry(S, BaseOffset);
      while (TE.Type == 0x8000) {
      long Pos = S.Position;
	S.Seek(BaseOffset + TE.Offset, SeekOrigin.Begin);
	this.ReadSubTOC(S, Root);
	S.Seek(Pos, SeekOrigin.Begin);
	TE = new TOCEntry(S, BaseOffset);
      }
    }

//    private void ExportFile(ListViewItem LVI, string directory) {
//    string SavedStatus = this.sbrStatus.Text;
//    TOCEntry TE = LVI.Tag as TOCEntry;
//    string FileName = Path.Combine(directory, TE.Name + ".png");
//      this.sbrStatus.Text = "Exporting: " + FileName;
//    FileStream OutputFile = new FileStream(FileName, FileMode.Create, FileAccess.Write);
//      TE.Source.Seek(TE.Offset, SeekOrigin.Begin);
//    BinaryReader BR = new BinaryReader(TE.Source);
//    BinaryWriter BW = new BinaryWriter(OutputFile);
//      BW.Write(BR.ReadBytes(TE.Size));
//      BW.Close();
//      this.sbrStatus.Text = SavedStatus;
//    }

    private void ClearImage() {
      this.picViewer.BackgroundImage = null;
      this.picViewer.Image = null;
    }

    private void ShowImage() {
      if (this.tvDataFiles.SelectedNode == null)
	return;
    TOCEntry TE = this.tvDataFiles.SelectedNode.Tag as TOCEntry;
      if (TE == null)
	return;
      TE.Source.Seek(TE.Offset, SeekOrigin.Begin);
      try {
      Image I = null;
	{ // Create buffer and make the image from that - Image.FromStream(this.CurrentFile) does NOT work
	BinaryReader BR = new BinaryReader(TE.Source);
	byte[] ImageData = BR.ReadBytes(TE.Size);
	MemoryStream MS = new MemoryStream(ImageData, false);
	  I = Image.FromStream(MS);
	  MS.Close();
	}
	if (this.mnuTiledImage.Checked)
	  this.picViewer.BackgroundImage = I;
	else
	  this.picViewer.Image = I;
      this.picViewer.SizeMode = (this.mnuStretchImage.Checked ? PictureBoxSizeMode.StretchImage : PictureBoxSizeMode.CenterImage);
      int bitdepth = 0;
	switch (I.PixelFormat) {
	  case PixelFormat.Format1bppIndexed:
	    bitdepth = 1;
	    break;
	  case PixelFormat.Format4bppIndexed:
	    bitdepth = 4;
	    break;
	  case PixelFormat.Format8bppIndexed:
	    bitdepth = 8;
	    break;
	  case PixelFormat.Format16bppGrayScale:
	  case PixelFormat.Format16bppArgb1555:
	  case PixelFormat.Format16bppRgb555:
	  case PixelFormat.Format16bppRgb565:
	    bitdepth = 16;
	    break;
	  case PixelFormat.Format24bppRgb:
	    bitdepth = 24;
	    break;
	  case PixelFormat.Format32bppArgb:
	  case PixelFormat.Format32bppPArgb:
	  case PixelFormat.Format32bppRgb:
	    bitdepth = 32;
	    break;
	  case PixelFormat.Format48bppRgb:
	    bitdepth = 48;
	    break;
	  case PixelFormat.Format64bppArgb:
	  case PixelFormat.Format64bppPArgb:
	    bitdepth = 64;
	    break;
	}
	//ImageConverter IC = new ImageConverter();
	this.sbrStatus.Text = String.Format("PNG Image - {0}x{1} {2}bpp", I.Width, I.Height, bitdepth);
      }
      catch {}
    }

    #region Windows Form Designer generated code

    private void InitializeComponent() {
System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainWindow));
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
this.SuspendLayout();
// 
// dlgBrowseFolder
// 
this.dlgBrowseFolder.Description = resources.GetString("dlgBrowseFolder.Description");
this.dlgBrowseFolder.SelectedPath = resources.GetString("dlgBrowseFolder.SelectedPath");
// 
// tvDataFiles
// 
this.tvDataFiles.AccessibleDescription = resources.GetString("tvDataFiles.AccessibleDescription");
this.tvDataFiles.AccessibleName = resources.GetString("tvDataFiles.AccessibleName");
this.tvDataFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("tvDataFiles.Anchor")));
this.tvDataFiles.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tvDataFiles.BackgroundImage")));
this.tvDataFiles.ContextMenu = this.mnuTreeContext;
this.tvDataFiles.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("tvDataFiles.Dock")));
this.tvDataFiles.Enabled = ((bool)(resources.GetObject("tvDataFiles.Enabled")));
this.tvDataFiles.Font = ((System.Drawing.Font)(resources.GetObject("tvDataFiles.Font")));
this.tvDataFiles.HideSelection = false;
this.tvDataFiles.ImageIndex = ((int)(resources.GetObject("tvDataFiles.ImageIndex")));
this.tvDataFiles.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("tvDataFiles.ImeMode")));
this.tvDataFiles.Indent = ((int)(resources.GetObject("tvDataFiles.Indent")));
this.tvDataFiles.ItemHeight = ((int)(resources.GetObject("tvDataFiles.ItemHeight")));
this.tvDataFiles.Location = ((System.Drawing.Point)(resources.GetObject("tvDataFiles.Location")));
this.tvDataFiles.Name = "tvDataFiles";
this.tvDataFiles.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("tvDataFiles.RightToLeft")));
this.tvDataFiles.SelectedImageIndex = ((int)(resources.GetObject("tvDataFiles.SelectedImageIndex")));
this.tvDataFiles.Size = ((System.Drawing.Size)(resources.GetObject("tvDataFiles.Size")));
this.tvDataFiles.TabIndex = ((int)(resources.GetObject("tvDataFiles.TabIndex")));
this.tvDataFiles.Text = resources.GetString("tvDataFiles.Text");
this.tvDataFiles.Visible = ((bool)(resources.GetObject("tvDataFiles.Visible")));
this.tvDataFiles.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvDataFiles_AfterSelect);
// 
// mnuTreeContext
// 
this.mnuTreeContext.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuExportAll,
            this.mnuExport});
this.mnuTreeContext.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mnuTreeContext.RightToLeft")));
// 
// mnuExportAll
// 
this.mnuExportAll.Enabled = ((bool)(resources.GetObject("mnuExportAll.Enabled")));
this.mnuExportAll.Index = 0;
this.mnuExportAll.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuExportAll.Shortcut")));
this.mnuExportAll.ShowShortcut = ((bool)(resources.GetObject("mnuExportAll.ShowShortcut")));
this.mnuExportAll.Text = resources.GetString("mnuExportAll.Text");
this.mnuExportAll.Visible = ((bool)(resources.GetObject("mnuExportAll.Visible")));
// 
// mnuExport
// 
this.mnuExport.Enabled = ((bool)(resources.GetObject("mnuExport.Enabled")));
this.mnuExport.Index = 1;
this.mnuExport.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuExport.Shortcut")));
this.mnuExport.ShowShortcut = ((bool)(resources.GetObject("mnuExport.ShowShortcut")));
this.mnuExport.Text = resources.GetString("mnuExport.Text");
this.mnuExport.Visible = ((bool)(resources.GetObject("mnuExport.Visible")));
// 
// sbrStatus
// 
this.sbrStatus.AccessibleDescription = resources.GetString("sbrStatus.AccessibleDescription");
this.sbrStatus.AccessibleName = resources.GetString("sbrStatus.AccessibleName");
this.sbrStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("sbrStatus.Anchor")));
this.sbrStatus.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("sbrStatus.BackgroundImage")));
this.sbrStatus.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("sbrStatus.Dock")));
this.sbrStatus.Enabled = ((bool)(resources.GetObject("sbrStatus.Enabled")));
this.sbrStatus.Font = ((System.Drawing.Font)(resources.GetObject("sbrStatus.Font")));
this.sbrStatus.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("sbrStatus.ImeMode")));
this.sbrStatus.Location = ((System.Drawing.Point)(resources.GetObject("sbrStatus.Location")));
this.sbrStatus.Name = "sbrStatus";
this.sbrStatus.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("sbrStatus.RightToLeft")));
this.sbrStatus.Size = ((System.Drawing.Size)(resources.GetObject("sbrStatus.Size")));
this.sbrStatus.TabIndex = ((int)(resources.GetObject("sbrStatus.TabIndex")));
this.sbrStatus.Text = resources.GetString("sbrStatus.Text");
this.sbrStatus.Visible = ((bool)(resources.GetObject("sbrStatus.Visible")));
// 
// picViewer
// 
this.picViewer.AccessibleDescription = resources.GetString("picViewer.AccessibleDescription");
this.picViewer.AccessibleName = resources.GetString("picViewer.AccessibleName");
this.picViewer.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("picViewer.Anchor")));
this.picViewer.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picViewer.BackgroundImage")));
this.picViewer.ContextMenu = this.mnuPictureContext;
this.picViewer.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("picViewer.Dock")));
this.picViewer.Enabled = ((bool)(resources.GetObject("picViewer.Enabled")));
this.picViewer.Font = ((System.Drawing.Font)(resources.GetObject("picViewer.Font")));
this.picViewer.Image = ((System.Drawing.Image)(resources.GetObject("picViewer.Image")));
this.picViewer.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("picViewer.ImeMode")));
this.picViewer.Location = ((System.Drawing.Point)(resources.GetObject("picViewer.Location")));
this.picViewer.Name = "picViewer";
this.picViewer.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("picViewer.RightToLeft")));
this.picViewer.Size = ((System.Drawing.Size)(resources.GetObject("picViewer.Size")));
this.picViewer.SizeMode = ((System.Windows.Forms.PictureBoxSizeMode)(resources.GetObject("picViewer.SizeMode")));
this.picViewer.TabIndex = ((int)(resources.GetObject("picViewer.TabIndex")));
this.picViewer.TabStop = false;
this.picViewer.Text = resources.GetString("picViewer.Text");
this.picViewer.Visible = ((bool)(resources.GetObject("picViewer.Visible")));
// 
// mnuPictureContext
// 
this.mnuPictureContext.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuNormalImage,
            this.mnuStretchImage,
            this.mnuTiledImage});
this.mnuPictureContext.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mnuPictureContext.RightToLeft")));
// 
// mnuNormalImage
// 
this.mnuNormalImage.Checked = true;
this.mnuNormalImage.Enabled = ((bool)(resources.GetObject("mnuNormalImage.Enabled")));
this.mnuNormalImage.Index = 0;
this.mnuNormalImage.RadioCheck = true;
this.mnuNormalImage.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuNormalImage.Shortcut")));
this.mnuNormalImage.ShowShortcut = ((bool)(resources.GetObject("mnuNormalImage.ShowShortcut")));
this.mnuNormalImage.Text = resources.GetString("mnuNormalImage.Text");
this.mnuNormalImage.Visible = ((bool)(resources.GetObject("mnuNormalImage.Visible")));
this.mnuNormalImage.Click += new System.EventHandler(this.ImageOption_Click);
// 
// mnuStretchImage
// 
this.mnuStretchImage.Enabled = ((bool)(resources.GetObject("mnuStretchImage.Enabled")));
this.mnuStretchImage.Index = 1;
this.mnuStretchImage.RadioCheck = true;
this.mnuStretchImage.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuStretchImage.Shortcut")));
this.mnuStretchImage.ShowShortcut = ((bool)(resources.GetObject("mnuStretchImage.ShowShortcut")));
this.mnuStretchImage.Text = resources.GetString("mnuStretchImage.Text");
this.mnuStretchImage.Visible = ((bool)(resources.GetObject("mnuStretchImage.Visible")));
this.mnuStretchImage.Click += new System.EventHandler(this.ImageOption_Click);
// 
// mnuTiledImage
// 
this.mnuTiledImage.Enabled = ((bool)(resources.GetObject("mnuTiledImage.Enabled")));
this.mnuTiledImage.Index = 2;
this.mnuTiledImage.RadioCheck = true;
this.mnuTiledImage.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuTiledImage.Shortcut")));
this.mnuTiledImage.ShowShortcut = ((bool)(resources.GetObject("mnuTiledImage.ShowShortcut")));
this.mnuTiledImage.Text = resources.GetString("mnuTiledImage.Text");
this.mnuTiledImage.Visible = ((bool)(resources.GetObject("mnuTiledImage.Visible")));
this.mnuTiledImage.Click += new System.EventHandler(this.ImageOption_Click);
// 
// MainWindow
// 
this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
this.AccessibleName = resources.GetString("$this.AccessibleName");
this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
this.Controls.Add(this.picViewer);
this.Controls.Add(this.tvDataFiles);
this.Controls.Add(this.sbrStatus);
this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
this.Name = "MainWindow";
this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
this.Text = resources.GetString("$this.Text");
this.ResumeLayout(false);

    }

    #endregion

    private void tvDataFiles_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e) {
      this.sbrStatus.Text = "";
      this.mnuExport.Visible = false;
      this.mnuExportAll.Visible = false;
      this.ClearImage();
      if (e.Node == null)
	return;
      if (e.Node.Parent != null && e.Node.Parent.Parent != null && e.Node.Nodes.Count == 0)
	this.mnuExport.Visible = true;
      if (e.Node.Parent != null && e.Node.Parent.Parent == null && e.Node.Nodes.Count != 0)
	this.mnuExportAll.Visible = true;
      this.ShowImage();
    }

    private void ImageOption_Click(object sender, System.EventArgs e) {
      this.ClearImage();
      foreach (MenuItem MI in this.mnuPictureContext.MenuItems)
	MI.Checked = (MI == sender);
      this.ShowImage();
    }

  }

}
