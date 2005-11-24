// $Id$

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;

using PlayOnline.Core;

namespace PlayOnline.FFXI.Utils.DataBrowser {

  internal partial class CSVOptionDialog : Form {

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
