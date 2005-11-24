namespace PlayOnline.FFXI.Utils.DataBrowser {

  internal partial class ItemFieldChooser {

    #region Controls

    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.CheckedListBox clstFields;

    private System.ComponentModel.Container components = null;

    #endregion

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemFieldChooser));
      this.btnOK = new System.Windows.Forms.Button();
      this.clstFields = new System.Windows.Forms.CheckedListBox();
      this.SuspendLayout();
      // 
      // btnOK
      // 
      resources.ApplyResources(this.btnOK, "btnOK");
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOK.Name = "btnOK";
      // 
      // clstFields
      // 
      this.clstFields.CheckOnClick = true;
      this.clstFields.FormattingEnabled = true;
      resources.ApplyResources(this.clstFields, "clstFields");
      this.clstFields.Name = "clstFields";
      this.clstFields.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clstFields_ItemCheck);
      // 
      // ItemFieldChooser
      // 
      this.AcceptButton = this.btnOK;
      resources.ApplyResources(this, "$this");
      this.Controls.Add(this.clstFields);
      this.Controls.Add(this.btnOK);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ItemFieldChooser";
      this.ShowInTaskbar = false;
      this.ResumeLayout(false);

    }

    #endregion

  }

}
