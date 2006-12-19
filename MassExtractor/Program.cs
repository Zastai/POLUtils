// $Id$

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

using PlayOnline.Core;
using PlayOnline.FFXI;
using PlayOnline.FFXI.Things;

namespace MassExtractor {

  internal class Program {

    private static void ExtractFile(int FileNumber, string OutputFile) {
    string ROMPath = FFXI.GetFilePath(FileNumber);
      if (ROMPath == null || !File.Exists(ROMPath)) {
	Console.ForegroundColor = ConsoleColor.Red;
	Console.WriteLine(I18N.GetText("BadFileID"), FileNumber, OutputFile);
	Console.ForegroundColor = ConsoleColor.White;
	return;
      }
      Program.ExtractFile(ROMPath, OutputFile);
    }

    private static void ExtractFile(string ROMPath, string OutputFile) {
      Console.Write(I18N.GetText("Extracting"), Path.GetFileName(OutputFile));
    ThingList KnownData = FileType.LoadAll(ROMPath, null);
      Console.ForegroundColor = ConsoleColor.White;
      Console.Write(I18N.GetText("Load"));
      if (KnownData != null) {
	Console.ForegroundColor = ConsoleColor.Green;
	Console.Write(I18N.GetText("OK"));
      bool SaveOK = KnownData.Save(OutputFile);
	Console.ForegroundColor = ConsoleColor.White;
	Console.Write(I18N.GetText("Save"));
	if (SaveOK) {
	  Console.ForegroundColor = ConsoleColor.Green;
	  Console.WriteLine(I18N.GetText("OK"));
	}
	else {
	  Console.ForegroundColor = ConsoleColor.Red;
	  Console.WriteLine(I18N.GetText("FAILED"));
	}
	KnownData.Clear();
      }
      else {
	Console.ForegroundColor = ConsoleColor.Red;
	Console.WriteLine(I18N.GetText("FAILED"));
      }
      Console.ForegroundColor = ConsoleColor.White;
    }

