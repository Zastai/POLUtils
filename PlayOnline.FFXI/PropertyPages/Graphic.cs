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

  public partial class Graphic : UserControl {

    public Graphic(FFXI.Graphic G) {
      InitializeComponent();
      this.picImage.Image = G.GetFieldValue("image") as Image;
      this.cmbViewMode.Items.Add(new NamedEnum(PictureBoxSizeMode.Normal));
      this.cmbViewMode.Items.Add(new NamedEnum(PictureBoxSizeMode.CenterImage));
      this.cmbViewMode.Items.Add(new NamedEnum(PictureBoxSizeMode.StretchImage));
      this.cmbViewMode.Items.Add(new NamedEnum(PictureBoxSizeMode.Zoom));
      this.cmbViewMode.SelectedIndex = 0;
    }

    public static List<TabPage> GetPages(FFXI.Graphic G) {
    List<TabPage> Pages = new List<TabPage>();
    Graphic PageControl = new Graphic(G);
      while (PageControl.tabPages.TabPages.Count > 0) {
	Pages.Add(PageControl.tabPages.TabPages[0]);
	PageControl.tabPages.TabPages.RemoveAt(0);
      }
      return Pages;
    }

    private Color SolidColor_ = Color.White;

    private void cmbViewMode_SelectedIndexChanged(object sender, EventArgs e) {
      NamedEnum NE = this.cmbViewMode.SelectedItem as NamedEnum;
      this.picImage.SizeMode = (PictureBoxSizeMode) NE.Value;
    }

    private void btnSelectColor_Click(object sender, EventArgs e) {
    ColorDialog CD = new ColorDialog();
      CD.Color = this.SolidColor_;
      if (CD.ShowDialog(this) == DialogResult.OK) {
	this.SolidColor_ = CD.Color;
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
