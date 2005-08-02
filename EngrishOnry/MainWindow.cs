// $Id$

using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

using PlayOnline.Core;

namespace EngrishOnry {

  internal class MainWindow : System.Windows.Forms.Form {

    #region Controls

    private System.Windows.Forms.Button btnTranslateItemData;
    private System.Windows.Forms.Button btnRestoreItemData;
    private System.Windows.Forms.Button btnTranslateAutoTrans;
    private System.Windows.Forms.Button btnRestoreAutoTrans;
    private System.Windows.Forms.Button btnRestoreDialogTables;
    private System.Windows.Forms.Button btnTranslateDialogTables;
    private System.Windows.Forms.Button btnRestoreSpellData;
    private System.Windows.Forms.Button btnTranslateSpellData;
    private System.Windows.Forms.Button btnRestoreStringTables;
    private System.Windows.Forms.Button btnTranslateStringTables;
    private System.Windows.Forms.Label lblDialogTables;
    private System.Windows.Forms.Label lblStringTables;
    private System.Windows.Forms.Label lblSpellData;
    private System.Windows.Forms.Label lblAutoTranslator;
    private System.Windows.Forms.Panel pnlLogPane;
    private System.Windows.Forms.Label lblActivityLog;
    private System.Windows.Forms.RichTextBox rtbActivityLog;
    private System.Windows.Forms.Label lblItemData;

    private System.ComponentModel.Container components = null;

    #endregion

    public MainWindow() {
      InitializeComponent();
      this.Icon = Icons.POLViewer;
    }

    #region General Support Routines

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

    #endregion

    #region File Swaps (Dialog Tables / String Tables)

    private SortedList SwappableFiles;

    private XmlDocument LoadSwappableFileInfo(string Category) {
      if (this.SwappableFiles == null)
	this.SwappableFiles = new SortedList();
      if (this.SwappableFiles[Category] == null) {
      Stream InfoData = Assembly.GetExecutingAssembly().GetManifestResourceStream(Category + ".xml");
	if (InfoData != null) {
	XmlReader XR = new XmlTextReader(InfoData);
	  try {
	  XmlDocument XD = new XmlDocument();
	    XD.Load(XR);
	    this.SwappableFiles[Category] = XD;
	  } catch { }
	  XR.Close();
	  InfoData.Close();
	}
	if (this.SwappableFiles[Category] == null)
	  this.AddLogEntry("*** LoadSwappableFileInfo() FAILED ***");
      }
      return this.SwappableFiles[Category] as XmlDocument;
    }

    private string GetSwappableFilePath(XmlNode DialogTable) {
    short App    = XmlConvert.ToInt16(DialogTable.Attributes.GetNamedItem("app").InnerText);
    short Dir    = XmlConvert.ToInt16(DialogTable.Attributes.GetNamedItem("dir").InnerText);
    short FileID = XmlConvert.ToInt16(DialogTable.Attributes.GetNamedItem("file").InnerText);
      return this.GetROMPath(App, Dir, FileID);
    }

    private bool BackupSwappableFile(XmlNode DialogTable) {
    short App    = XmlConvert.ToInt16(DialogTable.Attributes.GetNamedItem("app").InnerText);
    short Dir    = XmlConvert.ToInt16(DialogTable.Attributes.GetNamedItem("dir").InnerText);
    short FileID = XmlConvert.ToInt16(DialogTable.Attributes.GetNamedItem("file").InnerText);
      return this.BackupFile(App, Dir, FileID);
    }

    private void SwapFile(XmlNode Source, XmlNode Target) {
      try {
	if (!this.BackupSwappableFile(Target))
	  return;
      string SourceFile = this.GetSwappableFilePath(Source);
      string TargetFile = this.GetSwappableFilePath(Target);
	this.AddLogEntry(String.Format("Replacing Japanese ROM file {0}", TargetFile));
	this.AddLogEntry(String.Format("Source: English ROM file {0}", SourceFile));
	File.Copy(SourceFile, TargetFile, true);
      }
      catch {
	this.AddLogEntry("*** SwapFile() FAILED ***");
      }
    }

