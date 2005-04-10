// If this is defined, the scanning is done in a thread.  This has the positive effect of being able to abort
// the scan by closing the window, but the negative effect of making the scan much slower...
//#define ThreadBased

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
#if ThreadBased
using System.Threading;
#endif
using System.Windows.Forms;

using PlayOnline.Core;
using PlayOnline.FFXI;

namespace PlayOnline.Utils.FFXIDataBrowser {

  internal class FileScanDialog : System.Windows.Forms.Form {

    private string FileName;

    public ArrayList StringTableEntries = new ArrayList();
    public ArrayList Images             = new ArrayList();
    public ArrayList Items              = new ArrayList();

    #region Controls

    private System.Windows.Forms.ProgressBar prbScanProgress;
    private System.Windows.Forms.Label lblScanProgress;

    private System.ComponentModel.Container components = null;

    #endregion

    public FileScanDialog(string FileName) {
      InitializeComponent();
      this.FileName = FileName;
      this.DialogResult = DialogResult.None;
#if !ThreadBased
      this.ControlBox = false;
#endif
    }

    #region Scanners

    private void ScanFile() {
      if (FileName != null && File.Exists(FileName)) {
	this.prbScanProgress.Value = 0;
	this.prbScanProgress.Visible = true;
      BinaryReader BR = null;
	try {
	  BR = new BinaryReader(new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read), Encoding.ASCII);
	} catch { }
	if (BR != null && BR.BaseStream.CanSeek) {
  	  Application.DoEvents();
	  BR.BaseStream.Seek(0, SeekOrigin.Begin);
	  this.ScanXIStringFile(BR);
	  if (this.StringTableEntries.Count == 0) {
  	    Application.DoEvents();
	    BR.BaseStream.Seek(0, SeekOrigin.Begin);
	    this.ScanItemData(BR);
	    if (this.Items.Count == 0) {
  	      Application.DoEvents();
	      BR.BaseStream.Seek(0, SeekOrigin.Begin);
	      this.ScanImages(BR);
	    }
	  }
	  BR.Close();
	}
      }
#if ThreadBased
      this.ScanThread = null;
#else
      this.Scanning = false;
#endif
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void SetProgress(long Current, long Max) {
      this.prbScanProgress.Value = (int) (Math.Min((decimal) Current / Max, 1.0M) * this.prbScanProgress.Maximum);
      Application.DoEvents();
    }

    #region Images

    private void ScanImages(BinaryReader BR) {
      if (BR.BaseStream.Length < 421) // one image header
	return;
    long MaxPos = BR.BaseStream.Length - 421;
    long Pos = 0;
    int ImageCount = 0;
      this.lblScanProgress.Text = String.Format(I18N.GetText("ImageScan"), ImageCount);
      this.prbScanProgress.Value = 0;
      BR.BaseStream.Seek(Pos, SeekOrigin.Begin);
      while (Pos < MaxPos) {
      FFXIGraphic FG = FFXIGraphic.Read(BR);
	if (FG != null) {
	  this.Images.Add(FG);
	  Pos = BR.BaseStream.Position;
	  this.SetProgress(Pos, MaxPos);
	  this.lblScanProgress.Text = String.Format(I18N.GetText("ImageScan"), ++ImageCount);
	}
	else {
	  BR.BaseStream.Seek(++Pos, SeekOrigin.Begin);
	  if (Pos == MaxPos || (Pos % 0x100) == 0)
	    this.SetProgress(Pos, MaxPos);
	}
      }
    }

    #endregion

    #region Item Data

