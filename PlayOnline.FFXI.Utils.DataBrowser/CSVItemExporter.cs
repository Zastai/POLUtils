// $Id$

using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

using PlayOnline.Core;

namespace PlayOnline.FFXI.Utils.DataBrowser {

  internal class CSVItemExporter : IItemExporter {

    private static CSVOptionDialog dlgOptions = null;

    public CSVItemExporter() {
      if (CSVItemExporter.dlgOptions == null)
	CSVItemExporter.dlgOptions = new CSVOptionDialog();
    }

    public CSVItemExporter(ItemDataLanguage L, ItemDataType T) : this() {
      CSVItemExporter.dlgOptions.Language = L;
      CSVItemExporter.dlgOptions.Type     = T;
    }

    public override bool PrepareExport() {
      CSVItemExporter.dlgOptions.Reset();
      return (CSVItemExporter.dlgOptions.ShowDialog() == DialogResult.OK);
    }

    public override void DoExport(FFXIItem[] Items) {
    StreamWriter CSVFile = new StreamWriter(CSVItemExporter.dlgOptions.FileName, false, CSVItemExporter.dlgOptions.Encoding);
    string TextQuote = "\"";
    string DoubleTextQuote = TextQuote + TextQuote;
    long Index = 0;
    ItemField[] Fields = CSVItemExporter.dlgOptions.Fields;
      foreach (FFXIItem I in Items) {
      FFXIItem.IItemInfo II = I.GetInfo(CSVItemExporter.dlgOptions.Language, CSVItemExporter.dlgOptions.Type);
	if (Index == 0) {
	  CSVFile.Write("{0}{1}{0}", TextQuote, I18N.GetText("ColumnHeader:Index"));
	  foreach (ItemField IF in Fields)
	    CSVFile.Write("{0}{1}{2}{1}", CultureInfo.CurrentCulture.TextInfo.ListSeparator, TextQuote, new NamedEnum(IF).Name);
	  CSVFile.Write("{0}{1}{2}{1}", CultureInfo.CurrentCulture.TextInfo.ListSeparator, TextQuote, I18N.GetText("ColumnHeader:IconInfo"));
	  CSVFile.WriteLine();
	  Application.DoEvents();
	}
	CSVFile.Write(++Index);
	foreach (ItemField IF in Fields)
	  CSVFile.Write("{0}{1}{2}{1}", CultureInfo.CurrentCulture.TextInfo.ListSeparator, TextQuote, II.GetFieldText(IF).Replace(TextQuote, DoubleTextQuote).Replace("\r", "").Replace("\n", CSVFile.NewLine));
	CSVFile.Write("{0}{1}{2}{1}", CultureInfo.CurrentCulture.TextInfo.ListSeparator, TextQuote, I.IconGraphic.ToString(), TextQuote);
	CSVFile.WriteLine();
	Application.DoEvents();
      }
      CSVFile.Close();
    }

  }

}
