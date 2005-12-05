// $Id$

using System;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

using PlayOnline.Core;

namespace PlayOnline.FFXI.Utils.DataBrowser {

  internal abstract class IItemExporter {

    public abstract bool PrepareExport();

    public abstract void DoExport(FFXIItem[] Items);

    private static FolderBrowserDialog dlgBrowseFolder = null;

    private static void PrepareFolderBrowser() {
      if (IItemExporter.dlgBrowseFolder == null) {
	IItemExporter.dlgBrowseFolder = new FolderBrowserDialog();
	IItemExporter.dlgBrowseFolder.Description = I18N.GetText("Export:DirDialogDesc");
      string DefaultLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), Path.Combine("POLUtils", "Exported Item Data"));
      string InitialLocation = null;
	using (RegistryKey RK = POL.OpenPOLUtilsConfigKey()) {
	  if (RK != null)
	    InitialLocation = RK.GetValue(@"Data Browser\Export Location", null) as string;
	}
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
	using (RegistryKey RK = POL.OpenPOLUtilsConfigKey()) {
	  if (RK != null)
	    RK.SetValue(@"Data Browser\Export Location", IItemExporter.dlgBrowseFolder.SelectedPath);
	}
      }
    }

  }

}