    [STAThread]
    static int Main() {
      Console.Title = I18N.GetText("ConsoleTitle");
      Console.ForegroundColor = ConsoleColor.White;
      try {
      string FFXIFolder = null;
	if (FFXIFolder == null) FFXIFolder = POL.GetApplicationPath(AppID.FFXI, POL.Region.Japan);
	if (FFXIFolder == null) FFXIFolder = POL.GetApplicationPath(AppID.FFXI, POL.Region.NorthAmerica);
	if (FFXIFolder == null) FFXIFolder = POL.GetApplicationPath(AppID.FFXI, POL.Region.Europe);
	if (FFXIFolder == null) {
	  Console.ForegroundColor = ConsoleColor.Red;
	  Console.WriteLine(I18N.GetText("NoFFXI"));
	  return 1;
	}
      string OutputFolder = null;
      bool   ScanAllFiles = false;
	{
	string[] Args = Environment.GetCommandLineArgs();
	  if (Args.Length > 1) {
	    OutputFolder = Args[1];
	    if (OutputFolder == "-full-scan") {
	      OutputFolder = null;
	      ScanAllFiles = true;
	      if (Args.Length > 2)
		OutputFolder = Args[2];
	    }
	  }
	  if (OutputFolder == null) {
	    Console.ForegroundColor = ConsoleColor.Red;
	    Console.WriteLine(I18N.GetText("Usage"), Args[0]);
	    return 1;
	  }
	  OutputFolder = Path.GetFullPath(OutputFolder);
	  if (!Directory.Exists(OutputFolder)) {
	    Console.WriteLine(I18N.GetText("Creating"), OutputFolder);
	    Directory.CreateDirectory(OutputFolder);
	  }
	}
      DateTime ExtractionStart = DateTime.Now;
	Console.WriteLine(I18N.GetText("SourceFolder"), FFXIFolder);
	Console.WriteLine(I18N.GetText("TargetFolder"), OutputFolder);
	Console.WriteLine(String.Format(I18N.GetText("StartTime"), ExtractionStart));
	Directory.SetCurrentDirectory(FFXIFolder);
	Console.Write(I18N.GetText("DumpFileTable"));
	{ // Dump File Table
	StreamWriter FileTable = new StreamWriter(Path.Combine(OutputFolder, "file-table.csv"), false, Encoding.UTF8);
	  FileTable.WriteLine("\"File ID\"\t\"ROMDir\"\t\"Dir\"\t\"File\"");
	  for (byte i = 1; i < 10; ++i) {
	  string Suffix = "";
	  string DataDir = FFXIFolder;
	    if (i > 1) {
	      Suffix = i.ToString();
	      DataDir = Path.Combine(DataDir, "Rom" + Suffix);
	    }
	  string VTableFile = Path.Combine(DataDir, String.Format("VTABLE{0}.DAT", Suffix));
	  string FTableFile = Path.Combine(DataDir, String.Format("FTABLE{0}.DAT", Suffix));
	    if (i == 1) // add the Rom now (not needed for the *TABLE.DAT, but needed for the other DAT paths)
	      DataDir = Path.Combine(DataDir, "Rom");
	    if (System.IO.File.Exists(VTableFile) && System.IO.File.Exists(FTableFile)) {
	      try {
	      BinaryReader VBR = new BinaryReader(new FileStream(VTableFile, FileMode.Open, FileAccess.Read, FileShare.Read));
	      BinaryReader FBR = new BinaryReader(new FileStream(FTableFile, FileMode.Open, FileAccess.Read, FileShare.Read));
	      long FileCount = VBR.BaseStream.Length;
		for (long j = 0; j < FileCount; ++j) {
		  if (VBR.ReadByte() == i) {
		    FBR.BaseStream.Seek(2 * j, SeekOrigin.Begin);
		  ushort FileDir = FBR.ReadUInt16();
		  byte Dir  = (byte) (FileDir / 0x80);
		  byte File = (byte) (FileDir % 0x80);
		    FileTable.WriteLine("{0}\t\"{1}\"\t\"{2}\"\t\"{3}\"", j, Path.GetFileName(DataDir), Dir.ToString(), Path.ChangeExtension(File.ToString(), ".dat"));
		  }
		}
		FBR.Close();
		VBR.Close();
	      }
	      catch { }
	    }
	  }
	  FileTable.Close();
	  Console.ForegroundColor = ConsoleColor.Green;
	  Console.WriteLine(I18N.GetText("OK"));
	  Console.ForegroundColor = ConsoleColor.White;
	}
	if (ScanAllFiles) {
	  for (int i = 1; i < 10; ++i) {
	  string ROMFolder = "Rom";
	    if (i > 1)
	      ROMFolder += i.ToString();
	    if (Directory.Exists(ROMFolder)) {
	      for (int j = 0; j < 1000; ++j) {
	      string ROMSubFolder = Path.Combine(ROMFolder, j.ToString());
		if (Directory.Exists(ROMSubFolder)) {
		  Console.WriteLine(I18N.GetText("Scanning"), ROMSubFolder);
		long Files      = 0;
		long KnownFiles = 0;
		  for (int k = 0; k < 128; ++k) {
		  string ROMFile = Path.Combine(ROMSubFolder, String.Format("{0}.DAT", k));
		    if (File.Exists(ROMFile)) {
		    ThingList KnownData = FileType.LoadAll(ROMFile, null);
		      if (KnownData != null && KnownData.Count > 0) {
			Console.WriteLine(I18N.GetText("ExtractingAll"), KnownData.Count, ROMFile);
			++KnownFiles;
		      ThingList<Graphic> Images = new ThingList<Graphic>();
		      ThingList NonImages = new ThingList();
			foreach (IThing T in KnownData) {
			  if (T is Graphic)
			    Images.Add(T as Graphic);
			  else
			    NonImages.Add(T);
			}
			KnownData.Clear();
			if (Images.Count == 1) {
			Image I = Images[0].GetFieldValue("image") as Image;
			  if (I != null) {
			  string Category  = Images[0].GetFieldText("category");
			  string ID        = Images[0].GetFieldText("id");
			  string ImageFile = String.Format("{0}-{1}-{2} - ({3}) {4}.png", i, j, k, Category, ID);
			    I.Save(Path.Combine(OutputFolder, ImageFile), ImageFormat.Png);
			  }
			}
			else if (Images.Count > 0) {
			string ImageFolder = Path.Combine(OutputFolder, String.Format("{0}-{1}-{2}", i, j, k));
			int    ImageIndex  = 0;
			  foreach (Graphic G in Images) {
			  Image I = G.GetFieldValue("image") as Image;
			    if (I != null) {
			      if (!Directory.Exists(ImageFolder))
				Directory.CreateDirectory(ImageFolder);
			    string Category  = G.GetFieldText("category");
			    string ID        = G.GetFieldText("id");
			    string ImageFile = String.Format("{0} - ({1}) {2}.png", ++ImageIndex, Category, ID);
			      I.Save(Path.Combine(ImageFolder, ImageFile), ImageFormat.Png);
			    }
			  }
			}
			Images.Clear();
			if (NonImages.Count > 0)
			  NonImages.Save(Path.Combine(OutputFolder, String.Format("{0}-{1}-{2}.xml", i, j, k)));
			NonImages.Clear();
		      }
		      ++Files;
		    }
		  }
		  Console.WriteLine(" => {0} of {1} file(s) contained recogniseable data", KnownFiles, Files);
		}
	      }
	    }
	  }
	}
	else { // Scan "known" files
	  // Interesting Data
	  Program.ExtractFile(73, Path.Combine(OutputFolder, "items-general.xml"));
	  Program.ExtractFile(74, Path.Combine(OutputFolder, "items-usable.xml"));
	  Program.ExtractFile(75, Path.Combine(OutputFolder, "items-weapons.xml"));
	  Program.ExtractFile(76, Path.Combine(OutputFolder, "items-armor.xml"));
	  Program.ExtractFile(77, Path.Combine(OutputFolder, "items-puppet.xml"));
	  // 78 = main UI image file (fonts, icons, ...)
	  // 79 = logon UI image file (logo, character creation text)
	  // 80 = ? (but initally loads like quests)
	  // 81 = ?
	  Program.ExtractFile(82, Path.Combine(OutputFolder, "quests.xml"));
	  // 83 = ?
	  Program.ExtractFile(84, Path.Combine(OutputFolder, "titles.xml"));
	  Program.ExtractFile(85, Path.Combine(OutputFolder, "abilities.xml"));
	  Program.ExtractFile(86, Path.Combine(OutputFolder, "spells.xml"));
	  Program.ExtractFile(87, Path.Combine(OutputFolder, "statuses.xml"));
	  // Dialog Tables
	  for (ushort i = 0; i < 0x100; ++i)
	    Program.ExtractFile(6420 + i, Path.Combine(OutputFolder, String.Format("dialog-table-{0:X2}.xml", i)));
	  // Mob Lists
	  for (ushort i = 0; i < 0x100; ++i)
	    Program.ExtractFile(6720 + i, Path.Combine(OutputFolder, String.Format("mob-list-{0:X2}.xml", i)));
	  // String Tables
	  Program.ExtractFile(55465, Path.Combine(OutputFolder, "area-names.xml"));
	  Program.ExtractFile(55466, Path.Combine(OutputFolder, "area-names-search.xml"));
	  Program.ExtractFile(55467, Path.Combine(OutputFolder, "job-names.xml"));
	  Program.ExtractFile(55468, Path.Combine(OutputFolder, "job-names-short.xml"));
	  Program.ExtractFile(55469, Path.Combine(OutputFolder, "race-names.xml"));
	  Program.ExtractFile(55470, Path.Combine(OutputFolder, "character-selection-strings.xml"));
	  Program.ExtractFile(55471, Path.Combine(OutputFolder, "equipment-locations.xml"));
	  // More String Tables
	  Program.ExtractFile(55645, Path.Combine(OutputFolder, "various-1.xml"));
	  Program.ExtractFile(55646, Path.Combine(OutputFolder, "error-messages.xml"));
	  Program.ExtractFile(55647, Path.Combine(OutputFolder, "pol-messages.xml"));
	  Program.ExtractFile(55648, Path.Combine(OutputFolder, "ingame-messages-1.xml"));
	  Program.ExtractFile(55649, Path.Combine(OutputFolder, "ingame-messages-2.xml"));
	  Program.ExtractFile(55650, Path.Combine(OutputFolder, "chat-filter-types.xml"));
	  Program.ExtractFile(55651, Path.Combine(OutputFolder, "menu-item-description.xml"));
	  Program.ExtractFile(55652, Path.Combine(OutputFolder, "menu-item-text.xml"));
	  Program.ExtractFile(55653, Path.Combine(OutputFolder, "various-2.xml"));
	  Program.ExtractFile(55654, Path.Combine(OutputFolder, "region-names.xml"));
	  // 55655-55656 are assigned to a JP string table (Rom\97\56.dat)
	  Program.ExtractFile(55657, Path.Combine(OutputFolder, "weather-types.xml"));
	  Program.ExtractFile(55658, Path.Combine(OutputFolder, "day-names.xml"));
	  Program.ExtractFile(55659, Path.Combine(OutputFolder, "directions.xml"));
	  Program.ExtractFile(55660, Path.Combine(OutputFolder, "moon-phases.xml"));
	  Program.ExtractFile(55661, Path.Combine(OutputFolder, "area-names-alternate.xml"));
	  // 55662-55664 are assigned to a JP string table (Rom\97\56.dat)
	}
      DateTime ExtractionEnd = DateTime.Now;
	Console.WriteLine(String.Format(I18N.GetText("EndTime"), ExtractionEnd));
	Console.WriteLine(String.Format(I18N.GetText("ElapsedTime"), ExtractionEnd - ExtractionStart));
	return 0;
      }
      catch (Exception E) {
	Console.ForegroundColor = ConsoleColor.Red;
	Console.WriteLine(I18N.GetText("Exception"), E.Message);
	return 1;
      }
      finally { Console.ResetColor(); }
    }

  }

}
