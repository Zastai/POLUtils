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
      this.ieEditor = new PlayOnline.FFXI.ItemEditor();
      this.SuspendLayout();
      // 
      // ieEditor
      // 
      this.ieEditor.BackColor = System.Drawing.Color.Transparent;
      this.ieEditor.Item = null;
      resources.ApplyResources(this.ieEditor, "ieEditor");
      this.ieEditor.Name = "ieEditor";
      // 
      // Item
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.ieEditor);
      this.Name = "Item";
      this.TabName = "Item";
      this.ResumeLayout(false);

    }

    #endregion

    private ItemEditor ieEditor;


  }

}
