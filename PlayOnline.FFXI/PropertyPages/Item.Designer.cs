// $Id$

namespace PlayOnline.FFXI.PropertyPages {

  partial class Item {

    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
	components.Dispose();
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    private void InitializeComponent() {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Item));
      this.tabPages = new System.Windows.Forms.TabControl();
      this.SuspendLayout();
      // 
      // tabPages
      // 
      resources.ApplyResources(this.tabPages, "tabPages");
      this.tabPages.Name = "tabPages";
      this.tabPages.SelectedIndex = 0;
      // 
      // Item
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.tabPages);
      this.Name = "Item";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TabControl tabPages;

  }

}
