using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

using PlayOnline.Core;
using PlayOnline.FFXI;

namespace PlayOnline.Utils.FFXIDataBrowser {

  internal abstract class IItemExporter {

    public abstract void DoExport(FFXIItem[] Items);

    private static FolderBrowserDialog dlgBrowseFolder = null;

    private static void PrepareFolderBrowser() {
      if (IItemExporter.dlgBrowseFolder == null) {
	IItemExporter.dlgBrowseFolder = new FolderBrowserDialog();
	IItemExporter.dlgBrowseFolder.Description = I18N.GetText("Export:DirDialogDesc");
      string DefaultLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), Path.Combine("POLUtils", "Exported Item Data"));
      string InitialLocation = null;
	try {
	RegistryKey RK = Registry.CurrentUser.CreateSubKey(@"Software\Pebbles\POLUtils");
	  InitialLocation = RK.GetValue("Export Location", null) as string;
	  RK.Close();
	} catch { }
	if (InitialLocation == null || InitialLocation == String.Empty)
	  InitialLocation = DefaultLocation;
	if (!Directory.Exists(InitialLocation))
	  Directory.CreateDirectory(InitialLocation);
	IItemExporter.dlgBrowseFolder.SelectedPath = InitialLocation;
      }
    }

    public static string OutputPath {
      get {
	IItemExporter.PrepareFolderBrowser();
	return IItemExporter.dlgBrowseFolder.SelectedPath;
      }
    }

    public static void BrowseForOutputPath() {
      IItemExporter.PrepareFolderBrowser();
      if (IItemExporter.dlgBrowseFolder.ShowDialog() == DialogResult.OK) {
	try {
	RegistryKey RK = Registry.CurrentUser.CreateSubKey(@"Software\Pebbles\POLUtils");
	  RK.SetValue("Export Location", IItemExporter.dlgBrowseFolder.SelectedPath);
	  RK.Close();
	} catch { }
      }
    }

  }

}
