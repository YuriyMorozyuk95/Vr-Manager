﻿using System;
﻿using System.ComponentModel.Composition;
﻿using System.Runtime.Serialization;
﻿using System.Windows;
﻿using System.Windows.Threading;
using System.Windows.Media.Media3D;

using VrPlayer.Contracts.Trackers;
﻿using VrPlayer.Helpers;

namespace VrPlayer.Trackers.RazerHydraTracker
{
    [DataContract]
    public class RazerHydraTracker: TrackerBase, ITracker
    {
        private const int HydraId = 0;
        private const int SixenseButtonStart = 1;

        private readonly RazerHydraWrapper _hydra = new RazerHydraWrapper();
        private readonly DispatcherTimer _timer;

        public static readonly DependencyProperty FilterEnabledProperty =
            DependencyProperty.Register("FilterEnabledFilterEnabled", typeof(bool),
            typeof(RazerHydraTracker), new FrameworkPropertyMetadata(false));
        [DataMember]
        public bool FilterEnabled
        {
            get { return (bool)GetValue(FilterEnabledProperty); }
            set { SetValue(FilterEnabledProperty, value); }
        }

        public RazerHydraTracker()
        {
            _timer = new DispatcherTimer(DispatcherPriority.Input);
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 15);
            _timer.Tick += timer_Tick;
        }

        public override sealed void Load()
        {
            try
            {
                IsEnabled = true;

                var result = _hydra.Init();
                ThrowErrorOnResult(result, "Error while initializing the Razer Hydra");

                var filter = FilterEnabled ? 1 : 0;
                result = _hydra.SetFilterEnabled(filter);
                ThrowErrorOnResult(result, "Error while settings the Razer Hydra filter");
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(exc.Message, exc);
                IsEnabled = false;
            }
            _timer.Start();
        }

        public override sealed void Unload()
        {
            _timer.Stop();
            try
            {
                var result = _hydra.Exit();
                ThrowErrorOnResult(result, "Error shutting down the Razer Hydra");
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(exc.Message, exc);
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                var result = _hydra.GetNewestData(HydraId);
                ThrowErrorOnResult(result, "Error while getting data from the Razer Hydra");

                RawPosition = new Vector3D(
                    _hydra.Data.pos.x,
                    -_hydra.Data.pos.y,
                    _hydra.Data.pos.z);

                RawRotation = new Quaternion(
                    _hydra.Data.rot_quat.x,
                    -_hydra.Data.rot_quat.y,
                    _hydra.Data.rot_quat.z,
                    -_hydra.Data.rot_quat.w);

                if (_hydra.Data.buttons == SixenseButtonStart)
                {
                    Calibrate();
                }

                UpdatePositionAndRotation();
            }
            catch(Exception exc)
            {
                Logger.Instance.Error(exc.Message, exc);
            }
        }

        private void ThrowErrorOnResult(int result, string message)
        {
            if (result == -1)
            {
                throw new Exception(message);
            }
        }
    }
}