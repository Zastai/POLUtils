// Define this to write the WAV header in the WaveWriter class instead of relying on AudioFileStream to
// provide one.  If set, a loop marker will also be added to the WAV file if appropriate.
#define LocalWAVHeader

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using PlayOnline.Core.Audio;

namespace PlayOnline.Utils.AudioManager {

  public class WaveWriter : System.Windows.Forms.Form {

    private System.Windows.Forms.Label lblSource;
    private System.Windows.Forms.Label lblTarget;
    private System.Windows.Forms.TextBox txtSource;
    private System.Windows.Forms.TextBox txtTarget;
    private System.Windows.Forms.ProgressBar prbBytesWritten;
    private System.ComponentModel.Container components = null;

    public WaveWriter(AudioFile AF, string TargetFile) {
      InitializeComponent();
      this.prbBytesWritten.Select();
      this.txtSource.Text = AF.Path;
      this.txtTarget.Text = TargetFile;
      this.AF  = AF;
#if LocalWAVHeader
      this.AFS = AF.OpenStream(false);
#else
      this.AFS = AF.OpenStream(true);
#endif
      this.FS  = new FileStream(TargetFile, FileMode.Create, FileAccess.Write);
    }

    private AudioFile       AF;
    private AudioFileStream AFS;
    private FileStream      FS;

    private readonly int ChunkSize = 16 * 1024;

    private void WriteWave() {
      this.prbBytesWritten.Maximum = (int) this.AFS.Length;
#if LocalWAVHeader
      this.prbBytesWritten.Maximum += 0x2c;
      if (this.AF.Looped)
	this.prbBytesWritten.Maximum += 0x48; // For the "Loop Start" cue marker
      { // Write WAV header
      BinaryWriter BW = new BinaryWriter(this.FS, Encoding.ASCII);
	// File Header
	BW.Write("RIFF".ToCharArray());
	BW.Write(this.prbBytesWritten.Maximum);
	// Wave Format Header
	BW.Write("WAVEfmt ".ToCharArray());
	BW.Write((int) 0x10);
	// Wave Format Data
	BW.Write((short) 1); // PCM
	BW.Write((short) this.AF.Channels);
	BW.Write((int)   this.AF.SampleRate);
	BW.Write((int)   (2 * this.AF.Channels * this.AF.SampleRate)); // bytes per second
	BW.Write((short) (2 * this.AF.Channels)); // bytes per sample
	BW.Write((short) 16); // bits
	// Wave Data Header
	BW.Write("data".ToCharArray());
	BW.Write((int) this.AFS.Length);
      }
      this.prbBytesWritten.Value = 0x2c;
#endif
      // Write PCM data
    byte[] data = new byte[this.ChunkSize];
      while (true) {
      int read = AFS.Read(data, 0, this.ChunkSize);
	this.prbBytesWritten.Value += read;
	FS.Write(data, 0, read);
	if (read != this.ChunkSize)
	  break;
      }
      AFS.Close();
#if LocalWAVHeader
      // Write "Loop Start" cue marker
      if (this.AF.Looped) {
      BinaryWriter BW = new BinaryWriter(this.FS, Encoding.ASCII);
	BW.Write("cue ".ToCharArray());
	BW.Write((int) 0x1c);
	BW.Write((int) 1);
	BW.Write((int) 1);
	BW.Write((int) (this.AF.LoopStart * this.AF.SampleRate));
	BW.Write("data".ToCharArray());
	BW.Write((int) 0);
	BW.Write((int) 0);
	BW.Write((int) (this.AF.LoopStart * this.AF.SampleRate));
	BW.Write("LIST".ToCharArray());
	BW.Write((int) 0x10 + 12);
	BW.Write("adtl".ToCharArray());
	BW.Write("labl".ToCharArray());
	BW.Write((int) 0x04 + 12);
	BW.Write((int) 1);
	BW.Write("Loop Start\0\0".ToCharArray());
	this.prbBytesWritten.Value += 0x48;
      }
#endif
      FS.Close();
    }

