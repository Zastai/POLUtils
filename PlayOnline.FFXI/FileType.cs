using System;
using System.Collections.Generic;

using PlayOnline.FFXI.FileTypes;

namespace PlayOnline.FFXI {

  public abstract class FileType {

    public abstract ThingList Load(string FileName);

    public static List<FileType> AllTypes {
      get {
	if (FileType.AllTypes_ == null) {
	  FileType.AllTypes_ = new List<FileType>();
	  FileType.AllTypes_.Add(new ItemData());
	}
	return FileType.AllTypes_;
      }
    }
    private static List<FileType> AllTypes_;

  }

}