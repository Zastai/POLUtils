// $Id$

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Xml;

using PlayOnline.Core;

namespace PlayOnline.FFXI {

  public abstract class Thing : IThing {

    #region Implemented IThing Members

    public string GetFieldName(string Field) {
    string MessageID = String.Format("F:{0}:{1}", this.GetType().Name, Field);
    string Name;
      try {
	Name = I18N.GetText(MessageID, this.GetType().Assembly);
      } catch {
	Name = Field;
      }
      return Name;
    }

    /// <summary>
    /// Helper field; if set, it will be used by GetIcon to find and return an icon image.
    /// </summary>
    protected string IconField_;

    public virtual Image GetIcon() {
      if (this.IconField_ == null)
	return null;
    object IconValue = this.GetFieldValue(this.IconField_);
      if (IconValue == null || IconValue is Image)
	return IconValue as Image;
      else if (IconValue is IThing)
	return ((IThing) IconValue).GetIcon();
      return null;
    }

    public virtual List<string> GetFields() {
      List<String> Fields = new List<string>();
      foreach (string Field in this.GetAllFields()) {
	if (this.HasField(Field))
	  Fields.Add(Field);
      }
      return Fields;
    }

    public virtual List<TabPage> GetPropertyPages() {
    List<TabPage> Pages = new List<TabPage>();
      Pages.AddRange(PropertyPages.Thing.GetPages(this));
      return Pages;
    }

    public virtual void Load(XmlElement Element) {
      this.Clear();
      if (Element == null)
	throw new ArgumentNullException();
      if (Element.Name != "thing" || !Element.HasAttribute("type") || Element.GetAttribute("type") != this.TypeName)
	throw new ArgumentException(String.Format(I18N.GetText("InvalidThingToLoad"), this.TypeName));
      if (Element.GetAttribute("type") != this.TypeName)
	throw new ArgumentException(String.Format(I18N.GetText("WrongThingToLoad"), this.TypeName, Element.GetAttribute("type")));
      foreach (string FieldName in this.GetAllFields()) {
	try {
	XmlElement FieldElement = Element.SelectSingleNode(String.Format("./child::field[@name = '{0}']", FieldName)) as XmlElement;
	  if (FieldElement != null)
	    this.LoadField(FieldName, FieldElement);
	} catch { }
      }
    }

    public virtual XmlElement Save(XmlDocument Document) {
    XmlElement E = Document.CreateElement("thing");
      {
      XmlAttribute A = Document.CreateAttribute("type");
	A.InnerText = this.TypeName;
	E.Attributes.Append(A);
      }
      foreach (string FieldName in this.GetFields()) {
      XmlElement F = Document.CreateElement("field");
	{
	XmlAttribute A = Document.CreateAttribute("name");
	  A.InnerText = FieldName;
	  F.Attributes.Append(A);
	}
	this.SaveField(FieldName, Document, F);
	E.AppendChild(F);
      }
      return E;
    }

    #endregion

    #region Load()/Save() Subroutines

    protected abstract void LoadField(string Field, XmlElement Element);

    protected Nullable<UInt64> LoadHexField(XmlElement Node) {
      try { return ulong.Parse(Node.InnerText, NumberStyles.HexNumber); } catch { return null; }
    }

    protected Image LoadImageField(XmlElement Node) {
      if (!Node.HasAttribute("format") || Node.GetAttribute("format") != "image/png")
	return null;
      if (!Node.HasAttribute("encoding") || Node.GetAttribute("encoding") != "base64")
	return null;
    byte[] ImageData = Convert.FromBase64String(Node.InnerText);
    MemoryStream MS = new MemoryStream(ImageData, false);
    Image Result = new Bitmap(MS);
      MS.Close();
      MS.Dispose();
      return Result;
    }

    protected Nullable<Int64> LoadSignedIntegerField(XmlElement Node) {
      try { return long.Parse(Node.InnerText, NumberStyles.Integer); } catch { return null; }
    }

    protected void LoadThingField(XmlElement Node, IThing T) {
    XmlElement ThingRoot = Node.SelectSingleNode(String.Format("./child::thing[@type = '{0}']", T.TypeName)) as XmlElement;
      if (ThingRoot != null)
	T.Load(ThingRoot);
      else
	throw new ArgumentException(String.Format(I18N.GetText("InvalidThingField"), T.TypeName));
    }

    protected string LoadTextField(XmlElement Node) {
      return Node.InnerText;
    }

    protected Nullable<UInt64> LoadUnsignedIntegerField(XmlElement Node) {
      try { return ulong.Parse(Node.InnerText, NumberStyles.Integer); } catch { return null; }
    }

    protected virtual void SaveField(string Field, XmlDocument Document, XmlElement Element) {
      // Default Implementation:
      // - IThing          -> Save()
      // - Image           -> save as PNG/base64
      // - Enum            -> save as hex number
      // - Everything Else -> GetFieldText()
    object Value = this.GetFieldValue(Field);
      if (Value != null) {
	if (Value is IThing)
	  Element.AppendChild(((IThing) Value).Save(Document));
	else if (Value is Image) {
	  {
	  XmlAttribute A = Document.CreateAttribute("format");
	    A.InnerText = "image/png";
	    Element.Attributes.Append(A);
	  }
	  {
	  XmlAttribute A = Document.CreateAttribute("encoding");
	    A.InnerText = "base64";
	    Element.Attributes.Append(A);
	  }
	MemoryStream MS = new MemoryStream();
	  ((Image) Value).Save(MS, ImageFormat.Png);
	  Element.InnerText = Convert.ToBase64String(MS.GetBuffer());
	  MS.Close();
	  MS.Dispose();
	}
	else if (Value is Enum) // Store enums as hex numbers
	  Element.InnerText = ((Enum) Value).ToString("X");
	else
	  Element.InnerText = Value.ToString();
      }
    }

    #endregion

    #region Abstract IThing Members

    public abstract string TypeName { get; }

    public abstract bool         HasField     (string Field);
    public abstract List<string> GetAllFields();
    public abstract string       GetFieldText (string Field);
    public abstract object       GetFieldValue(string Field);
    public abstract void         Clear();

    #endregion

    #region Thing Instantiation

    public static IThing Create(string TypeName) {
      // FIXME: It would be nicer if this was handled differently, i.e. without harcoding type names here.
      switch (TypeName) {
	case "FFXI Graphic": return new Graphic();
	case "FFXI Item":    return new Item();
	default:             return null;
      }
    }

    #endregion

  }

}
