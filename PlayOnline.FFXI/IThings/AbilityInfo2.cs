// $Id$

// Copyright © 2010 Chris Baggett, Tim Van Holder
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Collections.Generic;
using System.IO;

using PlayOnline.Core;

namespace PlayOnline.FFXI.Things {

  public class AbilityInfo2 : Thing {

    public AbilityInfo2() {
      // Clear fields
      this.Clear();
    }

    public override string ToString() {
      return String.Format("Ability #{0}", this.ID_);
    }

    public override List<PropertyPages.IThing> GetPropertyPages() {
      return base.GetPropertyPages();
    }

    #region Fields

    public static List<string> AllFields {
      get {
	return new List<string>(new string[] {
	  "id",
	  "type",
	  "list-icon-id",
	  "mp-cost",
	  "valid-targets",
	  "unknown-1",
	});
      }
    }

    public override List<string> GetAllFields() {
      return AbilityInfo2.AllFields;
    }

    #region Data Fields

    private ushort?      ID_;
    private AbilityType? Type_;
    private byte?        ListIconID_;
    private ushort?      MPCost_;
    private ushort?      Unknown1_;
    private ValidTarget? ValidTargets_;
    
    #endregion

    public override void Clear() {
      this.ID_           = null;
      this.Type_         = null;
      this.ListIconID_   = null;
      this.MPCost_       = null;
      this.Unknown1_     = null;
      this.ValidTargets_ = null;
    }

    #endregion

    #region Field Access

    public override bool HasField(string Field) {
      switch (Field) {
	// Nullables
	case "id":            return this.ID_.HasValue;
	case "list-icon-id":  return this.ListIconID_.HasValue;
	case "mp-cost":       return this.MPCost_.HasValue;
	case "type":          return this.Type_.HasValue;
	case "unknown-1":     return this.Unknown1_.HasValue;
	case "valid-targets": return this.ValidTargets_.HasValue;
	default:              return false;
      }
    }

    public override string GetFieldText(string Field) {
      switch (Field) {
	// Nullables - Simple Values
	case "id":            return (!this.ID_.HasValue           ? String.Empty : String.Format("{0}", this.ID_.Value));
	case "list-icon-id":  return (!this.ListIconID_.HasValue   ? String.Empty : String.Format("{0}", this.ListIconID_.Value));
	case "mp-cost":       return (!this.MPCost_.HasValue       ? String.Empty : String.Format("{0}", this.MPCost_.Value));
	case "type":          return (!this.Type_.HasValue         ? String.Empty : String.Format("{0}", this.Type_.Value));
	case "valid-targets": return (!this.ValidTargets_.HasValue ? String.Empty : String.Format("{0}", this.ValidTargets_.Value));
	// Nullables - Hex Values
	case "unknown-1":     return (!this.Unknown1_.HasValue     ? String.Empty : String.Format("{0:X4} ({0})", this.Unknown1_.Value));
	default:              return null;
      }
    }

    public override object GetFieldValue(string Field) {
      switch (Field) {
	// Nullables
	case "id":            return (!this.ID_.HasValue           ? null : (object) this.ID_.Value);
	case "list-icon-id":  return (!this.ListIconID_.HasValue   ? null : (object) this.ListIconID_.Value);
	case "mp-cost":       return (!this.MPCost_.HasValue       ? null : (object) this.MPCost_.Value);
	case "type":          return (!this.Type_.HasValue         ? null : (object) this.Type_.Value);
	case "unknown-1":     return (!this.Unknown1_.HasValue     ? null : (object) this.Unknown1_.Value);
	case "valid-targets": return (!this.ValidTargets_.HasValue ? null : (object) this.ValidTargets_.Value);
	default:              return null;
      }
    }

    protected override void LoadField(string Field, System.Xml.XmlElement Node) {
      switch (Field) {
	// "Simple" Fields
	case "id":            this.ID_           = (ushort)      this.LoadUnsignedIntegerField(Node); break;
	case "list-icon-id":  this.ListIconID_   = (byte)        this.LoadUnsignedIntegerField(Node); break;
	case "mp-cost":       this.MPCost_       = (ushort)      this.LoadUnsignedIntegerField(Node); break;
	case "type":          this.Type_         = (AbilityType) this.LoadHexField            (Node); break;
	case "unknown-1":     this.Unknown1_     = (ushort)      this.LoadUnsignedIntegerField(Node); break;
	case "valid-targets": this.ValidTargets_ = (ValidTarget) this.LoadHexField            (Node); break;
      }
    }

    #endregion

    #region ROM File Reading

    // Block Layout:
    // 000-001 U16 Index
    // 002-003 U16 List Icon ID (e.g. 40-47 for the elemental-colored dots)
    // 004-005 U16 MP Cost
    // 006-007 U16 Unknown (used to be the cooldown time)
    // 008-009 U16 Valid Targets
    // 00a-02e U8  Padding (NULs)
    // 02f-02f U8  End marker (0xff)
    public bool Read(BinaryReader BR) {
      this.Clear();
      try {
      byte[] Bytes = BR.ReadBytes(0x30);
	if (Bytes[0x9] != 0x00 || Bytes[0x2f] != 0xff)
	  return false;
	if (!FFXIEncryption.DecodeDataBlock(Bytes))
	  return false;
      FFXIEncoding E = new FFXIEncoding();
	BR = new BinaryReader(new MemoryStream(Bytes, false));
      } catch { return false; }
      this.ID_           = BR.ReadUInt16();
      this.Type_         = (AbilityType) BR.ReadByte();
      this.ListIconID_   = BR.ReadByte();
      this.MPCost_       = BR.ReadUInt16();
      this.Unknown1_     = BR.ReadUInt16();
      this.ValidTargets_ = (ValidTarget) BR.ReadUInt16();
#if DEBUG // Check the padding bytes for unexpected data
      for (byte i = 0; i < 37; ++i) {
      byte PaddingByte = BR.ReadByte();
	if (PaddingByte != 0)
	  Console.WriteLine("AbilityInfo2: Entry #{0}: Padding Byte #{1} is non-zero: {2:X2} ({2})", this.ID_, i + 1, PaddingByte);
      }
#endif
      BR.Close();
      return true;
    }

    #endregion

  }

}
