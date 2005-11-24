// $Id$

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using PlayOnline.Core;

namespace PlayOnline.FFXI.Utils.DataBrowser {

  public partial class MainWindow : Form {

    #region Custom Menu Items

    private class ROMMenuItem : MenuItem {

      public ROMMenuItem(string Text, int App, int Dir, int File, EventHandler OnClick) : base(Text, OnClick) {
	this.App_  = App;
	this.Dir_  = Dir;
	this.File_ = File;
	this.Enabled = System.IO.File.Exists(this.ROMFilePath);
      }

      public int ROMApp  { get { return this.App_;  } }
      public int ROMDir  { get { return this.Dir_;  } }
      public int ROMFile { get { return this.File_; } }

      private int App_;
      private int Dir_;
      private int File_;

      public string ROMFilePath {
	get {
	string ROMDir = "Rom";
	  if (this.App_ > 0)
	    ROMDir += String.Format("{0}", this.App_ + 1);
	  return Path.Combine(Path.Combine(Path.Combine(POL.GetApplicationPath(AppID.FFXI), ROMDir), this.Dir_.ToString()), String.Format("{0}.DAT", this.File_));
	}
      }

    }

    private class CategoryMenuItem : MenuItem {

      private bool         SubItemsAdded_    = false;
      private XmlNode      XCategory_        = null;
      private EventHandler ROMMenuItemClick_ = null;

      public CategoryMenuItem(XmlNode XCategory, EventHandler ROMMenuItemClick) : base() {
	this.XCategory_ = XCategory;
	this.ROMMenuItemClick_ = ROMMenuItemClick;
	this.Text = this.BuildItemName(XCategory.SelectSingleNode("./name"));
	this.MenuItems.Add(String.Empty); // dummy entry - replaced on first popup
      }

      protected override void OnPopup(EventArgs e) {
	if (!this.SubItemsAdded_) {
	  this.MenuItems.Clear();
	  foreach (XmlNode XN in this.XCategory_.ChildNodes) {
	    if (XN is XmlElement) {
	      if (XN.Name == "category")
		this.MenuItems.Add(new CategoryMenuItem(XN, this.ROMMenuItemClick_));
	      else if (XN.Name == "separator")
		this.MenuItems.Add("-");
	      else if (XN.Name == "rom-file") {
		try {
		int ROMApp  = XmlConvert.ToInt32(XN.Attributes["app"].InnerText);
		int ROMDir  = XmlConvert.ToInt32(XN.Attributes["dir"].InnerText);
		int ROMFile = XmlConvert.ToInt32(XN.Attributes["file"].InnerText);
		  this.MenuItems.Add(new ROMMenuItem(this.BuildItemName(XN), ROMApp, ROMDir, ROMFile, this.ROMMenuItemClick_));
		} catch { }
	      }
	    }
	  }
	  this.SubItemsAdded_ = true;
	}
	base.OnPopup(e);
      }

      private string BuildItemName(XmlNode XName) {
	if (XName == null)
	  return "???";
      string ItemName = String.Empty;
	foreach (XmlNode XN in XName.ChildNodes) {
	  if (XN is XmlText)
	    ItemName += XN.InnerText;
	  else if (XN is XmlElement) {
	  XmlElement XE = XN as XmlElement;
	    if (XE.Name == "i18n-string" && XE.HasAttribute("id"))
	      ItemName += I18N.GetText(XE.Attributes["id"].InnerText);
	    else if (XE.Name == "ffxi-string" && XE.HasAttribute("id")) {
	    uint ResID = 0;
	      try { ResID = uint.Parse(XE.Attributes["id"].InnerText, NumberStyles.HexNumber); } catch { }
	      ItemName += FFXIResourceManager.GetResourceString(ResID);
	     }
	    else
	      ItemName += '?' + XE.Name + '?';
	  }
	}
	return ItemName;
      }

    }

    #endregion

