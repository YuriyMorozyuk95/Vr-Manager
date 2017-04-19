using System;
using System.Collections.Generic;
using System.IO;
using VrPlayer.Helpers;

namespace VrPlayer.Stabilizers.Deshaker
{
    public class DeshakerParser
    {
        private static readonly char[] Delimiters = new[] { '\t' };

        public static IEnumerable<DeshakerFrame> Parse(string filePath)
        {
            var deshakerFrames = new List<DeshakerFrame>();

            using (var reader = new StreamReader(filePath))
            {
                var i = 0;
                while (true)
                {
                    var line = reader.ReadLine();
                    if (line == null)
                    {
                        break;
                    }
                    var parts = line.Split(Delimiters);

                    try
                    {
                        var frame = new DeshakerFrame
                        {
                            FrameNumber = int.Parse(parts[0]),
                            PanX = double.Parse(parts[1]),
                            PanY = double.Parse(parts[2]),
                            Rotation = double.Parse(parts[3]),
                            Zoom = double.Parse(parts[4])
                        };

                        //Convert relative to absolute values
                        if (i > 0)
                        {
                            frame.PanX += deshakerFrames[i - 1].PanX;
                            frame.PanY += deshakerFrames[i - 1].PanY;
                            frame.Rotation += deshakerFrames[i - 1].Rotation;
                            frame.Zoom += deshakerFrames[i - 1].Zoom;
                        }
                        
                        deshakerFrames.Add(frame);
                    }
                    catch (Exception exc)
                    {
                        Logger.Instance.Error(string.Format("Error while parsing deshaker log file at line {0}.", i+1), exc);
                    }
                    i++;
                }
            }

            return deshakerFrames;
        }
    }
}