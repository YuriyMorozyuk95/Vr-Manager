using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using VrManager.Helpers;
using VrManager.MonitorManagerHelper;

namespace VrManager
{

    public class MonitorManager
    {
        public static string PlayerWinName { get; } = "VR Player";
        public static string VrManagerName { get; } = "VrManager";
        public static string NameProgram { get; set; }

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        public MonitorManager()
        {
            ConnectedMonitors = new List<Monitor>();
            foreach (Screen screen in Screen.AllScreens)
            {
                Monitor monitor = new Monitor
                {
                    NameScreen = screen.DeviceName,
                    Coord = new Coordinates
                    {
                        X = screen.WorkingArea.X,
                        Y = screen.WorkingArea.Y
                    },
                    Size = new Coordinates
                    {
                        X = screen.Bounds.Size.Width,
                        Y = screen.Bounds.Size.Height
                    }

                };
                ConnectedMonitors.Add(monitor);
            }
        }

        public static bool IsTwoMonitor
        {
            get
            {
                return Screen.AllScreens.Count() == 2;
            }
        }

        public static List<Monitor> ConnectedMonitors { get; private set; }
        /// <summary>
        /// set window to 2 locations
        /// </summary>
        public void SetLocations(int numMonitor)
        {

            switch (numMonitor)
            {
                case 2:
                    MoveWindow(VrManagerName, 0);
                    MoveWindow(PlayerWinName, 1);
                    App.LockDisplayWindow.ChangeDisplay(1); 
                    App.CurentVideoMonitor = 1;
                    break;
                case 1:
                 
                    MoveWindow(VrManagerName, 1);
                    MoveWindow(PlayerWinName, 0);
                    App.LockDisplayWindow.ChangeDisplay(0);
                    App.CurentVideoMonitor = 0;
                    break;
            }
              System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() => { })).Wait();
        }
        public bool IsWindowPlayerExist()
        {
            IntPtr findedWindow = FindWindow(null, PlayerWinName);

            if (findedWindow.ToString() != "0")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static Monitor getMonitorCord(int id)
        {
            if (id < ConnectedMonitors.Count)
            {
                return ConnectedMonitors[id];
            }
            else
            {
                throw new Exception("Monitor not finded");
            }
        }

        public static int getMonitorCordX(int id)
        {
            if (id < ConnectedMonitors.Count)
            {
                return ConnectedMonitors[id].Coord.X;
            }
            else
            {
                throw new Exception("Monitor not finded");
            }
        }
        public static int getMonitorCordY(int id)
        {
            if (id < ConnectedMonitors.Count)
            {
                return ConnectedMonitors[id].Coord.Y;
            }
            else
            {
                throw new Exception("Monitor not finded");
            }
        }

        public static void MoveWindow(string titleWindow, int monitorId)
        {
            Monitor monit = getMonitorCord(monitorId);
            IntPtr intPrt = FindWindow(null, titleWindow);
            MoveWindow(intPrt, monit.Coord.X, monit.Coord.Y, monit.Size.X, monit.Size.Y, true);
            System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() => { })).Wait();
        }


        public static void CenteredCursorInLeft()
        {
            System.Windows.Forms.Cursor.Position = new System.Drawing.Point(
                getMonitorCordX(App.Setting.NumberLeftMonitor) + (int)(getMonitorCord(App.Setting.NumberLeftMonitor).Size.X / 2)
                , getMonitorCordY(App.Setting.NumberLeftMonitor) + (int)(getMonitorCord(App.Setting.NumberLeftMonitor).Size.Y / 2));
        }
        public static void CenteredCursorInRight()
        {
            System.Windows.Forms.Cursor.Position = new System.Drawing.Point(
                getMonitorCordX(App.Setting.NumberRightMonitor) + (int)(getMonitorCord(App.Setting.NumberRightMonitor).Size.X / 2)
                , getMonitorCordY(App.Setting.NumberRightMonitor) + (int)(getMonitorCord(App.Setting.NumberRightMonitor).Size.Y / 2));
        }

        public static void RestartPositionWindows()
        {
            App.LockDisplayWindow.WindowState = WindowState.Normal;
            App.LockDisplayWindow.WindowState = WindowState.Maximized;
            App.MainWnd.WindowState = WindowState.Normal;
            App.MainWnd.WindowState = WindowState.Maximized;
            MoveWindow(App.LockDisplayWindow.Title, App.Setting.NumberLeftMonitor);
            MoveWindow(App.MainWnd.Title, App.Setting.NumberRightMonitor);
        }

    }
}
