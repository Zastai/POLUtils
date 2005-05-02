using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;

using PlayOnline.Core;

namespace PlayOnline.FFXI.Utils.ConfigEditor {

  public class CharacterConfig {

    public CharacterConfig(Character C) {
      this.Character_ = C;
      this.LoadConfig();
    }

    #region Properties

    public Character Character { get { return this.Character_; } }
    public Color[]   Colors    { get { return this.Colors_;    } }

    public string CharacterName { // For ComboBox purposes
      get {
	return ((this.Character_ == null) ? "<No Character>" : this.Character_.Name);
      }
    }

    #region Private Data

    private Character Character_;
    private Color[]   Colors_;

    #endregion

    #endregion

    private void LoadConfig() {
      if (this.Character_ == null)
	return;
    BinaryReader BR = new BinaryReader(this.Character_.OpenUserFile("cnf.dat", FileMode.Open, FileAccess.Read));
      if (BR != null) {
	BR.BaseStream.Seek(0x50, SeekOrigin.Begin);
	this.Colors_ = new Color[23] {
	  FFXIGraphic.ReadColor(BR, 32), FFXIGraphic.ReadColor(BR, 32), FFXIGraphic.ReadColor(BR, 32),
	  FFXIGraphic.ReadColor(BR, 32), FFXIGraphic.ReadColor(BR, 32), FFXIGraphic.ReadColor(BR, 32),
	  FFXIGraphic.ReadColor(BR, 32), FFXIGraphic.ReadColor(BR, 32), FFXIGraphic.ReadColor(BR, 32),
	  FFXIGraphic.ReadColor(BR, 32), FFXIGraphic.ReadColor(BR, 32), FFXIGraphic.ReadColor(BR, 32),
	  FFXIGraphic.ReadColor(BR, 32), FFXIGraphic.ReadColor(BR, 32), FFXIGraphic.ReadColor(BR, 32),
	  FFXIGraphic.ReadColor(BR, 32), FFXIGraphic.ReadColor(BR, 32), FFXIGraphic.ReadColor(BR, 32),
	  FFXIGraphic.ReadColor(BR, 32), FFXIGraphic.ReadColor(BR, 32), FFXIGraphic.ReadColor(BR, 32),
	  FFXIGraphic.ReadColor(BR, 32), FFXIGraphic.ReadColor(BR, 32)
	};
	BR.Close();
      }
    }

  }

}
