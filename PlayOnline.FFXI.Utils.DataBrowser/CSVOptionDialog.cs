using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;

using PlayOnline.Core;

namespace PlayOnline.FFXI.Utils.DataBrowser {

  internal class CSVOptionDialog : System.Windows.Forms.Form {

    #region Controls

    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.GroupBox grpDisplayMode;
    private System.Windows.Forms.ComboBox cmbItemType;
    private System.Windows.Forms.ComboBox cmbLanguage;
    private System.Windows.Forms.Button btnChooseFields;
    private System.Windows.Forms.Label lblTextEncoding;
    private System.Windows.Forms.ComboBox cmbTextEncoding;
    private System.Windows.Forms.GroupBox grpFolder;
    private System.Windows.Forms.TextBox txtFolder;
    private System.Windows.Forms.Button btnBrowseFolder;

    private System.ComponentModel.Container components = null;

    #endregion

    public CSVOptionDialog() {
      this.InitializeComponent();
      this.txtFolder.Text = IItemExporter.OutputPath;
      this.cmbTextEncoding.Items.Add(Encoding.ASCII);
      this.cmbTextEncoding.Items.Add(Encoding.UTF8);
      this.cmbTextEncoding.Items.Add(Encoding.Unicode);
      this.cmbTextEncoding.SelectedIndex = 1; // UTF-8
      this.Fields_ = new ArrayList();
      this.cmbLanguage.Items.AddRange(NamedEnum.GetAll(typeof(ItemDataLanguage)));
      this.cmbLanguage.SelectedIndex = 0; // English
      this.cmbItemType.Items.AddRange(NamedEnum.GetAll(typeof(ItemDataType)));
      this.cmbItemType.SelectedIndex = 1; // Object
    }

    public void Reset() {
      // About to be (re)used - so clear the last run's field selection, and refresh the output path
      this.ResetFields();
      this.txtFolder.Text = IItemExporter.OutputPath;
    }

    private void ResetFields() {
      if (this.cmbLanguage.SelectedItem == null || this.cmbItemType.SelectedItem == null)
	return;
      this.Fields_.Clear();
      this.Fields_.AddRange(FFXIItem.GetFields(this.Language, this.Type));
    }

    #region Option Properties

    public ItemDataLanguage Language {
      get {
	return (ItemDataLanguage) (this.cmbLanguage.SelectedItem as NamedEnum).Value;
      }
      set {
	foreach (NamedEnum NE in this.cmbLanguage.Items) {
	  if ((ItemDataLanguage) NE.Value == value) {
	    this.cmbLanguage.SelectedItem = NE;
	    break;
	  }
	}
      }
    }

    public ItemDataType Type {
      get {
	return (ItemDataType) (this.cmbItemType.SelectedItem as NamedEnum).Value;
      }
      set {
	foreach (NamedEnum NE in this.cmbItemType.Items) {
	  if ((ItemDataType) NE.Value == value) {
	    this.cmbItemType.SelectedItem = NE;
	    break;
	  }
	}
      }
    }

    public string FileName {
      get {
      string Folder = this.txtFolder.Text;
      string File = String.Format("{0}-{1}-{2}.csv", ((this.Language == ItemDataLanguage.English) ? "en" : "jp"), this.Type.ToString().ToLower(), DateTime.Now.ToString("yyyyMMddHHmmss"));
	return Path.Combine(Folder, File);
      }
    }

    public Encoding Encoding {
      get {
	return this.cmbTextEncoding.SelectedItem as Encoding;
      }
      set {
	if (value == null)
	  return;
	foreach (Encoding E in this.cmbTextEncoding.Items) {
	  if (E.WindowsCodePage == value.WindowsCodePage) {
	    this.cmbTextEncoding.SelectedItem = E;
	    break;
	  }
	}
      }
    }

