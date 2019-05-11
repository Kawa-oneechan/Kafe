/*
 * FMOD Ex C# wrapper, remolded from the Firelight code by Kawa.
 */

using System;
using System.Text;
using System.Runtime.InteropServices;

namespace FMOD
{
	/// <summary>
	/// FMOD version number. Check this against FMOD::System::getVersion / System.GetVersion
	/// 0xAAAABBCC, where AA is the major version number, BB is the minor version number and CC is the development version number.
	/// </summary>
    public class Version
    {
        public const int Number = 0x00042601;
        public const string Dll = "fmodex";
    }

	/// <summary>
	/// Describes a point in 3D space.
	/// </summary>
	/// <remarks>
	/// FMOD uses a left-handed coordinate system by default. To use a right-handed system, specify InitFlags.RightHanded3DSystem in System.Init().
	/// </remarks>
	/// <seealso cref="System.Set3DListenerAttributes"/>
	/// <seealso cref="System.Get3DListenerAttributes"/>
	/// <seealso cref="Channel.Set3DListenerAttributes"/>
	/// <seealso cref="Channel.Get3DListenerAttributes"/>
	/// <seealso cref="Geometry.AddPolygon"/>
	/// <seealso cref="Geometry.SetPolygonVertex"/>
	/// <seealso cref="Geometry.GetPolygonVertex"/>
	/// <seealso cref="Geometry.SetRotation"/>
	/// <seealso cref="Geometry.GetRotation"/>
	/// <seealso cref="Geometry.SetPosition"/>
	/// <seealso cref="Geometry.GetPosition"/>
	/// <seealso cref="Geometry.SetScale"/>
	/// <seealso cref="Geometry.GetScale"/>
	/// <seealso cref="InitFlags"/>
	[StructLayout(LayoutKind.Sequential)]
    public struct Vector
    {
        public float x;
        public float y;
        public float z;
    }

	/// <summary>
	/// Represents a globally unique identifier (GUID).
	/// </summary>
	/// <seealso cref="System.GetDriverInfo"/>
    [StructLayout(LayoutKind.Sequential)]
    public struct GUID
    {
        public uint   Data1;       /* Specifies the first 8 hexadecimal digits of the GUID */
        public ushort Data2;       /* Specifies the first group of 4 hexadecimal digits.   */
        public ushort Data3;       /* Specifies the second group of 4 hexadecimal digits.  */
        [MarshalAs(UnmanagedType.ByValArray,SizeConst=8)]
        public byte[] Data4;       /* Array of 8 bytes. The first 2 bytes contain the third group of 4 hexadecimal digits. The remaining 6 bytes contain the final 12 hexadecimal digits. */
    }

	/// <summary>
	/// Error codes returned from every function.
	/// </summary>
    public enum Result
    {
        OK,                        /* No errors. */
        AlreadyLocked,         /* Tried to call lock a second time before unlock was called. */
        BadCommand,            /* Tried to call a function on a data type that does not allow this type of functionality (ie calling Sound::lock on a streaming sound). */
        CDDDriverLoadError,          /* Neither NTSCSI nor ASPI could be initialised. */
        CDDAInitFailed,             /* An error occurred while initialising the CDDA subsystem. */
        CDDAInvalidDevice,   /* Couldn't find the specified device. */
        CDDANoAudio,          /* No audio tracks on the specified disc. */
        CDDANoDevices,        /* No CD/DVD devices were found. */ 
        CDDANoDisc,           /* No disc present in the specified drive. */
        CDDAReadError,             /* A CDDA read error occurred. */
        ChannelAllocationError,         /* Error trying to allocate a channel. */
        ChannelStolen,        /* The specified channel has been reused to play another sound. */
        COMError,                   /* A Win32 COM related error occured. COM failed to initialize or a QueryInterface failed meaning a Windows codec or driver was not installed properly. */
        DMAFailure,                   /* DMA Failure.  See debug output for more information. */
        DSPConnectionError,        /* DSP connection error.  Connection possibly caused a cyclic dependancy. */
        DSPFormatIncorrect,            /* DSP Format error.  A DSP unit may have attempted to connect to this network with the wrong format. */
        DSPUnitNotFound,          /* DSP connection error.  Couldn't find the DSP unit specified. */
        ERR_DSP_RUNNING,           /* DSP error.  Cannot perform this operation while the network is in the middle of running.  This will most likely happen if a connection or disconnection is attempted in a DSP callback. */
        ERR_DSP_TOOMANYCONNECTIONS,/* DSP connection error.  The unit being connected to or disconnected should only have 1 input or output. */
        ERR_FILE_BAD,              /* Error loading file. */
        ERR_FILE_COULDNOTSEEK,     /* Couldn't perform seek operation.  This is a limitation of the medium (ie netstreams) or the file format. */
        ERR_FILE_DISKEJECTED,      /* Media was ejected while reading. */
        ERR_FILE_EOF,              /* End of file unexpectedly reached while trying to read essential data (truncated data?). */
        ERR_FILE_NOTFOUND,         /* File not found. */
        ERR_FILE_UNWANTED,         /* Unwanted file access occured. */
        ERR_FORMAT,                /* Unsupported file or audio format. */
        ERR_HTTP,                  /* A HTTP error occurred. This is a catch-all for HTTP errors not listed elsewhere. */
        ERR_HTTP_ACCESS,           /* The specified resource requires authentication or is forbidden. */
        ERR_HTTP_PROXY_AUTH,       /* Proxy authentication is required to access the specified resource. */
        ERR_HTTP_SERVER_ERROR,     /* A HTTP server error occurred. */
        ERR_HTTP_TIMEOUT,          /* The HTTP request timed out. */
        ERR_INITIALIZATION,        /* FMOD was not initialized correctly to support this function. */
        ERR_INITIALIZED,           /* Cannot call this command after System::init. */
        ERR_INTERNAL,              /* An error occured that wasn't supposed to.  Contact support. */
        ERR_INVALID_ADDRESS,       /* On Xbox 360, this memory address passed to FMOD must be physical, (ie allocated with XPhysicalAlloc.) */
        ERR_INVALID_FLOAT,         /* Value passed in was a NaN, Inf or denormalized float. */
        ERR_INVALID_HANDLE,        /* An invalid object handle was used. */
        ERR_INVALID_PARAM,         /* An invalid parameter was passed to this function. */
        ERR_INVALID_SPEAKER,       /* An invalid speaker was passed to this function based on the current speaker mode. */
        ERR_INVALID_SYNCPOINT,     /* The syncpoint did not come from this sound handle. */
        ERR_INVALID_Vector,        /* The Vectors passed in are not unit length, or perpendicular. */
        ERR_IRX,                   /* PS2 only.  fmodex.irx failed to initialize.  This is most likely because you forgot to load it. */
        ERR_MAXAUDIBLE,            /* Reached maximum audible playback count for this sound's soundgroup. */
        ERR_MEMORY,                /* Not enough memory or resources. */
        ERR_MEMORY_CANTPOINT,      /* Can't use FMOD_OPENMEMORY_POINT on non PCM source data, or non mp3/xma/adpcm data if CREATECOMPRESSEDSAMPLE was used. */
        ERR_MEMORY_IOP,            /* PS2 only.  Not enough memory or resources on PlayStation 2 IOP ram. */
        ERR_MEMORY_SRAM,           /* Not enough memory or resources on console sound ram. */
        ERR_NEEDS2D,               /* Tried to call a command on a 3d sound when the command was meant for 2d sound. */
        ERR_NEEDS3D,               /* Tried to call a command on a 2d sound when the command was meant for 3d sound. */
        ERR_NEEDSHARDWARE,         /* Tried to use a feature that requires hardware support.  (ie trying to play a VAG compressed sound in software on PS2). */
        ERR_NEEDSSOFTWARE,         /* Tried to use a feature that requires the software engine.  Software engine has either been turned off, or command was executed on a hardware channel which does not support this feature. */
        ERR_NET_CONNECT,           /* Couldn't connect to the specified host. */
        ERR_NET_SOCKET_ERROR,      /* A socket error occurred.  This is a catch-all for socket-related errors not listed elsewhere. */
        ERR_NET_URL,               /* The specified URL couldn't be resolved. */
        ERR_NET_WOULD_BLOCK,       /* Operation on a non-blocking socket could not complete immediately. */
        ERR_NOTREADY,              /* Operation could not be performed because specified sound is not ready. */
        ERR_OUTPUT_ALLOCATED,      /* Error initializing output device, but more specifically, the output device is already in use and cannot be reused. */
        ERR_OUTPUT_CREATEBUFFER,   /* Error creating hardware sound buffer. */
        ERR_OUTPUT_DRIVERCALL,     /* A call to a standard soundcard driver failed, which could possibly mean a bug in the driver or resources were missing or exhausted. */
        ERR_OUTPUT_ENUMERATION,    /* Error enumerating the available driver list. List may be inconsistent due to a recent device addition or removal. */
        ERR_OUTPUT_FORMAT,         /* Soundcard does not support the minimum features needed for this soundsystem (16bit stereo output). */
        ERR_OUTPUT_INIT,           /* Error initializing output device. */
        ERR_OUTPUT_NOHARDWARE,     /* FMOD_HARDWARE was specified but the sound card does not have the resources nescessary to play it. */
        ERR_OUTPUT_NOSOFTWARE,     /* Attempted to create a software sound but no software channels were specified in System::init. */
        ERR_PAN,                   /* Panning only works with mono or stereo sound sources. */
        ERR_PLUGIN,                /* An unspecified error has been returned from a 3rd party plugin. */
        ERR_PLUGIN_INSTANCES,      /* The number of allowed instances of a plugin has been exceeded */
        ERR_PLUGIN_MISSING,        /* A requested output, dsp unit type or codec was not available. */
        ERR_PLUGIN_RESOURCE,       /* A resource that the plugin requires cannot be found. (ie the DLS file for MIDI playback) */
        ERR_RECORD,                /* An error occured trying to initialize the recording device. */
        ERR_REVERB_INSTANCE,       /* Specified Instance in REVERB_PROPERTIES couldn't be set. Most likely because another application has locked the EAX4 FX slot. */
        ERR_SUBSOUND_ALLOCATED,    /* This subsound is already being used by another sound, you cannot have more than one parent to a sound.  Null out the other parent's entry first. */
        ERR_SUBSOUND_CANTMOVE,     /* Shared subsounds cannot be replaced or moved from their parent stream, such as when the parent stream is an FSB file. */
        ERR_SUBSOUND_MODE,         /* The subsound's mode bits do not match with the parent sound's mode bits.  See documentation for function that it was called with. */
        ERR_SUBSOUNDS,             /* The error occured because the sound referenced contains subsounds.  (ie you cannot play the parent sound as a static sample, only its subsounds.) */
        ERR_TAGNOTFOUND,           /* The specified tag could not be found or there are no tags. */
        ERR_TOOMANYCHANNELS,       /* The sound created exceeds the allowable input channel count.  This can be increased using the maxinputchannels parameter in System::setSoftwareFormat. */
        ERR_UNIMPLEMENTED,         /* Something in FMOD hasn't been implemented when it should be! contact support! */
        ERR_UNINITIALIZED,         /* This command failed because System::init or System::setDriver was not called. */
        ERR_UNSUPPORTED,           /* A command issued was not supported by this object.  Possibly a plugin without certain callbacks specified. */
        ERR_UPDATE,                /* An error caused by System::update occured. */
        ERR_VERSION,               /* The version number of this file format is not supported. */

        ERR_EVENT_FAILED,          /* An Event failed to be retrieved, most likely due to 'just fail' being specified as the max playbacks behavior. */
        ERR_EVENT_INFOONLY,        /* Can't execute this command on an EVENT_INFOONLY event. */
        ERR_EVENT_INTERNAL,        /* An error occured that wasn't supposed to.  See debug log for reason. */
        ERR_EVENT_MAXSTREAMS,      /* Event failed because 'Max streams' was hit when FMOD_INIT_FAIL_ON_MAXSTREAMS was specified. */
        ERR_EVENT_MISMATCH,        /* FSB mis-matches the FEV it was compiled with. */
        ERR_EVENT_NAMECONFLICT,    /* A category with the same name already exists. */
        ERR_EVENT_NOTFOUND,        /* The requested event, event group, event category or event property could not be found. */
        ERR_EVENT_NEEDSSIMPLE,     /* Tried to call a function on a complex event that's only supported by simple events. */
        ERR_EVENT_GUIDCONFLICT,    /* An event with the same GUID already exists. */

        ERR_MUSIC_UNINITIALIZED    /* Music system is not initialized probably because no music data is loaded. */
    }

	/// <summary>
	/// The output type to use with System.GetOutput/System.SetOutput.
	/// </summary>
	/// <remarks>
	/// To drive the output synchronously, and to disable FMOD's timing thread, use the Initialisation.NonRealtime flag.
	/// To pass information to the driver when initializing FMOD, use the ExtraDriverData parameter for the following reasons:
	/// <para>1. OutputType.WaveWriter -- ExtraDriverData is a pointer to a char * fileName that the wave writer will output to.</para>
	/// <para>2. OutputType.WaveWriterNonRealtime -- ExtraDriverData is a pointer to a char * fileName that that the wave writer will output to.</para>
	/// <para>3. OutputType.DirectSound -- ExtraDriverData is a pointer to a hWnd, so that FMOD can set the focus on the audio for a particular window.</para>
	/// <para>4. OutputType.GameCube -- ExtraDriverData is a pointer to an FMOD.ARamBlockInfo structure. This can be found in fmodgc.h.</para>
	/// Currently these are the only FMOD drivers that take extra information.  Other unknown plugins may have different requirements.
	/// </remarks>
	/// <seealso cref="System.SetOutput"/>
	/// <seealso cref="System.GetOutput"/>
	/// <seealso cref="System.SetSoftwareFormat"/>
	/// <seealso cref="System.GetSoftwareFormat"/>
	/// <seealso cref="System.Init"/>
	public enum OutputType
    {
        AutoDetect,    /* Picks the best output mode for the platform.  This is the default. */

        Unknown,       /* All         - 3rd party plugin, unknown.  This is for use with System::getOutput only. */
        NoSound,       /* All         - All calls in this mode succeed but make no sound. */
        WaveWriter,     /* All         - All         - Writes output to fmodout.wav by default.  Use System::setSoftwareFormat to set the filename. */
        NoSoundNonRealtime,   /* All         - Non-realtime version of FMOD_OutputType_NOSOUND.  User can drive mixer with System::update at whatever rate they want. */
        WaveWriterNonRealtime, /* All         - Non-realtime version of FMOD_OutputType_WAVWRITER.  User can drive mixer with System::update at whatever rate they want. */

        DirectSound,        /* Win32/Win64 - DirectSound output.  Use this to get hardware accelerated 3d audio and EAX Reverb support. (Default on Windows) */
        WinMM,         /* Win32/Win64 - Windows Multimedia output. */
        OpenAL,        /* Win32/Win64 - OpenAL 1.1 output. Use this for lower CPU overhead than FMOD_OutputType_DSOUND, and also Vista H/W support with Creative Labs cards. */
        WindowsAudioSession,        /* Win32       - Windows Audio Session API. (Default on Windows Vista) */        
        ASIO,          /* Win32       - Low latency ASIO driver. */

		OpenSoundSystem,           /* Linux       - Open Sound System output. */
        AdvancedLinuxSoundSystem,          /* Linux       - Advanced Linux Sound Architecture output. */
        EnlightmentSoundDaemon,           /* Linux       - Enlightment Sound Daemon output. */

		SoundManager,  /* Mac         - Macintosh SoundManager output. */
        CoreAudio,     /* Mac         - Macintosh CoreAudio output */

		Xbox,          /* Xbox        - Native hardware output. */
        PlayStation2,           /* PS2         - Native hardware output. */
        PlayStation3,           /* PS3         - Native hardware output. (Default on PS3) */
        GameCube,            /* GameCube    - Native hardware output. */
        Xbox360,       /* Xbox 360    - Native hardware output. */
        PlayStationPortable,           /* PSP         - Native hardware output. */
        Wii,           /* Wii         - Native hardware output. (Default on Wii) */

        Maximum            /* Maximum number of output types supported. */
    }

	[Flags]
    public enum Capabilities
    {
        None = 0, // Device has no special capabilities.
		Hardware = 1, // Device supports hardware mixing.
		HardwareEmulated = 2, // User has hardware accelleration turned off. This incurs an extra 200 milliseconds of latency.
		MultiChannelOutput = 4, // Device has more than two channels.

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
		LimitedReverb = 0x2000, // Device supports some form of limited hardware reverb, maybe parameterless and only selectable by environment.
    }

	/// <summary>
	/// Bit fields to use with Debug.SetLevel/Debug.GetLevel to control the level of TTY debug output with logging versions of FMOD (fmodL).
	/// </summary>
	/// <seealso cref="Debug.SetLevel"/>
	/// <seealso cref="Debug.GetLevel"/>
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

	/// <summary>
	/// Bit field for the memory allocation type, passed into FMOD memory callbacks.
	/// </summary>
	[Flags]
    public enum MemoryType
    {
		Normal = 0,
		XBox360Physical = 0x100000,
		Persistent = 0x200000,
		Secondary = 0x400000,
    }

	/// <summary>
	/// Speaker types defined for use with System.SetSpeakerMode and System.GetSpeakerMode.
	/// </summary>
	/// <remarks>
	/// These are important notes on speaker modes in regards to sounds created with FMOD_SOFTWARE.
	/// Note below the phrase "sound channels" is used. These are the subchannels inside a sound. They are not related and have nothing to do with
	/// the FMOD Channel class. For example, a mono sound has one sound channel, a stereo sound has two.
	/// <para>
	/// SpeakerMode.Raw:
	/// This mode is for output devices that are not specifically mono/stereo/quad/whatever, but -are- multichannel.
	/// Sound channels map to speakers sequentially, so a mono sound maps to output speaker 0, stereo maps to output speakers 0 and 1.
	/// The user assumes knowledge of the speaker order. Speaker enumerations may not apply, so raw channel indices should be used.
	/// Multichannel sounds map input channels to output channels one-on-one.
	/// Channel.SetPan and Channel.SetSpeakerMix do not work. Speaker levels must be manually set with Channel.SetSpeakerLevels.
	/// </para>
	/// <para>
	/// SpeakerMode.Mono:
	/// This mode is for a one-speaker arrangement. Panning does not work in this mode. Mono, stereo and multichannel sounds have each sound channel
	/// played on the single speaker unit. Mix behavior for multichannel sounds can be set with Channel.SetSpeakerLevels.
	/// Channel.SetSpeakerMix does not work.
	/// </para>
	/// <para>
	/// SpeakerMode.Stereo:
	/// This mode is for 2 speaker arrangements that have a left and right speaker. Mono sounds default to an even distribution between left and right.
	/// They can be panned with Channel.SetPan. Stereo sounds default to the middle, or full left in the left speaker and full right in the right
	/// speaker. They can be cross faded with Channel.SetPan. Multichannel sounds have each sound channel played on each speaker at unity.
	/// Mix behaviour for multichannel sounds can be set with Channel.SetSpeakerLevels.
	/// Channel.SetSpeakerMix works but only front left and right parameters are used, the rest are ignored.
	/// </para>
	/// <para>
	/// SpeakerMode.Quad:
	/// This mode is for 4 speaker arrangements that have a front left, front right, rear left and a rear right speaker.
	/// Mono sounds default to an even distribution between front left and front right. They can be panned with Channel.SetPan.
	/// Stereo sounds default to the left sound channel played on the front left, and the right sound channel played on the front right.
	/// They can be cross faded with Channel.SetPan.
	/// Multichannel sounds default to all of their sound channels being played on each speaker in order of input.
	/// Mix behaviour for multichannel sounds can be set with Channel.SetSpeakerLevels.
	/// Channel.SetSpeakerMix works but side left, side right, center and lfe are ignored.
	/// </para>
	/// </remarks>
    public enum SpeakerMode
    {
        Raw,              /* There is no specific speakermode.  Sound channels are mapped in order of input to output.  See remarks for more information. */
        Mono,             /* The speakers are monaural. */
        Stereo,           /* The speakers are stereo (DEFAULT). */
        Quad,             /* 4 speaker setup.  This includes front left, front right, rear left, rear right.  */
        Surround,         /* 4 speaker setup.  This includes front left, front right, center, rear center (rear left/rear right are averaged). */
        Dolby51,         /* 5.1 speaker setup.  This includes front left, front right, center, rear left, rear right and a subwoofer. */
        Dolby71,         /* 7.1 speaker setup.  This includes front left, front right, center, rear left, rear right, side left, side right and a subwoofer. */
        ProLogic,         /* Stereo output, but data is encoded in a way that is picked up by a Prologic/Prologic2 decoder and split into a 5.1 speaker setup. */

        Maximum,              /* Maximum number of speaker modes supported. */
    }

	/// <summary>
	/// These are speaker types defined for use with the Channel.SetSpeakerLevels command.
	/// They can also be used for speaker placement with System.SetSpeakerPosition.
	/// </summary>
	/// <remarks>
	/// If you are using SpeakerMode.Raw and speaker assignments are meaningless, just cast a raw integer value to this type.
	/// </remarks>
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
    
