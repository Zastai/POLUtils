using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;

using PlayOnline.Core;
using PlayOnline.FFXI;

namespace PlayOnline.Utils.FFXIDataBrowser {

  internal class CSVOptionDialog : System.Windows.Forms.Form {

    private static SaveFileDialog dlgSaveFile = null;

    #region Controls

    private System.Windows.Forms.Label lblFileName;
    private System.Windows.Forms.TextBox txtFileName;
    private System.Windows.Forms.Button btnBrowseFile;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Button btnCancel;

    private System.ComponentModel.Container components = null;

    #endregion

    public CSVOptionDialog() {
      this.InitializeComponent();
      if (CSVOptionDialog.dlgSaveFile == null) {
	CSVOptionDialog.dlgSaveFile = new SaveFileDialog();
	CSVOptionDialog.dlgSaveFile.Title = I18N.GetText("CSVExport:SaveDialogTitle");
	CSVOptionDialog.dlgSaveFile.Filter = I18N.GetText("CSVExport:SaveDialogFilter");
	CSVOptionDialog.dlgSaveFile.FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "items.csv");
      }
      else // don't force the location, just the filename
	CSVOptionDialog.dlgSaveFile.FileName = Path.Combine(Path.GetDirectoryName(CSVOptionDialog.dlgSaveFile.FileName), "items.csv");
      this.txtFileName.Text = CSVOptionDialog.dlgSaveFile.FileName;
    }

    #region Option Properties

    public ItemDataLanguage Language {
      get {
	return ItemDataLanguage.English;
      }
      set {
      }
    }

    public ItemDataType Type {
      get {
	return ItemDataType.Object;
      }
      set {
      }
    }

    public string FileName {
      get {
	return this.txtFileName.Text;
      }
      set {
	this.txtFileName.Text = value;
      }
    }

    public Encoding Encoding {
      get {
	return Encoding.Unicode;
      }
      set {
      }
    }

