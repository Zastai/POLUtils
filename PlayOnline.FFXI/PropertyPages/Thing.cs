// $Id$

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace PlayOnline.FFXI.PropertyPages {

  public partial class Thing : UserControl {

    public Thing(FFXI.Thing T) {
      InitializeComponent();
      this.picIcon.Image = T.GetIcon();
      this.lblText.Text = T.ToString();
      this.lblTypeName.Text = T.TypeName;
    List<String> AllFields = T.GetAllFields();
      foreach (string FieldName in AllFields) {
      ListViewItem LVI = this.lstFields.Items.Add(FieldName);
	if (T.HasField(FieldName))
	  LVI.SubItems.Add(T.GetFieldText(FieldName));
	else
	  LVI.ForeColor = SystemColors.GrayText;
      }
    }

    public static List<TabPage> GetPages(FFXI.Thing T) {
    List<TabPage> Pages = new List<TabPage>();
    Thing PageControl = new Thing(T);
      while (PageControl.tabPages.TabPages.Count > 0) {
	Pages.Add(PageControl.tabPages.TabPages[0]);
	PageControl.tabPages.TabPages.RemoveAt(0);
      }
      return Pages;
    }

  }

}
