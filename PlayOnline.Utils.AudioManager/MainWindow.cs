//#define UseDirectX

#if UseDirectX // We only support this mess for "real" DirectX
//#define BufferSound
#endif

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

#if UseDirectX
using Microsoft.DirectX.DirectSound;
#else
using ManagedDirectX;
using ManagedDirectX.DirectSound;
#endif

using PlayOnline.Core;
using PlayOnline.Core.Audio;

namespace PlayOnline.Utils.AudioManager {

  public class MainWindow : System.Windows.Forms.Form {

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

    private System.ComponentModel.IContainer components;

    #endregion

    public MainWindow() {
      this.InitializeComponent();
#if !UseDirectX
      if (!ManagedDirectSound.Available) {
	MessageBox.Show(this, "Managed DirectX is not available; sound playback will be disabled.", "DirectSound Initialization Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
	this.AudioEnabled = false;
      }
#endif
      this.Icon = Icons.AudioStuff;
      try {
	this.ilMusicBrowserIcons.Images.Add(Icons.AudioFolder);
	this.ilSoundBrowserIcons.Images.Add(Icons.AudioFolder);
	this.ilMusicBrowserIcons.Images.Add(Icons.FolderClosed);
	this.ilSoundBrowserIcons.Images.Add(Icons.FolderClosed);
	this.ilMusicBrowserIcons.Images.Add(Icons.FolderOpen);
	this.ilSoundBrowserIcons.Images.Add(Icons.FolderOpen);
	this.ilMusicBrowserIcons.Images.Add(Icons.AudioFile);
	this.ilSoundBrowserIcons.Images.Add(Icons.AudioFile);
      }
      catch (Exception E) {
	Console.WriteLine(E.ToString());
	this.tvMusicBrowser.ImageList = null;
	this.tvSoundBrowser.ImageList = null;
      }
    }

    protected override void Dispose(bool disposing) {
      if (disposing && components != null)
	components.Dispose();
      base.Dispose(disposing);
    }

    private void RefreshBrowser() {
      this.ClearFileInfo();
      using (InfoBuilder IB = new InfoBuilder()) {
	Application.DoEvents();
	// Clear treeview, create main root and fill them
	switch (this.tabBrowsers.SelectedIndex) {
	  case 0: // Music
	    this.tvMusicBrowser.Nodes.Clear();
	  TreeNode MusicRoot = new TreeNode("Music");
	    this.tvMusicBrowser.Nodes.Add(MusicRoot);
	    IB.TargetNode   = MusicRoot;
	    IB.FileTypeName = MusicRoot.Text;
	    IB.FilePattern  = "*.bgw";
	    IB.ResourceName = "MusicInfo.xml";
	    IB.ShowDialog(this);
	    MusicRoot.Expand();
	    this.tvMusicBrowser.SelectedNode = MusicRoot;
	    break;
	  case 1: // Sound Effects
	    this.tvSoundBrowser.Nodes.Clear();
	  TreeNode SoundRoot = new TreeNode("Sound Effects");
	    this.tvSoundBrowser.Nodes.Add(SoundRoot);
	    IB.TargetNode   = SoundRoot;
	    IB.FileTypeName = SoundRoot.Text;
	    IB.FilePattern  = "*.spw";
	    IB.ResourceName = "SFXInfo.xml";
	    IB.ShowDialog(this);
	    SoundRoot.Expand();
	    this.tvSoundBrowser.SelectedNode = SoundRoot;
	    break;
	}
      }
    }

    private void MaybeRefreshBrowser() {
      // If the treeview is empty, populate it
      switch (this.tabBrowsers.SelectedIndex) {
	case 0: // Music
	  if (this.tvMusicBrowser.Nodes.Count == 0)
	    this.RefreshBrowser();
	  break;
	case 1: // Sound Effects
	  if (this.tvSoundBrowser.Nodes.Count == 0)
	    this.RefreshBrowser();
	  break;
      }
    }

    private void ClearFileInfo() {
      foreach (Control C in this.grpFileInfo.Controls) {
	if (C is TextBox)
	  C.Text = null;
      }
      this.btnPlay.Enabled = false;
      this.btnDecode.Enabled = false;
    }

    private string LengthText(double seconds) {
    TimeSpan FileLength = new TimeSpan((long) (seconds * 10000000));
    string Result = String.Format("{0}s", FileLength.Seconds);
      if (FileLength.Minutes > 0)
	Result = String.Format("{0}m ", FileLength.Minutes) + Result;
      if (FileLength.TotalHours >= 1)
	Result = String.Format("{0}h ", (long) Math.Floor(FileLength.TotalHours)) + Result;
      return Result;
    }

    private bool            AudioEnabled       = true;
    private readonly int    AudioBufferSize    = 256 * 1024;
    private bool            AudioIsLooping     = false;
    private AudioFileStream CurrentStream      = null;

    private Device          AudioDevice        = null;
    private SecondaryBuffer CurrentBuffer      = null;

#if BufferSound

    private readonly int    AudioBufferMarkers = 2;
    private AutoResetEvent  AudioUpdateTrigger = null;
    private Thread          AudioUpdateThread  = null;

    private void AudioUpdate() {
      while (this.CurrentBuffer != null && this.CurrentStream != null && this.AudioUpdateThread == Thread.CurrentThread) {
	if (this.AudioUpdateTrigger.WaitOne(100, true))
	  this.UpdateBufferContents();
      }
    }

    private void UpdateBufferContents() {
      lock (this) {
	if (this.CurrentBuffer == null)
	  return;
	// Determine the proper update location
      int ChunkSize = this.AudioBufferSize / this.AudioBufferMarkers;
      int StartPos  = (int) this.CurrentStream.Position;
      int EndPos    = StartPos + ChunkSize;
	// Normalize it vs the buffer size
	StartPos %= this.AudioBufferSize;
	EndPos   %= this.AudioBufferSize;
	// Ensure the region we want to write isn't currently being played
	if (this.CurrentBuffer.PlayPosition >= StartPos && this.CurrentBuffer.PlayPosition < EndPos) {
	  // If we're at the end of the file, stop playback
	  if (this.CurrentStream.Position == this.CurrentStream.Length)
	    this.StopPlayback();
	  return;
	}
	// Write the data
      	this.CurrentBuffer.Write(StartPos, this.CurrentStream, ChunkSize, LockFlag.None);
      }
    }

#endif

    private void PausePlayback() {
      if (this.CurrentBuffer.Status.Playing) {
	this.CurrentBuffer.Stop();
	this.btnPause.Text = "&Resume";
      }
      else {
	this.CurrentBuffer.Play(0, (this.AudioIsLooping ? BufferPlayFlags.Looping : BufferPlayFlags.Default));
	this.btnPause.Text = "Pa&use";
      }
    }

    private void StopPlayback() {
      lock (this) {
	if (this.CurrentBuffer != null) {
	  this.CurrentBuffer.Stop();
	  this.CurrentBuffer.Dispose();
	  this.CurrentBuffer = null;
	}
	if (this.CurrentStream != null) {
	  this.CurrentStream.Close();
	  this.CurrentStream = null;
	}
#if BufferSound
	if (this.AudioUpdateThread != null)
	  this.AudioUpdateThread = null;
#endif
      }
      this.btnPause.Enabled = false;
      this.btnPause.Text = "Pa&use";
      this.btnStop.Enabled = false;
      this.AudioIsLooping = false;
    }
    
    private void PlayFile(FileInfo FI) {
      lock (this) {
	if (this.AudioDevice == null) {
	  this.AudioDevice = new Device();
	  AudioDevice.SetCooperativeLevel(this, CooperativeLevel.Normal);
	}
	this.StopPlayback();
      WaveFormat fmt = new WaveFormat();
	fmt.FormatTag = WaveFormatTag.Pcm;
	fmt.Channels = FI.AudioFile.Channels;
	fmt.SamplesPerSecond = FI.AudioFile.SampleRate;
	fmt.BitsPerSample = 16;
	fmt.BlockAlign = (short) (FI.AudioFile.Channels * (fmt.BitsPerSample / 8));
	fmt.AverageBytesPerSecond = fmt.SamplesPerSecond * fmt.BlockAlign;
      BufferDescription BD = new BufferDescription(fmt);
	BD.BufferBytes = this.AudioBufferSize;
	BD.GlobalFocus = true;
	BD.StickyFocus = true;
#if BufferSound
	BD.ControlPositionNotify = true;
	this.CurrentBuffer = new SecondaryBuffer(BD, this.AudioDevice);
	if (this.AudioUpdateTrigger == null)
	  this.AudioUpdateTrigger = new AutoResetEvent(false);
      int ChunkSize = this.AudioBufferSize / this.AudioBufferMarkers;
      BufferPositionNotify[] UpdatePositions = new BufferPositionNotify[this.AudioBufferMarkers];
	for (int i = 0; i < this.AudioBufferMarkers; ++i) {
	  UpdatePositions[i] = new BufferPositionNotify();
	  UpdatePositions[i].EventNotifyHandle = this.AudioUpdateTrigger.Handle;
	  UpdatePositions[i].Offset = ChunkSize * i;
	}
      Notify N = new Notify(this.CurrentBuffer);
	N.SetNotificationPositions(UpdatePositions);
	this.CurrentStream = FI.AudioFile.OpenStream();
	this.CurrentBuffer.Write(0, this.CurrentStream, this.CurrentBuffer.Caps.BufferBytes, LockFlag.EntireBuffer);
	if (this.CurrentStream.Position < this.CurrentStream.Length) {
	  this.AudioUpdateTrigger.Reset();
	  this.AudioUpdateThread = new Thread(new ThreadStart(this.AudioUpdate));
	  this.AudioUpdateThread.Start();
	  this.btnPause.Enabled = true;
	  this.btnStop.Enabled = true;
	  this.AudioIsLooping = true;
	}
	else {
	  this.CurrentStream.Close();
	  this.CurrentStream = null;
	  this.AudioIsLooping = false;
	}
#else
	this.CurrentStream = FI.AudioFile.OpenStream(true);
	this.CurrentBuffer = new SecondaryBuffer(this.CurrentStream, BD, this.AudioDevice);
	this.btnPause.Enabled = true;
	this.btnStop.Enabled = true;
#endif
	this.CurrentBuffer.Play(0, (this.AudioIsLooping ? BufferPlayFlags.Looping : BufferPlayFlags.Default));
      }
    }

    #region Windows Form Designer generated code

    private void InitializeComponent() {
      this.components = new System.ComponentModel.Container();
      System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainWindow));
      this.ilMusicBrowserIcons = new System.Windows.Forms.ImageList(this.components);
      this.pnlInfoArea = new System.Windows.Forms.Panel();
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
      this.ilMusicBrowserIcons.ImageSize = ((System.Drawing.Size)(resources.GetObject("ilMusicBrowserIcons.ImageSize")));
      this.ilMusicBrowserIcons.TransparentColor = System.Drawing.Color.Transparent;
      // 
      // pnlInfoArea
      // 
      this.pnlInfoArea.AccessibleDescription = resources.GetString("pnlInfoArea.AccessibleDescription");
      this.pnlInfoArea.AccessibleName = resources.GetString("pnlInfoArea.AccessibleName");
      this.pnlInfoArea.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("pnlInfoArea.Anchor")));
      this.pnlInfoArea.AutoScroll = ((bool)(resources.GetObject("pnlInfoArea.AutoScroll")));
      this.pnlInfoArea.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("pnlInfoArea.AutoScrollMargin")));
      this.pnlInfoArea.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("pnlInfoArea.AutoScrollMinSize")));
      this.pnlInfoArea.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlInfoArea.BackgroundImage")));
      this.pnlInfoArea.Controls.Add(this.btnStop);
      this.pnlInfoArea.Controls.Add(this.btnPause);
      this.pnlInfoArea.Controls.Add(this.btnPlay);
      this.pnlInfoArea.Controls.Add(this.btnDecode);
      this.pnlInfoArea.Controls.Add(this.grpFileInfo);
      this.pnlInfoArea.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("pnlInfoArea.Dock")));
      this.pnlInfoArea.Enabled = ((bool)(resources.GetObject("pnlInfoArea.Enabled")));
      this.pnlInfoArea.Font = ((System.Drawing.Font)(resources.GetObject("pnlInfoArea.Font")));
      this.pnlInfoArea.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("pnlInfoArea.ImeMode")));
      this.pnlInfoArea.Location = ((System.Drawing.Point)(resources.GetObject("pnlInfoArea.Location")));
      this.pnlInfoArea.Name = "pnlInfoArea";
      this.pnlInfoArea.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("pnlInfoArea.RightToLeft")));
      this.pnlInfoArea.Size = ((System.Drawing.Size)(resources.GetObject("pnlInfoArea.Size")));
      this.pnlInfoArea.TabIndex = ((int)(resources.GetObject("pnlInfoArea.TabIndex")));
      this.pnlInfoArea.Text = resources.GetString("pnlInfoArea.Text");
      this.pnlInfoArea.Visible = ((bool)(resources.GetObject("pnlInfoArea.Visible")));
      // 
      // btnStop
      // 
      this.btnStop.AccessibleDescription = resources.GetString("btnStop.AccessibleDescription");
      this.btnStop.AccessibleName = resources.GetString("btnStop.AccessibleName");
      this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnStop.Anchor")));
      this.btnStop.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnStop.BackgroundImage")));
      this.btnStop.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnStop.Dock")));
      this.btnStop.Enabled = ((bool)(resources.GetObject("btnStop.Enabled")));
      this.btnStop.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnStop.FlatStyle")));
      this.btnStop.Font = ((System.Drawing.Font)(resources.GetObject("btnStop.Font")));
      this.btnStop.Image = ((System.Drawing.Image)(resources.GetObject("btnStop.Image")));
      this.btnStop.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnStop.ImageAlign")));
      this.btnStop.ImageIndex = ((int)(resources.GetObject("btnStop.ImageIndex")));
      this.btnStop.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnStop.ImeMode")));
      this.btnStop.Location = ((System.Drawing.Point)(resources.GetObject("btnStop.Location")));
      this.btnStop.Name = "btnStop";
      this.btnStop.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnStop.RightToLeft")));
      this.btnStop.Size = ((System.Drawing.Size)(resources.GetObject("btnStop.Size")));
      this.btnStop.TabIndex = ((int)(resources.GetObject("btnStop.TabIndex")));
      this.btnStop.Text = resources.GetString("btnStop.Text");
      this.btnStop.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnStop.TextAlign")));
      this.btnStop.Visible = ((bool)(resources.GetObject("btnStop.Visible")));
      this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
      // 
      // btnPause
      // 
      this.btnPause.AccessibleDescription = resources.GetString("btnPause.AccessibleDescription");
      this.btnPause.AccessibleName = resources.GetString("btnPause.AccessibleName");
      this.btnPause.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnPause.Anchor")));
      this.btnPause.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPause.BackgroundImage")));
      this.btnPause.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnPause.Dock")));
      this.btnPause.Enabled = ((bool)(resources.GetObject("btnPause.Enabled")));
      this.btnPause.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnPause.FlatStyle")));
      this.btnPause.Font = ((System.Drawing.Font)(resources.GetObject("btnPause.Font")));
      this.btnPause.Image = ((System.Drawing.Image)(resources.GetObject("btnPause.Image")));
      this.btnPause.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnPause.ImageAlign")));
      this.btnPause.ImageIndex = ((int)(resources.GetObject("btnPause.ImageIndex")));
      this.btnPause.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnPause.ImeMode")));
      this.btnPause.Location = ((System.Drawing.Point)(resources.GetObject("btnPause.Location")));
      this.btnPause.Name = "btnPause";
      this.btnPause.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnPause.RightToLeft")));
      this.btnPause.Size = ((System.Drawing.Size)(resources.GetObject("btnPause.Size")));
      this.btnPause.TabIndex = ((int)(resources.GetObject("btnPause.TabIndex")));
      this.btnPause.Text = resources.GetString("btnPause.Text");
      this.btnPause.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnPause.TextAlign")));
      this.btnPause.Visible = ((bool)(resources.GetObject("btnPause.Visible")));
      this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
      // 
      // btnPlay
      // 
      this.btnPlay.AccessibleDescription = resources.GetString("btnPlay.AccessibleDescription");
      this.btnPlay.AccessibleName = resources.GetString("btnPlay.AccessibleName");
      this.btnPlay.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnPlay.Anchor")));
      this.btnPlay.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPlay.BackgroundImage")));
      this.btnPlay.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnPlay.Dock")));
      this.btnPlay.Enabled = ((bool)(resources.GetObject("btnPlay.Enabled")));
      this.btnPlay.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnPlay.FlatStyle")));
      this.btnPlay.Font = ((System.Drawing.Font)(resources.GetObject("btnPlay.Font")));
      this.btnPlay.Image = ((System.Drawing.Image)(resources.GetObject("btnPlay.Image")));
      this.btnPlay.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnPlay.ImageAlign")));
      this.btnPlay.ImageIndex = ((int)(resources.GetObject("btnPlay.ImageIndex")));
      this.btnPlay.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnPlay.ImeMode")));
      this.btnPlay.Location = ((System.Drawing.Point)(resources.GetObject("btnPlay.Location")));
      this.btnPlay.Name = "btnPlay";
      this.btnPlay.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnPlay.RightToLeft")));
      this.btnPlay.Size = ((System.Drawing.Size)(resources.GetObject("btnPlay.Size")));
      this.btnPlay.TabIndex = ((int)(resources.GetObject("btnPlay.TabIndex")));
      this.btnPlay.Text = resources.GetString("btnPlay.Text");
      this.btnPlay.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnPlay.TextAlign")));
      this.btnPlay.Visible = ((bool)(resources.GetObject("btnPlay.Visible")));
      this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
      // 
      // btnDecode
      // 
      this.btnDecode.AccessibleDescription = resources.GetString("btnDecode.AccessibleDescription");
      this.btnDecode.AccessibleName = resources.GetString("btnDecode.AccessibleName");
      this.btnDecode.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("btnDecode.Anchor")));
      this.btnDecode.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnDecode.BackgroundImage")));
      this.btnDecode.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("btnDecode.Dock")));
      this.btnDecode.Enabled = ((bool)(resources.GetObject("btnDecode.Enabled")));
      this.btnDecode.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("btnDecode.FlatStyle")));
      this.btnDecode.Font = ((System.Drawing.Font)(resources.GetObject("btnDecode.Font")));
      this.btnDecode.Image = ((System.Drawing.Image)(resources.GetObject("btnDecode.Image")));
      this.btnDecode.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnDecode.ImageAlign")));
      this.btnDecode.ImageIndex = ((int)(resources.GetObject("btnDecode.ImageIndex")));
      this.btnDecode.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("btnDecode.ImeMode")));
      this.btnDecode.Location = ((System.Drawing.Point)(resources.GetObject("btnDecode.Location")));
      this.btnDecode.Name = "btnDecode";
      this.btnDecode.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("btnDecode.RightToLeft")));
      this.btnDecode.Size = ((System.Drawing.Size)(resources.GetObject("btnDecode.Size")));
      this.btnDecode.TabIndex = ((int)(resources.GetObject("btnDecode.TabIndex")));
      this.btnDecode.Text = resources.GetString("btnDecode.Text");
      this.btnDecode.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("btnDecode.TextAlign")));
      this.btnDecode.Visible = ((bool)(resources.GetObject("btnDecode.Visible")));
      this.btnDecode.Click += new System.EventHandler(this.btnDecode_Click);
      // 
      // grpFileInfo
      // 
      this.grpFileInfo.AccessibleDescription = resources.GetString("grpFileInfo.AccessibleDescription");
      this.grpFileInfo.AccessibleName = resources.GetString("grpFileInfo.AccessibleName");
      this.grpFileInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("grpFileInfo.Anchor")));
      this.grpFileInfo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("grpFileInfo.BackgroundImage")));
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
      this.grpFileInfo.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("grpFileInfo.Dock")));
      this.grpFileInfo.Enabled = ((bool)(resources.GetObject("grpFileInfo.Enabled")));
      this.grpFileInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.grpFileInfo.Font = ((System.Drawing.Font)(resources.GetObject("grpFileInfo.Font")));
      this.grpFileInfo.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("grpFileInfo.ImeMode")));
      this.grpFileInfo.Location = ((System.Drawing.Point)(resources.GetObject("grpFileInfo.Location")));
      this.grpFileInfo.Name = "grpFileInfo";
      this.grpFileInfo.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("grpFileInfo.RightToLeft")));
      this.grpFileInfo.Size = ((System.Drawing.Size)(resources.GetObject("grpFileInfo.Size")));
      this.grpFileInfo.TabIndex = ((int)(resources.GetObject("grpFileInfo.TabIndex")));
      this.grpFileInfo.TabStop = false;
      this.grpFileInfo.Text = resources.GetString("grpFileInfo.Text");
      this.grpFileInfo.Visible = ((bool)(resources.GetObject("grpFileInfo.Visible")));
      // 
      // txtComposer
      // 
      this.txtComposer.AccessibleDescription = resources.GetString("txtComposer.AccessibleDescription");
      this.txtComposer.AccessibleName = resources.GetString("txtComposer.AccessibleName");
      this.txtComposer.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtComposer.Anchor")));
      this.txtComposer.AutoSize = ((bool)(resources.GetObject("txtComposer.AutoSize")));
      this.txtComposer.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtComposer.BackgroundImage")));
      this.txtComposer.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtComposer.Dock")));
      this.txtComposer.Enabled = ((bool)(resources.GetObject("txtComposer.Enabled")));
      this.txtComposer.Font = ((System.Drawing.Font)(resources.GetObject("txtComposer.Font")));
      this.txtComposer.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtComposer.ImeMode")));
      this.txtComposer.Location = ((System.Drawing.Point)(resources.GetObject("txtComposer.Location")));
      this.txtComposer.MaxLength = ((int)(resources.GetObject("txtComposer.MaxLength")));
      this.txtComposer.Multiline = ((bool)(resources.GetObject("txtComposer.Multiline")));
      this.txtComposer.Name = "txtComposer";
      this.txtComposer.PasswordChar = ((char)(resources.GetObject("txtComposer.PasswordChar")));
      this.txtComposer.ReadOnly = true;
      this.txtComposer.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtComposer.RightToLeft")));
      this.txtComposer.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtComposer.ScrollBars")));
      this.txtComposer.Size = ((System.Drawing.Size)(resources.GetObject("txtComposer.Size")));
      this.txtComposer.TabIndex = ((int)(resources.GetObject("txtComposer.TabIndex")));
      this.txtComposer.Text = resources.GetString("txtComposer.Text");
      this.txtComposer.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtComposer.TextAlign")));
      this.txtComposer.Visible = ((bool)(resources.GetObject("txtComposer.Visible")));
      this.txtComposer.WordWrap = ((bool)(resources.GetObject("txtComposer.WordWrap")));
      // 
      // lblComposer
      // 
      this.lblComposer.AccessibleDescription = resources.GetString("lblComposer.AccessibleDescription");
      this.lblComposer.AccessibleName = resources.GetString("lblComposer.AccessibleName");
      this.lblComposer.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblComposer.Anchor")));
      this.lblComposer.AutoSize = ((bool)(resources.GetObject("lblComposer.AutoSize")));
      this.lblComposer.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblComposer.Dock")));
      this.lblComposer.Enabled = ((bool)(resources.GetObject("lblComposer.Enabled")));
      this.lblComposer.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblComposer.Font = ((System.Drawing.Font)(resources.GetObject("lblComposer.Font")));
      this.lblComposer.Image = ((System.Drawing.Image)(resources.GetObject("lblComposer.Image")));
      this.lblComposer.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblComposer.ImageAlign")));
      this.lblComposer.ImageIndex = ((int)(resources.GetObject("lblComposer.ImageIndex")));
      this.lblComposer.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblComposer.ImeMode")));
      this.lblComposer.Location = ((System.Drawing.Point)(resources.GetObject("lblComposer.Location")));
      this.lblComposer.Name = "lblComposer";
      this.lblComposer.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblComposer.RightToLeft")));
      this.lblComposer.Size = ((System.Drawing.Size)(resources.GetObject("lblComposer.Size")));
      this.lblComposer.TabIndex = ((int)(resources.GetObject("lblComposer.TabIndex")));
      this.lblComposer.Text = resources.GetString("lblComposer.Text");
      this.lblComposer.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblComposer.TextAlign")));
      this.lblComposer.Visible = ((bool)(resources.GetObject("lblComposer.Visible")));
      // 
      // txtTitle
      // 
      this.txtTitle.AccessibleDescription = resources.GetString("txtTitle.AccessibleDescription");
      this.txtTitle.AccessibleName = resources.GetString("txtTitle.AccessibleName");
      this.txtTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtTitle.Anchor")));
      this.txtTitle.AutoSize = ((bool)(resources.GetObject("txtTitle.AutoSize")));
      this.txtTitle.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtTitle.BackgroundImage")));
      this.txtTitle.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtTitle.Dock")));
      this.txtTitle.Enabled = ((bool)(resources.GetObject("txtTitle.Enabled")));
      this.txtTitle.Font = ((System.Drawing.Font)(resources.GetObject("txtTitle.Font")));
      this.txtTitle.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtTitle.ImeMode")));
      this.txtTitle.Location = ((System.Drawing.Point)(resources.GetObject("txtTitle.Location")));
      this.txtTitle.MaxLength = ((int)(resources.GetObject("txtTitle.MaxLength")));
      this.txtTitle.Multiline = ((bool)(resources.GetObject("txtTitle.Multiline")));
      this.txtTitle.Name = "txtTitle";
      this.txtTitle.PasswordChar = ((char)(resources.GetObject("txtTitle.PasswordChar")));
      this.txtTitle.ReadOnly = true;
      this.txtTitle.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtTitle.RightToLeft")));
      this.txtTitle.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtTitle.ScrollBars")));
      this.txtTitle.Size = ((System.Drawing.Size)(resources.GetObject("txtTitle.Size")));
      this.txtTitle.TabIndex = ((int)(resources.GetObject("txtTitle.TabIndex")));
      this.txtTitle.Text = resources.GetString("txtTitle.Text");
      this.txtTitle.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtTitle.TextAlign")));
      this.txtTitle.Visible = ((bool)(resources.GetObject("txtTitle.Visible")));
      this.txtTitle.WordWrap = ((bool)(resources.GetObject("txtTitle.WordWrap")));
      // 
      // lblTitle
      // 
      this.lblTitle.AccessibleDescription = resources.GetString("lblTitle.AccessibleDescription");
      this.lblTitle.AccessibleName = resources.GetString("lblTitle.AccessibleName");
      this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblTitle.Anchor")));
      this.lblTitle.AutoSize = ((bool)(resources.GetObject("lblTitle.AutoSize")));
      this.lblTitle.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblTitle.Dock")));
      this.lblTitle.Enabled = ((bool)(resources.GetObject("lblTitle.Enabled")));
      this.lblTitle.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblTitle.Font = ((System.Drawing.Font)(resources.GetObject("lblTitle.Font")));
      this.lblTitle.Image = ((System.Drawing.Image)(resources.GetObject("lblTitle.Image")));
      this.lblTitle.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblTitle.ImageAlign")));
      this.lblTitle.ImageIndex = ((int)(resources.GetObject("lblTitle.ImageIndex")));
      this.lblTitle.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblTitle.ImeMode")));
      this.lblTitle.Location = ((System.Drawing.Point)(resources.GetObject("lblTitle.Location")));
      this.lblTitle.Name = "lblTitle";
      this.lblTitle.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblTitle.RightToLeft")));
      this.lblTitle.Size = ((System.Drawing.Size)(resources.GetObject("lblTitle.Size")));
      this.lblTitle.TabIndex = ((int)(resources.GetObject("lblTitle.TabIndex")));
      this.lblTitle.Text = resources.GetString("lblTitle.Text");
      this.lblTitle.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblTitle.TextAlign")));
      this.lblTitle.Visible = ((bool)(resources.GetObject("lblTitle.Visible")));
      // 
      // txtFileLength
      // 
      this.txtFileLength.AccessibleDescription = resources.GetString("txtFileLength.AccessibleDescription");
      this.txtFileLength.AccessibleName = resources.GetString("txtFileLength.AccessibleName");
      this.txtFileLength.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtFileLength.Anchor")));
      this.txtFileLength.AutoSize = ((bool)(resources.GetObject("txtFileLength.AutoSize")));
      this.txtFileLength.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtFileLength.BackgroundImage")));
      this.txtFileLength.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtFileLength.Dock")));
      this.txtFileLength.Enabled = ((bool)(resources.GetObject("txtFileLength.Enabled")));
      this.txtFileLength.Font = ((System.Drawing.Font)(resources.GetObject("txtFileLength.Font")));
      this.txtFileLength.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtFileLength.ImeMode")));
      this.txtFileLength.Location = ((System.Drawing.Point)(resources.GetObject("txtFileLength.Location")));
      this.txtFileLength.MaxLength = ((int)(resources.GetObject("txtFileLength.MaxLength")));
      this.txtFileLength.Multiline = ((bool)(resources.GetObject("txtFileLength.Multiline")));
      this.txtFileLength.Name = "txtFileLength";
      this.txtFileLength.PasswordChar = ((char)(resources.GetObject("txtFileLength.PasswordChar")));
      this.txtFileLength.ReadOnly = true;
      this.txtFileLength.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtFileLength.RightToLeft")));
      this.txtFileLength.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtFileLength.ScrollBars")));
      this.txtFileLength.Size = ((System.Drawing.Size)(resources.GetObject("txtFileLength.Size")));
      this.txtFileLength.TabIndex = ((int)(resources.GetObject("txtFileLength.TabIndex")));
      this.txtFileLength.Text = resources.GetString("txtFileLength.Text");
      this.txtFileLength.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtFileLength.TextAlign")));
      this.txtFileLength.Visible = ((bool)(resources.GetObject("txtFileLength.Visible")));
      this.txtFileLength.WordWrap = ((bool)(resources.GetObject("txtFileLength.WordWrap")));
      // 
      // lblFileLength
      // 
      this.lblFileLength.AccessibleDescription = resources.GetString("lblFileLength.AccessibleDescription");
      this.lblFileLength.AccessibleName = resources.GetString("lblFileLength.AccessibleName");
      this.lblFileLength.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblFileLength.Anchor")));
      this.lblFileLength.AutoSize = ((bool)(resources.GetObject("lblFileLength.AutoSize")));
      this.lblFileLength.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblFileLength.Dock")));
      this.lblFileLength.Enabled = ((bool)(resources.GetObject("lblFileLength.Enabled")));
      this.lblFileLength.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblFileLength.Font = ((System.Drawing.Font)(resources.GetObject("lblFileLength.Font")));
      this.lblFileLength.Image = ((System.Drawing.Image)(resources.GetObject("lblFileLength.Image")));
      this.lblFileLength.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblFileLength.ImageAlign")));
      this.lblFileLength.ImageIndex = ((int)(resources.GetObject("lblFileLength.ImageIndex")));
      this.lblFileLength.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblFileLength.ImeMode")));
      this.lblFileLength.Location = ((System.Drawing.Point)(resources.GetObject("lblFileLength.Location")));
      this.lblFileLength.Name = "lblFileLength";
      this.lblFileLength.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblFileLength.RightToLeft")));
      this.lblFileLength.Size = ((System.Drawing.Size)(resources.GetObject("lblFileLength.Size")));
      this.lblFileLength.TabIndex = ((int)(resources.GetObject("lblFileLength.TabIndex")));
      this.lblFileLength.Text = resources.GetString("lblFileLength.Text");
      this.lblFileLength.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblFileLength.TextAlign")));
      this.lblFileLength.Visible = ((bool)(resources.GetObject("lblFileLength.Visible")));
      // 
      // txtFormat
      // 
      this.txtFormat.AccessibleDescription = resources.GetString("txtFormat.AccessibleDescription");
      this.txtFormat.AccessibleName = resources.GetString("txtFormat.AccessibleName");
      this.txtFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtFormat.Anchor")));
      this.txtFormat.AutoSize = ((bool)(resources.GetObject("txtFormat.AutoSize")));
      this.txtFormat.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtFormat.BackgroundImage")));
      this.txtFormat.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtFormat.Dock")));
      this.txtFormat.Enabled = ((bool)(resources.GetObject("txtFormat.Enabled")));
      this.txtFormat.Font = ((System.Drawing.Font)(resources.GetObject("txtFormat.Font")));
      this.txtFormat.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtFormat.ImeMode")));
      this.txtFormat.Location = ((System.Drawing.Point)(resources.GetObject("txtFormat.Location")));
      this.txtFormat.MaxLength = ((int)(resources.GetObject("txtFormat.MaxLength")));
      this.txtFormat.Multiline = ((bool)(resources.GetObject("txtFormat.Multiline")));
      this.txtFormat.Name = "txtFormat";
      this.txtFormat.PasswordChar = ((char)(resources.GetObject("txtFormat.PasswordChar")));
      this.txtFormat.ReadOnly = true;
      this.txtFormat.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtFormat.RightToLeft")));
      this.txtFormat.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtFormat.ScrollBars")));
      this.txtFormat.Size = ((System.Drawing.Size)(resources.GetObject("txtFormat.Size")));
      this.txtFormat.TabIndex = ((int)(resources.GetObject("txtFormat.TabIndex")));
      this.txtFormat.Text = resources.GetString("txtFormat.Text");
      this.txtFormat.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtFormat.TextAlign")));
      this.txtFormat.Visible = ((bool)(resources.GetObject("txtFormat.Visible")));
      this.txtFormat.WordWrap = ((bool)(resources.GetObject("txtFormat.WordWrap")));
      // 
      // lblFormat
      // 
      this.lblFormat.AccessibleDescription = resources.GetString("lblFormat.AccessibleDescription");
      this.lblFormat.AccessibleName = resources.GetString("lblFormat.AccessibleName");
      this.lblFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblFormat.Anchor")));
      this.lblFormat.AutoSize = ((bool)(resources.GetObject("lblFormat.AutoSize")));
      this.lblFormat.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblFormat.Dock")));
      this.lblFormat.Enabled = ((bool)(resources.GetObject("lblFormat.Enabled")));
      this.lblFormat.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblFormat.Font = ((System.Drawing.Font)(resources.GetObject("lblFormat.Font")));
      this.lblFormat.Image = ((System.Drawing.Image)(resources.GetObject("lblFormat.Image")));
      this.lblFormat.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblFormat.ImageAlign")));
      this.lblFormat.ImageIndex = ((int)(resources.GetObject("lblFormat.ImageIndex")));
      this.lblFormat.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblFormat.ImeMode")));
      this.lblFormat.Location = ((System.Drawing.Point)(resources.GetObject("lblFormat.Location")));
      this.lblFormat.Name = "lblFormat";
      this.lblFormat.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblFormat.RightToLeft")));
      this.lblFormat.Size = ((System.Drawing.Size)(resources.GetObject("lblFormat.Size")));
      this.lblFormat.TabIndex = ((int)(resources.GetObject("lblFormat.TabIndex")));
      this.lblFormat.Text = resources.GetString("lblFormat.Text");
      this.lblFormat.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblFormat.TextAlign")));
      this.lblFormat.Visible = ((bool)(resources.GetObject("lblFormat.Visible")));
      // 
      // txtFileType
      // 
      this.txtFileType.AccessibleDescription = resources.GetString("txtFileType.AccessibleDescription");
      this.txtFileType.AccessibleName = resources.GetString("txtFileType.AccessibleName");
      this.txtFileType.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtFileType.Anchor")));
      this.txtFileType.AutoSize = ((bool)(resources.GetObject("txtFileType.AutoSize")));
      this.txtFileType.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtFileType.BackgroundImage")));
      this.txtFileType.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtFileType.Dock")));
      this.txtFileType.Enabled = ((bool)(resources.GetObject("txtFileType.Enabled")));
      this.txtFileType.Font = ((System.Drawing.Font)(resources.GetObject("txtFileType.Font")));
      this.txtFileType.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtFileType.ImeMode")));
      this.txtFileType.Location = ((System.Drawing.Point)(resources.GetObject("txtFileType.Location")));
      this.txtFileType.MaxLength = ((int)(resources.GetObject("txtFileType.MaxLength")));
      this.txtFileType.Multiline = ((bool)(resources.GetObject("txtFileType.Multiline")));
      this.txtFileType.Name = "txtFileType";
      this.txtFileType.PasswordChar = ((char)(resources.GetObject("txtFileType.PasswordChar")));
      this.txtFileType.ReadOnly = true;
      this.txtFileType.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtFileType.RightToLeft")));
      this.txtFileType.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtFileType.ScrollBars")));
      this.txtFileType.Size = ((System.Drawing.Size)(resources.GetObject("txtFileType.Size")));
      this.txtFileType.TabIndex = ((int)(resources.GetObject("txtFileType.TabIndex")));
      this.txtFileType.Text = resources.GetString("txtFileType.Text");
      this.txtFileType.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtFileType.TextAlign")));
      this.txtFileType.Visible = ((bool)(resources.GetObject("txtFileType.Visible")));
      this.txtFileType.WordWrap = ((bool)(resources.GetObject("txtFileType.WordWrap")));
      // 
      // lblFileType
      // 
      this.lblFileType.AccessibleDescription = resources.GetString("lblFileType.AccessibleDescription");
      this.lblFileType.AccessibleName = resources.GetString("lblFileType.AccessibleName");
      this.lblFileType.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblFileType.Anchor")));
      this.lblFileType.AutoSize = ((bool)(resources.GetObject("lblFileType.AutoSize")));
      this.lblFileType.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblFileType.Dock")));
      this.lblFileType.Enabled = ((bool)(resources.GetObject("lblFileType.Enabled")));
      this.lblFileType.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblFileType.Font = ((System.Drawing.Font)(resources.GetObject("lblFileType.Font")));
      this.lblFileType.Image = ((System.Drawing.Image)(resources.GetObject("lblFileType.Image")));
      this.lblFileType.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblFileType.ImageAlign")));
      this.lblFileType.ImageIndex = ((int)(resources.GetObject("lblFileType.ImageIndex")));
      this.lblFileType.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblFileType.ImeMode")));
      this.lblFileType.Location = ((System.Drawing.Point)(resources.GetObject("lblFileType.Location")));
      this.lblFileType.Name = "lblFileType";
      this.lblFileType.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblFileType.RightToLeft")));
      this.lblFileType.Size = ((System.Drawing.Size)(resources.GetObject("lblFileType.Size")));
      this.lblFileType.TabIndex = ((int)(resources.GetObject("lblFileType.TabIndex")));
      this.lblFileType.Text = resources.GetString("lblFileType.Text");
      this.lblFileType.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblFileType.TextAlign")));
      this.lblFileType.Visible = ((bool)(resources.GetObject("lblFileType.Visible")));
      // 
      // txtLocation
      // 
      this.txtLocation.AccessibleDescription = resources.GetString("txtLocation.AccessibleDescription");
      this.txtLocation.AccessibleName = resources.GetString("txtLocation.AccessibleName");
      this.txtLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("txtLocation.Anchor")));
      this.txtLocation.AutoSize = ((bool)(resources.GetObject("txtLocation.AutoSize")));
      this.txtLocation.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtLocation.BackgroundImage")));
      this.txtLocation.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("txtLocation.Dock")));
      this.txtLocation.Enabled = ((bool)(resources.GetObject("txtLocation.Enabled")));
      this.txtLocation.Font = ((System.Drawing.Font)(resources.GetObject("txtLocation.Font")));
      this.txtLocation.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("txtLocation.ImeMode")));
      this.txtLocation.Location = ((System.Drawing.Point)(resources.GetObject("txtLocation.Location")));
      this.txtLocation.MaxLength = ((int)(resources.GetObject("txtLocation.MaxLength")));
      this.txtLocation.Multiline = ((bool)(resources.GetObject("txtLocation.Multiline")));
      this.txtLocation.Name = "txtLocation";
      this.txtLocation.PasswordChar = ((char)(resources.GetObject("txtLocation.PasswordChar")));
      this.txtLocation.ReadOnly = true;
      this.txtLocation.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("txtLocation.RightToLeft")));
      this.txtLocation.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("txtLocation.ScrollBars")));
      this.txtLocation.Size = ((System.Drawing.Size)(resources.GetObject("txtLocation.Size")));
      this.txtLocation.TabIndex = ((int)(resources.GetObject("txtLocation.TabIndex")));
      this.txtLocation.Text = resources.GetString("txtLocation.Text");
      this.txtLocation.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("txtLocation.TextAlign")));
      this.txtLocation.Visible = ((bool)(resources.GetObject("txtLocation.Visible")));
      this.txtLocation.WordWrap = ((bool)(resources.GetObject("txtLocation.WordWrap")));
      // 
      // lblLocation
      // 
      this.lblLocation.AccessibleDescription = resources.GetString("lblLocation.AccessibleDescription");
      this.lblLocation.AccessibleName = resources.GetString("lblLocation.AccessibleName");
      this.lblLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("lblLocation.Anchor")));
      this.lblLocation.AutoSize = ((bool)(resources.GetObject("lblLocation.AutoSize")));
      this.lblLocation.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("lblLocation.Dock")));
      this.lblLocation.Enabled = ((bool)(resources.GetObject("lblLocation.Enabled")));
      this.lblLocation.FlatStyle = System.Windows.Forms.FlatStyle.System;
      this.lblLocation.Font = ((System.Drawing.Font)(resources.GetObject("lblLocation.Font")));
      this.lblLocation.Image = ((System.Drawing.Image)(resources.GetObject("lblLocation.Image")));
      this.lblLocation.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblLocation.ImageAlign")));
      this.lblLocation.ImageIndex = ((int)(resources.GetObject("lblLocation.ImageIndex")));
      this.lblLocation.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("lblLocation.ImeMode")));
      this.lblLocation.Location = ((System.Drawing.Point)(resources.GetObject("lblLocation.Location")));
      this.lblLocation.Name = "lblLocation";
      this.lblLocation.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("lblLocation.RightToLeft")));
      this.lblLocation.Size = ((System.Drawing.Size)(resources.GetObject("lblLocation.Size")));
      this.lblLocation.TabIndex = ((int)(resources.GetObject("lblLocation.TabIndex")));
      this.lblLocation.Text = resources.GetString("lblLocation.Text");
      this.lblLocation.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("lblLocation.TextAlign")));
      this.lblLocation.Visible = ((bool)(resources.GetObject("lblLocation.Visible")));
      // 
      // tabBrowsers
      // 
      this.tabBrowsers.AccessibleDescription = resources.GetString("tabBrowsers.AccessibleDescription");
      this.tabBrowsers.AccessibleName = resources.GetString("tabBrowsers.AccessibleName");
      this.tabBrowsers.Alignment = ((System.Windows.Forms.TabAlignment)(resources.GetObject("tabBrowsers.Alignment")));
      this.tabBrowsers.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("tabBrowsers.Anchor")));
      this.tabBrowsers.Appearance = ((System.Windows.Forms.TabAppearance)(resources.GetObject("tabBrowsers.Appearance")));
      this.tabBrowsers.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tabBrowsers.BackgroundImage")));
      this.tabBrowsers.Controls.Add(this.tabMusicBrowser);
      this.tabBrowsers.Controls.Add(this.tabSoundBrowser);
      this.tabBrowsers.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("tabBrowsers.Dock")));
      this.tabBrowsers.Enabled = ((bool)(resources.GetObject("tabBrowsers.Enabled")));
      this.tabBrowsers.Font = ((System.Drawing.Font)(resources.GetObject("tabBrowsers.Font")));
      this.tabBrowsers.ImageList = this.ilMusicBrowserIcons;
      this.tabBrowsers.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("tabBrowsers.ImeMode")));
      this.tabBrowsers.ItemSize = ((System.Drawing.Size)(resources.GetObject("tabBrowsers.ItemSize")));
      this.tabBrowsers.Location = ((System.Drawing.Point)(resources.GetObject("tabBrowsers.Location")));
      this.tabBrowsers.Name = "tabBrowsers";
      this.tabBrowsers.Padding = ((System.Drawing.Point)(resources.GetObject("tabBrowsers.Padding")));
      this.tabBrowsers.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("tabBrowsers.RightToLeft")));
      this.tabBrowsers.SelectedIndex = 0;
      this.tabBrowsers.ShowToolTips = ((bool)(resources.GetObject("tabBrowsers.ShowToolTips")));
      this.tabBrowsers.Size = ((System.Drawing.Size)(resources.GetObject("tabBrowsers.Size")));
      this.tabBrowsers.TabIndex = ((int)(resources.GetObject("tabBrowsers.TabIndex")));
      this.tabBrowsers.Text = resources.GetString("tabBrowsers.Text");
      this.tabBrowsers.Visible = ((bool)(resources.GetObject("tabBrowsers.Visible")));
      this.tabBrowsers.SelectedIndexChanged += new System.EventHandler(this.tabBrowsers_SelectedIndexChanged);
      // 
      // tabMusicBrowser
      // 
      this.tabMusicBrowser.AccessibleDescription = resources.GetString("tabMusicBrowser.AccessibleDescription");
      this.tabMusicBrowser.AccessibleName = resources.GetString("tabMusicBrowser.AccessibleName");
      this.tabMusicBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("tabMusicBrowser.Anchor")));
      this.tabMusicBrowser.AutoScroll = ((bool)(resources.GetObject("tabMusicBrowser.AutoScroll")));
      this.tabMusicBrowser.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("tabMusicBrowser.AutoScrollMargin")));
      this.tabMusicBrowser.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("tabMusicBrowser.AutoScrollMinSize")));
      this.tabMusicBrowser.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tabMusicBrowser.BackgroundImage")));
      this.tabMusicBrowser.Controls.Add(this.tvMusicBrowser);
      this.tabMusicBrowser.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("tabMusicBrowser.Dock")));
      this.tabMusicBrowser.Enabled = ((bool)(resources.GetObject("tabMusicBrowser.Enabled")));
      this.tabMusicBrowser.Font = ((System.Drawing.Font)(resources.GetObject("tabMusicBrowser.Font")));
      this.tabMusicBrowser.ImageIndex = ((int)(resources.GetObject("tabMusicBrowser.ImageIndex")));
      this.tabMusicBrowser.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("tabMusicBrowser.ImeMode")));
      this.tabMusicBrowser.Location = ((System.Drawing.Point)(resources.GetObject("tabMusicBrowser.Location")));
      this.tabMusicBrowser.Name = "tabMusicBrowser";
      this.tabMusicBrowser.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("tabMusicBrowser.RightToLeft")));
      this.tabMusicBrowser.Size = ((System.Drawing.Size)(resources.GetObject("tabMusicBrowser.Size")));
      this.tabMusicBrowser.TabIndex = ((int)(resources.GetObject("tabMusicBrowser.TabIndex")));
      this.tabMusicBrowser.Text = resources.GetString("tabMusicBrowser.Text");
      this.tabMusicBrowser.ToolTipText = resources.GetString("tabMusicBrowser.ToolTipText");
      this.tabMusicBrowser.Visible = ((bool)(resources.GetObject("tabMusicBrowser.Visible")));
      // 
      // tvMusicBrowser
      // 
      this.tvMusicBrowser.AccessibleDescription = resources.GetString("tvMusicBrowser.AccessibleDescription");
      this.tvMusicBrowser.AccessibleName = resources.GetString("tvMusicBrowser.AccessibleName");
      this.tvMusicBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("tvMusicBrowser.Anchor")));
      this.tvMusicBrowser.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tvMusicBrowser.BackgroundImage")));
      this.tvMusicBrowser.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("tvMusicBrowser.Dock")));
      this.tvMusicBrowser.Enabled = ((bool)(resources.GetObject("tvMusicBrowser.Enabled")));
      this.tvMusicBrowser.Font = ((System.Drawing.Font)(resources.GetObject("tvMusicBrowser.Font")));
      this.tvMusicBrowser.HideSelection = false;
      this.tvMusicBrowser.HotTracking = true;
      this.tvMusicBrowser.ImageIndex = ((int)(resources.GetObject("tvMusicBrowser.ImageIndex")));
      this.tvMusicBrowser.ImageList = this.ilMusicBrowserIcons;
      this.tvMusicBrowser.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("tvMusicBrowser.ImeMode")));
      this.tvMusicBrowser.Indent = ((int)(resources.GetObject("tvMusicBrowser.Indent")));
      this.tvMusicBrowser.ItemHeight = ((int)(resources.GetObject("tvMusicBrowser.ItemHeight")));
      this.tvMusicBrowser.Location = ((System.Drawing.Point)(resources.GetObject("tvMusicBrowser.Location")));
      this.tvMusicBrowser.Name = "tvMusicBrowser";
      this.tvMusicBrowser.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("tvMusicBrowser.RightToLeft")));
      this.tvMusicBrowser.SelectedImageIndex = ((int)(resources.GetObject("tvMusicBrowser.SelectedImageIndex")));
      this.tvMusicBrowser.Size = ((System.Drawing.Size)(resources.GetObject("tvMusicBrowser.Size")));
      this.tvMusicBrowser.TabIndex = ((int)(resources.GetObject("tvMusicBrowser.TabIndex")));
      this.tvMusicBrowser.Text = resources.GetString("tvMusicBrowser.Text");
      this.tvMusicBrowser.Visible = ((bool)(resources.GetObject("tvMusicBrowser.Visible")));
      this.tvMusicBrowser.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvBrowser_AfterExpand);
      this.tvMusicBrowser.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tvBrowser_AfterCollapse);
      this.tvMusicBrowser.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvBrowser_AfterSelect);
      // 
      // tabSoundBrowser
      // 
      this.tabSoundBrowser.AccessibleDescription = resources.GetString("tabSoundBrowser.AccessibleDescription");
      this.tabSoundBrowser.AccessibleName = resources.GetString("tabSoundBrowser.AccessibleName");
      this.tabSoundBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("tabSoundBrowser.Anchor")));
      this.tabSoundBrowser.AutoScroll = ((bool)(resources.GetObject("tabSoundBrowser.AutoScroll")));
      this.tabSoundBrowser.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("tabSoundBrowser.AutoScrollMargin")));
      this.tabSoundBrowser.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("tabSoundBrowser.AutoScrollMinSize")));
      this.tabSoundBrowser.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tabSoundBrowser.BackgroundImage")));
      this.tabSoundBrowser.Controls.Add(this.tvSoundBrowser);
      this.tabSoundBrowser.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("tabSoundBrowser.Dock")));
      this.tabSoundBrowser.Enabled = ((bool)(resources.GetObject("tabSoundBrowser.Enabled")));
      this.tabSoundBrowser.Font = ((System.Drawing.Font)(resources.GetObject("tabSoundBrowser.Font")));
      this.tabSoundBrowser.ImageIndex = ((int)(resources.GetObject("tabSoundBrowser.ImageIndex")));
      this.tabSoundBrowser.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("tabSoundBrowser.ImeMode")));
      this.tabSoundBrowser.Location = ((System.Drawing.Point)(resources.GetObject("tabSoundBrowser.Location")));
      this.tabSoundBrowser.Name = "tabSoundBrowser";
      this.tabSoundBrowser.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("tabSoundBrowser.RightToLeft")));
      this.tabSoundBrowser.Size = ((System.Drawing.Size)(resources.GetObject("tabSoundBrowser.Size")));
      this.tabSoundBrowser.TabIndex = ((int)(resources.GetObject("tabSoundBrowser.TabIndex")));
      this.tabSoundBrowser.Text = resources.GetString("tabSoundBrowser.Text");
      this.tabSoundBrowser.ToolTipText = resources.GetString("tabSoundBrowser.ToolTipText");
      this.tabSoundBrowser.Visible = ((bool)(resources.GetObject("tabSoundBrowser.Visible")));
      // 
      // tvSoundBrowser
      // 
      this.tvSoundBrowser.AccessibleDescription = resources.GetString("tvSoundBrowser.AccessibleDescription");
      this.tvSoundBrowser.AccessibleName = resources.GetString("tvSoundBrowser.AccessibleName");
      this.tvSoundBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("tvSoundBrowser.Anchor")));
      this.tvSoundBrowser.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tvSoundBrowser.BackgroundImage")));
      this.tvSoundBrowser.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("tvSoundBrowser.Dock")));
      this.tvSoundBrowser.Enabled = ((bool)(resources.GetObject("tvSoundBrowser.Enabled")));
      this.tvSoundBrowser.Font = ((System.Drawing.Font)(resources.GetObject("tvSoundBrowser.Font")));
      this.tvSoundBrowser.HideSelection = false;
      this.tvSoundBrowser.HotTracking = true;
      this.tvSoundBrowser.ImageIndex = ((int)(resources.GetObject("tvSoundBrowser.ImageIndex")));
      this.tvSoundBrowser.ImageList = this.ilSoundBrowserIcons;
      this.tvSoundBrowser.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("tvSoundBrowser.ImeMode")));
      this.tvSoundBrowser.Indent = ((int)(resources.GetObject("tvSoundBrowser.Indent")));
      this.tvSoundBrowser.ItemHeight = ((int)(resources.GetObject("tvSoundBrowser.ItemHeight")));
      this.tvSoundBrowser.Location = ((System.Drawing.Point)(resources.GetObject("tvSoundBrowser.Location")));
      this.tvSoundBrowser.Name = "tvSoundBrowser";
      this.tvSoundBrowser.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("tvSoundBrowser.RightToLeft")));
      this.tvSoundBrowser.SelectedImageIndex = ((int)(resources.GetObject("tvSoundBrowser.SelectedImageIndex")));
      this.tvSoundBrowser.Size = ((System.Drawing.Size)(resources.GetObject("tvSoundBrowser.Size")));
      this.tvSoundBrowser.TabIndex = ((int)(resources.GetObject("tvSoundBrowser.TabIndex")));
      this.tvSoundBrowser.Text = resources.GetString("tvSoundBrowser.Text");
      this.tvSoundBrowser.Visible = ((bool)(resources.GetObject("tvSoundBrowser.Visible")));
      this.tvSoundBrowser.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvBrowser_AfterExpand);
      this.tvSoundBrowser.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tvBrowser_AfterCollapse);
      this.tvSoundBrowser.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvBrowser_AfterSelect);
      // 
      // ilSoundBrowserIcons
      // 
      this.ilSoundBrowserIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
      this.ilSoundBrowserIcons.ImageSize = ((System.Drawing.Size)(resources.GetObject("ilSoundBrowserIcons.ImageSize")));
      this.ilSoundBrowserIcons.TransparentColor = System.Drawing.Color.Transparent;
      // 
      // dlgSaveWave
      // 
      this.dlgSaveWave.DefaultExt = "wav";
      this.dlgSaveWave.Filter = resources.GetString("dlgSaveWave.Filter");
      this.dlgSaveWave.RestoreDirectory = true;
      this.dlgSaveWave.Title = resources.GetString("dlgSaveWave.Title");
      // 
      // MainWindow
      // 
      this.AccessibleDescription = resources.GetString("$this.AccessibleDescription");
      this.AccessibleName = resources.GetString("$this.AccessibleName");
      this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
      this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
      this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
      this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
      this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
      this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
      this.Controls.Add(this.tabBrowsers);
      this.Controls.Add(this.pnlInfoArea);
      this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
      this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
      this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
      this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
      this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
      this.Name = "MainWindow";
      this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
      this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
      this.Text = resources.GetString("$this.Text");
      this.Closed += new System.EventHandler(this.MainWindow_Closed);
      this.VisibleChanged += new System.EventHandler(this.MainWindow_VisibleChanged);
      this.pnlInfoArea.ResumeLayout(false);
      this.grpFileInfo.ResumeLayout(false);
      this.tabBrowsers.ResumeLayout(false);
      this.tabMusicBrowser.ResumeLayout(false);
      this.tabSoundBrowser.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private void MainWindow_Closed(object sender, System.EventArgs e) {
      this.StopPlayback();
    }

    private void MainWindow_VisibleChanged(object sender, System.EventArgs e) {
      if (this.Visible) {
      TreeView Browser = null;
	switch (this.tabBrowsers.SelectedIndex) {
	  case 0: Browser = this.tvMusicBrowser; break;
	  case 1: Browser = this.tvSoundBrowser; break;
	}
	if (Browser != null && Browser.Nodes.Count == 0) {
	  this.Show();
	  Application.DoEvents();
	  this.Activate();
	  Application.DoEvents();
	  this.Refresh();
	  Application.DoEvents();
	  this.RefreshBrowser();
	}
      }
    }

    private void tvBrowser_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e) {
      this.ClearFileInfo();
    object data = e.Node.Tag;
    FileInfo FI = data as FileInfo;
      if (FI == null) { // Root or App Directory
	if (e.Node.Parent == null)
	  this.txtLocation.Text = "<Root Node>";
	else
	  this.txtLocation.Text = data as string;
	this.txtFileType.Text = "Folder";
      }
      else { // File
	this.txtLocation.Text = FI.Location;
	this.txtTitle.Text    = FI.Title;
	this.txtComposer.Text = FI.Composer;
	if (FI != null && FI.AudioFile != null) {
	  switch (FI.AudioFile.Type) {
	    case AudioFileType.SoundEffect:
	      this.txtFileType.Text = "Sound Effect";
	      break;
	    case AudioFileType.BGMStream:
	      this.txtFileType.Text = "BGM Music Stream";
	      break;
	    case AudioFileType.Unknown:
	      this.txtFileType.Text = "Unknown Type";
	      break;
	  }
	  this.txtFormat.Text     = String.Format("{0}-channel {1}-bit {2}Hz {3}", FI.AudioFile.Channels, FI.AudioFile.BitsPerSample, FI.AudioFile.SampleRate, FI.AudioFile.SampleFormat);
	  this.txtFileLength.Text = this.LengthText(FI.AudioFile.Length);
	  this.btnDecode.Enabled  = true;
	  this.btnPlay.Enabled    = this.AudioEnabled;
	}
	else
	  this.txtFileType.Text = "Folder";
      }
    }

    private void tvBrowser_AfterExpand(object sender, System.Windows.Forms.TreeViewEventArgs e) {
      if (e.Node.Parent != null && e.Node.Nodes.Count != 0) { // Folders only
	++e.Node.ImageIndex;
	++e.Node.SelectedImageIndex;
      }
    }

    private void tvBrowser_AfterCollapse(object sender, System.Windows.Forms.TreeViewEventArgs e) {
      if (e.Node.Parent != null && e.Node.Nodes.Count != 0) { // Folders only
	--e.Node.ImageIndex;
	--e.Node.SelectedImageIndex;
      }
    }

    private void tabBrowsers_SelectedIndexChanged(object sender, System.EventArgs e) {
      this.MaybeRefreshBrowser();
    }

    private void btnDecode_Click(object sender, System.EventArgs e) {
    TreeNode TN = null;
      switch (this.tabBrowsers.SelectedIndex) {
	case 0: TN = this.tvMusicBrowser.SelectedNode; break;
	case 1: TN = this.tvSoundBrowser.SelectedNode; break;
      }
      if (TN != null) {
      FileInfo FI = TN.Tag as FileInfo;
	if (FI != null && FI.AudioFile != null) {
	  this.dlgSaveWave.FileName = TN.Text;
	  if (this.dlgSaveWave.ShowDialog() == DialogResult.OK) {
	    using (WaveWriter WW = new WaveWriter(FI.AudioFile, this.dlgSaveWave.FileName))
	      WW.ShowDialog(this);
	  }
	}
      }
    }

    private void btnPlay_Click(object sender, System.EventArgs e) {
    TreeNode TN = null;
      switch (this.tabBrowsers.SelectedIndex) {
	case 0: TN = this.tvMusicBrowser.SelectedNode; break;
	case 1: TN = this.tvSoundBrowser.SelectedNode; break;
      }
      if (TN == null)
	return;
    FileInfo FI = TN.Tag as FileInfo;
      if (FI == null || FI.AudioFile == null)
	return;
      this.PlayFile(FI);
    }

    private void btnPause_Click(object sender, System.EventArgs e) {
      this.PausePlayback();
    }

    private void btnStop_Click(object sender, System.EventArgs e) {
      this.StopPlayback();
    }

  }

}
