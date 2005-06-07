#define EnableTransparency

using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Xml;

using PlayOnline.Core;

namespace PlayOnline.FFXI {

  public class FFXIGraphic {

    public  string  Format;
  
    public  byte    Flag;
    public  string  Category; // 8 ASCII chars, space-padded
    public  string  ID;       // 8 ASCII chars, space-padded

    // Windows BITMAPINFOHEADER structure
    private uint    BMHeaderSize; // always 40
    public  int     Width;
    public  int     Height;
    public  ushort  Planes;
    public  ushort  BitCount;
    public  uint    Compression;
    public  uint    ImageSize;
    public  uint    HorizontalResolution;
    public  uint    VerticalResolution;
    public  uint    UsedColors;
    public  uint    ImportantColors;

    public  Bitmap  Bitmap;

    public override string ToString() {
      return String.Format("[{0}] {1} ({4}, {2}x{3})", this.Category, this.ID, this.Width, this.Height, this.Format);
    }

    private FFXIGraphic() {
    }

    public static FFXIGraphic UnDump(XmlElement DumpedItem) {
    FFXIGraphic FG = null;
    XmlElement XIcon = DumpedItem.SelectSingleNode("icon") as XmlElement;
      if (XIcon != null) {
	FG = new FFXIGraphic();
	FG.Category = XIcon.GetAttribute("category");
	FG.ID       = XIcon.GetAttribute("id");
	FG.Format   = XIcon.GetAttribute("format");
      string ContentType = XIcon.GetAttribute("content-type");
	if (ContentType == "image/png+base64") {
	byte[] ImageData = Convert.FromBase64String(XIcon.InnerText);
	  FG.Bitmap = new Bitmap(new MemoryStream(ImageData, false));
	  // Fill everything else
	  FG.Flag = 0x91;
	  FG.BMHeaderSize    = 40;
	  FG.Planes          = 1;
	  FG.Compression     = 0;
	  FG.BitCount        = 32;
	  FG.UsedColors      = 0;
	  FG.ImportantColors = 0;
	  FG.ImageSize       = (uint) ImageData.Length;
	  FG.Width                = FG.Bitmap.Width;
	  FG.Height               = FG.Bitmap.Height;
	  FG.HorizontalResolution = (uint) FG.Bitmap.HorizontalResolution;
	  FG.VerticalResolution   = (uint) FG.Bitmap.VerticalResolution;
	}
      }
      return FG;
    }

