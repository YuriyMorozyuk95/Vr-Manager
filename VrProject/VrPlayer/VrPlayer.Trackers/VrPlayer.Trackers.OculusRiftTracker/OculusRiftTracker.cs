using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Windows.Threading;
using System.Windows.Media.Media3D;
using VrPlayer.Contracts.Trackers;
using VrPlayer.Helpers;

namespace VrPlayer.Trackers.OculusRiftTracker
{
    [DataContract]
    unsafe public class OculusRiftTracker : TrackerBase, ITracker
    {
        [DllImport(@"RiftWrapper.dll")]
        static extern int OVR_Init();

        [DllImport(@"RiftWrapper.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int OVR_Peek(float* w, float* x, float* y, float* z);

        private readonly DispatcherTimer _timer;
            
        public OculusRiftTracker()
        {
            _timer = new DispatcherTimer(DispatcherPriority.Send);
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 15);
            _timer.Tick += timer_Tick;
        }

        public override void Load()
        {
            try
            {
                if (!IsEnabled)
                {
                    IsEnabled = true;
                    var result = OVR_Init();
                    ThrowErrorOnResult(result, "Error while initializing the Oculus Rift");
                }
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
        }

        void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                float w, x, y, z;
                var result = OVR_Peek(&w, &x, &y, &z);
                ThrowErrorOnResult(result, "Error while getting data from the Razer Hydra");

                RawRotation = new Quaternion(x, -y, z, -w);

                UpdatePositionAndRotation();
            }
            catch(Exception exc)
            {
                Logger.Instance.Error(exc.Message, exc);
            }
        }

        private static void ThrowErrorOnResult(int result, string message)
        {
            if (result == -1)
            {
                throw new Exception(message);
            }
        }
    }
}