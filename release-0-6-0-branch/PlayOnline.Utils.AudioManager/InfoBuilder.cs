// $Id$

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using PlayOnline.Core;
using PlayOnline.Core.Audio;

namespace PlayOnline.Utils.AudioManager {

  public class InfoBuilder : System.Windows.Forms.Form {

    private System.Windows.Forms.ProgressBar prbApplication;
    private System.Windows.Forms.Label lblApplication;
    private System.Windows.Forms.TextBox txtApplication;
    private System.Windows.Forms.TextBox txtDirectory;
    private System.Windows.Forms.Label lblDirectory;
    private System.Windows.Forms.ProgressBar prbDirectory;
    private System.Windows.Forms.TextBox txtFile;
    private System.Windows.Forms.Label lblFile;
    private System.Windows.Forms.ProgressBar prbFile;
    private System.ComponentModel.Container components = null;

    public InfoBuilder() {
      InitializeComponent();
      this.prbApplication.Select();
    }

    protected override void Dispose(bool disposing) {
      if(disposing && components != null)
	components.Dispose();
      base.Dispose(disposing);
    }

    private TreeNode TargetNode_;
    public TreeNode TargetNode {
      get { return this.TargetNode_; }
      set { this.TargetNode_ = value; }
    }

    private string FileTypeName_;
    public string FileTypeName {
      get { return this.FileTypeName_; }
      set { this.FileTypeName_ = value; }
    }

    private string ResourceName_;
    public string ResourceName {
      get { return this.ResourceName_; }
      set { this.ResourceName_ = value; }
    }

    private string FilePattern_;
    public string FilePattern {
      get { return this.FilePattern_; }
      set { this.FilePattern_ = value; }
    }

    public void Run() {
    Stream InfoData = Assembly.GetExecutingAssembly().GetManifestResourceStream(this.ResourceName_);
      if (InfoData != null) {
      XmlReader XR = new XmlTextReader(InfoData);
	try {
	XmlDocument XD = new XmlDocument();
	  XD.Load(XR);
	XmlNodeList Apps = XD.SelectNodes("/pol-audio-data/application");
	  this.txtApplication.Text    = null;
	  this.prbApplication.Value   = 0;
	  this.prbApplication.Maximum = Apps.Count;
	  foreach (XmlNode App in Apps) {
	    try {
	    string AppName = App.Attributes["name"].InnerText;
	      this.txtApplication.Text = String.Format("[{0}/{1}] {2}", this.prbApplication.Value + 1, this.prbApplication.Maximum, AppName);
	      this.txtDirectory.Text   = "Scanning...";
	      this.txtFile.Text        = "Scanning...";
	      this.prbDirectory.Value  = 0;
	      this.prbFile.Value       = 0;
	      Application.DoEvents();
	    string AppPath = POL.GetApplicationPath(App.Attributes["id"].InnerText);
	      if (AppPath != null) {
	      TreeNode AppNode = new TreeNode(AppName);
		AppNode.ImageIndex = 1;
		AppNode.SelectedImageIndex = 1;
		AppNode.Tag = AppPath;
	      XmlNodeList DataPaths = App.SelectNodes("data-path");
		// Precompute totals for directories/files
		this.prbDirectory.Maximum = 0;
		this.prbFile.Maximum = 0;
		foreach (XmlNode DataPath in DataPaths)
		  this.PreScanDataPath(Path.Combine(AppPath, DataPath.InnerText.Replace('/', Path.DirectorySeparatorChar)));
		++this.prbApplication.Value;
		// Now do a full scan
		foreach (XmlNode DataPath in DataPaths)
		  this.ScanDataPath(App, Path.Combine(AppPath, DataPath.InnerText.Replace('/', Path.DirectorySeparatorChar)), AppNode);
		if (AppNode.Nodes.Count > 0)
		  this.TargetNode_.Nodes.Add(AppNode);
	      }
	      else
		++this.prbApplication.Value;
	    }
	    catch (Exception E) {
	      Console.WriteLine(E.ToString());
	    }
	  }
	}
	catch (Exception E) {
	  Console.WriteLine(E.ToString());
	}
	XR.Close();
	InfoData.Close();
      }
    }

