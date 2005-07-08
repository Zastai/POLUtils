// $Id$

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using PlayOnline.Core;

namespace PlayOnline.FFXI.Utils.DataBrowser {

  internal class FileScanDialog : System.Windows.Forms.Form {

    private string FileName;

    public static bool AllowAbort = false;
    public static bool ShowProgressDetails = false;

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
      this.ControlBox = FileScanDialog.AllowAbort;
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
	  this.ScanDialogFile(BR);
	  if (this.StringTableEntries.Count != 0)
	    goto Done;
  	  Application.DoEvents();
	  BR.BaseStream.Seek(0, SeekOrigin.Begin);
	  this.ScanXIStringFile(BR);
	  if (this.StringTableEntries.Count != 0)
	    goto Done;
  	  Application.DoEvents();
	  BR.BaseStream.Seek(0, SeekOrigin.Begin);
	  this.ScanSimpleStringFile(BR);
	  if (this.StringTableEntries.Count != 0)
	    goto Done;
  	  Application.DoEvents();
	  BR.BaseStream.Seek(0, SeekOrigin.Begin);
	  this.ScanAbilityFile(BR);
	  if (this.StringTableEntries.Count != 0)
	    goto Done;
  	  Application.DoEvents();
	  BR.BaseStream.Seek(0, SeekOrigin.Begin);
	  this.ScanSpellFile(BR);
	  if (this.StringTableEntries.Count != 0)
	    goto Done;
  	  Application.DoEvents();
	  BR.BaseStream.Seek(0, SeekOrigin.Begin);
	  this.ScanItemData(BR);
	  if (this.Items.Count != 0)
	    goto Done;
	  // No specific format recognized - scan for embedded image data
  	  Application.DoEvents();
	  BR.BaseStream.Seek(0, SeekOrigin.Begin);
	  this.ScanImages(BR);
	Done:
	  BR.Close();
	}
      }
      this.Scanning = false;
      this.ScanThread = null;
      this.DialogResult = DialogResult.OK;
      this.Close();
    }

    private void SetProgress(long Current, long Max) {
      this.prbScanProgress.Value = (int) (Math.Min((decimal) Current / Max, 1.0M) * this.prbScanProgress.Maximum);
      Application.DoEvents();
    }

    private byte Rotate(byte B, int Bits) {
      return (byte) ((B >> Bits) | (B << (8 - Bits)));
    }

    private int CountBits(byte B) {
    int Count = 0;
      while (B != 0) {
       if ((B & 0x01) != 0)
         ++Count;
        B >>= 1;
      }
      return Count;
    }

    private bool DecodeInfoBlock(int Index, byte[] Bytes) {
    int BitCount = this.CountBits(Bytes[2]) - this.CountBits(Bytes[11]) + this.CountBits(Bytes[12]);
    byte ShiftSize = 0;
      switch (Math.Abs(BitCount) % 5) {
       case 0: ShiftSize = 7; break;
       case 1: ShiftSize = 1; break;
       case 2: ShiftSize = 6; break;
       case 3: ShiftSize = 2; break;
       case 4: ShiftSize = 5; break;
      }
      if (ShiftSize == 0 || Index != this.Rotate(Bytes[0], ShiftSize) + (this.Rotate(Bytes[1], ShiftSize) << 8))
	return false;
      for (int i = 0; i < Bytes.Length; ++i)
       Bytes[i] = this.Rotate(Bytes[i], ShiftSize);
      return true;
    }

    #region Abilities

    private void ScanAbilityFile(BinaryReader BR) {
      this.lblScanProgress.Text = I18N.GetText("AbilityCheck");
      this.SetProgress(0, 1);
      BR.BaseStream.Seek(0, SeekOrigin.Begin);
      if ((BR.BaseStream.Length % 0x400) != 0)
	return;
    long EntryCount = BR.BaseStream.Length / 0x400;
    Encoding E = new FFXIEncoding();
      this.lblScanProgress.Text = I18N.GetText("AbilityLoad");
      // Block Layout:
      // 000-001 U16 Index
      // 002-003 U16 Unknown
      // 004-005 U16 MP Cost
      // 006-007 U16 Cooldown
      // 008-008 U8  Target (1 = Self, 5 = PT Member, 20 = Monster)
      // 009-009 U8  Unknown (NUL?)
      // 00a-029 TXT Name
      // 02a-129 TXT Description (exact length unknown)
      // 12a-3fe U8  Padding (NULs)
      // 3ff-3ff U8  End marker (0xff)
      for (int i = 0; i < EntryCount; ++i) {
      byte[] Bytes = BR.ReadBytes(0x400);
	if (Bytes[0x3ff] != 0xff || Bytes[9] != 0x00 || !this.DecodeInfoBlock(i, Bytes)) {
	  this.StringTableEntries.Clear();
	  return;
	}
      string TextVersion = String.Empty;
	TextVersion += String.Format("\u3010{0}\u3011 ", E.GetString(Bytes, 10, 32).TrimEnd('\0'));
      int MP = Bytes[4] + (Bytes[5] << 8);
	if (MP != 0)
	  TextVersion += String.Format("(Cost: {0} MP) ", MP);
      int Cooldown = Bytes[6] + (Bytes[7] << 8);
	if (Cooldown != 0)
	  TextVersion += String.Format("{{{0}}} ", new TimeSpan(0, 0, Cooldown));
	switch (Bytes[8]) {
	  case  0: TextVersion += "<Trait> ";         break;
	  case  1: TextVersion += "<Target: Self> ";  break;
	  case  5: TextVersion += "<Target: Party> "; break;
	  case 32: TextVersion += "<Target: Enemy> "; break;
	  default:
	    this.StringTableEntries.Clear();
	    return;
	}
	TextVersion += E.GetString(Bytes, 42, 256).TrimEnd('\0');
	this.StringTableEntries.Add(TextVersion);
	if (FileScanDialog.ShowProgressDetails)
	  this.lblScanProgress.Text = String.Format(I18N.GetText("AbilityLoadProgress"), i + 1, EntryCount);
	this.SetProgress(i + 1, EntryCount);
      }
    }

    #endregion

    #region Dialog Files

    private void ScanDialogFile(BinaryReader BR) {
      this.lblScanProgress.Text = I18N.GetText("DialogCheck");
      this.SetProgress(0, 1);
      BR.BaseStream.Seek(0, SeekOrigin.Begin);
      if (BR.BaseStream.Length < 4)
	return;
    uint FileSizeMaybe = BR.ReadUInt32();
      if (FileSizeMaybe != (0x10000000 + BR.BaseStream.Length - 4))
	return;
    uint FirstTextPos = (BR.ReadUInt32() ^ 0x80808080);
      if ((FirstTextPos % 4) != 0 || FirstTextPos > BR.BaseStream.Length || FirstTextPos < 8)
	return;
    uint EntryCount = FirstTextPos / 4;
    Encoding E = new FFXIEncoding();
      this.lblScanProgress.Text = I18N.GetText("DialogLoad");
      for (int i = 0; i < EntryCount; ++i) {
	BR.BaseStream.Seek(4 + 4 * i, SeekOrigin.Begin);
      long Offset = (BR.ReadUInt32() ^ 0x80808080);
      long NextOffset = (((i + 1) == EntryCount) ? BR.BaseStream.Length : (BR.ReadUInt32() ^ 0x80808080));
	if (NextOffset < Offset || NextOffset > (Offset + 1024)) { // Sanity check - the 1024 is arbitrary
	  this.StringTableEntries.Clear();
	  return;
	}
	BR.BaseStream.Seek(4 + Offset, SeekOrigin.Begin);
      byte[] TextBytes = BR.ReadBytes((int) (NextOffset - Offset));
	for (int j = 0; j < TextBytes.Length; ++j)
	  TextBytes[j] ^= 0x80;
      string Entry = String.Empty;
      int LastPos = 0;
	for (int j = 0; j < TextBytes.Length; ++j) {
	  if (TextBytes[j] == 0x07) { // Line Break
	    if (LastPos < j)
	      Entry += E.GetString(TextBytes, LastPos, j - LastPos);
	    Entry += String.Format("{0}NewLine{1}", FFXIEncoding.SpecialMarkerStart, FFXIEncoding.SpecialMarkerEnd);
	    LastPos = j + 1;
	  }
	  else if (TextBytes[j] == 0x08) { // Character Name (You)
	    if (LastPos < j)
	      Entry += E.GetString(TextBytes, LastPos, j - LastPos);
	    Entry += String.Format("{0}Player Name{1}", FFXIEncoding.SpecialMarkerStart, FFXIEncoding.SpecialMarkerEnd);
	    LastPos = j + 1;
	  }
	  else if (TextBytes[j] == 0x09) { // Character Name (They)
	    if (LastPos < j)
	      Entry += E.GetString(TextBytes, LastPos, j - LastPos);
	    Entry += String.Format("{0}Speaker Name{1}", FFXIEncoding.SpecialMarkerStart, FFXIEncoding.SpecialMarkerEnd);
	    LastPos = j + 1;
	  }
	  else if (TextBytes[j] == 0x0a && j + 1 < TextBytes.Length) {
	    if (LastPos < j)
	      Entry += E.GetString(TextBytes, LastPos, j - LastPos);
	    Entry += String.Format("{0}Numeric Parameter {1}{2}", FFXIEncoding.SpecialMarkerStart, TextBytes[j + 1], FFXIEncoding.SpecialMarkerEnd);
	    LastPos = j + 2;
	    ++j;
	  }
	  else if (TextBytes[j] == 0x0b) { // Indicates that the lines after this are in a prompt window
	    if (LastPos < j)
	      Entry += E.GetString(TextBytes, LastPos, j - LastPos);
	    Entry += String.Format("{0}Selection Dialog{1}", FFXIEncoding.SpecialMarkerStart, FFXIEncoding.SpecialMarkerEnd);
	    LastPos = j + 1;
	  }
	  else if (TextBytes[j] == 0x0c && j + 1 < TextBytes.Length) {
	    if (LastPos < j)
	      Entry += E.GetString(TextBytes, LastPos, j - LastPos);
	    Entry += String.Format("{0}Multiple Choice (Parameter {1}){2}", FFXIEncoding.SpecialMarkerStart, TextBytes[j + 1], FFXIEncoding.SpecialMarkerEnd);
	    LastPos = j + 2;
	    ++j;
	  }
	  else if (TextBytes[j] == 0x19 && j + 1 < TextBytes.Length) {
	    if (LastPos < j)
	      Entry += E.GetString(TextBytes, LastPos, j - LastPos);
	    Entry += String.Format("{0}Item Parameter {1}{2}", FFXIEncoding.SpecialMarkerStart, TextBytes[j + 1], FFXIEncoding.SpecialMarkerEnd);
	    LastPos = j + 2;
	    ++j;
	  }
	  else if (TextBytes[j] == 0x1a && j + 1 < TextBytes.Length) {
	    if (LastPos < j)
	      Entry += E.GetString(TextBytes, LastPos, j - LastPos);
	    Entry += String.Format("{0}Marker: {1:X2}{2:X2}{3}", FFXIEncoding.SpecialMarkerStart, TextBytes[j + 0], TextBytes[j + 1], FFXIEncoding.SpecialMarkerEnd);
	    LastPos = j + 2;
	    ++j;
	  }
	  else if (TextBytes[j] == 0x1e && j + 1 < TextBytes.Length) {
	    if (LastPos < j)
	      Entry += E.GetString(TextBytes, LastPos, j - LastPos);
	    Entry += String.Format("{0}Set Color #{1}{2}", FFXIEncoding.SpecialMarkerStart, TextBytes[j + 1], FFXIEncoding.SpecialMarkerEnd);
	    LastPos = j + 2;
	    ++j;
	  }
	  else if (TextBytes[j] == 0x7f && j + 2 < TextBytes.Length) { // Unknown Type of Text Substitution
	    if (LastPos < j)
	      Entry += E.GetString(TextBytes, LastPos, j - LastPos);
	    Entry += String.Format("{0}Marker: {1:X2}{2:X2}{3:X2}{4}", FFXIEncoding.SpecialMarkerStart, TextBytes[j + 0], TextBytes[j + 1], TextBytes[j + 2], FFXIEncoding.SpecialMarkerEnd);
	    LastPos = j + 3;
	    j += 2;
	  }
#if DEBUG
	  else if (TextBytes[j] < 0x20) {
	    if (LastPos < j)
	      Entry += E.GetString(TextBytes, LastPos, j - LastPos);
	    Entry += String.Format("{0}Possible Special Code: {2:X2}{1}", FFXIEncoding.SpecialMarkerStart, FFXIEncoding.SpecialMarkerEnd, TextBytes[j]);
	    LastPos = j + 1;
	  }
#endif
	}
	if (LastPos < TextBytes.Length)
	  Entry += E.GetString(TextBytes, LastPos, TextBytes.Length - LastPos);
	this.StringTableEntries.Add(Entry);
	if (FileScanDialog.ShowProgressDetails)
	  this.lblScanProgress.Text = String.Format(I18N.GetText("DialogLoadProgress"), i + 1, EntryCount);
	this.SetProgress(i + 1, EntryCount);
      }
    }

    #endregion

    #region Images

    private void ScanImages(BinaryReader BR) {
      if (BR.BaseStream.Length < 421) // one image header
	return;
    long MaxPos = BR.BaseStream.Length - 421;
    long Pos = 0;
    int ImageCount = 0;
      this.lblScanProgress.Text = String.Format(I18N.GetText("ImageScan"), ImageCount);
      this.SetProgress(0, 1);
      BR.BaseStream.Seek(Pos, SeekOrigin.Begin);
      while (Pos < MaxPos) {
      FFXIGraphic FG = FFXIGraphic.Read(BR);
	if (FG != null) {
	  this.Images.Add(FG);
	  Pos = BR.BaseStream.Position;
	  this.SetProgress(Pos + 1, MaxPos);
	  if (FileScanDialog.ShowProgressDetails)
	    this.lblScanProgress.Text = String.Format(I18N.GetText("ImageScanProgress"), ++ImageCount);
	}
	else {
	  BR.BaseStream.Seek(++Pos, SeekOrigin.Begin);
	  if (Pos == MaxPos || (Pos % 0x100) == 0)
	    this.SetProgress(Pos + 1, MaxPos);
	}
      }
    }

    #endregion

    #region Item Data

    private void ScanItemData(BinaryReader BR) {
      this.lblScanProgress.Text = I18N.GetText("ItemCheck");
      this.SetProgress(0, 1);
      if (BR.BaseStream.Length < 0xc00 || (BR.BaseStream.Length % 0xc00) != 0)
	return;
      BR.BaseStream.Seek(0, SeekOrigin.Begin);
    long ItemCount = BR.BaseStream.Length / 0xc00;
      this.lblScanProgress.Text = I18N.GetText("ItemLoad");
      for (long i = 0; i < ItemCount; ++i) {
      byte[] ItemData  = BR.ReadBytes(0xc00);
	for (int j = 0; j < 0xc00; ++j)
	  ItemData[j] = this.Rotate(ItemData[j], 3);
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
	if (FileScanDialog.ShowProgressDetails)
	  this.lblScanProgress.Text = String.Format(I18N.GetText("ItemLoadProgress"), i + 1, ItemCount);
	this.SetProgress(i + 1, ItemCount);
      }
    }

    #endregion

    #region Spell Info

    // Same general layout as the ability info, different data
    private void ScanSpellFile(BinaryReader BR) {
      this.lblScanProgress.Text = I18N.GetText("SpellCheck");
      this.SetProgress(0, 1);
      BR.BaseStream.Seek(0, SeekOrigin.Begin);
      if ((BR.BaseStream.Length % 0x400) != 0)
	return;
    long EntryCount = BR.BaseStream.Length / 0x400;
    Encoding E = new FFXIEncoding();
      this.lblScanProgress.Text = I18N.GetText("SpellLoad");
      // Block Layout:
      // 000-001 U16 Index
      // 002-003 U16 Spell Type (1/2/3/4/5 - White/Black/Summon/Ninja/Bard)
      // 004-005 U16 Unknown
      // 006-007 U16 Unknown (Targeting Flags; 0x9D = Dead Player, 0x20 = Monster, etc.)?
      // 008-009 U16 Unknown
      // 00a-00b U16 MP Cost
      // 00c-00c U8  Cast Time (1/4 second)
      // 00d-00d U8  Recast Time (1/4 second)
      // 00f-01e U8  Level required (1 byte per job, 0xff if not learnable; first is for the NUL job, so always 0xff)
      // 01f-032 TXT Japanese Name
      // 033-046 TXT English Name
      // 047-0C6 TXT Japanese Description
      // 0C7-146 TXT English Description
      // 147-3fe U8  Padding (NULs)
      // 3ff-3ff U8  End marker (0xff)
      for (int i = 0; i < EntryCount; ++i) {
      byte[] Bytes = BR.ReadBytes(0x400);
	if (Bytes[0xf] != 0xff || Bytes[0x3ff] != 0xff || !this.DecodeInfoBlock(i, Bytes)) {
	  this.StringTableEntries.Clear();
	  return;
	}
      string TextVersion = String.Empty;
	{ // Spell Type
	int SpellType = Bytes[0x2] + (Bytes[0x3] << 8);
	  TextVersion += '\u3010';
	  switch (SpellType) {
	    case 0: TextVersion += "None";        break;
	    case 1: TextVersion += "White Magic"; break;
	    case 2: TextVersion += "Black Magic"; break;
	    case 3: TextVersion += "Summons";     break;
	    case 4: TextVersion += "Ninjutsu";    break;
	    case 5: TextVersion += "Bard Song";   break;
	    default:
	      this.StringTableEntries.Clear();
	      return;
	  }
	  TextVersion += "\u3011 ";
	}
	TextVersion += String.Format("{0} ({1}) ", E.GetString(Bytes, 0x1f, 20).TrimEnd('\0'), E.GetString(Bytes, 0x33, 20).TrimEnd('\0'));
	{ // MP Cost
	int MP = Bytes[0xa] + (Bytes[0xb] << 8);
	  if (MP != 0)
	    TextVersion += String.Format("(Cost: {0} MP) ", MP);
	}
	TextVersion += String.Format("(Cast Time: {0}s) ", Bytes[0xc] / 4.0);
	TextVersion += String.Format("(Recast Time: {0}s) ", Bytes[0xd] / 4.0);
	{ // Minimum Required Job Level (x16)
	string JobInfo = String.Empty;
	  for (int j = 1; j < 16; ++j) {
	    if (Bytes[0x00e + j] != 0xFF) {
	      if (JobInfo != String.Empty) JobInfo += '/';
	      JobInfo += Bytes[0x00e + j].ToString();
	      JobInfo += ((Job) (1 << j)).ToString();
	    }
	  }
	  if (JobInfo != String.Empty)
	    TextVersion += String.Format("[{0}] ", JobInfo);
	}
	TextVersion += String.Format("{0} ({1}) ", E.GetString(Bytes, 0x47, 128).TrimEnd('\0'), E.GetString(Bytes, 0xC7, 128).TrimEnd('\0'));
	this.StringTableEntries.Add(TextVersion);
	if (FileScanDialog.ShowProgressDetails)
	  this.lblScanProgress.Text = String.Format(I18N.GetText("SpellLoadProgress"), i + 1, EntryCount);
	this.SetProgress(i + 1, EntryCount);
      }
    }

    #endregion

    #region Status Effects

    // Coming Soon

    #endregion

    #region String Tables

    private void ScanSimpleStringFile(BinaryReader BR) {
      this.lblScanProgress.Text = I18N.GetText("StringCheck");
      this.SetProgress(0, 1);
      BR.BaseStream.Seek(0, SeekOrigin.Begin);
      if ((BR.BaseStream.Length % 0x40) != 0)
	return;
    long EntryCount = BR.BaseStream.Length / 0x40;
    Encoding E = new FFXIEncoding();
      this.lblScanProgress.Text = I18N.GetText("StringLoad");
      for (int i = 0; i < EntryCount; ++i) {
      uint EntryIndex = BR.ReadUInt32();
      byte[] EntryBytes = BR.ReadBytes(0x3c);
	if (EntryIndex != i || EntryBytes[0x3b] != 0xff) {
	  this.StringTableEntries.Clear();
	  return;
	}
	this.StringTableEntries.Add(E.GetString(EntryBytes, 0, 0x3b).TrimEnd('\0'));
	if (FileScanDialog.ShowProgressDetails)
	  this.lblScanProgress.Text = String.Format(I18N.GetText("StringLoadProgress"), i + 1, EntryCount);
	this.SetProgress(i + 1, EntryCount);
      }
    }

    private void ScanXIStringFile(BinaryReader BR) {
      this.lblScanProgress.Text = I18N.GetText("StringCheck");
      this.SetProgress(0, 1);
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
      this.lblScanProgress.Text = I18N.GetText("StringLoad");
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
	if (FileScanDialog.ShowProgressDetails)
	  this.lblScanProgress.Text = String.Format(I18N.GetText("StringLoadProgress"), i + 1, EntryCount);
	this.SetProgress(i + 1, EntryCount);
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
	this.StringTableEntries.Clear();
	this.Images.Clear();
	this.Items.Clear();
	this.DialogResult = DialogResult.Abort;
      }
    }

  }

}
