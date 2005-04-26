using System;
using System.Collections;
using System.IO;
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
      PWD.Close();
      FFXIResourceManager.Initialized = true;
      FFXIResourceManager.Initializing = false;
    }

    private static void LoadAutoTranslatorMessages() {
      foreach (AutoTranslator.MessageGroup MG in AutoTranslator.Data) {
	foreach (AutoTranslator.Message M in MG.Messages) {
	uint ResID = (uint) ((uint) ((M.Category & 0xff) << 24) + ((M.Category & 0xff00) << 8) + (ushort) (M.ParentGroup << 8) + M.ID);
	  FFXIResourceManager.ResourceStrings.Add(ResID, new ResourceString(ResID, M.Text, M.Text));
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

    public static string GetResourceString(uint ResourceID) {
      if (!FFXIResourceManager.Initialized)
	FFXIResourceManager.Init();
      if (FFXIResourceManager.Initialized) {
      ResourceString RS = FFXIResourceManager.ResourceStrings[ResourceID] as ResourceString;
	if (RS != null) {
	  if (POL.SelectedRegion == POL.Region.Japan)
	    return RS.Japanese;
	  else
	    return RS.English;
	}
      }
      return "Unknown String Resource";
    }

  }

}