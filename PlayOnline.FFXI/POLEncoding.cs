using System;
using System.Collections;
using System.Text;

using PlayOnline.Core;

namespace PlayOnline.FFXI {

  public class POLEncoding : Encoding {

    // The main base encoding
    private Encoding ShiftJIS;

    // These are used for subsets of the range using the 0x85 prefix
    private Encoding Cyrillic;
    private Encoding Western;
    
    public POLEncoding() {
      this.ShiftJIS = Encoding.GetEncoding("shift-jis");
      this.Cyrillic = Encoding.GetEncoding(1251);
      this.Western  = Encoding.GetEncoding(1252);
    }

    public override string EncodingName { get { return "Japanese (Shift-JIS, with PlayOnline extensions)"; } }
    public override string BodyName     { get { return "iso-2022-jp-pol"; } }
    public override string HeaderName   { get { return "iso-2022-jp-pol"; } }
    public override string WebName      { get { return "iso-2022-jp-pol"; } }

    public override int GetByteCount(char[] chars, int index, int count) {
    int ByteCount = 0;
    int LastSpecial = 0;
      for (int i = 0; i < count; ++i) {
      byte[] EncodedSpecialChar = null;
	if (chars[index + i] >= 0xFF61 && chars[index + i] <= 0xFF7F)
	  EncodedSpecialChar = new byte[] { (byte) (chars[index + i] - 0xFF00 + 0x1F) };
	else
	  EncodedSpecialChar = null;
	if (EncodedSpecialChar == null) {
	  EncodedSpecialChar = this.Western.GetBytes(chars, index + i, 1);
	  if (EncodedSpecialChar.Length == 1 && EncodedSpecialChar[0] >= 0xC0 && EncodedSpecialChar[0] <= 0xFF)
	    EncodedSpecialChar[0] -= 0x21;
	  else if (EncodedSpecialChar.Length == 1 && EncodedSpecialChar[0] >= 0x80 && EncodedSpecialChar[0] <= 0xBF)
	    EncodedSpecialChar[0] -= 0x40;
	  else
	    EncodedSpecialChar = null;
	}
	if (EncodedSpecialChar == null) {
	  EncodedSpecialChar = this.Cyrillic.GetBytes(chars, index + i, 1);
	  if (EncodedSpecialChar.Length == 1 && EncodedSpecialChar[0] >= 0xE0 && EncodedSpecialChar[0] <= 0xFF)
	    EncodedSpecialChar[0] -= 0xC1;
	  else
	    EncodedSpecialChar = null;
	}
	if (EncodedSpecialChar != null) {
	  if (LastSpecial < i)
	    ByteCount += this.ShiftJIS.GetByteCount(chars, index + LastSpecial, i - LastSpecial);
	  ByteCount += 2;
	  LastSpecial = i + 1;
	}
      }
      if (LastSpecial < count)
	ByteCount += this.ShiftJIS.GetByteCount(chars, index + LastSpecial, count - LastSpecial);
      return ByteCount;
    }

