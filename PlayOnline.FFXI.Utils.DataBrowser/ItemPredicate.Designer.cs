namespace PlayOnline.FFXI.Utils.DataBrowser {

  internal partial class ItemPredicate {

    #region Controls

    private System.Windows.Forms.ComboBox cmbTest;
    private System.Windows.Forms.Button btnChooseField;
    private System.Windows.Forms.TextBox txtTestParameter;
    private System.Windows.Forms.TextBox txtField;

    private System.ComponentModel.Container components = null;

    #endregion

    #region Component Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ItemPredicate));
      this.cmbTest = new System.Windows.Forms.ComboBox();
      this.btnChooseField = new System.Windows.Forms.Button();
      this.txtTestParameter = new System.Windows.Forms.TextBox();
      this.txtField = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // cmbTest
      // 
      this.cmbTest.DisplayMember = "Name";
      this.cmbTest.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbTest.FormattingEnabled = true;
      resources.ApplyResources(this.cmbTest, "cmbTest");
      this.cmbTest.Name = "cmbTest";
      // 
      // btnChooseField
      // 
      resources.ApplyResources(this.btnChooseField, "btnChooseField");
      this.btnChooseField.Name = "btnChooseField";
      this.btnChooseField.Click += new System.EventHandler(this.btnChooseField_Click);
      // 
      // txtTestParameter
      // 
      resources.ApplyResources(this.txtTestParameter, "txtTestParameter");
      this.txtTestParameter.Name = "txtTestParameter";
      // 
      // txtField
      // 
      resources.ApplyResources(this.txtField, "txtField");
      this.txtField.Name = "txtField";
      this.txtField.ReadOnly = true;
      // 
      // ItemPredicate
      // 
      this.BackColor = System.Drawing.SystemColors.Control;
      this.Controls.Add(this.cmbTest);
      this.Controls.Add(this.btnChooseField);
      this.Controls.Add(this.txtTestParameter);
      this.Controls.Add(this.txtField);
      this.Name = "ItemPredicate";
      resources.ApplyResources(this, "$this");
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

  }

}
