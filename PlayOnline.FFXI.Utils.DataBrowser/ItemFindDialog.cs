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

  internal partial class ItemFindDialog : Form {

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
      this.ilItemIcons.Images.Add(Item.IconGraphic.GetIcon());
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
	    this.lstItems.Columns.Add(String.Format(I18N.GetText("ColumnHeader:LineCount"), FieldName), 120, HorizontalAlignment.Right);
	  LVI.SubItems.Add(DescriptionLines.Length.ToString());
	  for (int i = 0; i < 6; ++i) {
	    if (AddColumns)
	      this.lstItems.Columns.Add(String.Format(I18N.GetText("ColumnHeader:LineNumber"), FieldName, i + 1), 120, HorizontalAlignment.Left);
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
	this.lstItems.Columns.Add(I18N.GetText("ColumnHeader:IconInfo"), 80, HorizontalAlignment.Left);
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

    private void DoExport(FFXIItem[] Items) {
    IItemExporter IE = new ItemExporter(this.Language, this.Type);
      if (IE.PrepareExport()) {
      PleaseWaitDialog PWD = new PleaseWaitDialog(I18N.GetText("Dialog:ExportItems"));
      Thread T = new Thread(new ThreadStart(delegate () {
	  Application.DoEvents();
	  IE.DoExport(Items);
	  Application.DoEvents();
	  PWD.Close();
	}));
	T.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
	T.Start();
	PWD.ShowDialog(this);
	this.Activate();
	PWD.Dispose();
	PWD = null; 
      }
    }

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

    private void mnuILCECResults_Click(object sender, System.EventArgs e) {
    ArrayList Items = new ArrayList();
      foreach (ListViewItem LVI in this.lstItems.Items)
	Items.Add(LVI.Tag);
      this.DoExport((FFXIItem[]) Items.ToArray(typeof(FFXIItem)));
    }

    private void mnuILCECSelected_Click(object sender, System.EventArgs e) {
    ArrayList Items = new ArrayList();
      foreach (ListViewItem LVI in this.lstItems.SelectedItems)
	Items.Add(LVI.Tag);
      this.DoExport((FFXIItem[]) Items.ToArray(typeof(FFXIItem)));
    }

    #endregion

  }

}
