// $Id$

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using PlayOnline.Core;

namespace PlayOnline.FFXI.Utils.DataBrowser {

  internal class ItemFieldChooser : System.Windows.Forms.Form {
  
    #region Controls

    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.CheckedListBox clstFields;

    private System.ComponentModel.Container components = null;

    #endregion

    public ItemFieldChooser(ItemDataLanguage L, ItemDataType T, bool IncludeAnyAndAll, params ItemField[] FieldParams) {
      this.InitializeComponent();
      // Set up fields - this should really be done via an IItemInfo, but that requires an item instance.
    ArrayList FieldsToCheck = new ArrayList(FieldParams);
      if (IncludeAnyAndAll) {
	this.AddField(ItemField.Any, FieldsToCheck);
	this.AddField(ItemField.All, FieldsToCheck);
      }
      this.AddField(ItemField.ID, FieldsToCheck);
      this.AddField(ItemField.Flags, FieldsToCheck);
      this.AddField(ItemField.StackSize, FieldsToCheck);
      this.AddField(ItemField.Type, FieldsToCheck);
      this.AddField(ItemField.EnglishName, FieldsToCheck);
      this.AddField(ItemField.JapaneseName, FieldsToCheck);
      if (L == ItemDataLanguage.English) {
	this.AddField(ItemField.LogNameSingular, FieldsToCheck);
	this.AddField(ItemField.LogNamePlural, FieldsToCheck);
      }
      this.AddField(ItemField.Description, FieldsToCheck);
      if (T != ItemDataType.Object) {
	this.AddField(ItemField.ResourceID, FieldsToCheck);
	this.AddField(ItemField.Level, FieldsToCheck);
	this.AddField(ItemField.Slots, FieldsToCheck);
	this.AddField(ItemField.Jobs, FieldsToCheck);
	this.AddField(ItemField.Races, FieldsToCheck);
	if (T == ItemDataType.Weapon) {
	  this.AddField(ItemField.Damage, FieldsToCheck);
	  this.AddField(ItemField.Delay, FieldsToCheck);
	  this.AddField(ItemField.Skill, FieldsToCheck);
	}
	else
	  this.AddField(ItemField.ShieldSize, FieldsToCheck);
	this.AddField(ItemField.MaxCharges, FieldsToCheck);
	this.AddField(ItemField.EquipDelay, FieldsToCheck);
	this.AddField(ItemField.ReuseTimer, FieldsToCheck);
      }
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

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
	components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ItemFieldChooser));
      this.btnOK = new System.Windows.Forms.Button();
      this.clstFields = new System.Windows.Forms.CheckedListBox();
      this.SuspendLayout();
      // 
      // btnOK
      // 
      this.btnOK.AccessibleDescription = resources.GetString("btnOK.AccessibleDescription");
      this.btnOK.AccessibleName = resources.GetString("btnOK.AccessibleName");
      this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnOK.Anchor")));
      this.btnOK.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnOK.BackgroundImage")));
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOK.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnOK.Dock")));
      this.btnOK.Enabled = ((bool)(resources.GetObject("btnOK.Enabled")));
      this.btnOK.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnOK.FlatStyle")));
      this.btnOK.Font = ((System.Drawing.Font)(resources.GetObject("btnOK.Font")));
      this.btnOK.Image = ((System.Drawing.Image)(resources.GetObject("btnOK.Image")));
      this.btnOK.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnOK.ImageAlign")));
      this.btnOK.ImageIndex = ((int)(resources.GetObject("btnOK.ImageIndex")));
      this.btnOK.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnOK.ImeMode")));
      this.btnOK.Location = ((System.Drawing.Point)(resources.GetObject("btnOK.Location")));
      this.btnOK.Name = "btnOK";
      this.btnOK.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnOK.RightToLeft")));
      this.btnOK.Size = ((System.Drawing.Size)(resources.GetObject("btnOK.Size")));
      this.btnOK.TabIndex = ((int)(resources.GetObject("btnOK.TabIndex")));
      this.btnOK.Text = resources.GetString("btnOK.Text");
      this.btnOK.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnOK.TextAlign")));
      this.btnOK.Visible = ((bool)(resources.GetObject("btnOK.Visible")));
      // 
      // clstFields
      // 
      this.clstFields.AccessibleDescription = resources.GetString("clstFields.AccessibleDescription");
      this.clstFields.AccessibleName = resources.GetString("clstFields.AccessibleName");
      this.clstFields.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("clstFields.Anchor")));
      this.clstFields.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("clstFields.BackgroundImage")));
      this.clstFields.CheckOnClick = true;
      this.clstFields.ColumnWidth = ((int)(resources.GetObject("clstFields.ColumnWidth")));
      this.clstFields.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("clstFields.Dock")));
      this.clstFields.Enabled = ((bool)(resources.GetObject("clstFields.Enabled")));
      this.clstFields.Font = ((System.Drawing.Font)(resources.GetObject("clstFields.Font")));
      this.clstFields.HorizontalExtent = ((int)(resources.GetObject("clstFields.HorizontalExtent")));
      this.clstFields.HorizontalScrollbar = ((bool)(resources.GetObject("clstFields.HorizontalScrollbar")));
      this.clstFields.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("clstFields.ImeMode")));
      this.clstFields.IntegralHeight = ((bool)(resources.GetObject("clstFields.IntegralHeight")));
      this.clstFields.Location = ((System.Drawing.Point)(resources.GetObject("clstFields.Location")));
      this.clstFields.Name = "clstFields";
      this.clstFields.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("clstFields.RightToLeft")));
      this.clstFields.ScrollAlwaysVisible = ((bool)(resources.GetObject("clstFields.ScrollAlwaysVisible")));
      this.clstFields.Size = ((System.Drawing.Size)(resources.GetObject("clstFields.Size")));
      this.clstFields.TabIndex = ((int)(resources.GetObject("clstFields.TabIndex")));
      this.clstFields.Visible = ((bool)(resources.GetObject("clstFields.Visible")));
      this.clstFields.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clstFields_ItemCheck);
      // 
      // ItemFieldChooser
      // 
      this.AcceptButton = this.btnOK;
      this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
      this.AccessibleName = resources.GetString("$this.AccessibleName");
      this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
      this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
      this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
      this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
      this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
      this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
      this.Controls.Add(this.clstFields);
      this.Controls.Add(this.btnOK);
      this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
      this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
      this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
      this.MaximizeBox = false;
      this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
      this.MinimizeBox = false;
      this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
      this.Name = "ItemFieldChooser";
      this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
      this.ShowInTaskbar = false;
      this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
      this.Text = resources.GetString("$this.Text");
      this.ResumeLayout(false);

    }

    #endregion

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
