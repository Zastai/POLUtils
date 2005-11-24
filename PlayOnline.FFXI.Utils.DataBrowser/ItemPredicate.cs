// $Id$

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using PlayOnline.Core;

namespace PlayOnline.FFXI.Utils.DataBrowser {

  internal partial class ItemPredicate : UserControl {

    private enum Test {
      Contains,
      DoesntContain,
      StartsWith,
      EndsWith,
      Equals,
      MatchesRegexp,
      DoesntMatchRegexp
    }

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

    public string ValidateQuery() {
    NamedEnum NE = this.cmbTest.SelectedItem as NamedEnum;
      if (NE == null)
	return I18N.GetText("Query:NoQueryType");
    Test T = (Test) NE.Value;
      switch (T) {
	case Test.StartsWith: case Test.EndsWith: case Test.Contains: case Test.DoesntContain:
	  if (this.txtTestParameter.Text == String.Empty)
	    return I18N.GetText("Query:NoEmptyString");
	  return null;
	case Test.Equals:
	  return null;
	case Test.MatchesRegexp: case Test.DoesntMatchRegexp:
	  if (this.txtTestParameter.Text == String.Empty)
	    return I18N.GetText("Query:NoEmptyString");
	  try { // Try to parse the regex
	    Regex RE = new Regex(this.txtTestParameter.Text, RegexOptions.Multiline | RegexOptions.ExplicitCapture);
	  } catch { return I18N.GetText("Query:BadRegexp"); }
	  return null;
      }
      return I18N.GetText("Query:BadQueryType");
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
