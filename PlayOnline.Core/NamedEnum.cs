using System;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace PlayOnline.Core {

  public class NamedEnum {

    private object EnumValue_;
    private string EnumValueName_;

    public NamedEnum(object EnumValue) {
      this.EnumValue_ = EnumValue;
    string MessageName = String.Format("E:{0}.{1}", EnumValue.GetType().Name, EnumValue);
      this.EnumValueName_ = I18N.GetText(MessageName);
      if (this.EnumValueName_ == MessageName)
	this.EnumValueName_ = EnumValue.ToString();
    }

    public string Name {
      get {
	return this.EnumValueName_;
      }
    }

    public object Value {
      get {
	return this.EnumValue_;
      }
    }

  }

}
