using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;


namespace VrManager.Helpers
{
    public static class ObserverUserActivity
    {
        private static DispatcherTimer TimerObserver;

        private static TimeSpan _timeFOrCheck = new TimeSpan(0, 0, 5);
        public static TimeSpan TimeForCheck
        {
            get
            {
                return _timeFOrCheck;
            }
            set
            {
                _timeFOrCheck = value;
            }
        }
        public static TimeSpan CurrentTime { get; set; }
        public static bool IsUserActive { get; private set; }
        public static bool IsAdvertisingShowing { get; private set; } = false;
        

        public static event EventHandler HideAdvertising;
        public static event EventHandler ShowAdvertising;

        public static void StartActivityObserv()
        {
            TimerObserver = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(1)
            };

         
            App.MainWnd.SetAdvertisingVideo(GetUriVideoList());

            TimerObserver.Tick += TimerObserver_Tick;
            App.MainWnd.MouseMove += HookMouse;
            try
            {
                App.LockDisplayWindow.MouseMove += HookMouse;
            }
            catch
            {
                MessageBox.Show("Подключите пожалуйста второй монитор или Окулус");
            }
            App.MainWnd.KeyUp += HookKey;
            App.LockDisplayWindow.KeyUp += HookKey;

            CurrentTime = new TimeSpan(0, 0, 0);
            TimerObserver.Start();
        }

        private static List<Uri> GetUriVideoList()
        {
            string pathToFolder = App.Setting.PathToFolderFiles + @"\Video\AdvertisingVideo";
            string[] allFileInDirectory = Directory.GetFiles(pathToFolder);
            List<Uri> list = new List<Uri>();

            foreach (string file in allFileInDirectory)
            {
                list.Add(new Uri(file));
            }

            if (list.Count == 0)
            {
                return null;
            }
            else
            {
                return list;
            }
        }

        public static void ResumeActivityObserv()
        {
            if (!TimerObserver.IsEnabled)
            {
                TimerObserver.Start();
            }
        }
        public static void EndActivityObserv()
        {
            TimerObserver.Stop();
        }
        private static void HookComplited()
        {
            if (IsAdvertisingShowing)
            {
                HideAdvertising?.Invoke(TimerObserver, new EventArgs());
                IsAdvertisingShowing = false;
            }
            IsUserActive = true;
        }

        private static void HookKey(object sender, System.Windows.Input.KeyEventArgs e)
        {
            HookComplited();
        }
        private static void HookMouse(object sender, System.Windows.Input.MouseEventArgs e)
        {
            IsUserActive = true;
        }
        private static void TimerObserver_Tick(object sender, EventArgs e)
        {
            CurrentTime += TimeSpan.FromSeconds(1);

            if (CurrentTime == TimeForCheck)
            {
                if (IsUserActive && !IsAdvertisingShowing)
                {
                    HideAdvertising?.Invoke(TimerObserver, new EventArgs());
                }
                else
                {
                    IsAdvertisingShowing = true;
                    ShowAdvertising?.Invoke(TimerObserver, new EventArgs());
                }

                IsUserActive = false;
                CurrentTime = new TimeSpan(0, 0, 0);
            }
        }

    }

       
}
