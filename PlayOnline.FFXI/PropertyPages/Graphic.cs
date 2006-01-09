// $Id$

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using PlayOnline.Core;

namespace PlayOnline.FFXI.PropertyPages {

  public partial class Graphic : IThing {

    public Graphic(FFXI.Graphic G) {
      InitializeComponent();
      this.picImage.Image = G.GetFieldValue("image") as Image;
      this.cmbViewMode.Items.Add(new NamedEnum(PictureBoxSizeMode.Normal));
      this.cmbViewMode.Items.Add(new NamedEnum(PictureBoxSizeMode.CenterImage));
      this.cmbViewMode.Items.Add(new NamedEnum(PictureBoxSizeMode.StretchImage));
      this.cmbViewMode.Items.Add(new NamedEnum(PictureBoxSizeMode.Zoom));
      this.cmbViewMode.SelectedIndex = 0;
    }

    private Color SolidColor_ = Color.White;

    private void cmbViewMode_SelectedIndexChanged(object sender, EventArgs e) {
      NamedEnum NE = this.cmbViewMode.SelectedItem as NamedEnum;
      this.picImage.SizeMode = (PictureBoxSizeMode) NE.Value;
    }

    private void btnSelectColor_Click(object sender, EventArgs e) {
      this.dlgChooseColor.Color = this.SolidColor_;
      if (this.dlgChooseColor.ShowDialog(this) == DialogResult.OK) {
	this.SolidColor_ = this.dlgChooseColor.Color;
	// FIXME: Persist custom colors?
	if (this.radSolid.Checked)
	  this.picImage.BackColor = this.SolidColor_;
      }
    }

    private void radTransparent_CheckedChanged(object sender, EventArgs e) {
      if (this.radTransparent.Checked)
	this.picImage.BackColor = Color.Transparent;
    }

    private void radSolid_CheckedChanged(object sender, EventArgs e) {
      if (this.radSolid.Checked)
	this.picImage.BackColor = this.SolidColor_;
    }

  }

}