    private void ScanItemData(BinaryReader BR) {
      this.lblScanProgress.Text = I18N.GetText("ItemCheck");
      this.prbScanProgress.Value = 0;
      if (BR.BaseStream.Length < 0xc00 || (BR.BaseStream.Length % 0xc00) != 0)
	return;
      BR.BaseStream.Seek(0, SeekOrigin.Begin);
    long ItemCount = BR.BaseStream.Length / 0xc00;
      for (long i = 0; i < ItemCount; ++i) {
	this.SetProgress(i + 1, ItemCount);
	this.lblScanProgress.Text = String.Format(I18N.GetText("ItemLoad"), i, ItemCount);
      byte[] ItemData  = BR.ReadBytes(0xc00);
	for (int j = 0; j < 0xc00; ++j)
	  ItemData[j] = (byte) ((ItemData[j] << 3) | (ItemData[j] >> 5));
      FFXIGraphic ItemIcon = null;
	{
	BinaryReader ImageBR = new BinaryReader(new MemoryStream(ItemData, 0x200, 0xa00, false, false));
	int ImageDataSize = ImageBR.ReadInt32();
	  if (ImageDataSize > 0)
	    ItemIcon = FFXIGraphic.Read(ImageBR);
	}
	if (ItemIcon == null) { // No image -> bad data
	  this.Items.Clear();
	  this.Images.Clear();
	  return;
	}
	this.Items.Add(new FFXIItem(i + 1, ItemData, ItemIcon));
	this.Images.Add(ItemIcon);
      }
      this.lblScanProgress.Text = String.Format(I18N.GetText("ItemLoad"), ItemCount, ItemCount);
    }

    #endregion

    #region String Tables

    private void ScanXIStringFile(BinaryReader BR) {
      this.lblScanProgress.Text = I18N.GetText("StringCheck");
      this.prbScanProgress.Value = 0;
      BR.BaseStream.Seek(0, SeekOrigin.Begin);
      if (BR.BaseStream.Length < 0x38)
	return;
    Encoding E = new FFXIEncoding();
      // Read past the marker (32 bytes)
      if ((E.GetString(BR.ReadBytes(10)) != "XISTRING".PadRight(10, '\0')) || BR.ReadUInt16() != 2)
	return;
      foreach (byte B in BR.ReadBytes(20)) {
	if (B != 0)
	  return;
      }
      // Read The Header
    uint FileSize = BR.ReadUInt32();
      if (FileSize != BR.BaseStream.Length)
	return;
    uint EntryCount = BR.ReadUInt32();
    uint EntryBytes = BR.ReadUInt32();
    uint DataBytes  = BR.ReadUInt32();
      BR.ReadUInt32(); // Unknown
      BR.ReadUInt32(); // Unknown
      if (EntryBytes != EntryCount * 12 || FileSize != 0x38 + EntryBytes + DataBytes)
	return;
      this.lblScanProgress.Text = I18N.GetText("StringScan");
      for (int i = 0; i < EntryCount; ++i) {
      uint  Offset = BR.ReadUInt32();
      short Size = BR.ReadInt16();
        BR.ReadUInt16(); // Unknown (0 or 1; so probably a flag of some sort)
	BR.ReadUInt32(); // Unknown
	if (Size > 0 && Offset + Size <= DataBytes) {
	long IndexPos = BR.BaseStream.Position;
	  BR.BaseStream.Seek(0x38 + EntryBytes + Offset, SeekOrigin.Begin);
	string Text = E.GetString(BR.ReadBytes(Size)).TrimEnd('\0');
	  BR.BaseStream.Seek(IndexPos, SeekOrigin.Begin);
	  this.StringTableEntries.Add(Text);
	}
	else
	  this.StringTableEntries.Add(I18N.GetText("InvalidEntry"));
	this.SetProgress(i, EntryCount);
      }
    }

    #endregion

