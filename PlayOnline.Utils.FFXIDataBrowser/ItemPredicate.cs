using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using PlayOnline.Core;
using PlayOnline.FFXI;

namespace PlayOnline.Utils.FFXIDataBrowser {

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
      this.SetFieldText();
      this.cmbTest.Items.AddRange(new NamedEnum[] { new NamedEnum(Test.Contains), new NamedEnum(Test.StartsWith), new NamedEnum(Test.EndsWith), new NamedEnum(Test.Equals), new NamedEnum(Test.MatchesRegexp) });
      this.cmbTest.SelectedIndex = 0;
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

    private ItemDataLanguage L = ItemDataLanguage.English;
    private ItemDataType     T = ItemDataType.Object;
    private ItemField        F = ItemField.EnglishName;

    public bool IsMatch(FFXIItem I) {
    FFXIItem.IItemInfo II = I.GetInfo(this.L, this.T);
      if (II == null)
	return false;
      else
	return this.MatchString(II.GetFieldText(this.F));
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
      this.txtField.Text = String.Format("{0}\u2192{1}\u2192{2}", new NamedEnum(this.L).Name, new NamedEnum(this.T).Name, new NamedEnum(this.F).Name);
    }

    private void btnChooseField_Click(object sender, System.EventArgs e) {
      using (ItemFieldChooser IFC = new ItemFieldChooser()) {
	if (IFC.ShowDialog(this) == DialogResult.OK) {
	  this.T = IFC.T;
	  this.L = IFC.L;
	  this.F = IFC.F;
	  this.SetFieldText();
	}
      }
    }

  }

}
