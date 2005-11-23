// $Id$

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using PlayOnline.Core;

namespace PlayOnline.FFXI.Utils.ItemComparison {

  public class MainWindow : System.Windows.Forms.Form {

    private FFXIItem[] LeftItems;
    private FFXIItem[] LeftItemsShown;
    private FFXIItem[] RightItems;
    private FFXIItem[] RightItemsShown;

    private ItemDataLanguage LLanguage;
    private ItemDataType     LType;
    private ItemDataLanguage RLanguage;
    private ItemDataType     RType;

    private int CurrentItem   = -1;
    private int StartupHeight = -1;

    #region Controls

    private PlayOnline.FFXI.FFXIItemEditor ieLeft;
    private PlayOnline.FFXI.FFXIItemEditor ieRight;
    private System.Windows.Forms.Button btnLoadItemSet1;
    private System.Windows.Forms.Button btnLoadItemSet2;
    private System.Windows.Forms.OpenFileDialog dlgLoadItems;
    private System.Windows.Forms.Button btnPrevious;
    private System.Windows.Forms.Button btnNext;
    private System.Windows.Forms.Button btnRemoveUnchanged;

    private System.ComponentModel.Container components = null;

    #endregion

    public MainWindow() {
      this.InitializeComponent();
      this.StartupHeight = this.Height;
      this.Icon = Icons.FileSearch;
      this.ieLeft.LockViewMode();
      this.ieRight.LockViewMode();
      this.EnableNavigation();
    }

    #region Item Loading

    private void LoadItems(string FileName, FFXIItemEditor IE) {
    ArrayList LoadedItems = new ArrayList();
    ItemDataLanguage LoadedLanguage = ItemDataLanguage.English;
    ItemDataType LoadedType = ItemDataType.Object;
    Thread T = new Thread(new ThreadStart(this.TLoadItems));
      T.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
      T.Start();
      Application.DoEvents();
      try {
      XmlDocument XD = new XmlDocument();
	XD.Load(FileName);
	Application.DoEvents();
	if (XD.DocumentElement.Name == "ItemList") {
	int Index = 0;
	  try { LoadedLanguage = (ItemDataLanguage) Enum.Parse(typeof(ItemDataLanguage), XD.DocumentElement.Attributes["Language"].InnerText); } catch { }
	  try { LoadedType     = (ItemDataType)     Enum.Parse(typeof(ItemDataType),     XD.DocumentElement.Attributes["Type"].InnerText);     } catch { }
	  IE.LockViewMode(LoadedLanguage, LoadedType);
	  foreach (XmlNode XN in XD.DocumentElement.ChildNodes) {
	    if (XN is XmlElement && XN.Name == "Item") {
	      LoadedItems.Add(new FFXIItem(Index++, XN as XmlElement));
	      Application.DoEvents();
	    }
	  }
	}
      } catch { }
      {
      FFXIItem[] LoadedItemArray = (FFXIItem[]) LoadedItems.ToArray(typeof(FFXIItem));
	if (IE == this.ieLeft) {
	  this.LeftItems = LoadedItemArray;
	  this.LLanguage = LoadedLanguage;
	  this.LType     = LoadedType;
	}
	else {
	  this.RightItems = LoadedItemArray;
	  this.RLanguage  = LoadedLanguage;
	  this.RType      = LoadedType;
	}
      }
      this.LeftItemsShown = null;
      this.RightItemsShown = null;
      if (this.RightItems == null && this.LeftItems == null)
	this.CurrentItem = -1;
      else
	this.CurrentItem = 0;
      if (this.RightItems != null && this.LeftItems != null)
	this.btnRemoveUnchanged.Enabled = true;
      T.Abort();
      this.Activate();
      this.EnableNavigation();
      this.MarkItemChanges();
    }

    #endregion

    #region Item Display

    private string GetIconString(FFXIItem I) {
    string IconString = "";
      if (I.IconGraphic != null) {
	IconString += I.IconGraphic.ToString(); // general description
	if (I.IconGraphic.Bitmap != null) {
	MemoryStream MS = new MemoryStream();
	  I.IconGraphic.Bitmap.Save(MS, ImageFormat.Png);
	  IconString += Convert.ToBase64String(MS.GetBuffer());
	  MS.Close();
	}
      }
      return IconString;
    }