    public MainWindow() {
      this.InitializeComponent();
      this.Icon = Icons.CheckedPage;
      try {
	this.ilBrowserIcons.Images.Add(Icons.DocFolder);
	this.ilBrowserIcons.Images.Add(Icons.FolderClosed);
	this.ilBrowserIcons.Images.Add(Icons.FolderOpen);
	this.ilBrowserIcons.Images.Add(Icons.ConfigFile);
      }
      catch (Exception E) {
	Console.WriteLine("{0}", E.ToString());
	this.tvDataFiles.ImageList = null;
      }
      for (int i = 1; i < 5; ++i) {
      string DataDir = Path.Combine(POL.GetApplicationPath(AppID.FFXI), "Rom");
	if (i > 1)
	  DataDir += i.ToString();
	if (Directory.Exists(DataDir)) {
	TreeNode Root = this.tvDataFiles.Nodes.Add(I18N.GetText(String.Format("FFXI{0}", i)));
	  Root.ImageIndex = Root.SelectedImageIndex = 0;
	  Root.Tag = DataDir;
	  Root.Nodes.Add("<dummy>").Tag = Root;
	}
      }
      this.lstEntries.ColumnClick += new ColumnClickEventHandler(ListViewColumnSorter.ListView_ColumnClick);
      this.InitializeROMMenus();
      this.ResetViewers();
    }

    private void ResetViewers() {
      // Clear the entire right-hand pane
      this.pnlNoViewers.Visible = false;
      this.tabViewers.Visible = false;
      // Reset the viewer tabs
      this.tabViewers.TabPages.Clear();
      // Reset all applicable viewer tab contents
      this.cmbItems.Items.Clear();
      this.cmbImageChooser.Items.Clear();
      this.picImageViewer.Image = null;
      this.picImageViewer.Tag = null;
      this.lstEntries.Items.Clear();
      this.lstEntries.ListViewItemSorter = null;
    }

    #region Image Viewer Events

    private void cmbImageChooser_SelectedIndexChanged(object sender, System.EventArgs e) {
    FFXIGraphic FG = this.cmbImageChooser.SelectedItem as FFXIGraphic;
      if (FG != null) {
	this.picImageViewer.Image = FG.Bitmap;
	this.picImageViewer.Tag = FG.ToString();
      }
    }

    private FolderBrowserDialog dlgBrowseFolder = null;

    private void PrepareFolderBrowser(string Description) {
      if (this.dlgBrowseFolder == null) {
	this.dlgBrowseFolder = new FolderBrowserDialog();
	this.dlgBrowseFolder.Description = Description;
      string Location = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "POLUtils");
	if (!Directory.Exists(Location))
	  Directory.CreateDirectory(Location);
	this.dlgBrowseFolder.SelectedPath = Location;
      }
    }

    private void TSaveAllImages() {
    PleaseWaitDialog PWD = new PleaseWaitDialog(I18N.GetText("Dialog:SaveAllImages"));
      try {
	Application.DoEvents();
	PWD.ShowDialog(this);
      } catch { PWD.Close(); }
    }

    private void btnImageSaveAll_Click(object sender, System.EventArgs e) {
      this.PrepareFolderBrowser(I18N.GetText("Description:BrowseImageExportFolder"));
      if (this.dlgBrowseFolder.ShowDialog() == DialogResult.OK) {
      Thread T = new Thread(new ThreadStart(this.TSaveAllImages));
	T.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
	T.Start();
	Application.DoEvents();
	foreach (FFXIGraphic G in this.cmbImageChooser.Items) {
	  G.Bitmap.Save(Path.Combine(this.dlgBrowseFolder.SelectedPath, G.ToString() + ".png"), ImageFormat.Png);
	  Application.DoEvents();
	}
	T.Abort();
	this.Activate();
      }
    }

    #region Context Menu Events

    private PictureBox GetSourcePicture(MenuItem SourceMenu) {
      if (SourceMenu != null) {
      ContextMenu CM = SourceMenu.GetContextMenu();
	if (CM != null)
	  return CM.SourceControl as PictureBox;
      }
      return null;
    }

    private void SetPictureSizeMode(PictureBox PB, PictureBoxSizeMode SizeMode) {
      if (PB != null)
	PB.SizeMode = SizeMode;
    }

    private void SetPictureBackground(PictureBox PB, Color BackColor) {
      if (PB != null)
	PB.BackColor = BackColor;
    }

    private void SavePicture(PictureBox PB) {
      if (PB != null) {
	this.dlgSavePicture.FileName = PB.Tag as string;
	if (this.dlgSavePicture.ShowDialog() == DialogResult.OK) {
	ImageFormat IF = ImageFormat.Bmp;
	  switch (this.dlgSavePicture.FilterIndex) {
	    case 1: IF = ImageFormat.Bmp; break;
	    case 2: IF = ImageFormat.Png; break;
	  }
	  PB.Image.Save(this.dlgSavePicture.FileName, IF);
	}
      }
    }

