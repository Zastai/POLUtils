using System;
using System.IO;
using System.Threading;
using System.Text;
using System.Windows.Forms;

using PlayOnline.Core;
using PlayOnline.FFXI;

namespace PlayOnline.FFXI.Utils.DataBrowser {

  internal class FileScanner {

    private FileScanDialog FSD = new FileScanDialog();

    public ThingList FileContents = null;

    public void ScanFile(IWin32Window ParentForm, string FileName) {
      lock (this.FSD) {
	if (FileName != null && File.Exists(FileName)) {
	  this.FSD = new FileScanDialog();
	Thread T = new Thread(new ThreadStart(delegate () {
	  try {
	    Application.DoEvents();
	    Thread.Sleep(0);
	    Application.DoEvents();
	    this.FSD.Invoke(new AnonymousMethod(delegate() { this.FSD.ResetProgress(); }));
	    this.FileContents = FileType.LoadAll(FileName, new FileType.ProgressCallback(
	      delegate (string Message, double PercentCompleted) {
		this.FSD.Invoke(new AnonymousMethod(delegate() { this.FSD.SetProgress(Message, PercentCompleted); }));
	      }
	    ));
	    this.FSD.Invoke(new AnonymousMethod(delegate() { this.FSD.Finish(); }));
	  } catch(Exception E) { this.FileContents = null; }
	}));
	  T.CurrentUICulture = Thread.CurrentThread.CurrentUICulture;
	  T.Start();
	  if (this.FSD.ShowDialog(ParentForm) == DialogResult.Abort)
	    this.FileContents = null;
	  this.FSD.Dispose();
	  this.FSD = null;
	}
      }
    }

  }

}