        Maximum,                               /* Maximum number of speaker types supported. */
        Mono = FrontLeft,    /* For use with FMOD_SPEAKERMODE_MONO and Channel::SetSpeakerLevels.  Mapped to same value as FMOD_SPEAKER_FRONT_LEFT. */
        Null = Maximum,           /* A non speaker.  Use this to send. */
        SideBackLeft = SideLeft,     /* For use with FMOD_SPEAKERMODE_7POINT1 on PS3 where the extra speakers are surround back inside of side speakers. */
        SideBackRight = SideRight,    /* For use with FMOD_SPEAKERMODE_7POINT1 on PS3 where the extra speakers are surround back inside of side speakers. */
    }

	/// <summary>
	/// These are plugin types defined for use with System.GetNumPlugins, System.GetPluginInfo and System.UnloadPlugin.
	/// </summary>
    public enum PluginType
    {
        Output,     /* The plugin type is an output module.  FMOD mixed audio will play through one of these devices */
        FileFormatCodec,      /* The plugin type is a file format codec.  FMOD will use these codecs to load file formats for playback. */
        DigitalSignalProcessor         /* The plugin type is a DSP unit.  FMOD will use these plugins as part of its DSP network to apply effects to output or generate sound in realtime. */
    }

	/// <summary>
	/// Use these with System.Initialise to change various behaviors.
	/// </summary>
	[Flags]
    public enum InitalisationFlags
    {
        Normal = 0,
        StreamFromUpdate = 1, // No stream thread is created internally. Streams are driven from System.Update. Mainly used with non-realtime outputs.
        RightHanded3DSystem = 2, // FMOD will treat +X as left, +Y as up and +Z as forwards.
        SoftwareMixerDisabled = 4, // Disable software mixer to save memory.  Anything created with FMOD_SOFTWARE will fail and DSP will not work. */
        Occlusion = 8,   /* All platforms - All FMOD_SOFTWARE with FMOD_3D based voices will add a software lowpass filter effect into the DSP chain which is automatically used when Channel::set3DOcclusion is used or the geometry API. */
        HRTF = 0x10,   /* All platforms - All FMOD_SOFTWARE with FMOD_3D based voices will add a software lowpass filter effect into the DSP chain which causes sounds to sound duller when the sound goes behind the listener. */
		EnableProfile = 0x20,   /* All platforms - Enable TCP/IP based host which allows "DSPNet Listener.exe" to connect to it, and view the DSP dataflow network graph in real-time. */
		LowMemoryReverb = 0x40,   /* All platforms - SFX reverb is run using 22/24khz delay buffers, halving the memory required. */
        VolumeZeroBecomesVirtual = 0x80,   /* All platforms - Any sounds that are 0 volume will go virtual and not be processed except for having their positions updated virtually.  Use System::setAdvancedSettings to adjust what volume besides zero to switch to virtual at. */

		// Windows only
		WASAPIExclusive = 0x100,   /* Win32 Vista only - for WASAPI output - Enable exclusive access to hardware, lower latency at the expense of excluding other applications from accessing the audio hardware. */
        DirectSoundHRTFNone = 0x200,   /* Win32 only - for DirectSound output - FMOD_HARDWARE | FMOD_3D buffers use simple stereo panning/doppler/attenuation when 3D hardware acceleration is not present. */
        DirectSoundHRTFLight = 0x400,   /* Win32 only - for DirectSound output - FMOD_HARDWARE | FMOD_3D buffers use a slightly higher quality algorithm when 3D hardware acceleration is not present. */
        DirectSoundHRTFFull = 0x800,   /* Win32 only - for DirectSound output - FMOD_HARDWARE | FMOD_3D buffers use full quality 3D playback when 3d hardware acceleration is not present. */

		// PS2 only
		DisableCore0Reverb = 0x10000,   /* PS2 only - Disable reverb on CORE 0 to regain SRAM. */
        DisableCore1Reverb = 0x20000,   /* PS2 only - Disable reverb on CORE 1 to regain SRAM. */
        DoNotUseScratchpad = 0x40000,   /* PS2 only - Disable FMOD's usage of the scratchpad. */
        SwitchDMAChannels = 0x80000,   /* PS2 only - Changes FMOD from using SPU DMA channel 0 for software mixing, and 1 for sound data upload/file streaming, to 1 and 0 respectively. */

		// XBox only
		RemoveHeadRoom = 0x100000,   /* XBox only - By default DirectSound attenuates all sound by 6db to avoid clipping/distortion.  CAUTION.  If you use this flag you are responsible for the final mix to make sure clipping / distortion doesn't happen. */
        DashboardMuteInsteadOfPause = 0x200000,   /* Xbox 360 only - The "music" channelgroup which by default pauses when custom 360 dashboard music is played, can be changed to mute (therefore continues playing) instead of pausing, by using this flag. */

		SyncMixerWithUpdate = 0x400000,   /* Win32/Wii/PS3/Xbox/Xbox 360 - FMOD Mixer thread is woken up to do a mix when System::update is called rather than waking periodically on its own timer. */
        NeuralTHX = 0x2000000    /* Win32/Mac/Linux/Solaris - Use Neural THX downmixing from 7.1 if speakermode set to FMOD_SPEAKERMODE_STEREO or FMOD_SPEAKERMODE_5POINT1. */
    }

	/// <summary>
	/// Describes the type of song being played.
	/// </summary>
	/// <seealso cref="Sound.GetFormat"/>
    public enum SoundType
    {
        Unknown,         /* 3rd party / unknown plugin format. */
        AAC,             /* AAC.  Currently unsupported. */
        AIFF,            /* AIFF. */
        ASF,             /* Microsoft Advanced Systems Format (ie WMA/ASF/WMV). */
        AT3,             /* Sony ATRAC 3 format */
        CDDA,            /* Digital CD audio. */
        DLS,             /* Sound font / downloadable sound bank. */
        FLAC,            /* FLAC lossless codec. */
        FSB,             /* FMOD Sample Bank. */
        GCADPCM,         /* GameCube ADPCM */
        IT,              /* Impulse Tracker. */
        MIDI,            /* MIDI. */
        MOD,             /* Protracker / Fasttracker MOD. */
        MPEG,            /* MP2/MP3 MPEG. */
        OggVorbis,       /* Ogg vorbis. */
        Playlist,        /* Information only from ASX/PLS/M3U/WAX playlists */
        Raw,             /* Raw PCM data. */
        S3M,             /* ScreamTracker 3. */
        SF2,             /* Sound font 2 format. */
        User,            /* User created sound. */
        WAV,             /* Microsoft WAV. */
        XM,              /* FastTracker 2 XM. */
        XMA,             /* Xbox360 XMA */
        VAG              /* PlayStation 2 / PlayStation Portable adpcm VAG format. */
    }

	/// <summary>
	/// The native format of the hardware or software buffer that will be used.
	/// </summary>
    public enum SoundFormat
    {
        None,     /* Unitialized / unknown */
        PCM8,     /* 8bit integer PCM data */
        PCM16,    /* 16bit integer PCM data  */
        PCM24,    /* 24bit integer PCM data  */
        PCM32,    /* 32bit integer PCM data  */
        PCMFloat, /* 32bit floating point PCM data  */
        GCADPCM,  /* Compressed GameCube DSP data */
        IMAADPCM, /* Compressed XBox ADPCM data */
        VAG,      /* Compressed PlayStation 2 ADPCM data */
        XMA,      /* Compressed Xbox360 data. */
        MPEG,     /* Compressed MPEG layer 2 or 3 data. */
        Maximum       /* Maximum number of sound formats supported. */ 
    }


    /*
    [DEFINE]
    [
        [NAME] 
        FMOD_MODE

        [DESCRIPTION]   
        Sound description bitfields, bitwise OR them together for loading and describing sounds.

        [REMARKS]
        By default a sound will open as a static sound that is decompressed fully into memory.<br>
        To have a sound stream instead, use FMOD_CREATESTREAM.<br>
        Some opening modes (ie FMOD_OPENUSER, FMOD_OPENMEMORY, FMOD_OPENRAW) will need extra information.<br>
        This can be provided using the FMOD_CREATESOUNDEXINFO structure.

        [PLATFORMS]
        Win32, Win64, Linux, Linux64, Macintosh, Xbox, Xbox360, PlayStation 2, GameCube, PlayStation Portable, PlayStation 3, Wii

        [SEE_ALSO]
        System::createSound
        System::createStream
        Sound::setMode
        Sound::getMode
        Channel::setMode
        Channel::getMode
        Sound::set3DCustomRolloff
        Channel::set3DCustomRolloff
    ]
    */
	[Flags]
    public enum Mode :uint
    {
		Default = 0, //Loop off, 2D, Hardware.
		NoLoop = 1, //Overrides Loop and LoopBidi.
		Loop = 2,
		LoopBidi = 4, //Loop back and forth. Only works on software-mixed static sounds.
		TwoD = 8,
		ThreeD = 0x10, //Makes the sound positionable in 3D. Overrides TwoD.
		Hardware = 0x20,
		Software = 0x40, //Overrides Hardware. Use this for FFT, DSP, 2D multispeaker support and other software-related features.
		CreateStream = 0x80, //Decompress at runtime, streaming from the source provided. Overrides CreateSample.
		CreateSample = 0x100, //Decompress at loadtime, decompressing the whole file into memory.
		CreateCompressedSample = 0x200, //Load MP3 or such into memory and leave it compressed. The sample is decoded during playback. Only works in combination with Software.
		OpenUser = 0x400, //Opens a user created static sample or stream.
		OpenMemory = 0x800, //Name will be interpreted as a pointer to memory instead of a filename.
		OpenRaw = 0x1000, //Ignores the file format and treats it as raw PCM. User may need to declare signedness.
		OpenOnly = 0x2000, //Just open the file but don't prebuffer or read yet. Good for fast opens for info.
		AccurateTime = 0x4000, //For accurate Sound.GetLength and Channel.SetPosition calls on VBR MP3 or AAC files and sequenced formats. Scans the file first, so load times increase.
		MPegSearch = 0x8000, //For corrupted MP3 files. This will search all the way through the file until it hits a valid MPeg header, instead of only up to 4k.
		NonBlocking = 0x10000, //Use Sound.GetOpenState to see if it's done.
		Unique = 0x20000, //Marks the sound so it can only be played once at a time.

		ThreeDHeadRelative = 0x40000, //Makes the sound's position, velocity and orientation relative to the listener.
		ThreeDWorldRelative = 0x80000, //Makes the sound's position, velocity and orientation absolute (relative to the world). Default.
		ThreeDLogRolloff = 0x100000, //This sound will follow the standard logarithmic rolloff model.
		ThreeDLinRolloff = 0x200000, //This sound will follow a linear rolloff model.
		ThreeDCustomRolloff = 0x4000000, //This sound will follow a rolloff model defined by Sound.Set3DCustomRolloff/Channel.Set3DCustomRolloff.
		ThreeDIgnoreGeometry = 0x40000000, //This sound is not affected by geometry occlusion.

		CDDAForceASPI = 0x400000, //For CD Audio only. Use ASPI instead of NTSCSI to access the specified CD/DVD device.
		CDDAJitterCorrect = 0x800000, //For CD Audio only. Perform jitter correction. This helps produce a more accurate CDDA stream at the cost of more CPU time.

		Unicode = 0x1000000, //Filename is UTF16.
		IgnoreTags = 0x2000000, //Skips ID3v2/ASF/etc tag checks when opening a sound to reduce seak/read overhead.

		LowMemory = 0x8000000, //Removes some features from samples to give a lower memory overhead, like Sound.GetName.
		LoadInSecondaryRAM = 0x20000000, //Load sound into the secondary RAM of supported platform. On the PlayStation 3, sounds will be loaded into RSX/VRAM.
		VirtualPlayFromStart = 0x80000000, //For sounds that start virtual (due to being quiet or low importance), instead of swapping back to audible, and playing at the correct offset according to time, this flag makes the sound play from the start.
    }


    /*
    [ENUM]
    [
        [DESCRIPTION]   
        These values describe what state a sound is in after NONBLOCKING has been used to open it.

        [REMARKS]    

        [PLATFORMS]
        Win32, Win64, Linux, Linux64, Macintosh, Xbox, Xbox360, PlayStation 2, GameCube, PlayStation Portable, PlayStation 3, Wii

        [SEE_ALSO]
        Sound::getOpenState
        MODE
    ]
    */
    public enum OPENSTATE
    {
        READY = 0,       /* Opened and ready to play */
        LOADING,         /* Initial load in progress */
        ERROR,           /* Failed to open - file not found, out of memory etc.  See return value of Sound::getOpenState for what happened. */
        CONNECTING,      /* Connecting to remote host (internet sounds only) */
        BUFFERING,       /* Buffering data */
        SEEKING,         /* Seeking to subsound and re-flushing stream buffer. */
        STREAMING,       /* Ready and playing, but not possible to release at this time without stalling the main thread. */
        SETPOSITION,     /* Seeking within a stream to a different position. */
    }


    /*
    [ENUM]
    [
        [DESCRIPTION]   
        These flags are used with SoundGroup::setMaxAudibleBehavior to determine what happens when more sounds 
        are played than are specified with SoundGroup::setMaxAudible.

        [REMARKS]
        When using FMOD_SOUNDGROUP_BEHAVIOR_MUTE, SoundGroup::setMuteFadeSpeed can be used to stop a sudden transition.  
        Instead, the time specified will be used to cross fade between the sounds that go silent and the ones that become audible.

        [PLATFORMS]
        Win32, Win64, Linux, Linux64, Macintosh, Xbox, Xbox360, PlayStation 2, GameCube, PlayStation Portable, PlayStation 3, Wii

        [SEE_ALSO]      
        SoundGroup::setMaxAudibleBehavior
        SoundGroup::getMaxAudibleBehavior
        SoundGroup::setMaxAudible
        SoundGroup::getMaxAudible
        SoundGroup::setMuteFadeSpeed
        SoundGroup::getMuteFadeSpeed
    ]
    */
    public enum SOUNDGROUP_BEHAVIOR
    {
        BEHAVIOR_FAIL,              /* Any sound played that puts the sound count over the SoundGroup::setMaxAudible setting, will simply fail during System::playSound. */
        BEHAVIOR_MUTE,              /* Any sound played that puts the sound count over the SoundGroup::setMaxAudible setting, will be silent, then if another sound in the group stops the sound that was silent before becomes audible again. */
        BEHAVIOR_STEALLOWEST        /* Any sound played that puts the sound count over the SoundGroup::setMaxAudible setting, will steal the quietest / least important sound playing in the group. */
    }

    /*
    [ENUM]
    [
        [DESCRIPTION]   
        These callback types are used with System::setCallback.

        [REMARKS]
        Each callback has commanddata parameters passed as int unique to the type of callback.<br>
        See reference to FMOD_SYSTEM_CALLBACK to determine what they might mean for each type of callback.<br>
        <br>
        <b>Note!</b>  Currently the user must call System::update for these callbacks to trigger!

        [PLATFORMS]
        Win32, Win64, Linux, Linux64, Macintosh, Xbox, Xbox360, PlayStation 2, GameCube, PlayStation Portable, PlayStation 3, Wii, Solaris

        [SEE_ALSO]      
        System::setCallback
        FMOD_SYSTEM_CALLBACK
        System::update
    ]
    */
    public enum SYSTEM_CALLBACKTYPE
    {
        DEVICELISTCHANGED,      /* Called when the enumerated list of devices has changed. */
        MEMORYALLOCATIONFAILED, /* Called directly when a memory allocation fails somewhere in FMOD. */
        THREADCREATED,          /* Called directly when a thread is created. */
        BADDSPCONNECTION,       /* Called when a bad connection was made with DSP::addInput. Usually called from mixer thread because that is where the connections are made.  */
        BADDSPLEVEL,            /* Called when too many effects were added exceeding the maximum tree depth of 128.  This is most likely caused by accidentally adding too many DSP effects. Usually called from mixer thread because that is where the connections are made.  */

        MAX                     /* Maximum number of callback types supported. */
    }

    /*
    [ENUM]
    [
        [DESCRIPTION]   
        These callback types are used with Channel::setCallback.

        [REMARKS]
        Each callback has commanddata parameters passed int unique to the type of callback.
        See reference to FMOD_CHANNEL_CALLBACK to determine what they might mean for each type of callback.

        [PLATFORMS]
        Win32, Win64, Linux, Linux64, Macintosh, Xbox, Xbox360, PlayStation 2, GameCube, PlayStation Portable, PlayStation 3, Wii

        [SEE_ALSO]      
        Channel::setCallback
        FMOD_CHANNEL_CALLBACK
    ]
    */
    public enum CHANNEL_CALLBACKTYPE
    {
        END,                  /* Called when a sound ends. */
        VIRTUALVOICE,         /* Called when a voice is swapped out or swapped in. */
        SYNCPOINT,            /* Called when a syncpoint is encountered.  Can be from wav file markers. */
        OCCLUSION,            /* Called when the channel has its geometry occlusion value calculated.  Can be used to clamp or change the value. */

        MAX
    }


    /* 
        FMOD Callbacks
    */
    public delegate Result SYSTEM_CALLBACK          (IntPtr systemraw, SYSTEM_CALLBACKTYPE type, IntPtr commanddata1, IntPtr commanddata2);

    public delegate Result CHANNEL_CALLBACK         (IntPtr channelraw, CHANNEL_CALLBACKTYPE type, IntPtr commanddata1, IntPtr commanddata2);

    public delegate Result SOUND_NONBLOCKCALLBACK   (IntPtr soundraw, Result Result);
    public delegate Result SOUND_PCMREADCALLBACK    (IntPtr soundraw, IntPtr data, uint datalen);
    public delegate Result SOUND_PCMSETPOSCALLBACK  (IntPtr soundraw, int subsound, uint position, TIMEUNIT postype);

    public delegate Result FILE_OPENCALLBACK        ([MarshalAs(UnmanagedType.LPWStr)]string name, int unicode, ref uint filesize, ref IntPtr handle, ref IntPtr userdata);
    public delegate Result FILE_CLOSECALLBACK       (IntPtr handle, IntPtr userdata);
    public delegate Result FILE_READCALLBACK        (IntPtr handle, IntPtr buffer, uint sizebytes, ref uint bytesread, IntPtr userdata);
    public delegate Result FILE_SEEKCALLBACK        (IntPtr handle, int pos, IntPtr userdata);

    public delegate float  CB_3D_ROLLOFFCALLBACK    (IntPtr channelraw, float distance);

    /*
    [ENUM]
    [
        [DESCRIPTION]   
        List of windowing methods used in spectrum analysis to reduce leakage / transient signals intefering with the analysis.
        This is a problem with analysis of continuous signals that only have a small portion of the signal sample (the fft window size).
        Windowing the signal with a curve or triangle tapers the sides of the fft window to help alleviate this problem.

        [REMARKS]
        Cyclic signals such as a sine wave that repeat their cycle in a multiple of the window size do not need windowing.
        I.e. If the sine wave repeats every 1024, 512, 256 etc samples and the FMOD fft window is 1024, then the signal would not need windowing.
        Not windowing is the same as FMOD_DSP_FFT_WINDOW_RECT, which is the default.
        If the cycle of the signal (ie the sine wave) is not a multiple of the window size, it will cause frequency abnormalities, so a different windowing method is needed.
        <exclude>
        
        FMOD_DSP_FFT_WINDOW_RECT.
        <img src = "rectangle.gif"></img>
        
        FMOD_DSP_FFT_WINDOW_TRIANGLE.
        <img src = "triangle.gif"></img>
        
        FMOD_DSP_FFT_WINDOW_HAMMING.
        <img src = "hamming.gif"></img>
        
        FMOD_DSP_FFT_WINDOW_HANNING.
        <img src = "hanning.gif"></img>
        
        FMOD_DSP_FFT_WINDOW_BLACKMAN.
        <img src = "blackman.gif"></img>
        
        FMOD_DSP_FFT_WINDOW_BLACKMANHARRIS.
        <img src = "blackmanharris.gif"></img>
        </exclude>

        [PLATFORMS]
        Win32, Win64, Linux, Linux64, Macintosh, Xbox, Xbox360, PlayStation 2, GameCube, PlayStation Portable, PlayStation 3, Wii

        [SEE_ALSO]      
        System::getSpectrum
        Channel::getSpectrum
    ]
    */
    public enum DSP_FFT_WINDOW
    {
        RECT,           /* w[n] = 1.0                                                                                            */
        TRIANGLE,       /* w[n] = TRI(2n/N)                                                                                      */
        HAMMING,        /* w[n] = 0.54 - (0.46 * COS(n/N) )                                                                      */
        HANNING,        /* w[n] = 0.5 *  (1.0  - COS(n/N) )                                                                      */
        BLACKMAN,       /* w[n] = 0.42 - (0.5  * COS(n/N) ) + (0.08 * COS(2.0 * n/N) )                                           */
        BLACKMANHARRIS, /* w[n] = 0.35875 - (0.48829 * COS(1.0 * n/N)) + (0.14128 * COS(2.0 * n/N)) - (0.01168 * COS(3.0 * n/N)) */

        MAX
    }


    /*
    [ENUM]
    [
        [DESCRIPTION]   
        List of interpolation types that the FMOD Ex software mixer supports.  

        [REMARKS]
        The default resampler type is FMOD_DSP_RESAMPLER_LINEAR.<br>
        Use System::setSoftwareFormat to tell FMOD the resampling quality you require for FMOD_SOFTWARE based sounds.

        [PLATFORMS]
        Win32, Win64, Linux, Linux64, Macintosh, Xbox, Xbox360, PlayStation 2, GameCube, PlayStation Portable, PlayStation 3, Wii

        [SEE_ALSO]      
        System::setSoftwareFormat
        System::getSoftwareFormat
    ]
    */
    public enum DSP_RESAMPLER
    {
        NOINTERP,        /* No interpolation.  High frequency aliasing hiss will be audible depending on the sample rate of the sound. */
        LINEAR,          /* Linear interpolation (default method).  Fast and good quality, causes very slight lowpass effect on low frequency sounds. */
        CUBIC,           /* Cubic interoplation.  Slower than linear interpolation but better quality. */
        SPLINE,          /* 5 point spline interoplation.  Slowest resampling method but best quality. */

        MAX,             /* Maximum number of resample methods supported. */
    }


    /*
    [ENUM]
    [
        [DESCRIPTION]   
        List of tag types that could be stored within a sound.  These include id3 tags, metadata from netstreams and vorbis/asf data.

        [REMARKS]

        [PLATFORMS]
        Win32, Win64, Linux, Linux64, Macintosh, Xbox, Xbox360, PlayStation 2, GameCube, PlayStation Portable, PlayStation 3, Wii

        [SEE_ALSO]      
        Sound::getTag
    ]
    */
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


    /*
    [ENUM]
    [
        [DESCRIPTION]   
        List of data types that can be returned by Sound::getTag

        [REMARKS]

        [PLATFORMS]
        Win32, Win64, Linux, Linux64, Macintosh, Xbox, Xbox360, PlayStation 2, GameCube, PlayStation Portable, PlayStation 3, Wii

        [SEE_ALSO]      
        Sound::getTag
    ]
    */
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

    /*
    [ENUM]
    [
        [DESCRIPTION]   
        Types of delay that can be used with Channel::setDelay / Channel::getDelay.

        [REMARKS]
        If you haven't called Channel::setDelay yet, if you call Channel::getDelay with FMOD_DELAYTYPE_DSPCLOCK_START it will return the 
        equivalent global DSP clock value to determine when a channel started, so that you can use it for other channels to sync against.<br>
        <br>
        Use System::getDSPClock to also get the current dspclock time, a base for future calls to Channel::setDelay.<br>
        <br>
        Use FMOD_64BIT_ADD or FMOD_64BIT_SUB to add a hi/lo combination together and cope with wraparound.
        <br>
        If FMOD_DELAYTYPE_END_MS is specified, the value is not treated as a 64 bit number, just the delayhi value is used and it is treated as milliseconds.

        [PLATFORMS]
        Win32, Win64, Linux, Linux64, Macintosh, Xbox, Xbox360, PlayStation 2, GameCube, PlayStation Portable, PlayStation 3, Wii, Solaris

        [SEE_ALSO]      
        Channel::setDelay
        Channel::getDelay
        System::getDSPClock
    ]
    */
    public enum DELAYTYPE
    {
        END_MS,              /* Delay at the end of the sound in milliseconds.  Use delayhi only.   Channel::isPlaying will remain true until this delay has passed even though the sound itself has stopped playing.*/
        DSPCLOCK_START,      /* Time the sound started if Channel::getDelay is used, or if Channel::setDelay is used, the sound will delay playing until this exact tick. */
        DSPCLOCK_END,        /* Time the sound should end. If this is non-zero, the channel will go silent at this exact tick. */
        DSPCLOCK_PAUSE,      /* Time the sound should pause. If this is non-zero, the channel will pause at this exact tick. */

        MAX                  /* Maximum number of tag datatypes supported. */
    }

    public class DELAYTYPE_UTILITY
    {
        void FMOD_64BIT_ADD(ref uint hi1, ref uint lo1, uint hi2, uint lo2)
        {
            hi1 += (uint)((hi2) + ((((lo1) + (lo2)) < (lo1)) ? 1 : 0));
            lo1 += (lo2);
        }

        void FMOD_64BIT_SUB(ref uint hi1, ref uint lo1, uint hi2, uint lo2)
        {
            hi1 -= (uint)((hi2) + ((((lo1) - (lo2)) > (lo1)) ? 1 : 0));
            lo1 -= (lo2);
        }
    }


    /*
    [STRUCTURE] 
    [
        [DESCRIPTION]   
        Structure describing a piece of tag data.

        [REMARKS]
        Members marked with [in] mean the user sets the value before passing it to the function.
        Members marked with [out] mean FMOD sets the value to be used after the function exits.

        [PLATFORMS]
        Win32, Win64, Linux, Linux64, Macintosh, Xbox, Xbox360, PlayStation 2, GameCube, PlayStation Portable, PlayStation 3, Wii

        [SEE_ALSO]      
        Sound::getTag
        TAGTYPE
        TAGDATATYPE
    ]
    */
    [StructLayout(LayoutKind.Sequential,CharSet=CharSet.Ansi)]
    public struct TAG
    {
        public TAGTYPE           type;         /* [out] The type of this tag. */
        public TAGDATATYPE       datatype;     /* [out] The type of data that this tag contains */
        public string            name;         /* [out] The name of this tag i.e. "TITLE", "ARTIST" etc. */
        public IntPtr            data;         /* [out] Pointer to the tag data - its format is determined by the datatype member */
        public uint              datalen;      /* [out] Length of the data contained in this tag */
        public bool              updated;      /* [out] True if this tag has been updated since last being accessed with Sound::getTag */
    }


    /*
    [STRUCTURE] 
    [
        [DESCRIPTION]   
        Structure describing a CD/DVD table of contents

        [REMARKS]
        Members marked with [in] mean the user sets the value before passing it to the function.
        Members marked with [out] mean FMOD sets the value to be used after the function exits.

        [PLATFORMS]
        Win32, Win64, Linux, Linux64, Macintosh, Xbox, Xbox360, PlayStation 2, GameCube, PlayStation Portable, PlayStation 3, Wii

        [SEE_ALSO]      
        Sound::getTag
    ]
    */
    [StructLayout(LayoutKind.Sequential)]
    public struct CDTOC
    {
        public int numtracks;                  /* [out] The number of tracks on the CD */
        [MarshalAs(UnmanagedType.ByValArray,SizeConst=100)]
        public int[] min;                   /* [out] The start offset of each track in minutes */
        [MarshalAs(UnmanagedType.ByValArray,SizeConst=100)]
        public int[] sec;                   /* [out] The start offset of each track in seconds */
        [MarshalAs(UnmanagedType.ByValArray,SizeConst=100)]
        public int[] frame;                 /* [out] The start offset of each track in frames */
    }


    /*
    [ENUM]
    [
        [DESCRIPTION]   
        List of time types that can be returned by Sound::getLength and used with Channel::setPosition or Channel::getPosition.

        [REMARKS]

        [PLATFORMS]
        Win32, Win64, Linux, Linux64, Macintosh, Xbox, Xbox360, PlayStation 2, GameCube, PlayStation Portable, PlayStation 3, Wii

        [SEE_ALSO]      
        Sound::getLength
        Channel::setPosition
        Channel::getPosition
    ]
    */
    public enum TIMEUNIT
    {
        MS                = 0x00000001,  /* Milliseconds. */
        PCM               = 0x00000002,  /* PCM Samples, related to milliseconds * samplerate / 1000. */
        PCMBYTES          = 0x00000004,  /* Bytes, related to PCM samples * channels * datawidth (ie 16bit = 2 bytes). */
        RAWBYTES          = 0x00000008,  /* Raw file bytes of (compressed) sound data (does not include headers).  Only used by Sound::getLength and Channel::getPosition. */
        MODORDER          = 0x00000100,  /* MOD/S3M/XM/IT.  Order in a sequenced module format.  Use Sound::getFormat to determine the format. */
        MODROW            = 0x00000200,  /* MOD/S3M/XM/IT.  Current row in a sequenced module format.  Sound::getLength will return the number if rows in the currently playing or seeked to pattern. */
        MODPATTERN        = 0x00000400,  /* MOD/S3M/XM/IT.  Current pattern in a sequenced module format.  Sound::getLength will return the number of patterns in the song and Channel::getPosition will return the currently playing pattern. */
        SENTENCE_MS       = 0x00010000,  /* Currently playing subsound in a sentence time in milliseconds. */
        SENTENCE_PCM      = 0x00020000,  /* Currently playing subsound in a sentence time in PCM Samples, related to milliseconds * samplerate / 1000. */
        SENTENCE_PCMBYTES = 0x00040000,  /* Currently playing subsound in a sentence time in bytes, related to PCM samples * channels * datawidth (ie 16bit = 2 bytes). */
        SENTENCE          = 0x00080000,  /* Currently playing sentence index according to the channel. */
        SENTENCE_SUBSOUND = 0x00100000,  /* Currently playing subsound index in a sentence. */
        BUFFERED          = 0x10000000,  /* Time value as seen by buffered stream.  This is always ahead of audible time, and is only used for processing. */
    }


    /*
    [ENUM]
    [
        [DESCRIPTION]
        When creating a multichannel sound, FMOD will pan them to their default speaker locations, for example a 6 channel sound will default to one channel per 5.1 output speaker.<br>
        Another example is a stereo sound.  It will default to left = front left, right = front right.<br>
        <br>
        This is for sounds that are not 'default'.  For example you might have a sound that is 6 channels but actually made up of 3 stereo pairs, that should all be located in front left, front right only.

        [REMARKS]
        For full flexibility of speaker assignments, use Channel::setSpeakerLevels.  This functionality is cheaper, uses less memory and easier to use.

        [PLATFORMS]
        Win32, Win64, Linux, Linux64, Macintosh, Xbox, Xbox360, PlayStation 2, GameCube, PlayStation Portable, PlayStation 3, Wii

        [SEE_ALSO]
        FMOD_CREATESOUNDEXINFO
        Channel::setSpeakerLevels
    ]
    */
    public enum SPEAKERMAPTYPE
    {
        DEFAULT,     /* This is the default, and just means FMOD decides which speakers it puts the source channels. */
        ALLMONO,     /* This means the sound is made up of all mono sounds.  All voices will be panned to the front center by default in this case.  */
        ALLSTEREO,   /* This means the sound is made up of all stereo sounds.  All voices will be panned to front left and front right alternating every second channel.  */
        _51_PROTOOLS /* Map a 5.1 sound to use protools L C R Ls Rs LFE mapping.  Will return an error if not a 6 channel sound. */
    }


    /*
    [STRUCTURE] 
    [
        [DESCRIPTION]
        Use this structure with System::createSound when more control is needed over loading.
        The possible reasons to use this with System::createSound are:
        <li>Loading a file from memory.
        <li>Loading a file from within another larger (possibly wad/pak) file, by giving the loader an offset and length.
        <li>To create a user created / non file based sound.
        <li>To specify a starting subsound to seek to within a multi-sample sounds (ie FSB/DLS/SF2) when created as a stream.
        <li>To specify which subsounds to load for multi-sample sounds (ie FSB/DLS/SF2) so that memory is saved and only a subset is actually loaded/read from disk.
        <li>To specify 'piggyback' read and seek callbacks for capture of sound data as fmod reads and decodes it.  Useful for ripping decoded PCM data from sounds as they are loaded / played.
        <li>To specify a MIDI DLS/SF2 sample set file to load when opening a MIDI file.
        See below on what members to fill for each of the above types of sound you want to create.

        [REMARKS]
        This structure is optional!  Specify 0 or NULL in System::createSound if you don't need it!
        
        Members marked with [in] mean the user sets the value before passing it to the function.
        Members marked with [out] mean FMOD sets the value to be used after the function exits.
        
        <u>Loading a file from memory.</u>
        <li>Create the sound using the FMOD_OPENMEMORY flag.
        <li>Mandantory.  Specify 'length' for the size of the memory block in bytes.
        <li>Other flags are optional.
        
        
        <u>Loading a file from within another larger (possibly wad/pak) file, by giving the loader an offset and length.</u>
        <li>Mandantory.  Specify 'fileoffset' and 'length'.
        <li>Other flags are optional.
        
        
        <u>To create a user created / non file based sound.</u>
        <li>Create the sound using the FMOD_OPENUSER flag.
        <li>Mandantory.  Specify 'defaultfrequency, 'numchannels' and 'format'.
        <li>Other flags are optional.
        
        
        <u>To specify a starting subsound to seek to and flush with, within a multi-sample stream (ie FSB/DLS/SF2).</u>
        
        <li>Mandantory.  Specify 'initialsubsound'.
        
        
        <u>To specify which subsounds to load for multi-sample sounds (ie FSB/DLS/SF2) so that memory is saved and only a subset is actually loaded/read from disk.</u>
        
        <li>Mandantory.  Specify 'inclusionlist' and 'inclusionlistnum'.
        
        
        <u>To specify 'piggyback' read and seek callbacks for capture of sound data as fmod reads and decodes it.  Useful for ripping decoded PCM data from sounds as they are loaded / played.</u>
        
        <li>Mandantory.  Specify 'pcmreadcallback' and 'pcmseekcallback'.
        
        
        <u>To specify a MIDI DLS/SF2 sample set file to load when opening a MIDI file.</u>
        
        <li>Mandantory.  Specify 'dlsname'.
        
        
        Setting the 'decodebuffersize' is for cpu intensive codecs that may be causing stuttering, not file intensive codecs (ie those from CD or netstreams) which are normally altered with System::setStreamBufferSize.  As an example of cpu intensive codecs, an mp3 file will take more cpu to decode than a PCM wav file.
        If you have a stuttering effect, then it is using more cpu than the decode buffer playback rate can keep up with.  Increasing the decode buffersize will most likely solve this problem.

        [PLATFORMS]
        Win32, Win64, Linux, Linux64, Macintosh, Xbox, Xbox360, PlayStation 2, GameCube, PlayStation Portable, PlayStation 3, Wii

        [SEE_ALSO]
        System::createSound
        System::setStreamBufferSize
        FMOD_MODE
    ]
    */
    [StructLayout(LayoutKind.Sequential)]
    public struct CREATESOUNDEXINFO
    {
        public int                         cbsize;                 /* [in] Size of this structure.  This is used so the structure can be expanded in the future and still work on older versions of FMOD Ex. */
        public uint                        length;                 /* [in] Optional. Specify 0 to ignore. Size in bytes of file to load, or sound to create (in this case only if FMOD_OPENUSER is used).  Required if loading from memory.  If 0 is specified, then it will use the size of the file (unless loading from memory then an error will be returned). */
        public uint                        fileoffset;             /* [in] Optional. Specify 0 to ignore. Offset from start of the file to start loading from.  This is useful for loading files from inside big data files. */
        public int                         numchannels;            /* [in] Optional. Specify 0 to ignore. Number of channels in a sound specified only if OPENUSER is used. */
        public int                         defaultfrequency;       /* [in] Optional. Specify 0 to ignore. Default frequency of sound in a sound specified only if OPENUSER is used.  Other formats use the frequency determined by the file format. */
        public SoundFormat                format;                 /* [in] Optional. Specify 0 or SOUND_FORMAT_NONE to ignore. Format of the sound specified only if OPENUSER is used.  Other formats use the format determined by the file format.   */
        public uint                        decodebuffersize;       /* [in] Optional. Specify 0 to ignore. For streams.  This determines the size of the double buffer (in PCM samples) that a stream uses.  Use this for user created streams if you want to determine the size of the callback buffer passed to you.  Specify 0 to use FMOD's default size which is currently equivalent to 400ms of the sound format created/loaded. */
        public int                         initialsubsound;        /* [in] Optional. Specify 0 to ignore. In a multi-sample file format such as .FSB/.DLS/.SF2, specify the initial subsound to seek to, only if CREATESTREAM is used. */
        public int                         numsubsounds;           /* [in] Optional. Specify 0 to ignore or have no subsounds.  In a user created multi-sample sound, specify the number of subsounds within the sound that are accessable with Sound::getSubSound / SoundGetSubSound. */
        public IntPtr                      inclusionlist;          /* [in] Optional. Specify 0 to ignore. In a multi-sample format such as .FSB/.DLS/.SF2 it may be desirable to specify only a subset of sounds to be loaded out of the whole file.  This is an array of subsound indicies to load into memory when created. */
        public int                         inclusionlistnum;       /* [in] Optional. Specify 0 to ignore. This is the number of integers contained within the */
        public SOUND_PCMREADCALLBACK       pcmreadcallback;        /* [in] Optional. Specify 0 to ignore. Callback to 'piggyback' on FMOD's read functions and accept or even write PCM data while FMOD is opening the sound.  Used for user sounds created with OPENUSER or for capturing decoded data as FMOD reads it. */
        public SOUND_PCMSETPOSCALLBACK     pcmsetposcallback;      /* [in] Optional. Specify 0 to ignore. Callback for when the user calls a seeking function such as Channel::setPosition within a multi-sample sound, and for when it is opened.*/
        public SOUND_NONBLOCKCALLBACK      nonblockcallback;       /* [in] Optional. Specify 0 to ignore. Callback for successful completion, or error while loading a sound that used the FMOD_NONBLOCKING flag.*/
        public string                      dlsname;                /* [in] Optional. Specify 0 to ignore. Filename for a DLS or SF2 sample set when loading a MIDI file.   If not specified, on windows it will attempt to open /windows/system32/drivers/gm.dls, otherwise the MIDI will fail to open.  */
        public string                      encryptionkey;          /* [in] Optional. Specify 0 to ignore. Key for encrypted FSB file.  Without this key an encrypted FSB file will not load. */
        public int                         maxpolyphony;           /* [in] Optional. Specify 0 to ingore. For sequenced formats with dynamic channel allocation such as .MID and .IT, this specifies the maximum voice count allowed while playing.  .IT defaults to 64.  .MID defaults to 32. */
        public IntPtr                      userdata;               /* [in] Optional. Specify 0 to ignore. This is user data to be attached to the sound during creation.  Access via Sound::getUserData. */
        public SoundType                  suggestedsoundtype;     /* [in] Optional. Specify 0 or FMOD_SOUND_TYPE_UNKNOWN to ignore.  Instead of scanning all codec types, use this to speed up loading by making it jump straight to this codec. */
        public FILE_OPENCALLBACK           useropen;               /* [in] Optional. Specify 0 to ignore. Callback for opening this file. */
        public FILE_CLOSECALLBACK          userclose;              /* [in] Optional. Specify 0 to ignore. Callback for closing this file. */
        public FILE_READCALLBACK           userread;               /* [in] Optional. Specify 0 to ignore. Callback for reading from this file. */
        public FILE_SEEKCALLBACK           userseek;               /* [in] Optional. Specify 0 to ignore. Callback for seeking within this file. */
        public SPEAKERMAPTYPE              speakermap;             /* [in] Optional. Specify 0 to ignore. Use this to differ the way fmod maps multichannel sounds to speakers.  See FMOD_SPEAKERMAPTYPE for more. */
        public IntPtr                      initialsoundgroup;      /* [in] Optional. Specify 0 to ignore. Specify a sound group if required, to put sound in as it is created. */
        public uint                        initialseekposition;    /* [in] Optional. Specify 0 to ignore. For streams. Specify an initial position to seek the stream to. */
        public TIMEUNIT                    initialseekpostype;     /* [in] Optional. Specify 0 to ignore. For streams. Specify the time unit for the position set in initialseekposition. */
        public int                         ignoresetfilesystem;    /* [in] Optional. Specify 0 to ignore. Set to 1 to use fmod's built in file system. Ignores setFileSystem callbacks and also FMOD_CREATESOUNEXINFO file callbacks.  Useful for specific cases where you don't want to use your own file system but want to use fmod's file system (ie net streaming). */
    }


    /*
    [STRUCTURE] 
    [
        [DESCRIPTION]
        Structure defining a reverb environment.<br>
        <br>
        For more indepth descriptions of the reverb properties under win32, please see the EAX2 and EAX3
        documentation at http://developer.creative.com/ under the 'downloads' section.<br>
        If they do not have the EAX3 documentation, then most information can be attained from
        the EAX2 documentation, as EAX3 only adds some more parameters and functionality on top of 
        EAX2.

        [REMARKS]
        Note the default reverb properties are the same as the FMOD_PRESET_GENERIC preset.<br>
        Note that integer values that typically range from -10,000 to 1000 are represented in 
        decibels, and are of a logarithmic scale, not linear, wheras float values are always linear.<br>
        <br>
        The numerical values listed below are the maximum, minimum and default values for each variable respectively.<br>
        <br>
        <b>SUPPORTED</b> next to each parameter means the platform the parameter can be set on.  Some platforms support all parameters and some don't.<br>
        EAX   means hardware reverb on FMOD_OutputType_DSOUND on windows only (must use FMOD_HARDWARE), on soundcards that support EAX 1 to 4.<br>
        EAX4  means hardware reverb on FMOD_OutputType_DSOUND on windows only (must use FMOD_HARDWARE), on soundcards that support EAX 4.<br>
        I3DL2 means hardware reverb on FMOD_OutputType_DSOUND on windows only (must use FMOD_HARDWARE), on soundcards that support I3DL2 non EAX native reverb.<br>
        GC    means Nintendo Gamecube hardware reverb (must use FMOD_HARDWARE).<br>
        WII   means Nintendo Wii hardware reverb (must use FMOD_HARDWARE).<br>
        Xbox1 means the original Xbox hardware reverb (must use FMOD_HARDWARE).<br>
        PS2   means Playstation 2 hardware reverb (must use FMOD_HARDWARE).<br>
        SFX   means FMOD SFX software reverb.  This works on any platform that uses FMOD_SOFTWARE for loading sounds.<br>
        <br>
        Members marked with [in] mean the user sets the value before passing it to the function.<br>
        Members marked with [out] mean FMOD sets the value to be used after the function exits.<br>

        [PLATFORMS]
        Win32, Win64, Linux, Linux64, Macintosh, Xbox, Xbox360, PlayStation 2, GameCube, PlayStation Portable, PlayStation 3, Wii

        [SEE_ALSO]
        System::setReverbProperties
        System::getReverbProperties
        FMOD_REVERB_PRESETS
        FMOD_REVERB_FLAGS
    ]
    */
    [StructLayout(LayoutKind.Sequential)]
    public struct REVERB_PROPERTIES
    {                                   /*          MIN     MAX    DEFAULT   DESCRIPTION */
        public int   Instance;          /* [in]     0     , 2     , 0      , EAX4 only. Environment Instance. 3 seperate reverbs simultaneously are possible. This specifies which one to set. (win32 only) */
        public uint  Environment;       /* [in/out] 0     , 25    , 0      , sets all listener properties (win32/ps2) */
        public float EnvSize;           /* [in/out] 1.0   , 100.0 , 7.5    , environment size in meters (win32 only) */
        public float EnvDiffusion;      /* [in/out] 0.0   , 1.0   , 1.0    , environment diffusion (win32/xbox) */
        public int   Room;              /* [in/out] -10000, 0     , -1000  , room effect level (at mid frequencies) (win32/xbox) */
        public int   RoomHF;            /* [in/out] -10000, 0     , -100   , relative room effect level at high frequencies (win32/xbox) */
        public int   RoomLF;            /* [in/out] -10000, 0     , 0      , relative room effect level at low frequencies (win32 only) */
        public float DecayTime;         /* [in/out] 0.1   , 20.0  , 1.49   , reverberation decay time at mid frequencies (win32/xbox) */
        public float DecayHFRatio;      /* [in/out] 0.1   , 2.0   , 0.83   , high-frequency to mid-frequency decay time ratio (win32/xbox) */
        public float DecayLFRatio;      /* [in/out] 0.1   , 2.0   , 1.0    , low-frequency to mid-frequency decay time ratio (win32 only) */
        public int   Reflections;       /* [in/out] -10000, 1000  , -2602  , early reflections level relative to room effect (win32/xbox) */
        public float ReflectionsDelay;  /* [in/out] 0.0   , 0.3   , 0.007  , initial reflection delay time (win32/xbox) */
        [MarshalAs(UnmanagedType.ByValArray,SizeConst=3)]
        public float[] ReflectionsPan;  /* [in/out]       ,       , [0,0,0], early reflections panning Vector (win32 only) */
        public int   Reverb;            /* [in/out] -10000, 2000  , 200    , late reverberation level relative to room effect (win32/xbox) */
        public float ReverbDelay;       /* [in/out] 0.0   , 0.1   , 0.011  , late reverberation delay time relative to initial reflection (win32/xbox) */
        [MarshalAs(UnmanagedType.ByValArray,SizeConst=3)]
        public float[] ReverbPan;       /* [in/out]       ,       , [0,0,0], late reverberation panning Vector (win32 only) */
        public float EchoTime;          /* [in/out] .075  , 0.25  , 0.25   , echo time (win32 only) */
        public float EchoDepth;         /* [in/out] 0.0   , 1.0   , 0.0    , echo depth (win32 only) */
        public float ModulationTime;    /* [in/out] 0.04  , 4.0   , 0.25   , modulation time (win32 only) */
        public float ModulationDepth;   /* [in/out] 0.0   , 1.0   , 0.0    , modulation depth (win32 only) */
        public float AirAbsorptionHF;   /* [in/out] -100  , 0.0   , -5.0   , change in level per meter at high frequencies (win32 only) */
        public float HFReference;       /* [in/out] 1000.0, 20000 , 5000.0 , reference high frequency (hz) (win32/xbox) */
        public float LFReference;       /* [in/out] 20.0  , 1000.0, 250.0  , reference low frequency (hz) (win32 only) */
        public float RoomRolloffFactor; /* [in/out] 0.0   , 10.0  , 0.0    , like rolloffscale in System::set3DSettings but for reverb room size effect (win32/Xbox) */
        public float Diffusion;         /* [in/out] 0.0   , 100.0 , 100.0  , Value that controls the echo density in the late reverberation decay. (xbox only) */
        public float Density;           /* [in/out] 0.0   , 100.0 , 100.0  , Value that controls the modal density in the late reverberation decay (xbox only) */
        public uint  Flags;             /* [in/out] REVERB_FLAGS - modifies the behavior of above properties (win32/ps2) */

        #region wrapperinternal
        public REVERB_PROPERTIES(int instance, uint environment, float envSize, float envDiffusion, int room, int roomHF, int roomLF,
            float decayTime, float decayHFRatio, float decayLFRatio, int reflections, float reflectionsDelay,
            float reflectionsPanx, float reflectionsPany, float reflectionsPanz, int reverb, float reverbDelay,
            float reverbPanx, float reverbPany, float reverbPanz, float echoTime, float echoDepth, float modulationTime,
            float modulationDepth, float airAbsorptionHF, float hfReference, float lfReference, float roomRolloffFactor,
            float diffusion, float density, uint flags)
        {
            ReflectionsPan      = new float[3];
            ReverbPan           = new float[3];

            Instance            = instance;
            Environment         = environment;
            EnvSize             = envSize;
            EnvDiffusion        = envDiffusion;
            Room                = room;
            RoomHF              = roomHF;
            RoomLF              = roomLF;
            DecayTime           = decayTime;
            DecayHFRatio        = decayHFRatio;
            DecayLFRatio        = decayLFRatio;
            Reflections         = reflections;
            ReflectionsDelay    = reflectionsDelay;
            ReflectionsPan[0]   = reflectionsPanx;
            ReflectionsPan[1]   = reflectionsPany;
            ReflectionsPan[2]   = reflectionsPanz;
            Reverb              = reverb;
            ReverbDelay          = reverbDelay;
            ReverbPan[0]        = reverbPanx;
            ReverbPan[1]        = reverbPany;
            ReverbPan[2]        = reverbPanz;
            EchoTime            = echoTime;
            EchoDepth           = echoDepth;
            ModulationTime      = modulationTime;
            ModulationDepth     = modulationDepth;
            AirAbsorptionHF     = airAbsorptionHF;
            HFReference         = hfReference;
            LFReference         = lfReference;
            RoomRolloffFactor   = roomRolloffFactor;
            Diffusion           = diffusion;
            Density             = density;
            Flags               = flags;
        }
        #endregion
    }


    /*
    [DEFINE] 
    [
        [NAME] 
        REVERB_FLAGS

        [DESCRIPTION]
        Values for the Flags member of the REVERB_PROPERTIES structure.

        [PLATFORMS]
        Win32, Win64, Linux, Linux64, Macintosh, Xbox, Xbox360, PlayStation 2, GameCube, PlayStation Portable, PlayStation 3, Wii

        [SEE_ALSO]
        REVERB_PROPERTIES
    ]
    */
    [StructLayout(LayoutKind.Sequential)]
    public struct REVERB_FLAGS
    {
        public const uint DECAYTIMESCALE        = 0x00000001;   /* 'EnvSize' affects reverberation decay time */
        public const uint REFLECTIONSSCALE      = 0x00000002;   /* 'EnvSize' affects reflection level */
        public const uint REFLECTIONSDELAYSCALE = 0x00000004;   /* 'EnvSize' affects initial reflection delay time */
        public const uint REVERBSCALE           = 0x00000008;   /* 'EnvSize' affects reflections level */
        public const uint REVERBDELAYSCALE      = 0x00000010;   /* 'EnvSize' affects late reverberation delay time */
        public const uint DECAYHFLIMIT          = 0x00000020;   /* AirAbsorptionHF affects DecayHFRatio */
        public const uint ECHOTIMESCALE         = 0x00000040;   /* 'EnvSize' affects echo time */
        public const uint MODULATIONTIMESCALE   = 0x00000080;   /* 'EnvSize' affects modulation time */
        public const uint DEFAULT               = (DECAYTIMESCALE | 
            REFLECTIONSSCALE | 
            REFLECTIONSDELAYSCALE | 
            REVERBSCALE | 
            REVERBDELAYSCALE | 
            DECAYHFLIMIT);
    }


    /*
    [DEFINE] 
    [
    [NAME] 
    FMOD_REVERB_PRESETS

    [DESCRIPTION]   
    A set of predefined environment PARAMETERS, created by Creative Labs
    These are used to initialize an FMOD_REVERB_PROPERTIES structure statically.
    ie 
    FMOD_REVERB_PROPERTIES prop = FMOD_PRESET_GENERIC;

    [PLATFORMS]
    Win32, Win64, Linux, Linux64, Macintosh, Xbox, Xbox360, PlayStation 2, GameCube, PlayStation Portable, PlayStation 3, Wii

    [SEE_ALSO]
    System::setReverbProperties
    ]
    */
    class PRESET
    {
        /*                                                                           Instance  Env   Size    Diffus  Room   RoomHF  RmLF DecTm   DecHF  DecLF   Refl  RefDel  RefPan           Revb  RevDel  ReverbPan       EchoTm  EchDp  ModTm  ModDp  AirAbs  HFRef    LFRef  RRlOff Diffus  Densty  FLAGS */
        public REVERB_PROPERTIES OFF()                 { return new REVERB_PROPERTIES(0,       0,    7.5f,   1.00f, -10000, -10000, 0,   1.00f,  1.00f, 1.0f,  -2602, 0.007f, 0.0f,0.0f,0.0f,   200, 0.011f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f,   0.0f,   0.0f, 0x3f );}
        public REVERB_PROPERTIES GENERIC()             { return new REVERB_PROPERTIES(0,       0,    7.5f,   1.00f, -1000,  -100,   0,   1.49f,  0.83f, 1.0f,  -2602, 0.007f, 0.0f,0.0f,0.0f,   200, 0.011f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x3f );}
        public REVERB_PROPERTIES PADDEDCELL()          { return new REVERB_PROPERTIES(0,       1,    1.4f,   1.00f, -1000,  -6000,  0,   0.17f,  0.10f, 1.0f,  -1204, 0.001f, 0.0f,0.0f,0.0f,   207, 0.002f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x3f );}
        public REVERB_PROPERTIES ROOM()                { return new REVERB_PROPERTIES(0,       2,    1.9f,   1.00f, -1000,  -454,   0,   0.40f,  0.83f, 1.0f,  -1646, 0.002f, 0.0f,0.0f,0.0f,    53, 0.003f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x3f );}
        public REVERB_PROPERTIES BATHROOM()            { return new REVERB_PROPERTIES(0,       3,    1.4f,   1.00f, -1000,  -1200,  0,   1.49f,  0.54f, 1.0f,   -370, 0.007f, 0.0f,0.0f,0.0f,  1030, 0.011f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f,  60.0f, 0x3f );}
        public REVERB_PROPERTIES LIVINGROOM()          { return new REVERB_PROPERTIES(0,       4,    2.5f,   1.00f, -1000,  -6000,  0,   0.50f,  0.10f, 1.0f,  -1376, 0.003f, 0.0f,0.0f,0.0f, -1104, 0.004f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x3f );}
        public REVERB_PROPERTIES STONEROOM()           { return new REVERB_PROPERTIES(0,       5,    11.6f,  1.00f, -1000,  -300,   0,   2.31f,  0.64f, 1.0f,   -711, 0.012f, 0.0f,0.0f,0.0f,    83, 0.017f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x3f );}
        public REVERB_PROPERTIES AUDITORIUM()          { return new REVERB_PROPERTIES(0,       6,    21.6f,  1.00f, -1000,  -476,   0,   4.32f,  0.59f, 1.0f,   -789, 0.020f, 0.0f,0.0f,0.0f,  -289, 0.030f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x3f );}
        public REVERB_PROPERTIES CONCERTHALL()         { return new REVERB_PROPERTIES(0,       7,    19.6f,  1.00f, -1000,  -500,   0,   3.92f,  0.70f, 1.0f,  -1230, 0.020f, 0.0f,0.0f,0.0f,    -2, 0.029f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x3f );}
        public REVERB_PROPERTIES CAVE()                { return new REVERB_PROPERTIES(0,       8,    14.6f,  1.00f, -1000,  0,      0,   2.91f,  1.30f, 1.0f,   -602, 0.015f, 0.0f,0.0f,0.0f,  -302, 0.022f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x1f );}
        public REVERB_PROPERTIES ARENA()               { return new REVERB_PROPERTIES(0,       9,    36.2f,  1.00f, -1000,  -698,   0,   7.24f,  0.33f, 1.0f,  -1166, 0.020f, 0.0f,0.0f,0.0f,    16, 0.030f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x3f );}
        public REVERB_PROPERTIES HANGAR()              { return new REVERB_PROPERTIES(0,       10,   50.3f,  1.00f, -1000,  -1000,  0,   10.05f, 0.23f, 1.0f,   -602, 0.020f, 0.0f,0.0f,0.0f,   198, 0.030f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x3f );}
        public REVERB_PROPERTIES CARPETTEDHALLWAY()    { return new REVERB_PROPERTIES(0,       11,   1.9f,   1.00f, -1000,  -4000,  0,   0.30f,  0.10f, 1.0f,  -1831, 0.002f, 0.0f,0.0f,0.0f, -1630, 0.030f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x3f );}
        public REVERB_PROPERTIES HALLWAY()             { return new REVERB_PROPERTIES(0,       12,   1.8f,   1.00f, -1000,  -300,   0,   1.49f,  0.59f, 1.0f,  -1219, 0.007f, 0.0f,0.0f,0.0f,   441, 0.011f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x3f );}
        public REVERB_PROPERTIES STONECORRIDOR()       { return new REVERB_PROPERTIES(0,       13,   13.5f,  1.00f, -1000,  -237,   0,   2.70f,  0.79f, 1.0f,  -1214, 0.013f, 0.0f,0.0f,0.0f,   395, 0.020f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x3f );}
        public REVERB_PROPERTIES ALLEY()               { return new REVERB_PROPERTIES(0,       14,   7.5f,   0.30f, -1000,  -270,   0,   1.49f,  0.86f, 1.0f,  -1204, 0.007f, 0.0f,0.0f,0.0f,    -4, 0.011f, 0.0f,0.0f,0.0f, 0.125f, 0.95f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x3f );}
        public REVERB_PROPERTIES FOREST()              { return new REVERB_PROPERTIES(0,       15,   38.0f,  0.30f, -1000,  -3300,  0,   1.49f,  0.54f, 1.0f,  -2560, 0.162f, 0.0f,0.0f,0.0f,  -229, 0.088f, 0.0f,0.0f,0.0f, 0.125f, 1.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f,  79.0f, 100.0f, 0x3f );}
        public REVERB_PROPERTIES CITY()                { return new REVERB_PROPERTIES(0,       16,   7.5f,   0.50f, -1000,  -800,   0,   1.49f,  0.67f, 1.0f,  -2273, 0.007f, 0.0f,0.0f,0.0f, -1691, 0.011f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f,  50.0f, 100.0f, 0x3f );}
        public REVERB_PROPERTIES MOUNTAINS()           { return new REVERB_PROPERTIES(0,       17,   100.0f, 0.27f, -1000,  -2500,  0,   1.49f,  0.21f, 1.0f,  -2780, 0.300f, 0.0f,0.0f,0.0f, -1434, 0.100f, 0.0f,0.0f,0.0f, 0.250f, 1.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f,  27.0f, 100.0f, 0x1f );}
        public REVERB_PROPERTIES QUARRY()              { return new REVERB_PROPERTIES(0,       18,   17.5f,  1.00f, -1000,  -1000,  0,   1.49f,  0.83f, 1.0f, -10000, 0.061f, 0.0f,0.0f,0.0f,   500, 0.025f, 0.0f,0.0f,0.0f, 0.125f, 0.70f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x3f );}
        public REVERB_PROPERTIES PLAIN()               { return new REVERB_PROPERTIES(0,       19,   42.5f,  0.21f, -1000,  -2000,  0,   1.49f,  0.50f, 1.0f,  -2466, 0.179f, 0.0f,0.0f,0.0f, -1926, 0.100f, 0.0f,0.0f,0.0f, 0.250f, 1.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f,  21.0f, 100.0f, 0x3f );}
        public REVERB_PROPERTIES PARKINGLOT()          { return new REVERB_PROPERTIES(0,       20,   8.3f,   1.00f, -1000,  0,      0,   1.65f,  1.50f, 1.0f,  -1363, 0.008f, 0.0f,0.0f,0.0f, -1153, 0.012f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x1f );}
        public REVERB_PROPERTIES SEWERPIPE()           { return new REVERB_PROPERTIES(0,       21,   1.7f,   0.80f, -1000,  -1000,  0,   2.81f,  0.14f, 1.0f,    429, 0.014f, 0.0f,0.0f,0.0f,  1023, 0.021f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f,  80.0f,  60.0f, 0x3f );}
        public REVERB_PROPERTIES UNDERWATER()          { return new REVERB_PROPERTIES(0,       22,   1.8f,   1.00f, -1000,  -4000,  0,   1.49f,  0.10f, 1.0f,   -449, 0.007f, 0.0f,0.0f,0.0f,  1700, 0.011f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 1.18f, 0.348f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x3f );}

        /* Non I3DL2 presets */

        public REVERB_PROPERTIES DRUGGED()             { return new REVERB_PROPERTIES(0,       23,   1.9f,   0.50f, -1000,  0,      0,   8.39f,  1.39f, 1.0f,  -115,  0.002f, 0.0f,0.0f,0.0f,   985, 0.030f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 0.25f, 1.000f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x1f );}
        public REVERB_PROPERTIES DIZZY()               { return new REVERB_PROPERTIES(0,       24,   1.8f,   0.60f, -1000,  -400,   0,   17.23f, 0.56f, 1.0f,  -1713, 0.020f, 0.0f,0.0f,0.0f,  -613, 0.030f, 0.0f,0.0f,0.0f, 0.250f, 1.00f, 0.81f, 0.310f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x1f );}
        public REVERB_PROPERTIES PSYCHOTIC()           { return new REVERB_PROPERTIES(0,       25,   1.0f,   0.50f, -1000,  -151,   0,   7.56f,  0.91f, 1.0f,  -626,  0.020f, 0.0f,0.0f,0.0f,   774, 0.030f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 4.00f, 1.000f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x1f );}

        /* PlayStation 2 Only presets */

        public REVERB_PROPERTIES PS2_ROOM()            { return new REVERB_PROPERTIES(0,       1,    0,     0,         0,  0,      0,   0.0f,   0.0f,  0.0f,     0,  0.000f,  0.0f,0.0f,0.0f ,     0, 0.000f,  0.0f,0.0f,0.0f , 0.000f, 0.00f, 0.00f, 0.000f,  0.0f, 0000.0f,   0.0f, 0.0f,   0.0f,   0.0f, 0x31f );}
        public REVERB_PROPERTIES PS2_STUDIO_A()        { return new REVERB_PROPERTIES(0,       2,    0,     0,         0,  0,      0,   0.0f,   0.0f,  0.0f,     0,  0.000f,  0.0f,0.0f,0.0f ,     0, 0.000f,  0.0f,0.0f,0.0f , 0.000f, 0.00f, 0.00f, 0.000f,  0.0f, 0000.0f,   0.0f, 0.0f,   0.0f,   0.0f, 0x31f );}
        public REVERB_PROPERTIES PS2_STUDIO_B()        { return new REVERB_PROPERTIES(0,       3,    0,     0,         0,  0,      0,   0.0f,   0.0f,  0.0f,     0,  0.000f,  0.0f,0.0f,0.0f ,     0, 0.000f,  0.0f,0.0f,0.0f , 0.000f, 0.00f, 0.00f, 0.000f,  0.0f, 0000.0f,   0.0f, 0.0f,   0.0f,   0.0f, 0x31f );}
        public REVERB_PROPERTIES PS2_STUDIO_C()        { return new REVERB_PROPERTIES(0,       4,    0,     0,         0,  0,      0,   0.0f,   0.0f,  0.0f,     0,  0.000f,  0.0f,0.0f,0.0f ,     0, 0.000f,  0.0f,0.0f,0.0f , 0.000f, 0.00f, 0.00f, 0.000f,  0.0f, 0000.0f,   0.0f, 0.0f,   0.0f,   0.0f, 0x31f );}
        public REVERB_PROPERTIES PS2_HALL()            { return new REVERB_PROPERTIES(0,       5,    0,     0,         0,  0,      0,   0.0f,   0.0f,  0.0f,     0,  0.000f,  0.0f,0.0f,0.0f ,     0, 0.000f,  0.0f,0.0f,0.0f , 0.000f, 0.00f, 0.00f, 0.000f,  0.0f, 0000.0f,   0.0f, 0.0f,   0.0f,   0.0f, 0x31f );}
        public REVERB_PROPERTIES PS2_SPACE()           { return new REVERB_PROPERTIES(0,       6,    0,     0,         0,  0,      0,   0.0f,   0.0f,  0.0f,     0,  0.000f,  0.0f,0.0f,0.0f ,     0, 0.000f,  0.0f,0.0f,0.0f , 0.000f, 0.00f, 0.00f, 0.000f,  0.0f, 0000.0f,   0.0f, 0.0f,   0.0f,   0.0f, 0x31f );}
        public REVERB_PROPERTIES PS2_ECHO()            { return new REVERB_PROPERTIES(0,       7,    0,     0,         0,  0,      0,   0.0f,   0.0f,  0.0f,     0,  0.000f,  0.0f,0.0f,0.0f ,     0, 0.000f,  0.0f,0.0f,0.0f , 0.000f, 0.00f, 0.00f, 0.000f,  0.0f, 0000.0f,   0.0f, 0.0f,   0.0f,   0.0f, 0x31f );}
        public REVERB_PROPERTIES PS2_DELAY()           { return new REVERB_PROPERTIES(0,       8,    0,     0,         0,  0,      0,   0.0f,   0.0f,  0.0f,     0,  0.000f,  0.0f,0.0f,0.0f ,     0, 0.000f,  0.0f,0.0f,0.0f , 0.000f, 0.00f, 0.00f, 0.000f,  0.0f, 0000.0f,   0.0f, 0.0f,   0.0f,   0.0f, 0x31f );}
        public REVERB_PROPERTIES PS2_PIPE()            { return new REVERB_PROPERTIES(0,       9,    0,     0,         0,  0,      0,   0.0f,   0.0f,  0.0f,     0,  0.000f,  0.0f,0.0f,0.0f ,     0, 0.000f,  0.0f,0.0f,0.0f , 0.000f, 0.00f, 0.00f, 0.000f,  0.0f, 0000.0f,   0.0f, 0.0f,   0.0f,   0.0f, 0x31f );}
    }

    /*
    [STRUCTURE] 
    [
        [DESCRIPTION]
        Structure defining the properties for a reverb source, related to a FMOD channel.

        For more indepth descriptions of the reverb properties under win32, please see the EAX3
        documentation at http://developer.creative.com/ under the 'downloads' section.
        If they do not have the EAX3 documentation, then most information can be attained from
        the EAX2 documentation, as EAX3 only adds some more parameters and functionality on top of 
        EAX2.

        Note the default reverb properties are the same as the PRESET_GENERIC preset.
        Note that integer values that typically range from -10,000 to 1000 are represented in 
        decibels, and are of a logarithmic scale, not linear, wheras FLOAT values are typically linear.
        PORTABILITY: Each member has the platform it supports in braces ie (win32/xbox).  
        Some reverb parameters are only supported in win32 and some only on xbox. If all parameters are set then
        the reverb should product a similar effect on either platform.
        Linux and FMODCE do not support the reverb api.

        The numerical values listed below are the maximum, minimum and default values for each variable respectively.

        [REMARKS]
        For EAX4 support with multiple reverb environments, set FMOD_REVERB_CHANNELFLAGS_ENVIRONMENT0,
        FMOD_REVERB_CHANNELFLAGS_ENVIRONMENT1 or/and FMOD_REVERB_CHANNELFLAGS_ENVIRONMENT2 in the flags member 
        of FMOD_REVERB_CHANNELPROPERTIES to specify which environment instance(s) to target. 
        Only up to 2 environments to target can be specified at once. Specifying three will Result in an error.
        If the sound card does not support EAX4, the environment flag is ignored.

        [PLATFORMS]
        Win32, Win64, Linux, Linux64, Macintosh, Xbox, Xbox360, PlayStation 2, GameCube, PlayStation Portable, PlayStation 3, Wii

        [SEE_ALSO]
        Channel::setReverbProperties
        Channel::getReverbProperties
        REVERB_CHANNELFLAGS
    ]
    */
    [StructLayout(LayoutKind.Sequential)]
    public struct REVERB_CHANNELPROPERTIES  
    {                                      /*          MIN     MAX    DEFAULT  DESCRIPTION */
        public int       Direct;               /* [in/out] -10000, 1000,  0,       direct path level (at low and mid frequencies) (win32/xbox) */
        public int       DirectHF;             /* [in/out] -10000, 0,     0,       relative direct path level at high frequencies (win32/xbox) */
        public int       Room;                 /* [in/out] -10000, 1000,  0,       room effect level (at low and mid frequencies) (win32/xbox) */
        public int       RoomHF;               /* [in/out] -10000, 0,     0,       relative room effect level at high frequencies (win32/xbox) */
        public int       Obstruction;          /* [in/out] -10000, 0,     0,       main obstruction control (attenuation at high frequencies)  (win32/xbox) */
        public float     ObstructionLFRatio;   /* [in/out] 0.0,    1.0,   0.0,     obstruction low-frequency level re. main control (win32/xbox) */
        public int       Occlusion;            /* [in/out] -10000, 0,     0,       main occlusion control (attenuation at high frequencies) (win32/xbox) */
        public float     OcclusionLFRatio;     /* [in/out] 0.0,    1.0,   0.25,    occlusion low-frequency level re. main control (win32/xbox) */
        public float     OcclusionRoomRatio;   /* [in/out] 0.0,    10.0,  1.5,     relative occlusion control for room effect (win32) */
        public float     OcclusionDirectRatio; /* [in/out] 0.0,    10.0,  1.0,     relative occlusion control for direct path (win32) */
        public int       Exclusion;            /* [in/out] -10000, 0,     0,       main exlusion control (attenuation at high frequencies) (win32) */
        public float     ExclusionLFRatio;     /* [in/out] 0.0,    1.0,   1.0,     exclusion low-frequency level re. main control (win32) */
        public int       OutsideVolumeHF;      /* [in/out] -10000, 0,     0,       outside sound cone level at high frequencies (win32) */
        public float     DopplerFactor;        /* [in/out] 0.0,    10.0,  0.0,     like DS3D flDopplerFactor but per source (win32) */
        public float     RolloffFactor;        /* [in/out] 0.0,    10.0,  0.0,     like DS3D flRolloffFactor but per source (win32) */
        public float     RoomRolloffFactor;    /* [in/out] 0.0,    10.0,  0.0,     like DS3D flRolloffFactor but for room effect (win32/xbox) */
        public float     AirAbsorptionFactor;  /* [in/out] 0.0,    10.0,  1.0,     multiplies AirAbsorptionHF member of REVERB_PROPERTIES (win32) */
        public uint      Flags;                /* [in/out] REVERB_CHANNELFLAGS - modifies the behavior of properties (win32) */
        public IntPtr    ConnectionPoint;      /* [in/out] See remarks.            DSP network location to connect reverb for this channel.    (SUPPORTED:SFX only).*/
    }


    /*
    [DEFINE] 
    [
        [NAME] 
        REVERB_CHANNELFLAGS

        [DESCRIPTION]
        Values for the Flags member of the REVERB_CHANNELPROPERTIES structure.

        [PLATFORMS]
        Win32, Win64, Linux, Linux64, Macintosh, Xbox, Xbox360, PlayStation 2, GameCube, PlayStation Portable, PlayStation 3, Wii

        [SEE_ALSO]
        REVERB_CHANNELPROPERTIES
    ]
    */
    [StructLayout(LayoutKind.Sequential)]
    public struct REVERB_CHANNELFLAGS
    {
        public const uint DIRECTHFAUTO  = 0x00000001; /* Automatic setting of 'Direct'  due to distance from listener */
        public const uint ROOMAUTO      = 0x00000002; /* Automatic setting of 'Room'  due to distance from listener */
        public const uint ROOMHFAUTO    = 0x00000004; /* Automatic setting of 'RoomHF' due to distance from listener */
        public const uint INSTANCE0     = 0x00000008; /* EAX4/SFX/GameCube/Wii. Specify channel to target reverb instance 0.  Default target. */
        public const uint INSTANCE1     = 0x00000010; /* EAX4/SFX/GameCube/Wii. Specify channel to target reverb instance 1. */
        public const uint INSTANCE2     = 0x00000020; /* EAX4/SFX/GameCube/Wii. Specify channel to target reverb instance 2. */
        public const uint INSTANCE3     = 0x00000040; /* EAX5/SFX. Specify channel to target reverb instance 3. */
        public const uint DEFAULT       = (DIRECTHFAUTO | ROOMAUTO | ROOMHFAUTO | INSTANCE0);
    }


    /*
    [STRUCTURE] 
    [
        [DESCRIPTION]
        Settings for advanced features like configuring memory and cpu usage for the FMOD_CREATECOMPRESSEDSAMPLE feature.
   
        [REMARKS]
        maxMPEGcodecs / maxADPCMcodecs / maxXMAcodecs will determine the maximum cpu usage of playing realtime samples.  Use this to lower potential excess cpu usage and also control memory usage.<br>
   
        [PLATFORMS]
        Win32, Win64, Linux, Linux64, Macintosh, Xbox, Xbox360, PlayStation 2, GameCube, PlayStation Portable, PlayStation 3
   
        [SEE_ALSO]
        System::setAdvancedSettings
        System::getAdvancedSettings
    ]
    */
    [StructLayout(LayoutKind.Sequential)]
    public struct ADVANCEDSETTINGS
    {                       
        public int     cbsize;                   /* Size of structure.  Use sizeof(FMOD_ADVANCEDSETTINGS) */
        public int     maxMPEGcodecs;            /* For use with FMOD_CREATECOMPRESSEDSAMPLE only.  Mpeg  codecs consume 48,696 per instance and this number will determine how many mpeg channels can be played simultaneously.  Default = 16. */
        public int     maxADPCMcodecs;           /* For use with FMOD_CREATECOMPRESSEDSAMPLE only.  ADPCM codecs consume 1k per instance and this number will determine how many ADPCM channels can be played simultaneously.  Default = 32. */
        public int     maxXMAcodecs;             /* For use with FMOD_CREATECOMPRESSEDSAMPLE only.  XMA   codecs consume 8k per instance and this number will determine how many XMA channels can be played simultaneously.  Default = 32.  */
        public int     maxPCMcodecs;             /* [in/out] Optional. Specify 0 to ignore. For use with PS3 only.                          PCM   codecs consume 12,672 bytes per instance and this number will determine how many streams and PCM voices can be played simultaneously. Default = 16 */
        public int     ASIONumChannels;          /* [in/out] */
        public IntPtr  ASIOChannelList;          /* [in/out] */
        public IntPtr  ASIOSpeakerList;          /* [in/out] Optional. Specify 0 to ignore. Pointer to a list of speakers that the ASIO channels map to.  This can be called after System::init to remap ASIO output. */
        public int     max3DReverbDSPs;          /* [in/out] The max number of 3d reverb DSP's in the system. */
        public float   HRTFMinAngle;             /* [in/out] For use with FMOD_INIT_SOFTWARE_HRTF.  The angle (0-360) of a 3D sound from the listener's forward Vector at which the HRTF function begins to have an effect.  Default = 180.0. */
        public float   HRTFMaxAngle;             /* [in/out] For use with FMOD_INIT_SOFTWARE_HRTF.  The angle (0-360) of a 3D sound from the listener's forward Vector at which the HRTF function begins to have maximum effect.  Default = 360.0.  */
        public float   HRTFFreq;                 /* [in/out] For use with FMOD_INIT_SOFTWARE_HRTF.  The cutoff frequency of the HRTF's lowpass filter function when at maximum effect. (i.e. at HRTFMaxAngle).  Default = 4000.0. */
        public float   vol0virtualvol;           /* [in/out] For use with FMOD_INIT_VOL0_BECOMES_VIRTUAL.  If this flag is used, and the volume is 0.0, then the sound will become virtual.  Use this value to raise the threshold to a different point where a sound goes virtual. */
        public int     eventqueuesize;           /* [in/out] Optional. Specify 0 to ignore. For use with FMOD Event system only.  Specifies the number of slots available for simultaneous non blocking loads.  Default = 32. */
        public uint    defaultDecodeBufferSize;  /* [in/out] Optional. Specify 0 to ignore. For streams. This determines the default size of the double buffer (in milliseconds) that a stream uses.  Default = 400ms */
        public IntPtr  debugLogFilename;         /* [in/out] Optional. Specify 0 to ignore. Gives fmod's logging system a path/filename.  Normally the log is placed in the same directory as the executable and called fmod.log. When using System::getAdvancedSettings, provide at least 256 bytes of memory to copy into. */
        public ushort  profileport;              /* [in/out] Optional. Specify 0 to ignore. For use with FMOD_INIT_ENABLE_PROFILE.  Specify the port to listen on for connections by the profiler application. */
    }


    /*
    [ENUM] 
    [
        [NAME] 
        FMOD_MISC_VALUES

        [DESCRIPTION]
        Miscellaneous values for FMOD functions.

        [PLATFORMS]
        Win32, Win64, Linux, Linux64, Macintosh, Xbox, Xbox360, PlayStation 2, GameCube, PlayStation Portable, PlayStation 3, Wii

        [SEE_ALSO]
        System::playSound
        System::playDSP
        System::getChannel
    ]
    */
    public enum CHANNELINDEX
    {
        FREE   = -1,     /* For a channel index, FMOD chooses a free voice using the priority system. */
        REUSE  = -2      /* For a channel index, re-use the channel handle that was passed in. */
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

    /*
        FMOD System factory functions.  Use this to create an FMOD System Instance.  below you will see System_Init/Close to get started.
    */
    public class Factory
    {
		/// <summary>
		/// Creates a new FMOD.System object to use.
		/// </summary>
		/// <returns>The new FMOD.System object.</returns>
		public static System CreateSystem()
        {
            Result Result      = Result.OK;
            IntPtr      systemraw   = new IntPtr();
            System      systemnew   = null;

            Result = FMOD_System_Create(ref systemraw);
            if (Result != Result.OK)
            {
				throw new Exception(Error.String(Result));
            }

            systemnew = new System();
            systemnew.setRaw(systemraw);
            //system = systemnew;

            return systemnew;
        }

        #region importfunctions
  
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_Create (ref IntPtr system);

        #endregion
    }

    
    public class Memory
    {
		public static Result GetStats(ref int currentalloced, ref int maxalloced)
        {
            return FMOD_Memory_GetStats(ref currentalloced, ref maxalloced);
        }


        #region importfunctions
  
        [DllImport (Version.Dll)]
		private static extern Result FMOD_Memory_GetStats(ref int currentalloced, ref int maxalloced);

        #endregion
    }
        

    /*
        'System' API
    */
    public class System
    {
        public Result release                ()
        {
            return FMOD_System_Release(systemraw);
        }


        // Pre-init functions.
        public Result setOutput              (OutputType output)
        {
            return FMOD_System_SetOutput(systemraw, output);
        }
        public Result getOutput              (ref OutputType output)
        {
            return FMOD_System_GetOutput(systemraw, ref output);
        }
        public Result getNumDrivers          (ref int numdrivers)
        {
            return FMOD_System_GetNumDrivers(systemraw, ref numdrivers);
        }
        public Result getDriverInfo          (int id, StringBuilder name, int namelen, ref GUID guid)
        {
            return FMOD_System_GetDriverInfo(systemraw, id, name, namelen, ref guid);
        }
        public Result getDriverCaps          (int id, ref Capabilities caps, ref int minfrequency, ref int maxfrequency, ref SpeakerMode controlpanelspeakermode)
        {
            return FMOD_System_GetDriverCaps(systemraw, id, ref caps, ref minfrequency, ref maxfrequency, ref controlpanelspeakermode);
        }
        public Result setDriver              (int driver)
        {
            return FMOD_System_SetDriver(systemraw, driver);
        }
        public Result getDriver              (ref int driver)
        {
            return FMOD_System_GetDriver(systemraw, ref driver);
        }
        public Result setHardwareChannels    (int min2d, int max2d, int min3d, int max3d)
        {
            return FMOD_System_SetHardwareChannels(systemraw, min2d, max2d, min3d, max3d);
        }
        public Result setSoftwareChannels    (int numsoftwarechannels)
        {
            return FMOD_System_SetSoftwareChannels(systemraw, numsoftwarechannels);
        }
        public Result getSoftwareChannels    (ref int numsoftwarechannels)
        {
            return FMOD_System_GetSoftwareChannels(systemraw, ref numsoftwarechannels);
        }
        public Result setSoftwareFormat      (int samplerate, SoundFormat format, int numoutputchannels, int maxinputchannels, DSP_RESAMPLER resamplemethod)
        {
            return FMOD_System_SetSoftwareFormat(systemraw, samplerate, format, numoutputchannels, maxinputchannels, resamplemethod);
        }
        public Result getSoftwareFormat      (ref int samplerate, ref SoundFormat format, ref int numoutputchannels, ref int maxinputchannels, ref DSP_RESAMPLER resamplemethod, ref int bits)
        {
            return FMOD_System_GetSoftwareFormat(systemraw, ref samplerate, ref format, ref numoutputchannels, ref maxinputchannels, ref resamplemethod, ref bits);
        }
        public Result setDSPBufferSize       (uint bufferlength, int numbuffers)
        {
            return FMOD_System_SetDSPBufferSize(systemraw, bufferlength, numbuffers);
        }
        public Result getDSPBufferSize       (ref uint bufferlength, ref int numbuffers)
        {
            return FMOD_System_GetDSPBufferSize(systemraw, ref bufferlength, ref numbuffers);
        }
        public Result setFileSystem          (FILE_OPENCALLBACK useropen, FILE_CLOSECALLBACK userclose, FILE_READCALLBACK userread, FILE_SEEKCALLBACK userseek, int buffersize)
        {
            return FMOD_System_SetFileSystem(systemraw, useropen, userclose, userread, userseek, buffersize);
        }
        public Result attachFileSystem       (FILE_OPENCALLBACK useropen, FILE_CLOSECALLBACK userclose, FILE_READCALLBACK userread, FILE_SEEKCALLBACK userseek)
        {
            return FMOD_System_AttachFileSystem(systemraw, useropen, userclose, userread, userseek);
        }
        public Result setAdvancedSettings    (ref ADVANCEDSETTINGS settings)
        {
            return FMOD_System_SetAdvancedSettings(systemraw, ref settings);
        }
        public Result getAdvancedSettings    (ref ADVANCEDSETTINGS settings)
        {
            return FMOD_System_GetAdvancedSettings(systemraw, ref settings);
        }
        public Result setSpeakerMode         (SpeakerMode speakermode)
        {
            return FMOD_System_SetSpeakerMode(systemraw, speakermode);
        }
        public Result getSpeakerMode         (ref SpeakerMode speakermode)
        {
            return FMOD_System_GetSpeakerMode(systemraw, ref speakermode);
        }
        
                         
        // Plug-in support
        public Result setPluginPath          (string path)
        {
            return FMOD_System_SetPluginPath(systemraw, path);
        }
        public Result loadPlugin             (string filename, ref uint handle, uint priority)
        {
            return FMOD_System_LoadPlugin(systemraw, filename, ref handle, priority);
        }
        public Result unloadPlugin           (uint handle)
        {
            return FMOD_System_UnloadPlugin(systemraw, handle);
        }
        public Result getNumPlugins          (PluginType plugintype, ref int numplugins)
        {
            return FMOD_System_GetNumPlugins(systemraw, plugintype, ref numplugins);
        }
        public Result getPluginHandle        (PluginType plugintype, int index, ref uint handle)
        {
            return FMOD_System_GetPluginHandle(systemraw, plugintype, index, ref handle);
        }
        public Result getPluginInfo          (uint handle, ref PluginType plugintype, StringBuilder name, int namelen, ref uint version)
        {
            return FMOD_System_GetPluginInfo(systemraw, handle, ref plugintype, name, namelen, ref version);
        }

        public Result setOutputByPlugin      (uint handle)
        {
            return FMOD_System_SetOutputByPlugin(systemraw, handle);
        }
        public Result getOutputByPlugin      (ref uint handle)
        {
            return FMOD_System_GetOutputByPlugin(systemraw, ref handle);
        }
        public Result createDSPByPlugin      (uint handle, ref IntPtr dsp)
        {
            return FMOD_System_CreateDSPByPlugin(systemraw, handle, ref dsp);
        }
        public Result createCodec            (IntPtr codecdescription, uint priority)
        {
            return FMOD_System_CreateCodec(systemraw, codecdescription, priority);
        }


        // Init/Close 
        public Result init                   (int maxchannels, InitalisationFlags flags, IntPtr extradata)
        {
            return FMOD_System_Init(systemraw, maxchannels, flags, extradata);
        }
        public Result close                  ()
        {
            return FMOD_System_Close(systemraw);
        }


        // General post-init system functions
        public Result update                 ()
        {
            return FMOD_System_Update(systemraw);
        }

        public Result set3DSettings          (float dopplerscale, float distancefactor, float rolloffscale)
        {
            return FMOD_System_Set3DSettings(systemraw, dopplerscale, distancefactor, rolloffscale);
        }
        public Result get3DSettings          (ref float dopplerscale, ref float distancefactor, ref float rolloffscale)
        {
            return FMOD_System_Get3DSettings(systemraw, ref dopplerscale, ref distancefactor, ref rolloffscale);
        }
        public Result set3DNumListeners      (int numlisteners)
        {
            return FMOD_System_Set3DNumListeners(systemraw, numlisteners);
        }
        public Result get3DNumListeners      (ref int numlisteners)
        {
            return FMOD_System_Get3DNumListeners(systemraw, ref numlisteners);
        }
		public Result set3DListenerAttributes(int listener, ref Vector pos, ref Vector vel, ref Vector forward, ref Vector up)
        {
            return FMOD_System_Set3DListenerAttributes(systemraw, listener, ref pos, ref vel, ref forward, ref up);
        }
		public Result get3DListenerAttributes(int listener, ref Vector pos, ref Vector vel, ref Vector forward, ref Vector up)
        {
            return FMOD_System_Get3DListenerAttributes(systemraw, listener, ref pos, ref vel, ref forward, ref up);
        }

        public Result set3DRolloffCallback   (CB_3D_ROLLOFFCALLBACK callback)
        {
            return  FMOD_System_Set3DRolloffCallback   (systemraw, callback);
        }
        public Result set3DSpeakerPosition     (Speaker speaker, float x, float y, bool active)
        {
            return FMOD_System_Set3DSpeakerPosition(systemraw, speaker, x, y, (active ? 1 : 0));
        }
        public Result get3DSpeakerPosition     (Speaker speaker, ref float x, ref float y, ref bool active)
        {
            Result Result;
            
            int isactive = 0;

            Result = FMOD_System_Get3DSpeakerPosition(systemraw, speaker, ref x, ref y, ref isactive);

            active = (isactive != 0);

            return Result;
        }

        public Result setStreamBufferSize    (uint filebuffersize, TIMEUNIT filebuffersizetype)
        {
            return FMOD_System_SetStreamBufferSize(systemraw, filebuffersize, filebuffersizetype);
        }
        public Result getStreamBufferSize    (ref uint filebuffersize, ref TIMEUNIT filebuffersizetype)
        {
            return FMOD_System_GetStreamBufferSize(systemraw, ref filebuffersize, ref filebuffersizetype);
        }


        // System information functions.
        public Result getVersion             (ref uint version)
        {
            return FMOD_System_GetVersion(systemraw, ref version);
        }
        public Result getOutputHandle        (ref IntPtr handle)
        {
            return FMOD_System_GetOutputHandle(systemraw, ref handle);
        }
        public Result getChannelsPlaying     (ref int channels)
        {
            return FMOD_System_GetChannelsPlaying(systemraw, ref channels);
        }
        public Result getHardwareChannels    (ref int num2d, ref int num3d, ref int total)
        {
            return FMOD_System_GetHardwareChannels(systemraw, ref num2d, ref num3d, ref total);
        }
        public Result getCPUUsage            (ref float dsp, ref float stream, ref float update, ref float total)
        {
            return FMOD_System_GetCPUUsage(systemraw, ref dsp, ref stream, ref update, ref total);
        }
        public Result getSoundRam            (ref int currentalloced, ref int maxalloced, ref int total)
        {
            return FMOD_System_GetSoundRAM(systemraw, ref currentalloced, ref maxalloced, ref total);
        }
        public Result getNumCDROMDrives      (ref int numdrives)
        {
            return FMOD_System_GetNumCDROMDrives(systemraw, ref numdrives);
        }
        public Result getCDROMDriveName      (int drive, StringBuilder drivename, int drivenamelen, StringBuilder scsiname, int scsinamelen, StringBuilder devicename, int devicenamelen)
        {
            return FMOD_System_GetCDROMDriveName(systemraw, drive, drivename, drivenamelen, scsiname, scsinamelen, devicename, devicenamelen);
        }
        public Result getSpectrum            (float[] spectrumarray, int numvalues, int channeloffset, DSP_FFT_WINDOW windowtype)
        {
            return FMOD_System_GetSpectrum(systemraw, spectrumarray, numvalues, channeloffset, windowtype);
        }
        public Result getWaveData            (float[] wavearray, int numvalues, int channeloffset)
        {
            return FMOD_System_GetWaveData(systemraw, wavearray, numvalues, channeloffset);
        }


        // Sound/DSP/Channel creation and retrieval. 
        public Result createSound            (string name_or_data, Mode mode, ref CREATESOUNDEXINFO exinfo, ref Sound sound)
        {
            Result Result           = Result.OK;
            IntPtr      soundraw    = new IntPtr();
            Sound       soundnew    = null;

			mode = mode | Mode.Unicode;

            try
            {
                Result = FMOD_System_CreateSound(systemraw, name_or_data, mode, ref exinfo, ref soundraw);
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
        public Result createSound            (byte[] data, Mode mode, ref CREATESOUNDEXINFO exinfo, ref Sound sound)
        {
            Result Result           = Result.OK;
            IntPtr      soundraw    = new IntPtr();
            Sound       soundnew    = null;

            try
            {
                Result = FMOD_System_CreateSound(systemraw, data, mode, ref exinfo, ref soundraw);
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
		public Result createSound(string name_or_data, Mode mode, ref Sound sound)
        {
            Result Result           = Result.OK;
            IntPtr      soundraw    = new IntPtr();
            Sound       soundnew    = null;

            mode = mode | Mode.Unicode;

            try
            {
                Result = FMOD_System_CreateSound(systemraw, name_or_data, mode, 0, ref soundraw);
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
		public Result createStream(string name_or_data, Mode mode, ref CREATESOUNDEXINFO exinfo, ref Sound sound)
        {
            Result Result           = Result.OK;
            IntPtr      soundraw    = new IntPtr();
            Sound       soundnew    = null;

            mode = mode | Mode.Unicode;

            try
            {
                Result = FMOD_System_CreateStream(systemraw, name_or_data, mode, ref exinfo, ref soundraw);
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
		public Result createStream(byte[] data, Mode mode, ref CREATESOUNDEXINFO exinfo, ref Sound sound)
        {
            Result Result           = Result.OK;
            IntPtr      soundraw    = new IntPtr();
            Sound       soundnew    = null;

            try
            {
                Result = FMOD_System_CreateStream(systemraw, data, mode, ref exinfo, ref soundraw);
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
		public Result createStream(string name_or_data, Mode mode, ref Sound sound)
        {
            Result Result           = Result.OK;
            IntPtr      soundraw    = new IntPtr();
            Sound       soundnew    = null;

            mode = mode | Mode.Unicode;

            try
            {
                Result = FMOD_System_CreateStream(systemraw, name_or_data, mode, 0, ref soundraw);
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
        /*public Result createDSP              (ref DSP_DESCRIPTION description, ref DSP dsp)
        {
            Result Result = Result.OK;
            IntPtr dspraw = new IntPtr();
            DSP    dspnew = null;

            try
            {
                Result = FMOD_System_CreateDSP(systemraw, ref description, ref dspraw);
            }
            catch
            {
                Result = Result.ERR_INVALID_PARAM;
            }
            if (Result != Result.OK)
            {
                return Result;
            }

            if (dsp == null)
            {
                dspnew = new DSP();
                dspnew.setRaw(dspraw);
                dsp = dspnew;
            }
            else
            {
                dsp.setRaw(dspraw);
            }
                             
            return Result;  
        }
		public Result createDSPByType          (DSP_TYPE type, ref DSP dsp)
        {
            Result Result = Result.OK;
            IntPtr dspraw = new IntPtr();
            DSP    dspnew = null;

            try
            {
                Result = FMOD_System_CreateDSPByType(systemraw, type, ref dspraw);
            }
            catch
            {
                Result = Result.ERR_INVALID_PARAM;
            }
            if (Result != Result.OK)
            {
                return Result;
            }

            if (dsp == null)
            {
                dspnew = new DSP();
                dspnew.setRaw(dspraw);
                dsp = dspnew;
            }
            else
            {
                dsp.setRaw(dspraw);
            }
                             
            return Result;  
        }
        public Result createDSPByIndex       (int index, ref DSP dsp)
        {
            Result Result = Result.OK;
            IntPtr dspraw = new IntPtr();
            DSP    dspnew = null;

            try
            {
                Result = FMOD_System_CreateDSPByIndex(systemraw, index, ref dspraw);
            }
            catch
            {
                Result = Result.ERR_INVALID_PARAM;
            }
            if (Result != Result.OK)
            {
                return Result;
            }

            if (dsp == null)
            {
                dspnew = new DSP();
                dspnew.setRaw(dspraw);
                dsp = dspnew;
            }
            else
            {
                dsp.setRaw(dspraw);
            }
                             
            return Result;  
        }*/
                       
        public Result createChannelGroup     (string name, ref ChannelGroup channelgroup)
        {
            Result Result = Result.OK;
            IntPtr channelgroupraw = new IntPtr();
            ChannelGroup    channelgroupnew = null;

            try
            {
                Result = FMOD_System_CreateChannelGroup(systemraw, name, ref channelgroupraw);
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
        public Result playSound              (CHANNELINDEX channelid, Sound sound, bool paused, ref Channel channel)
        {
            Result Result      = Result.OK;
            IntPtr      channelraw;
            Channel     channelnew  = null;

            if (channel != null)
            {
                channelraw = channel.getRaw();
            }
            else
            {
                channelraw  = new IntPtr();
            }

            try
            {
                Result = FMOD_System_PlaySound(systemraw, channelid, sound.getRaw(), (paused ? 1 : 0), ref channelraw);
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
        /* public Result playDSP                (CHANNELINDEX channelid, DSP dsp, bool paused, ref Channel channel)
        {
            Result Result           = Result.OK;
            IntPtr      channelraw;
            Channel     channelnew  = null;

            if (channel != null)
            {
                channelraw = channel.getRaw();
            }
            else
            {
                channelraw  = new IntPtr();
            }

            try
            {
                Result = FMOD_System_PlayDSP(systemraw, channelid, dsp.getRaw(), (paused ? 1 : 0), ref channelraw);
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
        } */
        public Result getChannel             (int channelid, ref Channel channel)
        {
            Result Result      = Result.OK;
            IntPtr      channelraw  = new IntPtr();
            Channel     channelnew  = null;

            try
            {
                Result = FMOD_System_GetChannel(systemraw, channelid, ref channelraw);
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
     
        public Result getMasterChannelGroup  (ref ChannelGroup channelgroup)
        {
            Result Result = Result.OK;
            IntPtr channelgroupraw = new IntPtr();
            ChannelGroup    channelgroupnew = null;

            try
            {
                Result = FMOD_System_GetMasterChannelGroup(systemraw, ref channelgroupraw);
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

        public Result getMasterSoundGroup    (ref SoundGroup soundgroup)
        {
            Result Result = Result.OK;
            IntPtr soundgroupraw = new IntPtr();
            SoundGroup    soundgroupnew = null;

            try
            {
                Result = FMOD_System_GetMasterSoundGroup(systemraw, ref soundgroupraw);
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

        // Reverb api
        public Result setReverbProperties    (ref REVERB_PROPERTIES prop)
        {
            return FMOD_System_SetReverbProperties(systemraw, ref prop);
        }
        public Result getReverbProperties    (ref REVERB_PROPERTIES prop)
        {
            return FMOD_System_GetReverbProperties(systemraw, ref prop);
        }
                                        
        public Result setReverbAmbientProperties (ref REVERB_PROPERTIES prop)
        {
            return FMOD_System_SetReverbAmbientProperties(systemraw, ref prop);
        }
        public Result getReverbAmbientProperties (ref REVERB_PROPERTIES prop)
        {
            return FMOD_System_GetReverbAmbientProperties(systemraw, ref prop);
        }

        // System level DSP access.
		/* public Result getDSPHead             (ref DSP dsp)
        {
            Result Result   = Result.OK;
            IntPtr dspraw   = new IntPtr();
            DSP    dspnew   = null;

            try
            {
                Result = FMOD_System_GetDSPHead(systemraw, ref dspraw);
            }
            catch
            {
                Result = Result.ERR_INVALID_PARAM;
            }
            if (Result != Result.OK)
            {
                return Result;
            }

            if (dsp == null)
            {
                dspnew = new DSP();
                dspnew.setRaw(dspraw);
                dsp = dspnew;
            }
            else
            {
                dsp.setRaw(dspraw);
            }

            return Result;
        }
        public Result addDSP                 (DSP dsp, ref DSPConnection dspconnection)
        {
            Result Result = Result.OK;
            IntPtr dspconnectionraw = new IntPtr();
            DSPConnection dspconnectionnew = null;

            try
            {
                Result = FMOD_System_AddDSP(systemraw, dsp.getRaw(), ref dspconnectionraw);
            }
            catch
            {
                Result = Result.ERR_INVALID_PARAM;
            }
            if (Result != Result.OK)
            { 
                return Result;
            }

            if (dspconnection == null)
            {
                dspconnectionnew = new DSPConnection();
                dspconnectionnew.setRaw(dspconnectionraw);
                dspconnection = dspconnectionnew;
            }
            else
            {
                dspconnection.setRaw(dspconnectionraw);
            }

            return Result;
        }
        public Result lockDSP            ()
        {
            return FMOD_System_LockDSP(systemraw);
        }
        public Result unlockDSP          ()
        {
            return FMOD_System_UnlockDSP(systemraw);
        } */                                    
        
        // Recording api
		/*
        public Result getRecordNumDrivers    (ref int numdrivers)
        {
            return FMOD_System_GetRecordNumDrivers(systemraw, ref numdrivers);
        }
        public Result getRecordDriverInfo    (int id, StringBuilder name, int namelen, ref GUID guid)
        {
            return FMOD_System_GetRecordDriverInfo(systemraw, id, name, namelen, ref guid);
        }
 
        public Result getRecordPosition      (int id, ref uint position)
        {
            return FMOD_System_GetRecordPosition(systemraw, id, ref position);
        }
        public Result recordStart            (int id, Sound sound, bool loop)
        {
            return FMOD_System_RecordStart(systemraw, id, sound.getRaw(), loop);
        }
        public Result recordStop             (int id)
        {
            return FMOD_System_RecordStop(systemraw, id);
        }
        public Result isRecording            (int id, ref bool recording)
        {
            return FMOD_System_IsRecording(systemraw, id, ref recording);
        }
         
      
        // Geometry api 
        public Result createGeometry         (int maxpolygons, int maxvertices, ref Geometry geometryf)
        {
            Result Result           = Result.OK;
            IntPtr      geometryraw    = new IntPtr();
            Geometry    geometrynew    = null;

            try
            {
                Result = FMOD_System_CreateGeometry(systemraw, maxpolygons, maxvertices, ref geometryraw);
            }
            catch
            {
                Result = Result.ERR_INVALID_PARAM;
            }
            if (Result != Result.OK)
            {
                return Result;
            }

            if (geometryf == null)
            {
                geometrynew = new Geometry();
                geometrynew.setRaw(geometryraw);
                geometryf = geometrynew;
            }
            else
            {
                geometryf.setRaw(geometryraw);
            }

            return Result;
        }
        public Result setGeometrySettings    (float maxworldsize)
        {
            return FMOD_System_SetGeometrySettings(systemraw, maxworldsize);
        }
        public Result getGeometrySettings    (ref float maxworldsize)
        {
            return FMOD_System_GetGeometrySettings(systemraw, ref maxworldsize);
        }
        public Result loadGeometry(IntPtr data, int datasize, ref Geometry geometry)
        {
            Result Result           = Result.OK;
            IntPtr      geometryraw    = new IntPtr();
            Geometry    geometrynew    = null;

            try
            {
                Result = FMOD_System_LoadGeometry(systemraw, data, datasize, ref geometryraw);
            }
            catch
            {
                Result = Result.ERR_INVALID_PARAM;
            }
            if (Result != Result.OK)
            {
                return Result;
            }

            if (geometry == null)
            {
                geometrynew = new Geometry();
                geometrynew.setRaw(geometryraw);
                geometry = geometrynew;
            }
            else
            {
                geometry.setRaw(geometryraw);
            }

            return Result;
        }

  
        // Network functions
        public Result setNetworkProxy               (string proxy)
        {
            return FMOD_System_SetNetworkProxy(systemraw, proxy);
        }
        public Result getProxy               (StringBuilder proxy, int proxylen)
        {
            return FMOD_System_GetNetworkProxy(systemraw, proxy, proxylen);
        }
        public Result setNetworkTimeout      (int timeout)
        {
            return FMOD_System_SetNetworkTimeout(systemraw, timeout);
        }
        public Result getNetworkTimeout(ref int timeout)
        {
            return FMOD_System_GetNetworkTimeout(systemraw, ref timeout);
        } */
                                     
        // Userdata set/get                         
        public Result setUserData            (IntPtr userdata)
        {
            return FMOD_System_SetUserData(systemraw, userdata);
        }
        public Result getUserData            (ref IntPtr userdata)
        {
            return FMOD_System_GetUserData(systemraw, ref userdata);
        }

        public Result getMemoryInfo(uint memorybits, uint event_memorybits, ref uint memoryused, IntPtr memoryused_array)
        {
            return FMOD_System_GetMemoryInfo(systemraw, memorybits, event_memorybits, ref memoryused, memoryused_array);
        }

        #region Imported functions
		[DllImport(Version.Dll)]
        private static extern Result FMOD_System_Release                (IntPtr system);
		[DllImport(Version.Dll)]
        private static extern Result FMOD_System_SetOutput              (IntPtr system, OutputType output);
		[DllImport(Version.Dll)]
        private static extern Result FMOD_System_GetOutput              (IntPtr system, ref OutputType output);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_GetNumDrivers          (IntPtr system, ref int numdrivers);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_GetDriverInfo          (IntPtr system, int id, StringBuilder name, int namelen, ref GUID guid);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_GetDriverCaps          (IntPtr system, int id, ref Capabilities caps, ref int minfrequency, ref int maxfrequency, ref SpeakerMode controlpanelspeakermode);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_SetDriver              (IntPtr system, int driver);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_GetDriver              (IntPtr system, ref int driver);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_SetHardwareChannels    (IntPtr system, int min2d, int max2d, int min3d, int max3d);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_GetHardwareChannels    (IntPtr system, ref int num2d, ref int num3d, ref int total);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_SetSoftwareChannels    (IntPtr system, int numsoftwarechannels);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_GetSoftwareChannels    (IntPtr system, ref int numsoftwarechannels);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_SetSoftwareFormat      (IntPtr system, int samplerate, SoundFormat format, int numoutputchannels, int maxinputchannels, DSP_RESAMPLER resamplemethod);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_GetSoftwareFormat      (IntPtr system, ref int samplerate, ref SoundFormat format, ref int numoutputchannels, ref int maxinputchannels, ref DSP_RESAMPLER resamplemethod, ref int bits);        
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_SetDSPBufferSize       (IntPtr system, uint bufferlength, int numbuffers);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_GetDSPBufferSize       (IntPtr system, ref uint bufferlength, ref int numbuffers);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_SetFileSystem          (IntPtr system, FILE_OPENCALLBACK useropen, FILE_CLOSECALLBACK userclose, FILE_READCALLBACK userread, FILE_SEEKCALLBACK userseek, int buffersize);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_AttachFileSystem       (IntPtr system, FILE_OPENCALLBACK useropen, FILE_CLOSECALLBACK userclose, FILE_READCALLBACK userread, FILE_SEEKCALLBACK userseek);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_SetPluginPath          (IntPtr system, string path);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_LoadPlugin             (IntPtr system, string filename, ref uint handle, uint priority);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_UnloadPlugin           (IntPtr system, uint handle);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_GetNumPlugins          (IntPtr system, PluginType plugintype, ref int numplugins);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_GetPluginHandle        (IntPtr system, PluginType plugintype, int index, ref uint handle);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_GetPluginInfo          (IntPtr system, uint handle, ref PluginType plugintype, StringBuilder name, int namelen, ref uint version);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_CreateDSPByPlugin      (IntPtr system, uint handle, ref IntPtr dsp);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_CreateCodec            (IntPtr system, IntPtr codecdescription, uint priority);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_SetOutputByPlugin      (IntPtr system, uint handle);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_GetOutputByPlugin      (IntPtr system, ref uint handle);        
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_Init                   (IntPtr system, int maxchannels, InitalisationFlags flags, IntPtr extradata);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_Close                  (IntPtr system);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_Update                 (IntPtr system);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_UpdateFinished         (IntPtr system);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_SetAdvancedSettings    (IntPtr system, ref ADVANCEDSETTINGS settings);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_GetAdvancedSettings    (IntPtr system, ref ADVANCEDSETTINGS settings);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_SetSpeakerMode         (IntPtr system, SpeakerMode speakermode);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_GetSpeakerMode         (IntPtr system, ref SpeakerMode speakermode);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_Set3DRolloffCallback   (IntPtr system, CB_3D_ROLLOFFCALLBACK callback);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_Set3DSpeakerPosition     (IntPtr system, Speaker speaker, float x, float y, int active);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_Get3DSpeakerPosition     (IntPtr system, Speaker speaker, ref float x, ref float y, ref int active);
        [DllImport (Version.Dll)]                       
        private static extern Result FMOD_System_Set3DSettings          (IntPtr system, float dopplerscale, float distancefactor, float rolloffscale);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_Get3DSettings          (IntPtr system, ref float dopplerscale, ref float distancefactor, ref float rolloffscale);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_Set3DNumListeners      (IntPtr system, int numlisteners);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_Get3DNumListeners      (IntPtr system, ref int numlisteners);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_Set3DListenerAttributes(IntPtr system, int listener, ref Vector pos, ref Vector vel, ref Vector forward, ref Vector up);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_Get3DListenerAttributes(IntPtr system, int listener, ref Vector pos, ref Vector vel, ref Vector forward, ref Vector up);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_SetFileBufferSize      (IntPtr system, int sizebytes);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_GetFileBufferSize      (IntPtr system, ref int sizebytes);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_SetStreamBufferSize    (IntPtr system, uint filebuffersize, TIMEUNIT filebuffersizetype);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_GetStreamBufferSize    (IntPtr system, ref uint filebuffersize, ref TIMEUNIT filebuffersizetype);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_GetVersion             (IntPtr system, ref uint version);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_GetOutputHandle        (IntPtr system, ref IntPtr handle);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_GetChannelsPlaying     (IntPtr system, ref int channels);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_GetCPUUsage            (IntPtr system, ref float dsp, ref float stream, ref float update, ref float total);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_GetSoundRAM            (IntPtr system, ref int currentalloced, ref int maxalloced, ref int total);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_GetNumCDROMDrives      (IntPtr system, ref int numdrives);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_GetCDROMDriveName      (IntPtr system, int drive, StringBuilder drivename, int drivenamelen, StringBuilder scsiname, int scsinamelen, StringBuilder devicename, int devicenamelen);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_GetSpectrum            (IntPtr system, [MarshalAs(UnmanagedType.LPArray)]float[] spectrumarray, int numvalues, int channeloffset, DSP_FFT_WINDOW windowtype);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_GetWaveData            (IntPtr system, [MarshalAs(UnmanagedType.LPArray)]float[] wavearray, int numvalues, int channeloffset);
        [DllImport (Version.Dll, CharSet = CharSet.Unicode)]
		private static extern Result FMOD_System_CreateSound(IntPtr system, string name_or_data, Mode mode, ref CREATESOUNDEXINFO exinfo, ref IntPtr sound);
        [DllImport (Version.Dll, CharSet = CharSet.Unicode)]
		private static extern Result FMOD_System_CreateStream(IntPtr system, string name_or_data, Mode mode, ref CREATESOUNDEXINFO exinfo, ref IntPtr sound);
        [DllImport(Version.Dll, CharSet = CharSet.Unicode)]
		private static extern Result FMOD_System_CreateSound(IntPtr system, string name_or_data, Mode mode, int exinfo, ref IntPtr sound);
        [DllImport(Version.Dll, CharSet = CharSet.Unicode)]
		private static extern Result FMOD_System_CreateStream(IntPtr system, string name_or_data, Mode mode, int exinfo, ref IntPtr sound);   
        [DllImport (Version.Dll)]
		private static extern Result FMOD_System_CreateSound(IntPtr system, byte[] name_or_data, Mode mode, ref CREATESOUNDEXINFO exinfo, ref IntPtr sound);
        [DllImport (Version.Dll)]
		private static extern Result FMOD_System_CreateStream(IntPtr system, byte[] name_or_data, Mode mode, ref CREATESOUNDEXINFO exinfo, ref IntPtr sound);
        [DllImport (Version.Dll)]
		private static extern Result FMOD_System_CreateSound(IntPtr system, byte[] name_or_data, Mode mode, int exinfo, ref IntPtr sound);
        [DllImport (Version.Dll)]
		private static extern Result FMOD_System_CreateStream(IntPtr system, byte[] name_or_data, Mode mode, int exinfo, ref IntPtr sound);
        //[DllImport (Version.Dll)]
        //private static extern Result FMOD_System_CreateDSP              (IntPtr system, ref DSP_DESCRIPTION description, ref IntPtr dsp);
        //[DllImport (Version.Dll)]
        //private static extern Result FMOD_System_CreateDSPByType        (IntPtr system, DSP_TYPE type, ref IntPtr dsp);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_CreateDSPByIndex       (IntPtr system, int index, ref IntPtr dsp);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_CreateChannelGroup     (IntPtr system, string name, ref IntPtr channelgroup);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_CreateSoundGroup       (IntPtr system, StringBuilder name, ref SoundGroup soundgroup);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_CreateReverb           (IntPtr system, ref IntPtr reverb);
        [DllImport (Version.Dll)]                 
        private static extern Result FMOD_System_PlaySound              (IntPtr system, CHANNELINDEX channelid, IntPtr sound, int paused, ref IntPtr channel);
        [DllImport (Version.Dll)]
        public static extern Result FMOD_System_PlayDSP                 (IntPtr system, CHANNELINDEX channelid, IntPtr dsp, int paused, ref IntPtr channel);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_GetChannel             (IntPtr system, int channelid, ref IntPtr channel);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_GetMasterChannelGroup  (IntPtr system, ref IntPtr channelgroup);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_GetMasterSoundGroup    (IntPtr system, ref IntPtr soundgroup);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_SetReverbProperties    (IntPtr system, ref REVERB_PROPERTIES prop);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_GetReverbProperties    (IntPtr system, ref REVERB_PROPERTIES prop);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_SetReverbAmbientProperties(IntPtr system, ref REVERB_PROPERTIES prop);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_GetReverbAmbientProperties(IntPtr system, ref REVERB_PROPERTIES prop);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_GetDSPHead             (IntPtr system, ref IntPtr dsp);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_AddDSP                 (IntPtr system, IntPtr dsp, ref IntPtr dspconnection);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_LockDSP                (IntPtr system);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_UnlockDSP              (IntPtr system);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_GetRecordNumDrivers    (IntPtr system, ref int numdrivers);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_GetRecordDriverInfo    (IntPtr system, int id, StringBuilder name, int namelen, ref GUID guid);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_GetRecordPosition      (IntPtr system, int id, ref uint position);
        [DllImport (Version.Dll)]  
        private static extern Result FMOD_System_RecordStart            (IntPtr system, int id, IntPtr sound, bool loop);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_RecordStop             (IntPtr system, int id);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_IsRecording            (IntPtr system, int id, ref bool recording);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_CreateGeometry         (IntPtr system, int maxPolygons, int maxVertices, ref IntPtr geometryf);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_SetGeometrySettings    (IntPtr system, float maxWorldSize);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_GetGeometrySettings    (IntPtr system, ref float maxWorldSize);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_LoadGeometry           (IntPtr system, IntPtr data, int dataSize, ref IntPtr geometry);
        [DllImport (Version.Dll)]               
        private static extern Result FMOD_System_SetNetworkProxy        (IntPtr system, string proxy);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_GetNetworkProxy        (IntPtr system, StringBuilder proxy, int proxylen);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_SetNetworkTimeout      (IntPtr system, int timeout);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_GetNetworkTimeout      (IntPtr system, ref int timeout);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_SetUserData            (IntPtr system, IntPtr userdata);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_GetUserData            (IntPtr system, ref IntPtr userdata);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_System_GetMemoryInfo          (IntPtr system, uint memorybits, uint event_memorybits, ref uint memoryused, IntPtr memoryused_array);

        #endregion

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
    }
    

    /*
        'Sound' API
    */
    public class Sound
    {
        public Result release                 ()
        {
            return FMOD_Sound_Release(soundraw);
        }
        public Result getSystemObject         (ref System system)
        {
            Result Result   = Result.OK;
            IntPtr systemraw   = new IntPtr();
            System systemnew   = null;

            try
            {
                Result = FMOD_Sound_GetSystemObject(soundraw, ref systemraw);
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
                     

        public Result @lock                   (uint offset, uint length, ref IntPtr ptr1, ref IntPtr ptr2, ref uint len1, ref uint len2)
        {
            return FMOD_Sound_Lock(soundraw, offset, length, ref ptr1, ref ptr2, ref len1, ref len2);
        }
        public Result unlock                  (IntPtr ptr1,  IntPtr ptr2, uint len1, uint len2)
        {
            return FMOD_Sound_Unlock(soundraw, ptr1, ptr2, len1, len2);
        }
        public Result setDefaults             (float frequency, float volume, float pan, int priority)
        {
            return FMOD_Sound_SetDefaults(soundraw, frequency, volume, pan, priority);
        }
        public Result getDefaults             (ref float frequency, ref float volume, ref float pan, ref int priority)
        {
            return FMOD_Sound_GetDefaults(soundraw, ref frequency, ref volume, ref pan, ref priority);
        }
        public Result setVariations           (float frequencyvar, float volumevar, float panvar)
        {
            return FMOD_Sound_SetVariations(soundraw, frequencyvar, volumevar, panvar);
        }
        public Result getVariations           (ref float frequencyvar, ref float volumevar, ref float panvar)
        {
            return FMOD_Sound_GetVariations(soundraw, ref frequencyvar, ref volumevar, ref panvar); 
        }
        public Result set3DMinMaxDistance     (float min, float max)
        {
            return FMOD_Sound_Set3DMinMaxDistance(soundraw, min, max);
        }
        public Result get3DMinMaxDistance     (ref float min, ref float max)
        {
            return FMOD_Sound_Get3DMinMaxDistance(soundraw, ref min, ref max);
        }
        public Result set3DConeSettings       (float insideconeangle, float outsideconeangle, float outsidevolume)
        {
            return FMOD_Sound_Set3DConeSettings(soundraw, insideconeangle, outsideconeangle, outsidevolume);
        }
        public Result get3DConeSettings       (ref float insideconeangle, ref float outsideconeangle, ref float outsidevolume)
        {
            return FMOD_Sound_Get3DConeSettings(soundraw, ref insideconeangle, ref outsideconeangle, ref outsidevolume);
        }
        public Result set3DCustomRolloff      (ref Vector points, int numpoints)
        {
            return FMOD_Sound_Set3DCustomRolloff(soundraw, ref points, numpoints);
        }
        public Result get3DCustomRolloff      (ref IntPtr points, ref int numpoints)
        {
            return FMOD_Sound_Get3DCustomRolloff(soundraw, ref points, ref numpoints);
        }
        public Result setSubSound             (int index, Sound subsound)
        {
            IntPtr subsoundraw = subsound.getRaw();

            return FMOD_Sound_SetSubSound(soundraw, index, subsoundraw);
        }
        public Result getSubSound             (int index, ref Sound subsound)
        {
            Result Result       = Result.OK;
            IntPtr subsoundraw  = new IntPtr();
            Sound  subsoundnew  = null;

            try
            {
                Result = FMOD_Sound_GetSubSound(soundraw, index, ref subsoundraw);
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
        public Result setSubSoundSentence     (int[] subsoundlist, int numsubsounds)
        {
            return FMOD_Sound_SetSubSoundSentence(soundraw, subsoundlist, numsubsounds);
        }
        public Result getName                 (StringBuilder name, int namelen)
        {
            return FMOD_Sound_GetName(soundraw, name, namelen);
        }
        public Result getLength               (ref uint length, TIMEUNIT lengthtype)
        {
            return FMOD_Sound_GetLength(soundraw, ref length, lengthtype);
        }
        public Result getFormat               (ref SoundType type, ref SoundFormat format, ref int channels, ref int bits)
        {
            return FMOD_Sound_GetFormat(soundraw, ref type, ref format, ref channels, ref bits);
        }
        public Result getNumSubSounds         (ref int numsubsounds)
        {
            return FMOD_Sound_GetNumSubSounds(soundraw, ref numsubsounds);
        }
        public Result getNumTags              (ref int numtags, ref int numtagsupdated)
        {
            return FMOD_Sound_GetNumTags(soundraw, ref numtags, ref numtagsupdated);
        }
        public Result getTag                  (string name, int index, ref TAG tag)
        {
            IntPtr ptr    = Marshal.AllocCoTaskMem(Marshal.SizeOf(tag));
            Result Result = FMOD_Sound_GetTag(soundraw, name, index, ptr);
            if(Result == Result.OK)
            {
                tag = (TAG)Marshal.PtrToStructure(ptr, typeof(TAG));
            }
            return Result; 
        }
        public Result getOpenState            (ref OPENSTATE openstate, ref uint percentbuffered, ref bool starving)
        {
            return FMOD_Sound_GetOpenState(soundraw, ref openstate, ref percentbuffered, ref starving);
        }
        public Result readData                (IntPtr buffer, uint lenbytes, ref uint read)
        {
            return FMOD_Sound_ReadData(soundraw, buffer, lenbytes, ref read);
        }
        public Result seekData                (uint pcm)
        {
            return FMOD_Sound_SeekData(soundraw, pcm);
        }


        public Result setSoundGroup           (SoundGroup soundgroup)
        {
            return FMOD_Sound_SetSoundGroup(soundraw, soundgroup.getRaw());
        }
        public Result getSoundGroup           (ref SoundGroup soundgroup)
        {
            Result Result = Result.OK;
            IntPtr soundgroupraw = new IntPtr();
            SoundGroup    soundgroupnew = null;

            try
            {
                Result = FMOD_Sound_GetSoundGroup(soundraw, ref soundgroupraw);
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


        public Result getNumSyncPoints        (ref int numsyncpoints)
        {
            return FMOD_Sound_GetNumSyncPoints(soundraw, ref numsyncpoints);
        }
        public Result getSyncPoint            (int index, ref IntPtr point)
        {
            return FMOD_Sound_GetSyncPoint(soundraw, index, ref point);
        }
        public Result getSyncPointInfo        (IntPtr point, StringBuilder name, int namelen, ref uint offset, TIMEUNIT offsettype)
        {
            return FMOD_Sound_GetSyncPointInfo(soundraw, point, name, namelen, ref offset, offsettype);
        }
        public Result addSyncPoint            (int offset, TIMEUNIT offsettype, string name, ref IntPtr point)
        {
            return FMOD_Sound_AddSyncPoint(soundraw, offset, offsettype, name, ref point);
        }
        public Result deleteSyncPoint         (IntPtr point)
        {
            return FMOD_Sound_DeleteSyncPoint(soundraw, point);
        }


		public Result setMode(Mode mode)
        {
            return FMOD_Sound_SetMode(soundraw, mode);
        }
		public Result getMode(ref Mode mode)
        {
            return FMOD_Sound_GetMode(soundraw, ref mode);
        }
        public Result setLoopCount            (int loopcount)
        {
            return FMOD_Sound_SetLoopCount(soundraw, loopcount);
        }
        public Result getLoopCount            (ref int loopcount)
        {
            return FMOD_Sound_GetLoopCount(soundraw, ref loopcount);
        }
        public Result setLoopPoints           (uint loopstart, TIMEUNIT loopstarttype, uint loopend, TIMEUNIT loopendtype)
        {
            return FMOD_Sound_SetLoopPoints(soundraw, loopstart, loopstarttype, loopend, loopendtype);
        }
        public Result getLoopPoints           (ref uint loopstart, TIMEUNIT loopstarttype, ref uint loopend, TIMEUNIT loopendtype)
        {
            return FMOD_Sound_GetLoopPoints(soundraw, ref loopstart, loopstarttype, ref loopend, loopendtype);
        }

        public Result getMusicNumChannels       (ref int numchannels)
        {
            return FMOD_Sound_GetMusicNumChannels(soundraw, ref numchannels);
        }
        public Result setMusicChannelVolume     (int channel, float volume)
        {
            return FMOD_Sound_SetMusicChannelVolume(soundraw, channel, volume);
        }
        public Result getMusicChannelVolume     (int channel, ref float volume)
        {
            return FMOD_Sound_GetMusicChannelVolume(soundraw, channel, ref volume);
        }

        public Result setUserData             (IntPtr userdata)
        {
            return FMOD_Sound_SetUserData(soundraw, userdata);
        }
        public Result getUserData             (ref IntPtr userdata)
        {
            return FMOD_Sound_GetUserData(soundraw, ref userdata);
        }

        public Result getMemoryInfo(uint memorybits, uint event_memorybits, ref uint memoryused, IntPtr memoryused_array)
        {
            return FMOD_Sound_GetMemoryInfo(soundraw, memorybits, event_memorybits, ref memoryused, memoryused_array);
        }


        #region importfunctions

        [DllImport (Version.Dll)]
        private static extern Result FMOD_Sound_Release                 (IntPtr sound);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Sound_GetSystemObject         (IntPtr sound, ref IntPtr system);
        [DllImport (Version.Dll)]                   
        private static extern Result FMOD_Sound_Lock                   (IntPtr sound, uint offset, uint length, ref IntPtr ptr1, ref IntPtr ptr2, ref uint len1, ref uint len2);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Sound_Unlock                  (IntPtr sound, IntPtr ptr1,  IntPtr ptr2, uint len1, uint len2);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Sound_SetDefaults             (IntPtr sound, float frequency, float volume, float pan, int priority);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Sound_GetDefaults             (IntPtr sound, ref float frequency, ref float volume, ref float pan, ref int priority);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Sound_SetVariations           (IntPtr sound, float frequencyvar, float volumevar, float panvar);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Sound_GetVariations           (IntPtr sound, ref float frequencyvar, ref float volumevar, ref float panvar);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Sound_Set3DMinMaxDistance     (IntPtr sound, float min, float max);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Sound_Get3DMinMaxDistance     (IntPtr sound, ref float min, ref float max);
        [DllImport(Version.Dll)]
        private static extern Result FMOD_Sound_Set3DConeSettings       (IntPtr sound, float insideconeangle, float outsideconeangle, float outsidevolume);
        [DllImport(Version.Dll)]
        private static extern Result FMOD_Sound_Get3DConeSettings       (IntPtr sound, ref float insideconeangle, ref float outsideconeangle, ref float outsidevolume);
        [DllImport(Version.Dll)]
        private static extern Result FMOD_Sound_Set3DCustomRolloff      (IntPtr sound, ref Vector points, int numpoints);
        [DllImport(Version.Dll)]
        private static extern Result FMOD_Sound_Get3DCustomRolloff      (IntPtr sound, ref IntPtr points, ref int numpoints);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Sound_SetSubSound             (IntPtr sound, int index, IntPtr subsound);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Sound_GetSubSound             (IntPtr sound, int index, ref IntPtr subsound);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Sound_SetSubSoundSentence     (IntPtr sound, int[] subsoundlist, int numsubsounds);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Sound_GetName                 (IntPtr sound, StringBuilder name, int namelen);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Sound_GetLength               (IntPtr sound, ref uint length, TIMEUNIT lengthtype);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Sound_GetFormat               (IntPtr sound, ref SoundType type, ref SoundFormat format, ref int channels, ref int bits);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Sound_GetNumSubSounds         (IntPtr sound, ref int numsubsounds);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Sound_GetNumTags              (IntPtr sound, ref int numtags, ref int numtagsupdated);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Sound_GetTag                  (IntPtr sound, string name, int index, IntPtr tag);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Sound_GetOpenState            (IntPtr sound, ref OPENSTATE openstate, ref uint percentbuffered, ref bool starving);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Sound_ReadData                (IntPtr sound, IntPtr buffer, uint lenbytes, ref uint read);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Sound_SeekData                (IntPtr sound, uint pcm);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Sound_SetSoundGroup           (IntPtr sound, IntPtr soundgroup);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Sound_GetSoundGroup           (IntPtr sound, ref IntPtr soundgroup);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Sound_GetNumSyncPoints        (IntPtr sound, ref int numsyncpoints);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Sound_GetSyncPoint            (IntPtr sound, int index, ref IntPtr point);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Sound_GetSyncPointInfo        (IntPtr sound, IntPtr point, StringBuilder name, int namelen, ref uint offset, TIMEUNIT offsettype);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Sound_AddSyncPoint            (IntPtr sound, int offset, TIMEUNIT offsettype, string name, ref IntPtr point);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Sound_DeleteSyncPoint         (IntPtr sound, IntPtr point);
        [DllImport (Version.Dll)]
		private static extern Result FMOD_Sound_SetMode(IntPtr sound, Mode mode);
        [DllImport (Version.Dll)]
		private static extern Result FMOD_Sound_GetMode(IntPtr sound, ref Mode mode);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Sound_SetLoopCount            (IntPtr sound, int loopcount);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Sound_GetLoopCount            (IntPtr sound, ref int loopcount);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Sound_SetLoopPoints           (IntPtr sound, uint loopstart, TIMEUNIT loopstarttype, uint loopend, TIMEUNIT loopendtype);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Sound_GetLoopPoints           (IntPtr sound, ref uint loopstart, TIMEUNIT loopstarttype, ref uint loopend, TIMEUNIT loopendtype);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Sound_GetMusicNumChannels     (IntPtr sound, ref int numchannels);
        [DllImport(Version.Dll)]
        private static extern Result FMOD_Sound_SetMusicChannelVolume   (IntPtr sound, int channel, float volume);
        [DllImport(Version.Dll)]
        private static extern Result FMOD_Sound_GetMusicChannelVolume   (IntPtr sound, int channel, ref float volume);
        [DllImport(Version.Dll)]           
        private static extern Result FMOD_Sound_SetUserData             (IntPtr sound, IntPtr userdata);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Sound_GetUserData             (IntPtr sound, ref IntPtr userdata);
        [DllImport(Version.Dll)]
        private static extern Result FMOD_Sound_GetMemoryInfo           (IntPtr sound, uint memorybits, uint event_memorybits, ref uint memoryused, IntPtr memoryused_array);
        #endregion

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
    }


    /*
        'Channel' API
    */
    public class Channel
    {
        public Result getSystemObject       (ref System system)
        {
            Result Result   = Result.OK;
            IntPtr systemraw   = new IntPtr();
            System systemnew   = null;

            try
            {
                Result = FMOD_Channel_GetSystemObject(channelraw, ref systemraw);
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


        public Result stop                  ()
        {
            return FMOD_Channel_Stop(channelraw);
        }
        public Result setPaused             (bool paused)
        {
            return FMOD_Channel_SetPaused(channelraw, (paused ? 1 : 0));
        }
        public Result getPaused             (ref bool paused)
        {
            Result Result;
            int p = 0;

            Result = FMOD_Channel_GetPaused(channelraw, ref p);

            paused = (p != 0);

            return Result;
        }
        public Result setVolume             (float volume)
        {
            return FMOD_Channel_SetVolume(channelraw, volume);
        }
        public Result getVolume             (ref float volume)
        {
            return FMOD_Channel_GetVolume(channelraw, ref volume);
        }
        public Result setFrequency          (float frequency)
        {
            return FMOD_Channel_SetFrequency(channelraw, frequency);
        }
        public Result getFrequency          (ref float frequency)
        {
            return FMOD_Channel_GetFrequency(channelraw, ref frequency);
        }
        public Result setPan                (float pan)
        {
            return FMOD_Channel_SetPan(channelraw, pan);
        }
        public Result getPan                (ref float pan)
        {
            return FMOD_Channel_GetPan(channelraw, ref pan);
        }
        public Result setDelay              (DELAYTYPE delaytype, uint delayhi, uint delaylo)
        {
            return FMOD_Channel_SetDelay(channelraw, delaytype, delayhi, delaylo);
        }
        public Result getDelay              (DELAYTYPE delaytype, ref uint delayhi, ref uint delaylo)
        {
            return FMOD_Channel_GetDelay(channelraw, delaytype, ref delayhi, ref delaylo);
        }
        public Result setSpeakerMix         (float frontleft, float frontright, float center, float lfe, float backleft, float backright, float sideleft, float sideright)
        {
            return FMOD_Channel_SetSpeakerMix(channelraw, frontleft, frontright, center, lfe, backleft, backright, sideleft, sideright);
        }
        public Result getSpeakerMix         (ref float frontleft, ref float frontright, ref float center, ref float lfe, ref float backleft, ref float backright, ref float sideleft, ref float sideright)
        {
            return FMOD_Channel_GetSpeakerMix(channelraw, ref frontleft, ref frontright, ref center, ref lfe, ref backleft, ref backright, ref sideleft, ref sideright);
        }
        public Result setSpeakerLevels      (Speaker speaker, float[] levels, int numlevels)
        {
            return FMOD_Channel_SetSpeakerLevels(channelraw, speaker, levels, numlevels);
        }
        public Result getSpeakerLevels      (Speaker speaker, float[] levels, int numlevels)
        {
            return FMOD_Channel_GetSpeakerLevels(channelraw, speaker, levels, numlevels);
        }
        public Result setInputChannelMix    (ref float levels, int numlevels)
        {
            return FMOD_Channel_SetInputChannelMix(channelraw, ref levels, numlevels);
        }
        public Result getInputChannelMix    (ref float levels, int numlevels)
        {
            return FMOD_Channel_GetInputChannelMix(channelraw, ref levels, numlevels);
        }
        public Result setMute               (bool mute)
        {
            return FMOD_Channel_SetMute(channelraw, (mute ? 1 : 0));
        }
        public Result getMute               (ref bool mute)
        {
            Result Result;
            int m = 0;

            Result = FMOD_Channel_GetMute(channelraw, ref m);

            mute = (m != 0);

            return Result;
        }
        public Result setPriority           (int priority)
        {
            return FMOD_Channel_SetPriority(channelraw, priority);
        }
        public Result getPriority           (ref int priority)
        {
            return FMOD_Channel_GetPriority(channelraw, ref priority);
        }
        public Result setPosition           (uint position, TIMEUNIT postype)
        {
            return FMOD_Channel_SetPosition(channelraw, position, postype);
        }
        public Result getPosition           (ref uint position, TIMEUNIT postype)
        {
            return FMOD_Channel_GetPosition(channelraw, ref position, postype);
        }
        
        public Result setReverbProperties   (ref REVERB_CHANNELPROPERTIES prop)
        {
            return FMOD_Channel_SetReverbProperties(channelraw, ref prop);
        }
        public Result getReverbProperties   (ref REVERB_CHANNELPROPERTIES prop)
        {
            return FMOD_Channel_GetReverbProperties(channelraw, ref prop);
        }
        public Result setChannelGroup       (ChannelGroup channelgroup)
        {
            return FMOD_Channel_SetChannelGroup(channelraw, channelgroup.getRaw());
        }
        public Result getChannelGroup        (ref ChannelGroup channelgroup)
        {
            Result Result = Result.OK;
            IntPtr channelgroupraw = new IntPtr();
            ChannelGroup    channelgroupnew = null;

            try
            {
                Result = FMOD_Channel_GetChannelGroup(channelraw, ref channelgroupraw);
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

        public Result setCallback           (CallBackDelegate callback)
        {
            return FMOD_Channel_SetCallback(channelraw, callback);
        }
        public delegate Result CallBackDelegate(IntPtr channel, CHANNEL_CALLBACKTYPE type, int commandData1, int commandData2);
       
        public Result set3DAttributes       (ref Vector pos, ref Vector vel)
        {
            return FMOD_Channel_Set3DAttributes(channelraw, ref pos, ref vel);
        }
        public Result get3DAttributes       (ref Vector pos, ref Vector vel)
        {
            return FMOD_Channel_Get3DAttributes(channelraw, ref pos, ref vel);
        }
        public Result set3DMinMaxDistance   (float mindistance, float maxdistance)
        {
            return FMOD_Channel_Set3DMinMaxDistance(channelraw, mindistance, maxdistance);
        }
        public Result get3DMinMaxDistance   (ref float mindistance, ref float maxdistance)
        {
            return FMOD_Channel_Get3DMinMaxDistance(channelraw, ref mindistance, ref maxdistance);
        }
        public Result set3DConeSettings     (float insideconeangle, float outsideconeangle, float outsidevolume)
        {
            return FMOD_Channel_Set3DConeSettings(channelraw, insideconeangle, outsideconeangle, outsidevolume);
        }
        public Result get3DConeSettings     (ref float insideconeangle, ref float outsideconeangle, ref float outsidevolume)
        {
            return FMOD_Channel_Get3DConeSettings(channelraw, ref insideconeangle, ref outsideconeangle, ref outsidevolume);
        }
        public Result set3DConeOrientation  (ref Vector orientation)
        {
            return FMOD_Channel_Set3DConeOrientation(channelraw, ref orientation);
        }
        public Result get3DConeOrientation  (ref Vector orientation)
        {
            return FMOD_Channel_Get3DConeOrientation(channelraw, ref orientation);
        }
        public Result set3DCustomRolloff    (ref Vector points, int numpoints)
        {
            return FMOD_Channel_Set3DCustomRolloff(channelraw, ref points, numpoints);
        }
        public Result get3DCustomRolloff    (ref IntPtr points, ref int numpoints)
        {
            return FMOD_Channel_Get3DCustomRolloff(channelraw, ref points, ref numpoints);
        }
        public Result set3DOcclusion        (float directOcclusion, float reverbOcclusion)
        {
            return FMOD_Channel_Set3DOcclusion(channelraw, directOcclusion, reverbOcclusion);
        }
        public Result get3DOcclusion        (ref float directOcclusion, ref float reverbOcclusion)
        {
            return FMOD_Channel_Get3DOcclusion(channelraw, ref directOcclusion, ref reverbOcclusion);
        }
        public Result set3DSpread           (float angle)
        {
            return FMOD_Channel_Set3DSpread(channelraw, angle);
        }
        public Result get3DSpread           (ref float angle)
        {
            return FMOD_Channel_Get3DSpread(channelraw, ref angle);
        }
        public Result set3DPanLevel         (float level)
        {
            return FMOD_Channel_Set3DPanLevel(channelraw, level);
        }
        public Result get3DPanLevel         (ref float level)
        {
            return FMOD_Channel_Get3DPanLevel(channelraw, ref level);
        }
        public Result set3DDopplerLevel     (float level)
        {
            return FMOD_Channel_Set3DDopplerLevel(channelraw, level);
        }
        public Result get3DDopplerLevel     (ref float level)
        {
            return FMOD_Channel_Get3DDopplerLevel(channelraw, ref level);
        }

        public Result isPlaying             (ref bool isplaying)
        {
            return FMOD_Channel_IsPlaying(channelraw, ref isplaying);
        }
        public Result isVirtual             (ref bool isvirtual)
        {
            return FMOD_Channel_IsVirtual(channelraw, ref isvirtual);
        }
        public Result getAudibility         (ref float audibility)
        {
            return FMOD_Channel_GetAudibility(channelraw, ref audibility);
        }
        public Result getCurrentSound       (ref Sound sound)
        {
            Result Result      = Result.OK;
            IntPtr soundraw    = new IntPtr();
            Sound  soundnew    = null;

            try
            {
                Result = FMOD_Channel_GetCurrentSound(channelraw, ref soundraw);
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
        public Result getSpectrum           (float[] spectrumarray, int numvalues, int channeloffset, DSP_FFT_WINDOW windowtype)
        {
            return FMOD_Channel_GetSpectrum(channelraw, spectrumarray, numvalues, channeloffset, windowtype);
        }
        public Result getWaveData           (float[] wavearray, int numvalues, int channeloffset)
        {
            return FMOD_Channel_GetWaveData(channelraw, wavearray, numvalues, channeloffset);
        }
        public Result getIndex              (ref int index)
        {
            return FMOD_Channel_GetIndex(channelraw, ref index);
        }

        /* public Result getDSPHead            (ref DSP dsp)
        {
            Result Result      = Result.OK;
            IntPtr dspraw      = new IntPtr();
            DSP    dspnew      = null;

            try
            {
                Result = FMOD_Channel_GetDSPHead(channelraw, ref dspraw);
            }
            catch
            {
                Result = Result.ERR_INVALID_PARAM;
            }
            if (Result != Result.OK)
            {
                return Result;
            }

            dspnew = new DSP();
            dspnew.setRaw(dspraw);
            dsp = dspnew;

            return Result; 
        }
        public Result addDSP                (DSP dsp, ref DSPConnection dspconnection)
        {
            Result Result = Result.OK;
            IntPtr dspconnectionraw = new IntPtr();
            DSPConnection dspconnectionnew = null;

            try
            {
                Result = FMOD_Channel_AddDSP(channelraw, dsp.getRaw(), ref dspconnectionraw);
            }
            catch
            {
                Result = Result.ERR_INVALID_PARAM;
            }
            if (Result != Result.OK)
            {
                return Result;
            }

            if (dspconnection == null)
            {
                dspconnectionnew = new DSPConnection();
                dspconnectionnew.setRaw(dspconnectionraw);
                dspconnection = dspconnectionnew;
            }
            else
            {
                dspconnection.setRaw(dspconnectionraw);
            }

            return Result;
        } */


		public Result setMode(Mode mode)
        {
            return FMOD_Channel_SetMode(channelraw, mode);
        }
		public Result getMode(ref Mode mode)
        {
            return FMOD_Channel_GetMode(channelraw, ref mode);
        }
        public Result setLoopCount          (int loopcount)
        {
            return FMOD_Channel_SetLoopCount(channelraw, loopcount);
        }
        public Result getLoopCount          (ref int loopcount)
        {
            return FMOD_Channel_GetLoopCount(channelraw, ref loopcount);
        }
        public Result setLoopPoints         (uint loopstart, TIMEUNIT loopstarttype, uint loopend, TIMEUNIT loopendtype)
        {
            return FMOD_Channel_SetLoopPoints(channelraw, loopstart, loopstarttype, loopend, loopendtype);
        }
        public Result getLoopPoints         (ref uint loopstart, TIMEUNIT loopstarttype, ref uint loopend, TIMEUNIT loopendtype)
        {
            return FMOD_Channel_GetLoopPoints(channelraw, ref loopstart, loopstarttype, ref loopend, loopendtype);
        }


        public Result setUserData           (IntPtr userdata)
        {
            return FMOD_Channel_SetUserData(channelraw, userdata);
        }
        public Result getUserData           (ref IntPtr userdata)
        {
            return FMOD_Channel_GetUserData(channelraw, ref userdata);
        }

        public Result getMemoryInfo(uint memorybits, uint event_memorybits, ref uint memoryused, IntPtr memoryused_array)
        {
            return FMOD_Channel_GetMemoryInfo(channelraw, memorybits, event_memorybits, ref memoryused, memoryused_array);
        }

        #region importfunctions

        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_GetSystemObject       (IntPtr channel, ref IntPtr system);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_Stop                  (IntPtr channel);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_SetPaused             (IntPtr channel, int paused);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_GetPaused             (IntPtr channel, ref int paused);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_SetVolume             (IntPtr channel, float volume);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_GetVolume             (IntPtr channel, ref float volume);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_SetFrequency          (IntPtr channel, float frequency);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_GetFrequency          (IntPtr channel, ref float frequency);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_SetPan                (IntPtr channel, float pan);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_GetPan                (IntPtr channel, ref float pan);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_SetDelay              (IntPtr channel, DELAYTYPE delaytype, uint delayhi, uint delaylo);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_GetDelay              (IntPtr channel, DELAYTYPE delaytype, ref uint delayhi, ref uint delaylo);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_SetSpeakerMix         (IntPtr channel, float frontleft, float frontright, float center, float lfe, float backleft, float backright, float sideleft, float sideright);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_GetSpeakerMix         (IntPtr channel, ref float frontleft, ref float frontright, ref float center, ref float lfe, ref float backleft, ref float backright, ref float sideleft, ref float sideright);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_SetSpeakerLevels      (IntPtr channel, Speaker speaker, float[] levels, int numlevels);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_GetSpeakerLevels      (IntPtr channel, Speaker speaker, [MarshalAs(UnmanagedType.LPArray)]float[] levels, int numlevels);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_SetInputChannelMix    (IntPtr channel, ref float levels, int numlevels);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_GetInputChannelMix    (IntPtr channel, ref float levels, int numlevels);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_SetMute               (IntPtr channel, int mute);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_GetMute               (IntPtr channel, ref int mute);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_SetPriority           (IntPtr channel, int priority);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_GetPriority           (IntPtr channel, ref int priority);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_Set3DAttributes       (IntPtr channel, ref Vector pos, ref Vector vel);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_Get3DAttributes       (IntPtr channel, ref Vector pos, ref Vector vel);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_Set3DMinMaxDistance   (IntPtr channel, float mindistance, float maxdistance);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_Get3DMinMaxDistance   (IntPtr channel, ref float mindistance, ref float maxdistance);
        [DllImport (Version.Dll)]        
        private static extern Result FMOD_Channel_Set3DConeSettings     (IntPtr channel, float insideconeangle, float outsideconeangle, float outsidevolume);
        [DllImport (Version.Dll)] 
        private static extern Result FMOD_Channel_Get3DConeSettings     (IntPtr channel, ref float insideconeangle, ref float outsideconeangle, ref float outsidevolume);
        [DllImport (Version.Dll)] 
        private static extern Result FMOD_Channel_Set3DConeOrientation  (IntPtr channel, ref Vector orientation);
        [DllImport (Version.Dll)] 
        private static extern Result FMOD_Channel_Get3DConeOrientation  (IntPtr channel, ref Vector orientation);
        [DllImport (Version.Dll)] 
        private static extern Result FMOD_Channel_Set3DCustomRolloff    (IntPtr channel, ref Vector points, int numpoints);
        [DllImport (Version.Dll)] 
        private static extern Result FMOD_Channel_Get3DCustomRolloff    (IntPtr channel, ref IntPtr points, ref int numpoints);
        [DllImport (Version.Dll)] 
        private static extern Result FMOD_Channel_Set3DOcclusion        (IntPtr channel, float directOcclusion, float reverbOcclusion);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_Get3DOcclusion        (IntPtr channel, ref float directOcclusion, ref float reverbOcclusion);
        [DllImport (Version.Dll)]          
        private static extern Result FMOD_Channel_Set3DSpread           (IntPtr channel, float angle);
        [DllImport (Version.Dll)]    
        private static extern Result FMOD_Channel_Get3DSpread           (IntPtr channel, ref float angle);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_Set3DPanLevel         (IntPtr channel, float level);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_Get3DPanLevel         (IntPtr channel, ref float level);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_Set3DDopplerLevel     (IntPtr channel, float level);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_Get3DDopplerLevel     (IntPtr channel, ref float level);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_SetReverbProperties   (IntPtr channel, ref REVERB_CHANNELPROPERTIES prop);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_GetReverbProperties   (IntPtr channel, ref REVERB_CHANNELPROPERTIES prop);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_SetChannelGroup       (IntPtr channel, IntPtr channelgroup);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_GetChannelGroup       (IntPtr channel, ref IntPtr channelgroup);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_IsPlaying             (IntPtr channel, ref bool isplaying);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_IsVirtual             (IntPtr channel, ref bool isvirtual);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_GetAudibility         (IntPtr channel, ref float audibility);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_GetCurrentSound       (IntPtr channel, ref IntPtr sound);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_GetSpectrum           (IntPtr channel, [MarshalAs(UnmanagedType.LPArray)] float[] spectrumarray, int numvalues, int channeloffset, DSP_FFT_WINDOW windowtype);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_GetWaveData           (IntPtr channel, [MarshalAs(UnmanagedType.LPArray)] float[] wavearray, int numvalues, int channeloffset);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_GetIndex              (IntPtr channel, ref int index);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_SetCallback           (IntPtr channel, CallBackDelegate callback);
        //private static extern Result FMOD_Channel_SetCallback           (IntPtr channel, CHANNEL_CALLBACK callback);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_SetPosition           (IntPtr channel, uint position, TIMEUNIT postype);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_GetPosition           (IntPtr channel, ref uint position, TIMEUNIT postype);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_GetDSPHead            (IntPtr channel, ref IntPtr dsp);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_AddDSP                (IntPtr channel, IntPtr dsp, ref IntPtr dspconnection);
        [DllImport (Version.Dll)]
		private static extern Result FMOD_Channel_SetMode(IntPtr channel, Mode mode);
        [DllImport (Version.Dll)]
		private static extern Result FMOD_Channel_GetMode(IntPtr channel, ref Mode mode);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_SetLoopCount          (IntPtr channel, int loopcount);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_GetLoopCount          (IntPtr channel, ref int loopcount);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_SetLoopPoints         (IntPtr channel, uint  loopstart, TIMEUNIT loopstarttype, uint  loopend, TIMEUNIT loopendtype);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_GetLoopPoints         (IntPtr channel, ref uint loopstart, TIMEUNIT loopstarttype, ref uint loopend, TIMEUNIT loopendtype);
        [DllImport (Version.Dll)]                                        
        private static extern Result FMOD_Channel_SetUserData           (IntPtr channel, IntPtr userdata);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Channel_GetUserData           (IntPtr channel, ref IntPtr userdata);
        [DllImport(Version.Dll)]
        private static extern Result FMOD_Channel_GetMemoryInfo         (IntPtr channel, uint memorybits, uint event_memorybits, ref uint memoryused, IntPtr memoryused_array);
        #endregion
        
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
    }


    /*
        'ChannelGroup' API
    */
    public class ChannelGroup
    {
        public Result release                ()
        {
            return FMOD_ChannelGroup_Release(channelgroupraw);
        }
        public Result getSystemObject        (ref System system)
        {
            Result Result = Result.OK;
            IntPtr systemraw = new IntPtr();
            System systemnew = null;

            try
            {
                Result = FMOD_ChannelGroup_GetSystemObject(channelgroupraw, ref systemraw);
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


        // Channelgroup scale values.  (scales the current volume or pitch of all channels and channel groups, DOESN'T overwrite)
        public Result setVolume              (float volume)
        {
            return FMOD_ChannelGroup_SetVolume(channelgroupraw, volume);
        }
        public Result getVolume              (ref float volume)
        {
            return FMOD_ChannelGroup_GetVolume(channelgroupraw, ref volume);
        }
        public Result setPitch               (float pitch)
        {
            return FMOD_ChannelGroup_SetPitch(channelgroupraw, pitch);
        }
        public Result getPitch               (ref float pitch)
        {
            return FMOD_ChannelGroup_GetPitch(channelgroupraw, ref pitch);
        }

        public Result setPaused              (bool paused)
        {
            return FMOD_ChannelGroup_SetPaused(channelgroupraw, (paused ? 1 : 0));
        }
        public Result getPaused              (ref bool paused)
        {
            Result Result;
            int p = 0;

            Result = FMOD_ChannelGroup_GetPaused(channelgroupraw, ref p);

            paused = (p != 0);

            return Result;
        }
        public Result setMute                (bool mute)
        {
            return FMOD_ChannelGroup_SetMute(channelgroupraw, (mute ? 1 : 0));
        }
        public Result getMute                (ref bool mute)
        {
            Result Result;
            int m = 0;

            Result = FMOD_ChannelGroup_GetMute(channelgroupraw, ref m);
            
            mute = (m != 0);

            return Result;
        }


        // Channelgroup override values.  (recursively overwrites whatever settings the channels had)
        public Result stop                   ()
        {
            return FMOD_ChannelGroup_Stop(channelgroupraw);
        }
        public Result overrideVolume         (float volume)
        {
            return FMOD_ChannelGroup_OverrideVolume(channelgroupraw, volume);
        }
        public Result overrideFrequency      (float frequency)
        {
            return FMOD_ChannelGroup_OverrideFrequency(channelgroupraw, frequency);
        }
        public Result overridePan            (float pan)
        {
            return FMOD_ChannelGroup_OverridePan(channelgroupraw, pan);
        }
        public Result overrideReverbProperties (ref REVERB_CHANNELPROPERTIES prop)
        {
            return FMOD_ChannelGroup_OverrideReverbProperties(channelgroupraw, ref prop);
        }
        public Result override3DAttributes   (ref Vector pos, ref Vector vel)
        {
            return FMOD_ChannelGroup_Override3DAttributes(channelgroupraw, ref pos, ref vel);
        }
        public Result overrideSpeakerMix     (float frontleft, float frontright, float center, float lfe, float backleft, float backright, float sideleft, float sideright)
        {
            return FMOD_ChannelGroup_OverrideSpeakerMix(channelgroupraw, frontleft, frontright, center, lfe, backleft, backright, sideleft, sideright);
        }


        // Nested channel groups.
        public Result addGroup               (ChannelGroup group)
        {
            return FMOD_ChannelGroup_AddGroup(channelgroupraw, group.getRaw());
        }
        public Result getNumGroups           (ref int numgroups)
        {
            return FMOD_ChannelGroup_GetNumGroups(channelgroupraw, ref numgroups);
        }
        public Result getGroup               (int index, ref ChannelGroup group)
        {
            Result Result = Result.OK;
            IntPtr channelraw = new IntPtr();
            ChannelGroup    channelnew = null;

            try
            {
                Result = FMOD_ChannelGroup_GetGroup(channelgroupraw, index, ref channelraw);
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
        public Result getParentGroup         (ref ChannelGroup group)
        {
            Result Result = Result.OK;
            IntPtr channelraw = new IntPtr();
            ChannelGroup    channelnew = null;

            try
            {
                Result = FMOD_ChannelGroup_GetParentGroup(channelgroupraw, ref channelraw);
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


        // DSP functionality only for channel groups playing sounds created with FMOD_SOFTWARE.
        /* public Result getDSPHead             (ref DSP dsp)
        {
            Result Result = Result.OK;
            IntPtr dspraw = new IntPtr();
            DSP    dspnew = null;

            try
            {
                Result = FMOD_ChannelGroup_GetDSPHead(channelgroupraw, ref dspraw);
            }
            catch
            {
                Result = Result.ERR_INVALID_PARAM;
            }
            if (Result != Result.OK)
            {
                return Result;
            }

            if (dsp == null)
            {
                dspnew = new DSP();
                dspnew.setRaw(dspraw);
                dsp = dspnew;
            }
            else
            {
                dsp.setRaw(dspraw);
            }
                             
            return Result; 
        }

        public Result addDSP                 (DSP dsp, ref DSPConnection dspconnection)
        {
            Result Result = Result.OK;
            IntPtr dspconnectionraw = new IntPtr();
            DSPConnection dspconnectionnew = null;

            try
            {
                Result = FMOD_ChannelGroup_AddDSP(channelgroupraw, dsp.getRaw(), ref dspconnectionraw);
            }
            catch
            {
                Result = Result.ERR_INVALID_PARAM;
            }
            if (Result != Result.OK)
            {
                return Result;
            }

            if (dspconnection == null)
            {
                dspconnectionnew = new DSPConnection();
                dspconnectionnew.setRaw(dspconnectionraw);
                dspconnection = dspconnectionnew;
            }
            else
            {
                dspconnection.setRaw(dspconnectionraw);
            }

            return Result;
        } */


        // Information only functions.
        public Result getName                (StringBuilder name, int namelen)
        {
            return FMOD_ChannelGroup_GetName(channelgroupraw, name, namelen);
        }
        public Result getNumChannels         (ref int numchannels)
        {
            return FMOD_ChannelGroup_GetNumChannels(channelgroupraw, ref numchannels);
        }
        public Result getChannel             (int index, ref Channel channel)
        {
            Result Result = Result.OK;
            IntPtr channelraw = new IntPtr();
            Channel    channelnew = null;

            try
            {
                Result = FMOD_ChannelGroup_GetChannel(channelgroupraw, index, ref channelraw);
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
        public Result getSpectrum            (float[] spectrumarray, int numvalues, int channeloffset, DSP_FFT_WINDOW windowtype)
        {
            return FMOD_ChannelGroup_GetSpectrum(channelgroupraw, spectrumarray, numvalues, channeloffset, windowtype);
        }
        public Result getWaveData            (float[] wavearray, int numvalues, int channeloffset)
        {
            return FMOD_ChannelGroup_GetWaveData(channelgroupraw, wavearray, numvalues, channeloffset);
        }


        // Userdata set/get.
        public Result setUserData            (IntPtr userdata)
        {
            return FMOD_ChannelGroup_SetUserData(channelgroupraw, userdata);
        }
        public Result getUserData            (ref IntPtr userdata)
        {
            return FMOD_ChannelGroup_GetUserData(channelgroupraw, ref userdata);
        }

        public Result getMemoryInfo(uint memorybits, uint event_memorybits, ref uint memoryused, IntPtr memoryused_array)
        {
            return FMOD_ChannelGroup_GetMemoryInfo(channelgroupraw, memorybits, event_memorybits, ref memoryused, memoryused_array);
        }

        #region importfunctions


        [DllImport (Version.Dll)]
        private static extern Result FMOD_ChannelGroup_Release          (IntPtr channelgroup);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_ChannelGroup_GetSystemObject  (IntPtr channelgroup, ref IntPtr system);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_ChannelGroup_SetVolume        (IntPtr channelgroup, float volume);
        [DllImport (Version.Dll)]        
        private static extern Result FMOD_ChannelGroup_GetVolume        (IntPtr channelgroup, ref float volume);
        [DllImport (Version.Dll)]       
        private static extern Result FMOD_ChannelGroup_SetPitch         (IntPtr channelgroup, float pitch);
        [DllImport (Version.Dll)]       
        private static extern Result FMOD_ChannelGroup_GetPitch         (IntPtr channelgroup, ref float pitch);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_ChannelGroup_SetPaused        (IntPtr channelgroup, int paused);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_ChannelGroup_GetPaused        (IntPtr channelgroup, ref int paused);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_ChannelGroup_SetMute          (IntPtr channelgroup, int mute);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_ChannelGroup_GetMute          (IntPtr channelgroup, ref int mute);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_ChannelGroup_Stop             (IntPtr channelgroup);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_ChannelGroup_OverridePaused   (IntPtr channelgroup, bool paused);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_ChannelGroup_OverrideVolume   (IntPtr channelgroup, float volume);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_ChannelGroup_OverrideFrequency(IntPtr channelgroup, float frequency);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_ChannelGroup_OverridePan      (IntPtr channelgroup, float pan);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_ChannelGroup_OverrideMute     (IntPtr channelgroup, bool mute);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_ChannelGroup_OverrideReverbProperties(IntPtr channelgroup, ref REVERB_CHANNELPROPERTIES prop);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_ChannelGroup_Override3DAttributes  (IntPtr channelgroup, ref Vector pos, ref Vector vel);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_ChannelGroup_OverrideSpeakerMix(IntPtr channelgroup, float frontleft, float frontright, float center, float lfe, float backleft, float backright, float sideleft, float sideright);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_ChannelGroup_AddGroup         (IntPtr channelgroup, IntPtr group);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_ChannelGroup_GetNumGroups     (IntPtr channelgroup, ref int numgroups);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_ChannelGroup_GetGroup         (IntPtr channelgroup, int index, ref IntPtr group);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_ChannelGroup_GetParentGroup   (IntPtr channelgroup, ref IntPtr group);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_ChannelGroup_GetDSPHead       (IntPtr channelgroup, ref IntPtr dsp);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_ChannelGroup_AddDSP           (IntPtr channelgroup, IntPtr dsp, ref IntPtr dspconnection);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_ChannelGroup_GetName          (IntPtr channelgroup, StringBuilder name, int namelen);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_ChannelGroup_GetNumChannels   (IntPtr channelgroup, ref int numchannels);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_ChannelGroup_GetChannel       (IntPtr channelgroup, int index, ref IntPtr channel);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_ChannelGroup_GetSpectrum      (IntPtr channelgroup, [MarshalAs(UnmanagedType.LPArray)] float[] spectrumarray, int numvalues, int channeloffset, DSP_FFT_WINDOW windowtype);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_ChannelGroup_GetWaveData      (IntPtr channelgroup, [MarshalAs(UnmanagedType.LPArray)] float[] wavearray, int numvalues, int channeloffset);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_ChannelGroup_SetUserData      (IntPtr channelgroup, IntPtr userdata);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_ChannelGroup_GetUserData      (IntPtr channelgroup, ref IntPtr userdata);
        [DllImport(Version.Dll)]
        private static extern Result FMOD_ChannelGroup_GetMemoryInfo    (IntPtr channelgroup, uint memorybits, uint event_memorybits, ref uint memoryused, IntPtr memoryused_array);
        #endregion

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
    }


    /*
        'SoundGroup' API
    */
    public class SoundGroup
    {
        public Result release                ()
        {
            return FMOD_SoundGroup_Release(soundgroupraw);
        }

        public Result getSystemObject        (ref System system)
        {
            Result Result         = Result.OK;
            IntPtr systemraw      = new IntPtr();
            System systemnew      = null;

            try
            {
                Result = FMOD_SoundGroup_GetSystemObject(soundgroupraw, ref systemraw);
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

        // SoundGroup control functions.
        public Result setMaxAudible          (int maxaudible)
        {
            return FMOD_SoundGroup_SetMaxAudible(soundgroupraw, maxaudible);
        }

        public Result getMaxAudible          (ref int maxaudible)
        {
            return FMOD_SoundGroup_GetMaxAudible(soundgroupraw, ref maxaudible);
        }

        public Result setMaxAudibleBehavior  (SOUNDGROUP_BEHAVIOR behavior)
        {
            return FMOD_SoundGroup_SetMaxAudibleBehavior(soundgroupraw, behavior);
        }
        public Result getMaxAudibleBehavior  (ref SOUNDGROUP_BEHAVIOR behavior)
        {
            return FMOD_SoundGroup_GetMaxAudibleBehavior(soundgroupraw, ref behavior);
        }
        public Result setMuteFadeSpeed       (float speed)
        {
            return FMOD_SoundGroup_SetMuteFadeSpeed(soundgroupraw, speed);
        }
        public Result getMuteFadeSpeed       (ref float speed)
        {
            return FMOD_SoundGroup_GetMuteFadeSpeed(soundgroupraw, ref speed);
        }

        // Information only functions.
        public Result getName                (StringBuilder name, int namelen)
        {
            return FMOD_SoundGroup_GetName(soundgroupraw, name, namelen);
        }
        public Result getNumSounds           (ref int numsounds)
        {
            return FMOD_SoundGroup_GetNumSounds(soundgroupraw, ref numsounds);
        }
        public Result getSound               (int index, ref Sound sound)
        {
            Result Result         = Result.OK;
            IntPtr soundraw      = new IntPtr();
            Sound soundnew      = null;

            try
            {
                Result = FMOD_SoundGroup_GetSound(soundgroupraw, index, ref soundraw);
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
        public Result getNumPlaying          (ref int numplaying)
        {
            return FMOD_SoundGroup_GetNumPlaying(soundgroupraw, ref numplaying);
        }

        // Userdata set/get.
        public Result setUserData            (IntPtr userdata)
        {
            return FMOD_SoundGroup_SetUserData(soundgroupraw, userdata);
        }
        public Result getUserData            (ref IntPtr userdata)
        {
            return FMOD_SoundGroup_GetUserData(soundgroupraw, ref userdata);
        }

        public Result getMemoryInfo(uint memorybits, uint event_memorybits, ref uint memoryused, IntPtr memoryused_array)
        {
            return FMOD_SoundGroup_GetMemoryInfo(soundgroupraw, memorybits, event_memorybits, ref memoryused, memoryused_array);
        }

        #region importfunctions
        [DllImport (Version.Dll)]
        private static extern Result FMOD_SoundGroup_Release            (IntPtr soundgroupraw);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_SoundGroup_GetSystemObject    (IntPtr soundgroupraw, ref IntPtr system);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_SoundGroup_SetMaxAudible      (IntPtr soundgroupraw, int maxaudible);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_SoundGroup_GetMaxAudible      (IntPtr soundgroupraw, ref int maxaudible);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_SoundGroup_SetMaxAudibleBehavior(IntPtr soundgroupraw, SOUNDGROUP_BEHAVIOR behavior);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_SoundGroup_GetMaxAudibleBehavior(IntPtr soundgroupraw, ref SOUNDGROUP_BEHAVIOR behavior);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_SoundGroup_SetMuteFadeSpeed   (IntPtr soundgroupraw, float speed);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_SoundGroup_GetMuteFadeSpeed   (IntPtr soundgroupraw, ref float speed);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_SoundGroup_GetName            (IntPtr soundgroupraw, StringBuilder name, int namelen);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_SoundGroup_GetNumSounds       (IntPtr soundgroupraw, ref int numsounds);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_SoundGroup_GetSound           (IntPtr soundgroupraw, int index, ref IntPtr sound);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_SoundGroup_GetNumPlaying      (IntPtr soundgroupraw, ref int numplaying);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_SoundGroup_SetUserData        (IntPtr soundgroupraw, IntPtr userdata);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_SoundGroup_GetUserData        (IntPtr soundgroupraw, ref IntPtr userdata);
        [DllImport(Version.Dll)]
        private static extern Result FMOD_SoundGroup_GetMemoryInfo      (IntPtr soundgroupraw, uint memorybits, uint event_memorybits, ref uint memoryused, IntPtr memoryused_array);
        #endregion

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
    }


    /*
        'DSP' API
    */
	/*
    public class DSP
    {
        public Result release                   ()
        {
            return FMOD_DSP_Release(dspraw);
        }
        public Result getSystemObject           (ref System system)
        {
            Result Result         = Result.OK;
            IntPtr systemraw      = new IntPtr();
            System systemnew      = null;

            try
            {
                Result = FMOD_DSP_GetSystemObject(dspraw, ref systemraw);
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
                systemnew.setRaw(dspraw);
                system = systemnew;
            }
            else
            {
                system.setRaw(systemraw);
            }

            return Result;             
        }
                     

        public Result addInput                  (DSP target, ref DSPConnection dspconnection)
        {
            Result Result = Result.OK;
            IntPtr dspconnectionraw = new IntPtr();
            DSPConnection dspconnectionnew = null;

            try
            {
                Result = FMOD_DSP_AddInput(dspraw, target.getRaw(), ref dspconnectionraw);
            }
            catch
            {
                Result = Result.ERR_INVALID_PARAM;
            }
            if (Result != Result.OK)
            {
                return Result;
            }

            if (dspconnection == null)
            {
                dspconnectionnew = new DSPConnection();
                dspconnectionnew.setRaw(dspconnectionraw);
                dspconnection = dspconnectionnew;
            }
            else
            {
                dspconnection.setRaw(dspconnectionraw);
            }

            return Result;  
        }
        public Result disconnectFrom            (DSP target)
        {
            return FMOD_DSP_DisconnectFrom(dspraw, target.getRaw());
        }
        public Result disconnectAll             (bool inputs, bool outputs)
        {
            return FMOD_DSP_DisconnectAll(dspraw, (inputs ? 1 : 0), (outputs ? 1 : 0));
        }
        public Result remove                    ()
        {
            return FMOD_DSP_Remove(dspraw);
        }
        public Result getNumInputs              (ref int numinputs)
        {
            return FMOD_DSP_GetNumInputs(dspraw, ref numinputs);
        }
        public Result getNumOutputs             (ref int numoutputs)
        {
            return FMOD_DSP_GetNumOutputs(dspraw, ref numoutputs);
        }
        public Result getInput                  (int index, ref DSP input, ref DSPConnection inputconnection)
        {
            Result Result      = Result.OK;
            IntPtr dsprawnew   = new IntPtr();
            DSP    dspnew      = null;
            IntPtr dspconnectionraw = new IntPtr();
            DSPConnection dspconnectionnew = null;

            try
            {
                Result = FMOD_DSP_GetInput(dspraw, index, ref dsprawnew, ref dspconnectionraw);
            }
            catch
            {
                Result = Result.ERR_INVALID_PARAM;
            }
            if (Result != Result.OK)
            {
                return Result;
            }

            if (input == null)
            {
                dspnew = new DSP();
                dspnew.setRaw(dsprawnew);
                input = dspnew;
            }
            else
            {
                input.setRaw(dsprawnew);
            }

            if (inputconnection == null)
            {
                dspconnectionnew = new DSPConnection();
                dspconnectionnew.setRaw(dspconnectionraw);
                inputconnection = dspconnectionnew;
            }
            else
            {
                inputconnection.setRaw(dspconnectionraw);
            }

            return Result; 
        }
        public Result getOutput                 (int index, ref DSP output, ref DSPConnection outputconnection)
        {
            Result Result      = Result.OK;
            IntPtr dsprawnew   = new IntPtr();
            DSP    dspnew      = null;
            IntPtr dspconnectionraw = new IntPtr();
            DSPConnection dspconnectionnew = null;

            try
            {
                Result = FMOD_DSP_GetOutput(dspraw, index, ref dsprawnew, ref dspconnectionraw);
            }
            catch
            {
                Result = Result.ERR_INVALID_PARAM;
            }
            if (Result != Result.OK)
            {
                return Result;
            }

            if (output == null)
            {
                dspnew = new DSP();
                dspnew.setRaw(dsprawnew);
                output = dspnew;
            }
            else
            {
                output.setRaw(dsprawnew);
            }

            if (outputconnection == null)
            {
                dspconnectionnew = new DSPConnection();
                dspconnectionnew.setRaw(dspconnectionraw);
                outputconnection = dspconnectionnew;
            }
            else
            {
                outputconnection.setRaw(dspconnectionraw);
            }

            return Result; 
        }

        public Result setActive                 (bool active)
        {
            return FMOD_DSP_SetActive(dspraw, (active ? 1 : 0));
        }
        public Result getActive                 (ref bool active)
        {
            Result Result;
            int a = 0;

            Result = FMOD_DSP_GetActive(dspraw, ref a);

            active = (a != 0);

            return Result;
        }
        public Result setBypass                 (bool bypass)
        {
            return FMOD_DSP_SetBypass(dspraw, (bypass? 1 : 0));
        }
        public Result getBypass                 (ref bool bypass)
        {
            Result Result;
            int b = 0;

            Result = FMOD_DSP_GetBypass(dspraw, ref b);

            bypass = (b != 0);

            return Result;
        }
        public Result reset                     ()
        {
            return FMOD_DSP_Reset(dspraw);
        }

                     
        public Result setParameter              (int index, float val)
        {
            return FMOD_DSP_SetParameter(dspraw, index, val);
        }
        public Result getParameter              (int index, ref float val, StringBuilder valuestr, int valuestrlen)
        {
            return FMOD_DSP_GetParameter(dspraw, index, ref val, valuestr, valuestrlen);
        }
        public Result getNumParameters          (ref int numparams)
        {
            return FMOD_DSP_GetNumParameters(dspraw, ref numparams);
        }
        public Result getParameterInfo          (int index, StringBuilder name, StringBuilder label, StringBuilder description, int descriptionlen, ref float min, ref float max)
        {
            return FMOD_DSP_GetParameterInfo(dspraw, index, name, label, description, descriptionlen, ref min, ref max);
        }
        public Result showConfigDialog          (IntPtr hwnd, bool show)
        {
            return FMOD_DSP_ShowConfigDialog          (dspraw, hwnd, show);
        }


        public Result getInfo                   (StringBuilder name, ref uint version, ref int channels, ref int configwidth, ref int configheight)
        {
            return FMOD_DSP_GetInfo(dspraw, name, ref version, ref channels, ref configwidth, ref configheight);
        }
        public Result getType                   (ref DSP_TYPE type)
        {
            return FMOD_DSP_GetType(dspraw, ref type);
        }
        public Result setDefaults               (float frequency, float volume, float pan, int priority)
        {
            return FMOD_DSP_SetDefaults(dspraw, frequency, volume, pan, priority);
        }
        public Result getDefaults               (ref float frequency, ref float volume, ref float pan, ref int priority)
        {
            return FMOD_DSP_GetDefaults(dspraw, ref frequency, ref volume, ref pan, ref priority);
        }


        public Result setUserData               (IntPtr userdata)
        {
            return FMOD_DSP_SetUserData(dspraw, userdata);
        }
        public Result getUserData               (ref IntPtr userdata)
        {
            return FMOD_DSP_GetUserData(dspraw, ref userdata);
        }

        public Result getMemoryInfo(uint memorybits, uint event_memorybits, ref uint memoryused, IntPtr memoryused_array)
        {
            return FMOD_DSP_GetMemoryInfo(dspraw, memorybits, event_memorybits, ref memoryused, memoryused_array);
        }

        #region importfunctions

        [DllImport (Version.Dll)]
        private static extern Result FMOD_DSP_Release                   (IntPtr dsp);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_DSP_GetSystemObject           (IntPtr dsp, ref IntPtr system);
        [DllImport (Version.Dll)]                   
        private static extern Result FMOD_DSP_AddInput                  (IntPtr dsp, IntPtr target, ref IntPtr dspconnection);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_DSP_DisconnectFrom            (IntPtr dsp, IntPtr target);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_DSP_DisconnectAll             (IntPtr dsp, int inputs, int outputs);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_DSP_Remove                    (IntPtr dsp);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_DSP_GetNumInputs              (IntPtr dsp, ref int numinputs);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_DSP_GetNumOutputs             (IntPtr dsp, ref int numoutputs);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_DSP_GetInput                  (IntPtr dsp, int index, ref IntPtr input, ref IntPtr inputconnection);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_DSP_GetOutput                 (IntPtr dsp, int index, ref IntPtr output, ref IntPtr outputconnection);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_DSP_SetActive                 (IntPtr dsp, int active);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_DSP_GetActive                 (IntPtr dsp, ref int active);    
        [DllImport (Version.Dll)]
        private static extern Result FMOD_DSP_SetBypass                 (IntPtr dsp, int bypass);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_DSP_GetBypass                 (IntPtr dsp, ref int bypass);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_DSP_Reset                     (IntPtr dsp);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_DSP_SetParameter              (IntPtr dsp, int index, float val);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_DSP_GetParameter              (IntPtr dsp, int index, ref float val, StringBuilder valuestr, int valuestrlen);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_DSP_GetNumParameters          (IntPtr dsp, ref int numparams);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_DSP_GetParameterInfo          (IntPtr dsp, int index, StringBuilder name, StringBuilder label, StringBuilder description, int descriptionlen, ref float min, ref float max);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_DSP_ShowConfigDialog          (IntPtr dsp, IntPtr hwnd, bool show);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_DSP_GetInfo                   (IntPtr dsp, StringBuilder name, ref uint version, ref int channels, ref int configwidth, ref int configheight);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_DSP_GetType                   (IntPtr dsp, ref DSP_TYPE type);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_DSP_SetDefaults               (IntPtr dsp, float frequency, float volume, float pan, int priority);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_DSP_GetDefaults               (IntPtr dsp, ref float frequency, ref float volume, ref float pan, ref int priority);
        [DllImport (Version.Dll)]                   
        private static extern Result FMOD_DSP_SetUserData               (IntPtr dsp, IntPtr userdata);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_DSP_GetUserData               (IntPtr dsp, ref IntPtr userdata);
        [DllImport(Version.Dll)]
        private static extern Result FMOD_DSP_GetMemoryInfo             (IntPtr dsp, uint memorybits, uint event_memorybits, ref uint memoryused, IntPtr memoryused_array);
        #endregion

        #region wrapperinternal

        private IntPtr dspraw;

        public void setRaw(IntPtr dsp)
        {
            dspraw = new IntPtr();

            dspraw = dsp;
        }

        public IntPtr getRaw()
        {
            return dspraw;
        }

        #endregion
    }
	*/

    /*
        'DSPConnection' API
    */
    /* public class DSPConnection
    {
        public Result getInput              (ref DSP input)
        {
            Result Result = Result.OK;
            IntPtr dspraw = new IntPtr();
            DSP    dspnew = null;

            try
            {
                Result = FMOD_DSPConnection_GetInput(dspconnectionraw, ref dspraw);
            }
            catch
            {
                Result = Result.ERR_INVALID_PARAM;
            }
            if (Result != Result.OK)
            {
                return Result;
            }

            if (input == null)
            {
                dspnew = new DSP();
                dspnew.setRaw(dspraw);
                input = dspnew;
            }
            else
            {
                input.setRaw(dspraw);
            }

            return Result;
        }
        public Result getOutput             (ref DSP output)
        {
            Result Result = Result.OK;
            IntPtr dspraw = new IntPtr();
            DSP dspnew = null;

            try
            {
                Result = FMOD_DSPConnection_GetOutput(dspconnectionraw, ref dspraw);
            }
            catch
            {
                Result = Result.ERR_INVALID_PARAM;
            }
            if (Result != Result.OK)
            {
                return Result;
            }

            if (output == null)
            {
                dspnew = new DSP();
                dspnew.setRaw(dspraw);
                output = dspnew;
            }
            else
            {
                output.setRaw(dspraw);
            }

            return Result;
        }
        public Result setMix                (float volume)
        {
            return FMOD_DSPConnection_SetMix(dspconnectionraw, volume);
        }
        public Result getMix                (ref float volume)
        {
            return FMOD_DSPConnection_GetMix(dspconnectionraw, ref volume);
        }
        public Result setLevels             (Speaker speaker, float[] levels, int numlevels)
        {
            return FMOD_DSPConnection_SetLevels(dspconnectionraw, speaker, levels, numlevels);
        }
        public Result getLevels             (Speaker speaker, float[] levels, int numlevels)
        {
            return FMOD_DSPConnection_GetLevels(dspconnectionraw, speaker, levels, numlevels);
        }
        public Result setUserData(IntPtr userdata)
        {
            return FMOD_DSPConnection_SetUserData(dspconnectionraw, userdata);
        }
        public Result getUserData(ref IntPtr userdata)
        {
            return FMOD_DSPConnection_GetUserData(dspconnectionraw, ref userdata);
        }

        public Result getMemoryInfo(uint memorybits, uint event_memorybits, ref uint memoryused, IntPtr memoryused_array)
        {
            return FMOD_DSPConnection_GetMemoryInfo(dspconnectionraw, memorybits, event_memorybits, ref memoryused, memoryused_array);
        }

        #region importfunctions

        [DllImport (Version.Dll)]
        private static extern Result FMOD_DSPConnection_GetInput        (IntPtr dspconnection, ref IntPtr input);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_DSPConnection_GetOutput       (IntPtr dspconnection, ref IntPtr output);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_DSPConnection_SetMix          (IntPtr dspconnection, float volume);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_DSPConnection_GetMix          (IntPtr dspconnection, ref float volume);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_DSPConnection_SetLevels       (IntPtr dspconnection, Speaker speaker, float[] levels, int numlevels);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_DSPConnection_GetLevels       (IntPtr dspconnection, Speaker speaker, float[] levels, int numlevels);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_DSPConnection_SetUserData     (IntPtr dspconnection, IntPtr userdata);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_DSPConnection_GetUserData     (IntPtr dspconnection, ref IntPtr userdata);
        [DllImport(Version.Dll)]
        private static extern Result FMOD_DSPConnection_GetMemoryInfo   (IntPtr dspconnection, uint memorybits, uint event_memorybits, ref uint memoryused, IntPtr memoryused_array);
        #endregion

        #region wrapperinternal

        private IntPtr dspconnectionraw;

        public void setRaw(IntPtr dspconnection)
        {
            dspconnectionraw = new IntPtr();

            dspconnectionraw = dspconnection;
        }

        public IntPtr getRaw()
        {
            return dspconnectionraw;
        }

        #endregion
    } */

    /*
        'Geometry' API
    */
    public class Geometry
    {
        public Result release               ()
        {
            return FMOD_Geometry_Release(geometryraw);
        }       
        public Result addPolygon            (float directocclusion, float reverbocclusion, bool doublesided, int numvertices, Vector[] vertices, ref int polygonindex)
        {
            return FMOD_Geometry_AddPolygon(geometryraw, directocclusion, reverbocclusion, (doublesided ? 1 : 0), numvertices, vertices, ref polygonindex);
        }


        public Result getNumPolygons        (ref int numPolygons)
        {
            return FMOD_Geometry_GetNumPolygons(geometryraw, ref numPolygons);
        }
        public Result getMaxPolygons        (ref int maxPolygons, ref int maxVertices)
        {
            return FMOD_Geometry_GetMaxPolygons(geometryraw, ref maxPolygons, ref maxVertices);
        }
        public Result getPolygonNumVertices (int polygonIndex, ref int numVertices)
        {
            return FMOD_Geometry_GetPolygonNumVertices(geometryraw, polygonIndex, ref numVertices);
        }
        public Result setPolygonVertex      (int polygonIndex, int vertexIndex, ref Vector vertex)
        {
            return FMOD_Geometry_SetPolygonVertex(geometryraw, polygonIndex, vertexIndex, ref vertex);
        }
        public Result getPolygonVertex      (int polygonIndex, int vertexIndex, ref Vector vertex)
        {
            return FMOD_Geometry_GetPolygonVertex(geometryraw, polygonIndex, vertexIndex, ref vertex);
        }
        public Result setPolygonAttributes  (int polygonIndex, float directOcclusion, float reverbOcclusion, bool doubleSided)
        {
            return FMOD_Geometry_SetPolygonAttributes(geometryraw, polygonIndex, directOcclusion, reverbOcclusion, doubleSided);
        }
        public Result getPolygonAttributes  (int polygonIndex, ref float directOcclusion, ref float reverbOcclusion, ref bool doubleSided)
        {
            return FMOD_Geometry_GetPolygonAttributes(geometryraw, polygonIndex, ref directOcclusion, ref reverbOcclusion, ref doubleSided);
        }

        public Result setActive             (bool active)
        {
            return FMOD_Geometry_SetActive  (geometryraw, active);
        }
        public Result getActive             (ref bool active)
        {
            return FMOD_Geometry_GetActive  (geometryraw, ref active);
        }
        public Result setRotation           (ref Vector forward, ref Vector up)
        {
            return FMOD_Geometry_SetRotation(geometryraw, ref forward, ref up);
        }
        public Result getRotation           (ref Vector forward, ref Vector up)
        {
            return FMOD_Geometry_GetRotation(geometryraw, ref forward, ref up);
        }
        public Result setPosition           (ref Vector position)
        {
            return FMOD_Geometry_SetPosition(geometryraw, ref position);
        }
        public Result getPosition           (ref Vector position)
        {
            return FMOD_Geometry_GetPosition(geometryraw, ref position);
        }
        public Result setScale              (ref Vector scale)
        {
            return FMOD_Geometry_SetScale(geometryraw, ref scale);
        }
        public Result getScale              (ref Vector scale)
        {
            return FMOD_Geometry_GetScale(geometryraw, ref scale);
        }
        public Result save                  (IntPtr data, ref int datasize)
        {
            return FMOD_Geometry_Save(geometryraw, data, ref datasize);
        }


        public Result setUserData               (IntPtr userdata)
        {
            return FMOD_Geometry_SetUserData(geometryraw, userdata);
        }
        public Result getUserData               (ref IntPtr userdata)
        {
            return FMOD_Geometry_GetUserData(geometryraw, ref userdata);
        }

        public Result getMemoryInfo(uint memorybits, uint event_memorybits, ref uint memoryused, IntPtr memoryused_array)
        {
            return FMOD_Geometry_GetMemoryInfo(geometryraw, memorybits, event_memorybits, ref memoryused, memoryused_array);
        }

        #region importfunctions

        [DllImport (Version.Dll)]
        private static extern Result FMOD_Geometry_Release   (IntPtr geometry);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Geometry_AddPolygon           (IntPtr geometry, float directocclusion, float reverbocclusion, int doublesided, int numvertices, Vector[] vertices, ref int polygonindex);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Geometry_GetNumPolygons       (IntPtr geometry, ref int numPolygons);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Geometry_GetMaxPolygons       (IntPtr geometry, ref int maxPolygons, ref int maxVertices);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Geometry_GetPolygonNumVertices(IntPtr geometry, int polygonIndex, ref int numVertices);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Geometry_SetPolygonVertex     (IntPtr geometry, int polygonIndex, int vertexIndex, ref Vector vertex);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Geometry_GetPolygonVertex     (IntPtr geometry, int polygonIndex, int vertexIndex, ref Vector vertex);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Geometry_SetPolygonAttributes (IntPtr geometry, int polygonIndex, float directOcclusion, float reverbOcclusion, bool doubleSided);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Geometry_GetPolygonAttributes (IntPtr geometry, int polygonIndex, ref float directOcclusion, ref float reverbOcclusion, ref bool doubleSided);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Geometry_Flush                (IntPtr geometry);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Geometry_SetActive                    (IntPtr gemoetry, bool active);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Geometry_GetActive                    (IntPtr gemoetry, ref bool active);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Geometry_SetRotation          (IntPtr geometry, ref Vector forward, ref Vector up);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Geometry_GetRotation          (IntPtr geometry, ref Vector forward, ref Vector up);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Geometry_SetPosition          (IntPtr geometry, ref Vector position);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Geometry_GetPosition          (IntPtr geometry, ref Vector position);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Geometry_SetScale             (IntPtr geometry, ref Vector scale);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Geometry_GetScale             (IntPtr geometry, ref Vector scale);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Geometry_Save                 (IntPtr geometry, IntPtr data, ref int datasize);
        [DllImport (Version.Dll)]                   
        private static extern Result FMOD_Geometry_SetUserData          (IntPtr geometry, IntPtr userdata);
        [DllImport (Version.Dll)]
        private static extern Result FMOD_Geometry_GetUserData          (IntPtr geometry, ref IntPtr userdata);
        [DllImport(Version.Dll)]
        private static extern Result FMOD_Geometry_GetMemoryInfo        (IntPtr geometry, uint memorybits, uint event_memorybits, ref uint memoryused, IntPtr memoryused_array);
        #endregion

        #region wrapperinternal

        private IntPtr geometryraw;

        public void setRaw(IntPtr geometry)
        {
            geometryraw = new IntPtr();

            geometryraw = geometry;
        }

        public IntPtr getRaw()
        {
            return geometryraw;
        }

        #endregion

        /*
            'Reverb' API
        */
        public class Reverb
        { 

            public Result release                ()
            {
                return FMOD_Reverb_Release(reverbraw);
            }

            // Reverb manipulation.
            public Result set3DAttributes        (ref Vector position, float mindistance, float maxdistance)
            {
                return FMOD_Reverb_Set3DAttributes(reverbraw, ref position, mindistance, maxdistance);
            }
            public Result get3DAttributes        (ref Vector position, ref float mindistance, ref float maxdistance)
            {
                return FMOD_Reverb_Get3DAttributes(reverbraw, ref position, ref mindistance, ref maxdistance);
            }
            public Result setProperties          (ref REVERB_PROPERTIES properties)
            {
                return FMOD_Reverb_SetProperties(reverbraw, ref properties);
            }
            public Result getProperties          (ref REVERB_PROPERTIES properties)
            {
                return FMOD_Reverb_GetProperties(reverbraw, ref properties);
            }
            public Result setActive              (int active)
            {
                return FMOD_Reverb_SetActive(reverbraw, active);
            }
            public Result getActive              (ref int active)
            {
                return FMOD_Reverb_GetActive(reverbraw, ref active);
            }

            // Userdata set/get.
            public Result setUserData            (IntPtr userdata)
            {
                return FMOD_Reverb_SetUserData(reverbraw, userdata);
            }
            public Result getUserData            (ref IntPtr userdata)
            {
                return FMOD_Reverb_GetUserData(reverbraw, ref userdata);
            }

            public Result getMemoryInfo(uint memorybits, uint event_memorybits, ref uint memoryused, IntPtr memoryused_array)
            {
                return FMOD_Reverb_GetMemoryInfo(reverbraw, memorybits, event_memorybits, ref memoryused, memoryused_array);
            }

            #region importfunctions

            [DllImport (Version.Dll)]
            private static extern Result FMOD_Reverb_Release                (IntPtr reverb);
            [DllImport (Version.Dll)]
            private static extern Result FMOD_Reverb_Set3DAttributes        (IntPtr reverb, ref Vector position, float mindistance, float maxdistance);
            [DllImport (Version.Dll)]
            private static extern Result FMOD_Reverb_Get3DAttributes        (IntPtr reverb, ref Vector position, ref float mindistance, ref float maxdistance);
            [DllImport (Version.Dll)]
            private static extern Result FMOD_Reverb_SetProperties          (IntPtr reverb, ref REVERB_PROPERTIES properties);
            [DllImport (Version.Dll)]
            private static extern Result FMOD_Reverb_GetProperties          (IntPtr reverb, ref REVERB_PROPERTIES properties);
            [DllImport (Version.Dll)]
            private static extern Result FMOD_Reverb_SetActive              (IntPtr reverb, int active);
            [DllImport (Version.Dll)]
            private static extern Result FMOD_Reverb_GetActive              (IntPtr reverb, ref int active);
            [DllImport (Version.Dll)]
            private static extern Result FMOD_Reverb_SetUserData            (IntPtr reverb, IntPtr userdata);
            [DllImport (Version.Dll)]
            private static extern Result FMOD_Reverb_GetUserData            (IntPtr reverb, ref IntPtr userdata);
            [DllImport(Version.Dll)]
            private static extern Result FMOD_Reverb_GetMemoryInfo          (IntPtr reverb, uint memorybits, uint event_memorybits, ref uint memoryused, IntPtr memoryused_array);
            #endregion

            #region wrapperinternal

            private IntPtr reverbraw;

            public void setRaw(IntPtr rev)
            {
                reverbraw = new IntPtr();

                reverbraw = rev;
            }

            public IntPtr getRaw()
            {
                return reverbraw;
            }

            #endregion
        }
    }
}