    private void mnuPCModeNormal_Click(object sender, System.EventArgs e) {
      this.mnuPCModeNormal.Checked    = true;
      this.mnuPCModeCentered.Checked  = false;
      this.mnuPCModeStretched.Checked = false;
      this.SetPictureSizeMode(this.GetSourcePicture(sender as MenuItem), PictureBoxSizeMode.Normal);
    }

    private void mnuPCModeCentered_Click(object sender, System.EventArgs e) {
      this.mnuPCModeNormal.Checked    = false;
      this.mnuPCModeCentered.Checked  = true;
      this.mnuPCModeStretched.Checked = false;
      this.SetPictureSizeMode(this.GetSourcePicture(sender as MenuItem), PictureBoxSizeMode.CenterImage);
    }

    private void mnuPCModeStretched_Click(object sender, System.EventArgs e) {
      this.mnuPCModeNormal.Checked    = false;
      this.mnuPCModeCentered.Checked  = false;
      this.mnuPCModeStretched.Checked = true;
      this.SetPictureSizeMode(this.GetSourcePicture(sender as MenuItem), PictureBoxSizeMode.StretchImage);
    }

    private void mnuPCBackgroundBlack_Click(object sender, System.EventArgs e) {
      this.mnuPCBackgroundBlack.Checked       = true;
      this.mnuPCBackgroundWhite.Checked       = false;
      this.mnuPCBackgroundTransparent.Checked = false;
      this.SetPictureBackground(this.GetSourcePicture(sender as MenuItem), Color.Black);
    }

    private void mnuPCBackgroundWhite_Click(object sender, System.EventArgs e) {
      this.mnuPCBackgroundBlack.Checked       = false;
      this.mnuPCBackgroundWhite.Checked       = true;
      this.mnuPCBackgroundTransparent.Checked = false;
      this.SetPictureBackground(this.GetSourcePicture(sender as MenuItem), Color.White);
    }

    private void mnuPCBackgroundTransparent_Click(object sender, System.EventArgs e) {
      this.mnuPCBackgroundBlack.Checked       = false;
      this.mnuPCBackgroundWhite.Checked       = false;
      this.mnuPCBackgroundTransparent.Checked = true;
      this.SetPictureBackground(this.GetSourcePicture(sender as MenuItem), Color.Transparent);
    }

    private void mnuPCSaveAs_Click(object sender, System.EventArgs e) {
      this.SavePicture(this.GetSourcePicture(sender as MenuItem));
    }

    #endregion

    #endregion

    #region Item Data Viewer Events

    private void TExportItems() {
    PleaseWaitDialog PWD = new PleaseWaitDialog(I18N.GetText("Dialog:ExportItems"));
      try {
	Application.DoEvents();
	PWD.ShowDialog(this);
      } catch { PWD.Close(); }
    }

    private FFXIItem[] LoadedItems_ = null;

    private void btnExportItems_Click(object sender, System.EventArgs e) {
    ItemExporter IE = new ItemExporter(this.ieItemViewer.ChosenItemLanguage, this.ieItemViewer.ChosenItemType);
      if (IE.PrepareExport()) {
      Thread T = new Thread(new ThreadStart(this.TExportItems));
	T.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
	T.Start();
	Application.DoEvents();
	IE.DoExport(this.LoadedItems_);
	T.Abort();
	this.Activate();
      }
    }

    private void btnFindItems_Click(object sender, System.EventArgs e) {
      using (ItemFindDialog IFD = new ItemFindDialog(this.LoadedItems_)) {
	IFD.Language = this.ieItemViewer.ChosenItemLanguage;
	IFD.Type     = this.ieItemViewer.ChosenItemType;
	if (IFD.ShowDialog(this) == DialogResult.OK && IFD.SelectedItem != null)
	  this.cmbItems.SelectedItem = IFD.SelectedItem;
      }
    }

    private void cmbItems_SelectedIndexChanged(object sender, System.EventArgs e) {
      this.ieItemViewer.Item = this.cmbItems.SelectedItem as FFXIItem;
    }

    #endregion

    #region String Table Viewer Events

    private void CopyStringTableRow() {
    string FullText = String.Empty;
      foreach (ListViewItem LVI in this.lstEntries.SelectedItems) {
      string ItemText = String.Empty;
	foreach (ListViewItem.ListViewSubItem LVSI in LVI.SubItems) {
	  if (ItemText != String.Empty)
	    ItemText += '\t';
	  ItemText += LVSI.Text;
	}
	if (FullText != String.Empty)
	  FullText += '\n';
	FullText += ItemText;
      }
      if (FullText != String.Empty)
	Clipboard.SetDataObject(FullText);
    }

