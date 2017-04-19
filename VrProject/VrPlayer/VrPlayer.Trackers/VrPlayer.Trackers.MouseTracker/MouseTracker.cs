using System;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Input;
using VrPlayer.Contracts.Trackers;
using VrPlayer.Helpers;

namespace VrPlayer.Trackers.MouseTracker
{
    [DataContract]
    public class MouseTracker : TrackerBase, ITracker
    {
        private FrameworkElement _viewport;
        private double _yaw;
        private double _pitch;

        #region Fields

        public static readonly DependencyProperty SensitivityProperty =
            DependencyProperty.Register("Sensitivity", typeof(double),
            typeof(MouseTracker), new FrameworkPropertyMetadata(1D));
        [DataMember]
        public double Sensitivity
        {
            get { return (double)GetValue(SensitivityProperty); }
            set { SetValue(SensitivityProperty, value); }
        }

        #endregion

        public MouseTracker()
        {
            //Handlers
            Application.Current.Activated += Current_Activated;

            IsEnabled = true;
        }

        void Current_Activated(object sender, EventArgs e)
        {
            try
            {
                if (_viewport != null) return;
                _viewport = Application.Current.MainWindow;
                _viewport.MouseMove += mouseZone_MouseMove;
                _viewport.KeyDown += ViewportOnKeyDown;
            }
            catch
            {
                return;
            }
        }

        private void ViewportOnKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            UpdatePositionAndRotation();
        }

        void mouseZone_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsActive)
            {
                _viewport.Cursor = Cursors.None;

                var centerOfViewport = _viewport.PointToScreen(new Point(_viewport.ActualWidth / 2, _viewport.ActualHeight / 2));
                var relativePos = e.MouseDevice.GetPosition(_viewport);
                var actualRelativePos = new Point(relativePos.X - _viewport.ActualWidth / 2, _viewport.ActualHeight / 2 - relativePos.Y);
                var dx = actualRelativePos.X;
                var dy = actualRelativePos.Y;
                _yaw += dx;
                _pitch += dy;
                
                // Rotate
                RawRotation = QuaternionHelper.EulerAnglesInDegToQuaternion(_pitch * Sensitivity * 0.1, _yaw * Sensitivity * 0.1, 0);
                UpdatePositionAndRotation();
                
                // Set mouse position back to the center of the viewport in screen coordinates
                MouseUtilities.SetPosition(centerOfViewport);
            }
            else
            {
                _viewport.Cursor = Cursors.Arrow;
            }
        }

        public override void Load()
        {
            
        }

        public override void Unload()
        {
        }
    }
}