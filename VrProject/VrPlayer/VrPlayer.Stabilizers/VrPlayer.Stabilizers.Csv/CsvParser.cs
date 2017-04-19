using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Media3D;
using VrPlayer.Helpers;

namespace VrPlayer.Stabilizers.Csv
{
    public class CsvParser
    {
        private static readonly char[] Delimiters = new[] { ',' };

        public static IEnumerable<CsvFrame> Parse(string filePath)
        {
            var csvFrames = new List<CsvFrame>();

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

                    try
                    {
                        var parts = line.Split(Delimiters);

                        var frame = new CsvFrame
                        {
                            FrameNumber = int.Parse(parts[0]),
                            Translation = new Vector3D(
                                double.Parse(parts[1]),
                                double.Parse(parts[2]), 
                                double.Parse(parts[3])),
                            Rotation = QuaternionHelper.EulerAnglesInRadToQuaternion(
                                double.Parse(parts[4]),
                                double.Parse(parts[5]),
                                double.Parse(parts[6]))
                        };

                        csvFrames.Add(frame);
                    }
                    catch (Exception exc)
                    {
                        Logger.Instance.Error(string.Format("Error while parsing deshaker log file at line {0}.", i + 1), exc);
                    }
                    i++;
                }
            }

            return csvFrames;
        }
    }
}