    #endregion

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
	components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CSVOptionDialog));
      this.btnBrowseFile = new System.Windows.Forms.Button();
      this.btnOK = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.lblFileName = new System.Windows.Forms.Label();
      this.txtFileName = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // btnBrowseFile
      // 
      this.btnBrowseFile.AccessibleDescription = resources.GetString("btnBrowseFile.AccessibleDescription");
      this.btnBrowseFile.AccessibleName = resources.GetString("btnBrowseFile.AccessibleName");
      this.btnBrowseFile.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnBrowseFile.Anchor")));
      this.btnBrowseFile.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBrowseFile.BackgroundImage")));
      this.btnBrowseFile.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnBrowseFile.Dock")));
      this.btnBrowseFile.Enabled = ((bool)(resources.GetObject("btnBrowseFile.Enabled")));
      this.btnBrowseFile.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnBrowseFile.FlatStyle")));
      this.btnBrowseFile.Font = ((System.Drawing.Font)(resources.GetObject("btnBrowseFile.Font")));
      this.btnBrowseFile.Image = ((System.Drawing.Image)(resources.GetObject("btnBrowseFile.Image")));
      this.btnBrowseFile.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBrowseFile.ImageAlign")));
      this.btnBrowseFile.ImageIndex = ((int)(resources.GetObject("btnBrowseFile.ImageIndex")));
      this.btnBrowseFile.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnBrowseFile.ImeMode")));
      this.btnBrowseFile.Location = ((System.Drawing.Point)(resources.GetObject("btnBrowseFile.Location")));
      this.btnBrowseFile.Name = "btnBrowseFile";
      this.btnBrowseFile.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnBrowseFile.RightToLeft")));
      this.btnBrowseFile.Size = ((System.Drawing.Size)(resources.GetObject("btnBrowseFile.Size")));
      this.btnBrowseFile.TabIndex = ((int)(resources.GetObject("btnBrowseFile.TabIndex")));
      this.btnBrowseFile.Text = resources.GetString("btnBrowseFile.Text");
      this.btnBrowseFile.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBrowseFile.TextAlign")));
      this.btnBrowseFile.Visible = ((bool)(resources.GetObject("btnBrowseFile.Visible")));
      this.btnBrowseFile.Click += new System.EventHandler(this.btnBrowseFile_Click);
      // 
      // btnOK
      // 
      this.btnOK.AccessibleDescription = resources.GetString("btnOK.AccessibleDescription");
      this.btnOK.AccessibleName = resources.GetString("btnOK.AccessibleName");
      this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnOK.Anchor")));
      this.btnOK.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnOK.BackgroundImage")));
      this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnOK.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnOK.Dock")));
      this.btnOK.Enabled = ((bool)(resources.GetObject("btnOK.Enabled")));
      this.btnOK.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnOK.FlatStyle")));
      this.btnOK.Font = ((System.Drawing.Font)(resources.GetObject("btnOK.Font")));
      this.btnOK.Image = ((System.Drawing.Image)(resources.GetObject("btnOK.Image")));
      this.btnOK.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnOK.ImageAlign")));
      this.btnOK.ImageIndex = ((int)(resources.GetObject("btnOK.ImageIndex")));
      this.btnOK.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnOK.ImeMode")));
      this.btnOK.Location = ((System.Drawing.Point)(resources.GetObject("btnOK.Location")));
      this.btnOK.Name = "btnOK";
      this.btnOK.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnOK.RightToLeft")));
      this.btnOK.Size = ((System.Drawing.Size)(resources.GetObject("btnOK.Size")));
      this.btnOK.TabIndex = ((int)(resources.GetObject("btnOK.TabIndex")));
      this.btnOK.Text = resources.GetString("btnOK.Text");
      this.btnOK.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnOK.TextAlign")));
      this.btnOK.Visible = ((bool)(resources.GetObject("btnOK.Visible")));
      // 
      // btnCancel
      // 
      this.btnCancel.AccessibleDescription = resources.GetString("btnCancel.AccessibleDescription");
      this.btnCancel.AccessibleName = resources.GetString("btnCancel.AccessibleName");
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnCancel.Anchor")));
      this.btnCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.BackgroundImage")));
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnCancel.Dock")));
      this.btnCancel.Enabled = ((bool)(resources.GetObject("btnCancel.Enabled")));
      this.btnCancel.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnCancel.FlatStyle")));
      this.btnCancel.Font = ((System.Drawing.Font)(resources.GetObject("btnCancel.Font")));
      this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
      this.btnCancel.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnCancel.ImageAlign")));
      this.btnCancel.ImageIndex = ((int)(resources.GetObject("btnCancel.ImageIndex")));
      this.btnCancel.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnCancel.ImeMode")));
      this.btnCancel.Location = ((System.Drawing.Point)(resources.GetObject("btnCancel.Location")));
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnCancel.RightToLeft")));
      this.btnCancel.Size = ((System.Drawing.Size)(resources.GetObject("btnCancel.Size")));
      this.btnCancel.TabIndex = ((int)(resources.GetObject("btnCancel.TabIndex")));
      this.btnCancel.Text = resources.GetString("btnCancel.Text");
      this.btnCancel.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnCancel.TextAlign")));
      this.btnCancel.Visible = ((bool)(resources.GetObject("btnCancel.Visible")));
      // 
      // lblFileName
      // 
      this.lblFileName.AccessibleDescription = resources.GetString("lblFileName.AccessibleDescription");
      this.lblFileName.AccessibleName = resources.GetString("lblFileName.AccessibleName");
      this.lblFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblFileName.Anchor")));
      this.lblFileName.AutoSize = ((bool)(resources.GetObject("lblFileName.AutoSize")));
      this.lblFileName.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblFileName.Dock")));
      this.lblFileName.Enabled = ((bool)(resources.GetObject("lblFileName.Enabled")));
      this.lblFileName.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblFileName.Font = ((System.Drawing.Font)(resources.GetObject("lblFileName.Font")));
      this.lblFileName.Image = ((System.Drawing.Image)(resources.GetObject("lblFileName.Image")));
      this.lblFileName.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblFileName.ImageAlign")));
      this.lblFileName.ImageIndex = ((int)(resources.GetObject("lblFileName.ImageIndex")));
      this.lblFileName.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblFileName.ImeMode")));
      this.lblFileName.Location = ((System.Drawing.Point)(resources.GetObject("lblFileName.Location")));
      this.lblFileName.Name = "lblFileName";
      this.lblFileName.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblFileName.RightToLeft")));
      this.lblFileName.Size = ((System.Drawing.Size)(resources.GetObject("lblFileName.Size")));
      this.lblFileName.TabIndex = ((int)(resources.GetObject("lblFileName.TabIndex")));
      this.lblFileName.Text = resources.GetString("lblFileName.Text");
      this.lblFileName.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblFileName.TextAlign")));
      this.lblFileName.Visible = ((bool)(resources.GetObject("lblFileName.Visible")));
      // 
      // txtFileName
      // 
      this.txtFileName.AccessibleDescription = resources.GetString("txtFileName.AccessibleDescription");
      this.txtFileName.AccessibleName = resources.GetString("txtFileName.AccessibleName");
      this.txtFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtFileName.Anchor")));
      this.txtFileName.AutoSize = ((bool)(resources.GetObject("txtFileName.AutoSize")));
      this.txtFileName.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtFileName.BackgroundImage")));
      this.txtFileName.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtFileName.Dock")));
      this.txtFileName.Enabled = ((bool)(resources.GetObject("txtFileName.Enabled")));
      this.txtFileName.Font = ((System.Drawing.Font)(resources.GetObject("txtFileName.Font")));
      this.txtFileName.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtFileName.ImeMode")));
      this.txtFileName.Location = ((System.Drawing.Point)(resources.GetObject("txtFileName.Location")));
      this.txtFileName.MaxLength = ((int)(resources.GetObject("txtFileName.MaxLength")));
      this.txtFileName.Multiline = ((bool)(resources.GetObject("txtFileName.Multiline")));
      this.txtFileName.Name = "txtFileName";
      this.txtFileName.PasswordChar = ((char)(resources.GetObject("txtFileName.PasswordChar")));
      this.txtFileName.ReadOnly = true;
      this.txtFileName.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtFileName.RightToLeft")));
      this.txtFileName.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtFileName.ScrollBars")));
      this.txtFileName.Size = ((System.Drawing.Size)(resources.GetObject("txtFileName.Size")));
      this.txtFileName.TabIndex = ((int)(resources.GetObject("txtFileName.TabIndex")));
      this.txtFileName.Text = resources.GetString("txtFileName.Text");
      this.txtFileName.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtFileName.TextAlign")));
      this.txtFileName.Visible = ((bool)(resources.GetObject("txtFileName.Visible")));
      this.txtFileName.WordWrap = ((bool)(resources.GetObject("txtFileName.WordWrap")));
      // 
      // CSVOptionDialog
      // 
      this.AcceptButton = this.btnOK;
      this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
      this.AccessibleName = resources.GetString("$this.AccessibleName");
      this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
      this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
      this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
      this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
      this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
      this.CancelButton = this.btnCancel;
      this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
      this.Controls.Add(this.txtFileName);
      this.Controls.Add(this.lblFileName);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.btnBrowseFile);
      this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
      this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
      this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
      this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
      this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
      this.Name = "CSVOptionDialog";
      this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
      this.ShowInTaskbar = false;
      this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
      this.Text = resources.GetString("$this.Text");
      this.ResumeLayout(false);

    }

    #endregion

    #region Events

    private void btnBrowseFile_Click(object sender, System.EventArgs e) {
      CSVOptionDialog.dlgSaveFile.FileName = this.txtFileName.Text;
      if (CSVOptionDialog.dlgSaveFile.ShowDialog() == DialogResult.OK)
	this.txtFileName.Text = CSVOptionDialog.dlgSaveFile.FileName;
    }

    #endregion

  }

}
