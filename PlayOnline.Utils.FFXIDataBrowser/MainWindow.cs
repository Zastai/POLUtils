using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using PlayOnline.Core;
using PlayOnline.FFXI;

namespace PlayOnline.Utils.FFXIDataBrowser {

  public class MainWindow : System.Windows.Forms.Form {

    private class ROMMenuItem : MenuItem {

      public ROMMenuItem(string Text, int App, int Dir, int File, EventHandler OnClick) : base(Text, OnClick) {
	this.App_  = App;
	this.Dir_  = Dir;
	this.File_ = File;
      }

      public int ROMApp  { get { return this.App_;  } }
      public int ROMDir  { get { return this.Dir_;  } }
      public int ROMFile { get { return this.File_; } }

      private int App_;
      private int Dir_;
      private int File_;

    }

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
    private System.Windows.Forms.MenuItem mnuStringTables;
    private System.Windows.Forms.MenuItem mnuSTEnglish;
    private System.Windows.Forms.MenuItem mnuSTJapanese;
    private System.Windows.Forms.MenuItem mnuSTShared;
    private System.Windows.Forms.ContextMenu mnuStringTableContext;
    private System.Windows.Forms.MenuItem mnuSTCCopy;
    private System.Windows.Forms.MenuItem mnuImages;
    private System.Windows.Forms.MenuItem mnuIMaps;
    private System.Windows.Forms.MenuItem mnuIOther;
    private System.Windows.Forms.Panel pnlViewerArea;
    private System.Windows.Forms.MenuItem mnuWindows;
    private System.Windows.Forms.MenuItem mnuWFileTable;
    private System.Windows.Forms.TabControl tabViewers;
    private System.Windows.Forms.ListView lstEntries;
    private System.Windows.Forms.TabPage tabViewerImages;
    private System.Windows.Forms.PictureBox picImageViewer;
    private System.Windows.Forms.Panel pnlImageChooser;
    private System.Windows.Forms.Label lblImageChooser;
    private System.Windows.Forms.ComboBox cmbImageChooser;
    private System.Windows.Forms.Panel pnlNoViewers;
    private System.Windows.Forms.Label lblNoViewers;
    private System.Windows.Forms.MenuItem mnuIMSandoria;
    private System.Windows.Forms.MenuItem mnuIMBastok;
    private System.Windows.Forms.MenuItem mnuIMWindurst;
    private System.Windows.Forms.MenuItem mnuIMJeuno;
    private System.Windows.Forms.MenuItem mnuIMRonfaure;
    private System.Windows.Forms.MenuItem mnuIMZulkheim;
    private System.Windows.Forms.MenuItem mnuIMNorvallen;
    private System.Windows.Forms.MenuItem mnuIMGustaberg;
    private System.Windows.Forms.MenuItem mnuIMDerfland;
    private System.Windows.Forms.MenuItem mnuIMSarutabaruta;
    private System.Windows.Forms.MenuItem mnuIMKolshushu;
    private System.Windows.Forms.MenuItem mnuIMAragoneu;
    private System.Windows.Forms.MenuItem mnuIMFauregandi;
    private System.Windows.Forms.MenuItem mnuIMValdeaunia;
    private System.Windows.Forms.MenuItem mnuIMQufim;
    private System.Windows.Forms.MenuItem mnuIMLiTelor;
    private System.Windows.Forms.MenuItem mnuIMKuzotz;
    private System.Windows.Forms.MenuItem mnuIMVollbow;
    private System.Windows.Forms.MenuItem mnuIMElshimoLow;
    private System.Windows.Forms.MenuItem mnuIMElshimoUp;
    private System.Windows.Forms.MenuItem mnuIMTuLia;
    private System.Windows.Forms.MenuItem mnuIMMovalpolos;
    private System.Windows.Forms.MenuItem mnuIMTavMarquisate;
    private System.Windows.Forms.MenuItem mnuIMTavArchipelago;
    private System.Windows.Forms.MenuItem mnuIMPromyvion;
    private System.Windows.Forms.MenuItem mnuIMOther;
    private System.Windows.Forms.TabPage tabViewerItems;
    private System.Windows.Forms.ComboBox cmbItemType;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ListView lstItems;
    private System.Windows.Forms.ImageList ilItemIcons;
    private System.Windows.Forms.ColumnHeader colXIEntryNum;
    private System.Windows.Forms.ColumnHeader colXIEntryText;
    private System.Windows.Forms.MenuItem mnuItemData;
    private System.Windows.Forms.MenuItem mnuIDEnglish;
    private System.Windows.Forms.MenuItem mnuIDJapanese;
    private System.Windows.Forms.TabPage tabViewerStringTable;

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
      this.lstItems.ColumnClick   += new ColumnClickEventHandler(ListViewColumnSorter.ListView_ColumnClick);
      this.lstEntries.ColumnClick += new ColumnClickEventHandler(ListViewColumnSorter.ListView_ColumnClick);
      this.ResetViewers();
    }

