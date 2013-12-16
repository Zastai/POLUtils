// $Id$

// Copyright © 2004-2010 Tim Van Holder
// 
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS"
// BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

// This needs work - the current code is based on the original "buffered playback" code, and it mostly works quite nicely.
// However, there are occasional access violations, so perhaps it should be rewritten to use XAudio (which does add even more dependencies).
using SharpDX;
using SharpDX.DirectSound;
using SharpDX.Multimedia;

using PlayOnline.Core;
using PlayOnline.Core.Audio;

namespace PlayOnline.Utils.AudioManager {

  public partial class MainWindow : Form {

    public MainWindow() {
      this.InitializeComponent();
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

    private void RefreshBrowser() {
      this.ClearFileInfo();
      using (var ib = new InfoBuilder()) {
        Application.DoEvents();
        // Clear treeview, create main root and fill them
        switch (this.tabBrowsers.SelectedIndex) {
          case 0: { // Music
            this.tvMusicBrowser.Nodes.Clear();
            var root = new TreeNode("Music");
            this.tvMusicBrowser.Nodes.Add(root);
            ib.TargetNode = root;
            ib.FileTypeName = root.Text;
            ib.FilePattern = "*.bgw";
            ib.ResourceName = "PlayOnline.Utils.AudioManager.MusicInfo.xml";
            ib.ShowDialog(this);
            root.Expand();
            this.tvMusicBrowser.SelectedNode = root;
            break;
          }
          case 1: { // Sound Effects
            this.tvSoundBrowser.Nodes.Clear();
            var root = new TreeNode("Sound Effects");
            this.tvSoundBrowser.Nodes.Add(root);
            ib.TargetNode = root;
            ib.FileTypeName = root.Text;
            ib.FilePattern = "*.spw";
            ib.ResourceName = "PlayOnline.Utils.AudioManager.SFXInfo.xml";
            ib.ShowDialog(this);
            root.Expand();
            this.tvSoundBrowser.SelectedNode = root;
            break;
          }
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
      foreach (var tb in this.grpFileInfo.Controls.OfType<TextBox>())
        tb.Text = null;
      this.btnPlay.Enabled = false;
      this.btnDecode.Enabled = false;
    }

    private string LengthText(double seconds) {
      var FileLength = new TimeSpan((long) (seconds * 10000000));
      var Result = string.Format("{0}s", FileLength.Seconds);
      if (FileLength.Minutes > 0)
        Result = String.Format("{0}m ", FileLength.Minutes) + Result;
      if (FileLength.TotalHours >= 1)
        Result = String.Format("{0}h ", (long) Math.Floor(FileLength.TotalHours)) + Result;
      return Result;
    }

    private readonly int    AudioBufferMarkers = 4;
    private readonly int    AudioBufferSize    = 1024 * 1024;
    private bool            AudioIsLooping     = false;
    private AutoResetEvent  AudioUpdateTrigger = null;
    private Thread          AudioUpdateThread  = null;
    private AudioFileStream CurrentStream      = null;

    private DirectSound          DS            = null;
    private SecondarySoundBuffer CurrentBuffer = null;

    private void AudioUpdate() {
      while (this.CurrentBuffer != null && this.CurrentStream != null && this.AudioUpdateThread == Thread.CurrentThread) {
        if (this.AudioUpdateTrigger.WaitOne(100, true))
          this.UpdateBufferContents();
      }
    }

    private void UpdateBufferContents() {
      lock (this) {
        if (this.CurrentBuffer == null || this.CurrentStream == null)
          return;
        // Determine the proper update location
        var chunksize = this.AudioBufferSize / this.AudioBufferMarkers;
        var startpos  = (int) this.CurrentStream.Position;
        var endpos    = startpos + chunksize;
        // Normalize it vs the buffer size
        startpos %= this.AudioBufferSize;
        endpos   %= this.AudioBufferSize;
        // Ensure the region we want to write isn't currently being played
        int playpos, writepos;
        this.CurrentBuffer.GetCurrentPosition(out playpos, out writepos);
        if ((endpos < startpos && (playpos >= startpos || playpos < endpos)) || (endpos > startpos && playpos >= startpos && playpos < endpos)) {
          // If we're at the end of the file, stop playback
          if (this.CurrentStream.Position == this.CurrentStream.Length)
            goto AtEnd; // Need to escape the lock block, since StopPlayback() also locks the form
          return;
        }
        // Write the data
        var bytes = new byte[chunksize];
        var readbytes = this.CurrentStream.Read(bytes, 0, chunksize);
        if (readbytes < chunksize)
          Array.Clear(bytes, readbytes, chunksize - readbytes);
        if (endpos < startpos && endpos != 0) // Adjust it so we write until the end of the buffer
          chunksize = this.AudioBufferSize - startpos;
        DataStream audiodata2;
        var audiodata1 = this.CurrentBuffer.Lock(startpos, chunksize, LockFlags.None, out audiodata2);
        audiodata1.Write(bytes, 0, chunksize);
        this.CurrentBuffer.Unlock(audiodata1, audiodata2);
        if (endpos < startpos && endpos != 0) { // Now write the rest at the start of the buffer
          audiodata1 = this.CurrentBuffer.Lock(0, endpos, LockFlags.None, out audiodata2);
          audiodata1.Write(bytes, chunksize, endpos);
          this.CurrentBuffer.Unlock(audiodata1, audiodata2);
        }
      }
      return;
    AtEnd:
      this.Invoke(new Action(this.StopPlayback));
    }

    private void PausePlayback() {
      if ((this.CurrentBuffer.Status & (int) BufferStatus.Playing) == (int) BufferStatus.Playing) {
        this.CurrentBuffer.Stop();
        this.btnPause.Text = "&Resume";
      }
      else {
        this.CurrentBuffer.Play(0, (this.AudioIsLooping ? PlayFlags.Looping : PlayFlags.None));
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
        this.AudioUpdateThread = null;
      }
      this.btnPause.Enabled = false;
      this.btnPause.Text = "Pa&use";
      this.btnStop.Enabled = false;
      this.AudioIsLooping = false;
    }
    
    private void PlayFile(FileInfo FI) {
      lock (this) {
        if (this.DS == null) {
          this.DS = new DirectSound();
          this.DS.SetCooperativeLevel(this.Handle, CooperativeLevel.Normal);
        }
        this.StopPlayback();
        var bd = new SoundBufferDescription {
          Format      = new WaveFormat(FI.AudioFile.SampleRate, 16, FI.AudioFile.Channels),
          BufferBytes = this.AudioBufferSize,
          Flags       = BufferFlags.GlobalFocus | BufferFlags.StickyFocus | BufferFlags.ControlVolume | BufferFlags.GetCurrentPosition2 | BufferFlags.ControlPositionNotify
        };
        this.CurrentBuffer = new SecondarySoundBuffer(this.DS, bd);
        if (this.AudioUpdateTrigger == null)
          this.AudioUpdateTrigger = new AutoResetEvent(false);
        var chunkSize = this.AudioBufferSize / this.AudioBufferMarkers;
        var updatePositions = new NotificationPosition[this.AudioBufferMarkers];
        for (var i = 0; i < this.AudioBufferMarkers; ++i) {
          updatePositions[i] = new NotificationPosition() {
            WaitHandle = this.AudioUpdateTrigger,
            Offset = chunkSize * i
          };
        }
        this.CurrentBuffer.SetNotificationPositions(updatePositions);
        this.CurrentStream = FI.AudioFile.OpenStream();
        {
          var bytes = new byte[this.CurrentBuffer.Capabilities.BufferBytes];
          var readbytes = this.CurrentStream.Read(bytes, 0, this.CurrentBuffer.Capabilities.BufferBytes);
          if (readbytes < this.CurrentBuffer.Capabilities.BufferBytes)
            Array.Clear(bytes, readbytes, this.CurrentBuffer.Capabilities.BufferBytes - readbytes);
          DataStream audiodata2;
          var audiodata1 = this.CurrentBuffer.Lock(0, this.CurrentBuffer.Capabilities.BufferBytes, LockFlags.EntireBuffer, out audiodata2);
          audiodata1.Write(bytes, 0, this.CurrentBuffer.Capabilities.BufferBytes);
          this.CurrentBuffer.Unlock(audiodata1, audiodata2);
        }
        if (this.CurrentStream.Position < this.CurrentStream.Length) {
          this.AudioUpdateTrigger.Reset();
          this.AudioUpdateThread = new Thread(this.AudioUpdate);
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
        this.CurrentBuffer.Play(0, (this.AudioIsLooping ? PlayFlags.Looping : PlayFlags.None));
      }
    }

    private void MainWindow_Closed(object sender, System.EventArgs e) {
      this.StopPlayback();
    }

    private void MainWindow_VisibleChanged(object sender, System.EventArgs e) {
      if (!this.Visible)
        return;
      TreeView browser = null;
      switch (this.tabBrowsers.SelectedIndex) {
        case 0: browser = this.tvMusicBrowser; break;
        case 1: browser = this.tvSoundBrowser; break;
      }
      if (browser == null || browser.Nodes.Count != 0)
        return;
      this.Show();
      Application.DoEvents();
      this.Activate();
      Application.DoEvents();
      this.Refresh();
      Application.DoEvents();
      this.RefreshBrowser();
    }

    private void tvBrowser_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e) {
      this.ClearFileInfo();
    var data = e.Node.Tag;
    var fi = data as FileInfo;
      if (fi == null) { // Root or App Directory
        if (e.Node.Parent == null)
          this.txtLocation.Text = "<Root Node>";
        else
          this.txtLocation.Text = data as string;
        this.txtFileType.Text = "Folder";
      }
      else { // File
        this.txtLocation.Text = fi.Location;
        this.txtTitle.Text    = fi.Title;
        this.txtComposer.Text = fi.Composer;
        if (fi.AudioFile != null) {
          switch (fi.AudioFile.Type) {
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
          this.txtFormat.Text     = String.Format("{0}-channel {1}-bit {2}Hz {3}", fi.AudioFile.Channels, fi.AudioFile.BitsPerSample, fi.AudioFile.SampleRate, fi.AudioFile.SampleFormat);
          this.txtFileLength.Text = this.LengthText(fi.AudioFile.Length);
          this.btnDecode.Enabled  = fi.AudioFile.Playable;
          this.btnPlay.Enabled    = fi.AudioFile.Playable;
        }
        else
          this.txtFileType.Text = "Folder";
      }
    }

    private void tvBrowser_AfterExpand(object sender, System.Windows.Forms.TreeViewEventArgs e) {
      if (e.Node.Parent == null || e.Node.Nodes.Count == 0)
        return; // Folders only
      ++e.Node.ImageIndex;
      ++e.Node.SelectedImageIndex;
    }

    private void tvBrowser_AfterCollapse(object sender, System.Windows.Forms.TreeViewEventArgs e) {
      if (e.Node.Parent == null || e.Node.Nodes.Count == 0)
        return; // Folders only
      --e.Node.ImageIndex;
      --e.Node.SelectedImageIndex;
    }

    private void tabBrowsers_SelectedIndexChanged(object sender, System.EventArgs e) {
      this.MaybeRefreshBrowser();
    }

    private void btnDecode_Click(object sender, System.EventArgs e) {
    TreeNode tn = null;
      switch (this.tabBrowsers.SelectedIndex) {
        case 0: tn = this.tvMusicBrowser.SelectedNode; break;
        case 1: tn = this.tvSoundBrowser.SelectedNode; break;
      }
      if (tn == null)
        return;
      var fi = tn.Tag as FileInfo;
      if (fi == null || fi.AudioFile == null)
        return;
      this.dlgSaveWave.FileName = tn.Text;
      if (this.dlgSaveWave.ShowDialog() != DialogResult.OK)
        return;
      try {
        var safeName = Path.GetInvalidPathChars().Aggregate(this.dlgSaveWave.FileName, (current, C) => current.Replace(C, '_'));
        using (var ww = new WaveWriter(fi.AudioFile, safeName))
          ww.ShowDialog(this);
      }
      catch (Exception ex) {
        MessageBox.Show("Failed to decode audio file: " + ex.Message, "Audio Decode Failed");
      }
    }

    private void btnPlay_Click(object sender, System.EventArgs e) {
    TreeNode tn = null;
      switch (this.tabBrowsers.SelectedIndex) {
        case 0: tn = this.tvMusicBrowser.SelectedNode; break;
        case 1: tn = this.tvSoundBrowser.SelectedNode; break;
      }
      if (tn == null)
        return;
    var fi = tn.Tag as FileInfo;
      if (fi == null || fi.AudioFile == null)
        return;
      this.PlayFile(fi);
    }

    private void btnPause_Click(object sender, System.EventArgs e) {
      this.PausePlayback();
    }

    private void btnStop_Click(object sender, System.EventArgs e) {
      this.StopPlayback();
    }

  }

}
