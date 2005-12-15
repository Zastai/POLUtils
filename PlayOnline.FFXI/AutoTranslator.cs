// $Id$

using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;

using PlayOnline.Core;

namespace PlayOnline.FFXI {

  public class AutoTranslator {

    public class MessageGroup {

      public byte              ParentGroup;
      public byte              ID;
      public ushort            Category; // 0x0202 = English Message, 0x0104 = Japanese Message, ...
      public string            Title;
      public string            Description;
      public MessageCollection Messages;

      internal MessageGroup() {
	this.ParentGroup = 0;
	this.ID          = 0;
	this.Category    = 0;
	this.Title       = String.Empty;
	this.Description = String.Empty;
	this.Messages    = new MessageCollection();
      }

    }

    public class Message {

      public byte   ParentGroup;
      public byte   ID;
      public ushort Category; // 0x0202 = English Text, 0x0104 = Japanese Text

      public string Text {
	get { return this.MaybeExpand(ref this.Text_); }
	set { this.Text_ = value; }
      }
      private string Text_;

      public string AlternateText {
	get { return this.MaybeExpand(ref this.AlternateText_); }
	set { this.AlternateText_ = value; }
      }
      private string AlternateText_;

      internal Message() {
	this.ParentGroup   = 0;
	this.ID            = 0;
	this.Category      = 0;
	this.Text          = String.Empty;
	this.AlternateText = String.Empty;
      }

      private string GetStringTableEntry(string File, byte EntryNum) {
      FileStream FS = null;
	try {
	  FS = new FileStream(Path.Combine(POL.GetApplicationPath(AppID.FFXI), File), FileMode.Open, FileAccess.Read);
	BinaryReader BR = new BinaryReader(FS, Encoding.ASCII);
	  // First: Minimal Sanity Check
	  BR.BaseStream.Seek(0x20, SeekOrigin.Begin);
	  if (BR.ReadUInt32() != BR.BaseStream.Length)
	    return null;
	uint EntryCount = BR.ReadUInt32();
	uint EntryBytes = BR.ReadUInt32();
	  if (EntryCount < EntryNum || EntryBytes != EntryCount * 12)
	    return null;
	  // Read the offset & size
	  BR.BaseStream.Seek(0x38 + 12 * EntryNum, SeekOrigin.Begin);
	uint  Offset = BR.ReadUInt32();
	short Size = BR.ReadInt16();
	  if (Size < 0 || Offset + Size > BR.BaseStream.Length)
	    return null;
	  // Read the actual string
	  BR.BaseStream.Seek(0x38 + EntryBytes + Offset, SeekOrigin.Begin);
	FFXIEncoding E = new FFXIEncoding();
	  return E.GetString(BR.ReadBytes(Size)).TrimEnd('\0');
	} catch {
	  return null;
	} finally {
	  if (FS != null)
	    FS.Close();
	}
      }

      private string GetAreaName(byte EntryNum) {
      string Name = this.GetStringTableEntry(((this.Category == 0x104) ? "Rom/97/56.dat" : "Rom/97/53.dat"), EntryNum);
	if (Name != null)
	  return Name;
	return String.Format("<Place Name #{0}>", EntryNum);
      }

      private string GetJobName(byte EntryNum) {
      string Name = this.GetStringTableEntry(((this.Category == 0x104) ? "Rom/97/29.dat" : "Rom/97/55.dat"), EntryNum);
	if (Name != null)
	  return Name;
	return String.Format("<Job Name #{0}>", EntryNum);
      }

      private string MaybeExpand(ref string Text) {
	// Reference to a string table entry? => return referenced string
	if (Text != null && Text.Length == 4 && Text[0] == '@') {
	char ReferenceType = Text[1];
	  try {
	  byte EntryNumber = byte.Parse(Text.Substring(2), NumberStyles.AllowHexSpecifier);
	    switch (ReferenceType) {
	      case 'A': Text = this.GetAreaName(EntryNumber); break;
	      case 'J': Text = this.GetJobName (EntryNumber); break;
	    }
	  } catch { }
	}
	return Text;
      }

    }

    public static MessageGroupCollection Data {
      get {
	if (AutoTranslator.Data_ == null)
	  AutoTranslator.LoadData();
	return AutoTranslator.Data_;
      }
    }
    private static MessageGroupCollection Data_ = null;

    private static void LoadData() {
      AutoTranslator.Data_ = new MessageGroupCollection();
      try {
      string DataFilePath = Path.Combine(POL.GetApplicationPath(AppID.FFXI), "ROM/76/23.DAT");
      FileStream FS = new FileStream(DataFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
      Encoding E = new FFXIEncoding();
      BinaryReader BR = new BinaryReader(FS, E);
	while (FS.Position < FS.Length)
	  AutoTranslator.Data_.Add(AutoTranslator.ReadMessageGroup(BR, E));
	BR.Close();
      } catch (Exception E) { Console.WriteLine(E.ToString()); }
    }

    private static MessageGroup ReadMessageGroup(BinaryReader BR, Encoding E) {
    MessageGroup MG = new MessageGroup();
      MG.Category    = BR.ReadUInt16();
      MG.ID          = BR.ReadByte();
      MG.ParentGroup = BR.ReadByte();
      MG.Title       = E.GetString(BR.ReadBytes(32)).TrimEnd('\0');
      MG.Description = E.GetString(BR.ReadBytes(32)).TrimEnd('\0');
    uint MessageCount = BR.ReadUInt32();
    uint MessageBytes = BR.ReadUInt32();
    long MessageStart = BR.BaseStream.Position;
      for (int i = 0; i < MessageCount; ++i)
	MG.Messages.Add(AutoTranslator.ReadMessage(BR, E));
      return MG;
    }

    private static Message ReadMessage(BinaryReader BR, Encoding E) {
    Message M = new Message();
      M.Category = BR.ReadUInt16();
      M.ParentGroup = BR.ReadByte();
      M.ID = BR.ReadByte();
      M.Text = E.GetString(BR.ReadBytes(BR.ReadByte())).TrimEnd('\0');
      if (M.Category == 0x0104) // Read alternate text
	M.AlternateText = E.GetString(BR.ReadBytes(BR.ReadByte())).TrimEnd('\0');
      return M;
    }

  }

}
