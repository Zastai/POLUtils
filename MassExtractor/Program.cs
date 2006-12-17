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

    private static void ExtractFile(string ROMFile, string OutputFile) {
      Console.Write(I18N.GetText("Extracting"), Path.GetFileName(OutputFile));
    ThingList KnownData = FileType.LoadAll(ROMFile, null);
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
	  FileTable.WriteLine("\"File ID\",\"Path\"");
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
		    FileTable.WriteLine("{0},\"{1}\"", j, Path.Combine(DataDir, Path.Combine(Dir.ToString(), Path.ChangeExtension(File.ToString(), ".dat"))));
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
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/119/55.DAT"),   Path.Combine(OutputFolder, "abilities.xml"));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/165/83.DAT"),   Path.Combine(OutputFolder, "area-names-alternate.xml"));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/165/85.DAT"),   Path.Combine(OutputFolder, "area-names-search.xml"));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/165/84.DAT"),   Path.Combine(OutputFolder, "area-names.xml"));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/165/74.DAT"),   Path.Combine(OutputFolder, "chat-filter-types.xml"));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/165/80.DAT"),   Path.Combine(OutputFolder, "day-names.xml"));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/165/81.DAT"),   Path.Combine(OutputFolder, "directions.xml"));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/165/70.DAT"),   Path.Combine(OutputFolder, "error-messages.xml"));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/165/72.DAT"),   Path.Combine(OutputFolder, "ingame-messages-1.xml"));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/165/73.DAT"),   Path.Combine(OutputFolder, "ingame-messages-2.xml"));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/118/109.DAT"),  Path.Combine(OutputFolder, "items-armor.xml"));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/118/106.DAT"),  Path.Combine(OutputFolder, "items-general-1.xml"));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/118/107.DAT"),  Path.Combine(OutputFolder, "items-general-2.xml"));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/118/110.DAT"),  Path.Combine(OutputFolder, "items-puppet.xml"));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/118/108.DAT"),  Path.Combine(OutputFolder, "items-weapons.xml"));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/165/87.DAT"),   Path.Combine(OutputFolder, "job-names-short.xml"));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/165/86.DAT"),   Path.Combine(OutputFolder, "job-names.xml"));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/165/75.DAT"),   Path.Combine(OutputFolder, "menu-item-description.xml"));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/165/76.DAT"),   Path.Combine(OutputFolder, "menu-item-text.xml"));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/165/82.DAT"),   Path.Combine(OutputFolder, "moon-phases.xml"));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/165/71.DAT"),   Path.Combine(OutputFolder, "pol-messages.xml"));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/118/115.DAT"),  Path.Combine(OutputFolder, "quests.xml"));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/165/78.DAT"),   Path.Combine(OutputFolder, "region-names.xml"));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/119/56.DAT"),   Path.Combine(OutputFolder, "spells.xml"));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/119/57.DAT"),   Path.Combine(OutputFolder, "statuses.xml"));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/119/54.DAT"),   Path.Combine(OutputFolder, "titles.xml"));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/165/69.DAT"),   Path.Combine(OutputFolder, "various-1.xml"));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/165/77.DAT"),   Path.Combine(OutputFolder, "various-2.xml"));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/165/79.DAT"),   Path.Combine(OutputFolder, "weather-types.xml"));
	  // Dialog Tables (to detect new quests etc)
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/11.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0001)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/12.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0002)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/13.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0003)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/14.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0004)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/15.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0005)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/16.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0006)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/17.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0007)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/18.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0008)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/19.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0009)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/20.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x000A)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/21.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x000B)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/22.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x000C)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/23.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x000D)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/24.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x000E)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/26.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0010)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/27.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0011)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/28.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0012)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/29.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0013)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/30.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0014)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/31.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0015)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/32.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0016)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/33.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0017)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/34.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0018)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/35.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0019)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/36.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x001A)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/37.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x001B)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/38.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x001C)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/39.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x001D)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/40.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x001E)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/41.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x001F)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/42.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0020)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/43.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0021)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/44.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0022)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/45.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0023)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/46.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0024)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/47.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0025)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/48.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0026)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/50.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0027)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/51.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0028)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/52.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0029)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/53.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x002A)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/53.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x002B)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/54.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x002C)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/0/118.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x002E)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/0/119.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x002F)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/0/121.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0030)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/0/123.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0032)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/0/124.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0033)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/0/125.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0034)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/0/126.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0035)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/0/127.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0036)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/0.DAT"),     Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0037)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/1.DAT"),     Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0038)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/2.DAT"),     Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0039)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/3.DAT"),     Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x003A)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/4.DAT"),     Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x003B)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/5.DAT"),     Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x003C)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/6.DAT"),     Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x003D)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/7.DAT"),     Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x003E)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/8.DAT"),     Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x003F)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/9.DAT"),     Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0040)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/10.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0041)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/11.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0042)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/12.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0043)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/13.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0044)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/14.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0045)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/15.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0046)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/16.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0047)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/17.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0048)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/18.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0049)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/19.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x004A)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/20.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x004B)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/21.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x004C)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/22.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x004D)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/23.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x004E)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/24.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x004F)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/33.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0064)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/34.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0065)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/35.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0066)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/36.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0067)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/37.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0068)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/38.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0069)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/39.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x006A)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/40.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x006B)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/41.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x006C)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/42.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x006D)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/43.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x006E)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/44.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x006F)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/45.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0070)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/46.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0071)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/47.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0072)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/48.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0073)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/49.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0074)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/50.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0075)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/51.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0076)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/52.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0077)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/53.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0078)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/54.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0079)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/55.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x007A)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/56.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x007B)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/57.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x007C)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/58.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x007D)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/59.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x007E)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/60.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x007F)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/61.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0080)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/63.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0082)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/64.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0083)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/67.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0086)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/68.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0087)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/72.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x008B)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/73.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x008C)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/74.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x008D)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/75.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x008E)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/76.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x008F)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/77.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0090)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/78.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0091)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/79.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0092)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/80.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0093)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/81.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0094)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/82.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0095)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/83.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0096)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/84.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0097)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/85.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0098)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/86.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x0099)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/87.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x009A)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/90.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x009D)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/91.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x009E)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/92.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x009F)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/93.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00A0)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/94.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00A1)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/95.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00A2)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/96.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00A3)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/98.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00A5)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/99.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00A6)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/100.DAT"),  Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00A7)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/101.DAT"),  Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00A8)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/102.DAT"),  Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00A9)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/103.DAT"),  Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00AA)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/105.DAT"),  Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00AC)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/106.DAT"),  Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00AD)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/107.DAT"),  Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00AE)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/109.DAT"),  Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00B0)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/110.DAT"),  Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00B1)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/111.DAT"),  Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00B2)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/112.DAT"),  Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00B3)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/113.DAT"),  Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00B4)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/114.DAT"),  Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00B5)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/116.DAT"),  Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00B7)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/117.DAT"),  Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00B8)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/118.DAT"),  Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00B9)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/119.DAT"),  Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00BA)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/120.DAT"),  Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00BB)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/121.DAT"),  Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00BC)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/123.DAT"),  Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00BE)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/124.DAT"),  Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00BF)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/125.DAT"),  Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00C0)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/126.DAT"),  Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00C1)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/17/127.DAT"),  Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00C2)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/0.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00C3)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/1.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00C4)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/2.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00C5)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/3.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00C6)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/5.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00C8)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/6.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00C9)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/7.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00CA)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/8.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00CB)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/9.DAT"),    Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00CC)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/10.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00CD)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/11.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00CE)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/12.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00CF)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/13.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00D0)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/14.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00D1)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/16.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00D3)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/17.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00D4)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/18.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00D5)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/25.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00DC)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/26.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00DD)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/28.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00DF)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/29.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00E0)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/30.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00E1)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/31.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00E2)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/32.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00E3)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/33.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00E4)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/35.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00E6)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/36.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00E7)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/37.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00E8)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/38.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00E9)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/39.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00EA)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/40.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00EB)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/41.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00EC)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/42.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00ED)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/43.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00EE)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/44.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00EF)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/45.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00F0)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/46.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00F1)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/47.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00F2)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/48.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00F3)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/49.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00F4)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/50.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00F5)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/51.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00F6)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/52.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00F7)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/53.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00F8)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/54.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00F9)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/55.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00FA)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/56.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00FB)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/57.DAT"),   Path.Combine(OutputFolder, String.Format("dialog-table-{0:X4}.xml", 0x00FC)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/18/15.DAT"),   Path.Combine(OutputFolder, "dialog-table-cheat-menu.xml"));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/48.DAT"),    Path.Combine(OutputFolder, "dialog-table-unknown-1.xml"));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/49.DAT"),    Path.Combine(OutputFolder, "dialog-table-unknown-2.xml"));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/56.DAT"),    Path.Combine(OutputFolder, "dialog-table-unknown-3.xml"));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/78.DAT"),    Path.Combine(OutputFolder, "dialog-table-unknown-4.xml"));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/79.DAT"),    Path.Combine(OutputFolder, "dialog-table-unknown-5.xml"));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/90.DAT"),    Path.Combine(OutputFolder, "dialog-table-unknown-6.xml"));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/97.DAT"),    Path.Combine(OutputFolder, "dialog-table-unknown-7.xml"));
	  // Mob Lists (to detect new NMs)
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/111.DAT"),   Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0001)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/112.DAT"),   Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0002)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/113.DAT"),   Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0003)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/114.DAT"),   Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0004)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/115.DAT"),   Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0005)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/116.DAT"),   Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0006)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/117.DAT"),   Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0007)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/118.DAT"),   Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0008)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/119.DAT"),   Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0009)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/120.DAT"),   Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x000A)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/121.DAT"),   Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x000B)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/122.DAT"),   Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x000C)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/123.DAT"),   Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x000D)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/124.DAT"),   Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x000E)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/126.DAT"),   Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0010)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/2/127.DAT"),   Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0011)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/3/0.DAT"),     Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0012)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/3/1.DAT"),     Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0013)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/3/2.DAT"),     Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0014)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/3/3.DAT"),     Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0015)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/3/4.DAT"),     Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0016)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/3/5.DAT"),     Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0017)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/3/6.DAT"),     Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0018)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/3/7.DAT"),     Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0019)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/3/8.DAT"),     Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x001A)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/3/9.DAT"),     Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x001B)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/3/10.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x001C)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/3/11.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x001D)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/3/12.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x001E)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/3/13.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x001F)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/3/14.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0020)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/3/15.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0021)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/3/16.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0022)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/3/17.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0023)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/3/18.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0024)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/3/19.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0025)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/3/20.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0026)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/3/21.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0027)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/3/22.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0028)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/3/23.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0029)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/3/24.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x002A)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/3/25.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x002B)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM3/3/26.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x002C)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/45.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x002E)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/46.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x002F)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/47.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0030)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/49.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0032)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/50.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0033)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/51.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0034)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/52.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0035)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/53.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0036)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/54.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0037)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/55.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0038)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/56.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0039)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/57.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x003A)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/58.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x003B)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/59.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x003C)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/60.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x003D)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/61.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x003E)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/62.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x003F)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/63.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0040)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/64.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0041)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/65.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0042)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/66.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0043)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/67.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0044)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/68.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0045)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/69.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0046)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/70.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0047)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/71.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0048)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/72.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0049)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/73.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x004A)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/74.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x004B)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/75.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x004C)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/76.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x004D)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/77.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x004E)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM4/1/78.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x004F)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/37.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0064)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/38.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0065)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/39.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0066)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/40.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0067)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/41.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0068)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/42.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0069)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/43.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x006A)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/44.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x006B)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/45.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x006C)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/46.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x006D)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/47.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x006E)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/48.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x006F)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/49.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0070)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/13/95.DAT"),   Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0071)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/13/96.DAT"),   Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0072)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/52.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0073)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/53.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0074)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/27/54.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0075)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/55.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0076)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/56.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0077)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/57.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0078)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/13/97.DAT"),   Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0079)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/13/98.DAT"),   Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x007A)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/13/99.DAT"),   Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x007B)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/13/100.DAT"),  Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x007C)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/13/101.DAT"),  Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x007D)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/63.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x007E)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/64.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x007F)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/13/102.DAT"),  Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0080)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/13/103.DAT"),  Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0082)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/68.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0083)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/13/104.DAT"),  Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0086)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/13/105.DAT"),  Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0087)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/76.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x008B)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/77.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x008C)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/78.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x008D)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/79.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x008E)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/80.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x008F)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/81.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0090)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/82.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0091)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/83.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0092)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/84.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0093)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/85.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0094)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/86.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0095)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/87.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0096)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/88.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0097)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/89.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0098)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/13/106.DAT"),  Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x0099)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/13/107.DAT"),  Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x009A)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/94.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x009D)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/95.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x009E)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/13/108.DAT"),  Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x009F)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/13/109.DAT"),  Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00A0)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/98.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00A1)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/99.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00A2)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/13/110.DAT"),  Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00A3)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/102.DAT"),   Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00A5)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/103.DAT"),   Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00A6)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/104.DAT"),   Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00A7)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/13/111.DAT"),  Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00A8)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/106.DAT"),   Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00A9)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/13/112.DAT"),  Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00AA)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/109.DAT"),   Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00AC)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/13/113.DAT"),  Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00AD)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/13/114.DAT"),  Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00AE)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/13/115.DAT"),  Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00B0)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/13/116.DAT"),  Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00B1)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/13/117.DAT"),  Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00B2)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/13/118.DAT"),  Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00B3)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/13/119.DAT"),  Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00B4)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/13/120.DAT"),  Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00B5)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/121.DAT"),   Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00B8)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/13/121.DAT"),  Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00B9)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/13/122.DAT"),  Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00BA)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/13/123.DAT"),  Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00BB)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/13/124.DAT"),  Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00BC)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/26/127.DAT"),   Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00BE)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/27/0.DAT"),     Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00BF)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/27/1.DAT"),     Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00C0)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/27/2.DAT"),     Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00C1)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/27/3.DAT"),     Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00C2)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/27/4.DAT"),     Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00C3)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/27/5.DAT"),     Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00C4)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/27/6.DAT"),     Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00C5)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/27/7.DAT"),     Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00C6)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/27/9.DAT"),     Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00C8)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/13/125.DAT"),  Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00C9)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/13/126.DAT"),  Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00CA)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/13/127.DAT"),  Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00CB)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/27/13.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00CC)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/14/0.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00CD)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/27/15.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00CE)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/14/1.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00CF)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/14/2.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00D0)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/14/3.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00D1)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/14/4.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00D3)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/14/5.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00D4)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/14/6.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00D5)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/27/29.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00DC)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/27/30.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00DD)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/27/32.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00DF)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/27/33.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00E0)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/27/34.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00E1)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/14/7.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00E2)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/27/36.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00E3)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/27/37.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00E4)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/27/39.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00E6)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/27/40.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00E7)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/27/41.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00E8)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/27/42.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00E9)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/27/43.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00EA)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/27/44.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00EB)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/27/45.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00EC)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/27/46.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00ED)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/27/48.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00EE)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/27/47.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00EF)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/27/49.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00F0)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/27/50.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00F1)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/27/51.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00F2)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/27/52.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00F3)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/27/53.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00F4)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/27/54.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00F5)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/27/55.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00F6)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/14/8.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00F7)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/27/57.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00F8)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM/27/58.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00F9)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/14/9.DAT"),    Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00FA)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/14/10.DAT"),   Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00FB)));
	  Program.ExtractFile(Path.Combine(FFXIFolder, "ROM2/14/11.DAT"),   Path.Combine(OutputFolder, String.Format("mob-list-{0:X4}.xml", 0x00FC)));
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
