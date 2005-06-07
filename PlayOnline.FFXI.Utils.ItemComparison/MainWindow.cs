using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;

using PlayOnline.Core;

namespace PlayOnline.FFXI.Utils.ItemComparison {

  public class MainWindow : System.Windows.Forms.Form {

    private FFXIItem[] LeftItems;
    private FFXIItem[] RightItems;

    private int CurrentItem = -1;

    #region Controls

    private PlayOnline.FFXI.FFXIItemEditor ieLeft;
    private PlayOnline.FFXI.FFXIItemEditor ieRight;
    private System.Windows.Forms.Button btnLoadItemsLeft;
    private System.Windows.Forms.Button btnLoadItemsRight;
    private System.Windows.Forms.OpenFileDialog dlgLoadItems;
    private System.Windows.Forms.Button btnBack;
    private System.Windows.Forms.Button btnForward;

    private System.ComponentModel.Container components = null;

    #endregion

    public MainWindow() {
      this.InitializeComponent();
      this.EnableNavigation();
    }

    private void CompareItems() {
      this.ieLeft.Item = null;
      this.ieRight.Item = null;
      if (this.CurrentItem < 0)
	return;
      if (this.LeftItems != null && this.LeftItems.Length > this.CurrentItem)
	this.ieLeft.Item = this.LeftItems[this.CurrentItem];
      if (this.RightItems != null && this.RightItems.Length > this.CurrentItem)
	this.ieRight.Item = this.RightItems[this.CurrentItem];
      if (this.ieLeft.Item != null && this.ieRight.Item != null) {
	// TODO: Mark fields that differ
      }
    }

    private FFXIItem[] LoadItems(string FileName, FFXIItemEditor IE) {
    ArrayList LoadedItems = new ArrayList();
      try {
      XmlDocument XD = new XmlDocument();
	XD.Load(FileName);
	if (XD.DocumentElement.Name == "ffxi-item-info") {
	int Index = 0;
	  {
	  XmlNode XLang = XD.DocumentElement.SelectSingleNode("data-language");
	  XmlNode XType = XD.DocumentElement.SelectSingleNode("data-type");
	    if (XLang != null && XType != null) try {
	      IE.LockViewMode((ItemDataLanguage) Enum.Parse(typeof(ItemDataLanguage), XLang.InnerText),
			      (ItemDataType) Enum.Parse(typeof(ItemDataType), XType.InnerText));
	    } catch { }
	  }
	  foreach (XmlNode XN in XD.DocumentElement.ChildNodes) {
	    if (XN is XmlElement && XN.Name == "item")
	      LoadedItems.Add(new FFXIItem(Index++, XN as XmlElement));
	  }
	}
      } catch { }
      if (LoadedItems.Count == 0)
	return null;
      return (FFXIItem[]) LoadedItems.ToArray(typeof(FFXIItem));
    }

