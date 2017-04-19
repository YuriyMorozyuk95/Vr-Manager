using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using VrPlayer.Contracts.Stabilizers;
using VrPlayer.Helpers;

namespace VrPlayer.Stabilizers.Csv
{
    [DataContract]
    public class CsvStabilizer : StabilizerBase
    {
        public IEnumerable<CsvFrame> CsvData { get; set; }

        private string _filePath;

        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                OnPropertyChanged("FilePath");
            }
        }

        public CsvStabilizer()
        {
            PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName != "FilePath") return;

            try
            {
                if (File.Exists(FilePath))
                {
                    CsvData = CsvParser.Parse(FilePath);
                }
                else
                {
                    throw new FileNotFoundException("CSV stabilization file not found.", FilePath);
                }
            }
            catch (Exception exc)
            {
                Logger.Instance.Warn("Error while loading CSV stabilization file.", exc);
            }
        }

        public override void UpdateCurrentFrame(int frame)
        {
            var data = CsvData.FirstOrDefault(d => d.FrameNumber == frame);
            if (data == null) return;

            Translation = data.Translation;
            Rotation = data.Rotation;
        }

        public override int GetFramesCount()
        {
            return CsvData == null ? 0 : CsvData.Count();
        }
    }
}
