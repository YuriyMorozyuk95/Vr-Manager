using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Runtime.Serialization;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using System.Threading;
using YEISensorLib.RawApi;
using YEISensorLib.Sharped;

using VrPlayer.Contracts.Trackers;

namespace VrPlayer.Trackers.YEI3SpaceTracker
{
    [DataContract]
    public class YEI3SpaceTracker : TrackerBase, ITracker, INotifyPropertyChanged
    {

        private SensorDevice _device;
        private ThreeSpaceInterop.DataCallbackDelegate _keepCallbackAlive;
        private uint _lastTimeStamp;
        private Vector3D _velocityVec;
        private Vector3D _positionVec;
        // keepCallbackAlive is to prevent crash from garbage collection not being able to track into the unmanged code of ThreeSpace_API.dll
        private void dataCallbackFunc(uint deviceId, IntPtr outputData, uint outputDataLength, IntPtr timestamp)
        {
            unsafe
            {
                float* ptr = (float*)outputData;
                uint time = ((uint*)timestamp)[0];
                Monitor.Enter(this);
                this.Dispatcher.Invoke((Action)(() =>
                    {
                        /*if (_lastTimeStamp == 0)
                        {
                            RawPosition = PositionScaleFactor * _positionVec;
                            _lastTimeStamp = time;
                        }
                        else
                        {
                            double timeDiff = (time - _lastTimeStamp)*0.000001;
                            _lastTimeStamp = time;
                            _velocityVec += new Vector3D(ptr[4] * timeDiff, ptr[5] * timeDiff, ptr[6] * timeDiff);
                            _positionVec += new Vector3D(_velocityVec.X * timeDiff, _velocityVec.Y * timeDiff, _velocityVec.Z * timeDiff);
                            RawPosition = _positionVec;
                        }*/
                        RawRotation = new System.Windows.Media.Media3D.Quaternion(
                            ptr[0],
                            ptr[1],
                            ptr[2],
                            ptr[3]);

                        UpdatePositionAndRotation();
                    }));

                Monitor.Exit(this);
            }
        }

        public override void Load()
        {
            try
            {
                IsEnabled = true;
                PositionScaleFactor = 0.1;
                _lastTimeStamp = 0;
                _velocityVec = new Vector3D(0, 0, 0);
                _positionVec = new Vector3D(0, 0, 0);
                RawPosition = _positionVec;
                _device = SensorDevices.GetFirstAvailable(FilterEnum.FindUSB);
                _keepCallbackAlive = new ThreeSpaceInterop.DataCallbackDelegate(dataCallbackFunc);
                // keepCallbackAlive is to prevent crash from garbage collection not being able to track into the unmanged code of ThreeSpace_API.dll
                ThreeSpaceInterop.SetNewDataCallBack(_device.DeviceId, _keepCallbackAlive);
                StreamCommandSlots slots = new StreamCommandSlots(StreamCommand.TSS_NULL);
                slots.Slot0 = StreamCommand.TSS_GET_TARED_ORIENTATION_AS_QUATERNION;
                slots.Slot1 = StreamCommand.TSS_GET_CORRECTED_ACCELEROMETER_VECTOR;
                ThreeSpaceInterop.SetStreamingTiming(_device.DeviceId, 0, 0xffffffff, 0, ref _device.TimeStamp);
                ThreeSpaceInterop.SetStreamingSlots(_device.DeviceId, ref slots, ref _device.TimeStamp);
                ThreeSpaceInterop.StartStreaming(_device.DeviceId, ref _device.TimeStamp);
                Calibrate();
            }
            catch (Exception exc)
            {
                IsEnabled = false;
            }
        }

        public override void Unload()
        {
            if (_device.DeviceId != 0)
            {
                ThreeSpaceInterop.StopStreaming(_device.DeviceId, ref _device.TimeStamp);
                ThreeSpaceInterop.CloseDevice(_device.DeviceId);
            }
        }
    }
}
