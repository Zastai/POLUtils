using System;
using System.Collections;
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
      public ushort Category; // 0x0202 = English Text, 0x0104 = Japanese Text, 0x0702 = Item Name
      public string Text;
      public string AlternateText;

      internal Message() {
	this.ParentGroup   = 0;
	this.ID            = 0;
	this.Category      = 0;
	this.Text          = String.Empty;
	this.AlternateText = String.Empty;
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

    public static string GetMessage(byte ParentGroup, byte ID, ushort Category) {
    string Text = "???";
      if (AutoTranslator.Data != null) {
	foreach (MessageGroup MG in AutoTranslator.Data) {
	  foreach (Message M in MG.Messages) {
	    if (M.ParentGroup == ParentGroup && M.ID == ID && M.Category == Category)
	      Text = M.Text;
	  }
	}
      }
      return Text;
    }

    private static void LoadData() {
      AutoTranslator.Data_ = new MessageGroupCollection();
      try { // Get pure autotranslator text
      string DataFilePath = Path.Combine(POL.GetApplicationPath(AppID.FFXI), "ROM/76/23.DAT");
      FileStream FS = new FileStream(DataFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
      Encoding E = new POLEncoding();
      BinaryReader BR = new BinaryReader(FS, E);
	while (FS.Position < FS.Length)
	  AutoTranslator.Data_.Add(AutoTranslator.ReadMessageGroup(BR, E));
	BR.Close();
      } catch (Exception E) { Console.WriteLine(E.ToString()); }
#if !false
      try { // Get item names - weapons
      string DataFilePath = Path.Combine(POL.GetApplicationPath(AppID.FFXI), "ROM/118/108.DAT");
      FileStream FS = new FileStream(DataFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
      Encoding E = new POLEncoding();
      BinaryReader BR = new BinaryReader(FS, E);
      MessageGroup MG = new MessageGroup();
	MG.Category = 0x0207;
	MG.Title = MG.Description = "Weapon Names";
	MG.ParentGroup = 0;
	MG.ID = 0;
	while (FS.Position < FS.Length) {
	Message M = new Message();
	  M.Category = 0x0207;
	  { byte B = BR.ReadByte(); M.ID          = (byte) ((B << 3) | (B >> 5)); }
	  { byte B = BR.ReadByte(); M.ParentGroup = (byte) ((B << 3) | (B >> 5)); }
	  FS.Seek(0x1C, SeekOrigin.Current);
	  { // JP Name
	  byte[] EncodedBytes = BR.ReadBytes(22);
	    for (int i = 0; i < EncodedBytes.Length; ++i)
	      EncodedBytes[i] = (byte) ((EncodedBytes[i] << 3) | (EncodedBytes[i] >> 5));
	  string DecodedText = E.GetString(EncodedBytes).TrimEnd('\0');
	    if (POL.SelectedRegion == POL.Region.Japan)
	      M.Text = DecodedText;
	    else
	      M.AlternateText = DecodedText;
	  }
	  { // EN Name
	  byte[] EncodedBytes = BR.ReadBytes(22);
	    for (int i = 0; i < EncodedBytes.Length; ++i)
	      EncodedBytes[i] = (byte) ((EncodedBytes[i] << 3) | (EncodedBytes[i] >> 5));
	  string DecodedText = E.GetString(EncodedBytes).TrimEnd('\0');
	    if (POL.SelectedRegion == POL.Region.Japan)
	      M.AlternateText = DecodedText;
	    else
	      M.Text = DecodedText;
	  }
	  FS.Seek(0xC00 - 2 - 0x1C - 22 - 22, SeekOrigin.Current);
	  MG.Messages.Add(M);
	}
	BR.Close();
	AutoTranslator.Data_.Add(MG);
      } catch (Exception E) { Console.WriteLine(E.ToString()); }
#endif
      try { // Get key item names
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
      if (BR.BaseStream.Position != MessageStart + MessageBytes)
	Console.WriteLine("Advanced {0} bytes reading group, but group specified a size of {0} bytes.", BR.BaseStream.Position - MessageStart, MessageBytes);
      return MG;
    }

    private static Message ReadMessage(BinaryReader BR, Encoding E) {
    Message M = new Message();
      M.Category = BR.ReadUInt16();
      M.ParentGroup = BR.ReadByte();
      M.ID = BR.ReadByte();
    uint TextByteCount = BR.ReadUInt32();
      M.Text = E.GetString(BR.ReadBytes((int) TextByteCount)).TrimEnd('\0');
      if (M.Category == 0x0104) { // Read alternate text
	TextByteCount = BR.ReadUInt32();
	M.AlternateText = E.GetString(BR.ReadBytes((int) TextByteCount)).TrimEnd('\0');
      }
      return M;
    }

  }

}
