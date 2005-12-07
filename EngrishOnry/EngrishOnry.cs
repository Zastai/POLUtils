// $Id$

using System;
using System.Windows.Forms;

using PlayOnline.Core;

namespace EngrishOnry {

  public class EngrishOnry {

    [STAThread]
    static void Main() {
      Application.EnableVisualStyles();
      if ((POL.AvailableRegions & POL.Region.Japan) == 0) {
	MessageBox.Show(null, I18N.GetText("NeedJPPOL"), I18N.GetText("ErrorCaption"), MessageBoxButtons.OK, MessageBoxIcon.Error);
	return;
      }
      POL.SelectedRegion = POL.Region.Japan;
      if (!POL.IsAppInstalled(AppID.FFXI)) {
	MessageBox.Show(null, I18N.GetText("NeedJPFFXI"), I18N.GetText("ErrorCaption"), MessageBoxButtons.OK, MessageBoxIcon.Error);
	return;
      }
      Application.Run(new MainWindow());
    }

  }

}