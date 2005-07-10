// $Id$

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using PlayOnline.Core;

namespace PlayOnline.FFXI.Utils.DataBrowser {

  internal class ItemFindDialog : System.Windows.Forms.Form {

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

    public ItemFindDialog(FFXIItem[] Items) {
      InitializeComponent();
      this.Icon   = Icons.Search;
      this.Items_ = Items;
      this.SelectedItem_ = null;
      this.Predicates_ = new ArrayList(1);
      this.cmbLanguage.Items.AddRange(NamedEnum.GetAll(typeof(ItemDataLanguage)));
      this.cmbLanguage.SelectedIndex = 0;
      this.cmbItemType.Items.AddRange(NamedEnum.GetAll(typeof(ItemDataType)));
      this.cmbItemType.SelectedIndex = 1;
      this.lstItems.ColumnClick += new ColumnClickEventHandler(ListViewColumnSorter.ListView_ColumnClick);
      this.AddPredicate();
    }

    private FFXIItem[] Items_;
    private FFXIItem   SelectedItem_;

    public FFXIItem SelectedItem {
      get { return this.SelectedItem_; }
    }

    public ItemDataType Type {
      get {
	return (ItemDataType) (this.cmbItemType.SelectedItem as NamedEnum).Value;
      }
      set {
	foreach (NamedEnum NE in this.cmbItemType.Items) {
	  if ((ItemDataType) NE.Value == value) {
	    this.cmbItemType.SelectedItem = NE;
	    break;
	  }
	}
      }
    }

    public ItemDataLanguage Language {
      get {
	return (ItemDataLanguage) (this.cmbLanguage.SelectedItem as NamedEnum).Value;
      }
      set {
	foreach (NamedEnum NE in this.cmbLanguage.Items) {
	  if ((ItemDataLanguage) NE.Value == value) {
	    this.cmbLanguage.SelectedItem = NE;
	    break;
	  }
	}
      }
    }

    #region Predicate Handling

    private ArrayList Predicates_;

    private void AddPredicate() {
    ItemPredicate IP = new ItemPredicate();
      this.pnlSearchOptions.Height += IP.Height + 4;
      IP.Left     = this.grpDisplayMode.Left;
      IP.Top      = this.pnlSearchOptions.Height - IP.Height - 4;
      IP.Width    = this.pnlSearchOptions.Width - 2 * IP.Left;
      IP.Anchor   = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
      IP.Language = this.Language;
      IP.Type     = this.Type;
      this.pnlSearchOptions.Controls.Add(IP);
    Button B = new Button(); // Add or Remove button
      B.Tag       = IP;
      B.FlatStyle = FlatStyle.System;
      B.Width     = 20;
      B.Height    = IP.Height;
      IP.Width   -= 4 + B.Width;
      B.Top       = IP.Top;
      B.Left      = IP.Left + IP.Width + 4;
      B.Anchor    = AnchorStyles.Right | AnchorStyles.Top;
      if (this.Predicates_.Count == 0) {
	B.Text    = "+";
	B.Click  += new EventHandler(this.AddButton_Click);
      }
      else {
	B.Text    = "-";
	B.Click  += new EventHandler(this.RemoveButton_Click);
      }
      this.pnlSearchOptions.Controls.Add(B);
      this.Predicates_.Add(IP);
    }

    private void AddButton_Click(object sender, System.EventArgs e) {
      this.AddPredicate();
    }

    private void RemoveButton_Click(object sender, System.EventArgs e) {
    Button B = sender as Button;
      if (B == null)
	return;
    ItemPredicate IP = B.Tag as ItemPredicate;
      if (IP != null) {
      int idx = this.Predicates_.IndexOf(IP);
	this.pnlSearchOptions.Controls.Remove(IP);
	this.pnlSearchOptions.Controls.Remove(B);
	this.Predicates_.Remove(B.Width);
	for (int i = idx; i < this.Predicates_.Count; ++i) {
	ItemPredicate LowerIP = this.Predicates_[i] as ItemPredicate;
	  LowerIP.Top -= 4 + IP.Height;
	  foreach (Control C in this.pnlSearchOptions.Controls) {
	    if (C is Button && C.Tag != null && C.Tag.Equals(LowerIP)) {
	      C.Top -= 4 + IP.Height;
	      break;
	    }
	  }
	}
	this.pnlSearchOptions.Height -= 4 + IP.Height;
      }
    }

