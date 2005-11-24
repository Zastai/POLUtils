// $Id$

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace PlayOnline.FFXI.Utils.DataBrowser {

  internal partial class SettingsDialog : Form {

    public SettingsDialog() {
      this.InitializeComponent();
      this.RetrieveSettings();
    }

    private void RetrieveSettings() {
      this.chkFSShowProgressDetails.Checked = FileScanDialog.ShowProgressDetails;
      this.chkFSAllowAbort.Checked          = FileScanDialog.AllowAbort;
    }

    private void StoreSettings() {
      FileScanDialog.ShowProgressDetails = this.chkFSShowProgressDetails.Checked;
      FileScanDialog.AllowAbort          = this.chkFSAllowAbort.Checked;
    }

    private void btnOK_Click(object sender, System.EventArgs e) {
      this.StoreSettings();
    }

  }

}
