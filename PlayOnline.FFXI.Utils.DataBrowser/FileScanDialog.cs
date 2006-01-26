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

using PlayOnline.Core;

namespace PlayOnline.FFXI.Utils.DataBrowser {

  internal partial class FileScanDialog : Form {

    private string FileName;

    public static bool AllowAbort          = false;
    public static bool ShowProgressDetails = false;

    public FileScanDialog(string FileName) {
      InitializeComponent();
      this.FileName = FileName;
      this.DialogResult = DialogResult.None;
      this.ControlBox = FileScanDialog.AllowAbort;
    }

    public ThingList FileContents = new ThingList();

    private void ScanFile() {

      if (this.FileName != null && File.Exists(this.FileName)) {
	this.prbScanProgress.Text    = String.Empty;
	this.prbScanProgress.Value   = 0;
	this.prbScanProgress.Visible = true;
	this.FileContents = FileType.LoadAll(this.FileName, new FileType.ProgressCallback(
	  delegate (string Message, double PercentCompleted) {
	    if (Message != null)
	      this.lblScanProgress.Text = Message;
	    this.prbScanProgress.Value = Math.Min((int) (PercentCompleted * this.prbScanProgress.Maximum), this.prbScanProgress.Maximum);
	    Application.DoEvents();
	  }
	));
      }
      this.Scanning = false;
      this.ScanThread = null;
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private bool   Scanning   = false;
    private Thread ScanThread = null;

    private void FileScanDialog_Activated(object sender, System.EventArgs e) {
      lock (this) {
	if (this.Scanning)
	  return;
	this.Scanning = true;
	if (FileScanDialog.AllowAbort) {
	  this.ScanThread = new Thread(new ThreadStart(this.ScanFile));
	  this.ScanThread.Start();
	}
	else
	  this.ScanFile();
      }
    }

    private void FileScanDialog_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
      if (FileScanDialog.AllowAbort && this.ScanThread != null) {
	this.ScanThread.Abort();
	this.ScanThread = null;
	this.FileContents.Clear();
	this.DialogResult = DialogResult.Abort;
      }
    }

  }

}
