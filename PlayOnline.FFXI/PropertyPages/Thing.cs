// $Id$

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using PlayOnline.Core;

namespace PlayOnline.FFXI.PropertyPages {

  public partial class Thing : IThing {

    public Thing(FFXI.IThing T) {
      InitializeComponent();
      this.picIcon.Image = T.GetIcon();
      if (this.picIcon.Image == null)
	this.picIcon.Image = Icons.POLViewer.ToBitmap();
      this.lblText.Text = T.ToString();
      this.lblTypeName.Text = T.TypeName;
      this.AddFieldEntries(T);
    }

    private void AddFieldEntries(FFXI.IThing T) {
    List<String> AllFields = T.GetAllFields();
      foreach (string FieldName in AllFields) {
      ListViewItem LVI = this.lstFields.Items.Add(FieldName);
	if (T.HasField(FieldName)) {
	object V = T.GetFieldValue(FieldName);
	  LVI.Tag = V;
	  LVI.SubItems.Add(T.GetFieldText(FieldName));
	  if (V is FFXI.IThing)
	    LVI.Font = new Font(LVI.Font, FontStyle.Underline);
	}
	else
	  LVI.ForeColor = SystemColors.GrayText;
      }
    }

    private void lstFields_ItemActivate(object sender, EventArgs e) {
      if (this.lstFields.SelectedItems != null && this.lstFields.SelectedItems.Count > 0) {
      FFXI.IThing T = this.lstFields.SelectedItems[0].Tag as FFXI.IThing;
	if (T != null) {
	  using (ThingPropertyPages TPP = new ThingPropertyPages(T))
	    TPP.ShowDialog(this);
	}
      }
    }

  }

}
