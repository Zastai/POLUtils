using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Xml;

using PlayOnline.Core;

namespace PlayOnline.FFXI {

  public class Macro {

    public Macro() : this(String.Empty) { }

    public Macro(string Name) {
      this.Name_ = Name;
      this.Commands_ = new string[6];
    }

    public void Clear() {
      this.Name_ = String.Empty;
      this.Commands_ = new string[6];
    }

    public Macro Clone() {
    Macro M = new Macro(this.Name_);
      for (int i = 0; i < 6; ++i)
	M.Commands_[i] = this.Commands_[i];
      return M;
    }

    #region Properties

    public string Name {
      get { return this.Name_;  }
      set { this.Name_ = value; }
    }

    public string[] Commands {
      get { return this.Commands_; }
    }

    public bool Empty {
      get {
	if (this.Name_ != null && this.Name_ != String.Empty)
	  return false;
	foreach (string Command in this.Commands_) {
	  if (Command != null && Command != String.Empty)
	    return false;
	}
	return true;
      }
    }

    #region Private Data

    private string   Name_;
    private string[] Commands_;

    #endregion

    #endregion

    #region MacroBar Access

    internal static Macro ReadFromMacroBar(BinaryReader BR) {
    Macro M = new Macro();
      if (BR != null) {
      Encoding E = new FFXIEncoding();
	BR.ReadInt32(); // Unknown
	for (int i = 0; i < 6; ++i) { // 6 Lines of text, 61 bytes each, null-terminated shift-jis
	string Command = "";
	  Command = E.GetString(BR.ReadBytes(61));
	  M.Commands_[i] = Command.TrimEnd('\0');
	}
	M.Name_ = E.GetString(BR.ReadBytes(10)).TrimEnd('\0');
      }
      return M;
    }

    internal void WriteToMacroBar(BinaryWriter BW) {
    Encoding E = new FFXIEncoding();
      BW.Write((uint) 0);
      for (int i = 0; i < 6; ++i) // 6 Lines of text, 61 bytes each, nul-terminated shift-jis
	this.WriteEncodedString(BW, this.Commands_[i], E, 61);
      this.WriteEncodedString(BW, this.Name_, E, 10);
    }

    private void WriteEncodedString(BinaryWriter BW, string Text, Encoding E, int Bytes) {
    ArrayList OutBytes = new ArrayList(Bytes);
      OutBytes.AddRange(E.GetBytes(Text));
      while (OutBytes.Count > Bytes)
	OutBytes.RemoveAt(Bytes);
      while (OutBytes.Count < Bytes)
	OutBytes.Add((byte) 0);
      BW.Write((byte[]) OutBytes.ToArray(typeof(byte)));
    }

    #endregion

    #region XML Access

    internal static Macro LoadFromXml(XmlElement MacroNode) {
    Macro M = new Macro();
      if (MacroNode.Attributes["name"] != null)
	M.Name_ = MacroNode.Attributes["name"].InnerText;
      for (int i = 0; i < 6; ++i) {
      XmlNode CommandNode = MacroNode.SelectSingleNode(String.Format("command[@line = {0}]", i + 1));
	if (CommandNode != null && CommandNode is XmlElement) {
	string CommandText = String.Empty;
	  foreach (XmlNode XN in CommandNode.ChildNodes) {
	    if (XN is XmlText)
	      CommandText += XN.InnerText;
	    else if (XN is XmlElement && XN.Name == "autotrans") {
	    ushort Category = 0; try { XmlAttribute XCat   = XN.Attributes["category"]; Category = ushort.Parse(XCat.InnerText);   } catch {}
	    byte   Group    = 0; try { XmlAttribute XGroup = XN.Attributes["group"];    Group    =   byte.Parse(XGroup.InnerText); } catch {}
	    byte   ID       = 0; try { XmlAttribute XID    = XN.Attributes["id"];       ID       =   byte.Parse(XID.InnerText);    } catch {}
	      CommandText += String.Format("\x00AB{0}/{1}/{2}: {3}\x00BB", Category, Group, ID, AutoTranslator.GetMessage(Group, ID, Category));
	    }
	  }
	  M.Commands_[i] = CommandText;
	}
      }
      return M;
    }

