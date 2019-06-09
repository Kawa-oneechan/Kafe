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
            player.Initialize(16, InitalisationFlags.Normal, IntPtr.Zero);

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
				var ret = player.CreateSound(data, Mode.Loop | Mode.OpenMemory, ref exInfo, ref sound);
				if (ret != Result.OK)
					return;
				ret = player.PlaySound(CHANNELINDEX.REUSE, sound, true, ref musicChannel);
				if (ret != Result.OK)
					return;

				if (name.EndsWith(".ogg"))
				{
					var tag = new TAG();
					uint loopStart = 0;
					uint loopEnd = 0;
					ret = sound.GetTag("LOOP_START", 0, ref tag);
					if (ret != Result.ERR_TOOMANYCHANNELS)
					{
						if (ret != Result.ERR_TAGNOTFOUND)
							loopStart = uint.Parse(tag.StringData);
						ret = sound.GetTag("LOOP_END", 0, ref tag);
						if (ret != Result.ERR_TAGNOTFOUND)
							loopEnd = uint.Parse(tag.StringData);
						if (loopEnd > 0)
							sound.SetLoopPoints(loopStart, TIMEUNIT.PCM, loopEnd, TIMEUNIT.PCM);
					}
				}

				musicChannel.SetPaused(false);
			}
        }
    }
}
