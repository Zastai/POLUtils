using System;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace PlayOnline.Core {

  public class I18N {

    public static string GetText(string Name) {
    ResourceManager Resources = new ResourceManager("Messages", Assembly.GetCallingAssembly());
    string ResourceString = Resources.GetObject(Name, CultureInfo.CurrentUICulture) as string;
      if (ResourceString == null)
	return Name;
      else
	return ResourceString;
    }

  }

}
