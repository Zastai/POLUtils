// $Id$

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using PlayOnline.Core;

namespace PlayOnline.FFXI.Utils.DataBrowser {

  internal class XMLItemExporter : IItemExporter {

    private static XMLOptionDialog dlgOptions = null;

    public XMLItemExporter() {
      if (XMLItemExporter.dlgOptions == null)
	XMLItemExporter.dlgOptions = new XMLOptionDialog();
    }

    public XMLItemExporter(ItemDataLanguage L, ItemDataType T) : this() {
      XMLItemExporter.dlgOptions.Language = L;
      XMLItemExporter.dlgOptions.Type     = T;
    }

    public override bool PrepareExport() {
      XMLItemExporter.dlgOptions.Reset();
      return (XMLItemExporter.dlgOptions.ShowDialog() == DialogResult.OK);
    }

    public override void DoExport(FFXIItem[] Items) {
    XmlDocument XD = new XmlDocument();
    XmlElement XRoot = XD.AppendChild(XD.CreateElement("ItemList")) as XmlElement;
      XRoot.Attributes.Append(XD.CreateAttribute("Language")).InnerText = XMLItemExporter.dlgOptions.Language.ToString();
      XRoot.Attributes.Append(XD.CreateAttribute("Type")).InnerText = XMLItemExporter.dlgOptions.Type.ToString();
      foreach (FFXIItem I in Items) {
      XmlElement XItem = XRoot.AppendChild(XD.CreateElement("Item")) as XmlElement;
      FFXIItem.IItemInfo II = I.GetInfo(XMLItemExporter.dlgOptions.Language, XMLItemExporter.dlgOptions.Type);
	foreach (ItemField IF in II.GetFields()) {
	XmlElement XField = XItem.AppendChild(XD.CreateElement(IF.ToString())) as XmlElement;
	  {
	  object FieldValue = II.GetFieldValue(IF);
	    if (FieldValue is Enum) // Store enums as hex numbers
	      XField.InnerText = ((Enum) FieldValue).ToString("X");
	    else
	      XField.InnerText = FieldValue.ToString();
	  }
	}
	{ // Add the icon (raw bitmap data, in base64)
	XmlElement XIcon = XItem.AppendChild(XD.CreateElement("Icon")) as XmlElement;
	  XIcon.AppendChild(I.IconGraphic.Save(XD));
	}
	Application.DoEvents();
      }
    XmlTextWriter XW = new XmlTextWriter(XMLItemExporter.dlgOptions.FileName, Encoding.UTF8);
      XW.Formatting = Formatting.Indented;
      XW.IndentChar = ' ';
      XW.Indentation = 2;
      XD.WriteContentTo(XW);
      XW.Close();
    }

  }

}
