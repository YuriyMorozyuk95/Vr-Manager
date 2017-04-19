using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VrPlayer.Helpers.Mvvm;

namespace VrPlayer
{
    public class StartUpConfig : ViewModelBase
    {
        public const string PauseOnStartProperty = "-pause";
        public const string ScreenNumberProperty = "-screen";
        public const string PresetPathProperty = "-config";
        public const string MediaPathProperty = "-media";
        public const string FullScreenProperty = "-fullscreen";

        public int ScreenNumber { get; set; }
        public bool PauseOnStart { get; set; }
        public string PresetPath { get; set; }
        public string MediaPath { get; set; }
        public bool FullScreen { get; set; }

        public static StartUpConfig CreateConfig(string[] args, string defaultMedia)
        {
            StartUpConfig config = new StartUpConfig(){ScreenNumber = 0, MediaPath = defaultMedia};
            
            foreach (var arg in args)
            {
                if (string.IsNullOrWhiteSpace(arg))
                {
                    continue;
                }

                var splitArgs = arg.Trim().Split(':');
                if (splitArgs.Length >= 2)
                {
                    if (splitArgs[0] == FullScreenProperty)
                    {
                        bool fullscreen;
                        if (bool.TryParse(splitArgs[1], out fullscreen))
                        {
                            config.FullScreen = fullscreen;
                        }
                    }
                    if (splitArgs[0] == PauseOnStartProperty)
                    {
                        bool pause;
                        if (bool.TryParse(splitArgs[1], out pause))
                        {
                            config.PauseOnStart = pause;
                        }
                    }
                    if (splitArgs[0] == ScreenNumberProperty)
                    {
                        int screen;
                        if (int.TryParse(splitArgs[1], out screen))
                        {
                            config.ScreenNumber = screen;
                        }
                    }
                    if (splitArgs[0] == PresetPathProperty)
                    {
                        if (splitArgs.Length == 3)
                        {
                            config.PresetPath = String.Format("{0}:{1}", splitArgs[1], splitArgs[2]);
                        }
                        else
                        {
                            config.PresetPath = splitArgs[1];
                        }
                    }
                    if (splitArgs[0] == MediaPathProperty)
                    {
                        if (splitArgs.Length == 3)
                        {
                            config.MediaPath = String.Format("{0}:{1}", splitArgs[1], splitArgs[2]);
                        }
                        else
                        {
                            config.MediaPath = splitArgs[1];
                        }
                    }
                }
            }
            return config;
        }

    }
}
