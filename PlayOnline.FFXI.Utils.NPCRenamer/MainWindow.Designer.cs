namespace PlayOnline.FFXI.Utils.NPCRenamer {

  partial class MainWindow {

    private System.ComponentModel.Container components = null;

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      System.Windows.Forms.ColumnHeader colNPCName;
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
      this.grpArea = new System.Windows.Forms.GroupBox();
      this.cmbArea = new System.Windows.Forms.ComboBox();
      this.grpNames = new System.Windows.Forms.GroupBox();
      this.lstNPCNames = new System.Windows.Forms.ListView();
      this.btnClose = new System.Windows.Forms.Button();
      this.btnShowChanges = new System.Windows.Forms.Button();
      colNPCName = new System.Windows.Forms.ColumnHeader();
      this.grpArea.SuspendLayout();
      this.grpNames.SuspendLayout();
      this.SuspendLayout();
      // 
      // colNPCName
      // 
      resources.ApplyResources(colNPCName, "colNPCName");
      // 
      // grpArea
      // 
      resources.ApplyResources(this.grpArea, "grpArea");
      this.grpArea.Controls.Add(this.cmbArea);
      this.grpArea.Name = "grpArea";
      this.grpArea.TabStop = false;
      // 
      // cmbArea
      // 
      resources.ApplyResources(this.cmbArea, "cmbArea");
      this.cmbArea.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbArea.FormattingEnabled = true;
      this.cmbArea.Name = "cmbArea";
      this.cmbArea.Sorted = true;
      this.cmbArea.SelectedIndexChanged += new System.EventHandler(this.cmbArea_SelectedIndexChanged);
      // 
      // grpNames
      // 
      resources.ApplyResources(this.grpNames, "grpNames");
      this.grpNames.Controls.Add(this.lstNPCNames);
      this.grpNames.Name = "grpNames";
      this.grpNames.TabStop = false;
      // 
      // lstNPCNames
      // 
      resources.ApplyResources(this.lstNPCNames, "lstNPCNames");
      this.lstNPCNames.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            colNPCName});
      this.lstNPCNames.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
      this.lstNPCNames.LabelEdit = true;
      this.lstNPCNames.MultiSelect = false;
      this.lstNPCNames.Name = "lstNPCNames";
      this.lstNPCNames.ShowGroups = false;
      this.lstNPCNames.UseCompatibleStateImageBehavior = false;
      this.lstNPCNames.View = System.Windows.Forms.View.Details;
      this.lstNPCNames.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lstNPCNames_KeyDown);
      this.lstNPCNames.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.lstNPCNames_AfterLabelEdit);
      // 
      // btnClose
      // 
      resources.ApplyResources(this.btnClose, "btnClose");
      this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnClose.Name = "btnClose";
      this.btnClose.UseVisualStyleBackColor = true;
      this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
      // 
      // btnShowChanges
      // 
      resources.ApplyResources(this.btnShowChanges, "btnShowChanges");
      this.btnShowChanges.Name = "btnShowChanges";
      this.btnShowChanges.UseVisualStyleBackColor = true;
      this.btnShowChanges.Click += new System.EventHandler(this.btnShowChanges_Click);
      // 
      // MainWindow
      // 
      resources.ApplyResources(this, "$this");
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnClose;
      this.Controls.Add(this.btnShowChanges);
      this.Controls.Add(this.btnClose);
      this.Controls.Add(this.grpNames);
      this.Controls.Add(this.grpArea);
      this.Name = "MainWindow";
      this.Shown += new System.EventHandler(this.MainWindow_Shown);
      this.grpArea.ResumeLayout(false);
      this.grpNames.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox grpArea;
    private System.Windows.Forms.ComboBox cmbArea;
    private System.Windows.Forms.GroupBox grpNames;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.Button btnShowChanges;
    private System.Windows.Forms.ListView lstNPCNames;

  }

}