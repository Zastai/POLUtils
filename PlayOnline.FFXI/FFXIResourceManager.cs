// $Id$

using System;
using System.Globalization;
using System.IO;
using System.Text;

using PlayOnline.Core;
using PlayOnline.FFXI.Things;

namespace PlayOnline.FFXI {

  // TODO: Make a real design decision on how to decide what language a string resource should be returned as.
  //       Should it be the UI language, the selected POL region, ...?
  // For now, this will always return the English text, except for non-expando autotrans messages, which will be in the
  // language specified by their ID.
  public class FFXIResourceManager {

    private static FFXIEncoding E = new FFXIEncoding();

    public static string GetResourceString(uint ResourceID) {
    string ResourceString = FFXIResourceManager.GetResourceStringInternal(ResourceID);
      return ((ResourceString == null) ? I18N.GetText("BadResID") : ResourceString);
    }

    public static bool IsValidResourceID(uint ResourceID) {
      return (FFXIResourceManager.GetResourceStringInternal(ResourceID) != null);
    }

    public static string GetAreaName(ushort ID) {
      return FFXIResourceManager.GetStringTableEntry(55465, ID); // JP = 55535
    }

    public static string GetJobName(ushort ID) {
      return FFXIResourceManager.GetStringTableEntry(55467, ID); // JP = 55536
    }

    public static string GetRegionName(ushort ID) {
      return FFXIResourceManager.GetStringTableEntry(55654, ID); // JP = 55534
    }

    public static string GetAbilityName(ushort ID) {
    BinaryReader BR = FFXIResourceManager.OpenDATFile(85); // JP = 10
      if (BR != null) {
	if ((ID + 1) * 0x400 <= BR.BaseStream.Length) {
	  BR.BaseStream.Position = ID * 0x400;
	byte[] AbilityData = BR.ReadBytes(0x400);
	  BR.Close();
	  if (FFXIEncryption.DecodeDataBlock(AbilityData))
	    return FFXIResourceManager.E.GetString(AbilityData, 0x0a, 32).TrimEnd('\0');
	}
	BR.Close();
      }
      return null;
    }

    public static string GetSpellName(ushort ID) {
    BinaryReader BR = FFXIResourceManager.OpenDATFile(86);
      if (BR != null) {
	if ((ID + 1) * 0x400 <= BR.BaseStream.Length) {
	  BR.BaseStream.Position = ID * 0x400;
	byte[] SpellData = BR.ReadBytes(0x400);
	  BR.Close();
	  if (FFXIEncryption.DecodeDataBlock(SpellData))
	    return FFXIResourceManager.E.GetString(SpellData, 0x3d, 20).TrimEnd('\0');
	}
	BR.Close();
      }
      return null;
    }

    // JP: 4, 5, 6, 7, 8
    private static ushort[] ItemDATs = new ushort[] { 73, 74, 75, 76, 77 };

    public static string GetItemName(byte Language, ushort ID) {
      foreach (ushort ItemDAT in FFXIResourceManager.ItemDATs) {
      BinaryReader BR = FFXIResourceManager.OpenDATFile(ItemDAT);
	if (BR != null) {
	uint FirstID = 0xFFFFFFFF;
	  try { // We used to get the ID of the very first (dummy) entry - but that was reset to 0 in the July 24th 2006 patch, so now we check the second entry
	    BR.BaseStream.Position = 0xC00;
	  byte[] FirstIDBytes = BR.ReadBytes(4);
	    FFXIEncryption.Rotate(FirstIDBytes, 5);
	    FirstID = FirstIDBytes[0] + (uint) FirstIDBytes[1] * 256 + (uint) FirstIDBytes[2] * 256 * 256 + (uint) FirstIDBytes[3] * 256 * 256 * 256 - 1;
	  } catch { }
	  if (FirstID <= ID && ID <= (FirstID + BR.BaseStream.Length / 0xC00)) {
	  Item.Language L;
	  Item.Type T;
	    BR.BaseStream.Position = 0;
	    Item.DeduceLanguageAndType(BR, out L, out T);
	    BR.BaseStream.Position = 0xC00 * (ID - FirstID);
	    switch (T) {
	      case Item.Type.Armor:      BR.BaseStream.Position += 48; break;
	      case Item.Type.Object:     BR.BaseStream.Position += 36; break;
	      case Item.Type.PuppetItem: BR.BaseStream.Position += 44; break;
	      case Item.Type.Weapon:     BR.BaseStream.Position += 54; break;
	    }
	  byte[] NameBytes = BR.ReadBytes(22);
	    FFXIEncryption.Rotate(NameBytes, 5);
	    BR.Close();
	    return FFXIResourceManager.E.GetString(NameBytes).TrimEnd('\0');
	  }
	  BR.Close();
	}
      }
      return null;
    }

