// $Id$

// Copyright © 2004-2010 Tim Van Holder
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

namespace PlayOnline.FFXI.Utils.DataBrowser {

  public partial class MainWindow {

    private System.ComponentModel.IContainer components;

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      this.components = new System.ComponentModel.Container();
      this.tvDataFiles = new System.Windows.Forms.TreeView();
      this.ilBrowserIcons = new System.Windows.Forms.ImageList(this.components);
      this.splSplitter = new System.Windows.Forms.Splitter();
      this.mnuPictureContext = new System.Windows.Forms.ContextMenu();
      this.mnuPCMode = new System.Windows.Forms.MenuItem();
      this.mnuPCModeNormal = new System.Windows.Forms.MenuItem();
      this.mnuPCModeCentered = new System.Windows.Forms.MenuItem();
      this.mnuPCModeStretched = new System.Windows.Forms.MenuItem();
      this.mnuPCModeZoomed = new System.Windows.Forms.MenuItem();
      this.mnuPCBackground = new System.Windows.Forms.MenuItem();
      this.mnuPCBackgroundBlack = new System.Windows.Forms.MenuItem();
      this.mnuPCBackgroundWhite = new System.Windows.Forms.MenuItem();
      this.mnuPCBackgroundTransparent = new System.Windows.Forms.MenuItem();
      this.mnuPCSaveAs = new System.Windows.Forms.MenuItem();
      this.dlgSavePicture = new System.Windows.Forms.SaveFileDialog();
      this.mnuEntryListContext = new System.Windows.Forms.ContextMenu();
      this.mnuELCProperties = new System.Windows.Forms.MenuItem();
      this.mnuELCSep = new System.Windows.Forms.MenuItem();
      this.mnuELCCopyRow = new System.Windows.Forms.MenuItem();
      this.mnuELCCopyField = new System.Windows.Forms.MenuItem();
      this.mnuELCSep2 = new System.Windows.Forms.MenuItem();
      this.mnuELCExport = new System.Windows.Forms.MenuItem();
      this.mnuELCEAll = new System.Windows.Forms.MenuItem();
      this.mnuELCESelected = new System.Windows.Forms.MenuItem();
      this.mnuMain = new System.Windows.Forms.MainMenu(this.components);
      this.pnlViewerArea = new System.Windows.Forms.Panel();
      this.tabViewers = new System.Windows.Forms.TabControl();
      this.tabViewerItems = new System.Windows.Forms.ThemedTabPage();
      this.ieItemViewer = new PlayOnline.FFXI.ItemEditor();
      this.grpMainItemActions = new System.Windows.Forms.GroupBox();
      this.cmbItems = new System.Windows.Forms.ComboBox();
      this.btnFindItems = new System.Windows.Forms.Button();
      this.tabViewerImages = new System.Windows.Forms.ThemedTabPage();
      this.picImageViewer = new System.Windows.Forms.PictureBox();
      this.pnlImageChooser = new System.Windows.Forms.Panel();
      this.cmbImageChooser = new System.Windows.Forms.ComboBox();
      this.lblImageChooser = new System.Windows.Forms.Label();
      this.tabViewerGeneral = new System.Windows.Forms.ThemedTabPage();
      this.pnlGeneralContents = new System.Windows.Forms.Panel();
      this.pnlThingListActions = new System.Windows.Forms.Panel();
      this.chkShowIcons = new System.Windows.Forms.CheckBox();
      this.btnThingListSaveImages = new System.Windows.Forms.Button();
      this.btnThingListExportXML = new System.Windows.Forms.Button();
      this.pnlNoViewers = new System.Windows.Forms.Panel();
      this.btnReloadFile = new System.Windows.Forms.Button();
      this.lblNoViewers = new System.Windows.Forms.Label();
      this.dlgExportFile = new System.Windows.Forms.SaveFileDialog();
      this.pnlViewerArea.SuspendLayout();
      this.tabViewers.SuspendLayout();
      this.tabViewerItems.SuspendLayout();
      this.grpMainItemActions.SuspendLayout();
      this.tabViewerImages.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.picImageViewer)).BeginInit();
      this.pnlImageChooser.SuspendLayout();
      this.tabViewerGeneral.SuspendLayout();
      this.pnlThingListActions.SuspendLayout();
      this.pnlNoViewers.SuspendLayout();
      this.SuspendLayout();
      // 
      // tvDataFiles
      // 
      this.tvDataFiles.Dock = System.Windows.Forms.DockStyle.Left;
      this.tvDataFiles.HideSelection = false;
      this.tvDataFiles.ImageIndex = 0;
      this.tvDataFiles.ImageList = this.ilBrowserIcons;
      this.tvDataFiles.Indent = 19;
      this.tvDataFiles.ItemHeight = 16;
      this.tvDataFiles.Location = new System.Drawing.Point(0, 0);
      this.tvDataFiles.Name = "tvDataFiles";
      this.tvDataFiles.PathSeparator = "/";
      this.tvDataFiles.SelectedImageIndex = 0;
      this.tvDataFiles.Size = new System.Drawing.Size(324, 457);
      this.tvDataFiles.TabIndex = 0;
      this.tvDataFiles.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tvDataFiles_AfterCollapse);
      this.tvDataFiles.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvDataFiles_BeforeExpand);
      this.tvDataFiles.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvDataFiles_AfterExpand);
      this.tvDataFiles.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvDataFiles_AfterSelect);
      // 
      // ilBrowserIcons
      // 
      this.ilBrowserIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
      this.ilBrowserIcons.ImageSize = new System.Drawing.Size(16, 16);
      this.ilBrowserIcons.TransparentColor = System.Drawing.Color.Transparent;
      // 
      // splSplitter
      // 
      this.splSplitter.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.splSplitter.Location = new System.Drawing.Point(324, 0);
      this.splSplitter.Name = "splSplitter";
      this.splSplitter.Size = new System.Drawing.Size(4, 457);
      this.splSplitter.TabIndex = 2;
      this.splSplitter.TabStop = false;
      // 
      // mnuPictureContext
      // 
      this.mnuPictureContext.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuPCMode,
            this.mnuPCBackground,
            this.mnuPCSaveAs});
      // 
      // mnuPCMode
      // 
      this.mnuPCMode.Index = 0;
      this.mnuPCMode.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuPCModeNormal,
            this.mnuPCModeCentered,
            this.mnuPCModeStretched,
            this.mnuPCModeZoomed});
      this.mnuPCMode.Text = "&Mode";
      // 
      // mnuPCModeNormal
      // 
      this.mnuPCModeNormal.Checked = true;
      this.mnuPCModeNormal.Index = 0;
      this.mnuPCModeNormal.RadioCheck = true;
      this.mnuPCModeNormal.Text = "&Normal";
      this.mnuPCModeNormal.Click += new System.EventHandler(this.mnuPCModeNormal_Click);
      // 
      // mnuPCModeCentered
      // 
      this.mnuPCModeCentered.Index = 1;
      this.mnuPCModeCentered.RadioCheck = true;
      this.mnuPCModeCentered.Text = "&Centered";
      this.mnuPCModeCentered.Click += new System.EventHandler(this.mnuPCModeCentered_Click);
      // 
      // mnuPCModeStretched
      // 
      this.mnuPCModeStretched.Index = 2;
      this.mnuPCModeStretched.RadioCheck = true;
      this.mnuPCModeStretched.Text = "&Stretched";
      this.mnuPCModeStretched.Click += new System.EventHandler(this.mnuPCModeStretched_Click);
      // 
      // mnuPCModeZoomed
      // 
      this.mnuPCModeZoomed.Index = 3;
      this.mnuPCModeZoomed.Text = "&Zoomed";
      this.mnuPCModeZoomed.Click += new System.EventHandler(this.mnuPCModeZoomed_Click);
      // 
      // mnuPCBackground
      // 
      this.mnuPCBackground.Index = 1;
      this.mnuPCBackground.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuPCBackgroundBlack,
            this.mnuPCBackgroundWhite,
            this.mnuPCBackgroundTransparent});
      this.mnuPCBackground.Text = "&Background";
      // 
      // mnuPCBackgroundBlack
      // 
      this.mnuPCBackgroundBlack.Index = 0;
      this.mnuPCBackgroundBlack.RadioCheck = true;
      this.mnuPCBackgroundBlack.Text = "&Black";
      this.mnuPCBackgroundBlack.Click += new System.EventHandler(this.mnuPCBackgroundBlack_Click);
      // 
      // mnuPCBackgroundWhite
      // 
      this.mnuPCBackgroundWhite.Index = 1;
      this.mnuPCBackgroundWhite.RadioCheck = true;
      this.mnuPCBackgroundWhite.Text = "&White";
      this.mnuPCBackgroundWhite.Click += new System.EventHandler(this.mnuPCBackgroundWhite_Click);
      // 
      // mnuPCBackgroundTransparent
      // 
      this.mnuPCBackgroundTransparent.Checked = true;
      this.mnuPCBackgroundTransparent.Index = 2;
      this.mnuPCBackgroundTransparent.RadioCheck = true;
      this.mnuPCBackgroundTransparent.Text = "&Transparent";
      this.mnuPCBackgroundTransparent.Click += new System.EventHandler(this.mnuPCBackgroundTransparent_Click);
      // 
      // mnuPCSaveAs
      // 
      this.mnuPCSaveAs.Index = 2;
      this.mnuPCSaveAs.Text = "&Save As...";
      this.mnuPCSaveAs.Click += new System.EventHandler(this.mnuPCSaveAs_Click);
      // 
      // dlgSavePicture
      // 
      this.dlgSavePicture.Filter = "Windows Bitmap (*.bmp)|*.bmp|Portable Network Graphic (*.png)|*.png";
      this.dlgSavePicture.Title = "Save Picture As...";
      // 
      // mnuEntryListContext
      // 
      this.mnuEntryListContext.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuELCProperties,
            this.mnuELCSep,
            this.mnuELCCopyRow,
            this.mnuELCCopyField,
            this.mnuELCSep2,
            this.mnuELCExport});
      this.mnuEntryListContext.Popup += new System.EventHandler(this.mnuStringTableContext_Popup);
      // 
      // mnuELCProperties
      // 
      this.mnuELCProperties.Index = 0;
      this.mnuELCProperties.Text = "&Properties...";
      this.mnuELCProperties.Click += new System.EventHandler(this.mnuELCProperties_Click);
      // 
      // mnuELCSep
      // 
      this.mnuELCSep.Index = 1;
      this.mnuELCSep.Text = "-";
      // 
      // mnuELCCopyRow
      // 
      this.mnuELCCopyRow.Index = 2;
      this.mnuELCCopyRow.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
      this.mnuELCCopyRow.Text = "Copy &Row";
      this.mnuELCCopyRow.Click += new System.EventHandler(this.mnuELCCopyRow_Click);
      // 
      // mnuELCCopyField
      // 
      this.mnuELCCopyField.Index = 3;
      this.mnuELCCopyField.Text = "Copy &Field";
      // 
      // mnuELCSep2
      // 
      this.mnuELCSep2.Index = 4;
      this.mnuELCSep2.Text = "-";
      // 
      // mnuELCExport
      // 
      this.mnuELCExport.Index = 5;
      this.mnuELCExport.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuELCEAll,
            this.mnuELCESelected});
      this.mnuELCExport.Text = "&Export";
      // 
      // mnuELCEAll
      // 
      this.mnuELCEAll.Index = 0;
      this.mnuELCEAll.Text = "&All Entries...";
      this.mnuELCEAll.Click += new System.EventHandler(this.mnuELCEAll_Click);
      // 
      // mnuELCESelected
      // 
      this.mnuELCESelected.Index = 1;
      this.mnuELCESelected.Text = "&Selected Entries...";
      this.mnuELCESelected.Click += new System.EventHandler(this.mnuELCESelected_Click);
      // 
      // pnlViewerArea
      // 
      this.pnlViewerArea.Controls.Add(this.tabViewers);
      this.pnlViewerArea.Controls.Add(this.pnlNoViewers);
      this.pnlViewerArea.Dock = System.Windows.Forms.DockStyle.Fill;
      this.pnlViewerArea.Location = new System.Drawing.Point(328, 0);
      this.pnlViewerArea.Name = "pnlViewerArea";
      this.pnlViewerArea.Size = new System.Drawing.Size(440, 457);
      this.pnlViewerArea.TabIndex = 10;
      // 
      // tabViewers
      // 
      this.tabViewers.Controls.Add(this.tabViewerItems);
      this.tabViewers.Controls.Add(this.tabViewerImages);
      this.tabViewers.Controls.Add(this.tabViewerGeneral);
      this.tabViewers.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabViewers.ItemSize = new System.Drawing.Size(0, 18);
      this.tabViewers.Location = new System.Drawing.Point(0, 30);
      this.tabViewers.Name = "tabViewers";
      this.tabViewers.SelectedIndex = 0;
      this.tabViewers.Size = new System.Drawing.Size(440, 427);
      this.tabViewers.TabIndex = 12;
      // 
      // tabViewerItems
      // 
      this.tabViewerItems.Controls.Add(this.ieItemViewer);
      this.tabViewerItems.Controls.Add(this.grpMainItemActions);
      this.tabViewerItems.Location = new System.Drawing.Point(4, 22);
      this.tabViewerItems.Name = "tabViewerItems";
      this.tabViewerItems.Size = new System.Drawing.Size(432, 401);
      this.tabViewerItems.TabIndex = 3;
      this.tabViewerItems.Text = "Item Data";
      this.tabViewerItems.UseVisualStyleBackColor = true;
      // 
      // ieItemViewer
      // 
      this.ieItemViewer.BackColor = System.Drawing.Color.Transparent;
      this.ieItemViewer.Item = null;
      this.ieItemViewer.Location = new System.Drawing.Point(4, 44);
      this.ieItemViewer.Name = "ieItemViewer";
      this.ieItemViewer.Size = new System.Drawing.Size(424, 260);
      this.ieItemViewer.TabIndex = 1;
      this.ieItemViewer.SizeChanged += new System.EventHandler(this.ieItemViewer_SizeChanged);
      // 
      // grpMainItemActions
      // 
      this.grpMainItemActions.Controls.Add(this.cmbItems);
      this.grpMainItemActions.Controls.Add(this.btnFindItems);
      this.grpMainItemActions.Location = new System.Drawing.Point(4, 4);
      this.grpMainItemActions.Name = "grpMainItemActions";
      this.grpMainItemActions.Size = new System.Drawing.Size(424, 40);
      this.grpMainItemActions.TabIndex = 0;
      this.grpMainItemActions.TabStop = false;
      // 
      // cmbItems
      // 
      this.cmbItems.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.cmbItems.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbItems.FormattingEnabled = true;
      this.cmbItems.ItemHeight = 13;
      this.cmbItems.Location = new System.Drawing.Point(8, 12);
      this.cmbItems.MaxDropDownItems = 10;
      this.cmbItems.Name = "cmbItems";
      this.cmbItems.Size = new System.Drawing.Size(322, 21);
      this.cmbItems.TabIndex = 1;
      this.cmbItems.SelectedIndexChanged += new System.EventHandler(this.cmbItems_SelectedIndexChanged);
      // 
      // btnFindItems
      // 
      this.btnFindItems.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnFindItems.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnFindItems.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.btnFindItems.Location = new System.Drawing.Point(336, 11);
      this.btnFindItems.Name = "btnFindItems";
      this.btnFindItems.Size = new System.Drawing.Size(80, 22);
      this.btnFindItems.TabIndex = 3;
      this.btnFindItems.Text = "&Find Item(s)...";
      this.btnFindItems.Click += new System.EventHandler(this.btnFindItems_Click);
      // 
      // tabViewerImages
      // 
      this.tabViewerImages.Controls.Add(this.picImageViewer);
      this.tabViewerImages.Controls.Add(this.pnlImageChooser);
      this.tabViewerImages.Location = new System.Drawing.Point(4, 22);
      this.tabViewerImages.Name = "tabViewerImages";
      this.tabViewerImages.Size = new System.Drawing.Size(432, 401);
      this.tabViewerImages.TabIndex = 2;
      this.tabViewerImages.Text = "Image(s)";
      this.tabViewerImages.UseVisualStyleBackColor = true;
      this.tabViewerImages.Visible = false;
      // 
      // picImageViewer
      // 
      this.picImageViewer.BackColor = System.Drawing.Color.Transparent;
      this.picImageViewer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.picImageViewer.ContextMenu = this.mnuPictureContext;
      this.picImageViewer.Dock = System.Windows.Forms.DockStyle.Fill;
      this.picImageViewer.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.picImageViewer.Location = new System.Drawing.Point(0, 36);
      this.picImageViewer.Name = "picImageViewer";
      this.picImageViewer.Size = new System.Drawing.Size(432, 365);
      this.picImageViewer.TabIndex = 3;
      this.picImageViewer.TabStop = false;
      // 
      // pnlImageChooser
      // 
      this.pnlImageChooser.Controls.Add(this.cmbImageChooser);
      this.pnlImageChooser.Controls.Add(this.lblImageChooser);
      this.pnlImageChooser.Dock = System.Windows.Forms.DockStyle.Top;
      this.pnlImageChooser.Location = new System.Drawing.Point(0, 0);
      this.pnlImageChooser.Name = "pnlImageChooser";
      this.pnlImageChooser.Size = new System.Drawing.Size(432, 36);
      this.pnlImageChooser.TabIndex = 4;
      // 
      // cmbImageChooser
      // 
      this.cmbImageChooser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.cmbImageChooser.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbImageChooser.FormattingEnabled = true;
      this.cmbImageChooser.ItemHeight = 13;
      this.cmbImageChooser.Location = new System.Drawing.Point(140, 8);
      this.cmbImageChooser.Name = "cmbImageChooser";
      this.cmbImageChooser.Size = new System.Drawing.Size(284, 21);
      this.cmbImageChooser.TabIndex = 1;
      this.cmbImageChooser.SelectedIndexChanged += new System.EventHandler(this.cmbImageChooser_SelectedIndexChanged);
      // 
      // lblImageChooser
      // 
      this.lblImageChooser.BackColor = System.Drawing.Color.Transparent;
      this.lblImageChooser.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblImageChooser.Location = new System.Drawing.Point(8, 12);
      this.lblImageChooser.Name = "lblImageChooser";
      this.lblImageChooser.Size = new System.Drawing.Size(136, 16);
      this.lblImageChooser.TabIndex = 0;
      this.lblImageChooser.Text = "Choose an image to view:";
      this.lblImageChooser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // tabViewerGeneral
      // 
      this.tabViewerGeneral.Controls.Add(this.pnlGeneralContents);
      this.tabViewerGeneral.Controls.Add(this.pnlThingListActions);
      this.tabViewerGeneral.Location = new System.Drawing.Point(4, 22);
      this.tabViewerGeneral.Name = "tabViewerGeneral";
      this.tabViewerGeneral.Size = new System.Drawing.Size(432, 401);
      this.tabViewerGeneral.TabIndex = 1;
      this.tabViewerGeneral.Text = "General Contents";
      this.tabViewerGeneral.UseVisualStyleBackColor = true;
      // 
      // pnlGeneralContents
      // 
      this.pnlGeneralContents.Dock = System.Windows.Forms.DockStyle.Fill;
      this.pnlGeneralContents.Location = new System.Drawing.Point(0, 28);
      this.pnlGeneralContents.Name = "pnlGeneralContents";
      this.pnlGeneralContents.Size = new System.Drawing.Size(432, 373);
      this.pnlGeneralContents.TabIndex = 5;
      // 
      // pnlThingListActions
      // 
      this.pnlThingListActions.Controls.Add(this.chkShowIcons);
      this.pnlThingListActions.Controls.Add(this.btnThingListSaveImages);
      this.pnlThingListActions.Controls.Add(this.btnThingListExportXML);
      this.pnlThingListActions.Dock = System.Windows.Forms.DockStyle.Top;
      this.pnlThingListActions.Location = new System.Drawing.Point(0, 0);
      this.pnlThingListActions.Name = "pnlThingListActions";
      this.pnlThingListActions.Size = new System.Drawing.Size(432, 28);
      this.pnlThingListActions.TabIndex = 4;
      // 
      // chkShowIcons
      // 
      this.chkShowIcons.AutoSize = true;
      this.chkShowIcons.Location = new System.Drawing.Point(253, 7);
      this.chkShowIcons.Name = "chkShowIcons";
      this.chkShowIcons.Size = new System.Drawing.Size(82, 17);
      this.chkShowIcons.TabIndex = 2;
      this.chkShowIcons.Text = "&Show Icons";
      this.chkShowIcons.UseVisualStyleBackColor = true;
      this.chkShowIcons.CheckedChanged += new System.EventHandler(this.chkShowIcons_CheckedChanged);
      // 
      // btnThingListSaveImages
      // 
      this.btnThingListSaveImages.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.btnThingListSaveImages.Location = new System.Drawing.Point(134, 3);
      this.btnThingListSaveImages.Name = "btnThingListSaveImages";
      this.btnThingListSaveImages.Size = new System.Drawing.Size(113, 23);
      this.btnThingListSaveImages.TabIndex = 1;
      this.btnThingListSaveImages.Text = "&Save All Images...";
      this.btnThingListSaveImages.UseVisualStyleBackColor = true;
      this.btnThingListSaveImages.Click += new System.EventHandler(this.btnThingListSaveImages_Click);
      // 
      // btnThingListExportXML
      // 
      this.btnThingListExportXML.Location = new System.Drawing.Point(3, 3);
      this.btnThingListExportXML.Name = "btnThingListExportXML";
      this.btnThingListExportXML.Size = new System.Drawing.Size(125, 23);
      this.btnThingListExportXML.TabIndex = 0;
      this.btnThingListExportXML.Text = "&Export List As XML...";
      this.btnThingListExportXML.UseVisualStyleBackColor = true;
      this.btnThingListExportXML.Click += new System.EventHandler(this.btnThingListExportXML_Click);
      // 
      // pnlNoViewers
      // 
      this.pnlNoViewers.Controls.Add(this.btnReloadFile);
      this.pnlNoViewers.Controls.Add(this.lblNoViewers);
      this.pnlNoViewers.Dock = System.Windows.Forms.DockStyle.Top;
      this.pnlNoViewers.Location = new System.Drawing.Point(0, 0);
      this.pnlNoViewers.Name = "pnlNoViewers";
      this.pnlNoViewers.Size = new System.Drawing.Size(440, 30);
      this.pnlNoViewers.TabIndex = 2;
      // 
      // btnReloadFile
      // 
      this.btnReloadFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnReloadFile.Location = new System.Drawing.Point(362, 3);
      this.btnReloadFile.Name = "btnReloadFile";
      this.btnReloadFile.Size = new System.Drawing.Size(75, 23);
      this.btnReloadFile.TabIndex = 3;
      this.btnReloadFile.Text = "&Reload File";
      this.btnReloadFile.UseVisualStyleBackColor = true;
      this.btnReloadFile.Click += new System.EventHandler(this.btnReloadFile_Click);
      // 
      // lblNoViewers
      // 
      this.lblNoViewers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.lblNoViewers.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblNoViewers.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblNoViewers.Location = new System.Drawing.Point(3, 6);
      this.lblNoViewers.Name = "lblNoViewers";
      this.lblNoViewers.Size = new System.Drawing.Size(353, 16);
      this.lblNoViewers.TabIndex = 2;
      this.lblNoViewers.Text = "There are no viewers available for this file.";
      this.lblNoViewers.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // dlgExportFile
      // 
      this.dlgExportFile.DefaultExt = "xml";
      this.dlgExportFile.Filter = "XML Files (*.xml)|*.xml|All Files|*.*";
      this.dlgExportFile.Title = "Select File To Export To";
      // 
      // MainWindow
      // 
      this.ClientSize = new System.Drawing.Size(768, 457);
      this.Controls.Add(this.pnlViewerArea);
      this.Controls.Add(this.splSplitter);
      this.Controls.Add(this.tvDataFiles);
      this.Menu = this.mnuMain;
      this.Name = "MainWindow";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "FFXI Data Browser";
      this.pnlViewerArea.ResumeLayout(false);
      this.tabViewers.ResumeLayout(false);
      this.tabViewerItems.ResumeLayout(false);
      this.grpMainItemActions.ResumeLayout(false);
      this.tabViewerImages.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.picImageViewer)).EndInit();
      this.pnlImageChooser.ResumeLayout(false);
      this.tabViewerGeneral.ResumeLayout(false);
      this.pnlThingListActions.ResumeLayout(false);
      this.pnlThingListActions.PerformLayout();
      this.pnlNoViewers.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TreeView tvDataFiles;
    private System.Windows.Forms.ImageList ilBrowserIcons;
    private System.Windows.Forms.Splitter splSplitter;
    private System.Windows.Forms.ContextMenu mnuPictureContext;
    private System.Windows.Forms.MenuItem mnuPCBackgroundBlack;
    private System.Windows.Forms.MenuItem mnuPCBackgroundWhite;
    private System.Windows.Forms.MenuItem mnuPCBackgroundTransparent;
    private System.Windows.Forms.SaveFileDialog dlgSavePicture;
    private System.Windows.Forms.MainMenu mnuMain;
    private System.Windows.Forms.MenuItem mnuPCMode;
    private System.Windows.Forms.MenuItem mnuPCModeNormal;
    private System.Windows.Forms.MenuItem mnuPCModeCentered;
    private System.Windows.Forms.MenuItem mnuPCModeStretched;
    private System.Windows.Forms.MenuItem mnuPCBackground;
    private System.Windows.Forms.MenuItem mnuPCSaveAs;
    private System.Windows.Forms.ContextMenu mnuEntryListContext;
    private System.Windows.Forms.Panel pnlViewerArea;
    private System.Windows.Forms.TabControl tabViewers;
    private System.Windows.Forms.PictureBox picImageViewer;
    private System.Windows.Forms.Panel pnlImageChooser;
    private System.Windows.Forms.Label lblImageChooser;
    private System.Windows.Forms.ComboBox cmbImageChooser;
    private System.Windows.Forms.Panel pnlNoViewers;
    private System.Windows.Forms.Label lblNoViewers;
    private System.Windows.Forms.Button btnFindItems;
    private System.Windows.Forms.GroupBox grpMainItemActions;
    private System.Windows.Forms.ComboBox cmbItems;
    private PlayOnline.FFXI.ItemEditor ieItemViewer;
    private System.Windows.Forms.MenuItem mnuELCCopyRow;
    private System.Windows.Forms.MenuItem mnuELCCopyField;
    private System.Windows.Forms.SaveFileDialog dlgExportFile;
    private System.Windows.Forms.ThemedTabPage tabViewerImages;
    private System.Windows.Forms.ThemedTabPage tabViewerItems;
    private System.Windows.Forms.ThemedTabPage tabViewerGeneral;
    private System.Windows.Forms.MenuItem mnuPCModeZoomed;
    private System.Windows.Forms.MenuItem mnuELCProperties;
    private System.Windows.Forms.MenuItem mnuELCSep;
    private System.Windows.Forms.Panel pnlThingListActions;
    private System.Windows.Forms.Button btnThingListExportXML;
    private System.Windows.Forms.MenuItem mnuELCSep2;
    private System.Windows.Forms.MenuItem mnuELCExport;
    private System.Windows.Forms.MenuItem mnuELCEAll;
    private System.Windows.Forms.MenuItem mnuELCESelected;
    private System.Windows.Forms.Button btnThingListSaveImages;
    private System.Windows.Forms.Button btnReloadFile;
    private System.Windows.Forms.CheckBox chkShowIcons;
    private System.Windows.Forms.Panel pnlGeneralContents;

  }

}
