using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using PlayOnline.Core;
using PlayOnline.FFXI;

namespace MassExtractor {

  internal class Program {

    private static bool Ask(string Question) {
      Console.ForegroundColor = ConsoleColor.Green;
      Console.Write(String.Format("{0} [Yn] ", Question));
      Console.ForegroundColor = ConsoleColor.White;
    string Answer = Console.ReadLine();
      return (Answer == String.Empty || Answer == "Y" || Answer == "y");
    }

    [STAThread]
    static int Main() {
      try {
      string FFXIFolder = null;
	if (FFXIFolder == null) FFXIFolder = POL.GetApplicationPath(AppID.FFXI, POL.Region.Japan);
	if (FFXIFolder == null) FFXIFolder = POL.GetApplicationPath(AppID.FFXI, POL.Region.NorthAmerica);
	if (FFXIFolder == null) FFXIFolder = POL.GetApplicationPath(AppID.FFXI, POL.Region.Europe);
	if (FFXIFolder == null) {
	  Console.ForegroundColor = ConsoleColor.Red;
	  Console.WriteLine("There is no version of FFXI installed on this machine.");
	  return 1;
	}
      string OutputFolder;
	{
	string[] Args = Environment.GetCommandLineArgs();
	  if (Args.Length != 2) {
	    Console.ForegroundColor = ConsoleColor.Red;
	    Console.WriteLine("Usage: {0} <Output Folder>", Args[0]);
	    return 1;
	  }
	  OutputFolder = Args[1];
	  if (!Directory.Exists(OutputFolder)) {
	    if (Program.Ask(String.Format("Directory '{0}' does not exist - do you want me to create it?", OutputFolder)))
	      Directory.CreateDirectory(OutputFolder);
	    else
	      return 0;
	  }
	}
	for (int i = 0; i < 10; ++i) {
	string ROMFolder = Path.Combine(FFXIFolder, String.Format("Rom{0}", ((i == 0) ? "" : i.ToString())));
	  if (Directory.Exists(ROMFolder)) {
	    for (int j = 0; j < 1000; ++j) {
	    string ROMSubFolder = Path.Combine(ROMFolder, j.ToString());
	      if (Directory.Exists(ROMSubFolder)) {
		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.WriteLine("Processing {0}...", ROMSubFolder);
	      long Files      = 0;
	      long KnownFiles = 0;
		for (int k = 0; k < 128; ++k) {
		string ROMFile = Path.Combine(ROMSubFolder, String.Format("{0}.DAT", k));
		  if (File.Exists(ROMFile)) {
		    Console.ForegroundColor = ConsoleColor.White;
		    Console.WriteLine(" - {0}", Path.GetFileName(ROMFile));
		  ThingList KnownData = FileType.LoadAll(ROMFile, null);
		    if (KnownData != null && KnownData.Count > 0) {
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
		Console.ForegroundColor = ConsoleColor.White;
		Console.WriteLine(" => {0} of {1} file(s) contained recogniseable data", KnownFiles, Files);
	      }
	    }
	  }
	}
	return 0;
      }
#if !DEBUG
      catch (Exception E) {
	Console.ForegroundColor = ConsoleColor.Red;
	Console.WriteLine("A problem occurred: {0}", E.Message);
	return 1;
      }
#endif
      finally { Console.ResetColor(); }
    }

  }

}
