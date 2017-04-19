//Source based on: http://sixense.com/forum/vbulletin/showthread.php?3671-I-got-it-working!-Here-s-how

using System;
using System.Runtime.InteropServices;

namespace VrPlayer.Trackers.RazerHydraTracker
{
    public class RazerHydraWrapper
    {
        public static Int32 SIXENSE_SUCCESS = 0;
        public static Int32 SIXENSE_FAILURE = -1;

        public struct Vector3
        {
            public float x;
            public float y;
            public float z;
        }

        public struct Quaternion
        {
            public float x;
            public float y;
            public float z;
            public float w;
        }

        public struct SixenseControllerData
        {
            public Vector3 pos;
            public Vector3 rot_mat_x;
            public Vector3 rot_mat_y;
            public Vector3 rot_mat_z;
            public float joystick_x;
            public float joystick_y;
            public float trigger;
            public UInt32 buttons;
            public byte sequence_number;
            public Quaternion rot_quat;
            public ushort firmware_revision;
            public ushort hardware_revision;
            public ushort packet_type;
            public ushort magnetic_frequency;
            public Int32 enabled;
            public Int32 controller_index;
            public byte is_docked;
            public byte which_hand;
            public byte hemi_tracking_enabled;
        };

        public SixenseControllerData Data;

        [DllImport("sixense", CallingConvention=CallingConvention.Cdecl)]
        private static extern int sixenseInit();
        [DllImport("sixense", CallingConvention=CallingConvention.Cdecl)]
        private static extern int sixenseExit();
        [DllImport("sixense", CallingConvention=CallingConvention.Cdecl)]
        private static extern int sixenseGetNewestData(int which, out SixenseControllerData data);
        [DllImport("sixense", CallingConvention=CallingConvention.Cdecl)]
        private static extern int sixenseSetFilterEnabled(int filterEnabled);

        // [DllImport ("sixense, CallingConvention=CallingConvention.Cdecl")] private static extern int sixenseGetMaxBases( );
        // [DllImport ("sixense, CallingConvention=CallingConvention.Cdecl")] private static extern int sixenseSetActiveBase( int baseNum);
        // [DllImport ("sixense, CallingConvention=CallingConvention.Cdecl")] private static extern int sixenseIsBaseConnected(int baseNum );
        // [DllImport ("sixense, CallingConvention=CallingConvention.Cdecl")] private static extern int sixenseGetMaxControllers( );
        // [DllImport ("sixense, CallingConvention=CallingConvention.Cdecl")] private static extern int sixenseGetNumActiveControllers( );
        // [DllImport ("sixense, CallingConvention=CallingConvention.Cdecl")] private static extern int sixenseIsControllerEnabled(int which);
        // [DllImport ("sixense, CallingConvention=CallingConvention.Cdecl")] private static extern int sixenseGetAllNewestData(out _sixenseAllControllerData allData);
        // [DllImport ("sixense, CallingConvention=CallingConvention.Cdecl")] private static extern int sixenseGetAllData(int indexBack, out _sixenseAllControllerData allData);
        // [DllImport ("sixense, CallingConvention=CallingConvention.Cdecl")] private static extern int sixenseGetData(int which, int indexData,out _sixenseControllerData data);
        // [DllImport ("sixense, CallingConvention=CallingConvention.Cdecl")] private static extern int sixenseGetHistorySize( );
        // [DllImport ("sixense, CallingConvention=CallingConvention.Cdecl")] private static extern int sixenseGetFilterEnabled(out int filterEnabled);
        // [DllImport ("sixense, CallingConvention=CallingConvention.Cdecl")] private static extern int sixenseSetFilterParams(float nearRange, float nearVal, float farRange, float farVal );
        // [DllImport ("sixense, CallingConvention=CallingConvention.Cdecl")] private static extern int sixenseGetFilterParams(out float nearRange,out float nearVal,out float farRange,out float farVal );
        // [DllImport ("sixense, CallingConvention=CallingConvention.Cdecl")] private static extern int sixenseTriggerVibration(int controllerId, int duration100ms, int patternId );
        // [DllImport ("sixense, CallingConvention=CallingConvention.Cdecl")] private static extern int sixenseAutoEnableHemisphereTracking(int whichController );
        // [DllImport ("sixense, CallingConvention=CallingConvention.Cdecl")] private static extern int sixenseSetHighPriorityBindingEnabled(int onOrOff );
        // [DllImport ("sixense, CallingConvention=CallingConvention.Cdecl")] private static extern int sixenseGetHighPriorityBindingEnabled(out int onOrOff );

        public Int32 Init()
        {
            return sixenseInit();
        }

        public Int32 Exit()
        {
            return sixenseExit();
        }

        public Int32 SetFilterEnabled(Int32 enabled)
        {
            return sixenseSetFilterEnabled(enabled);
        }

        public Int32 GetNewestData(Int32 id)
        {
            return sixenseGetNewestData(id, out Data);
        }
    }
}