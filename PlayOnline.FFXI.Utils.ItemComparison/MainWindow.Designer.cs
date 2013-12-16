// $Id$

// Copyright © 2004-2010 Tim Van Holder
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

namespace PlayOnline.FFXI.Utils.ItemComparison {

  public partial class MainWindow {

    #region Controls

    private PlayOnline.FFXI.ItemEditor ieLeft;
    private PlayOnline.FFXI.ItemEditor ieRight;
    private System.Windows.Forms.Button btnLoadItemSet1;
    private System.Windows.Forms.Button btnLoadItemSet2;
    private System.Windows.Forms.Button btnPrevious;
    private System.Windows.Forms.Button btnNext;
    private System.Windows.Forms.Button btnRemoveUnchanged;

    private System.ComponentModel.Container components = null;

    #endregion

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      this.ieLeft = new PlayOnline.FFXI.ItemEditor();
      this.ieRight = new PlayOnline.FFXI.ItemEditor();
      this.btnLoadItemSet1 = new System.Windows.Forms.Button();
      this.btnLoadItemSet2 = new System.Windows.Forms.Button();
      this.btnPrevious = new System.Windows.Forms.Button();
      this.btnNext = new System.Windows.Forms.Button();
      this.btnRemoveUnchanged = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // ieLeft
      // 
      this.ieLeft.BackColor = System.Drawing.Color.Transparent;
      this.ieLeft.Item = null;
      this.ieLeft.Location = new System.Drawing.Point(4, 32);
      this.ieLeft.Name = "ieLeft";
      this.ieLeft.Size = new System.Drawing.Size(424, 260);
      this.ieLeft.TabIndex = 0;
      this.ieLeft.SizeChanged += new System.EventHandler(this.ItemViewerSizeChanged);
      // 
      // ieRight
      // 
      this.ieRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.ieRight.BackColor = System.Drawing.Color.Transparent;
      this.ieRight.Item = null;
      this.ieRight.Location = new System.Drawing.Point(436, 32);
      this.ieRight.Name = "ieRight";
      this.ieRight.Size = new System.Drawing.Size(424, 260);
      this.ieRight.TabIndex = 0;
      this.ieRight.SizeChanged += new System.EventHandler(this.ItemViewerSizeChanged);
      // 
      // btnLoadItemSet1
      // 
      this.btnLoadItemSet1.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnLoadItemSet1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.btnLoadItemSet1.Location = new System.Drawing.Point(4, 4);
      this.btnLoadItemSet1.Name = "btnLoadItemSet1";
      this.btnLoadItemSet1.Size = new System.Drawing.Size(104, 24);
      this.btnLoadItemSet1.TabIndex = 1;
      this.btnLoadItemSet1.Text = "Load Item Set &1...";
      this.btnLoadItemSet1.Click += new System.EventHandler(this.btnLoadItemSet1_Click);
      // 
      // btnLoadItemSet2
      // 
      this.btnLoadItemSet2.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnLoadItemSet2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.btnLoadItemSet2.Location = new System.Drawing.Point(756, 4);
      this.btnLoadItemSet2.Name = "btnLoadItemSet2";
      this.btnLoadItemSet2.Size = new System.Drawing.Size(104, 24);
      this.btnLoadItemSet2.TabIndex = 4;
      this.btnLoadItemSet2.Text = "Load Item Set &2...";
      this.btnLoadItemSet2.Click += new System.EventHandler(this.btnLoadItemSet2_Click);
      // 
      // btnPrevious
      // 
      this.btnPrevious.Enabled = false;
      this.btnPrevious.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnPrevious.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.btnPrevious.Location = new System.Drawing.Point(301, 4);
      this.btnPrevious.Name = "btnPrevious";
      this.btnPrevious.Size = new System.Drawing.Size(60, 24);
      this.btnPrevious.TabIndex = 2;
      this.btnPrevious.Text = "< &Previous";
      this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
      // 
      // btnNext
      // 
      this.btnNext.Enabled = false;
      this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnNext.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.btnNext.Location = new System.Drawing.Point(505, 4);
      this.btnNext.Name = "btnNext";
      this.btnNext.Size = new System.Drawing.Size(60, 24);
      this.btnNext.TabIndex = 3;
      this.btnNext.Text = "&Next >";
      this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
      // 
      // btnRemoveUnchanged
      // 
      this.btnRemoveUnchanged.Enabled = false;
      this.btnRemoveUnchanged.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnRemoveUnchanged.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.btnRemoveUnchanged.Location = new System.Drawing.Point(365, 4);
      this.btnRemoveUnchanged.Name = "btnRemoveUnchanged";
      this.btnRemoveUnchanged.Size = new System.Drawing.Size(136, 24);
      this.btnRemoveUnchanged.TabIndex = 5;
      this.btnRemoveUnchanged.Text = "&Remove Unchanged Items";
      this.btnRemoveUnchanged.Click += new System.EventHandler(this.btnRemoveUnchanged_Click);
      // 
      // MainWindow
      // 
      this.ClientSize = new System.Drawing.Size(866, 31);
      this.Controls.Add(this.btnRemoveUnchanged);
      this.Controls.Add(this.btnNext);
      this.Controls.Add(this.btnPrevious);
      this.Controls.Add(this.btnLoadItemSet2);
      this.Controls.Add(this.btnLoadItemSet1);
      this.Controls.Add(this.ieLeft);
      this.Controls.Add(this.ieRight);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.Name = "MainWindow";
      this.Text = "FFXI Item Data Comparison";
      this.ResumeLayout(false);

    }

    #endregion

  }

}