    #endregion

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
	components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(FileScanDialog));
      this.prbScanProgress = new System.Windows.Forms.ProgressBar();
      this.lblScanProgress = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // prbScanProgress
      // 
      this.prbScanProgress.AccessibleDescription = resources.GetString("prbScanProgress.AccessibleDescription");
      this.prbScanProgress.AccessibleName = resources.GetString("prbScanProgress.AccessibleName");
      this.prbScanProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("prbScanProgress.Anchor")));
      this.prbScanProgress.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("prbScanProgress.BackgroundImage")));
      this.prbScanProgress.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("prbScanProgress.Dock")));
      this.prbScanProgress.Enabled = ((bool)(resources.GetObject("prbScanProgress.Enabled")));
      this.prbScanProgress.Font = ((System.Drawing.Font)(resources.GetObject("prbScanProgress.Font")));
      this.prbScanProgress.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("prbScanProgress.ImeMode")));
      this.prbScanProgress.Location = ((System.Drawing.Point)(resources.GetObject("prbScanProgress.Location")));
      this.prbScanProgress.Maximum = 1000;
      this.prbScanProgress.Name = "prbScanProgress";
      this.prbScanProgress.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("prbScanProgress.RightToLeft")));
      this.prbScanProgress.Size = ((System.Drawing.Size)(resources.GetObject("prbScanProgress.Size")));
      this.prbScanProgress.TabIndex = ((int)(resources.GetObject("prbScanProgress.TabIndex")));
      this.prbScanProgress.Text = resources.GetString("prbScanProgress.Text");
      this.prbScanProgress.Visible = ((bool)(resources.GetObject("prbScanProgress.Visible")));
      // 
      // lblScanProgress
      // 
      this.lblScanProgress.AccessibleDescription = resources.GetString("lblScanProgress.AccessibleDescription");
      this.lblScanProgress.AccessibleName = resources.GetString("lblScanProgress.AccessibleName");
      this.lblScanProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblScanProgress.Anchor")));
      this.lblScanProgress.AutoSize = ((bool)(resources.GetObject("lblScanProgress.AutoSize")));
      this.lblScanProgress.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblScanProgress.Dock")));
      this.lblScanProgress.Enabled = ((bool)(resources.GetObject("lblScanProgress.Enabled")));
      this.lblScanProgress.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblScanProgress.Font = ((System.Drawing.Font)(resources.GetObject("lblScanProgress.Font")));
      this.lblScanProgress.Image = ((System.Drawing.Image)(resources.GetObject("lblScanProgress.Image")));
      this.lblScanProgress.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblScanProgress.ImageAlign")));
      this.lblScanProgress.ImageIndex = ((int)(resources.GetObject("lblScanProgress.ImageIndex")));
      this.lblScanProgress.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblScanProgress.ImeMode")));
      this.lblScanProgress.Location = ((System.Drawing.Point)(resources.GetObject("lblScanProgress.Location")));
      this.lblScanProgress.Name = "lblScanProgress";
      this.lblScanProgress.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblScanProgress.RightToLeft")));
      this.lblScanProgress.Size = ((System.Drawing.Size)(resources.GetObject("lblScanProgress.Size")));
      this.lblScanProgress.TabIndex = ((int)(resources.GetObject("lblScanProgress.TabIndex")));
      this.lblScanProgress.Text = resources.GetString("lblScanProgress.Text");
      this.lblScanProgress.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblScanProgress.TextAlign")));
      this.lblScanProgress.Visible = ((bool)(resources.GetObject("lblScanProgress.Visible")));
      // 
      // FileScanDialog
      // 
      this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
      this.AccessibleName = resources.GetString("$this.AccessibleName");
      this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
      this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
      this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
      this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
      this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
      this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
      this.Controls.Add(this.prbScanProgress);
      this.Controls.Add(this.lblScanProgress);
      this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
      this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
      this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
      this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
      this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
      this.Name = "FileScanDialog";
      this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
      this.ShowInTaskbar = false;
      this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
      this.Text = resources.GetString("$this.Text");
      this.Closing += new System.ComponentModel.CancelEventHandler(this.FileScanDialog_Closing);
      this.Activated += new System.EventHandler(this.FileScanDialog_Activated);
      this.ResumeLayout(false);

    }

    #endregion

#if ThreadBased

    private Thread ScanThread = null;

    private void FileScanDialog_Activated(object sender, System.EventArgs e) {
      lock (this) {
	if (this.ScanThread != null)
	  return;
	this.ScanThread = new Thread(new ThreadStart(this.ScanFile));
	this.ScanThread.Start();
      }
    }

    private void FileScanDialog_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
      if (this.ScanThread != null) {
	this.ScanThread.Abort();
	this.ScanThread = null;
	this.StringTableEntries.Clear();
	this.Images.Clear();
	this.Items.Clear();
	this.DialogResult = DialogResult.Abort;
      }
    }

#else

    private bool Scanning = false;

    private void FileScanDialog_Activated(object sender, System.EventArgs e) {
      lock (this) {
	if (this.Scanning)
	  return;
	this.Scanning = true;
	this.ScanFile();
      }
    }

    private void FileScanDialog_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
    }

#endif

  }

}