    #endregion

    private void InitializeResultsPane(ItemDataLanguage Language, ItemDataType Type) {
      this.lstItems.Items.Clear();
      this.lstItems.Columns.Clear();
      this.lstItems.HeaderStyle = ColumnHeaderStyle.Nonclickable;
      this.ilItemIcons.Images.Clear();
      this.stbStatus.Visible = true;
      this.stbStatus.Text = String.Format(I18N.GetText("Status:ItemSearch"), this.lstItems.Items.Count);
      Application.DoEvents();
    }

    private void AddResult(FFXIItem Item, ItemDataLanguage Language, ItemDataType Type) {
      this.ilItemIcons.Images.Add(Item.IconGraphic.Bitmap);
    bool AddColumns = (this.lstItems.Columns.Count == 0);
      if (AddColumns)
	this.lstItems.Columns.Add("Index", 80, HorizontalAlignment.Left);
    ListViewItem LVI = this.lstItems.Items.Add(String.Format("{0}", Item.Index), this.ilItemIcons.Images.Count - 1);
      LVI.Tag  = Item;
    FFXIItem.IItemInfo II = Item.GetInfo(Language, Type);
      foreach (ItemField IF in II.GetFields()) {
	if (IF == ItemField.Description) {
	string FieldName = new NamedEnum(IF).Name;
	string[] DescriptionLines = II.GetFieldText(IF).Split('\n');
	  if (AddColumns)
	    this.lstItems.Columns.Add(String.Format(I18N.GetText("ColumnHeader:DescriptionLineCount"), FieldName), 120, HorizontalAlignment.Right);
	  LVI.SubItems.Add(DescriptionLines.Length.ToString());
	  for (int i = 0; i < 6; ++i) {
	    if (AddColumns)
	      this.lstItems.Columns.Add(String.Format(I18N.GetText("ColumnHeader:DescriptionLine"), FieldName, i + 1), 120, HorizontalAlignment.Left);
	    LVI.SubItems.Add((i < DescriptionLines.Length) ? DescriptionLines[i] : "");
	  }
	}
	else {
	  if (AddColumns)
	    this.lstItems.Columns.Add(new NamedEnum(IF).Name, 100, HorizontalAlignment.Left);
	  LVI.SubItems.Add(II.GetFieldText(IF));
	}
      }
      if (AddColumns)
	this.lstItems.Columns.Add("Icon Info", 80, HorizontalAlignment.Left);
      LVI.SubItems.Add(Item.IconGraphic.ToString());
      this.stbStatus.Text = String.Format(I18N.GetText("Status:ItemSearch"), this.lstItems.Items.Count);
      Application.DoEvents();
    }

    private void FinalizeResultsPane(ItemDataLanguage Language, ItemDataType Type) {
      foreach (ColumnHeader CH in this.lstItems.Columns) {
	CH.Width = -1;
	CH.Width += 2;
      }
      this.lstItems.HeaderStyle = ColumnHeaderStyle.Clickable;
      this.mnuILCEResults.Enabled = (this.lstItems.Items.Count > 0);
      this.stbStatus.Text = String.Format(I18N.GetText("Status:ItemSearchDone"), this.lstItems.Items.Count, this.Items_.Length);
    }

