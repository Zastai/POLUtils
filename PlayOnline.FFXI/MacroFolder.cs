using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

using PlayOnline.Core;

namespace PlayOnline.FFXI {

  public class MacroFolder {

    // Folder Creation

    public MacroFolder() : this(null) {
    }

    public MacroFolder(string Name) {
      this.Name_    = Name;
      this.Folders_ = new MacroFolderCollection();
      this.Macros_  = new MacroCollection();
      this.Locked_  = false;
    }

    public MacroFolder Clone() {
    MacroFolder MF = new MacroFolder(this.Name_);
      foreach (MacroFolder SubFolder in this.Folders_)
	MF.Folders_.Add(SubFolder.Clone());
      foreach (Macro M in this.Macros_)
	MF.Macros_.Add(M.Clone());
      MF.Locked_ = this.Locked_;
      return MF;
    }

    #region MacroBar Access

    public static MacroFolder LoadFromMacroBar(string PathName) {
    MacroFolder MF = new MacroFolder();
      MF.Locked_ = true;
      {
      MacroFolder TopBar = new MacroFolder("Top Bar (Control)");
	TopBar.Locked_ = true;
	MF.Folders_.Add(TopBar);
      }
      {
      MacroFolder BottomBar = new MacroFolder("Bottom Bar (Alt)");
	BottomBar.Locked_ = true;
	MF.Folders_.Add(BottomBar);
      }
    BinaryReader BR = null;
      if (File.Exists(PathName)) {
	try {
	FileStream MacroBarFile = new FileStream(PathName, FileMode.Open, FileAccess.Read, FileShare.Read);
	  BR = new BinaryReader(MacroBarFile, Encoding.ASCII);
	}
	catch { }
	if (BR != null && (BR.BaseStream.Length != 7624 || BR.ReadUInt32() != 1)) {
	  BR.Close();
	  BR = null;
	}
	else { // Read past header
	  BR.ReadUInt32(); // Unknown - sometimes zero, sometimes 0x80000000
	byte[] StoredMD5 = BR.ReadBytes(16); // MD5 Checksum of the rest of the data
	  {
	  byte[] Data = BR.ReadBytes(7600);
	    BR.BaseStream.Seek(-7600, SeekOrigin.Current);
	  MD5 Hash = new MD5CryptoServiceProvider();
	  byte[] ComputedMD5 = Hash.ComputeHash(Data);
	    for (int i = 0; i < 16; ++i) {
	      if (StoredMD5[i] != ComputedMD5[i]) {
		Console.WriteLine("MD5 Checksum failure for {0}:", PathName);
		Console.Write("- Stored Hash  :");
		for (int j = 0; j < 16; ++j)
		  Console.Write(" {0,2:X}", StoredMD5[j]);
		Console.WriteLine();
		Console.Write("- Computed Hash:");
		for (int j = 0; j < 16; ++j)
		  Console.Write(" {0,2:X}", ComputedMD5[j]);
		Console.WriteLine();
		break;
	      }
	    }
	  }
	}
      }
      for (int i = 0; i < 2; ++i) {
	for (int j = 0; j < 10; ++j)
	  MF.Folders_[i].Macros_.Add((BR != null) ? Macro.ReadFromMacroBar(BR) : new Macro());
      }
      if (BR != null)
	BR.Close();
      return MF;
    }

    public bool WriteToMacroBar(string PathName) {
      try {
      byte[] MacroBytes = new byte[7600];
	{ // Since we need the MD5 hash, we use a MemoryStream first
	MemoryStream MS = new MemoryStream(MacroBytes, true);
	BinaryWriter BW = new BinaryWriter(MS);
	  this.WriteToMacroBar(BW);
	  BW.Close();
	}
	{
	BinaryWriter BW = new BinaryWriter(new FileStream(PathName, FileMode.OpenOrCreate, FileAccess.Write));
	MD5 Hash = new MD5CryptoServiceProvider();
	  BW.Write((uint) 1);
	  BW.Write((uint) 0); // TODO: Figure out when to write 0x80000000
	  BW.Write(Hash.ComputeHash(MacroBytes));
	  BW.Write(MacroBytes);
	  BW.Close();
	}
	return true;
      } catch (Exception E) { Console.WriteLine(E.ToString()); }
      return false;
    }

    private void WriteToMacroBar(BinaryWriter BW) {
      foreach (MacroFolder MF in this.Folders_)
	MF.WriteToMacroBar(BW);
      foreach (Macro M in this.Macros_)
	M.WriteToMacroBar(BW);
    }

    #endregion

    #region XML Access

    public static MacroFolder LoadFromXml(string FileName) {
    XmlDocument XD = new XmlDocument();
      XD.Load(FileName);
      return MacroFolder.LoadFromXml(XD.DocumentElement);
    }

    private static MacroFolder LoadFromXml(XmlElement FolderNode) {
    MacroFolder MF = new MacroFolder();
      if (FolderNode.Attributes["name"] != null)
	MF.Name_ = FolderNode.Attributes["name"].InnerText;
      { // Load contained macros
      XmlNodeList Macros = FolderNode.SelectNodes("macro");
	foreach (XmlNode MacroNode in Macros) {
	  if (MacroNode is XmlElement)
	    MF.Macros.Add(Macro.LoadFromXml(MacroNode as XmlElement));
	}
      }
      { // Load contained folders
      XmlNodeList SubFolders = FolderNode.SelectNodes("folder");
	foreach (XmlNode SubFolderNode in SubFolders) {
	  if (SubFolderNode is XmlElement)
	    MF.Folders_.Add(MacroFolder.LoadFromXml(SubFolderNode as XmlElement));
	}
      }
      return MF;
    }

    public bool WriteToXml(string PathName) {
      try {
      XmlDocument XDoc = new XmlDocument();
	XDoc.PreserveWhitespace = true;
	XDoc.AppendChild(XDoc.CreateXmlDeclaration("1.0", "utf-8", null));
	XDoc.AppendChild(XDoc.CreateComment("NOTE: Editing this file by hand is NOT recommended"));
	this.WriteToXml(XDoc, XDoc);
      XmlTextWriter XW = new XmlTextWriter(PathName, Encoding.UTF8);
	XW.Formatting = Formatting.Indented;
	XDoc.WriteTo(XW);
	XW.Close();
	return true;
      } catch (Exception E) { Console.WriteLine(E.ToString()); }
      return false;
    }

    private void WriteToXml(XmlDocument XDoc, XmlNode Parent) {
    XmlElement XFolder = XDoc.CreateElement("folder");
      if (this.Name_ != null && this.Name_ != String.Empty) {
      XmlAttribute XName = XDoc.CreateAttribute("name");
	XName.InnerText = this.Name_;
	XFolder.Attributes.Append(XName);
      }
      foreach (MacroFolder MF in this.Folders_)
	MF.WriteToXml(XDoc, XFolder);
      foreach (Macro M in this.Macros_)
	M.WriteToXml(XDoc, XFolder);
      Parent.AppendChild(XFolder);
    }

    #endregion

    #region Data Members

    public string Name {
      get { return this.Name_;  }
      set { this.Name_ = value; }
    }

    public MacroFolderCollection Folders { get { return this.Folders_; } }
    public MacroCollection       Macros  { get { return this.Macros_;  } }

    // If true, folders or macros can neither be added or removed
    public bool Locked { get { return this.Locked_; } }

    #region Private Fields

    private string                Name_;
    private MacroFolderCollection Folders_;
    private MacroCollection       Macros_;
    private bool                  Locked_;

    #endregion

    #endregion

  }

}
