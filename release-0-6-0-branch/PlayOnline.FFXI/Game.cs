// $Id$

using System;
using System.IO;

using PlayOnline.Core;

namespace PlayOnline.FFXI {

  public class Game {

    private static CharacterCollection Characters_;

    public static CharacterCollection Characters {
      get {
	if (Game.Characters_ == null) {
	  Game.Characters_ = new CharacterCollection();
	string AppPath = POL.GetApplicationPath(AppID.FFXI);
	  if (AppPath != null) {
	    foreach (string SubDir in Directory.GetDirectories(Path.Combine(AppPath, "User"))) {
	      if (File.Exists(Path.Combine(SubDir, "ffxiusr.msg")))
		Game.Characters_.Add(new Character(Path.GetFileName(SubDir)));
	    }
	  }
	}
	return Game.Characters_;
      }
    }

    public static void Clear() {
      Game.Characters_ = null; // Forces reload
    }

  }

}