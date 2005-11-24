// $Id$

using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

using PlayOnline.Core;
using PlayOnline.FFXI;

namespace EngrishOnry {

  internal partial class MainWindow : System.Windows.Forms.Form {

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
