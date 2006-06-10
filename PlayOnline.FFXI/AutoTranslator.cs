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

      public byte              Category; // 2 = English AT Text, 4 = Japanese AT Text With Alternate
      public byte              Language; // 1 = Japanese, 2 = English
      public byte              ID;
      public byte              ParentGroup;
      public string            Title;
      public string            Description;
      public MessageCollection Messages;

      public uint ResID { get { return (uint) ((this.Category << 24) + (this.Language << 16) + (this.ParentGroup << 8) + this.ID); } }

      internal MessageGroup() {
	this.Category    = 0;
	this.Language    = 0;
	this.ID          = 0;
	this.ParentGroup = 0;
	this.Title       = String.Empty;
	this.Description = String.Empty;
	this.Messages    = new MessageCollection();
      }

    }

    public class Message {

      public byte Category; // 2 = English AT Text, 4 = Japanese AT Text With Alternate
      public byte Language; // 1 = Japanese, 2 = English
      public byte ID;
      public byte ParentGroup;

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

      public uint ResID { get { return (uint) ((this.Category << 24) + (this.Language << 16) + (this.ParentGroup << 8) + this.ID); } }

      internal Message() {
	this.Category      = 0;
	this.Language      = 0;
	this.ID            = 0;
	this.ParentGroup   = 0;
	this.Text          = String.Empty;
	this.AlternateText = String.Empty;
      }

      private string MaybeExpand(ref string Text) {
	// Reference to a string table entry? => return referenced string
	if (Text != null && Text.Length > 2 && Text.Length <= 6 && Text[0] == '@') {
	char ReferenceType = Text[1];
	  try {
	  uint EntryNumber = uint.Parse(Text.Substring(2), NumberStyles.AllowHexSpecifier);
	    switch (ReferenceType) {
	      case 'A': Text = FFXIResourceManager.GetResourceString(0x00020000U + EntryNumber); break; // Area
	      case 'C': Text = FFXIResourceManager.GetResourceString(0x00050000U + EntryNumber); break; // Spell
	      case 'J': Text = FFXIResourceManager.GetResourceString(0x00030000U + EntryNumber); break; // Job
	      case 'Y': Text = FFXIResourceManager.GetResourceString(0x00040000U + EntryNumber); break; // Ability
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
      MG.Category    = BR.ReadByte();
      MG.Language    = BR.ReadByte();
      MG.ID          = BR.ReadByte();
      MG.ParentGroup = BR.ReadByte();
      MG.Title       = E.GetString(BR.ReadBytes(32)).TrimEnd('\0');
      MG.Description = E.GetString(BR.ReadBytes(32)).TrimEnd('\0');
    uint MessageCount = BR.ReadUInt32();
      /* MessageBytes */ BR.ReadUInt32();
      for (int i = 0; i < MessageCount; ++i)
	MG.Messages.Add(AutoTranslator.ReadMessage(BR, E));
      return MG;
    }

    private static Message ReadMessage(BinaryReader BR, Encoding E) {
    Message M = new Message();
      M.Category    = BR.ReadByte();
      M.Language    = BR.ReadByte();
      M.ParentGroup = BR.ReadByte();
      M.ID          = BR.ReadByte();
      M.Text        = E.GetString(BR.ReadBytes(BR.ReadByte())).TrimEnd('\0');
      if (M.Category == 4)
	M.AlternateText = E.GetString(BR.ReadBytes(BR.ReadByte())).TrimEnd('\0');
      return M;
    }

  }

}