    internal void WriteToXml(XmlDocument XDoc, XmlNode Parent) {
    XmlElement XMacro = XDoc.CreateElement("macro");
      if (this.Name_ != null && this.Name_ != String.Empty) {
      XmlAttribute XName = XDoc.CreateAttribute("name");
	XName.InnerText = this.Name_;
	XMacro.Attributes.Append(XName);
      }
      for (int i = 0; i < 6; ++i) {
	if (this.Commands_[i] != null && this.Commands_[i] != String.Empty) {
	XmlElement XCommand = XDoc.CreateElement("command");
	  {
	  XmlAttribute XLine = XDoc.CreateAttribute("line");
	    XLine.InnerText = String.Format("{0}", i + 1);
	    XCommand.Attributes.Append(XLine);
	  }
	string CommandText = this.Commands_[i];
	int MarkerPos = CommandText.IndexOf('\x00AB', 0);
	  while (MarkerPos >= 0) {
	  int EndMarkerPos = CommandText.IndexOf('\x00BB', MarkerPos);
	  int NextMarkerPos = CommandText.IndexOf('\x00AB', MarkerPos + 1);
	    if (EndMarkerPos > 0 && (NextMarkerPos < 0 || NextMarkerPos > EndMarkerPos)) { // Parse Contents
	      if (MarkerPos > 0) // Save preceding text (if any)
		XCommand.AppendChild(XDoc.CreateTextNode(CommandText.Substring(0, MarkerPos)));
	    string Marker = CommandText.Substring(MarkerPos + 1, (EndMarkerPos - MarkerPos - 1));
	      CommandText = CommandText.Substring(EndMarkerPos + 1);
	    XmlElement XAutoTrans = null;
	    string[] MarkerAndText = Marker.Split(new char[] { ':' }, 2);
	      if (MarkerAndText.Length == 2) {
	      string[] MarkerValues = MarkerAndText[0].Split('/');
		if (MarkerValues.Length == 3) {
		XAutoTrans = XDoc.CreateElement("autotrans");
		  XAutoTrans.InnerText = MarkerAndText[1].Substring(1); // skip space after the ':'
		  { // add 'category' attribute
		  XmlAttribute XATCategory = XDoc.CreateAttribute("category");
		    XATCategory.InnerText = MarkerValues[0];
		    XAutoTrans.Attributes.Append(XATCategory);
		  }
		  { // add 'group' attribute
		  XmlAttribute XATLang = XDoc.CreateAttribute("group");
		    XATLang.InnerText = MarkerValues[1];
		    XAutoTrans.Attributes.Append(XATLang);
		  }
		  { // add 'id' attribute
		  XmlAttribute XATLang = XDoc.CreateAttribute("id");
		    XATLang.InnerText = MarkerValues[2];
		    XAutoTrans.Attributes.Append(XATLang);
		  }
		}
	      }
	      if (XAutoTrans == null) // invalid marker
		XCommand.AppendChild(XDoc.CreateTextNode(Marker));
	      else
		XCommand.AppendChild(XAutoTrans);
	    }
	    else { // Not a valid autotrans marker
	      XCommand.AppendChild(XDoc.CreateTextNode(CommandText.Substring(0, MarkerPos + 1)));
	      CommandText = CommandText.Substring(MarkerPos + 1);
	    }
	    MarkerPos = CommandText.IndexOf('\x00AB', 0);
	  }
	  if (CommandText != String.Empty)
	    XCommand.AppendChild(XDoc.CreateTextNode(CommandText));
	  XMacro.AppendChild(XCommand);
	}
      }
      Parent.AppendChild(XMacro);
    }

    #endregion

  }

}
