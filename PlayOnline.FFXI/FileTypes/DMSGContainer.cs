// $Id$

using System;
using System.IO;
using System.Collections.Generic;

using PlayOnline.Core;

namespace PlayOnline.FFXI.FileTypes {

  public class DMSGContainer : FileType {

    public override ThingList Load(BinaryReader BR, ProgressCallback ProgressCallback) {
    ThingList TL = new ThingList();
      if (ProgressCallback != null)
	ProgressCallback(I18N.GetText("FTM:CheckingFile"), 0);
      if (BR.BaseStream.Length < 0x40 || BR.BaseStream.Position != 0)
	return TL;
    FFXIEncoding E = new FFXIEncoding();
      if (E.GetString(BR.ReadBytes(8)) != "d_msg".PadRight(8, '\0'))
	return TL;
    ushort Flag1 = BR.ReadUInt16();
      if (Flag1 != 0 && Flag1 != 1)
	return TL;
    ushort Flag2 = BR.ReadUInt16();
      if (Flag2 != 0 && Flag2 != 1)
	return TL;
      if (BR.ReadUInt32() != 3 || BR.ReadUInt32() != 3)
	return TL;
    uint FileSize = BR.ReadUInt32();
      if (FileSize != BR.BaseStream.Length)
	return TL;
    uint HeaderBytes = BR.ReadUInt32();
      if (HeaderBytes != 0x40)
	return TL;
      if (BR.ReadUInt32() != 0)
	return TL;
    int BytesPerEntry = BR.ReadInt32();
      if (BytesPerEntry < 0)
	return TL;
    uint DataBytes = BR.ReadUInt32();
      if (FileSize != (HeaderBytes + DataBytes) || (DataBytes % BytesPerEntry) != 0)
	return TL;
    uint EntryCount = BR.ReadUInt32();
      if (EntryCount * BytesPerEntry != DataBytes)
	return TL;
      if (BR.ReadUInt32() != 1 || BR.ReadUInt64() != 0 || BR.ReadUInt64() != 0)
	return TL;
      if (ProgressCallback != null)
	ProgressCallback(I18N.GetText("FTM:LoadingData"), 0);
      // Unlike previous d_msg files, these are really a container for any number of data block types
      // Known Data Sizes:
      // - Ability Info 1     = 0x050 (EN, JP) / 0x0A0 (FR, DE)
      // - Ability Info 2     = 0x100
      // - Key Items          = 0x280
      // - Missions           = 0x280
      // - Quests             = 0x280
      // - Spell Names        = 0x08C
      // - Spell Descriptions = 0x100
      for (uint i = 0; i < EntryCount; ++i) {
      BinaryReader EntryBR = new BinaryReader(new MemoryStream(BR.ReadBytes(BytesPerEntry)));
	EntryBR.BaseStream.Position = 0;
      bool ItemAdded = false;
	{
	Things.DMSGStringBlock SB = new Things.DMSGStringBlock();
	  if (SB.Read(EntryBR, E, i)) {
	    TL.Add(SB);
	    ItemAdded = true;
	  }
	}
	EntryBR.Close();
	if (!ItemAdded) {
	  TL.Clear();
	  break;
	}
	if (ProgressCallback != null)
	  ProgressCallback(null, (double) (i + 1) / EntryCount);
      }
      return TL;
    }

  }

}
