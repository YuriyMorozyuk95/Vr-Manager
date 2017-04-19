using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VrManager.ProgramSetting;
using VrManager.Data.Concrete;
using System.IO;
using VrManager.Helpers;
using VrManager.Pages;
using log4net;
using VrManager.Data.Entity;
using System.Diagnostics;

namespace VrManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static bool _isLicensed = false;
        private static Setting _setting;
        public static bool IsAuthorized { get; set; }
        public static ModelUser AuthorizedUser { get; internal set; }
        public static bool ІsLicensed
        {
            get
            {
                return _isLicensed;
            }
            set
            {
                _isLicensed = value;
            }
        }
        public static ProcessorHelper CurrentProceess { get; set; }
        public static ILog Logger { get; private set; } = LogManager.GetLogger("VrManagerLogger"); 
        public static MainWindow MainWnd { get; set; }
        public static LockScreenWindow LockDisplayWindow { get; set; }
        public static Frame Frame { get; set; }
        public static Setting Setting
        {
            get
            {
                return _setting ?? (_setting = new Setting());
            }

        }
        public static EntityRepository Repository = new EntityRepository();
      
        public static string PathToBackgroundImage
        {
            get
            {
                return (App.Setting.PathToFolderFiles + @"\Config\Background\Background.png");
            }
        }
        public static int CurentVideoMonitor = 0;

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                Convert.ToUInt32("sdsa");
                if (MonitorManager.IsTwoMonitor)
                {
                    LockDisplayWindow.Close();
                }
                MainWnd.Close();
                Taskbar.Show();

                if (CurrentProceess?.IsProcessOpened ?? false)
                {
                    CurrentProceess.StopProcess();
                }
                Repository.Dispose();
                Logger.Info("End seanse in VrManager!!!!!");
            }
            catch(Exception ex)
            {
                App.Logger.Error($"exeption {ex.Message} ||| in class {nameof(App)} and method {GetStackTraseMethod().GetMethod().Name}");
            }
        }

        public StackFrame GetStackTraseMethod()
        {
            return _stackTrace.GetFrame(1);
        }

        public void Application_Startup(object sender, StartupEventArgs e)
        { 
            try
            {
                log4net.Config.XmlConfigurator.Configure();
               
                Logger.Info("Start seanse in VrManager!!!!!");
                var a = App.Repository.Setting;
            }
            catch(Exception ex)
            {
                App.Logger.Error($"exeption {ex.Message} (Пожалуста установите Microsoft SQL Server Compact 4.0) ||| in class {nameof(App)} and method {GetStackTraseMethod().GetMethod().Name}");
                MessageBox.Show("Пожалуста установите Microsoft SQL Server Compact 4.0");
                return;
            }
            try
            {
                MainWnd = new VrManager.MainWindow();
                MainWnd.Show();
                var _rep = App.Repository;
                if (MonitorManager.IsTwoMonitor)
                {
                    LockDisplayWindow = new LockScreenWindow();
                }

                if (_rep.Setting.FirstOrDefault() == null)
                {
                    _rep.FirstExecute();
                }
                else
                {
                    Setting.Import();
                }

                ObserverUserActivity.StartActivityObserv();

                if (App.Setting.IsKioskMode)
                {
                    Taskbar.Hide();
                    MainWnd.Topmost = true;
                }
                else
                {
                    MainWnd.Topmost = false;
                }

                if (MonitorManager.IsTwoMonitor)
                {
                    LockDisplayWindow.Show();
                }
                MainWnd.Show();

                MonitorManager.RestartPositionWindows();

            }
            catch (Exception ex)
            {
                try
                {
                    MainWnd.Show();
                }
                catch
                {
                    MessageBox.Show("Подключите оккулус или второй монитор");
                }
                App.Frame.Navigate(new MainSettingPage(true));
            }

        }

        private void LoadFontsToStyle()
        {
            //StyleManager.AddIconFont("OpenFileDialog");
        }

        public static void RestartApp()
        {
            try
            {
            var _rep = App.Repository;
            MainWnd = new MainWindow();
            if (MonitorManager.IsTwoMonitor)
            {
                LockDisplayWindow = new LockScreenWindow();

            }
            else
            {
                MessageBox.Show("Пожалуйста подключите второй монитор или окулус");
                return;
            }
           
                if (_rep.Setting.FirstOrDefault() == null)
                {
                    _rep.FirstExecute();
                }
                else
                {
                    Setting.Import();
                }


                if (App.Setting.IsKioskMode)
                {
                    Taskbar.Hide();
                    MainWnd.Topmost = true;
                }
                else
                {
                    MainWnd.Topmost = false;
                }

                if (MonitorManager.IsTwoMonitor)
                {
                    LockDisplayWindow.Show();
                }
                MainWnd.Show();
                MonitorManager meneger = new MonitorManager();

                if (MonitorManager.IsTwoMonitor)
                {
                    MonitorManager.RestartPositionWindows();
                }

            }
            catch (Exception ex)
            {
                MainWnd.Show();
                App.Frame.Navigate(new MainSettingPage(true));
            }
        }
        public static bool IsVideoMod { get; set; } = false;

        public static  RoutedEventHandler LaunchMedia;
        public static  RoutedEventHandler StartMedia;
        public static  RoutedEventHandler PauseMedia;
        public static  RoutedEventHandler StopMedia;
        private StackTrace _stackTrace;

        public static void OnLaunchMedia()
        {
            LaunchMedia?.Invoke(null, null);
        }
        public static void OnStartMedia()
        {
            StartMedia?.Invoke(null, null);
        }
        public static void OnPauseMedia()
        {
            PauseMedia?.Invoke(null, null);
        }
        public static void OnStopMedia()
        {
            StopMedia?.Invoke(null, null);
        }

    }
}
