using System;
using FMOD;

namespace Kafe
{
    public static class SoundEngine
    {
        public static string ContentPath { get; set; }

        private static FMOD.System player;
        private static Channel musicChannel;

        private static Random randomizer;

        public static void Initialize()
        {
            player = Factory.CreateSystem();
            player.init(16, InitalisationFlags.Normal, IntPtr.Zero);

            randomizer = new Random();
        }

        public static void PlaySong(string name)
        {
            if (player == null)
                return;
			if (Mix.FileExists(name))
			{
				var sound = new Sound();
				var data = Mix.GetBytes(name);
				var exInfo = new CREATESOUNDEXINFO()
				{
					cbsize = 112,
					length = (uint)data.Length,
				};
				if (System.IO.File.Exists("kafe.dls"))
					exInfo.dlsname = "kafe.dls";
				var ret = player.createSound(data, Mode.Loop | Mode.OpenMemory, ref exInfo, ref sound);
				if (ret != Result.OK)
					return;
				ret = player.playSound(CHANNELINDEX.REUSE, sound, true, ref musicChannel);
				if (ret != Result.OK)
					return;

				if (name.EndsWith(".ogg"))
				{
					var tag = new TAG();
					uint loopStart = 0;
					uint loopEnd = 0;
					ret = sound.getTag("LOOP_START", 0, ref tag);
					if (ret != Result.ERR_TOOMANYCHANNELS)
					{
						if (ret != Result.ERR_TAGNOTFOUND)
							loopStart = uint.Parse(System.Runtime.InteropServices.Marshal.PtrToStringAnsi(tag.data));
						ret = sound.getTag("LOOP_END", 0, ref tag);
						if (ret != Result.ERR_TAGNOTFOUND)
							loopEnd = uint.Parse(System.Runtime.InteropServices.Marshal.PtrToStringAnsi(tag.data));
						if (loopEnd > 0)
							sound.setLoopPoints(loopStart, TIMEUNIT.PCM, loopEnd, TIMEUNIT.PCM);
					}
				}

				musicChannel.setPaused(false);
			}
        }
    }
}
