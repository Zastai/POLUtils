using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace PlayOnline.Core {

  public class NamedEnum {

    private object EnumValue_;
    private string EnumValueName_;

    public NamedEnum(object EnumValue) {
      this.EnumValue_ = EnumValue;
      // TODO: Maybe use FullName?
    string MessageName = String.Format("E:{0}.{1}", EnumValue.GetType().Name, EnumValue);
      this.EnumValueName_ = I18N.GetText(MessageName, EnumValue.GetType().Assembly);
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

    public override string ToString() {
      return this.Name;
    }

    public static NamedEnum[] GetAll(Type T) {
      if (!T.IsEnum)
	return null;
    ArrayList Values = new ArrayList();
      foreach (FieldInfo FI in T.GetFields(BindingFlags.Public | BindingFlags.Static))
	Values.Add(new NamedEnum(FI.GetValue(null)));
      return (NamedEnum[]) Values.ToArray(typeof(NamedEnum));
    }

  }

}
