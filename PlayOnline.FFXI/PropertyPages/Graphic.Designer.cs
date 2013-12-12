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

  partial class Graphic {

    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    private void InitializeComponent() {
      this.btnSelectColor = new System.Windows.Forms.Button();
      this.lblBackColor = new System.Windows.Forms.Label();
      this.cmbViewMode = new System.Windows.Forms.ComboBox();
      this.lblViewMode = new System.Windows.Forms.Label();
      this.picImage = new System.Windows.Forms.PictureBox();
      this.dlgChooseColor = new System.Windows.Forms.ColorDialog();
      this.cmbBackColor = new System.Windows.Forms.ComboBox();
      this.btnSave = new System.Windows.Forms.Button();
      this.dlgSaveImage = new System.Windows.Forms.SaveFileDialog();
      ((System.ComponentModel.ISupportInitialize)(this.picImage)).BeginInit();
      this.SuspendLayout();
      // 
      // btnSelectColor
      // 
      this.btnSelectColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnSelectColor.Enabled = false;
      this.btnSelectColor.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.btnSelectColor.Location = new System.Drawing.Point(199, 356);
      this.btnSelectColor.Name = "btnSelectColor";
      this.btnSelectColor.Size = new System.Drawing.Size(59, 23);
      this.btnSelectColor.TabIndex = 5;
      this.btnSelectColor.Text = "Select...";
      this.btnSelectColor.UseVisualStyleBackColor = true;
      this.btnSelectColor.Click += new System.EventHandler(this.btnSelectColor_Click);
      // 
      // lblBackColor
      // 
      this.lblBackColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblBackColor.AutoSize = true;
      this.lblBackColor.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblBackColor.Location = new System.Drawing.Point(3, 361);
      this.lblBackColor.Name = "lblBackColor";
      this.lblBackColor.Size = new System.Drawing.Size(68, 13);
      this.lblBackColor.TabIndex = 3;
      this.lblBackColor.Text = "Background:";
      // 
      // cmbViewMode
      // 
      this.cmbViewMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.cmbViewMode.DisplayMember = "Name";
      this.cmbViewMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbViewMode.Location = new System.Drawing.Point(72, 329);
      this.cmbViewMode.Name = "cmbViewMode";
      this.cmbViewMode.Size = new System.Drawing.Size(251, 21);
      this.cmbViewMode.TabIndex = 2;
      this.cmbViewMode.ValueMember = "Value";
      this.cmbViewMode.SelectedIndexChanged += new System.EventHandler(this.cmbViewMode_SelectedIndexChanged);
      // 
      // lblViewMode
      // 
      this.lblViewMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.lblViewMode.AutoSize = true;
      this.lblViewMode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblViewMode.Location = new System.Drawing.Point(3, 332);
      this.lblViewMode.Name = "lblViewMode";
      this.lblViewMode.Size = new System.Drawing.Size(63, 13);
      this.lblViewMode.TabIndex = 1;
      this.lblViewMode.Text = "View Mode:";
      // 
      // picImage
      // 
      this.picImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.picImage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.picImage.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.picImage.Location = new System.Drawing.Point(3, 3);
      this.picImage.Name = "picImage";
      this.picImage.Size = new System.Drawing.Size(320, 320);
      this.picImage.TabIndex = 9;
      this.picImage.TabStop = false;
      // 
      // dlgChooseColor
      // 
      this.dlgChooseColor.AnyColor = true;
      this.dlgChooseColor.FullOpen = true;
      // 
      // cmbBackColor
      // 
      this.cmbBackColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.cmbBackColor.DisplayMember = "Name";
      this.cmbBackColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbBackColor.Items.AddRange(new object[] {
            "Transparent",
            "Solid Color"});
      this.cmbBackColor.Location = new System.Drawing.Point(72, 358);
      this.cmbBackColor.Name = "cmbBackColor";
      this.cmbBackColor.Size = new System.Drawing.Size(121, 21);
      this.cmbBackColor.TabIndex = 4;
      this.cmbBackColor.ValueMember = "Value";
      this.cmbBackColor.SelectedIndexChanged += new System.EventHandler(this.cmbBackColor_SelectedIndexChanged);
      // 
      // btnSave
      // 
      this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.btnSave.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.btnSave.Location = new System.Drawing.Point(264, 356);
      this.btnSave.Name = "btnSave";
      this.btnSave.Size = new System.Drawing.Size(59, 23);
      this.btnSave.TabIndex = 6;
      this.btnSave.Text = "&Save...";
      this.btnSave.UseVisualStyleBackColor = true;
      this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
      // 
      // dlgSaveImage
      // 
      this.dlgSaveImage.Filter = "Portable Network Graphic (*.png)|*.png";
      this.dlgSaveImage.Title = "Save Graphic As...";
      // 
      // Graphic
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.btnSave);
      this.Controls.Add(this.cmbBackColor);
      this.Controls.Add(this.btnSelectColor);
      this.Controls.Add(this.lblBackColor);
      this.Controls.Add(this.cmbViewMode);
      this.Controls.Add(this.lblViewMode);
      this.Controls.Add(this.picImage);
      this.Name = "Graphic";
      this.TabName = "Graphic";
      this.Size = new System.Drawing.Size(326, 382);
      ((System.ComponentModel.ISupportInitialize)(this.picImage)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnSelectColor;
    private System.Windows.Forms.Label lblBackColor;
    private System.Windows.Forms.ComboBox cmbViewMode;
    private System.Windows.Forms.Label lblViewMode;
    private System.Windows.Forms.PictureBox picImage;
    private System.Windows.Forms.ColorDialog dlgChooseColor;
    private System.Windows.Forms.ComboBox cmbBackColor;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.SaveFileDialog dlgSaveImage;


  }

}
