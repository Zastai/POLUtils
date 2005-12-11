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

namespace PlayOnline.FFXI.Utils.ItemComparison {

  public partial class MainWindow : Form {

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

    public MainWindow() {
      this.InitializeComponent();
      this.StartupHeight = this.Height;
      this.Icon = Icons.FileSearch;
      this.ieLeft.LockViewMode();
      this.ieRight.LockViewMode();
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