    private void PreScanDataPath(string DataPath) {
      if (!Directory.Exists(DataPath))
	return;
      ++this.prbDirectory.Maximum;
      this.prbFile.Maximum += Directory.GetFiles(DataPath, this.FilePattern_).Length;
      foreach (string SubDir in Directory.GetDirectories(DataPath))
	this.PreScanDataPath(SubDir);
    }

    private void ScanDataPath(XmlNode App, string DataPath, TreeNode AppNode) {
      if (!Directory.Exists(DataPath))
	return;
      this.txtDirectory.Text = String.Format("[{0}/{1}] {2}", this.prbDirectory.Value + 1, this.prbDirectory.Maximum, DataPath);
      Application.DoEvents();
      foreach (string File in Directory.GetFiles(DataPath, this.FilePattern_)) {
	this.txtFile.Text = String.Format("[{0}/{1}] {2}", this.prbFile.Value + 1, this.prbFile.Maximum, Path.GetFileName(File));
	Application.DoEvents();
      AudioFile AF = new AudioFile(File);
	if (AF.Type != AudioFileType.Unknown) {
	FileInfo FI = new FileInfo(App, AF);
	TreeNode FileNode = new TreeNode(String.Format("[{0}] {1}", AF.ID, ((FI.Title == null) ? Path.GetFileName(File) : FI.Title)));
	  FileNode.ImageIndex = 3;
	  FileNode.SelectedImageIndex = 3;
	  FileNode.Tag = FI;
	  AppNode.Nodes.Add(FileNode);
	}
	++this.prbFile.Value;
      }
      ++this.prbDirectory.Value;
      // Recurse
      foreach (string SubDir in Directory.GetDirectories(DataPath)) {
      TreeNode DirNode = new TreeNode(Path.GetFileName(SubDir));
	DirNode.ImageIndex = 1;
	DirNode.SelectedImageIndex = 1;
      FileInfo FI = new FileInfo(App, SubDir);
	DirNode.Tag = FI;
	if (FI.Title != null)
	  DirNode.Text += String.Format(" - {0}", FI.Title);
	this.ScanDataPath(App, SubDir, DirNode);
	if (DirNode.Nodes.Count > 0)
	  AppNode.Nodes.Add(DirNode);
      }
    }

    #region Windows Form Designer generated code

