using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

using PlayOnline.Core;

namespace EngrishOnry {

  internal class MainWindow : System.Windows.Forms.Form {

    #region Controls

    private System.Windows.Forms.RichTextBox rtbActivityLog;
    private System.Windows.Forms.Label lblActivityLog;
    private System.Windows.Forms.Button btnTranslateItemData;
    private System.Windows.Forms.Button btnRestoreItemData;
    private System.Windows.Forms.Button btnTranslateAutoTrans;
    private System.Windows.Forms.Button btnRestoreAutoTrans;

    private System.ComponentModel.Container components = null;

    #endregion

    public MainWindow() {
      InitializeComponent();
      this.Icon = Icons.POLViewer;
    }

    private void AddLogEntry(string Text) {
      this.rtbActivityLog.Text += String.Format("[{0}] {1}\n", DateTime.Now.ToString(), Text);
      Application.DoEvents();
    }

    private string GetROMPath(short App, short Dir, short FileID) {
    string ROMDir = "Rom";
      if (App > 0)
	ROMDir += String.Format("{0}", App + 1);
      return Path.Combine(Path.Combine(POL.GetApplicationPath(AppID.FFXI), ROMDir), Path.Combine(String.Format("{0}", Dir), String.Format("{0}.DAT", FileID)));
    }

    private string MakeBackupFolder(bool CreateIfNeeded) {
      try {
      string BackupFolder = Path.Combine(POL.GetApplicationPath(AppID.FFXI), "EngrishFFXI-Backup");
	if (CreateIfNeeded && !Directory.Exists(BackupFolder)) {
	  this.AddLogEntry(String.Format("Creating backup folder: {0}", BackupFolder));
	  Directory.CreateDirectory(BackupFolder);
	}
	return BackupFolder;
      }
      catch {
	this.AddLogEntry("*** MakeBackupFolder() FAILED ***");
	return null;
      }
    }

    private string MakeBackupFolder() {
      return this.MakeBackupFolder(true);
    }

    private bool BackupFile(short App, short Dir, short FileID) {
      try {
      string BackupName = Path.Combine(this.MakeBackupFolder(), String.Format("{0}-{1}-{2}.dat", App, Dir, FileID));
	if (!File.Exists(BackupName)) {
	string OriginalName = this.GetROMPath(App, Dir, FileID);
	  this.AddLogEntry(String.Format("Backing up: {0}", OriginalName));
	  File.Copy(OriginalName, BackupName);
	}
	return true;
      }
      catch {
	this.AddLogEntry("*** BackupFile() FAILED ***");
	return false;
      }
    }

    private bool RestoreFile(short App, short Dir, short FileID) {
      try {
      string BackupName = Path.Combine(this.MakeBackupFolder(false), String.Format("{0}-{1}-{2}.dat", App, Dir, FileID));
	if (File.Exists(BackupName)) {
	string OriginalName = this.GetROMPath(App, Dir, FileID);
	  this.AddLogEntry(String.Format("Restoring: {0}", OriginalName));
	  File.Copy(BackupName, OriginalName, true);
	}
	return true;
      }
      catch {
	this.AddLogEntry("*** RestoreFile() FAILED ***");
	return false;
      }
    }

