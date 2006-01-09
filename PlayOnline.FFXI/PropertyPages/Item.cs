// $Id$

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace PlayOnline.FFXI.PropertyPages {

  public partial class Item : IThing {

    public Item(FFXI.Item I) {
      this.InitializeComponent();
    }

  }

}
