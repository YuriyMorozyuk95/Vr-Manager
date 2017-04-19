using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Navigation;
using Vr.License;
using VrPlayer.Helpers;
using VrPlayer.Models.Config;
using VrPlayer.Models.Plugins;
using VrPlayer.Models.Presets;
using VrPlayer.Models.Settings;
using VrPlayer.Models.State;
using VrPlayer.Properties;
using VrPlayer.Services;
using VrPlayer.ViewModels;
using log4net;
using System.Threading;
using System.Threading.Tasks;

namespace VrPlayer
{
    public partial class App : Application
    {
        private const string DefaultMedia = @"Samples\1-Grid.jpg";
        private const string UriScheme = @"vrplayer:";

        private readonly IApplicationConfig _appConfig;
        private readonly ISettingsManager _settingsManager;
        private readonly IPluginManager _pluginManager;
        private readonly IApplicationState _appState;
        private readonly IPresetsManager _presetsManager;
        private readonly IMediaService _mediaService;
        private readonly ILog logger = log4net.LogManager.GetLogger
    (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ViewModelFactory ViewModelFactory { get; private set; }
        public static MainWindow MainWindowPlayer { get; internal set; }

        public App()
        {
            try
            {
              
                var applicationPath = new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)).LocalPath;
                string[] pluginFolders = {"Medias", "Effects", "Distortions", "Trackers", "Projections", "Stabilizers"};

                //Init services and inject dependancies
                _appConfig = new AppSettingsApplicationConfig();
                _pluginManager = new DynamicPluginManager(applicationPath, pluginFolders);
                _appState = new DefaultApplicationState(_appConfig, _pluginManager);
                _settingsManager = new SettingsManager(_appState, _pluginManager, _appConfig, Settings.Default);
                _presetsManager = new PresetsManager(_appConfig, _appState, _pluginManager);
                _mediaService = new MediaService(_appState, _pluginManager, _presetsManager);

                ViewModelFactory = new ViewModelFactory(_appConfig, _pluginManager, _appState, _presetsManager,
                    _mediaService);

                //Проверка лицензии ассинхронно, чтобы не задерживать движения - на коленке сделано
                //Task.Factory.StartNew(() =>
                //{
                //    try
                //    {
                //        if (File.Exists("Vr.lic") == false)
                //        {

                //            throw new VrLicenseException("Файл лицензии не найден!");
                //        }

                //        using (StreamReader sr = new StreamReader("Vr.lic"))
                //        {
                //            LicenseProvider licenseProvider = new LicenseProvider();
                //            var licese = licenseProvider.GetLicenseValue();
                //            String line = sr.ReadToEnd();
                //            if (licese.Equals(line) == false)
                //            {
                //                throw new VrLicenseException("Неверный файл лицензии!");
                //            }
                //        }
                //    }
                //    catch (Exception e)
                //    {

                //        Logger.Instance.Error("Error while initializing application.", e);
                //        Environment.Exit(1);
                //    }
                //});



            }
            //catch (Exception le)
            //{
            //    MessageBox.Show(le.Message);
            //    Application.Current.Shutdown(-1);
            //}
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while initializing application.", exc);
            }
        }

        public void App_OnStartup(object sender, StartupEventArgs e)
        {
            try
            {
               // using (FileStream fs = File.Create(@"D:\hh.txt"))
                    //   TaskBarHelper.HideTaskBar();
                    //Set default culture
                    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                
                //Load settings
                _settingsManager.Load();
                
                //Load media and preset from command line arguments
                var args = Environment.GetCommandLineArgs();

                var config = StartUpConfig.CreateConfig(args, Path.GetFullPath(DefaultMedia));
                Logger.Instance.Info(string.Format("Loading media '{0}'...", config.MediaPath));

                ((MediaService) _mediaService).StartUpConfig = config;
                _mediaService.Load(config.MediaPath);


                if (string.IsNullOrWhiteSpace(config.PresetPath) == false)
                {
                    Logger.Instance.Info(string.Format("Loading preset '{0}'...", config.PresetPath));
                    _presetsManager.LoadFromUri(config.PresetPath);
                }
                ViewModelFactory.StartUpConfig = config;


                //////////////////////////////////////////////////////////////////////////
                //// loading sequence for vrplayer URI-schemes:
                //////////////////////////////////////////////////////////////////////////
                ////if (args.Length > 1 && args[1].Length > UriScheme.Length)
                //if (args.Length > 1 && args[1].ToLower().StartsWith(UriScheme))
                //{
                //    ////////////////////////////////////////////////////////////////////
                //    // this is a "vrplayer:" command line parameter
                //    ////////////////////////////////////////////////////////////////////
                //    var source = args[1].Remove(0, UriScheme.Length);

                //    Logger.Instance.Info(string.Format("Loading media '{0}'...", source));
                //    _mediaService.Load(source);
                //}
                //else if (args.Length > 1)
                //{
                //    /////////////////////////////////////////////////////////////////////
                //    // a command line parameter was passed, so let's assume the first one
                //    // is a media file to load and play
                //    /////////////////////////////////////////////////////////////////////

                //    var source = args[1];
                //    if (File.Exists(source))
                //    {
                //        Logger.Instance.Info(string.Format("Loading media '{0}'...", source));
                //        _mediaService.Load(source);
                //    }

                //}
                //else
                //{

                //    /////////////////////////////////////////////////////////////////////
                //    // CL: nothing passed, load the default media
                //    /////////////////////////////////////////////////////////////////////
                //    _mediaService.Load(Path.GetFullPath(DefaultMedia));

                //}

                //if (args.Length > 2)
                //{
                //    var preset = args[2];
                //    Logger.Instance.Info(string.Format("Loading preset '{0}'...", preset));
                //    _presetsManager.LoadFromUri(preset);   
                //}              

            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while starting up application.", exc);
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                _settingsManager.Save();
                _pluginManager.Dispose();
                foreach (NavigationWindow win in Current.Windows)
                {
                    win.Close();
                }
                TaskBarHelper.ShowTaskBar();
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while closing application.", exc);
            }
        }
    }
}
