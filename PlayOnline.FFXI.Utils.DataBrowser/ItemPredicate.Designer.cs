// $Id$

// Copyright © 2004-2010 Tim Van Holder
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

namespace PlayOnline.FFXI.Utils.DataBrowser {

  internal partial class ItemPredicate {

    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing) {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    private void InitializeComponent() {
      this.cmbTest = new System.Windows.Forms.ComboBox();
      this.txtTestParameter = new System.Windows.Forms.TextBox();
      this.cmbField = new System.Windows.Forms.ComboBox();
      this.SuspendLayout();
      // 
      // cmbTest
      // 
      this.cmbTest.DisplayMember = "Name";
      this.cmbTest.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbTest.FormattingEnabled = true;
      this.cmbTest.ItemHeight = 13;
      this.cmbTest.Location = new System.Drawing.Point(228, 0);
      this.cmbTest.Name = "cmbTest";
      this.cmbTest.Size = new System.Drawing.Size(136, 21);
      this.cmbTest.TabIndex = 2;
      this.cmbTest.ValueMember = "Value";
      // 
      // txtTestParameter
      // 
      this.txtTestParameter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtTestParameter.Location = new System.Drawing.Point(370, 0);
      this.txtTestParameter.Name = "txtTestParameter";
      this.txtTestParameter.Size = new System.Drawing.Size(339, 20);
      this.txtTestParameter.TabIndex = 3;
      // 
      // cmbField
      // 
      this.cmbField.DisplayMember = "Name";
      this.cmbField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbField.FormattingEnabled = true;
      this.cmbField.ItemHeight = 13;
      this.cmbField.Location = new System.Drawing.Point(3, 0);
      this.cmbField.Name = "cmbField";
      this.cmbField.Size = new System.Drawing.Size(219, 21);
      this.cmbField.TabIndex = 1;
      this.cmbField.ValueMember = "Field";
      // 
      // ItemPredicate
      // 
      this.BackColor = System.Drawing.Color.Transparent;
      this.Controls.Add(this.cmbField);
      this.Controls.Add(this.cmbTest);
      this.Controls.Add(this.txtTestParameter);
      this.Name = "ItemPredicate";
      this.Size = new System.Drawing.Size(712, 25);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ComboBox cmbField;
    private System.Windows.Forms.ComboBox cmbTest;
    private System.Windows.Forms.TextBox txtTestParameter;

  }

}
