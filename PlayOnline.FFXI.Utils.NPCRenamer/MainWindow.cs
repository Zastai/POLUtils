using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace PlayOnline.FFXI.Utils.NPCRenamer {

  public partial class MainWindow : Form {

    private class AreaNPCList {

      private class NPCName {
	public uint   ID;
	public string Name;
      }

      private ushort        AreaID;
      private string        AreaName;
      private List<NPCName> NPCNames;
      private bool          ChangesMade;

      public AreaNPCList(ushort AreaID, string AreaName) {
	this.AreaID      = AreaID;
	this.AreaName    = AreaName;
	this.NPCNames    = null;
	this.ChangesMade = false;
      }

      public override string ToString() { return this.AreaName; }

      public int Count {
	get {
	  if (this.NPCNames == null)
	    this.LoadNames();
	  return this.NPCNames.Count;
	}
      }

      public class NPCNameRef {

	private AreaNPCList L;
	private int Index;

	public NPCNameRef(AreaNPCList L, int Index) {
	  this.L     = L;
	  this.Index = Index;
	}

	public string Name {
	  get { return this.L.GetName(this.Index); }
	  set { this.L.SetName(this.Index, value); }
	}

      }

      private string GetName(int Index) {
	if (this.NPCNames == null)
	  this.LoadNames();
	return this.NPCNames[Index].Name;
      }

      private void SetName(int Index, string Name) {
	if (this.NPCNames == null)
	  this.LoadNames();
	if (Name != this.NPCNames[Index].Name) {
	  this.NPCNames[Index].Name = Name;
	  this.ChangesMade = true;
	}
      }

      private void LoadNames() {
	this.NPCNames = new List<NPCName>();
      string DATFile = FFXI.GetFilePath(6720 + AreaID);
	if (File.Exists(DATFile)) {
	BinaryReader BR = new BinaryReader(new FileStream(DATFile, FileMode.Open, FileAccess.Read));
	  try {
	    while (BR.BaseStream.Position != BR.BaseStream.Length) {
	    NPCName N = new NPCName();
	      N.Name = Encoding.ASCII.GetString(BR.ReadBytes(0x18)).TrimEnd('\0');
	      N.ID = BR.ReadUInt32();
	      this.NPCNames.Add(N);
	    }
	  } catch {
	    this.NPCNames.Clear();
	  }
	  BR.Close();
	}
      }

      public void SaveChanges() {
	if (this.NPCNames != null && this.ChangesMade) {
	string DATFile = FFXI.GetFilePath(6720 + this.AreaID);
	  this.ChangesMade = false;
	  if (File.Exists(DATFile)) {
	  BinaryWriter BW = new BinaryWriter(new FileStream(DATFile + ".new", FileMode.Create, FileAccess.Write));
	    foreach (NPCName N in this.NPCNames) {
	    byte[] NameBytes = Encoding.ASCII.GetBytes(N.Name);
	      if (NameBytes.Length >= 0x18)
		BW.Write(NameBytes, 0, 0x18);
	      else {
		BW.Write(NameBytes);
		for (int i = NameBytes.Length; i < 0x18; ++i)
		  BW.Write((byte) 0);
	      }
	      BW.Write(N.ID);
	    }
	    BW.Close();
	    File.Replace(DATFile + ".new", DATFile, null);
	  }
	}
      }

    }

    public MainWindow() {
      this.InitializeComponent();
    }

    private void MainWindow_Shown(object sender, EventArgs e) {
      this.Enabled = false;
      for (ushort AreaID = 0; AreaID < 256; ++AreaID) {
      string AreaName = FFXIResourceManager.GetAreaName(AreaID);
	if (AreaName == null || AreaName == String.Empty)
	  continue;
	this.cmbArea.Items.Add(new AreaNPCList(AreaID, AreaName));
      }
      if (this.cmbArea.Items.Count > 0)
	this.cmbArea.SelectedIndex = 0;
      this.cmbArea.Select();
      this.Enabled = true;
    }

    private void cmbArea_SelectedIndexChanged(object sender, EventArgs e) {
      this.lstNPCNames.Items.Clear();
    AreaNPCList L = this.cmbArea.SelectedItem as AreaNPCList;
      if (L != null) {
	for (ushort i = 0; i < L.Count; ++i) {
	ListViewItem LVI = new ListViewItem();
	AreaNPCList.NPCNameRef NR = new AreaNPCList.NPCNameRef(L, i);
	  LVI.Tag  = NR;
	  LVI.Text = NR.Name;
	  this.lstNPCNames.Items.Add(LVI);
	}
      }
    }

    private void lstNPCNames_AfterLabelEdit(object sender, LabelEditEventArgs e) {
      if (e.Label == null) // User made no changes to the label text
	return;
    AreaNPCList.NPCNameRef NR = this.lstNPCNames.Items[e.Item].Tag as AreaNPCList.NPCNameRef;
      if (NR != null) {
	if (e.Label.Length > 0x18)
	  NR.Name = e.Label.Substring(0, 0x18);
	else
	  NR.Name = e.Label;
	this.btnSaveChanges.Enabled = true;
      }
    }

    private void lstNPCNames_KeyDown(object sender, KeyEventArgs e) {
      if (e.KeyCode == Keys.F2 && this.lstNPCNames.SelectedItems.Count > 0) {
	this.lstNPCNames.SelectedItems[0].BeginEdit();
	e.Handled = true;
      }
    }

    private void btnSaveChanges_Click(object sender, EventArgs e) {
      foreach (AreaNPCList L in this.cmbArea.Items)
	L.SaveChanges();
      this.btnSaveChanges.Enabled = false;
    }

    private void btnClose_Click(object sender, EventArgs e) {
      this.Close();
    }

  }

}
