// $Id$

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml;

using PlayOnline.Core;
using PlayOnline.FFXI;
using PlayOnline.FFXI.Things;

namespace PlayOnline.FFXI.Utils.ItemComparison {

  public partial class MainWindow : Form {

    private ThingList<Item> LeftItems;
    private ThingList<Item> LeftItemsShown;
    private ThingList<Item> RightItems;
    private ThingList<Item> RightItemsShown;

    private int CurrentItem   = -1;
    private int StartupHeight = -1;

    public MainWindow() {
      this.InitializeComponent();
      this.StartupHeight = this.Height;
      this.Icon = Icons.FileSearch;
      this.EnableNavigation();
    }

    // If possible, give the window that nice gradient look
    protected override void OnPaintBackground(PaintEventArgs e) {
      if (VisualStyleRenderer.IsSupported) {
      VisualStyleRenderer VSR = new VisualStyleRenderer(VisualStyleElement.Tab.Body.Normal);
	VSR.DrawBackground(e.Graphics, this.ClientRectangle, e.ClipRectangle);
      }
      else
	base.OnPaintBackground(e);
    }

    private PleaseWaitDialog PWD = null;

    private OpenFileDialog dlgLoadItems1 = null;
    private OpenFileDialog dlgLoadItems2 = null;

    #region Item Loading & Duplicate Removal

    private void LoadItemsWorker(string FileName, ItemEditor IE) {
    ThingList<Item> TL = new ThingList<Item>();
      if (TL.Load(FileName)) {
	if (IE == this.ieLeft)
	  this.LeftItems = TL;
	else
	  this.RightItems = TL;
      }
      this.LeftItemsShown  = null;
      this.RightItemsShown = null;
      if (this.RightItems == null && this.LeftItems == null)
	this.CurrentItem = -1;
      else
	this.CurrentItem = 0;
      if (this.RightItems != null && this.LeftItems != null)
	this.btnRemoveUnchanged.Invoke(new AnonymousMethod(delegate () { this.btnRemoveUnchanged.Enabled = true; }));
      this.PWD.Invoke(new AnonymousMethod(delegate() { this.PWD.Close(); }));
    }

    private void LoadItems(string FileName, ItemEditor IE) {
      this.PWD = new PleaseWaitDialog(I18N.GetText("Dialog:LoadItems"));
    Thread T = new Thread(new ThreadStart(delegate() { this.LoadItemsWorker(FileName, IE); }));
      T.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
      T.Start();
      PWD.ShowDialog(this);
      this.Activate();
      this.PWD.Dispose();
      this.PWD = null;
      this.EnableNavigation();
      this.MarkItemChanges();
    }

    private void RemoveUnchangedItemsWorker() {
      Application.DoEvents();
      this.LeftItemsShown  = new ThingList<Item>();
      this.RightItemsShown = new ThingList<Item>();
      for (int i = 0; i < this.LeftItems.Count && i < this.RightItems.Count; ++i) {
      bool DifferenceSeen = false;
	if (this.GetIconString(this.LeftItems[i]) != this.GetIconString(this.RightItems[i]))
	  DifferenceSeen = true;
	else {
	  foreach (string Field in Item.AllFields) {
	    if (this.LeftItems[i].GetFieldText(Field) != this.RightItems[i].GetFieldText(Field)) {
	      DifferenceSeen = true;
	      break;
	    }
	  }
	}
	if (DifferenceSeen) {
	  this.LeftItemsShown.Add(this.LeftItems[i]);
	  this.RightItemsShown.Add(this.RightItems[i]);
	}
	Application.DoEvents();
      }
      // All non-dummy overflow items are "changed"
      if (this.LeftItems.Count < this.RightItems.Count) {
      int OverflowPos = this.LeftItems.Count;
	while (OverflowPos < this.RightItems.Count) {
	Item I = this.RightItems[OverflowPos++];
	  if (I.GetFieldText("english-name") == String.Empty || I.GetFieldText("english-name") == ".")
	    continue;
	  this.RightItemsShown.Add(I);
	}
      }
      else if (this.LeftItems.Count > this.RightItems.Count) {
      int OverflowPos = this.RightItems.Count;
	while (OverflowPos < this.LeftItems.Count) {
	Item I = this.LeftItems[OverflowPos++];
	  if (I.GetFieldText("english-name") == String.Empty || I.GetFieldText("english-name") == ".")
	    continue;
	  this.LeftItemsShown.Add(I);
	}
      }
      this.CurrentItem = ((this.LeftItemsShown.Count == 0) ? -1 : 0);
      this.PWD.Invoke(new AnonymousMethod(delegate() { this.PWD.Close(); }));
    }

