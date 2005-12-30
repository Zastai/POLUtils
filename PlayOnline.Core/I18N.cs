// $Id$

using System;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace PlayOnline.Core {

  public class I18N {

    public static string GetText(string Name) {
      return I18N.GetText(Name, Assembly.GetCallingAssembly());
    }

    public static string GetText(string Name, Assembly A) {
      try {
      ResourceManager Resources = new ResourceManager("Messages", A);
      string ResourceString = Resources.GetObject(Name, CultureInfo.CurrentUICulture) as string;
	if (ResourceString == null)
	  ResourceString = Resources.GetObject(Name, CultureInfo.InvariantCulture) as string;
	if (ResourceString != null)
	  return ResourceString;
      } catch { }
      return Name;
    }

  }

}
