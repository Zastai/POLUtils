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
      foreach (TabPage P in T.GetPropertyPages())
	this.tabPages.TabPages.Add(P);
    }

    private void btnClose_Click(object sender, EventArgs e) {
      this.Close();
    }

  }

}