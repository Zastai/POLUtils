// $Id$

// Copyright © 2004-2010 Tim Van Holder
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

namespace PlayOnline.FFXI.Utils.StrangeApparatus {

  partial class MainWindow {

    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent() {
      this.btnGenerateCodes = new System.Windows.Forms.Button();
      this.txtCharacterName = new System.Windows.Forms.TextBox();
      this.lblCharacterName = new System.Windows.Forms.Label();
      this.lvCodes = new System.Windows.Forms.ListView();
      this.colArea = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.colElement = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.colChipColor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.colCode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.SuspendLayout();
      // 
      // btnGenerateCodes
      // 
      this.btnGenerateCodes.Location = new System.Drawing.Point(254, 6);
      this.btnGenerateCodes.Name = "btnGenerateCodes";
      this.btnGenerateCodes.Size = new System.Drawing.Size(103, 23);
      this.btnGenerateCodes.TabIndex = 0;
      this.btnGenerateCodes.Text = "&Generate Codes";
      this.btnGenerateCodes.UseVisualStyleBackColor = true;
      this.btnGenerateCodes.Click += new System.EventHandler(this.btnGenerateCodes_Click);
      // 
      // txtCharacterName
      // 
      this.txtCharacterName.Location = new System.Drawing.Point(105, 6);
      this.txtCharacterName.Name = "txtCharacterName";
      this.txtCharacterName.Size = new System.Drawing.Size(143, 20);
      this.txtCharacterName.TabIndex = 1;
      // 
      // lblCharacterName
      // 
      this.lblCharacterName.AutoSize = true;
      this.lblCharacterName.Location = new System.Drawing.Point(12, 9);
      this.lblCharacterName.Name = "lblCharacterName";
      this.lblCharacterName.Size = new System.Drawing.Size(87, 13);
      this.lblCharacterName.TabIndex = 2;
      this.lblCharacterName.Text = "Character Name:";
      // 
      // lvCodes
      // 
      this.lvCodes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colArea,
            this.colElement,
            this.colChipColor,
            this.colCode});
      this.lvCodes.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
      this.lvCodes.Location = new System.Drawing.Point(12, 32);
      this.lvCodes.Name = "lvCodes";
      this.lvCodes.Size = new System.Drawing.Size(345, 143);
      this.lvCodes.TabIndex = 3;
      this.lvCodes.UseCompatibleStateImageBehavior = false;
      this.lvCodes.View = System.Windows.Forms.View.Details;
      // 
      // colArea
      // 
      this.colArea.Text = "Area";
      this.colArea.Width = 150;
      // 
      // colElement
      // 
      this.colElement.Text = "Element";
      // 
      // colChipColor
      // 
      this.colChipColor.Text = "Chip Color";
      this.colChipColor.Width = 70;
      // 
      // colCode
      // 
      this.colCode.Text = "Code";
      this.colCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      // 
      // MainWindow
      // 
      this.AcceptButton = this.btnGenerateCodes;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(369, 187);
      this.Controls.Add(this.lvCodes);
      this.Controls.Add(this.lblCharacterName);
      this.Controls.Add(this.txtCharacterName);
      this.Controls.Add(this.btnGenerateCodes);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.Name = "MainWindow";
      this.ShowIcon = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Strange Apparatus Code Generator";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button btnGenerateCodes;
    private System.Windows.Forms.TextBox txtCharacterName;
    private System.Windows.Forms.Label lblCharacterName;
    private System.Windows.Forms.ListView lvCodes;
    private System.Windows.Forms.ColumnHeader colArea;
    private System.Windows.Forms.ColumnHeader colElement;
    private System.Windows.Forms.ColumnHeader colChipColor;
    private System.Windows.Forms.ColumnHeader colCode;

  }

}