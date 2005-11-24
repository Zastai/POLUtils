// $Id$

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace PlayOnline.FFXI.Utils.DataBrowser {

  internal partial class ExportMethodDialog : Form {

    public ExportMethodDialog() {
      this.InitializeComponent();
    }

    public ItemExportMethod SelectedMethod {
      get {
	if (this.optCSV.Checked)
	  return ItemExportMethod.CSV;
	if (this.optXML.Checked)
	  return ItemExportMethod.XML;
	return ItemExportMethod.UserSelect;
      }
    }

  }

}
