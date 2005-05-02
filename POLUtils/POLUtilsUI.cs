using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

using PlayOnline.Core;

namespace POLUtils {

  public class POLUtilsUI : System.Windows.Forms.Form {

    #region Controls

    private System.Windows.Forms.GroupBox grpRegion;
    private System.Windows.Forms.Label    lblSelectedRegion;
    private System.Windows.Forms.TextBox  txtSelectedRegion;
    private System.Windows.Forms.Button   btnChooseRegion;
    private System.Windows.Forms.ComboBox cmbCultures;

    private System.Windows.Forms.Button btnAudioManager;
    private System.Windows.Forms.Button btnFFXIMacroManager;
    private System.Windows.Forms.Button btnFFXIDataBrowser;
    private System.Windows.Forms.Button btnTetraViewer;
    private System.Windows.Forms.Label lblToolLanguage;
    private System.Windows.Forms.Button btnFFXIConfigEditor;

    private System.ComponentModel.Container components = null;

    #endregion

    public POLUtilsUI() {
      this.InitializeComponent();
      this.Icon = Icons.POLViewer;
      {
      Version V = Assembly.GetExecutingAssembly().GetName().Version;
	this.Text += String.Format(" {0}.{1}.{2}", V.Major, V.Minor, V.Revision);
      }
      this.cmbCultures.Items.AddRange(POLUtils.AvailableCultures.ToArray());
      this.cmbCultures.SelectedItem = CultureChoice.Current;
      if (this.cmbCultures.Items.Count < 2)
	this.cmbCultures.Enabled = false;
      this.Show();
      this.UpdateSelectedRegion();
    }

