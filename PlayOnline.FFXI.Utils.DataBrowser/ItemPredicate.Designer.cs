namespace PlayOnline.FFXI.Utils.DataBrowser {

  internal partial class ItemPredicate {

    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing) {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    private void InitializeComponent() {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemPredicate));
      this.cmbTest = new System.Windows.Forms.ComboBox();
      this.txtTestParameter = new System.Windows.Forms.TextBox();
      this.cmbField = new System.Windows.Forms.ComboBox();
      this.SuspendLayout();
      // 
      // cmbTest
      // 
      this.cmbTest.DisplayMember = "Name";
      this.cmbTest.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbTest.FormattingEnabled = true;
      resources.ApplyResources(this.cmbTest, "cmbTest");
      this.cmbTest.Name = "cmbTest";
      this.cmbTest.ValueMember = "Value";
      // 
      // txtTestParameter
      // 
      resources.ApplyResources(this.txtTestParameter, "txtTestParameter");
      this.txtTestParameter.Name = "txtTestParameter";
      // 
      // cmbField
      // 
      this.cmbField.DisplayMember = "Name";
      this.cmbField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbField.FormattingEnabled = true;
      resources.ApplyResources(this.cmbField, "cmbField");
      this.cmbField.Name = "cmbField";
      this.cmbField.ValueMember = "Field";
      // 
      // ItemPredicate
      // 
      this.BackColor = System.Drawing.Color.Transparent;
      this.Controls.Add(this.cmbField);
      this.Controls.Add(this.cmbTest);
      this.Controls.Add(this.txtTestParameter);
      this.Name = "ItemPredicate";
      resources.ApplyResources(this, "$this");
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ComboBox cmbField;
    private System.Windows.Forms.ComboBox cmbTest;
    private System.Windows.Forms.TextBox txtTestParameter;

  }

}
