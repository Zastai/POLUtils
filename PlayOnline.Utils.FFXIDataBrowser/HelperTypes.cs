using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using PlayOnline.Core;
using PlayOnline.FFXI;

namespace PlayOnline.Utils.FFXIDataBrowser {

  internal class FFXIItem {
  
    public long        Number;
    public byte[]      Data;
    public FFXIGraphic Image;

    public FFXIItem(long Number) {
      this.Number = Number;
      this.Data   = null;
      this.Image  = null;
    }

    public override string ToString() { return this.Number.ToString(); }

  }

  internal class ListViewColumnSorter : IComparer {

    public int       Column  = 0;
    public bool      Numeric = false;
    public SortOrder Order   = SortOrder.Ascending;

    public int Compare(object x, object y) {
      if (this.Order == SortOrder.None)
	return 0;
    ListViewItem LVI1 = x as ListViewItem;
    ListViewItem LVI2 = y as ListViewItem;
      if (LVI1 == null || LVI2 == null || LVI1.SubItems.Count <= this.Column || LVI2.SubItems.Count <= this.Column)
	return 0;
    int result = 0;
      if (this.Numeric) { // File ID -> compare as numbers
	try {
	double L1 = double.Parse(LVI1.SubItems[Column].Text);
	double L2 = double.Parse(LVI2.SubItems[Column].Text);
	  result = L1.CompareTo(L2);
	} catch { }
      }
      else
	result = LVI1.SubItems[this.Column].Text.CompareTo(LVI2.SubItems[this.Column].Text);
      if (this.Order == SortOrder.Descending)
	result *= -1;
      return result;
    }

    public static void ListView_ColumnClick(object sender, System.Windows.Forms.ColumnClickEventArgs e) {
    ListView LV = sender as ListView;
      if (LV == null)
	return;
    ListViewColumnSorter S = LV.ListViewItemSorter as ListViewColumnSorter;
      if (S == null) {
	S = new ListViewColumnSorter();
	LV.ListViewItemSorter = S;
	S.Column = -1;
      }
      if (S.Column == e.Column)
	S.Order = ((S.Order == SortOrder.Ascending) ? SortOrder.Descending : SortOrder.Ascending);
      else {
	S.Column = e.Column;
	S.Order = SortOrder.Ascending;
      }
      LV.Sort();
    }

  }

}
