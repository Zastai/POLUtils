// $Id$

namespace PlayOnline.FFXI.PropertyPages {

  partial class Graphic {

    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
	components.Dispose();
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    private void InitializeComponent() {
      System.Windows.Forms.ThemedTabPage tabGraphicInfo;
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Graphic));
      this.btnSelectColor = new System.Windows.Forms.Button();
      this.radSolid = new System.Windows.Forms.RadioButton();
      this.radTransparent = new System.Windows.Forms.RadioButton();
      this.lblBackColor = new System.Windows.Forms.Label();
      this.cmbViewMode = new System.Windows.Forms.ComboBox();
      this.lblViewMode = new System.Windows.Forms.Label();
      this.picImage = new System.Windows.Forms.PictureBox();
      this.tabPages = new System.Windows.Forms.TabControl();
      tabGraphicInfo = new System.Windows.Forms.ThemedTabPage();
      tabGraphicInfo.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize) (this.picImage)).BeginInit();
      this.tabPages.SuspendLayout();
      this.SuspendLayout();
      // 
      // tabGraphicInfo
      // 
      tabGraphicInfo.Controls.Add(this.btnSelectColor);
      tabGraphicInfo.Controls.Add(this.radSolid);
      tabGraphicInfo.Controls.Add(this.radTransparent);
      tabGraphicInfo.Controls.Add(this.lblBackColor);
      tabGraphicInfo.Controls.Add(this.cmbViewMode);
      tabGraphicInfo.Controls.Add(this.lblViewMode);
      tabGraphicInfo.Controls.Add(this.picImage);
      resources.ApplyResources(tabGraphicInfo, "tabGraphicInfo");
      tabGraphicInfo.Name = "tabGraphicInfo";
      tabGraphicInfo.UseVisualStyleBackColor = true;
      // 
      // btnSelectColor
      // 
      resources.ApplyResources(this.btnSelectColor, "btnSelectColor");
      this.btnSelectColor.Name = "btnSelectColor";
      this.btnSelectColor.UseVisualStyleBackColor = true;
      this.btnSelectColor.Click += new System.EventHandler(this.btnSelectColor_Click);
      // 
      // radSolid
      // 
      resources.ApplyResources(this.radSolid, "radSolid");
      this.radSolid.Name = "radSolid";
      this.radSolid.UseVisualStyleBackColor = true;
      this.radSolid.CheckedChanged += new System.EventHandler(this.radSolid_CheckedChanged);
      // 
      // radTransparent
      // 
      resources.ApplyResources(this.radTransparent, "radTransparent");
      this.radTransparent.Checked = true;
      this.radTransparent.Name = "radTransparent";
      this.radTransparent.TabStop = true;
      this.radTransparent.UseVisualStyleBackColor = true;
      this.radTransparent.CheckedChanged += new System.EventHandler(this.radTransparent_CheckedChanged);
      // 
      // lblBackColor
      // 
      resources.ApplyResources(this.lblBackColor, "lblBackColor");
      this.lblBackColor.Name = "lblBackColor";
      // 
      // cmbViewMode
      // 
      resources.ApplyResources(this.cmbViewMode, "cmbViewMode");
      this.cmbViewMode.DisplayMember = "Name";
      this.cmbViewMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbViewMode.Name = "cmbViewMode";
      this.cmbViewMode.ValueMember = "Value";
      this.cmbViewMode.SelectedIndexChanged += new System.EventHandler(this.cmbViewMode_SelectedIndexChanged);
      // 
      // lblViewMode
      // 
      resources.ApplyResources(this.lblViewMode, "lblViewMode");
      this.lblViewMode.Name = "lblViewMode";
      // 
      // picImage
      // 
      resources.ApplyResources(this.picImage, "picImage");
      this.picImage.Name = "picImage";
      this.picImage.TabStop = false;
      // 
      // tabPages
      // 
      this.tabPages.Controls.Add(tabGraphicInfo);
      resources.ApplyResources(this.tabPages, "tabPages");
      this.tabPages.Name = "tabPages";
      this.tabPages.SelectedIndex = 0;
      // 
      // Graphic
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.tabPages);
      this.Name = "Graphic";
      tabGraphicInfo.ResumeLayout(false);
      tabGraphicInfo.PerformLayout();
      ((System.ComponentModel.ISupportInitialize) (this.picImage)).EndInit();
      this.tabPages.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TabControl tabPages;
    private System.Windows.Forms.PictureBox picImage;
    private System.Windows.Forms.ComboBox cmbViewMode;
    private System.Windows.Forms.Label lblViewMode;
    private System.Windows.Forms.Button btnSelectColor;
    private System.Windows.Forms.RadioButton radSolid;
    private System.Windows.Forms.RadioButton radTransparent;
    private System.Windows.Forms.Label lblBackColor;

  }

}
