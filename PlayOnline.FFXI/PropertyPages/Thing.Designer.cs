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

  partial class Thing {

    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    private void InitializeComponent() {
      this.colFieldName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.colFieldValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.lblText = new System.Windows.Forms.Label();
      this.lstFields = new System.Windows.Forms.ListView();
      this.lblFields = new System.Windows.Forms.Label();
      this.lblTypeName = new System.Windows.Forms.Label();
      this.lblType = new System.Windows.Forms.Label();
      this.picIcon = new System.Windows.Forms.PictureBox();
      ((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
      this.SuspendLayout();
      // 
      // colFieldName
      // 
      this.colFieldName.Text = "Name";
      this.colFieldName.Width = 100;
      // 
      // colFieldValue
      // 
      this.colFieldValue.Text = "Value";
      this.colFieldValue.Width = 195;
      // 
      // lblText
      // 
      this.lblText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lblText.AutoEllipsis = true;
      this.lblText.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblText.Location = new System.Drawing.Point(41, 4);
      this.lblText.Name = "lblText";
      this.lblText.Size = new System.Drawing.Size(285, 13);
      this.lblText.TabIndex = 11;
      this.lblText.Text = "Thing Text";
      // 
      // lstFields
      // 
      this.lstFields.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lstFields.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colFieldName,
            this.colFieldValue});
      this.lstFields.FullRowSelect = true;
      this.lstFields.Location = new System.Drawing.Point(3, 60);
      this.lstFields.Name = "lstFields";
      this.lstFields.Size = new System.Drawing.Size(320, 319);
      this.lstFields.TabIndex = 10;
      this.lstFields.UseCompatibleStateImageBehavior = false;
      this.lstFields.View = System.Windows.Forms.View.Details;
      this.lstFields.ItemActivate += new System.EventHandler(this.lstFields_ItemActivate);
      // 
      // lblFields
      // 
      this.lblFields.AutoSize = true;
      this.lblFields.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblFields.Location = new System.Drawing.Point(0, 44);
      this.lblFields.Name = "lblFields";
      this.lblFields.Size = new System.Drawing.Size(37, 13);
      this.lblFields.TabIndex = 9;
      this.lblFields.Text = "Fields:";
      // 
      // lblTypeName
      // 
      this.lblTypeName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lblTypeName.AutoEllipsis = true;
      this.lblTypeName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblTypeName.Location = new System.Drawing.Point(81, 21);
      this.lblTypeName.Name = "lblTypeName";
      this.lblTypeName.Size = new System.Drawing.Size(242, 13);
      this.lblTypeName.TabIndex = 8;
      this.lblTypeName.Text = "Thing Type";
      // 
      // lblType
      // 
      this.lblType.AutoSize = true;
      this.lblType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblType.Location = new System.Drawing.Point(41, 21);
      this.lblType.Name = "lblType";
      this.lblType.Size = new System.Drawing.Size(34, 13);
      this.lblType.TabIndex = 7;
      this.lblType.Text = "Type:";
      // 
      // picIcon
      // 
      this.picIcon.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.picIcon.Location = new System.Drawing.Point(3, 3);
      this.picIcon.Name = "picIcon";
      this.picIcon.Size = new System.Drawing.Size(32, 32);
      this.picIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
      this.picIcon.TabIndex = 6;
      this.picIcon.TabStop = false;
      // 
      // Thing
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.lblText);
      this.Controls.Add(this.lstFields);
      this.Controls.Add(this.lblFields);
      this.Controls.Add(this.lblTypeName);
      this.Controls.Add(this.lblType);
      this.Controls.Add(this.picIcon);
      this.Name = "Thing";
      this.Size = new System.Drawing.Size(326, 382);
      this.TabName = "General";
      ((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label lblText;
    private System.Windows.Forms.ListView lstFields;
    private System.Windows.Forms.Label lblFields;
    private System.Windows.Forms.Label lblTypeName;
    private System.Windows.Forms.Label lblType;
    private System.Windows.Forms.PictureBox picIcon;
    private System.Windows.Forms.ColumnHeader colFieldName;
    private System.Windows.Forms.ColumnHeader colFieldValue;


  }

}
