using System;
using System.Windows.Forms;

using PlayOnline.Core;
using PlayOnline.FFXI;

namespace PlayOnline.Utils.FFXIDataBrowser {

  internal enum ItemExportMethod {
    CSV,
    XML,
    UserSelect
  }

  internal class ItemExporter : IItemExporter {

    public ItemExporter() {
    }

    private ExportMethodDialog EMD_ = null;
    private IItemExporter      CSV_ = null;
    private IItemExporter      XML_ = null;

    public void DoExport(ItemExportMethod Method, FFXIItem[] Items) {
      if (Method == ItemExportMethod.UserSelect) {
	if (this.EMD_ == null)
	  this.EMD_ = new ExportMethodDialog();
	if (this.EMD_.ShowDialog() == DialogResult.OK)
	  Method = this.EMD_.SelectedMethod;
      }
      switch (Method) {
	case ItemExportMethod.CSV:
	  if (this.CSV_ == null)
	    this.CSV_ = new CSVItemExporter();
	  this.CSV_.DoExport(Items);
	  break;
	case ItemExportMethod.XML:
	  if (this.XML_ == null)
	    this.XML_ = new XMLItemExporter();
	  this.XML_.DoExport(Items);
	  break;
      }
    }

    public void DoExport(FFXIItem[] Items) {
      this.DoExport(ItemExportMethod.UserSelect, Items);
    }

  }

}
