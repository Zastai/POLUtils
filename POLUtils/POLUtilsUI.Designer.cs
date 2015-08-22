// $Id$

// Copyright © 2004-2010 Tim Van Holder
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

namespace POLUtils {

  public partial class POLUtilsUI {

    #region Windows Form Designer generated code

    private System.ComponentModel.Container components = null;

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      this.btnTetraViewer = new System.Windows.Forms.Button();
      this.grpRegion = new System.Windows.Forms.GroupBox();
      this.btnChooseRegion = new System.Windows.Forms.Button();
      this.txtSelectedRegion = new System.Windows.Forms.TextBox();
      this.lblSelectedRegion = new System.Windows.Forms.Label();
      this.btnAudioManager = new System.Windows.Forms.Button();
      this.btnFFXIMacroManager = new System.Windows.Forms.Button();
      this.btnFFXIDataBrowser = new System.Windows.Forms.Button();
      this.btnFFXIConfigEditor = new System.Windows.Forms.Button();
      this.btnFFXIItemComparison = new System.Windows.Forms.Button();
      this.btnFFXIEngrishOnry = new System.Windows.Forms.Button();
      this.btnFFXIStrangeApparatus = new System.Windows.Forms.Button();
      this.btnFFXINPCRenamer = new System.Windows.Forms.Button();
      this.btnFFXITCDataBrowser = new System.Windows.Forms.Button();
      this.grpRegion.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnTetraViewer
      // 
      this.btnTetraViewer.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnTetraViewer.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.btnTetraViewer.Location = new System.Drawing.Point(184, 132);
      this.btnTetraViewer.Name = "btnTetraViewer";
      this.btnTetraViewer.Size = new System.Drawing.Size(172, 24);
      this.btnTetraViewer.TabIndex = 8;
      this.btnTetraViewer.Text = "&Tetra Master Data Viewer";
      this.btnTetraViewer.Click += new System.EventHandler(this.btnTetraViewer_Click);
      // 
      // grpRegion
      // 
      this.grpRegion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.grpRegion.Controls.Add(this.btnChooseRegion);
      this.grpRegion.Controls.Add(this.txtSelectedRegion);
      this.grpRegion.Controls.Add(this.lblSelectedRegion);
      this.grpRegion.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.grpRegion.Location = new System.Drawing.Point(8, 0);
      this.grpRegion.Name = "grpRegion";
      this.grpRegion.Size = new System.Drawing.Size(348, 43);
      this.grpRegion.TabIndex = 0;
      this.grpRegion.TabStop = false;
      // 
      // btnChooseRegion
      // 
      this.btnChooseRegion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnChooseRegion.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnChooseRegion.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.btnChooseRegion.Location = new System.Drawing.Point(316, 16);
      this.btnChooseRegion.Name = "btnChooseRegion";
      this.btnChooseRegion.Size = new System.Drawing.Size(23, 20);
      this.btnChooseRegion.TabIndex = 101;
      this.btnChooseRegion.Text = "...";
      this.btnChooseRegion.Click += new System.EventHandler(this.btnChooseRegion_Click);
      // 
      // txtSelectedRegion
      // 
      this.txtSelectedRegion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtSelectedRegion.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.txtSelectedRegion.Location = new System.Drawing.Point(132, 16);
      this.txtSelectedRegion.Name = "txtSelectedRegion";
      this.txtSelectedRegion.ReadOnly = true;
      this.txtSelectedRegion.Size = new System.Drawing.Size(180, 20);
      this.txtSelectedRegion.TabIndex = 100;
      this.txtSelectedRegion.TabStop = false;
      // 
      // lblSelectedRegion
      // 
      this.lblSelectedRegion.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblSelectedRegion.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.lblSelectedRegion.Location = new System.Drawing.Point(8, 19);
      this.lblSelectedRegion.Name = "lblSelectedRegion";
      this.lblSelectedRegion.Size = new System.Drawing.Size(124, 16);
      this.lblSelectedRegion.TabIndex = 0;
      this.lblSelectedRegion.Text = "PlayOnline Client Region:";
      // 
      // btnAudioManager
      // 
      this.btnAudioManager.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnAudioManager.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.btnAudioManager.Location = new System.Drawing.Point(8, 48);
      this.btnAudioManager.Name = "btnAudioManager";
      this.btnAudioManager.Size = new System.Drawing.Size(172, 24);
      this.btnAudioManager.TabIndex = 1;
      this.btnAudioManager.Text = "&Audio Manager";
      this.btnAudioManager.Click += new System.EventHandler(this.btnAudioManager_Click);
      // 
      // btnFFXIMacroManager
      // 
      this.btnFFXIMacroManager.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnFFXIMacroManager.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.btnFFXIMacroManager.Location = new System.Drawing.Point(8, 104);
      this.btnFFXIMacroManager.Name = "btnFFXIMacroManager";
      this.btnFFXIMacroManager.Size = new System.Drawing.Size(172, 24);
      this.btnFFXIMacroManager.TabIndex = 5;
      this.btnFFXIMacroManager.Text = "FFXI &Macro Manager";
      this.btnFFXIMacroManager.Click += new System.EventHandler(this.btnFFXIMacroManager_Click);
      // 
      // btnFFXIDataBrowser
      // 
      this.btnFFXIDataBrowser.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnFFXIDataBrowser.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.btnFFXIDataBrowser.Location = new System.Drawing.Point(8, 76);
      this.btnFFXIDataBrowser.Name = "btnFFXIDataBrowser";
      this.btnFFXIDataBrowser.Size = new System.Drawing.Size(172, 24);
      this.btnFFXIDataBrowser.TabIndex = 3;
      this.btnFFXIDataBrowser.Text = "FFXI &Data Browser";
      this.btnFFXIDataBrowser.Click += new System.EventHandler(this.btnFFXIDataBrowser_Click);
      // 
      // btnFFXIConfigEditor
      // 
      this.btnFFXIConfigEditor.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnFFXIConfigEditor.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.btnFFXIConfigEditor.Location = new System.Drawing.Point(184, 48);
      this.btnFFXIConfigEditor.Name = "btnFFXIConfigEditor";
      this.btnFFXIConfigEditor.Size = new System.Drawing.Size(172, 24);
      this.btnFFXIConfigEditor.TabIndex = 2;
      this.btnFFXIConfigEditor.Text = "FFXI &Configuration Editor";
      this.btnFFXIConfigEditor.Click += new System.EventHandler(this.btnFFXIConfigEditor_Click);
      // 
      // btnFFXIItemComparison
      // 
      this.btnFFXIItemComparison.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnFFXIItemComparison.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.btnFFXIItemComparison.Location = new System.Drawing.Point(184, 76);
      this.btnFFXIItemComparison.Name = "btnFFXIItemComparison";
      this.btnFFXIItemComparison.Size = new System.Drawing.Size(172, 24);
      this.btnFFXIItemComparison.TabIndex = 4;
      this.btnFFXIItemComparison.Text = "FFXI &Item Data Comparison";
      this.btnFFXIItemComparison.Click += new System.EventHandler(this.btnFFXIItemComparison_Click);
      // 
      // btnFFXIEngrishOnry
      // 
      this.btnFFXIEngrishOnry.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnFFXIEngrishOnry.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.btnFFXIEngrishOnry.Location = new System.Drawing.Point(8, 132);
      this.btnFFXIEngrishOnry.Name = "btnFFXIEngrishOnry";
      this.btnFFXIEngrishOnry.Size = new System.Drawing.Size(172, 24);
      this.btnFFXIEngrishOnry.TabIndex = 7;
      this.btnFFXIEngrishOnry.Text = "\"&Engrish Onry\"";
      this.btnFFXIEngrishOnry.Click += new System.EventHandler(this.btnFFXIEngrishOnry_Click);
      // 
      // btnFFXIStrangeApparatus
      // 
      this.btnFFXIStrangeApparatus.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnFFXIStrangeApparatus.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.btnFFXIStrangeApparatus.Location = new System.Drawing.Point(184, 104);
      this.btnFFXIStrangeApparatus.Name = "btnFFXIStrangeApparatus";
      this.btnFFXIStrangeApparatus.Size = new System.Drawing.Size(172, 24);
      this.btnFFXIStrangeApparatus.TabIndex = 6;
      this.btnFFXIStrangeApparatus.Text = "FFXI &Strange Apparatus Codes";
      this.btnFFXIStrangeApparatus.Click += new System.EventHandler(this.btnFFXIStrangeApparatus_Click);
      // 
      // btnFFXINPCRenamer
      // 
      this.btnFFXINPCRenamer.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnFFXINPCRenamer.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.btnFFXINPCRenamer.Location = new System.Drawing.Point(8, 160);
      this.btnFFXINPCRenamer.Name = "btnFFXINPCRenamer";
      this.btnFFXINPCRenamer.Size = new System.Drawing.Size(172, 24);
      this.btnFFXINPCRenamer.TabIndex = 9;
      this.btnFFXINPCRenamer.Text = "FFXI NPC Renamer";
      this.btnFFXINPCRenamer.Click += new System.EventHandler(this.btnFFXINPCRenamer_Click);
      // 
      // btnFFXITCDataBrowser
      // 
      this.btnFFXITCDataBrowser.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnFFXITCDataBrowser.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.btnFFXITCDataBrowser.Location = new System.Drawing.Point(184, 160);
      this.btnFFXITCDataBrowser.Name = "btnFFXITCDataBrowser";
      this.btnFFXITCDataBrowser.Size = new System.Drawing.Size(172, 24);
      this.btnFFXITCDataBrowser.TabIndex = 10;
      this.btnFFXITCDataBrowser.Text = "FFXITC &Data Browser";
      this.btnFFXITCDataBrowser.Click += new System.EventHandler(this.btnFFXITCDataBrowser_Click);
      // 
      // POLUtilsUI
      // 
      this.ClientSize = new System.Drawing.Size(362, 190);
      this.Controls.Add(this.btnFFXITCDataBrowser);
      this.Controls.Add(this.btnFFXINPCRenamer);
      this.Controls.Add(this.btnFFXIStrangeApparatus);
      this.Controls.Add(this.btnFFXIEngrishOnry);
      this.Controls.Add(this.btnFFXIItemComparison);
      this.Controls.Add(this.btnFFXIConfigEditor);
      this.Controls.Add(this.btnFFXIDataBrowser);
      this.Controls.Add(this.btnFFXIMacroManager);
      this.Controls.Add(this.btnAudioManager);
      this.Controls.Add(this.grpRegion);
      this.Controls.Add(this.btnTetraViewer);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.MaximizeBox = false;
      this.Name = "POLUtilsUI";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "PlayOnline Utilities";
      this.grpRegion.ResumeLayout(false);
      this.grpRegion.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    // Controls
    private System.Windows.Forms.GroupBox grpRegion;
    private System.Windows.Forms.Label lblSelectedRegion;
    private System.Windows.Forms.TextBox txtSelectedRegion;
    private System.Windows.Forms.Button btnChooseRegion;
    private System.Windows.Forms.Button btnAudioManager;
    private System.Windows.Forms.Button btnFFXIMacroManager;
    private System.Windows.Forms.Button btnFFXIDataBrowser;
    private System.Windows.Forms.Button btnTetraViewer;
    private System.Windows.Forms.Button btnFFXIConfigEditor;
    private System.Windows.Forms.Button btnFFXIItemComparison;
    private System.Windows.Forms.Button btnFFXIEngrishOnry;
    private System.Windows.Forms.Button btnFFXIStrangeApparatus;
    private System.Windows.Forms.Button btnFFXINPCRenamer;
    private System.Windows.Forms.Button btnFFXITCDataBrowser;

  }

}