    private void RestoreSwappedFile(XmlNode DialogTable) {
      try {
      short App    = XmlConvert.ToInt16(DialogTable.Attributes.GetNamedItem("app").InnerText);
      short Dir    = XmlConvert.ToInt16(DialogTable.Attributes.GetNamedItem("dir").InnerText);
      short FileID = XmlConvert.ToInt16(DialogTable.Attributes.GetNamedItem("file").InnerText);
	this.RestoreFile(App, Dir, FileID);
      }
      catch {
	this.AddLogEntry("*** RestoreDialogTable() FAILED ***");
      }
    }

    private void SwapFiles(string Category) {
    XmlDocument XD = this.LoadSwappableFileInfo(Category);
      if (XD != null) {
	foreach (XmlNode XN in XD.SelectNodes("/rom-files/rom-file"))
	  this.SwapFile(XN.SelectSingleNode("./english"), XN.SelectSingleNode("./japanese"));
      }
    }

    private void RestoreSwappedFiles(string Category) {
    XmlDocument XD = this.LoadSwappableFileInfo(Category);
      if (XD != null) {
	foreach (XmlNode XN in XD.SelectNodes("/rom-files/rom-file"))
	  this.RestoreSwappedFile(XN.SelectSingleNode("./japanese"));
      }
    }

    #endregion

    #region Item Data Translation

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

    #endregion

    #region Spell Data Translation