    private void MarkItemChanges() {
      if (this.ieLeft.Item != null && this.ieRight.Item != null) {
	{ // Compare icon
	bool IconChanged = (this.GetIconString(this.ieLeft.Item) != this.GetIconString(this.ieRight.Item));
	  this.ieLeft.MarkIcon (IconChanged ? FFXIItemEditor.Mark.Changed : FFXIItemEditor.Mark.None);
	  this.ieRight.MarkIcon(IconChanged ? FFXIItemEditor.Mark.Changed : FFXIItemEditor.Mark.None);
	}
	// Compare fields
	foreach (ItemField IF in Enum.GetValues(typeof(ItemField))) {
	bool FieldChanged = (this.ieLeft.ItemInfo.GetFieldText(IF) != this.ieRight.ItemInfo.GetFieldText(IF));
	  this.ieLeft.MarkField (IF, FieldChanged ? FFXIItemEditor.Mark.Changed : FFXIItemEditor.Mark.None);
	  this.ieRight.MarkField(IF, FieldChanged ? FFXIItemEditor.Mark.Changed : FFXIItemEditor.Mark.None);
	}
      }
    }

    private void EnableNavigation() {
      this.ieLeft.Item = null;
      this.ieRight.Item = null;
      this.btnPrevious.Enabled = (this.CurrentItem > 0);
      this.btnNext.Enabled = false;
    FFXIItem LeftItem  = null;
    FFXIItem RightItem = null;
      if (this.CurrentItem >= 0) {
	if (this.LeftItemsShown != null) {
	  if (this.CurrentItem < this.LeftItemsShown.Length)
	    LeftItem = this.LeftItemsShown[this.CurrentItem];
	  if (this.CurrentItem < this.LeftItemsShown.Length - 1)
	    this.btnNext.Enabled = true;
	}
	else if (this.LeftItems != null) {
	  if (this.CurrentItem < this.LeftItems.Length)
	    LeftItem = this.LeftItems[this.CurrentItem];
	  if (this.CurrentItem < this.LeftItems.Length - 1)
	    this.btnNext.Enabled = true;
	}
	if (this.RightItemsShown != null) {
	  if (this.CurrentItem < this.RightItemsShown.Length)
	    RightItem = this.RightItemsShown[this.CurrentItem];
	  if (this.CurrentItem < this.RightItemsShown.Length - 1)
	    this.btnNext.Enabled = true;
	}
	else if (this.RightItems != null) {
	  if (this.CurrentItem < this.RightItems.Length)
	    RightItem = this.RightItems[this.CurrentItem];
	  if (this.CurrentItem < this.RightItems.Length - 1)
	    this.btnNext.Enabled = true;
	}
      }
      else
	this.btnNext.Enabled = false;
      this.ieLeft.Item  = LeftItem;
      this.ieRight.Item = RightItem;
    }

    #endregion

    #region "Please Wait" Threads

    private void TLoadItems() {
    PleaseWaitDialog PWD = new PleaseWaitDialog(I18N.GetText("Dialog:LoadItems"));
      try {
	Application.DoEvents();
	PWD.ShowDialog(this);
      } catch { PWD.Close(); }
    }

    private void TRemoveUnchanged() {
    PleaseWaitDialog PWD = new PleaseWaitDialog(I18N.GetText("Dialog:RemoveUnchanged"));
      try {
	Application.DoEvents();
	PWD.ShowDialog(this);
      } catch { PWD.Close(); }
    }