    private void EnableNavigation() {
      this.btnBack.Enabled = (this.CurrentItem > 0);
      this.btnForward.SuspendLayout();
      this.btnForward.Enabled = false;
      if (this.LeftItems != null && this.CurrentItem < (this.LeftItems.Length - 1))
	this.btnForward.Enabled = true;
      if (this.RightItems != null && this.CurrentItem < (this.RightItems.Length - 1))
	this.btnForward.Enabled = true;
      this.btnForward.ResumeLayout(true);
    }

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
	components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainWindow));
      this.ieLeft = new PlayOnline.FFXI.FFXIItemEditor();
      this.ieRight = new PlayOnline.FFXI.FFXIItemEditor();
      this.btnLoadItemsLeft = new System.Windows.Forms.Button();
      this.btnLoadItemsRight = new System.Windows.Forms.Button();
      this.dlgLoadItems = new System.Windows.Forms.OpenFileDialog();
      this.btnBack = new System.Windows.Forms.Button();
      this.btnForward = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // ieLeft
      // 
      this.ieLeft.AccessibleDescription = resources.GetString("ieLeft.AccessibleDescription");
      this.ieLeft.AccessibleName = resources.GetString("ieLeft.AccessibleName");
      this.ieLeft.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("ieLeft.Anchor")));
      this.ieLeft.AutoScroll = ((bool)(resources.GetObject("ieLeft.AutoScroll")));
      this.ieLeft.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("ieLeft.AutoScrollMargin")));
      this.ieLeft.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("ieLeft.AutoScrollMinSize")));
      this.ieLeft.BackColor = System.Drawing.SystemColors.Control;
      this.ieLeft.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ieLeft.BackgroundImage")));
      this.ieLeft.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("ieLeft.Dock")));
      this.ieLeft.Enabled = ((bool)(resources.GetObject("ieLeft.Enabled")));
      this.ieLeft.Font = ((System.Drawing.Font)(resources.GetObject("ieLeft.Font")));
      this.ieLeft.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("ieLeft.ImeMode")));
      this.ieLeft.Item = null;
      this.ieLeft.Location = ((System.Drawing.Point)(resources.GetObject("ieLeft.Location")));
      this.ieLeft.Name = "ieLeft";
      this.ieLeft.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("ieLeft.RightToLeft")));
      this.ieLeft.Size = ((System.Drawing.Size)(resources.GetObject("ieLeft.Size")));
      this.ieLeft.TabIndex = ((int)(resources.GetObject("ieLeft.TabIndex")));
      this.ieLeft.Visible = ((bool)(resources.GetObject("ieLeft.Visible")));
      // 
      // ieRight
      // 
      this.ieRight.AccessibleDescription = resources.GetString("ieRight.AccessibleDescription");
      this.ieRight.AccessibleName = resources.GetString("ieRight.AccessibleName");
      this.ieRight.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("ieRight.Anchor")));
      this.ieRight.AutoScroll = ((bool)(resources.GetObject("ieRight.AutoScroll")));
      this.ieRight.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("ieRight.AutoScrollMargin")));
      this.ieRight.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("ieRight.AutoScrollMinSize")));
      this.ieRight.BackColor = System.Drawing.SystemColors.Control;
      this.ieRight.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ieRight.BackgroundImage")));
      this.ieRight.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("ieRight.Dock")));
      this.ieRight.Enabled = ((bool)(resources.GetObject("ieRight.Enabled")));
      this.ieRight.Font = ((System.Drawing.Font)(resources.GetObject("ieRight.Font")));
      this.ieRight.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("ieRight.ImeMode")));
      this.ieRight.Item = null;
      this.ieRight.Location = ((System.Drawing.Point)(resources.GetObject("ieRight.Location")));
      this.ieRight.Name = "ieRight";
      this.ieRight.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("ieRight.RightToLeft")));
      this.ieRight.Size = ((System.Drawing.Size)(resources.GetObject("ieRight.Size")));
      this.ieRight.TabIndex = ((int)(resources.GetObject("ieRight.TabIndex")));
      this.ieRight.Visible = ((bool)(resources.GetObject("ieRight.Visible")));
      // 
      // btnLoadItemsLeft
      // 
      this.btnLoadItemsLeft.AccessibleDescription = resources.GetString("btnLoadItemsLeft.AccessibleDescription");
      this.btnLoadItemsLeft.AccessibleName = resources.GetString("btnLoadItemsLeft.AccessibleName");
      this.btnLoadItemsLeft.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnLoadItemsLeft.Anchor")));
      this.btnLoadItemsLeft.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnLoadItemsLeft.BackgroundImage")));
      this.btnLoadItemsLeft.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnLoadItemsLeft.Dock")));
      this.btnLoadItemsLeft.Enabled = ((bool)(resources.GetObject("btnLoadItemsLeft.Enabled")));
      this.btnLoadItemsLeft.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnLoadItemsLeft.FlatStyle")));
      this.btnLoadItemsLeft.Font = ((System.Drawing.Font)(resources.GetObject("btnLoadItemsLeft.Font")));
      this.btnLoadItemsLeft.Image = ((System.Drawing.Image)(resources.GetObject("btnLoadItemsLeft.Image")));
      this.btnLoadItemsLeft.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnLoadItemsLeft.ImageAlign")));
      this.btnLoadItemsLeft.ImageIndex = ((int)(resources.GetObject("btnLoadItemsLeft.ImageIndex")));
      this.btnLoadItemsLeft.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnLoadItemsLeft.ImeMode")));
      this.btnLoadItemsLeft.Location = ((System.Drawing.Point)(resources.GetObject("btnLoadItemsLeft.Location")));
      this.btnLoadItemsLeft.Name = "btnLoadItemsLeft";
      this.btnLoadItemsLeft.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnLoadItemsLeft.RightToLeft")));
      this.btnLoadItemsLeft.Size = ((System.Drawing.Size)(resources.GetObject("btnLoadItemsLeft.Size")));
      this.btnLoadItemsLeft.TabIndex = ((int)(resources.GetObject("btnLoadItemsLeft.TabIndex")));
      this.btnLoadItemsLeft.Text = resources.GetString("btnLoadItemsLeft.Text");
      this.btnLoadItemsLeft.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnLoadItemsLeft.TextAlign")));
      this.btnLoadItemsLeft.Visible = ((bool)(resources.GetObject("btnLoadItemsLeft.Visible")));
      this.btnLoadItemsLeft.Click += new System.EventHandler(this.btnLoadItemsLeft_Click);
      // 
      // btnLoadItemsRight
      // 
      this.btnLoadItemsRight.AccessibleDescription = resources.GetString("btnLoadItemsRight.AccessibleDescription");
      this.btnLoadItemsRight.AccessibleName = resources.GetString("btnLoadItemsRight.AccessibleName");
      this.btnLoadItemsRight.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnLoadItemsRight.Anchor")));
      this.btnLoadItemsRight.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnLoadItemsRight.BackgroundImage")));
      this.btnLoadItemsRight.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnLoadItemsRight.Dock")));
      this.btnLoadItemsRight.Enabled = ((bool)(resources.GetObject("btnLoadItemsRight.Enabled")));
      this.btnLoadItemsRight.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnLoadItemsRight.FlatStyle")));
      this.btnLoadItemsRight.Font = ((System.Drawing.Font)(resources.GetObject("btnLoadItemsRight.Font")));
      this.btnLoadItemsRight.Image = ((System.Drawing.Image)(resources.GetObject("btnLoadItemsRight.Image")));
      this.btnLoadItemsRight.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnLoadItemsRight.ImageAlign")));
      this.btnLoadItemsRight.ImageIndex = ((int)(resources.GetObject("btnLoadItemsRight.ImageIndex")));
      this.btnLoadItemsRight.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnLoadItemsRight.ImeMode")));
      this.btnLoadItemsRight.Location = ((System.Drawing.Point)(resources.GetObject("btnLoadItemsRight.Location")));
      this.btnLoadItemsRight.Name = "btnLoadItemsRight";
      this.btnLoadItemsRight.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnLoadItemsRight.RightToLeft")));
      this.btnLoadItemsRight.Size = ((System.Drawing.Size)(resources.GetObject("btnLoadItemsRight.Size")));
      this.btnLoadItemsRight.TabIndex = ((int)(resources.GetObject("btnLoadItemsRight.TabIndex")));
      this.btnLoadItemsRight.Text = resources.GetString("btnLoadItemsRight.Text");
      this.btnLoadItemsRight.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnLoadItemsRight.TextAlign")));
      this.btnLoadItemsRight.Visible = ((bool)(resources.GetObject("btnLoadItemsRight.Visible")));
      this.btnLoadItemsRight.Click += new System.EventHandler(this.btnLoadItemsRight_Click);
      // 
      // dlgLoadItems
      // 
      this.dlgLoadItems.Filter = resources.GetString("dlgLoadItems.Filter");
      this.dlgLoadItems.Title = resources.GetString("dlgLoadItems.Title");
      // 
      // btnBack
      // 
      this.btnBack.AccessibleDescription = resources.GetString("btnBack.AccessibleDescription");
      this.btnBack.AccessibleName = resources.GetString("btnBack.AccessibleName");
      this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnBack.Anchor")));
      this.btnBack.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBack.BackgroundImage")));
      this.btnBack.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnBack.Dock")));
      this.btnBack.Enabled = ((bool)(resources.GetObject("btnBack.Enabled")));
      this.btnBack.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnBack.FlatStyle")));
      this.btnBack.Font = ((System.Drawing.Font)(resources.GetObject("btnBack.Font")));
      this.btnBack.Image = ((System.Drawing.Image)(resources.GetObject("btnBack.Image")));
      this.btnBack.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBack.ImageAlign")));
      this.btnBack.ImageIndex = ((int)(resources.GetObject("btnBack.ImageIndex")));
      this.btnBack.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnBack.ImeMode")));
      this.btnBack.Location = ((System.Drawing.Point)(resources.GetObject("btnBack.Location")));
      this.btnBack.Name = "btnBack";
      this.btnBack.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnBack.RightToLeft")));
      this.btnBack.Size = ((System.Drawing.Size)(resources.GetObject("btnBack.Size")));
      this.btnBack.TabIndex = ((int)(resources.GetObject("btnBack.TabIndex")));
      this.btnBack.Text = resources.GetString("btnBack.Text");
      this.btnBack.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBack.TextAlign")));
      this.btnBack.Visible = ((bool)(resources.GetObject("btnBack.Visible")));
      this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
      // 
      // btnForward
      // 
      this.btnForward.AccessibleDescription = resources.GetString("btnForward.AccessibleDescription");
      this.btnForward.AccessibleName = resources.GetString("btnForward.AccessibleName");
      this.btnForward.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnForward.Anchor")));
      this.btnForward.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnForward.BackgroundImage")));
      this.btnForward.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnForward.Dock")));
      this.btnForward.Enabled = ((bool)(resources.GetObject("btnForward.Enabled")));
      this.btnForward.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnForward.FlatStyle")));
      this.btnForward.Font = ((System.Drawing.Font)(resources.GetObject("btnForward.Font")));
      this.btnForward.Image = ((System.Drawing.Image)(resources.GetObject("btnForward.Image")));
      this.btnForward.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnForward.ImageAlign")));
      this.btnForward.ImageIndex = ((int)(resources.GetObject("btnForward.ImageIndex")));
      this.btnForward.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnForward.ImeMode")));
      this.btnForward.Location = ((System.Drawing.Point)(resources.GetObject("btnForward.Location")));
      this.btnForward.Name = "btnForward";
      this.btnForward.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnForward.RightToLeft")));
      this.btnForward.Size = ((System.Drawing.Size)(resources.GetObject("btnForward.Size")));
      this.btnForward.TabIndex = ((int)(resources.GetObject("btnForward.TabIndex")));
      this.btnForward.Text = resources.GetString("btnForward.Text");
      this.btnForward.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnForward.TextAlign")));
      this.btnForward.Visible = ((bool)(resources.GetObject("btnForward.Visible")));
      this.btnForward.Click += new System.EventHandler(this.btnForward_Click);
      // 
      // MainWindow
      // 
      this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
      this.AccessibleName = resources.GetString("$this.AccessibleName");
      this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
      this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
      this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
      this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
      this.BackColor = System.Drawing.SystemColors.Control;
      this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
      this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
      this.Controls.Add(this.btnForward);
      this.Controls.Add(this.btnBack);
      this.Controls.Add(this.btnLoadItemsRight);
      this.Controls.Add(this.btnLoadItemsLeft);
      this.Controls.Add(this.ieLeft);
      this.Controls.Add(this.ieRight);
      this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
      this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
      this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
      this.MaximizeBox = false;
      this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
      this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
      this.Name = "MainWindow";
      this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
      this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
      this.Text = resources.GetString("$this.Text");
      this.ResumeLayout(false);

    }

    #endregion

    #region Event Handlers

    private void btnLoadItemsLeft_Click(object sender, System.EventArgs e) {
      if (this.dlgLoadItems.ShowDialog(this) == DialogResult.OK) {
	this.ieLeft.Item = null;
	this.ieLeft.Reset();
	this.LeftItems = this.LoadItems(this.dlgLoadItems.FileName, this.ieLeft);
	if (this.RightItems == null && this.LeftItems == null)
	  this.CurrentItem = -1;
	else if (this.CurrentItem < 0)
	  this.CurrentItem = 0;
	this.EnableNavigation();
	this.CompareItems();
      }
    }

    private void btnLoadItemsRight_Click(object sender, System.EventArgs e) {
      if (this.dlgLoadItems.ShowDialog(this) == DialogResult.OK) {
	this.ieRight.Item = null;
	this.ieRight.Reset();
	this.RightItems = this.LoadItems(this.dlgLoadItems.FileName, this.ieRight);
	if (this.RightItems == null && this.LeftItems == null)
	  this.CurrentItem = -1;
	else if (this.CurrentItem < 0)
	  this.CurrentItem = 0;
	this.EnableNavigation();
	this.CompareItems();
      }
    }

    private void btnBack_Click(object sender, System.EventArgs e) {
      --this.CurrentItem;
      this.EnableNavigation();
      this.CompareItems();
    }

    private void btnForward_Click(object sender, System.EventArgs e) {
      ++this.CurrentItem;
      this.EnableNavigation();
      this.CompareItems();
    }

    #endregion

  }

}
