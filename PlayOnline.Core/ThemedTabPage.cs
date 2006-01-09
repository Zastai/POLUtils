using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms {

  public class ThemedTabPage : TabPage {

    public ThemedTabPage() : base() {
    }

    public ThemedTabPage(string text) : base(text) {
    }

    // The UseVisualStyleBackColor only uses a plain background color, not the background rendering for
    // the current visual style.  This corrects that error.
    protected override void OnPaintBackground(PaintEventArgs e) {
      if (!VisualStyleRenderer.IsSupported || !this.UseVisualStyleBackColor) {
	base.OnPaintBackground(e);
	return;
      }
    TabControl P = this.Parent as TabControl;
      if (P == null || P.Appearance != TabAppearance.Normal) {
	base.OnPaintBackground(e);
	return;
      }
    VisualStyleRenderer VSR = new VisualStyleRenderer(VisualStyleElement.Tab.Body.Normal);
      VSR.DrawBackground(e.Graphics, this.ClientRectangle, e.ClipRectangle);
    }

  }

}