// $Id$

// Copyright © 2004-2010 Tim Van Holder
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System.Globalization;
using System.Reflection;
using System.Resources;

namespace PlayOnline.Core {

  public static class I18N {

    public static string GetText(string name) {
      return I18N.GetText(name, Assembly.GetCallingAssembly());
    }

    public static string GetText(string name, Assembly asm) {
      try {
      var resman = new ResourceManager(asm.GetName().Name + ".Messages", asm);
      var text = resman.GetString(name, CultureInfo.CurrentUICulture);
        if (text == null)
          text = resman.GetString(name, CultureInfo.InvariantCulture);
        if (text != null)
          return text;
      } catch { }
      return name;
    }

  }

}