    private void lstEntries_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
      // Support Ctrl-Ins in addition to the Ctrl-C supported by the context menu
      if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Insert)
	this.CopyStringTableRow();
    }

    #region Context Menu Events

    private void mnuSTCCopyRow_Click(object sender, System.EventArgs e) {
      this.CopyStringTableRow();
    }

    private void StringTableCopyFieldMenuItem_Click(object sender, System.EventArgs e) {
    MenuItem MI = sender as MenuItem;
    string CopyText = String.Empty;
      foreach (ListViewItem LVI in this.lstEntries.SelectedItems) {
	if (CopyText != "")
	  CopyText += '\n';
	CopyText += LVI.SubItems[MI.Index].Text;
      }
      Clipboard.SetDataObject(CopyText);
    }


    private void mnuStringTableContext_Popup(object sender, System.EventArgs e) {
      if (this.lstEntries.SelectedItems.Count > 0) { // Set up sub-menu with all available columns
	this.mnuSTCCopyField.MenuItems.Clear();
	foreach (ColumnHeader CH in this.lstEntries.Columns)
	  this.mnuSTCCopyField.MenuItems.Add(CH.Index, new MenuItem(CH.Text, new EventHandler(this.StringTableCopyFieldMenuItem_Click)));
	this.mnuSTCCopyField.Enabled = true;
      }
      else
	this.mnuSTCCopyField.Enabled = false;
    }

    #endregion

    #endregion

    #region TreeView Events

    private void tvDataFiles_AfterCollapse(object sender, System.Windows.Forms.TreeViewEventArgs e) {
      if (e.Node.ImageIndex == 2)
	e.Node.ImageIndex = e.Node.SelectedImageIndex = 1;
    }

    private void tvDataFiles_AfterExpand(object sender, System.Windows.Forms.TreeViewEventArgs e) {
      if (e.Node.ImageIndex == 1)
	e.Node.ImageIndex = e.Node.SelectedImageIndex = 2;
    }

    private void tvDataFiles_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e) {
      this.ResetViewers();
    string FileName = this.tvDataFiles.SelectedNode.Tag as string;
      if (FileName != null && File.Exists(FileName)) {
	this.Enabled = false;
      FileScanDialog FSD = new FileScanDialog(FileName);
	if (FSD.ShowDialog(this) == DialogResult.OK) {
	  if (FSD.StringTableEntries.Count > 0) {
	    this.tabViewers.Visible = true;
	    this.tabViewers.TabPages.Add(this.tabViewerStringTable);
	    Application.DoEvents();
	    this.lstEntries.Select();
	    this.lstEntries.SmallImageList = null;
	  int i = 0;
	    foreach (Array A in FSD.StringTableEntries) {
	      if (i == 0) {
		this.lstEntries.Columns.Clear();
		this.lstEntries.Columns.Add(I18N.GetText("ColumnHeader:Entry"), 1, HorizontalAlignment.Center);
		foreach (ColumnHeader CH in A)
		  this.lstEntries.Columns.Add(CH);
	      }
	      else {
	      ListViewItem LVI = this.lstEntries.Items.Add(String.Format("{0:00000000}", i), i - 1);
		foreach (string S in A)
		  LVI.SubItems.Add(S);
	      }
	      if ((++i % 100) == 0)
		Application.DoEvents();
	    }
	    if (FSD.Images.Count == FSD.StringTableEntries.Count - 1) {
	    ImageList EntryIcons = new ImageList();
	      foreach (FFXIGraphic G in FSD.Images)
		EntryIcons.Images.Add(G.Bitmap);
	      this.lstEntries.SmallImageList = EntryIcons;
	    }
	    foreach (ColumnHeader CH in this.lstEntries.Columns) {
	      CH.Width = -1;
	      CH.Width += 2;
	    }
	    Application.DoEvents();
	  }
	  if (FSD.Items.Count > 0) {
	    this.tabViewers.Visible = true;
	    this.tabViewers.TabPages.Add(this.tabViewerItems);
	    this.LoadedItems_ = FSD.Items.ToArray(typeof(FFXIItem)) as FFXIItem[];
	    this.cmbItems.Select();
	    this.cmbItems.SelectedItem = null;
	    this.cmbItems.Items.AddRange(this.LoadedItems_);
	    this.cmbItems.SelectedIndex = 0;
	    Application.DoEvents();
	  }
	  if (FSD.Images.Count > 0) {
	    this.tabViewers.Visible = true;
	    this.tabViewers.TabPages.Add(this.tabViewerImages);
	    this.cmbImageChooser.Select();
	    this.cmbImageChooser.SelectedItem = null;
	    this.cmbImageChooser.Items.AddRange(FSD.Images.ToArray());
	    this.cmbImageChooser.SelectedIndex = 0;
	    Application.DoEvents();
	  }
	}
	if (!this.tabViewers.Visible)
	  this.pnlNoViewers.Visible = true;
	this.Enabled = true;
      }
    }

    private void tvDataFiles_BeforeExpand(object sender, System.Windows.Forms.TreeViewCancelEventArgs e) {
      if (e.Node.FirstNode != null && e.Node.FirstNode.Tag == e.Node) {
	e.Node.Nodes.Clear();
	if (e.Node.Parent == null) {
	  for (int i = 0; i < 0x200; ++i) {
	  string SubDir = String.Format("{0}", i);
	  string SubDirPath = Path.Combine(e.Node.Tag as string, SubDir);
	    if (Directory.Exists(SubDirPath)) {
	    TreeNode SubDirNode = e.Node.Nodes.Add(SubDir);
	      SubDirNode.ImageIndex = SubDirNode.SelectedImageIndex = 1;
	      SubDirNode.Tag = SubDirPath;
	      SubDirNode.Nodes.Add("<dummy>").Tag = SubDirNode;
	    }
	  }
	}
	else {
	  for (int i = 0; i < 0x80; ++i) {
	  string DataFile = String.Format("{0}.DAT", i);
	  string DataFilePath = Path.Combine(e.Node.Tag as string, DataFile);
	    if (File.Exists(DataFilePath)) {
	    TreeNode DataFileNode = e.Node.Nodes.Add(DataFile);
	      DataFileNode.ImageIndex = DataFileNode.SelectedImageIndex = 3;
	      DataFileNode.Tag = DataFilePath;
	    }
	  }
	}
      }
    }

    #endregion

    #region Main Menu Events

    private void InitializeROMMenus() {
    XmlDocument XD = new XmlDocument();
      try {
      Stream InfoData = this.GetType().Assembly.GetManifestResourceStream("ROMFileMappings.xml");
	if (InfoData != null) {
	  XD.Load(new XmlTextReader(InfoData));
	  InfoData.Close();
	}
      } catch { }
      if (XD.DocumentElement == null)
	return;
    XmlNodeList Categories = XD.DocumentElement.SelectNodes("./category");
    int MenuPos = 0;
      foreach (XmlNode XN in Categories) {
	if (XN is XmlElement)
	  this.mnuMain.MenuItems.Add(MenuPos++, new CategoryMenuItem(XN as XmlElement, new EventHandler(this.ROMMenuItem_Click)));
      }
    }

    private void SelectEntry(int App, int Dir, int File) {
      if (App < 0 || App > this.tvDataFiles.Nodes.Count)
	return;
    TreeNode AppNode = this.tvDataFiles.Nodes[App];
      AppNode.Expand();
    TreeNode DirNode = null;
    string DirNodeText = String.Format("{0}", Dir);
      foreach (TreeNode TN in AppNode.Nodes) {
	if (TN.Text == DirNodeText) {
	  DirNode = TN;
	  break;
	}
      }
      if (DirNode != null) {
	DirNode.Expand();
      TreeNode FileNode = null;
      string FileNodeText = String.Format("{0}.DAT", File);
	foreach (TreeNode TN in DirNode.Nodes) {
	  if (TN.Text == FileNodeText) {
	    FileNode = TN;
	    break;
	  }
	}
	if (FileNode != null)
	  this.tvDataFiles.SelectedNode = FileNode;
      }
    }

    private void ROMMenuItem_Click(object sender, System.EventArgs e) {
    ROMMenuItem RMI = sender as ROMMenuItem;
      if (RMI != null)
	this.SelectEntry(RMI.ROMApp, RMI.ROMDir, RMI.ROMFile);
    }

    private void mnuOFileTable_Click(object sender, System.EventArgs e) {
      using (FileTableDialog FTD = new FileTableDialog())
	FTD.ShowDialog(this);
      this.Activate();
    }

    private void mnuOSettings_Click(object sender, System.EventArgs e) {
      using (SettingsDialog SD = new SettingsDialog())
	SD.ShowDialog(this);
      this.Activate();
    }

    #endregion

    private void ieItemViewer_SizeChanged(object sender, System.EventArgs e) {
      if (this.Height < this.ieItemViewer.Height + 128)
	this.Height = this.ieItemViewer.Height + 128;
    }

  }

}
