using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using PlayOnline.Core;
using PlayOnline.FFXI;

namespace PlayOnline.Utils.FFXIDataBrowser {

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

    public override void DoExport(FFXIItem[] Items) {
      XMLItemExporter.dlgOptions.Reset();
      if (XMLItemExporter.dlgOptions.ShowDialog() == DialogResult.OK) {
      XmlDocument XD = new XmlDocument();
	XD.AppendChild(XD.CreateElement("ffxi-item-info"));
	{
	XmlElement XLanguage = XD.CreateElement("data-language");
	  XLanguage.InnerText = XMLItemExporter.dlgOptions.Language.ToString();
	  XD.DocumentElement.AppendChild(XLanguage);
	}
	{
	XmlElement XType = XD.CreateElement("data-type");
	  XType.InnerText = XMLItemExporter.dlgOptions.Type.ToString();
	  XD.DocumentElement.AppendChild(XType);
	}
	foreach (FFXIItem I in Items) {
	XmlElement XItem = XD.CreateElement("item");
	FFXIItem.IItemInfo II = I.GetInfo(XMLItemExporter.dlgOptions.Language, XMLItemExporter.dlgOptions.Type);
	  foreach (ItemField IF in II.GetFields()) {
	  XmlElement XField = XD.CreateElement("field");
	    {
	    XmlAttribute XFieldName = XD.CreateAttribute("name");
	      XFieldName.InnerText = IF.ToString();
	      XField.Attributes.Append(XFieldName);
	    }
	    XField.InnerText = II.GetFieldValue(IF).ToString();
	    XItem.AppendChild(XField);
	  }
	  XD.DocumentElement.AppendChild(XItem);
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

}
