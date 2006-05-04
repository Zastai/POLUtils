using System;
using System.IO;
using System.Collections.Generic;

using PlayOnline.Core;

namespace PlayOnline.FFXI.FileTypes {

  public class MobList : FileType {

    public override ThingList Load(BinaryReader BR, ProgressCallback ProgressCallback) {
    ThingList TL = new ThingList();
      ProgressCallback(I18N.GetText("FTM:CheckingFile"), 0);
      if ((BR.BaseStream.Length % 0x1C) != 0 || BR.BaseStream.Position != 0)
	return TL;
    long EntryCount = BR.BaseStream.Length / 0x1C;
      ProgressCallback(I18N.GetText("FTM:LoadingData"), 0);
      try {
#if DEBUG
      uint LastID = 0;
#endif
	for (int i = 0; i < EntryCount; ++i) {
	FFXI.MobListEntry MLE = new FFXI.MobListEntry();
	  if (!MLE.Read(BR)) {
	    TL.Clear();
	    break;
	  }
	uint ThisID = (uint) MLE.GetFieldValue("id");
	  if (i == 0 && (ThisID != 0 || MLE.GetFieldText("name") != "none")) {
	    TL.Clear();
	    break;
	  }
#if DEBUG
	  if (ThisID <= LastID)
	    Console.WriteLine("Mob List Entry #{0}: ID {1:X8} <= Previous ID {2:X8}", i, ThisID, LastID);
	  LastID = ThisID;
#endif
	  ProgressCallback(null, (double) (i + 1) / EntryCount);
	  TL.Add(MLE);
	}
      } catch {
	TL.Clear();
      }
      return TL;
    }

  }

}
