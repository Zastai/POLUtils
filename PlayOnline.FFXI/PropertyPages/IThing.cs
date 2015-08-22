// $Id$

// Copyright © 2004-2010 Tim Van Holder
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace PlayOnline.FFXI.PropertyPages {

  public partial class IThing : UserControl {

    public IThing() {
      this.TabName = String.Empty;
      this.IsFixedSize = false;
      this.InitializeComponent();
    }

    [Browsable(true), Category("Appearance"), DefaultValue("")]
    public string TabName { get; set; }

    [Browsable(true), Category("Appearance"), DefaultValue(false)]
    public bool IsFixedSize { get; set; }

  }

}
