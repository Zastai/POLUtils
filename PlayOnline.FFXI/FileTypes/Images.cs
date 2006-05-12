using System;
using System.IO;
using System.Collections.Generic;

using PlayOnline.Core;

namespace PlayOnline.FFXI.FileTypes {

  public class Images : FileType {

    public override ThingList Load(BinaryReader BR, ProgressCallback ProgressCallback) {
    ThingList TL = new ThingList();
      if (ProgressCallback != null)
	ProgressCallback(I18N.GetText("FTM:ScanningFile"), 0);
    Graphic G = new Graphic();
      while (BR.BaseStream.Position < BR.BaseStream.Length) {
      long Pos = BR.BaseStream.Position; // Save Position (G.Read() will advance it an unknown amount)
	if (G.Read(BR)) {
	  TL.Add(G);
	  G = new Graphic();
	  ProgressCallback(null, (double) (BR.BaseStream.Position + 1) / BR.BaseStream.Length);
	}
	else {
	  BR.BaseStream.Seek(Pos + 1, SeekOrigin.Begin);
	  if (BR.BaseStream.Position == BR.BaseStream.Length || (BR.BaseStream.Position % 1024) == 0)
	    ProgressCallback(null, (double) (BR.BaseStream.Position + 1) / BR.BaseStream.Length);
	}
      }
      G = null;
      return TL;
    }

  }

}
