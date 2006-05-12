// $Id$

using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using PlayOnline.Core;

namespace PlayOnline.FFXI {

  public class FFXIResourceManager {

    private class ResourceString {

      public uint   ID;
      public string English;
      public string Japanese;

      public ResourceString(uint ID, string EN, string JP) {
	this.ID       = ID;
	this.English  = EN;
	this.Japanese = JP;
      }

      public ResourceString() : this(0, String.Empty, String.Empty) { }

    }

    private static bool         Initializing    = false;
    private static bool         Initialized     = false;
    private static SortedList   ResourceStrings = new SortedList();
    private static FFXIEncoding E               = null;

    public static string GetResourceString(uint ResourceID) {
      if (!FFXIResourceManager.Initialized && !FFXIResourceManager.Initializing)
	FFXIResourceManager.Init();
    ResourceString RS = FFXIResourceManager.ResourceStrings[ResourceID] as ResourceString;
      if (RS != null) {
	if (Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName == "ja")
	  return RS.Japanese;
	else
	  return RS.English;
      }
      return I18N.GetText("BadResID");
    }

    public static void Init() {
      lock (FFXIResourceManager.ResourceStrings) {
	if (FFXIResourceManager.Initializing || FFXIResourceManager.Initialized)
	  return;
	FFXIResourceManager.Initializing = true;
	if (FFXIResourceManager.E == null)
	  FFXIResourceManager.E = new FFXIEncoding();
      }
    PleaseWaitDialog PWD = new PleaseWaitDialog(I18N.GetText("PWD:StringResources"));
      PWD.StartPosition = FormStartPosition.CenterScreen;
      PWD.Show();
      try {
	Application.DoEvents();
	FFXIResourceManager.LoadItemNames("ROM/118/109.DAT");
	Application.DoEvents();
	FFXIResourceManager.LoadItemNames("ROM/118/106.DAT");
	Application.DoEvents();
	FFXIResourceManager.LoadItemNames("ROM/118/107.DAT");
	Application.DoEvents();
	FFXIResourceManager.LoadItemNames("ROM/118/110.DAT");
	Application.DoEvents();
	FFXIResourceManager.LoadItemNames("ROM/118/108.DAT");
	Application.DoEvents();
	FFXIResourceManager.LoadKeyItemNames("ROM/118/115.DAT", "ROM/118/113.DAT");
	Application.DoEvents();
	// These aren't "real" resource strings as such, but it's convenient to access them this way
	FFXIResourceManager.LoadStringTable ("ROM/97/48.DAT",  "ROM/97/30.DAT",  0x0001); // Region Names
	Application.DoEvents();
	FFXIResourceManager.LoadStringTable ("ROM/97/53.DAT",  "ROM/97/56.DAT",  0x0002); // Area Names
	Application.DoEvents();
	FFXIResourceManager.LoadStringTable ("ROM/97/55.DAT",  "ROM/97/29.DAT",  0x0003); // Job Names
	Application.DoEvents();
	FFXIResourceManager.LoadAbilityNames("ROM/119/55.DAT", "ROM/0/10.DAT", 0x0004); // Ability Names
	Application.DoEvents();
	FFXIResourceManager.LoadSpellNames  ("ROM/119/56.DAT", 0x0005);
	Application.DoEvents();
	// Needs to be at the end because it references some of the other tables
	FFXIResourceManager.LoadAutoTranslatorMessages();
	Application.DoEvents();
      } catch { /* FIXME: Any action needed? */ }
      PWD.Close();
      FFXIResourceManager.Initialized = true;
      FFXIResourceManager.Initializing = false;
    }

    public static bool IsValidResourceID(uint ResourceID) {
      if (!FFXIResourceManager.Initialized)
	FFXIResourceManager.Init();
      return (FFXIResourceManager.Initialized && (FFXIResourceManager.ResourceStrings[ResourceID] as ResourceString) != null);
    }

    private static void LoadAutoTranslatorMessages() {
      foreach (AutoTranslator.MessageGroup MG in AutoTranslator.Data) {
	foreach (AutoTranslator.Message M in MG.Messages)
	  FFXIResourceManager.ResourceStrings.Add(M.ResID, new ResourceString(M.ResID, M.Text, M.Text));
      }
    }

    private static void LoadAbilityNames(string EFile, string JFile, ushort Prefix) {
    FileType FT = new FileTypes.AbilityInfo();
    ThingList Contents = null;
      Contents = FT.Load(Path.Combine(POL.GetApplicationPath(AppID.FFXI), EFile));
      if (Contents != null) {
	foreach (IThing T in Contents) {
	uint ResID = (uint) ((Prefix << 16) + (ushort) T.GetFieldValue("id"));
	string Name = T.GetFieldText("name");
	  FFXIResourceManager.ResourceStrings.Add(ResID, new ResourceString(ResID, Name, Name));
	}
	Contents.Clear();
      }
      Contents = FT.Load(Path.Combine(POL.GetApplicationPath(AppID.FFXI), EFile));
      if (Contents != null) {
	foreach (IThing T in Contents) {
	uint ResID = (uint) ((Prefix << 16) + (ushort) T.GetFieldValue("id"));
	string Name = T.GetFieldText("name");
	ResourceString RS = FFXIResourceManager.ResourceStrings[ResID] as ResourceString;
	  if (RS == null)
	    FFXIResourceManager.ResourceStrings.Add(ResID, new ResourceString(ResID, Name, Name));
	  else
	    RS.Japanese = Name;
	}
	Contents.Clear();
      }
    }

