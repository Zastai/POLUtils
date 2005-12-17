// $Id$

using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml;
using System.Xml.Xsl;

namespace ItemListUpgrade {

  public partial class MainWindow : Form {

    [STAThread]
    static void Main() {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new MainWindow());
    }

    public MainWindow() {
      InitializeComponent();
    }

    // If possible, give the window that nice gradient look
    protected override void OnPaintBackground(PaintEventArgs e) {
      if (VisualStyleRenderer.IsSupported) {
      VisualStyleRenderer VSR = new VisualStyleRenderer(VisualStyleElement.Tab.Body.Normal);
	VSR.DrawBackground(e.Graphics, this.ClientRectangle, e.ClipRectangle);
      }
      else
	base.OnPaintBackground(e);
    }

    #region Applying the XSLT transform

    private XslCompiledTransform UpgradeTransform = null;

    private void PrepareTransform() {
      if (this.UpgradeTransform != null)
	return;
      try {
	this.UpgradeTransform = new XslCompiledTransform();
      XmlReader XR = new XmlTextReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("ItemListUpgrade.xslt"));
	this.UpgradeTransform.Load(XR);
	XR.Close();
      } catch {
	this.UpgradeTransform = null;
      }
    }

    private void PerformUpgrade(string OldListFile, string NewListFile) {
      this.PrepareTransform();
      if (this.UpgradeTransform != null) {
	try {
	XmlDocument XD = new XmlDocument();
	  XD.Load(OldListFile);
	XmlWriter XW = XmlTextWriter.Create(NewListFile, this.UpgradeTransform.OutputSettings);
	  this.UpgradeTransform.Transform(XD, XW);
	  XW.Close();
	} catch (Exception E) {
	  MessageBox.Show(this, String.Format("Applying the upgrade failed: {0}", E.Message), "Upgrade Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
	}
      }
      else
	MessageBox.Show(this, "Failed to prepare the upgrade transform.", "Upgrade Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    #endregion

    private void btnOldFile_Click(object sender, EventArgs e) {
      if (this.dlgOldFile.ShowDialog(this) == DialogResult.OK)
	this.txtOldFile.Text = this.dlgOldFile.FileName;
    }

    private void btnNewFile_Click(object sender, EventArgs e) {
      if (this.dlgNewFile.ShowDialog(this) == DialogResult.OK)
	this.txtNewFile.Text = this.dlgNewFile.FileName;
    }

    private void btnPerformUpgrade_Click(object sender, EventArgs e) {
      this.PerformUpgrade(this.txtOldFile.Text, this.txtNewFile.Text);
    }

    private void txtOldFile_TextChanged(object sender, EventArgs e) {
      this.btnPerformUpgrade.Enabled = (File.Exists(this.txtOldFile.Text) && this.txtNewFile.Text != String.Empty);
    }

    private void txtNewFile_TextChanged(object sender, EventArgs e) {
      this.btnPerformUpgrade.Enabled = (File.Exists(this.txtOldFile.Text) && this.txtNewFile.Text != String.Empty);
    }

  }

}