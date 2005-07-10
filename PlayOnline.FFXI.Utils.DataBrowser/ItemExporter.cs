// $Id$

using System.Windows.Forms;

namespace PlayOnline.FFXI.Utils.DataBrowser {

  internal enum ItemExportMethod {
    CSV,
    XML,
    UserSelect
  }

  internal class ItemExporter : IItemExporter {

    private ItemExportMethod Method_ = ItemExportMethod.UserSelect;
    private IItemExporter    CSV_ = null;
    private IItemExporter    XML_ = null;

    public ItemExporter() {
    }

    public ItemExporter(ItemDataLanguage L, ItemDataType T) {
      this.CSV_ = new CSVItemExporter(L, T);
      this.XML_ = new XMLItemExporter(L, T);
    }

    public override bool PrepareExport() {
      return this.PrepareExport(ItemExportMethod.UserSelect);
    }

    public bool PrepareExport(ItemExportMethod Method) {
      if (Method == ItemExportMethod.UserSelect) {
      ExportMethodDialog EMD = new ExportMethodDialog();
	if (EMD.ShowDialog() != DialogResult.OK)
	  return false;
	this.Method_ = EMD.SelectedMethod;
      }
      switch (this.Method_) {
	case ItemExportMethod.CSV:
	  if (this.CSV_ == null)
	    this.CSV_ = new CSVItemExporter();
	  return this.CSV_.PrepareExport();
	case ItemExportMethod.XML:
	  if (this.XML_ == null)
	    this.XML_ = new XMLItemExporter();
	  return this.XML_.PrepareExport();
      }
      return false;
    }

    public override void DoExport(FFXIItem[] Items) {
      switch (this.Method_) {
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

  }

}
