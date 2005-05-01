using System;
using System.IO;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

namespace ManagedDirectX {

  #region DirectSound

  #region Top-Level Helper Class

  public class ManagedDirectSound {

    private ManagedDirectSound() { }

    private static bool     Initialized = false;
    private static Assembly Assembly = null;

    private static void Load() {
      if (!ManagedDirectSound.Initialized) {
	try {
	RegistryKey MDXRoot = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\.NETFramework\AssemblyFolders\ManagedDX");
	  if (MDXRoot != null) {
	  AppDomainSetup DXDomainSetup = new AppDomainSetup();
	    DXDomainSetup.ApplicationBase = MDXRoot.GetValue("") as string;
	  AppDomain DXDomain = AppDomain.CreateDomain("ManagedDirectX", AppDomain.CurrentDomain.Evidence, DXDomainSetup);
	    ManagedDirectSound.Assembly = DXDomain.Load("Microsoft.DirectX.DirectSound");
	    MDXRoot.Close();
	  }
	} catch (Exception E) { Console.WriteLine(E.ToString()); }
	ManagedDirectSound.Initialized = true;
      }
    }

    public static bool Available {
      get {
	ManagedDirectSound.Load();
	return (ManagedDirectSound.Assembly != null);
      }
    }

    public static Type GetType(string Name) {
      if (ManagedDirectSound.Available)
	return ManagedDirectSound.Assembly.GetType(Name, false, false);
      return null;
    }

    public static object CreateObject(string Type) {
      if (ManagedDirectSound.Available)
	return ManagedDirectSound.Assembly.CreateInstance(Type, false);
      return null;
    }

    public static object CreateObject(string Type, params object[] Arguments) {
      if (ManagedDirectSound.Available)
	return ManagedDirectSound.Assembly.CreateInstance(Type, false, BindingFlags.CreateInstance, null, Arguments, Application.CurrentCulture, null);
      return null;
    }

    public static object CreateObject(Type Type) {
      if (ManagedDirectSound.Available)
	return ManagedDirectSound.Assembly.CreateInstance(Type.FullName, false);
      return null;
    }

    public static object CreateObject(Type Type, params object[] Arguments) {
      if (Arguments == null)
	Arguments = new object[] { };
      if (ManagedDirectSound.Available)
	return ManagedDirectSound.Assembly.CreateInstance(Type.FullName, false, BindingFlags.CreateInstance, null, Arguments, Application.CurrentCulture, null);
      return null;
    }

  }

  #endregion

  namespace DirectSound {

    #region Enums

    public class BufferPlayFlags {

      private static Type TMe = ManagedDirectSound.GetType("Microsoft.DirectX.DirectSound.BufferPlayFlags");

      private BufferPlayFlags() { }

      public static object Default {
	get {
	  try { return TMe.GetField("Default").GetValue(null); } catch { }
	  return null;
	}
      }

      public static object Looping {
	get {
	  try { return TMe.GetField("Looping").GetValue(null); } catch { }
	  return null;
	}
      }

    }

    public class CooperativeLevel {

      private static Type TMe = ManagedDirectSound.GetType("Microsoft.DirectX.DirectSound.CooperativeLevel");

      private CooperativeLevel() { }

      public static object Normal {
	get {
	  try { return TMe.GetField("Normal").GetValue(null); } catch { }
	  return null;
	}
      }

    }

    public class WaveFormatTag {

      private static Type TMe = ManagedDirectSound.GetType("Microsoft.DirectX.DirectSound.WaveFormatTag");

      private WaveFormatTag() { }

      public static object Pcm {
	get {
	  try { return TMe.GetField("Pcm").GetValue(null); } catch { }
	  return null;
	}
      }

    }

    #endregion

    #region Static Classes

    public class DSoundHelper {

      private static Type TMe = ManagedDirectSound.GetType("Microsoft.DirectX.DirectSound.DSoundHelper");

      private DSoundHelper() { }

      public static Guid DefaultPlaybackDevice {
	get {
	  try { return (Guid) TMe.GetField("DefaultPlaybackDevice").GetValue(null); } catch { }
	  return Guid.Empty;
	}
      }

    }

    #endregion

    #region Normal Classes

    public class Device {

      private static Type TMe = ManagedDirectSound.GetType("Microsoft.DirectX.DirectSound.Device");
      internal object Me;

      public Device() {
	Me = ManagedDirectSound.CreateObject(TMe);
      }

      public void SetCooperativeLevel(Control Owner, object CooperativeLevel) {
	TMe.InvokeMember("SetCooperativeLevel", BindingFlags.InvokeMethod, null, Me, new object[] { Owner, CooperativeLevel }, Application.CurrentCulture);
      }

    }

    public class BufferDescription {

      private static Type TMe = ManagedDirectSound.GetType("Microsoft.DirectX.DirectSound.BufferDescription");
      internal object Me;

