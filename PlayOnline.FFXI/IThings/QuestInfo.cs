// $Id$

using System;
using System.Collections.Generic;
using System.IO;

using PlayOnline.Core;

namespace PlayOnline.FFXI {

  public class QuestInfo : Thing {

    public QuestInfo() {
      // Clear fields
      this.Clear();
    }

    public override string ToString() {
      return String.Format("{0} ({1})", this.Name1_, this.Name2_);
    }

    public override List<PropertyPages.IThing> GetPropertyPages() {
      return base.GetPropertyPages();
    }

    #region Fields

    public static List<string> AllFields {
      get {
	return new List<string>(new string[] {
	  "category",
	  "id",
	  "name-1",
	  "name-2",
	  "description",
	});
      }
    }

    public override List<string> GetAllFields() {
      return QuestInfo.AllFields;
    }

    #region Data Fields

    private string        Category_;
    private Nullable<int> ID_;
    private string        Name1_;
    private string        Name2_;
    private string        Description_;
    
    #endregion

    public override void Clear() {
      this.Category_    = null;
      this.ID_          = null;
      this.Name1_       = null;
      this.Name2_       = null;
      this.Description_ = null;
    }

    #endregion

    #region Field Access

    public override bool HasField(string Field) {
      switch (Field) {
	// Objects
	case "category":    return (this.Category_    != null);
	case "description": return (this.Description_ != null);
	case "name1":       return (this.Name1_       != null);
	case "name2":       return (this.Name2_       != null);
	// Nullables
	case "id":          return this.ID_.HasValue;
	default:            return false;
      }
    }

    public override string GetFieldText(string Field) {
      if (!this.HasField(Field))
	return String.Empty;
      switch (Field) {
	// Objects
	case "category":    return this.Category_;
	case "description": return this.Description_;
	case "name1":       return this.Name1_;
	case "name2":       return this.Name2_;
	// Nullables
	case "id":          return String.Format("{0}", this.ID_.Value);
	default:            return null;
      }
    }

    public override object GetFieldValue(string Field) {
      if (!this.HasField(Field))
	return null;
      switch (Field) {
	// Objects
	case "category":    return this.Category_;
	case "description": return this.Description_;
	case "name1":       return this.Name1_;
	case "name2":       return this.Name2_;
	// Nullables
	case "id":          return (object) this.ID_.Value;
	default:            return null;
      }
    }

    protected override void LoadField(string Field, System.Xml.XmlElement Node) {
      switch (Field) {
	// "Simple" Fields
	case "category":    this.Category_    =       this.LoadTextField         (Node); break;
	case "description": this.Description_ =       this.LoadTextField         (Node); break;
	case "id":          this.ID_          = (int) this.LoadSignedIntegerField(Node); break;
	case "name1":       this.Name1_       =       this.LoadTextField         (Node); break;
	case "name2":       this.Name2_       =       this.LoadTextField         (Node); break;
      }
    }

    #endregion

    #region ROM File Reading

    public bool Read(BinaryReader BR, string Category, long MenuStart) {
      this.Clear();
      this.Category_ = Category;
      try {
	this.ID_      = BR.ReadInt32();
      long Name1Start = BR.ReadInt32();
      long Name2Start = BR.ReadInt32();
      long BodyStart  = BR.ReadInt32();
	 // Unknown (BodyEnd? not likely as it does not match "BodyStart + 4 * (1 + LineCount)"
      long Unknown    = BR.ReadInt32();
	if (Name1Start < 0 || Name2Start < 0 || BodyStart < 0 || Unknown < 0)
	  return false;
      FFXIEncoding E = new FFXIEncoding();
      long CurPos = BR.BaseStream.Position;
	BR.BaseStream.Seek(MenuStart + Name1Start, SeekOrigin.Begin);
	this.Name1_ = FFXIEncryption.ReadEncodedString(BR, E);
	BR.BaseStream.Seek(MenuStart + Name2Start, SeekOrigin.Begin);
	this.Name2_ = FFXIEncryption.ReadEncodedString(BR, E);
	BR.BaseStream.Seek(MenuStart + BodyStart, SeekOrigin.Begin);
       int LineCount = BR.ReadInt32();
	if (LineCount < 0) {
	  BR.BaseStream.Seek(CurPos, SeekOrigin.Begin);
	  return false;
	}
	{ // Read entry description lines
	long[] LineStart = new long[LineCount];
	  for (int i = 0; i < LineCount; ++i) {
	    LineStart[i] = BR.ReadInt32();
	    if (LineStart[i] < 0) {
	      BR.BaseStream.Seek(CurPos, SeekOrigin.Begin);
	      return false;
	    }
	  }
	  this.Description_ = String.Empty;
	  for (int i = 0; i < LineCount; ++i) {
	    BR.BaseStream.Seek(MenuStart + LineStart[i], SeekOrigin.Begin);
	    if (this.Description_ != String.Empty)
	      this.Description_ += "\r\n";
	    this.Description_ += FFXIEncryption.ReadEncodedString(BR, E);
	  }
	}
	BR.BaseStream.Seek(CurPos, SeekOrigin.Begin);
	return true;
      } catch { return false; }
    }

    #endregion

  }

}
