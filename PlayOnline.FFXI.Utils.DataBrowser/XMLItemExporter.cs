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
	  if (XMLItemExporter.dlgOptions.IncludeIconData) { // Add the icon (raw bitmap data, in base64)
	  XmlElement XIcon = XD.CreateElement("icon");
	    {
	    XmlAttribute XHeight = XD.CreateAttribute("height");
	      XHeight.InnerText = I.IconGraphic.Height.ToString();
	      XIcon.Attributes.Append(XHeight);
	    }
	    {
	    XmlAttribute XWidth = XD.CreateAttribute("width");
	      XWidth.InnerText = I.IconGraphic.Width.ToString();
	      XIcon.Attributes.Append(XWidth);
	    }
	    {
	    XmlAttribute XContentType = XD.CreateAttribute("content-type");
	      XContentType.InnerText = "image/png+base64";
	      XIcon.Attributes.Append(XContentType);
	    }
	    {
	    MemoryStream MS = new MemoryStream();
	      I.IconGraphic.Bitmap.Save(MS, ImageFormat.Png);
	      XIcon.InnerText = Convert.ToBase64String(MS.GetBuffer());
	      MS.Close();
	    }
	    XItem.AppendChild(XIcon);
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
