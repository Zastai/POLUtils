using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace PlayOnline.FFXI.FileTypes {

  public class ItemData : FileType {

    public override ThingList Load(string FileName) {
    BinaryReader BR = null;
      try {
	BR = new BinaryReader(new FileStream(FileName, FileMode.Open, FileAccess.Read), Encoding.ASCII);
      } catch { }
      if (BR == null || BR.BaseStream == null)
	return null;
      if ((BR.BaseStream.Length % 0xC00) != 0) {
	BR.Close();
	return null;
      }
    ThingList TL = new ThingList();
      // First deduce the type of item data is in the file.
    Item.Language L;
    Item.Type T;
      Item.DeduceLanguageAndType(BR, out L, out T);
      // Now read the items
      while (BR.BaseStream.Position < BR.BaseStream.Length) {
      Item I = new Item();
	if (!I.Read(BR, L, T)) {
	  TL.Clear();
	  TL = null;
	  break;
	}
      }
      BR.Close();
      return TL;
    }

  }

}