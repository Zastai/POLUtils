// $Id$

// Copyright © 2004-2010 Tim Van Holder
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;
using PlayOnline.Core;

namespace ItemListUpgrade {

  internal class Program {

    private readonly OpenFileDialog _dlgOldFile = new OpenFileDialog();
    private readonly SaveFileDialog _dlgNewFile = new SaveFileDialog();

    private Program() {
      // Prepare dialogs
      this._dlgOldFile.DefaultExt = "xml";
      this._dlgOldFile.Filter = I18N.GetText("FileFilter");
      this._dlgOldFile.Title = I18N.GetText("Title:OldFile");
      this._dlgNewFile.DefaultExt = "xml";
      this._dlgNewFile.Filter = I18N.GetText("FileFilter");
      this._dlgNewFile.Title = I18N.GetText("Title:NewFile");
    }

    private void Run() {
     if (this._dlgOldFile.ShowDialog() != DialogResult.OK)
        return;
      if (this._dlgNewFile.ShowDialog() != DialogResult.OK)
        return;
      this.PerformUpgrade(this._dlgOldFile.FileName, this._dlgNewFile.FileName);
    }

    #region Applying the XSLT transform

    private XslCompiledTransform _xlstUpgrade;

    private void PrepareTransform() {
      if (this._xlstUpgrade != null)
        return;
      try {
        var rs = Assembly.GetExecutingAssembly().GetManifestResourceStream("ItemListUpgrade.ItemListUpgrade.xslt");
        if (rs == null)
          return;
        this._xlstUpgrade = new XslCompiledTransform();
        var xr = new XmlTextReader(rs);
        this._xlstUpgrade.Load(xr);
        xr.Close();
      } catch {
        this._xlstUpgrade = null;
      }
    }

    private void PerformUpgrade(string oldList, string newList) {
      this.PrepareTransform();
      if (this._xlstUpgrade != null) {
        try {
          var xd = new XmlDocument();
          xd.Load(oldList);
          var xw = XmlWriter.Create(newList, this._xlstUpgrade.OutputSettings);
          this._xlstUpgrade.Transform(xd, xw);
          xw.Close();
          MessageBox.Show(null, I18N.GetText("UpgradeSuccess"), I18N.GetText("Title:UpgradeComplete"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex) {
          MessageBox.Show(null, String.Format(I18N.GetText("UpgradeFailed"), ex.Message), I18N.GetText("Title:UpgradeFailed"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      }
      else
        MessageBox.Show(null, I18N.GetText("PrepareFailed"), I18N.GetText("Title:UpgradeFailed"), MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    #endregion

    [STAThread]
    static void Main() {
      Application.EnableVisualStyles();
      new Program().Run();
    }

  }

}