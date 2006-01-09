using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace PlayOnline.FFXI.PropertyPages {

  public partial class IThing : UserControl {

    [Browsable(true), Category("Appearance"), DefaultValue("")]
    public string TabName {
      get {
	return this.TabName_;
      }
      set {
	this.TabName_ = value;
      }
    }
    private string TabName_ = String.Empty;

    public IThing() {
      InitializeComponent();
    }

  }

}
