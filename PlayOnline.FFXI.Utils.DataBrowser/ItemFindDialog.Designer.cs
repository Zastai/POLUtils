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

  internal partial class ItemFindDialog {

    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing) {
      if (disposing && this.components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent() {
      this.components = new System.ComponentModel.Container();
      this.lstItems = new System.Windows.Forms.ListView();
      this.mnuItemListContext = new System.Windows.Forms.ContextMenu();
      this.mnuILCProperties = new System.Windows.Forms.MenuItem();
      this.mnuILCCopy = new System.Windows.Forms.MenuItem();
      this.mnuILCExport = new System.Windows.Forms.MenuItem();
      this.mnuILCEAll = new System.Windows.Forms.MenuItem();
      this.mnuILCEResults = new System.Windows.Forms.MenuItem();
      this.mnuILCESelected = new System.Windows.Forms.MenuItem();
      this.ilItemIcons = new System.Windows.Forms.ImageList(this.components);
      this.pnlSearchOptions = new System.Windows.Forms.Panel();
      this.chkShowIcons = new System.Windows.Forms.CheckBox();
      this.btnClose = new System.Windows.Forms.Button();
      this.btnRunQuery = new System.Windows.Forms.Button();
      this.stbStatus = new System.Windows.Forms.StatusBar();
      this.dlgExportFile = new System.Windows.Forms.SaveFileDialog();
      this.pnlSearchOptions.SuspendLayout();
      this.SuspendLayout();
      // 
      // lstItems
      // 
      this.lstItems.AllowColumnReorder = true;
      this.lstItems.ContextMenu = this.mnuItemListContext;
      this.lstItems.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lstItems.FullRowSelect = true;
      this.lstItems.GridLines = true;
      this.lstItems.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
      this.lstItems.HideSelection = false;
      this.lstItems.Location = new System.Drawing.Point(0, 28);
      this.lstItems.Name = "lstItems";
      this.lstItems.Size = new System.Drawing.Size(640, 410);
      this.lstItems.TabIndex = 5;
      this.lstItems.UseCompatibleStateImageBehavior = false;
      this.lstItems.View = System.Windows.Forms.View.Details;
      this.lstItems.SelectedIndexChanged += new System.EventHandler(this.lstItems_SelectedIndexChanged);
      this.lstItems.DoubleClick += new System.EventHandler(this.lstItems_DoubleClick);
      // 
      // mnuItemListContext
      // 
      this.mnuItemListContext.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuILCProperties,
            this.mnuILCCopy,
            this.mnuILCExport});
      // 
      // mnuILCProperties
      // 
      this.mnuILCProperties.Index = 0;
      this.mnuILCProperties.Text = "&Properties...";
      this.mnuILCProperties.Click += new System.EventHandler(this.mnuILCProperties_Click);
      // 
      // mnuILCCopy
      // 
      this.mnuILCCopy.Index = 1;
      this.mnuILCCopy.Text = "&Copy";
      // 
      // mnuILCExport
      // 
      this.mnuILCExport.Index = 2;
      this.mnuILCExport.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuILCEAll,
            this.mnuILCEResults,
            this.mnuILCESelected});
      this.mnuILCExport.Text = "&Export";
      // 
      // mnuILCEAll
      // 
      this.mnuILCEAll.Index = 0;
      this.mnuILCEAll.Text = "&All Items...";
      this.mnuILCEAll.Click += new System.EventHandler(this.mnuILCECAll_Click);
      // 
      // mnuILCEResults
      // 
      this.mnuILCEResults.Enabled = false;
      this.mnuILCEResults.Index = 1;
      this.mnuILCEResults.Text = "Search &Results...";
      // 
      // mnuILCESelected
      // 
      this.mnuILCESelected.Enabled = false;
      this.mnuILCESelected.Index = 2;
      this.mnuILCESelected.Text = "&Selected Item(s)...";
      // 
      // ilItemIcons
      // 
      this.ilItemIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
      this.ilItemIcons.ImageSize = new System.Drawing.Size(16, 16);
      this.ilItemIcons.TransparentColor = System.Drawing.Color.Transparent;
      // 
      // pnlSearchOptions
      // 
      this.pnlSearchOptions.Controls.Add(this.chkShowIcons);
      this.pnlSearchOptions.Controls.Add(this.btnClose);
      this.pnlSearchOptions.Controls.Add(this.btnRunQuery);
      this.pnlSearchOptions.Dock = System.Windows.Forms.DockStyle.Top;
      this.pnlSearchOptions.Location = new System.Drawing.Point(0, 0);
      this.pnlSearchOptions.Name = "pnlSearchOptions";
      this.pnlSearchOptions.Size = new System.Drawing.Size(640, 28);
      this.pnlSearchOptions.TabIndex = 7;
      // 
      // chkShowIcons
      // 
      this.chkShowIcons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.chkShowIcons.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.chkShowIcons.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.chkShowIcons.Location = new System.Drawing.Point(388, 6);
      this.chkShowIcons.Name = "chkShowIcons";
      this.chkShowIcons.Size = new System.Drawing.Size(76, 16);
      this.chkShowIcons.TabIndex = 17;
      this.chkShowIcons.Text = "&Show Icons";
      this.chkShowIcons.CheckedChanged += new System.EventHandler(this.chkShowIcons_CheckedChanged);
      // 
      // btnClose
      // 
      this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnClose.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.btnClose.Location = new System.Drawing.Point(561, 3);
      this.btnClose.Name = "btnClose";
      this.btnClose.Size = new System.Drawing.Size(76, 20);
      this.btnClose.TabIndex = 13;
      this.btnClose.Text = "&Close";
      this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
      // 
      // btnRunQuery
      // 
      this.btnRunQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnRunQuery.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnRunQuery.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.btnRunQuery.Location = new System.Drawing.Point(479, 3);
      this.btnRunQuery.Name = "btnRunQuery";
      this.btnRunQuery.Size = new System.Drawing.Size(76, 20);
      this.btnRunQuery.TabIndex = 7;
      this.btnRunQuery.Text = "&Run Query";
      this.btnRunQuery.Click += new System.EventHandler(this.btnRunQuery_Click);
      // 
      // stbStatus
      // 
      this.stbStatus.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.stbStatus.Location = new System.Drawing.Point(0, 420);
      this.stbStatus.Name = "stbStatus";
      this.stbStatus.Size = new System.Drawing.Size(640, 18);
      this.stbStatus.TabIndex = 8;
      this.stbStatus.Visible = false;
      // 
      // dlgExportFile
      // 
      this.dlgExportFile.DefaultExt = "xml";
      this.dlgExportFile.Filter = "XML Files (*.xml)|*.xml|All Files|*.*";
      this.dlgExportFile.Title = "Select File To Export To";
      // 
      // ItemFindDialog
      // 
      this.AcceptButton = this.btnRunQuery;
      this.CancelButton = this.btnClose;
      this.ClientSize = new System.Drawing.Size(640, 438);
      this.Controls.Add(this.stbStatus);
      this.Controls.Add(this.lstItems);
      this.Controls.Add(this.pnlSearchOptions);
      this.Name = "ItemFindDialog";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Find Item(s)";
      this.pnlSearchOptions.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListView lstItems;
    private System.Windows.Forms.ImageList ilItemIcons;
    private System.Windows.Forms.Panel pnlSearchOptions;
    private System.Windows.Forms.Button btnRunQuery;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.StatusBar stbStatus;
    private System.Windows.Forms.CheckBox chkShowIcons;
    private System.Windows.Forms.SaveFileDialog dlgExportFile;
    private System.Windows.Forms.ContextMenu mnuItemListContext;
    private System.Windows.Forms.MenuItem mnuILCProperties;
    private System.Windows.Forms.MenuItem mnuILCCopy;
    private System.Windows.Forms.MenuItem mnuILCExport;
    private System.Windows.Forms.MenuItem mnuILCEAll;
    private System.Windows.Forms.MenuItem mnuILCEResults;
    private System.Windows.Forms.MenuItem mnuILCESelected;
 
  }

}
