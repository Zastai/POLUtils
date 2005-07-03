// $Id$

using System.Windows.Forms;

namespace PlayOnline.FFXI.Utils.DataBrowser {

  internal enum ItemExportMethod {
    CSV,
    XML,
    UserSelect
  }

  internal class ItemExporter : IItemExporter {

    private ExportMethodDialog EMD_ = null;
    private IItemExporter      CSV_ = null;
    private IItemExporter      XML_ = null;

    public ItemExporter() {
    }

    public ItemExporter(ItemDataLanguage L, ItemDataType T) {
      this.CSV_ = new CSVItemExporter(L, T);
      this.XML_ = new XMLItemExporter(L, T);
    }

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

    public override void DoExport(FFXIItem[] Items) {
      this.DoExport(ItemExportMethod.UserSelect, Items);
    }

  }

}
