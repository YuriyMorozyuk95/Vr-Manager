using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using Forms = System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using VrManager.View;
using VrManager.Data.Concrete;
using VrManager.Data.Entity;
using System.Linq;
using VrManager.Pages;
using MahApps.Metro.Controls;
using System.ComponentModel;
using System.IO;
using System.Windows.Controls;
using VrManager.Helpers;
using System.Threading;
using System.Collections.Generic;

namespace VrManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region KioskModeSetting
        [StructLayout(LayoutKind.Sequential)]
        private struct KBDLLHOOKSTRUCT
        {
            public Forms.Keys key;
            public int scanCode;
            public int flags;
            public int time;
            public IntPtr extra;
        }

        internal void ShowHomeButton(bool isShowHomeButton)
        {
            HomeButton.IsEnabled = isShowHomeButton;
            if (isShowHomeButton)
            {
                App.Frame.Navigate(new System.Windows.Navigation.PageFunction<string>() { RemoveFromJournal = true });
                App.Frame.Navigate(new StartUpPage());
            }
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
        private static extern short GetAsyncKeyState(Forms.Keys key);
        private IntPtr ptrHook;
        private LowLevelKeyboardProc objKeyboardProcess;
        DispatcherTimer mIdle;
        private const long cIdleSeconds = 120;
        private MonitorManager monitor = new MonitorManager();

        public bool StartOrPause { get; set; } = false;
        private IntPtr captureKey(int nCode, IntPtr wp, IntPtr lp)
            {
            if (nCode >= 0)
            {
                KBDLLHOOKSTRUCT objKeyInfo = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lp, typeof(KBDLLHOOKSTRUCT));
                if (objKeyInfo.key == Forms.Keys.Right)
                {
                    MonitorManager.CenteredCursorInRight();

                }
                if (objKeyInfo.key == Forms.Keys.Left)
                {
                    MonitorManager.CenteredCursorInLeft();
                }

                KeyBordControlMedia(objKeyInfo);

                if (App.Setting.IsKioskMode)
                {
                    if (objKeyInfo.key == Forms.Keys.RWin ||
                      objKeyInfo.key == Forms.Keys.LWin ||
                      objKeyInfo.key == Forms.Keys.Alt ||
                      objKeyInfo.key == Forms.Keys.Tab ||
                      objKeyInfo.key == Forms.Keys.Delete ||
                      objKeyInfo.key == Forms.Keys.F4 ||
                      objKeyInfo.key == Forms.Keys.Control ||
                      objKeyInfo.key == Forms.Keys.Escape)
                        return (IntPtr)1;
                }
            }


            return CallNextHookEx(ptrHook, nCode, wp, lp);
        }
        private void KeyBordControlMedia(KBDLLHOOKSTRUCT objKeyInfo)
       {
            if(objKeyInfo.key == Forms.Keys.Escape)
            {
                App.OnStopMedia();
            }
            if (objKeyInfo.key == Forms.Keys.Space)
            {
                if (App.IsVideoMod)
                {
                    if (StartOrPause)
                    {
                        Thread.Sleep(1000);
                            App.OnStartMedia();
                    }
                    else
                    {
                        Thread.Sleep(1000);
                        App.OnPauseMedia();
                    }

                    StartOrPause = !StartOrPause;
                }
                else
                {
                    App.OnStartMedia();
                }
            }
        }

        private const int INTERNET_OPTION_END_BROWSER_SESSION = 42;
        private bool _canGoBack;

        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int lpdwBufferLength);




        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (App.Repository.Users.FirstOrDefault() != null)
                {
                    App.Frame.Navigate(new AuthorizePage(true));
                }
                else
                {
                    Application.Current.Shutdown();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Если вы уже выбрали путь к файлам, перапустите прилажения для коректной роботы");
                Application.Current.Shutdown();
            }
            //this.Close();
        }

        public void ChangeTitle(string title)
        {
            TitleContent.Content = title;
        }

        public MainWindow()
        {
            InitializeComponent();
            App.Frame = MainFrame;
            ProcessModule objCurrentModule = Process.GetCurrentProcess().MainModule;
            objKeyboardProcess = new LowLevelKeyboardProc(captureKey);
            ptrHook = SetWindowsHookEx(13, objKeyboardProcess, GetModuleHandle(objCurrentModule.ModuleName), 0);

            ObserverUserActivity.ShowAdvertising += ObserverUserActivity_ShowAdvertising;
            ObserverUserActivity.HideAdvertising += ObserverUserActivity_HideAdvertising;


            var size1 = MonitorManager.ConnectedMonitors[App.Setting.NumberLeftMonitor].Size;
            var size2 = MonitorManager.ConnectedMonitors[App.Setting.NumberRightMonitor].Size;

            Height = size1.Y;
            Width = size1.X;
        }

        private void ObserverUserActivity_HideAdvertising(object sender, EventArgs e)
        {
            AdvertisingScreen.Visibility = Visibility.Collapsed;
            AdvertisingBanner.StopPlayer();
        }

        private void ObserverUserActivity_ShowAdvertising(object sender, EventArgs e)
        {
            if (AdvertisingBanner.VideoCollection != null)
            {
                AdvertisingScreen.Visibility = Visibility.Visible;
                AdvertisingBanner.StartPlayer();
            }
        }

        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            if (App.Setting.IsKioskMode)
            {
                Taskbar.Hide();
            }
            MainFrame.Navigate(new StartUpPage());
            //MainFrame.Navigate(new PageTest());
            App.MainWnd = this;


        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
          //  SetFullSize();
        }

        public void SetFullSize()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            App.Frame.Navigate(new StartUpPage());
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.Frame.CanGoBack)
            {
                App.Frame.GoBack();
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.Frame.CanGoForward)
            {
                App.Frame.GoForward();
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Window_LocationChanged(object sender, EventArgs e)
        {
            WindowState = WindowState.Normal;
            WindowState = WindowState.Maximized;
        }

        private void MainFrame_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() => { })).Wait();
            Page a = e.Content as Page;
            string title = a.Title;
            if (title == "Дополнительные компоненты" || title == "PageTest")
            {
                return;
            }
            if (!(title == "Главное меню" || title == "Проверка лицензии" || title == "Меню настроек" || title == "Основные настройки" || title == "Авторизацыя"))
            {
                //PermissionsHelper.NotLicensedPage();
            }

        }

        private void MouseObserverEvent(object sender, MouseButtonEventArgs e)
        {
            Observer.ObservIteration();
        }

        public void SetAdvertisingVideo(List<Uri> videoList)
        {
            AdvertisingBanner.VideoCollection = videoList;
        }
    }

   
}