    public static string GetKeyItemName(byte Language, ushort ID) {
    BinaryReader BR = FFXIResourceManager.OpenDATFile(82); // JP = 80
      if (BR != null) {
	if (Encoding.ASCII.GetString(BR.ReadBytes(4)) == "menu" && BR.ReadUInt32() == 0x101) {
	  BR.BaseStream.Position = 0x20;
	  while (BR.BaseStream.Position < BR.BaseStream.Length) {
	  long   Offset    = BR.BaseStream.Position;
	  string ShortName = Encoding.ASCII.GetString(BR.ReadBytes(4));
	  uint   SizeInfo  = BR.ReadUInt32();
	    if (BR.ReadUInt64() != 0)
	      break;
	    if (ShortName == "sc_i") {
	      BR.BaseStream.Position += 0x14;
	    uint EntryCount = BR.ReadUInt32();
	      for (uint i = 0; i < EntryCount; ++i) {
		if (BR.ReadUInt32() == ID) {
		  BR.BaseStream.Position += 4;
		  BR.BaseStream.Position = Offset + 0x10 + BR.ReadUInt32();
		  return FFXIEncryption.ReadEncodedString(BR, FFXIResourceManager.E);
		}
		BR.BaseStream.Position += 16;
	      }
	    }
	    // Skip to next one
	    BR.BaseStream.Position = Offset + ((SizeInfo & 0xFFFFFF80) >> 3);
	  }
	}
	BR.Close();
      }
      return null;
    }

    public static string GetAutoTranslatorMessage(byte Category, byte Language, ushort ID) {
    BinaryReader BR = FFXIResourceManager.OpenDATFile(55665); // JP = 55545
      if (BR != null) {
	while (BR.BaseStream.Position + 76 <= BR.BaseStream.Length) {
	byte   GroupCat  = BR.ReadByte();
	byte   GroupLang = BR.ReadByte();
	ushort GroupID   = (ushort) (BR.ReadByte() * 256 + BR.ReadByte());
	  BR.BaseStream.Position += 64;
	uint   Messages  = BR.ReadUInt32();
	uint   DataBytes = BR.ReadUInt32();
	  if (GroupID == (ID & 0xff00)) { // We found the right group (ignoring category & language for now)
	    for (uint i = 0; i < Messages && BR.BaseStream.Position + 5 < BR.BaseStream.Length; ++i) {
	    byte   MessageCat  = BR.ReadByte();
	    byte   MessageLang = BR.ReadByte();
	    ushort MessageID   = (ushort) (BR.ReadByte() * 256 + BR.ReadByte());
	    byte   TextLength  = BR.ReadByte();
	      if (MessageID == ID) { // We found the right message (ignoring category & language for now)
	      byte[] MessageBytes = BR.ReadBytes(TextLength);
		BR.Close();
	      string MessageText = FFXIResourceManager.E.GetString(MessageBytes).TrimEnd('\0');
		return FFXIResourceManager.MaybeExpandAutoTranslatorMessage(MessageText);
	      }
	      else {
		BR.BaseStream.Position += TextLength;
		if (MessageLang == 0x04) { // There is an extra string to skip for Japanese entries
		  TextLength  = BR.ReadByte();
		  BR.BaseStream.Position += TextLength;
		}
	      }
	    }
	  }
	  else
	    BR.BaseStream.Position += DataBytes;
	}
	BR.Close();
      }
      return null;
    }