    public static FFXIGraphic Read(BinaryReader BR) {
    FFXIGraphic FG = new FFXIGraphic();
      try {
	FG.Flag = BR.ReadByte();
	if (FG.Flag != 0x91 && FG.Flag != 0xA1 && FG.Flag != 0xB1) // Accuracy unknown
	  return null;
	FG.Category = new string(BR.ReadChars(8)).TrimEnd(' ');
	FG.ID = new string(BR.ReadChars(8)).TrimEnd(' ');
      long Pos = BR.BaseStream.Position;
	BR.BaseStream.Seek(Pos, SeekOrigin.Begin);
	FG.BMHeaderSize         = BR.ReadUInt32(); if (FG.BMHeaderSize != 40) return null;
	FG.Width                = BR.ReadInt32 (); if (FG.Width  < 0 || FG.Width  > 16 * 1024) return null;
	FG.Height               = BR.ReadInt32 (); if (FG.Height < 0 || FG.Height > 16 * 1024) return null;
	FG.Planes               = BR.ReadUInt16(); if (FG.Planes != 1) return null;
	FG.BitCount             = BR.ReadUInt16();
	FG.Compression          = BR.ReadUInt32();
	FG.ImageSize            = BR.ReadUInt32();
	FG.HorizontalResolution = BR.ReadUInt32();
	FG.VerticalResolution   = BR.ReadUInt32();
	FG.UsedColors           = BR.ReadUInt32();
	FG.ImportantColors      = BR.ReadUInt32();
      int PixelCount = FG.Height * FG.Width;
	if (FG.Flag == 0xA1) { // Assume DirectX texture
	string FourCC = String.Empty;
	  for (int i = 0; i < 4; ++i)
	    FourCC = (char) BR.ReadByte() + FourCC;
	  FourCC = FourCC.TrimEnd('\0');
	  FG.Format = String.Format(I18N.GetText("GraphicTypeDXT"), FourCC);
	  // Currently, only the DirectX texture format is (partially) supported
	  if (FourCC.StartsWith("DXT")) { // DirectX Texture format
	  int TexelBlockCount = PixelCount / 16; // 4x4 blocks
	    BR.ReadUInt64(); // Unknown
	  int x = 0;
	  int y = 0;
	  PixelFormat PF;
	    if (FourCC == "DXT2" || FourCC == "DXT4") // These have premultiplied RGB values
	      PF = PixelFormat.Format32bppPArgb;
	    else
	      PF = PixelFormat.Format32bppArgb;
	  Bitmap BM = new Bitmap(FG.Width, FG.Height, PF);
	    for (int i = 0; i < TexelBlockCount; ++i) {
	    Color[] TexelBlock = FFXIGraphic.ReadTexelBlock(BR, FourCC);
	      for (int j = 0; j < 16; ++j)
		BM.SetPixel(x + (j % 4), y + (j - (j % 4)) / 4, TexelBlock[j]);
	      x += 4;
	      if (x >= FG.Width) {
		y += 4;
		x = 0;
	      }
	    }
	    FG.Bitmap = BM;
	  }
	}
	else if (FG.Flag == 0x91 || FG.Flag == 0xB1) { // Bitmap
	  if (FG.Flag == 0xB1)
	    BR.ReadInt32(); // Unknown - always seems to be 10 (0x0000000A)
	Bitmap BM = new Bitmap(FG.Width, FG.Height, PixelFormat.Format32bppArgb);
	  FG.Format = String.Format(I18N.GetText("GraphicTypeBitmap"), FG.BitCount);
	  if (FG.BitCount == 8) { // 8-bit, with palette
	  Color[] Palette = new Color[256];
	    for (int i = 0; i < 256; ++i)
	      Palette[i] = FFXIGraphic.ReadColor(BR, 32);
	  byte[] BitFields = BR.ReadBytes(PixelCount);
	    for (int i = 0; i < PixelCount; ++i) {
	    int x = (i % FG.Width);
	    int y = FG.Height - 1 - ((i - x) / FG.Width);
	      BM.SetPixel(x, y, Palette[BitFields[i]]);
	    }
	  }
	  else {
	    for (int i = 0; i < PixelCount; ++i) {
	    int x = (i % FG.Width);
	    int y = FG.Height - 1 - ((i - x) / FG.Width);
	      BM.SetPixel(x, y, FFXIGraphic.ReadColor(BR, FG.BitCount));
	    }
	  }
	  FG.Bitmap = BM;
	}
      } catch { FG = null; }
      return ((FG != null && FG.Bitmap != null) ? FG : null);
    }

    public static Color ReadColor(BinaryReader BR, int BitDepth) {
      switch (BitDepth) {
	case 16: return FFXIGraphic.DecodeRGB565(BR.ReadUInt16());
	case 32: case 24: {
	int B = BR.ReadByte();
	int G = BR.ReadByte();
	int R = BR.ReadByte();
	int A = 255;
	  if (BitDepth == 32) {
	  byte SemiAlpha = BR.ReadByte();
	    if (SemiAlpha < 0x80)
	      A = 2 * SemiAlpha;
	  }
	  return Color.FromArgb(A, R, G, B);
	}
	case  8: {
	int GrayScale = BR.ReadByte();
	  return Color.FromArgb(GrayScale, GrayScale, GrayScale);
	}
	default:
	  return Color.HotPink;
      }
    }

    private static Color DecodeRGB565(ushort C) {
    short R = (short) ((C & 0xf800) >> 8);
    short G = (short) ((C & 0x07e0) >> 3);
    short B = (short) ((C & 0x001f) << 3);
      return Color.FromArgb(R, G, B);
    }

