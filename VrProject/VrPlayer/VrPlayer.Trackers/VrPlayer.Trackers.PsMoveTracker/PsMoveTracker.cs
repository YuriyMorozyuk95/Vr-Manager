using System;
using System.Runtime.Serialization;
using System.Threading;
using System.Windows.Media.Media3D;
using MoveFramework_CS;

using VrPlayer.Contracts.Trackers;
using VrPlayer.Helpers;

namespace VrPlayer.Trackers.PsMoveTracker
{
    [DataContract]
    public class PsMoveTracker : TrackerBase, ITracker
    {
        public PsMoveTracker()
        {
        }

        public override void Load()
        {
            try
            {
                IsEnabled = true;
                MoveWrapper.init();
                var moveCount = MoveWrapper.getMovesCount();
                if (moveCount <= 0)
                {
                    IsEnabled = false;
                    return;
                }

                MoveWrapper.setRumble(0, 255);
                Thread.Sleep(40);
                MoveWrapper.setRumble(0, 0);

                MoveWrapper.subscribeMoveUpdate(
                    MoveUpdateCallback,
                    MoveKeyDownCallback,
                    MoveKeyUpCallback,
                    NavUpdateCallback,
                    NavKeyDownCallback,
                    NavKeyUpCallback
                    );
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(exc.Message, exc);
                IsEnabled = false;
            }
        }

        public override void Unload()
        {
            try
            {
                MoveWrapper.unsubscribeMove();
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(exc.Message, exc);
            }
        }

        void MoveUpdateCallback(int id, MoveWrapper.Vector3 pos, MoveWrapper.Quaternion rot, int trigger)
        {
            RawPosition = new Vector3D(pos.x, pos.y, pos.z);
            RawRotation = new Quaternion(rot.x, -rot.y, rot.z, -rot.w);

            if (MoveWrapper.getButtonState(0, MoveButton.B_START))
            {
                Dispatcher.Invoke((Action)(Calibrate));
            }

            Dispatcher.Invoke((Action)(UpdatePositionAndRotation));
        }

    	void MoveKeyUpCallback(int id, int keyCode)
        {
        }

    	void MoveKeyDownCallback(int id, int keyCode)
        {
        }

    	void NavUpdateCallback(int id, int trigger1, int trigger2, int stickX, int stickY)
        {
        }

    	void NavKeyUpCallback(int id, int keyCode)
        {
        }

    	void NavKeyDownCallback(int id, int keyCode)
        {
        }
    }
}
