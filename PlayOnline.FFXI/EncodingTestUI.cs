// $Id$

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PlayOnline.FFXI {

  public class EncodingTestUI : System.Windows.Forms.Form {

    #region Controls

    private System.Windows.Forms.Label lblFFXICode;
    private System.Windows.Forms.Label lblArrow;
    private System.Windows.Forms.Label lblUniCode;
    private System.Windows.Forms.ComboBox cmbSecondByte;
    private System.Windows.Forms.ComboBox cmbLeadByte;
    private System.Windows.Forms.Label lblCharacter;

    private System.ComponentModel.Container components = null;

    #endregion

    private FFXIEncoding E = new FFXIEncoding();

    private class HexByte {
      public        HexByte(byte B) { this.Value = B; }
      public byte   Value;
      public string HexValue        { get { return String.Format("{0:X2}", this.Value); } }
      public string TextValue       { get { if (this.Value == 0) return "--"; return this.HexValue; } }
    }

    public EncodingTestUI() {
      this.InitializeComponent();
      this.cmbLeadByte.Items.Add(new HexByte(0));
      // get lead bytes
    BinaryReader BR = this.E.GetConversionTable(0x00);
      if (BR != null) {
	BR.BaseStream.Seek(0, SeekOrigin.Begin);
	for (short i = 0; i < 0x100; ++i) {
	  if (BR.ReadUInt16() == 0xFFFE)
	    this.cmbLeadByte.Items.Add(new HexByte((byte) i));
  	}
      }
      // Start with single-byte characters
      this.cmbLeadByte.SelectedIndex = 0;
    }

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if(disposing && components != null)
	components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      this.lblFFXICode = new System.Windows.Forms.Label();
      this.lblArrow = new System.Windows.Forms.Label();
      this.lblCharacter = new System.Windows.Forms.Label();
      this.lblUniCode = new System.Windows.Forms.Label();
      this.cmbSecondByte = new System.Windows.Forms.ComboBox();
      this.cmbLeadByte = new System.Windows.Forms.ComboBox();
      this.SuspendLayout();
      // 
      // lblFFXICode
      // 
      this.lblFFXICode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblFFXICode.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblFFXICode.Location = new System.Drawing.Point(30, 172);
      this.lblFFXICode.Name = "lblFFXICode";
      this.lblFFXICode.Size = new System.Drawing.Size(40, 16);
      this.lblFFXICode.TabIndex = 2;
      this.lblFFXICode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // lblArrow
      // 
      this.lblArrow.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblArrow.Location = new System.Drawing.Point(74, 172);
      this.lblArrow.Name = "lblArrow";
      this.lblArrow.Size = new System.Drawing.Size(12, 16);
      this.lblArrow.TabIndex = 3;
      this.lblArrow.Text = "→";
      this.lblArrow.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      // 
      // lblCharacter
      // 
      this.lblCharacter.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
      this.lblCharacter.Location = new System.Drawing.Point(6, 36);
      this.lblCharacter.Name = "lblCharacter";
      this.lblCharacter.Size = new System.Drawing.Size(148, 136);
      this.lblCharacter.TabIndex = 4;
      this.lblCharacter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // lblUniCode
      // 
      this.lblUniCode.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.lblUniCode.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblUniCode.Location = new System.Drawing.Point(90, 172);
      this.lblUniCode.Name = "lblUniCode";
      this.lblUniCode.Size = new System.Drawing.Size(40, 16);
      this.lblUniCode.TabIndex = 5;
      this.lblUniCode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // cmbSecondByte
      // 
      this.cmbSecondByte.DisplayMember = "HexValue";
      this.cmbSecondByte.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbSecondByte.Location = new System.Drawing.Point(84, 8);
      this.cmbSecondByte.Name = "cmbSecondByte";
      this.cmbSecondByte.Size = new System.Drawing.Size(48, 21);
      this.cmbSecondByte.TabIndex = 7;
      this.cmbSecondByte.SelectedIndexChanged += new System.EventHandler(this.cmbSecondByte_SelectedIndexChanged);
      // 
      // cmbLeadByte
      // 
      this.cmbLeadByte.DisplayMember = "TextValue";
      this.cmbLeadByte.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbLeadByte.Location = new System.Drawing.Point(28, 8);
      this.cmbLeadByte.Name = "cmbLeadByte";
      this.cmbLeadByte.Size = new System.Drawing.Size(48, 21);
      this.cmbLeadByte.TabIndex = 6;
      this.cmbLeadByte.SelectedIndexChanged += new System.EventHandler(this.cmbLeadByte_SelectedIndexChanged);
      // 
      // EncodingTestUI
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(160, 198);
      this.Controls.Add(this.cmbSecondByte);
      this.Controls.Add(this.cmbLeadByte);
      this.Controls.Add(this.lblUniCode);
      this.Controls.Add(this.lblCharacter);
      this.Controls.Add(this.lblArrow);
      this.Controls.Add(this.lblFFXICode);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "EncodingTestUI";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Encoding Test";
      this.ResumeLayout(false);

    }
    #endregion

    private void cmbLeadByte_SelectedIndexChanged(object sender, System.EventArgs e) {
      this.cmbSecondByte.Items.Clear();
      this.lblCharacter.Text = String.Empty;
      this.lblFFXICode.Text = "????";
      this.lblUniCode.Text = "????";
    HexByte HB = this.cmbLeadByte.SelectedItem as HexByte;
      if (HB != null) {
      BinaryReader BR = this.E.GetConversionTable(HB.Value);
	if (BR != null) {
	  BR.BaseStream.Seek(0, SeekOrigin.Begin);
	  for (short i = 0x00; i <= 0xff; ++i) {
	    if (BR.ReadUInt16() < 0xFFFE)
	      this.cmbSecondByte.Items.Add(new HexByte((byte) i));
  	  }
	}
        this.lblFFXICode.Text = String.Format("{0:X2}??", HB.Value);
      }
      if (this.cmbSecondByte.Items.Count > 0)
	this.cmbSecondByte.SelectedIndex = 0;
    }

    private void cmbSecondByte_SelectedIndexChanged(object sender, System.EventArgs e) {
    HexByte HB1 = this.cmbLeadByte.SelectedItem as HexByte;
    HexByte HB2 = this.cmbSecondByte.SelectedItem as HexByte;
      if (HB1 != null && HB2 != null) {
        this.lblFFXICode.Text = String.Format("{0:X2}{1:X2}", HB1.Value, HB2.Value);
      BinaryReader BR = this.E.GetConversionTable(HB1.Value);
	BR.BaseStream.Seek(2 * HB2.Value, SeekOrigin.Begin);
      ushort DecodedChar = BR.ReadUInt16();
	this.lblUniCode.Text = String.Format("{0:X4}", DecodedChar);
	this.lblCharacter.Text = String.Format("{0}", (char) DecodedChar);
      }
    }

  }

}
