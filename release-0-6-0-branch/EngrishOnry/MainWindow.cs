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
using PlayOnline.FFXI;

namespace EngrishOnry {

  internal class MainWindow : System.Windows.Forms.Form {

    #region Controls

    private System.Windows.Forms.Label lblActivityLog;
    private System.Windows.Forms.RichTextBox rtbActivityLog;
    private System.Windows.Forms.ContextMenu mnuConfigSpellData;
    private System.Windows.Forms.ContextMenu mnuConfigItemData;
    private System.Windows.Forms.MenuItem mnuTranslateSpellNames;
    private System.Windows.Forms.MenuItem mnuTranslateSpellDescriptions;
    private System.Windows.Forms.ContextMenu mnuConfigAutoTrans;
    private System.Windows.Forms.Panel pnlLog;
    private System.Windows.Forms.Panel pnlActions;
    private System.Windows.Forms.Button btnConfigAutoTrans;
    private System.Windows.Forms.Button btnConfigItemData;
    private System.Windows.Forms.Button btnConfigSpellData;
    private System.Windows.Forms.Label lblItemData;
    private System.Windows.Forms.Label lblAutoTranslator;
    private System.Windows.Forms.Label lblSpellData;
    private System.Windows.Forms.Label lblStringTables;
    private System.Windows.Forms.Label lblDialogTables;
    private System.Windows.Forms.Button btnRestoreSpellData;
    private System.Windows.Forms.Button btnTranslateSpellData;
    private System.Windows.Forms.Button btnRestoreStringTables;
    private System.Windows.Forms.Button btnTranslateStringTables;
    private System.Windows.Forms.Button btnRestoreDialogTables;
    private System.Windows.Forms.Button btnTranslateDialogTables;
    private System.Windows.Forms.Button btnRestoreAutoTrans;
    private System.Windows.Forms.Button btnTranslateAutoTrans;
    private System.Windows.Forms.Button btnRestoreItemData;
    private System.Windows.Forms.Button btnTranslateItemData;
    private System.Windows.Forms.MenuItem mnuTranslateItemNames;
    private System.Windows.Forms.MenuItem mnuTranslateItemDescriptions;
    private System.Windows.Forms.MenuItem mnuPreserveJapaneseATCompletion;
    private System.Windows.Forms.MenuItem mnuEnglishATCompletionOnly;
    private System.Windows.Forms.Button btnConfigStringTables;
    private System.Windows.Forms.Button btnConfigDialogTables;
    private System.Windows.Forms.Button btnConfigAbilities;
    private System.Windows.Forms.Label lblAbilities;
    private System.Windows.Forms.Button btnRestoreAbilities;
    private System.Windows.Forms.Button btnTranslateAbilities;
    private System.Windows.Forms.ContextMenu mnuConfigAbilities;
    private System.Windows.Forms.MenuItem mnuTranslateAbilityNames;
    private System.Windows.Forms.MenuItem mnuTranslateAbilityDescriptions;

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

    #region Item Data Translation