    private void RemoveUnchangedItems() {
      this.btnRemoveUnchanged.Enabled = false;
      this.PWD = new PleaseWaitDialog(I18N.GetText("Dialog:RemoveUnchanged"));
    Thread T = new Thread(new ThreadStart(delegate() { this.RemoveUnchangedItemsWorker(); }));
      T.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
      T.Start();
      PWD.ShowDialog(this);
      this.Activate();
      this.PWD.Dispose();
      this.PWD = null;
      this.EnableNavigation();
      this.MarkItemChanges();
    }

    #endregion

    #region Item Display

    private string GetIconString(Item I) {
    string IconString = I.GetFieldText("icon");
    Image Icon = I.GetIcon();
      if (Icon != null) {
      MemoryStream MS = new MemoryStream();
	Icon.Save(MS, ImageFormat.Png);
	IconString += Convert.ToBase64String(MS.GetBuffer());
	MS.Close();
      }
      return IconString;
    }

    private void MarkItemChanges() {
      if (this.ieLeft.Item != null && this.ieRight.Item != null) {
	// Compare fields
	foreach (string Field in Item.AllFields) {
	bool FieldChanged = (this.ieLeft.Item.GetFieldText(Field) != this.ieRight.Item.GetFieldText(Field));
	  this.ieLeft.MarkField (Field, FieldChanged);
	  this.ieRight.MarkField(Field, FieldChanged);
	}
	{ // Compare icon
	bool IconChanged = (this.GetIconString(this.ieLeft.Item) != this.GetIconString(this.ieRight.Item));
	  this.ieLeft.MarkField ("icon", IconChanged);
	  this.ieRight.MarkField("icon", IconChanged);
	}
      }
    }

    private void EnableNavigation() {
      this.ieLeft.Item = null;
      this.ieRight.Item = null;
      this.btnPrevious.Enabled = (this.CurrentItem > 0);
      this.btnNext.Enabled = false;
    Item LeftItem  = null;
    Item RightItem = null;
      if (this.CurrentItem >= 0) {
	if (this.LeftItemsShown != null) {
	  if (this.CurrentItem < this.LeftItemsShown.Count)
	    LeftItem = this.LeftItemsShown[this.CurrentItem];
	  if (this.CurrentItem < this.LeftItemsShown.Count - 1)
	    this.btnNext.Enabled = true;
	}
	else if (this.LeftItems != null) {
	  if (this.CurrentItem < this.LeftItems.Count)
	    LeftItem = this.LeftItems[this.CurrentItem];
	  if (this.CurrentItem < this.LeftItems.Count - 1)
	    this.btnNext.Enabled = true;
	}
	if (this.RightItemsShown != null) {
	  if (this.CurrentItem < this.RightItemsShown.Count)
	    RightItem = this.RightItemsShown[this.CurrentItem];
	  if (this.CurrentItem < this.RightItemsShown.Count - 1)
	    this.btnNext.Enabled = true;
	}
	else if (this.RightItems != null) {
	  if (this.CurrentItem < this.RightItems.Count)
	    RightItem = this.RightItems[this.CurrentItem];
	  if (this.CurrentItem < this.RightItems.Count - 1)
	    this.btnNext.Enabled = true;
	}
      }
      else
	this.btnNext.Enabled = false;
      this.ieLeft.Item  = LeftItem;
      this.ieRight.Item = RightItem;
    }

    #endregion

    #region Event Handlers

    private OpenFileDialog CreateLoadItemsDialog() {
    OpenFileDialog OFD = new OpenFileDialog();
      OFD.Title  = I18N.GetText("OpenDialog:Title");
      OFD.Filter = I18N.GetText("OpenDialog:Filter");
      return OFD;
    }

    private void btnLoadItemSet1_Click(object sender, System.EventArgs e) {
      if (this.dlgLoadItems1 == null)
	this.dlgLoadItems1 = this.CreateLoadItemsDialog();
      if (this.dlgLoadItems1.ShowDialog(this) == DialogResult.OK)
	this.LoadItems(this.dlgLoadItems1.FileName, this.ieLeft);
    }

    private void btnLoadItemSet2_Click(object sender, System.EventArgs e) {
      if (this.dlgLoadItems2 == null)
	this.dlgLoadItems2 = this.CreateLoadItemsDialog();
      if (this.dlgLoadItems2.ShowDialog(this) == DialogResult.OK)
	this.LoadItems(this.dlgLoadItems2.FileName, this.ieRight);
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
      this.RemoveUnchangedItems();
    }

    private void ItemViewerSizeChanged(object sender, System.EventArgs e) {
    int WantedHeight = this.StartupHeight + Math.Max(this.ieLeft.Height, this.ieRight.Height) + 4;
      if (this.Height < WantedHeight)
	this.Height = WantedHeight;
    }

    #endregion

  }

}
