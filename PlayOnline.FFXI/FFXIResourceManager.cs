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
      Application.DoEvents();
      FFXIResourceManager.LoadAutoTranslatorMessages();
      Application.DoEvents();
      FFXIResourceManager.LoadItemNames("ROM/118/109.DAT", ItemDataLanguage.English, ItemDataType.Armor);
      Application.DoEvents();
      FFXIResourceManager.LoadItemNames("ROM/118/106.DAT", ItemDataLanguage.English, ItemDataType.Object);
      Application.DoEvents();
      FFXIResourceManager.LoadItemNames("ROM/118/107.DAT", ItemDataLanguage.English, ItemDataType.Object);
      Application.DoEvents();
      FFXIResourceManager.LoadItemNames("ROM/118/110.DAT", ItemDataLanguage.English, ItemDataType.Object);
      Application.DoEvents();
      FFXIResourceManager.LoadItemNames("ROM/118/108.DAT", ItemDataLanguage.English, ItemDataType.Weapon);
      Application.DoEvents();
      // These aren't "real" resource strings as such, but it's convenient to access them this way
      FFXIResourceManager.LoadStringTable("ROM/97/48.DAT", "ROM/97/30.DAT", 0x0001); // Region Names
      Application.DoEvents();
      PWD.Close();
      FFXIResourceManager.Initialized = true;
      FFXIResourceManager.Initializing = false;
    }

    private static void LoadAutoTranslatorMessages() {
      foreach (AutoTranslator.MessageGroup MG in AutoTranslator.Data) {
	foreach (AutoTranslator.Message M in MG.Messages) {
	uint ResID = (uint) ((M.ParentGroup << 8) + M.ID);
	  {
	  ResourceString RS = FFXIResourceManager.ResourceStrings[0x2020000 + ResID] as ResourceString;
	    if (RS == null)
	      FFXIResourceManager.ResourceStrings.Add(0x2020000 + ResID, new ResourceString(0x2020000 + ResID, M.Text, M.Text));
	    else if (M.Category == 0x0202)
	      RS.English = M.Text;
	    else
	      RS.Japanese = M.Text;
	  }
	  {
	  ResourceString RS = FFXIResourceManager.ResourceStrings[0x1040000 + ResID] as ResourceString;
	    if (RS == null)
	      FFXIResourceManager.ResourceStrings.Add(0x1040000 + ResID, new ResourceString(0x1040000 + ResID, M.Text, M.Text));
	    else if (M.Category == 0x0202)
	      RS.English = M.Text;
	    else
	      RS.Japanese = M.Text;
	  }
	}
      }
    }

    private static void LoadItemNames(string File, ItemDataLanguage L, ItemDataType T) {
    BinaryReader BR = new BinaryReader(new FileStream(Path.Combine(POL.GetApplicationPath(AppID.FFXI), File), FileMode.Open, FileAccess.Read));
      while (BR.BaseStream.Position < BR.BaseStream.Length) {
      byte[] ItemData = BR.ReadBytes(0x200);
	for (int i = 0; i < 0x200; ++i)
	  ItemData[i] = (byte) ((ItemData[i] << 3) | (ItemData[i] >> 5));
      BinaryReader IBR = new BinaryReader(new MemoryStream(ItemData, false));
      uint ResID = 0x07020000U + IBR.ReadUInt32();
	switch (T) {
	  case ItemDataType.Armor:  IBR.BaseStream.Seek(0x14, SeekOrigin.Current); break;
	  case ItemDataType.Object: IBR.BaseStream.Seek(0x0A, SeekOrigin.Current); break;
	  case ItemDataType.Weapon: IBR.BaseStream.Seek(0x1A, SeekOrigin.Current); break;
	}
      string JP = E.GetString(IBR.ReadBytes(22)).TrimEnd('\0');
      string EN = E.GetString(IBR.ReadBytes(22)).TrimEnd('\0');
	IBR.Close();
	FFXIResourceManager.ResourceStrings.Add(ResID, new ResourceString(ResID, EN, JP));
	BR.BaseStream.Seek(0xa00, SeekOrigin.Current); // Skip icon
      }
      BR.Close();
    }

    // TODO: Factor out XIString file load (in common with databrowser)
    private static void LoadStringTable(string FileE, string FileJ, ushort Prefix) {
    Encoding E = new FFXIEncoding();
      { // Read English version
      BinaryReader BR = new BinaryReader(new FileStream(Path.Combine(POL.GetApplicationPath(AppID.FFXI), FileE), FileMode.Open, FileAccess.Read));
	if ((E.GetString(BR.ReadBytes(10)) != "XISTRING".PadRight(10, '\0')) || BR.ReadUInt16() != 2)
	  return;
	foreach (byte B in BR.ReadBytes(20)) {
	  if (B != 0)
	    return;
	}
      uint FileSize = BR.ReadUInt32();
	if (FileSize != BR.BaseStream.Length)
	  return;
      uint EntryCount = BR.ReadUInt32();
      uint EntryBytes = BR.ReadUInt32();
      uint DataBytes  = BR.ReadUInt32();
	BR.ReadUInt32(); // Unknown
	BR.ReadUInt32(); // Unknown
	if (EntryBytes != EntryCount * 12 || FileSize != 0x38 + EntryBytes + DataBytes)
	  return;
	for (int i = 0; i < EntryCount; ++i) {
	  BR.BaseStream.Seek(0x38 + (12 * i), SeekOrigin.Begin);
	uint  Offset = BR.ReadUInt32();
	short Size = BR.ReadInt16();
	string Text = String.Empty;
	  BR.ReadUInt16(); // Unknown (0 or 1; so probably a flag of some sort)
	  BR.ReadUInt32(); // Unknown
	  if (Size > 0 && Offset + Size <= DataBytes) {
	    BR.BaseStream.Seek(0x38 + EntryBytes + Offset, SeekOrigin.Begin);
	    Text = E.GetString(BR.ReadBytes(Size)).TrimEnd('\0');
	  }
	  else
	    Text = I18N.GetText("InvalidEntry");
	ResourceString RS = new ResourceString((uint) ((Prefix << 16) + i), Text, Text);
	  FFXIResourceManager.ResourceStrings.Add(RS.ID, RS);
	}
      }
      { // Read Japanese version
      BinaryReader BR = new BinaryReader(new FileStream(Path.Combine(POL.GetApplicationPath(AppID.FFXI), FileJ), FileMode.Open, FileAccess.Read));
	if ((E.GetString(BR.ReadBytes(10)) != "XISTRING".PadRight(10, '\0')) || BR.ReadUInt16() != 2)
	  return;
	foreach (byte B in BR.ReadBytes(20)) {
	  if (B != 0)
	    return;
	}
      uint FileSize = BR.ReadUInt32();
	if (FileSize != BR.BaseStream.Length)
	  return;
      uint EntryCount = BR.ReadUInt32();
      uint EntryBytes = BR.ReadUInt32();
      uint DataBytes  = BR.ReadUInt32();
	BR.ReadUInt32(); // Unknown
	BR.ReadUInt32(); // Unknown
	if (EntryBytes != EntryCount * 12 || FileSize != 0x38 + EntryBytes + DataBytes)
	  return;
	for (int i = 0; i < EntryCount; ++i) {
	  BR.BaseStream.Seek(0x38 + (12 * i), SeekOrigin.Begin);
	uint  Offset = BR.ReadUInt32();
	short Size = BR.ReadInt16();
	string Text = String.Empty;
	  BR.ReadUInt16(); // Unknown (0 or 1; so probably a flag of some sort)
	  BR.ReadUInt32(); // Unknown
	  if (Size > 0 && Offset + Size <= DataBytes) {
	    BR.BaseStream.Seek(0x38 + EntryBytes + Offset, SeekOrigin.Begin);
	    Text = E.GetString(BR.ReadBytes(Size)).TrimEnd('\0');
	  }
	  else
	    Text = I18N.GetText("InvalidEntry");
	uint ResID = (uint) ((Prefix << 16) + i);
	ResourceString RS = FFXIResourceManager.ResourceStrings[ResID] as ResourceString;
	  if (RS == null)
	    FFXIResourceManager.ResourceStrings.Add(ResID, new ResourceString(ResID, Text, Text));
	  else
	    RS.Japanese = Text;
	}
      }
    }

    public static string GetResourceString(uint ResourceID) {
      if (!FFXIResourceManager.Initialized)
	FFXIResourceManager.Init();
      if (FFXIResourceManager.Initialized) {
      ResourceString RS = FFXIResourceManager.ResourceStrings[ResourceID] as ResourceString;
	if (RS != null) {
	  if (Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName == "ja")
	    return RS.Japanese;
	  else
	    return RS.English;
	}
      }
      return I18N.GetText("BadResID");
    }

  }

}