    private static string GetResourceStringInternal(uint ResourceID) {
    byte   Category = (byte) ((ResourceID >> 24) & 0xff);
    byte   Language = (byte) ((ResourceID >> 16) & 0xff);
    ushort ID       = (ushort) (ResourceID & 0xffff);
      switch (Category) {
	case 0x00: // My own additions (scheduled for removal now that there are separate accessors for them)
	  switch (Language) {
	    case 1: return FFXIResourceManager.GetRegionName(ID);
	    case 2: return FFXIResourceManager.GetAreaName(ID);
	    case 3: return FFXIResourceManager.GetJobName(ID);
	    case 4: return FFXIResourceManager.GetAbilityName(ID);
	    case 5: return FFXIResourceManager.GetSpellName(ID);
	  }
	  break;
	case 0x02: return FFXIResourceManager.GetAutoTranslatorMessage(Category, Language, ID);
	case 0x04: return FFXIResourceManager.GetAutoTranslatorMessage(Category, Language, ID);
	case 0x06: return FFXIResourceManager.GetItemName(Language, ID);
	case 0x07: return FFXIResourceManager.GetItemName(Language, ID);
	case 0x08: return FFXIResourceManager.GetItemName(Language, ID);
	case 0x09: return FFXIResourceManager.GetItemName(Language, ID);
	case 0x13: return FFXIResourceManager.GetKeyItemName(Language, ID);
      }
      return null;
    }

    private static string GetStringTableEntry(ushort FileNumber, ushort ID) {
    BinaryReader BR = FFXIResourceManager.OpenDATFile(FileNumber);
      if (BR != null) {
	// Assume the header's fine - just skip to the relevant bits
	BR.BaseStream.Position += 20;
      uint EntryCount = BR.ReadUInt32();
	BR.BaseStream.Position += 4;
	if (ID < EntryCount && BR.ReadUInt32() == BR.BaseStream.Length) {
	uint HeaderBytes = BR.ReadUInt32();
	uint EntryBytes  = BR.ReadUInt32();
	uint DataBytes   = BR.ReadUInt32();
	  if (HeaderBytes == 0x38 && EntryBytes == EntryCount * 36 && BR.BaseStream.Length == HeaderBytes + EntryBytes + DataBytes) {
	    BR.BaseStream.Position = HeaderBytes + ID * 36;
	  uint Offset = BR.ReadUInt32();
	    BR.BaseStream.Position += 4;
	  ushort Length = BR.ReadUInt16();
	    BR.BaseStream.Position = HeaderBytes + EntryBytes + Offset;
	  string Text = FFXIResourceManager.E.GetString(BR.ReadBytes(Length)).TrimEnd('\0');
	    BR.Close();
	    return Text;
	  }
	}
	BR.Close();
      }
      return null;
    }

    private static string MaybeExpandAutoTranslatorMessage(string Text) {
      // Reference to a string table entry? => return referenced string
      if (Text != null && Text.Length > 2 && Text.Length <= 6 && Text[0] == '@') {
      char ReferenceType = Text[1];
	try {
	ushort ID = ushort.Parse(Text.Substring(2), NumberStyles.AllowHexSpecifier);
	  switch (ReferenceType) {
	    case 'A': return FFXIResourceManager.GetAreaName   (ID);
	    case 'C': return FFXIResourceManager.GetSpellName  (ID);
	    case 'J': return FFXIResourceManager.GetJobName    (ID);
	    case 'Y': return FFXIResourceManager.GetAbilityName(ID);
	  }
	} catch { }
      }
      return Text;
    }

    private static BinaryReader OpenDATFile(ushort FileNumber) {
      try {
      string FullDATFileName = FFXI.GetFilePath(FileNumber);
	if (File.Exists(FullDATFileName))
	  return new BinaryReader(new FileStream(FullDATFileName, FileMode.Open, FileAccess.Read));
      } catch { }
      return null;
    }

  }

}