using System;
using System.IO;
using System.Collections.Generic;

using PlayOnline.Core;

namespace PlayOnline.FFXI.FileTypes {

  public class SpellInfo : FileType {

    public override ThingList Load(BinaryReader BR, ProgressCallback ProgressCallback) {
    ThingList TL = new ThingList();
      if (ProgressCallback != null)
	ProgressCallback(I18N.GetText("FTM:CheckingFile"), 0);
      if ((BR.BaseStream.Length % 0x400) != 0 || BR.BaseStream.Position != 0)
	return TL;
    long EntryCount = BR.BaseStream.Length / 0x400;
      if (ProgressCallback != null)
	ProgressCallback(I18N.GetText("FTM:LoadingData"), 0);
      for (int i = 0; i < EntryCount; ++i) {
      Things.SpellInfo SI = new Things.SpellInfo();
	if (!SI.Read(BR)) {
	  TL.Clear();
	  break;
	}
	if (ProgressCallback != null)
	  ProgressCallback(null, (double) (i + 1) / EntryCount);
	TL.Add(SI);
      }
      return TL;
    }

  }

}
