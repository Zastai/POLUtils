using System;
using System.Windows.Forms;

using PlayOnline.Core;
using PlayOnline.FFXI;

namespace PlayOnline.Utils.FFXIDataBrowser {

  internal class XMLItemExporter : IItemExporter {

    public XMLItemExporter() {
    }

    public void DoExport(FFXIItem[] Items) {
      MessageBox.Show("XML export is not implemented yet.", "Sorry");
    }

  }

}
