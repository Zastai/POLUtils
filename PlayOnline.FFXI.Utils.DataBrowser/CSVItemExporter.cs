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
    char TextQuote = '"';
    long Index = 0;
    ItemField[] Fields = CSVItemExporter.dlgOptions.Fields;
      foreach (FFXIItem I in Items) {
      FFXIItem.IItemInfo II = I.GetInfo(CSVItemExporter.dlgOptions.Language, CSVItemExporter.dlgOptions.Type);
	if (Index == 0) {
	  CSVFile.Write("{0}{1}{0}", TextQuote, I18N.GetText("ColumnHeader:Index"));
	  foreach (ItemField IF in Fields) {
	    if (IF == ItemField.Description) {
	    string FieldName = new NamedEnum(IF).Name;
	      CSVFile.Write("{0}{1}{2}{1}", CultureInfo.CurrentCulture.TextInfo.ListSeparator, TextQuote, String.Format(I18N.GetText("ColumnHeader:LineCount"), FieldName));
	      for (byte i = 0; i < 6; ++i) // Max 6 lines - should be plenty
		CSVFile.Write("{0}{1}{2}{1}", CultureInfo.CurrentCulture.TextInfo.ListSeparator, TextQuote, String.Format(I18N.GetText("ColumnHeader:LineNumber"), FieldName, i + 1));
	    }
	    else
	      CSVFile.Write("{0}{1}{2}{1}", CultureInfo.CurrentCulture.TextInfo.ListSeparator, TextQuote, new NamedEnum(IF).Name);
	  }
	  CSVFile.WriteLine();
	  Application.DoEvents();
	}
	CSVFile.Write(++Index);
	foreach (ItemField IF in Fields) {
	  if (IF == ItemField.Description) {
	  string[] DescriptionLines = II.GetFieldText(IF).Replace(new string(TextQuote, 1), new string(TextQuote, 2)).Split('\n');
	    CSVFile.Write("{0}{1}{2}{1}", CultureInfo.CurrentCulture.TextInfo.ListSeparator, TextQuote, DescriptionLines.Length.ToString());
	    for (int i = 0; i < 6; ++i)
	      CSVFile.Write("{0}{1}{2}{1}", CultureInfo.CurrentCulture.TextInfo.ListSeparator, TextQuote, ((i < DescriptionLines.Length) ? DescriptionLines[i] : ""));
	  }
	  else
	    CSVFile.Write("{0}{1}{2}{1}", CultureInfo.CurrentCulture.TextInfo.ListSeparator, TextQuote, II.GetFieldText(IF).Replace(new string(TextQuote, 1), new string(TextQuote, 2)));
	}
	CSVFile.WriteLine();
	Application.DoEvents();
      }
      CSVFile.Close();
    }

  }

}
