using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text;

using PlayOnline.Core;

namespace PlayOnline.FFXI {

  public class FFXIEncoding : Encoding { // http://www.microsoft.com/globaldev/reference/dbcs/932.htm

    // The main table, and the 60 lead-byte tables
    private static SortedList ConversionTables = new SortedList(61);

    public FFXIEncoding() {
    }

    public override string EncodingName { get { return "Japanese (Shift-JIS, with FFXI extensions)"; } }
    public override string BodyName     { get { return "iso-2022-jp-ffxi"; } }
    public override string HeaderName   { get { return "iso-2022-jp-ffxi"; } }
    public override string WebName      { get { return "iso-2022-jp-ffxi"; } }

    private static readonly char SpecialMarkerStart = '\u00AB';
    private static readonly char SpecialMarkerEnd   = '\u00BB';

    #region Utility Functions

    public override int GetByteCount(char[] chars, int index, int count) {
      return this.GetBytes(chars, index, count).Length;
    }

    public override int GetCharCount(byte[] bytes, int index, int count) {
      return this.GetString(bytes, index, count).Length;
    }

    public override int GetMaxByteCount(int charCount) {
      return charCount * 2;
    }

    public override int GetMaxCharCount(int byteCount) {
      // Assume all autotrans stuff -> every 6 bytes can become, say, 60 characters -> bytes * 10
      // Assume all elements -> every 2 bytes can become <Element: ElementName> (say, 30 characters,
      //  given that ElementName (and possibly Element:) should be localizable) -> bytes * 15.
      return byteCount * 15;
    }

    private BinaryReader GetConversionTable(byte Table) {
      if (FFXIEncoding.ConversionTables[Table] == null) {
      Stream ResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(String.Format("ConversionTables.{0:X2}xx.dat", Table));
	if (ResourceStream != null)
	  FFXIEncoding.ConversionTables[Table] = new BinaryReader(ResourceStream);
      }
      return FFXIEncoding.ConversionTables[Table] as BinaryReader;
    }

    private ushort GetTableEntry(byte Table, byte Entry) {
    BinaryReader BR = this.GetConversionTable(Table);
      if (BR != null) {
	BR.BaseStream.Seek(2 * Entry, SeekOrigin.Begin);
	return BR.ReadUInt16();
      }
      return 0xFFFF;
    }

    #endregion

    #region Encoding

    private byte[] EncodeSpecialMarker(string Marker) {
      // No match with one of our special marker formats => let GetBytes() do regular processing
      return null;
    }

    private ushort FindTableEntry(char C) {
      // Check main table, branching off to other tables if main table indicates a valid lead byte
    BinaryReader MainBR = this.GetConversionTable(0x00);
      if (MainBR != null) {
	MainBR.BaseStream.Seek(0, SeekOrigin.Begin);
	for (ushort i = 0; i < 0x100; ++i) {
	ushort MainEntry = MainBR.ReadUInt16();
	  if (MainEntry == (ushort) C) // match found
	    return i;
	  else if (MainEntry == 0xFFFE) { // valid lead byte
	  BinaryReader SubBR = this.GetConversionTable((byte) i);
	    if (SubBR != null) {
	      SubBR.BaseStream.Seek(0, SeekOrigin.Begin);
	      for (ushort j = 0x40; j < 0x100; ++j) {
		if (SubBR.ReadUInt16() == (ushort) C) // match found
		  return (ushort) ((i << 8) + j);
	      }
	    }
	  }
	}
      }
      return 0xFFFF; // no such entry in conversion tables => cannot be encoded
    }

    public override byte[] GetBytes(char[] chars, int index, int count) {
    ArrayList EncodedBytes = new ArrayList();
      for (int pos = index; pos < index + count; ++pos) {
	if (chars[pos] == FFXIEncoding.SpecialMarkerStart) { // Potential special string
	int endpos = pos + 1;
	  while (endpos < index + count && chars[endpos] != FFXIEncoding.SpecialMarkerEnd)
	    ++endpos;
	  if (endpos < index + count) { // valid end marker found => parse
	  byte[] EncodedMarker = this.EncodeSpecialMarker(new string(chars, pos + 1, endpos - pos - 1));
	    if (EncodedMarker != null) {
	      EncodedBytes.AddRange(EncodedMarker);
	      pos = endpos - 1;
	      continue;
	    }
	  }
	}
      ushort TableEntry = this.FindTableEntry(chars[pos]);
	if (TableEntry != 0xFFFF) {
	  if (TableEntry >= 0x100)
	    EncodedBytes.Add((byte) ((TableEntry & 0xFF00) >> 8));
	  EncodedBytes.Add((byte) (TableEntry & 0xFF));
	}
      }
      return (byte[]) EncodedBytes.ToArray(typeof(byte));
    }

