// $Id$

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PlayOnline.FFXI.Utils.DataBrowser {

  internal partial class FileScanDialog : Form {

    public FileScanDialog() {
      InitializeComponent();
      this.DialogResult = DialogResult.Abort;
    }

    public void ResetProgress() {
      this.prbScanProgress.Text    = String.Empty;
      this.prbScanProgress.Value   = 0;
      this.prbScanProgress.Visible = true;
    }

    public void SetProgress(string Message, double PercentCompleted) {
      if (Message != null)
	this.lblScanProgress.Text = Message;
      this.prbScanProgress.Value = Math.Min((int) (PercentCompleted * this.prbScanProgress.Maximum), this.prbScanProgress.Maximum);
    }

    public void Finish() {
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

  }

}