    private void InitializeComponent() {
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(InfoBuilder));
      this.prbApplication = new System.Windows.Forms.ProgressBar();
      this.lblApplication = new System.Windows.Forms.Label();
      this.txtApplication = new System.Windows.Forms.TextBox();
      this.txtDirectory = new System.Windows.Forms.TextBox();
      this.lblDirectory = new System.Windows.Forms.Label();
      this.prbDirectory = new System.Windows.Forms.ProgressBar();
      this.txtFile = new System.Windows.Forms.TextBox();
      this.lblFile = new System.Windows.Forms.Label();
      this.prbFile = new System.Windows.Forms.ProgressBar();
      this.SuspendLayout();
      // 
      // prbApplication
      // 
      this.prbApplication.AccessibleDescription = resources.GetString("prbApplication.AccessibleDescription");
      this.prbApplication.AccessibleName = resources.GetString("prbApplication.AccessibleName");
      this.prbApplication.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("prbApplication.Anchor")));
      this.prbApplication.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("prbApplication.BackgroundImage")));
      this.prbApplication.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("prbApplication.Dock")));
      this.prbApplication.Enabled = ((bool)(resources.GetObject("prbApplication.Enabled")));
      this.prbApplication.Font = ((System.Drawing.Font)(resources.GetObject("prbApplication.Font")));
      this.prbApplication.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("prbApplication.ImeMode")));
      this.prbApplication.Location = ((System.Drawing.Point)(resources.GetObject("prbApplication.Location")));
      this.prbApplication.Name = "prbApplication";
      this.prbApplication.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("prbApplication.RightToLeft")));
      this.prbApplication.Size = ((System.Drawing.Size)(resources.GetObject("prbApplication.Size")));
      this.prbApplication.TabIndex = ((int)(resources.GetObject("prbApplication.TabIndex")));
      this.prbApplication.Text = resources.GetString("prbApplication.Text");
      this.prbApplication.Visible = ((bool)(resources.GetObject("prbApplication.Visible")));
      // 
      // lblApplication
      // 
      this.lblApplication.AccessibleDescription = resources.GetString("lblApplication.AccessibleDescription");
      this.lblApplication.AccessibleName = resources.GetString("lblApplication.AccessibleName");
      this.lblApplication.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblApplication.Anchor")));
      this.lblApplication.AutoSize = ((bool)(resources.GetObject("lblApplication.AutoSize")));
      this.lblApplication.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblApplication.Dock")));
      this.lblApplication.Enabled = ((bool)(resources.GetObject("lblApplication.Enabled")));
      this.lblApplication.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblApplication.Font = ((System.Drawing.Font)(resources.GetObject("lblApplication.Font")));
      this.lblApplication.Image = ((System.Drawing.Image)(resources.GetObject("lblApplication.Image")));
      this.lblApplication.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblApplication.ImageAlign")));
      this.lblApplication.ImageIndex = ((int)(resources.GetObject("lblApplication.ImageIndex")));
      this.lblApplication.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblApplication.ImeMode")));
      this.lblApplication.Location = ((System.Drawing.Point)(resources.GetObject("lblApplication.Location")));
      this.lblApplication.Name = "lblApplication";
      this.lblApplication.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblApplication.RightToLeft")));
      this.lblApplication.Size = ((System.Drawing.Size)(resources.GetObject("lblApplication.Size")));
      this.lblApplication.TabIndex = ((int)(resources.GetObject("lblApplication.TabIndex")));
      this.lblApplication.Text = resources.GetString("lblApplication.Text");
      this.lblApplication.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblApplication.TextAlign")));
      this.lblApplication.Visible = ((bool)(resources.GetObject("lblApplication.Visible")));
      // 
      // txtApplication
      // 
      this.txtApplication.AccessibleDescription = resources.GetString("txtApplication.AccessibleDescription");
      this.txtApplication.AccessibleName = resources.GetString("txtApplication.AccessibleName");
      this.txtApplication.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtApplication.Anchor")));
      this.txtApplication.AutoSize = ((bool)(resources.GetObject("txtApplication.AutoSize")));
      this.txtApplication.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtApplication.BackgroundImage")));
      this.txtApplication.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtApplication.Dock")));
      this.txtApplication.Enabled = ((bool)(resources.GetObject("txtApplication.Enabled")));
      this.txtApplication.Font = ((System.Drawing.Font)(resources.GetObject("txtApplication.Font")));
      this.txtApplication.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtApplication.ImeMode")));
      this.txtApplication.Location = ((System.Drawing.Point)(resources.GetObject("txtApplication.Location")));
      this.txtApplication.MaxLength = ((int)(resources.GetObject("txtApplication.MaxLength")));
      this.txtApplication.Multiline = ((bool)(resources.GetObject("txtApplication.Multiline")));
      this.txtApplication.Name = "txtApplication";
      this.txtApplication.PasswordChar = ((char)(resources.GetObject("txtApplication.PasswordChar")));
      this.txtApplication.ReadOnly = true;
      this.txtApplication.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtApplication.RightToLeft")));
      this.txtApplication.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtApplication.ScrollBars")));
      this.txtApplication.Size = ((System.Drawing.Size)(resources.GetObject("txtApplication.Size")));
      this.txtApplication.TabIndex = ((int)(resources.GetObject("txtApplication.TabIndex")));
      this.txtApplication.Text = resources.GetString("txtApplication.Text");
      this.txtApplication.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtApplication.TextAlign")));
      this.txtApplication.Visible = ((bool)(resources.GetObject("txtApplication.Visible")));
      this.txtApplication.WordWrap = ((bool)(resources.GetObject("txtApplication.WordWrap")));
      // 
      // txtDirectory
      // 
      this.txtDirectory.AccessibleDescription = resources.GetString("txtDirectory.AccessibleDescription");
      this.txtDirectory.AccessibleName = resources.GetString("txtDirectory.AccessibleName");
      this.txtDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtDirectory.Anchor")));
      this.txtDirectory.AutoSize = ((bool)(resources.GetObject("txtDirectory.AutoSize")));
      this.txtDirectory.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtDirectory.BackgroundImage")));
      this.txtDirectory.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtDirectory.Dock")));
      this.txtDirectory.Enabled = ((bool)(resources.GetObject("txtDirectory.Enabled")));
      this.txtDirectory.Font = ((System.Drawing.Font)(resources.GetObject("txtDirectory.Font")));
      this.txtDirectory.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtDirectory.ImeMode")));
      this.txtDirectory.Location = ((System.Drawing.Point)(resources.GetObject("txtDirectory.Location")));
      this.txtDirectory.MaxLength = ((int)(resources.GetObject("txtDirectory.MaxLength")));
      this.txtDirectory.Multiline = ((bool)(resources.GetObject("txtDirectory.Multiline")));
      this.txtDirectory.Name = "txtDirectory";
      this.txtDirectory.PasswordChar = ((char)(resources.GetObject("txtDirectory.PasswordChar")));
      this.txtDirectory.ReadOnly = true;
      this.txtDirectory.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtDirectory.RightToLeft")));
      this.txtDirectory.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtDirectory.ScrollBars")));
      this.txtDirectory.Size = ((System.Drawing.Size)(resources.GetObject("txtDirectory.Size")));
      this.txtDirectory.TabIndex = ((int)(resources.GetObject("txtDirectory.TabIndex")));
      this.txtDirectory.Text = resources.GetString("txtDirectory.Text");
      this.txtDirectory.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtDirectory.TextAlign")));
      this.txtDirectory.Visible = ((bool)(resources.GetObject("txtDirectory.Visible")));
      this.txtDirectory.WordWrap = ((bool)(resources.GetObject("txtDirectory.WordWrap")));
      // 
      // lblDirectory
      // 
      this.lblDirectory.AccessibleDescription = resources.GetString("lblDirectory.AccessibleDescription");
      this.lblDirectory.AccessibleName = resources.GetString("lblDirectory.AccessibleName");
      this.lblDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblDirectory.Anchor")));
      this.lblDirectory.AutoSize = ((bool)(resources.GetObject("lblDirectory.AutoSize")));
      this.lblDirectory.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblDirectory.Dock")));
      this.lblDirectory.Enabled = ((bool)(resources.GetObject("lblDirectory.Enabled")));
      this.lblDirectory.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblDirectory.Font = ((System.Drawing.Font)(resources.GetObject("lblDirectory.Font")));
      this.lblDirectory.Image = ((System.Drawing.Image)(resources.GetObject("lblDirectory.Image")));
      this.lblDirectory.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblDirectory.ImageAlign")));
      this.lblDirectory.ImageIndex = ((int)(resources.GetObject("lblDirectory.ImageIndex")));
      this.lblDirectory.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblDirectory.ImeMode")));
      this.lblDirectory.Location = ((System.Drawing.Point)(resources.GetObject("lblDirectory.Location")));
      this.lblDirectory.Name = "lblDirectory";
      this.lblDirectory.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblDirectory.RightToLeft")));
      this.lblDirectory.Size = ((System.Drawing.Size)(resources.GetObject("lblDirectory.Size")));
      this.lblDirectory.TabIndex = ((int)(resources.GetObject("lblDirectory.TabIndex")));
      this.lblDirectory.Text = resources.GetString("lblDirectory.Text");
      this.lblDirectory.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblDirectory.TextAlign")));
      this.lblDirectory.Visible = ((bool)(resources.GetObject("lblDirectory.Visible")));
      // 
      // prbDirectory
      // 
      this.prbDirectory.AccessibleDescription = resources.GetString("prbDirectory.AccessibleDescription");
      this.prbDirectory.AccessibleName = resources.GetString("prbDirectory.AccessibleName");
      this.prbDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("prbDirectory.Anchor")));
      this.prbDirectory.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("prbDirectory.BackgroundImage")));
      this.prbDirectory.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("prbDirectory.Dock")));
      this.prbDirectory.Enabled = ((bool)(resources.GetObject("prbDirectory.Enabled")));
      this.prbDirectory.Font = ((System.Drawing.Font)(resources.GetObject("prbDirectory.Font")));
      this.prbDirectory.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("prbDirectory.ImeMode")));
      this.prbDirectory.Location = ((System.Drawing.Point)(resources.GetObject("prbDirectory.Location")));
      this.prbDirectory.Name = "prbDirectory";
      this.prbDirectory.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("prbDirectory.RightToLeft")));
      this.prbDirectory.Size = ((System.Drawing.Size)(resources.GetObject("prbDirectory.Size")));
      this.prbDirectory.TabIndex = ((int)(resources.GetObject("prbDirectory.TabIndex")));
      this.prbDirectory.Text = resources.GetString("prbDirectory.Text");
      this.prbDirectory.Visible = ((bool)(resources.GetObject("prbDirectory.Visible")));
      // 
      // txtFile
      // 
      this.txtFile.AccessibleDescription = resources.GetString("txtFile.AccessibleDescription");
      this.txtFile.AccessibleName = resources.GetString("txtFile.AccessibleName");
      this.txtFile.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtFile.Anchor")));
      this.txtFile.AutoSize = ((bool)(resources.GetObject("txtFile.AutoSize")));
      this.txtFile.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtFile.BackgroundImage")));
      this.txtFile.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtFile.Dock")));
      this.txtFile.Enabled = ((bool)(resources.GetObject("txtFile.Enabled")));
      this.txtFile.Font = ((System.Drawing.Font)(resources.GetObject("txtFile.Font")));
      this.txtFile.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtFile.ImeMode")));
      this.txtFile.Location = ((System.Drawing.Point)(resources.GetObject("txtFile.Location")));
      this.txtFile.MaxLength = ((int)(resources.GetObject("txtFile.MaxLength")));
      this.txtFile.Multiline = ((bool)(resources.GetObject("txtFile.Multiline")));
      this.txtFile.Name = "txtFile";
      this.txtFile.PasswordChar = ((char)(resources.GetObject("txtFile.PasswordChar")));
      this.txtFile.ReadOnly = true;
      this.txtFile.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtFile.RightToLeft")));
      this.txtFile.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtFile.ScrollBars")));
      this.txtFile.Size = ((System.Drawing.Size)(resources.GetObject("txtFile.Size")));
      this.txtFile.TabIndex = ((int)(resources.GetObject("txtFile.TabIndex")));
      this.txtFile.Text = resources.GetString("txtFile.Text");
      this.txtFile.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtFile.TextAlign")));
      this.txtFile.Visible = ((bool)(resources.GetObject("txtFile.Visible")));
      this.txtFile.WordWrap = ((bool)(resources.GetObject("txtFile.WordWrap")));
      // 
      // lblFile
      // 
      this.lblFile.AccessibleDescription = resources.GetString("lblFile.AccessibleDescription");
      this.lblFile.AccessibleName = resources.GetString("lblFile.AccessibleName");
      this.lblFile.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblFile.Anchor")));
      this.lblFile.AutoSize = ((bool)(resources.GetObject("lblFile.AutoSize")));
      this.lblFile.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblFile.Dock")));
      this.lblFile.Enabled = ((bool)(resources.GetObject("lblFile.Enabled")));
      this.lblFile.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblFile.Font = ((System.Drawing.Font)(resources.GetObject("lblFile.Font")));
      this.lblFile.Image = ((System.Drawing.Image)(resources.GetObject("lblFile.Image")));
      this.lblFile.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblFile.ImageAlign")));
      this.lblFile.ImageIndex = ((int)(resources.GetObject("lblFile.ImageIndex")));
      this.lblFile.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblFile.ImeMode")));
      this.lblFile.Location = ((System.Drawing.Point)(resources.GetObject("lblFile.Location")));
      this.lblFile.Name = "lblFile";
      this.lblFile.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblFile.RightToLeft")));
      this.lblFile.Size = ((System.Drawing.Size)(resources.GetObject("lblFile.Size")));
      this.lblFile.TabIndex = ((int)(resources.GetObject("lblFile.TabIndex")));
      this.lblFile.Text = resources.GetString("lblFile.Text");
      this.lblFile.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblFile.TextAlign")));
      this.lblFile.Visible = ((bool)(resources.GetObject("lblFile.Visible")));
      // 
      // prbFile
      // 
      this.prbFile.AccessibleDescription = resources.GetString("prbFile.AccessibleDescription");
      this.prbFile.AccessibleName = resources.GetString("prbFile.AccessibleName");
      this.prbFile.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("prbFile.Anchor")));
      this.prbFile.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("prbFile.BackgroundImage")));
      this.prbFile.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("prbFile.Dock")));
      this.prbFile.Enabled = ((bool)(resources.GetObject("prbFile.Enabled")));
      this.prbFile.Font = ((System.Drawing.Font)(resources.GetObject("prbFile.Font")));
      this.prbFile.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("prbFile.ImeMode")));
      this.prbFile.Location = ((System.Drawing.Point)(resources.GetObject("prbFile.Location")));
      this.prbFile.Name = "prbFile";
      this.prbFile.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("prbFile.RightToLeft")));
      this.prbFile.Size = ((System.Drawing.Size)(resources.GetObject("prbFile.Size")));
      this.prbFile.TabIndex = ((int)(resources.GetObject("prbFile.TabIndex")));
      this.prbFile.Text = resources.GetString("prbFile.Text");
      this.prbFile.Visible = ((bool)(resources.GetObject("prbFile.Visible")));
      // 
      // InfoBuilder
      // 
      this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
      this.AccessibleName = resources.GetString("$this.AccessibleName");
      this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
      this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
      this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
      this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
      this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
      this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
      this.ControlBox = false;
      this.Controls.Add(this.txtFile);
      this.Controls.Add(this.txtDirectory);
      this.Controls.Add(this.txtApplication);
      this.Controls.Add(this.lblFile);
      this.Controls.Add(this.prbFile);
      this.Controls.Add(this.lblDirectory);
      this.Controls.Add(this.prbDirectory);
      this.Controls.Add(this.lblApplication);
      this.Controls.Add(this.prbApplication);
      this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
      this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
      this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
      this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
      this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
      this.Name = "InfoBuilder";
      this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
      this.ShowInTaskbar = false;
      this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
      this.Text = resources.GetString("$this.Text");
      this.VisibleChanged += new System.EventHandler(this.InfoBuilder_VisibleChanged);
      this.ResumeLayout(false);

    }

    #endregion

    private bool Running = false;

    private void InfoBuilder_VisibleChanged(object sender, System.EventArgs e) {
      if (this.TargetNode == null)
	throw new InvalidOperationException("No target node set.");
      lock (this) {
	if (this.Running)
	  return;
	this.Running = true;
      }
      this.Show();
      Application.DoEvents();
      this.Activate();
      Application.DoEvents();
      this.Refresh();
      Application.DoEvents();
      this.Run();
      this.Close();
    }

  }

}
