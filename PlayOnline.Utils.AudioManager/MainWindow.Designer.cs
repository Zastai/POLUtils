// $Id$

// Copyright © 2004-2010 Tim Van Holder
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

namespace PlayOnline.Utils.AudioManager {

  public partial class MainWindow {

    #region Controls

    private System.Windows.Forms.Panel pnlInfoArea;
    private System.Windows.Forms.GroupBox grpFileInfo;
    private System.Windows.Forms.Label lblLocation;
    private System.Windows.Forms.TextBox txtLocation;
    private System.Windows.Forms.TabControl tabBrowsers;
    private System.Windows.Forms.TabPage tabMusicBrowser;
    private System.Windows.Forms.TreeView tvMusicBrowser;
    private System.Windows.Forms.TabPage tabSoundBrowser;
    private System.Windows.Forms.TreeView tvSoundBrowser;
    private System.Windows.Forms.ImageList ilMusicBrowserIcons;
    private System.Windows.Forms.ImageList ilSoundBrowserIcons;
    private System.Windows.Forms.Label lblFileType;
    private System.Windows.Forms.TextBox txtFileType;
    private System.Windows.Forms.Label lblFormat;
    private System.Windows.Forms.TextBox txtFormat;
    private System.Windows.Forms.TextBox txtFileLength;
    private System.Windows.Forms.Label lblFileLength;
    private System.Windows.Forms.TextBox txtTitle;
    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.TextBox txtComposer;
    private System.Windows.Forms.Label lblComposer;
    private System.Windows.Forms.SaveFileDialog dlgSaveWave;
    private System.Windows.Forms.Button btnDecode;
    private System.Windows.Forms.Button btnPlay;
    private System.Windows.Forms.Button btnPause;
    private System.Windows.Forms.Button btnStop;
    private System.Windows.Forms.CheckBox chkBufferedPlayback;
    private System.Windows.Forms.ToolTip ttInfo;

    private System.ComponentModel.IContainer components;

    #endregion