    private static void LoadItemNames(string File) {
    FileType FT = new FileTypes.ItemData();
    ThingList Contents = null;
      Contents = FT.Load(Path.Combine(POL.GetApplicationPath(AppID.FFXI), File));
      if (Contents != null) {
	foreach (IThing T in Contents) {
	uint   ID    = (uint) T.GetFieldValue("id");
	string EName =        T.GetFieldText("english-name");
	string JName =        T.GetFieldText("japanese-name");
	  FFXIResourceManager.ResourceStrings.Add(0x07010000U + ID, new ResourceString(0x07010000U + ID, EName, JName));
	  FFXIResourceManager.ResourceStrings.Add(0x07020000U + ID, new ResourceString(0x07020000U + ID, EName, JName));
	}
	Contents.Clear();
      }
    }

    private static void LoadKeyItemNames(string EFile, string JFile) {
    FileType FT = new FileTypes.QuestInfo();
    ThingList Contents = null;
      Contents = FT.Load(Path.Combine(POL.GetApplicationPath(AppID.FFXI), EFile));
      if (Contents != null) {
	foreach (IThing T in Contents) {
	  if (T is QuestInfo && T.GetFieldText("category") == "sc_item_") {
	  uint   ID   = (uint) T.GetFieldValue("id");
	  string Name = T.GetFieldText("name-2");
	    FFXIResourceManager.ResourceStrings.Add(0x13010000U + ID, new ResourceString(0x13010000U + ID, Name, Name));
	    FFXIResourceManager.ResourceStrings.Add(0x13020000U + ID, new ResourceString(0x13020000U + ID, Name, Name));
	  }
	}
	Contents.Clear();
      }
      Contents = FT.Load(Path.Combine(POL.GetApplicationPath(AppID.FFXI), JFile));
      if (Contents != null) {
	foreach (IThing T in Contents) {
	  if (T is QuestInfo && T.GetFieldText("category") == "sc_item") {
	  uint   ID   = (uint) T.GetFieldValue("id");
	  string Name = T.GetFieldText("name-1");
	  ResourceString RS = null;
	    RS = FFXIResourceManager.ResourceStrings[0x13010000U + ID] as ResourceString;
	    if (RS == null)
	      FFXIResourceManager.ResourceStrings.Add(0x13010000U + ID, new ResourceString(0x13010000U + ID, Name, Name));
	    else
	      RS.Japanese = Name;
	    RS = FFXIResourceManager.ResourceStrings[0x13020000U + ID] as ResourceString;
	    if (RS == null)
	      FFXIResourceManager.ResourceStrings.Add(0x13020000U + ID, new ResourceString(0x13020000U + ID, Name, Name));
	    else
	      RS.Japanese = Name;
	  }
	}
	Contents.Clear();
      }
    }

    private static void LoadSpellNames(string File, ushort Prefix) {
    FileType FT = new FileTypes.SpellInfo();
    ThingList Contents = null;
      Contents = FT.Load(Path.Combine(POL.GetApplicationPath(AppID.FFXI), File));
      if (Contents != null) {
	foreach (IThing T in Contents) {
	  if (T is SpellInfo) {
	  uint ID = (ushort) T.GetFieldValue("id");
	    if (ID != 0) {
	      ID += (uint) (Prefix << 16);
	      FFXIResourceManager.ResourceStrings.Add(ID, new ResourceString(ID, T.GetFieldText("english-name"), T.GetFieldText("japanese-name")));
	    }
	  }
	}
	Contents.Clear();
      }
    }

    private static void LoadStringTable(string EFile, string JFile, ushort Prefix) {
    FileType FT = new FileTypes.XIStringTable();
    ThingList Contents = null;
      Contents = FT.Load(Path.Combine(POL.GetApplicationPath(AppID.FFXI), EFile));
      if (Contents != null) {
	foreach (IThing T in Contents) {
	uint ResID = (uint) ((Prefix << 16) + (uint) T.GetFieldValue("index"));
	string Text = T.GetFieldText("text");
	  FFXIResourceManager.ResourceStrings.Add(ResID, new ResourceString(ResID, Text, Text));
	}
	Contents.Clear();
      }
      Contents = FT.Load(Path.Combine(POL.GetApplicationPath(AppID.FFXI), EFile));
      if (Contents != null) {
	foreach (IThing T in Contents) {
	uint ResID = (uint) ((Prefix << 16) + (uint) T.GetFieldValue("index"));
	string Text = T.GetFieldText("text");
	ResourceString RS = FFXIResourceManager.ResourceStrings[ResID] as ResourceString;
	  if (RS == null)
	    FFXIResourceManager.ResourceStrings.Add(ResID, new ResourceString(ResID, Text, Text));
	  else
	    RS.Japanese = Text;
	}
	Contents.Clear();
      }
    }

  }

}