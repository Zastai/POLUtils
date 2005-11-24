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

  internal partial class XMLOptionDialog : Form {

    public XMLOptionDialog() {
      this.InitializeComponent();
      this.txtFolder.Text = IItemExporter.OutputPath;
      this.cmbLanguage.Items.AddRange(NamedEnum.GetAll(typeof(ItemDataLanguage)));
      this.cmbLanguage.SelectedIndex = 0; // English
      this.cmbItemType.Items.AddRange(NamedEnum.GetAll(typeof(ItemDataType)));
      this.cmbItemType.SelectedIndex = 1; // Object
    }

    public void Reset() {
      // About to be (re)used - so refresh the output path
      this.txtFolder.Text = IItemExporter.OutputPath;
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
      string File = String.Format("{0}-{1}-{2}.xml", ((this.Language == ItemDataLanguage.English) ? "en" : "jp"), this.Type.ToString().ToLower(), DateTime.Now.ToString("yyyyMMddHHmmss"));
	return Path.Combine(Folder, File);
      }
    }

    #endregion

    #region Events

    private void btnBrowseFolder_Click(object sender, System.EventArgs e) {
      IItemExporter.BrowseForOutputPath();
      this.txtFolder.Text = IItemExporter.OutputPath;
    }

    private void cmbLanguage_SelectedIndexChanged(object sender, System.EventArgs e) {
    }

    private void cmbItemType_SelectedIndexChanged(object sender, System.EventArgs e) {
    }

    #endregion

  }

}