    private void UpdateSelectedRegion() {
      this.txtSelectedRegion.Text      = new NamedEnum(POL.SelectedRegion).Name;
      this.btnChooseRegion.Enabled     = POL.MultipleRegionsAvailable;
      this.btnFFXIConfigEditor.Enabled = POL.IsAppInstalled(AppID.FFXI);
      this.btnFFXIDataBrowser.Enabled  = POL.IsAppInstalled(AppID.FFXI);
      this.btnFFXIMacroManager.Enabled = POL.IsAppInstalled(AppID.FFXI);
      this.btnTetraViewer.Enabled      = POL.IsAppInstalled(AppID.TetraMaster);
    }

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
	components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(POLUtilsUI));
      this.btnTetraViewer = new System.Windows.Forms.Button();
      this.grpRegion = new System.Windows.Forms.GroupBox();
      this.cmbCultures = new System.Windows.Forms.ComboBox();
      this.btnChooseRegion = new System.Windows.Forms.Button();
      this.txtSelectedRegion = new System.Windows.Forms.TextBox();
      this.lblSelectedRegion = new System.Windows.Forms.Label();
      this.lblToolLanguage = new System.Windows.Forms.Label();
      this.btnAudioManager = new System.Windows.Forms.Button();
      this.btnFFXIMacroManager = new System.Windows.Forms.Button();
      this.btnFFXIDataBrowser = new System.Windows.Forms.Button();
      this.btnFFXIConfigEditor = new System.Windows.Forms.Button();
      this.grpRegion.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnTetraViewer
      // 
      this.btnTetraViewer.AccessibleDescription = resources.GetString("btnTetraViewer.AccessibleDescription");
      this.btnTetraViewer.AccessibleName = resources.GetString("btnTetraViewer.AccessibleName");
      this.btnTetraViewer.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnTetraViewer.Anchor")));
      this.btnTetraViewer.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnTetraViewer.BackgroundImage")));
      this.btnTetraViewer.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnTetraViewer.Dock")));
      this.btnTetraViewer.Enabled = ((bool)(resources.GetObject("btnTetraViewer.Enabled")));
      this.btnTetraViewer.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnTetraViewer.FlatStyle")));
      this.btnTetraViewer.Font = ((System.Drawing.Font)(resources.GetObject("btnTetraViewer.Font")));
      this.btnTetraViewer.Image = ((System.Drawing.Image)(resources.GetObject("btnTetraViewer.Image")));
      this.btnTetraViewer.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnTetraViewer.ImageAlign")));
      this.btnTetraViewer.ImageIndex = ((int)(resources.GetObject("btnTetraViewer.ImageIndex")));
      this.btnTetraViewer.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnTetraViewer.ImeMode")));
      this.btnTetraViewer.Location = ((System.Drawing.Point)(resources.GetObject("btnTetraViewer.Location")));
      this.btnTetraViewer.Name = "btnTetraViewer";
      this.btnTetraViewer.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnTetraViewer.RightToLeft")));
      this.btnTetraViewer.Size = ((System.Drawing.Size)(resources.GetObject("btnTetraViewer.Size")));
      this.btnTetraViewer.TabIndex = ((int)(resources.GetObject("btnTetraViewer.TabIndex")));
      this.btnTetraViewer.Text = resources.GetString("btnTetraViewer.Text");
      this.btnTetraViewer.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnTetraViewer.TextAlign")));
      this.btnTetraViewer.Visible = ((bool)(resources.GetObject("btnTetraViewer.Visible")));
      this.btnTetraViewer.Click += new System.EventHandler(this.btnTetraViewer_Click);
      // 
      // grpRegion
      // 
      this.grpRegion.AccessibleDescription = resources.GetString("grpRegion.AccessibleDescription");
      this.grpRegion.AccessibleName = resources.GetString("grpRegion.AccessibleName");
      this.grpRegion.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("grpRegion.Anchor")));
      this.grpRegion.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpRegion.BackgroundImage")));
      this.grpRegion.Controls.Add(this.cmbCultures);
      this.grpRegion.Controls.Add(this.btnChooseRegion);
      this.grpRegion.Controls.Add(this.txtSelectedRegion);
      this.grpRegion.Controls.Add(this.lblSelectedRegion);
      this.grpRegion.Controls.Add(this.lblToolLanguage);
      this.grpRegion.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("grpRegion.Dock")));
      this.grpRegion.Enabled = ((bool)(resources.GetObject("grpRegion.Enabled")));
      this.grpRegion.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.grpRegion.Font = ((System.Drawing.Font)(resources.GetObject("grpRegion.Font")));
      this.grpRegion.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("grpRegion.ImeMode")));
      this.grpRegion.Location = ((System.Drawing.Point)(resources.GetObject("grpRegion.Location")));
      this.grpRegion.Name = "grpRegion";
      this.grpRegion.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("grpRegion.RightToLeft")));
      this.grpRegion.Size = ((System.Drawing.Size)(resources.GetObject("grpRegion.Size")));
      this.grpRegion.TabIndex = ((int)(resources.GetObject("grpRegion.TabIndex")));
      this.grpRegion.TabStop = false;
      this.grpRegion.Text = resources.GetString("grpRegion.Text");
      this.grpRegion.Visible = ((bool)(resources.GetObject("grpRegion.Visible")));
      // 
      // cmbCultures
      // 
      this.cmbCultures.AccessibleDescription = resources.GetString("cmbCultures.AccessibleDescription");
      this.cmbCultures.AccessibleName = resources.GetString("cmbCultures.AccessibleName");
      this.cmbCultures.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("cmbCultures.Anchor")));
      this.cmbCultures.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("cmbCultures.BackgroundImage")));
      this.cmbCultures.DisplayMember = "Name";
      this.cmbCultures.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("cmbCultures.Dock")));
      this.cmbCultures.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbCultures.Enabled = ((bool)(resources.GetObject("cmbCultures.Enabled")));
      this.cmbCultures.Font = ((System.Drawing.Font)(resources.GetObject("cmbCultures.Font")));
      this.cmbCultures.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("cmbCultures.ImeMode")));
      this.cmbCultures.IntegralHeight = ((bool)(resources.GetObject("cmbCultures.IntegralHeight")));
      this.cmbCultures.ItemHeight = ((int)(resources.GetObject("cmbCultures.ItemHeight")));
      this.cmbCultures.Location = ((System.Drawing.Point)(resources.GetObject("cmbCultures.Location")));
      this.cmbCultures.MaxDropDownItems = ((int)(resources.GetObject("cmbCultures.MaxDropDownItems")));
      this.cmbCultures.MaxLength = ((int)(resources.GetObject("cmbCultures.MaxLength")));
      this.cmbCultures.Name = "cmbCultures";
      this.cmbCultures.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("cmbCultures.RightToLeft")));
      this.cmbCultures.Size = ((System.Drawing.Size)(resources.GetObject("cmbCultures.Size")));
      this.cmbCultures.TabIndex = ((int)(resources.GetObject("cmbCultures.TabIndex")));
      this.cmbCultures.Text = resources.GetString("cmbCultures.Text");
      this.cmbCultures.Visible = ((bool)(resources.GetObject("cmbCultures.Visible")));
      this.cmbCultures.SelectedIndexChanged += new System.EventHandler(this.cmbCultures_SelectedIndexChanged);
      // 
      // btnChooseRegion
      // 
      this.btnChooseRegion.AccessibleDescription = resources.GetString("btnChooseRegion.AccessibleDescription");
      this.btnChooseRegion.AccessibleName = resources.GetString("btnChooseRegion.AccessibleName");
      this.btnChooseRegion.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnChooseRegion.Anchor")));
      this.btnChooseRegion.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnChooseRegion.BackgroundImage")));
      this.btnChooseRegion.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnChooseRegion.Dock")));
      this.btnChooseRegion.Enabled = ((bool)(resources.GetObject("btnChooseRegion.Enabled")));
      this.btnChooseRegion.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnChooseRegion.FlatStyle")));
      this.btnChooseRegion.Font = ((System.Drawing.Font)(resources.GetObject("btnChooseRegion.Font")));
      this.btnChooseRegion.Image = ((System.Drawing.Image)(resources.GetObject("btnChooseRegion.Image")));
      this.btnChooseRegion.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnChooseRegion.ImageAlign")));
      this.btnChooseRegion.ImageIndex = ((int)(resources.GetObject("btnChooseRegion.ImageIndex")));
      this.btnChooseRegion.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnChooseRegion.ImeMode")));
      this.btnChooseRegion.Location = ((System.Drawing.Point)(resources.GetObject("btnChooseRegion.Location")));
      this.btnChooseRegion.Name = "btnChooseRegion";
      this.btnChooseRegion.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnChooseRegion.RightToLeft")));
      this.btnChooseRegion.Size = ((System.Drawing.Size)(resources.GetObject("btnChooseRegion.Size")));
      this.btnChooseRegion.TabIndex = ((int)(resources.GetObject("btnChooseRegion.TabIndex")));
      this.btnChooseRegion.Text = resources.GetString("btnChooseRegion.Text");
      this.btnChooseRegion.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnChooseRegion.TextAlign")));
      this.btnChooseRegion.Visible = ((bool)(resources.GetObject("btnChooseRegion.Visible")));
      this.btnChooseRegion.Click += new System.EventHandler(this.btnChooseRegion_Click);
      // 
      // txtSelectedRegion
      // 
      this.txtSelectedRegion.AccessibleDescription = resources.GetString("txtSelectedRegion.AccessibleDescription");
      this.txtSelectedRegion.AccessibleName = resources.GetString("txtSelectedRegion.AccessibleName");
      this.txtSelectedRegion.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtSelectedRegion.Anchor")));
      this.txtSelectedRegion.AutoSize = ((bool)(resources.GetObject("txtSelectedRegion.AutoSize")));
      this.txtSelectedRegion.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtSelectedRegion.BackgroundImage")));
      this.txtSelectedRegion.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtSelectedRegion.Dock")));
      this.txtSelectedRegion.Enabled = ((bool)(resources.GetObject("txtSelectedRegion.Enabled")));
      this.txtSelectedRegion.Font = ((System.Drawing.Font)(resources.GetObject("txtSelectedRegion.Font")));
      this.txtSelectedRegion.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtSelectedRegion.ImeMode")));
      this.txtSelectedRegion.Location = ((System.Drawing.Point)(resources.GetObject("txtSelectedRegion.Location")));
      this.txtSelectedRegion.MaxLength = ((int)(resources.GetObject("txtSelectedRegion.MaxLength")));
      this.txtSelectedRegion.Multiline = ((bool)(resources.GetObject("txtSelectedRegion.Multiline")));
      this.txtSelectedRegion.Name = "txtSelectedRegion";
      this.txtSelectedRegion.PasswordChar = ((char)(resources.GetObject("txtSelectedRegion.PasswordChar")));
      this.txtSelectedRegion.ReadOnly = true;
      this.txtSelectedRegion.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtSelectedRegion.RightToLeft")));
      this.txtSelectedRegion.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtSelectedRegion.ScrollBars")));
      this.txtSelectedRegion.Size = ((System.Drawing.Size)(resources.GetObject("txtSelectedRegion.Size")));
      this.txtSelectedRegion.TabIndex = ((int)(resources.GetObject("txtSelectedRegion.TabIndex")));
      this.txtSelectedRegion.TabStop = false;
      this.txtSelectedRegion.Text = resources.GetString("txtSelectedRegion.Text");
      this.txtSelectedRegion.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtSelectedRegion.TextAlign")));
      this.txtSelectedRegion.Visible = ((bool)(resources.GetObject("txtSelectedRegion.Visible")));
      this.txtSelectedRegion.WordWrap = ((bool)(resources.GetObject("txtSelectedRegion.WordWrap")));
      // 
      // lblSelectedRegion
      // 
      this.lblSelectedRegion.AccessibleDescription = resources.GetString("lblSelectedRegion.AccessibleDescription");
      this.lblSelectedRegion.AccessibleName = resources.GetString("lblSelectedRegion.AccessibleName");
      this.lblSelectedRegion.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblSelectedRegion.Anchor")));
      this.lblSelectedRegion.AutoSize = ((bool)(resources.GetObject("lblSelectedRegion.AutoSize")));
      this.lblSelectedRegion.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblSelectedRegion.Dock")));
      this.lblSelectedRegion.Enabled = ((bool)(resources.GetObject("lblSelectedRegion.Enabled")));
      this.lblSelectedRegion.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblSelectedRegion.Font = ((System.Drawing.Font)(resources.GetObject("lblSelectedRegion.Font")));
      this.lblSelectedRegion.Image = ((System.Drawing.Image)(resources.GetObject("lblSelectedRegion.Image")));
      this.lblSelectedRegion.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblSelectedRegion.ImageAlign")));
      this.lblSelectedRegion.ImageIndex = ((int)(resources.GetObject("lblSelectedRegion.ImageIndex")));
      this.lblSelectedRegion.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblSelectedRegion.ImeMode")));
      this.lblSelectedRegion.Location = ((System.Drawing.Point)(resources.GetObject("lblSelectedRegion.Location")));
      this.lblSelectedRegion.Name = "lblSelectedRegion";
      this.lblSelectedRegion.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblSelectedRegion.RightToLeft")));
      this.lblSelectedRegion.Size = ((System.Drawing.Size)(resources.GetObject("lblSelectedRegion.Size")));
      this.lblSelectedRegion.TabIndex = ((int)(resources.GetObject("lblSelectedRegion.TabIndex")));
      this.lblSelectedRegion.Text = resources.GetString("lblSelectedRegion.Text");
      this.lblSelectedRegion.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblSelectedRegion.TextAlign")));
      this.lblSelectedRegion.Visible = ((bool)(resources.GetObject("lblSelectedRegion.Visible")));
      // 
      // lblToolLanguage
      // 
      this.lblToolLanguage.AccessibleDescription = resources.GetString("lblToolLanguage.AccessibleDescription");
      this.lblToolLanguage.AccessibleName = resources.GetString("lblToolLanguage.AccessibleName");
      this.lblToolLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblToolLanguage.Anchor")));
      this.lblToolLanguage.AutoSize = ((bool)(resources.GetObject("lblToolLanguage.AutoSize")));
      this.lblToolLanguage.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblToolLanguage.Dock")));
      this.lblToolLanguage.Enabled = ((bool)(resources.GetObject("lblToolLanguage.Enabled")));
      this.lblToolLanguage.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblToolLanguage.Font = ((System.Drawing.Font)(resources.GetObject("lblToolLanguage.Font")));
      this.lblToolLanguage.Image = ((System.Drawing.Image)(resources.GetObject("lblToolLanguage.Image")));
      this.lblToolLanguage.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblToolLanguage.ImageAlign")));
      this.lblToolLanguage.ImageIndex = ((int)(resources.GetObject("lblToolLanguage.ImageIndex")));
      this.lblToolLanguage.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblToolLanguage.ImeMode")));
      this.lblToolLanguage.Location = ((System.Drawing.Point)(resources.GetObject("lblToolLanguage.Location")));
      this.lblToolLanguage.Name = "lblToolLanguage";
      this.lblToolLanguage.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblToolLanguage.RightToLeft")));
      this.lblToolLanguage.Size = ((System.Drawing.Size)(resources.GetObject("lblToolLanguage.Size")));
      this.lblToolLanguage.TabIndex = ((int)(resources.GetObject("lblToolLanguage.TabIndex")));
      this.lblToolLanguage.Text = resources.GetString("lblToolLanguage.Text");
      this.lblToolLanguage.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblToolLanguage.TextAlign")));
      this.lblToolLanguage.Visible = ((bool)(resources.GetObject("lblToolLanguage.Visible")));
      // 
      // btnAudioManager
      // 
      this.btnAudioManager.AccessibleDescription = resources.GetString("btnAudioManager.AccessibleDescription");
      this.btnAudioManager.AccessibleName = resources.GetString("btnAudioManager.AccessibleName");
      this.btnAudioManager.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnAudioManager.Anchor")));
      this.btnAudioManager.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAudioManager.BackgroundImage")));
      this.btnAudioManager.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnAudioManager.Dock")));
      this.btnAudioManager.Enabled = ((bool)(resources.GetObject("btnAudioManager.Enabled")));
      this.btnAudioManager.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnAudioManager.FlatStyle")));
      this.btnAudioManager.Font = ((System.Drawing.Font)(resources.GetObject("btnAudioManager.Font")));
      this.btnAudioManager.Image = ((System.Drawing.Image)(resources.GetObject("btnAudioManager.Image")));
      this.btnAudioManager.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnAudioManager.ImageAlign")));
      this.btnAudioManager.ImageIndex = ((int)(resources.GetObject("btnAudioManager.ImageIndex")));
      this.btnAudioManager.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnAudioManager.ImeMode")));
      this.btnAudioManager.Location = ((System.Drawing.Point)(resources.GetObject("btnAudioManager.Location")));
      this.btnAudioManager.Name = "btnAudioManager";
      this.btnAudioManager.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnAudioManager.RightToLeft")));
      this.btnAudioManager.Size = ((System.Drawing.Size)(resources.GetObject("btnAudioManager.Size")));
      this.btnAudioManager.TabIndex = ((int)(resources.GetObject("btnAudioManager.TabIndex")));
      this.btnAudioManager.Text = resources.GetString("btnAudioManager.Text");
      this.btnAudioManager.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnAudioManager.TextAlign")));
      this.btnAudioManager.Visible = ((bool)(resources.GetObject("btnAudioManager.Visible")));
      this.btnAudioManager.Click += new System.EventHandler(this.btnAudioManager_Click);
      // 
      // btnFFXIMacroManager
      // 
      this.btnFFXIMacroManager.AccessibleDescription = resources.GetString("btnFFXIMacroManager.AccessibleDescription");
      this.btnFFXIMacroManager.AccessibleName = resources.GetString("btnFFXIMacroManager.AccessibleName");
      this.btnFFXIMacroManager.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnFFXIMacroManager.Anchor")));
      this.btnFFXIMacroManager.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFFXIMacroManager.BackgroundImage")));
      this.btnFFXIMacroManager.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnFFXIMacroManager.Dock")));
      this.btnFFXIMacroManager.Enabled = ((bool)(resources.GetObject("btnFFXIMacroManager.Enabled")));
      this.btnFFXIMacroManager.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnFFXIMacroManager.FlatStyle")));
      this.btnFFXIMacroManager.Font = ((System.Drawing.Font)(resources.GetObject("btnFFXIMacroManager.Font")));
      this.btnFFXIMacroManager.Image = ((System.Drawing.Image)(resources.GetObject("btnFFXIMacroManager.Image")));
      this.btnFFXIMacroManager.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnFFXIMacroManager.ImageAlign")));
      this.btnFFXIMacroManager.ImageIndex = ((int)(resources.GetObject("btnFFXIMacroManager.ImageIndex")));
      this.btnFFXIMacroManager.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnFFXIMacroManager.ImeMode")));
      this.btnFFXIMacroManager.Location = ((System.Drawing.Point)(resources.GetObject("btnFFXIMacroManager.Location")));
      this.btnFFXIMacroManager.Name = "btnFFXIMacroManager";
      this.btnFFXIMacroManager.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnFFXIMacroManager.RightToLeft")));
      this.btnFFXIMacroManager.Size = ((System.Drawing.Size)(resources.GetObject("btnFFXIMacroManager.Size")));
      this.btnFFXIMacroManager.TabIndex = ((int)(resources.GetObject("btnFFXIMacroManager.TabIndex")));
      this.btnFFXIMacroManager.Text = resources.GetString("btnFFXIMacroManager.Text");
      this.btnFFXIMacroManager.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnFFXIMacroManager.TextAlign")));
      this.btnFFXIMacroManager.Visible = ((bool)(resources.GetObject("btnFFXIMacroManager.Visible")));
      this.btnFFXIMacroManager.Click += new System.EventHandler(this.btnFFXIMacroManager_Click);
      // 
      // btnFFXIDataBrowser
      // 
      this.btnFFXIDataBrowser.AccessibleDescription = resources.GetString("btnFFXIDataBrowser.AccessibleDescription");
      this.btnFFXIDataBrowser.AccessibleName = resources.GetString("btnFFXIDataBrowser.AccessibleName");
      this.btnFFXIDataBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnFFXIDataBrowser.Anchor")));
      this.btnFFXIDataBrowser.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFFXIDataBrowser.BackgroundImage")));
      this.btnFFXIDataBrowser.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnFFXIDataBrowser.Dock")));
      this.btnFFXIDataBrowser.Enabled = ((bool)(resources.GetObject("btnFFXIDataBrowser.Enabled")));
      this.btnFFXIDataBrowser.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnFFXIDataBrowser.FlatStyle")));
      this.btnFFXIDataBrowser.Font = ((System.Drawing.Font)(resources.GetObject("btnFFXIDataBrowser.Font")));
      this.btnFFXIDataBrowser.Image = ((System.Drawing.Image)(resources.GetObject("btnFFXIDataBrowser.Image")));
      this.btnFFXIDataBrowser.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnFFXIDataBrowser.ImageAlign")));
      this.btnFFXIDataBrowser.ImageIndex = ((int)(resources.GetObject("btnFFXIDataBrowser.ImageIndex")));
      this.btnFFXIDataBrowser.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnFFXIDataBrowser.ImeMode")));
      this.btnFFXIDataBrowser.Location = ((System.Drawing.Point)(resources.GetObject("btnFFXIDataBrowser.Location")));
      this.btnFFXIDataBrowser.Name = "btnFFXIDataBrowser";
      this.btnFFXIDataBrowser.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnFFXIDataBrowser.RightToLeft")));
      this.btnFFXIDataBrowser.Size = ((System.Drawing.Size)(resources.GetObject("btnFFXIDataBrowser.Size")));
      this.btnFFXIDataBrowser.TabIndex = ((int)(resources.GetObject("btnFFXIDataBrowser.TabIndex")));
      this.btnFFXIDataBrowser.Text = resources.GetString("btnFFXIDataBrowser.Text");
      this.btnFFXIDataBrowser.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnFFXIDataBrowser.TextAlign")));
      this.btnFFXIDataBrowser.Visible = ((bool)(resources.GetObject("btnFFXIDataBrowser.Visible")));
      this.btnFFXIDataBrowser.Click += new System.EventHandler(this.btnFFXIDataBrowser_Click);
      // 
      // btnFFXIConfigEditor
      // 
      this.btnFFXIConfigEditor.AccessibleDescription = resources.GetString("btnFFXIConfigEditor.AccessibleDescription");
      this.btnFFXIConfigEditor.AccessibleName = resources.GetString("btnFFXIConfigEditor.AccessibleName");
      this.btnFFXIConfigEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnFFXIConfigEditor.Anchor")));
      this.btnFFXIConfigEditor.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFFXIConfigEditor.BackgroundImage")));
      this.btnFFXIConfigEditor.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnFFXIConfigEditor.Dock")));
      this.btnFFXIConfigEditor.Enabled = ((bool)(resources.GetObject("btnFFXIConfigEditor.Enabled")));
      this.btnFFXIConfigEditor.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnFFXIConfigEditor.FlatStyle")));
      this.btnFFXIConfigEditor.Font = ((System.Drawing.Font)(resources.GetObject("btnFFXIConfigEditor.Font")));
      this.btnFFXIConfigEditor.Image = ((System.Drawing.Image)(resources.GetObject("btnFFXIConfigEditor.Image")));
      this.btnFFXIConfigEditor.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnFFXIConfigEditor.ImageAlign")));
      this.btnFFXIConfigEditor.ImageIndex = ((int)(resources.GetObject("btnFFXIConfigEditor.ImageIndex")));
      this.btnFFXIConfigEditor.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnFFXIConfigEditor.ImeMode")));
      this.btnFFXIConfigEditor.Location = ((System.Drawing.Point)(resources.GetObject("btnFFXIConfigEditor.Location")));
      this.btnFFXIConfigEditor.Name = "btnFFXIConfigEditor";
      this.btnFFXIConfigEditor.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnFFXIConfigEditor.RightToLeft")));
      this.btnFFXIConfigEditor.Size = ((System.Drawing.Size)(resources.GetObject("btnFFXIConfigEditor.Size")));
      this.btnFFXIConfigEditor.TabIndex = ((int)(resources.GetObject("btnFFXIConfigEditor.TabIndex")));
      this.btnFFXIConfigEditor.Text = resources.GetString("btnFFXIConfigEditor.Text");
      this.btnFFXIConfigEditor.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnFFXIConfigEditor.TextAlign")));
      this.btnFFXIConfigEditor.Visible = ((bool)(resources.GetObject("btnFFXIConfigEditor.Visible")));
      this.btnFFXIConfigEditor.Click += new System.EventHandler(this.btnFFXIConfigEditor_Click);
      // 
      // POLUtilsUI
      // 
      this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
      this.AccessibleName = resources.GetString("$this.AccessibleName");
      this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
      this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
      this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
      this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
      this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
      this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
      this.Controls.Add(this.btnFFXIConfigEditor);
      this.Controls.Add(this.btnFFXIDataBrowser);
      this.Controls.Add(this.btnFFXIMacroManager);
      this.Controls.Add(this.btnAudioManager);
      this.Controls.Add(this.grpRegion);
      this.Controls.Add(this.btnTetraViewer);
      this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
      this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
      this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
      this.MaximizeBox = false;
      this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
      this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
      this.Name = "POLUtilsUI";
      this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
      this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
      this.Text = resources.GetString("$this.Text");
      this.grpRegion.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    #region Language Selection Events

    private void cmbCultures_SelectedIndexChanged(object sender, System.EventArgs e) {
      if (!this.Visible || this.cmbCultures.SelectedItem == null)
	return;
    CultureChoice CC = this.cmbCultures.SelectedItem as CultureChoice;
      if (CC != null && CC.Name != CultureChoice.Current.Name) {
	CultureChoice.Current = CC;
	// Need to close and reopen the form to activate the changes
	POLUtils.KeepGoing = true;
	this.Close();
      }
    }

    #endregion

    #region Button Events

    private void btnChooseRegion_Click(object sender, System.EventArgs e) {
      POL.ChooseRegion(this);
      this.UpdateSelectedRegion();
    }

    private void btnAudioManager_Click(object sender, System.EventArgs e) {
      this.Hide();
      using (Form Utility = new PlayOnline.Utils.AudioManager.MainWindow())
	Utility.ShowDialog(this);
      this.Show();
      this.Activate();
    }

    private void btnFFXIConfigEditor_Click(object sender, System.EventArgs e) {
      this.Hide();
      using (Form Utility = new PlayOnline.FFXI.Utils.ConfigEditor.MainWindow())
	Utility.ShowDialog(this);
      this.Show();
      this.Activate();
    }

    private void btnFFXIDataBrowser_Click(object sender, System.EventArgs e) {
      this.Hide();
      using (Form Utility = new PlayOnline.FFXI.Utils.DataBrowser.MainWindow())
	Utility.ShowDialog(this);
      this.Show();
      this.Activate();
    }

    private void btnFFXIMacroManager_Click(object sender, System.EventArgs e) {
      this.Hide();
      using (Form Utility = new PlayOnline.FFXI.Utils.MacroManager.MainWindow())
	Utility.ShowDialog(this);
      this.Show();
      this.Activate();
    }

    private void btnTetraViewer_Click(object sender, System.EventArgs e) {
      this.Hide();
      using (Form Utility = new PlayOnline.Utils.TetraViewer.MainWindow())
	Utility.ShowDialog(this);
      this.Show();
      this.Activate();
    }

    #endregion

  }

}