    private void TranslateItemFile(short JApp, short JDir, short JFileID, short EApp, short EDir, short EFileID, short Type) {
      if (!this.mnuTranslateItemNames.Checked && !this.mnuTranslateItemDescriptions.Checked)
	return;
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
	  if (this.mnuTranslateItemNames.Checked) {
	    switch (Type) {
	      case 0: Array.Copy(EData, 0x24, JData, 0x0E, 0x16); break; // Item
	      case 1: Array.Copy(EData, 0x34, JData, 0x1E, 0x16); break; // Weapon
	      case 2: Array.Copy(EData, 0x2E, JData, 0x18, 0x16); break; // Armor
	    }
	  }
	  if (this.mnuTranslateItemDescriptions.Checked) {
	    switch (Type) {
	      case 0: Array.Copy(EData, 0xC6, JData, 0x3A, 0xBC); break; // Item
	      case 1: Array.Copy(EData, 0xD6, JData, 0x4A, 0xBC); break; // Weapon
	      case 2: Array.Copy(EData, 0xD0, JData, 0x44, 0xBC); break; // Armor
	    }
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
      if (!this.mnuTranslateSpellNames.Checked && !this.mnuTranslateSpellDescriptions.Checked)
	return;
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
	  if (this.mnuTranslateSpellNames.Checked)
	    Array.Copy(TextBlock, 0x14, TextBlock, 0x00, 0x14); // Copy english name
	  if (this.mnuTranslateSpellDescriptions.Checked)
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

    #region Ability Data Translation

    private void TranslateAbilityFile(short JApp, short JDir, short JFileID, short EApp, short EDir, short EFileID) {
      if (!this.mnuTranslateAbilityNames.Checked && !this.mnuTranslateAbilityDescriptions.Checked)
	return;
      if (!this.BackupFile(JApp, JDir, JFileID))
	return;
      try {
      string JFileName = this.GetROMPath(JApp, JDir, JFileID);
      string EFileName = this.GetROMPath(EApp, EDir, EFileID);
	this.AddLogEntry(String.Format("Translating abilities data file: {0}", JFileName));
	this.AddLogEntry(String.Format("Using English data file: {0}", EFileName));
      FileStream JFileStream = new FileStream(JFileName, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
      FileStream EFileStream = new FileStream(JFileName, FileMode.Open, FileAccess.Read);
	if ((JFileStream.Length % 0x400) != 0 || (EFileStream.Length % 0x400) != 0) {
	  this.AddLogEntry("*** File size suggests this isn't ability data - Translation Aborted ***");
	  goto TranslationDone;
	}
	if (JFileStream.Length != EFileStream.Length) {
	  this.AddLogEntry("*** File sizes of Japanese and English files do not match - Translation Aborted ***");
	  goto TranslationDone;
	}
      long AbilityCount = JFileStream.Length / 0x400;
      byte[] JTextBlock = new byte[0x120];
      byte[] ETextBlock = new byte[0x120];
	for (long i = 0; i < AbilityCount; ++i) {
	  JFileStream.Seek(i * 0x400 + 0xa, SeekOrigin.Begin); JFileStream.Read (JTextBlock, 0, 0x120);
	  EFileStream.Seek(i * 0x400 + 0xa, SeekOrigin.Begin); EFileStream.Read (ETextBlock, 0, 0x120);
	  if (this.mnuTranslateAbilityNames.Checked)        Array.Copy(ETextBlock, 0x00, JTextBlock, 0x00, 0x020);
	  if (this.mnuTranslateAbilityDescriptions.Checked) Array.Copy(ETextBlock, 0x20, JTextBlock, 0x20, 0x100);
	  JFileStream.Seek(i * 0x400 + 0xa, SeekOrigin.Begin); JFileStream.Write(JTextBlock, 0, 0x120);
	}
      TranslationDone:
	JFileStream.Close();
	EFileStream.Close();
      }
      catch {
	this.AddLogEntry("*** TranslateAbilityFile() FAILED ***");
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
	FFXIEncoding E = null;
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
		  // Set the english text as primary
		  BW.Write(ENText.Length); BW.Write(ENText);
		  if (this.mnuPreserveJapaneseATCompletion.Checked) {
		  byte[] JPPrimary   = BR.ReadBytes(BR.ReadInt32());
		  byte[] JPSecondary = BR.ReadBytes(BR.ReadInt32());
		    if (JPSecondary.Length == 0)
		      JPSecondary = JPPrimary;
		    BW.Write(JPSecondary.Length); BW.Write(JPSecondary);
		  }
		  else if (this.mnuEnglishATCompletionOnly.Checked) {
		    // Skip JP strings
		    BR.BaseStream.Seek(BR.ReadInt32(), SeekOrigin.Current);
		    BR.BaseStream.Seek(BR.ReadInt32(), SeekOrigin.Current);
		    if (E == null)
		      E = new FFXIEncoding();
		  string NormalText = E.GetString(ENText);
		  string LowerCaseText = NormalText.ToLower();
		    if (LowerCaseText != NormalText) {
		      ENText = E.GetBytes(LowerCaseText);
		      BW.Write(ENText.Length); BW.Write(ENText);
		    }
		    else
		      BW.Write((uint) 0);
		  }
		  else
		    BW.Write((uint) 0);
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

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
	components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      this.pnlLog = new System.Windows.Forms.Panel();
      this.lblActivityLog = new System.Windows.Forms.Label();
      this.rtbActivityLog = new System.Windows.Forms.RichTextBox();
      this.mnuConfigSpellData = new System.Windows.Forms.ContextMenu();
      this.mnuTranslateSpellNames = new System.Windows.Forms.MenuItem();
      this.mnuTranslateSpellDescriptions = new System.Windows.Forms.MenuItem();
      this.mnuConfigItemData = new System.Windows.Forms.ContextMenu();
      this.mnuTranslateItemNames = new System.Windows.Forms.MenuItem();
      this.mnuTranslateItemDescriptions = new System.Windows.Forms.MenuItem();
      this.mnuConfigAutoTrans = new System.Windows.Forms.ContextMenu();
      this.mnuPreserveJapaneseATCompletion = new System.Windows.Forms.MenuItem();
      this.mnuEnglishATCompletionOnly = new System.Windows.Forms.MenuItem();
      this.pnlActions = new System.Windows.Forms.Panel();
      this.btnConfigAbilities = new System.Windows.Forms.Button();
      this.lblAbilities = new System.Windows.Forms.Label();
      this.btnRestoreAbilities = new System.Windows.Forms.Button();
      this.btnTranslateAbilities = new System.Windows.Forms.Button();
      this.btnConfigDialogTables = new System.Windows.Forms.Button();
      this.btnConfigStringTables = new System.Windows.Forms.Button();
      this.btnConfigAutoTrans = new System.Windows.Forms.Button();
      this.btnConfigItemData = new System.Windows.Forms.Button();
      this.btnConfigSpellData = new System.Windows.Forms.Button();
      this.lblItemData = new System.Windows.Forms.Label();
      this.lblAutoTranslator = new System.Windows.Forms.Label();
      this.lblSpellData = new System.Windows.Forms.Label();
      this.lblStringTables = new System.Windows.Forms.Label();
      this.lblDialogTables = new System.Windows.Forms.Label();
      this.btnRestoreSpellData = new System.Windows.Forms.Button();
      this.btnTranslateSpellData = new System.Windows.Forms.Button();
      this.btnRestoreStringTables = new System.Windows.Forms.Button();
      this.btnTranslateStringTables = new System.Windows.Forms.Button();
      this.btnRestoreDialogTables = new System.Windows.Forms.Button();
      this.btnTranslateDialogTables = new System.Windows.Forms.Button();
      this.btnRestoreAutoTrans = new System.Windows.Forms.Button();
      this.btnTranslateAutoTrans = new System.Windows.Forms.Button();
      this.btnRestoreItemData = new System.Windows.Forms.Button();
      this.btnTranslateItemData = new System.Windows.Forms.Button();
      this.mnuConfigAbilities = new System.Windows.Forms.ContextMenu();
      this.mnuTranslateAbilityNames = new System.Windows.Forms.MenuItem();
      this.mnuTranslateAbilityDescriptions = new System.Windows.Forms.MenuItem();
      this.pnlLog.SuspendLayout();
      this.pnlActions.SuspendLayout();
      this.SuspendLayout();
      // 
      // pnlLog
      // 
      this.pnlLog.Controls.Add(this.lblActivityLog);
      this.pnlLog.Controls.Add(this.rtbActivityLog);
      this.pnlLog.Dock = System.Windows.Forms.DockStyle.Fill;
      this.pnlLog.Location = new System.Drawing.Point(0, 72);
      this.pnlLog.Name = "pnlLog";
      this.pnlLog.Size = new System.Drawing.Size(520, 150);
      this.pnlLog.TabIndex = 0;
      // 
      // lblActivityLog
      // 
      this.lblActivityLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
	| System.Windows.Forms.AnchorStyles.Right)));
      this.lblActivityLog.Location = new System.Drawing.Point(4, 4);
      this.lblActivityLog.Name = "lblActivityLog";
      this.lblActivityLog.Size = new System.Drawing.Size(508, 16);
      this.lblActivityLog.TabIndex = 0;
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
      this.rtbActivityLog.Size = new System.Drawing.Size(512, 126);
      this.rtbActivityLog.TabIndex = 500;
      this.rtbActivityLog.Text = "";
      // 
      // mnuConfigSpellData
      // 
      this.mnuConfigSpellData.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
										       this.mnuTranslateSpellNames,
										       this.mnuTranslateSpellDescriptions});
      // 
      // mnuTranslateSpellNames
      // 
      this.mnuTranslateSpellNames.Checked = true;
      this.mnuTranslateSpellNames.Index = 0;
      this.mnuTranslateSpellNames.Text = "Translate Spell &Names";
      this.mnuTranslateSpellNames.Click += new System.EventHandler(this.mnuTranslateSpellNames_Click);
      // 
      // mnuTranslateSpellDescriptions
      // 
      this.mnuTranslateSpellDescriptions.Checked = true;
      this.mnuTranslateSpellDescriptions.Index = 1;
      this.mnuTranslateSpellDescriptions.Text = "Translate Spell &Descriptions";
      this.mnuTranslateSpellDescriptions.Click += new System.EventHandler(this.mnuTranslateSpellDescriptions_Click);
      // 
      // mnuConfigItemData
      // 
      this.mnuConfigItemData.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
										      this.mnuTranslateItemNames,
										      this.mnuTranslateItemDescriptions});
      // 
      // mnuTranslateItemNames
      // 
      this.mnuTranslateItemNames.Checked = true;
      this.mnuTranslateItemNames.Index = 0;
      this.mnuTranslateItemNames.Text = "Translate Item &Names";
      this.mnuTranslateItemNames.Click += new System.EventHandler(this.mnuTranslateItemNames_Click);
      // 
      // mnuTranslateItemDescriptions
      // 
      this.mnuTranslateItemDescriptions.Checked = true;
      this.mnuTranslateItemDescriptions.Index = 1;
      this.mnuTranslateItemDescriptions.Text = "Translate Item &Descriptions";
      this.mnuTranslateItemDescriptions.Click += new System.EventHandler(this.mnuTranslateItemDescriptions_Click);
      // 
      // mnuConfigAutoTrans
      // 
      this.mnuConfigAutoTrans.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
										       this.mnuPreserveJapaneseATCompletion,
										       this.mnuEnglishATCompletionOnly});
      // 
      // mnuPreserveJapaneseATCompletion
      // 
      this.mnuPreserveJapaneseATCompletion.Checked = true;
      this.mnuPreserveJapaneseATCompletion.Index = 0;
      this.mnuPreserveJapaneseATCompletion.Text = "Preserve &Japanese Completion";
      this.mnuPreserveJapaneseATCompletion.Click += new System.EventHandler(this.mnuPreserveJapaneseATCompletion_Click);
      // 
      // mnuEnglishATCompletionOnly
      // 
      this.mnuEnglishATCompletionOnly.Index = 1;
      this.mnuEnglishATCompletionOnly.Text = "&English Completion Only";
      this.mnuEnglishATCompletionOnly.Click += new System.EventHandler(this.mnuEnglishATCompletionOnly_Click);
      // 
      // pnlActions
      // 
      this.pnlActions.Controls.Add(this.btnConfigAbilities);
      this.pnlActions.Controls.Add(this.lblAbilities);
      this.pnlActions.Controls.Add(this.btnRestoreAbilities);
      this.pnlActions.Controls.Add(this.btnTranslateAbilities);
      this.pnlActions.Controls.Add(this.btnConfigDialogTables);
      this.pnlActions.Controls.Add(this.btnConfigStringTables);
      this.pnlActions.Controls.Add(this.btnConfigAutoTrans);
      this.pnlActions.Controls.Add(this.btnConfigItemData);
      this.pnlActions.Controls.Add(this.btnConfigSpellData);
      this.pnlActions.Controls.Add(this.lblItemData);
      this.pnlActions.Controls.Add(this.lblAutoTranslator);
      this.pnlActions.Controls.Add(this.lblSpellData);
      this.pnlActions.Controls.Add(this.lblStringTables);
      this.pnlActions.Controls.Add(this.lblDialogTables);
      this.pnlActions.Controls.Add(this.btnRestoreSpellData);
      this.pnlActions.Controls.Add(this.btnTranslateSpellData);
      this.pnlActions.Controls.Add(this.btnRestoreStringTables);
      this.pnlActions.Controls.Add(this.btnTranslateStringTables);
      this.pnlActions.Controls.Add(this.btnRestoreDialogTables);
      this.pnlActions.Controls.Add(this.btnTranslateDialogTables);
      this.pnlActions.Controls.Add(this.btnRestoreAutoTrans);
      this.pnlActions.Controls.Add(this.btnTranslateAutoTrans);
      this.pnlActions.Controls.Add(this.btnRestoreItemData);
      this.pnlActions.Controls.Add(this.btnTranslateItemData);
      this.pnlActions.Dock = System.Windows.Forms.DockStyle.Top;
      this.pnlActions.Location = new System.Drawing.Point(0, 0);
      this.pnlActions.Name = "pnlActions";
      this.pnlActions.Size = new System.Drawing.Size(520, 72);
      this.pnlActions.TabIndex = 14;
      // 
      // btnConfigAbilities
      // 
      this.btnConfigAbilities.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnConfigAbilities.Location = new System.Drawing.Point(188, 48);
      this.btnConfigAbilities.Name = "btnConfigAbilities";
      this.btnConfigAbilities.Size = new System.Drawing.Size(48, 20);
      this.btnConfigAbilities.TabIndex = 37;
      this.btnConfigAbilities.Text = "Options";
      this.btnConfigAbilities.Click += new System.EventHandler(this.btnConfigAbilities_Click);
      // 
      // lblAbilities
      // 
      this.lblAbilities.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblAbilities.Location = new System.Drawing.Point(8, 52);
      this.lblAbilities.Name = "lblAbilities";
      this.lblAbilities.Size = new System.Drawing.Size(60, 16);
      this.lblAbilities.TabIndex = 34;
      this.lblAbilities.Text = "Ability Data:";
      this.lblAbilities.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // btnRestoreAbilities
      // 
      this.btnRestoreAbilities.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnRestoreAbilities.Location = new System.Drawing.Point(128, 48);
      this.btnRestoreAbilities.Name = "btnRestoreAbilities";
      this.btnRestoreAbilities.Size = new System.Drawing.Size(60, 20);
      this.btnRestoreAbilities.TabIndex = 36;
      this.btnRestoreAbilities.Text = "Restore";
      this.btnRestoreAbilities.Click += new System.EventHandler(this.btnRestoreAbilities_Click);
      // 
      // btnTranslateAbilities
      // 
      this.btnTranslateAbilities.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnTranslateAbilities.Location = new System.Drawing.Point(72, 48);
      this.btnTranslateAbilities.Name = "btnTranslateAbilities";
      this.btnTranslateAbilities.Size = new System.Drawing.Size(56, 20);
      this.btnTranslateAbilities.TabIndex = 35;
      this.btnTranslateAbilities.Text = "Translate";
      this.btnTranslateAbilities.Click += new System.EventHandler(this.btnTranslateAbilities_Click);
      // 
      // btnConfigDialogTables
      // 
      this.btnConfigDialogTables.Enabled = false;
      this.btnConfigDialogTables.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnConfigDialogTables.Location = new System.Drawing.Point(464, 8);
      this.btnConfigDialogTables.Name = "btnConfigDialogTables";
      this.btnConfigDialogTables.Size = new System.Drawing.Size(48, 20);
      this.btnConfigDialogTables.TabIndex = 33;
      this.btnConfigDialogTables.Text = "Options";
      // 
      // btnConfigStringTables
      // 
      this.btnConfigStringTables.Enabled = false;
      this.btnConfigStringTables.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnConfigStringTables.Location = new System.Drawing.Point(464, 28);
      this.btnConfigStringTables.Name = "btnConfigStringTables";
      this.btnConfigStringTables.Size = new System.Drawing.Size(48, 20);
      this.btnConfigStringTables.TabIndex = 32;
      this.btnConfigStringTables.Text = "Options";
      // 
      // btnConfigAutoTrans
      // 
      this.btnConfigAutoTrans.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnConfigAutoTrans.Location = new System.Drawing.Point(464, 48);
      this.btnConfigAutoTrans.Name = "btnConfigAutoTrans";
      this.btnConfigAutoTrans.Size = new System.Drawing.Size(48, 20);
      this.btnConfigAutoTrans.TabIndex = 27;
      this.btnConfigAutoTrans.Text = "Options";
      this.btnConfigAutoTrans.Click += new System.EventHandler(this.btnConfigAutoTrans_Click);
      // 
      // btnConfigItemData
      // 
      this.btnConfigItemData.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnConfigItemData.Location = new System.Drawing.Point(188, 28);
      this.btnConfigItemData.Name = "btnConfigItemData";
      this.btnConfigItemData.Size = new System.Drawing.Size(48, 20);
      this.btnConfigItemData.TabIndex = 24;
      this.btnConfigItemData.Text = "Options";
      this.btnConfigItemData.Click += new System.EventHandler(this.btnConfigItemData_Click);
      // 
      // btnConfigSpellData
      // 
      this.btnConfigSpellData.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnConfigSpellData.Location = new System.Drawing.Point(188, 8);
      this.btnConfigSpellData.Name = "btnConfigSpellData";
      this.btnConfigSpellData.Size = new System.Drawing.Size(48, 20);
      this.btnConfigSpellData.TabIndex = 21;
      this.btnConfigSpellData.Text = "Options";
      this.btnConfigSpellData.Click += new System.EventHandler(this.btnConfigSpellData_Click);
      // 
      // lblItemData
      // 
      this.lblItemData.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblItemData.Location = new System.Drawing.Point(8, 32);
      this.lblItemData.Name = "lblItemData";
      this.lblItemData.Size = new System.Drawing.Size(60, 16);
      this.lblItemData.TabIndex = 17;
      this.lblItemData.Text = "Item Data:";
      this.lblItemData.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // lblAutoTranslator
      // 
      this.lblAutoTranslator.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblAutoTranslator.Location = new System.Drawing.Point(260, 52);
      this.lblAutoTranslator.Name = "lblAutoTranslator";
      this.lblAutoTranslator.Size = new System.Drawing.Size(82, 16);
      this.lblAutoTranslator.TabIndex = 14;
      this.lblAutoTranslator.Text = "Auto-Translator:";
      this.lblAutoTranslator.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // lblSpellData
      // 
      this.lblSpellData.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblSpellData.Location = new System.Drawing.Point(8, 12);
      this.lblSpellData.Name = "lblSpellData";
      this.lblSpellData.Size = new System.Drawing.Size(60, 16);
      this.lblSpellData.TabIndex = 15;
      this.lblSpellData.Text = "Spell Data:";
      this.lblSpellData.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // lblStringTables
      // 
      this.lblStringTables.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblStringTables.Location = new System.Drawing.Point(240, 32);
      this.lblStringTables.Name = "lblStringTables";
      this.lblStringTables.Size = new System.Drawing.Size(104, 16);
      this.lblStringTables.TabIndex = 16;
      this.lblStringTables.Text = "Other String Tables:";
      this.lblStringTables.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // lblDialogTables
      // 
      this.lblDialogTables.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblDialogTables.Location = new System.Drawing.Point(240, 12);
      this.lblDialogTables.Name = "lblDialogTables";
      this.lblDialogTables.Size = new System.Drawing.Size(104, 16);
      this.lblDialogTables.TabIndex = 18;
      this.lblDialogTables.Text = "Zone Dialog Tables:";
      this.lblDialogTables.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // btnRestoreSpellData
      // 
      this.btnRestoreSpellData.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnRestoreSpellData.Location = new System.Drawing.Point(128, 8);
      this.btnRestoreSpellData.Name = "btnRestoreSpellData";
      this.btnRestoreSpellData.Size = new System.Drawing.Size(60, 20);
      this.btnRestoreSpellData.TabIndex = 20;
      this.btnRestoreSpellData.Text = "Restore";
      this.btnRestoreSpellData.Click += new System.EventHandler(this.btnRestoreSpellData_Click);
      // 
      // btnTranslateSpellData
      // 
      this.btnTranslateSpellData.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnTranslateSpellData.Location = new System.Drawing.Point(72, 8);
      this.btnTranslateSpellData.Name = "btnTranslateSpellData";
      this.btnTranslateSpellData.Size = new System.Drawing.Size(56, 20);
      this.btnTranslateSpellData.TabIndex = 19;
      this.btnTranslateSpellData.Text = "Translate";
      this.btnTranslateSpellData.Click += new System.EventHandler(this.btnTranslateSpellData_Click);
      // 
      // btnRestoreStringTables
      // 
      this.btnRestoreStringTables.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnRestoreStringTables.Location = new System.Drawing.Point(404, 28);
      this.btnRestoreStringTables.Name = "btnRestoreStringTables";
      this.btnRestoreStringTables.Size = new System.Drawing.Size(60, 20);
      this.btnRestoreStringTables.TabIndex = 31;
      this.btnRestoreStringTables.Text = "Restore";
      this.btnRestoreStringTables.Click += new System.EventHandler(this.btnRestoreStringTables_Click);
      // 
      // btnTranslateStringTables
      // 
      this.btnTranslateStringTables.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnTranslateStringTables.Location = new System.Drawing.Point(348, 28);
      this.btnTranslateStringTables.Name = "btnTranslateStringTables";
      this.btnTranslateStringTables.Size = new System.Drawing.Size(56, 20);
      this.btnTranslateStringTables.TabIndex = 30;
      this.btnTranslateStringTables.Text = "Translate";
      this.btnTranslateStringTables.Click += new System.EventHandler(this.btnTranslateStringTables_Click);
      // 
      // btnRestoreDialogTables
      // 
      this.btnRestoreDialogTables.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnRestoreDialogTables.Location = new System.Drawing.Point(404, 8);
      this.btnRestoreDialogTables.Name = "btnRestoreDialogTables";
      this.btnRestoreDialogTables.Size = new System.Drawing.Size(60, 20);
      this.btnRestoreDialogTables.TabIndex = 29;
      this.btnRestoreDialogTables.Text = "Restore";
      this.btnRestoreDialogTables.Click += new System.EventHandler(this.btnRestoreDialogTables_Click);
      // 
      // btnTranslateDialogTables
      // 
      this.btnTranslateDialogTables.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnTranslateDialogTables.Location = new System.Drawing.Point(348, 8);
      this.btnTranslateDialogTables.Name = "btnTranslateDialogTables";
      this.btnTranslateDialogTables.Size = new System.Drawing.Size(56, 20);
      this.btnTranslateDialogTables.TabIndex = 28;
      this.btnTranslateDialogTables.Text = "Translate";
      this.btnTranslateDialogTables.Click += new System.EventHandler(this.btnTranslateDialogTables_Click);
      // 
      // btnRestoreAutoTrans
      // 
      this.btnRestoreAutoTrans.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnRestoreAutoTrans.Location = new System.Drawing.Point(404, 48);
      this.btnRestoreAutoTrans.Name = "btnRestoreAutoTrans";
      this.btnRestoreAutoTrans.Size = new System.Drawing.Size(60, 20);
      this.btnRestoreAutoTrans.TabIndex = 26;
      this.btnRestoreAutoTrans.Text = "Restore";
      this.btnRestoreAutoTrans.Click += new System.EventHandler(this.btnRestoreAutoTrans_Click);
      // 
      // btnTranslateAutoTrans
      // 
      this.btnTranslateAutoTrans.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnTranslateAutoTrans.Location = new System.Drawing.Point(348, 48);
      this.btnTranslateAutoTrans.Name = "btnTranslateAutoTrans";
      this.btnTranslateAutoTrans.Size = new System.Drawing.Size(56, 20);
      this.btnTranslateAutoTrans.TabIndex = 25;
      this.btnTranslateAutoTrans.Text = "Translate";
      this.btnTranslateAutoTrans.Click += new System.EventHandler(this.btnTranslateAutoTrans_Click);
      // 
      // btnRestoreItemData
      // 
      this.btnRestoreItemData.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnRestoreItemData.Location = new System.Drawing.Point(128, 28);
      this.btnRestoreItemData.Name = "btnRestoreItemData";
      this.btnRestoreItemData.Size = new System.Drawing.Size(60, 20);
      this.btnRestoreItemData.TabIndex = 23;
      this.btnRestoreItemData.Text = "Restore";
      this.btnRestoreItemData.Click += new System.EventHandler(this.btnRestoreItemData_Click);
      // 
      // btnTranslateItemData
      // 
      this.btnTranslateItemData.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnTranslateItemData.Location = new System.Drawing.Point(72, 28);
      this.btnTranslateItemData.Name = "btnTranslateItemData";
      this.btnTranslateItemData.Size = new System.Drawing.Size(56, 20);
      this.btnTranslateItemData.TabIndex = 22;
      this.btnTranslateItemData.Text = "Translate";
      this.btnTranslateItemData.Click += new System.EventHandler(this.btnTranslateItemData_Click);
      // 
      // mnuConfigAbilities
      // 
      this.mnuConfigAbilities.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
										       this.mnuTranslateAbilityNames,
										       this.mnuTranslateAbilityDescriptions});
      // 
      // mnuTranslateAbilityNames
      // 
      this.mnuTranslateAbilityNames.Checked = true;
      this.mnuTranslateAbilityNames.Index = 0;
      this.mnuTranslateAbilityNames.Text = "Translate Ability &Names";
      // 
      // mnuTranslateAbilityDescriptions
      // 
      this.mnuTranslateAbilityDescriptions.Checked = true;
      this.mnuTranslateAbilityDescriptions.Index = 1;
      this.mnuTranslateAbilityDescriptions.Text = "Translate Ability &Descriptions";
      // 
      // MainWindow
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(520, 222);
      this.Controls.Add(this.pnlLog);
      this.Controls.Add(this.pnlActions);
      this.Name = "MainWindow";
      this.Text = "Make JP FFXI Engrish!";
      this.pnlLog.ResumeLayout(false);
      this.pnlActions.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    #region Events

    #region Spell Data

    private void btnTranslateSpellData_Click(object sender, System.EventArgs e) {
      this.TranslateSpellFile(0,   0, 11);
      this.TranslateSpellFile(0, 119, 56);
      this.AddLogEntry("=== Spell Data Translation Completed ===");
    }

    private void btnRestoreSpellData_Click(object sender, System.EventArgs e) {
      this.RestoreFile(0,   0, 11);
      this.RestoreFile(0, 119, 56);
      this.AddLogEntry("=== Spell Data Restoration Completed ===");
    }

    private void btnConfigSpellData_Click(object sender, System.EventArgs e) {
      this.mnuConfigSpellData.Show(this.btnConfigSpellData, new Point(0, this.btnConfigSpellData.Height));
    }

    private void mnuTranslateSpellNames_Click(object sender, System.EventArgs e) {
      this.mnuTranslateSpellNames.Checked = !this.mnuTranslateSpellNames.Checked;
    }

    private void mnuTranslateSpellDescriptions_Click(object sender, System.EventArgs e) {
      this.mnuTranslateSpellDescriptions.Checked = !this.mnuTranslateSpellDescriptions.Checked;
    }

    #endregion

    #region Item Data

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

    private void btnConfigItemData_Click(object sender, System.EventArgs e) {
      this.mnuConfigItemData.Show(this.btnConfigItemData, new Point(0, this.btnConfigItemData.Height));
    }

    private void mnuTranslateItemNames_Click(object sender, System.EventArgs e) {
      this.mnuTranslateItemNames.Checked = !this.mnuTranslateItemNames.Checked;
    }

    private void mnuTranslateItemDescriptions_Click(object sender, System.EventArgs e) {
      this.mnuTranslateItemDescriptions.Checked = !this.mnuTranslateItemDescriptions.Checked;
    }

    #endregion

    #region Abilities

    private void btnTranslateAbilities_Click(object sender, System.EventArgs e) {
      this.TranslateAbilityFile(0, 0, 10, 0, 119, 55);
      this.AddLogEntry("=== Ability Data Translation Completed ===");
    }

    private void btnRestoreAbilities_Click(object sender, System.EventArgs e) {
      this.RestoreFile(0, 0, 10);
      this.AddLogEntry("=== Ability Data Restoration Completed ===");
    }

    private void btnConfigAbilities_Click(object sender, System.EventArgs e) {
      this.mnuConfigAbilities.Show(this.btnConfigAbilities, new Point(0, this.btnConfigAbilities.Height));
    }

    #endregion

    #region Dialog Tables

    private void btnTranslateDialogTables_Click(object sender, System.EventArgs e) {
      this.SwapFiles("DialogTables");
      this.AddLogEntry("=== Dialog Table Translation Completed ===");
    }

    private void btnRestoreDialogTables_Click(object sender, System.EventArgs e) {
      this.RestoreSwappedFiles("DialogTables");
      this.AddLogEntry("=== Dialog Table Restoration Completed ===");
    }

    #endregion

    #region String Tables

    private void btnTranslateStringTables_Click(object sender, System.EventArgs e) {
      this.SwapFiles("StringTables");
      this.AddLogEntry("=== String Table Translation Completed ===");
    }

    private void btnRestoreStringTables_Click(object sender, System.EventArgs e) {
      this.RestoreSwappedFiles("StringTables");
      this.AddLogEntry("=== String Table Restoration Completed ===");
    }

    #endregion

    #region Auto-Translator

    private void btnTranslateAutoTrans_Click(object sender, System.EventArgs e) {
      this.TranslateAutoTranslatorFile(0, 76, 23);
      this.AddLogEntry("=== Auto-Translator Translation Completed ===");
    }

    private void btnRestoreAutoTrans_Click(object sender, System.EventArgs e) {
      this.RestoreFile(0, 76, 23);
      this.AddLogEntry("=== Auto-Translator Restoration Completed ===");
    }

    private void btnConfigAutoTrans_Click(object sender, System.EventArgs e) {
      this.mnuConfigAutoTrans.Show(this.btnConfigAutoTrans, new Point(0, this.btnConfigAutoTrans.Height));
    }

    private void mnuPreserveJapaneseATCompletion_Click(object sender, System.EventArgs e) {
      this.mnuPreserveJapaneseATCompletion.Checked = true;
      this.mnuEnglishATCompletionOnly.Checked = false;
    }

    private void mnuEnglishATCompletionOnly_Click(object sender, System.EventArgs e) {
      this.mnuEnglishATCompletionOnly.Checked = true;
      this.mnuPreserveJapaneseATCompletion.Checked = false;
    }

    #endregion

    #endregion

  }

}
