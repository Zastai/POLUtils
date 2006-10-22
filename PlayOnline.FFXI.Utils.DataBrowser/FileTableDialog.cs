// $Id$

using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using PlayOnline.Core;

namespace PlayOnline.FFXI.Utils.DataBrowser {

  internal partial class FileTableDialog : Form {

    public FileTableDialog() {
      this.InitializeComponent();
      this.lstFileTable.ColumnClick += new ColumnClickEventHandler(ListViewColumnSorter.ListView_ColumnClick);
    }

    private void PopulateList() {
      this.lstFileTable.HeaderStyle = ColumnHeaderStyle.Nonclickable;
    string DataRoot = POL.GetApplicationPath(AppID.FFXI);
      for (int i = 1; i < 10; ++i) {
      string AppName = I18N.GetText(String.Format("FFXI{0}", i));
      string Suffix = "";
      string DataDir = DataRoot;
	if (i > 1) {
	  Suffix = i.ToString();
	  DataDir = Path.Combine(DataRoot, "Rom" + Suffix);
	}
      string VTableFile = Path.Combine(DataDir, String.Format("VTABLE{0}.DAT", Suffix));
      string FTableFile = Path.Combine(DataDir, String.Format("FTABLE{0}.DAT", Suffix));
	if (i == 1) // add the Rom now (not needed for the *TABLE.DAT, but needed for the other DAT paths)
	  DataDir = Path.Combine(DataRoot, "Rom");
	if (File.Exists(VTableFile) && File.Exists(FTableFile)) {
	  try {
	  BinaryReader VBR = new BinaryReader(new FileStream(VTableFile, FileMode.Open, FileAccess.Read, FileShare.Read));
	  BinaryReader FBR = new BinaryReader(new FileStream(FTableFile, FileMode.Open, FileAccess.Read, FileShare.Read));
	  long MaxFileNo = VBR.BaseStream.Length;
	    this.stbStatus.Text = AppName;
	  long FileCount = 0;
	    for (long FileNo = 0; FileNo < MaxFileNo; ++FileNo) {
	      if (VBR.ReadByte() == i) {
	      ListViewItem LVI = this.lstFileTable.Items.Add(String.Format("{0:D6}", FileNo));
		LVI.SubItems.Add(AppName);
		FBR.BaseStream.Seek(2 * FileNo, SeekOrigin.Begin);
	      ushort Location = FBR.ReadUInt16();
		LVI.SubItems.Add(Path.Combine(DataDir, String.Format("{0}{1}{2}.DAT", Location / 0x80, Path.DirectorySeparatorChar, Location % 0x80)));
		++FileCount;
		if ((FileCount % 100) == 0) {
		  this.stbStatus.Text = String.Format(I18N.GetText("FileTableDialog:StatusFormat"), AppName, FileCount);
		  Application.DoEvents();
		}
	      }
	    }
	    VBR.Close();
	    FBR.Close();
	  }
	  catch (Exception E) { Console.WriteLine("{0}", E.ToString()); }
	}
      }
      Application.DoEvents();
      this.lstFileTable.HeaderStyle = ColumnHeaderStyle.Clickable;
      this.stbStatus.Text = String.Empty;
      Application.DoEvents();
    }

    private void FileTableDialog_Activated(object sender, System.EventArgs e) {
      if (this.Visible && this.lstFileTable.Items.Count == 0)
	this.PopulateList();
    }

  }

}
