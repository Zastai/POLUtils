// $Id$

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace PlayOnline.Core {

  internal partial class ChooseRegionDialog : Form {

    public ChooseRegionDialog() {
      this.InitializeComponent();
      this.radJapan.Checked        = (POL.SelectedRegion == POL.Region.Japan);
      this.radJapan.Enabled        = ((POL.AvailableRegions & POL.Region.Japan)        != 0);
      this.radNorthAmerica.Checked = (POL.SelectedRegion == POL.Region.NorthAmerica);
      this.radNorthAmerica.Enabled = ((POL.AvailableRegions & POL.Region.NorthAmerica) != 0);
      this.radEurope.Checked       = (POL.SelectedRegion == POL.Region.Europe);
      this.radEurope.Enabled       = ((POL.AvailableRegions & POL.Region.Europe)       != 0);
    }

    #region Events

    private void btnOK_Click(object sender, System.EventArgs e) {
      if (this.radJapan.Checked)        POL.SelectedRegion = POL.Region.Japan;
      if (this.radNorthAmerica.Checked) POL.SelectedRegion = POL.Region.NorthAmerica;
      if (this.radEurope.Checked)       POL.SelectedRegion = POL.Region.Europe;
    }

    #endregion

  }

}
