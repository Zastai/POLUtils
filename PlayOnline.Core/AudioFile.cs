using System;
using System.IO;
using System.Text;

namespace PlayOnline.Core.Audio {

  public enum AudioFileType {
    Unknown,
    BGMStream,
    SoundEffect,
  }

  public enum SampleFormat {
    PCM,
    ADPCM,
  }

  internal class AudioFileHeader {
    public Int32 Size;
    public Int32 Raw;
    public Int32 ID;
    public Int32 SampleBlocks;
    public Int32 LoopStart;
    public Int32 SampleRateLow;
    public Int32 SampleRateHigh;
    public Int32 Unknown1;
    public byte  Unknown2;
    public byte  Unknown3;
    public byte  Channels;
    public byte  BlockSize;
    public Int32 Unknown4;
  }

  public class AudioFile {

    // === Data === //

    private string          Path_;
    private AudioFileType   Type_;
    private AudioFileHeader Header_;

    // === Public Member Functions === //

    public AudioFile(string Path) {
      this.Path_ = Path;
      try {
      BinaryReader BR = new BinaryReader(new FileStream(this.Path_, FileMode.Open, FileAccess.Read, FileShare.ReadWrite, 0x30), Encoding.ASCII);
	this.DetermineType(BR);
	if (this.Type_ != AudioFileType.Unknown) {
	  this.Header_ = new AudioFileHeader();
	  this.Header_.Size           = BR.ReadInt32();
	  this.Header_.Raw            = ((this.Type_ == AudioFileType.SoundEffect) ? BR.ReadInt32() : 0);
	  this.Header_.ID             = BR.ReadInt32();
	  this.Header_.SampleBlocks   = BR.ReadInt32();
	  this.Header_.LoopStart      = BR.ReadInt32();
	  this.Header_.SampleRateHigh = BR.ReadInt32();
	  this.Header_.SampleRateLow  = BR.ReadInt32();
	  this.Header_.Unknown1       = BR.ReadInt32();
	  this.Header_.Unknown2       = BR.ReadByte ();
	  this.Header_.Unknown3       = BR.ReadByte ();
	  this.Header_.Channels       = BR.ReadByte ();
	  this.Header_.BlockSize      = BR.ReadByte ();
	  this.Header_.Unknown4       = ((this.Type_ == AudioFileType.SoundEffect) ? BR.ReadInt32() : 0);
	}
	BR.Close();
      }
      catch {
	this.Type_   = AudioFileType.Unknown;
	this.Header_ = new AudioFileHeader();
      }
    }

    public AudioFileStream OpenStream() {
      return this.OpenStream(false);
    }

    public AudioFileStream OpenStream(bool AddWAVHeader) {
      if (this.Type_ == AudioFileType.Unknown)
	return null;
      return new AudioFileStream(this.Path_, this.Header_, AddWAVHeader);
    }

    // === Properties === //

    public string        Path          { get { return this.Path_;                } }
    public AudioFileType Type          { get { return this.Type_;                } }
    public Int32         Size          { get { return this.Header_.Size;         } }
    public Int32         ID            { get { return this.Header_.ID;           } }
    public byte          Channels      { get { return this.Header_.Channels;     } }
    public byte          BitsPerSample { get { return 16;                        } }

    public Int32 SampleRate {
      get {
	return (this.Header_.SampleRateHigh + this.Header_.SampleRateLow);
      }
    }

    public bool Looped {
      get {
	return (this.Header_.LoopStart >= 0);
      }
    }

    public SampleFormat SampleFormat {
      get {
	return ((this.Header_.Raw == 0) ? SampleFormat.ADPCM : SampleFormat.PCM);
      }
    }

    public double Length {
      get {
	return (double) this.Header_.SampleBlocks * ((this.Header_.Raw == 0) ? (int) this.Header_.BlockSize : 1) / this.SampleRate;
      }
    }

    public Int32 LoopStart {
      get { return this.Header_.LoopStart * ((this.Header_.Raw == 0) ? (int) this.Header_.BlockSize : 1) / this.SampleRate; }
    }

    // === Private Member Functions === //

    private void DetermineType(BinaryReader BR) {
    char[] first8 = BR.ReadChars(8);
    string marker = new string(first8);
      if (marker == "SeWave\0\0")
	this.Type_ = AudioFileType.SoundEffect;
      else {
      char[] next8 = BR.ReadChars(8);
	marker += new string(next8);
	if (marker == "BGMStream\0\0\0\0\0\0\0")
	  this.Type_ = AudioFileType.BGMStream;
      }
    }

  }

}
