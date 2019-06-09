/*
 * FMOD Ex C# wrapper, remolded from the Firelight code by Kawa.
 */
using System;
using System.Text;
using System.Runtime.InteropServices;
namespace FMOD
{
    public class Version
    {
        public const int Number = 0x00042601;
        public const string Dll = "fmodex";
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Vector
    {
        public float x;
        public float y;
        public float z;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct GUID
    {
        public uint Data1;
        public ushort Data2;
        public ushort Data3;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] Data4;
    }

    public enum Result
    {
        OK,
        AlreadyLocked,
        BadCommand,
        CDDDriverLoadError,
        CDDAInitFailed,
        CDDAInvalidDevice,
        CDDANoAudio,
        CDDANoDevices,
        CDDANoDisc,
        CDDAReadError,
        ChannelAllocationError,
        ChannelStolen,
        COMError,
        DMAFailure,
        DSPConnectionError,
        DSPFormatIncorrect,
        DSPUnitNotFound,
        ERR_DSP_RUNNING,
        ERR_DSP_TOOMANYCONNECTIONS,
        ERR_FILE_BAD,
        ERR_FILE_COULDNOTSEEK,
        ERR_FILE_DISKEJECTED,
        ERR_FILE_EOF,
        ERR_FILE_NOTFOUND,
        ERR_FILE_UNWANTED,
        ERR_FORMAT,
        ERR_HTTP,
        ERR_HTTP_ACCESS,
        ERR_HTTP_PROXY_AUTH,
        ERR_HTTP_SERVER_ERROR,
        ERR_HTTP_TIMEOUT,
        ERR_INITIALIZATION,
        ERR_INITIALIZED,
        ERR_INTERNAL,
        ERR_INVALID_ADDRESS,
        ERR_INVALID_FLOAT,
        ERR_INVALID_HANDLE,
        ERR_INVALID_PARAM,
        ERR_INVALID_SPEAKER,
        ERR_INVALID_SYNCPOINT,
        ERR_INVALID_Vector,
        ERR_IRX,
        ERR_MAXAUDIBLE,
        ERR_MEMORY,
        ERR_MEMORY_CANTPOINT,
        ERR_MEMORY_IOP,
        ERR_MEMORY_SRAM,
        ERR_NEEDS2D,
        ERR_NEEDS3D,
        ERR_NEEDSHARDWARE,
        ERR_NEEDSSOFTWARE,
        ERR_NET_CONNECT,
        ERR_NET_SOCKET_ERROR,
        ERR_NET_URL,
        ERR_NET_WOULD_BLOCK,
        ERR_NOTREADY,
        ERR_OUTPUT_ALLOCATED,
        ERR_OUTPUT_CREATEBUFFER,
        ERR_OUTPUT_DRIVERCALL,
        ERR_OUTPUT_ENUMERATION,
        ERR_OUTPUT_FORMAT,
        ERR_OUTPUT_INIT,
        ERR_OUTPUT_NOHARDWARE,
        ERR_OUTPUT_NOSOFTWARE,
        ERR_PAN,
        ERR_PLUGIN,
        ERR_PLUGIN_INSTANCES,
        ERR_PLUGIN_MISSING,
        ERR_PLUGIN_RESOURCE,
        ERR_RECORD,
        ERR_REVERB_INSTANCE,
        ERR_SUBSOUND_ALLOCATED,
        ERR_SUBSOUND_CANTMOVE,
        ERR_SUBSOUND_MODE,
        ERR_SUBSOUNDS,
        ERR_TAGNOTFOUND,
        ERR_TOOMANYCHANNELS,
        ERR_UNIMPLEMENTED,
        ERR_UNINITIALIZED,
        ERR_UNSUPPORTED,
        ERR_UPDATE,
        ERR_VERSION,
        ERR_EVENT_FAILED,
        ERR_EVENT_INFOONLY,
        ERR_EVENT_INTERNAL,
        ERR_EVENT_MAXSTREAMS,
        ERR_EVENT_MISMATCH,
        ERR_EVENT_NAMECONFLICT,
        ERR_EVENT_NOTFOUND,
        ERR_EVENT_NEEDSSIMPLE,
        ERR_EVENT_GUIDCONFLICT,
        ERR_MUSIC_UNINITIALIZED
    }
    public enum OutputType
    {
        AutoDetect,
        Unknown,
        NoSound,
        WaveWriter,
        NoSoundNonRealtime,
        WaveWriterNonRealtime,
        DirectSound,
        WinMM,
        OpenAL,
        WindowsAudioSession,
        ASIO,
        OpenSoundSystem,
        AdvancedLinuxSoundSystem,
        EnlightmentSoundDaemon,
        SoundManager,
        CoreAudio,
        Xbox,
        PlayStation2,
        PlayStation3,
        GameCube,
        Xbox360,
        PlayStationPortable,
        Wii,
        Maximum
    }

    [Flags]
    public enum Capabilities
    {
        None = 0,
        Hardware = 1,
        HardwareEmulated = 2,
        MultiChannelOutput = 4,
        EightBitPCM = 8,
        SixteenBitPCM = 0x10,
        TwentyFourBitPCM = 0x20,
        ThirtyTwoBitPCM = 0x40,
        FloatingPointPCM = 0x80,
        EAX2Reverb = 0x100,
        EAX3Reverb = 0x200,
        EAX4Reverb = 0x400,
        EAX5Reverb = 0x800,
        I3DL2Reverb = 0x1000,
        LimitedReverb = 0x2000,
    }

    [Flags]
    public enum DebugLevel
    {
        NoLevel = 0,
        LogLevel = 1,
        ErrorLevel = 2,
        WarningLevel = 4,
        HintLevel = 8,
        AllLevels = 0xFF,
        MemoryType = 0x100,
        ThreadType = 0x200,
        FileType = 0x400,
        NetType = 0x800,
        EventType = 0x1000,
        AllTypes = 0xFFFF,
        DisplayTimestamps = 0x100000,
        DisplayLineNumbers = 0x200000,
        DisplayCompressed = 0x400000,
        DisplayThread = 0x800000,
        DisplayAll = 0xF00000,
        All = unchecked((int)0xFFFFFFFF)
    }

    [Flags]
    public enum MemoryType
    {
        Normal = 0,
        XBox360Physical = 0x100000,
        Persistent = 0x200000,
        Secondary = 0x400000,
    }

    public enum SpeakerMode
    {
        Raw,
        Mono,
        Stereo,
        Quad,
        Surround,
        Dolby51,
        Dolby71,
        ProLogic,
        Maximum,
    }

    public enum Speaker
    {
        FrontLeft,
        FrontRight,
        FrontCenter,
        LowFrequency,
        BackLeft,
        BackRight,
        SideLeft,
        SideRight,
        Maximum,
        Mono = FrontLeft,
        Null = Maximum,
        SideBackLeft = SideLeft,
        SideBackRight = SideRight,
    }

    public enum PluginType
    {
        Output,
        FileFormatCodec,
        DigitalSignalProcessor
    }

    [Flags]
    public enum InitalisationFlags
    {
        Normal = 0,
        StreamFromUpdate = 1,
        RightHanded3DSystem = 2,
        SoftwareMixerDisabled = 4,
        Occlusion = 8,
        HRTF = 0x10,
        EnableProfile = 0x20,
        LowMemoryReverb = 0x40,
        VolumeZeroBecomesVirtual = 0x80,
        //Windows only
        WASAPIExclusive = 0x100,
        DirectSoundHRTFNone = 0x200,
        DirectSoundHRTFLight = 0x400,
        DirectSoundHRTFFull = 0x800,
    }

    public enum SoundType
    {
        Unknown,
        AAC,
        AIFF,
        ASF,
        AT3,
        CDDA,
        DLS,
        FLAC,
        FSB,
        GCADPCM,
        IT,
        MIDI,
        MOD,
        MPEG,
        OggVorbis,
        Playlist,
        Raw,
        S3M,
        SF2,
        User,
        WAV,
        XM,
        XMA,
        VAG
    }

    public enum SoundFormat
    {
        None,
        PCM8,
        PCM16,
        PCM24,
        PCM32,
        PCMFloat,
        GCADPCM,
        IMAADPCM,
        VAG,
        XMA,
        MPEG,
        Maximum
    }
 
    [Flags]
    public enum Mode : uint
    {
        Default = 0,
        NoLoop = 1,
        Loop = 2,
        LoopBidi = 4,
        TwoD = 8,
        ThreeD = 0x10,
        Hardware = 0x20,
        Software = 0x40,
        CreateStream = 0x80,
        CreateSample = 0x100,
        CreateCompressedSample = 0x200,
        OpenUser = 0x400,
        OpenMemory = 0x800,
        OpenRaw = 0x1000,
        OpenOnly = 0x2000,
        AccurateTime = 0x4000,
        MPegSearch = 0x8000,
        NonBlocking = 0x10000,
        Unique = 0x20000,
        ThreeDHeadRelative = 0x40000,
        ThreeDWorldRelative = 0x80000,
        ThreeDLogRolloff = 0x100000,
        ThreeDLinRolloff = 0x200000,
        ThreeDCustomRolloff = 0x4000000,
        ThreeDIgnoreGeometry = 0x40000000,
        CDDAForceASPI = 0x400000,
        CDDAJitterCorrect = 0x800000,
        Unicode = 0x1000000,
        IgnoreTags = 0x2000000,
        LowMemory = 0x8000000,
        LoadInSecondaryRAM = 0x20000000,
        VirtualPlayFromStart = 0x80000000,
    }

    public enum OPENSTATE
    {
        READY = 0,
        LOADING,
        ERROR,
        CONNECTING,
        BUFFERING,
        SEEKING,
        STREAMING,
        SETPOSITION,
    }

    public enum SOUNDGROUP_BEHAVIOR
    {
        BEHAVIOR_FAIL,
        BEHAVIOR_MUTE,
        BEHAVIOR_STEALLOWEST
    }

    public enum SYSTEM_CALLBACKTYPE
    {
        DEVICELISTCHANGED,
        MEMORYALLOCATIONFAILED,
        THREADCREATED,
        BADDSPCONNECTION,
        BADDSPLEVEL,
        MAX
    }

    public enum CHANNEL_CALLBACKTYPE
    {
        END,
        VIRTUALVOICE,
        SYNCPOINT,
        OCCLUSION,
        MAX
    }

    public delegate Result SYSTEM_CALLBACK(IntPtr systemraw, SYSTEM_CALLBACKTYPE type, IntPtr commanddata1, IntPtr commanddata2);
    public delegate Result CHANNEL_CALLBACK(IntPtr channelraw, CHANNEL_CALLBACKTYPE type, IntPtr commanddata1, IntPtr commanddata2);
    public delegate Result SOUND_NONBLOCKCALLBACK(IntPtr soundraw, Result Result);
    public delegate Result SOUND_PCMREADCALLBACK(IntPtr soundraw, IntPtr data, uint datalen);
    public delegate Result SOUND_PCMSETPOSCALLBACK(IntPtr soundraw, int subsound, uint position, TIMEUNIT postype);
    public delegate Result FILE_OPENCALLBACK([MarshalAs(UnmanagedType.LPWStr)]string name, int unicode, ref uint filesize, ref IntPtr handle, ref IntPtr userdata);
    public delegate Result FILE_CLOSECALLBACK(IntPtr handle, IntPtr userdata);
    public delegate Result FILE_READCALLBACK(IntPtr handle, IntPtr buffer, uint sizebytes, ref uint bytesread, IntPtr userdata);
    public delegate Result FILE_SEEKCALLBACK(IntPtr handle, int pos, IntPtr userdata);
    public delegate float CB_3D_ROLLOFFCALLBACK(IntPtr channelraw, float distance);