    private void TranslateSpellFile(short App, short Dir, short FileID) {
      if (!this.BackupFile(App, Dir, FileID))
	return;
      try {
      string FileName = this.GetROMPath(App, Dir, FileID);
	this.AddLogEntry(String.Format("Translating spell data file: {0}", FileName));
      FileStream SpellStream = new FileStream(FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
	if ((SpellStream.Length % 0x400) != 0) {
	  this.AddLogEntry("*** File size suggests this isn't spell data - Translation Aborted ***");
	  goto TranslationDone;
	}
      long SpellCount = SpellStream.Length / 0x400;
      byte[] TextBlock = new byte[0x128];
	for (long i = 0; i < SpellCount; ++i) {
	  SpellStream.Seek(i * 0x400 + 0x1f, SeekOrigin.Begin); SpellStream.Read (TextBlock, 0, 0x128);
	  Array.Copy(TextBlock, 0x14, TextBlock, 0x00, 0x14); // Copy english name
	  Array.Copy(TextBlock, 0xa8, TextBlock, 0x28, 0x80); // Copy english description
	  SpellStream.Seek(i * 0x400 + 0x1f, SeekOrigin.Begin); SpellStream.Write(TextBlock, 0, 0x128);
	}
      TranslationDone:
	SpellStream.Close();
      }
      catch {
	this.AddLogEntry("*** TranslateSpellFile() FAILED ***");
      }
    }

    #endregion

    #region Auto-Translator Translation

    private void TranslateAutoTranslatorFile(short App, short Dir, short FileID) {
      if (!this.BackupFile(App, Dir, FileID))
	return;
    FileStream ATStream = null;
      try {
      string FileName = this.GetROMPath(App, Dir, FileID);
	ATStream = new FileStream(FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
	this.AddLogEntry(String.Format("Translating auto-translator file: {0}", FileName));
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

    #endregion

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
	components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      this.btnTranslateItemData = new System.Windows.Forms.Button();
      this.btnRestoreItemData = new System.Windows.Forms.Button();
      this.btnTranslateAutoTrans = new System.Windows.Forms.Button();
      this.btnRestoreAutoTrans = new System.Windows.Forms.Button();
      this.btnRestoreDialogTables = new System.Windows.Forms.Button();
      this.btnTranslateDialogTables = new System.Windows.Forms.Button();
      this.btnRestoreSpellData = new System.Windows.Forms.Button();
      this.btnTranslateSpellData = new System.Windows.Forms.Button();
      this.btnRestoreStringTables = new System.Windows.Forms.Button();
      this.btnTranslateStringTables = new System.Windows.Forms.Button();
      this.lblDialogTables = new System.Windows.Forms.Label();
      this.lblStringTables = new System.Windows.Forms.Label();
      this.lblSpellData = new System.Windows.Forms.Label();
      this.lblAutoTranslator = new System.Windows.Forms.Label();
      this.lblItemData = new System.Windows.Forms.Label();
      this.pnlLogPane = new System.Windows.Forms.Panel();
      this.lblActivityLog = new System.Windows.Forms.Label();
      this.rtbActivityLog = new System.Windows.Forms.RichTextBox();
      this.pnlLogPane.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnTranslateItemData
      // 
      this.btnTranslateItemData.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnTranslateItemData.Location = new System.Drawing.Point(160, 24);
      this.btnTranslateItemData.Name = "btnTranslateItemData";
      this.btnTranslateItemData.Size = new System.Drawing.Size(56, 20);
      this.btnTranslateItemData.TabIndex = 2;
      this.btnTranslateItemData.Text = "Translate";
      this.btnTranslateItemData.Click += new System.EventHandler(this.btnTranslateItemData_Click);
      // 
      // btnRestoreItemData
      // 
      this.btnRestoreItemData.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnRestoreItemData.Location = new System.Drawing.Point(216, 24);
      this.btnRestoreItemData.Name = "btnRestoreItemData";
      this.btnRestoreItemData.Size = new System.Drawing.Size(60, 20);
      this.btnRestoreItemData.TabIndex = 5;
      this.btnRestoreItemData.Text = "Restore";
      this.btnRestoreItemData.Click += new System.EventHandler(this.btnRestoreItemData_Click);
      // 
      // btnTranslateAutoTrans
      // 
      this.btnTranslateAutoTrans.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnTranslateAutoTrans.Location = new System.Drawing.Point(160, 44);
      this.btnTranslateAutoTrans.Name = "btnTranslateAutoTrans";
      this.btnTranslateAutoTrans.Size = new System.Drawing.Size(56, 20);
      this.btnTranslateAutoTrans.TabIndex = 3;
      this.btnTranslateAutoTrans.Text = "Translate";
      this.btnTranslateAutoTrans.Click += new System.EventHandler(this.btnTranslateAutoTrans_Click);
      // 
      // btnRestoreAutoTrans
      // 
      this.btnRestoreAutoTrans.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnRestoreAutoTrans.Location = new System.Drawing.Point(216, 44);
      this.btnRestoreAutoTrans.Name = "btnRestoreAutoTrans";
      this.btnRestoreAutoTrans.Size = new System.Drawing.Size(60, 20);
      this.btnRestoreAutoTrans.TabIndex = 6;
      this.btnRestoreAutoTrans.Text = "Restore";
      this.btnRestoreAutoTrans.Click += new System.EventHandler(this.btnRestoreAutoTrans_Click);
      // 
      // btnRestoreDialogTables
      // 
      this.btnRestoreDialogTables.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnRestoreDialogTables.Location = new System.Drawing.Point(488, 4);
      this.btnRestoreDialogTables.Name = "btnRestoreDialogTables";
      this.btnRestoreDialogTables.Size = new System.Drawing.Size(60, 20);
      this.btnRestoreDialogTables.TabIndex = 4;
      this.btnRestoreDialogTables.Text = "Restore";
      this.btnRestoreDialogTables.Click += new System.EventHandler(this.btnRestoreDialogTables_Click);
      // 
      // btnTranslateDialogTables
      // 
      this.btnTranslateDialogTables.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnTranslateDialogTables.Location = new System.Drawing.Point(432, 4);
      this.btnTranslateDialogTables.Name = "btnTranslateDialogTables";
      this.btnTranslateDialogTables.Size = new System.Drawing.Size(56, 20);
      this.btnTranslateDialogTables.TabIndex = 1;
      this.btnTranslateDialogTables.Text = "Translate";
      this.btnTranslateDialogTables.Click += new System.EventHandler(this.btnTranslateDialogTables_Click);
      // 
      // btnRestoreSpellData
      // 
      this.btnRestoreSpellData.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnRestoreSpellData.Location = new System.Drawing.Point(216, 4);
      this.btnRestoreSpellData.Name = "btnRestoreSpellData";
      this.btnRestoreSpellData.Size = new System.Drawing.Size(60, 20);
      this.btnRestoreSpellData.TabIndex = 105;
      this.btnRestoreSpellData.Text = "Restore";
      this.btnRestoreSpellData.Click += new System.EventHandler(this.btnRestoreSpellData_Click);
      // 
      // btnTranslateSpellData
      // 
      this.btnTranslateSpellData.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnTranslateSpellData.Location = new System.Drawing.Point(160, 4);
      this.btnTranslateSpellData.Name = "btnTranslateSpellData";
      this.btnTranslateSpellData.Size = new System.Drawing.Size(56, 20);
      this.btnTranslateSpellData.TabIndex = 102;
      this.btnTranslateSpellData.Text = "Translate";
      this.btnTranslateSpellData.Click += new System.EventHandler(this.btnTranslateSpellData_Click);
      // 
      // btnRestoreStringTables
      // 
      this.btnRestoreStringTables.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnRestoreStringTables.Location = new System.Drawing.Point(488, 24);
      this.btnRestoreStringTables.Name = "btnRestoreStringTables";
      this.btnRestoreStringTables.Size = new System.Drawing.Size(60, 20);
      this.btnRestoreStringTables.TabIndex = 106;
      this.btnRestoreStringTables.Text = "Restore";
      this.btnRestoreStringTables.Click += new System.EventHandler(this.btnRestoreStringTables_Click);
      // 
      // btnTranslateStringTables
      // 
      this.btnTranslateStringTables.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnTranslateStringTables.Location = new System.Drawing.Point(432, 24);
      this.btnTranslateStringTables.Name = "btnTranslateStringTables";
      this.btnTranslateStringTables.Size = new System.Drawing.Size(56, 20);
      this.btnTranslateStringTables.TabIndex = 103;
      this.btnTranslateStringTables.Text = "Translate";
      this.btnTranslateStringTables.Click += new System.EventHandler(this.btnTranslateStringTables_Click);
      // 
      // lblDialogTables
      // 
      this.lblDialogTables.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblDialogTables.Location = new System.Drawing.Point(284, 8);
      this.lblDialogTables.Name = "lblDialogTables";
      this.lblDialogTables.Size = new System.Drawing.Size(144, 16);
      this.lblDialogTables.TabIndex = 108;
      this.lblDialogTables.Text = "Zone Dialog Tables:";
      this.lblDialogTables.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // lblStringTables
      // 
      this.lblStringTables.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblStringTables.Location = new System.Drawing.Point(284, 28);
      this.lblStringTables.Name = "lblStringTables";
      this.lblStringTables.Size = new System.Drawing.Size(144, 16);
      this.lblStringTables.TabIndex = 110;
      this.lblStringTables.Text = "Other String Tables:";
      this.lblStringTables.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // lblSpellData
      // 
      this.lblSpellData.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblSpellData.Location = new System.Drawing.Point(12, 8);
      this.lblSpellData.Name = "lblSpellData";
      this.lblSpellData.Size = new System.Drawing.Size(144, 16);
      this.lblSpellData.TabIndex = 111;
      this.lblSpellData.Text = "Spell Names && Descriptions:";
      this.lblSpellData.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // lblAutoTranslator
      // 
      this.lblAutoTranslator.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblAutoTranslator.Location = new System.Drawing.Point(12, 48);
      this.lblAutoTranslator.Name = "lblAutoTranslator";
      this.lblAutoTranslator.Size = new System.Drawing.Size(144, 16);
      this.lblAutoTranslator.TabIndex = 112;
      this.lblAutoTranslator.Text = "Auto-Translator Strings:";
      this.lblAutoTranslator.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // lblItemData
      // 
      this.lblItemData.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemData.Location = new System.Drawing.Point(12, 28);
      this.lblItemData.Name = "lblItemData";
      this.lblItemData.Size = new System.Drawing.Size(144, 16);
      this.lblItemData.TabIndex = 113;
      this.lblItemData.Text = "Item Names && Descriptions:";
      this.lblItemData.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // pnlLogPane
      // 
      this.pnlLogPane.Controls.Add(this.lblActivityLog);
      this.pnlLogPane.Controls.Add(this.rtbActivityLog);
      this.pnlLogPane.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.pnlLogPane.Location = new System.Drawing.Point(0, 70);
      this.pnlLogPane.Name = "pnlLogPane";
      this.pnlLogPane.Size = new System.Drawing.Size(552, 184);
      this.pnlLogPane.TabIndex = 114;
      // 
      // lblActivityLog
      // 
      this.lblActivityLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
	| System.Windows.Forms.AnchorStyles.Right)));
      this.lblActivityLog.Location = new System.Drawing.Point(4, 4);
      this.lblActivityLog.Name = "lblActivityLog";
      this.lblActivityLog.Size = new System.Drawing.Size(540, 16);
      this.lblActivityLog.TabIndex = 102;
      this.lblActivityLog.Text = "Activity Log:";
      // 
      // rtbActivityLog
      // 
      this.rtbActivityLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
	| System.Windows.Forms.AnchorStyles.Left) 
	| System.Windows.Forms.AnchorStyles.Right)));
      this.rtbActivityLog.Location = new System.Drawing.Point(4, 20);
      this.rtbActivityLog.Name = "rtbActivityLog";
      this.rtbActivityLog.ReadOnly = true;
      this.rtbActivityLog.Size = new System.Drawing.Size(544, 160);
      this.rtbActivityLog.TabIndex = 103;
      this.rtbActivityLog.Text = "";
      // 
      // MainWindow
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(552, 254);
      this.Controls.Add(this.pnlLogPane);
      this.Controls.Add(this.lblItemData);
      this.Controls.Add(this.lblAutoTranslator);
      this.Controls.Add(this.lblSpellData);
      this.Controls.Add(this.lblStringTables);
      this.Controls.Add(this.lblDialogTables);
      this.Controls.Add(this.btnRestoreSpellData);
      this.Controls.Add(this.btnTranslateSpellData);
      this.Controls.Add(this.btnRestoreStringTables);
      this.Controls.Add(this.btnTranslateStringTables);
      this.Controls.Add(this.btnRestoreDialogTables);
      this.Controls.Add(this.btnTranslateDialogTables);
      this.Controls.Add(this.btnRestoreAutoTrans);
      this.Controls.Add(this.btnTranslateAutoTrans);
      this.Controls.Add(this.btnRestoreItemData);
      this.Controls.Add(this.btnTranslateItemData);
      this.Name = "MainWindow";
      this.Text = "Make JP FFXI Engrish!";
      this.pnlLogPane.ResumeLayout(false);
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

    private void btnTranslateSpellData_Click(object sender, System.EventArgs e) {
      this.TranslateSpellFile(0, 0, 11);
      this.AddLogEntry("=== Spell Data Translation Completed ===");
    }

    private void btnRestoreSpellData_Click(object sender, System.EventArgs e) {
      this.RestoreFile(0, 0, 11);
      this.AddLogEntry("=== Spell Data Restoration Completed ===");
    }

    private void btnTranslateAutoTrans_Click(object sender, System.EventArgs e) {
      this.TranslateAutoTranslatorFile(0, 76, 23);
      this.AddLogEntry("=== Auto-Translator Translation Completed ===");
    }

    private void btnRestoreAutoTrans_Click(object sender, System.EventArgs e) {
      this.RestoreFile(0, 76, 23);
      this.AddLogEntry("=== Auto-Translator Restoration Completed ===");
    }

    private void btnTranslateDialogTables_Click(object sender, System.EventArgs e) {
      this.SwapFiles("DialogTables");
      this.AddLogEntry("=== Dialog Table Translation Completed ===");
    }

    private void btnRestoreDialogTables_Click(object sender, System.EventArgs e) {
      this.RestoreSwappedFiles("DialogTables");
      this.AddLogEntry("=== Dialog Table Restoration Completed ===");
    }

    private void btnTranslateStringTables_Click(object sender, System.EventArgs e) {
      this.SwapFiles("StringTables");
      this.AddLogEntry("=== String Table Translation Completed ===");
    }

    private void btnRestoreStringTables_Click(object sender, System.EventArgs e) {
      this.RestoreSwappedFiles("StringTables");
      this.AddLogEntry("=== String Table Restoration Completed ===");
    }

    #endregion

  }

}
