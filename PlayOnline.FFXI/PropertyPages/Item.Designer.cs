// $Id$

// Copyright © 2004-2010 Tim Van Holder
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

namespace PlayOnline.FFXI.PropertyPages {

  partial class Item {

    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    private void InitializeComponent() {
      this.ieEditor = new PlayOnline.FFXI.ItemEditor();
      this.SuspendLayout();
      // 
      // ieEditor
      // 
      this.ieEditor.BackColor = System.Drawing.Color.Transparent;
      this.ieEditor.Item = null;
      this.ieEditor.Location = new System.Drawing.Point(0, 0);
      this.ieEditor.Name = "ieEditor";
      this.ieEditor.Size = new System.Drawing.Size(424, 260);
      this.ieEditor.TabIndex = 0;
      // 
      // Item
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.ieEditor);
      this.Name = "Item";
      this.TabName = "Item";
      this.IsFixedSize = true;
      this.Size = new System.Drawing.Size(424, 260);
      this.ResumeLayout(false);

    }

    #endregion

    private ItemEditor ieEditor;

  }

}
