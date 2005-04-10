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
    private System.Windows.Forms.ColumnHeader colXIEntryNum;
    private System.Windows.Forms.ColumnHeader colXIEntryText;
    private System.Windows.Forms.MenuItem mnuItemData;
    private System.Windows.Forms.MenuItem mnuIDEnglish;
    private System.Windows.Forms.MenuItem mnuIDJapanese;
    private System.Windows.Forms.TabPage tabViewerStringTable;
    private System.Windows.Forms.Button btnExportItems;
    private System.Windows.Forms.Button btnFindItems;
    private System.Windows.Forms.GroupBox grpCommonItemInfo;
    private System.Windows.Forms.PictureBox picItemIcon;
    private System.Windows.Forms.GroupBox grpMainItemActions;
    private System.Windows.Forms.ComboBox cmbItems;
    private System.Windows.Forms.ToolTip ttToolTip;
    private System.Windows.Forms.TextBox txtItemID;
    private System.Windows.Forms.TextBox txtItemType;
    private System.Windows.Forms.TextBox txtItemFlags;
    private System.Windows.Forms.TextBox txtItemStackSize;
    private System.Windows.Forms.Label lblItemID;
    private System.Windows.Forms.Label lblItemType;
    private System.Windows.Forms.GroupBox grpSpecializedItemInfo;
    private System.Windows.Forms.GroupBox grpItemViewMode;
    private System.Windows.Forms.CheckBox chkViewItemAsJWeapon;
    private System.Windows.Forms.CheckBox chkViewItemAsJObject;
    private System.Windows.Forms.CheckBox chkViewItemAsJArmor;
    private System.Windows.Forms.CheckBox chkViewItemAsEWeapon;
    private System.Windows.Forms.CheckBox chkViewItemAsEObject;
    private System.Windows.Forms.CheckBox chkViewItemAsEArmor;
    private System.Windows.Forms.TextBox txtItemPlural;
    private System.Windows.Forms.TextBox txtItemSingular;
    private System.Windows.Forms.TextBox txtItemJName;
    private System.Windows.Forms.TextBox txtItemEName;
    private System.Windows.Forms.TextBox txtItemDescription;
    private System.Windows.Forms.Label lblItemEName;
    private System.Windows.Forms.Label lblItemJName;
    private System.Windows.Forms.Label lblItemSingular;
    private System.Windows.Forms.Label lblItemPlural;
    private System.Windows.Forms.Label lblItemDescription;
    private System.Windows.Forms.Label lblItemResourceID;
    private System.Windows.Forms.TextBox txtItemReuseTimer;
    private System.Windows.Forms.TextBox txtItemEquipDelay;
    private System.Windows.Forms.TextBox txtItemSkill;
    private System.Windows.Forms.TextBox txtItemDelay;
    private System.Windows.Forms.TextBox txtItemDamage;
    private System.Windows.Forms.TextBox txtItemResourceID;
    private System.Windows.Forms.Label lblItemReuseTimer;
    private System.Windows.Forms.Label lblItemEquipDelay;
    private System.Windows.Forms.Label lblItemDamage;
    private System.Windows.Forms.Label lblItemDelay;
    private System.Windows.Forms.Label lblItemSkill;
    private System.Windows.Forms.Label lblItemShieldSize;
    private System.Windows.Forms.TextBox txtItemShieldSize;
    private System.Windows.Forms.Label lblItemLevel;
    private System.Windows.Forms.TextBox txtItemLevel;
    private System.Windows.Forms.Label lblItemJobs;
    private System.Windows.Forms.TextBox txtItemJobs;
    private System.Windows.Forms.Label lblItemSlots;
    private System.Windows.Forms.TextBox txtItemSlots;
    private System.Windows.Forms.Label lblItemRaces;
    private System.Windows.Forms.TextBox txtItemRaces;
    private System.Windows.Forms.Label lblItemMaxCharges;
    private System.Windows.Forms.TextBox txtItemMaxCharges;
    private System.Windows.Forms.Label lblItemStackSize;
    private System.Windows.Forms.Label lblItemFlags;

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
    ItemExporter IE = new ItemExporter(this.ChosenItemLanguage, this.ChosenItemType);
      IE.DoExport(this.LoadedItems_);
    }

    private ItemDataLanguage ChosenItemLanguage {
      get {
	return ((this.chkViewItemAsEArmor.Checked || this.chkViewItemAsEObject.Checked || this.chkViewItemAsEWeapon.Checked) ? ItemDataLanguage.English : ItemDataLanguage.Japanese);
      }
    }

    private ItemDataType ChosenItemType {
      get {
	if (this.chkViewItemAsEArmor.Checked || this.chkViewItemAsJArmor.Checked)
	  return ItemDataType.Armor;
	if (this.chkViewItemAsEWeapon.Checked || this.chkViewItemAsJWeapon.Checked)
	  return ItemDataType.Weapon;
	return ItemDataType.Object;
      }
    }

    private void btnFindItems_Click(object sender, System.EventArgs e) {
      using (ItemFindDialog IFD = new ItemFindDialog(this.LoadedItems_)) {
	IFD.Language = this.ChosenItemLanguage;
	IFD.Type     = this.ChosenItemType;
	if (IFD.ShowDialog(this) == DialogResult.OK && IFD.SelectedItem != null)
	  this.cmbItems.SelectedItem = IFD.SelectedItem;
      }
    }

    private void cmbItems_SelectedIndexChanged(object sender, System.EventArgs e) {
    FFXIItem I = this.cmbItems.SelectedItem as FFXIItem;
      this.chkViewItemAsEArmor.Checked = this.chkViewItemAsEObject.Checked = this.chkViewItemAsEWeapon.Checked = false;
      this.chkViewItemAsJArmor.Checked = this.chkViewItemAsJObject.Checked = this.chkViewItemAsJWeapon.Checked = false;
      if (I != null) {
	// Common Info
	this.picItemIcon.Image     = I.IconGraphic.Bitmap;
	this.ttToolTip.SetToolTip(this.picItemIcon, I.IconGraphic.ToString());
	this.txtItemID.Text        = String.Format("{0:X4} ({0})", I.Common.ID);
	this.txtItemType.Text      = String.Format("{0:X} ({0})",  I.Common.Type);
	this.txtItemFlags.Text     = String.Format("{0:X} ({0})",  I.Common.Flags);
	this.txtItemStackSize.Text = String.Format("{0}",          I.Common.StackSize);
	switch (I.Common.Type) {
	  case ItemType.Armor: {
	  string LogNames = I.ENArmor.LogNameSingular + I.ENArmor.LogNamePlural;
	    if (LogNames == String.Empty)
	      this.chkViewItemAsJArmor.Checked = true;
	    else {
	    bool ExtendedCharSeen = false;
	      foreach (char C in LogNames) {
		if ((int) C > 0x100) {
		  ExtendedCharSeen = true;
		  break;
		}
	      }
	      this.chkViewItemAsEArmor.Checked = !ExtendedCharSeen;
	      this.chkViewItemAsJArmor.Checked =  ExtendedCharSeen;
	    }
	    break;
	  }
	  case ItemType.Weapon: {
	  string LogNames = I.ENWeapon.LogNameSingular + I.ENArmor.LogNamePlural;
	    if (LogNames == String.Empty)
	      this.chkViewItemAsJWeapon.Checked = true;
	    else {
	    bool ExtendedCharSeen = false;
	      foreach (char C in LogNames) {
		if ((int) C > 0x100) {
		  ExtendedCharSeen = true;
		  break;
		}
	      }
	      this.chkViewItemAsEWeapon.Checked = !ExtendedCharSeen;
	      this.chkViewItemAsJWeapon.Checked =  ExtendedCharSeen;
	    }
	    break;
	  }
	  default: {
	  string LogNames = I.ENObject.LogNameSingular + I.ENArmor.LogNamePlural;
	    if (LogNames == String.Empty)
	      this.chkViewItemAsJObject.Checked = true;
	    else {
	    bool ExtendedCharSeen = false;
	      foreach (char C in LogNames) {
		if ((int) C > 0x100) {
		  ExtendedCharSeen = true;
		  break;
		}
	      }
	      this.chkViewItemAsEObject.Checked = !ExtendedCharSeen;
	      this.chkViewItemAsJObject.Checked =  ExtendedCharSeen;
	    }
	    break;
	  }
	}
      }
      else {
	// Common Info
	this.picItemIcon.Image = null;
	this.ttToolTip.SetToolTip(this.picItemIcon, null);
	this.txtItemID.Text = this.txtItemType.Text = this.txtItemFlags.Text = this.txtItemStackSize.Text = String.Empty;
      }
    }

    private void chkViewItem_Click(object sender, System.EventArgs e) {
      this.chkViewItemAsEArmor.Checked  = sender.Equals(this.chkViewItemAsEArmor);
      this.chkViewItemAsEObject.Checked = sender.Equals(this.chkViewItemAsEObject);
      this.chkViewItemAsEWeapon.Checked = sender.Equals(this.chkViewItemAsEWeapon);
      this.chkViewItemAsJArmor.Checked  = sender.Equals(this.chkViewItemAsJArmor);
      this.chkViewItemAsJObject.Checked = sender.Equals(this.chkViewItemAsJObject);
      this.chkViewItemAsJWeapon.Checked = sender.Equals(this.chkViewItemAsJWeapon);
    }

    private void FillCommonFields(FFXIItem.IItemInfo II) {
      this.txtItemEName.Text       = II.GetFieldText(ItemField.EnglishName);
      this.txtItemJName.Text       = II.GetFieldText(ItemField.JapaneseName);
      this.txtItemSingular.Text    = II.GetFieldText(ItemField.LogNameSingular);
      this.txtItemPlural.Text      = II.GetFieldText(ItemField.LogNamePlural);
      this.txtItemDescription.Text = II.GetFieldText(ItemField.Description).Replace("\n", "\r\n");
    }

    private void FillCommonExtraFields(FFXIItem.IItemInfo II) {
      this.txtItemResourceID.Text = II.GetFieldText(ItemField.ResourceID);
      this.txtItemLevel.Text      = II.GetFieldText(ItemField.Level);
      this.txtItemSlots.Text      = II.GetFieldText(ItemField.Slots);
      this.txtItemJobs.Text       = II.GetFieldText(ItemField.Jobs);
      this.txtItemRaces.Text      = II.GetFieldText(ItemField.Races);
      this.txtItemMaxCharges.Text = II.GetFieldText(ItemField.MaxCharges);
      this.txtItemEquipDelay.Text = II.GetFieldText(ItemField.EquipDelay);
      this.txtItemReuseTimer.Text = II.GetFieldText(ItemField.ReuseTimer);
    }

    private void FillObjectFields(FFXIItem.IItemInfo II) {
      this.FillCommonFields(II);
      this.grpSpecializedItemInfo.Visible = false;
    }

    private void FillArmorFields(FFXIItem.IItemInfo II) {
      this.FillCommonFields(II);
      this.grpSpecializedItemInfo.Visible = true;
      this.lblItemDamage.Visible = this.lblItemDelay.Visible = this.lblItemSkill.Visible = false;
      this.txtItemDamage.Visible = this.txtItemDelay.Visible = this.txtItemSkill.Visible = false;
      this.lblItemShieldSize.Visible = this.txtItemShieldSize.Visible = true;
      this.txtItemShieldSize.Text = II.GetFieldText(ItemField.ShieldSize);
      this.FillCommonExtraFields(II);
    }

    private void FillWeaponFields(FFXIItem.IItemInfo II) {
      this.FillCommonFields(II);
      this.grpSpecializedItemInfo.Visible = true;
      this.lblItemDamage.Visible = this.lblItemDelay.Visible = this.lblItemSkill.Visible = true;
      this.txtItemDamage.Visible = this.txtItemDelay.Visible = this.txtItemSkill.Visible = true;
      this.lblItemShieldSize.Visible = this.txtItemShieldSize.Visible = false;
      this.txtItemDamage.Text = II.GetFieldText(ItemField.Damage);
      this.txtItemDelay.Text  = II.GetFieldText(ItemField.Delay);
      this.txtItemSkill.Text  = II.GetFieldText(ItemField.Skill);
      this.FillCommonExtraFields(II);
    }

    private void chkViewItemAsEArmor_CheckedChanged(object sender, System.EventArgs e) {
    FFXIItem I = this.cmbItems.SelectedItem as FFXIItem;
      if (I != null && this.chkViewItemAsEArmor.Checked)
	this.FillArmorFields(I.ENArmor);
    }

    private void chkViewItemAsEObject_CheckedChanged(object sender, System.EventArgs e) {
    FFXIItem I = this.cmbItems.SelectedItem as FFXIItem;
      if (I != null && this.chkViewItemAsEObject.Checked)
	this.FillObjectFields(I.ENObject);
    }

    private void chkViewItemAsEWeapon_CheckedChanged(object sender, System.EventArgs e) {
    FFXIItem I = this.cmbItems.SelectedItem as FFXIItem;
      if (I != null && this.chkViewItemAsEWeapon.Checked)
	this.FillWeaponFields(I.ENWeapon);
    }

    private void chkViewItemAsJArmor_CheckedChanged(object sender, System.EventArgs e) {
    FFXIItem I = this.cmbItems.SelectedItem as FFXIItem;
      if (I != null && this.chkViewItemAsJArmor.Checked)
	this.FillArmorFields(I.JPArmor);
    }

    private void chkViewItemAsJObject_CheckedChanged(object sender, System.EventArgs e) {
    FFXIItem I = this.cmbItems.SelectedItem as FFXIItem;
      if (I != null && this.chkViewItemAsJObject.Checked)
	this.FillObjectFields(I.JPObject);
    }

    private void chkViewItemAsJWeapon_CheckedChanged(object sender, System.EventArgs e) {
    FFXIItem I = this.cmbItems.SelectedItem as FFXIItem;
      if (I != null && this.chkViewItemAsJWeapon.Checked)
	this.FillWeaponFields(I.JPWeapon);
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
      this.grpItemViewMode = new System.Windows.Forms.GroupBox();
      this.chkViewItemAsJWeapon = new System.Windows.Forms.CheckBox();
      this.chkViewItemAsJObject = new System.Windows.Forms.CheckBox();
      this.chkViewItemAsJArmor = new System.Windows.Forms.CheckBox();
      this.chkViewItemAsEWeapon = new System.Windows.Forms.CheckBox();
      this.chkViewItemAsEObject = new System.Windows.Forms.CheckBox();
      this.chkViewItemAsEArmor = new System.Windows.Forms.CheckBox();
      this.grpSpecializedItemInfo = new System.Windows.Forms.GroupBox();
      this.lblItemRaces = new System.Windows.Forms.Label();
      this.txtItemRaces = new System.Windows.Forms.TextBox();
      this.lblItemSlots = new System.Windows.Forms.Label();
      this.txtItemSlots = new System.Windows.Forms.TextBox();
      this.lblItemJobs = new System.Windows.Forms.Label();
      this.txtItemJobs = new System.Windows.Forms.TextBox();
      this.lblItemLevel = new System.Windows.Forms.Label();
      this.txtItemLevel = new System.Windows.Forms.TextBox();
      this.lblItemEquipDelay = new System.Windows.Forms.Label();
      this.lblItemReuseTimer = new System.Windows.Forms.Label();
      this.lblItemMaxCharges = new System.Windows.Forms.Label();
      this.lblItemResourceID = new System.Windows.Forms.Label();
      this.txtItemReuseTimer = new System.Windows.Forms.TextBox();
      this.txtItemEquipDelay = new System.Windows.Forms.TextBox();
      this.txtItemMaxCharges = new System.Windows.Forms.TextBox();
      this.txtItemResourceID = new System.Windows.Forms.TextBox();
      this.lblItemSkill = new System.Windows.Forms.Label();
      this.lblItemDelay = new System.Windows.Forms.Label();
      this.lblItemDamage = new System.Windows.Forms.Label();
      this.txtItemSkill = new System.Windows.Forms.TextBox();
      this.txtItemDelay = new System.Windows.Forms.TextBox();
      this.txtItemDamage = new System.Windows.Forms.TextBox();
      this.lblItemShieldSize = new System.Windows.Forms.Label();
      this.txtItemShieldSize = new System.Windows.Forms.TextBox();
      this.grpCommonItemInfo = new System.Windows.Forms.GroupBox();
      this.lblItemDescription = new System.Windows.Forms.Label();
      this.lblItemPlural = new System.Windows.Forms.Label();
      this.lblItemSingular = new System.Windows.Forms.Label();
      this.lblItemJName = new System.Windows.Forms.Label();
      this.lblItemEName = new System.Windows.Forms.Label();
      this.txtItemPlural = new System.Windows.Forms.TextBox();
      this.txtItemSingular = new System.Windows.Forms.TextBox();
      this.txtItemJName = new System.Windows.Forms.TextBox();
      this.txtItemEName = new System.Windows.Forms.TextBox();
      this.txtItemDescription = new System.Windows.Forms.TextBox();
      this.lblItemStackSize = new System.Windows.Forms.Label();
      this.lblItemFlags = new System.Windows.Forms.Label();
      this.lblItemType = new System.Windows.Forms.Label();
      this.lblItemID = new System.Windows.Forms.Label();
      this.txtItemStackSize = new System.Windows.Forms.TextBox();
      this.txtItemFlags = new System.Windows.Forms.TextBox();
      this.txtItemType = new System.Windows.Forms.TextBox();
      this.txtItemID = new System.Windows.Forms.TextBox();
      this.picItemIcon = new System.Windows.Forms.PictureBox();
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
      this.colXIEntryNum = new System.Windows.Forms.ColumnHeader();
      this.colXIEntryText = new System.Windows.Forms.ColumnHeader();
      this.pnlNoViewers = new System.Windows.Forms.Panel();
      this.lblNoViewers = new System.Windows.Forms.Label();
      this.ttToolTip = new System.Windows.Forms.ToolTip(this.components);
      this.pnlViewerArea.SuspendLayout();
      this.tabViewers.SuspendLayout();
      this.tabViewerItems.SuspendLayout();
      this.grpItemViewMode.SuspendLayout();
      this.grpSpecializedItemInfo.SuspendLayout();
      this.grpCommonItemInfo.SuspendLayout();
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
      this.ttToolTip.SetToolTip(this.tvDataFiles, resources.GetString("tvDataFiles.ToolTip"));
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
      this.ttToolTip.SetToolTip(this.splSplitter, resources.GetString("splSplitter.ToolTip"));
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
      this.ttToolTip.SetToolTip(this.pnlViewerArea, resources.GetString("pnlViewerArea.ToolTip"));
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
      this.ttToolTip.SetToolTip(this.tabViewers, resources.GetString("tabViewers.ToolTip"));
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
      this.tabViewerItems.Controls.Add(this.grpItemViewMode);
      this.tabViewerItems.Controls.Add(this.grpSpecializedItemInfo);
      this.tabViewerItems.Controls.Add(this.grpCommonItemInfo);
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
      this.ttToolTip.SetToolTip(this.tabViewerItems, resources.GetString("tabViewerItems.ToolTip"));
      this.tabViewerItems.ToolTipText = resources.GetString("tabViewerItems.ToolTipText");
      this.tabViewerItems.Visible = ((bool)(resources.GetObject("tabViewerItems.Visible")));
      // 
      // grpItemViewMode
      // 
      this.grpItemViewMode.AccessibleDescription = resources.GetString("grpItemViewMode.AccessibleDescription");
      this.grpItemViewMode.AccessibleName = resources.GetString("grpItemViewMode.AccessibleName");
      this.grpItemViewMode.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("grpItemViewMode.Anchor")));
      this.grpItemViewMode.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpItemViewMode.BackgroundImage")));
      this.grpItemViewMode.Controls.Add(this.chkViewItemAsJWeapon);
      this.grpItemViewMode.Controls.Add(this.chkViewItemAsJObject);
      this.grpItemViewMode.Controls.Add(this.chkViewItemAsJArmor);
      this.grpItemViewMode.Controls.Add(this.chkViewItemAsEWeapon);
      this.grpItemViewMode.Controls.Add(this.chkViewItemAsEObject);
      this.grpItemViewMode.Controls.Add(this.chkViewItemAsEArmor);
      this.grpItemViewMode.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("grpItemViewMode.Dock")));
      this.grpItemViewMode.Enabled = ((bool)(resources.GetObject("grpItemViewMode.Enabled")));
      this.grpItemViewMode.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.grpItemViewMode.Font = ((System.Drawing.Font)(resources.GetObject("grpItemViewMode.Font")));
      this.grpItemViewMode.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("grpItemViewMode.ImeMode")));
      this.grpItemViewMode.Location = ((System.Drawing.Point)(resources.GetObject("grpItemViewMode.Location")));
      this.grpItemViewMode.Name = "grpItemViewMode";
      this.grpItemViewMode.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("grpItemViewMode.RightToLeft")));
      this.grpItemViewMode.Size = ((System.Drawing.Size)(resources.GetObject("grpItemViewMode.Size")));
      this.grpItemViewMode.TabIndex = ((int)(resources.GetObject("grpItemViewMode.TabIndex")));
      this.grpItemViewMode.TabStop = false;
      this.grpItemViewMode.Text = resources.GetString("grpItemViewMode.Text");
      this.ttToolTip.SetToolTip(this.grpItemViewMode, resources.GetString("grpItemViewMode.ToolTip"));
      this.grpItemViewMode.Visible = ((bool)(resources.GetObject("grpItemViewMode.Visible")));
      // 
      // chkViewItemAsJWeapon
      // 
      this.chkViewItemAsJWeapon.AccessibleDescription = resources.GetString("chkViewItemAsJWeapon.AccessibleDescription");
      this.chkViewItemAsJWeapon.AccessibleName = resources.GetString("chkViewItemAsJWeapon.AccessibleName");
      this.chkViewItemAsJWeapon.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkViewItemAsJWeapon.Anchor")));
      this.chkViewItemAsJWeapon.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkViewItemAsJWeapon.Appearance")));
      this.chkViewItemAsJWeapon.AutoCheck = false;
      this.chkViewItemAsJWeapon.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkViewItemAsJWeapon.BackgroundImage")));
      this.chkViewItemAsJWeapon.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkViewItemAsJWeapon.CheckAlign")));
      this.chkViewItemAsJWeapon.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkViewItemAsJWeapon.Dock")));
      this.chkViewItemAsJWeapon.Enabled = ((bool)(resources.GetObject("chkViewItemAsJWeapon.Enabled")));
      this.chkViewItemAsJWeapon.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkViewItemAsJWeapon.FlatStyle")));
      this.chkViewItemAsJWeapon.Font = ((System.Drawing.Font)(resources.GetObject("chkViewItemAsJWeapon.Font")));
      this.chkViewItemAsJWeapon.Image = ((System.Drawing.Image)(resources.GetObject("chkViewItemAsJWeapon.Image")));
      this.chkViewItemAsJWeapon.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkViewItemAsJWeapon.ImageAlign")));
      this.chkViewItemAsJWeapon.ImageIndex = ((int)(resources.GetObject("chkViewItemAsJWeapon.ImageIndex")));
      this.chkViewItemAsJWeapon.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkViewItemAsJWeapon.ImeMode")));
      this.chkViewItemAsJWeapon.Location = ((System.Drawing.Point)(resources.GetObject("chkViewItemAsJWeapon.Location")));
      this.chkViewItemAsJWeapon.Name = "chkViewItemAsJWeapon";
      this.chkViewItemAsJWeapon.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkViewItemAsJWeapon.RightToLeft")));
      this.chkViewItemAsJWeapon.Size = ((System.Drawing.Size)(resources.GetObject("chkViewItemAsJWeapon.Size")));
      this.chkViewItemAsJWeapon.TabIndex = ((int)(resources.GetObject("chkViewItemAsJWeapon.TabIndex")));
      this.chkViewItemAsJWeapon.Text = resources.GetString("chkViewItemAsJWeapon.Text");
      this.chkViewItemAsJWeapon.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkViewItemAsJWeapon.TextAlign")));
      this.ttToolTip.SetToolTip(this.chkViewItemAsJWeapon, resources.GetString("chkViewItemAsJWeapon.ToolTip"));
      this.chkViewItemAsJWeapon.Visible = ((bool)(resources.GetObject("chkViewItemAsJWeapon.Visible")));
      this.chkViewItemAsJWeapon.Click += new System.EventHandler(this.chkViewItem_Click);
      this.chkViewItemAsJWeapon.CheckedChanged += new System.EventHandler(this.chkViewItemAsJWeapon_CheckedChanged);
      // 
      // chkViewItemAsJObject
      // 
      this.chkViewItemAsJObject.AccessibleDescription = resources.GetString("chkViewItemAsJObject.AccessibleDescription");
      this.chkViewItemAsJObject.AccessibleName = resources.GetString("chkViewItemAsJObject.AccessibleName");
      this.chkViewItemAsJObject.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkViewItemAsJObject.Anchor")));
      this.chkViewItemAsJObject.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkViewItemAsJObject.Appearance")));
      this.chkViewItemAsJObject.AutoCheck = false;
      this.chkViewItemAsJObject.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkViewItemAsJObject.BackgroundImage")));
      this.chkViewItemAsJObject.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkViewItemAsJObject.CheckAlign")));
      this.chkViewItemAsJObject.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkViewItemAsJObject.Dock")));
      this.chkViewItemAsJObject.Enabled = ((bool)(resources.GetObject("chkViewItemAsJObject.Enabled")));
      this.chkViewItemAsJObject.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkViewItemAsJObject.FlatStyle")));
      this.chkViewItemAsJObject.Font = ((System.Drawing.Font)(resources.GetObject("chkViewItemAsJObject.Font")));
      this.chkViewItemAsJObject.Image = ((System.Drawing.Image)(resources.GetObject("chkViewItemAsJObject.Image")));
      this.chkViewItemAsJObject.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkViewItemAsJObject.ImageAlign")));
      this.chkViewItemAsJObject.ImageIndex = ((int)(resources.GetObject("chkViewItemAsJObject.ImageIndex")));
      this.chkViewItemAsJObject.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkViewItemAsJObject.ImeMode")));
      this.chkViewItemAsJObject.Location = ((System.Drawing.Point)(resources.GetObject("chkViewItemAsJObject.Location")));
      this.chkViewItemAsJObject.Name = "chkViewItemAsJObject";
      this.chkViewItemAsJObject.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkViewItemAsJObject.RightToLeft")));
      this.chkViewItemAsJObject.Size = ((System.Drawing.Size)(resources.GetObject("chkViewItemAsJObject.Size")));
      this.chkViewItemAsJObject.TabIndex = ((int)(resources.GetObject("chkViewItemAsJObject.TabIndex")));
      this.chkViewItemAsJObject.Text = resources.GetString("chkViewItemAsJObject.Text");
      this.chkViewItemAsJObject.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkViewItemAsJObject.TextAlign")));
      this.ttToolTip.SetToolTip(this.chkViewItemAsJObject, resources.GetString("chkViewItemAsJObject.ToolTip"));
      this.chkViewItemAsJObject.Visible = ((bool)(resources.GetObject("chkViewItemAsJObject.Visible")));
      this.chkViewItemAsJObject.Click += new System.EventHandler(this.chkViewItem_Click);
      this.chkViewItemAsJObject.CheckedChanged += new System.EventHandler(this.chkViewItemAsJObject_CheckedChanged);
      // 
      // chkViewItemAsJArmor
      // 
      this.chkViewItemAsJArmor.AccessibleDescription = resources.GetString("chkViewItemAsJArmor.AccessibleDescription");
      this.chkViewItemAsJArmor.AccessibleName = resources.GetString("chkViewItemAsJArmor.AccessibleName");
      this.chkViewItemAsJArmor.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkViewItemAsJArmor.Anchor")));
      this.chkViewItemAsJArmor.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkViewItemAsJArmor.Appearance")));
      this.chkViewItemAsJArmor.AutoCheck = false;
      this.chkViewItemAsJArmor.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkViewItemAsJArmor.BackgroundImage")));
      this.chkViewItemAsJArmor.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkViewItemAsJArmor.CheckAlign")));
      this.chkViewItemAsJArmor.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkViewItemAsJArmor.Dock")));
      this.chkViewItemAsJArmor.Enabled = ((bool)(resources.GetObject("chkViewItemAsJArmor.Enabled")));
      this.chkViewItemAsJArmor.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkViewItemAsJArmor.FlatStyle")));
      this.chkViewItemAsJArmor.Font = ((System.Drawing.Font)(resources.GetObject("chkViewItemAsJArmor.Font")));
      this.chkViewItemAsJArmor.Image = ((System.Drawing.Image)(resources.GetObject("chkViewItemAsJArmor.Image")));
      this.chkViewItemAsJArmor.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkViewItemAsJArmor.ImageAlign")));
      this.chkViewItemAsJArmor.ImageIndex = ((int)(resources.GetObject("chkViewItemAsJArmor.ImageIndex")));
      this.chkViewItemAsJArmor.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkViewItemAsJArmor.ImeMode")));
      this.chkViewItemAsJArmor.Location = ((System.Drawing.Point)(resources.GetObject("chkViewItemAsJArmor.Location")));
      this.chkViewItemAsJArmor.Name = "chkViewItemAsJArmor";
      this.chkViewItemAsJArmor.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkViewItemAsJArmor.RightToLeft")));
      this.chkViewItemAsJArmor.Size = ((System.Drawing.Size)(resources.GetObject("chkViewItemAsJArmor.Size")));
      this.chkViewItemAsJArmor.TabIndex = ((int)(resources.GetObject("chkViewItemAsJArmor.TabIndex")));
      this.chkViewItemAsJArmor.Text = resources.GetString("chkViewItemAsJArmor.Text");
      this.chkViewItemAsJArmor.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkViewItemAsJArmor.TextAlign")));
      this.ttToolTip.SetToolTip(this.chkViewItemAsJArmor, resources.GetString("chkViewItemAsJArmor.ToolTip"));
      this.chkViewItemAsJArmor.Visible = ((bool)(resources.GetObject("chkViewItemAsJArmor.Visible")));
      this.chkViewItemAsJArmor.Click += new System.EventHandler(this.chkViewItem_Click);
      this.chkViewItemAsJArmor.CheckedChanged += new System.EventHandler(this.chkViewItemAsJArmor_CheckedChanged);
      // 
      // chkViewItemAsEWeapon
      // 
      this.chkViewItemAsEWeapon.AccessibleDescription = resources.GetString("chkViewItemAsEWeapon.AccessibleDescription");
      this.chkViewItemAsEWeapon.AccessibleName = resources.GetString("chkViewItemAsEWeapon.AccessibleName");
      this.chkViewItemAsEWeapon.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkViewItemAsEWeapon.Anchor")));
      this.chkViewItemAsEWeapon.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkViewItemAsEWeapon.Appearance")));
      this.chkViewItemAsEWeapon.AutoCheck = false;
      this.chkViewItemAsEWeapon.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkViewItemAsEWeapon.BackgroundImage")));
      this.chkViewItemAsEWeapon.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkViewItemAsEWeapon.CheckAlign")));
      this.chkViewItemAsEWeapon.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkViewItemAsEWeapon.Dock")));
      this.chkViewItemAsEWeapon.Enabled = ((bool)(resources.GetObject("chkViewItemAsEWeapon.Enabled")));
      this.chkViewItemAsEWeapon.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkViewItemAsEWeapon.FlatStyle")));
      this.chkViewItemAsEWeapon.Font = ((System.Drawing.Font)(resources.GetObject("chkViewItemAsEWeapon.Font")));
      this.chkViewItemAsEWeapon.Image = ((System.Drawing.Image)(resources.GetObject("chkViewItemAsEWeapon.Image")));
      this.chkViewItemAsEWeapon.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkViewItemAsEWeapon.ImageAlign")));
      this.chkViewItemAsEWeapon.ImageIndex = ((int)(resources.GetObject("chkViewItemAsEWeapon.ImageIndex")));
      this.chkViewItemAsEWeapon.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkViewItemAsEWeapon.ImeMode")));
      this.chkViewItemAsEWeapon.Location = ((System.Drawing.Point)(resources.GetObject("chkViewItemAsEWeapon.Location")));
      this.chkViewItemAsEWeapon.Name = "chkViewItemAsEWeapon";
      this.chkViewItemAsEWeapon.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkViewItemAsEWeapon.RightToLeft")));
      this.chkViewItemAsEWeapon.Size = ((System.Drawing.Size)(resources.GetObject("chkViewItemAsEWeapon.Size")));
      this.chkViewItemAsEWeapon.TabIndex = ((int)(resources.GetObject("chkViewItemAsEWeapon.TabIndex")));
      this.chkViewItemAsEWeapon.Text = resources.GetString("chkViewItemAsEWeapon.Text");
      this.chkViewItemAsEWeapon.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkViewItemAsEWeapon.TextAlign")));
      this.ttToolTip.SetToolTip(this.chkViewItemAsEWeapon, resources.GetString("chkViewItemAsEWeapon.ToolTip"));
      this.chkViewItemAsEWeapon.Visible = ((bool)(resources.GetObject("chkViewItemAsEWeapon.Visible")));
      this.chkViewItemAsEWeapon.Click += new System.EventHandler(this.chkViewItem_Click);
      this.chkViewItemAsEWeapon.CheckedChanged += new System.EventHandler(this.chkViewItemAsEWeapon_CheckedChanged);
      // 
      // chkViewItemAsEObject
      // 
      this.chkViewItemAsEObject.AccessibleDescription = resources.GetString("chkViewItemAsEObject.AccessibleDescription");
      this.chkViewItemAsEObject.AccessibleName = resources.GetString("chkViewItemAsEObject.AccessibleName");
      this.chkViewItemAsEObject.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkViewItemAsEObject.Anchor")));
      this.chkViewItemAsEObject.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkViewItemAsEObject.Appearance")));
      this.chkViewItemAsEObject.AutoCheck = false;
      this.chkViewItemAsEObject.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkViewItemAsEObject.BackgroundImage")));
      this.chkViewItemAsEObject.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkViewItemAsEObject.CheckAlign")));
      this.chkViewItemAsEObject.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkViewItemAsEObject.Dock")));
      this.chkViewItemAsEObject.Enabled = ((bool)(resources.GetObject("chkViewItemAsEObject.Enabled")));
      this.chkViewItemAsEObject.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkViewItemAsEObject.FlatStyle")));
      this.chkViewItemAsEObject.Font = ((System.Drawing.Font)(resources.GetObject("chkViewItemAsEObject.Font")));
      this.chkViewItemAsEObject.Image = ((System.Drawing.Image)(resources.GetObject("chkViewItemAsEObject.Image")));
      this.chkViewItemAsEObject.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkViewItemAsEObject.ImageAlign")));
      this.chkViewItemAsEObject.ImageIndex = ((int)(resources.GetObject("chkViewItemAsEObject.ImageIndex")));
      this.chkViewItemAsEObject.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkViewItemAsEObject.ImeMode")));
      this.chkViewItemAsEObject.Location = ((System.Drawing.Point)(resources.GetObject("chkViewItemAsEObject.Location")));
      this.chkViewItemAsEObject.Name = "chkViewItemAsEObject";
      this.chkViewItemAsEObject.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkViewItemAsEObject.RightToLeft")));
      this.chkViewItemAsEObject.Size = ((System.Drawing.Size)(resources.GetObject("chkViewItemAsEObject.Size")));
      this.chkViewItemAsEObject.TabIndex = ((int)(resources.GetObject("chkViewItemAsEObject.TabIndex")));
      this.chkViewItemAsEObject.Text = resources.GetString("chkViewItemAsEObject.Text");
      this.chkViewItemAsEObject.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkViewItemAsEObject.TextAlign")));
      this.ttToolTip.SetToolTip(this.chkViewItemAsEObject, resources.GetString("chkViewItemAsEObject.ToolTip"));
      this.chkViewItemAsEObject.Visible = ((bool)(resources.GetObject("chkViewItemAsEObject.Visible")));
      this.chkViewItemAsEObject.Click += new System.EventHandler(this.chkViewItem_Click);
      this.chkViewItemAsEObject.CheckedChanged += new System.EventHandler(this.chkViewItemAsEObject_CheckedChanged);
      // 
      // chkViewItemAsEArmor
      // 
      this.chkViewItemAsEArmor.AccessibleDescription = resources.GetString("chkViewItemAsEArmor.AccessibleDescription");
      this.chkViewItemAsEArmor.AccessibleName = resources.GetString("chkViewItemAsEArmor.AccessibleName");
      this.chkViewItemAsEArmor.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkViewItemAsEArmor.Anchor")));
      this.chkViewItemAsEArmor.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkViewItemAsEArmor.Appearance")));
      this.chkViewItemAsEArmor.AutoCheck = false;
      this.chkViewItemAsEArmor.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkViewItemAsEArmor.BackgroundImage")));
      this.chkViewItemAsEArmor.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkViewItemAsEArmor.CheckAlign")));
      this.chkViewItemAsEArmor.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkViewItemAsEArmor.Dock")));
      this.chkViewItemAsEArmor.Enabled = ((bool)(resources.GetObject("chkViewItemAsEArmor.Enabled")));
      this.chkViewItemAsEArmor.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkViewItemAsEArmor.FlatStyle")));
      this.chkViewItemAsEArmor.Font = ((System.Drawing.Font)(resources.GetObject("chkViewItemAsEArmor.Font")));
      this.chkViewItemAsEArmor.Image = ((System.Drawing.Image)(resources.GetObject("chkViewItemAsEArmor.Image")));
      this.chkViewItemAsEArmor.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkViewItemAsEArmor.ImageAlign")));
      this.chkViewItemAsEArmor.ImageIndex = ((int)(resources.GetObject("chkViewItemAsEArmor.ImageIndex")));
      this.chkViewItemAsEArmor.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkViewItemAsEArmor.ImeMode")));
      this.chkViewItemAsEArmor.Location = ((System.Drawing.Point)(resources.GetObject("chkViewItemAsEArmor.Location")));
      this.chkViewItemAsEArmor.Name = "chkViewItemAsEArmor";
      this.chkViewItemAsEArmor.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkViewItemAsEArmor.RightToLeft")));
      this.chkViewItemAsEArmor.Size = ((System.Drawing.Size)(resources.GetObject("chkViewItemAsEArmor.Size")));
      this.chkViewItemAsEArmor.TabIndex = ((int)(resources.GetObject("chkViewItemAsEArmor.TabIndex")));
      this.chkViewItemAsEArmor.Text = resources.GetString("chkViewItemAsEArmor.Text");
      this.chkViewItemAsEArmor.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkViewItemAsEArmor.TextAlign")));
      this.ttToolTip.SetToolTip(this.chkViewItemAsEArmor, resources.GetString("chkViewItemAsEArmor.ToolTip"));
      this.chkViewItemAsEArmor.Visible = ((bool)(resources.GetObject("chkViewItemAsEArmor.Visible")));
      this.chkViewItemAsEArmor.Click += new System.EventHandler(this.chkViewItem_Click);
      this.chkViewItemAsEArmor.CheckedChanged += new System.EventHandler(this.chkViewItemAsEArmor_CheckedChanged);
      // 
      // grpSpecializedItemInfo
      // 
      this.grpSpecializedItemInfo.AccessibleDescription = resources.GetString("grpSpecializedItemInfo.AccessibleDescription");
      this.grpSpecializedItemInfo.AccessibleName = resources.GetString("grpSpecializedItemInfo.AccessibleName");
      this.grpSpecializedItemInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("grpSpecializedItemInfo.Anchor")));
      this.grpSpecializedItemInfo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpSpecializedItemInfo.BackgroundImage")));
      this.grpSpecializedItemInfo.Controls.Add(this.lblItemRaces);
      this.grpSpecializedItemInfo.Controls.Add(this.txtItemRaces);
      this.grpSpecializedItemInfo.Controls.Add(this.lblItemSlots);
      this.grpSpecializedItemInfo.Controls.Add(this.txtItemSlots);
      this.grpSpecializedItemInfo.Controls.Add(this.lblItemJobs);
      this.grpSpecializedItemInfo.Controls.Add(this.txtItemJobs);
      this.grpSpecializedItemInfo.Controls.Add(this.lblItemLevel);
      this.grpSpecializedItemInfo.Controls.Add(this.txtItemLevel);
      this.grpSpecializedItemInfo.Controls.Add(this.lblItemEquipDelay);
      this.grpSpecializedItemInfo.Controls.Add(this.lblItemReuseTimer);
      this.grpSpecializedItemInfo.Controls.Add(this.lblItemMaxCharges);
      this.grpSpecializedItemInfo.Controls.Add(this.lblItemResourceID);
      this.grpSpecializedItemInfo.Controls.Add(this.txtItemReuseTimer);
      this.grpSpecializedItemInfo.Controls.Add(this.txtItemEquipDelay);
      this.grpSpecializedItemInfo.Controls.Add(this.txtItemMaxCharges);
      this.grpSpecializedItemInfo.Controls.Add(this.txtItemResourceID);
      this.grpSpecializedItemInfo.Controls.Add(this.lblItemSkill);
      this.grpSpecializedItemInfo.Controls.Add(this.lblItemDelay);
      this.grpSpecializedItemInfo.Controls.Add(this.lblItemDamage);
      this.grpSpecializedItemInfo.Controls.Add(this.txtItemSkill);
      this.grpSpecializedItemInfo.Controls.Add(this.txtItemDelay);
      this.grpSpecializedItemInfo.Controls.Add(this.txtItemDamage);
      this.grpSpecializedItemInfo.Controls.Add(this.lblItemShieldSize);
      this.grpSpecializedItemInfo.Controls.Add(this.txtItemShieldSize);
      this.grpSpecializedItemInfo.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("grpSpecializedItemInfo.Dock")));
      this.grpSpecializedItemInfo.Enabled = ((bool)(resources.GetObject("grpSpecializedItemInfo.Enabled")));
      this.grpSpecializedItemInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.grpSpecializedItemInfo.Font = ((System.Drawing.Font)(resources.GetObject("grpSpecializedItemInfo.Font")));
      this.grpSpecializedItemInfo.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("grpSpecializedItemInfo.ImeMode")));
      this.grpSpecializedItemInfo.Location = ((System.Drawing.Point)(resources.GetObject("grpSpecializedItemInfo.Location")));
      this.grpSpecializedItemInfo.Name = "grpSpecializedItemInfo";
      this.grpSpecializedItemInfo.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("grpSpecializedItemInfo.RightToLeft")));
      this.grpSpecializedItemInfo.Size = ((System.Drawing.Size)(resources.GetObject("grpSpecializedItemInfo.Size")));
      this.grpSpecializedItemInfo.TabIndex = ((int)(resources.GetObject("grpSpecializedItemInfo.TabIndex")));
      this.grpSpecializedItemInfo.TabStop = false;
      this.grpSpecializedItemInfo.Text = resources.GetString("grpSpecializedItemInfo.Text");
      this.ttToolTip.SetToolTip(this.grpSpecializedItemInfo, resources.GetString("grpSpecializedItemInfo.ToolTip"));
      this.grpSpecializedItemInfo.Visible = ((bool)(resources.GetObject("grpSpecializedItemInfo.Visible")));
      // 
      // lblItemRaces
      // 
      this.lblItemRaces.AccessibleDescription = resources.GetString("lblItemRaces.AccessibleDescription");
      this.lblItemRaces.AccessibleName = resources.GetString("lblItemRaces.AccessibleName");
      this.lblItemRaces.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblItemRaces.Anchor")));
      this.lblItemRaces.AutoSize = ((bool)(resources.GetObject("lblItemRaces.AutoSize")));
      this.lblItemRaces.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblItemRaces.Dock")));
      this.lblItemRaces.Enabled = ((bool)(resources.GetObject("lblItemRaces.Enabled")));
      this.lblItemRaces.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemRaces.Font = ((System.Drawing.Font)(resources.GetObject("lblItemRaces.Font")));
      this.lblItemRaces.Image = ((System.Drawing.Image)(resources.GetObject("lblItemRaces.Image")));
      this.lblItemRaces.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemRaces.ImageAlign")));
      this.lblItemRaces.ImageIndex = ((int)(resources.GetObject("lblItemRaces.ImageIndex")));
      this.lblItemRaces.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblItemRaces.ImeMode")));
      this.lblItemRaces.Location = ((System.Drawing.Point)(resources.GetObject("lblItemRaces.Location")));
      this.lblItemRaces.Name = "lblItemRaces";
      this.lblItemRaces.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblItemRaces.RightToLeft")));
      this.lblItemRaces.Size = ((System.Drawing.Size)(resources.GetObject("lblItemRaces.Size")));
      this.lblItemRaces.TabIndex = ((int)(resources.GetObject("lblItemRaces.TabIndex")));
      this.lblItemRaces.Text = resources.GetString("lblItemRaces.Text");
      this.lblItemRaces.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemRaces.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblItemRaces, resources.GetString("lblItemRaces.ToolTip"));
      this.lblItemRaces.Visible = ((bool)(resources.GetObject("lblItemRaces.Visible")));
      // 
      // txtItemRaces
      // 
      this.txtItemRaces.AccessibleDescription = resources.GetString("txtItemRaces.AccessibleDescription");
      this.txtItemRaces.AccessibleName = resources.GetString("txtItemRaces.AccessibleName");
      this.txtItemRaces.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtItemRaces.Anchor")));
      this.txtItemRaces.AutoSize = ((bool)(resources.GetObject("txtItemRaces.AutoSize")));
      this.txtItemRaces.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtItemRaces.BackgroundImage")));
      this.txtItemRaces.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtItemRaces.Dock")));
      this.txtItemRaces.Enabled = ((bool)(resources.GetObject("txtItemRaces.Enabled")));
      this.txtItemRaces.Font = ((System.Drawing.Font)(resources.GetObject("txtItemRaces.Font")));
      this.txtItemRaces.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtItemRaces.ImeMode")));
      this.txtItemRaces.Location = ((System.Drawing.Point)(resources.GetObject("txtItemRaces.Location")));
      this.txtItemRaces.MaxLength = ((int)(resources.GetObject("txtItemRaces.MaxLength")));
      this.txtItemRaces.Multiline = ((bool)(resources.GetObject("txtItemRaces.Multiline")));
      this.txtItemRaces.Name = "txtItemRaces";
      this.txtItemRaces.PasswordChar = ((char)(resources.GetObject("txtItemRaces.PasswordChar")));
      this.txtItemRaces.ReadOnly = true;
      this.txtItemRaces.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtItemRaces.RightToLeft")));
      this.txtItemRaces.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtItemRaces.ScrollBars")));
      this.txtItemRaces.Size = ((System.Drawing.Size)(resources.GetObject("txtItemRaces.Size")));
      this.txtItemRaces.TabIndex = ((int)(resources.GetObject("txtItemRaces.TabIndex")));
      this.txtItemRaces.Text = resources.GetString("txtItemRaces.Text");
      this.txtItemRaces.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtItemRaces.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtItemRaces, resources.GetString("txtItemRaces.ToolTip"));
      this.txtItemRaces.Visible = ((bool)(resources.GetObject("txtItemRaces.Visible")));
      this.txtItemRaces.WordWrap = ((bool)(resources.GetObject("txtItemRaces.WordWrap")));
      // 
      // lblItemSlots
      // 
      this.lblItemSlots.AccessibleDescription = resources.GetString("lblItemSlots.AccessibleDescription");
      this.lblItemSlots.AccessibleName = resources.GetString("lblItemSlots.AccessibleName");
      this.lblItemSlots.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblItemSlots.Anchor")));
      this.lblItemSlots.AutoSize = ((bool)(resources.GetObject("lblItemSlots.AutoSize")));
      this.lblItemSlots.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblItemSlots.Dock")));
      this.lblItemSlots.Enabled = ((bool)(resources.GetObject("lblItemSlots.Enabled")));
      this.lblItemSlots.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemSlots.Font = ((System.Drawing.Font)(resources.GetObject("lblItemSlots.Font")));
      this.lblItemSlots.Image = ((System.Drawing.Image)(resources.GetObject("lblItemSlots.Image")));
      this.lblItemSlots.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemSlots.ImageAlign")));
      this.lblItemSlots.ImageIndex = ((int)(resources.GetObject("lblItemSlots.ImageIndex")));
      this.lblItemSlots.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblItemSlots.ImeMode")));
      this.lblItemSlots.Location = ((System.Drawing.Point)(resources.GetObject("lblItemSlots.Location")));
      this.lblItemSlots.Name = "lblItemSlots";
      this.lblItemSlots.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblItemSlots.RightToLeft")));
      this.lblItemSlots.Size = ((System.Drawing.Size)(resources.GetObject("lblItemSlots.Size")));
      this.lblItemSlots.TabIndex = ((int)(resources.GetObject("lblItemSlots.TabIndex")));
      this.lblItemSlots.Text = resources.GetString("lblItemSlots.Text");
      this.lblItemSlots.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemSlots.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblItemSlots, resources.GetString("lblItemSlots.ToolTip"));
      this.lblItemSlots.Visible = ((bool)(resources.GetObject("lblItemSlots.Visible")));
      // 
      // txtItemSlots
      // 
      this.txtItemSlots.AccessibleDescription = resources.GetString("txtItemSlots.AccessibleDescription");
      this.txtItemSlots.AccessibleName = resources.GetString("txtItemSlots.AccessibleName");
      this.txtItemSlots.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtItemSlots.Anchor")));
      this.txtItemSlots.AutoSize = ((bool)(resources.GetObject("txtItemSlots.AutoSize")));
      this.txtItemSlots.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtItemSlots.BackgroundImage")));
      this.txtItemSlots.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtItemSlots.Dock")));
      this.txtItemSlots.Enabled = ((bool)(resources.GetObject("txtItemSlots.Enabled")));
      this.txtItemSlots.Font = ((System.Drawing.Font)(resources.GetObject("txtItemSlots.Font")));
      this.txtItemSlots.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtItemSlots.ImeMode")));
      this.txtItemSlots.Location = ((System.Drawing.Point)(resources.GetObject("txtItemSlots.Location")));
      this.txtItemSlots.MaxLength = ((int)(resources.GetObject("txtItemSlots.MaxLength")));
      this.txtItemSlots.Multiline = ((bool)(resources.GetObject("txtItemSlots.Multiline")));
      this.txtItemSlots.Name = "txtItemSlots";
      this.txtItemSlots.PasswordChar = ((char)(resources.GetObject("txtItemSlots.PasswordChar")));
      this.txtItemSlots.ReadOnly = true;
      this.txtItemSlots.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtItemSlots.RightToLeft")));
      this.txtItemSlots.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtItemSlots.ScrollBars")));
      this.txtItemSlots.Size = ((System.Drawing.Size)(resources.GetObject("txtItemSlots.Size")));
      this.txtItemSlots.TabIndex = ((int)(resources.GetObject("txtItemSlots.TabIndex")));
      this.txtItemSlots.Text = resources.GetString("txtItemSlots.Text");
      this.txtItemSlots.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtItemSlots.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtItemSlots, resources.GetString("txtItemSlots.ToolTip"));
      this.txtItemSlots.Visible = ((bool)(resources.GetObject("txtItemSlots.Visible")));
      this.txtItemSlots.WordWrap = ((bool)(resources.GetObject("txtItemSlots.WordWrap")));
      // 
      // lblItemJobs
      // 
      this.lblItemJobs.AccessibleDescription = resources.GetString("lblItemJobs.AccessibleDescription");
      this.lblItemJobs.AccessibleName = resources.GetString("lblItemJobs.AccessibleName");
      this.lblItemJobs.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblItemJobs.Anchor")));
      this.lblItemJobs.AutoSize = ((bool)(resources.GetObject("lblItemJobs.AutoSize")));
      this.lblItemJobs.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblItemJobs.Dock")));
      this.lblItemJobs.Enabled = ((bool)(resources.GetObject("lblItemJobs.Enabled")));
      this.lblItemJobs.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemJobs.Font = ((System.Drawing.Font)(resources.GetObject("lblItemJobs.Font")));
      this.lblItemJobs.Image = ((System.Drawing.Image)(resources.GetObject("lblItemJobs.Image")));
      this.lblItemJobs.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemJobs.ImageAlign")));
      this.lblItemJobs.ImageIndex = ((int)(resources.GetObject("lblItemJobs.ImageIndex")));
      this.lblItemJobs.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblItemJobs.ImeMode")));
      this.lblItemJobs.Location = ((System.Drawing.Point)(resources.GetObject("lblItemJobs.Location")));
      this.lblItemJobs.Name = "lblItemJobs";
      this.lblItemJobs.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblItemJobs.RightToLeft")));
      this.lblItemJobs.Size = ((System.Drawing.Size)(resources.GetObject("lblItemJobs.Size")));
      this.lblItemJobs.TabIndex = ((int)(resources.GetObject("lblItemJobs.TabIndex")));
      this.lblItemJobs.Text = resources.GetString("lblItemJobs.Text");
      this.lblItemJobs.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemJobs.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblItemJobs, resources.GetString("lblItemJobs.ToolTip"));
      this.lblItemJobs.Visible = ((bool)(resources.GetObject("lblItemJobs.Visible")));
      // 
      // txtItemJobs
      // 
      this.txtItemJobs.AccessibleDescription = resources.GetString("txtItemJobs.AccessibleDescription");
      this.txtItemJobs.AccessibleName = resources.GetString("txtItemJobs.AccessibleName");
      this.txtItemJobs.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtItemJobs.Anchor")));
      this.txtItemJobs.AutoSize = ((bool)(resources.GetObject("txtItemJobs.AutoSize")));
      this.txtItemJobs.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtItemJobs.BackgroundImage")));
      this.txtItemJobs.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtItemJobs.Dock")));
      this.txtItemJobs.Enabled = ((bool)(resources.GetObject("txtItemJobs.Enabled")));
      this.txtItemJobs.Font = ((System.Drawing.Font)(resources.GetObject("txtItemJobs.Font")));
      this.txtItemJobs.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtItemJobs.ImeMode")));
      this.txtItemJobs.Location = ((System.Drawing.Point)(resources.GetObject("txtItemJobs.Location")));
      this.txtItemJobs.MaxLength = ((int)(resources.GetObject("txtItemJobs.MaxLength")));
      this.txtItemJobs.Multiline = ((bool)(resources.GetObject("txtItemJobs.Multiline")));
      this.txtItemJobs.Name = "txtItemJobs";
      this.txtItemJobs.PasswordChar = ((char)(resources.GetObject("txtItemJobs.PasswordChar")));
      this.txtItemJobs.ReadOnly = true;
      this.txtItemJobs.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtItemJobs.RightToLeft")));
      this.txtItemJobs.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtItemJobs.ScrollBars")));
      this.txtItemJobs.Size = ((System.Drawing.Size)(resources.GetObject("txtItemJobs.Size")));
      this.txtItemJobs.TabIndex = ((int)(resources.GetObject("txtItemJobs.TabIndex")));
      this.txtItemJobs.Text = resources.GetString("txtItemJobs.Text");
      this.txtItemJobs.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtItemJobs.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtItemJobs, resources.GetString("txtItemJobs.ToolTip"));
      this.txtItemJobs.Visible = ((bool)(resources.GetObject("txtItemJobs.Visible")));
      this.txtItemJobs.WordWrap = ((bool)(resources.GetObject("txtItemJobs.WordWrap")));
      // 
      // lblItemLevel
      // 
      this.lblItemLevel.AccessibleDescription = resources.GetString("lblItemLevel.AccessibleDescription");
      this.lblItemLevel.AccessibleName = resources.GetString("lblItemLevel.AccessibleName");
      this.lblItemLevel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblItemLevel.Anchor")));
      this.lblItemLevel.AutoSize = ((bool)(resources.GetObject("lblItemLevel.AutoSize")));
      this.lblItemLevel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblItemLevel.Dock")));
      this.lblItemLevel.Enabled = ((bool)(resources.GetObject("lblItemLevel.Enabled")));
      this.lblItemLevel.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemLevel.Font = ((System.Drawing.Font)(resources.GetObject("lblItemLevel.Font")));
      this.lblItemLevel.Image = ((System.Drawing.Image)(resources.GetObject("lblItemLevel.Image")));
      this.lblItemLevel.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemLevel.ImageAlign")));
      this.lblItemLevel.ImageIndex = ((int)(resources.GetObject("lblItemLevel.ImageIndex")));
      this.lblItemLevel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblItemLevel.ImeMode")));
      this.lblItemLevel.Location = ((System.Drawing.Point)(resources.GetObject("lblItemLevel.Location")));
      this.lblItemLevel.Name = "lblItemLevel";
      this.lblItemLevel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblItemLevel.RightToLeft")));
      this.lblItemLevel.Size = ((System.Drawing.Size)(resources.GetObject("lblItemLevel.Size")));
      this.lblItemLevel.TabIndex = ((int)(resources.GetObject("lblItemLevel.TabIndex")));
      this.lblItemLevel.Text = resources.GetString("lblItemLevel.Text");
      this.lblItemLevel.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemLevel.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblItemLevel, resources.GetString("lblItemLevel.ToolTip"));
      this.lblItemLevel.Visible = ((bool)(resources.GetObject("lblItemLevel.Visible")));
      // 
      // txtItemLevel
      // 
      this.txtItemLevel.AccessibleDescription = resources.GetString("txtItemLevel.AccessibleDescription");
      this.txtItemLevel.AccessibleName = resources.GetString("txtItemLevel.AccessibleName");
      this.txtItemLevel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtItemLevel.Anchor")));
      this.txtItemLevel.AutoSize = ((bool)(resources.GetObject("txtItemLevel.AutoSize")));
      this.txtItemLevel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtItemLevel.BackgroundImage")));
      this.txtItemLevel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtItemLevel.Dock")));
      this.txtItemLevel.Enabled = ((bool)(resources.GetObject("txtItemLevel.Enabled")));
      this.txtItemLevel.Font = ((System.Drawing.Font)(resources.GetObject("txtItemLevel.Font")));
      this.txtItemLevel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtItemLevel.ImeMode")));
      this.txtItemLevel.Location = ((System.Drawing.Point)(resources.GetObject("txtItemLevel.Location")));
      this.txtItemLevel.MaxLength = ((int)(resources.GetObject("txtItemLevel.MaxLength")));
      this.txtItemLevel.Multiline = ((bool)(resources.GetObject("txtItemLevel.Multiline")));
      this.txtItemLevel.Name = "txtItemLevel";
      this.txtItemLevel.PasswordChar = ((char)(resources.GetObject("txtItemLevel.PasswordChar")));
      this.txtItemLevel.ReadOnly = true;
      this.txtItemLevel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtItemLevel.RightToLeft")));
      this.txtItemLevel.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtItemLevel.ScrollBars")));
      this.txtItemLevel.Size = ((System.Drawing.Size)(resources.GetObject("txtItemLevel.Size")));
      this.txtItemLevel.TabIndex = ((int)(resources.GetObject("txtItemLevel.TabIndex")));
      this.txtItemLevel.Text = resources.GetString("txtItemLevel.Text");
      this.txtItemLevel.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtItemLevel.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtItemLevel, resources.GetString("txtItemLevel.ToolTip"));
      this.txtItemLevel.Visible = ((bool)(resources.GetObject("txtItemLevel.Visible")));
      this.txtItemLevel.WordWrap = ((bool)(resources.GetObject("txtItemLevel.WordWrap")));
      // 
      // lblItemEquipDelay
      // 
      this.lblItemEquipDelay.AccessibleDescription = resources.GetString("lblItemEquipDelay.AccessibleDescription");
      this.lblItemEquipDelay.AccessibleName = resources.GetString("lblItemEquipDelay.AccessibleName");
      this.lblItemEquipDelay.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblItemEquipDelay.Anchor")));
      this.lblItemEquipDelay.AutoSize = ((bool)(resources.GetObject("lblItemEquipDelay.AutoSize")));
      this.lblItemEquipDelay.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblItemEquipDelay.Dock")));
      this.lblItemEquipDelay.Enabled = ((bool)(resources.GetObject("lblItemEquipDelay.Enabled")));
      this.lblItemEquipDelay.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemEquipDelay.Font = ((System.Drawing.Font)(resources.GetObject("lblItemEquipDelay.Font")));
      this.lblItemEquipDelay.Image = ((System.Drawing.Image)(resources.GetObject("lblItemEquipDelay.Image")));
      this.lblItemEquipDelay.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemEquipDelay.ImageAlign")));
      this.lblItemEquipDelay.ImageIndex = ((int)(resources.GetObject("lblItemEquipDelay.ImageIndex")));
      this.lblItemEquipDelay.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblItemEquipDelay.ImeMode")));
      this.lblItemEquipDelay.Location = ((System.Drawing.Point)(resources.GetObject("lblItemEquipDelay.Location")));
      this.lblItemEquipDelay.Name = "lblItemEquipDelay";
      this.lblItemEquipDelay.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblItemEquipDelay.RightToLeft")));
      this.lblItemEquipDelay.Size = ((System.Drawing.Size)(resources.GetObject("lblItemEquipDelay.Size")));
      this.lblItemEquipDelay.TabIndex = ((int)(resources.GetObject("lblItemEquipDelay.TabIndex")));
      this.lblItemEquipDelay.Text = resources.GetString("lblItemEquipDelay.Text");
      this.lblItemEquipDelay.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemEquipDelay.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblItemEquipDelay, resources.GetString("lblItemEquipDelay.ToolTip"));
      this.lblItemEquipDelay.Visible = ((bool)(resources.GetObject("lblItemEquipDelay.Visible")));
      // 
      // lblItemReuseTimer
      // 
      this.lblItemReuseTimer.AccessibleDescription = resources.GetString("lblItemReuseTimer.AccessibleDescription");
      this.lblItemReuseTimer.AccessibleName = resources.GetString("lblItemReuseTimer.AccessibleName");
      this.lblItemReuseTimer.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblItemReuseTimer.Anchor")));
      this.lblItemReuseTimer.AutoSize = ((bool)(resources.GetObject("lblItemReuseTimer.AutoSize")));
      this.lblItemReuseTimer.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblItemReuseTimer.Dock")));
      this.lblItemReuseTimer.Enabled = ((bool)(resources.GetObject("lblItemReuseTimer.Enabled")));
      this.lblItemReuseTimer.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemReuseTimer.Font = ((System.Drawing.Font)(resources.GetObject("lblItemReuseTimer.Font")));
      this.lblItemReuseTimer.Image = ((System.Drawing.Image)(resources.GetObject("lblItemReuseTimer.Image")));
      this.lblItemReuseTimer.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemReuseTimer.ImageAlign")));
      this.lblItemReuseTimer.ImageIndex = ((int)(resources.GetObject("lblItemReuseTimer.ImageIndex")));
      this.lblItemReuseTimer.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblItemReuseTimer.ImeMode")));
      this.lblItemReuseTimer.Location = ((System.Drawing.Point)(resources.GetObject("lblItemReuseTimer.Location")));
      this.lblItemReuseTimer.Name = "lblItemReuseTimer";
      this.lblItemReuseTimer.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblItemReuseTimer.RightToLeft")));
      this.lblItemReuseTimer.Size = ((System.Drawing.Size)(resources.GetObject("lblItemReuseTimer.Size")));
      this.lblItemReuseTimer.TabIndex = ((int)(resources.GetObject("lblItemReuseTimer.TabIndex")));
      this.lblItemReuseTimer.Text = resources.GetString("lblItemReuseTimer.Text");
      this.lblItemReuseTimer.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemReuseTimer.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblItemReuseTimer, resources.GetString("lblItemReuseTimer.ToolTip"));
      this.lblItemReuseTimer.Visible = ((bool)(resources.GetObject("lblItemReuseTimer.Visible")));
      // 
      // lblItemMaxCharges
      // 
      this.lblItemMaxCharges.AccessibleDescription = resources.GetString("lblItemMaxCharges.AccessibleDescription");
      this.lblItemMaxCharges.AccessibleName = resources.GetString("lblItemMaxCharges.AccessibleName");
      this.lblItemMaxCharges.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblItemMaxCharges.Anchor")));
      this.lblItemMaxCharges.AutoSize = ((bool)(resources.GetObject("lblItemMaxCharges.AutoSize")));
      this.lblItemMaxCharges.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblItemMaxCharges.Dock")));
      this.lblItemMaxCharges.Enabled = ((bool)(resources.GetObject("lblItemMaxCharges.Enabled")));
      this.lblItemMaxCharges.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemMaxCharges.Font = ((System.Drawing.Font)(resources.GetObject("lblItemMaxCharges.Font")));
      this.lblItemMaxCharges.Image = ((System.Drawing.Image)(resources.GetObject("lblItemMaxCharges.Image")));
      this.lblItemMaxCharges.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemMaxCharges.ImageAlign")));
      this.lblItemMaxCharges.ImageIndex = ((int)(resources.GetObject("lblItemMaxCharges.ImageIndex")));
      this.lblItemMaxCharges.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblItemMaxCharges.ImeMode")));
      this.lblItemMaxCharges.Location = ((System.Drawing.Point)(resources.GetObject("lblItemMaxCharges.Location")));
      this.lblItemMaxCharges.Name = "lblItemMaxCharges";
      this.lblItemMaxCharges.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblItemMaxCharges.RightToLeft")));
      this.lblItemMaxCharges.Size = ((System.Drawing.Size)(resources.GetObject("lblItemMaxCharges.Size")));
      this.lblItemMaxCharges.TabIndex = ((int)(resources.GetObject("lblItemMaxCharges.TabIndex")));
      this.lblItemMaxCharges.Text = resources.GetString("lblItemMaxCharges.Text");
      this.lblItemMaxCharges.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemMaxCharges.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblItemMaxCharges, resources.GetString("lblItemMaxCharges.ToolTip"));
      this.lblItemMaxCharges.Visible = ((bool)(resources.GetObject("lblItemMaxCharges.Visible")));
      // 
      // lblItemResourceID
      // 
      this.lblItemResourceID.AccessibleDescription = resources.GetString("lblItemResourceID.AccessibleDescription");
      this.lblItemResourceID.AccessibleName = resources.GetString("lblItemResourceID.AccessibleName");
      this.lblItemResourceID.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblItemResourceID.Anchor")));
      this.lblItemResourceID.AutoSize = ((bool)(resources.GetObject("lblItemResourceID.AutoSize")));
      this.lblItemResourceID.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblItemResourceID.Dock")));
      this.lblItemResourceID.Enabled = ((bool)(resources.GetObject("lblItemResourceID.Enabled")));
      this.lblItemResourceID.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemResourceID.Font = ((System.Drawing.Font)(resources.GetObject("lblItemResourceID.Font")));
      this.lblItemResourceID.Image = ((System.Drawing.Image)(resources.GetObject("lblItemResourceID.Image")));
      this.lblItemResourceID.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemResourceID.ImageAlign")));
      this.lblItemResourceID.ImageIndex = ((int)(resources.GetObject("lblItemResourceID.ImageIndex")));
      this.lblItemResourceID.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblItemResourceID.ImeMode")));
      this.lblItemResourceID.Location = ((System.Drawing.Point)(resources.GetObject("lblItemResourceID.Location")));
      this.lblItemResourceID.Name = "lblItemResourceID";
      this.lblItemResourceID.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblItemResourceID.RightToLeft")));
      this.lblItemResourceID.Size = ((System.Drawing.Size)(resources.GetObject("lblItemResourceID.Size")));
      this.lblItemResourceID.TabIndex = ((int)(resources.GetObject("lblItemResourceID.TabIndex")));
      this.lblItemResourceID.Text = resources.GetString("lblItemResourceID.Text");
      this.lblItemResourceID.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemResourceID.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblItemResourceID, resources.GetString("lblItemResourceID.ToolTip"));
      this.lblItemResourceID.Visible = ((bool)(resources.GetObject("lblItemResourceID.Visible")));
      // 
      // txtItemReuseTimer
      // 
      this.txtItemReuseTimer.AccessibleDescription = resources.GetString("txtItemReuseTimer.AccessibleDescription");
      this.txtItemReuseTimer.AccessibleName = resources.GetString("txtItemReuseTimer.AccessibleName");
      this.txtItemReuseTimer.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtItemReuseTimer.Anchor")));
      this.txtItemReuseTimer.AutoSize = ((bool)(resources.GetObject("txtItemReuseTimer.AutoSize")));
      this.txtItemReuseTimer.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtItemReuseTimer.BackgroundImage")));
      this.txtItemReuseTimer.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtItemReuseTimer.Dock")));
      this.txtItemReuseTimer.Enabled = ((bool)(resources.GetObject("txtItemReuseTimer.Enabled")));
      this.txtItemReuseTimer.Font = ((System.Drawing.Font)(resources.GetObject("txtItemReuseTimer.Font")));
      this.txtItemReuseTimer.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtItemReuseTimer.ImeMode")));
      this.txtItemReuseTimer.Location = ((System.Drawing.Point)(resources.GetObject("txtItemReuseTimer.Location")));
      this.txtItemReuseTimer.MaxLength = ((int)(resources.GetObject("txtItemReuseTimer.MaxLength")));
      this.txtItemReuseTimer.Multiline = ((bool)(resources.GetObject("txtItemReuseTimer.Multiline")));
      this.txtItemReuseTimer.Name = "txtItemReuseTimer";
      this.txtItemReuseTimer.PasswordChar = ((char)(resources.GetObject("txtItemReuseTimer.PasswordChar")));
      this.txtItemReuseTimer.ReadOnly = true;
      this.txtItemReuseTimer.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtItemReuseTimer.RightToLeft")));
      this.txtItemReuseTimer.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtItemReuseTimer.ScrollBars")));
      this.txtItemReuseTimer.Size = ((System.Drawing.Size)(resources.GetObject("txtItemReuseTimer.Size")));
      this.txtItemReuseTimer.TabIndex = ((int)(resources.GetObject("txtItemReuseTimer.TabIndex")));
      this.txtItemReuseTimer.Text = resources.GetString("txtItemReuseTimer.Text");
      this.txtItemReuseTimer.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtItemReuseTimer.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtItemReuseTimer, resources.GetString("txtItemReuseTimer.ToolTip"));
      this.txtItemReuseTimer.Visible = ((bool)(resources.GetObject("txtItemReuseTimer.Visible")));
      this.txtItemReuseTimer.WordWrap = ((bool)(resources.GetObject("txtItemReuseTimer.WordWrap")));
      // 
      // txtItemEquipDelay
      // 
      this.txtItemEquipDelay.AccessibleDescription = resources.GetString("txtItemEquipDelay.AccessibleDescription");
      this.txtItemEquipDelay.AccessibleName = resources.GetString("txtItemEquipDelay.AccessibleName");
      this.txtItemEquipDelay.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtItemEquipDelay.Anchor")));
      this.txtItemEquipDelay.AutoSize = ((bool)(resources.GetObject("txtItemEquipDelay.AutoSize")));
      this.txtItemEquipDelay.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtItemEquipDelay.BackgroundImage")));
      this.txtItemEquipDelay.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtItemEquipDelay.Dock")));
      this.txtItemEquipDelay.Enabled = ((bool)(resources.GetObject("txtItemEquipDelay.Enabled")));
      this.txtItemEquipDelay.Font = ((System.Drawing.Font)(resources.GetObject("txtItemEquipDelay.Font")));
      this.txtItemEquipDelay.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtItemEquipDelay.ImeMode")));
      this.txtItemEquipDelay.Location = ((System.Drawing.Point)(resources.GetObject("txtItemEquipDelay.Location")));
      this.txtItemEquipDelay.MaxLength = ((int)(resources.GetObject("txtItemEquipDelay.MaxLength")));
      this.txtItemEquipDelay.Multiline = ((bool)(resources.GetObject("txtItemEquipDelay.Multiline")));
      this.txtItemEquipDelay.Name = "txtItemEquipDelay";
      this.txtItemEquipDelay.PasswordChar = ((char)(resources.GetObject("txtItemEquipDelay.PasswordChar")));
      this.txtItemEquipDelay.ReadOnly = true;
      this.txtItemEquipDelay.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtItemEquipDelay.RightToLeft")));
      this.txtItemEquipDelay.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtItemEquipDelay.ScrollBars")));
      this.txtItemEquipDelay.Size = ((System.Drawing.Size)(resources.GetObject("txtItemEquipDelay.Size")));
      this.txtItemEquipDelay.TabIndex = ((int)(resources.GetObject("txtItemEquipDelay.TabIndex")));
      this.txtItemEquipDelay.Text = resources.GetString("txtItemEquipDelay.Text");
      this.txtItemEquipDelay.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtItemEquipDelay.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtItemEquipDelay, resources.GetString("txtItemEquipDelay.ToolTip"));
      this.txtItemEquipDelay.Visible = ((bool)(resources.GetObject("txtItemEquipDelay.Visible")));
      this.txtItemEquipDelay.WordWrap = ((bool)(resources.GetObject("txtItemEquipDelay.WordWrap")));
      // 
      // txtItemMaxCharges
      // 
      this.txtItemMaxCharges.AccessibleDescription = resources.GetString("txtItemMaxCharges.AccessibleDescription");
      this.txtItemMaxCharges.AccessibleName = resources.GetString("txtItemMaxCharges.AccessibleName");
      this.txtItemMaxCharges.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtItemMaxCharges.Anchor")));
      this.txtItemMaxCharges.AutoSize = ((bool)(resources.GetObject("txtItemMaxCharges.AutoSize")));
      this.txtItemMaxCharges.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtItemMaxCharges.BackgroundImage")));
      this.txtItemMaxCharges.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtItemMaxCharges.Dock")));
      this.txtItemMaxCharges.Enabled = ((bool)(resources.GetObject("txtItemMaxCharges.Enabled")));
      this.txtItemMaxCharges.Font = ((System.Drawing.Font)(resources.GetObject("txtItemMaxCharges.Font")));
      this.txtItemMaxCharges.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtItemMaxCharges.ImeMode")));
      this.txtItemMaxCharges.Location = ((System.Drawing.Point)(resources.GetObject("txtItemMaxCharges.Location")));
      this.txtItemMaxCharges.MaxLength = ((int)(resources.GetObject("txtItemMaxCharges.MaxLength")));
      this.txtItemMaxCharges.Multiline = ((bool)(resources.GetObject("txtItemMaxCharges.Multiline")));
      this.txtItemMaxCharges.Name = "txtItemMaxCharges";
      this.txtItemMaxCharges.PasswordChar = ((char)(resources.GetObject("txtItemMaxCharges.PasswordChar")));
      this.txtItemMaxCharges.ReadOnly = true;
      this.txtItemMaxCharges.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtItemMaxCharges.RightToLeft")));
      this.txtItemMaxCharges.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtItemMaxCharges.ScrollBars")));
      this.txtItemMaxCharges.Size = ((System.Drawing.Size)(resources.GetObject("txtItemMaxCharges.Size")));
      this.txtItemMaxCharges.TabIndex = ((int)(resources.GetObject("txtItemMaxCharges.TabIndex")));
      this.txtItemMaxCharges.Text = resources.GetString("txtItemMaxCharges.Text");
      this.txtItemMaxCharges.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtItemMaxCharges.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtItemMaxCharges, resources.GetString("txtItemMaxCharges.ToolTip"));
      this.txtItemMaxCharges.Visible = ((bool)(resources.GetObject("txtItemMaxCharges.Visible")));
      this.txtItemMaxCharges.WordWrap = ((bool)(resources.GetObject("txtItemMaxCharges.WordWrap")));
      // 
      // txtItemResourceID
      // 
      this.txtItemResourceID.AccessibleDescription = resources.GetString("txtItemResourceID.AccessibleDescription");
      this.txtItemResourceID.AccessibleName = resources.GetString("txtItemResourceID.AccessibleName");
      this.txtItemResourceID.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtItemResourceID.Anchor")));
      this.txtItemResourceID.AutoSize = ((bool)(resources.GetObject("txtItemResourceID.AutoSize")));
      this.txtItemResourceID.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtItemResourceID.BackgroundImage")));
      this.txtItemResourceID.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtItemResourceID.Dock")));
      this.txtItemResourceID.Enabled = ((bool)(resources.GetObject("txtItemResourceID.Enabled")));
      this.txtItemResourceID.Font = ((System.Drawing.Font)(resources.GetObject("txtItemResourceID.Font")));
      this.txtItemResourceID.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtItemResourceID.ImeMode")));
      this.txtItemResourceID.Location = ((System.Drawing.Point)(resources.GetObject("txtItemResourceID.Location")));
      this.txtItemResourceID.MaxLength = ((int)(resources.GetObject("txtItemResourceID.MaxLength")));
      this.txtItemResourceID.Multiline = ((bool)(resources.GetObject("txtItemResourceID.Multiline")));
      this.txtItemResourceID.Name = "txtItemResourceID";
      this.txtItemResourceID.PasswordChar = ((char)(resources.GetObject("txtItemResourceID.PasswordChar")));
      this.txtItemResourceID.ReadOnly = true;
      this.txtItemResourceID.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtItemResourceID.RightToLeft")));
      this.txtItemResourceID.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtItemResourceID.ScrollBars")));
      this.txtItemResourceID.Size = ((System.Drawing.Size)(resources.GetObject("txtItemResourceID.Size")));
      this.txtItemResourceID.TabIndex = ((int)(resources.GetObject("txtItemResourceID.TabIndex")));
      this.txtItemResourceID.Text = resources.GetString("txtItemResourceID.Text");
      this.txtItemResourceID.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtItemResourceID.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtItemResourceID, resources.GetString("txtItemResourceID.ToolTip"));
      this.txtItemResourceID.Visible = ((bool)(resources.GetObject("txtItemResourceID.Visible")));
      this.txtItemResourceID.WordWrap = ((bool)(resources.GetObject("txtItemResourceID.WordWrap")));
      // 
      // lblItemSkill
      // 
      this.lblItemSkill.AccessibleDescription = resources.GetString("lblItemSkill.AccessibleDescription");
      this.lblItemSkill.AccessibleName = resources.GetString("lblItemSkill.AccessibleName");
      this.lblItemSkill.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblItemSkill.Anchor")));
      this.lblItemSkill.AutoSize = ((bool)(resources.GetObject("lblItemSkill.AutoSize")));
      this.lblItemSkill.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblItemSkill.Dock")));
      this.lblItemSkill.Enabled = ((bool)(resources.GetObject("lblItemSkill.Enabled")));
      this.lblItemSkill.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemSkill.Font = ((System.Drawing.Font)(resources.GetObject("lblItemSkill.Font")));
      this.lblItemSkill.Image = ((System.Drawing.Image)(resources.GetObject("lblItemSkill.Image")));
      this.lblItemSkill.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemSkill.ImageAlign")));
      this.lblItemSkill.ImageIndex = ((int)(resources.GetObject("lblItemSkill.ImageIndex")));
      this.lblItemSkill.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblItemSkill.ImeMode")));
      this.lblItemSkill.Location = ((System.Drawing.Point)(resources.GetObject("lblItemSkill.Location")));
      this.lblItemSkill.Name = "lblItemSkill";
      this.lblItemSkill.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblItemSkill.RightToLeft")));
      this.lblItemSkill.Size = ((System.Drawing.Size)(resources.GetObject("lblItemSkill.Size")));
      this.lblItemSkill.TabIndex = ((int)(resources.GetObject("lblItemSkill.TabIndex")));
      this.lblItemSkill.Text = resources.GetString("lblItemSkill.Text");
      this.lblItemSkill.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemSkill.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblItemSkill, resources.GetString("lblItemSkill.ToolTip"));
      this.lblItemSkill.Visible = ((bool)(resources.GetObject("lblItemSkill.Visible")));
      // 
      // lblItemDelay
      // 
      this.lblItemDelay.AccessibleDescription = resources.GetString("lblItemDelay.AccessibleDescription");
      this.lblItemDelay.AccessibleName = resources.GetString("lblItemDelay.AccessibleName");
      this.lblItemDelay.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblItemDelay.Anchor")));
      this.lblItemDelay.AutoSize = ((bool)(resources.GetObject("lblItemDelay.AutoSize")));
      this.lblItemDelay.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblItemDelay.Dock")));
      this.lblItemDelay.Enabled = ((bool)(resources.GetObject("lblItemDelay.Enabled")));
      this.lblItemDelay.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemDelay.Font = ((System.Drawing.Font)(resources.GetObject("lblItemDelay.Font")));
      this.lblItemDelay.Image = ((System.Drawing.Image)(resources.GetObject("lblItemDelay.Image")));
      this.lblItemDelay.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemDelay.ImageAlign")));
      this.lblItemDelay.ImageIndex = ((int)(resources.GetObject("lblItemDelay.ImageIndex")));
      this.lblItemDelay.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblItemDelay.ImeMode")));
      this.lblItemDelay.Location = ((System.Drawing.Point)(resources.GetObject("lblItemDelay.Location")));
      this.lblItemDelay.Name = "lblItemDelay";
      this.lblItemDelay.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblItemDelay.RightToLeft")));
      this.lblItemDelay.Size = ((System.Drawing.Size)(resources.GetObject("lblItemDelay.Size")));
      this.lblItemDelay.TabIndex = ((int)(resources.GetObject("lblItemDelay.TabIndex")));
      this.lblItemDelay.Text = resources.GetString("lblItemDelay.Text");
      this.lblItemDelay.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemDelay.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblItemDelay, resources.GetString("lblItemDelay.ToolTip"));
      this.lblItemDelay.Visible = ((bool)(resources.GetObject("lblItemDelay.Visible")));
      // 
      // lblItemDamage
      // 
      this.lblItemDamage.AccessibleDescription = resources.GetString("lblItemDamage.AccessibleDescription");
      this.lblItemDamage.AccessibleName = resources.GetString("lblItemDamage.AccessibleName");
      this.lblItemDamage.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblItemDamage.Anchor")));
      this.lblItemDamage.AutoSize = ((bool)(resources.GetObject("lblItemDamage.AutoSize")));
      this.lblItemDamage.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblItemDamage.Dock")));
      this.lblItemDamage.Enabled = ((bool)(resources.GetObject("lblItemDamage.Enabled")));
      this.lblItemDamage.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemDamage.Font = ((System.Drawing.Font)(resources.GetObject("lblItemDamage.Font")));
      this.lblItemDamage.Image = ((System.Drawing.Image)(resources.GetObject("lblItemDamage.Image")));
      this.lblItemDamage.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemDamage.ImageAlign")));
      this.lblItemDamage.ImageIndex = ((int)(resources.GetObject("lblItemDamage.ImageIndex")));
      this.lblItemDamage.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblItemDamage.ImeMode")));
      this.lblItemDamage.Location = ((System.Drawing.Point)(resources.GetObject("lblItemDamage.Location")));
      this.lblItemDamage.Name = "lblItemDamage";
      this.lblItemDamage.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblItemDamage.RightToLeft")));
      this.lblItemDamage.Size = ((System.Drawing.Size)(resources.GetObject("lblItemDamage.Size")));
      this.lblItemDamage.TabIndex = ((int)(resources.GetObject("lblItemDamage.TabIndex")));
      this.lblItemDamage.Text = resources.GetString("lblItemDamage.Text");
      this.lblItemDamage.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemDamage.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblItemDamage, resources.GetString("lblItemDamage.ToolTip"));
      this.lblItemDamage.Visible = ((bool)(resources.GetObject("lblItemDamage.Visible")));
      // 
      // txtItemSkill
      // 
      this.txtItemSkill.AccessibleDescription = resources.GetString("txtItemSkill.AccessibleDescription");
      this.txtItemSkill.AccessibleName = resources.GetString("txtItemSkill.AccessibleName");
      this.txtItemSkill.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtItemSkill.Anchor")));
      this.txtItemSkill.AutoSize = ((bool)(resources.GetObject("txtItemSkill.AutoSize")));
      this.txtItemSkill.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtItemSkill.BackgroundImage")));
      this.txtItemSkill.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtItemSkill.Dock")));
      this.txtItemSkill.Enabled = ((bool)(resources.GetObject("txtItemSkill.Enabled")));
      this.txtItemSkill.Font = ((System.Drawing.Font)(resources.GetObject("txtItemSkill.Font")));
      this.txtItemSkill.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtItemSkill.ImeMode")));
      this.txtItemSkill.Location = ((System.Drawing.Point)(resources.GetObject("txtItemSkill.Location")));
      this.txtItemSkill.MaxLength = ((int)(resources.GetObject("txtItemSkill.MaxLength")));
      this.txtItemSkill.Multiline = ((bool)(resources.GetObject("txtItemSkill.Multiline")));
      this.txtItemSkill.Name = "txtItemSkill";
      this.txtItemSkill.PasswordChar = ((char)(resources.GetObject("txtItemSkill.PasswordChar")));
      this.txtItemSkill.ReadOnly = true;
      this.txtItemSkill.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtItemSkill.RightToLeft")));
      this.txtItemSkill.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtItemSkill.ScrollBars")));
      this.txtItemSkill.Size = ((System.Drawing.Size)(resources.GetObject("txtItemSkill.Size")));
      this.txtItemSkill.TabIndex = ((int)(resources.GetObject("txtItemSkill.TabIndex")));
      this.txtItemSkill.Text = resources.GetString("txtItemSkill.Text");
      this.txtItemSkill.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtItemSkill.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtItemSkill, resources.GetString("txtItemSkill.ToolTip"));
      this.txtItemSkill.Visible = ((bool)(resources.GetObject("txtItemSkill.Visible")));
      this.txtItemSkill.WordWrap = ((bool)(resources.GetObject("txtItemSkill.WordWrap")));
      // 
      // txtItemDelay
      // 
      this.txtItemDelay.AccessibleDescription = resources.GetString("txtItemDelay.AccessibleDescription");
      this.txtItemDelay.AccessibleName = resources.GetString("txtItemDelay.AccessibleName");
      this.txtItemDelay.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtItemDelay.Anchor")));
      this.txtItemDelay.AutoSize = ((bool)(resources.GetObject("txtItemDelay.AutoSize")));
      this.txtItemDelay.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtItemDelay.BackgroundImage")));
      this.txtItemDelay.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtItemDelay.Dock")));
      this.txtItemDelay.Enabled = ((bool)(resources.GetObject("txtItemDelay.Enabled")));
      this.txtItemDelay.Font = ((System.Drawing.Font)(resources.GetObject("txtItemDelay.Font")));
      this.txtItemDelay.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtItemDelay.ImeMode")));
      this.txtItemDelay.Location = ((System.Drawing.Point)(resources.GetObject("txtItemDelay.Location")));
      this.txtItemDelay.MaxLength = ((int)(resources.GetObject("txtItemDelay.MaxLength")));
      this.txtItemDelay.Multiline = ((bool)(resources.GetObject("txtItemDelay.Multiline")));
      this.txtItemDelay.Name = "txtItemDelay";
      this.txtItemDelay.PasswordChar = ((char)(resources.GetObject("txtItemDelay.PasswordChar")));
      this.txtItemDelay.ReadOnly = true;
      this.txtItemDelay.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtItemDelay.RightToLeft")));
      this.txtItemDelay.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtItemDelay.ScrollBars")));
      this.txtItemDelay.Size = ((System.Drawing.Size)(resources.GetObject("txtItemDelay.Size")));
      this.txtItemDelay.TabIndex = ((int)(resources.GetObject("txtItemDelay.TabIndex")));
      this.txtItemDelay.Text = resources.GetString("txtItemDelay.Text");
      this.txtItemDelay.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtItemDelay.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtItemDelay, resources.GetString("txtItemDelay.ToolTip"));
      this.txtItemDelay.Visible = ((bool)(resources.GetObject("txtItemDelay.Visible")));
      this.txtItemDelay.WordWrap = ((bool)(resources.GetObject("txtItemDelay.WordWrap")));
      // 
      // txtItemDamage
      // 
      this.txtItemDamage.AccessibleDescription = resources.GetString("txtItemDamage.AccessibleDescription");
      this.txtItemDamage.AccessibleName = resources.GetString("txtItemDamage.AccessibleName");
      this.txtItemDamage.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtItemDamage.Anchor")));
      this.txtItemDamage.AutoSize = ((bool)(resources.GetObject("txtItemDamage.AutoSize")));
      this.txtItemDamage.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtItemDamage.BackgroundImage")));
      this.txtItemDamage.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtItemDamage.Dock")));
      this.txtItemDamage.Enabled = ((bool)(resources.GetObject("txtItemDamage.Enabled")));
      this.txtItemDamage.Font = ((System.Drawing.Font)(resources.GetObject("txtItemDamage.Font")));
      this.txtItemDamage.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtItemDamage.ImeMode")));
      this.txtItemDamage.Location = ((System.Drawing.Point)(resources.GetObject("txtItemDamage.Location")));
      this.txtItemDamage.MaxLength = ((int)(resources.GetObject("txtItemDamage.MaxLength")));
      this.txtItemDamage.Multiline = ((bool)(resources.GetObject("txtItemDamage.Multiline")));
      this.txtItemDamage.Name = "txtItemDamage";
      this.txtItemDamage.PasswordChar = ((char)(resources.GetObject("txtItemDamage.PasswordChar")));
      this.txtItemDamage.ReadOnly = true;
      this.txtItemDamage.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtItemDamage.RightToLeft")));
      this.txtItemDamage.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtItemDamage.ScrollBars")));
      this.txtItemDamage.Size = ((System.Drawing.Size)(resources.GetObject("txtItemDamage.Size")));
      this.txtItemDamage.TabIndex = ((int)(resources.GetObject("txtItemDamage.TabIndex")));
      this.txtItemDamage.Text = resources.GetString("txtItemDamage.Text");
      this.txtItemDamage.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtItemDamage.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtItemDamage, resources.GetString("txtItemDamage.ToolTip"));
      this.txtItemDamage.Visible = ((bool)(resources.GetObject("txtItemDamage.Visible")));
      this.txtItemDamage.WordWrap = ((bool)(resources.GetObject("txtItemDamage.WordWrap")));
      // 
      // lblItemShieldSize
      // 
      this.lblItemShieldSize.AccessibleDescription = resources.GetString("lblItemShieldSize.AccessibleDescription");
      this.lblItemShieldSize.AccessibleName = resources.GetString("lblItemShieldSize.AccessibleName");
      this.lblItemShieldSize.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblItemShieldSize.Anchor")));
      this.lblItemShieldSize.AutoSize = ((bool)(resources.GetObject("lblItemShieldSize.AutoSize")));
      this.lblItemShieldSize.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblItemShieldSize.Dock")));
      this.lblItemShieldSize.Enabled = ((bool)(resources.GetObject("lblItemShieldSize.Enabled")));
      this.lblItemShieldSize.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemShieldSize.Font = ((System.Drawing.Font)(resources.GetObject("lblItemShieldSize.Font")));
      this.lblItemShieldSize.Image = ((System.Drawing.Image)(resources.GetObject("lblItemShieldSize.Image")));
      this.lblItemShieldSize.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemShieldSize.ImageAlign")));
      this.lblItemShieldSize.ImageIndex = ((int)(resources.GetObject("lblItemShieldSize.ImageIndex")));
      this.lblItemShieldSize.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblItemShieldSize.ImeMode")));
      this.lblItemShieldSize.Location = ((System.Drawing.Point)(resources.GetObject("lblItemShieldSize.Location")));
      this.lblItemShieldSize.Name = "lblItemShieldSize";
      this.lblItemShieldSize.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblItemShieldSize.RightToLeft")));
      this.lblItemShieldSize.Size = ((System.Drawing.Size)(resources.GetObject("lblItemShieldSize.Size")));
      this.lblItemShieldSize.TabIndex = ((int)(resources.GetObject("lblItemShieldSize.TabIndex")));
      this.lblItemShieldSize.Text = resources.GetString("lblItemShieldSize.Text");
      this.lblItemShieldSize.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemShieldSize.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblItemShieldSize, resources.GetString("lblItemShieldSize.ToolTip"));
      this.lblItemShieldSize.Visible = ((bool)(resources.GetObject("lblItemShieldSize.Visible")));
      // 
      // txtItemShieldSize
      // 
      this.txtItemShieldSize.AccessibleDescription = resources.GetString("txtItemShieldSize.AccessibleDescription");
      this.txtItemShieldSize.AccessibleName = resources.GetString("txtItemShieldSize.AccessibleName");
      this.txtItemShieldSize.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtItemShieldSize.Anchor")));
      this.txtItemShieldSize.AutoSize = ((bool)(resources.GetObject("txtItemShieldSize.AutoSize")));
      this.txtItemShieldSize.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtItemShieldSize.BackgroundImage")));
      this.txtItemShieldSize.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtItemShieldSize.Dock")));
      this.txtItemShieldSize.Enabled = ((bool)(resources.GetObject("txtItemShieldSize.Enabled")));
      this.txtItemShieldSize.Font = ((System.Drawing.Font)(resources.GetObject("txtItemShieldSize.Font")));
      this.txtItemShieldSize.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtItemShieldSize.ImeMode")));
      this.txtItemShieldSize.Location = ((System.Drawing.Point)(resources.GetObject("txtItemShieldSize.Location")));
      this.txtItemShieldSize.MaxLength = ((int)(resources.GetObject("txtItemShieldSize.MaxLength")));
      this.txtItemShieldSize.Multiline = ((bool)(resources.GetObject("txtItemShieldSize.Multiline")));
      this.txtItemShieldSize.Name = "txtItemShieldSize";
      this.txtItemShieldSize.PasswordChar = ((char)(resources.GetObject("txtItemShieldSize.PasswordChar")));
      this.txtItemShieldSize.ReadOnly = true;
      this.txtItemShieldSize.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtItemShieldSize.RightToLeft")));
      this.txtItemShieldSize.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtItemShieldSize.ScrollBars")));
      this.txtItemShieldSize.Size = ((System.Drawing.Size)(resources.GetObject("txtItemShieldSize.Size")));
      this.txtItemShieldSize.TabIndex = ((int)(resources.GetObject("txtItemShieldSize.TabIndex")));
      this.txtItemShieldSize.Text = resources.GetString("txtItemShieldSize.Text");
      this.txtItemShieldSize.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtItemShieldSize.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtItemShieldSize, resources.GetString("txtItemShieldSize.ToolTip"));
      this.txtItemShieldSize.Visible = ((bool)(resources.GetObject("txtItemShieldSize.Visible")));
      this.txtItemShieldSize.WordWrap = ((bool)(resources.GetObject("txtItemShieldSize.WordWrap")));
      // 
      // grpCommonItemInfo
      // 
      this.grpCommonItemInfo.AccessibleDescription = resources.GetString("grpCommonItemInfo.AccessibleDescription");
      this.grpCommonItemInfo.AccessibleName = resources.GetString("grpCommonItemInfo.AccessibleName");
      this.grpCommonItemInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("grpCommonItemInfo.Anchor")));
      this.grpCommonItemInfo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpCommonItemInfo.BackgroundImage")));
      this.grpCommonItemInfo.Controls.Add(this.lblItemDescription);
      this.grpCommonItemInfo.Controls.Add(this.lblItemPlural);
      this.grpCommonItemInfo.Controls.Add(this.lblItemSingular);
      this.grpCommonItemInfo.Controls.Add(this.lblItemJName);
      this.grpCommonItemInfo.Controls.Add(this.lblItemEName);
      this.grpCommonItemInfo.Controls.Add(this.txtItemPlural);
      this.grpCommonItemInfo.Controls.Add(this.txtItemSingular);
      this.grpCommonItemInfo.Controls.Add(this.txtItemJName);
      this.grpCommonItemInfo.Controls.Add(this.txtItemEName);
      this.grpCommonItemInfo.Controls.Add(this.txtItemDescription);
      this.grpCommonItemInfo.Controls.Add(this.lblItemStackSize);
      this.grpCommonItemInfo.Controls.Add(this.lblItemFlags);
      this.grpCommonItemInfo.Controls.Add(this.lblItemType);
      this.grpCommonItemInfo.Controls.Add(this.lblItemID);
      this.grpCommonItemInfo.Controls.Add(this.txtItemStackSize);
      this.grpCommonItemInfo.Controls.Add(this.txtItemFlags);
      this.grpCommonItemInfo.Controls.Add(this.txtItemType);
      this.grpCommonItemInfo.Controls.Add(this.txtItemID);
      this.grpCommonItemInfo.Controls.Add(this.picItemIcon);
      this.grpCommonItemInfo.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("grpCommonItemInfo.Dock")));
      this.grpCommonItemInfo.Enabled = ((bool)(resources.GetObject("grpCommonItemInfo.Enabled")));
      this.grpCommonItemInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.grpCommonItemInfo.Font = ((System.Drawing.Font)(resources.GetObject("grpCommonItemInfo.Font")));
      this.grpCommonItemInfo.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("grpCommonItemInfo.ImeMode")));
      this.grpCommonItemInfo.Location = ((System.Drawing.Point)(resources.GetObject("grpCommonItemInfo.Location")));
      this.grpCommonItemInfo.Name = "grpCommonItemInfo";
      this.grpCommonItemInfo.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("grpCommonItemInfo.RightToLeft")));
      this.grpCommonItemInfo.Size = ((System.Drawing.Size)(resources.GetObject("grpCommonItemInfo.Size")));
      this.grpCommonItemInfo.TabIndex = ((int)(resources.GetObject("grpCommonItemInfo.TabIndex")));
      this.grpCommonItemInfo.TabStop = false;
      this.grpCommonItemInfo.Text = resources.GetString("grpCommonItemInfo.Text");
      this.ttToolTip.SetToolTip(this.grpCommonItemInfo, resources.GetString("grpCommonItemInfo.ToolTip"));
      this.grpCommonItemInfo.Visible = ((bool)(resources.GetObject("grpCommonItemInfo.Visible")));
      // 
      // lblItemDescription
      // 
      this.lblItemDescription.AccessibleDescription = resources.GetString("lblItemDescription.AccessibleDescription");
      this.lblItemDescription.AccessibleName = resources.GetString("lblItemDescription.AccessibleName");
      this.lblItemDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblItemDescription.Anchor")));
      this.lblItemDescription.AutoSize = ((bool)(resources.GetObject("lblItemDescription.AutoSize")));
      this.lblItemDescription.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblItemDescription.Dock")));
      this.lblItemDescription.Enabled = ((bool)(resources.GetObject("lblItemDescription.Enabled")));
      this.lblItemDescription.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemDescription.Font = ((System.Drawing.Font)(resources.GetObject("lblItemDescription.Font")));
      this.lblItemDescription.Image = ((System.Drawing.Image)(resources.GetObject("lblItemDescription.Image")));
      this.lblItemDescription.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemDescription.ImageAlign")));
      this.lblItemDescription.ImageIndex = ((int)(resources.GetObject("lblItemDescription.ImageIndex")));
      this.lblItemDescription.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblItemDescription.ImeMode")));
      this.lblItemDescription.Location = ((System.Drawing.Point)(resources.GetObject("lblItemDescription.Location")));
      this.lblItemDescription.Name = "lblItemDescription";
      this.lblItemDescription.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblItemDescription.RightToLeft")));
      this.lblItemDescription.Size = ((System.Drawing.Size)(resources.GetObject("lblItemDescription.Size")));
      this.lblItemDescription.TabIndex = ((int)(resources.GetObject("lblItemDescription.TabIndex")));
      this.lblItemDescription.Text = resources.GetString("lblItemDescription.Text");
      this.lblItemDescription.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemDescription.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblItemDescription, resources.GetString("lblItemDescription.ToolTip"));
      this.lblItemDescription.Visible = ((bool)(resources.GetObject("lblItemDescription.Visible")));
      // 
      // lblItemPlural
      // 
      this.lblItemPlural.AccessibleDescription = resources.GetString("lblItemPlural.AccessibleDescription");
      this.lblItemPlural.AccessibleName = resources.GetString("lblItemPlural.AccessibleName");
      this.lblItemPlural.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblItemPlural.Anchor")));
      this.lblItemPlural.AutoSize = ((bool)(resources.GetObject("lblItemPlural.AutoSize")));
      this.lblItemPlural.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblItemPlural.Dock")));
      this.lblItemPlural.Enabled = ((bool)(resources.GetObject("lblItemPlural.Enabled")));
      this.lblItemPlural.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemPlural.Font = ((System.Drawing.Font)(resources.GetObject("lblItemPlural.Font")));
      this.lblItemPlural.Image = ((System.Drawing.Image)(resources.GetObject("lblItemPlural.Image")));
      this.lblItemPlural.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemPlural.ImageAlign")));
      this.lblItemPlural.ImageIndex = ((int)(resources.GetObject("lblItemPlural.ImageIndex")));
      this.lblItemPlural.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblItemPlural.ImeMode")));
      this.lblItemPlural.Location = ((System.Drawing.Point)(resources.GetObject("lblItemPlural.Location")));
      this.lblItemPlural.Name = "lblItemPlural";
      this.lblItemPlural.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblItemPlural.RightToLeft")));
      this.lblItemPlural.Size = ((System.Drawing.Size)(resources.GetObject("lblItemPlural.Size")));
      this.lblItemPlural.TabIndex = ((int)(resources.GetObject("lblItemPlural.TabIndex")));
      this.lblItemPlural.Text = resources.GetString("lblItemPlural.Text");
      this.lblItemPlural.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemPlural.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblItemPlural, resources.GetString("lblItemPlural.ToolTip"));
      this.lblItemPlural.Visible = ((bool)(resources.GetObject("lblItemPlural.Visible")));
      // 
      // lblItemSingular
      // 
      this.lblItemSingular.AccessibleDescription = resources.GetString("lblItemSingular.AccessibleDescription");
      this.lblItemSingular.AccessibleName = resources.GetString("lblItemSingular.AccessibleName");
      this.lblItemSingular.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblItemSingular.Anchor")));
      this.lblItemSingular.AutoSize = ((bool)(resources.GetObject("lblItemSingular.AutoSize")));
      this.lblItemSingular.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblItemSingular.Dock")));
      this.lblItemSingular.Enabled = ((bool)(resources.GetObject("lblItemSingular.Enabled")));
      this.lblItemSingular.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemSingular.Font = ((System.Drawing.Font)(resources.GetObject("lblItemSingular.Font")));
      this.lblItemSingular.Image = ((System.Drawing.Image)(resources.GetObject("lblItemSingular.Image")));
      this.lblItemSingular.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemSingular.ImageAlign")));
      this.lblItemSingular.ImageIndex = ((int)(resources.GetObject("lblItemSingular.ImageIndex")));
      this.lblItemSingular.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblItemSingular.ImeMode")));
      this.lblItemSingular.Location = ((System.Drawing.Point)(resources.GetObject("lblItemSingular.Location")));
      this.lblItemSingular.Name = "lblItemSingular";
      this.lblItemSingular.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblItemSingular.RightToLeft")));
      this.lblItemSingular.Size = ((System.Drawing.Size)(resources.GetObject("lblItemSingular.Size")));
      this.lblItemSingular.TabIndex = ((int)(resources.GetObject("lblItemSingular.TabIndex")));
      this.lblItemSingular.Text = resources.GetString("lblItemSingular.Text");
      this.lblItemSingular.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemSingular.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblItemSingular, resources.GetString("lblItemSingular.ToolTip"));
      this.lblItemSingular.Visible = ((bool)(resources.GetObject("lblItemSingular.Visible")));
      // 
      // lblItemJName
      // 
      this.lblItemJName.AccessibleDescription = resources.GetString("lblItemJName.AccessibleDescription");
      this.lblItemJName.AccessibleName = resources.GetString("lblItemJName.AccessibleName");
      this.lblItemJName.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblItemJName.Anchor")));
      this.lblItemJName.AutoSize = ((bool)(resources.GetObject("lblItemJName.AutoSize")));
      this.lblItemJName.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblItemJName.Dock")));
      this.lblItemJName.Enabled = ((bool)(resources.GetObject("lblItemJName.Enabled")));
      this.lblItemJName.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemJName.Font = ((System.Drawing.Font)(resources.GetObject("lblItemJName.Font")));
      this.lblItemJName.Image = ((System.Drawing.Image)(resources.GetObject("lblItemJName.Image")));
      this.lblItemJName.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemJName.ImageAlign")));
      this.lblItemJName.ImageIndex = ((int)(resources.GetObject("lblItemJName.ImageIndex")));
      this.lblItemJName.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblItemJName.ImeMode")));
      this.lblItemJName.Location = ((System.Drawing.Point)(resources.GetObject("lblItemJName.Location")));
      this.lblItemJName.Name = "lblItemJName";
      this.lblItemJName.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblItemJName.RightToLeft")));
      this.lblItemJName.Size = ((System.Drawing.Size)(resources.GetObject("lblItemJName.Size")));
      this.lblItemJName.TabIndex = ((int)(resources.GetObject("lblItemJName.TabIndex")));
      this.lblItemJName.Text = resources.GetString("lblItemJName.Text");
      this.lblItemJName.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemJName.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblItemJName, resources.GetString("lblItemJName.ToolTip"));
      this.lblItemJName.Visible = ((bool)(resources.GetObject("lblItemJName.Visible")));
      // 
      // lblItemEName
      // 
      this.lblItemEName.AccessibleDescription = resources.GetString("lblItemEName.AccessibleDescription");
      this.lblItemEName.AccessibleName = resources.GetString("lblItemEName.AccessibleName");
      this.lblItemEName.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblItemEName.Anchor")));
      this.lblItemEName.AutoSize = ((bool)(resources.GetObject("lblItemEName.AutoSize")));
      this.lblItemEName.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblItemEName.Dock")));
      this.lblItemEName.Enabled = ((bool)(resources.GetObject("lblItemEName.Enabled")));
      this.lblItemEName.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemEName.Font = ((System.Drawing.Font)(resources.GetObject("lblItemEName.Font")));
      this.lblItemEName.Image = ((System.Drawing.Image)(resources.GetObject("lblItemEName.Image")));
      this.lblItemEName.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemEName.ImageAlign")));
      this.lblItemEName.ImageIndex = ((int)(resources.GetObject("lblItemEName.ImageIndex")));
      this.lblItemEName.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblItemEName.ImeMode")));
      this.lblItemEName.Location = ((System.Drawing.Point)(resources.GetObject("lblItemEName.Location")));
      this.lblItemEName.Name = "lblItemEName";
      this.lblItemEName.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblItemEName.RightToLeft")));
      this.lblItemEName.Size = ((System.Drawing.Size)(resources.GetObject("lblItemEName.Size")));
      this.lblItemEName.TabIndex = ((int)(resources.GetObject("lblItemEName.TabIndex")));
      this.lblItemEName.Text = resources.GetString("lblItemEName.Text");
      this.lblItemEName.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemEName.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblItemEName, resources.GetString("lblItemEName.ToolTip"));
      this.lblItemEName.Visible = ((bool)(resources.GetObject("lblItemEName.Visible")));
      // 
      // txtItemPlural
      // 
      this.txtItemPlural.AccessibleDescription = resources.GetString("txtItemPlural.AccessibleDescription");
      this.txtItemPlural.AccessibleName = resources.GetString("txtItemPlural.AccessibleName");
      this.txtItemPlural.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtItemPlural.Anchor")));
      this.txtItemPlural.AutoSize = ((bool)(resources.GetObject("txtItemPlural.AutoSize")));
      this.txtItemPlural.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtItemPlural.BackgroundImage")));
      this.txtItemPlural.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtItemPlural.Dock")));
      this.txtItemPlural.Enabled = ((bool)(resources.GetObject("txtItemPlural.Enabled")));
      this.txtItemPlural.Font = ((System.Drawing.Font)(resources.GetObject("txtItemPlural.Font")));
      this.txtItemPlural.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtItemPlural.ImeMode")));
      this.txtItemPlural.Location = ((System.Drawing.Point)(resources.GetObject("txtItemPlural.Location")));
      this.txtItemPlural.MaxLength = ((int)(resources.GetObject("txtItemPlural.MaxLength")));
      this.txtItemPlural.Multiline = ((bool)(resources.GetObject("txtItemPlural.Multiline")));
      this.txtItemPlural.Name = "txtItemPlural";
      this.txtItemPlural.PasswordChar = ((char)(resources.GetObject("txtItemPlural.PasswordChar")));
      this.txtItemPlural.ReadOnly = true;
      this.txtItemPlural.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtItemPlural.RightToLeft")));
      this.txtItemPlural.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtItemPlural.ScrollBars")));
      this.txtItemPlural.Size = ((System.Drawing.Size)(resources.GetObject("txtItemPlural.Size")));
      this.txtItemPlural.TabIndex = ((int)(resources.GetObject("txtItemPlural.TabIndex")));
      this.txtItemPlural.Text = resources.GetString("txtItemPlural.Text");
      this.txtItemPlural.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtItemPlural.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtItemPlural, resources.GetString("txtItemPlural.ToolTip"));
      this.txtItemPlural.Visible = ((bool)(resources.GetObject("txtItemPlural.Visible")));
      this.txtItemPlural.WordWrap = ((bool)(resources.GetObject("txtItemPlural.WordWrap")));
      // 
      // txtItemSingular
      // 
      this.txtItemSingular.AccessibleDescription = resources.GetString("txtItemSingular.AccessibleDescription");
      this.txtItemSingular.AccessibleName = resources.GetString("txtItemSingular.AccessibleName");
      this.txtItemSingular.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtItemSingular.Anchor")));
      this.txtItemSingular.AutoSize = ((bool)(resources.GetObject("txtItemSingular.AutoSize")));
      this.txtItemSingular.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtItemSingular.BackgroundImage")));
      this.txtItemSingular.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtItemSingular.Dock")));
      this.txtItemSingular.Enabled = ((bool)(resources.GetObject("txtItemSingular.Enabled")));
      this.txtItemSingular.Font = ((System.Drawing.Font)(resources.GetObject("txtItemSingular.Font")));
      this.txtItemSingular.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtItemSingular.ImeMode")));
      this.txtItemSingular.Location = ((System.Drawing.Point)(resources.GetObject("txtItemSingular.Location")));
      this.txtItemSingular.MaxLength = ((int)(resources.GetObject("txtItemSingular.MaxLength")));
      this.txtItemSingular.Multiline = ((bool)(resources.GetObject("txtItemSingular.Multiline")));
      this.txtItemSingular.Name = "txtItemSingular";
      this.txtItemSingular.PasswordChar = ((char)(resources.GetObject("txtItemSingular.PasswordChar")));
      this.txtItemSingular.ReadOnly = true;
      this.txtItemSingular.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtItemSingular.RightToLeft")));
      this.txtItemSingular.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtItemSingular.ScrollBars")));
      this.txtItemSingular.Size = ((System.Drawing.Size)(resources.GetObject("txtItemSingular.Size")));
      this.txtItemSingular.TabIndex = ((int)(resources.GetObject("txtItemSingular.TabIndex")));
      this.txtItemSingular.Text = resources.GetString("txtItemSingular.Text");
      this.txtItemSingular.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtItemSingular.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtItemSingular, resources.GetString("txtItemSingular.ToolTip"));
      this.txtItemSingular.Visible = ((bool)(resources.GetObject("txtItemSingular.Visible")));
      this.txtItemSingular.WordWrap = ((bool)(resources.GetObject("txtItemSingular.WordWrap")));
      // 
      // txtItemJName
      // 
      this.txtItemJName.AccessibleDescription = resources.GetString("txtItemJName.AccessibleDescription");
      this.txtItemJName.AccessibleName = resources.GetString("txtItemJName.AccessibleName");
      this.txtItemJName.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtItemJName.Anchor")));
      this.txtItemJName.AutoSize = ((bool)(resources.GetObject("txtItemJName.AutoSize")));
      this.txtItemJName.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtItemJName.BackgroundImage")));
      this.txtItemJName.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtItemJName.Dock")));
      this.txtItemJName.Enabled = ((bool)(resources.GetObject("txtItemJName.Enabled")));
      this.txtItemJName.Font = ((System.Drawing.Font)(resources.GetObject("txtItemJName.Font")));
      this.txtItemJName.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtItemJName.ImeMode")));
      this.txtItemJName.Location = ((System.Drawing.Point)(resources.GetObject("txtItemJName.Location")));
      this.txtItemJName.MaxLength = ((int)(resources.GetObject("txtItemJName.MaxLength")));
      this.txtItemJName.Multiline = ((bool)(resources.GetObject("txtItemJName.Multiline")));
      this.txtItemJName.Name = "txtItemJName";
      this.txtItemJName.PasswordChar = ((char)(resources.GetObject("txtItemJName.PasswordChar")));
      this.txtItemJName.ReadOnly = true;
      this.txtItemJName.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtItemJName.RightToLeft")));
      this.txtItemJName.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtItemJName.ScrollBars")));
      this.txtItemJName.Size = ((System.Drawing.Size)(resources.GetObject("txtItemJName.Size")));
      this.txtItemJName.TabIndex = ((int)(resources.GetObject("txtItemJName.TabIndex")));
      this.txtItemJName.Text = resources.GetString("txtItemJName.Text");
      this.txtItemJName.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtItemJName.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtItemJName, resources.GetString("txtItemJName.ToolTip"));
      this.txtItemJName.Visible = ((bool)(resources.GetObject("txtItemJName.Visible")));
      this.txtItemJName.WordWrap = ((bool)(resources.GetObject("txtItemJName.WordWrap")));
      // 
      // txtItemEName
      // 
      this.txtItemEName.AccessibleDescription = resources.GetString("txtItemEName.AccessibleDescription");
      this.txtItemEName.AccessibleName = resources.GetString("txtItemEName.AccessibleName");
      this.txtItemEName.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtItemEName.Anchor")));
      this.txtItemEName.AutoSize = ((bool)(resources.GetObject("txtItemEName.AutoSize")));
      this.txtItemEName.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtItemEName.BackgroundImage")));
      this.txtItemEName.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtItemEName.Dock")));
      this.txtItemEName.Enabled = ((bool)(resources.GetObject("txtItemEName.Enabled")));
      this.txtItemEName.Font = ((System.Drawing.Font)(resources.GetObject("txtItemEName.Font")));
      this.txtItemEName.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtItemEName.ImeMode")));
      this.txtItemEName.Location = ((System.Drawing.Point)(resources.GetObject("txtItemEName.Location")));
      this.txtItemEName.MaxLength = ((int)(resources.GetObject("txtItemEName.MaxLength")));
      this.txtItemEName.Multiline = ((bool)(resources.GetObject("txtItemEName.Multiline")));
      this.txtItemEName.Name = "txtItemEName";
      this.txtItemEName.PasswordChar = ((char)(resources.GetObject("txtItemEName.PasswordChar")));
      this.txtItemEName.ReadOnly = true;
      this.txtItemEName.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtItemEName.RightToLeft")));
      this.txtItemEName.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtItemEName.ScrollBars")));
      this.txtItemEName.Size = ((System.Drawing.Size)(resources.GetObject("txtItemEName.Size")));
      this.txtItemEName.TabIndex = ((int)(resources.GetObject("txtItemEName.TabIndex")));
      this.txtItemEName.Text = resources.GetString("txtItemEName.Text");
      this.txtItemEName.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtItemEName.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtItemEName, resources.GetString("txtItemEName.ToolTip"));
      this.txtItemEName.Visible = ((bool)(resources.GetObject("txtItemEName.Visible")));
      this.txtItemEName.WordWrap = ((bool)(resources.GetObject("txtItemEName.WordWrap")));
      // 
      // txtItemDescription
      // 
      this.txtItemDescription.AccessibleDescription = resources.GetString("txtItemDescription.AccessibleDescription");
      this.txtItemDescription.AccessibleName = resources.GetString("txtItemDescription.AccessibleName");
      this.txtItemDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtItemDescription.Anchor")));
      this.txtItemDescription.AutoSize = ((bool)(resources.GetObject("txtItemDescription.AutoSize")));
      this.txtItemDescription.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtItemDescription.BackgroundImage")));
      this.txtItemDescription.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtItemDescription.Dock")));
      this.txtItemDescription.Enabled = ((bool)(resources.GetObject("txtItemDescription.Enabled")));
      this.txtItemDescription.Font = ((System.Drawing.Font)(resources.GetObject("txtItemDescription.Font")));
      this.txtItemDescription.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtItemDescription.ImeMode")));
      this.txtItemDescription.Location = ((System.Drawing.Point)(resources.GetObject("txtItemDescription.Location")));
      this.txtItemDescription.MaxLength = ((int)(resources.GetObject("txtItemDescription.MaxLength")));
      this.txtItemDescription.Multiline = ((bool)(resources.GetObject("txtItemDescription.Multiline")));
      this.txtItemDescription.Name = "txtItemDescription";
      this.txtItemDescription.PasswordChar = ((char)(resources.GetObject("txtItemDescription.PasswordChar")));
      this.txtItemDescription.ReadOnly = true;
      this.txtItemDescription.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtItemDescription.RightToLeft")));
      this.txtItemDescription.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtItemDescription.ScrollBars")));
      this.txtItemDescription.Size = ((System.Drawing.Size)(resources.GetObject("txtItemDescription.Size")));
      this.txtItemDescription.TabIndex = ((int)(resources.GetObject("txtItemDescription.TabIndex")));
      this.txtItemDescription.Text = resources.GetString("txtItemDescription.Text");
      this.txtItemDescription.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtItemDescription.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtItemDescription, resources.GetString("txtItemDescription.ToolTip"));
      this.txtItemDescription.Visible = ((bool)(resources.GetObject("txtItemDescription.Visible")));
      this.txtItemDescription.WordWrap = ((bool)(resources.GetObject("txtItemDescription.WordWrap")));
      // 
      // lblItemStackSize
      // 
      this.lblItemStackSize.AccessibleDescription = resources.GetString("lblItemStackSize.AccessibleDescription");
      this.lblItemStackSize.AccessibleName = resources.GetString("lblItemStackSize.AccessibleName");
      this.lblItemStackSize.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblItemStackSize.Anchor")));
      this.lblItemStackSize.AutoSize = ((bool)(resources.GetObject("lblItemStackSize.AutoSize")));
      this.lblItemStackSize.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblItemStackSize.Dock")));
      this.lblItemStackSize.Enabled = ((bool)(resources.GetObject("lblItemStackSize.Enabled")));
      this.lblItemStackSize.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemStackSize.Font = ((System.Drawing.Font)(resources.GetObject("lblItemStackSize.Font")));
      this.lblItemStackSize.Image = ((System.Drawing.Image)(resources.GetObject("lblItemStackSize.Image")));
      this.lblItemStackSize.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemStackSize.ImageAlign")));
      this.lblItemStackSize.ImageIndex = ((int)(resources.GetObject("lblItemStackSize.ImageIndex")));
      this.lblItemStackSize.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblItemStackSize.ImeMode")));
      this.lblItemStackSize.Location = ((System.Drawing.Point)(resources.GetObject("lblItemStackSize.Location")));
      this.lblItemStackSize.Name = "lblItemStackSize";
      this.lblItemStackSize.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblItemStackSize.RightToLeft")));
      this.lblItemStackSize.Size = ((System.Drawing.Size)(resources.GetObject("lblItemStackSize.Size")));
      this.lblItemStackSize.TabIndex = ((int)(resources.GetObject("lblItemStackSize.TabIndex")));
      this.lblItemStackSize.Text = resources.GetString("lblItemStackSize.Text");
      this.lblItemStackSize.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemStackSize.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblItemStackSize, resources.GetString("lblItemStackSize.ToolTip"));
      this.lblItemStackSize.Visible = ((bool)(resources.GetObject("lblItemStackSize.Visible")));
      // 
      // lblItemFlags
      // 
      this.lblItemFlags.AccessibleDescription = resources.GetString("lblItemFlags.AccessibleDescription");
      this.lblItemFlags.AccessibleName = resources.GetString("lblItemFlags.AccessibleName");
      this.lblItemFlags.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblItemFlags.Anchor")));
      this.lblItemFlags.AutoSize = ((bool)(resources.GetObject("lblItemFlags.AutoSize")));
      this.lblItemFlags.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblItemFlags.Dock")));
      this.lblItemFlags.Enabled = ((bool)(resources.GetObject("lblItemFlags.Enabled")));
      this.lblItemFlags.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemFlags.Font = ((System.Drawing.Font)(resources.GetObject("lblItemFlags.Font")));
      this.lblItemFlags.Image = ((System.Drawing.Image)(resources.GetObject("lblItemFlags.Image")));
      this.lblItemFlags.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemFlags.ImageAlign")));
      this.lblItemFlags.ImageIndex = ((int)(resources.GetObject("lblItemFlags.ImageIndex")));
      this.lblItemFlags.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblItemFlags.ImeMode")));
      this.lblItemFlags.Location = ((System.Drawing.Point)(resources.GetObject("lblItemFlags.Location")));
      this.lblItemFlags.Name = "lblItemFlags";
      this.lblItemFlags.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblItemFlags.RightToLeft")));
      this.lblItemFlags.Size = ((System.Drawing.Size)(resources.GetObject("lblItemFlags.Size")));
      this.lblItemFlags.TabIndex = ((int)(resources.GetObject("lblItemFlags.TabIndex")));
      this.lblItemFlags.Text = resources.GetString("lblItemFlags.Text");
      this.lblItemFlags.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemFlags.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblItemFlags, resources.GetString("lblItemFlags.ToolTip"));
      this.lblItemFlags.Visible = ((bool)(resources.GetObject("lblItemFlags.Visible")));
      // 
      // lblItemType
      // 
      this.lblItemType.AccessibleDescription = resources.GetString("lblItemType.AccessibleDescription");
      this.lblItemType.AccessibleName = resources.GetString("lblItemType.AccessibleName");
      this.lblItemType.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblItemType.Anchor")));
      this.lblItemType.AutoSize = ((bool)(resources.GetObject("lblItemType.AutoSize")));
      this.lblItemType.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblItemType.Dock")));
      this.lblItemType.Enabled = ((bool)(resources.GetObject("lblItemType.Enabled")));
      this.lblItemType.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemType.Font = ((System.Drawing.Font)(resources.GetObject("lblItemType.Font")));
      this.lblItemType.Image = ((System.Drawing.Image)(resources.GetObject("lblItemType.Image")));
      this.lblItemType.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemType.ImageAlign")));
      this.lblItemType.ImageIndex = ((int)(resources.GetObject("lblItemType.ImageIndex")));
      this.lblItemType.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblItemType.ImeMode")));
      this.lblItemType.Location = ((System.Drawing.Point)(resources.GetObject("lblItemType.Location")));
      this.lblItemType.Name = "lblItemType";
      this.lblItemType.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblItemType.RightToLeft")));
      this.lblItemType.Size = ((System.Drawing.Size)(resources.GetObject("lblItemType.Size")));
      this.lblItemType.TabIndex = ((int)(resources.GetObject("lblItemType.TabIndex")));
      this.lblItemType.Text = resources.GetString("lblItemType.Text");
      this.lblItemType.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemType.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblItemType, resources.GetString("lblItemType.ToolTip"));
      this.lblItemType.Visible = ((bool)(resources.GetObject("lblItemType.Visible")));
      // 
      // lblItemID
      // 
      this.lblItemID.AccessibleDescription = resources.GetString("lblItemID.AccessibleDescription");
      this.lblItemID.AccessibleName = resources.GetString("lblItemID.AccessibleName");
      this.lblItemID.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblItemID.Anchor")));
      this.lblItemID.AutoSize = ((bool)(resources.GetObject("lblItemID.AutoSize")));
      this.lblItemID.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblItemID.Dock")));
      this.lblItemID.Enabled = ((bool)(resources.GetObject("lblItemID.Enabled")));
      this.lblItemID.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemID.Font = ((System.Drawing.Font)(resources.GetObject("lblItemID.Font")));
      this.lblItemID.Image = ((System.Drawing.Image)(resources.GetObject("lblItemID.Image")));
      this.lblItemID.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemID.ImageAlign")));
      this.lblItemID.ImageIndex = ((int)(resources.GetObject("lblItemID.ImageIndex")));
      this.lblItemID.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblItemID.ImeMode")));
      this.lblItemID.Location = ((System.Drawing.Point)(resources.GetObject("lblItemID.Location")));
      this.lblItemID.Name = "lblItemID";
      this.lblItemID.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblItemID.RightToLeft")));
      this.lblItemID.Size = ((System.Drawing.Size)(resources.GetObject("lblItemID.Size")));
      this.lblItemID.TabIndex = ((int)(resources.GetObject("lblItemID.TabIndex")));
      this.lblItemID.Text = resources.GetString("lblItemID.Text");
      this.lblItemID.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblItemID.TextAlign")));
      this.ttToolTip.SetToolTip(this.lblItemID, resources.GetString("lblItemID.ToolTip"));
      this.lblItemID.Visible = ((bool)(resources.GetObject("lblItemID.Visible")));
      // 
      // txtItemStackSize
      // 
      this.txtItemStackSize.AccessibleDescription = resources.GetString("txtItemStackSize.AccessibleDescription");
      this.txtItemStackSize.AccessibleName = resources.GetString("txtItemStackSize.AccessibleName");
      this.txtItemStackSize.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtItemStackSize.Anchor")));
      this.txtItemStackSize.AutoSize = ((bool)(resources.GetObject("txtItemStackSize.AutoSize")));
      this.txtItemStackSize.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtItemStackSize.BackgroundImage")));
      this.txtItemStackSize.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtItemStackSize.Dock")));
      this.txtItemStackSize.Enabled = ((bool)(resources.GetObject("txtItemStackSize.Enabled")));
      this.txtItemStackSize.Font = ((System.Drawing.Font)(resources.GetObject("txtItemStackSize.Font")));
      this.txtItemStackSize.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtItemStackSize.ImeMode")));
      this.txtItemStackSize.Location = ((System.Drawing.Point)(resources.GetObject("txtItemStackSize.Location")));
      this.txtItemStackSize.MaxLength = ((int)(resources.GetObject("txtItemStackSize.MaxLength")));
      this.txtItemStackSize.Multiline = ((bool)(resources.GetObject("txtItemStackSize.Multiline")));
      this.txtItemStackSize.Name = "txtItemStackSize";
      this.txtItemStackSize.PasswordChar = ((char)(resources.GetObject("txtItemStackSize.PasswordChar")));
      this.txtItemStackSize.ReadOnly = true;
      this.txtItemStackSize.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtItemStackSize.RightToLeft")));
      this.txtItemStackSize.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtItemStackSize.ScrollBars")));
      this.txtItemStackSize.Size = ((System.Drawing.Size)(resources.GetObject("txtItemStackSize.Size")));
      this.txtItemStackSize.TabIndex = ((int)(resources.GetObject("txtItemStackSize.TabIndex")));
      this.txtItemStackSize.Text = resources.GetString("txtItemStackSize.Text");
      this.txtItemStackSize.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtItemStackSize.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtItemStackSize, resources.GetString("txtItemStackSize.ToolTip"));
      this.txtItemStackSize.Visible = ((bool)(resources.GetObject("txtItemStackSize.Visible")));
      this.txtItemStackSize.WordWrap = ((bool)(resources.GetObject("txtItemStackSize.WordWrap")));
      // 
      // txtItemFlags
      // 
      this.txtItemFlags.AccessibleDescription = resources.GetString("txtItemFlags.AccessibleDescription");
      this.txtItemFlags.AccessibleName = resources.GetString("txtItemFlags.AccessibleName");
      this.txtItemFlags.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtItemFlags.Anchor")));
      this.txtItemFlags.AutoSize = ((bool)(resources.GetObject("txtItemFlags.AutoSize")));
      this.txtItemFlags.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtItemFlags.BackgroundImage")));
      this.txtItemFlags.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtItemFlags.Dock")));
      this.txtItemFlags.Enabled = ((bool)(resources.GetObject("txtItemFlags.Enabled")));
      this.txtItemFlags.Font = ((System.Drawing.Font)(resources.GetObject("txtItemFlags.Font")));
      this.txtItemFlags.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtItemFlags.ImeMode")));
      this.txtItemFlags.Location = ((System.Drawing.Point)(resources.GetObject("txtItemFlags.Location")));
      this.txtItemFlags.MaxLength = ((int)(resources.GetObject("txtItemFlags.MaxLength")));
      this.txtItemFlags.Multiline = ((bool)(resources.GetObject("txtItemFlags.Multiline")));
      this.txtItemFlags.Name = "txtItemFlags";
      this.txtItemFlags.PasswordChar = ((char)(resources.GetObject("txtItemFlags.PasswordChar")));
      this.txtItemFlags.ReadOnly = true;
      this.txtItemFlags.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtItemFlags.RightToLeft")));
      this.txtItemFlags.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtItemFlags.ScrollBars")));
      this.txtItemFlags.Size = ((System.Drawing.Size)(resources.GetObject("txtItemFlags.Size")));
      this.txtItemFlags.TabIndex = ((int)(resources.GetObject("txtItemFlags.TabIndex")));
      this.txtItemFlags.Text = resources.GetString("txtItemFlags.Text");
      this.txtItemFlags.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtItemFlags.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtItemFlags, resources.GetString("txtItemFlags.ToolTip"));
      this.txtItemFlags.Visible = ((bool)(resources.GetObject("txtItemFlags.Visible")));
      this.txtItemFlags.WordWrap = ((bool)(resources.GetObject("txtItemFlags.WordWrap")));
      // 
      // txtItemType
      // 
      this.txtItemType.AccessibleDescription = resources.GetString("txtItemType.AccessibleDescription");
      this.txtItemType.AccessibleName = resources.GetString("txtItemType.AccessibleName");
      this.txtItemType.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtItemType.Anchor")));
      this.txtItemType.AutoSize = ((bool)(resources.GetObject("txtItemType.AutoSize")));
      this.txtItemType.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtItemType.BackgroundImage")));
      this.txtItemType.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtItemType.Dock")));
      this.txtItemType.Enabled = ((bool)(resources.GetObject("txtItemType.Enabled")));
      this.txtItemType.Font = ((System.Drawing.Font)(resources.GetObject("txtItemType.Font")));
      this.txtItemType.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtItemType.ImeMode")));
      this.txtItemType.Location = ((System.Drawing.Point)(resources.GetObject("txtItemType.Location")));
      this.txtItemType.MaxLength = ((int)(resources.GetObject("txtItemType.MaxLength")));
      this.txtItemType.Multiline = ((bool)(resources.GetObject("txtItemType.Multiline")));
      this.txtItemType.Name = "txtItemType";
      this.txtItemType.PasswordChar = ((char)(resources.GetObject("txtItemType.PasswordChar")));
      this.txtItemType.ReadOnly = true;
      this.txtItemType.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtItemType.RightToLeft")));
      this.txtItemType.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtItemType.ScrollBars")));
      this.txtItemType.Size = ((System.Drawing.Size)(resources.GetObject("txtItemType.Size")));
      this.txtItemType.TabIndex = ((int)(resources.GetObject("txtItemType.TabIndex")));
      this.txtItemType.Text = resources.GetString("txtItemType.Text");
      this.txtItemType.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtItemType.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtItemType, resources.GetString("txtItemType.ToolTip"));
      this.txtItemType.Visible = ((bool)(resources.GetObject("txtItemType.Visible")));
      this.txtItemType.WordWrap = ((bool)(resources.GetObject("txtItemType.WordWrap")));
      // 
      // txtItemID
      // 
      this.txtItemID.AccessibleDescription = resources.GetString("txtItemID.AccessibleDescription");
      this.txtItemID.AccessibleName = resources.GetString("txtItemID.AccessibleName");
      this.txtItemID.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtItemID.Anchor")));
      this.txtItemID.AutoSize = ((bool)(resources.GetObject("txtItemID.AutoSize")));
      this.txtItemID.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtItemID.BackgroundImage")));
      this.txtItemID.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtItemID.Dock")));
      this.txtItemID.Enabled = ((bool)(resources.GetObject("txtItemID.Enabled")));
      this.txtItemID.Font = ((System.Drawing.Font)(resources.GetObject("txtItemID.Font")));
      this.txtItemID.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtItemID.ImeMode")));
      this.txtItemID.Location = ((System.Drawing.Point)(resources.GetObject("txtItemID.Location")));
      this.txtItemID.MaxLength = ((int)(resources.GetObject("txtItemID.MaxLength")));
      this.txtItemID.Multiline = ((bool)(resources.GetObject("txtItemID.Multiline")));
      this.txtItemID.Name = "txtItemID";
      this.txtItemID.PasswordChar = ((char)(resources.GetObject("txtItemID.PasswordChar")));
      this.txtItemID.ReadOnly = true;
      this.txtItemID.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtItemID.RightToLeft")));
      this.txtItemID.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtItemID.ScrollBars")));
      this.txtItemID.Size = ((System.Drawing.Size)(resources.GetObject("txtItemID.Size")));
      this.txtItemID.TabIndex = ((int)(resources.GetObject("txtItemID.TabIndex")));
      this.txtItemID.Text = resources.GetString("txtItemID.Text");
      this.txtItemID.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtItemID.TextAlign")));
      this.ttToolTip.SetToolTip(this.txtItemID, resources.GetString("txtItemID.ToolTip"));
      this.txtItemID.Visible = ((bool)(resources.GetObject("txtItemID.Visible")));
      this.txtItemID.WordWrap = ((bool)(resources.GetObject("txtItemID.WordWrap")));
      // 
      // picItemIcon
      // 
      this.picItemIcon.AccessibleDescription = resources.GetString("picItemIcon.AccessibleDescription");
      this.picItemIcon.AccessibleName = resources.GetString("picItemIcon.AccessibleName");
      this.picItemIcon.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("picItemIcon.Anchor")));
      this.picItemIcon.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picItemIcon.BackgroundImage")));
      this.picItemIcon.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.picItemIcon.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("picItemIcon.Dock")));
      this.picItemIcon.Enabled = ((bool)(resources.GetObject("picItemIcon.Enabled")));
      this.picItemIcon.Font = ((System.Drawing.Font)(resources.GetObject("picItemIcon.Font")));
      this.picItemIcon.Image = ((System.Drawing.Image)(resources.GetObject("picItemIcon.Image")));
      this.picItemIcon.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("picItemIcon.ImeMode")));
      this.picItemIcon.Location = ((System.Drawing.Point)(resources.GetObject("picItemIcon.Location")));
      this.picItemIcon.Name = "picItemIcon";
      this.picItemIcon.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("picItemIcon.RightToLeft")));
      this.picItemIcon.Size = ((System.Drawing.Size)(resources.GetObject("picItemIcon.Size")));
      this.picItemIcon.SizeMode = ((System.Windows.Forms.PictureBoxSizeMode)(resources.GetObject("picItemIcon.SizeMode")));
      this.picItemIcon.TabIndex = ((int)(resources.GetObject("picItemIcon.TabIndex")));
      this.picItemIcon.TabStop = false;
      this.picItemIcon.Text = resources.GetString("picItemIcon.Text");
      this.ttToolTip.SetToolTip(this.picItemIcon, resources.GetString("picItemIcon.ToolTip"));
      this.picItemIcon.Visible = ((bool)(resources.GetObject("picItemIcon.Visible")));
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
      this.ttToolTip.SetToolTip(this.grpMainItemActions, resources.GetString("grpMainItemActions.ToolTip"));
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
      this.ttToolTip.SetToolTip(this.cmbItems, resources.GetString("cmbItems.ToolTip"));
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
      this.ttToolTip.SetToolTip(this.btnFindItems, resources.GetString("btnFindItems.ToolTip"));
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
      this.ttToolTip.SetToolTip(this.btnExportItems, resources.GetString("btnExportItems.ToolTip"));
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
      this.ttToolTip.SetToolTip(this.tabViewerImages, resources.GetString("tabViewerImages.ToolTip"));
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
      this.ttToolTip.SetToolTip(this.picImageViewer, resources.GetString("picImageViewer.ToolTip"));
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
      this.ttToolTip.SetToolTip(this.pnlImageChooser, resources.GetString("pnlImageChooser.ToolTip"));
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
      this.ttToolTip.SetToolTip(this.lblImageChooser, resources.GetString("lblImageChooser.ToolTip"));
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
      this.ttToolTip.SetToolTip(this.cmbImageChooser, resources.GetString("cmbImageChooser.ToolTip"));
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
      this.ttToolTip.SetToolTip(this.tabViewerStringTable, resources.GetString("tabViewerStringTable.ToolTip"));
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
      this.ttToolTip.SetToolTip(this.lstEntries, resources.GetString("lstEntries.ToolTip"));
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
      this.ttToolTip.SetToolTip(this.pnlNoViewers, resources.GetString("pnlNoViewers.ToolTip"));
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
      this.ttToolTip.SetToolTip(this.lblNoViewers, resources.GetString("lblNoViewers.ToolTip"));
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
      this.ttToolTip.SetToolTip(this, resources.GetString("$this.ToolTip"));
      this.pnlViewerArea.ResumeLayout(false);
      this.tabViewers.ResumeLayout(false);
      this.tabViewerItems.ResumeLayout(false);
      this.grpItemViewMode.ResumeLayout(false);
      this.grpSpecializedItemInfo.ResumeLayout(false);
      this.grpCommonItemInfo.ResumeLayout(false);
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
	    foreach (string S in FSD.StringTableEntries) {
	      this.lstEntries.Items.Add(String.Format("{0:00000000}", ++i)).SubItems.Add(S);
	      Application.DoEvents();
	    }
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
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:AreaNames"),       0,  97,  57, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:AreaNamesAlt"),    0,  97,  53, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:ChatFilterTypes"), 0,  97,  39, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:DayNames"),        0,  97,  45, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:Directions"),      0,  97,  43, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:ErrorMessages"),   0,  97,  35, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:InGameMessages1"), 0,  97,  37, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:InGameMessages2"), 0,  97,  38, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:JobNames"),        0,  97,  55, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:MenuItemDesc"),    0,  97,  42, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:MenuItemText"),    0,  97,  41, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:MoonPhases"),      0,  97,  46, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:POLMessages"),     0,  97,  36, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:RegionNames"),     0,  97,  48, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:Time+Pronouns"),   0, 118, 103, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:Various1"),        0,  97,  34, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:Various2"),        0,  97,  40, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:Various3"),        0,  97,  49, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:WeatherTypes"),    0,  97,  44, new EventHandler(this.ROMMenuItem_Click)));
      }
    }

    private void mnuSTJapanese_Popup(object sender, System.EventArgs e) {
    MenuItem MI = sender as MenuItem;
      if (MI != null && MI.MenuItems.Count == 1 && MI.MenuItems[0].Text == this.DummyText) {
	MI.MenuItems.Clear();
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:AreaNames"),       0,  97,  56, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:ChatFilterTypes"), 0,  97,  21, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:DayNames"),        0,  97,  27, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:Directions"),      0,  97,  25, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:ErrorMessages"),   0,  97,  18, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:InGameMessages1"), 0,  97,  19, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:InGameMessages2"), 0,  97,  20, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:JobNames"),        0,  97,  29, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:MenuItemDesc"),    0,  97,  24, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:MenuItemText"),    0,  97,  23, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:MoonPhases"),      0,  97,  28, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:POLMessages"),     0,  97,   8, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:RegionNames"),     0,  97,  30, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:Time+Pronouns"),   0, 118, 102, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:Various1"),        0,  97,  17, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:Various2"),        0,  97,  22, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:Various3"),        0,  97,  31, new EventHandler(this.ROMMenuItem_Click)));
	MI.MenuItems.Add(new ROMMenuItem(I18N.GetText("Menu:WeatherTypes"),    0,  97,  26, new EventHandler(this.ROMMenuItem_Click)));
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