    private static Color[] ReadTexelBlock(BinaryReader BR, string FourCC) {
    ulong AlphaBlock = 0;
      if (FourCC == "DXT2" || FourCC == "DXT3" || FourCC == "DXT4" || FourCC == "DXT5")
	AlphaBlock = BR.ReadUInt64();
      else if (FourCC != "DXT1")
	return null;
    ushort C0 = BR.ReadUInt16();
    ushort C1 = BR.ReadUInt16();
    Color[] Colors = new Color[4];
      Colors[0] = FFXIGraphic.DecodeRGB565(C0);
      Colors[1] = FFXIGraphic.DecodeRGB565(C1);
      if (C0 > C1 || FourCC != "DXT1") { // opaque, 4-color
	Colors[2] = Color.FromArgb((2 * Colors[0].R + Colors[1].R + 1) / 3, (2 * Colors[0].G + Colors[1].G + 1) / 3, (2 * Colors[0].B + Colors[1].B + 1) / 3);
	Colors[3] = Color.FromArgb((2 * Colors[1].R + Colors[0].R + 1) / 3, (2 * Colors[1].G + Colors[0].G + 1) / 3, (2 * Colors[1].B + Colors[0].B + 1) / 3);
      }
      else { // 1-bit alpha, 3-color
	Colors[2] = Color.FromArgb((Colors[0].R + Colors[1].R) / 2, (Colors[0].G + Colors[1].G) / 2, (Colors[0].B + Colors[1].B) / 2);
	Colors[3] = Color.Transparent;
      }
    uint CompressedColor = BR.ReadUInt32();
    Color[] DecodedColors = new Color[16];
      for (int i = 0; i < 16; ++i) {
	if (FourCC == "DXT2" || FourCC == "DXT3" || FourCC == "DXT4" || FourCC == "DXT5") {
	int A = 255;
#if EnableTransparency
	  if (FourCC == "DXT2" || FourCC == "DXT3") {
	    // Seems to be 8 maximum; so treat 8 as 255 and all other values as 3-bit alpha
	    A = (int) ((AlphaBlock >> (4 * i)) & 0xf);
	    if (A >= 8)
	      A = 255;
	    else
	      A <<= 5;
	  }
	  else { // Interpolated alpha
	  int[] Alphas = new int[8];
	    Alphas[0] = (byte) ((AlphaBlock >> 0) & 0xff);
	    Alphas[1] = (byte) ((AlphaBlock >> 8) & 0xff);
	    if (Alphas[0] > Alphas[1]) {
	      Alphas[2] = (Alphas[0] * 6 + Alphas[1] * 1 + 3) / 7;
	      Alphas[3] = (Alphas[0] * 5 + Alphas[1] * 2 + 3) / 7;
	      Alphas[4] = (Alphas[0] * 4 + Alphas[1] * 3 + 3) / 7;
	      Alphas[5] = (Alphas[0] * 3 + Alphas[1] * 4 + 3) / 7;
	      Alphas[6] = (Alphas[0] * 2 + Alphas[1] * 5 + 3) / 7;
	      Alphas[7] = (Alphas[0] * 1 + Alphas[1] * 6 + 3) / 7;
	    }
	    else {
	      Alphas[2] = (Alphas[0] * 4 + Alphas[1] * 1 + 2) / 5;
	      Alphas[3] = (Alphas[0] * 3 + Alphas[1] * 2 + 2) / 5;
	      Alphas[4] = (Alphas[0] * 2 + Alphas[1] * 3 + 2) / 5;
	      Alphas[5] = (Alphas[0] * 1 + Alphas[1] * 4 + 2) / 5;
	      Alphas[6] =   0;
	      Alphas[7] = 255;
	    }
	  ulong AlphaMatrix = (AlphaBlock >> 16) & 0xffffffffffffL;
	    A = Alphas[(AlphaMatrix >> (3 * i)) & 0x7];
	  }
#endif
	  DecodedColors[i] = Color.FromArgb(A, Colors[CompressedColor & 0x3]);
	}
	else
	  DecodedColors[i] = Colors[CompressedColor & 0x3];
	CompressedColor >>= 2;
      }
      return DecodedColors;
    }

  }

}
