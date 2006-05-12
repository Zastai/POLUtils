// $Id$

using System;
using System.Collections;
using System.IO;
using System.Text;

namespace PlayOnline.FFXI {

  public sealed class FFXIEncryption {

    private static byte Rotate(byte B, int ShiftSize) {
      return (byte) ((B >> ShiftSize) | (B << (8 - ShiftSize)));
    }

    public static bool Rotate(byte[] Data, byte ShiftSize) {
      return FFXIEncryption.Rotate(Data, 0L, Data.Length, ShiftSize);
    }

    public static bool Rotate(byte[] Data, long Offset, long Size, byte ShiftSize) {
      if (ShiftSize < 1 || ShiftSize > 8)
	return false;
      for (long i = 0; i < Size; ++i)
	Data[Offset + i] = FFXIEncryption.Rotate(Data[Offset + i], ShiftSize);
      return true;
    }

    private static int CountBits(byte B) {
    int Count = 0;
      while (B != 0) {
       if ((B & 0x01) != 0)
         ++Count;
        B >>= 1;
      }
      return Count;
    }

    private static byte GetTextShiftSize(byte[] Data, long Offset, long Size) {
      if (Size < 2)
	return 0;
      if (Data[Offset + 0] == 0 && Data[Offset + 1] == 0)
	return 0;
      // This is the heuristic that ffxitool uses to determine the shift size - it makes absolutely no
      // sense to me, but it works; I suppose the author of ffxitool reverse engineered what FFXI does.
    int BitCount = FFXIEncryption.CountBits(Data[Offset + 1]) - FFXIEncryption.CountBits(Data[Offset + 0]);
      switch (Math.Abs(BitCount) % 5) {
	case 0: return 1;
	case 1: return 7;
	case 2: return 2;
	case 3: return 6;
	case 4: return 3;
      }
      return 0;
    }

    private static byte GetDataShiftSize(byte[] Data, long Offset, long Size) {
      if (Size < 13)
	return 0;
      // This is the heuristic that ffxitool uses to determine the shift size - it makes absolutely no
      // sense to me, but it works; I suppose the author of ffxitool reverse engineered what FFXI does.
    int BitCount = FFXIEncryption.CountBits(Data[Offset +  2])
                 - FFXIEncryption.CountBits(Data[Offset + 11])
                 + FFXIEncryption.CountBits(Data[Offset + 12]);
      switch (Math.Abs(BitCount) % 5) {
	case 0: return 7;
	case 1: return 1;
	case 2: return 6;
	case 3: return 2;
	case 4: return 5;
      }
      return 0;
    }

    public static bool DecodeTextBlock(byte[] Data) {
      return FFXIEncryption.DecodeTextBlock(Data, 0L, Data.LongLength);
    }

    public static bool DecodeTextBlock(byte[] Data, long Offset, long Size) {
      return FFXIEncryption.Rotate(Data, Offset, Size, FFXIEncryption.GetTextShiftSize(Data, Offset, Size));
    }

    public static bool DecodeDataBlock(byte[] Data) {
      return FFXIEncryption.DecodeDataBlock(Data, 0L, Data.LongLength);
    }

    public static bool DecodeDataBlock(byte[] Data, long Offset, long Size) {
      return FFXIEncryption.Rotate(Data, Offset, Size, FFXIEncryption.GetDataShiftSize(Data, Offset, Size));
    }

    public static string ReadEncodedString(BinaryReader BR, Encoding E) {
    ArrayList LineBytes = new ArrayList();
    byte B = BR.ReadByte();
      while (B != 0) {
	LineBytes.Add(B);
	B = BR.ReadByte();
      }
    byte[] EncodedText = (byte[]) LineBytes.ToArray(typeof(byte));
      FFXIEncryption.DecodeTextBlock(EncodedText);
      return E.GetString(EncodedText);
    }

  }

}
