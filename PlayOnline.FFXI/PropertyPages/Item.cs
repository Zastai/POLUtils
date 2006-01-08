// $Id$

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace PlayOnline.FFXI.PropertyPages {

  public partial class Item : UserControl {

    public Item(FFXI.Item I) {
      InitializeComponent();
    }

    public static List<TabPage> GetPages(FFXI.Item I) {
    List<TabPage> Pages = new List<TabPage>();
    Item PageControl = new Item(I);
      while (PageControl.tabPages.TabPages.Count > 0) {
	Pages.Add(PageControl.tabPages.TabPages[0]);
	PageControl.tabPages.TabPages.RemoveAt(0);
      }
      PageControl.Dispose();
      return Pages;
    }

  }

}
