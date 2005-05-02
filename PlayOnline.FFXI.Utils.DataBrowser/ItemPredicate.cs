using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using PlayOnline.Core;

namespace PlayOnline.FFXI.Utils.DataBrowser {

  internal class ItemPredicate : System.Windows.Forms.UserControl {

    private enum Test {
      Contains,
      DoesntContain,
      StartsWith,
      EndsWith,
      Equals,
      MatchesRegexp,
      DoesntMatchRegexp
    }

    #region Controls

    private System.Windows.Forms.ComboBox cmbTest;
    private System.Windows.Forms.Button   btnChooseField;
    private System.Windows.Forms.TextBox  txtTestParameter;
    private System.Windows.Forms.TextBox  txtField;

    private System.ComponentModel.Container components = null;

    #endregion

    public ItemPredicate() {
      InitializeComponent();
      this.cmbTest.Items.AddRange(NamedEnum.GetAll(typeof(Test)));
      this.cmbTest.SelectedIndex = 0; // Contains
      this.L = ItemDataLanguage.English;
      this.T = ItemDataType.Object;
      this.Fields = new ArrayList();
      this.Fields.Add(ItemField.Any);
      this.SetFieldText();
    }

    private ItemDataLanguage L;
    private ItemDataType     T;
    private ArrayList        Fields;

    public ItemDataLanguage Language {
      get {
	return this.L;
      }
      set {
	if (value != this.L) {
	  this.L = value;
	  this.Fields.Clear();
	  this.Fields.Add(ItemField.Any);
	  this.SetFieldText();
	}
      }
    }

    public ItemDataType Type {
      get {
	return this.T;
      }
      set {
	if (value != this.T) {
	  this.T = value;
	  this.Fields.Clear();
	  this.Fields.Add(ItemField.Any);
	  this.SetFieldText();
	}
      }
    }

    #region Applying The Selected Predicate

    private bool MatchString(string S) {
      if (S == null)
	return false;
    NamedEnum NE = this.cmbTest.SelectedItem as NamedEnum;
      if (NE == null)
	return false;
    Test T = (Test) NE.Value;
      switch (T) {
	case Test.StartsWith:    return S.StartsWith(this.txtTestParameter.Text);
	case Test.EndsWith:      return S.EndsWith(this.txtTestParameter.Text);
	case Test.Equals:        return (S == this.txtTestParameter.Text);
	case Test.Contains: case Test.DoesntContain: {
	int Pos = S.IndexOf(this.txtTestParameter.Text);
	  return ((T == Test.Contains) ? (Pos >= 0) : (Pos < 0));
	}
	case Test.MatchesRegexp: case Test.DoesntMatchRegexp: {
	Regex RE = new Regex(this.txtTestParameter.Text, RegexOptions.Multiline | RegexOptions.ExplicitCapture);
	  return ((T == Test.MatchesRegexp) ? RE.IsMatch(S) : !RE.IsMatch(S));
	}
      }
      return false;
    }

    public bool IsMatch(FFXIItem I) {
    FFXIItem.IItemInfo II = I.GetInfo(this.L, this.T);
      if (II != null) {
	// Any => OR of test on all fields
	if (this.Fields.Contains(ItemField.Any)) {
	  foreach (ItemField IF in II.GetFields()) {
	    if (this.MatchString(II.GetFieldText(IF)))
	      return true;
	  }
	  return false;
	}
	// All => AND of test on all fields
	if (this.Fields.Contains(ItemField.All)) {
	  foreach (ItemField IF in II.GetFields()) {
	    if (!this.MatchString(II.GetFieldText(IF)))
	      return false;
	  }
	  return true;
	}
	// Otherwise: OR of all selected fields
	foreach (ItemField IF in this.Fields) {
	  if (this.MatchString(II.GetFieldText(IF)))
	    return true;
	}
      }
      return false;
    }

    #endregion

    #region Component Designer generated code

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
	components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent() {
      this.cmbTest = new System.Windows.Forms.ComboBox();
      this.btnChooseField = new System.Windows.Forms.Button();
      this.txtTestParameter = new System.Windows.Forms.TextBox();
      this.txtField = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // cmbTest
      // 
      this.cmbTest.DisplayMember = "Name";
      this.cmbTest.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cmbTest.ItemHeight = 13;
      this.cmbTest.Location = new System.Drawing.Point(228, 0);
      this.cmbTest.Name = "cmbTest";
      this.cmbTest.Size = new System.Drawing.Size(136, 21);
      this.cmbTest.TabIndex = 3;
      // 
      // btnChooseField
      // 
      this.btnChooseField.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.btnChooseField.ImeMode = System.Windows.Forms.ImeMode.NoControl;
      this.btnChooseField.Location = new System.Drawing.Point(204, 0);
      this.btnChooseField.Name = "btnChooseField";
      this.btnChooseField.Size = new System.Drawing.Size(24, 20);
      this.btnChooseField.TabIndex = 2;
      this.btnChooseField.Text = "...";
      this.btnChooseField.Click += new System.EventHandler(this.btnChooseField_Click);
      // 
      // txtTestParameter
      // 
      this.txtTestParameter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
	| System.Windows.Forms.AnchorStyles.Left) 
	| System.Windows.Forms.AnchorStyles.Right)));
      this.txtTestParameter.Location = new System.Drawing.Point(364, 0);
      this.txtTestParameter.Name = "txtTestParameter";
      this.txtTestParameter.Size = new System.Drawing.Size(348, 20);
      this.txtTestParameter.TabIndex = 4;
      this.txtTestParameter.Text = "";
      // 
      // txtField
      // 
      this.txtField.Location = new System.Drawing.Point(0, 0);
      this.txtField.Name = "txtField";
      this.txtField.ReadOnly = true;
      this.txtField.Size = new System.Drawing.Size(204, 20);
      this.txtField.TabIndex = 1;
      this.txtField.Text = "";
      // 
      // ItemPredicate
      // 
      this.BackColor = System.Drawing.SystemColors.Control;
      this.Controls.Add(this.cmbTest);
      this.Controls.Add(this.btnChooseField);
      this.Controls.Add(this.txtTestParameter);
      this.Controls.Add(this.txtField);
      this.Name = "ItemPredicate";
      this.Size = new System.Drawing.Size(712, 20);
      this.ResumeLayout(false);

    }

    #endregion

    private void SetFieldText() {
      this.txtField.Text = String.Format("{0}\u2192{1}\u2192", new NamedEnum(this.L).Name, new NamedEnum(this.T).Name);
    bool FieldAdded = false;
      foreach (ItemField IF in this.Fields) {
	if (FieldAdded)
	  this.txtField.Text += ", ";
	this.txtField.Text += new NamedEnum(IF).Name;
	FieldAdded = true;
      }
    }

    private void btnChooseField_Click(object sender, System.EventArgs e) {
      using (ItemFieldChooser IFC = new ItemFieldChooser(this.Language, this.Type, true, (ItemField[]) this.Fields.ToArray(typeof(ItemField)))) {
	if (IFC.ShowDialog(this) == DialogResult.OK) {
	  this.Fields = IFC.Fields;
	  this.SetFieldText();
	}
      }
    }

  }

}
