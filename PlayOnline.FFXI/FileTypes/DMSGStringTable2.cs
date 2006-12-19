// $Id$

using System;
using System.IO;
using System.Collections.Generic;

using PlayOnline.Core;

namespace PlayOnline.FFXI.FileTypes {

  public class DMSGStringTable2 : FileType {

    public override ThingList Load(BinaryReader BR, ProgressCallback ProgressCallback) {
    ThingList TL = new ThingList();
      if (ProgressCallback != null)
	ProgressCallback(I18N.GetText("FTM:CheckingFile"), 0);
      if (BR.BaseStream.Length < 0x40 || BR.BaseStream.Position != 0)
	return TL;
    FFXIEncoding E = new FFXIEncoding();
      // Skip (presumably) fixed portion of the header
      if ((E.GetString(BR.ReadBytes(8)) != "d_msg".PadRight(8, '\0')) || BR.ReadUInt16() != 1 || BR.ReadUInt16() != 1 || BR.ReadUInt32() != 3 || BR.ReadUInt32() != 3)
	return TL;
      // Read the useful header fields
    uint FileSize = BR.ReadUInt32();
      if (FileSize != BR.BaseStream.Length)
	return TL;
    uint HeaderBytes = BR.ReadUInt32();
      if (HeaderBytes != 0x40)
	return TL;
    uint EntryBytes = BR.ReadUInt32();
      if (BR.ReadUInt32() != 0)
	return TL;
    uint DataBytes  = BR.ReadUInt32();
      if (FileSize != HeaderBytes + EntryBytes + DataBytes)
	return TL;
    uint EntryCount = BR.ReadUInt32();
      if (EntryBytes != EntryCount * 8)
	return TL;
      if (BR.ReadUInt32() != 1 || BR.ReadUInt64() != 0 || BR.ReadUInt64() != 0)
	return TL;
      if (ProgressCallback != null)
	ProgressCallback(I18N.GetText("FTM:LoadingData"), 0);
      for (uint i = 0; i < EntryCount; ++i) {
      Things.DMSGStringTableEntry2 DSTE2 = new Things.DMSGStringTableEntry2();
	if (!DSTE2.Read(BR, E, i, HeaderBytes, EntryBytes, DataBytes)) {
	  TL.Clear();
	  break;
	}
	if (ProgressCallback != null)
	  ProgressCallback(null, (double) (i + 1) / EntryCount);
	TL.Add(DSTE2);
      }
      return TL;
    }

  }

}
