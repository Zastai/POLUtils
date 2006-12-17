using System;
using System.IO;
using System.Collections.Generic;

using PlayOnline.Core;
using PlayOnline.FFXI.Things;

namespace PlayOnline.FFXI.FileTypes {

  public class ItemData : FileType {

    public override ThingList Load(BinaryReader BR, ProgressCallback ProgressCallback) {
    ThingList TL = new ThingList();
      if (ProgressCallback != null)
	ProgressCallback(I18N.GetText("FTM:CheckingFile"), 0);
      if ((BR.BaseStream.Length % 0xC00) != 0 || BR.BaseStream.Position != 0)
	return TL;
      // First deduce the type of item data is in the file.
    Item.Language L;
    Item.Type T;
      Item.DeduceLanguageAndType(BR, out L, out T);
      // Now read the items
      if (ProgressCallback != null)
	ProgressCallback(I18N.GetText("FTM:LoadingData"), 0);
    long ItemCount = BR.BaseStream.Length / 0xC00;
    long CurrentItem = 0;
      while (BR.BaseStream.Position < BR.BaseStream.Length) {
      Item I = new Item();
	if (!I.Read(BR, L, T)) {
	  TL.Clear();
	  break;
	}
	if (ProgressCallback != null)
	  ProgressCallback(null, (double) ++CurrentItem / ItemCount);
	TL.Add(I);
      }
      return TL;
    }

  }

}