    private void ResetViewers() {
      // Clear the entire right-hand pane
      this.pnlNoViewers.Visible = false;
      this.tabViewers.Visible = false;
      // Reset the viewer tabs
      this.tabViewers.TabPages.Clear();
      // Reset all applicable viewer tab contents
      this.lstItems.Items.Clear();
      this.lstItems.Columns.Clear();
      this.lstItems.ListViewItemSorter = null;
      this.ilItemIcons.Images.Clear();
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

    private FFXIItem[] LoadedItems = null;

    private void ShowItemData() {
      this.lstItems.Items.Clear();
      this.lstItems.Columns.Clear();
      this.lstItems.HeaderStyle = ColumnHeaderStyle.Nonclickable;
    bool AddImages = false;
      if (this.ilItemIcons.Images.Count == 0) {
	AddImages = true;
	this.cmbImageChooser.Items.Clear();
      }
      // Set up columns
      this.lstItems.Columns.Add("Item Number",    90, HorizontalAlignment.Left);
      this.lstItems.Columns.Add("Flags",          60, HorizontalAlignment.Left);
      this.lstItems.Columns.Add("Stack Size",     75, HorizontalAlignment.Left);
      this.lstItems.Columns.Add("Item Type",      70, HorizontalAlignment.Left);
      if (this.cmbItemType.SelectedIndex >= 2) {
	this.lstItems.Columns.Add("Resource ID",  90, HorizontalAlignment.Left);
	this.lstItems.Columns.Add("Level",        45, HorizontalAlignment.Left);
	this.lstItems.Columns.Add("Slots",        70, HorizontalAlignment.Left);
	this.lstItems.Columns.Add("Races",        85, HorizontalAlignment.Left);
	this.lstItems.Columns.Add("Jobs",        300, HorizontalAlignment.Left);
	if (this.cmbItemType.SelectedIndex < 4)
	  this.lstItems.Columns.Add("Defense",    60, HorizontalAlignment.Left);
	else {
	  this.lstItems.Columns.Add("Damage",     60, HorizontalAlignment.Left);
	  this.lstItems.Columns.Add("Delay",      50, HorizontalAlignment.Left);
	  this.lstItems.Columns.Add("Unknown 2",  70, HorizontalAlignment.Left);
	  this.lstItems.Columns.Add("Skill",      80, HorizontalAlignment.Left);
	  this.lstItems.Columns.Add("Unknown 3",  70, HorizontalAlignment.Left);
	}
      }
      else {
	this.lstItems.Columns.Add("Unknown 2",    70, HorizontalAlignment.Left);
	this.lstItems.Columns.Add("Unknown 3",    70, HorizontalAlignment.Left);
      }
      this.lstItems.Columns.Add("Japanese Name", 125, HorizontalAlignment.Left);
      this.lstItems.Columns.Add("English Name",  125, HorizontalAlignment.Left);
      if ((this.cmbItemType.SelectedIndex % 2) == 0) { // English data has two additional text fields
	this.lstItems.Columns.Add("Log Name (Singular)", 200, HorizontalAlignment.Left);
	this.lstItems.Columns.Add("Log Name (Plural)",   200, HorizontalAlignment.Left);
      }
      this.lstItems.Columns.Add("Description (1)", 200, HorizontalAlignment.Left);
      this.lstItems.Columns.Add("Description (2)", 200, HorizontalAlignment.Left);
      this.lstItems.Columns.Add("Description (3)", 200, HorizontalAlignment.Left);
      this.lstItems.Columns.Add("Description (4)", 200, HorizontalAlignment.Left);
      this.lstItems.Columns.Add("Description (5)", 200, HorizontalAlignment.Left);
      this.lstItems.Columns.Add("Description (6)", 200, HorizontalAlignment.Left);
      this.lstItems.Columns.Add("Icon Info",   190, HorizontalAlignment.Left);
      Application.DoEvents();
      // Load values
    int ItemCount = 0;
      foreach (FFXIItem FI in this.LoadedItems) {
	if (AddImages) {
	  this.ilItemIcons.Images.Add(FI.Image.Bitmap);
	  this.cmbImageChooser.Items.Add(FI.Image);
	}
      ListViewItem LVI = this.lstItems.Items.Add("", ItemCount++);
      Encoding E = new POLEncoding();
      BinaryReader BR = new BinaryReader(new MemoryStream(FI.Data, false));
	LVI.Text = String.Format("{0:X8}", BR.ReadUInt32());
	{
	ushort Flags = BR.ReadUInt16();
	  LVI.SubItems.Add(String.Format("{0:X4} ({1})", Flags, (ItemFlags) Flags));
	}
	LVI.SubItems.Add(String.Format("{0}", BR.ReadUInt16()));
	{
	ushort ItemType = BR.ReadUInt16();
	  LVI.SubItems.Add(String.Format("{0:X4} ({1})", ItemType, (ItemType) ItemType));
	}
	if (this.cmbItemType.SelectedIndex >= 2) {
	  LVI.SubItems.Add(String.Format("{0:X8}", BR.ReadUInt32()));
	  LVI.SubItems.Add(String.Format("{0}", BR.ReadUInt16()));
	  LVI.SubItems.Add(String.Format("{0}", (EquipmentSlot) BR.ReadUInt16()));
	  LVI.SubItems.Add(String.Format("{0}", (Race) BR.ReadUInt16()));
	  LVI.SubItems.Add(String.Format("{0}", (Job) BR.ReadUInt16()));
	  LVI.SubItems.Add(String.Format("{0}", BR.ReadUInt16()));
	}
	else {
	  LVI.SubItems.Add(String.Format("{0:X4} ({0})", BR.ReadUInt16()));
	  LVI.SubItems.Add(String.Format("{0:X4} ({0})", BR.ReadUInt16()));
	}
	if (this.cmbItemType.SelectedIndex >= 4) {
	  LVI.SubItems.Add(String.Format("{0}", BR.ReadUInt16()));
	  LVI.SubItems.Add(String.Format("{0:X4} ({0})", BR.ReadUInt16()));
	  {
	  byte ItemSkill = BR.ReadByte();
	    LVI.SubItems.Add(String.Format("{0:X2} ({1})", ItemSkill, (ItemSkill) ItemSkill));
	  }
	  LVI.SubItems.Add(String.Format("{0:X2} ({0})", BR.ReadByte()));
	}
	if ((this.cmbItemType.SelectedIndex % 2) == 1) { // JP
	  LVI.SubItems.Add(E.GetString(BR.ReadBytes(22)).TrimEnd('\0'));
	  LVI.SubItems.Add(E.GetString(BR.ReadBytes(22)).TrimEnd('\0'));
	}
	else { // EN
	  LVI.SubItems.Add(E.GetString(BR.ReadBytes(22)).TrimEnd('\0'));
	  LVI.SubItems.Add(E.GetString(BR.ReadBytes(34)).TrimEnd('\0'));
	  LVI.SubItems.Add(E.GetString(BR.ReadBytes(64)).TrimEnd('\0'));
	  LVI.SubItems.Add(E.GetString(BR.ReadBytes(64)).TrimEnd('\0'));
	}
	{ // 188 is a guess, based on non-NUL chars after a string of NULs - so this could be less
	string DescriptionText = E.GetString(BR.ReadBytes(188)).TrimEnd('\0');
	string[] DescriptionLines = DescriptionText.Split(new char[] { '\0' });
	  for (int i = 0; i < 6; ++i)
	    LVI.SubItems.Add((i < DescriptionLines.Length) ? DescriptionLines[i] : "");
	}
	LVI.SubItems.Add(FI.Image.ToString());
	Application.DoEvents();
      }
      this.lstItems.HeaderStyle = ColumnHeaderStyle.Clickable;
      if (AddImages)
	this.cmbImageChooser.SelectedIndex = 0;
    }

    private void cmbItemType_SelectedIndexChanged(object sender, System.EventArgs e) {
      if (this.cmbItemType.SelectedItem != null)
	this.ShowItemData();
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
      this.mnuImages = new System.Windows.Forms.MenuItem();
      this.mnuIMaps = new System.Windows.Forms.MenuItem();
      this.mnuIMSandoria = new System.Windows.Forms.MenuItem();
      this.mnuIMBastok = new System.Windows.Forms.MenuItem();
      this.mnuIMWindurst = new System.Windows.Forms.MenuItem();
      this.mnuIMJeuno = new System.Windows.Forms.MenuItem();
      this.mnuIMRonfaure = new System.Windows.Forms.MenuItem();
      this.mnuIMZulkheim = new System.Windows.Forms.MenuItem();
      this.mnuIMNorvallen = new System.Windows.Forms.MenuItem();
      this.mnuIMGustaberg = new System.Windows.Forms.MenuItem();
      this.mnuIMDerfland = new System.Windows.Forms.MenuItem();
      this.mnuIMSarutabaruta = new System.Windows.Forms.MenuItem();
      this.mnuIMKolshushu = new System.Windows.Forms.MenuItem();
      this.mnuIMAragoneu = new System.Windows.Forms.MenuItem();
      this.mnuIMFauregandi = new System.Windows.Forms.MenuItem();
      this.mnuIMValdeaunia = new System.Windows.Forms.MenuItem();
      this.mnuIMQufim = new System.Windows.Forms.MenuItem();
      this.mnuIMLiTelor = new System.Windows.Forms.MenuItem();
      this.mnuIMKuzotz = new System.Windows.Forms.MenuItem();
      this.mnuIMVollbow = new System.Windows.Forms.MenuItem();
      this.mnuIMElshimoLow = new System.Windows.Forms.MenuItem();
      this.mnuIMElshimoUp = new System.Windows.Forms.MenuItem();
      this.mnuIMTuLia = new System.Windows.Forms.MenuItem();
      this.mnuIMMovalpolos = new System.Windows.Forms.MenuItem();
      this.mnuIMTavMarquisate = new System.Windows.Forms.MenuItem();
      this.mnuIMTavArchipelago = new System.Windows.Forms.MenuItem();
      this.mnuIMPromyvion = new System.Windows.Forms.MenuItem();
      this.mnuIMOther = new System.Windows.Forms.MenuItem();
      this.mnuIOther = new System.Windows.Forms.MenuItem();
      this.mnuItemData = new System.Windows.Forms.MenuItem();
      this.mnuIDEnglish = new System.Windows.Forms.MenuItem();
      this.mnuIDJapanese = new System.Windows.Forms.MenuItem();
      this.mnuStringTables = new System.Windows.Forms.MenuItem();
      this.mnuSTEnglish = new System.Windows.Forms.MenuItem();
      this.mnuSTJapanese = new System.Windows.Forms.MenuItem();
      this.mnuSTShared = new System.Windows.Forms.MenuItem();
      this.mnuWindows = new System.Windows.Forms.MenuItem();
      this.mnuWFileTable = new System.Windows.Forms.MenuItem();
      this.pnlViewerArea = new System.Windows.Forms.Panel();
      this.tabViewers = new System.Windows.Forms.TabControl();
      this.tabViewerItems = new System.Windows.Forms.TabPage();
      this.lstItems = new System.Windows.Forms.ListView();
      this.ilItemIcons = new System.Windows.Forms.ImageList(this.components);
      this.label2 = new System.Windows.Forms.Label();
      this.cmbItemType = new System.Windows.Forms.ComboBox();
      this.tabViewerImages = new System.Windows.Forms.TabPage();
      this.picImageViewer = new System.Windows.Forms.PictureBox();
      this.pnlImageChooser = new System.Windows.Forms.Panel();
      this.lblImageChooser = new System.Windows.Forms.Label();
      this.cmbImageChooser = new System.Windows.Forms.ComboBox();
      this.tabViewerStringTable = new System.Windows.Forms.TabPage();
      this.lstEntries = new System.Windows.Forms.ListView();
      this.colXIEntryNum = new System.Windows.Forms.ColumnHeader();
      this.colXIEntryText = new System.Windows.Forms.ColumnHeader();
      this.pnlNoViewers = new System.Windows.Forms.Panel();
      this.lblNoViewers = new System.Windows.Forms.Label();
      this.pnlViewerArea.SuspendLayout();
      this.tabViewers.SuspendLayout();
      this.tabViewerItems.SuspendLayout();
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
									    this.mnuImages,
									    this.mnuItemData,
									    this.mnuStringTables,
									    this.mnuWindows});
      this.mnuMain.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mnuMain.RightToLeft")));
      // 
      // mnuImages
      // 
      this.mnuImages.Enabled = ((bool)(resources.GetObject("mnuImages.Enabled")));
      this.mnuImages.Index = 0;
      this.mnuImages.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
									      this.mnuIMaps,
									      this.mnuIOther});
      this.mnuImages.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuImages.Shortcut")));
      this.mnuImages.ShowShortcut = ((bool)(resources.GetObject("mnuImages.ShowShortcut")));
      this.mnuImages.Text = resources.GetString("mnuImages.Text");
      this.mnuImages.Visible = ((bool)(resources.GetObject("mnuImages.Visible")));
      this.mnuImages.Popup += new System.EventHandler(this.mnuImages_Popup);
      // 
      // mnuIMaps
      // 
      this.mnuIMaps.Enabled = ((bool)(resources.GetObject("mnuIMaps.Enabled")));
      this.mnuIMaps.Index = 0;
      this.mnuIMaps.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
									     this.mnuIMSandoria,
									     this.mnuIMBastok,
									     this.mnuIMWindurst,
									     this.mnuIMJeuno,
									     this.mnuIMRonfaure,
									     this.mnuIMZulkheim,
									     this.mnuIMNorvallen,
									     this.mnuIMGustaberg,
									     this.mnuIMDerfland,
									     this.mnuIMSarutabaruta,
									     this.mnuIMKolshushu,
									     this.mnuIMAragoneu,
									     this.mnuIMFauregandi,
									     this.mnuIMValdeaunia,
									     this.mnuIMQufim,
									     this.mnuIMLiTelor,
									     this.mnuIMKuzotz,
									     this.mnuIMVollbow,
									     this.mnuIMElshimoLow,
									     this.mnuIMElshimoUp,
									     this.mnuIMTuLia,
									     this.mnuIMMovalpolos,
									     this.mnuIMTavMarquisate,
									     this.mnuIMTavArchipelago,
									     this.mnuIMPromyvion,
									     this.mnuIMOther});
      this.mnuIMaps.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuIMaps.Shortcut")));
      this.mnuIMaps.ShowShortcut = ((bool)(resources.GetObject("mnuIMaps.ShowShortcut")));
      this.mnuIMaps.Text = resources.GetString("mnuIMaps.Text");
      this.mnuIMaps.Visible = ((bool)(resources.GetObject("mnuIMaps.Visible")));
      this.mnuIMaps.Popup += new System.EventHandler(this.mnuIMaps_Popup);
      // 
      // mnuIMSandoria
      // 
      this.mnuIMSandoria.Enabled = ((bool)(resources.GetObject("mnuIMSandoria.Enabled")));
      this.mnuIMSandoria.Index = 0;
      this.mnuIMSandoria.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuIMSandoria.Shortcut")));
      this.mnuIMSandoria.ShowShortcut = ((bool)(resources.GetObject("mnuIMSandoria.ShowShortcut")));
      this.mnuIMSandoria.Text = resources.GetString("mnuIMSandoria.Text");
      this.mnuIMSandoria.Visible = ((bool)(resources.GetObject("mnuIMSandoria.Visible")));
      this.mnuIMSandoria.Popup += new System.EventHandler(this.mnuIMSandoria_Popup);
      // 
      // mnuIMBastok
      // 
      this.mnuIMBastok.Enabled = ((bool)(resources.GetObject("mnuIMBastok.Enabled")));
      this.mnuIMBastok.Index = 1;
      this.mnuIMBastok.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuIMBastok.Shortcut")));
      this.mnuIMBastok.ShowShortcut = ((bool)(resources.GetObject("mnuIMBastok.ShowShortcut")));
      this.mnuIMBastok.Text = resources.GetString("mnuIMBastok.Text");
      this.mnuIMBastok.Visible = ((bool)(resources.GetObject("mnuIMBastok.Visible")));
      this.mnuIMBastok.Popup += new System.EventHandler(this.mnuIMBastok_Popup);
      // 
      // mnuIMWindurst
      // 
      this.mnuIMWindurst.Enabled = ((bool)(resources.GetObject("mnuIMWindurst.Enabled")));
      this.mnuIMWindurst.Index = 2;
      this.mnuIMWindurst.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuIMWindurst.Shortcut")));
      this.mnuIMWindurst.ShowShortcut = ((bool)(resources.GetObject("mnuIMWindurst.ShowShortcut")));
      this.mnuIMWindurst.Text = resources.GetString("mnuIMWindurst.Text");
      this.mnuIMWindurst.Visible = ((bool)(resources.GetObject("mnuIMWindurst.Visible")));
      this.mnuIMWindurst.Popup += new System.EventHandler(this.mnuIMWindurst_Popup);
      // 
      // mnuIMJeuno
      // 
      this.mnuIMJeuno.Enabled = ((bool)(resources.GetObject("mnuIMJeuno.Enabled")));
      this.mnuIMJeuno.Index = 3;
      this.mnuIMJeuno.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuIMJeuno.Shortcut")));
      this.mnuIMJeuno.ShowShortcut = ((bool)(resources.GetObject("mnuIMJeuno.ShowShortcut")));
      this.mnuIMJeuno.Text = resources.GetString("mnuIMJeuno.Text");
      this.mnuIMJeuno.Visible = ((bool)(resources.GetObject("mnuIMJeuno.Visible")));
      this.mnuIMJeuno.Popup += new System.EventHandler(this.mnuIMJeuno_Popup);
      // 
      // mnuIMRonfaure
      // 
      this.mnuIMRonfaure.Enabled = ((bool)(resources.GetObject("mnuIMRonfaure.Enabled")));
      this.mnuIMRonfaure.Index = 4;
      this.mnuIMRonfaure.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuIMRonfaure.Shortcut")));
      this.mnuIMRonfaure.ShowShortcut = ((bool)(resources.GetObject("mnuIMRonfaure.ShowShortcut")));
      this.mnuIMRonfaure.Text = resources.GetString("mnuIMRonfaure.Text");
      this.mnuIMRonfaure.Visible = ((bool)(resources.GetObject("mnuIMRonfaure.Visible")));
      this.mnuIMRonfaure.Popup += new System.EventHandler(this.mnuIMRonfaure_Popup);
      // 
      // mnuIMZulkheim
      // 
      this.mnuIMZulkheim.Enabled = ((bool)(resources.GetObject("mnuIMZulkheim.Enabled")));
      this.mnuIMZulkheim.Index = 5;
      this.mnuIMZulkheim.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuIMZulkheim.Shortcut")));
      this.mnuIMZulkheim.ShowShortcut = ((bool)(resources.GetObject("mnuIMZulkheim.ShowShortcut")));
      this.mnuIMZulkheim.Text = resources.GetString("mnuIMZulkheim.Text");
      this.mnuIMZulkheim.Visible = ((bool)(resources.GetObject("mnuIMZulkheim.Visible")));
      this.mnuIMZulkheim.Popup += new System.EventHandler(this.mnuIMZulkheim_Popup);
      // 
      // mnuIMNorvallen
      // 
      this.mnuIMNorvallen.Enabled = ((bool)(resources.GetObject("mnuIMNorvallen.Enabled")));
      this.mnuIMNorvallen.Index = 6;
      this.mnuIMNorvallen.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuIMNorvallen.Shortcut")));
      this.mnuIMNorvallen.ShowShortcut = ((bool)(resources.GetObject("mnuIMNorvallen.ShowShortcut")));
      this.mnuIMNorvallen.Text = resources.GetString("mnuIMNorvallen.Text");
      this.mnuIMNorvallen.Visible = ((bool)(resources.GetObject("mnuIMNorvallen.Visible")));
      this.mnuIMNorvallen.Popup += new System.EventHandler(this.mnuIMNorvallen_Popup);
      // 
      // mnuIMGustaberg
      // 
      this.mnuIMGustaberg.Enabled = ((bool)(resources.GetObject("mnuIMGustaberg.Enabled")));
      this.mnuIMGustaberg.Index = 7;
      this.mnuIMGustaberg.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuIMGustaberg.Shortcut")));
      this.mnuIMGustaberg.ShowShortcut = ((bool)(resources.GetObject("mnuIMGustaberg.ShowShortcut")));
      this.mnuIMGustaberg.Text = resources.GetString("mnuIMGustaberg.Text");
      this.mnuIMGustaberg.Visible = ((bool)(resources.GetObject("mnuIMGustaberg.Visible")));
      this.mnuIMGustaberg.Popup += new System.EventHandler(this.mnuIMGustaberg_Popup);
      // 
      // mnuIMDerfland
      // 
      this.mnuIMDerfland.Enabled = ((bool)(resources.GetObject("mnuIMDerfland.Enabled")));
      this.mnuIMDerfland.Index = 8;
      this.mnuIMDerfland.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuIMDerfland.Shortcut")));
      this.mnuIMDerfland.ShowShortcut = ((bool)(resources.GetObject("mnuIMDerfland.ShowShortcut")));
      this.mnuIMDerfland.Text = resources.GetString("mnuIMDerfland.Text");
      this.mnuIMDerfland.Visible = ((bool)(resources.GetObject("mnuIMDerfland.Visible")));
      this.mnuIMDerfland.Popup += new System.EventHandler(this.mnuIMDerfland_Popup);
      // 
      // mnuIMSarutabaruta
      // 
      this.mnuIMSarutabaruta.Enabled = ((bool)(resources.GetObject("mnuIMSarutabaruta.Enabled")));
      this.mnuIMSarutabaruta.Index = 9;
      this.mnuIMSarutabaruta.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuIMSarutabaruta.Shortcut")));
      this.mnuIMSarutabaruta.ShowShortcut = ((bool)(resources.GetObject("mnuIMSarutabaruta.ShowShortcut")));
      this.mnuIMSarutabaruta.Text = resources.GetString("mnuIMSarutabaruta.Text");
      this.mnuIMSarutabaruta.Visible = ((bool)(resources.GetObject("mnuIMSarutabaruta.Visible")));
      this.mnuIMSarutabaruta.Popup += new System.EventHandler(this.mnuIMSarutabaruta_Popup);
      // 
      // mnuIMKolshushu
      // 
      this.mnuIMKolshushu.Enabled = ((bool)(resources.GetObject("mnuIMKolshushu.Enabled")));
      this.mnuIMKolshushu.Index = 10;
      this.mnuIMKolshushu.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuIMKolshushu.Shortcut")));
      this.mnuIMKolshushu.ShowShortcut = ((bool)(resources.GetObject("mnuIMKolshushu.ShowShortcut")));
      this.mnuIMKolshushu.Text = resources.GetString("mnuIMKolshushu.Text");
      this.mnuIMKolshushu.Visible = ((bool)(resources.GetObject("mnuIMKolshushu.Visible")));
      this.mnuIMKolshushu.Popup += new System.EventHandler(this.mnuIMKolshushu_Popup);
      // 
      // mnuIMAragoneu
      // 
      this.mnuIMAragoneu.Enabled = ((bool)(resources.GetObject("mnuIMAragoneu.Enabled")));
      this.mnuIMAragoneu.Index = 11;
      this.mnuIMAragoneu.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuIMAragoneu.Shortcut")));
      this.mnuIMAragoneu.ShowShortcut = ((bool)(resources.GetObject("mnuIMAragoneu.ShowShortcut")));
      this.mnuIMAragoneu.Text = resources.GetString("mnuIMAragoneu.Text");
      this.mnuIMAragoneu.Visible = ((bool)(resources.GetObject("mnuIMAragoneu.Visible")));
      this.mnuIMAragoneu.Popup += new System.EventHandler(this.mnuIMAragoneu_Popup);
      // 
      // mnuIMFauregandi
      // 
      this.mnuIMFauregandi.Enabled = ((bool)(resources.GetObject("mnuIMFauregandi.Enabled")));
      this.mnuIMFauregandi.Index = 12;
      this.mnuIMFauregandi.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuIMFauregandi.Shortcut")));
      this.mnuIMFauregandi.ShowShortcut = ((bool)(resources.GetObject("mnuIMFauregandi.ShowShortcut")));
      this.mnuIMFauregandi.Text = resources.GetString("mnuIMFauregandi.Text");
      this.mnuIMFauregandi.Visible = ((bool)(resources.GetObject("mnuIMFauregandi.Visible")));
      this.mnuIMFauregandi.Popup += new System.EventHandler(this.mnuIMFauregandi_Popup);
      // 
      // mnuIMValdeaunia
      // 
      this.mnuIMValdeaunia.Enabled = ((bool)(resources.GetObject("mnuIMValdeaunia.Enabled")));
      this.mnuIMValdeaunia.Index = 13;
      this.mnuIMValdeaunia.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuIMValdeaunia.Shortcut")));
      this.mnuIMValdeaunia.ShowShortcut = ((bool)(resources.GetObject("mnuIMValdeaunia.ShowShortcut")));
      this.mnuIMValdeaunia.Text = resources.GetString("mnuIMValdeaunia.Text");
      this.mnuIMValdeaunia.Visible = ((bool)(resources.GetObject("mnuIMValdeaunia.Visible")));
      this.mnuIMValdeaunia.Popup += new System.EventHandler(this.mnuIMValdeaunia_Popup);
      // 
      // mnuIMQufim
      // 
      this.mnuIMQufim.Enabled = ((bool)(resources.GetObject("mnuIMQufim.Enabled")));
      this.mnuIMQufim.Index = 14;
      this.mnuIMQufim.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuIMQufim.Shortcut")));
      this.mnuIMQufim.ShowShortcut = ((bool)(resources.GetObject("mnuIMQufim.ShowShortcut")));
      this.mnuIMQufim.Text = resources.GetString("mnuIMQufim.Text");
      this.mnuIMQufim.Visible = ((bool)(resources.GetObject("mnuIMQufim.Visible")));
      this.mnuIMQufim.Popup += new System.EventHandler(this.mnuIMQufim_Popup);
      // 
      // mnuIMLiTelor
      // 
      this.mnuIMLiTelor.Enabled = ((bool)(resources.GetObject("mnuIMLiTelor.Enabled")));
      this.mnuIMLiTelor.Index = 15;
      this.mnuIMLiTelor.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuIMLiTelor.Shortcut")));
      this.mnuIMLiTelor.ShowShortcut = ((bool)(resources.GetObject("mnuIMLiTelor.ShowShortcut")));
      this.mnuIMLiTelor.Text = resources.GetString("mnuIMLiTelor.Text");
      this.mnuIMLiTelor.Visible = ((bool)(resources.GetObject("mnuIMLiTelor.Visible")));
      this.mnuIMLiTelor.Popup += new System.EventHandler(this.mnuIMLiTelor_Popup);
      // 
      // mnuIMKuzotz
      // 
      this.mnuIMKuzotz.Enabled = ((bool)(resources.GetObject("mnuIMKuzotz.Enabled")));
      this.mnuIMKuzotz.Index = 16;
      this.mnuIMKuzotz.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuIMKuzotz.Shortcut")));
      this.mnuIMKuzotz.ShowShortcut = ((bool)(resources.GetObject("mnuIMKuzotz.ShowShortcut")));
      this.mnuIMKuzotz.Text = resources.GetString("mnuIMKuzotz.Text");
      this.mnuIMKuzotz.Visible = ((bool)(resources.GetObject("mnuIMKuzotz.Visible")));
      this.mnuIMKuzotz.Popup += new System.EventHandler(this.mnuIMKuzotz_Popup);
      // 
      // mnuIMVollbow
      // 
      this.mnuIMVollbow.Enabled = ((bool)(resources.GetObject("mnuIMVollbow.Enabled")));
      this.mnuIMVollbow.Index = 17;
      this.mnuIMVollbow.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuIMVollbow.Shortcut")));
      this.mnuIMVollbow.ShowShortcut = ((bool)(resources.GetObject("mnuIMVollbow.ShowShortcut")));
      this.mnuIMVollbow.Text = resources.GetString("mnuIMVollbow.Text");
      this.mnuIMVollbow.Visible = ((bool)(resources.GetObject("mnuIMVollbow.Visible")));
      this.mnuIMVollbow.Popup += new System.EventHandler(this.mnuIMVollbow_Popup);
      // 
      // mnuIMElshimoLow
      // 
      this.mnuIMElshimoLow.Enabled = ((bool)(resources.GetObject("mnuIMElshimoLow.Enabled")));
      this.mnuIMElshimoLow.Index = 18;
      this.mnuIMElshimoLow.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuIMElshimoLow.Shortcut")));
      this.mnuIMElshimoLow.ShowShortcut = ((bool)(resources.GetObject("mnuIMElshimoLow.ShowShortcut")));
      this.mnuIMElshimoLow.Text = resources.GetString("mnuIMElshimoLow.Text");
      this.mnuIMElshimoLow.Visible = ((bool)(resources.GetObject("mnuIMElshimoLow.Visible")));
      this.mnuIMElshimoLow.Popup += new System.EventHandler(this.mnuIMElshimoLow_Popup);
      // 
      // mnuIMElshimoUp
      // 
      this.mnuIMElshimoUp.Enabled = ((bool)(resources.GetObject("mnuIMElshimoUp.Enabled")));
      this.mnuIMElshimoUp.Index = 19;
      this.mnuIMElshimoUp.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuIMElshimoUp.Shortcut")));
      this.mnuIMElshimoUp.ShowShortcut = ((bool)(resources.GetObject("mnuIMElshimoUp.ShowShortcut")));
      this.mnuIMElshimoUp.Text = resources.GetString("mnuIMElshimoUp.Text");
      this.mnuIMElshimoUp.Visible = ((bool)(resources.GetObject("mnuIMElshimoUp.Visible")));
      this.mnuIMElshimoUp.Popup += new System.EventHandler(this.mnuIMElshimoUp_Popup);
      // 
      // mnuIMTuLia
      // 
      this.mnuIMTuLia.Enabled = ((bool)(resources.GetObject("mnuIMTuLia.Enabled")));
      this.mnuIMTuLia.Index = 20;
      this.mnuIMTuLia.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuIMTuLia.Shortcut")));
      this.mnuIMTuLia.ShowShortcut = ((bool)(resources.GetObject("mnuIMTuLia.ShowShortcut")));
      this.mnuIMTuLia.Text = resources.GetString("mnuIMTuLia.Text");
      this.mnuIMTuLia.Visible = ((bool)(resources.GetObject("mnuIMTuLia.Visible")));
      this.mnuIMTuLia.Popup += new System.EventHandler(this.mnuIMTuLia_Popup);
      // 
      // mnuIMMovalpolos
      // 
      this.mnuIMMovalpolos.Enabled = ((bool)(resources.GetObject("mnuIMMovalpolos.Enabled")));
      this.mnuIMMovalpolos.Index = 21;
      this.mnuIMMovalpolos.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuIMMovalpolos.Shortcut")));
      this.mnuIMMovalpolos.ShowShortcut = ((bool)(resources.GetObject("mnuIMMovalpolos.ShowShortcut")));
      this.mnuIMMovalpolos.Text = resources.GetString("mnuIMMovalpolos.Text");
      this.mnuIMMovalpolos.Visible = ((bool)(resources.GetObject("mnuIMMovalpolos.Visible")));
      this.mnuIMMovalpolos.Popup += new System.EventHandler(this.mnuIMMovalpolos_Popup);
      // 
      // mnuIMTavMarquisate
      // 
      this.mnuIMTavMarquisate.Enabled = ((bool)(resources.GetObject("mnuIMTavMarquisate.Enabled")));
      this.mnuIMTavMarquisate.Index = 22;
      this.mnuIMTavMarquisate.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuIMTavMarquisate.Shortcut")));
      this.mnuIMTavMarquisate.ShowShortcut = ((bool)(resources.GetObject("mnuIMTavMarquisate.ShowShortcut")));
      this.mnuIMTavMarquisate.Text = resources.GetString("mnuIMTavMarquisate.Text");
      this.mnuIMTavMarquisate.Visible = ((bool)(resources.GetObject("mnuIMTavMarquisate.Visible")));
      this.mnuIMTavMarquisate.Popup += new System.EventHandler(this.mnuIMTavMarquisate_Popup);
      // 
      // mnuIMTavArchipelago
      // 
      this.mnuIMTavArchipelago.Enabled = ((bool)(resources.GetObject("mnuIMTavArchipelago.Enabled")));
      this.mnuIMTavArchipelago.Index = 23;
      this.mnuIMTavArchipelago.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuIMTavArchipelago.Shortcut")));
      this.mnuIMTavArchipelago.ShowShortcut = ((bool)(resources.GetObject("mnuIMTavArchipelago.ShowShortcut")));
      this.mnuIMTavArchipelago.Text = resources.GetString("mnuIMTavArchipelago.Text");
      this.mnuIMTavArchipelago.Visible = ((bool)(resources.GetObject("mnuIMTavArchipelago.Visible")));
      this.mnuIMTavArchipelago.Popup += new System.EventHandler(this.mnuIMTavArchipelago_Popup);
      // 
      // mnuIMPromyvion
      // 
      this.mnuIMPromyvion.Enabled = ((bool)(resources.GetObject("mnuIMPromyvion.Enabled")));
      this.mnuIMPromyvion.Index = 24;
      this.mnuIMPromyvion.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuIMPromyvion.Shortcut")));
      this.mnuIMPromyvion.ShowShortcut = ((bool)(resources.GetObject("mnuIMPromyvion.ShowShortcut")));
      this.mnuIMPromyvion.Text = resources.GetString("mnuIMPromyvion.Text");
      this.mnuIMPromyvion.Visible = ((bool)(resources.GetObject("mnuIMPromyvion.Visible")));
      this.mnuIMPromyvion.Popup += new System.EventHandler(this.mnuIMPromyvion_Popup);
      // 
      // mnuIMOther
      // 
      this.mnuIMOther.Enabled = ((bool)(resources.GetObject("mnuIMOther.Enabled")));
      this.mnuIMOther.Index = 25;
      this.mnuIMOther.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuIMOther.Shortcut")));
      this.mnuIMOther.ShowShortcut = ((bool)(resources.GetObject("mnuIMOther.ShowShortcut")));
      this.mnuIMOther.Text = resources.GetString("mnuIMOther.Text");
      this.mnuIMOther.Visible = ((bool)(resources.GetObject("mnuIMOther.Visible")));
      this.mnuIMOther.Popup += new System.EventHandler(this.mnuIMOther_Popup);
      // 
      // mnuIOther
      // 
      this.mnuIOther.Enabled = ((bool)(resources.GetObject("mnuIOther.Enabled")));
      this.mnuIOther.Index = 1;
      this.mnuIOther.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuIOther.Shortcut")));
      this.mnuIOther.ShowShortcut = ((bool)(resources.GetObject("mnuIOther.ShowShortcut")));
      this.mnuIOther.Text = resources.GetString("mnuIOther.Text");
      this.mnuIOther.Visible = ((bool)(resources.GetObject("mnuIOther.Visible")));
      this.mnuIOther.Popup += new System.EventHandler(this.mnuIOther_Popup);
      // 
      // mnuItemData
      // 
      this.mnuItemData.Enabled = ((bool)(resources.GetObject("mnuItemData.Enabled")));
      this.mnuItemData.Index = 1;
      this.mnuItemData.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
										this.mnuIDEnglish,
										this.mnuIDJapanese});
      this.mnuItemData.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuItemData.Shortcut")));
      this.mnuItemData.ShowShortcut = ((bool)(resources.GetObject("mnuItemData.ShowShortcut")));
      this.mnuItemData.Text = resources.GetString("mnuItemData.Text");
      this.mnuItemData.Visible = ((bool)(resources.GetObject("mnuItemData.Visible")));
      this.mnuItemData.Popup += new System.EventHandler(this.mnuItemData_Popup);
      // 
      // mnuIDEnglish
      // 
      this.mnuIDEnglish.Enabled = ((bool)(resources.GetObject("mnuIDEnglish.Enabled")));
      this.mnuIDEnglish.Index = 0;
      this.mnuIDEnglish.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuIDEnglish.Shortcut")));
      this.mnuIDEnglish.ShowShortcut = ((bool)(resources.GetObject("mnuIDEnglish.ShowShortcut")));
      this.mnuIDEnglish.Text = resources.GetString("mnuIDEnglish.Text");
      this.mnuIDEnglish.Visible = ((bool)(resources.GetObject("mnuIDEnglish.Visible")));
      this.mnuIDEnglish.Popup += new System.EventHandler(this.mnuIDEnglish_Popup);
      // 
      // mnuIDJapanese
      // 
      this.mnuIDJapanese.Enabled = ((bool)(resources.GetObject("mnuIDJapanese.Enabled")));
      this.mnuIDJapanese.Index = 1;
      this.mnuIDJapanese.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuIDJapanese.Shortcut")));
      this.mnuIDJapanese.ShowShortcut = ((bool)(resources.GetObject("mnuIDJapanese.ShowShortcut")));
      this.mnuIDJapanese.Text = resources.GetString("mnuIDJapanese.Text");
      this.mnuIDJapanese.Visible = ((bool)(resources.GetObject("mnuIDJapanese.Visible")));
      this.mnuIDJapanese.Popup += new System.EventHandler(this.mnuIDJapanese_Popup);
      // 
      // mnuStringTables
      // 
      this.mnuStringTables.Enabled = ((bool)(resources.GetObject("mnuStringTables.Enabled")));
      this.mnuStringTables.Index = 2;
      this.mnuStringTables.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
										    this.mnuSTEnglish,
										    this.mnuSTJapanese,
										    this.mnuSTShared});
      this.mnuStringTables.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuStringTables.Shortcut")));
      this.mnuStringTables.ShowShortcut = ((bool)(resources.GetObject("mnuStringTables.ShowShortcut")));
      this.mnuStringTables.Text = resources.GetString("mnuStringTables.Text");
      this.mnuStringTables.Visible = ((bool)(resources.GetObject("mnuStringTables.Visible")));
      this.mnuStringTables.Popup += new System.EventHandler(this.mnuStringTables_Popup);
      // 
      // mnuSTEnglish
      // 
      this.mnuSTEnglish.Enabled = ((bool)(resources.GetObject("mnuSTEnglish.Enabled")));
      this.mnuSTEnglish.Index = 0;
      this.mnuSTEnglish.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuSTEnglish.Shortcut")));
      this.mnuSTEnglish.ShowShortcut = ((bool)(resources.GetObject("mnuSTEnglish.ShowShortcut")));
      this.mnuSTEnglish.Text = resources.GetString("mnuSTEnglish.Text");
      this.mnuSTEnglish.Visible = ((bool)(resources.GetObject("mnuSTEnglish.Visible")));
      this.mnuSTEnglish.Popup += new System.EventHandler(this.mnuSTEnglish_Popup);
      // 
      // mnuSTJapanese
      // 
      this.mnuSTJapanese.Enabled = ((bool)(resources.GetObject("mnuSTJapanese.Enabled")));
      this.mnuSTJapanese.Index = 1;
      this.mnuSTJapanese.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuSTJapanese.Shortcut")));
      this.mnuSTJapanese.ShowShortcut = ((bool)(resources.GetObject("mnuSTJapanese.ShowShortcut")));
      this.mnuSTJapanese.Text = resources.GetString("mnuSTJapanese.Text");
      this.mnuSTJapanese.Visible = ((bool)(resources.GetObject("mnuSTJapanese.Visible")));
      this.mnuSTJapanese.Popup += new System.EventHandler(this.mnuSTJapanese_Popup);
      // 
      // mnuSTShared
      // 
      this.mnuSTShared.Enabled = ((bool)(resources.GetObject("mnuSTShared.Enabled")));
      this.mnuSTShared.Index = 2;
      this.mnuSTShared.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuSTShared.Shortcut")));
      this.mnuSTShared.ShowShortcut = ((bool)(resources.GetObject("mnuSTShared.ShowShortcut")));
      this.mnuSTShared.Text = resources.GetString("mnuSTShared.Text");
      this.mnuSTShared.Visible = ((bool)(resources.GetObject("mnuSTShared.Visible")));
      this.mnuSTShared.Popup += new System.EventHandler(this.mnuSTShared_Popup);
      // 
      // mnuWindows
      // 
      this.mnuWindows.Enabled = ((bool)(resources.GetObject("mnuWindows.Enabled")));
      this.mnuWindows.Index = 3;
      this.mnuWindows.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
									       this.mnuWFileTable});
      this.mnuWindows.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuWindows.Shortcut")));
      this.mnuWindows.ShowShortcut = ((bool)(resources.GetObject("mnuWindows.ShowShortcut")));
      this.mnuWindows.Text = resources.GetString("mnuWindows.Text");
      this.mnuWindows.Visible = ((bool)(resources.GetObject("mnuWindows.Visible")));
      // 
      // mnuWFileTable
      // 
      this.mnuWFileTable.Enabled = ((bool)(resources.GetObject("mnuWFileTable.Enabled")));
      this.mnuWFileTable.Index = 0;
      this.mnuWFileTable.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuWFileTable.Shortcut")));
      this.mnuWFileTable.ShowShortcut = ((bool)(resources.GetObject("mnuWFileTable.ShowShortcut")));
      this.mnuWFileTable.Text = resources.GetString("mnuWFileTable.Text");
      this.mnuWFileTable.Visible = ((bool)(resources.GetObject("mnuWFileTable.Visible")));
      this.mnuWFileTable.Click += new System.EventHandler(this.mnuWFileTable_Click);
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
      this.tabViewerItems.Controls.Add(this.lstItems);
      this.tabViewerItems.Controls.Add(this.label2);
      this.tabViewerItems.Controls.Add(this.cmbItemType);
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
      // lstItems
      // 
      this.lstItems.AccessibleDescription = resources.GetString("lstItems.AccessibleDescription");
      this.lstItems.AccessibleName = resources.GetString("lstItems.AccessibleName");
      this.lstItems.Alignment = ((System.Windows.Forms.ListViewAlignment)(resources.GetObject("lstItems.Alignment")));
      this.lstItems.AllowColumnReorder = true;
      this.lstItems.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lstItems.Anchor")));
      this.lstItems.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("lstItems.BackgroundImage")));
      this.lstItems.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lstItems.Dock")));
      this.lstItems.Enabled = ((bool)(resources.GetObject("lstItems.Enabled")));
      this.lstItems.Font = ((System.Drawing.Font)(resources.GetObject("lstItems.Font")));
      this.lstItems.FullRowSelect = true;
      this.lstItems.GridLines = true;
      this.lstItems.HideSelection = false;
      this.lstItems.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lstItems.ImeMode")));
      this.lstItems.LabelWrap = ((bool)(resources.GetObject("lstItems.LabelWrap")));
      this.lstItems.LargeImageList = this.ilItemIcons;
      this.lstItems.Location = ((System.Drawing.Point)(resources.GetObject("lstItems.Location")));
      this.lstItems.Name = "lstItems";
      this.lstItems.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lstItems.RightToLeft")));
      this.lstItems.Size = ((System.Drawing.Size)(resources.GetObject("lstItems.Size")));
      this.lstItems.SmallImageList = this.ilItemIcons;
      this.lstItems.TabIndex = ((int)(resources.GetObject("lstItems.TabIndex")));
      this.lstItems.Text = resources.GetString("lstItems.Text");
      this.lstItems.View = System.Windows.Forms.View.Details;
      this.lstItems.Visible = ((bool)(resources.GetObject("lstItems.Visible")));
      // 
      // ilItemIcons
      // 
      this.ilItemIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
      this.ilItemIcons.ImageSize = ((System.Drawing.Size)(resources.GetObject("ilItemIcons.ImageSize")));
      this.ilItemIcons.TransparentColor = System.Drawing.Color.Transparent;
      // 
      // label2
      // 
      this.label2.AccessibleDescription = resources.GetString("label2.AccessibleDescription");
      this.label2.AccessibleName = resources.GetString("label2.AccessibleName");
      this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label2.Anchor")));
      this.label2.AutoSize = ((bool)(resources.GetObject("label2.AutoSize")));
      this.label2.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label2.Dock")));
      this.label2.Enabled = ((bool)(resources.GetObject("label2.Enabled")));
      this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.label2.Font = ((System.Drawing.Font)(resources.GetObject("label2.Font")));
      this.label2.Image = ((System.Drawing.Image)(resources.GetObject("label2.Image")));
      this.label2.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label2.ImageAlign")));
      this.label2.ImageIndex = ((int)(resources.GetObject("label2.ImageIndex")));
      this.label2.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label2.ImeMode")));
      this.label2.Location = ((System.Drawing.Point)(resources.GetObject("label2.Location")));
      this.label2.Name = "label2";
      this.label2.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label2.RightToLeft")));
      this.label2.Size = ((System.Drawing.Size)(resources.GetObject("label2.Size")));
      this.label2.TabIndex = ((int)(resources.GetObject("label2.TabIndex")));
      this.label2.Text = resources.GetString("label2.Text");
      this.label2.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label2.TextAlign")));
      this.label2.Visible = ((bool)(resources.GetObject("label2.Visible")));
      // 
      // cmbItemType
      // 
      this.cmbItemType.AccessibleDescription = resources.GetString("cmbItemType.AccessibleDescription");
      this.cmbItemType.AccessibleName = resources.GetString("cmbItemType.AccessibleName");
      this.cmbItemType.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("cmbItemType.Anchor")));
      this.cmbItemType.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("cmbItemType.BackgroundImage")));
      this.cmbItemType.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("cmbItemType.Dock")));
      this.cmbItemType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbItemType.Enabled = ((bool)(resources.GetObject("cmbItemType.Enabled")));
      this.cmbItemType.Font = ((System.Drawing.Font)(resources.GetObject("cmbItemType.Font")));
      this.cmbItemType.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("cmbItemType.ImeMode")));
      this.cmbItemType.IntegralHeight = ((bool)(resources.GetObject("cmbItemType.IntegralHeight")));
      this.cmbItemType.ItemHeight = ((int)(resources.GetObject("cmbItemType.ItemHeight")));
      this.cmbItemType.Items.AddRange(new object[] {
						     resources.GetString("cmbItemType.Items"),
						     resources.GetString("cmbItemType.Items1"),
						     resources.GetString("cmbItemType.Items2"),
						     resources.GetString("cmbItemType.Items3"),
						     resources.GetString("cmbItemType.Items4"),
						     resources.GetString("cmbItemType.Items5")});
      this.cmbItemType.Location = ((System.Drawing.Point)(resources.GetObject("cmbItemType.Location")));
      this.cmbItemType.MaxDropDownItems = ((int)(resources.GetObject("cmbItemType.MaxDropDownItems")));
      this.cmbItemType.MaxLength = ((int)(resources.GetObject("cmbItemType.MaxLength")));
      this.cmbItemType.Name = "cmbItemType";
      this.cmbItemType.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("cmbItemType.RightToLeft")));
      this.cmbItemType.Size = ((System.Drawing.Size)(resources.GetObject("cmbItemType.Size")));
      this.cmbItemType.TabIndex = ((int)(resources.GetObject("cmbItemType.TabIndex")));
      this.cmbItemType.Text = resources.GetString("cmbItemType.Text");
      this.cmbItemType.Visible = ((bool)(resources.GetObject("cmbItemType.Visible")));
      this.cmbItemType.SelectedIndexChanged += new System.EventHandler(this.cmbItemType_SelectedIndexChanged);
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
      this.lblImageChooser.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblImageChooser.Dock")));
      this.lblImageChooser.Enabled = ((bool)(resources.GetObject("lblImageChooser.Enabled")));
      this.lblImageChooser.FlatStyle = System.Windows.Forms.FlatStyle.System;
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
      this.lstEntries.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
										 this.colXIEntryNum,
										 this.colXIEntryText});
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
      // 
      // colXIEntryNum
      // 
      this.colXIEntryNum.Text = resources.GetString("colXIEntryNum.Text");
      this.colXIEntryNum.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("colXIEntryNum.TextAlign")));
      this.colXIEntryNum.Width = ((int)(resources.GetObject("colXIEntryNum.Width")));
      // 
      // colXIEntryText
      // 
      this.colXIEntryText.Text = resources.GetString("colXIEntryText.Text");
      this.colXIEntryText.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("colXIEntryText.TextAlign")));
      this.colXIEntryText.Width = ((int)(resources.GetObject("colXIEntryText.Width")));
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
      FileScanDialog FSD = new FileScanDialog(FileName);
	this.Enabled = false;
	FSD.ShowDialog(this);
	this.Enabled = true;
	if (FSD.StringTableEntries.Count > 0) {
	  this.tabViewers.Visible = true;
	  this.tabViewers.TabPages.Add(this.tabViewerStringTable);
	  Application.DoEvents();
	int i = 0;
	  foreach (string S in FSD.StringTableEntries) {
	    this.lstEntries.Items.Add(String.Format("{0:00000000}", ++i)).SubItems.Add(S);
	    Application.DoEvents();
	  }
	}
	if (FSD.Items.Count > 0) {
	  this.tabViewers.Visible = true;
	  this.cmbItemType.SelectedItem = null;
	  this.tabViewers.TabPages.Add(this.tabViewerItems);
	  this.tabViewers.TabPages.Add(this.tabViewerImages);
	  Application.DoEvents();
	  this.LoadedItems = FSD.Items.ToArray(typeof(FFXIItem)) as FFXIItem[];
	  Application.DoEvents();
	  this.cmbItemType.SelectedIndex = 0;
	}
	if (FSD.Images.Count > 0) {
	  this.tabViewers.Visible = true;
	  this.cmbImageChooser.SelectedItem = null;
	  this.tabViewers.TabPages.Add(this.tabViewerImages);
	  this.cmbImageChooser.Items.AddRange(FSD.Images.ToArray());
	  Application.DoEvents();
	  this.cmbImageChooser.SelectedIndex = 0;
	}
	if (!this.tabViewers.Visible)
	  this.pnlNoViewers.Visible = true;
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

    private void mnuSTCCopy_Click(object sender, System.EventArgs e) {
    string ItemText = String.Empty;
      foreach (ListViewItem LVI in this.lstEntries.SelectedItems) {
	if (ItemText != String.Empty)
	  ItemText += '\n';
	ItemText += LVI.SubItems[1].Text;
      }
      if (ItemText != String.Empty)
	Clipboard.SetDataObject(ItemText);
    }

    #endregion

    #region Main Menu Events

    private readonly string DummyText = "--DUMMY--";

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

    private void ROMMenuItem_Click(object sender, System.EventArgs e)
    {
    ROMMenuItem RMI = sender as ROMMenuItem;
      if (RMI != null)
	this.SelectEntry(RMI.ROMApp, RMI.ROMDir, RMI.ROMFile);
    }

    #region Images

    private void mnuImages_Popup(object sender, System.EventArgs e) {
      if (this.mnuIOther.MenuItems.Count == 0) this.mnuIOther.MenuItems.Add(this.DummyText);
    }

    #region Maps

    private void mnuIMaps_Popup(object sender, System.EventArgs e) {
      foreach (MenuItem MI in this.mnuIMaps.MenuItems) {
	if (MI.MenuItems.Count == 0)
	  MI.MenuItems.Add(this.DummyText);
      }
    }

    private void mnuIMSandoria_Popup(object sender, System.EventArgs e) {
    MenuItem RegionMenu = sender as MenuItem;
      if (RegionMenu != null && RegionMenu.MenuItems.Count == 1 && RegionMenu.MenuItems[0].Text == this.DummyText) {
	RegionMenu.MenuItems.Clear();
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("The Kingdom of San d'Oria");
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("Highlight: Southern San d'Oria", 0, 18, 63, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("Highlight: Northern San d'Oria", 0, 18, 86, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("Highlight: Port San d'Oria",     0, 18, 87, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("Highlight: Château d'Oraguille", 0, 18, 88, new EventHandler(this.ROMMenuItem_Click)));
	}
	RegionMenu.MenuItems.Add(new ROMMenuItem("Southern San d'Oria", 0, 18, 59, new EventHandler(this.ROMMenuItem_Click)));
	RegionMenu.MenuItems.Add(new ROMMenuItem("Northern San d'Oria", 0, 18, 60, new EventHandler(this.ROMMenuItem_Click)));
	RegionMenu.MenuItems.Add(new ROMMenuItem("Port San d'Oria",     0, 18, 61, new EventHandler(this.ROMMenuItem_Click)));
	RegionMenu.MenuItems.Add(new ROMMenuItem("Château d'Oraguille", 0, 18, 62, new EventHandler(this.ROMMenuItem_Click)));
      }
    }

    private void mnuIMBastok_Popup(object sender, System.EventArgs e) {
    MenuItem RegionMenu = sender as MenuItem;
      if (RegionMenu != null && RegionMenu.MenuItems.Count == 1 && RegionMenu.MenuItems[0].Text == this.DummyText) {
	RegionMenu.MenuItems.Clear();
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("The Republic of Bastok");
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("Highlight: Bastok Mines",   0, 18, 68, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("Highlight: Bastok Markets", 0, 18, 89, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("Highlight: Port Bastok",    0, 18, 90, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("Highlight: Metalworks",     0, 18, 91, new EventHandler(this.ROMMenuItem_Click)));
	}
	RegionMenu.MenuItems.Add(new ROMMenuItem("Bastok Mines",   0, 18, 64, new EventHandler(this.ROMMenuItem_Click)));
	RegionMenu.MenuItems.Add(new ROMMenuItem("Bastok Markets", 0, 18, 65, new EventHandler(this.ROMMenuItem_Click)));
	RegionMenu.MenuItems.Add(new ROMMenuItem("Port Bastok",    0, 18, 66, new EventHandler(this.ROMMenuItem_Click)));
	RegionMenu.MenuItems.Add(new ROMMenuItem("Metalworks",     0, 18, 67, new EventHandler(this.ROMMenuItem_Click)));
      }
    }

    private void mnuIMWindurst_Popup(object sender, System.EventArgs e) {
    MenuItem RegionMenu = sender as MenuItem;
      if (RegionMenu != null && RegionMenu.MenuItems.Count == 1 && RegionMenu.MenuItems[0].Text == this.DummyText) {
	RegionMenu.MenuItems.Clear();
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("The Federation of Windurst");
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("Highlight: Windurst Waters", 0, 18, 74, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("Highlight: Windurst Walls",  0, 18, 92, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("Highlight: Port Windurst",   0, 18, 93, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("Highlight: Windurst Woods",  0, 18, 94, new EventHandler(this.ROMMenuItem_Click)));
	}
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("Windurst Waters");
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("Full",       0, 18,  69, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("North Half", 0, 18,  84, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("South Half", 0, 18,  85, new EventHandler(this.ROMMenuItem_Click)));
	}
	RegionMenu.MenuItems.Add(new ROMMenuItem("Windurst Walls", 0, 18,  70, new EventHandler(this.ROMMenuItem_Click)));
	RegionMenu.MenuItems.Add(new ROMMenuItem("Port Windurst",  0, 18,  71, new EventHandler(this.ROMMenuItem_Click)));
	RegionMenu.MenuItems.Add(new ROMMenuItem("Windurst Woods", 0, 18,  72, new EventHandler(this.ROMMenuItem_Click)));
      }
    }

    private void mnuIMJeuno_Popup(object sender, System.EventArgs e) {
    MenuItem RegionMenu = sender as MenuItem;
      if (RegionMenu != null && RegionMenu.MenuItems.Count == 1 && RegionMenu.MenuItems[0].Text == this.DummyText) {
	RegionMenu.MenuItems.Clear();
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("The Grand Duchy of Jeuno");
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("Highlight: Ru'Lude Gardens", 0, 18, 79, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("Highlight: Upper Jeuno",     0, 18, 95, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("Highlight: Lower Jeuno",     0, 18, 96, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("Highlight: Port Jeuno",      0, 18, 97, new EventHandler(this.ROMMenuItem_Click)));
	}
	RegionMenu.MenuItems.Add(new ROMMenuItem("Ru'Lude Gardens",          0, 18,  75, new EventHandler(this.ROMMenuItem_Click)));
	RegionMenu.MenuItems.Add(new ROMMenuItem("Upper Jeuno",              0, 18,  76, new EventHandler(this.ROMMenuItem_Click)));
	RegionMenu.MenuItems.Add(new ROMMenuItem("Lower Jeuno",              0, 18,  77, new EventHandler(this.ROMMenuItem_Click)));
	RegionMenu.MenuItems.Add(new ROMMenuItem("Port Jeuno",               0, 18,  78, new EventHandler(this.ROMMenuItem_Click)));
      }
    }

    private void mnuIMRonfaure_Popup(object sender, System.EventArgs e) {
    MenuItem RegionMenu = sender as MenuItem;
      if (RegionMenu != null && RegionMenu.MenuItems.Count == 1 && RegionMenu.MenuItems[0].Text == this.DummyText) {
	RegionMenu.MenuItems.Clear();
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("Bostaunieux Oubliette");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 17, 112, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 17, 113, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 17, 114, new EventHandler(this.ROMMenuItem_Click)));
	}
	RegionMenu.MenuItems.Add(new ROMMenuItem("East Ronfaure", 0, 17,  25, new EventHandler(this.ROMMenuItem_Click)));
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("Ghelsba Outpost");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 17, 51, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 17, 52, new EventHandler(this.ROMMenuItem_Click)));
	}
	RegionMenu.MenuItems.Add(new ROMMenuItem("Fort Ghelsba", 0, 17,  53, new EventHandler(this.ROMMenuItem_Click)));
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("King Ranperre's Tomb");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18,  8, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18, 11, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18, 12, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18, 13, new EventHandler(this.ROMMenuItem_Click)));
	}
	RegionMenu.MenuItems.Add(new ROMMenuItem("West Ronfaure", 0, 17,  24, new EventHandler(this.ROMMenuItem_Click)));
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("Yughott Grotto");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 17,  54, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18, 112, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18, 113, new EventHandler(this.ROMMenuItem_Click)));
	}
      }
    }

    private void mnuIMZulkheim_Popup(object sender, System.EventArgs e) {
    MenuItem RegionMenu = sender as MenuItem;
      if (RegionMenu != null && RegionMenu.MenuItems.Count == 1 && RegionMenu.MenuItems[0].Text == this.DummyText) {
	RegionMenu.MenuItems.Clear();
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("Gusgen Mines");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18,  26, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18,  27, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18,  28, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18,  29, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18, 117, new EventHandler(this.ROMMenuItem_Click)));
	}
	RegionMenu.MenuItems.Add(new ROMMenuItem("La Theine Plateau",   0, 17, 26, new EventHandler(this.ROMMenuItem_Click)));
	RegionMenu.MenuItems.Add(new ROMMenuItem("Konschtat Highlands", 0, 17, 32, new EventHandler(this.ROMMenuItem_Click)));
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("Ordelle's Caves");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18,  20, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18,  21, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18,  22, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18, 114, new EventHandler(this.ROMMenuItem_Click)));
	}
	RegionMenu.MenuItems.Add(new ROMMenuItem("Selbina",       0, 18, 80, new EventHandler(this.ROMMenuItem_Click)));
	RegionMenu.MenuItems.Add(new ROMMenuItem("Valkurm Dunes", 0, 17, 27, new EventHandler(this.ROMMenuItem_Click)));
      }
    }

    private void mnuIMNorvallen_Popup(object sender, System.EventArgs e) {
    MenuItem RegionMenu = sender as MenuItem;
      if (RegionMenu != null && RegionMenu.MenuItems.Count == 1 && RegionMenu.MenuItems[0].Text == this.DummyText) {
	RegionMenu.MenuItems.Clear();
	RegionMenu.MenuItems.Add(new ROMMenuItem("Batallia Downs",  0, 17,  29, new EventHandler(this.ROMMenuItem_Click)));
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("Carpenter's Landing");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 2, 3, 99, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 2, 7, 38, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 2, 7, 38, new EventHandler(this.ROMMenuItem_Click)));
	}
	RegionMenu.MenuItems.Add(new ROMMenuItem("Davoi",            0, 17,  66, new EventHandler(this.ROMMenuItem_Click)));
	RegionMenu.MenuItems.Add(new ROMMenuItem("Jugner Forest",    0, 17,  28, new EventHandler(this.ROMMenuItem_Click)));
	RegionMenu.MenuItems.Add(new ROMMenuItem("Monastic Cavern",  0, 17,  67, new EventHandler(this.ROMMenuItem_Click)));
	RegionMenu.MenuItems.Add(new ROMMenuItem("Phanauet Channel", 2,  8,  31, new EventHandler(this.ROMMenuItem_Click)));
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("The Eldieme Necropolis");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18,  23, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18,  24, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18,  25, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18, 116, new EventHandler(this.ROMMenuItem_Click)));
	}
      }
    }

    private void mnuIMGustaberg_Popup(object sender, System.EventArgs e) {
    MenuItem RegionMenu = sender as MenuItem;
      if (RegionMenu != null && RegionMenu.MenuItems.Count == 1 && RegionMenu.MenuItems[0].Text == this.DummyText) {
	RegionMenu.MenuItems.Clear();
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("Dangruf Wadi");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18,  9, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18, 10, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18, 14, new EventHandler(this.ROMMenuItem_Click)));
	}
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("Korroloka Tunnel");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 17, 121, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 16, 110, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 16, 111, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 16, 112, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 16, 113, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 16, 114, new EventHandler(this.ROMMenuItem_Click)));
	}
	RegionMenu.MenuItems.Add(new ROMMenuItem("North Gustaberg",  0, 17,  30, new EventHandler(this.ROMMenuItem_Click)));
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("Palborough Mines");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 17, 55, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 17, 56, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 17, 57, new EventHandler(this.ROMMenuItem_Click)));
	}
	RegionMenu.MenuItems.Add(new ROMMenuItem("South Gustaberg", 0, 17,  31, new EventHandler(this.ROMMenuItem_Click)));
	RegionMenu.MenuItems.Add(new ROMMenuItem("Zeruhn Mines",    0, 17, 120, new EventHandler(this.ROMMenuItem_Click)));
      }
    }

    private void mnuIMDerfland_Popup(object sender, System.EventArgs e) {
    MenuItem RegionMenu = sender as MenuItem;
      if (RegionMenu != null && RegionMenu.MenuItems.Count == 1 && RegionMenu.MenuItems[0].Text == this.DummyText) {
	RegionMenu.MenuItems.Clear();
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("Beadeaux");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 17, 62, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 17, 63, new EventHandler(this.ROMMenuItem_Click)));
	}
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("Crawlers' Nest");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18,  30, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18,  31, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18, 111, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18, 118, new EventHandler(this.ROMMenuItem_Click)));
	}
	RegionMenu.MenuItems.Add(new ROMMenuItem("Passhow Marshlands", 0, 17,  33, new EventHandler(this.ROMMenuItem_Click)));
	RegionMenu.MenuItems.Add(new ROMMenuItem("Qulun Dome",         0, 17,  65, new EventHandler(this.ROMMenuItem_Click)));
	RegionMenu.MenuItems.Add(new ROMMenuItem("Rolanberry Fields",  0, 17,  34, new EventHandler(this.ROMMenuItem_Click)));
      }
    }

    private void mnuIMSarutabaruta_Popup(object sender, System.EventArgs e) {
    MenuItem RegionMenu = sender as MenuItem;
      if (RegionMenu != null && RegionMenu.MenuItems.Count == 1 && RegionMenu.MenuItems[0].Text == this.DummyText) {
	RegionMenu.MenuItems.Clear();
	RegionMenu.MenuItems.Add(new ROMMenuItem("East Sarutabaruta", 0, 17, 40, new EventHandler(this.ROMMenuItem_Click)));
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("Giddeus");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 17, 59, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 17, 60, new EventHandler(this.ROMMenuItem_Click)));
	}
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("Horutoto Ruins");
	  {
	  MenuItem TowerMenu = ZoneMenu.MenuItems.Add("Amaryllis Tower");
	  int MapNum = 0;
	    TowerMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18,  15, new EventHandler(this.ROMMenuItem_Click)));
	    TowerMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18,  50, new EventHandler(this.ROMMenuItem_Click)));
	    TowerMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18, 115, new EventHandler(this.ROMMenuItem_Click)));
	  }
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("Beetle's Burrow",  0, 18,  45, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("Dahlia Tower",     0, 18,  19, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("Lilac Tower",      0, 18,  16, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("Lily Tower",       0, 18,  18, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("Marguerite Tower", 0, 18,  17, new EventHandler(this.ROMMenuItem_Click)));
	  {
	  MenuItem TowerMenu = ZoneMenu.MenuItems.Add("Rose Tower");
	  int MapNum = 0;
	    TowerMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18, 46, new EventHandler(this.ROMMenuItem_Click)));
	    TowerMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18, 49, new EventHandler(this.ROMMenuItem_Click)));
	  }
	}
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("Toraimarai Canal");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 17, 117, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18,  48, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 16, 109, new EventHandler(this.ROMMenuItem_Click)));
	}
	RegionMenu.MenuItems.Add(new ROMMenuItem("West Sarutabaruta", 0, 17, 39, new EventHandler(this.ROMMenuItem_Click)));
      }
    }

    private void mnuIMKolshushu_Popup(object sender, System.EventArgs e) {
    MenuItem RegionMenu = sender as MenuItem;
      if (RegionMenu != null && RegionMenu.MenuItems.Count == 1 && RegionMenu.MenuItems[0].Text == this.DummyText) {
	RegionMenu.MenuItems.Clear();
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("Bibiki Bay");
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("Mainland",        2, 3, 100, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("Purgonorgo Isle", 2, 3, 101, new EventHandler(this.ROMMenuItem_Click)));
	}
	RegionMenu.MenuItems.Add(new ROMMenuItem("Buburimu Peninsula",  0, 17, 42, new EventHandler(this.ROMMenuItem_Click)));
	RegionMenu.MenuItems.Add(new ROMMenuItem("Labyrinth of Onzozo", 1, 17, 27, new EventHandler(this.ROMMenuItem_Click)));
	RegionMenu.MenuItems.Add(new ROMMenuItem("Manaclipper",         2,  8, 32, new EventHandler(this.ROMMenuItem_Click)));
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("Maze of Shakhrami");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18,  32, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18,  33, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18, 119, new EventHandler(this.ROMMenuItem_Click)));
	}
	RegionMenu.MenuItems.Add(new ROMMenuItem("Mhaura",          0, 18, 81, new EventHandler(this.ROMMenuItem_Click)));
	RegionMenu.MenuItems.Add(new ROMMenuItem("Tahrongi Canyon", 0, 17, 41, new EventHandler(this.ROMMenuItem_Click)));
      }
    }

    private void mnuIMAragoneu_Popup(object sender, System.EventArgs e) {
    MenuItem RegionMenu = sender as MenuItem;
      if (RegionMenu != null && RegionMenu.MenuItems.Count == 1 && RegionMenu.MenuItems[0].Text == this.DummyText) {
	RegionMenu.MenuItems.Clear();
	RegionMenu.MenuItems.Add(new ROMMenuItem("Altar Room",    0, 17,  72, new EventHandler(this.ROMMenuItem_Click)));
	RegionMenu.MenuItems.Add(new ROMMenuItem("Attohwa Chasm", 2,  3, 103, new EventHandler(this.ROMMenuItem_Click)));
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("Castle Oztroja");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 17, 68, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 17, 69, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 17, 70, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 17, 71, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18, 47, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18, 51, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18, 52, new EventHandler(this.ROMMenuItem_Click)));
	}
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("Garlaige Citadel");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18,  34, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18,  42, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18,  43, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18,  44, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18, 120, new EventHandler(this.ROMMenuItem_Click)));
	}
	RegionMenu.MenuItems.Add(new ROMMenuItem("Meriphataud Mountains", 0, 17,  43, new EventHandler(this.ROMMenuItem_Click)));
	RegionMenu.MenuItems.Add(new ROMMenuItem("Sauromugue Champaign",  0, 17,  44, new EventHandler(this.ROMMenuItem_Click)));
      }
    }

    private void mnuIMFauregandi_Popup(object sender, System.EventArgs e) {
    MenuItem RegionMenu = sender as MenuItem;
      if (RegionMenu != null && RegionMenu.MenuItems.Count == 1 && RegionMenu.MenuItems[0].Text == this.DummyText) {
	RegionMenu.MenuItems.Clear();
	RegionMenu.MenuItems.Add(new ROMMenuItem("Beaucedine Glacier", 0, 17,  35, new EventHandler(this.ROMMenuItem_Click)));
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("Fei'yin");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18,  37, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18,  38, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18, 121, new EventHandler(this.ROMMenuItem_Click)));
	}
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("Pso'Xja");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 2, 3, 104, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 2, 3, 105, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 2, 3, 106, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 2, 3, 107, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 2, 3, 108, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 2, 3, 109, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 2, 3, 110, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 2, 3, 111, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 2, 3, 112, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 2, 3, 113, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 2, 3, 114, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 2, 3, 115, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 2, 3, 116, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 2, 3, 117, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 2, 3, 118, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 2, 3, 119, new EventHandler(this.ROMMenuItem_Click)));
	}
	RegionMenu.MenuItems.Add(new ROMMenuItem("Ranguemont Pass", 0, 17, 111, new EventHandler(this.ROMMenuItem_Click)));
      }
    }

    private void mnuIMValdeaunia_Popup(object sender, System.EventArgs e) {
    MenuItem RegionMenu = sender as MenuItem;
      if (RegionMenu != null && RegionMenu.MenuItems.Count == 1 && RegionMenu.MenuItems[0].Text == this.DummyText) {
	RegionMenu.MenuItems.Clear();
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("Castle Zvahl Baileys");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 17, 101, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18,  98, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18,  99, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18, 100, new EventHandler(this.ROMMenuItem_Click)));
	}
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("Castle Zvahl Keep");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 17, 107, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 17, 108, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 17, 109, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 17, 110, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18, 107, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18, 108, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18, 109, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18, 110, new EventHandler(this.ROMMenuItem_Click)));
	}
	RegionMenu.MenuItems.Add(new ROMMenuItem("Uleguerand Range", 2,  3, 102, new EventHandler(this.ROMMenuItem_Click)));
	RegionMenu.MenuItems.Add(new ROMMenuItem("Xarcabard",        0, 17,  36, new EventHandler(this.ROMMenuItem_Click)));
      }
    }

    private void mnuIMQufim_Popup(object sender, System.EventArgs e) {
    MenuItem RegionMenu = sender as MenuItem;
      if (RegionMenu != null && RegionMenu.MenuItems.Count == 1 && RegionMenu.MenuItems[0].Text == this.DummyText) {
	RegionMenu.MenuItems.Clear();
	RegionMenu.MenuItems.Add(new ROMMenuItem("Behemoth's Dominion",   0, 17,  48, new EventHandler(this.ROMMenuItem_Click)));
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("Delkfutt's Tower");
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("Basement",          0, 17,  96, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("First Floor",       0, 17,  84, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("Second Floor",      0, 17,  85, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("Third Floor",       0, 17,  86, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("Fourth Floor",      0, 17,  87, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("Fifth Floor",       0, 17,  88, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("Sixth Floor",       0, 17,  89, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("Seventh Floor",     0, 17,  90, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("Eighth Floor",      0, 17,  91, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("Ninth Floor",       0, 17,  92, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("Tenth Floor",       0, 17,  93, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("Eleventh Floor",    0, 17,  94, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("Twelfth Floor",     0, 17,  95, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("Hidden Elevator 1", 0, 18, 101, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("Hidden Elevator 2", 0, 18, 102, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem("Hidden Elevator 3", 0, 18, 103, new EventHandler(this.ROMMenuItem_Click)));
	}
	RegionMenu.MenuItems.Add(new ROMMenuItem("Qufim Island",          0, 17,  47, new EventHandler(this.ROMMenuItem_Click)));
      }
    }

    private void mnuIMLiTelor_Popup(object sender, System.EventArgs e) {
    MenuItem RegionMenu = sender as MenuItem;
      if (RegionMenu != null && RegionMenu.MenuItems.Count == 1 && RegionMenu.MenuItems[0].Text == this.DummyText) {
	RegionMenu.MenuItems.Clear();
	RegionMenu.MenuItems.Add(new ROMMenuItem("Dragon's Aery",    1, 16, 95, new EventHandler(this.ROMMenuItem_Click)));
	RegionMenu.MenuItems.Add(new ROMMenuItem("Hall of the Gods", 1, 17, 31, new EventHandler(this.ROMMenuItem_Click)));
	RegionMenu.MenuItems.Add(new ROMMenuItem("Ro'Maeve",         1, 16, 85, new EventHandler(this.ROMMenuItem_Click)));
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("The Boyahda Tree");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 16, 91, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 16, 92, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 16, 93, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 16, 94, new EventHandler(this.ROMMenuItem_Click)));
	}
	RegionMenu.MenuItems.Add(new ROMMenuItem("The Sanctuary of Zi'Tah", 1, 16, 84, new EventHandler(this.ROMMenuItem_Click)));
      }
    }

    private void mnuIMKuzotz_Popup(object sender, System.EventArgs e) {
    MenuItem RegionMenu = sender as MenuItem;
      if (RegionMenu != null && RegionMenu.MenuItems.Count == 1 && RegionMenu.MenuItems[0].Text == this.DummyText) {
	RegionMenu.MenuItems.Clear();
	RegionMenu.MenuItems.Add(new ROMMenuItem("Eastern Altepa Desert", 1, 16, 83, new EventHandler(this.ROMMenuItem_Click)));
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("Quicksand Caves");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 17, 19, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 17, 20, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 17, 21, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 17, 22, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 17, 23, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 17, 24, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 24,  7, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 24,  8, new EventHandler(this.ROMMenuItem_Click)));
	}
	RegionMenu.MenuItems.Add(new ROMMenuItem("Rabao",                 1, 17, 29, new EventHandler(this.ROMMenuItem_Click)));
	RegionMenu.MenuItems.Add(new ROMMenuItem("Western Altepa Desert", 1, 16, 88, new EventHandler(this.ROMMenuItem_Click)));
      }
    }

    private void mnuIMVollbow_Popup(object sender, System.EventArgs e) {
    MenuItem RegionMenu = sender as MenuItem;
      if (RegionMenu != null && RegionMenu.MenuItems.Count == 1 && RegionMenu.MenuItems[0].Text == this.DummyText) {
	RegionMenu.MenuItems.Clear();
	RegionMenu.MenuItems.Add(new ROMMenuItem("Cape Terrigan",     1, 16, 82, new EventHandler(this.ROMMenuItem_Click)));
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("Gustav Tunnel");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 17, 25, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 17, 26, new EventHandler(this.ROMMenuItem_Click)));
	}
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("Kuftal Tunnel");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 17, 122, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 16, 115, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 16, 116, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 16, 117, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 16, 118, new EventHandler(this.ROMMenuItem_Click)));
	}
	RegionMenu.MenuItems.Add(new ROMMenuItem("Valley of Sorrows", 1, 16, 89, new EventHandler(this.ROMMenuItem_Click)));
      }
    }

    private void mnuIMElshimoLow_Popup(object sender, System.EventArgs e) {
    MenuItem RegionMenu = sender as MenuItem;
      if (RegionMenu != null && RegionMenu.MenuItems.Count == 1 && RegionMenu.MenuItems[0].Text == this.DummyText) {
	RegionMenu.MenuItems.Clear();
	RegionMenu.MenuItems.Add(new ROMMenuItem("Kazham", 1, 17, 30, new EventHandler(this.ROMMenuItem_Click)));
	RegionMenu.MenuItems.Add(new ROMMenuItem("Norg",   1, 17, 32, new EventHandler(this.ROMMenuItem_Click)));
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("Sea Serpent Grotto");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 16, 119, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 16, 120, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 16, 121, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 16, 122, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 16, 123, new EventHandler(this.ROMMenuItem_Click)));
	}
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("The Cavern of Flames");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18, 39, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 0, 18, 40, new EventHandler(this.ROMMenuItem_Click)));
	}
	RegionMenu.MenuItems.Add(new ROMMenuItem("Yuhtunga Jungle", 1, 16, 86, new EventHandler(this.ROMMenuItem_Click)));
      }
    }

    private void mnuIMElshimoUp_Popup(object sender, System.EventArgs e) {
    MenuItem RegionMenu = sender as MenuItem;
      if (RegionMenu != null && RegionMenu.MenuItems.Count == 1 && RegionMenu.MenuItems[0].Text == this.DummyText) {
	RegionMenu.MenuItems.Clear();
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("Den of Rancor");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 16, 100, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 16, 101, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 16, 102, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 16, 103, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 16, 104, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 16, 105, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 16, 106, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 16, 107, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 16, 108, new EventHandler(this.ROMMenuItem_Click)));
	}
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("Ifrit's Cauldron");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 17, 11, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 17, 12, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 17, 13, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 17, 14, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 17, 15, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 17, 16, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 17, 17, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 17, 18, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 24,  6, new EventHandler(this.ROMMenuItem_Click)));
	}
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("Temple of Uggalepih");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 16, 96, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 16, 97, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 16, 98, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 16, 99, new EventHandler(this.ROMMenuItem_Click)));
	}
	RegionMenu.MenuItems.Add(new ROMMenuItem("Yhoator Jungle", 1, 16, 87, new EventHandler(this.ROMMenuItem_Click)));
      }
    }

    private void mnuIMTuLia_Popup(object sender, System.EventArgs e) {
    MenuItem RegionMenu = sender as MenuItem;
      if (RegionMenu != null && RegionMenu.MenuItems.Count == 1 && RegionMenu.MenuItems[0].Text == this.DummyText) {
	RegionMenu.MenuItems.Clear();
	RegionMenu.MenuItems.Add(new ROMMenuItem("Ru'Aun Gardens", 1, 16, 90, new EventHandler(this.ROMMenuItem_Click)));
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("The Shrine Ru'Avitau");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 17,  5, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 17,  6, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 17,  7, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 17,  8, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 17,  9, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 17, 10, new EventHandler(this.ROMMenuItem_Click)));
	}
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("Ve'Lugannon Palace");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 16, 124, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 16, 125, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 16, 126, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 16, 127, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 17,   0, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 17,   1, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 17,   2, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 17,   3, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 1, 17,   4, new EventHandler(this.ROMMenuItem_Click)));
	}
      }
    }

    private void mnuIMMovalpolos_Popup(object sender, System.EventArgs e) {
    MenuItem RegionMenu = sender as MenuItem;
      if (RegionMenu != null && RegionMenu.MenuItems.Count == 1 && RegionMenu.MenuItems[0].Text == this.DummyText) {
	RegionMenu.MenuItems.Clear();
	RegionMenu.MenuItems.Add(new ROMMenuItem("Oldton Movalpolos", 2, 3, 120, new EventHandler(this.ROMMenuItem_Click)));
	RegionMenu.MenuItems.Add(new ROMMenuItem("Newton Movalpolos", 2, 3, 121, new EventHandler(this.ROMMenuItem_Click)));
      }
    }

    private void mnuIMTavMarquisate_Popup(object sender, System.EventArgs e) {
    MenuItem RegionMenu = sender as MenuItem;
      if (RegionMenu != null && RegionMenu.MenuItems.Count == 1 && RegionMenu.MenuItems[0].Text == this.DummyText) {
	RegionMenu.MenuItems.Clear();
	RegionMenu.MenuItems.Add(new ROMMenuItem("Al'taieu", 2, 4, 14, new EventHandler(this.ROMMenuItem_Click)));
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("Diorama Abdhaljs");
	  RegionMenu.MenuItems.Add(new ROMMenuItem("Ghelsba Outpost",   2, 8, 33, new EventHandler(this.ROMMenuItem_Click)));
	  RegionMenu.MenuItems.Add(new ROMMenuItem("Purgonorgo Isle 1", 2, 8, 34, new EventHandler(this.ROMMenuItem_Click)));
	  RegionMenu.MenuItems.Add(new ROMMenuItem("Purgonorgo Isle 2", 2, 8, 35, new EventHandler(this.ROMMenuItem_Click)));
	}
	RegionMenu.MenuItems.Add(new ROMMenuItem("Al'taieu", 2, 4, 14, new EventHandler(this.ROMMenuItem_Click)));
	RegionMenu.MenuItems.Add(new ROMMenuItem("Al'taieu", 2, 4, 14, new EventHandler(this.ROMMenuItem_Click)));
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("Grand Palace of Hu'Xzoi");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 2, 4,  14, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 2, 4,  15, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 2, 4,  16, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 2, 5, 120, new EventHandler(this.ROMMenuItem_Click)));
	}
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("The Garden of Ru'Hmet");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 2, 4, 17, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 2, 4, 18, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 2, 4, 19, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 2, 4, 20, new EventHandler(this.ROMMenuItem_Click)));
	}
      }
    }

    private void mnuIMTavArchipelago_Popup(object sender, System.EventArgs e) {
    MenuItem RegionMenu = sender as MenuItem;
      if (RegionMenu != null && RegionMenu.MenuItems.Count == 1 && RegionMenu.MenuItems[0].Text == this.DummyText) {
	RegionMenu.MenuItems.Clear();
	RegionMenu.MenuItems.Add(new ROMMenuItem("Lufaise Meadows", 2, 4, 1, new EventHandler(this.ROMMenuItem_Click)));
	RegionMenu.MenuItems.Add(new ROMMenuItem("Misareaux Coast", 2, 4, 1, new EventHandler(this.ROMMenuItem_Click)));
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("Phomiuna Aqueducts");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 2, 4,   6, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 2, 4,   7, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 2, 5, 118, new EventHandler(this.ROMMenuItem_Click)));
	}
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("Riverne - Site #A01");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 2, 4, 12, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 2, 4, 13, new EventHandler(this.ROMMenuItem_Click)));
	}
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("Riverne - Site #B01");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 2, 4, 10, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 2, 4, 11, new EventHandler(this.ROMMenuItem_Click)));
	}
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("Sacrarium");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 2, 4, 8, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 2, 4, 9, new EventHandler(this.ROMMenuItem_Click)));
	}
	RegionMenu.MenuItems.Add(new ROMMenuItem("Sealion's Den", 2, 5, 119, new EventHandler(this.ROMMenuItem_Click)));
	{
	MenuItem ZoneMenu = RegionMenu.MenuItems.Add("Tavnazian Safehold");
	int MapNum = 0;
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 2, 4, 3, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 2, 4, 4, new EventHandler(this.ROMMenuItem_Click)));
	  ZoneMenu.MenuItems.Add(new ROMMenuItem(String.Format(I18N.GetText("Menu:MapN"), ++MapNum), 2, 4, 5, new EventHandler(this.ROMMenuItem_Click)));
	}
      }
    }

    private void mnuIMPromyvion_Popup(object sender, System.EventArgs e) {
    MenuItem RegionMenu = sender as MenuItem;
      if (RegionMenu != null && RegionMenu.MenuItems.Count == 1 && RegionMenu.MenuItems[0].Text == this.DummyText) {
	RegionMenu.MenuItems.Clear();
	RegionMenu.MenuItems.Add(new ROMMenuItem("Hall of Transference (Dem?)",   2, 3, 123, new EventHandler(this.ROMMenuItem_Click)));
	RegionMenu.MenuItems.Add(new ROMMenuItem("Hall of Transference (Holla?)", 2, 3, 122, new EventHandler(this.ROMMenuItem_Click)));
	RegionMenu.MenuItems.Add(new ROMMenuItem("Hall of Transference (Mea?)",   2, 3, 124, new EventHandler(this.ROMMenuItem_Click)));
	RegionMenu.MenuItems.Add(new ROMMenuItem("Promyvion - Dem",               2, 3, 126, new EventHandler(this.ROMMenuItem_Click)));
	RegionMenu.MenuItems.Add(new ROMMenuItem("Promyvion - Holla",             2, 3, 125, new EventHandler(this.ROMMenuItem_Click)));
	RegionMenu.MenuItems.Add(new ROMMenuItem("Promyvion - Mea",               2, 3, 127, new EventHandler(this.ROMMenuItem_Click)));
	RegionMenu.MenuItems.Add(new ROMMenuItem("Promyvion - Vahzl",             2, 4,   0, new EventHandler(this.ROMMenuItem_Click)));
      }
    }

    private void mnuIMOther_Popup(object sender, System.EventArgs e) {
    MenuItem OtherMenu = sender as MenuItem;
      if (OtherMenu != null && OtherMenu.MenuItems.Count == 1 && OtherMenu.MenuItems[0].Text == this.DummyText) {
	OtherMenu.MenuItems.Clear();
	OtherMenu.MenuItems.Add(new ROMMenuItem("Conquest",           0, 17,  23, new EventHandler(this.ROMMenuItem_Click)));
	OtherMenu.MenuItems.Add(new ROMMenuItem("Creature Chart",     0, 18, 124, new EventHandler(this.ROMMenuItem_Click)));
	// Also: 0/17/38,45,46,49,64,75-82,97-100,102-106,115,116,118,119,123-127
	// Also: 0/18/0-7,35,36,58,73,82,83
	OtherMenu.MenuItems.Add(new ROMMenuItem("Dummy Map",          0, 17,  37, new EventHandler(this.ROMMenuItem_Click)));
	OtherMenu.MenuItems.Add(new ROMMenuItem("Element Chart",      0, 18, 123, new EventHandler(this.ROMMenuItem_Click)));
	// Also: 0/17/61,73,74
	// Also: 0/18/41,105,106
	OtherMenu.MenuItems.Add(new ROMMenuItem("No Map",             0, 17,  50, new EventHandler(this.ROMMenuItem_Click)));
	// Also: 0/17/83
	OtherMenu.MenuItems.Add(new ROMMenuItem("No Map (Alternate)", 0, 17,  58, new EventHandler(this.ROMMenuItem_Click)));
	OtherMenu.MenuItems.Add(new ROMMenuItem("Stellar Map",        0, 18, 122, new EventHandler(this.ROMMenuItem_Click)));
	{
	MenuItem RouteMenu = OtherMenu.MenuItems.Add("&Transport Routes");
	  RouteMenu.MenuItems.Add(new ROMMenuItem("Airship - Jeuno → Bastok",     0, 18, 56, new EventHandler(this.ROMMenuItem_Click)));
	  RouteMenu.MenuItems.Add(new ROMMenuItem("Airship - Jeuno → Kazham",     1, 17, 28, new EventHandler(this.ROMMenuItem_Click)));
	  RouteMenu.MenuItems.Add(new ROMMenuItem("Airship - Jeuno → San d'Oria", 0, 18, 55, new EventHandler(this.ROMMenuItem_Click)));
	  RouteMenu.MenuItems.Add(new ROMMenuItem("Airship - Jeuno → Windurst",   0, 18, 57, new EventHandler(this.ROMMenuItem_Click)));
	  RouteMenu.MenuItems.Add(new ROMMenuItem("Ship - Mhaura → Selbina",      0, 18, 54, new EventHandler(this.ROMMenuItem_Click)));
	  RouteMenu.MenuItems.Add(new ROMMenuItem("Ship - Selbina → Mhaura",      0, 18, 53, new EventHandler(this.ROMMenuItem_Click)));
	}
      }
    }

    #endregion

    private void mnuIOther_Popup(object sender, System.EventArgs e) {
    MenuItem MI = sender as MenuItem;
      if (MI != null && MI.MenuItems.Count == 1 && MI.MenuItems[0].Text == this.DummyText) {
	MI.MenuItems.Clear();
	MI.MenuItems.Add(new ROMMenuItem("&Fonts && Menus",    0,  0,   1, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem("&Rank (Bastok)",     0, 16, 101, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem("&Rank (Windurst)",   0, 16, 116, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem("&Rank (San d'Oria)", 0, 17,   4, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem("&Status Icons",      0,  0,  12, new EventHandler(this.ROMMenuItem_Click)));
	{
	MenuItem UIStyleMenu = MI.MenuItems.Add("&UI Windows");
	  UIStyleMenu.MenuItems.Add(new ROMMenuItem("Style &1",  0,  0,  14, new EventHandler(this.ROMMenuItem_Click)));
	  UIStyleMenu.MenuItems.Add(new ROMMenuItem("Style &2",  0,  0,  15, new EventHandler(this.ROMMenuItem_Click)));
	  UIStyleMenu.MenuItems.Add(new ROMMenuItem("Style &3",  0,  0,  16, new EventHandler(this.ROMMenuItem_Click)));
	  UIStyleMenu.MenuItems.Add(new ROMMenuItem("Style &4",  0,  0,  17, new EventHandler(this.ROMMenuItem_Click)));
	  UIStyleMenu.MenuItems.Add(new ROMMenuItem("Style &5",  0,  0,  18, new EventHandler(this.ROMMenuItem_Click)));
	  UIStyleMenu.MenuItems.Add(new ROMMenuItem("Style &6",  0,  0,  19, new EventHandler(this.ROMMenuItem_Click)));
	  UIStyleMenu.MenuItems.Add(new ROMMenuItem("Style &7",  0,  0,  20, new EventHandler(this.ROMMenuItem_Click)));
	  UIStyleMenu.MenuItems.Add(new ROMMenuItem("Style &8",  0,  0,  21, new EventHandler(this.ROMMenuItem_Click)));
	}
      }
    }

    #endregion

    #region Item Data

    private void mnuItemData_Popup(object sender, System.EventArgs e) {
      if (this.mnuIDEnglish.MenuItems.Count  == 0)  this.mnuIDEnglish.MenuItems.Add(this.DummyText);
      if (this.mnuIDJapanese.MenuItems.Count == 0) this.mnuIDJapanese.MenuItems.Add(this.DummyText);
    }

    private void mnuIDEnglish_Popup(object sender, System.EventArgs e) {
    MenuItem MI = sender as MenuItem;
      if (MI != null && MI.MenuItems.Count == 1 && MI.MenuItems[0].Text == this.DummyText) {
	MI.MenuItems.Clear();
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:Armor"),   0, 118, 109, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:Items1"),  0, 118, 106, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:Items2"),  0, 118, 107, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:Unused"),  0, 118, 110, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:Weapons"), 0, 118, 108, new EventHandler(this.ROMMenuItem_Click)));
      }
    }

    private void mnuIDJapanese_Popup(object sender, System.EventArgs e) {
    MenuItem MI = sender as MenuItem;
      if (MI != null && MI.MenuItems.Count == 1 && MI.MenuItems[0].Text == this.DummyText) {
	MI.MenuItems.Clear();
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:Armor"),   0, 0, 7, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:Items1"),  0, 0, 4, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:Items2"),  0, 0, 5, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:Unused"),  0, 0, 8, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:Weapons"), 0, 0, 6, new EventHandler(this.ROMMenuItem_Click)));
      }
    }

    #endregion

    #region String Tables

    private void mnuStringTables_Popup(object sender, System.EventArgs e) {
      if (this.mnuSTEnglish.MenuItems.Count  == 0)  this.mnuSTEnglish.MenuItems.Add(this.DummyText);
      if (this.mnuSTJapanese.MenuItems.Count == 0) this.mnuSTJapanese.MenuItems.Add(this.DummyText);
      if (this.mnuSTShared.MenuItems.Count   == 0)   this.mnuSTShared.MenuItems.Add(this.DummyText);
    }

    private void mnuSTEnglish_Popup(object sender, System.EventArgs e) {
    MenuItem MI = sender as MenuItem;
      if (MI != null && MI.MenuItems.Count == 1 && MI.MenuItems[0].Text == this.DummyText) {
	MI.MenuItems.Clear();
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:AreaNames"),       0, 97, 57, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:AreaNamesAlt"),    0, 97, 53, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:ChatFilterTypes"), 0, 97, 39, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:DayNames"),        0, 97, 45, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:Directions"),      0, 97, 43, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:ErrorMessages"),   0, 97, 35, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:InGameMessages1"), 0, 97, 37, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:InGameMessages2"), 0, 97, 38, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:JobNames"),        0, 97, 55, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:MenuItemDesc"),    0, 97, 42, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:MenuItemText"),    0, 97, 41, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:MoonPhases"),      0, 97, 46, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:POLMessages"),     0, 97, 36, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:RegionNames"),     0, 97, 48, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:Various1"),        0, 97, 34, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:Various2"),        0, 97, 40, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:Various3"),        0, 97, 49, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:WeatherTypes"),    0, 97, 44, new EventHandler(this.ROMMenuItem_Click)));
      }
    }

    private void mnuSTJapanese_Popup(object sender, System.EventArgs e) {
    MenuItem MI = sender as MenuItem;
      if (MI != null && MI.MenuItems.Count == 1 && MI.MenuItems[0].Text == this.DummyText) {
	MI.MenuItems.Clear();
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:AreaNames"),       0, 97, 56, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:ChatFilterTypes"), 0, 97, 21, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:DayNames"),        0, 97, 27, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:Directions"),      0, 97, 25, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:ErrorMessages"),   0, 97, 18, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:InGameMessages1"), 0, 97, 19, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:InGameMessages2"), 0, 97, 20, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:JobNames"),        0, 97, 29, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:MenuItemDesc"),    0, 97, 24, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:MenuItemText"),    0, 97, 23, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:MoonPhases"),      0, 97, 28, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:POLMessages"),     0, 97,  8, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:RegionNames"),     0, 97, 30, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:Various1"),        0, 97, 17, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:Various2"),        0, 97, 22, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:Various3"),        0, 97, 31, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:WeatherTypes"),    0, 97, 26, new EventHandler(this.ROMMenuItem_Click)));
      }
    }

    private void mnuSTShared_Popup(object sender, System.EventArgs e) {
    MenuItem MI = sender as MenuItem;
      if (MI != null && MI.MenuItems.Count == 1 && MI.MenuItems[0].Text == this.DummyText) {
	MI.MenuItems.Clear();
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:AreaNamesShort"),  0,  97,  54, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:JobNamesShort"),   0, 118, 104, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:RaceNames"),       0, 118, 105, new EventHandler(this.ROMMenuItem_Click)));
      }
    }

    #endregion

    private void mnuWFileTable_Click(object sender, System.EventArgs e) {
      using (FileTableDialog FTD = new FileTableDialog())
	FTD.ShowDialog(this);
      this.Activate();
    }

    #endregion

  }

}
