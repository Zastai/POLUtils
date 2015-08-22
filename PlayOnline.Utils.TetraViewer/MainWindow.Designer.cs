// $Id$

// Copyright © 2004-2010 Tim Van Holder
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

namespace PlayOnline.Utils.TetraViewer {

  public partial class MainWindow {

    #region Controls

    private System.Windows.Forms.FolderBrowserDialog dlgBrowseFolder;
    private System.Windows.Forms.TreeView tvDataFiles;
    private System.Windows.Forms.StatusBar sbrStatus;
    private System.Windows.Forms.ContextMenu mnuTreeContext;
    private System.Windows.Forms.ContextMenu mnuPictureContext;
    private System.Windows.Forms.MenuItem mnuStretchImage;
    private System.Windows.Forms.MenuItem mnuExportAll;
    private System.Windows.Forms.MenuItem mnuExport;
    private System.Windows.Forms.PictureBox picViewer;
    private System.Windows.Forms.MenuItem mnuNormalImage;
    private System.Windows.Forms.MenuItem mnuTiledImage;

    private System.ComponentModel.Container components = null;

    #endregion

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      this.dlgBrowseFolder = new System.Windows.Forms.FolderBrowserDialog();
      this.tvDataFiles = new System.Windows.Forms.TreeView();
      this.mnuTreeContext = new System.Windows.Forms.ContextMenu();
      this.mnuExportAll = new System.Windows.Forms.MenuItem();
      this.mnuExport = new System.Windows.Forms.MenuItem();
      this.sbrStatus = new System.Windows.Forms.StatusBar();
      this.picViewer = new System.Windows.Forms.PictureBox();
      this.mnuPictureContext = new System.Windows.Forms.ContextMenu();
      this.mnuNormalImage = new System.Windows.Forms.MenuItem();
      this.mnuStretchImage = new System.Windows.Forms.MenuItem();
      this.mnuTiledImage = new System.Windows.Forms.MenuItem();
      ((System.ComponentModel.ISupportInitialize)(this.picViewer)).BeginInit();
      this.SuspendLayout();
      // 
      // dlgBrowseFolder
      // 
      this.dlgBrowseFolder.Description = "Select the directory where the exported files should be stored:";
      // 
      // tvDataFiles
      // 
      this.tvDataFiles.ContextMenu = this.mnuTreeContext;
      this.tvDataFiles.Dock = System.Windows.Forms.DockStyle.Left;
      this.tvDataFiles.HideSelection = false;
      this.tvDataFiles.Indent = 19;
      this.tvDataFiles.ItemHeight = 16;
      this.tvDataFiles.Location = new System.Drawing.Point(0, 0);
      this.tvDataFiles.Name = "tvDataFiles";
      this.tvDataFiles.Size = new System.Drawing.Size(212, 374);
      this.tvDataFiles.TabIndex = 0;
      this.tvDataFiles.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvDataFiles_AfterSelect);
      // 
      // mnuTreeContext
      // 
      this.mnuTreeContext.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuExportAll,
            this.mnuExport});
      // 
      // mnuExportAll
      // 
      this.mnuExportAll.Enabled = false;
      this.mnuExportAll.Index = 0;
      this.mnuExportAll.Text = "&Export All...";
      // 
      // mnuExport
      // 
      this.mnuExport.Enabled = false;
      this.mnuExport.Index = 1;
      this.mnuExport.Text = "&Export...";
      // 
      // sbrStatus
      // 
      this.sbrStatus.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.sbrStatus.Location = new System.Drawing.Point(0, 374);
      this.sbrStatus.Name = "sbrStatus";
      this.sbrStatus.Size = new System.Drawing.Size(584, 20);
      this.sbrStatus.TabIndex = 1;
      // 
      // picViewer
      // 
      this.picViewer.ContextMenu = this.mnuPictureContext;
      this.picViewer.Dock = System.Windows.Forms.DockStyle.Fill;
      this.picViewer.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.picViewer.Location = new System.Drawing.Point(212, 0);
      this.picViewer.Name = "picViewer";
      this.picViewer.Size = new System.Drawing.Size(372, 374);
      this.picViewer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
      this.picViewer.TabIndex = 2;
      this.picViewer.TabStop = false;
      // 
      // mnuPictureContext
      // 
      this.mnuPictureContext.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuNormalImage,
            this.mnuStretchImage,
            this.mnuTiledImage});
      // 
      // mnuNormalImage
      // 
      this.mnuNormalImage.Checked = true;
      this.mnuNormalImage.Index = 0;
      this.mnuNormalImage.RadioCheck = true;
      this.mnuNormalImage.Text = "&Normal";
      this.mnuNormalImage.Click += new System.EventHandler(this.ImageOption_Click);
      // 
      // mnuStretchImage
      // 
      this.mnuStretchImage.Index = 1;
      this.mnuStretchImage.RadioCheck = true;
      this.mnuStretchImage.Text = "&Strech";
      this.mnuStretchImage.Click += new System.EventHandler(this.ImageOption_Click);
      // 
      // mnuTiledImage
      // 
      this.mnuTiledImage.Enabled = false;
      this.mnuTiledImage.Index = 2;
      this.mnuTiledImage.RadioCheck = true;
      this.mnuTiledImage.Text = "&Tiled";
      this.mnuTiledImage.Click += new System.EventHandler(this.ImageOption_Click);
      // 
      // MainWindow
      // 
      this.ClientSize = new System.Drawing.Size(584, 394);
      this.Controls.Add(this.picViewer);
      this.Controls.Add(this.tvDataFiles);
      this.Controls.Add(this.sbrStatus);
      this.Name = "MainWindow";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Tetra Viewer";
      ((System.ComponentModel.ISupportInitialize)(this.picViewer)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

  }

}
