using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

using PlayOnline.Core;
using PlayOnline.FFXI;

namespace PlayOnline.Utils.FFXIDataBrowser {

  internal class CSVItemExporter : IItemExporter {

    private static CSVOptionDialog dlgOptions = null;

    public CSVItemExporter() {
      if (CSVItemExporter.dlgOptions == null)
	CSVItemExporter.dlgOptions = new CSVOptionDialog();
    }

    public void DoExport(FFXIItem[] Items) {
      if (CSVItemExporter.dlgOptions.ShowDialog() == DialogResult.OK) {
      StreamWriter CSVFile = new StreamWriter(CSVItemExporter.dlgOptions.FileName, false, CSVItemExporter.dlgOptions.Encoding);
      char TextQuote = '"';
      long Index = 0;
	foreach (FFXIItem I in Items) {
	FFXIItem.IItemInfo II = I.GetInfo(CSVItemExporter.dlgOptions.Language, CSVItemExporter.dlgOptions.Type);
	  if (Index == 0) {
	    CSVFile.Write("{0}{1}{0}", TextQuote, I18N.GetText("ColumnHeader:Index"));
	    foreach (ItemField IF in II.GetFields()) {
	      if (IF == ItemField.Description) {
	      string FieldName = new NamedEnum(IF).Name;
		CSVFile.Write("{0}{1}{2}{1}", CultureInfo.CurrentCulture.TextInfo.ListSeparator, TextQuote, String.Format(I18N.GetText("ColumnHeader:DescriptionLineCount"), FieldName));
		for (byte i = 0; i < 6; ++i) // Max 6 lines - should be plenty
		  CSVFile.Write("{0}{1}{2}{1}", CultureInfo.CurrentCulture.TextInfo.ListSeparator, TextQuote, String.Format(I18N.GetText("ColumnHeader:DescriptionLine"), FieldName, i + 1));
	      }
	      else
		CSVFile.Write("{0}{1}{2}{1}", CultureInfo.CurrentCulture.TextInfo.ListSeparator, TextQuote, new NamedEnum(IF).Name);
	    }
	    CSVFile.WriteLine();
	  }
	  CSVFile.Write(++Index);
	  foreach (ItemField IF in II.GetFields()) {
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
	}
	CSVFile.Close();
      }
    }

  }

}