    public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex) {
    int ByteCount = 0;
    int LastSpecial = 0;
      for (int i = 0; i < charCount; ++i) {
      byte[] EncodedSpecialChar = null;
	if (chars[charIndex + i] >= 0xFF61 && chars[charIndex + i] <= 0xFF7F)
	  EncodedSpecialChar = new byte[] { (byte) (chars[charIndex + i] - 0xFF00 + 0x1F) };
	else
	  EncodedSpecialChar = null;
	if (EncodedSpecialChar == null) {
	  EncodedSpecialChar = this.Western.GetBytes(chars, charIndex + i, 1);
	  if (EncodedSpecialChar.Length == 1 && EncodedSpecialChar[0] >= 0xC0 && EncodedSpecialChar[0] <= 0xFF)
	    EncodedSpecialChar[0] -= 0x21;
	  else if (EncodedSpecialChar.Length == 1 && EncodedSpecialChar[0] >= 0x80 && EncodedSpecialChar[0] <= 0xBF)
	    EncodedSpecialChar[0] -= 0x40;
	  else
	    EncodedSpecialChar = null;
	}
	if (EncodedSpecialChar == null) {
	  EncodedSpecialChar = this.Cyrillic.GetBytes(chars, charIndex + i, 1);
	  if (EncodedSpecialChar.Length == 1 && EncodedSpecialChar[0] >= 0xE0 && EncodedSpecialChar[0] <= 0xFF)
	    EncodedSpecialChar[0] -= 0xC1;
	  else
	    EncodedSpecialChar = null;
	}
	if (EncodedSpecialChar != null) {
	  if (LastSpecial < i) {
	  byte[] EncodedText = this.ShiftJIS.GetBytes(chars, charIndex + LastSpecial, i - LastSpecial);
	    Array.Copy(EncodedText, 0, bytes, byteIndex + ByteCount, EncodedText.Length);
	    ByteCount += EncodedText.Length;
	  }
	  bytes[byteIndex + ByteCount++] = 0x85;
	  bytes[byteIndex + ByteCount++] = EncodedSpecialChar[0];
	  LastSpecial = i + 1;
	}
      }
      if (LastSpecial < charCount) {
      byte[] EncodedText = this.ShiftJIS.GetBytes(chars, charIndex + LastSpecial, charCount - LastSpecial);
	Array.Copy(EncodedText, 0, bytes, byteIndex + ByteCount, EncodedText.Length);
	ByteCount += EncodedText.Length;
      }
      return ByteCount;
    }

    private char DecodeSpecialChar(byte SpecialChar) {
      if (SpecialChar >= 0x1F && SpecialChar <= 0x3E) // Special Symbols
	return this.Cyrillic.GetChars(new byte[] { (byte) (SpecialChar + 0xC1) })[0];
      else if (SpecialChar >= 0x40 && SpecialChar <= 0x7F) // Special Symbols
	return this.Western.GetChars(new byte[] { (byte) (SpecialChar + 0x40) })[0];
      else if (SpecialChar >= 0x80 && SpecialChar <= 0x9E) { // Half-Width Katakana
	return (char) (0xFF00 + SpecialChar - 0x1F);
      }
      else if (SpecialChar >= 0x9F && SpecialChar <= 0xDE) // Accented Latin Letters
	return this.Western.GetChars(new byte[] { (byte) (SpecialChar + 0x21) })[0];
      else
	return '?';
    }

    public override int GetCharCount(byte[] bytes, int index, int count) {
    int CharCount = 0;
    int LastSpecial = 0;
      for (int i = 0; i < count; ++i) {
	if (bytes[index + i] == 0x85) {
	  if (LastSpecial < i)
	    CharCount += this.ShiftJIS.GetCharCount(bytes, index + LastSpecial, (i - LastSpecial));
	  ++i; ++CharCount;
	  LastSpecial = i + 1;
	}
      }
      if (LastSpecial < count)
	CharCount += this.ShiftJIS.GetCharCount(bytes, index + LastSpecial, (count - LastSpecial));
      return CharCount;
    }

    public override char[] GetChars(byte[] bytes, int index, int count) {
    ArrayList DecodedChars = new ArrayList();
    int LastSpecial = 0;
      for (int i = 0; i < count; ++i) {
	if (bytes[index + i] == 0x85 && (i + 1) < count) {
	  if (LastSpecial < i)
	    DecodedChars.AddRange(this.ShiftJIS.GetChars(bytes, index + LastSpecial, (i - LastSpecial)));
	  DecodedChars.Add(this.DecodeSpecialChar(bytes[index + i + 1]));
	  ++i;
	  LastSpecial = i + 1;
	}
      }
      if (LastSpecial < count)
	DecodedChars.AddRange(this.ShiftJIS.GetChars(bytes, index + LastSpecial, (count - LastSpecial)));
      return (char[]) DecodedChars.ToArray(typeof(char));
    }

    public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) {
    char[] DecodedChars = this.GetChars(bytes, byteIndex, byteCount);
      Array.Copy(DecodedChars, 0, chars, charIndex, DecodedChars.Length);
      return DecodedChars.Length;
    }

    public override int GetMaxByteCount(int charCount) {
      return charCount * 2;
    }

    public override int GetMaxCharCount(int byteCount) {
      return byteCount;
    }


  }
  
}