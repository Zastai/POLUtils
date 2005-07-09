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

  public class MainWindow : System.Windows.Forms.Form {

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
    private System.Windows.Forms.MenuItem mnuSTCCopy;
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

    private System.ComponentModel.IContainer components;

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

    #endregion

    #region Item Data Viewer Events

    private FFXIItem[] LoadedItems_ = null;

    private void btnExportItems_Click(object sender, System.EventArgs e) {
    ItemExporter IE = new ItemExporter(this.ieItemViewer.ChosenItemLanguage, this.ieItemViewer.ChosenItemType);
      IE.DoExport(this.LoadedItems_);
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

    #endregion

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
	components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      this.components = new System.ComponentModel.Container();
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainWindow));
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
      this.mnuSTCCopy = new System.Windows.Forms.MenuItem();
      this.mnuMain = new System.Windows.Forms.MainMenu();
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
      this.lblImageChooser = new System.Windows.Forms.Label();
      this.cmbImageChooser = new System.Windows.Forms.ComboBox();
      this.tabViewerStringTable = new System.Windows.Forms.TabPage();
      this.lstEntries = new System.Windows.Forms.ListView();
      this.pnlNoViewers = new System.Windows.Forms.Panel();
      this.lblNoViewers = new System.Windows.Forms.Label();
      this.pnlViewerArea.SuspendLayout();
      this.tabViewers.SuspendLayout();
      this.tabViewerItems.SuspendLayout();
      this.grpMainItemActions.SuspendLayout();
      this.tabViewerImages.SuspendLayout();
      this.pnlImageChooser.SuspendLayout();
      this.tabViewerStringTable.SuspendLayout();
      this.pnlNoViewers.SuspendLayout();
      this.SuspendLayout();
      // 
      // tvDataFiles
      // 
      this.tvDataFiles.AccessibleDescription = resources.GetString("tvDataFiles.AccessibleDescription");
      this.tvDataFiles.AccessibleName = resources.GetString("tvDataFiles.AccessibleName");
      this.tvDataFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("tvDataFiles.Anchor")));
      this.tvDataFiles.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tvDataFiles.BackgroundImage")));
      this.tvDataFiles.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("tvDataFiles.Dock")));
      this.tvDataFiles.Enabled = ((bool)(resources.GetObject("tvDataFiles.Enabled")));
      this.tvDataFiles.Font = ((System.Drawing.Font)(resources.GetObject("tvDataFiles.Font")));
      this.tvDataFiles.HideSelection = false;
      this.tvDataFiles.ImageIndex = ((int)(resources.GetObject("tvDataFiles.ImageIndex")));
      this.tvDataFiles.ImageList = this.ilBrowserIcons;
      this.tvDataFiles.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("tvDataFiles.ImeMode")));
      this.tvDataFiles.Indent = ((int)(resources.GetObject("tvDataFiles.Indent")));
      this.tvDataFiles.ItemHeight = ((int)(resources.GetObject("tvDataFiles.ItemHeight")));
      this.tvDataFiles.Location = ((System.Drawing.Point)(resources.GetObject("tvDataFiles.Location")));
      this.tvDataFiles.Name = "tvDataFiles";
      this.tvDataFiles.PathSeparator = "/";
      this.tvDataFiles.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("tvDataFiles.RightToLeft")));
      this.tvDataFiles.SelectedImageIndex = ((int)(resources.GetObject("tvDataFiles.SelectedImageIndex")));
      this.tvDataFiles.Size = ((System.Drawing.Size)(resources.GetObject("tvDataFiles.Size")));
      this.tvDataFiles.TabIndex = ((int)(resources.GetObject("tvDataFiles.TabIndex")));
      this.tvDataFiles.Text = resources.GetString("tvDataFiles.Text");
      this.tvDataFiles.Visible = ((bool)(resources.GetObject("tvDataFiles.Visible")));
      this.tvDataFiles.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvDataFiles_AfterExpand);
      this.tvDataFiles.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tvDataFiles_AfterCollapse);
      this.tvDataFiles.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvDataFiles_AfterSelect);
      this.tvDataFiles.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvDataFiles_BeforeExpand);
      // 
      // ilBrowserIcons
      // 
      this.ilBrowserIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
      this.ilBrowserIcons.ImageSize = ((System.Drawing.Size)(resources.GetObject("ilBrowserIcons.ImageSize")));
      this.ilBrowserIcons.TransparentColor = System.Drawing.Color.Transparent;
      // 
      // splSplitter
      // 
      this.splSplitter.AccessibleDescription = resources.GetString("splSplitter.AccessibleDescription");
      this.splSplitter.AccessibleName = resources.GetString("splSplitter.AccessibleName");
      this.splSplitter.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("splSplitter.Anchor")));
      this.splSplitter.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("splSplitter.BackgroundImage")));
      this.splSplitter.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("splSplitter.Dock")));
      this.splSplitter.Enabled = ((bool)(resources.GetObject("splSplitter.Enabled")));
      this.splSplitter.Font = ((System.Drawing.Font)(resources.GetObject("splSplitter.Font")));
      this.splSplitter.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("splSplitter.ImeMode")));
      this.splSplitter.Location = ((System.Drawing.Point)(resources.GetObject("splSplitter.Location")));
      this.splSplitter.MinExtra = ((int)(resources.GetObject("splSplitter.MinExtra")));
      this.splSplitter.MinSize = ((int)(resources.GetObject("splSplitter.MinSize")));
      this.splSplitter.Name = "splSplitter";
      this.splSplitter.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("splSplitter.RightToLeft")));
      this.splSplitter.Size = ((System.Drawing.Size)(resources.GetObject("splSplitter.Size")));
      this.splSplitter.TabIndex = ((int)(resources.GetObject("splSplitter.TabIndex")));
      this.splSplitter.TabStop = false;
      this.splSplitter.Visible = ((bool)(resources.GetObject("splSplitter.Visible")));
      // 
      // mnuPictureContext
      // 
      this.mnuPictureContext.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
										      this.mnuPCMode,
										      this.mnuPCBackground,
										      this.mnuPCSaveAs});
      this.mnuPictureContext.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mnuPictureContext.RightToLeft")));
      // 
      // mnuPCMode
      // 
      this.mnuPCMode.Enabled = ((bool)(resources.GetObject("mnuPCMode.Enabled")));
      this.mnuPCMode.Index = 0;
      this.mnuPCMode.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
									      this.mnuPCModeNormal,
									      this.mnuPCModeCentered,
									      this.mnuPCModeStretched});
      this.mnuPCMode.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuPCMode.Shortcut")));
      this.mnuPCMode.ShowShortcut = ((bool)(resources.GetObject("mnuPCMode.ShowShortcut")));
      this.mnuPCMode.Text = resources.GetString("mnuPCMode.Text");
      this.mnuPCMode.Visible = ((bool)(resources.GetObject("mnuPCMode.Visible")));
      // 
      // mnuPCModeNormal
      // 
      this.mnuPCModeNormal.Checked = true;
      this.mnuPCModeNormal.Enabled = ((bool)(resources.GetObject("mnuPCModeNormal.Enabled")));
      this.mnuPCModeNormal.Index = 0;
      this.mnuPCModeNormal.RadioCheck = true;
      this.mnuPCModeNormal.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuPCModeNormal.Shortcut")));
      this.mnuPCModeNormal.ShowShortcut = ((bool)(resources.GetObject("mnuPCModeNormal.ShowShortcut")));
      this.mnuPCModeNormal.Text = resources.GetString("mnuPCModeNormal.Text");
      this.mnuPCModeNormal.Visible = ((bool)(resources.GetObject("mnuPCModeNormal.Visible")));
      this.mnuPCModeNormal.Click += new System.EventHandler(this.mnuPCModeNormal_Click);
      // 
      // mnuPCModeCentered
      // 
      this.mnuPCModeCentered.Enabled = ((bool)(resources.GetObject("mnuPCModeCentered.Enabled")));
      this.mnuPCModeCentered.Index = 1;
      this.mnuPCModeCentered.RadioCheck = true;
      this.mnuPCModeCentered.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuPCModeCentered.Shortcut")));
      this.mnuPCModeCentered.ShowShortcut = ((bool)(resources.GetObject("mnuPCModeCentered.ShowShortcut")));
      this.mnuPCModeCentered.Text = resources.GetString("mnuPCModeCentered.Text");
      this.mnuPCModeCentered.Visible = ((bool)(resources.GetObject("mnuPCModeCentered.Visible")));
      this.mnuPCModeCentered.Click += new System.EventHandler(this.mnuPCModeCentered_Click);
      // 
      // mnuPCModeStretched
      // 
      this.mnuPCModeStretched.Enabled = ((bool)(resources.GetObject("mnuPCModeStretched.Enabled")));
      this.mnuPCModeStretched.Index = 2;
      this.mnuPCModeStretched.RadioCheck = true;
      this.mnuPCModeStretched.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuPCModeStretched.Shortcut")));
      this.mnuPCModeStretched.ShowShortcut = ((bool)(resources.GetObject("mnuPCModeStretched.ShowShortcut")));
      this.mnuPCModeStretched.Text = resources.GetString("mnuPCModeStretched.Text");
      this.mnuPCModeStretched.Visible = ((bool)(resources.GetObject("mnuPCModeStretched.Visible")));
      this.mnuPCModeStretched.Click += new System.EventHandler(this.mnuPCModeStretched_Click);
      // 
      // mnuPCBackground
      // 
      this.mnuPCBackground.Enabled = ((bool)(resources.GetObject("mnuPCBackground.Enabled")));
      this.mnuPCBackground.Index = 1;
      this.mnuPCBackground.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
										    this.mnuPCBackgroundBlack,
										    this.mnuPCBackgroundWhite,
										    this.mnuPCBackgroundTransparent});
      this.mnuPCBackground.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuPCBackground.Shortcut")));
      this.mnuPCBackground.ShowShortcut = ((bool)(resources.GetObject("mnuPCBackground.ShowShortcut")));
      this.mnuPCBackground.Text = resources.GetString("mnuPCBackground.Text");
      this.mnuPCBackground.Visible = ((bool)(resources.GetObject("mnuPCBackground.Visible")));
      // 
      // mnuPCBackgroundBlack
      // 
      this.mnuPCBackgroundBlack.Checked = true;
      this.mnuPCBackgroundBlack.Enabled = ((bool)(resources.GetObject("mnuPCBackgroundBlack.Enabled")));
      this.mnuPCBackgroundBlack.Index = 0;
      this.mnuPCBackgroundBlack.RadioCheck = true;
      this.mnuPCBackgroundBlack.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuPCBackgroundBlack.Shortcut")));
      this.mnuPCBackgroundBlack.ShowShortcut = ((bool)(resources.GetObject("mnuPCBackgroundBlack.ShowShortcut")));
      this.mnuPCBackgroundBlack.Text = resources.GetString("mnuPCBackgroundBlack.Text");
      this.mnuPCBackgroundBlack.Visible = ((bool)(resources.GetObject("mnuPCBackgroundBlack.Visible")));
      this.mnuPCBackgroundBlack.Click += new System.EventHandler(this.mnuPCBackgroundBlack_Click);
      // 
      // mnuPCBackgroundWhite
      // 
      this.mnuPCBackgroundWhite.Enabled = ((bool)(resources.GetObject("mnuPCBackgroundWhite.Enabled")));
      this.mnuPCBackgroundWhite.Index = 1;
      this.mnuPCBackgroundWhite.RadioCheck = true;
      this.mnuPCBackgroundWhite.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuPCBackgroundWhite.Shortcut")));
      this.mnuPCBackgroundWhite.ShowShortcut = ((bool)(resources.GetObject("mnuPCBackgroundWhite.ShowShortcut")));
      this.mnuPCBackgroundWhite.Text = resources.GetString("mnuPCBackgroundWhite.Text");
      this.mnuPCBackgroundWhite.Visible = ((bool)(resources.GetObject("mnuPCBackgroundWhite.Visible")));
      this.mnuPCBackgroundWhite.Click += new System.EventHandler(this.mnuPCBackgroundWhite_Click);
      // 
      // mnuPCBackgroundTransparent
      // 
      this.mnuPCBackgroundTransparent.Enabled = ((bool)(resources.GetObject("mnuPCBackgroundTransparent.Enabled")));
      this.mnuPCBackgroundTransparent.Index = 2;
      this.mnuPCBackgroundTransparent.RadioCheck = true;
      this.mnuPCBackgroundTransparent.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuPCBackgroundTransparent.Shortcut")));
      this.mnuPCBackgroundTransparent.ShowShortcut = ((bool)(resources.GetObject("mnuPCBackgroundTransparent.ShowShortcut")));
      this.mnuPCBackgroundTransparent.Text = resources.GetString("mnuPCBackgroundTransparent.Text");
      this.mnuPCBackgroundTransparent.Visible = ((bool)(resources.GetObject("mnuPCBackgroundTransparent.Visible")));
      this.mnuPCBackgroundTransparent.Click += new System.EventHandler(this.mnuPCBackgroundTransparent_Click);
      // 
      // mnuPCSaveAs
      // 
      this.mnuPCSaveAs.Enabled = ((bool)(resources.GetObject("mnuPCSaveAs.Enabled")));
      this.mnuPCSaveAs.Index = 2;
      this.mnuPCSaveAs.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuPCSaveAs.Shortcut")));
      this.mnuPCSaveAs.ShowShortcut = ((bool)(resources.GetObject("mnuPCSaveAs.ShowShortcut")));
      this.mnuPCSaveAs.Text = resources.GetString("mnuPCSaveAs.Text");
      this.mnuPCSaveAs.Visible = ((bool)(resources.GetObject("mnuPCSaveAs.Visible")));
      this.mnuPCSaveAs.Click += new System.EventHandler(this.mnuPCSaveAs_Click);
      // 
      // dlgSavePicture
      // 
      this.dlgSavePicture.Filter = resources.GetString("dlgSavePicture.Filter");
      this.dlgSavePicture.Title = resources.GetString("dlgSavePicture.Title");
      // 
      // mnuStringTableContext
      // 
      this.mnuStringTableContext.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
											  this.mnuSTCCopy});
      this.mnuStringTableContext.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mnuStringTableContext.RightToLeft")));
      // 
      // mnuSTCCopy
      // 
      this.mnuSTCCopy.Enabled = ((bool)(resources.GetObject("mnuSTCCopy.Enabled")));
      this.mnuSTCCopy.Index = 0;
      this.mnuSTCCopy.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuSTCCopy.Shortcut")));
      this.mnuSTCCopy.ShowShortcut = ((bool)(resources.GetObject("mnuSTCCopy.ShowShortcut")));
      this.mnuSTCCopy.Text = resources.GetString("mnuSTCCopy.Text");
      this.mnuSTCCopy.Visible = ((bool)(resources.GetObject("mnuSTCCopy.Visible")));
      this.mnuSTCCopy.Click += new System.EventHandler(this.mnuSTCCopy_Click);
      // 
      // mnuMain
      // 
      this.mnuMain.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
									    this.mnuWindows});
      this.mnuMain.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mnuMain.RightToLeft")));
      // 
      // mnuWindows
      // 
      this.mnuWindows.Enabled = ((bool)(resources.GetObject("mnuWindows.Enabled")));
      this.mnuWindows.Index = 0;
      this.mnuWindows.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
									       this.mnuOFileTable,
									       this.mnuOSettings});
      this.mnuWindows.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuWindows.Shortcut")));
      this.mnuWindows.ShowShortcut = ((bool)(resources.GetObject("mnuWindows.ShowShortcut")));
      this.mnuWindows.Text = resources.GetString("mnuWindows.Text");
      this.mnuWindows.Visible = ((bool)(resources.GetObject("mnuWindows.Visible")));
      // 
      // mnuOFileTable
      // 
      this.mnuOFileTable.Enabled = ((bool)(resources.GetObject("mnuOFileTable.Enabled")));
      this.mnuOFileTable.Index = 0;
      this.mnuOFileTable.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuOFileTable.Shortcut")));
      this.mnuOFileTable.ShowShortcut = ((bool)(resources.GetObject("mnuOFileTable.ShowShortcut")));
      this.mnuOFileTable.Text = resources.GetString("mnuOFileTable.Text");
      this.mnuOFileTable.Visible = ((bool)(resources.GetObject("mnuOFileTable.Visible")));
      this.mnuOFileTable.Click += new System.EventHandler(this.mnuOFileTable_Click);
      // 
      // mnuOSettings
      // 
      this.mnuOSettings.Enabled = ((bool)(resources.GetObject("mnuOSettings.Enabled")));
      this.mnuOSettings.Index = 1;
      this.mnuOSettings.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuOSettings.Shortcut")));
      this.mnuOSettings.ShowShortcut = ((bool)(resources.GetObject("mnuOSettings.ShowShortcut")));
      this.mnuOSettings.Text = resources.GetString("mnuOSettings.Text");
      this.mnuOSettings.Visible = ((bool)(resources.GetObject("mnuOSettings.Visible")));
      this.mnuOSettings.Click += new System.EventHandler(this.mnuOSettings_Click);
      // 
      // pnlViewerArea
      // 
      this.pnlViewerArea.AccessibleDescription = resources.GetString("pnlViewerArea.AccessibleDescription");
      this.pnlViewerArea.AccessibleName = resources.GetString("pnlViewerArea.AccessibleName");
      this.pnlViewerArea.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("pnlViewerArea.Anchor")));
      this.pnlViewerArea.AutoScroll = ((bool)(resources.GetObject("pnlViewerArea.AutoScroll")));
      this.pnlViewerArea.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("pnlViewerArea.AutoScrollMargin")));
      this.pnlViewerArea.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("pnlViewerArea.AutoScrollMinSize")));
      this.pnlViewerArea.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlViewerArea.BackgroundImage")));
      this.pnlViewerArea.Controls.Add(this.tabViewers);
      this.pnlViewerArea.Controls.Add(this.pnlNoViewers);
      this.pnlViewerArea.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("pnlViewerArea.Dock")));
      this.pnlViewerArea.Enabled = ((bool)(resources.GetObject("pnlViewerArea.Enabled")));
      this.pnlViewerArea.Font = ((System.Drawing.Font)(resources.GetObject("pnlViewerArea.Font")));
      this.pnlViewerArea.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("pnlViewerArea.ImeMode")));
      this.pnlViewerArea.Location = ((System.Drawing.Point)(resources.GetObject("pnlViewerArea.Location")));
      this.pnlViewerArea.Name = "pnlViewerArea";
      this.pnlViewerArea.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("pnlViewerArea.RightToLeft")));
      this.pnlViewerArea.Size = ((System.Drawing.Size)(resources.GetObject("pnlViewerArea.Size")));
      this.pnlViewerArea.TabIndex = ((int)(resources.GetObject("pnlViewerArea.TabIndex")));
      this.pnlViewerArea.Text = resources.GetString("pnlViewerArea.Text");
      this.pnlViewerArea.Visible = ((bool)(resources.GetObject("pnlViewerArea.Visible")));
      // 
      // tabViewers
      // 
      this.tabViewers.AccessibleDescription = resources.GetString("tabViewers.AccessibleDescription");
      this.tabViewers.AccessibleName = resources.GetString("tabViewers.AccessibleName");
      this.tabViewers.Alignment = ((System.Windows.Forms.TabAlignment)(resources.GetObject("tabViewers.Alignment")));
      this.tabViewers.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("tabViewers.Anchor")));
      this.tabViewers.Appearance = ((System.Windows.Forms.TabAppearance)(resources.GetObject("tabViewers.Appearance")));
      this.tabViewers.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tabViewers.BackgroundImage")));
      this.tabViewers.Controls.Add(this.tabViewerItems);
      this.tabViewers.Controls.Add(this.tabViewerImages);
      this.tabViewers.Controls.Add(this.tabViewerStringTable);
      this.tabViewers.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("tabViewers.Dock")));
      this.tabViewers.Enabled = ((bool)(resources.GetObject("tabViewers.Enabled")));
      this.tabViewers.Font = ((System.Drawing.Font)(resources.GetObject("tabViewers.Font")));
      this.tabViewers.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("tabViewers.ImeMode")));
      this.tabViewers.ItemSize = ((System.Drawing.Size)(resources.GetObject("tabViewers.ItemSize")));
      this.tabViewers.Location = ((System.Drawing.Point)(resources.GetObject("tabViewers.Location")));
      this.tabViewers.Name = "tabViewers";
      this.tabViewers.Padding = ((System.Drawing.Point)(resources.GetObject("tabViewers.Padding")));
      this.tabViewers.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("tabViewers.RightToLeft")));
      this.tabViewers.SelectedIndex = 0;
      this.tabViewers.ShowToolTips = ((bool)(resources.GetObject("tabViewers.ShowToolTips")));
      this.tabViewers.Size = ((System.Drawing.Size)(resources.GetObject("tabViewers.Size")));
      this.tabViewers.TabIndex = ((int)(resources.GetObject("tabViewers.TabIndex")));
      this.tabViewers.Text = resources.GetString("tabViewers.Text");
      this.tabViewers.Visible = ((bool)(resources.GetObject("tabViewers.Visible")));
      // 
      // tabViewerItems
      // 
      this.tabViewerItems.AccessibleDescription = resources.GetString("tabViewerItems.AccessibleDescription");
      this.tabViewerItems.AccessibleName = resources.GetString("tabViewerItems.AccessibleName");
      this.tabViewerItems.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("tabViewerItems.Anchor")));
      this.tabViewerItems.AutoScroll = ((bool)(resources.GetObject("tabViewerItems.AutoScroll")));
      this.tabViewerItems.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("tabViewerItems.AutoScrollMargin")));
      this.tabViewerItems.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("tabViewerItems.AutoScrollMinSize")));
      this.tabViewerItems.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tabViewerItems.BackgroundImage")));
      this.tabViewerItems.Controls.Add(this.ieItemViewer);
      this.tabViewerItems.Controls.Add(this.grpMainItemActions);
      this.tabViewerItems.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("tabViewerItems.Dock")));
      this.tabViewerItems.Enabled = ((bool)(resources.GetObject("tabViewerItems.Enabled")));
      this.tabViewerItems.Font = ((System.Drawing.Font)(resources.GetObject("tabViewerItems.Font")));
      this.tabViewerItems.ImageIndex = ((int)(resources.GetObject("tabViewerItems.ImageIndex")));
      this.tabViewerItems.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("tabViewerItems.ImeMode")));
      this.tabViewerItems.Location = ((System.Drawing.Point)(resources.GetObject("tabViewerItems.Location")));
      this.tabViewerItems.Name = "tabViewerItems";
      this.tabViewerItems.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("tabViewerItems.RightToLeft")));
      this.tabViewerItems.Size = ((System.Drawing.Size)(resources.GetObject("tabViewerItems.Size")));
      this.tabViewerItems.TabIndex = ((int)(resources.GetObject("tabViewerItems.TabIndex")));
      this.tabViewerItems.Text = resources.GetString("tabViewerItems.Text");
      this.tabViewerItems.ToolTipText = resources.GetString("tabViewerItems.ToolTipText");
      this.tabViewerItems.Visible = ((bool)(resources.GetObject("tabViewerItems.Visible")));
      // 
      // ieItemViewer
      // 
      this.ieItemViewer.AccessibleDescription = resources.GetString("ieItemViewer.AccessibleDescription");
      this.ieItemViewer.AccessibleName = resources.GetString("ieItemViewer.AccessibleName");
      this.ieItemViewer.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("ieItemViewer.Anchor")));
      this.ieItemViewer.AutoScroll = ((bool)(resources.GetObject("ieItemViewer.AutoScroll")));
      this.ieItemViewer.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("ieItemViewer.AutoScrollMargin")));
      this.ieItemViewer.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("ieItemViewer.AutoScrollMinSize")));
      this.ieItemViewer.BackColor = System.Drawing.SystemColors.Control;
      this.ieItemViewer.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ieItemViewer.BackgroundImage")));
      this.ieItemViewer.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("ieItemViewer.Dock")));
      this.ieItemViewer.Enabled = ((bool)(resources.GetObject("ieItemViewer.Enabled")));
      this.ieItemViewer.Font = ((System.Drawing.Font)(resources.GetObject("ieItemViewer.Font")));
      this.ieItemViewer.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("ieItemViewer.ImeMode")));
      this.ieItemViewer.Item = null;
      this.ieItemViewer.Location = ((System.Drawing.Point)(resources.GetObject("ieItemViewer.Location")));
      this.ieItemViewer.Name = "ieItemViewer";
      this.ieItemViewer.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("ieItemViewer.RightToLeft")));
      this.ieItemViewer.Size = ((System.Drawing.Size)(resources.GetObject("ieItemViewer.Size")));
      this.ieItemViewer.TabIndex = ((int)(resources.GetObject("ieItemViewer.TabIndex")));
      this.ieItemViewer.Visible = ((bool)(resources.GetObject("ieItemViewer.Visible")));
      // 
      // grpMainItemActions
      // 
      this.grpMainItemActions.AccessibleDescription = resources.GetString("grpMainItemActions.AccessibleDescription");
      this.grpMainItemActions.AccessibleName = resources.GetString("grpMainItemActions.AccessibleName");
      this.grpMainItemActions.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("grpMainItemActions.Anchor")));
      this.grpMainItemActions.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpMainItemActions.BackgroundImage")));
      this.grpMainItemActions.Controls.Add(this.cmbItems);
      this.grpMainItemActions.Controls.Add(this.btnFindItems);
      this.grpMainItemActions.Controls.Add(this.btnExportItems);
      this.grpMainItemActions.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("grpMainItemActions.Dock")));
      this.grpMainItemActions.Enabled = ((bool)(resources.GetObject("grpMainItemActions.Enabled")));
      this.grpMainItemActions.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.grpMainItemActions.Font = ((System.Drawing.Font)(resources.GetObject("grpMainItemActions.Font")));
      this.grpMainItemActions.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("grpMainItemActions.ImeMode")));
      this.grpMainItemActions.Location = ((System.Drawing.Point)(resources.GetObject("grpMainItemActions.Location")));
      this.grpMainItemActions.Name = "grpMainItemActions";
      this.grpMainItemActions.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("grpMainItemActions.RightToLeft")));
      this.grpMainItemActions.Size = ((System.Drawing.Size)(resources.GetObject("grpMainItemActions.Size")));
      this.grpMainItemActions.TabIndex = ((int)(resources.GetObject("grpMainItemActions.TabIndex")));
      this.grpMainItemActions.TabStop = false;
      this.grpMainItemActions.Text = resources.GetString("grpMainItemActions.Text");
      this.grpMainItemActions.Visible = ((bool)(resources.GetObject("grpMainItemActions.Visible")));
      // 
      // cmbItems
      // 
      this.cmbItems.AccessibleDescription = resources.GetString("cmbItems.AccessibleDescription");
      this.cmbItems.AccessibleName = resources.GetString("cmbItems.AccessibleName");
      this.cmbItems.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("cmbItems.Anchor")));
      this.cmbItems.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("cmbItems.BackgroundImage")));
      this.cmbItems.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("cmbItems.Dock")));
      this.cmbItems.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbItems.Enabled = ((bool)(resources.GetObject("cmbItems.Enabled")));
      this.cmbItems.Font = ((System.Drawing.Font)(resources.GetObject("cmbItems.Font")));
      this.cmbItems.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("cmbItems.ImeMode")));
      this.cmbItems.IntegralHeight = ((bool)(resources.GetObject("cmbItems.IntegralHeight")));
      this.cmbItems.ItemHeight = ((int)(resources.GetObject("cmbItems.ItemHeight")));
      this.cmbItems.Location = ((System.Drawing.Point)(resources.GetObject("cmbItems.Location")));
      this.cmbItems.MaxDropDownItems = ((int)(resources.GetObject("cmbItems.MaxDropDownItems")));
      this.cmbItems.MaxLength = ((int)(resources.GetObject("cmbItems.MaxLength")));
      this.cmbItems.Name = "cmbItems";
      this.cmbItems.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("cmbItems.RightToLeft")));
      this.cmbItems.Size = ((System.Drawing.Size)(resources.GetObject("cmbItems.Size")));
      this.cmbItems.TabIndex = ((int)(resources.GetObject("cmbItems.TabIndex")));
      this.cmbItems.Text = resources.GetString("cmbItems.Text");
      this.cmbItems.Visible = ((bool)(resources.GetObject("cmbItems.Visible")));
      this.cmbItems.SelectedIndexChanged += new System.EventHandler(this.cmbItems_SelectedIndexChanged);
      // 
      // btnFindItems
      // 
      this.btnFindItems.AccessibleDescription = resources.GetString("btnFindItems.AccessibleDescription");
      this.btnFindItems.AccessibleName = resources.GetString("btnFindItems.AccessibleName");
      this.btnFindItems.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnFindItems.Anchor")));
      this.btnFindItems.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFindItems.BackgroundImage")));
      this.btnFindItems.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnFindItems.Dock")));
      this.btnFindItems.Enabled = ((bool)(resources.GetObject("btnFindItems.Enabled")));
      this.btnFindItems.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnFindItems.FlatStyle")));
      this.btnFindItems.Font = ((System.Drawing.Font)(resources.GetObject("btnFindItems.Font")));
      this.btnFindItems.Image = ((System.Drawing.Image)(resources.GetObject("btnFindItems.Image")));
      this.btnFindItems.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnFindItems.ImageAlign")));
      this.btnFindItems.ImageIndex = ((int)(resources.GetObject("btnFindItems.ImageIndex")));
      this.btnFindItems.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnFindItems.ImeMode")));
      this.btnFindItems.Location = ((System.Drawing.Point)(resources.GetObject("btnFindItems.Location")));
      this.btnFindItems.Name = "btnFindItems";
      this.btnFindItems.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnFindItems.RightToLeft")));
      this.btnFindItems.Size = ((System.Drawing.Size)(resources.GetObject("btnFindItems.Size")));
      this.btnFindItems.TabIndex = ((int)(resources.GetObject("btnFindItems.TabIndex")));
      this.btnFindItems.Text = resources.GetString("btnFindItems.Text");
      this.btnFindItems.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnFindItems.TextAlign")));
      this.btnFindItems.Visible = ((bool)(resources.GetObject("btnFindItems.Visible")));
      this.btnFindItems.Click += new System.EventHandler(this.btnFindItems_Click);
      // 
      // btnExportItems
      // 
      this.btnExportItems.AccessibleDescription = resources.GetString("btnExportItems.AccessibleDescription");
      this.btnExportItems.AccessibleName = resources.GetString("btnExportItems.AccessibleName");
      this.btnExportItems.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnExportItems.Anchor")));
      this.btnExportItems.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnExportItems.BackgroundImage")));
      this.btnExportItems.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnExportItems.Dock")));
      this.btnExportItems.Enabled = ((bool)(resources.GetObject("btnExportItems.Enabled")));
      this.btnExportItems.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnExportItems.FlatStyle")));
      this.btnExportItems.Font = ((System.Drawing.Font)(resources.GetObject("btnExportItems.Font")));
      this.btnExportItems.Image = ((System.Drawing.Image)(resources.GetObject("btnExportItems.Image")));
      this.btnExportItems.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnExportItems.ImageAlign")));
      this.btnExportItems.ImageIndex = ((int)(resources.GetObject("btnExportItems.ImageIndex")));
      this.btnExportItems.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnExportItems.ImeMode")));
      this.btnExportItems.Location = ((System.Drawing.Point)(resources.GetObject("btnExportItems.Location")));
      this.btnExportItems.Name = "btnExportItems";
      this.btnExportItems.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnExportItems.RightToLeft")));
      this.btnExportItems.Size = ((System.Drawing.Size)(resources.GetObject("btnExportItems.Size")));
      this.btnExportItems.TabIndex = ((int)(resources.GetObject("btnExportItems.TabIndex")));
      this.btnExportItems.Text = resources.GetString("btnExportItems.Text");
      this.btnExportItems.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnExportItems.TextAlign")));
      this.btnExportItems.Visible = ((bool)(resources.GetObject("btnExportItems.Visible")));
      this.btnExportItems.Click += new System.EventHandler(this.btnExportItems_Click);
      // 
      // tabViewerImages
      // 
      this.tabViewerImages.AccessibleDescription = resources.GetString("tabViewerImages.AccessibleDescription");
      this.tabViewerImages.AccessibleName = resources.GetString("tabViewerImages.AccessibleName");
      this.tabViewerImages.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("tabViewerImages.Anchor")));
      this.tabViewerImages.AutoScroll = ((bool)(resources.GetObject("tabViewerImages.AutoScroll")));
      this.tabViewerImages.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("tabViewerImages.AutoScrollMargin")));
      this.tabViewerImages.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("tabViewerImages.AutoScrollMinSize")));
      this.tabViewerImages.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tabViewerImages.BackgroundImage")));
      this.tabViewerImages.Controls.Add(this.picImageViewer);
      this.tabViewerImages.Controls.Add(this.pnlImageChooser);
      this.tabViewerImages.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("tabViewerImages.Dock")));
      this.tabViewerImages.Enabled = ((bool)(resources.GetObject("tabViewerImages.Enabled")));
      this.tabViewerImages.Font = ((System.Drawing.Font)(resources.GetObject("tabViewerImages.Font")));
      this.tabViewerImages.ImageIndex = ((int)(resources.GetObject("tabViewerImages.ImageIndex")));
      this.tabViewerImages.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("tabViewerImages.ImeMode")));
      this.tabViewerImages.Location = ((System.Drawing.Point)(resources.GetObject("tabViewerImages.Location")));
      this.tabViewerImages.Name = "tabViewerImages";
      this.tabViewerImages.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("tabViewerImages.RightToLeft")));
      this.tabViewerImages.Size = ((System.Drawing.Size)(resources.GetObject("tabViewerImages.Size")));
      this.tabViewerImages.TabIndex = ((int)(resources.GetObject("tabViewerImages.TabIndex")));
      this.tabViewerImages.Text = resources.GetString("tabViewerImages.Text");
      this.tabViewerImages.ToolTipText = resources.GetString("tabViewerImages.ToolTipText");
      this.tabViewerImages.Visible = ((bool)(resources.GetObject("tabViewerImages.Visible")));
      // 
      // picImageViewer
      // 
      this.picImageViewer.AccessibleDescription = resources.GetString("picImageViewer.AccessibleDescription");
      this.picImageViewer.AccessibleName = resources.GetString("picImageViewer.AccessibleName");
      this.picImageViewer.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("picImageViewer.Anchor")));
      this.picImageViewer.BackColor = System.Drawing.Color.Black;
      this.picImageViewer.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picImageViewer.BackgroundImage")));
      this.picImageViewer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.picImageViewer.ContextMenu = this.mnuPictureContext;
      this.picImageViewer.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("picImageViewer.Dock")));
      this.picImageViewer.Enabled = ((bool)(resources.GetObject("picImageViewer.Enabled")));
      this.picImageViewer.Font = ((System.Drawing.Font)(resources.GetObject("picImageViewer.Font")));
      this.picImageViewer.Image = ((System.Drawing.Image)(resources.GetObject("picImageViewer.Image")));
      this.picImageViewer.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("picImageViewer.ImeMode")));
      this.picImageViewer.Location = ((System.Drawing.Point)(resources.GetObject("picImageViewer.Location")));
      this.picImageViewer.Name = "picImageViewer";
      this.picImageViewer.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("picImageViewer.RightToLeft")));
      this.picImageViewer.Size = ((System.Drawing.Size)(resources.GetObject("picImageViewer.Size")));
      this.picImageViewer.SizeMode = ((System.Windows.Forms.PictureBoxSizeMode)(resources.GetObject("picImageViewer.SizeMode")));
      this.picImageViewer.TabIndex = ((int)(resources.GetObject("picImageViewer.TabIndex")));
      this.picImageViewer.TabStop = false;
      this.picImageViewer.Text = resources.GetString("picImageViewer.Text");
      this.picImageViewer.Visible = ((bool)(resources.GetObject("picImageViewer.Visible")));
      // 
      // pnlImageChooser
      // 
      this.pnlImageChooser.AccessibleDescription = resources.GetString("pnlImageChooser.AccessibleDescription");
      this.pnlImageChooser.AccessibleName = resources.GetString("pnlImageChooser.AccessibleName");
      this.pnlImageChooser.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("pnlImageChooser.Anchor")));
      this.pnlImageChooser.AutoScroll = ((bool)(resources.GetObject("pnlImageChooser.AutoScroll")));
      this.pnlImageChooser.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("pnlImageChooser.AutoScrollMargin")));
      this.pnlImageChooser.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("pnlImageChooser.AutoScrollMinSize")));
      this.pnlImageChooser.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlImageChooser.BackgroundImage")));
      this.pnlImageChooser.Controls.Add(this.lblImageChooser);
      this.pnlImageChooser.Controls.Add(this.cmbImageChooser);
      this.pnlImageChooser.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("pnlImageChooser.Dock")));
      this.pnlImageChooser.Enabled = ((bool)(resources.GetObject("pnlImageChooser.Enabled")));
      this.pnlImageChooser.Font = ((System.Drawing.Font)(resources.GetObject("pnlImageChooser.Font")));
      this.pnlImageChooser.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("pnlImageChooser.ImeMode")));
      this.pnlImageChooser.Location = ((System.Drawing.Point)(resources.GetObject("pnlImageChooser.Location")));
      this.pnlImageChooser.Name = "pnlImageChooser";
      this.pnlImageChooser.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("pnlImageChooser.RightToLeft")));
      this.pnlImageChooser.Size = ((System.Drawing.Size)(resources.GetObject("pnlImageChooser.Size")));
      this.pnlImageChooser.TabIndex = ((int)(resources.GetObject("pnlImageChooser.TabIndex")));
      this.pnlImageChooser.Text = resources.GetString("pnlImageChooser.Text");
      this.pnlImageChooser.Visible = ((bool)(resources.GetObject("pnlImageChooser.Visible")));
      // 
      // lblImageChooser
      // 
      this.lblImageChooser.AccessibleDescription = resources.GetString("lblImageChooser.AccessibleDescription");
      this.lblImageChooser.AccessibleName = resources.GetString("lblImageChooser.AccessibleName");
      this.lblImageChooser.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblImageChooser.Anchor")));
      this.lblImageChooser.AutoSize = ((bool)(resources.GetObject("lblImageChooser.AutoSize")));
      this.lblImageChooser.BackColor = System.Drawing.Color.Transparent;
      this.lblImageChooser.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblImageChooser.Dock")));
      this.lblImageChooser.Enabled = ((bool)(resources.GetObject("lblImageChooser.Enabled")));
      this.lblImageChooser.Font = ((System.Drawing.Font)(resources.GetObject("lblImageChooser.Font")));
      this.lblImageChooser.Image = ((System.Drawing.Image)(resources.GetObject("lblImageChooser.Image")));
      this.lblImageChooser.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblImageChooser.ImageAlign")));
      this.lblImageChooser.ImageIndex = ((int)(resources.GetObject("lblImageChooser.ImageIndex")));
      this.lblImageChooser.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblImageChooser.ImeMode")));
      this.lblImageChooser.Location = ((System.Drawing.Point)(resources.GetObject("lblImageChooser.Location")));
      this.lblImageChooser.Name = "lblImageChooser";
      this.lblImageChooser.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblImageChooser.RightToLeft")));
      this.lblImageChooser.Size = ((System.Drawing.Size)(resources.GetObject("lblImageChooser.Size")));
      this.lblImageChooser.TabIndex = ((int)(resources.GetObject("lblImageChooser.TabIndex")));
      this.lblImageChooser.Text = resources.GetString("lblImageChooser.Text");
      this.lblImageChooser.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblImageChooser.TextAlign")));
      this.lblImageChooser.Visible = ((bool)(resources.GetObject("lblImageChooser.Visible")));
      // 
      // cmbImageChooser
      // 
      this.cmbImageChooser.AccessibleDescription = resources.GetString("cmbImageChooser.AccessibleDescription");
      this.cmbImageChooser.AccessibleName = resources.GetString("cmbImageChooser.AccessibleName");
      this.cmbImageChooser.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("cmbImageChooser.Anchor")));
      this.cmbImageChooser.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("cmbImageChooser.BackgroundImage")));
      this.cmbImageChooser.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("cmbImageChooser.Dock")));
      this.cmbImageChooser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbImageChooser.Enabled = ((bool)(resources.GetObject("cmbImageChooser.Enabled")));
      this.cmbImageChooser.Font = ((System.Drawing.Font)(resources.GetObject("cmbImageChooser.Font")));
      this.cmbImageChooser.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("cmbImageChooser.ImeMode")));
      this.cmbImageChooser.IntegralHeight = ((bool)(resources.GetObject("cmbImageChooser.IntegralHeight")));
      this.cmbImageChooser.ItemHeight = ((int)(resources.GetObject("cmbImageChooser.ItemHeight")));
      this.cmbImageChooser.Location = ((System.Drawing.Point)(resources.GetObject("cmbImageChooser.Location")));
      this.cmbImageChooser.MaxDropDownItems = ((int)(resources.GetObject("cmbImageChooser.MaxDropDownItems")));
      this.cmbImageChooser.MaxLength = ((int)(resources.GetObject("cmbImageChooser.MaxLength")));
      this.cmbImageChooser.Name = "cmbImageChooser";
      this.cmbImageChooser.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("cmbImageChooser.RightToLeft")));
      this.cmbImageChooser.Size = ((System.Drawing.Size)(resources.GetObject("cmbImageChooser.Size")));
      this.cmbImageChooser.TabIndex = ((int)(resources.GetObject("cmbImageChooser.TabIndex")));
      this.cmbImageChooser.Text = resources.GetString("cmbImageChooser.Text");
      this.cmbImageChooser.Visible = ((bool)(resources.GetObject("cmbImageChooser.Visible")));
      this.cmbImageChooser.SelectedIndexChanged += new System.EventHandler(this.cmbImageChooser_SelectedIndexChanged);
      // 
      // tabViewerStringTable
      // 
      this.tabViewerStringTable.AccessibleDescription = resources.GetString("tabViewerStringTable.AccessibleDescription");
      this.tabViewerStringTable.AccessibleName = resources.GetString("tabViewerStringTable.AccessibleName");
      this.tabViewerStringTable.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("tabViewerStringTable.Anchor")));
      this.tabViewerStringTable.AutoScroll = ((bool)(resources.GetObject("tabViewerStringTable.AutoScroll")));
      this.tabViewerStringTable.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("tabViewerStringTable.AutoScrollMargin")));
      this.tabViewerStringTable.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("tabViewerStringTable.AutoScrollMinSize")));
      this.tabViewerStringTable.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tabViewerStringTable.BackgroundImage")));
      this.tabViewerStringTable.Controls.Add(this.lstEntries);
      this.tabViewerStringTable.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("tabViewerStringTable.Dock")));
      this.tabViewerStringTable.Enabled = ((bool)(resources.GetObject("tabViewerStringTable.Enabled")));
      this.tabViewerStringTable.Font = ((System.Drawing.Font)(resources.GetObject("tabViewerStringTable.Font")));
      this.tabViewerStringTable.ImageIndex = ((int)(resources.GetObject("tabViewerStringTable.ImageIndex")));
      this.tabViewerStringTable.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("tabViewerStringTable.ImeMode")));
      this.tabViewerStringTable.Location = ((System.Drawing.Point)(resources.GetObject("tabViewerStringTable.Location")));
      this.tabViewerStringTable.Name = "tabViewerStringTable";
      this.tabViewerStringTable.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("tabViewerStringTable.RightToLeft")));
      this.tabViewerStringTable.Size = ((System.Drawing.Size)(resources.GetObject("tabViewerStringTable.Size")));
      this.tabViewerStringTable.TabIndex = ((int)(resources.GetObject("tabViewerStringTable.TabIndex")));
      this.tabViewerStringTable.Text = resources.GetString("tabViewerStringTable.Text");
      this.tabViewerStringTable.ToolTipText = resources.GetString("tabViewerStringTable.ToolTipText");
      this.tabViewerStringTable.Visible = ((bool)(resources.GetObject("tabViewerStringTable.Visible")));
      // 
      // lstEntries
      // 
      this.lstEntries.AccessibleDescription = resources.GetString("lstEntries.AccessibleDescription");
      this.lstEntries.AccessibleName = resources.GetString("lstEntries.AccessibleName");
      this.lstEntries.Alignment = ((System.Windows.Forms.ListViewAlignment)(resources.GetObject("lstEntries.Alignment")));
      this.lstEntries.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lstEntries.Anchor")));
      this.lstEntries.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("lstEntries.BackgroundImage")));
      this.lstEntries.ContextMenu = this.mnuStringTableContext;
      this.lstEntries.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lstEntries.Dock")));
      this.lstEntries.Enabled = ((bool)(resources.GetObject("lstEntries.Enabled")));
      this.lstEntries.Font = ((System.Drawing.Font)(resources.GetObject("lstEntries.Font")));
      this.lstEntries.FullRowSelect = true;
      this.lstEntries.GridLines = true;
      this.lstEntries.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lstEntries.ImeMode")));
      this.lstEntries.LabelWrap = ((bool)(resources.GetObject("lstEntries.LabelWrap")));
      this.lstEntries.Location = ((System.Drawing.Point)(resources.GetObject("lstEntries.Location")));
      this.lstEntries.Name = "lstEntries";
      this.lstEntries.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lstEntries.RightToLeft")));
      this.lstEntries.Size = ((System.Drawing.Size)(resources.GetObject("lstEntries.Size")));
      this.lstEntries.TabIndex = ((int)(resources.GetObject("lstEntries.TabIndex")));
      this.lstEntries.Text = resources.GetString("lstEntries.Text");
      this.lstEntries.View = System.Windows.Forms.View.Details;
      this.lstEntries.Visible = ((bool)(resources.GetObject("lstEntries.Visible")));
      this.lstEntries.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstEntries_KeyDown);
      // 
      // pnlNoViewers
      // 
      this.pnlNoViewers.AccessibleDescription = resources.GetString("pnlNoViewers.AccessibleDescription");
      this.pnlNoViewers.AccessibleName = resources.GetString("pnlNoViewers.AccessibleName");
      this.pnlNoViewers.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("pnlNoViewers.Anchor")));
      this.pnlNoViewers.AutoScroll = ((bool)(resources.GetObject("pnlNoViewers.AutoScroll")));
      this.pnlNoViewers.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("pnlNoViewers.AutoScrollMargin")));
      this.pnlNoViewers.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("pnlNoViewers.AutoScrollMinSize")));
      this.pnlNoViewers.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlNoViewers.BackgroundImage")));
      this.pnlNoViewers.Controls.Add(this.lblNoViewers);
      this.pnlNoViewers.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("pnlNoViewers.Dock")));
      this.pnlNoViewers.Enabled = ((bool)(resources.GetObject("pnlNoViewers.Enabled")));
      this.pnlNoViewers.Font = ((System.Drawing.Font)(resources.GetObject("pnlNoViewers.Font")));
      this.pnlNoViewers.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("pnlNoViewers.ImeMode")));
      this.pnlNoViewers.Location = ((System.Drawing.Point)(resources.GetObject("pnlNoViewers.Location")));
      this.pnlNoViewers.Name = "pnlNoViewers";
      this.pnlNoViewers.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("pnlNoViewers.RightToLeft")));
      this.pnlNoViewers.Size = ((System.Drawing.Size)(resources.GetObject("pnlNoViewers.Size")));
      this.pnlNoViewers.TabIndex = ((int)(resources.GetObject("pnlNoViewers.TabIndex")));
      this.pnlNoViewers.Text = resources.GetString("pnlNoViewers.Text");
      this.pnlNoViewers.Visible = ((bool)(resources.GetObject("pnlNoViewers.Visible")));
      // 
      // lblNoViewers
      // 
      this.lblNoViewers.AccessibleDescription = resources.GetString("lblNoViewers.AccessibleDescription");
      this.lblNoViewers.AccessibleName = resources.GetString("lblNoViewers.AccessibleName");
      this.lblNoViewers.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblNoViewers.Anchor")));
      this.lblNoViewers.AutoSize = ((bool)(resources.GetObject("lblNoViewers.AutoSize")));
      this.lblNoViewers.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblNoViewers.Dock")));
      this.lblNoViewers.Enabled = ((bool)(resources.GetObject("lblNoViewers.Enabled")));
      this.lblNoViewers.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblNoViewers.Font = ((System.Drawing.Font)(resources.GetObject("lblNoViewers.Font")));
      this.lblNoViewers.Image = ((System.Drawing.Image)(resources.GetObject("lblNoViewers.Image")));
      this.lblNoViewers.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblNoViewers.ImageAlign")));
      this.lblNoViewers.ImageIndex = ((int)(resources.GetObject("lblNoViewers.ImageIndex")));
      this.lblNoViewers.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblNoViewers.ImeMode")));
      this.lblNoViewers.Location = ((System.Drawing.Point)(resources.GetObject("lblNoViewers.Location")));
      this.lblNoViewers.Name = "lblNoViewers";
      this.lblNoViewers.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblNoViewers.RightToLeft")));
      this.lblNoViewers.Size = ((System.Drawing.Size)(resources.GetObject("lblNoViewers.Size")));
      this.lblNoViewers.TabIndex = ((int)(resources.GetObject("lblNoViewers.TabIndex")));
      this.lblNoViewers.Text = resources.GetString("lblNoViewers.Text");
      this.lblNoViewers.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblNoViewers.TextAlign")));
      this.lblNoViewers.Visible = ((bool)(resources.GetObject("lblNoViewers.Visible")));
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
      this.Controls.Add(this.pnlViewerArea);
      this.Controls.Add(this.splSplitter);
      this.Controls.Add(this.tvDataFiles);
      this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
      this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
      this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
      this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
      this.Menu = this.mnuMain;
      this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
      this.Name = "MainWindow";
      this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
      this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
      this.Text = resources.GetString("$this.Text");
      this.pnlViewerArea.ResumeLayout(false);
      this.tabViewers.ResumeLayout(false);
      this.tabViewerItems.ResumeLayout(false);
      this.grpMainItemActions.ResumeLayout(false);
      this.tabViewerImages.ResumeLayout(false);
      this.pnlImageChooser.ResumeLayout(false);
      this.tabViewerStringTable.ResumeLayout(false);
      this.pnlNoViewers.ResumeLayout(false);
      this.ResumeLayout(false);

    }

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
	  int i = 0;
	    foreach (Array A in FSD.StringTableEntries) {
	      if (i == 0) {
		this.lstEntries.Columns.Clear();
		this.lstEntries.Columns.Add(I18N.GetText("ColumnHeader:Entry"), 1, HorizontalAlignment.Center);
		foreach (ColumnHeader CH in A)
		  this.lstEntries.Columns.Add(CH);
		++i;
	      }
	      else {
	      ListViewItem LVI = this.lstEntries.Items.Add(String.Format("{0:00000000}", i++));
		foreach (string S in A)
		  LVI.SubItems.Add(S);
	      }
	      if ((i % 100) == 0)
		Application.DoEvents();
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

    #region Picture Context Menu Events

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

    #region String Table Context Menu Events

    private void CopyStringTableText() {
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
      if (e.Modifiers == Keys.Control && (e.KeyCode == Keys.C || e.KeyCode == Keys.Insert))
	this.CopyStringTableText();
    }

    private void mnuSTCCopy_Click(object sender, System.EventArgs e) {
      this.CopyStringTableText();
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

  }

}
