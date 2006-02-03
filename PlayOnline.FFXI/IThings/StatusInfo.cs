// $Id$

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

using PlayOnline.Core;

namespace PlayOnline.FFXI {

  public class StatusInfo : Thing {

    public StatusInfo() {
      // Fill Thing helpers
      this.IconField_ = "icon";
      // Clear fields
      this.Clear();
    }

    public override string ToString() {
      return this.Name_;
    }

    public override List<PropertyPages.IThing> GetPropertyPages() {
      return base.GetPropertyPages();
    }

    #region Fields

    public static List<string> AllFields {
      get {
	return new List<string>(new string[] {
	  "id",
	  "name",
	  "status",
	  "description",
	  "icon",
	});
      }
    }

    public override List<string> GetAllFields() {
      return StatusInfo.AllFields;
    }

    #region Data Fields

    // General
    private Nullable<ushort> ID_;
    private string           Name_;
    private string           Status_;
    private string           Description_;
    private Graphic          Icon_;
    
    #endregion

    public override void Clear() {
      if (this.Icon_ != null)
	this.Icon_.Clear();
      this.ID_          = null;
      this.Name_        = null;
      this.Status_      = null;
      this.Description_ = null;
      this.Icon_        = null;
    }

    #endregion

    #region Field Access

    public override bool HasField(string Field) {
      switch (Field) {
	case "description": return (this.Description_ != null);
	case "icon":        return (this.Icon_        != null);
	case "id":          return this.ID_.HasValue;
	case "name":        return (this.Name_        != null);
	case "status":      return (this.Status_      != null);
	default:            return false;
      }
    }

    public override string GetFieldText(string Field) {
      switch (Field) {
	case "description": return this.Description_;
	case "icon":        return this.Icon_.ToString();
	case "id":          return (!this.ID_.HasValue ? String.Empty : String.Format("{0:X4}", this.ID_.Value));
	case "name":        return this.Name_;
	case "status":      return this.Status_;
	default:            return null;
      }
    }

    public override object GetFieldValue(string Field) {
      switch (Field) {
	case "description": return this.Description_;
	case "icon":        return this.Icon_;
	case "id":          return (this.ID_.HasValue ? (object) this.ID_.Value : null);
	case "name":        return this.Name_;
	case "status":      return this.Status_;
	default:            return null;
      }
    }

    protected override void LoadField(string Field, System.Xml.XmlElement Node) {
      switch (Field) {
	case "description": this.Description_ =          this.LoadTextField           (Node); break;
	case "name":        this.Name_        =          this.LoadTextField           (Node); break;
	case "id":          this.ID_          = (ushort) this.LoadUnsignedIntegerField(Node); break;
	case "status":      this.Status_      =          this.LoadTextField           (Node); break;
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
    // 002-021 TXT Name
    // 022-041 TXT Status
    // 042-141 TXT Description
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
      this.ID_          = BR.ReadUInt16();
      this.Name_        = E.GetString(BR.ReadBytes( 32)).TrimEnd('\0');
      this.Status_      = E.GetString(BR.ReadBytes( 32)).TrimEnd('\0');
      this.Description_ = E.GetString(BR.ReadBytes(256)).TrimEnd('\0');
      BR.Close();
      return true;
    }

    #endregion

  }

}