    public enum TAGTYPE
    {
        UNKNOWN = 0,
        ID3V1,
        ID3V2,
        VORBISCOMMENT,
        SHOUTCAST,
        ICECAST,
        ASF,
        MIDI,
        PLAYLIST,
        FMOD,
        USER
    }

    public enum TAGDATATYPE
    {
        BINARY = 0,
        INT,
        FLOAT,
        STRING,
        STRING_UTF16,
        STRING_UTF16BE,
        STRING_UTF8,
        CDTOC
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct TAG
    {
        public TAGTYPE type;
        public TAGDATATYPE datatype;
        public string name;
        private IntPtr data;
        private uint datalen;
        public bool updated;

        //Not allowed to have public-facing IntPtr, so we wrap the required functionality.
        //This also makes reading tags Just Easier.
        public string StringData
        {
            get
            {
                if (datatype == TAGDATATYPE.STRING)
                    return Marshal.PtrToStringAnsi(data);
                return null;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CDTOC
    {
        public int numtracks;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
        public int[] min;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
        public int[] sec;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
        public int[] frame;
    }

    public enum TIMEUNIT
    {
        MS = 0x00000001,
        PCM = 0x00000002,
        PCMBYTES = 0x00000004,
        RAWBYTES = 0x00000008,
        MODORDER = 0x00000100,
        MODROW = 0x00000200,
        MODPATTERN = 0x00000400,
        SENTENCE_MS = 0x00010000,
        SENTENCE_PCM = 0x00020000,
        SENTENCE_PCMBYTES = 0x00040000,
        SENTENCE = 0x00080000,
        SENTENCE_SUBSOUND = 0x00100000,
        BUFFERED = 0x10000000,
    }

    public enum SPEAKERMAPTYPE
    {
        DEFAULT,
        ALLMONO,
        ALLSTEREO,
        _51_PROTOOLS
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CREATESOUNDEXINFO
    {
        public int cbsize;
        public uint length;
        public uint fileoffset;
        public int numchannels;
        public int defaultfrequency;
        public SoundFormat format;
        public uint decodebuffersize;
        public int initialsubsound;
        public int numsubsounds;
        private IntPtr inclusionlist;
        public int inclusionlistnum;
        public SOUND_PCMREADCALLBACK pcmreadcallback;
        public SOUND_PCMSETPOSCALLBACK pcmsetposcallback;      
        public SOUND_NONBLOCKCALLBACK nonblockcallback;       
        public string dlsname;
        public string encryptionkey;
        public int maxpolyphony;
        private IntPtr userdata;
        public SoundType suggestedsoundtype;
        public FILE_OPENCALLBACK useropen;
        public FILE_CLOSECALLBACK userclose;
        public FILE_READCALLBACK userread;
        public FILE_SEEKCALLBACK userseek;
        public SPEAKERMAPTYPE speakermap;
        private IntPtr initialsoundgroup;
        public uint initialseekposition;
        public TIMEUNIT initialseekpostype;
        public int ignoresetfilesystem;
    }

    public enum CHANNELINDEX
    {
        FREE = -1,
        REUSE = -2
    }

    public static class Error
    {
        public static string String(Result errcode)
        {
            switch (errcode)
            {
                case Result.OK: return "No errors.";
                case Result.AlreadyLocked: return "Tried to call lock a second time before unlock was called. ";
                case Result.BadCommand: return "Tried to call a function on a data type that does not allow this type of functionality (ie calling Sound::lock on a streaming sound). ";
                case Result.CDDDriverLoadError: return "Neither NTSCSI nor ASPI could be initialised. ";
                case Result.CDDAInitFailed: return "An error occurred while initialising the CDDA subsystem. ";
                case Result.CDDAInvalidDevice: return "Couldn't find the specified device. ";
                case Result.CDDANoAudio: return "No audio tracks on the specified disc. ";
                case Result.CDDANoDevices: return "No CD/DVD devices were found. ";
                case Result.CDDANoDisc: return "No disc present in the specified drive. ";
                case Result.CDDAReadError: return "A CDDA read error occurred. ";
                case Result.ChannelAllocationError: return "Error trying to allocate a channel. ";
                case Result.ChannelStolen: return "The specified channel has been reused to play another sound. ";
                case Result.COMError: return "A Win32 COM related error occured. COM failed to initialize or a QueryInterface failed meaning a Windows codec or driver was not installed properly. ";
                case Result.DMAFailure: return "DMA Failure.  See debug output for more information. ";
                case Result.DSPConnectionError: return "DSP connection error.  Connection possibly caused a cyclic dependancy. ";
                case Result.DSPFormatIncorrect: return "DSP Format error.  A DSP unit may have attempted to connect to this network with the wrong format. ";
                case Result.DSPUnitNotFound: return "DSP connection error.  Couldn't find the DSP unit specified. ";
                case Result.ERR_DSP_RUNNING: return "DSP error.  Cannot perform this operation while the network is in the middle of running.  This will most likely happen if a connection or disconnection is attempted in a DSP callback. ";
                case Result.ERR_DSP_TOOMANYCONNECTIONS: return "DSP connection error.  The unit being connected to or disconnected should only have 1 input or output. ";
                case Result.ERR_FILE_BAD: return "Error loading file. ";
                case Result.ERR_FILE_COULDNOTSEEK: return "Couldn't perform seek operation.  This is a limitation of the medium (ie netstreams) or the file format. ";
                case Result.ERR_FILE_DISKEJECTED: return "Media was ejected while reading. ";
                case Result.ERR_FILE_EOF: return "End of file unexpectedly reached while trying to read essential data (truncated data?). ";
                case Result.ERR_FILE_NOTFOUND: return "File not found. ";
                case Result.ERR_FILE_UNWANTED: return "Unwanted file access occured. ";
                case Result.ERR_FORMAT: return "Unsupported file or audio format. ";
                case Result.ERR_HTTP: return "A HTTP error occurred. This is a catch-all for HTTP errors not listed elsewhere. ";
                case Result.ERR_HTTP_ACCESS: return "The specified resource requires authentication or is forbidden. ";
                case Result.ERR_HTTP_PROXY_AUTH: return "Proxy authentication is required to access the specified resource. ";
                case Result.ERR_HTTP_SERVER_ERROR: return "A HTTP server error occurred. ";
                case Result.ERR_HTTP_TIMEOUT: return "The HTTP request timed out. ";
                case Result.ERR_INITIALIZATION: return "FMOD was not initialized correctly to support this function. ";
                case Result.ERR_INITIALIZED: return "Cannot call this command after System::init. ";
                case Result.ERR_INTERNAL: return "An error occured that wasn't supposed to.  Contact support. ";
                case Result.ERR_INVALID_ADDRESS: return "On Xbox 360, this memory address passed to FMOD must be physical, (ie allocated with XPhysicalAlloc.) ";
                case Result.ERR_INVALID_FLOAT: return "Value passed in was a NaN, Inf or denormalized float. ";
                case Result.ERR_INVALID_HANDLE: return "An invalid object handle was used. ";
                case Result.ERR_INVALID_PARAM: return "An invalid parameter was passed to this function. ";
                case Result.ERR_INVALID_SPEAKER: return "An invalid speaker was passed to this function based on the current speaker mode. ";
                case Result.ERR_INVALID_SYNCPOINT: return "The syncpoint did not come from this sound handle.";
                case Result.ERR_INVALID_Vector: return "The Vectors passed in are not unit length, or perpendicular. ";
                case Result.ERR_IRX: return "PS2 only.  fmodex.irx failed to initialize.  This is most likely because you forgot to load it. ";
                case Result.ERR_MAXAUDIBLE: return "Reached maximum audible playback count for this sound's soundgroup. ";
                case Result.ERR_MEMORY: return "Not enough memory or resources. ";
                case Result.ERR_MEMORY_CANTPOINT: return "Can't use FMOD_OPENMEMORY_POINT on non PCM source data, or non mp3/xma/adpcm data if FMOD_CREATECOMPRESSEDSAMPLE was used. ";
                case Result.ERR_MEMORY_IOP: return "PS2 only.  Not enough memory or resources on PlayStation 2 IOP ram. ";
                case Result.ERR_MEMORY_SRAM: return "Not enough memory or resources on console sound ram. ";
                case Result.ERR_NEEDS2D: return "Tried to call a command on a 3d sound when the command was meant for 2d sound. ";
                case Result.ERR_NEEDS3D: return "Tried to call a command on a 2d sound when the command was meant for 3d sound. ";
                case Result.ERR_NEEDSHARDWARE: return "Tried to use a feature that requires hardware support.  (ie trying to play a VAG compressed sound in software on PS2). ";
                case Result.ERR_NEEDSSOFTWARE: return "Tried to use a feature that requires the software engine.  Software engine has either been turned off, or command was executed on a hardware channel which does not support this feature. ";
                case Result.ERR_NET_CONNECT: return "Couldn't connect to the specified host. ";
                case Result.ERR_NET_SOCKET_ERROR: return "A socket error occurred.  This is a catch-all for socket-related errors not listed elsewhere. ";
                case Result.ERR_NET_URL: return "The specified URL couldn't be resolved. ";
                case Result.ERR_NET_WOULD_BLOCK: return "Operation on a non-blocking socket could not complete immediately. ";
                case Result.ERR_NOTREADY: return "Operation could not be performed because specified sound is not ready. ";
                case Result.ERR_OUTPUT_ALLOCATED: return "Error initializing output device, but more specifically, the output device is already in use and cannot be reused. ";
                case Result.ERR_OUTPUT_CREATEBUFFER: return "Error creating hardware sound buffer. ";
                case Result.ERR_OUTPUT_DRIVERCALL: return "A call to a standard soundcard driver failed, which could possibly mean a bug in the driver or resources were missing or exhausted. ";
                case Result.ERR_OUTPUT_ENUMERATION: return "Error enumerating the available driver list. List may be inconsistent due to a recent device addition or removal.";
                case Result.ERR_OUTPUT_FORMAT: return "Soundcard does not support the minimum features needed for this soundsystem (16bit stereo output). ";
                case Result.ERR_OUTPUT_INIT: return "Error initializing output device. ";
                case Result.ERR_OUTPUT_NOHARDWARE: return "FMOD_HARDWARE was specified but the sound card does not have the resources nescessary to play it. ";
                case Result.ERR_OUTPUT_NOSOFTWARE: return "Attempted to create a software sound but no software channels were specified in System::init. ";
                case Result.ERR_PAN: return "Panning only works with mono or stereo sound sources. ";
                case Result.ERR_PLUGIN: return "An unspecified error has been returned from a 3rd party plugin. ";
                case Result.ERR_PLUGIN_INSTANCES: return "The number of allowed instances of a plugin has been exceeded ";
                case Result.ERR_PLUGIN_MISSING: return "A requested output, dsp unit type or codec was not available. ";
                case Result.ERR_PLUGIN_RESOURCE: return "A resource that the plugin requires cannot be found. (ie the DLS file for MIDI playback) ";
                case Result.ERR_RECORD: return "An error occured trying to initialize the recording device. ";
                case Result.ERR_REVERB_INSTANCE: return "Specified Instance in FMOD_REVERB_PROPERTIES couldn't be set. Most likely because another application has locked the EAX4 FX slot. ";
                case Result.ERR_SUBSOUND_ALLOCATED: return "This subsound is already being used by another sound, you cannot have more than one parent to a sound.  Null out the other parent's entry first. ";
                case Result.ERR_SUBSOUND_CANTMOVE: return "Shared subsounds cannot be replaced or moved from their parent stream, such as when the parent stream is an FSB file.";
                case Result.ERR_SUBSOUND_MODE: return "The subsound's mode bits do not match with the parent sound's mode bits.  See documentation for function that it was called with.";
                case Result.ERR_SUBSOUNDS: return "The error occured because the sound referenced contains subsounds.  (ie you cannot play the parent sound as a static sample, only its subsounds.) ";
                case Result.ERR_TAGNOTFOUND: return "The specified tag could not be found or there are no tags. ";
                case Result.ERR_TOOMANYCHANNELS: return "The sound created exceeds the allowable input channel count.  This can be increased using the maxinputchannels parameter in System::setSoftwareFormat. ";
                case Result.ERR_UNIMPLEMENTED: return "Something in FMOD hasn't been implemented when it should be! contact support! ";
                case Result.ERR_UNINITIALIZED: return "This command failed because System::init or System::setDriver was not called. ";
                case Result.ERR_UNSUPPORTED: return "A command issued was not supported by this object.  Possibly a plugin without certain callbacks specified. ";
                case Result.ERR_UPDATE: return "An error caused by System::update occured. ";
                case Result.ERR_VERSION: return "The version number of this file format is not supported. ";
                case Result.ERR_EVENT_FAILED: return "An Event failed to be retrieved, most likely due to 'just fail' being specified as the max playbacks behavior. ";
                case Result.ERR_EVENT_INFOONLY: return "Can't execute this command on an EVENT_INFOONLY event. ";
                case Result.ERR_EVENT_INTERNAL: return "An error occured that wasn't supposed to.  See debug log for reason. ";
                case Result.ERR_EVENT_MAXSTREAMS: return "Event failed because 'Max streams' was hit when FMOD_INIT_FAIL_ON_MAXSTREAMS was specified. ";
                case Result.ERR_EVENT_MISMATCH: return "FSB mis-matches the FEV it was compiled with. ";
                case Result.ERR_EVENT_NAMECONFLICT: return "A category with the same name already exists. ";
                case Result.ERR_EVENT_NOTFOUND: return "The requested event, event group, event category or event property could not be found. ";
                default: return "Unknown error.";
            }
        }
    }

    public class Factory
    {
        public static System CreateSystem()
        {
            Result Result = Result.OK;
            IntPtr systemraw = new IntPtr();
            System systemnew = null;
            Result = NativeMethods.FMOD_System_Create(ref systemraw);
            if (Result != Result.OK)
            {
                throw new Exception(Error.String(Result));
            }

            systemnew = new System();
            systemnew.setRaw(systemraw);
           
            return systemnew;
        }

        private class NativeMethods
        {
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_Create(ref IntPtr system);
        }
    }

    public class Memory
    {
        public static Result GetStats(ref int currentalloced, ref int maxalloced)
        {
            return NativeMethods.FMOD_Memory_GetStats(ref currentalloced, ref maxalloced);
        }

        private class NativeMethods
        {
    [DllImport(Version.Dll)]
        public static extern Result FMOD_Memory_GetStats(ref int currentalloced, ref int maxalloced);
        }
    }

    public class System
    {
        public Result Release()
        {
            return NativeMethods.FMOD_System_Release(systemraw);
        }
        
        public Result SetOutput(OutputType output)
        {
            return NativeMethods.FMOD_System_SetOutput(systemraw, output);
        }

        public Result GetOutput(ref OutputType output)
        {
            return NativeMethods.FMOD_System_GetOutput(systemraw, ref output);
        }

        public Result GetNumDrivers(ref int numdrivers)
        {
            return NativeMethods.FMOD_System_GetNumDrivers(systemraw, ref numdrivers);
        }

        public Result GetDriverInfo(int id, StringBuilder name, int namelen, ref GUID guid)
        {
            return NativeMethods.FMOD_System_GetDriverInfo(systemraw, id, name, namelen, ref guid);
        }

        public Result GetDriverCaps(int id, ref Capabilities caps, ref int minfrequency, ref int maxfrequency, ref SpeakerMode controlpanelspeakermode)
        {
            return NativeMethods.FMOD_System_GetDriverCaps(systemraw, id, ref caps, ref minfrequency, ref maxfrequency, ref controlpanelspeakermode);
        }

        public Result SetDriver(int driver)
        {
            return NativeMethods.FMOD_System_SetDriver(systemraw, driver);
        }

        public Result GetDriver(ref int driver)
        {
            return NativeMethods.FMOD_System_GetDriver(systemraw, ref driver);
        }

        public Result SetHardwareChannels(int min2d, int max2d, int min3d, int max3d)
        {
            return NativeMethods.FMOD_System_SetHardwareChannels(systemraw, min2d, max2d, min3d, max3d);
        }

        public Result SetSoftwareChannels(int numsoftwarechannels)
        {
            return NativeMethods.FMOD_System_SetSoftwareChannels(systemraw, numsoftwarechannels);
        }

        public Result GetSoftwareChannels(ref int numsoftwarechannels)
        {
            return NativeMethods.FMOD_System_GetSoftwareChannels(systemraw, ref numsoftwarechannels);
        }

        public Result SetFileSystem(FILE_OPENCALLBACK useropen, FILE_CLOSECALLBACK userclose, FILE_READCALLBACK userread, FILE_SEEKCALLBACK userseek, int buffersize)
        {
            return NativeMethods.FMOD_System_SetFileSystem(systemraw, useropen, userclose, userread, userseek, buffersize);
        }

        public Result attachFileSystem(FILE_OPENCALLBACK useropen, FILE_CLOSECALLBACK userclose, FILE_READCALLBACK userread, FILE_SEEKCALLBACK userseek)
        {
            return NativeMethods.FMOD_System_AttachFileSystem(systemraw, useropen, userclose, userread, userseek);
        }

        public Result SetSpeakerMode(SpeakerMode speakermode)
        {
            return NativeMethods.FMOD_System_SetSpeakerMode(systemraw, speakermode);
        }

        public Result GetSpeakerMode(ref SpeakerMode speakermode)
        {
            return NativeMethods.FMOD_System_GetSpeakerMode(systemraw, ref speakermode);
        }

        public Result CreateCodec(IntPtr codecdescription, uint priority)
        {
            return NativeMethods.FMOD_System_CreateCodec(systemraw, codecdescription, priority);
        }
       
        public Result Initialize(int maxchannels, InitalisationFlags flags, IntPtr extradata)
        {
            return NativeMethods.FMOD_System_Init(systemraw, maxchannels, flags, extradata);
        }

        public Result Close()
        {
            return NativeMethods.FMOD_System_Close(systemraw);
        }
       
        public Result Update()
        {
            return NativeMethods.FMOD_System_Update(systemraw);
        }

        public Result SetStreamBufferSize(uint filebuffersize, TIMEUNIT filebuffersizetype)
        {
            return NativeMethods.FMOD_System_SetStreamBufferSize(systemraw, filebuffersize, filebuffersizetype);
        }

        public Result GetStreamBufferSize(ref uint filebuffersize, ref TIMEUNIT filebuffersizetype)
        {
            return NativeMethods.FMOD_System_GetStreamBufferSize(systemraw, ref filebuffersize, ref filebuffersizetype);
        }
       
        public Result GetVersion(ref uint version)
        {
            return NativeMethods.FMOD_System_GetVersion(systemraw, ref version);
        }

        public Result GetOutputHandle(ref IntPtr handle)
        {
            return NativeMethods.FMOD_System_GetOutputHandle(systemraw, ref handle);
        }

        public Result GetChannelsPlaying(ref int channels)
        {
            return NativeMethods.FMOD_System_GetChannelsPlaying(systemraw, ref channels);
        }

        public Result GetHardwareChannels(ref int num2d, ref int num3d, ref int total)
        {
            return NativeMethods.FMOD_System_GetHardwareChannels(systemraw, ref num2d, ref num3d, ref total);
        }

        public Result GetSoundRam(ref int currentalloced, ref int maxalloced, ref int total)
        {
            return NativeMethods.FMOD_System_GetSoundRAM(systemraw, ref currentalloced, ref maxalloced, ref total);
        }

        public Result GetNumCDROMDrives(ref int numdrives)
        {
            return NativeMethods.FMOD_System_GetNumCDROMDrives(systemraw, ref numdrives);
        }

        public Result GetCDROMDriveName(int drive, StringBuilder drivename, int drivenamelen, StringBuilder scsiname, int scsinamelen, StringBuilder devicename, int devicenamelen)
        {
            return NativeMethods.FMOD_System_GetCDROMDriveName(systemraw, drive, drivename, drivenamelen, scsiname, scsinamelen, devicename, devicenamelen);
        }

        public Result GetWaveData(float[] wavearray, int numvalues, int channeloffset)
        {
            return NativeMethods.FMOD_System_GetWaveData(systemraw, wavearray, numvalues, channeloffset);
        }
       
        public Result CreateSound(string name_or_data, Mode mode, ref CREATESOUNDEXINFO exinfo, ref Sound sound)
        {
            Result Result = Result.OK;
            IntPtr soundraw = new IntPtr();
            Sound soundnew = null;
            mode = mode | Mode.Unicode;
            try
            {
                Result = NativeMethods.FMOD_System_CreateSound(systemraw, name_or_data, mode, ref exinfo, ref soundraw);
            }

            catch
            {
                Result = Result.ERR_INVALID_PARAM;
            }

            if (Result != Result.OK)
            {
                return Result;
            }

            if (sound == null)
            {
                soundnew = new Sound();
                soundnew.setRaw(soundraw);
                sound = soundnew;
            }

            else
            {
                sound.setRaw(soundraw);
            }

            return Result;
        }

        public Result CreateSound(byte[] data, Mode mode, ref CREATESOUNDEXINFO exinfo, ref Sound sound)
        {
            Result Result = Result.OK;
            IntPtr soundraw = new IntPtr();
            Sound soundnew = null;
            try
            {
                Result = NativeMethods.FMOD_System_CreateSound(systemraw, data, mode, ref exinfo, ref soundraw);
            }

            catch
            {
                Result = Result.ERR_INVALID_PARAM;
            }

            if (Result != Result.OK)
            {
                return Result;
            }

            if (sound == null)
            {
                soundnew = new Sound();
                soundnew.setRaw(soundraw);
                sound = soundnew;
            }

            else
            {
                sound.setRaw(soundraw);
            }

            return Result;
        }
        public Result CreateSound(string name_or_data, Mode mode, ref Sound sound)
        {
            Result Result = Result.OK;
            IntPtr soundraw = new IntPtr();
            Sound soundnew = null;
            mode = mode | Mode.Unicode;
            try
            {
                Result = NativeMethods.FMOD_System_CreateSound(systemraw, name_or_data, mode, 0, ref soundraw);
            }

            catch
            {
                Result = Result.ERR_INVALID_PARAM;
            }

            if (Result != Result.OK)
            {
                return Result;
            }

            if (sound == null)
            {
                soundnew = new Sound();
                soundnew.setRaw(soundraw);
                sound = soundnew;
            }

            else
            {
                sound.setRaw(soundraw);
            }

            return Result;
        }
        public Result CreateStream(string name_or_data, Mode mode, ref CREATESOUNDEXINFO exinfo, ref Sound sound)
        {
            Result Result = Result.OK;
            IntPtr soundraw = new IntPtr();
            Sound soundnew = null;
            mode = mode | Mode.Unicode;
            try
            {
                Result = NativeMethods.FMOD_System_CreateStream(systemraw, name_or_data, mode, ref exinfo, ref soundraw);
            }

            catch
            {
                Result = Result.ERR_INVALID_PARAM;
            }

            if (Result != Result.OK)
            {
                return Result;
            }

            if (sound == null)
            {
                soundnew = new Sound();
                soundnew.setRaw(soundraw);
                sound = soundnew;
            }

            else
            {
                sound.setRaw(soundraw);
            }

            return Result;
        }
        public Result CreateStream(byte[] data, Mode mode, ref CREATESOUNDEXINFO exinfo, ref Sound sound)
        {
            Result Result = Result.OK;
            IntPtr soundraw = new IntPtr();
            Sound soundnew = null;
            try
            {
                Result = NativeMethods.FMOD_System_CreateStream(systemraw, data, mode, ref exinfo, ref soundraw);
            }

            catch
            {
                Result = Result.ERR_INVALID_PARAM;
            }

            if (Result != Result.OK)
            {
                return Result;
            }

            if (sound == null)
            {
                soundnew = new Sound();
                soundnew.setRaw(soundraw);
                sound = soundnew;
            }

            else
            {
                sound.setRaw(soundraw);
            }

            return Result;
        }
        public Result CreateStream(string name_or_data, Mode mode, ref Sound sound)
        {
            Result Result = Result.OK;
            IntPtr soundraw = new IntPtr();
            Sound soundnew = null;
            mode = mode | Mode.Unicode;
            try
            {
                Result = NativeMethods.FMOD_System_CreateStream(systemraw, name_or_data, mode, 0, ref soundraw);
            }

            catch
            {
                Result = Result.ERR_INVALID_PARAM;
            }

            if (Result != Result.OK)
            {
                return Result;
            }

            if (sound == null)
            {
                soundnew = new Sound();
                soundnew.setRaw(soundraw);
                sound = soundnew;
            }

            else
            {
                sound.setRaw(soundraw);
            }

            return Result;
        }

        public Result CreateChannelGroup(string name, ref ChannelGroup channelgroup)
        {
            Result Result = Result.OK;
            IntPtr channelgroupraw = new IntPtr();
            ChannelGroup channelgroupnew = null;
            try
            {
                Result = NativeMethods.FMOD_System_CreateChannelGroup(systemraw, name, ref channelgroupraw);
            }

            catch
            {
                Result = Result.ERR_INVALID_PARAM;
            }

            if (Result != Result.OK)
            {
                return Result;
            }

            if (channelgroup == null)
            {
                channelgroupnew = new ChannelGroup();
                channelgroupnew.setRaw(channelgroupraw);
                channelgroup = channelgroupnew;
            }

            else
            {
                channelgroup.setRaw(channelgroupraw);
            }

            return Result;
        }

        public Result PlaySound(CHANNELINDEX channelid, Sound sound, bool paused, ref Channel channel)
        {
            Result Result = Result.OK;
            IntPtr channelraw;
            Channel channelnew = null;
            if (channel != null)
            {
                channelraw = channel.getRaw();
            }

            else
            {
                channelraw = new IntPtr();
            }

            try
            {
                Result = NativeMethods.FMOD_System_PlaySound(systemraw, channelid, sound.getRaw(), (paused ? 1 : 0), ref channelraw);
            }

            catch
            {
                Result = Result.ERR_INVALID_PARAM;
            }

            if (Result != Result.OK)
            {
                return Result;
            }

            if (channel == null)
            {
                channelnew = new Channel();
                channelnew.setRaw(channelraw);
                channel = channelnew;
            }

            else
            {
                channel.setRaw(channelraw);
            }

            return Result;
        }

        public Result GetChannel(int channelid, ref Channel channel)
        {
            Result Result = Result.OK;
            IntPtr channelraw = new IntPtr();
            Channel channelnew = null;
            try
            {
                Result = NativeMethods.FMOD_System_GetChannel(systemraw, channelid, ref channelraw);
            }

            catch
            {
                Result = Result.ERR_INVALID_PARAM;
            }

            if (Result != Result.OK)
            {
                return Result;
            }

            if (channel == null)
            {
                channelnew = new Channel();
                channelnew.setRaw(channelraw);
                channel = channelnew;
            }

            else
            {
                channel.setRaw(channelraw);
            }

            return Result;
        }

        public Result GetMasterChannelGroup(ref ChannelGroup channelgroup)
        {
            Result Result = Result.OK;
            IntPtr channelgroupraw = new IntPtr();
            ChannelGroup channelgroupnew = null;
            try
            {
                Result = NativeMethods.FMOD_System_GetMasterChannelGroup(systemraw, ref channelgroupraw);
            }

            catch
            {
                Result = Result.ERR_INVALID_PARAM;
            }

            if (Result != Result.OK)
            {
                return Result;
            }

            if (channelgroup == null)
            {
                channelgroupnew = new ChannelGroup();
                channelgroupnew.setRaw(channelgroupraw);
                channelgroup = channelgroupnew;
            }

            else
            {
                channelgroup.setRaw(channelgroupraw);
            }

            return Result;
        }

        public Result GetMasterSoundGroup(ref SoundGroup soundgroup)
        {
            Result Result = Result.OK;
            IntPtr soundgroupraw = new IntPtr();
            SoundGroup soundgroupnew = null;
            try
            {
                Result = NativeMethods.FMOD_System_GetMasterSoundGroup(systemraw, ref soundgroupraw);
            }

            catch
            {
                Result = Result.ERR_INVALID_PARAM;
            }

            if (Result != Result.OK)
            {
                return Result;
            }

            if (soundgroup == null)
            {
                soundgroupnew = new SoundGroup();
                soundgroupnew.setRaw(soundgroupraw);
                soundgroup = soundgroupnew;
            }

            else
            {
                soundgroup.setRaw(soundgroupraw);
            }

            return Result;
        }
 
        public Result SetUserData(IntPtr userdata)
        {
            return NativeMethods.FMOD_System_SetUserData(systemraw, userdata);
        }

        public Result GetUserData(ref IntPtr userdata)
        {
            return NativeMethods.FMOD_System_GetUserData(systemraw, ref userdata);
        }

        public Result GetMemoryInfo(uint memorybits, uint event_memorybits, ref uint memoryused, IntPtr memoryused_array)
        {
            return NativeMethods.FMOD_System_GetMemoryInfo(systemraw, memorybits, event_memorybits, ref memoryused, memoryused_array);
        }
        #region wrapperinternal
        private IntPtr systemraw;
        public void setRaw(IntPtr system)
        {
            systemraw = new IntPtr();
            systemraw = system;
        }

        public IntPtr getRaw()
        {
            return systemraw;
        }

        #endregion

        private class NativeMethods
        {
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_Release(IntPtr system);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_SetOutput(IntPtr system, OutputType output);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_GetOutput(IntPtr system, ref OutputType output);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_GetNumDrivers(IntPtr system, ref int numdrivers);
            [DllImport(Version.Dll, CharSet = CharSet.Unicode)]
            public static extern Result FMOD_System_GetDriverInfo(IntPtr system, int id, StringBuilder name, int namelen, ref GUID guid);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_GetDriverCaps(IntPtr system, int id, ref Capabilities caps, ref int minfrequency, ref int maxfrequency, ref SpeakerMode controlpanelspeakermode);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_SetDriver(IntPtr system, int driver);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_GetDriver(IntPtr system, ref int driver);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_SetHardwareChannels(IntPtr system, int min2d, int max2d, int min3d, int max3d);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_GetHardwareChannels(IntPtr system, ref int num2d, ref int num3d, ref int total);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_SetSoftwareChannels(IntPtr system, int numsoftwarechannels);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_GetSoftwareChannels(IntPtr system, ref int numsoftwarechannels);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_SetFileSystem(IntPtr system, FILE_OPENCALLBACK useropen, FILE_CLOSECALLBACK userclose, FILE_READCALLBACK userread, FILE_SEEKCALLBACK userseek, int buffersize);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_AttachFileSystem(IntPtr system, FILE_OPENCALLBACK useropen, FILE_CLOSECALLBACK userclose, FILE_READCALLBACK userread, FILE_SEEKCALLBACK userseek);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_CreateCodec(IntPtr system, IntPtr codecdescription, uint priority);

            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_Init(IntPtr system, int maxchannels, InitalisationFlags flags, IntPtr extradata);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_Close(IntPtr system);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_Update(IntPtr system);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_UpdateFinished(IntPtr system);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_SetSpeakerMode(IntPtr system, SpeakerMode speakermode);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_GetSpeakerMode(IntPtr system, ref SpeakerMode speakermode);

            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_SetFileBufferSize(IntPtr system, int sizebytes);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_GetFileBufferSize(IntPtr system, ref int sizebytes);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_SetStreamBufferSize(IntPtr system, uint filebuffersize, TIMEUNIT filebuffersizetype);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_GetStreamBufferSize(IntPtr system, ref uint filebuffersize, ref TIMEUNIT filebuffersizetype);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_GetVersion(IntPtr system, ref uint version);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_GetOutputHandle(IntPtr system, ref IntPtr handle);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_GetChannelsPlaying(IntPtr system, ref int channels);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_GetSoundRAM(IntPtr system, ref int currentalloced, ref int maxalloced, ref int total);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_GetNumCDROMDrives(IntPtr system, ref int numdrives);
            [DllImport(Version.Dll, CharSet = CharSet.Unicode)]
            public static extern Result FMOD_System_GetCDROMDriveName(IntPtr system, int drive, StringBuilder drivename, int drivenamelen, StringBuilder scsiname, int scsinamelen, StringBuilder devicename, int devicenamelen);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_GetWaveData(IntPtr system, [MarshalAs(UnmanagedType.LPArray)]float[] wavearray, int numvalues, int channeloffset);
            [DllImport(Version.Dll, CharSet = CharSet.Unicode)]
            public static extern Result FMOD_System_CreateSound(IntPtr system, string name_or_data, Mode mode, ref CREATESOUNDEXINFO exinfo, ref IntPtr sound);
            [DllImport(Version.Dll, CharSet = CharSet.Unicode)]
            public static extern Result FMOD_System_CreateStream(IntPtr system, string name_or_data, Mode mode, ref CREATESOUNDEXINFO exinfo, ref IntPtr sound);
            [DllImport(Version.Dll, CharSet = CharSet.Unicode)]
            public static extern Result FMOD_System_CreateSound(IntPtr system, string name_or_data, Mode mode, int exinfo, ref IntPtr sound);
            [DllImport(Version.Dll, CharSet = CharSet.Unicode)]
            public static extern Result FMOD_System_CreateStream(IntPtr system, string name_or_data, Mode mode, int exinfo, ref IntPtr sound);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_CreateSound(IntPtr system, byte[] name_or_data, Mode mode, ref CREATESOUNDEXINFO exinfo, ref IntPtr sound);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_CreateStream(IntPtr system, byte[] name_or_data, Mode mode, ref CREATESOUNDEXINFO exinfo, ref IntPtr sound);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_CreateSound(IntPtr system, byte[] name_or_data, Mode mode, int exinfo, ref IntPtr sound);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_CreateStream(IntPtr system, byte[] name_or_data, Mode mode, int exinfo, ref IntPtr sound);

            [DllImport(Version.Dll, CharSet = CharSet.Unicode)]
            public static extern Result FMOD_System_CreateChannelGroup(IntPtr system, string name, ref IntPtr channelgroup);
            [DllImport(Version.Dll, CharSet = CharSet.Unicode)]
            public static extern Result FMOD_System_CreateSoundGroup(IntPtr system, StringBuilder name, ref SoundGroup soundgroup);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_PlaySound(IntPtr system, CHANNELINDEX channelid, IntPtr sound, int paused, ref IntPtr channel);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_GetChannel(IntPtr system, int channelid, ref IntPtr channel);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_GetMasterChannelGroup(IntPtr system, ref IntPtr channelgroup);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_GetMasterSoundGroup(IntPtr system, ref IntPtr soundgroup);

            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_GetRecordNumDrivers(IntPtr system, ref int numdrivers);
            [DllImport(Version.Dll, CharSet = CharSet.Unicode)]
            public static extern Result FMOD_System_GetRecordDriverInfo(IntPtr system, int id, StringBuilder name, int namelen, ref GUID guid);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_GetRecordPosition(IntPtr system, int id, ref uint position);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_RecordStart(IntPtr system, int id, IntPtr sound, bool loop);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_RecordStop(IntPtr system, int id);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_IsRecording(IntPtr system, int id, ref bool recording);

            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_SetUserData(IntPtr system, IntPtr userdata);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_GetUserData(IntPtr system, ref IntPtr userdata);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_System_GetMemoryInfo(IntPtr system, uint memorybits, uint event_memorybits, ref uint memoryused, IntPtr memoryused_array);
        }
    }

    public class Sound
    {
        public Result release()
        {
            return NativeMethods.FMOD_Sound_Release(soundraw);
        }

        public Result GetSystemObject(ref System system)
        {
            Result Result = Result.OK;
            IntPtr systemraw = new IntPtr();
            System systemnew = null;
            try
            {
                Result = NativeMethods.FMOD_Sound_GetSystemObject(soundraw, ref systemraw);
            }

            catch
            {
                Result = Result.ERR_INVALID_PARAM;
            }

            if (Result != Result.OK)
            {
                return Result;
            }

            if (system == null)
            {
                systemnew = new System();
                systemnew.setRaw(systemraw);
                system = systemnew;
            }

            else
            {
                system.setRaw(systemraw);
            }

            return Result;
        }

        public Result Lock(uint offset, uint length, ref IntPtr ptr1, ref IntPtr ptr2, ref uint len1, ref uint len2)
        {
            return NativeMethods.FMOD_Sound_Lock(soundraw, offset, length, ref ptr1, ref ptr2, ref len1, ref len2);
        }

        public Result Unlock(IntPtr ptr1, IntPtr ptr2, uint len1, uint len2)
        {
            return NativeMethods.FMOD_Sound_Unlock(soundraw, ptr1, ptr2, len1, len2);
        }

        public Result SetDefaults(float frequency, float volume, float pan, int priority)
        {
            return NativeMethods.FMOD_Sound_SetDefaults(soundraw, frequency, volume, pan, priority);
        }

        public Result GetDefaults(ref float frequency, ref float volume, ref float pan, ref int priority)
        {
            return NativeMethods.FMOD_Sound_GetDefaults(soundraw, ref frequency, ref volume, ref pan, ref priority);
        }

        public Result SetVariations(float frequencyvar, float volumevar, float panvar)
        {
            return NativeMethods.FMOD_Sound_SetVariations(soundraw, frequencyvar, volumevar, panvar);
        }

        public Result GetVariations(ref float frequencyvar, ref float volumevar, ref float panvar)
        {
            return NativeMethods.FMOD_Sound_GetVariations(soundraw, ref frequencyvar, ref volumevar, ref panvar);
        }

        public Result SetSubSound(int index, Sound subsound)
        {
            IntPtr subsoundraw = subsound.getRaw();
            return NativeMethods.FMOD_Sound_SetSubSound(soundraw, index, subsoundraw);
        }

        public Result GetSubSound(int index, ref Sound subsound)
        {
            Result Result = Result.OK;
            IntPtr subsoundraw = new IntPtr();
            Sound subsoundnew = null;
            try
            {
                Result = NativeMethods.FMOD_Sound_GetSubSound(soundraw, index, ref subsoundraw);
            }

            catch
            {
                Result = Result.ERR_INVALID_PARAM;
            }

            if (Result != Result.OK)
            {
                return Result;
            }

            if (subsound == null)
            {
                subsoundnew = new Sound();
                subsoundnew.setRaw(subsoundraw);
                subsound = subsoundnew;
            }

            else
            {
                subsound.setRaw(subsoundraw);
            }

            return Result;
        }

        public Result SetSubSoundSentence(int[] subsoundlist, int numsubsounds)
        {
            return NativeMethods.FMOD_Sound_SetSubSoundSentence(soundraw, subsoundlist, numsubsounds);
        }

        public Result GetName(StringBuilder name, int namelen)
        {
            return NativeMethods.FMOD_Sound_GetName(soundraw, name, namelen);
        }

        public Result GetLength(ref uint length, TIMEUNIT lengthtype)
        {
            return NativeMethods.FMOD_Sound_GetLength(soundraw, ref length, lengthtype);
        }

        public Result GetFormat(ref SoundType type, ref SoundFormat format, ref int channels, ref int bits)
        {
            return NativeMethods.FMOD_Sound_GetFormat(soundraw, ref type, ref format, ref channels, ref bits);
        }

        public Result GetNumSubSounds(ref int numsubsounds)
        {
            return NativeMethods.FMOD_Sound_GetNumSubSounds(soundraw, ref numsubsounds);
        }

        public Result GetNumTags(ref int numtags, ref int numtagsupdated)
        {
            return NativeMethods.FMOD_Sound_GetNumTags(soundraw, ref numtags, ref numtagsupdated);
        }

        public Result GetTag(string name, int index, ref TAG tag)
        {
            IntPtr ptr = Marshal.AllocCoTaskMem(Marshal.SizeOf(tag));
            Result Result = NativeMethods.FMOD_Sound_GetTag(soundraw, name, index, ptr);
            if (Result == Result.OK)
            {
                tag = (TAG)Marshal.PtrToStructure(ptr, typeof(TAG));
            }

            return Result;
        }

        public Result GetOpenState(ref OPENSTATE openstate, ref uint percentbuffered, ref bool starving)
        {
            return NativeMethods.FMOD_Sound_GetOpenState(soundraw, ref openstate, ref percentbuffered, ref starving);
        }

        public Result ReadData(IntPtr buffer, uint lenbytes, ref uint read)
        {
            return NativeMethods.FMOD_Sound_ReadData(soundraw, buffer, lenbytes, ref read);
        }

        public Result SeekData(uint pcm)
        {
            return NativeMethods.FMOD_Sound_SeekData(soundraw, pcm);
        }

        public Result SetSoundGroup(SoundGroup soundgroup)
        {
            return NativeMethods.FMOD_Sound_SetSoundGroup(soundraw, soundgroup.getRaw());
        }

        public Result GetSoundGroup(ref SoundGroup soundgroup)
        {
            Result Result = Result.OK;
            IntPtr soundgroupraw = new IntPtr();
            SoundGroup soundgroupnew = null;
            try
            {
                Result = NativeMethods.FMOD_Sound_GetSoundGroup(soundraw, ref soundgroupraw);
            }

            catch
            {
                Result = Result.ERR_INVALID_PARAM;
            }

            if (Result != Result.OK)
            {
                return Result;
            }

            if (soundgroup == null)
            {
                soundgroupnew = new SoundGroup();
                soundgroupnew.setRaw(soundgroupraw);
                soundgroup = soundgroupnew;
            }

            else
            {
                soundgroup.setRaw(soundgroupraw);
            }

            return Result;
        }

        public Result GetNumSyncPoints(ref int numsyncpoints)
        {
            return NativeMethods.FMOD_Sound_GetNumSyncPoints(soundraw, ref numsyncpoints);
        }

        public Result GetSyncPoint(int index, ref IntPtr point)
        {
            return NativeMethods.FMOD_Sound_GetSyncPoint(soundraw, index, ref point);
        }

        public Result GetSyncPointInfo(IntPtr point, StringBuilder name, int namelen, ref uint offset, TIMEUNIT offsettype)
        {
            return NativeMethods.FMOD_Sound_GetSyncPointInfo(soundraw, point, name, namelen, ref offset, offsettype);
        }

        public Result AddSyncPoint(int offset, TIMEUNIT offsettype, string name, ref IntPtr point)
        {
            return NativeMethods.FMOD_Sound_AddSyncPoint(soundraw, offset, offsettype, name, ref point);
        }

        public Result DeleteSyncPoint(IntPtr point)
        {
            return NativeMethods.FMOD_Sound_DeleteSyncPoint(soundraw, point);
        }
        public Result SetMode(Mode mode)
        {
            return NativeMethods.FMOD_Sound_SetMode(soundraw, mode);
        }
        public Result GetMode(ref Mode mode)
        {
            return NativeMethods.FMOD_Sound_GetMode(soundraw, ref mode);
        }

        public Result SetLoopCount(int loopcount)
        {
            return NativeMethods.FMOD_Sound_SetLoopCount(soundraw, loopcount);
        }

        public Result GetLoopCount(ref int loopcount)
        {
            return NativeMethods.FMOD_Sound_GetLoopCount(soundraw, ref loopcount);
        }

        public Result SetLoopPoints(uint loopstart, TIMEUNIT loopstarttype, uint loopend, TIMEUNIT loopendtype)
        {
            return NativeMethods.FMOD_Sound_SetLoopPoints(soundraw, loopstart, loopstarttype, loopend, loopendtype);
        }

        public Result GetLoopPoints(ref uint loopstart, TIMEUNIT loopstarttype, ref uint loopend, TIMEUNIT loopendtype)
        {
            return NativeMethods.FMOD_Sound_GetLoopPoints(soundraw, ref loopstart, loopstarttype, ref loopend, loopendtype);
        }

        public Result GetMusicNumChannels(ref int numchannels)
        {
            return NativeMethods.FMOD_Sound_GetMusicNumChannels(soundraw, ref numchannels);
        }

        public Result SetMusicChannelVolume(int channel, float volume)
        {
            return NativeMethods.FMOD_Sound_SetMusicChannelVolume(soundraw, channel, volume);
        }

        public Result GetMusicChannelVolume(int channel, ref float volume)
        {
            return NativeMethods.FMOD_Sound_GetMusicChannelVolume(soundraw, channel, ref volume);
        }

        public Result SetUserData(IntPtr userdata)
        {
            return NativeMethods.FMOD_Sound_SetUserData(soundraw, userdata);
        }

        public Result GetUserData(ref IntPtr userdata)
        {
            return NativeMethods.FMOD_Sound_GetUserData(soundraw, ref userdata);
        }

        public Result GetMemoryInfo(uint memorybits, uint event_memorybits, ref uint memoryused, IntPtr memoryused_array)
        {
            return NativeMethods.FMOD_Sound_GetMemoryInfo(soundraw, memorybits, event_memorybits, ref memoryused, memoryused_array);
        }

        #region wrapperinternal
        private IntPtr soundraw;
        public void setRaw(IntPtr sound)
        {
            soundraw = new IntPtr();
            soundraw = sound;
        }

        public IntPtr getRaw()
        {
            return soundraw;
        }

        #endregion

        private class NativeMethods
        {
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Sound_Release(IntPtr sound);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Sound_GetSystemObject(IntPtr sound, ref IntPtr system);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Sound_Lock(IntPtr sound, uint offset, uint length, ref IntPtr ptr1, ref IntPtr ptr2, ref uint len1, ref uint len2);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Sound_Unlock(IntPtr sound, IntPtr ptr1, IntPtr ptr2, uint len1, uint len2);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Sound_SetDefaults(IntPtr sound, float frequency, float volume, float pan, int priority);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Sound_GetDefaults(IntPtr sound, ref float frequency, ref float volume, ref float pan, ref int priority);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Sound_SetVariations(IntPtr sound, float frequencyvar, float volumevar, float panvar);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Sound_GetVariations(IntPtr sound, ref float frequencyvar, ref float volumevar, ref float panvar);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Sound_SetSubSound(IntPtr sound, int index, IntPtr subsound);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Sound_GetSubSound(IntPtr sound, int index, ref IntPtr subsound);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Sound_SetSubSoundSentence(IntPtr sound, int[] subsoundlist, int numsubsounds);
            [DllImport(Version.Dll, CharSet = CharSet.Unicode)]
            public static extern Result FMOD_Sound_GetName(IntPtr sound, StringBuilder name, int namelen);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Sound_GetLength(IntPtr sound, ref uint length, TIMEUNIT lengthtype);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Sound_GetFormat(IntPtr sound, ref SoundType type, ref SoundFormat format, ref int channels, ref int bits);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Sound_GetNumSubSounds(IntPtr sound, ref int numsubsounds);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Sound_GetNumTags(IntPtr sound, ref int numtags, ref int numtagsupdated);
            [DllImport(Version.Dll, CharSet = CharSet.Unicode)]
            public static extern Result FMOD_Sound_GetTag(IntPtr sound, string name, int index, IntPtr tag);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Sound_GetOpenState(IntPtr sound, ref OPENSTATE openstate, ref uint percentbuffered, ref bool starving);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Sound_ReadData(IntPtr sound, IntPtr buffer, uint lenbytes, ref uint read);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Sound_SeekData(IntPtr sound, uint pcm);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Sound_SetSoundGroup(IntPtr sound, IntPtr soundgroup);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Sound_GetSoundGroup(IntPtr sound, ref IntPtr soundgroup);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Sound_GetNumSyncPoints(IntPtr sound, ref int numsyncpoints);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Sound_GetSyncPoint(IntPtr sound, int index, ref IntPtr point);
            [DllImport(Version.Dll, CharSet = CharSet.Unicode)]
            public static extern Result FMOD_Sound_GetSyncPointInfo(IntPtr sound, IntPtr point, StringBuilder name, int namelen, ref uint offset, TIMEUNIT offsettype);
            [DllImport(Version.Dll, CharSet = CharSet.Unicode)]
            public static extern Result FMOD_Sound_AddSyncPoint(IntPtr sound, int offset, TIMEUNIT offsettype, string name, ref IntPtr point);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Sound_DeleteSyncPoint(IntPtr sound, IntPtr point);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Sound_SetMode(IntPtr sound, Mode mode);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Sound_GetMode(IntPtr sound, ref Mode mode);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Sound_SetLoopCount(IntPtr sound, int loopcount);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Sound_GetLoopCount(IntPtr sound, ref int loopcount);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Sound_SetLoopPoints(IntPtr sound, uint loopstart, TIMEUNIT loopstarttype, uint loopend, TIMEUNIT loopendtype);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Sound_GetLoopPoints(IntPtr sound, ref uint loopstart, TIMEUNIT loopstarttype, ref uint loopend, TIMEUNIT loopendtype);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Sound_GetMusicNumChannels(IntPtr sound, ref int numchannels);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Sound_SetMusicChannelVolume(IntPtr sound, int channel, float volume);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Sound_GetMusicChannelVolume(IntPtr sound, int channel, ref float volume);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Sound_SetUserData(IntPtr sound, IntPtr userdata);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Sound_GetUserData(IntPtr sound, ref IntPtr userdata);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Sound_GetMemoryInfo(IntPtr sound, uint memorybits, uint event_memorybits, ref uint memoryused, IntPtr memoryused_array);
        }
    }

    public class Channel
    {
        public Result GetSystemObject(ref System system)
        {
            Result Result = Result.OK;
            IntPtr systemraw = new IntPtr();
            System systemnew = null;
            try
            {
                Result = NativeMethods.FMOD_Channel_GetSystemObject(channelraw, ref systemraw);
            }

            catch
            {
                Result = Result.ERR_INVALID_PARAM;
            }

            if (Result != Result.OK)
            {
                return Result;
            }

            if (system == null)
            {
                systemnew = new System();
                systemnew.setRaw(systemraw);
                system = systemnew;
            }

            else
            {
                system.setRaw(systemraw);
            }

            return Result;
        }

        public Result Stop()
        {
            return NativeMethods.FMOD_Channel_Stop(channelraw);
        }

        public Result SetPaused(bool paused)
        {
            return NativeMethods.FMOD_Channel_SetPaused(channelraw, (paused ? 1 : 0));
        }

        public Result GetPaused(ref bool paused)
        {
            Result Result;
            int p = 0;
            Result = NativeMethods.FMOD_Channel_GetPaused(channelraw, ref p);
            paused = (p != 0);
            return Result;
        }

        public Result SetVolume(float volume)
        {
            return NativeMethods.FMOD_Channel_SetVolume(channelraw, volume);
        }

        public Result GetVolume(ref float volume)
        {
            return NativeMethods.FMOD_Channel_GetVolume(channelraw, ref volume);
        }

        public Result SetFrequency(float frequency)
        {
            return NativeMethods.FMOD_Channel_SetFrequency(channelraw, frequency);
        }

        public Result GetFrequency(ref float frequency)
        {
            return NativeMethods.FMOD_Channel_GetFrequency(channelraw, ref frequency);
        }

        public Result SetPan(float pan)
        {
            return NativeMethods.FMOD_Channel_SetPan(channelraw, pan);
        }

        public Result GetPan(ref float pan)
        {
            return NativeMethods.FMOD_Channel_GetPan(channelraw, ref pan);
        }

        public Result SetSpeakerMix(float frontleft, float frontright, float center, float lfe, float backleft, float backright, float sideleft, float sideright)
        {
            return NativeMethods.FMOD_Channel_SetSpeakerMix(channelraw, frontleft, frontright, center, lfe, backleft, backright, sideleft, sideright);
        }

        public Result GetSpeakerMix(ref float frontleft, ref float frontright, ref float center, ref float lfe, ref float backleft, ref float backright, ref float sideleft, ref float sideright)
        {
            return NativeMethods.FMOD_Channel_GetSpeakerMix(channelraw, ref frontleft, ref frontright, ref center, ref lfe, ref backleft, ref backright, ref sideleft, ref sideright);
        }

        public Result SetSpeakerLevels(Speaker speaker, float[] levels, int numlevels)
        {
            return NativeMethods.FMOD_Channel_SetSpeakerLevels(channelraw, speaker, levels, numlevels);
        }

        public Result GetSpeakerLevels(Speaker speaker, float[] levels, int numlevels)
        {
            return NativeMethods.FMOD_Channel_GetSpeakerLevels(channelraw, speaker, levels, numlevels);
        }

        public Result SetInputChannelMix(ref float levels, int numlevels)
        {
            return NativeMethods.FMOD_Channel_SetInputChannelMix(channelraw, ref levels, numlevels);
        }

        public Result GetInputChannelMix(ref float levels, int numlevels)
        {
            return NativeMethods.FMOD_Channel_GetInputChannelMix(channelraw, ref levels, numlevels);
        }

        public Result SetMute(bool mute)
        {
            return NativeMethods.FMOD_Channel_SetMute(channelraw, (mute ? 1 : 0));
        }

        public Result GetMute(ref bool mute)
        {
            Result Result;
            int m = 0;
            Result = NativeMethods.FMOD_Channel_GetMute(channelraw, ref m);
            mute = (m != 0);
            return Result;
        }

        public Result SetPriority(int priority)
        {
            return NativeMethods.FMOD_Channel_SetPriority(channelraw, priority);
        }

        public Result GetPriority(ref int priority)
        {
            return NativeMethods.FMOD_Channel_GetPriority(channelraw, ref priority);
        }

        public Result SetPosition(uint position, TIMEUNIT postype)
        {
            return NativeMethods.FMOD_Channel_SetPosition(channelraw, position, postype);
        }

        public Result GetPosition(ref uint position, TIMEUNIT postype)
        {
            return NativeMethods.FMOD_Channel_GetPosition(channelraw, ref position, postype);
        }

        public Result SetChannelGroup(ChannelGroup channelgroup)
        {
            return NativeMethods.FMOD_Channel_SetChannelGroup(channelraw, channelgroup.getRaw());
        }

        public Result GetChannelGroup(ref ChannelGroup channelgroup)
        {
            Result Result = Result.OK;
            IntPtr channelgroupraw = new IntPtr();
            ChannelGroup channelgroupnew = null;
            try
            {
                Result = NativeMethods.FMOD_Channel_GetChannelGroup(channelraw, ref channelgroupraw);
            }

            catch
            {
                Result = Result.ERR_INVALID_PARAM;
            }

            if (Result != Result.OK)
            {
                return Result;
            }

            if (channelgroup == null)
            {
                channelgroupnew = new ChannelGroup();
                channelgroupnew.setRaw(channelgroupraw);
                channelgroup = channelgroupnew;
            }

            else
            {
                channelgroup.setRaw(channelgroupraw);
            }

            return Result;
        }

        public Result SetCallback(CallBackDelegate callback)
        {
            return NativeMethods.FMOD_Channel_SetCallback(channelraw, callback);
        }

        public delegate Result CallBackDelegate(IntPtr channel, CHANNEL_CALLBACKTYPE type, int commandData1, int commandData2);

        public Result IsPlaying(ref bool isplaying)
        {
            return NativeMethods.FMOD_Channel_IsPlaying(channelraw, ref isplaying);
        }

        public Result IsVirtual(ref bool isvirtual)
        {
            return NativeMethods.FMOD_Channel_IsVirtual(channelraw, ref isvirtual);
        }

        public Result GetAudibility(ref float audibility)
        {
            return NativeMethods.FMOD_Channel_GetAudibility(channelraw, ref audibility);
        }

        public Result GetCurrentSound(ref Sound sound)
        {
            Result Result = Result.OK;
            IntPtr soundraw = new IntPtr();
            Sound soundnew = null;
            try
            {
                Result = NativeMethods.FMOD_Channel_GetCurrentSound(channelraw, ref soundraw);
            }

            catch
            {
                Result = Result.ERR_INVALID_PARAM;
            }

            if (Result != Result.OK)
            {
                return Result;
            }

            if (sound == null)
            {
                soundnew = new Sound();
                soundnew.setRaw(soundraw);
                sound = soundnew;
            }

            else
            {
                sound.setRaw(soundraw);
            }

            return Result;
        }

        public Result GetWaveData(float[] wavearray, int numvalues, int channeloffset)
        {
            return NativeMethods.FMOD_Channel_GetWaveData(channelraw, wavearray, numvalues, channeloffset);
        }

        public Result GetIndex(ref int index)
        {
            return NativeMethods.FMOD_Channel_GetIndex(channelraw, ref index);
        }

        public Result SetMode(Mode mode)
        {
            return NativeMethods.FMOD_Channel_SetMode(channelraw, mode);
        }
        public Result GetMode(ref Mode mode)
        {
            return NativeMethods.FMOD_Channel_GetMode(channelraw, ref mode);
        }

        public Result SetLoopCount(int loopcount)
        {
            return NativeMethods.FMOD_Channel_SetLoopCount(channelraw, loopcount);
        }

        public Result GetLoopCount(ref int loopcount)
        {
            return NativeMethods.FMOD_Channel_GetLoopCount(channelraw, ref loopcount);
        }

        public Result SetLoopPoints(uint loopstart, TIMEUNIT loopstarttype, uint loopend, TIMEUNIT loopendtype)
        {
            return NativeMethods.FMOD_Channel_SetLoopPoints(channelraw, loopstart, loopstarttype, loopend, loopendtype);
        }

        public Result GetLoopPoints(ref uint loopstart, TIMEUNIT loopstarttype, ref uint loopend, TIMEUNIT loopendtype)
        {
            return NativeMethods.FMOD_Channel_GetLoopPoints(channelraw, ref loopstart, loopstarttype, ref loopend, loopendtype);
        }

        public Result SetUserData(IntPtr userdata)
        {
            return NativeMethods.FMOD_Channel_SetUserData(channelraw, userdata);
        }

        public Result GetUserData(ref IntPtr userdata)
        {
            return NativeMethods.FMOD_Channel_GetUserData(channelraw, ref userdata);
        }

        public Result GetMemoryInfo(uint memorybits, uint event_memorybits, ref uint memoryused, IntPtr memoryused_array)
        {
            return NativeMethods.FMOD_Channel_GetMemoryInfo(channelraw, memorybits, event_memorybits, ref memoryused, memoryused_array);
        }

        #region wrapperinternal
        private IntPtr channelraw;
        public void setRaw(IntPtr channel)
        {
            channelraw = new IntPtr();
            channelraw = channel;
        }

        public IntPtr getRaw()
        {
            return channelraw;
        }

        #endregion

        private class NativeMethods
        {
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_GetSystemObject(IntPtr channel, ref IntPtr system);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_Stop(IntPtr channel);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_SetPaused(IntPtr channel, int paused);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_GetPaused(IntPtr channel, ref int paused);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_SetVolume(IntPtr channel, float volume);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_GetVolume(IntPtr channel, ref float volume);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_SetFrequency(IntPtr channel, float frequency);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_GetFrequency(IntPtr channel, ref float frequency);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_SetPan(IntPtr channel, float pan);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_GetPan(IntPtr channel, ref float pan);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_SetSpeakerMix(IntPtr channel, float frontleft, float frontright, float center, float lfe, float backleft, float backright, float sideleft, float sideright);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_GetSpeakerMix(IntPtr channel, ref float frontleft, ref float frontright, ref float center, ref float lfe, ref float backleft, ref float backright, ref float sideleft, ref float sideright);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_SetSpeakerLevels(IntPtr channel, Speaker speaker, float[] levels, int numlevels);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_GetSpeakerLevels(IntPtr channel, Speaker speaker, [MarshalAs(UnmanagedType.LPArray)]float[] levels, int numlevels);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_SetInputChannelMix(IntPtr channel, ref float levels, int numlevels);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_GetInputChannelMix(IntPtr channel, ref float levels, int numlevels);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_SetMute(IntPtr channel, int mute);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_GetMute(IntPtr channel, ref int mute);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_SetPriority(IntPtr channel, int priority);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_GetPriority(IntPtr channel, ref int priority);

            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_SetChannelGroup(IntPtr channel, IntPtr channelgroup);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_GetChannelGroup(IntPtr channel, ref IntPtr channelgroup);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_IsPlaying(IntPtr channel, ref bool isplaying);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_IsVirtual(IntPtr channel, ref bool isvirtual);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_GetAudibility(IntPtr channel, ref float audibility);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_GetCurrentSound(IntPtr channel, ref IntPtr sound);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_GetWaveData(IntPtr channel, [MarshalAs(UnmanagedType.LPArray)] float[] wavearray, int numvalues, int channeloffset);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_GetIndex(IntPtr channel, ref int index);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_SetCallback(IntPtr channel, Channel.CallBackDelegate callback);

            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_SetPosition(IntPtr channel, uint position, TIMEUNIT postype);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_GetPosition(IntPtr channel, ref uint position, TIMEUNIT postype);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_SetMode(IntPtr channel, Mode mode);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_GetMode(IntPtr channel, ref Mode mode);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_SetLoopCount(IntPtr channel, int loopcount);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_GetLoopCount(IntPtr channel, ref int loopcount);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_SetLoopPoints(IntPtr channel, uint loopstart, TIMEUNIT loopstarttype, uint loopend, TIMEUNIT loopendtype);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_GetLoopPoints(IntPtr channel, ref uint loopstart, TIMEUNIT loopstarttype, ref uint loopend, TIMEUNIT loopendtype);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_SetUserData(IntPtr channel, IntPtr userdata);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_GetUserData(IntPtr channel, ref IntPtr userdata);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_Channel_GetMemoryInfo(IntPtr channel, uint memorybits, uint event_memorybits, ref uint memoryused, IntPtr memoryused_array);
        }
    }

    public class ChannelGroup
    {
        public Result Release()
        {
            return NativeMethods.FMOD_ChannelGroup_Release(channelgroupraw);
        }

        public Result GetSystemObject(ref System system)
        {
            Result Result = Result.OK;
            IntPtr systemraw = new IntPtr();
            System systemnew = null;
            try
            {
                Result = NativeMethods.FMOD_ChannelGroup_GetSystemObject(channelgroupraw, ref systemraw);
            }

            catch
            {
                Result = Result.ERR_INVALID_PARAM;
            }

            if (Result != Result.OK)
            {
                return Result;
            }

            if (system == null)
            {
                systemnew = new System();
                systemnew.setRaw(systemraw);
                system = systemnew;
            }

            else
            {
                system.setRaw(systemraw);
            }

            return Result;
        }
     
        public Result SetVolume(float volume)
        {
            return NativeMethods.FMOD_ChannelGroup_SetVolume(channelgroupraw, volume);
        }

        public Result GetVolume(ref float volume)
        {
            return NativeMethods.FMOD_ChannelGroup_GetVolume(channelgroupraw, ref volume);
        }

        public Result SetPitch(float pitch)
        {
            return NativeMethods.FMOD_ChannelGroup_SetPitch(channelgroupraw, pitch);
        }

        public Result GetPitch(ref float pitch)
        {
            return NativeMethods.FMOD_ChannelGroup_GetPitch(channelgroupraw, ref pitch);
        }

        public Result SetPaused(bool paused)
        {
            return NativeMethods.FMOD_ChannelGroup_SetPaused(channelgroupraw, (paused ? 1 : 0));
        }

        public Result GetPaused(ref bool paused)
        {
            Result Result;
            int p = 0;
            Result = NativeMethods.FMOD_ChannelGroup_GetPaused(channelgroupraw, ref p);
            paused = (p != 0);
            return Result;
        }

        public Result SetMute(bool mute)
        {
            return NativeMethods.FMOD_ChannelGroup_SetMute(channelgroupraw, (mute ? 1 : 0));
        }

        public Result GetMute(ref bool mute)
        {
            Result Result;
            int m = 0;
            Result = NativeMethods.FMOD_ChannelGroup_GetMute(channelgroupraw, ref m);
            mute = (m != 0);
            return Result;
        }
       
        public Result Stop()
        {
            return NativeMethods.FMOD_ChannelGroup_Stop(channelgroupraw);
        }

        public Result OverrideVolume(float volume)
        {
            return NativeMethods.FMOD_ChannelGroup_OverrideVolume(channelgroupraw, volume);
        }

        public Result OverrideFrequency(float frequency)
        {
            return NativeMethods.FMOD_ChannelGroup_OverrideFrequency(channelgroupraw, frequency);
        }

        public Result OverridePan(float pan)
        {
            return NativeMethods.FMOD_ChannelGroup_OverridePan(channelgroupraw, pan);
        }
       
        public Result AddGroup(ChannelGroup group)
        {
            return NativeMethods.FMOD_ChannelGroup_AddGroup(channelgroupraw, group.getRaw());
        }

        public Result GetNumGroups(ref int numgroups)
        {
            return NativeMethods.FMOD_ChannelGroup_GetNumGroups(channelgroupraw, ref numgroups);
        }

        public Result GetGroup(int index, ref ChannelGroup group)
        {
            Result Result = Result.OK;
            IntPtr channelraw = new IntPtr();
            ChannelGroup channelnew = null;
            try
            {
                Result = NativeMethods.FMOD_ChannelGroup_GetGroup(channelgroupraw, index, ref channelraw);
            }

            catch
            {
                Result = Result.ERR_INVALID_PARAM;
            }

            if (Result != Result.OK)
            {
                return Result;
            }

            if (group == null)
            {
                channelnew = new ChannelGroup();
                channelnew.setRaw(channelraw);
                group = channelnew;
            }

            else
            {
                group.setRaw(channelraw);
            }

            return Result;
        }

        public Result GetParentGroup(ref ChannelGroup group)
        {
            Result Result = Result.OK;
            IntPtr channelraw = new IntPtr();
            ChannelGroup channelnew = null;
            try
            {
                Result = NativeMethods.FMOD_ChannelGroup_GetParentGroup(channelgroupraw, ref channelraw);
            }

            catch
            {
                Result = Result.ERR_INVALID_PARAM;
            }

            if (Result != Result.OK)
            {
                return Result;
            }

            if (group == null)
            {
                channelnew = new ChannelGroup();
                channelnew.setRaw(channelraw);
                group = channelnew;
            }

            else
            {
                group.setRaw(channelraw);
            }

            return Result;
        }

        public Result GetName(StringBuilder name, int namelen)
        {
            return NativeMethods.FMOD_ChannelGroup_GetName(channelgroupraw, name, namelen);
        }

        public Result GetNumChannels(ref int numchannels)
        {
            return NativeMethods.FMOD_ChannelGroup_GetNumChannels(channelgroupraw, ref numchannels);
        }

        public Result GetChannel(int index, ref Channel channel)
        {
            Result Result = Result.OK;
            IntPtr channelraw = new IntPtr();
            Channel channelnew = null;
            try
            {
                Result = NativeMethods.FMOD_ChannelGroup_GetChannel(channelgroupraw, index, ref channelraw);
            }

            catch
            {
                Result = Result.ERR_INVALID_PARAM;
            }

            if (Result != Result.OK)
            {
                return Result;
            }

            if (channel == null)
            {
                channelnew = new Channel();
                channelnew.setRaw(channelraw);
                channel = channelnew;
            }

            else
            {
                channel.setRaw(channelraw);
            }

            return Result;
        }

        public Result GetWaveData(float[] wavearray, int numvalues, int channeloffset)
        {
            return NativeMethods.FMOD_ChannelGroup_GetWaveData(channelgroupraw, wavearray, numvalues, channeloffset);
        }
       
        public Result SetUserData(IntPtr userdata)
        {
            return NativeMethods.FMOD_ChannelGroup_SetUserData(channelgroupraw, userdata);
        }

        public Result GetUserData(ref IntPtr userdata)
        {
            return NativeMethods.FMOD_ChannelGroup_GetUserData(channelgroupraw, ref userdata);
        }

        public Result GetMemoryInfo(uint memorybits, uint event_memorybits, ref uint memoryused, IntPtr memoryused_array)
        {
            return NativeMethods.FMOD_ChannelGroup_GetMemoryInfo(channelgroupraw, memorybits, event_memorybits, ref memoryused, memoryused_array);
        }

        #region wrapperinternal
        private IntPtr channelgroupraw;
        public void setRaw(IntPtr channelgroup)
        {
            channelgroupraw = new IntPtr();
            channelgroupraw = channelgroup;
        }

        public IntPtr getRaw()
        {
            return channelgroupraw;
        }

        #endregion

        private class NativeMethods
        {
            [DllImport(Version.Dll)]
            public static extern Result FMOD_ChannelGroup_Release(IntPtr channelgroup);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_ChannelGroup_GetSystemObject(IntPtr channelgroup, ref IntPtr system);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_ChannelGroup_SetVolume(IntPtr channelgroup, float volume);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_ChannelGroup_GetVolume(IntPtr channelgroup, ref float volume);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_ChannelGroup_SetPitch(IntPtr channelgroup, float pitch);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_ChannelGroup_GetPitch(IntPtr channelgroup, ref float pitch);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_ChannelGroup_SetPaused(IntPtr channelgroup, int paused);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_ChannelGroup_GetPaused(IntPtr channelgroup, ref int paused);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_ChannelGroup_SetMute(IntPtr channelgroup, int mute);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_ChannelGroup_GetMute(IntPtr channelgroup, ref int mute);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_ChannelGroup_Stop(IntPtr channelgroup);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_ChannelGroup_OverridePaused(IntPtr channelgroup, bool paused);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_ChannelGroup_OverrideVolume(IntPtr channelgroup, float volume);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_ChannelGroup_OverrideFrequency(IntPtr channelgroup, float frequency);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_ChannelGroup_OverridePan(IntPtr channelgroup, float pan);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_ChannelGroup_OverrideMute(IntPtr channelgroup, bool mute);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_ChannelGroup_AddGroup(IntPtr channelgroup, IntPtr group);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_ChannelGroup_GetNumGroups(IntPtr channelgroup, ref int numgroups);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_ChannelGroup_GetGroup(IntPtr channelgroup, int index, ref IntPtr group);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_ChannelGroup_GetParentGroup(IntPtr channelgroup, ref IntPtr group);
            [DllImport(Version.Dll, CharSet = CharSet.Unicode)]
            public static extern Result FMOD_ChannelGroup_GetName(IntPtr channelgroup, StringBuilder name, int namelen);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_ChannelGroup_GetNumChannels(IntPtr channelgroup, ref int numchannels);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_ChannelGroup_GetChannel(IntPtr channelgroup, int index, ref IntPtr channel);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_ChannelGroup_GetWaveData(IntPtr channelgroup, [MarshalAs(UnmanagedType.LPArray)] float[] wavearray, int numvalues, int channeloffset);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_ChannelGroup_SetUserData(IntPtr channelgroup, IntPtr userdata);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_ChannelGroup_GetUserData(IntPtr channelgroup, ref IntPtr userdata);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_ChannelGroup_GetMemoryInfo(IntPtr channelgroup, uint memorybits, uint event_memorybits, ref uint memoryused, IntPtr memoryused_array);
        }
    }

    public class SoundGroup
    {
        public Result Release()
        {
            return NativeMethods.FMOD_SoundGroup_Release(soundgroupraw);
        }

        public Result GetSystemObject(ref System system)
        {
            Result Result = Result.OK;
            IntPtr systemraw = new IntPtr();
            System systemnew = null;
            try
            {
                Result = NativeMethods.FMOD_SoundGroup_GetSystemObject(soundgroupraw, ref systemraw);
            }

            catch
            {
                Result = Result.ERR_INVALID_PARAM;
            }

            if (Result != Result.OK)
            {
                return Result;
            }

            if (system == null)
            {
                systemnew = new System();
                systemnew.setRaw(systemraw);
                system = systemnew;
            }

            else
            {
                system.setRaw(systemraw);
            }

            return Result;
        }
       
        public Result SetMaxAudible(int maxaudible)
        {
            return NativeMethods.FMOD_SoundGroup_SetMaxAudible(soundgroupraw, maxaudible);
        }

        public Result GetMaxAudible(ref int maxaudible)
        {
            return NativeMethods.FMOD_SoundGroup_GetMaxAudible(soundgroupraw, ref maxaudible);
        }

        public Result SetMaxAudibleBehavior(SOUNDGROUP_BEHAVIOR behavior)
        {
            return NativeMethods.FMOD_SoundGroup_SetMaxAudibleBehavior(soundgroupraw, behavior);
        }

        public Result GetMaxAudibleBehavior(ref SOUNDGROUP_BEHAVIOR behavior)
        {
            return NativeMethods.FMOD_SoundGroup_GetMaxAudibleBehavior(soundgroupraw, ref behavior);
        }

        public Result SetMuteFadeSpeed(float speed)
        {
            return NativeMethods.FMOD_SoundGroup_SetMuteFadeSpeed(soundgroupraw, speed);
        }

        public Result GetMuteFadeSpeed(ref float speed)
        {
            return NativeMethods.FMOD_SoundGroup_GetMuteFadeSpeed(soundgroupraw, ref speed);
        }

        public Result GetName(StringBuilder name, int namelen)
        {
            return NativeMethods.FMOD_SoundGroup_GetName(soundgroupraw, name, namelen);
        }

        public Result GetNumSounds(ref int numsounds)
        {
            return NativeMethods.FMOD_SoundGroup_GetNumSounds(soundgroupraw, ref numsounds);
        }

        public Result GetSound(int index, ref Sound sound)
        {
            Result Result = Result.OK;
            IntPtr soundraw = new IntPtr();
            Sound soundnew = null;
            try
            {
                Result = NativeMethods.FMOD_SoundGroup_GetSound(soundgroupraw, index, ref soundraw);
            }

            catch
            {
                Result = Result.ERR_INVALID_PARAM;
            }

            if (Result != Result.OK)
            {
                return Result;
            }

            if (sound == null)
            {
                soundnew = new Sound();
                soundnew.setRaw(soundraw);
                sound = soundnew;
            }

            else
            {
                sound.setRaw(soundraw);
            }

            return Result;
        }

        public Result GetNumPlaying(ref int numplaying)
        {
            return NativeMethods.FMOD_SoundGroup_GetNumPlaying(soundgroupraw, ref numplaying);
        }
        
        public Result SetUserData(IntPtr userdata)
        {
            return NativeMethods.FMOD_SoundGroup_SetUserData(soundgroupraw, userdata);
        }

        public Result GetUserData(ref IntPtr userdata)
        {
            return NativeMethods.FMOD_SoundGroup_GetUserData(soundgroupraw, ref userdata);
        }

        public Result GetMemoryInfo(uint memorybits, uint event_memorybits, ref uint memoryused, IntPtr memoryused_array)
        {
            return NativeMethods.FMOD_SoundGroup_GetMemoryInfo(soundgroupraw, memorybits, event_memorybits, ref memoryused, memoryused_array);
        }

        #region wrapperinternal
        private IntPtr soundgroupraw;
        public void setRaw(IntPtr soundgroup)
        {
            soundgroupraw = new IntPtr();
            soundgroupraw = soundgroup;
        }

        public IntPtr getRaw()
        {
            return soundgroupraw;
        }

        #endregion

        private class NativeMethods
        {
            [DllImport(Version.Dll)]
            public static extern Result FMOD_SoundGroup_Release(IntPtr soundgroupraw);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_SoundGroup_GetSystemObject(IntPtr soundgroupraw, ref IntPtr system);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_SoundGroup_SetMaxAudible(IntPtr soundgroupraw, int maxaudible);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_SoundGroup_GetMaxAudible(IntPtr soundgroupraw, ref int maxaudible);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_SoundGroup_SetMaxAudibleBehavior(IntPtr soundgroupraw, SOUNDGROUP_BEHAVIOR behavior);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_SoundGroup_GetMaxAudibleBehavior(IntPtr soundgroupraw, ref SOUNDGROUP_BEHAVIOR behavior);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_SoundGroup_SetMuteFadeSpeed(IntPtr soundgroupraw, float speed);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_SoundGroup_GetMuteFadeSpeed(IntPtr soundgroupraw, ref float speed);
            [DllImport(Version.Dll, CharSet = CharSet.Unicode)]
            public static extern Result FMOD_SoundGroup_GetName(IntPtr soundgroupraw, StringBuilder name, int namelen);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_SoundGroup_GetNumSounds(IntPtr soundgroupraw, ref int numsounds);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_SoundGroup_GetSound(IntPtr soundgroupraw, int index, ref IntPtr sound);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_SoundGroup_GetNumPlaying(IntPtr soundgroupraw, ref int numplaying);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_SoundGroup_SetUserData(IntPtr soundgroupraw, IntPtr userdata);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_SoundGroup_GetUserData(IntPtr soundgroupraw, ref IntPtr userdata);
            [DllImport(Version.Dll)]
            public static extern Result FMOD_SoundGroup_GetMemoryInfo(IntPtr soundgroupraw, uint memorybits, uint event_memorybits, ref uint memoryused, IntPtr memoryused_array);
        }
    }

}
