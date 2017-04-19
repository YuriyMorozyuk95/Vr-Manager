using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComPortPackages.Console
{
    public class StartUpConfig
    {
        public const string FileProperty = "-file";
        public const string OnlyDoneKey = "-only_done";

        public string File { get; set; }
        public bool OnlyDone { get; set; }

        public static StartUpConfig CreateConfig( string defaultFile)
        {
            StartUpConfig config = new StartUpConfig() { File = defaultFile };

            ////foreach (var arg in args)
            ////{
            ////    if (string.IsNullOrWhiteSpace(arg))
            ////    {
            ////        continue;
            ////    }

            ////    var splitArgs = arg.Trim().Split(':');
            ////    if (splitArgs.Length >= 2)
            ////    {
            ////        if (splitArgs[0] == FileProperty)
            ////        {
            ////            if (splitArgs.Length == 3)
            ////            {
            ////                config.File = $"{splitArgs[1]}:{splitArgs[2]}";
            ////            }
            ////            else
            ////            {
            ////                config.File = $"{splitArgs[1]}";
            ////            }

                       
            ////        }
            ////    }
            ////    if (arg == OnlyDoneKey)
            ////    {
            ////        config.OnlyDone = true;
            ////    }
            ////}
            return config;
        }
    }
}