    #endregion

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
	components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainWindow));
      this.ieLeft = new PlayOnline.FFXI.FFXIItemEditor();
      this.ieRight = new PlayOnline.FFXI.FFXIItemEditor();
      this.btnLoadItemSet1 = new System.Windows.Forms.Button();
      this.btnLoadItemSet2 = new System.Windows.Forms.Button();
      this.dlgLoadItems = new System.Windows.Forms.OpenFileDialog();
      this.btnPrevious = new System.Windows.Forms.Button();
      this.btnNext = new System.Windows.Forms.Button();
      this.btnRemoveUnchanged = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // ieLeft
      // 
      this.ieLeft.AccessibleDescription = resources.GetString("ieLeft.AccessibleDescription");
      this.ieLeft.AccessibleName = resources.GetString("ieLeft.AccessibleName");
      this.ieLeft.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("ieLeft.Anchor")));
      this.ieLeft.AutoScroll = ((bool)(resources.GetObject("ieLeft.AutoScroll")));
      this.ieLeft.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("ieLeft.AutoScrollMargin")));
      this.ieLeft.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("ieLeft.AutoScrollMinSize")));
      this.ieLeft.BackColor = System.Drawing.SystemColors.Control;
      this.ieLeft.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ieLeft.BackgroundImage")));
      this.ieLeft.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("ieLeft.Dock")));
      this.ieLeft.Enabled = ((bool)(resources.GetObject("ieLeft.Enabled")));
      this.ieLeft.Font = ((System.Drawing.Font)(resources.GetObject("ieLeft.Font")));
      this.ieLeft.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("ieLeft.ImeMode")));
      this.ieLeft.Item = null;
      this.ieLeft.Location = ((System.Drawing.Point)(resources.GetObject("ieLeft.Location")));
      this.ieLeft.Name = "ieLeft";
      this.ieLeft.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("ieLeft.RightToLeft")));
      this.ieLeft.Size = ((System.Drawing.Size)(resources.GetObject("ieLeft.Size")));
      this.ieLeft.TabIndex = ((int)(resources.GetObject("ieLeft.TabIndex")));
      this.ieLeft.Visible = ((bool)(resources.GetObject("ieLeft.Visible")));
      this.ieLeft.SizeChanged += new System.EventHandler(this.ItemViewerSizeChanged);
      // 
      // ieRight
      // 
      this.ieRight.AccessibleDescription = resources.GetString("ieRight.AccessibleDescription");
      this.ieRight.AccessibleName = resources.GetString("ieRight.AccessibleName");
      this.ieRight.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("ieRight.Anchor")));
      this.ieRight.AutoScroll = ((bool)(resources.GetObject("ieRight.AutoScroll")));
      this.ieRight.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("ieRight.AutoScrollMargin")));
      this.ieRight.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("ieRight.AutoScrollMinSize")));
      this.ieRight.BackColor = System.Drawing.SystemColors.Control;
      this.ieRight.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ieRight.BackgroundImage")));
      this.ieRight.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("ieRight.Dock")));
      this.ieRight.Enabled = ((bool)(resources.GetObject("ieRight.Enabled")));
      this.ieRight.Font = ((System.Drawing.Font)(resources.GetObject("ieRight.Font")));
      this.ieRight.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("ieRight.ImeMode")));
      this.ieRight.Item = null;
      this.ieRight.Location = ((System.Drawing.Point)(resources.GetObject("ieRight.Location")));
      this.ieRight.Name = "ieRight";
      this.ieRight.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("ieRight.RightToLeft")));
      this.ieRight.Size = ((System.Drawing.Size)(resources.GetObject("ieRight.Size")));
      this.ieRight.TabIndex = ((int)(resources.GetObject("ieRight.TabIndex")));
      this.ieRight.Visible = ((bool)(resources.GetObject("ieRight.Visible")));
      this.ieRight.SizeChanged += new System.EventHandler(this.ItemViewerSizeChanged);
      // 
      // btnLoadItemSet1
      // 
      this.btnLoadItemSet1.AccessibleDescription = resources.GetString("btnLoadItemSet1.AccessibleDescription");
      this.btnLoadItemSet1.AccessibleName = resources.GetString("btnLoadItemSet1.AccessibleName");
      this.btnLoadItemSet1.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnLoadItemSet1.Anchor")));
      this.btnLoadItemSet1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnLoadItemSet1.BackgroundImage")));
      this.btnLoadItemSet1.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnLoadItemSet1.Dock")));
      this.btnLoadItemSet1.Enabled = ((bool)(resources.GetObject("btnLoadItemSet1.Enabled")));
      this.btnLoadItemSet1.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnLoadItemSet1.FlatStyle")));
      this.btnLoadItemSet1.Font = ((System.Drawing.Font)(resources.GetObject("btnLoadItemSet1.Font")));
      this.btnLoadItemSet1.Image = ((System.Drawing.Image)(resources.GetObject("btnLoadItemSet1.Image")));
      this.btnLoadItemSet1.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnLoadItemSet1.ImageAlign")));
      this.btnLoadItemSet1.ImageIndex = ((int)(resources.GetObject("btnLoadItemSet1.ImageIndex")));
      this.btnLoadItemSet1.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnLoadItemSet1.ImeMode")));
      this.btnLoadItemSet1.Location = ((System.Drawing.Point)(resources.GetObject("btnLoadItemSet1.Location")));
      this.btnLoadItemSet1.Name = "btnLoadItemSet1";
      this.btnLoadItemSet1.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnLoadItemSet1.RightToLeft")));
      this.btnLoadItemSet1.Size = ((System.Drawing.Size)(resources.GetObject("btnLoadItemSet1.Size")));
      this.btnLoadItemSet1.TabIndex = ((int)(resources.GetObject("btnLoadItemSet1.TabIndex")));
      this.btnLoadItemSet1.Text = resources.GetString("btnLoadItemSet1.Text");
      this.btnLoadItemSet1.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnLoadItemSet1.TextAlign")));
      this.btnLoadItemSet1.Visible = ((bool)(resources.GetObject("btnLoadItemSet1.Visible")));
      this.btnLoadItemSet1.Click += new System.EventHandler(this.btnLoadItemSet1_Click);
      // 
      // btnLoadItemSet2
      // 
      this.btnLoadItemSet2.AccessibleDescription = resources.GetString("btnLoadItemSet2.AccessibleDescription");
      this.btnLoadItemSet2.AccessibleName = resources.GetString("btnLoadItemSet2.AccessibleName");
      this.btnLoadItemSet2.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnLoadItemSet2.Anchor")));
      this.btnLoadItemSet2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnLoadItemSet2.BackgroundImage")));
      this.btnLoadItemSet2.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnLoadItemSet2.Dock")));
      this.btnLoadItemSet2.Enabled = ((bool)(resources.GetObject("btnLoadItemSet2.Enabled")));
      this.btnLoadItemSet2.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnLoadItemSet2.FlatStyle")));
      this.btnLoadItemSet2.Font = ((System.Drawing.Font)(resources.GetObject("btnLoadItemSet2.Font")));
      this.btnLoadItemSet2.Image = ((System.Drawing.Image)(resources.GetObject("btnLoadItemSet2.Image")));
      this.btnLoadItemSet2.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnLoadItemSet2.ImageAlign")));
      this.btnLoadItemSet2.ImageIndex = ((int)(resources.GetObject("btnLoadItemSet2.ImageIndex")));
      this.btnLoadItemSet2.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnLoadItemSet2.ImeMode")));
      this.btnLoadItemSet2.Location = ((System.Drawing.Point)(resources.GetObject("btnLoadItemSet2.Location")));
      this.btnLoadItemSet2.Name = "btnLoadItemSet2";
      this.btnLoadItemSet2.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnLoadItemSet2.RightToLeft")));
      this.btnLoadItemSet2.Size = ((System.Drawing.Size)(resources.GetObject("btnLoadItemSet2.Size")));
      this.btnLoadItemSet2.TabIndex = ((int)(resources.GetObject("btnLoadItemSet2.TabIndex")));
      this.btnLoadItemSet2.Text = resources.GetString("btnLoadItemSet2.Text");
      this.btnLoadItemSet2.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnLoadItemSet2.TextAlign")));
      this.btnLoadItemSet2.Visible = ((bool)(resources.GetObject("btnLoadItemSet2.Visible")));
      this.btnLoadItemSet2.Click += new System.EventHandler(this.btnLoadItemSet2_Click);
      // 
      // dlgLoadItems
      // 
      this.dlgLoadItems.Filter = resources.GetString("dlgLoadItems.Filter");
      this.dlgLoadItems.Title = resources.GetString("dlgLoadItems.Title");
      // 
      // btnPrevious
      // 
      this.btnPrevious.AccessibleDescription = resources.GetString("btnPrevious.AccessibleDescription");
      this.btnPrevious.AccessibleName = resources.GetString("btnPrevious.AccessibleName");
      this.btnPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnPrevious.Anchor")));
      this.btnPrevious.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPrevious.BackgroundImage")));
      this.btnPrevious.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnPrevious.Dock")));
      this.btnPrevious.Enabled = ((bool)(resources.GetObject("btnPrevious.Enabled")));
      this.btnPrevious.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnPrevious.FlatStyle")));
      this.btnPrevious.Font = ((System.Drawing.Font)(resources.GetObject("btnPrevious.Font")));
      this.btnPrevious.Image = ((System.Drawing.Image)(resources.GetObject("btnPrevious.Image")));
      this.btnPrevious.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnPrevious.ImageAlign")));
      this.btnPrevious.ImageIndex = ((int)(resources.GetObject("btnPrevious.ImageIndex")));
      this.btnPrevious.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnPrevious.ImeMode")));
      this.btnPrevious.Location = ((System.Drawing.Point)(resources.GetObject("btnPrevious.Location")));
      this.btnPrevious.Name = "btnPrevious";
      this.btnPrevious.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnPrevious.RightToLeft")));
      this.btnPrevious.Size = ((System.Drawing.Size)(resources.GetObject("btnPrevious.Size")));
      this.btnPrevious.TabIndex = ((int)(resources.GetObject("btnPrevious.TabIndex")));
      this.btnPrevious.Text = resources.GetString("btnPrevious.Text");
      this.btnPrevious.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnPrevious.TextAlign")));
      this.btnPrevious.Visible = ((bool)(resources.GetObject("btnPrevious.Visible")));
      this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
      // 
      // btnNext
      // 
      this.btnNext.AccessibleDescription = resources.GetString("btnNext.AccessibleDescription");
      this.btnNext.AccessibleName = resources.GetString("btnNext.AccessibleName");
      this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnNext.Anchor")));
      this.btnNext.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNext.BackgroundImage")));
      this.btnNext.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnNext.Dock")));
      this.btnNext.Enabled = ((bool)(resources.GetObject("btnNext.Enabled")));
      this.btnNext.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnNext.FlatStyle")));
      this.btnNext.Font = ((System.Drawing.Font)(resources.GetObject("btnNext.Font")));
      this.btnNext.Image = ((System.Drawing.Image)(resources.GetObject("btnNext.Image")));
      this.btnNext.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnNext.ImageAlign")));
      this.btnNext.ImageIndex = ((int)(resources.GetObject("btnNext.ImageIndex")));
      this.btnNext.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnNext.ImeMode")));
      this.btnNext.Location = ((System.Drawing.Point)(resources.GetObject("btnNext.Location")));
      this.btnNext.Name = "btnNext";
      this.btnNext.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnNext.RightToLeft")));
      this.btnNext.Size = ((System.Drawing.Size)(resources.GetObject("btnNext.Size")));
      this.btnNext.TabIndex = ((int)(resources.GetObject("btnNext.TabIndex")));
      this.btnNext.Text = resources.GetString("btnNext.Text");
      this.btnNext.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnNext.TextAlign")));
      this.btnNext.Visible = ((bool)(resources.GetObject("btnNext.Visible")));
      this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
      // 
      // btnRemoveUnchanged
      // 
      this.btnRemoveUnchanged.AccessibleDescription = resources.GetString("btnRemoveUnchanged.AccessibleDescription");
      this.btnRemoveUnchanged.AccessibleName = resources.GetString("btnRemoveUnchanged.AccessibleName");
      this.btnRemoveUnchanged.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnRemoveUnchanged.Anchor")));
      this.btnRemoveUnchanged.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRemoveUnchanged.BackgroundImage")));
      this.btnRemoveUnchanged.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnRemoveUnchanged.Dock")));
      this.btnRemoveUnchanged.Enabled = ((bool)(resources.GetObject("btnRemoveUnchanged.Enabled")));
      this.btnRemoveUnchanged.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnRemoveUnchanged.FlatStyle")));
      this.btnRemoveUnchanged.Font = ((System.Drawing.Font)(resources.GetObject("btnRemoveUnchanged.Font")));
      this.btnRemoveUnchanged.Image = ((System.Drawing.Image)(resources.GetObject("btnRemoveUnchanged.Image")));
      this.btnRemoveUnchanged.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnRemoveUnchanged.ImageAlign")));
      this.btnRemoveUnchanged.ImageIndex = ((int)(resources.GetObject("btnRemoveUnchanged.ImageIndex")));
      this.btnRemoveUnchanged.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnRemoveUnchanged.ImeMode")));
      this.btnRemoveUnchanged.Location = ((System.Drawing.Point)(resources.GetObject("btnRemoveUnchanged.Location")));
      this.btnRemoveUnchanged.Name = "btnRemoveUnchanged";
      this.btnRemoveUnchanged.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnRemoveUnchanged.RightToLeft")));
      this.btnRemoveUnchanged.Size = ((System.Drawing.Size)(resources.GetObject("btnRemoveUnchanged.Size")));
      this.btnRemoveUnchanged.TabIndex = ((int)(resources.GetObject("btnRemoveUnchanged.TabIndex")));
      this.btnRemoveUnchanged.Text = resources.GetString("btnRemoveUnchanged.Text");
      this.btnRemoveUnchanged.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnRemoveUnchanged.TextAlign")));
      this.btnRemoveUnchanged.Visible = ((bool)(resources.GetObject("btnRemoveUnchanged.Visible")));
      this.btnRemoveUnchanged.Click += new System.EventHandler(this.btnRemoveUnchanged_Click);
      // 
      // MainWindow
      // 
      this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
      this.AccessibleName = resources.GetString("$this.AccessibleName");
      this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
      this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
      this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
      this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
      this.BackColor = System.Drawing.SystemColors.Control;
      this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
      this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
      this.Controls.Add(this.btnRemoveUnchanged);
      this.Controls.Add(this.btnNext);
      this.Controls.Add(this.btnPrevious);
      this.Controls.Add(this.btnLoadItemSet2);
      this.Controls.Add(this.btnLoadItemSet1);
      this.Controls.Add(this.ieLeft);
      this.Controls.Add(this.ieRight);
      this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
      this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
      this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
      this.MaximizeBox = false;
      this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
      this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
      this.Name = "MainWindow";
      this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
      this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
      this.Text = resources.GetString("$this.Text");
      this.ResumeLayout(false);

    }

    #endregion

    #region Event Handlers

    private void btnLoadItemSet1_Click(object sender, System.EventArgs e) {
      if (this.dlgLoadItems.ShowDialog(this) == DialogResult.OK)
	this.LoadItems(this.dlgLoadItems.FileName, this.ieLeft);
    }

    private void btnLoadItemSet2_Click(object sender, System.EventArgs e) {
      if (this.dlgLoadItems.ShowDialog(this) == DialogResult.OK)
	this.LoadItems(this.dlgLoadItems.FileName, this.ieRight);
    }

    private void btnPrevious_Click(object sender, System.EventArgs e) {
      --this.CurrentItem;
      this.EnableNavigation();
      this.MarkItemChanges();
    }

    private void btnNext_Click(object sender, System.EventArgs e) {
      ++this.CurrentItem;
      this.EnableNavigation();
      this.MarkItemChanges();
    }

    private void btnRemoveUnchanged_Click(object sender, System.EventArgs e) {
      this.btnRemoveUnchanged.Enabled = false;
    Thread T = new Thread(new ThreadStart(this.TRemoveUnchanged));
      T.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
      T.Start();
      Application.DoEvents();
      this.LeftItemsShown  = null;
      this.RightItemsShown = null;
    ArrayList LIS = new ArrayList();
    ArrayList RIS = new ArrayList();
      for (int i = 0; i < this.LeftItems.Length && i < this.RightItems.Length; ++i) {
      bool DifferenceSeen = false;
	if (this.GetIconString(this.LeftItems[i]) != this.GetIconString(this.RightItems[i]))
	  DifferenceSeen = true;
	else {
	  foreach (ItemField IF in Enum.GetValues(typeof(ItemField))) {
	    if (this.LeftItems[i].GetInfo(this.LLanguage, this.LType).GetFieldText(IF)
		!= this.RightItems[i].GetInfo(this.RLanguage, this.RType).GetFieldText(IF)) {
	      DifferenceSeen = true;
	      break;
	    }
	  }
	}
	if (DifferenceSeen) {
	  LIS.Add(this.LeftItems[i]);
	  RIS.Add(this.RightItems[i]);
	}
	Application.DoEvents();
      }
      T.Abort();
      this.Activate();
      this.LeftItemsShown  = (FFXIItem[]) LIS.ToArray(typeof(FFXIItem));
      this.RightItemsShown = (FFXIItem[]) RIS.ToArray(typeof(FFXIItem));
      this.CurrentItem = ((LIS.Count == 0) ? -1 : 0);
      this.EnableNavigation();
      this.MarkItemChanges();
    }

    private void ItemViewerSizeChanged(object sender, System.EventArgs e) {
    int WantedHeight = this.StartupHeight + Math.Max(this.ieLeft.Height, this.ieRight.Height) + 4;
      if (this.Height < WantedHeight)
	this.Height = WantedHeight;
    }

    #endregion

  }

}
