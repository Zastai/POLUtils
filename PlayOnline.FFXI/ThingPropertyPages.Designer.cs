namespace PlayOnline.FFXI {

  partial class ThingPropertyPages {

    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing) {
      if (disposing && this.components != null)
	components.Dispose();
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent() {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ThingPropertyPages));
      this.tabPages = new System.Windows.Forms.TabControl();
      this.tabDummy = new System.Windows.Forms.TabPage();
      this.pnlButtons = new System.Windows.Forms.Panel();
      this.btnClose = new System.Windows.Forms.Button();
      this.tabPages.SuspendLayout();
      this.pnlButtons.SuspendLayout();
      this.SuspendLayout();
      // 
      // tabPages
      // 
      this.tabPages.AccessibleDescription = null;
      this.tabPages.AccessibleName = null;
      resources.ApplyResources(this.tabPages, "tabPages");
      this.tabPages.BackgroundImage = null;
      this.tabPages.Controls.Add(this.tabDummy);
      this.tabPages.Font = null;
      this.tabPages.HotTrack = true;
      this.tabPages.Multiline = true;
      this.tabPages.Name = "tabPages";
      this.tabPages.SelectedIndex = 0;
      this.tabPages.SelectedIndexChanged += new System.EventHandler(this.tabPages_SelectedIndexChanged);
      // 
      // tabDummy
      // 
      this.tabDummy.AccessibleDescription = null;
      this.tabDummy.AccessibleName = null;
      resources.ApplyResources(this.tabDummy, "tabDummy");
      this.tabDummy.BackgroundImage = null;
      this.tabDummy.Font = null;
      this.tabDummy.Name = "tabDummy";
      this.tabDummy.UseVisualStyleBackColor = true;
      // 
      // pnlButtons
      // 
      this.pnlButtons.AccessibleDescription = null;
      this.pnlButtons.AccessibleName = null;
      resources.ApplyResources(this.pnlButtons, "pnlButtons");
      this.pnlButtons.BackColor = System.Drawing.Color.Transparent;
      this.pnlButtons.BackgroundImage = null;
      this.pnlButtons.Controls.Add(this.btnClose);
      this.pnlButtons.Font = null;
      this.pnlButtons.Name = "pnlButtons";
      // 
      // btnClose
      // 
      this.btnClose.AccessibleDescription = null;
      this.btnClose.AccessibleName = null;
      resources.ApplyResources(this.btnClose, "btnClose");
      this.btnClose.BackgroundImage = null;
      this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnClose.Font = null;
      this.btnClose.Name = "btnClose";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
      // 
      // ThingPropertyPages
      // 
      this.AcceptButton = this.btnClose;
      this.AccessibleDescription = null;
      this.AccessibleName = null;
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackgroundImage = null;
      this.Controls.Add(this.tabPages);
      this.Controls.Add(this.pnlButtons);
      this.Font = null;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = null;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ThingPropertyPages";
      this.ShowIcon = false;
      this.ShowInTaskbar = false;
      this.tabPages.ResumeLayout(false);
      this.pnlButtons.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TabControl tabPages;
    private System.Windows.Forms.Panel pnlButtons;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.TabPage tabDummy;

  }

}