namespace PlayOnline.FFXI.Utils.ItemComparison {

  public partial class MainWindow {

    #region Controls

    private PlayOnline.FFXI.ItemEditor ieLeft;
    private PlayOnline.FFXI.ItemEditor ieRight;
    private System.Windows.Forms.Button btnLoadItemSet1;
    private System.Windows.Forms.Button btnLoadItemSet2;
    private System.Windows.Forms.OpenFileDialog dlgLoadItems;
    private System.Windows.Forms.Button btnPrevious;
    private System.Windows.Forms.Button btnNext;
    private System.Windows.Forms.Button btnRemoveUnchanged;

    private System.ComponentModel.Container components = null;

    #endregion

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
      this.ieLeft = new PlayOnline.FFXI.ItemEditor();
      this.ieRight = new PlayOnline.FFXI.ItemEditor();
      this.btnLoadItemSet1 = new System.Windows.Forms.Button();
      this.btnLoadItemSet2 = new System.Windows.Forms.Button();
      this.dlgLoadItems = new System.Windows.Forms.OpenFileDialog();
      this.btnPrevious = new System.Windows.Forms.Button();
      this.btnNext = new System.Windows.Forms.Button();
      this.btnRemoveUnchanged = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // ieLeft
      // 
      this.ieLeft.AccessibleDescription = null;
      this.ieLeft.AccessibleName = null;
      resources.ApplyResources(this.ieLeft, "ieLeft");
      this.ieLeft.BackColor = System.Drawing.Color.Transparent;
      this.ieLeft.BackgroundImage = null;
      this.ieLeft.Font = null;
      this.ieLeft.Item = null;
      this.ieLeft.Name = "ieLeft";
      this.ieLeft.SizeChanged += new System.EventHandler(this.ItemViewerSizeChanged);
      // 
      // ieRight
      // 
      this.ieRight.AccessibleDescription = null;
      this.ieRight.AccessibleName = null;
      resources.ApplyResources(this.ieRight, "ieRight");
      this.ieRight.BackColor = System.Drawing.Color.Transparent;
      this.ieRight.BackgroundImage = null;
      this.ieRight.Font = null;
      this.ieRight.Item = null;
      this.ieRight.Name = "ieRight";
      this.ieRight.SizeChanged += new System.EventHandler(this.ItemViewerSizeChanged);
      // 
      // btnLoadItemSet1
      // 
      this.btnLoadItemSet1.AccessibleDescription = null;
      this.btnLoadItemSet1.AccessibleName = null;
      resources.ApplyResources(this.btnLoadItemSet1, "btnLoadItemSet1");
      this.btnLoadItemSet1.BackgroundImage = null;
      this.btnLoadItemSet1.Font = null;
      this.btnLoadItemSet1.Name = "btnLoadItemSet1";
      this.btnLoadItemSet1.Click += new System.EventHandler(this.btnLoadItemSet1_Click);
      // 
      // btnLoadItemSet2
      // 
      this.btnLoadItemSet2.AccessibleDescription = null;
      this.btnLoadItemSet2.AccessibleName = null;
      resources.ApplyResources(this.btnLoadItemSet2, "btnLoadItemSet2");
      this.btnLoadItemSet2.BackgroundImage = null;
      this.btnLoadItemSet2.Font = null;
      this.btnLoadItemSet2.Name = "btnLoadItemSet2";
      this.btnLoadItemSet2.Click += new System.EventHandler(this.btnLoadItemSet2_Click);
      // 
      // dlgLoadItems
      // 
      resources.ApplyResources(this.dlgLoadItems, "dlgLoadItems");
      // 
      // btnPrevious
      // 
      this.btnPrevious.AccessibleDescription = null;
      this.btnPrevious.AccessibleName = null;
      resources.ApplyResources(this.btnPrevious, "btnPrevious");
      this.btnPrevious.BackgroundImage = null;
      this.btnPrevious.Font = null;
      this.btnPrevious.Name = "btnPrevious";
      this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
      // 
      // btnNext
      // 
      this.btnNext.AccessibleDescription = null;
      this.btnNext.AccessibleName = null;
      resources.ApplyResources(this.btnNext, "btnNext");
      this.btnNext.BackgroundImage = null;
      this.btnNext.Font = null;
      this.btnNext.Name = "btnNext";
      this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
      // 
      // btnRemoveUnchanged
      // 
      this.btnRemoveUnchanged.AccessibleDescription = null;
      this.btnRemoveUnchanged.AccessibleName = null;
      resources.ApplyResources(this.btnRemoveUnchanged, "btnRemoveUnchanged");
      this.btnRemoveUnchanged.BackgroundImage = null;
      this.btnRemoveUnchanged.Font = null;
      this.btnRemoveUnchanged.Name = "btnRemoveUnchanged";
      this.btnRemoveUnchanged.Click += new System.EventHandler(this.btnRemoveUnchanged_Click);
      // 
      // MainWindow
      // 
      this.AccessibleDescription = null;
      this.AccessibleName = null;
      resources.ApplyResources(this, "$this");
      this.BackgroundImage = null;
      this.Controls.Add(this.btnRemoveUnchanged);
      this.Controls.Add(this.btnNext);
      this.Controls.Add(this.btnPrevious);
      this.Controls.Add(this.btnLoadItemSet2);
      this.Controls.Add(this.btnLoadItemSet1);
      this.Controls.Add(this.ieLeft);
      this.Controls.Add(this.ieRight);
      this.Font = null;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Icon = null;
      this.MaximizeBox = false;
      this.Name = "MainWindow";
      this.ResumeLayout(false);

    }

    #endregion

  }

}
