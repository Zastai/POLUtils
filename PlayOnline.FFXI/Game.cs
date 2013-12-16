// $Id$

// Copyright © 2004-2010 Tim Van Holder
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using PlayOnline.Core;

namespace PlayOnline.FFXI {

  public static class Game {

    private static List<Character> Characters_;

    public static IEnumerable<Character> Characters {
      get {
        if (Game.Characters_ == null) {
          Game.Characters_ = new List<Character>();
          var appPath = POL.GetApplicationPath(AppID.FFXI);
          if (appPath != null) {
            var userDir = Path.Combine(appPath, "User");
            if (Directory.Exists(userDir)) {
              foreach (var subdir in Directory.GetDirectories(userDir).Where(subdir => File.Exists(Path.Combine(subdir, "ffxiusr.msg"))))
                Game.Characters_.Add(new Character(Path.GetFileName(subdir)));
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