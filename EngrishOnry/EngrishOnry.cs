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
	MessageBox.Show(null, "This utility requires the JP version of the PlayOnline software to be installed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
	return;
      }
      POL.SelectedRegion = POL.Region.Japan;
      if (!POL.IsAppInstalled(AppID.FFXI)) {
	MessageBox.Show(null, "This utility requires the JP version of Final Fantasy XI to be installed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
	return;
      }
      Application.Run(new MainWindow());
    }

  }

}