      public BufferDescription(WaveFormat Format) {
	Me = ManagedDirectSound.CreateObject(TMe, Format.Me);
      }

      public int BufferBytes {
	get {
	  try { return (int) TMe.GetProperty("BufferBytes").GetValue(Me, null); } catch { }
	  return 0;
	}
	set {
	  try { TMe.GetProperty("BufferBytes").SetValue(Me, value, null); } catch { }
	}
      }

      public bool GlobalFocus {
	get {
	  try { return (bool) TMe.GetProperty("GlobalFocus").GetValue(Me, null); } catch { }
	  return false;
	}
	set {
	  try { TMe.GetProperty("GlobalFocus").SetValue(Me, value, null); } catch { }
	}
      }

      public bool StickyFocus {
	get {
	  try { return (bool) TMe.GetProperty("StickyFocus").GetValue(Me, null); } catch { }
	  return false;
	}
	set {
	  try { TMe.GetProperty("StickyFocus").SetValue(Me, value, null); } catch { }
	}
      }

    }

    public class WaveFormat {

      private static Type TMe = ManagedDirectSound.GetType("Microsoft.DirectX.DirectSound.WaveFormat");
      internal object Me;

      public WaveFormat() {
	Me = ManagedDirectSound.CreateObject(TMe);
      }

      public object FormatTag {
	get {
	  try { return TMe.GetProperty("FormatTag").GetValue(Me, null); } catch { }
	  return null;
	}
	set {
	  try { TMe.GetProperty("FormatTag").SetValue(Me, value, null); } catch { }
	}
      }

      public short Channels {
	get {
	  try { return (short) TMe.GetProperty("Channels").GetValue(Me, null); } catch { }
	  return 0;
	}
	set {
	  try { TMe.GetProperty("Channels").SetValue(Me, value, null); } catch { }
	}
      }

      public int SamplesPerSecond {
	get {
	  try { return (int) TMe.GetProperty("SamplesPerSecond").GetValue(Me, null); } catch { }
	  return 0;
	}
	set {
	  try { TMe.GetProperty("SamplesPerSecond").SetValue(Me, value, null); } catch { }
	}
      }

      public short BitsPerSample {
	get {
	  try { return (short) TMe.GetProperty("BitsPerSample").GetValue(Me, null); } catch { }
	  return 0;
	}
	set {
	  try { TMe.GetProperty("BitsPerSample").SetValue(Me, value, null); } catch { }
	}
      }

      public short BlockAlign {
	get {
	  try { return (short) TMe.GetProperty("BlockAlign").GetValue(Me, null); } catch { }
	  return 0;
	}
	set {
	  try { TMe.GetProperty("BlockAlign").SetValue(Me, value, null); } catch { }
	}
      }

      public int AverageBytesPerSecond {
	get {
	  try { return (int) TMe.GetProperty("AverageBytesPerSecond").GetValue(Me, null); } catch { }
	  return 0;
	}
	set {
	  try { TMe.GetProperty("AverageBytesPerSecond").SetValue(Me, value, null); } catch { }
	}
      }

    }

    public class BufferStatus {

      private static Type TMe = ManagedDirectSound.GetType("Microsoft.DirectX.DirectSound.BufferStatus");
      internal object Me;

      internal BufferStatus(object BufferStatus) {
	Me = BufferStatus;
      }

      public bool Playing {
	get {
	  try { return (bool) TMe.GetProperty("Playing").GetValue(Me, null); } catch { }
	  return false;
	}
      }

    }

    public class SecondaryBuffer : IDisposable {

      private static Type TMe = ManagedDirectSound.GetType("Microsoft.DirectX.DirectSound.SecondaryBuffer");
      internal object Me;

      public SecondaryBuffer(Stream Stream, Device ParentDevice) {
	Me = ManagedDirectSound.CreateObject(TMe, Stream, ParentDevice.Me);
      }

      public SecondaryBuffer(Stream Stream, BufferDescription BufferDescription, Device ParentDevice) {
	Me = ManagedDirectSound.CreateObject(TMe, Stream, BufferDescription.Me, ParentDevice.Me);
      }

      public void Play(int Priority, object BufferPlayFlags) {
	TMe.InvokeMember("Play", BindingFlags.InvokeMethod, null, Me, new object[] { Priority, BufferPlayFlags }, Application.CurrentCulture);
      }

      public BufferStatus Status {
	get {
	  try { return new BufferStatus(TMe.GetProperty("Status").GetValue(Me, null)); } catch { }
	  return null;
	}
      }

      public void Stop() {
	TMe.InvokeMember("Stop", BindingFlags.InvokeMethod, null, Me, null, Application.CurrentCulture);
      }

      public void Dispose() {
	if (Me != null && Me is IDisposable)
	  (Me as IDisposable).Dispose();
      }

    }

    #endregion

  }

  #endregion

}
