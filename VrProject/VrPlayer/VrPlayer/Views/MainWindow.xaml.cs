using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using VrPlayer.Helpers;
using VrPlayer.Service;
using VrPlayer.ViewModels;
using Application = System.Windows.Application;
using DataObject = System.Windows.DataObject;
using DragEventArgs = System.Windows.DragEventArgs;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using VrManager;
using System.Diagnostics;

namespace VrPlayer
{
    public partial class MainWindow : FullScreenWindow
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct KBDLLHOOKSTRUCT
        {
            public System.Windows.Forms.Keys key;
            public int scanCode;
            public int flags;
            public int time;
            public IntPtr extra;
        }
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int id, LowLevelKeyboardProc callback, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hook);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hook, int nCode, IntPtr wp, IntPtr lp);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string name);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern short GetAsyncKeyState(System.Windows.Forms.Keys key);
        private IntPtr ptrHook;
        private LowLevelKeyboardProc objKeyboardProcess;
        DispatcherTimer mIdle;
        private const long cIdleSeconds = 120;
        private MonitorManager monitor = new MonitorManager();
        private IntPtr captureKey(int nCode, IntPtr wp, IntPtr lp)
        {
            if (nCode >= 0)
            {
                KBDLLHOOKSTRUCT objKeyInfo = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lp, typeof(KBDLLHOOKSTRUCT));
                if (objKeyInfo.key == System.Windows.Forms.Keys.Right)
                {
                    System.Windows.Forms.Cursor.Position = new System.Drawing.Point(
                        MonitorManager.getMonitorCordX(0) + (int)(MonitorManager.getMonitorCord(0).Size.X / 2)
                        , MonitorManager.getMonitorCordY(0) + (int)(MonitorManager.getMonitorCord(0).Size.Y / 2));
                }
                if (objKeyInfo.key == System.Windows.Forms.Keys.Left)
                {
                    System.Windows.Forms.Cursor.Position = new System.Drawing.Point(MonitorManager.getMonitorCordX(1) + (int)(MonitorManager.getMonitorCord(1).Size.X / 2),
                        MonitorManager.getMonitorCordY(1) + (int)(MonitorManager.getMonitorCord(1).Size.Y / 2));

                }

            }


            return CallNextHookEx(ptrHook, nCode, wp, lp);
        }
        private const int INTERNET_OPTION_END_BROWSER_SESSION = 42;



       
        private readonly MainWindowViewModel _viewModel;

        private readonly MenuViewModel _menuViewModel;
        private ServerService _service;

        public MainWindow()
        {
            InitializeComponent();
            App.MainWindowPlayer = this;
            ProcessModule objCurrentModule = Process.GetCurrentProcess().MainModule;
            objKeyboardProcess = new LowLevelKeyboardProc(captureKey);
            ptrHook = SetWindowsHookEx(13, objKeyboardProcess, GetModuleHandle(objCurrentModule.ModuleName), 0);
            _service = new ServerService();
            try
            {
                _viewModel = ((App)Application.Current).ViewModelFactory.CreateMainWindowViewModel();
                DataContext = _viewModel;            
                _menuViewModel = ((App)Application.Current).ViewModelFactory.CreateMenuViewModel();
                



            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while initilizing Main window.", exc);
            }

            Loaded += MainWindow_Loaded;
            Closed += MainWindow_Closed;
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            _service.EndHosting();
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (_viewModel.StartUpConfig.FullScreen)
            {
                _viewModel.State.TrackerPlugin.Content.IsActive = true;

                if (_viewModel.StartUpConfig.ScreenNumber > 0 && _viewModel.StartUpConfig.ScreenNumber < Screen.AllScreens.Length)
                {
                    Screen secondScreen = Screen.AllScreens[_viewModel.StartUpConfig.ScreenNumber];
                    Rectangle secondRectangle = secondScreen.WorkingArea;
                    Top = secondRectangle.Top;
                    Left = secondRectangle.Left;
                }
                ToggleFullScreen();
            }
            else
            {
                if (_viewModel.StartUpConfig.ScreenNumber > 0 && _viewModel.StartUpConfig.ScreenNumber < Screen.AllScreens.Length)
                {
                    Screen secondScreen = Screen.AllScreens[_viewModel.StartUpConfig.ScreenNumber];
                    Rectangle secondRectangle = secondScreen.WorkingArea;
                    double tTop = Top;
                    double tLeft = Left;
                    Top = secondRectangle.Top + tTop;
                    Left = secondRectangle.Left + tLeft;
                }
            }


            //    if (Screen.AllScreens.Length > 1)
            //    {
            //        Screen secondScreen = Screen.AllScreens[1];
            //        Rectangle secondRectangle = secondScreen.WorkingArea;
            //        Top = secondRectangle.Top;
            //        Left = secondRectangle.Left;
            //    }

            //    else
            //    {
            //        Screen firstScreen = Screen.AllScreens[0];
            //        Rectangle firstRectangle = firstScreen.WorkingArea;
            //        Top = firstRectangle.Top;
            //        Left = firstRectangle.Left;
            //    }
            
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            _viewModel.KeyboardCommand.Execute(e.Key);
        }

        private void Window_OnDrop(object sender, DragEventArgs e)
        {
            try
            {
                //////////////////////////////////////////////////////////////////////
                // get filename that was dropped onto the window 
                //////////////////////////////////////////////////////////////////////
                var filename = ((DataObject)e.Data).GetFileDropList()[0];

                //////////////////////////////////////////////////////////////////////
                // play it
                //////////////////////////////////////////////////////////////////////
                _viewModel.MediaService.Load(filename);
            }
            catch (Exception exc)
            {
                //////////////////////////////////////////////////////////////////////
                // didn't work - why?
                //////////////////////////////////////////////////////////////////////
                Logger.Instance.Error("Error with the Drag&Drop", exc);
            }
        }


        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void FullScreenWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
          
        }

        private void FullScreenWindow_StateChanged(object sender, EventArgs e)
        {
           
        }

        
        public void ChangeSize()
        {
            //this.WindowState = WindowState.Normal;
            //this.WindowState = WindowState.Maximized; /////////////////////////
            //WindowStyle = WindowStyle.None;
            //ResizeMode = ResizeMode.NoResize;
            //th
        }
      
    }
}
