using System;
using System.Windows.Forms;

using PlayOnline.Core;
using PlayOnline.FFXI;

namespace PlayOnline.Utils.FFXIDataBrowser {

  internal interface IItemExporter {

    void DoExport(FFXIItem[] Items);

  }

}
