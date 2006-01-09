using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PlayOnline.FFXI {

  public partial class ThingPropertyPages: Form {

    private int DeltaW;
    private int DeltaH;

    public ThingPropertyPages(IThing T) {
      InitializeComponent();
      // Use the dummy page to get size deltas, then discard it
      this.DeltaW = this.Width  - this.tabDummy.Width;
      this.DeltaH = this.Height - this.tabDummy.Height;
      this.tabPages.TabPages.Clear();
      this.tabDummy.Dispose();
      this.tabDummy = null;
      // Add pages as needed
      foreach (PropertyPages.IThing P in T.GetPropertyPages()) {
	P.Left = 0;
	P.Top  = 0;
      TabPage TP = new ThemedTabPage(P.TabName);
	TP.UseVisualStyleBackColor = true;
	TP.Controls.Add(P);
	TP.Tag = P;
	this.tabPages.TabPages.Add(TP);
      }
      this.AdjustSize();
    }

    private void AdjustSize() {
      if (this.tabPages.SelectedTab == null)
	return;
      this.Width  = ((Control) this.tabPages.SelectedTab.Tag).Width  + this.DeltaW;
      this.Height = ((Control) this.tabPages.SelectedTab.Tag).Height + this.DeltaH;
    }

    private void btnClose_Click(object sender, EventArgs e) {
      this.Close();
    }

    private void tabPages_SelectedIndexChanged(object sender, EventArgs e) {
      this.AdjustSize();
    }

  }

}