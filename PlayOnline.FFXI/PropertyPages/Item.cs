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

    public Item(Things.Item I) {
      this.InitializeComponent();
      this.ieEditor.Item = I;
      this.Size = this.ieEditor.Size;
      ++this.Height;
      this.IsFixedSize = true;
    }

  }

}
