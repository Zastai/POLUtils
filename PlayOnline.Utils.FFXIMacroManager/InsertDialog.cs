using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using PlayOnline.Core;
using PlayOnline.FFXI;

namespace PlayOnline.Utils.FFXIMacroManager {

  public class InsertDialog : System.Windows.Forms.Form {
    private System.Windows.Forms.TreeView tvCategories;
    private System.Windows.Forms.Panel pnlCommands;
    private System.Windows.Forms.ListView lvEntries;
    private System.Windows.Forms.Button btnInsert;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.TextBox txtPreview;

    #region Controls

    private System.ComponentModel.Container components = null;

    #endregion

    public InsertDialog() {
      this.InitializeComponent();
    }

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
	components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      this.tvCategories = new System.Windows.Forms.TreeView();
      this.pnlCommands = new System.Windows.Forms.Panel();
      this.lvEntries = new System.Windows.Forms.ListView();
      this.btnInsert = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.txtPreview = new System.Windows.Forms.TextBox();
      this.pnlCommands.SuspendLayout();
      this.SuspendLayout();
      // 
      // tvCategories
      // 
      this.tvCategories.Dock = System.Windows.Forms.DockStyle.Left;
      this.tvCategories.HideSelection = false;
      this.tvCategories.ImageIndex = -1;
      this.tvCategories.Location = new System.Drawing.Point(0, 0);
      this.tvCategories.Name = "tvCategories";
      this.tvCategories.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
									     new System.Windows.Forms.TreeNode("Auto-Translator", new System.Windows.Forms.TreeNode[] {
																					new System.Windows.Forms.TreeNode("English Messages"),
																					new System.Windows.Forms.TreeNode("Japanese Messages"),
																					new System.Windows.Forms.TreeNode("Item Names", new System.Windows.Forms.TreeNode[] {
																															      new System.Windows.Forms.TreeNode("Weapons"),
																															      new System.Windows.Forms.TreeNode("Armor"),
																															      new System.Windows.Forms.TreeNode("Other")})}),
									     new System.Windows.Forms.TreeNode("Alphabets"),
									     new System.Windows.Forms.TreeNode("Faces")});
      this.tvCategories.SelectedImageIndex = -1;
      this.tvCategories.Size = new System.Drawing.Size(252, 332);
      this.tvCategories.TabIndex = 0;
      // 
      // pnlCommands
      // 
      this.pnlCommands.Controls.Add(this.txtPreview);
      this.pnlCommands.Controls.Add(this.btnCancel);
      this.pnlCommands.Controls.Add(this.btnInsert);
      this.pnlCommands.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.pnlCommands.Location = new System.Drawing.Point(252, 280);
      this.pnlCommands.Name = "pnlCommands";
      this.pnlCommands.Size = new System.Drawing.Size(482, 52);
      this.pnlCommands.TabIndex = 1;
      // 
      // lvEntries
      // 
      this.lvEntries.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lvEntries.Font = new System.Drawing.Font("Lucida Sans Unicode", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.lvEntries.FullRowSelect = true;
      this.lvEntries.GridLines = true;
      this.lvEntries.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
      this.lvEntries.HideSelection = false;
      this.lvEntries.Location = new System.Drawing.Point(252, 0);
      this.lvEntries.Name = "lvEntries";
      this.lvEntries.Size = new System.Drawing.Size(482, 280);
      this.lvEntries.TabIndex = 2;
      this.lvEntries.View = System.Windows.Forms.View.Details;
      // 
      // btnInsert
      // 
      this.btnInsert.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnInsert.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnInsert.Location = new System.Drawing.Point(388, 4);
      this.btnInsert.Name = "btnInsert";
      this.btnInsert.Size = new System.Drawing.Size(88, 20);
      this.btnInsert.TabIndex = 0;
      this.btnInsert.Text = "&Insert";
      // 
      // btnCancel
      // 
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnCancel.Location = new System.Drawing.Point(388, 28);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(88, 20);
      this.btnCancel.TabIndex = 1;
      this.btnCancel.Text = "&Cancel";
      // 
      // txtPreview
      // 
      this.txtPreview.Font = new System.Drawing.Font("Lucida Sans Unicode", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.txtPreview.Location = new System.Drawing.Point(4, 8);
      this.txtPreview.Name = "txtPreview";
      this.txtPreview.Size = new System.Drawing.Size(376, 36);
      this.txtPreview.TabIndex = 2;
      this.txtPreview.Text = "";
      // 
      // InsertDialog
      // 
      this.AcceptButton = this.btnInsert;
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(734, 332);
      this.Controls.Add(this.lvEntries);
      this.Controls.Add(this.pnlCommands);
      this.Controls.Add(this.tvCategories);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.MaximizeBox = false;
      this.Name = "InsertDialog";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Insert";
      this.pnlCommands.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

  }

}
