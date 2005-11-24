// $Id$

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using PlayOnline.Core;

namespace PlayOnline.FFXI.Utils.DataBrowser {

  internal partial class ItemFieldChooser : Form {

    public ItemFieldChooser(ItemDataLanguage L, ItemDataType T, bool IncludeAnyAndAll, params ItemField[] FieldParams) {
      this.InitializeComponent();
      // Set up fields - this should really be done via an IItemInfo, but that requires an item instance.
    ArrayList FieldsToCheck = new ArrayList(FieldParams);
      if (IncludeAnyAndAll) {
	this.AddField(ItemField.Any, FieldsToCheck);
	this.AddField(ItemField.All, FieldsToCheck);
      }
      foreach (ItemField IF in FFXIItem.GetFields(L, T))
	this.AddField(IF, FieldsToCheck);
    }

    private void AddField(ItemField Field, ArrayList FieldsToCheck) {
      this.clstFields.Items.Add(new NamedEnum(Field), FieldsToCheck.Contains(Field));
    }

    public ArrayList Fields {
      get {
      ArrayList Fields = new ArrayList();
	foreach (NamedEnum NE in this.clstFields.CheckedItems) {
	  if ((ItemField) NE.Value == ItemField.Any) {
	    Fields.Clear();
	    Fields.Add(ItemField.Any);
	    break;
	  }
	  else
	    Fields.Add(NE.Value);
	}
	return Fields;
      }
    }

    private void clstFields_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e) {
    NamedEnum NE = this.clstFields.Items[e.Index] as NamedEnum;
      if (NE != null && e.NewValue == CheckState.Checked) {
	switch ((ItemField) NE.Value) {
	  case ItemField.Any: case ItemField.All: // if selected, unselect all others
	    for (int i = 0; i < this.clstFields.Items.Count; ++i) {
	      if (i != e.Index)
		this.clstFields.SetItemChecked(i, false);
	    }
	    break;
	  default: // if selected, unselect any/all
	    for (int i = 0; i < this.clstFields.Items.Count; ++i) {
	    NamedEnum NE2 = this.clstFields.Items[i] as NamedEnum;
	      if (NE2 != null) {
		switch ((ItemField) NE2.Value) {
		  case ItemField.Any: case ItemField.All:
		    this.clstFields.SetItemChecked(i, false);
		    break;
		}
	      }
	    }
	    break;
	}
      }
      // Enable button as long as at least one entry selected
      this.btnOK.Enabled = !(this.clstFields.CheckedItems.Count <= 1 && e.NewValue != CheckState.Checked);
    }

  }

}
