// $Id$

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Text;
using System.Windows.Forms;

using PlayOnline.Core;

namespace PlayOnline.FFXI.PropertyPages {

  public partial class Graphic : IThing {

    public Graphic(FFXI.Graphic G) {
      InitializeComponent();
      this.dlgSaveImage.FileName = G.ToString() + ".png";
      this.picImage.Image = G.GetFieldValue("image") as Image;
      this.cmbViewMode.Items.Add(new NamedEnum(PictureBoxSizeMode.Normal));
      this.cmbViewMode.Items.Add(new NamedEnum(PictureBoxSizeMode.CenterImage));
      this.cmbViewMode.Items.Add(new NamedEnum(PictureBoxSizeMode.StretchImage));
      this.cmbViewMode.Items.Add(new NamedEnum(PictureBoxSizeMode.Zoom));
      this.cmbViewMode.SelectedIndex = 0;
      this.cmbBackColor.SelectedIndex = 0;
    }

    private Color SolidColor_ = Color.White;

    private void cmbViewMode_SelectedIndexChanged(object sender, EventArgs e) {
      NamedEnum NE = this.cmbViewMode.SelectedItem as NamedEnum;
      this.picImage.SizeMode = (PictureBoxSizeMode) NE.Value;
    }

    private void cmbBackColor_SelectedIndexChanged(object sender, EventArgs e) {
      this.btnSelectColor.Enabled = (this.cmbBackColor.SelectedIndex != 0);
      switch (this.cmbBackColor.SelectedIndex) {
	case 0:  this.picImage.BackColor = Color.Transparent; break;
	case 1:  this.picImage.BackColor = this.SolidColor_;  break;
	default: this.picImage.BackColor = Color.Black;       break;
      }
    }

    private void btnSelectColor_Click(object sender, EventArgs e) {
      this.dlgChooseColor.Color = this.SolidColor_;
      if (this.dlgChooseColor.ShowDialog(this) == DialogResult.OK) {
	this.SolidColor_ = this.dlgChooseColor.Color;
	this.picImage.BackColor = this.SolidColor_;
	// FIXME: Persist custom colors?
      }
    }

    private void btnSave_Click(object sender, EventArgs e) {
      if (this.dlgSaveImage.ShowDialog(this) == DialogResult.OK)
	this.picImage.Image.Save(this.dlgSaveImage.FileName, ImageFormat.Png);
    }

  }

}
