using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using PlayOnline.Core;
using PlayOnline.FFXI;

namespace PlayOnline.Utils.FFXIDataBrowser {

  internal class ItemFieldChooser : System.Windows.Forms.Form {
  
    #region Controls

    private System.Windows.Forms.TreeView tvItemFields;
    private System.Windows.Forms.Button btnOK;

    private System.ComponentModel.Container components = null;

    #endregion

    public ItemDataType     T;
    public ItemDataLanguage L;
    public ItemField        F;

    public ItemFieldChooser() {
      InitializeComponent();
      foreach (NamedEnum NE in NamedEnum.GetAll(typeof(ItemDataLanguage))) {
      TreeNode TN = this.tvItemFields.Nodes.Add(NE.Name);
	TN.Tag = NE.Value;
	this.AddTypeTrees((ItemDataLanguage) NE.Value, TN);
      }
    }

    private void AddTypeTrees(ItemDataLanguage L, TreeNode Parent) {
      foreach (NamedEnum NE in NamedEnum.GetAll(typeof(ItemDataType))) {
      TreeNode TN = Parent.Nodes.Add(NE.Name);
	TN.Tag = NE.Value;
	this.AddFields(L, (ItemDataType) NE.Value, TN);
      }
    }

    private void AddFields(ItemDataLanguage L, ItemDataType T, TreeNode Parent) {
      this.AddField(ItemField.ID, Parent);
      this.AddField(ItemField.Flags, Parent);
      this.AddField(ItemField.StackSize, Parent);
      this.AddField(ItemField.Type, Parent);
      this.AddField(ItemField.EnglishName, Parent);
      this.AddField(ItemField.JapaneseName, Parent);
      if (L == ItemDataLanguage.English) {
	this.AddField(ItemField.LogNameSingular, Parent);
	this.AddField(ItemField.LogNamePlural, Parent);
      }
      this.AddField(ItemField.Description, Parent);
      if (T != ItemDataType.Object) {
	this.AddField(ItemField.ResourceID, Parent);
	this.AddField(ItemField.Level, Parent);
	this.AddField(ItemField.Slots, Parent);
	this.AddField(ItemField.Jobs, Parent);
	this.AddField(ItemField.Races, Parent);
	if (T == ItemDataType.Weapon) {
	  this.AddField(ItemField.Damage, Parent);
	  this.AddField(ItemField.Delay, Parent);
	  this.AddField(ItemField.Skill, Parent);
	}
	else
	  this.AddField(ItemField.ShieldSize, Parent);
	this.AddField(ItemField.MaxCharges, Parent);
	this.AddField(ItemField.EquipDelay, Parent);
	this.AddField(ItemField.ReuseTimer, Parent);
      }
    }

    private void AddField(ItemField F, TreeNode Parent) {
    NamedEnum NE = new NamedEnum(F);
    TreeNode TN = Parent.Nodes.Add(NE.Name);
      TN.Tag = NE.Value;
    }

    private void tvItemFields_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e) {
      this.btnOK.Enabled = (e.Node.Nodes.Count == 0);
      if (e.Node.Nodes.Count == 0) { // Leaf Node
	this.F = (ItemField) e.Node.Tag;
	this.T = (ItemDataType) e.Node.Parent.Tag;
	this.L = (ItemDataLanguage) e.Node.Parent.Parent.Tag;
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
      this.tvItemFields = new System.Windows.Forms.TreeView();
      this.btnOK = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // tvItemFields
      // 
      this.tvItemFields.AccessibleDescription = resources.GetString("tvItemFields.AccessibleDescription");
      this.tvItemFields.AccessibleName = resources.GetString("tvItemFields.AccessibleName");
      this.tvItemFields.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("tvItemFields.Anchor")));
      this.tvItemFields.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tvItemFields.BackgroundImage")));
      this.tvItemFields.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("tvItemFields.Dock")));
      this.tvItemFields.Enabled = ((bool)(resources.GetObject("tvItemFields.Enabled")));
      this.tvItemFields.Font = ((System.Drawing.Font)(resources.GetObject("tvItemFields.Font")));
      this.tvItemFields.ImageIndex = ((int)(resources.GetObject("tvItemFields.ImageIndex")));
      this.tvItemFields.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("tvItemFields.ImeMode")));
      this.tvItemFields.Indent = ((int)(resources.GetObject("tvItemFields.Indent")));
      this.tvItemFields.ItemHeight = ((int)(resources.GetObject("tvItemFields.ItemHeight")));
      this.tvItemFields.Location = ((System.Drawing.Point)(resources.GetObject("tvItemFields.Location")));
      this.tvItemFields.Name = "tvItemFields";
      this.tvItemFields.PathSeparator = ".";
      this.tvItemFields.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("tvItemFields.RightToLeft")));
      this.tvItemFields.SelectedImageIndex = ((int)(resources.GetObject("tvItemFields.SelectedImageIndex")));
      this.tvItemFields.Size = ((System.Drawing.Size)(resources.GetObject("tvItemFields.Size")));
      this.tvItemFields.TabIndex = ((int)(resources.GetObject("tvItemFields.TabIndex")));
      this.tvItemFields.Text = resources.GetString("tvItemFields.Text");
      this.tvItemFields.Visible = ((bool)(resources.GetObject("tvItemFields.Visible")));
      this.tvItemFields.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvItemFields_AfterSelect);
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
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.tvItemFields);
      this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
      this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
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

  }

}
