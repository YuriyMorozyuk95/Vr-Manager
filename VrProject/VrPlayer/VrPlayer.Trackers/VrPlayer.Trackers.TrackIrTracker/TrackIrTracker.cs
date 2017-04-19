using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Windows.Threading;
using System.Windows.Media.Media3D;
using VrPlayer.Contracts.Trackers;
using VrPlayer.Helpers;

namespace VrPlayer.Trackers.TrackIrTracker
{
    [DataContract]
    unsafe public class TrackIrTracker : TrackerBase, ITracker
    {
        [DllImport(@"TrackIrWrapper.dll")]
        static extern int TIR_Init();

        [DllImport(@"TrackIrWrapper.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int TIR_Exit();

        [DllImport(@"TrackIrWrapper.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int TIR_Update(float* x, float* y, float* z, float* pitch, float* yaw, float* roll);

        [DllImport(@"TrackIrWrapper.dll")]
        static extern int TIR_ReCenter();

        private const double UnitsByDeg = Int16.MaxValue / 180;
        private readonly DispatcherTimer _timer;

        private const int MaxConnection = 10;
        private int _connectCounter;

        public TrackIrTracker()
        {
            _timer = new DispatcherTimer(DispatcherPriority.Send);
        }

        void init_timer_Tick(object sender, EventArgs e)
        {
            if(_connectCounter++ == MaxConnection)
            {  
                _timer.Stop();
                return;
            }

            if (Process.GetCurrentProcess().MainWindowHandle == IntPtr.Zero) 
                return;
            
            try
            {
                _timer.Stop();

                var result = TIR_Init();
                ThrowErrorOnResult(result, "Error while initializing Track IR");

                _timer.Interval = new TimeSpan(0, 0, 0, 0, 15);
                _timer.Tick -= init_timer_Tick;
                _timer.Tick += data_timer_Tick;
                _timer.Start();

                IsEnabled = true;
            }
            catch (Exception exc)
            {
                IsEnabled = false;
                Logger.Instance.Error(exc.Message, exc);
            }
        }

        void data_timer_Tick(object sender, EventArgs e)
        {
            try
            {
                float x, y, z, pitch, yaw, roll;
                var result = TIR_Update(&x, &y, &z, &pitch, &yaw, &roll);
                ThrowErrorOnResult(result, "Error while getting data from the Track IR");

                RawPosition = new Vector3D(-x,-y,z);
                RawRotation = QuaternionHelper.EulerAnglesInDegToQuaternion(
                    -(yaw - Int16.MaxValue) / UnitsByDeg,
                    -(pitch - Int16.MaxValue) / UnitsByDeg,
                    (roll - Int16.MaxValue) / UnitsByDeg);

                UpdatePositionAndRotation();
            }
            catch(Exception exc)
            {
                Logger.Instance.Error(exc.Message, exc);
            }    
        }

        public override void Calibrate()
        {
            try
            {
                var result = TIR_ReCenter();
                ThrowErrorOnResult(result, "Error while re-centering Track IR");
                base.Calibrate();
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(exc.Message, exc);
            }
        }

        public override void Load()
        {
            IsEnabled = false;
            _timer.Interval = new TimeSpan(0, 0, 0, 1);
            _timer.Tick += init_timer_Tick;
            _timer.Start();
        }

        public override void Unload()
        {
            _timer.Stop();
            try
            {
                var result = TIR_Exit();
                ThrowErrorOnResult(result, "Error while shuting down Track IR");
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(exc.Message, exc);
            }
        }

        private static void ThrowErrorOnResult(int result, string message)
        {
            if (result != 0)
            {
                throw new Exception(message);
            }
        }
    }
}