    private ArrayList Fields_ = null;
    public ItemField[] Fields {
      get {
	return (ItemField[]) this.Fields_.ToArray(typeof(ItemField));
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
      this.btnOK = new System.Windows.Forms.Button();
      this.btnCancel = new System.Windows.Forms.Button();
      this.grpDisplayMode = new System.Windows.Forms.GroupBox();
      this.btnChooseFields = new System.Windows.Forms.Button();
      this.cmbItemType = new System.Windows.Forms.ComboBox();
      this.cmbLanguage = new System.Windows.Forms.ComboBox();
      this.grpFolder = new System.Windows.Forms.GroupBox();
      this.cmbTextEncoding = new System.Windows.Forms.ComboBox();
      this.lblTextEncoding = new System.Windows.Forms.Label();
      this.txtFolder = new System.Windows.Forms.TextBox();
      this.btnBrowseFolder = new System.Windows.Forms.Button();
      this.grpDisplayMode.SuspendLayout();
      this.grpFolder.SuspendLayout();
      this.SuspendLayout();
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
      // grpDisplayMode
      // 
      this.grpDisplayMode.AccessibleDescription = resources.GetString("grpDisplayMode.AccessibleDescription");
      this.grpDisplayMode.AccessibleName = resources.GetString("grpDisplayMode.AccessibleName");
      this.grpDisplayMode.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("grpDisplayMode.Anchor")));
      this.grpDisplayMode.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpDisplayMode.BackgroundImage")));
      this.grpDisplayMode.Controls.Add(this.btnChooseFields);
      this.grpDisplayMode.Controls.Add(this.cmbItemType);
      this.grpDisplayMode.Controls.Add(this.cmbLanguage);
      this.grpDisplayMode.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("grpDisplayMode.Dock")));
      this.grpDisplayMode.Enabled = ((bool)(resources.GetObject("grpDisplayMode.Enabled")));
      this.grpDisplayMode.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.grpDisplayMode.Font = ((System.Drawing.Font)(resources.GetObject("grpDisplayMode.Font")));
      this.grpDisplayMode.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("grpDisplayMode.ImeMode")));
      this.grpDisplayMode.Location = ((System.Drawing.Point)(resources.GetObject("grpDisplayMode.Location")));
      this.grpDisplayMode.Name = "grpDisplayMode";
      this.grpDisplayMode.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("grpDisplayMode.RightToLeft")));
      this.grpDisplayMode.Size = ((System.Drawing.Size)(resources.GetObject("grpDisplayMode.Size")));
      this.grpDisplayMode.TabIndex = ((int)(resources.GetObject("grpDisplayMode.TabIndex")));
      this.grpDisplayMode.TabStop = false;
      this.grpDisplayMode.Text = resources.GetString("grpDisplayMode.Text");
      this.grpDisplayMode.Visible = ((bool)(resources.GetObject("grpDisplayMode.Visible")));
      // 
      // btnChooseFields
      // 
      this.btnChooseFields.AccessibleDescription = resources.GetString("btnChooseFields.AccessibleDescription");
      this.btnChooseFields.AccessibleName = resources.GetString("btnChooseFields.AccessibleName");
      this.btnChooseFields.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnChooseFields.Anchor")));
      this.btnChooseFields.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnChooseFields.BackgroundImage")));
      this.btnChooseFields.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnChooseFields.Dock")));
      this.btnChooseFields.Enabled = ((bool)(resources.GetObject("btnChooseFields.Enabled")));
      this.btnChooseFields.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnChooseFields.FlatStyle")));
      this.btnChooseFields.Font = ((System.Drawing.Font)(resources.GetObject("btnChooseFields.Font")));
      this.btnChooseFields.Image = ((System.Drawing.Image)(resources.GetObject("btnChooseFields.Image")));
      this.btnChooseFields.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnChooseFields.ImageAlign")));
      this.btnChooseFields.ImageIndex = ((int)(resources.GetObject("btnChooseFields.ImageIndex")));
      this.btnChooseFields.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnChooseFields.ImeMode")));
      this.btnChooseFields.Location = ((System.Drawing.Point)(resources.GetObject("btnChooseFields.Location")));
      this.btnChooseFields.Name = "btnChooseFields";
      this.btnChooseFields.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnChooseFields.RightToLeft")));
      this.btnChooseFields.Size = ((System.Drawing.Size)(resources.GetObject("btnChooseFields.Size")));
      this.btnChooseFields.TabIndex = ((int)(resources.GetObject("btnChooseFields.TabIndex")));
      this.btnChooseFields.Text = resources.GetString("btnChooseFields.Text");
      this.btnChooseFields.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnChooseFields.TextAlign")));
      this.btnChooseFields.Visible = ((bool)(resources.GetObject("btnChooseFields.Visible")));
      this.btnChooseFields.Click += new System.EventHandler(this.btnChooseFields_Click);
      // 
      // cmbItemType
      // 
      this.cmbItemType.AccessibleDescription = resources.GetString("cmbItemType.AccessibleDescription");
      this.cmbItemType.AccessibleName = resources.GetString("cmbItemType.AccessibleName");
      this.cmbItemType.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("cmbItemType.Anchor")));
      this.cmbItemType.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("cmbItemType.BackgroundImage")));
      this.cmbItemType.DisplayMember = "Name";
      this.cmbItemType.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("cmbItemType.Dock")));
      this.cmbItemType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbItemType.Enabled = ((bool)(resources.GetObject("cmbItemType.Enabled")));
      this.cmbItemType.Font = ((System.Drawing.Font)(resources.GetObject("cmbItemType.Font")));
      this.cmbItemType.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("cmbItemType.ImeMode")));
      this.cmbItemType.IntegralHeight = ((bool)(resources.GetObject("cmbItemType.IntegralHeight")));
      this.cmbItemType.ItemHeight = ((int)(resources.GetObject("cmbItemType.ItemHeight")));
      this.cmbItemType.Location = ((System.Drawing.Point)(resources.GetObject("cmbItemType.Location")));
      this.cmbItemType.MaxDropDownItems = ((int)(resources.GetObject("cmbItemType.MaxDropDownItems")));
      this.cmbItemType.MaxLength = ((int)(resources.GetObject("cmbItemType.MaxLength")));
      this.cmbItemType.Name = "cmbItemType";
      this.cmbItemType.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("cmbItemType.RightToLeft")));
      this.cmbItemType.Size = ((System.Drawing.Size)(resources.GetObject("cmbItemType.Size")));
      this.cmbItemType.Sorted = true;
      this.cmbItemType.TabIndex = ((int)(resources.GetObject("cmbItemType.TabIndex")));
      this.cmbItemType.Text = resources.GetString("cmbItemType.Text");
      this.cmbItemType.Visible = ((bool)(resources.GetObject("cmbItemType.Visible")));
      this.cmbItemType.SelectedIndexChanged += new System.EventHandler(this.cmbItemType_SelectedIndexChanged);
      // 
      // cmbLanguage
      // 
      this.cmbLanguage.AccessibleDescription = resources.GetString("cmbLanguage.AccessibleDescription");
      this.cmbLanguage.AccessibleName = resources.GetString("cmbLanguage.AccessibleName");
      this.cmbLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("cmbLanguage.Anchor")));
      this.cmbLanguage.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("cmbLanguage.BackgroundImage")));
      this.cmbLanguage.DisplayMember = "Name";
      this.cmbLanguage.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("cmbLanguage.Dock")));
      this.cmbLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbLanguage.Enabled = ((bool)(resources.GetObject("cmbLanguage.Enabled")));
      this.cmbLanguage.Font = ((System.Drawing.Font)(resources.GetObject("cmbLanguage.Font")));
      this.cmbLanguage.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("cmbLanguage.ImeMode")));
      this.cmbLanguage.IntegralHeight = ((bool)(resources.GetObject("cmbLanguage.IntegralHeight")));
      this.cmbLanguage.ItemHeight = ((int)(resources.GetObject("cmbLanguage.ItemHeight")));
      this.cmbLanguage.Location = ((System.Drawing.Point)(resources.GetObject("cmbLanguage.Location")));
      this.cmbLanguage.MaxDropDownItems = ((int)(resources.GetObject("cmbLanguage.MaxDropDownItems")));
      this.cmbLanguage.MaxLength = ((int)(resources.GetObject("cmbLanguage.MaxLength")));
      this.cmbLanguage.Name = "cmbLanguage";
      this.cmbLanguage.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("cmbLanguage.RightToLeft")));
      this.cmbLanguage.Size = ((System.Drawing.Size)(resources.GetObject("cmbLanguage.Size")));
      this.cmbLanguage.Sorted = true;
      this.cmbLanguage.TabIndex = ((int)(resources.GetObject("cmbLanguage.TabIndex")));
      this.cmbLanguage.Text = resources.GetString("cmbLanguage.Text");
      this.cmbLanguage.Visible = ((bool)(resources.GetObject("cmbLanguage.Visible")));
      this.cmbLanguage.SelectedIndexChanged += new System.EventHandler(this.cmbLanguage_SelectedIndexChanged);
      // 
      // grpFolder
      // 
      this.grpFolder.AccessibleDescription = resources.GetString("grpFolder.AccessibleDescription");
      this.grpFolder.AccessibleName = resources.GetString("grpFolder.AccessibleName");
      this.grpFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("grpFolder.Anchor")));
      this.grpFolder.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpFolder.BackgroundImage")));
      this.grpFolder.Controls.Add(this.cmbTextEncoding);
      this.grpFolder.Controls.Add(this.lblTextEncoding);
      this.grpFolder.Controls.Add(this.txtFolder);
      this.grpFolder.Controls.Add(this.btnBrowseFolder);
      this.grpFolder.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("grpFolder.Dock")));
      this.grpFolder.Enabled = ((bool)(resources.GetObject("grpFolder.Enabled")));
      this.grpFolder.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.grpFolder.Font = ((System.Drawing.Font)(resources.GetObject("grpFolder.Font")));
      this.grpFolder.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("grpFolder.ImeMode")));
      this.grpFolder.Location = ((System.Drawing.Point)(resources.GetObject("grpFolder.Location")));
      this.grpFolder.Name = "grpFolder";
      this.grpFolder.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("grpFolder.RightToLeft")));
      this.grpFolder.Size = ((System.Drawing.Size)(resources.GetObject("grpFolder.Size")));
      this.grpFolder.TabIndex = ((int)(resources.GetObject("grpFolder.TabIndex")));
      this.grpFolder.TabStop = false;
      this.grpFolder.Text = resources.GetString("grpFolder.Text");
      this.grpFolder.Visible = ((bool)(resources.GetObject("grpFolder.Visible")));
      // 
      // cmbTextEncoding
      // 
      this.cmbTextEncoding.AccessibleDescription = resources.GetString("cmbTextEncoding.AccessibleDescription");
      this.cmbTextEncoding.AccessibleName = resources.GetString("cmbTextEncoding.AccessibleName");
      this.cmbTextEncoding.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("cmbTextEncoding.Anchor")));
      this.cmbTextEncoding.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("cmbTextEncoding.BackgroundImage")));
      this.cmbTextEncoding.DisplayMember = "EncodingName";
      this.cmbTextEncoding.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("cmbTextEncoding.Dock")));
      this.cmbTextEncoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbTextEncoding.Enabled = ((bool)(resources.GetObject("cmbTextEncoding.Enabled")));
      this.cmbTextEncoding.Font = ((System.Drawing.Font)(resources.GetObject("cmbTextEncoding.Font")));
      this.cmbTextEncoding.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("cmbTextEncoding.ImeMode")));
      this.cmbTextEncoding.IntegralHeight = ((bool)(resources.GetObject("cmbTextEncoding.IntegralHeight")));
      this.cmbTextEncoding.ItemHeight = ((int)(resources.GetObject("cmbTextEncoding.ItemHeight")));
      this.cmbTextEncoding.Location = ((System.Drawing.Point)(resources.GetObject("cmbTextEncoding.Location")));
      this.cmbTextEncoding.MaxDropDownItems = ((int)(resources.GetObject("cmbTextEncoding.MaxDropDownItems")));
      this.cmbTextEncoding.MaxLength = ((int)(resources.GetObject("cmbTextEncoding.MaxLength")));
      this.cmbTextEncoding.Name = "cmbTextEncoding";
      this.cmbTextEncoding.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("cmbTextEncoding.RightToLeft")));
      this.cmbTextEncoding.Size = ((System.Drawing.Size)(resources.GetObject("cmbTextEncoding.Size")));
      this.cmbTextEncoding.TabIndex = ((int)(resources.GetObject("cmbTextEncoding.TabIndex")));
      this.cmbTextEncoding.Text = resources.GetString("cmbTextEncoding.Text");
      this.cmbTextEncoding.Visible = ((bool)(resources.GetObject("cmbTextEncoding.Visible")));
      // 
      // lblTextEncoding
      // 
      this.lblTextEncoding.AccessibleDescription = resources.GetString("lblTextEncoding.AccessibleDescription");
      this.lblTextEncoding.AccessibleName = resources.GetString("lblTextEncoding.AccessibleName");
      this.lblTextEncoding.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblTextEncoding.Anchor")));
      this.lblTextEncoding.AutoSize = ((bool)(resources.GetObject("lblTextEncoding.AutoSize")));
      this.lblTextEncoding.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblTextEncoding.Dock")));
      this.lblTextEncoding.Enabled = ((bool)(resources.GetObject("lblTextEncoding.Enabled")));
      this.lblTextEncoding.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblTextEncoding.Font = ((System.Drawing.Font)(resources.GetObject("lblTextEncoding.Font")));
      this.lblTextEncoding.Image = ((System.Drawing.Image)(resources.GetObject("lblTextEncoding.Image")));
      this.lblTextEncoding.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblTextEncoding.ImageAlign")));
      this.lblTextEncoding.ImageIndex = ((int)(resources.GetObject("lblTextEncoding.ImageIndex")));
      this.lblTextEncoding.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblTextEncoding.ImeMode")));
      this.lblTextEncoding.Location = ((System.Drawing.Point)(resources.GetObject("lblTextEncoding.Location")));
      this.lblTextEncoding.Name = "lblTextEncoding";
      this.lblTextEncoding.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblTextEncoding.RightToLeft")));
      this.lblTextEncoding.Size = ((System.Drawing.Size)(resources.GetObject("lblTextEncoding.Size")));
      this.lblTextEncoding.TabIndex = ((int)(resources.GetObject("lblTextEncoding.TabIndex")));
      this.lblTextEncoding.Text = resources.GetString("lblTextEncoding.Text");
      this.lblTextEncoding.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblTextEncoding.TextAlign")));
      this.lblTextEncoding.Visible = ((bool)(resources.GetObject("lblTextEncoding.Visible")));
      // 
      // txtFolder
      // 
      this.txtFolder.AccessibleDescription = resources.GetString("txtFolder.AccessibleDescription");
      this.txtFolder.AccessibleName = resources.GetString("txtFolder.AccessibleName");
      this.txtFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtFolder.Anchor")));
      this.txtFolder.AutoSize = ((bool)(resources.GetObject("txtFolder.AutoSize")));
      this.txtFolder.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtFolder.BackgroundImage")));
      this.txtFolder.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtFolder.Dock")));
      this.txtFolder.Enabled = ((bool)(resources.GetObject("txtFolder.Enabled")));
      this.txtFolder.Font = ((System.Drawing.Font)(resources.GetObject("txtFolder.Font")));
      this.txtFolder.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtFolder.ImeMode")));
      this.txtFolder.Location = ((System.Drawing.Point)(resources.GetObject("txtFolder.Location")));
      this.txtFolder.MaxLength = ((int)(resources.GetObject("txtFolder.MaxLength")));
      this.txtFolder.Multiline = ((bool)(resources.GetObject("txtFolder.Multiline")));
      this.txtFolder.Name = "txtFolder";
      this.txtFolder.PasswordChar = ((char)(resources.GetObject("txtFolder.PasswordChar")));
      this.txtFolder.ReadOnly = true;
      this.txtFolder.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtFolder.RightToLeft")));
      this.txtFolder.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtFolder.ScrollBars")));
      this.txtFolder.Size = ((System.Drawing.Size)(resources.GetObject("txtFolder.Size")));
      this.txtFolder.TabIndex = ((int)(resources.GetObject("txtFolder.TabIndex")));
      this.txtFolder.Text = resources.GetString("txtFolder.Text");
      this.txtFolder.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtFolder.TextAlign")));
      this.txtFolder.Visible = ((bool)(resources.GetObject("txtFolder.Visible")));
      this.txtFolder.WordWrap = ((bool)(resources.GetObject("txtFolder.WordWrap")));
      // 
      // btnBrowseFolder
      // 
      this.btnBrowseFolder.AccessibleDescription = resources.GetString("btnBrowseFolder.AccessibleDescription");
      this.btnBrowseFolder.AccessibleName = resources.GetString("btnBrowseFolder.AccessibleName");
      this.btnBrowseFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnBrowseFolder.Anchor")));
      this.btnBrowseFolder.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBrowseFolder.BackgroundImage")));
      this.btnBrowseFolder.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnBrowseFolder.Dock")));
      this.btnBrowseFolder.Enabled = ((bool)(resources.GetObject("btnBrowseFolder.Enabled")));
      this.btnBrowseFolder.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnBrowseFolder.FlatStyle")));
      this.btnBrowseFolder.Font = ((System.Drawing.Font)(resources.GetObject("btnBrowseFolder.Font")));
      this.btnBrowseFolder.Image = ((System.Drawing.Image)(resources.GetObject("btnBrowseFolder.Image")));
      this.btnBrowseFolder.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBrowseFolder.ImageAlign")));
      this.btnBrowseFolder.ImageIndex = ((int)(resources.GetObject("btnBrowseFolder.ImageIndex")));
      this.btnBrowseFolder.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnBrowseFolder.ImeMode")));
      this.btnBrowseFolder.Location = ((System.Drawing.Point)(resources.GetObject("btnBrowseFolder.Location")));
      this.btnBrowseFolder.Name = "btnBrowseFolder";
      this.btnBrowseFolder.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnBrowseFolder.RightToLeft")));
      this.btnBrowseFolder.Size = ((System.Drawing.Size)(resources.GetObject("btnBrowseFolder.Size")));
      this.btnBrowseFolder.TabIndex = ((int)(resources.GetObject("btnBrowseFolder.TabIndex")));
      this.btnBrowseFolder.Text = resources.GetString("btnBrowseFolder.Text");
      this.btnBrowseFolder.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnBrowseFolder.TextAlign")));
      this.btnBrowseFolder.Visible = ((bool)(resources.GetObject("btnBrowseFolder.Visible")));
      this.btnBrowseFolder.Click += new System.EventHandler(this.btnBrowseFolder_Click);
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
      this.Controls.Add(this.grpFolder);
      this.Controls.Add(this.grpDisplayMode);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnOK);
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
      this.grpDisplayMode.ResumeLayout(false);
      this.grpFolder.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    #region Events

    private void btnBrowseFolder_Click(object sender, System.EventArgs e) {
      IItemExporter.BrowseForOutputPath();
      this.txtFolder.Text = IItemExporter.OutputPath;
    }

    private void cmbLanguage_SelectedIndexChanged(object sender, System.EventArgs e) {
      this.ResetFields();
    }

    private void cmbItemType_SelectedIndexChanged(object sender, System.EventArgs e) {
      this.ResetFields();
    }

    private void btnChooseFields_Click(object sender, System.EventArgs e) {
      using (ItemFieldChooser IFC = new ItemFieldChooser(this.Language, this.Type, false, this.Fields)) {
	if (IFC.ShowDialog(this) == DialogResult.OK)
	  this.Fields_ = IFC.Fields;
      }
    }

    #endregion

  }

}