    public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex) {
    byte[] EncodedBytes = this.GetBytes(chars, charIndex, charCount);
      Array.Copy(EncodedBytes, 0, bytes, byteIndex, EncodedBytes.Length);
      return EncodedBytes.Length;
    }

    #endregion

    #region Decoding

    public override string GetString(byte[] bytes) {
      return this.GetString(bytes, bytes.GetLowerBound(0), 1 + (bytes.GetUpperBound(0) - bytes.GetLowerBound(0)));
    }

    public override string GetString(byte[] bytes, int index, int count) {
    string DecodedString = String.Empty;
      for (int pos = index; pos < index + count; ++pos) {
	// FFXI Extension: Elemental symbols
	if (bytes[pos] == 0xEF && (pos + 1) < (index + count) && bytes[pos + 1] >= 0x1F && bytes[pos + 1] <= 0x26) {
	  DecodedString += FFXIEncoding.SpecialMarkerStart;
	  DecodedString += "Element: ";
	  switch (bytes[++pos]) {
	    case 0x1F: DecodedString += "Fire";    break;
	    case 0x20: DecodedString += "Ice";     break;
	    case 0x21: DecodedString += "Wind";    break;
	    case 0x22: DecodedString += "Earth";   break;
	    case 0x23: DecodedString += "Thunder"; break;
	    case 0x24: DecodedString += "Water";   break;
	    case 0x25: DecodedString += "Light";   break;
	    case 0x26: DecodedString += "Dark";    break;
	    default:   DecodedString += String.Format("??? ({0:X2})", bytes[pos]); break;
	  }
	  DecodedString += FFXIEncoding.SpecialMarkerEnd;
	  continue;
	}
	// FFXI Extension: Resource Text (Auto-Translator or Item)
	if (bytes[pos] == 0xFD && pos + 5 < index + count && bytes[pos + 5] == 0xFD) {
	  DecodedString += String.Format("{0}{1:X2}/{2:X2}/{3:X2}/{4:X2}: String Resource{5}", FFXIEncoding.SpecialMarkerStart, bytes[pos + 1], bytes[pos + 2], bytes[pos + 3], bytes[pos + 4], FFXIEncoding.SpecialMarkerEnd);
	  pos += 5;
	  continue;
	}
	// Default behaviour - use table
      ushort DecodedChar = this.GetTableEntry(0, bytes[pos]);
	if (DecodedChar == 0xFFFE) { // Possible Lead Byte
	  if (pos + 1 < index + count && bytes[pos + 1] >= 0x40) {
	  byte Table = bytes[pos++];
	    DecodedChar = this.GetTableEntry(Table, (byte) (bytes[pos] - 0x40));
	    if (DecodedChar == 0xFFFF)
	      DecodedString += String.Format("{0}BAD CHAR: {1:X2}{2:X2}{3}", FFXIEncoding.SpecialMarkerStart, Table, bytes[pos], FFXIEncoding.SpecialMarkerEnd);
	    else
	      DecodedString += (char) DecodedChar;
	  }
	  else
	    DecodedString += String.Format("{0}BAD CHAR: {1:X2}{2}", FFXIEncoding.SpecialMarkerStart, bytes[pos], FFXIEncoding.SpecialMarkerEnd);
	}
	else if (DecodedChar == 0xFFFF)
	  DecodedString += String.Format("{0}BAD CHAR: {1:X2}{2}", FFXIEncoding.SpecialMarkerStart, bytes[pos], FFXIEncoding.SpecialMarkerEnd);
	else
	  DecodedString += (char) DecodedChar;
      }
      return DecodedString;
    }

    public override char[] GetChars(byte[] bytes, int index, int count) {
      return this.GetString(bytes, index, count).ToCharArray();
    }

    public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex) {
    char[] DecodedChars = this.GetChars(bytes, byteIndex, byteCount);
      Array.Copy(DecodedChars, 0, chars, charIndex, DecodedChars.Length);
      return DecodedChars.Length;
    }

    #endregion

  }

}
