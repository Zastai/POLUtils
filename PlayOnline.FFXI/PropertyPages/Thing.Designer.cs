// $Id$

namespace PlayOnline.FFXI.PropertyPages {

  partial class Thing {

    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
	components.Dispose();
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    private void InitializeComponent() {
      System.Windows.Forms.ColumnHeader colFieldName;
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Thing));
      System.Windows.Forms.ColumnHeader colFieldValue;
      System.Windows.Forms.ThemedTabPage tabGeneralInfo;
      this.lblText = new System.Windows.Forms.Label();
      this.lstFields = new System.Windows.Forms.ListView();
      this.lblFields = new System.Windows.Forms.Label();
      this.lblTypeName = new System.Windows.Forms.Label();
      this.lblType = new System.Windows.Forms.Label();
      this.picIcon = new System.Windows.Forms.PictureBox();
      this.tabPages = new System.Windows.Forms.TabControl();
      colFieldName = new System.Windows.Forms.ColumnHeader();
      colFieldValue = new System.Windows.Forms.ColumnHeader();
      tabGeneralInfo = new System.Windows.Forms.ThemedTabPage();
      tabGeneralInfo.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize) (this.picIcon)).BeginInit();
      this.tabPages.SuspendLayout();
      this.SuspendLayout();
      // 
      // colFieldName
      // 
      resources.ApplyResources(colFieldName, "colFieldName");
      // 
      // colFieldValue
      // 
      resources.ApplyResources(colFieldValue, "colFieldValue");
      // 
      // tabGeneralInfo
      // 
      tabGeneralInfo.Controls.Add(this.lblText);
      tabGeneralInfo.Controls.Add(this.lstFields);
      tabGeneralInfo.Controls.Add(this.lblFields);
      tabGeneralInfo.Controls.Add(this.lblTypeName);
      tabGeneralInfo.Controls.Add(this.lblType);
      tabGeneralInfo.Controls.Add(this.picIcon);
      resources.ApplyResources(tabGeneralInfo, "tabGeneralInfo");
      tabGeneralInfo.Name = "tabGeneralInfo";
      tabGeneralInfo.UseVisualStyleBackColor = true;
      // 
      // lblText
      // 
      resources.ApplyResources(this.lblText, "lblText");
      this.lblText.Name = "lblText";
      // 
      // lstFields
      // 
      resources.ApplyResources(this.lstFields, "lstFields");
      this.lstFields.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            colFieldName,
            colFieldValue});
      this.lstFields.FullRowSelect = true;
      this.lstFields.Name = "lstFields";
      this.lstFields.UseCompatibleStateImageBehavior = false;
      this.lstFields.View = System.Windows.Forms.View.Details;
      // 
      // lblFields
      // 
      resources.ApplyResources(this.lblFields, "lblFields");
      this.lblFields.Name = "lblFields";
      // 
      // lblTypeName
      // 
      resources.ApplyResources(this.lblTypeName, "lblTypeName");
      this.lblTypeName.Name = "lblTypeName";
      // 
      // lblType
      // 
      resources.ApplyResources(this.lblType, "lblType");
      this.lblType.Name = "lblType";
      // 
      // picIcon
      // 
      resources.ApplyResources(this.picIcon, "picIcon");
      this.picIcon.Name = "picIcon";
      this.picIcon.TabStop = false;
      // 
      // tabPages
      // 
      this.tabPages.Controls.Add(tabGeneralInfo);
      resources.ApplyResources(this.tabPages, "tabPages");
      this.tabPages.Name = "tabPages";
      this.tabPages.SelectedIndex = 0;
      // 
      // Thing
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.tabPages);
      this.Name = "Thing";
      tabGeneralInfo.ResumeLayout(false);
      tabGeneralInfo.PerformLayout();
      ((System.ComponentModel.ISupportInitialize) (this.picIcon)).EndInit();
      this.tabPages.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TabControl tabPages;
    private System.Windows.Forms.Label lblType;
    private System.Windows.Forms.PictureBox picIcon;
    private System.Windows.Forms.ListView lstFields;
    private System.Windows.Forms.Label lblFields;
    private System.Windows.Forms.Label lblTypeName;
    private System.Windows.Forms.Label lblText;

  }

}
