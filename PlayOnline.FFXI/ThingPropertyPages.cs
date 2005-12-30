using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PlayOnline.FFXI {

  public partial class ThingPropertyPages: Form {

    public ThingPropertyPages(IThing T) {
      InitializeComponent();
      foreach (TabPage P in T.GetPropertyPages()) {
	if (P.Width > this.tabDummy.Width)
	  this.Width += (P.Width - this.tabDummy.Width);
	if (P.Height > this.tabDummy.Height)
	  this.Height += (P.Height - this.tabDummy.Height);
	this.tabPages.TabPages.Add(P);
      }
      this.tabPages.TabPages.Remove(this.tabDummy);
      this.tabDummy.Dispose();
      this.tabDummy = null;
    }

    private void btnClose_Click(object sender, EventArgs e) {
      this.Close();
    }

  }

}