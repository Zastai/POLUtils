using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using PlayOnline.Core;
using PlayOnline.FFXI;

namespace PlayOnline.FFXI.Utils.StrangeApparatus {

  public partial class MainWindow : Form {

    public MainWindow() {
      this.InitializeComponent();
    }

    // The values to add to the digits do not match the element numbers used in the item data,
    // but instead match the order of the days (FEWWILLDie).
    private byte[] ElementOffsets = new byte[] { 0, 3, 5, 2, 1, 4, 6, 7 };

    // The area numbers where the strange apparatuses are located.
    private byte[] AreaIDs = new byte[] { 191, 196, 197, 193, 195, 194, 200, 198 };

    private void btnGenerateCodes_Click(object sender, EventArgs e) {
      this.lvCodes.Items.Clear();
      if (this.txtCharacterName.Text.Length < 3) {
	MessageBox.Show(this, I18N.GetText("Dialog:NameTooShort"), I18N.GetText("Title:NameTooShort"), MessageBoxButtons.OK, MessageBoxIcon.Error);
	return;
      }
      byte[] Digits = new byte[4];
      for (byte i = 0; i < 3; ++i) {
	char C = Char.ToUpperInvariant(this.txtCharacterName.Text[i]);
	if (C < 'A' || C > 'Z') {
	  MessageBox.Show(this, I18N.GetText("Dialog:InvalidName"), I18N.GetText("Title:InvalidName"), MessageBoxButtons.OK, MessageBoxIcon.Error);
	  return;
	}
	Digits[i] = (byte) (C - 'A');
      }
      Digits[3] = (byte) ((Digits[0] + Digits[1] + Digits[2]) % 100);
      for (byte i = 0; i < 8; ++i) {
      ListViewItem LVI = this.lvCodes.Items.Add(FFXIResourceManager.GetResourceString(0x00020000U + this.AreaIDs[i]));
	LVI.SubItems.Add(new NamedEnum((Element)      this.ElementOffsets[i]).ToString());
	LVI.SubItems.Add(new NamedEnum((ElementColor) this.ElementOffsets[i]).ToString());
	LVI.SubItems.Add(String.Format("{0:00}{1:00}{2:00}{3:00}", Digits[0] + i, Digits[1] + i, Digits[2] + i, (Digits[3] + 4 * i) % 100));
      }
    }

  }

}