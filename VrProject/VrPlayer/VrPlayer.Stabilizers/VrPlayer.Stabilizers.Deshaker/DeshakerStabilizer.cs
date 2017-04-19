using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media.Media3D;
using VrPlayer.Contracts.Stabilizers;
using VrPlayer.Helpers;

namespace VrPlayer.Stabilizers.Deshaker
{
    [DataContract]
    public class DeshakerStabilizer : StabilizerBase
    {
        public IEnumerable<DeshakerFrame> DeshakerData { get; set; }

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

        public static readonly DependencyProperty TranslationFactorProperty =
            DependencyProperty.Register("TranslationFactor", typeof(double),
            typeof(DeshakerStabilizer), new FrameworkPropertyMetadata(0.01));
        [DataMember]
        public double TranslationFactor
        {
            get { return (double)GetValue(TranslationFactorProperty); }
            set { SetValue(TranslationFactorProperty, value); }
        }

        public static readonly DependencyProperty RotationFactorProperty =
            DependencyProperty.Register("RotationFactor", typeof(double),
            typeof(DeshakerStabilizer), new FrameworkPropertyMetadata(1D));
        [DataMember]
        public double RotationFactor
        {
            get { return (double)GetValue(RotationFactorProperty); }
            set { SetValue(RotationFactorProperty, value); }
        }

        public static readonly DependencyProperty ZoomFactorProperty =
            DependencyProperty.Register("ZoomFactor", typeof(double),
            typeof(DeshakerStabilizer), new FrameworkPropertyMetadata(0.01));
        [DataMember]
        public double ZoomFactor
        {
            get { return (double)GetValue(ZoomFactorProperty); }
            set { SetValue(ZoomFactorProperty, value); }
        }


        public DeshakerStabilizer()
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
                    DeshakerData = DeshakerParser.Parse(FilePath);
                }
                else
                {
                    throw new FileNotFoundException("Deshaker log file not found.", FilePath);
                }
            }
            catch (Exception exc)
            {
                Logger.Instance.Warn("Error while loading deshaker log file.", exc);
            }
        }

        public override void UpdateCurrentFrame(int frame)
        {
            var data = DeshakerData.FirstOrDefault(d => d.FrameNumber == frame);
            if (data == null) return;

            var scaledTranslation = new Vector3D(-data.PanX, -data.PanY, -data.Zoom) / 10000;

            Logger.Instance.Info(string.Format("Translation: ({0},{1},{2})", scaledTranslation.X, scaledTranslation.Y, scaledTranslation.Z));
            Translation = new Vector3D(scaledTranslation.X * TranslationFactor, scaledTranslation.Y * TranslationFactor, scaledTranslation.Z * ZoomFactor);
            Rotation = QuaternionHelper.EulerAnglesInDegToQuaternion(0, 0, data.Rotation * RotationFactor);
        }

        public override int GetFramesCount()
        {
            return DeshakerData == null ? 0 : DeshakerData.Count();
        }
    }
}
