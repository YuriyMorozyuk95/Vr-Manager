using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Windows.Media.Media3D;
using WiimoteLib;

using VrPlayer.Contracts.Trackers;
using VrPlayer.Helpers;

namespace VrPlayer.Trackers.WiimoteTracker
{
    [DataContract]
    public class WiimoteTracker : TrackerBase, ITracker
    {
        private Wiimote _wiimote;

        public WiimoteTracker()
        {
        }

        public override void Load()
        {
            IsEnabled = true;

            try
            {
                _wiimote = new Wiimote();
                _wiimote.Connect();
                _wiimote.InitializeMotionPlus();
                _wiimote.WiimoteChanged += wiimote_WiimoteChanged;

                _wiimote.SetRumble(true);
                _wiimote.SetLEDs(true, false, false, true);
                Thread.Sleep(40);
                _wiimote.SetRumble(false);

                RawPosition = new Vector3D();
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(exc.Message, exc);
                try
                {
                    _wiimote.SetLEDs(false, false, false, false);
                }
                catch (Exception exception)
                {
                    Logger.Instance.Error(exception.Message, exception);
                }
                IsEnabled = false;
            }
        }

        public override void Unload()
        {
            try
            {
                _wiimote.SetLEDs(false, false, false, false);
                _wiimote.Disconnect();
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(exc.Message, exc);
            }
            _wiimote = null;
        }

        void wiimote_WiimoteChanged(object sender, WiimoteChangedEventArgs e)
        {
            RawRotation = QuaternionHelper.EulerAnglesInDegToQuaternion(
            e.WiimoteState.MotionPlusState.Values.Y,
            e.WiimoteState.MotionPlusState.Values.X,
            e.WiimoteState.MotionPlusState.Values.Z);

            if (e.WiimoteState.ButtonState.Plus)
            {
                Dispatcher.Invoke((Action)(Calibrate));
            }

            Dispatcher.Invoke((Action)(UpdatePositionAndRotation));
        }
    }
}