    #region Windows Form Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      this.components = new System.ComponentModel.Container();
      this.ilMusicBrowserIcons = new System.Windows.Forms.ImageList(this.components);
      this.pnlInfoArea = new System.Windows.Forms.Panel();
      this.chkBufferedPlayback = new System.Windows.Forms.CheckBox();
      this.btnStop = new System.Windows.Forms.Button();
      this.btnPause = new System.Windows.Forms.Button();
      this.btnPlay = new System.Windows.Forms.Button();
      this.btnDecode = new System.Windows.Forms.Button();
      this.grpFileInfo = new System.Windows.Forms.GroupBox();
      this.txtComposer = new System.Windows.Forms.TextBox();
      this.lblComposer = new System.Windows.Forms.Label();
      this.txtTitle = new System.Windows.Forms.TextBox();
      this.lblTitle = new System.Windows.Forms.Label();
      this.txtFileLength = new System.Windows.Forms.TextBox();
      this.lblFileLength = new System.Windows.Forms.Label();
      this.txtFormat = new System.Windows.Forms.TextBox();
      this.lblFormat = new System.Windows.Forms.Label();
      this.txtFileType = new System.Windows.Forms.TextBox();
      this.lblFileType = new System.Windows.Forms.Label();
      this.txtLocation = new System.Windows.Forms.TextBox();
      this.lblLocation = new System.Windows.Forms.Label();
      this.tabBrowsers = new System.Windows.Forms.TabControl();
      this.tabMusicBrowser = new System.Windows.Forms.TabPage();
      this.tvMusicBrowser = new System.Windows.Forms.TreeView();
      this.tabSoundBrowser = new System.Windows.Forms.TabPage();
      this.tvSoundBrowser = new System.Windows.Forms.TreeView();
      this.ilSoundBrowserIcons = new System.Windows.Forms.ImageList(this.components);
      this.dlgSaveWave = new System.Windows.Forms.SaveFileDialog();
      this.ttInfo = new System.Windows.Forms.ToolTip(this.components);
      this.pnlInfoArea.SuspendLayout();
      this.grpFileInfo.SuspendLayout();
      this.tabBrowsers.SuspendLayout();
      this.tabMusicBrowser.SuspendLayout();
      this.tabSoundBrowser.SuspendLayout();
      this.SuspendLayout();
      // 
      // ilMusicBrowserIcons
      // 
      this.ilMusicBrowserIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
      this.ilMusicBrowserIcons.ImageSize = new System.Drawing.Size(16, 16);
      this.ilMusicBrowserIcons.TransparentColor = System.Drawing.Color.Transparent;
      // 
      // pnlInfoArea
      // 
      this.pnlInfoArea.Controls.Add(this.chkBufferedPlayback);
      this.pnlInfoArea.Controls.Add(this.btnStop);
      this.pnlInfoArea.Controls.Add(this.btnPause);
      this.pnlInfoArea.Controls.Add(this.btnPlay);
      this.pnlInfoArea.Controls.Add(this.btnDecode);
      this.pnlInfoArea.Controls.Add(this.grpFileInfo);
      this.pnlInfoArea.Dock = System.Windows.Forms.DockStyle.Right;
      this.pnlInfoArea.Location = new System.Drawing.Point(392, 0);
      this.pnlInfoArea.Name = "pnlInfoArea";
      this.pnlInfoArea.Size = new System.Drawing.Size(412, 490);
      this.pnlInfoArea.TabIndex = 1;
      // 
      // chkBufferedPlayback
      // 
      this.chkBufferedPlayback.AutoSize = true;
      this.chkBufferedPlayback.Checked = true;
      this.chkBufferedPlayback.CheckState = System.Windows.Forms.CheckState.Checked;
      this.chkBufferedPlayback.Location = new System.Drawing.Point(4, 203);
      this.chkBufferedPlayback.Name = "chkBufferedPlayback";
      this.chkBufferedPlayback.Size = new System.Drawing.Size(135, 17);
      this.chkBufferedPlayback.TabIndex = 667;
      this.chkBufferedPlayback.Text = "Use &Buffered Playback";
      this.ttInfo.SetToolTip(this.chkBufferedPlayback, "If checked, playback will use a buffering thread, resulting a less memory use (and enabling playback of longer tracks such as Castle Zvahl and Yuhtunga Jungle).\nHowever, this will cause skipping and/or garbled playback if another DirectSound application (such as the POL Viewer) is open.");
      this.chkBufferedPlayback.UseVisualStyleBackColor = true;
      // 
      // btnStop
      // 
      this.btnStop.Enabled = false;
      this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnStop.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.btnStop.Location = new System.Drawing.Point(164, 172);
      this.btnStop.Name = "btnStop";
      this.btnStop.Size = new System.Drawing.Size(76, 24);
      this.btnStop.TabIndex = 3;
      this.btnStop.Text = "&Stop";
      this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
      // 
      // btnPause
      // 
      this.btnPause.Enabled = false;
      this.btnPause.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnPause.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.btnPause.Location = new System.Drawing.Point(84, 172);
      this.btnPause.Name = "btnPause";
      this.btnPause.Size = new System.Drawing.Size(76, 24);
      this.btnPause.TabIndex = 2;
      this.btnPause.Text = "&Pause";
      this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
      // 
      // btnPlay
      // 
      this.btnPlay.Enabled = false;
      this.btnPlay.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnPlay.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.btnPlay.Location = new System.Drawing.Point(4, 172);
      this.btnPlay.Name = "btnPlay";
      this.btnPlay.Size = new System.Drawing.Size(76, 24);
      this.btnPlay.TabIndex = 1;
      this.btnPlay.Text = "&Play";
      this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
      // 
      // btnDecode
      // 
      this.btnDecode.Enabled = false;
      this.btnDecode.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnDecode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.btnDecode.Location = new System.Drawing.Point(328, 172);
      this.btnDecode.Name = "btnDecode";
      this.btnDecode.Size = new System.Drawing.Size(76, 24);
      this.btnDecode.TabIndex = 3;
      this.btnDecode.Text = "&Decode...";
      this.btnDecode.Click += new System.EventHandler(this.btnDecode_Click);
      // 
      // grpFileInfo
      // 
      this.grpFileInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.grpFileInfo.Controls.Add(this.txtComposer);
      this.grpFileInfo.Controls.Add(this.lblComposer);
      this.grpFileInfo.Controls.Add(this.txtTitle);
      this.grpFileInfo.Controls.Add(this.lblTitle);
      this.grpFileInfo.Controls.Add(this.txtFileLength);
      this.grpFileInfo.Controls.Add(this.lblFileLength);
      this.grpFileInfo.Controls.Add(this.txtFormat);
      this.grpFileInfo.Controls.Add(this.lblFormat);
      this.grpFileInfo.Controls.Add(this.txtFileType);
      this.grpFileInfo.Controls.Add(this.lblFileType);
      this.grpFileInfo.Controls.Add(this.txtLocation);
      this.grpFileInfo.Controls.Add(this.lblLocation);
      this.grpFileInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.grpFileInfo.Location = new System.Drawing.Point(4, 4);
      this.grpFileInfo.Name = "grpFileInfo";
      this.grpFileInfo.Size = new System.Drawing.Size(400, 164);
      this.grpFileInfo.TabIndex = 666;
      this.grpFileInfo.TabStop = false;
      this.grpFileInfo.Text = "File Information";
      // 
      // txtComposer
      // 
      this.txtComposer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtComposer.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtComposer.Location = new System.Drawing.Point(64, 132);
      this.txtComposer.Name = "txtComposer";
      this.txtComposer.ReadOnly = true;
      this.txtComposer.Size = new System.Drawing.Size(328, 20);
      this.txtComposer.TabIndex = 11;
      // 
      // lblComposer
      // 
      this.lblComposer.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblComposer.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblComposer.Location = new System.Drawing.Point(8, 136);
      this.lblComposer.Name = "lblComposer";
      this.lblComposer.Size = new System.Drawing.Size(52, 16);
      this.lblComposer.TabIndex = 10;
      this.lblComposer.Text = "Composer:";
      // 
      // txtTitle
      // 
      this.txtTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtTitle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtTitle.Location = new System.Drawing.Point(64, 104);
      this.txtTitle.Name = "txtTitle";
      this.txtTitle.ReadOnly = true;
      this.txtTitle.Size = new System.Drawing.Size(328, 20);
      this.txtTitle.TabIndex = 9;
      // 
      // lblTitle
      // 
      this.lblTitle.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblTitle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblTitle.Location = new System.Drawing.Point(8, 108);
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.Size = new System.Drawing.Size(52, 16);
      this.lblTitle.TabIndex = 8;
      this.lblTitle.Text = "Title:";
      // 
      // txtFileLength
      // 
      this.txtFileLength.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtFileLength.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtFileLength.Location = new System.Drawing.Point(312, 76);
      this.txtFileLength.Name = "txtFileLength";
      this.txtFileLength.ReadOnly = true;
      this.txtFileLength.Size = new System.Drawing.Size(80, 20);
      this.txtFileLength.TabIndex = 7;
      // 
      // lblFileLength
      // 
      this.lblFileLength.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblFileLength.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblFileLength.Location = new System.Drawing.Point(272, 80);
      this.lblFileLength.Name = "lblFileLength";
      this.lblFileLength.Size = new System.Drawing.Size(40, 16);
      this.lblFileLength.TabIndex = 6;
      this.lblFileLength.Text = "Length:";
      // 
      // txtFormat
      // 
      this.txtFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtFormat.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtFormat.Location = new System.Drawing.Point(64, 76);
      this.txtFormat.Name = "txtFormat";
      this.txtFormat.ReadOnly = true;
      this.txtFormat.Size = new System.Drawing.Size(200, 20);
      this.txtFormat.TabIndex = 5;
      // 
      // lblFormat
      // 
      this.lblFormat.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblFormat.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblFormat.Location = new System.Drawing.Point(8, 80);
      this.lblFormat.Name = "lblFormat";
      this.lblFormat.Size = new System.Drawing.Size(52, 16);
      this.lblFormat.TabIndex = 4;
      this.lblFormat.Text = "Format:";
      // 
      // txtFileType
      // 
      this.txtFileType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtFileType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtFileType.Location = new System.Drawing.Point(64, 48);
      this.txtFileType.Name = "txtFileType";
      this.txtFileType.ReadOnly = true;
      this.txtFileType.Size = new System.Drawing.Size(328, 20);
      this.txtFileType.TabIndex = 3;
      // 
      // lblFileType
      // 
      this.lblFileType.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblFileType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblFileType.Location = new System.Drawing.Point(8, 52);
      this.lblFileType.Name = "lblFileType";
      this.lblFileType.Size = new System.Drawing.Size(52, 16);
      this.lblFileType.TabIndex = 2;
      this.lblFileType.Text = "File Type:";
      // 
      // txtLocation
      // 
      this.txtLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtLocation.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtLocation.Location = new System.Drawing.Point(64, 20);
      this.txtLocation.Name = "txtLocation";
      this.txtLocation.ReadOnly = true;
      this.txtLocation.Size = new System.Drawing.Size(328, 20);
      this.txtLocation.TabIndex = 1;
      // 
      // lblLocation
      // 
      this.lblLocation.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblLocation.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblLocation.Location = new System.Drawing.Point(8, 24);
      this.lblLocation.Name = "lblLocation";
      this.lblLocation.Size = new System.Drawing.Size(52, 16);
      this.lblLocation.TabIndex = 0;
      this.lblLocation.Text = "Location:";
      // 
      // tabBrowsers
      // 
      this.tabBrowsers.Controls.Add(this.tabMusicBrowser);
      this.tabBrowsers.Controls.Add(this.tabSoundBrowser);
      this.tabBrowsers.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabBrowsers.ImageList = this.ilMusicBrowserIcons;
      this.tabBrowsers.ItemSize = new System.Drawing.Size(42, 19);
      this.tabBrowsers.Location = new System.Drawing.Point(0, 0);
      this.tabBrowsers.Name = "tabBrowsers";
      this.tabBrowsers.SelectedIndex = 0;
      this.tabBrowsers.Size = new System.Drawing.Size(392, 490);
      this.tabBrowsers.TabIndex = 1;
      this.tabBrowsers.SelectedIndexChanged += new System.EventHandler(this.tabBrowsers_SelectedIndexChanged);
      // 
      // tabMusicBrowser
      // 
      this.tabMusicBrowser.Controls.Add(this.tvMusicBrowser);
      this.tabMusicBrowser.Location = new System.Drawing.Point(4, 23);
      this.tabMusicBrowser.Name = "tabMusicBrowser";
      this.tabMusicBrowser.Size = new System.Drawing.Size(384, 463);
      this.tabMusicBrowser.TabIndex = 0;
      this.tabMusicBrowser.Text = "Music";
      // 
      // tvMusicBrowser
      // 
      this.tvMusicBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tvMusicBrowser.HideSelection = false;
      this.tvMusicBrowser.HotTracking = true;
      this.tvMusicBrowser.ImageIndex = 0;
      this.tvMusicBrowser.ImageList = this.ilMusicBrowserIcons;
      this.tvMusicBrowser.Indent = 19;
      this.tvMusicBrowser.ItemHeight = 16;
      this.tvMusicBrowser.Location = new System.Drawing.Point(0, 0);
      this.tvMusicBrowser.Name = "tvMusicBrowser";
      this.tvMusicBrowser.SelectedImageIndex = 0;
      this.tvMusicBrowser.Size = new System.Drawing.Size(384, 463);
      this.tvMusicBrowser.TabIndex = 0;
      this.tvMusicBrowser.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tvBrowser_AfterCollapse);
      this.tvMusicBrowser.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvBrowser_AfterExpand);
      this.tvMusicBrowser.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvBrowser_AfterSelect);
      // 
      // tabSoundBrowser
      // 
      this.tabSoundBrowser.Controls.Add(this.tvSoundBrowser);
      this.tabSoundBrowser.Location = new System.Drawing.Point(4, 23);
      this.tabSoundBrowser.Name = "tabSoundBrowser";
      this.tabSoundBrowser.Size = new System.Drawing.Size(384, 463);
      this.tabSoundBrowser.TabIndex = 1;
      this.tabSoundBrowser.Text = "Sound Effects";
      // 
      // tvSoundBrowser
      // 
      this.tvSoundBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tvSoundBrowser.HideSelection = false;
      this.tvSoundBrowser.HotTracking = true;
      this.tvSoundBrowser.ImageIndex = 0;
      this.tvSoundBrowser.ImageList = this.ilSoundBrowserIcons;
      this.tvSoundBrowser.Indent = 19;
      this.tvSoundBrowser.ItemHeight = 16;
      this.tvSoundBrowser.Location = new System.Drawing.Point(0, 0);
      this.tvSoundBrowser.Name = "tvSoundBrowser";
      this.tvSoundBrowser.SelectedImageIndex = 0;
      this.tvSoundBrowser.Size = new System.Drawing.Size(384, 463);
      this.tvSoundBrowser.TabIndex = 0;
      this.tvSoundBrowser.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tvBrowser_AfterCollapse);
      this.tvSoundBrowser.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvBrowser_AfterExpand);
      this.tvSoundBrowser.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvBrowser_AfterSelect);
      // 
      // ilSoundBrowserIcons
      // 
      this.ilSoundBrowserIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
      this.ilSoundBrowserIcons.ImageSize = new System.Drawing.Size(16, 16);
      this.ilSoundBrowserIcons.TransparentColor = System.Drawing.Color.Transparent;
      // 
      // dlgSaveWave
      // 
      this.dlgSaveWave.DefaultExt = "wav";
      this.dlgSaveWave.Filter = "Wave Files (*.wav)|*.wav|All Files (*.*)|*.*";
      this.dlgSaveWave.RestoreDirectory = true;
      this.dlgSaveWave.Title = "Decode To Wave...";
      // 
      // MainWindow
      // 
      this.ClientSize = new System.Drawing.Size(804, 490);
      this.Controls.Add(this.tabBrowsers);
      this.Controls.Add(this.pnlInfoArea);
      this.Name = "MainWindow";
      this.Text = "PlayOnline Audio Manager";
      this.Closed += new System.EventHandler(this.MainWindow_Closed);
      this.VisibleChanged += new System.EventHandler(this.MainWindow_VisibleChanged);
      this.pnlInfoArea.ResumeLayout(false);
      this.pnlInfoArea.PerformLayout();
      this.grpFileInfo.ResumeLayout(false);
      this.grpFileInfo.PerformLayout();
      this.tabBrowsers.ResumeLayout(false);
      this.tabMusicBrowser.ResumeLayout(false);
      this.tabSoundBrowser.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

  }

}
