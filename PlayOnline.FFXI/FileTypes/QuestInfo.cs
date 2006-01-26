using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

using PlayOnline.Core;

namespace PlayOnline.FFXI.FileTypes {

  public class QuestInfo : FileType {

    public override ThingList Load(BinaryReader BR, ProgressCallback ProgressCallback) {
    ThingList TL = new ThingList();
      ProgressCallback(I18N.GetText("FTM:CheckingFile"), 0);
      if (Encoding.ASCII.GetString(BR.ReadBytes(4)) != "menu")
	return TL;
      if (BR.ReadInt32() != 0x101)
	return TL;
      if (BR.ReadInt64() != 0x000)
	return TL;
      if (BR.ReadInt64() != 0)
	return TL;
      if (BR.ReadInt64() != 0)
	return TL;
    string MenuNameStart = Encoding.ASCII.GetString(BR.ReadBytes(4));
      BR.ReadUInt32(); // unknown
      if (BR.ReadInt64() != 0)
	return TL;
      ProgressCallback(I18N.GetText("FTM:LoadingData"), 0);
    uint MenuCount = 0;
    FFXIEncoding E = new FFXIEncoding();
      while (BR.BaseStream.Position < BR.BaseStream.Length) {
	ProgressCallback(null, ((double) (BR.BaseStream.Position + 1) / BR.BaseStream.Length));
      string Marker   = Encoding.ASCII.GetString(BR.ReadBytes(4));
      string Filler   = Encoding.ASCII.GetString(BR.ReadBytes(4));
      string MenuName = Encoding.ASCII.GetString(BR.ReadBytes(8));
	if (Marker.StartsWith("end"))
	  break;
	if (Marker != "menu" || Filler != "    ")
	  continue;
	if (MenuCount++ == 0 && MenuName.Substring(0, 4) != MenuNameStart)
	  continue;
	// TODO: verify the menu name; would not be future-proof tho.  Perhaps just check for _qs or _ms?
	if (BR.ReadInt32() != 0) {
	  BR.BaseStream.Seek(-4, SeekOrigin.Current);
	  continue;
	}
      int EntryCount   = BR.ReadInt32();
      long MenuStart   = BR.BaseStream.Position - 0x18;
      long NextScanPos = BR.BaseStream.Position + 20 * EntryCount;
	for (int i = 0; i < EntryCount; ++i) {
	  FFXI.QuestInfo QI = new FFXI.QuestInfo();
	  if (!QI.Read(BR, MenuName, MenuStart)) {
	    NextScanPos = MenuStart + 0x10;
	    break;
	  }
	  TL.Add(QI);
	}
	if ((NextScanPos % 16) != 0)
	  NextScanPos += 16 - (NextScanPos % 16);
	if (NextScanPos < BR.BaseStream.Length)
	  BR.BaseStream.Seek(NextScanPos, SeekOrigin.Begin);
	else
	  break;
      }
      return TL;
    }

  }

}