    protected override void Dispose(bool disposing) {
      if (disposing) {
	if (components != null)
	  components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent() {
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(WaveWriter));
      this.prbBytesWritten = new System.Windows.Forms.ProgressBar();
      this.lblSource = new System.Windows.Forms.Label();
      this.lblTarget = new System.Windows.Forms.Label();
      this.txtSource = new System.Windows.Forms.TextBox();
      this.txtTarget = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // prbBytesWritten
      // 
      this.prbBytesWritten.AccessibleDescription = resources.GetString("prbBytesWritten.AccessibleDescription");
      this.prbBytesWritten.AccessibleName = resources.GetString("prbBytesWritten.AccessibleName");
      this.prbBytesWritten.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("prbBytesWritten.Anchor")));
      this.prbBytesWritten.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("prbBytesWritten.BackgroundImage")));
      this.prbBytesWritten.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("prbBytesWritten.Dock")));
      this.prbBytesWritten.Enabled = ((bool)(resources.GetObject("prbBytesWritten.Enabled")));
      this.prbBytesWritten.Font = ((System.Drawing.Font)(resources.GetObject("prbBytesWritten.Font")));
      this.prbBytesWritten.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("prbBytesWritten.ImeMode")));
      this.prbBytesWritten.Location = ((System.Drawing.Point)(resources.GetObject("prbBytesWritten.Location")));
      this.prbBytesWritten.Name = "prbBytesWritten";
      this.prbBytesWritten.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("prbBytesWritten.RightToLeft")));
      this.prbBytesWritten.Size = ((System.Drawing.Size)(resources.GetObject("prbBytesWritten.Size")));
      this.prbBytesWritten.TabIndex = ((int)(resources.GetObject("prbBytesWritten.TabIndex")));
      this.prbBytesWritten.Text = resources.GetString("prbBytesWritten.Text");
      this.prbBytesWritten.Visible = ((bool)(resources.GetObject("prbBytesWritten.Visible")));
      // 
      // lblSource
      // 
      this.lblSource.AccessibleDescription = resources.GetString("lblSource.AccessibleDescription");
      this.lblSource.AccessibleName = resources.GetString("lblSource.AccessibleName");
      this.lblSource.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblSource.Anchor")));
      this.lblSource.AutoSize = ((bool)(resources.GetObject("lblSource.AutoSize")));
      this.lblSource.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblSource.Dock")));
      this.lblSource.Enabled = ((bool)(resources.GetObject("lblSource.Enabled")));
      this.lblSource.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblSource.Font = ((System.Drawing.Font)(resources.GetObject("lblSource.Font")));
      this.lblSource.Image = ((System.Drawing.Image)(resources.GetObject("lblSource.Image")));
      this.lblSource.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblSource.ImageAlign")));
      this.lblSource.ImageIndex = ((int)(resources.GetObject("lblSource.ImageIndex")));
      this.lblSource.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblSource.ImeMode")));
      this.lblSource.Location = ((System.Drawing.Point)(resources.GetObject("lblSource.Location")));
      this.lblSource.Name = "lblSource";
      this.lblSource.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblSource.RightToLeft")));
      this.lblSource.Size = ((System.Drawing.Size)(resources.GetObject("lblSource.Size")));
      this.lblSource.TabIndex = ((int)(resources.GetObject("lblSource.TabIndex")));
      this.lblSource.Text = resources.GetString("lblSource.Text");
      this.lblSource.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblSource.TextAlign")));
      this.lblSource.Visible = ((bool)(resources.GetObject("lblSource.Visible")));
      // 
      // lblTarget
      // 
      this.lblTarget.AccessibleDescription = resources.GetString("lblTarget.AccessibleDescription");
      this.lblTarget.AccessibleName = resources.GetString("lblTarget.AccessibleName");
      this.lblTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblTarget.Anchor")));
      this.lblTarget.AutoSize = ((bool)(resources.GetObject("lblTarget.AutoSize")));
      this.lblTarget.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblTarget.Dock")));
      this.lblTarget.Enabled = ((bool)(resources.GetObject("lblTarget.Enabled")));
      this.lblTarget.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblTarget.Font = ((System.Drawing.Font)(resources.GetObject("lblTarget.Font")));
      this.lblTarget.Image = ((System.Drawing.Image)(resources.GetObject("lblTarget.Image")));
      this.lblTarget.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblTarget.ImageAlign")));
      this.lblTarget.ImageIndex = ((int)(resources.GetObject("lblTarget.ImageIndex")));
      this.lblTarget.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblTarget.ImeMode")));
      this.lblTarget.Location = ((System.Drawing.Point)(resources.GetObject("lblTarget.Location")));
      this.lblTarget.Name = "lblTarget";
      this.lblTarget.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblTarget.RightToLeft")));
      this.lblTarget.Size = ((System.Drawing.Size)(resources.GetObject("lblTarget.Size")));
      this.lblTarget.TabIndex = ((int)(resources.GetObject("lblTarget.TabIndex")));
      this.lblTarget.Text = resources.GetString("lblTarget.Text");
      this.lblTarget.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblTarget.TextAlign")));
      this.lblTarget.Visible = ((bool)(resources.GetObject("lblTarget.Visible")));
      // 
      // txtSource
      // 
      this.txtSource.AccessibleDescription = resources.GetString("txtSource.AccessibleDescription");
      this.txtSource.AccessibleName = resources.GetString("txtSource.AccessibleName");
      this.txtSource.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtSource.Anchor")));
      this.txtSource.AutoSize = ((bool)(resources.GetObject("txtSource.AutoSize")));
      this.txtSource.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtSource.BackgroundImage")));
      this.txtSource.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtSource.Dock")));
      this.txtSource.Enabled = ((bool)(resources.GetObject("txtSource.Enabled")));
      this.txtSource.Font = ((System.Drawing.Font)(resources.GetObject("txtSource.Font")));
      this.txtSource.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtSource.ImeMode")));
      this.txtSource.Location = ((System.Drawing.Point)(resources.GetObject("txtSource.Location")));
      this.txtSource.MaxLength = ((int)(resources.GetObject("txtSource.MaxLength")));
      this.txtSource.Multiline = ((bool)(resources.GetObject("txtSource.Multiline")));
      this.txtSource.Name = "txtSource";
      this.txtSource.PasswordChar = ((char)(resources.GetObject("txtSource.PasswordChar")));
      this.txtSource.ReadOnly = true;
      this.txtSource.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtSource.RightToLeft")));
      this.txtSource.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtSource.ScrollBars")));
      this.txtSource.Size = ((System.Drawing.Size)(resources.GetObject("txtSource.Size")));
      this.txtSource.TabIndex = ((int)(resources.GetObject("txtSource.TabIndex")));
      this.txtSource.Text = resources.GetString("txtSource.Text");
      this.txtSource.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtSource.TextAlign")));
      this.txtSource.Visible = ((bool)(resources.GetObject("txtSource.Visible")));
      this.txtSource.WordWrap = ((bool)(resources.GetObject("txtSource.WordWrap")));
      // 
      // txtTarget
      // 
      this.txtTarget.AccessibleDescription = resources.GetString("txtTarget.AccessibleDescription");
      this.txtTarget.AccessibleName = resources.GetString("txtTarget.AccessibleName");
      this.txtTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtTarget.Anchor")));
      this.txtTarget.AutoSize = ((bool)(resources.GetObject("txtTarget.AutoSize")));
      this.txtTarget.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtTarget.BackgroundImage")));
      this.txtTarget.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtTarget.Dock")));
      this.txtTarget.Enabled = ((bool)(resources.GetObject("txtTarget.Enabled")));
      this.txtTarget.Font = ((System.Drawing.Font)(resources.GetObject("txtTarget.Font")));
      this.txtTarget.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtTarget.ImeMode")));
      this.txtTarget.Location = ((System.Drawing.Point)(resources.GetObject("txtTarget.Location")));
      this.txtTarget.MaxLength = ((int)(resources.GetObject("txtTarget.MaxLength")));
      this.txtTarget.Multiline = ((bool)(resources.GetObject("txtTarget.Multiline")));
      this.txtTarget.Name = "txtTarget";
      this.txtTarget.PasswordChar = ((char)(resources.GetObject("txtTarget.PasswordChar")));
      this.txtTarget.ReadOnly = true;
      this.txtTarget.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtTarget.RightToLeft")));
      this.txtTarget.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtTarget.ScrollBars")));
      this.txtTarget.Size = ((System.Drawing.Size)(resources.GetObject("txtTarget.Size")));
      this.txtTarget.TabIndex = ((int)(resources.GetObject("txtTarget.TabIndex")));
      this.txtTarget.Text = resources.GetString("txtTarget.Text");
      this.txtTarget.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtTarget.TextAlign")));
      this.txtTarget.Visible = ((bool)(resources.GetObject("txtTarget.Visible")));
      this.txtTarget.WordWrap = ((bool)(resources.GetObject("txtTarget.WordWrap")));
      // 
      // WaveWriter
      // 
      this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
      this.AccessibleName = resources.GetString("$this.AccessibleName");
      this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
      this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
      this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
      this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
      this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
      this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
      this.ControlBox = false;
      this.Controls.Add(this.txtTarget);
      this.Controls.Add(this.txtSource);
      this.Controls.Add(this.lblTarget);
      this.Controls.Add(this.lblSource);
      this.Controls.Add(this.prbBytesWritten);
      this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
      this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
      this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
      this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
      this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
      this.Name = "WaveWriter";
      this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
      this.ShowInTaskbar = false;
      this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
      this.Text = resources.GetString("$this.Text");
      this.Activated += new System.EventHandler(this.WaveWriter_Activated);
      this.ResumeLayout(false);

    }

    #endregion

    private void WaveWriter_Activated(object sender, System.EventArgs e) {
      this.Refresh();
      Application.DoEvents();
      if (this.AFS == null)
	MessageBox.Show(this, "Could not create input audio stream", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      else if (this.FS == null)
	MessageBox.Show(this, "Could not create output file stream", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      else
	this.WriteWave();
      this.Close();
    }

  }

}
