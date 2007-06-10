// $Id$

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using PlayOnline.Core;

namespace PlayOnline.FFXI.Things {

  public class DMSGStringTableEntry2 : Thing {

    public DMSGStringTableEntry2() {
      // Clear fields
      this.Clear();
    }

    public override string ToString() {
    string Text = String.Empty;
      if (this.String1_ != null)
	Text += this.String1_;
      if (this.String2_ != null) {
	Text += " / ";
	Text += this.String2_;
	if (this.String3_ != null) {
	  Text += " / ";
	  Text += this.String3_;
	  if (this.String4_ != null) {
	    Text += " / ";
	    Text += this.String4_;
	    if (this.String5_ != null) {
	      Text += " / ";
	      Text += this.String5_;
	    }
	  }
	}
      }
      return Text;
    }

    public override List<PropertyPages.IThing> GetPropertyPages() {
      return base.GetPropertyPages();
    }

    #region Fields

    public static List<string> AllFields {
      get {
	return new List<string>(new string[] {
	  "index",
	  "string-1",
	  "string-2",
	  "string-3",
	  "string-4",
	  "string-5",
	  // 5 is an arbitrary limit; so far I've only seen tables with 1 or 2 strings per entry
	  "string-count"
	});
      }
    }

    public override List<string> GetAllFields() {
      return DMSGStringTableEntry2.AllFields;
    }

    #region Data Fields

    private uint?  Index_;
    private string String1_;
    private string String2_;
    private string String3_;
    private string String4_;
    private string String5_;
    private uint?  StringCount_;
    
    #endregion

    public override void Clear() {
      this.Index_       = null;
      this.String1_     = null;
      this.String2_     = null;
      this.String3_     = null;
      this.String4_     = null;
      this.String5_     = null;
      this.StringCount_ = null;
    }

    #endregion

    #region Field Access

    public override bool HasField(string Field) {
      switch (Field) {
	case "index":        return this.Index_.HasValue;
	case "string-1":     return (this.String1_ != null);
	case "string-2":     return (this.String2_ != null);
	case "string-3":     return (this.String3_ != null);
	case "string-4":     return (this.String4_ != null);
	case "string-5":     return (this.String5_ != null);
	case "string-count": return this.StringCount_.HasValue;
	default:             return false;
      }
    }

    public override string GetFieldText(string Field) {
      switch (Field) {
	case "index":        return (!this.Index_.HasValue ? String.Empty : String.Format("{0:00000}", this.Index_.Value));
	case "string-1":     return ((this.String1_ == null) ? String.Empty : this.String1_);
	case "string-2":     return ((this.String2_ == null) ? String.Empty : this.String2_);
	case "string-3":     return ((this.String3_ == null) ? String.Empty : this.String3_);
	case "string-4":     return ((this.String4_ == null) ? String.Empty : this.String4_);
	case "string-5":     return ((this.String5_ == null) ? String.Empty : this.String5_);
	case "string-count": return (!this.StringCount_.HasValue ? String.Empty : this.StringCount_.ToString());
	default:             return null;
      }
    }

    public override object GetFieldValue(string Field) {
      switch (Field) {
	case "index":        return (!this.Index_.HasValue ? null : (object) this.Index_.Value);
	case "string-1":     return this.String1_;
	case "string-2":     return this.String2_;
	case "string-3":     return this.String3_;
	case "string-4":     return this.String4_;
	case "string-5":     return this.String5_;
	case "string-count": return (!this.StringCount_.HasValue ? null : (object) this.StringCount_.Value);
	default:             return null;
      }
    }

    protected override void LoadField(string Field, System.Xml.XmlElement Node) {
      switch (Field) {
	case "index":        this.Index_       = (uint) this.LoadUnsignedIntegerField(Node); break;
	case "string-1":     this.String1_     = this.LoadTextField(Node); break;
	case "string-2":     this.String2_     = this.LoadTextField(Node); break;
	case "string-3":     this.String3_     = this.LoadTextField(Node); break;
	case "string-4":     this.String4_     = this.LoadTextField(Node); break;
	case "string-5":     this.String5_     = this.LoadTextField(Node); break;
	case "string-count": this.StringCount_ = (uint) this.LoadUnsignedIntegerField(Node); break;
      }
    }

    #endregion

    #region ROM File Reading

    public bool Read(BinaryReader BR, Encoding E, uint HeaderBytes, uint EntryBytes, uint DataBytes) {
      return this.Read(BR, E, null, HeaderBytes, EntryBytes, DataBytes);
    }

    public bool Read(BinaryReader BR, Encoding E, uint? Index, uint HeaderBytes, uint EntryBytes, uint DataBytes) {
      this.Clear();
      this.Index_ = Index;
      BR.BaseStream.Position = HeaderBytes + 0x8 * Index.Value;
      try {
      uint Offset = ~BR.ReadUInt32();
      uint Length = ~BR.ReadUInt32();
	if (Length < 0 || Offset + Length > DataBytes)
	  return false;
	BR.BaseStream.Position = HeaderBytes + EntryBytes + Offset;
	this.StringCount_ = ~BR.ReadUInt32();
      uint[] StringOffsets = new uint[this.StringCount_.Value];
      uint[] StringFlags   = new uint[this.StringCount_.Value];
	for (uint i = 0; i < this.StringCount_.Value; ++i) {
	  StringOffsets[i] = ~BR.ReadUInt32();
	  StringFlags  [i] = ~BR.ReadUInt32();
	  if (StringOffsets[i] < 0 || (StringFlags[i] != 0 && StringFlags[i] != 1))
	    return false;
	}
	// Meaning of flags is unclear so far.
	for (uint i = 0; i < this.StringCount_.Value; ++i) {
	   // not sure why the offsets are based 28 bytes past the start of the entry, but they are
	  BR.BaseStream.Position = HeaderBytes + EntryBytes + Offset + 28 + StringOffsets[i];
	List<byte> TextBytes = new List<byte>();
	  while (true) {
	  byte[] FourBytes = BR.ReadBytes(4);
	    FourBytes[0] ^= 0xff;
	    FourBytes[1] ^= 0xff;
	    FourBytes[2] ^= 0xff;
	    FourBytes[3] ^= 0xff;
	    TextBytes.AddRange(FourBytes);
	    if (FourBytes[3] == 0)
	      break;
	  }
	string Text = E.GetString(TextBytes.ToArray()).TrimEnd('\0');
	  switch (i) {
	    case 0: this.String1_ = Text; break;
	    case 1: this.String2_ = Text; break;
	    case 2: this.String3_ = Text; break;
	    case 3: this.String4_ = Text; break;
	    case 4: this.String5_ = Text; break;
	  }
	}
	return true;
      } catch { }
      this.Clear();
      return false;
    }

    #endregion

  }

}