    private bool CheckQuery(FFXIItem Item) {
      // Assume AND between predicates for now
      foreach (ItemPredicate IP in this.Predicates_) {
	if (!IP.IsMatch(Item))
	  return false;
      }
      return true;
    }

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
	components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      this.components = new System.ComponentModel.Container();
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ItemFindDialog));
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
      this.lstItems.AccessibleDescription = resources.GetString("lstItems.AccessibleDescription");
      this.lstItems.AccessibleName = resources.GetString("lstItems.AccessibleName");
      this.lstItems.Alignment = ((System.Windows.Forms.ListViewAlignment)(resources.GetObject("lstItems.Alignment")));
      this.lstItems.AllowColumnReorder = true;
      this.lstItems.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lstItems.Anchor")));
      this.lstItems.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("lstItems.BackgroundImage")));
      this.lstItems.ContextMenu = this.mnuItemListContext;
      this.lstItems.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lstItems.Dock")));
      this.lstItems.Enabled = ((bool)(resources.GetObject("lstItems.Enabled")));
      this.lstItems.Font = ((System.Drawing.Font)(resources.GetObject("lstItems.Font")));
      this.lstItems.FullRowSelect = true;
      this.lstItems.GridLines = true;
      this.lstItems.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
      this.lstItems.HideSelection = false;
      this.lstItems.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lstItems.ImeMode")));
      this.lstItems.LabelWrap = ((bool)(resources.GetObject("lstItems.LabelWrap")));
      this.lstItems.Location = ((System.Drawing.Point)(resources.GetObject("lstItems.Location")));
      this.lstItems.Name = "lstItems";
      this.lstItems.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lstItems.RightToLeft")));
      this.lstItems.Size = ((System.Drawing.Size)(resources.GetObject("lstItems.Size")));
      this.lstItems.TabIndex = ((int)(resources.GetObject("lstItems.TabIndex")));
      this.lstItems.Text = resources.GetString("lstItems.Text");
      this.lstItems.View = System.Windows.Forms.View.Details;
      this.lstItems.Visible = ((bool)(resources.GetObject("lstItems.Visible")));
      this.lstItems.DoubleClick += new System.EventHandler(this.lstItems_DoubleClick);
      this.lstItems.SelectedIndexChanged += new System.EventHandler(this.lstItems_SelectedIndexChanged);
      // 
      // mnuItemListContext
      // 
      this.mnuItemListContext.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
										       this.mnuILCCopy,
										       this.mnuILCExport});
      this.mnuItemListContext.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mnuItemListContext.RightToLeft")));
      this.mnuItemListContext.Popup += new System.EventHandler(this.mnuItemListContext_Popup);
      // 
      // mnuILCCopy
      // 
      this.mnuILCCopy.Enabled = ((bool)(resources.GetObject("mnuILCCopy.Enabled")));
      this.mnuILCCopy.Index = 0;
      this.mnuILCCopy.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuILCCopy.Shortcut")));
      this.mnuILCCopy.ShowShortcut = ((bool)(resources.GetObject("mnuILCCopy.ShowShortcut")));
      this.mnuILCCopy.Text = resources.GetString("mnuILCCopy.Text");
      this.mnuILCCopy.Visible = ((bool)(resources.GetObject("mnuILCCopy.Visible")));
      // 
      // mnuILCExport
      // 
      this.mnuILCExport.Enabled = ((bool)(resources.GetObject("mnuILCExport.Enabled")));
      this.mnuILCExport.Index = 1;
      this.mnuILCExport.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
										 this.mnuILCEAll,
										 this.mnuILCEResults,
										 this.mnuILCESelected});
      this.mnuILCExport.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuILCExport.Shortcut")));
      this.mnuILCExport.ShowShortcut = ((bool)(resources.GetObject("mnuILCExport.ShowShortcut")));
      this.mnuILCExport.Text = resources.GetString("mnuILCExport.Text");
      this.mnuILCExport.Visible = ((bool)(resources.GetObject("mnuILCExport.Visible")));
      // 
      // mnuILCEAll
      // 
      this.mnuILCEAll.Enabled = ((bool)(resources.GetObject("mnuILCEAll.Enabled")));
      this.mnuILCEAll.Index = 0;
      this.mnuILCEAll.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuILCEAll.Shortcut")));
      this.mnuILCEAll.ShowShortcut = ((bool)(resources.GetObject("mnuILCEAll.ShowShortcut")));
      this.mnuILCEAll.Text = resources.GetString("mnuILCEAll.Text");
      this.mnuILCEAll.Visible = ((bool)(resources.GetObject("mnuILCEAll.Visible")));
      this.mnuILCEAll.Click += new System.EventHandler(this.mnuILCECAll_Click);
      // 
      // mnuILCEResults
      // 
      this.mnuILCEResults.Enabled = ((bool)(resources.GetObject("mnuILCEResults.Enabled")));
      this.mnuILCEResults.Index = 1;
      this.mnuILCEResults.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuILCEResults.Shortcut")));
      this.mnuILCEResults.ShowShortcut = ((bool)(resources.GetObject("mnuILCEResults.ShowShortcut")));
      this.mnuILCEResults.Text = resources.GetString("mnuILCEResults.Text");
      this.mnuILCEResults.Visible = ((bool)(resources.GetObject("mnuILCEResults.Visible")));
      this.mnuILCEResults.Click += new System.EventHandler(this.mnuILCECResults_Click);
      // 
      // mnuILCESelected
      // 
      this.mnuILCESelected.Enabled = ((bool)(resources.GetObject("mnuILCESelected.Enabled")));
      this.mnuILCESelected.Index = 2;
      this.mnuILCESelected.Shortcut = ((System.Windows.Forms.Shortcut)(resources.GetObject("mnuILCESelected.Shortcut")));
      this.mnuILCESelected.ShowShortcut = ((bool)(resources.GetObject("mnuILCESelected.ShowShortcut")));
      this.mnuILCESelected.Text = resources.GetString("mnuILCESelected.Text");
      this.mnuILCESelected.Visible = ((bool)(resources.GetObject("mnuILCESelected.Visible")));
      this.mnuILCESelected.Click += new System.EventHandler(this.mnuILCECSelected_Click);
      // 
      // ilItemIcons
      // 
      this.ilItemIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
      this.ilItemIcons.ImageSize = ((System.Drawing.Size)(resources.GetObject("ilItemIcons.ImageSize")));
      this.ilItemIcons.TransparentColor = System.Drawing.Color.Transparent;
      // 
      // pnlSearchOptions
      // 
      this.pnlSearchOptions.AccessibleDescription = resources.GetString("pnlSearchOptions.AccessibleDescription");
      this.pnlSearchOptions.AccessibleName = resources.GetString("pnlSearchOptions.AccessibleName");
      this.pnlSearchOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("pnlSearchOptions.Anchor")));
      this.pnlSearchOptions.AutoScroll = ((bool)(resources.GetObject("pnlSearchOptions.AutoScroll")));
      this.pnlSearchOptions.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("pnlSearchOptions.AutoScrollMargin")));
      this.pnlSearchOptions.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("pnlSearchOptions.AutoScrollMinSize")));
      this.pnlSearchOptions.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlSearchOptions.BackgroundImage")));
      this.pnlSearchOptions.Controls.Add(this.btnClose);
      this.pnlSearchOptions.Controls.Add(this.grpDisplayMode);
      this.pnlSearchOptions.Controls.Add(this.btnRunQuery);
      this.pnlSearchOptions.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("pnlSearchOptions.Dock")));
      this.pnlSearchOptions.Enabled = ((bool)(resources.GetObject("pnlSearchOptions.Enabled")));
      this.pnlSearchOptions.Font = ((System.Drawing.Font)(resources.GetObject("pnlSearchOptions.Font")));
      this.pnlSearchOptions.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("pnlSearchOptions.ImeMode")));
      this.pnlSearchOptions.Location = ((System.Drawing.Point)(resources.GetObject("pnlSearchOptions.Location")));
      this.pnlSearchOptions.Name = "pnlSearchOptions";
      this.pnlSearchOptions.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("pnlSearchOptions.RightToLeft")));
      this.pnlSearchOptions.Size = ((System.Drawing.Size)(resources.GetObject("pnlSearchOptions.Size")));
      this.pnlSearchOptions.TabIndex = ((int)(resources.GetObject("pnlSearchOptions.TabIndex")));
      this.pnlSearchOptions.Text = resources.GetString("pnlSearchOptions.Text");
      this.pnlSearchOptions.Visible = ((bool)(resources.GetObject("pnlSearchOptions.Visible")));
      // 
      // btnClose
      // 
      this.btnClose.AccessibleDescription = resources.GetString("btnClose.AccessibleDescription");
      this.btnClose.AccessibleName = resources.GetString("btnClose.AccessibleName");
      this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnClose.Anchor")));
      this.btnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnClose.BackgroundImage")));
      this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnClose.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnClose.Dock")));
      this.btnClose.Enabled = ((bool)(resources.GetObject("btnClose.Enabled")));
      this.btnClose.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnClose.FlatStyle")));
      this.btnClose.Font = ((System.Drawing.Font)(resources.GetObject("btnClose.Font")));
      this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
      this.btnClose.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnClose.ImageAlign")));
      this.btnClose.ImageIndex = ((int)(resources.GetObject("btnClose.ImageIndex")));
      this.btnClose.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnClose.ImeMode")));
      this.btnClose.Location = ((System.Drawing.Point)(resources.GetObject("btnClose.Location")));
      this.btnClose.Name = "btnClose";
      this.btnClose.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnClose.RightToLeft")));
      this.btnClose.Size = ((System.Drawing.Size)(resources.GetObject("btnClose.Size")));
      this.btnClose.TabIndex = ((int)(resources.GetObject("btnClose.TabIndex")));
      this.btnClose.Text = resources.GetString("btnClose.Text");
      this.btnClose.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnClose.TextAlign")));
      this.btnClose.Visible = ((bool)(resources.GetObject("btnClose.Visible")));
      // 
      // grpDisplayMode
      // 
      this.grpDisplayMode.AccessibleDescription = resources.GetString("grpDisplayMode.AccessibleDescription");
      this.grpDisplayMode.AccessibleName = resources.GetString("grpDisplayMode.AccessibleName");
      this.grpDisplayMode.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("grpDisplayMode.Anchor")));
      this.grpDisplayMode.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpDisplayMode.BackgroundImage")));
      this.grpDisplayMode.Controls.Add(this.chkShowIcons);
      this.grpDisplayMode.Controls.Add(this.cmbItemType);
      this.grpDisplayMode.Controls.Add(this.cmbLanguage);
      this.grpDisplayMode.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("grpDisplayMode.Dock")));
      this.grpDisplayMode.Enabled = ((bool)(resources.GetObject("grpDisplayMode.Enabled")));
      this.grpDisplayMode.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.grpDisplayMode.Font = ((System.Drawing.Font)(resources.GetObject("grpDisplayMode.Font")));
      this.grpDisplayMode.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("grpDisplayMode.ImeMode")));
      this.grpDisplayMode.Location = ((System.Drawing.Point)(resources.GetObject("grpDisplayMode.Location")));
      this.grpDisplayMode.Name = "grpDisplayMode";
      this.grpDisplayMode.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("grpDisplayMode.RightToLeft")));
      this.grpDisplayMode.Size = ((System.Drawing.Size)(resources.GetObject("grpDisplayMode.Size")));
      this.grpDisplayMode.TabIndex = ((int)(resources.GetObject("grpDisplayMode.TabIndex")));
      this.grpDisplayMode.TabStop = false;
      this.grpDisplayMode.Text = resources.GetString("grpDisplayMode.Text");
      this.grpDisplayMode.Visible = ((bool)(resources.GetObject("grpDisplayMode.Visible")));
      // 
      // chkShowIcons
      // 
      this.chkShowIcons.AccessibleDescription = resources.GetString("chkShowIcons.AccessibleDescription");
      this.chkShowIcons.AccessibleName = resources.GetString("chkShowIcons.AccessibleName");
      this.chkShowIcons.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("chkShowIcons.Anchor")));
      this.chkShowIcons.Appearance = ((System.Windows.Forms.Appearance)(resources.GetObject("chkShowIcons.Appearance")));
      this.chkShowIcons.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chkShowIcons.BackgroundImage")));
      this.chkShowIcons.CheckAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkShowIcons.CheckAlign")));
      this.chkShowIcons.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("chkShowIcons.Dock")));
      this.chkShowIcons.Enabled = ((bool)(resources.GetObject("chkShowIcons.Enabled")));
      this.chkShowIcons.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("chkShowIcons.FlatStyle")));
      this.chkShowIcons.Font = ((System.Drawing.Font)(resources.GetObject("chkShowIcons.Font")));
      this.chkShowIcons.Image = ((System.Drawing.Image)(resources.GetObject("chkShowIcons.Image")));
      this.chkShowIcons.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkShowIcons.ImageAlign")));
      this.chkShowIcons.ImageIndex = ((int)(resources.GetObject("chkShowIcons.ImageIndex")));
      this.chkShowIcons.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("chkShowIcons.ImeMode")));
      this.chkShowIcons.Location = ((System.Drawing.Point)(resources.GetObject("chkShowIcons.Location")));
      this.chkShowIcons.Name = "chkShowIcons";
      this.chkShowIcons.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("chkShowIcons.RightToLeft")));
      this.chkShowIcons.Size = ((System.Drawing.Size)(resources.GetObject("chkShowIcons.Size")));
      this.chkShowIcons.TabIndex = ((int)(resources.GetObject("chkShowIcons.TabIndex")));
      this.chkShowIcons.Text = resources.GetString("chkShowIcons.Text");
      this.chkShowIcons.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("chkShowIcons.TextAlign")));
      this.chkShowIcons.Visible = ((bool)(resources.GetObject("chkShowIcons.Visible")));
      this.chkShowIcons.CheckedChanged += new System.EventHandler(this.chkShowIcons_CheckedChanged);
      // 
      // cmbItemType
      // 
      this.cmbItemType.AccessibleDescription = resources.GetString("cmbItemType.AccessibleDescription");
      this.cmbItemType.AccessibleName = resources.GetString("cmbItemType.AccessibleName");
      this.cmbItemType.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("cmbItemType.Anchor")));
      this.cmbItemType.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("cmbItemType.BackgroundImage")));
      this.cmbItemType.DisplayMember = "Name";
      this.cmbItemType.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("cmbItemType.Dock")));
      this.cmbItemType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbItemType.Enabled = ((bool)(resources.GetObject("cmbItemType.Enabled")));
      this.cmbItemType.Font = ((System.Drawing.Font)(resources.GetObject("cmbItemType.Font")));
      this.cmbItemType.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("cmbItemType.ImeMode")));
      this.cmbItemType.IntegralHeight = ((bool)(resources.GetObject("cmbItemType.IntegralHeight")));
      this.cmbItemType.ItemHeight = ((int)(resources.GetObject("cmbItemType.ItemHeight")));
      this.cmbItemType.Location = ((System.Drawing.Point)(resources.GetObject("cmbItemType.Location")));
      this.cmbItemType.MaxDropDownItems = ((int)(resources.GetObject("cmbItemType.MaxDropDownItems")));
      this.cmbItemType.MaxLength = ((int)(resources.GetObject("cmbItemType.MaxLength")));
      this.cmbItemType.Name = "cmbItemType";
      this.cmbItemType.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("cmbItemType.RightToLeft")));
      this.cmbItemType.Size = ((System.Drawing.Size)(resources.GetObject("cmbItemType.Size")));
      this.cmbItemType.Sorted = true;
      this.cmbItemType.TabIndex = ((int)(resources.GetObject("cmbItemType.TabIndex")));
      this.cmbItemType.Text = resources.GetString("cmbItemType.Text");
      this.cmbItemType.Visible = ((bool)(resources.GetObject("cmbItemType.Visible")));
      this.cmbItemType.SelectedIndexChanged += new System.EventHandler(this.cmbItemType_SelectedIndexChanged);
      // 
      // cmbLanguage
      // 
      this.cmbLanguage.AccessibleDescription = resources.GetString("cmbLanguage.AccessibleDescription");
      this.cmbLanguage.AccessibleName = resources.GetString("cmbLanguage.AccessibleName");
      this.cmbLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("cmbLanguage.Anchor")));
      this.cmbLanguage.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("cmbLanguage.BackgroundImage")));
      this.cmbLanguage.DisplayMember = "Name";
      this.cmbLanguage.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("cmbLanguage.Dock")));
      this.cmbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbLanguage.Enabled = ((bool)(resources.GetObject("cmbLanguage.Enabled")));
      this.cmbLanguage.Font = ((System.Drawing.Font)(resources.GetObject("cmbLanguage.Font")));
      this.cmbLanguage.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("cmbLanguage.ImeMode")));
      this.cmbLanguage.IntegralHeight = ((bool)(resources.GetObject("cmbLanguage.IntegralHeight")));
      this.cmbLanguage.ItemHeight = ((int)(resources.GetObject("cmbLanguage.ItemHeight")));
      this.cmbLanguage.Location = ((System.Drawing.Point)(resources.GetObject("cmbLanguage.Location")));
      this.cmbLanguage.MaxDropDownItems = ((int)(resources.GetObject("cmbLanguage.MaxDropDownItems")));
      this.cmbLanguage.MaxLength = ((int)(resources.GetObject("cmbLanguage.MaxLength")));
      this.cmbLanguage.Name = "cmbLanguage";
      this.cmbLanguage.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("cmbLanguage.RightToLeft")));
      this.cmbLanguage.Size = ((System.Drawing.Size)(resources.GetObject("cmbLanguage.Size")));
      this.cmbLanguage.Sorted = true;
      this.cmbLanguage.TabIndex = ((int)(resources.GetObject("cmbLanguage.TabIndex")));
      this.cmbLanguage.Text = resources.GetString("cmbLanguage.Text");
      this.cmbLanguage.Visible = ((bool)(resources.GetObject("cmbLanguage.Visible")));
      this.cmbLanguage.SelectedIndexChanged += new System.EventHandler(this.cmbLanguage_SelectedIndexChanged);
      // 
      // btnRunQuery
      // 
      this.btnRunQuery.AccessibleDescription = resources.GetString("btnRunQuery.AccessibleDescription");
      this.btnRunQuery.AccessibleName = resources.GetString("btnRunQuery.AccessibleName");
      this.btnRunQuery.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnRunQuery.Anchor")));
      this.btnRunQuery.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRunQuery.BackgroundImage")));
      this.btnRunQuery.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnRunQuery.Dock")));
      this.btnRunQuery.Enabled = ((bool)(resources.GetObject("btnRunQuery.Enabled")));
      this.btnRunQuery.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnRunQuery.FlatStyle")));
      this.btnRunQuery.Font = ((System.Drawing.Font)(resources.GetObject("btnRunQuery.Font")));
      this.btnRunQuery.Image = ((System.Drawing.Image)(resources.GetObject("btnRunQuery.Image")));
      this.btnRunQuery.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnRunQuery.ImageAlign")));
      this.btnRunQuery.ImageIndex = ((int)(resources.GetObject("btnRunQuery.ImageIndex")));
      this.btnRunQuery.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnRunQuery.ImeMode")));
      this.btnRunQuery.Location = ((System.Drawing.Point)(resources.GetObject("btnRunQuery.Location")));
      this.btnRunQuery.Name = "btnRunQuery";
      this.btnRunQuery.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnRunQuery.RightToLeft")));
      this.btnRunQuery.Size = ((System.Drawing.Size)(resources.GetObject("btnRunQuery.Size")));
      this.btnRunQuery.TabIndex = ((int)(resources.GetObject("btnRunQuery.TabIndex")));
      this.btnRunQuery.Text = resources.GetString("btnRunQuery.Text");
      this.btnRunQuery.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnRunQuery.TextAlign")));
      this.btnRunQuery.Visible = ((bool)(resources.GetObject("btnRunQuery.Visible")));
      this.btnRunQuery.Click += new System.EventHandler(this.btnRunQuery_Click);
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
      // ItemFindDialog
      // 
      this.AcceptButton = this.btnRunQuery;
      this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
      this.AccessibleName = resources.GetString("$this.AccessibleName");
      this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
      this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
      this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
      this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
      this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
      this.CancelButton = this.btnClose;
      this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
      this.Controls.Add(this.stbStatus);
      this.Controls.Add(this.lstItems);
      this.Controls.Add(this.pnlSearchOptions);
      this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
      this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
      this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
      this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
      this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
      this.Name = "ItemFindDialog";
      this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
      this.ShowInTaskbar = false;
      this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
      this.Text = resources.GetString("$this.Text");
      this.pnlSearchOptions.ResumeLayout(false);
      this.grpDisplayMode.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    #region Events

    private void btnRunQuery_Click(object sender, System.EventArgs e) {
      // Ensure the query is valid
      for (int i = 0; i < this.Predicates_.Count; ++i) {
      ItemPredicate IP = this.Predicates_[i] as ItemPredicate;
	if (IP == null)
	  continue;
      string ValidationError = IP.ValidateQuery();
	if (ValidationError != null) {
	  MessageBox.Show(this, String.Format(I18N.GetText("Message:InvalidQuery"), i + 1, ValidationError), I18N.GetText("Title:InvalidQuery"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
	  return;
	}
      }
      // Clear the results
      this.InitializeResultsPane(this.Language, this.Type);
      // And add all items that match
      foreach (FFXIItem FI in this.Items_) {
	if (this.CheckQuery(FI))
	  this.AddResult(FI, this.Language, this.Type);
      }
      this.FinalizeResultsPane(this.Language, this.Type);
    }

    private void cmbLanguage_SelectedIndexChanged(object sender, System.EventArgs e) {
      if (this.cmbLanguage.SelectedItem == null)
	return;
      foreach (ItemPredicate IP in this.Predicates_)
	IP.Language = this.Language;
    }

    private void cmbItemType_SelectedIndexChanged(object sender, System.EventArgs e) {
      if (this.cmbItemType.SelectedItem == null)
	return;
      foreach (ItemPredicate IP in this.Predicates_)
	IP.Type = this.Type;
    }

    private void chkShowIcons_CheckedChanged(object sender, System.EventArgs e) {
      this.lstItems.SmallImageList = (this.chkShowIcons.Checked ? this.ilItemIcons : null);
    }

    private void lstItems_SelectedIndexChanged(object sender, System.EventArgs e) {
      this.mnuILCESelected.Enabled = (this.lstItems.SelectedItems.Count > 0);
    }

    private void lstItems_DoubleClick(object sender, System.EventArgs e) {
      this.SelectedItem_ = this.lstItems.SelectedItems[0].Tag as FFXIItem;
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void CopyContextMenu_Click(object sender, System.EventArgs e) {
    MenuItem MI = sender as MenuItem;
      if (MI != null && this.lstItems.SelectedItems.Count > 0) {
      string CopyText = String.Empty;
	foreach (ListViewItem LVI in this.lstItems.SelectedItems) {
	  if (CopyText != "")
	    CopyText += '\n';
	  CopyText += LVI.SubItems[MI.Index].Text;
	}
	Clipboard.SetDataObject(CopyText);
      }
    }

    private void mnuItemListContext_Popup(object sender, System.EventArgs e) {
      // Set Copy sub-menu up with all available columns
      this.mnuILCCopy.MenuItems.Clear();
      foreach (ColumnHeader CH in this.lstItems.Columns)
	this.mnuILCCopy.MenuItems.Add(CH.Index, new MenuItem(CH.Text, new EventHandler(this.CopyContextMenu_Click)));
    }

    private void mnuILCECAll_Click(object sender, System.EventArgs e) {
    IItemExporter IE = new ItemExporter(this.Language, this.Type);
      IE.DoExport(this.Items_);
    }

    private void TExportItems() {
    PleaseWaitDialog PWD = new PleaseWaitDialog(I18N.GetText("Dialog:ExportItems"));
      try {
	Application.DoEvents();
	PWD.ShowDialog(this);
      } catch { PWD.Close(); }
    }

    private void mnuILCECResults_Click(object sender, System.EventArgs e) {
    ArrayList Items = new ArrayList();
      foreach (ListViewItem LVI in this.lstItems.Items)
	Items.Add(LVI.Tag);
    IItemExporter IE = new ItemExporter(this.Language, this.Type);
      if (IE.PrepareExport()) {
      Thread T = new Thread(new ThreadStart(this.TExportItems));
	T.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
	T.Start();
	Application.DoEvents();
	IE.DoExport((FFXIItem[]) Items.ToArray(typeof(FFXIItem)));
	T.Abort();
	this.Activate();
      }
    }

    private void mnuILCECSelected_Click(object sender, System.EventArgs e) {
    ArrayList Items = new ArrayList();
      foreach (ListViewItem LVI in this.lstItems.SelectedItems)
	Items.Add(LVI.Tag);
    IItemExporter IE = new ItemExporter(this.Language, this.Type);
      if (IE.PrepareExport()) {
      Thread T = new Thread(new ThreadStart(this.TExportItems));
	T.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
	T.Start();
	Application.DoEvents();
	IE.DoExport((FFXIItem[]) Items.ToArray(typeof(FFXIItem)));
	T.Abort();
	this.Activate();
      }
    }

    #endregion

  }

}