    private void TranslateItemFile(short JApp, short JDir, short JFileID, short EApp, short EDir, short EFileID, short Type) {
      if (!this.BackupFile(JApp, JDir, JFileID))
	return;
      try {
      string JFileName = this.GetROMPath(JApp, JDir, JFileID);
      string EFileName = this.GetROMPath(EApp, EDir, EFileID);
	this.AddLogEntry(String.Format("Translating item data file: {0}", JFileName));
      FileStream JStream = new FileStream(JFileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
      FileStream EStream = new FileStream(EFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
	if ((JStream.Length % 0xc00) != 0) {
	  this.AddLogEntry("File size suggests this isn't item data - Translation Aborted");
	  goto TranslationDone;
	}
	if (JStream.Length != EStream.Length) {
	  this.AddLogEntry("File Size (JP vs EN) does not match - Translation Aborted");
	  goto TranslationDone;
	}
      long ItemCount = JStream.Length / 0xc00;
      byte[] JData = new byte[0x200];
      byte[] EData = new byte[0x200];
	for (long i = 0; i < ItemCount; ++i) {
	  JStream.Seek(i * 0xc00, SeekOrigin.Begin); JStream.Read(JData, 0, 0x200);
	  EStream.Seek(i * 0xc00, SeekOrigin.Begin); EStream.Read(EData, 0, 0x200);
	  switch (Type) {
	    case 0: // Item
	      Array.Copy(EData, 0x24, JData, 0x0E, 0x16); // Name
	      Array.Copy(EData, 0xC6, JData, 0x3A, 0xBC); // Description
	      break;
	    case 1: // Weapon
	      Array.Copy(EData, 0x34, JData, 0x1E, 0x16); // Name
	      Array.Copy(EData, 0xD6, JData, 0x4A, 0xBC); // Description
	      break;
	    case 2: // Armor
	      Array.Copy(EData, 0x2E, JData, 0x18, 0x16); // Name
	      Array.Copy(EData, 0xD0, JData, 0x44, 0xBC); // Description
	      break;
	  }
	  JStream.Seek(i * 0xc00, SeekOrigin.Begin); JStream.Write(JData, 0, 0x200);
	}
      TranslationDone:
	JStream.Close();
	EStream.Close();
      }
      catch {
	this.AddLogEntry("*** TranslateItemFile() FAILED ***");
      }
    }

    private void TranslateAutoTranslator() {
      if (!this.BackupFile(0, 76, 23))
	return;
    FileStream ATStream = null;
      try {
	ATStream = new FileStream(this.GetROMPath(0, 76, 23), FileMode.Open, FileAccess.ReadWrite, FileShare.None);
	if (ATStream.Length < 1024 * 1024) {
	byte[] ATData = new byte[ATStream.Length];
	  ATStream.Read(ATData, 0, (int) ATStream.Length);
	BinaryReader BR = new BinaryReader(new MemoryStream(ATData, false));
	Hashtable EnglishMessages = new Hashtable();
	  while (BR.BaseStream.Position != BR.BaseStream.Length) { // Scan through, recording the location of english messages
	  uint GroupID = BR.ReadUInt32();
	    if ((GroupID & 0xffff) == 0x202) {
	      EnglishMessages[(GroupID >> 16) & 0xffff] = BR.BaseStream.Position;
	      BR.BaseStream.Seek(32 + 32, SeekOrigin.Current);
	    uint MessageCount = BR.ReadUInt32();
	      BR.BaseStream.Seek(4, SeekOrigin.Current);
	      for (uint i = 0; i < MessageCount; ++i) {
	      uint MessageID = BR.ReadUInt32();
		EnglishMessages[(MessageID >> 16) & 0xffff] = BR.BaseStream.Position;
		BR.BaseStream.Seek(BR.ReadUInt32(), SeekOrigin.Current);
	      }
	    }
	    else { // Skip the group header and data
	      BR.BaseStream.Seek(32 + 32 + 4, SeekOrigin.Current);
	      BR.BaseStream.Seek(BR.ReadUInt32(), SeekOrigin.Current);
	    }
	  }
	  ATStream.Seek(0, SeekOrigin.Begin);
	BinaryWriter BW = new BinaryWriter(ATStream);
	  BR.BaseStream.Seek(0, SeekOrigin.Begin);
	  while (BR.BaseStream.Position != BR.BaseStream.Length) { // Scan through, recording the location of english messages
	  uint GroupID = BR.ReadUInt32();
	    BW.Write(GroupID);
	  long ENGroupPos = 0;
	    if ((GroupID & 0xffff) != 0x202 && EnglishMessages.ContainsKey((GroupID >> 16) & 0xffff))
	      ENGroupPos = (long) EnglishMessages[(GroupID >> 16) & 0xffff];
	    if (ENGroupPos == 0) { // EN, or JP without EN equivalent
	      BW.Write(BR.ReadBytes(32 + 32 + 4));
	    byte[] GroupBytes = BR.ReadBytes(BR.ReadInt32());
	      BW.Write(GroupBytes.Length); BW.Write(GroupBytes);
	    }
	    else {
	      BR.BaseStream.Seek(32 + 32, SeekOrigin.Current); // Skip "old" names
	      {
	      long CurPos = BR.BaseStream.Position;
		BR.BaseStream.Seek(ENGroupPos, SeekOrigin.Begin);
		BW.Write(BR.ReadBytes(32 + 32));
		BR.BaseStream.Seek(CurPos, SeekOrigin.Begin);
	      }
	    uint MessageCount = BR.ReadUInt32();
	      BW.Write(MessageCount);
	    long SizePos = BW.BaseStream.Position; // we'll be back here to update the group size
	      BW.Write(BR.ReadUInt32());
	      for (uint i = 0; i < MessageCount; ++i) {
	      uint MessageID = BR.ReadUInt32();
		BW.Write(MessageID);
	      long ENMessagePos = 0;
		if (EnglishMessages.ContainsKey((MessageID >> 16) & 0xffff))
		  ENMessagePos =  (long) EnglishMessages[(MessageID >> 16) & 0xffff];
		if (ENMessagePos == 0) { // Simply copy
		byte[] Text = BR.ReadBytes(BR.ReadInt32());
		  BW.Write(Text.Length); BW.Write(Text);
		  Text = BR.ReadBytes(BR.ReadInt32());
		  BW.Write(Text.Length); BW.Write(Text);
		}
		else {
		byte[] ENText = null;
		  { // Skip back and grab english text
		  long CurPos = BR.BaseStream.Position;
		    BR.BaseStream.Seek(ENMessagePos, SeekOrigin.Begin);
		    ENText = BR.ReadBytes(BR.ReadInt32());
		    BR.BaseStream.Seek(CurPos, SeekOrigin.Begin);
		  }
		  // Get jp text and skip secondary JP text
		byte[] JPText = BR.ReadBytes(BR.ReadInt32());
		  BR.BaseStream.Seek(BR.ReadUInt32(), SeekOrigin.Current);
		  // Write english text and use old jp text as secondary
		  BW.Write(ENText.Length); BW.Write(ENText);
		  BW.Write(JPText.Length); BW.Write(JPText);
		}
	      }
	      // Update group size
	    uint GroupSize = (uint) (BW.BaseStream.Position - SizePos - 4);
	      BW.BaseStream.Seek(SizePos, SeekOrigin.Begin);
	      BW.Write(GroupSize);
	      BW.BaseStream.Seek(GroupSize, SeekOrigin.Current);
	    }
	  }
	  ATStream.SetLength(ATStream.Position);
	  BR.Close();
	}
	else
	  this.AddLogEntry("*** AutoTranslator file seems too large - translation aborted ***");
      }
      catch (Exception E) {
	this.AddLogEntry("*** TranslateAutoTranslator() FAILED ***");
	this.AddLogEntry(E.ToString());
      }
      finally {
	if (ATStream != null)
	  ATStream.Close();
      }
    }

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
	components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      this.rtbActivityLog = new System.Windows.Forms.RichTextBox();
      this.lblActivityLog = new System.Windows.Forms.Label();
      this.btnTranslateItemData = new System.Windows.Forms.Button();
      this.btnRestoreItemData = new System.Windows.Forms.Button();
      this.btnTranslateAutoTrans = new System.Windows.Forms.Button();
      this.btnRestoreAutoTrans = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // rtbActivityLog
      // 
      this.rtbActivityLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
	| System.Windows.Forms.AnchorStyles.Left) 
	| System.Windows.Forms.AnchorStyles.Right)));
      this.rtbActivityLog.Location = new System.Drawing.Point(4, 100);
      this.rtbActivityLog.Name = "rtbActivityLog";
      this.rtbActivityLog.ReadOnly = true;
      this.rtbActivityLog.Size = new System.Drawing.Size(612, 132);
      this.rtbActivityLog.TabIndex = 0;
      this.rtbActivityLog.Text = "";
      // 
      // lblActivityLog
      // 
      this.lblActivityLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
	| System.Windows.Forms.AnchorStyles.Right)));
      this.lblActivityLog.Location = new System.Drawing.Point(4, 80);
      this.lblActivityLog.Name = "lblActivityLog";
      this.lblActivityLog.Size = new System.Drawing.Size(612, 16);
      this.lblActivityLog.TabIndex = 1;
      this.lblActivityLog.Text = "Activity Log:";
      // 
      // btnTranslateItemData
      // 
      this.btnTranslateItemData.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.btnTranslateItemData.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnTranslateItemData.Location = new System.Drawing.Point(180, 8);
      this.btnTranslateItemData.Name = "btnTranslateItemData";
      this.btnTranslateItemData.Size = new System.Drawing.Size(112, 28);
      this.btnTranslateItemData.TabIndex = 2;
      this.btnTranslateItemData.Text = "Translate ItemData";
      this.btnTranslateItemData.Click += new System.EventHandler(this.btnTranslateItemData_Click);
      // 
      // btnRestoreItemData
      // 
      this.btnRestoreItemData.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.btnRestoreItemData.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnRestoreItemData.Location = new System.Drawing.Point(180, 40);
      this.btnRestoreItemData.Name = "btnRestoreItemData";
      this.btnRestoreItemData.Size = new System.Drawing.Size(112, 28);
      this.btnRestoreItemData.TabIndex = 3;
      this.btnRestoreItemData.Text = "Restore Item Data";
      this.btnRestoreItemData.Click += new System.EventHandler(this.btnRestoreItemData_Click);
      // 
      // btnTranslateAutoTrans
      // 
      this.btnTranslateAutoTrans.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.btnTranslateAutoTrans.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnTranslateAutoTrans.Location = new System.Drawing.Point(296, 8);
      this.btnTranslateAutoTrans.Name = "btnTranslateAutoTrans";
      this.btnTranslateAutoTrans.Size = new System.Drawing.Size(144, 28);
      this.btnTranslateAutoTrans.TabIndex = 4;
      this.btnTranslateAutoTrans.Text = "Translate AutoTranslator";
      this.btnTranslateAutoTrans.Click += new System.EventHandler(this.btnTranslateAutoTrans_Click);
      // 
      // btnRestoreAutoTrans
      // 
      this.btnRestoreAutoTrans.Anchor = System.Windows.Forms.AnchorStyles.Top;
      this.btnRestoreAutoTrans.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnRestoreAutoTrans.Location = new System.Drawing.Point(296, 40);
      this.btnRestoreAutoTrans.Name = "btnRestoreAutoTrans";
      this.btnRestoreAutoTrans.Size = new System.Drawing.Size(144, 28);
      this.btnRestoreAutoTrans.TabIndex = 5;
      this.btnRestoreAutoTrans.Text = "Restore Auto-Translator";
      this.btnRestoreAutoTrans.Click += new System.EventHandler(this.btnRestoreAutoTrans_Click);
      // 
      // MainWindow
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(620, 238);
      this.Controls.Add(this.btnRestoreAutoTrans);
      this.Controls.Add(this.btnTranslateAutoTrans);
      this.Controls.Add(this.btnRestoreItemData);
      this.Controls.Add(this.btnTranslateItemData);
      this.Controls.Add(this.lblActivityLog);
      this.Controls.Add(this.rtbActivityLog);
      this.Name = "MainWindow";
      this.Text = "Make JP FFXI Engrish!";
      this.ResumeLayout(false);

    }

    #endregion

    #region Events

    private void btnTranslateItemData_Click(object sender, System.EventArgs e) {
      this.TranslateItemFile(0, 0, 4, 0, 118, 106, 0);
      this.TranslateItemFile(0, 0, 5, 0, 118, 107, 0);
      this.TranslateItemFile(0, 0, 6, 0, 118, 108, 1);
      this.TranslateItemFile(0, 0, 7, 0, 118, 109, 2);
      this.TranslateItemFile(0, 0, 8, 0, 118, 110, 0);
      this.AddLogEntry("=== Item Data Translation Completed ===");
    }

    private void btnRestoreItemData_Click(object sender, System.EventArgs e) {
      this.RestoreFile(0, 0, 4);
      this.RestoreFile(0, 0, 5);
      this.RestoreFile(0, 0, 6);
      this.RestoreFile(0, 0, 7);
      this.RestoreFile(0, 0, 8);
      this.AddLogEntry("=== Item Data Restoration Completed ===");
    }

    private void btnTranslateAutoTrans_Click(object sender, System.EventArgs e) {
      this.TranslateAutoTranslator();
      this.AddLogEntry("=== Auto-Translator Translation Completed ===");
    }

    private void btnRestoreAutoTrans_Click(object sender, System.EventArgs e) {
      this.RestoreFile(0, 76, 23);
      this.AddLogEntry("=== Auto-Translator Restoration Completed ===");
    }

    #endregion

  }

}
