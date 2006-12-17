// $Id$

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

using PlayOnline.Core;

namespace PlayOnline.FFXI.Things {

  public class StatusInfo : Thing {

    public StatusInfo() {
      // Fill Thing helpers
      this.IconField_ = "icon";
      // Clear fields
      this.Clear();
    }

    public override string ToString() {
      return String.Format("{0} / {1}", this.EnglishName_, this.JapaneseName_);
    }

    public override List<PropertyPages.IThing> GetPropertyPages() {
      return base.GetPropertyPages();
    }

    #region Fields

    public static List<string> AllFields {
      get {
	return new List<string>(new string[] {
	  "id",
	  "english-name",
	  "english-description",
	  "japanese-name",
	  "japanese-description",
	  "icon",
	  "unknown-1",
	});
      }
    }

    public override List<string> GetAllFields() {
      return StatusInfo.AllFields;
    }

    #region Data Fields

    // General
    private ushort? ID_;
    private string  EnglishName_;
    private string  JapaneseName_;
    private string  EnglishDescription_;
    private string  JapaneseDescription_;
    private ushort? Unknown1_;
    private Graphic Icon_;
    
    #endregion

    public override void Clear() {
      if (this.Icon_ != null)
	this.Icon_.Clear();
      this.ID_                  = null;
      this.EnglishName_         = null;
      this.JapaneseName_        = null;
      this.EnglishDescription_  = null;
      this.JapaneseDescription_ = null;
      this.Unknown1_            = null;
    }

    #endregion

    #region Field Access

    public override bool HasField(string Field) {
      switch (Field) {
	// Objects
	case "english-description":  return (this.EnglishDescription_  != null);
	case "english-name":         return (this.EnglishName_         != null);
	case "icon":                 return (this.Icon_                != null);
	case "japanese-description": return (this.JapaneseDescription_ != null);
	case "japanese-name":        return (this.JapaneseName_        != null);
	// Nullables
	case "id":                   return this.ID_.HasValue;
	case "unknown-1":            return this.Unknown1_.HasValue;
	default:                     return false;
      }
    }

    public override string GetFieldText(string Field) {
      switch (Field) {
	// Strings
	case "english-description":  return this.EnglishDescription_;
	case "english-name":         return this.EnglishName_;
	case "japanese-description": return this.JapaneseDescription_;
	case "japanese-name":        return this.JapaneseName_;
	// Objects
	case "icon":                 return this.Icon_.ToString();
	// Nullables
	case "id":                   return (!this.ID_.HasValue ? String.Empty : String.Format("{0}", this.ID_.Value));
	// Nullables - Hex form
	case "unknown-1":            return (!this.Unknown1_.HasValue ? String.Empty : String.Format("{0:X4}", this.Unknown1_.Value));
	default:                     return null;
      }
    }

    public override object GetFieldValue(string Field) {
      switch (Field) {
	// Objects
	case "english-description":  return this.EnglishDescription_;
	case "english-name":         return this.EnglishName_;
	case "icon":                 return this.Icon_;
	case "japanese-description": return this.JapaneseDescription_;
	case "japanese-name":        return this.JapaneseName_;
	// Nullables
	case "id":                   return (this.ID_.HasValue       ? (object) this.ID_.Value       : null);
	case "unknown-1":            return (this.Unknown1_.HasValue ? (object) this.Unknown1_.Value : null);
	default:                     return null;
      }
    }

    protected override void LoadField(string Field, System.Xml.XmlElement Node) {
      switch (Field) {
	case "english-description":  this.EnglishDescription_  =          this.LoadTextField           (Node); break;
	case "english-name":         this.EnglishName_         =          this.LoadTextField           (Node); break;
	case "id":                   this.ID_                  = (ushort) this.LoadUnsignedIntegerField(Node); break;
	case "japanese-description": this.JapaneseDescription_ =          this.LoadTextField           (Node); break;
	case "japanese-name":        this.JapaneseName_        =          this.LoadTextField           (Node); break;
	case "unknown-1":            this.Unknown1_            = (ushort) this.LoadUnsignedIntegerField(Node); break;
	case "icon":
	  if (this.Icon_ == null)
	    this.Icon_ = new Graphic();
	  this.LoadThingField(Node, this.Icon_); break;
      }
    }

    #endregion

    #region ROM File Reading

    // Block Layout:
    // 000-001 U16 Index
    // 002-021 TXT English Name
    // 022-041 TXT Japanese Name
    // 042-0C1 TXT English Description
    // 0C2-141 TXT Japanese Description
    // 200-201 U16 Icon Size
    // 202-bff IMG Icon (+ padding)
    public bool Read(BinaryReader BR) {
      this.Clear();
      try {
      byte[] Bytes     = BR.ReadBytes(0x200);
      byte[] IconBytes = BR.ReadBytes(0xa00);
	if (!FFXIEncryption.DecodeDataBlock(Bytes))
	  return false;
	if (IconBytes[0x9ff] != 0xff)
	  return false;
	{ // Verify that the icon info is valid
	Graphic StatusIcon = new Graphic();
	BinaryReader IconBR = new BinaryReader(new MemoryStream(IconBytes, false));
	int IconSize = IconBR.ReadInt32();
	  if (IconSize > 0 && IconSize <= 0x9fb) {
	    if (!StatusIcon.Read(IconBR) || IconBR.BaseStream.Position != 4 + IconSize) {
	      IconBR.Close();
	      return false;
	    }
	  }
	  IconBR.Close();
	  if (StatusIcon == null)
	    return false;
	  this.Icon_ = StatusIcon;
	}
	BR = new BinaryReader(new MemoryStream(Bytes, false));
      } catch { return false; }
    FFXIEncoding E = new FFXIEncoding();
      this.ID_                  = BR.ReadUInt16();
      this.EnglishName_         = E.GetString(BR.ReadBytes( 32)).TrimEnd('\0');
      this.JapaneseName_        = E.GetString(BR.ReadBytes( 32)).TrimEnd('\0');
      this.EnglishDescription_  = E.GetString(BR.ReadBytes(128)).TrimEnd('\0');
      this.JapaneseDescription_ = E.GetString(BR.ReadBytes(128)).TrimEnd('\0');
      this.Unknown1_            = BR.ReadUInt16();
      BR.Close();
      return true;
    }

    #endregion

  }

}
