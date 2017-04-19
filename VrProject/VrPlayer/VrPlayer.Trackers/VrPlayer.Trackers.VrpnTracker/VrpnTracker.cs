using System;
using System.Runtime.Serialization;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using VrPlayer.Helpers;
using Vrpn;

using VrPlayer.Contracts.Trackers;

namespace VrPlayer.Trackers.VrpnTracker
{
    [DataContract]
    public class VrpnTracker : TrackerBase, ITracker
    {
        private readonly DispatcherTimer _timer;
        private TrackerRemote _tracker;
        private ButtonRemote _button;

        private string _trackerAddress;
        [DataMember]
        public string TrackerAddress
        {
            get
            {
                return _trackerAddress;
            }
            set
            {
                _trackerAddress = value;
                OnPropertyChanged("TrackerAddress");
            }
        }

        private string _buttonAddress;
        [DataMember]
        public string ButtonAddress
        {
            get
            {
                return _buttonAddress;
            }
            set
            {
                _buttonAddress = value;
                OnPropertyChanged("ButtonAddress");
            }
        }
        
        public VrpnTracker(string trackerAddress, string buttonAddress)
        {
            _buttonAddress = buttonAddress;
            _trackerAddress = trackerAddress;

            _timer = new DispatcherTimer(DispatcherPriority.Send);
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 15);
            _timer.Tick += timer_Tick;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                _tracker.Update();
                _button.Update();
            }
            catch(Exception exc)
            {
                Logger.Instance.Error(exc.Message, exc);
            }
        }

        private void PositionChanged(object sender, TrackerChangeEventArgs e)
        {
            try
            {
                //TODO: Support user defined sensor index or autodetect
                if (e.Sensor == 0)
                {
                    RawPosition = new Vector3D(
                        -e.Position.X,
                        -e.Position.Z,
                        e.Position.Y);

                    RawRotation = new System.Windows.Media.Media3D.Quaternion(
                        e.Orientation.Y,
                        e.Orientation.W,
                        -e.Orientation.X,
                        e.Orientation.Z);

                    UpdatePositionAndRotation();
                }
            }
            catch(Exception exc)
            {
                Logger.Instance.Error(exc.Message, exc);
            }
        }

        private void ButtonChanged(object sender, ButtonChangeEventArgs e)
        {
            if (e.Button == 0)
                Calibrate();
        }

        public override void Load()
        {
            try
            {
                IsEnabled = true;
                PositionScaleFactor = 0.001;

                _tracker = new TrackerRemote(_trackerAddress);
                _tracker.PositionChanged += PositionChanged;
                _tracker.MuteWarnings = true;

                _button = new ButtonRemote(_buttonAddress);
                _button.ButtonChanged += ButtonChanged;
                _button.MuteWarnings = true;
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(exc.Message, exc);
                IsEnabled = false;
            }
            _timer.Start();
        }

        public override void Unload()
        {
            _timer.Stop();
            _tracker.Dispose();
            _button.Dispose();
        }
    }
}