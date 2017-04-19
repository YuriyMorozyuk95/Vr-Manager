using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Distortions;
using VrPlayer.Contracts.Effects;
using VrPlayer.Contracts.Medias;
using VrPlayer.Contracts.Projections;
using VrPlayer.Contracts.Trackers;
using VrPlayer.Helpers;
using VrPlayer.Helpers.Mvvm;
using VrPlayer.Models.Plugins;
using VrPlayer.Models.Presets;
using VrPlayer.Models.State;
using VrPlayer.Models.Config;
using VrPlayer.Views.Dialogs;
using VrPlayer.Views.Settings;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace VrPlayer.ViewModels
{
	public class MenuViewModel: ViewModelBase
	{
        private readonly IApplicationConfig _config;
        public IApplicationConfig Config
        {
            get { return _config; }
        }

        private readonly IApplicationState _state;
        public IApplicationState State
        {
            get { return _state; }
        }

        private readonly IPluginManager _pluginManager;
        public IPluginManager PluginManager
        {
            get { return _pluginManager; }
        }

        private readonly IPresetsManager _presetsManager;
        public IPresetsManager PresetsManager
        {
            get { return _presetsManager; }
        }

        //Todo: Create a manager to detect screens activites
	    public bool SupportDualScreen
	    {
            get { return Screen.AllScreens.Count() >= 2; }
	    }

	    public bool CanOpenFile
	    {
	        get
	        {
                try
                {
                    return _pluginManager.Medias.Any(media => media.Content.OpenFileCommand.CanExecute(null));
                }
                catch
                {
                    return true;
                }
            }
	    }
        
        public bool CanOpenStream
        {
            get
            {
                try
                {
                    return _pluginManager.Medias.Any(media => media.Content.OpenStreamCommand.CanExecute(null));
                }
                catch
                {
                    return true;
                }
            }
        }

        public bool CanOpenDisc
        {
            get
            {
                try
                {
                    return _pluginManager.Medias.Any(media => media.Content.OpenDiscCommand.CanExecute(null));
                }
                catch
                {
                    return true;
                }
            
        }
        }

        public bool CanOpenDevice
        {
            get
            {
                try
                {
                    return _pluginManager.Medias.Any(media => media.Content.OpenDeviceCommand.CanExecute(null));

                }
                catch
                {
                    return true;
                }
            }
        }

        public bool CanOpenProcess
        {
            get
            {
                try
                { 
                return _pluginManager.Medias.Any(media => media.Content.OpenProcessCommand.CanExecute(null));
                }
                catch
                {
                    return true;
                }
            }
        }

	    #region Commands

        private readonly ICommand _openCommand;
        public ICommand OpenCommand
        {
            get { return _openCommand; }
        }

        private readonly ICommand _openFileCommand;
        public ICommand OpenFileCommand
        {
            get { return _openFileCommand; }
        }

        private readonly ICommand _openStreamCommand;
        public ICommand OpenStreamCommand
        {
            get { return _openStreamCommand; }
        }

        private readonly ICommand _openDiscCommand;
        public ICommand OpenDiscCommand
        {
            get { return _openDiscCommand; }
        }

        private readonly ICommand _openDeviceCommand;
        public ICommand OpenDeviceCommand
        {
            get { return _openDeviceCommand; }
        }

        private readonly ICommand _openProcessCommand;
        public ICommand OpenProcessCommand
        {
            get { return _openProcessCommand; }
        }        
        
        private readonly ICommand _browseSamplesCommand;
        public ICommand BrowseSamplesCommand
        {
            get { return _browseSamplesCommand; }
        }

        private readonly ICommand _exitCommand;
        public ICommand ExitCommand
        {
            get { return _exitCommand; }
        }

        private readonly ICommand _saveMediaPresetCommand;
        public ICommand SaveMediaPresetCommand
        {
            get { return _saveMediaPresetCommand; }
        }

        private readonly ICommand _saveDevicePresetCommand;
        public ICommand SaveDevicePresetCommand
        {
            get { return _saveDevicePresetCommand; }
        }

        private readonly ICommand _saveAllPresetCommand;
        public ICommand SaveAllPresetCommand
        {
            get { return _saveAllPresetCommand; }
        }

        private readonly ICommand _loadMediaPresetCommand;
        public ICommand LoadMediaPresetCommand
        {
            get { return _loadMediaPresetCommand; }
        }

        private readonly ICommand _resetPresetCommand;
        public ICommand ResetPresetCommand
        {
            get { return _resetPresetCommand; }
        }

        private readonly ICommand _settingsCommand;
        public ICommand SettingsCommand
        {
            get { return _settingsCommand; }
        }

        private readonly ICommand _launchWebBrowserCommand;
        public ICommand LaunchWebBrowserCommand
        {
            get { return _launchWebBrowserCommand; }
        }

        private readonly ICommand _aboutCommand;
        public ICommand AboutCommand
        {
            get { return _aboutCommand; }
        }

        private readonly ICommand _changeProjectionCommand;
        public ICommand ChangeProjectionCommand
        {
            get { return _changeProjectionCommand; }
        }

        private readonly ICommand _changeEffectCommand;
        public ICommand ChangeEffectCommand
        {
            get { return _changeEffectCommand; }
        }

        private readonly ICommand _changeDistortionCommand;
        public ICommand ChangeDistortionCommand
        {
            get { return _changeDistortionCommand; }
        }

        private readonly ICommand _changeTrackerCommand;
        public ICommand ChangeTrackerCommand
        {
            get { return _changeTrackerCommand; }
        }

        private readonly ICommand _changeFormatCommand;
        public ICommand ChangeFormatCommand
        {
            get { return _changeFormatCommand; }
        }

        private readonly ICommand _changeLayoutCommand;
	    public ICommand ChangeLayoutCommand
        {
            get { return _changeLayoutCommand; }
        }

        #endregion

        public MenuViewModel(IApplicationState state, IPluginManager pluginManager, IApplicationConfig config, IPresetsManager presetManager)
        {
            _pluginManager = pluginManager;
            _state = state;
            _config = config;
            _presetsManager = presetManager;

            //Commands
            _openCommand = new DelegateCommand(Open);
            _openFileCommand = new DelegateCommand(OpenFile);
            _openStreamCommand = new DelegateCommand(OpenStream);
            _openDiscCommand = new DelegateCommand(OpenDisc);
            _openDeviceCommand = new DelegateCommand(OpenDevice);
            _openProcessCommand = new DelegateCommand(OpenProcess);
            _browseSamplesCommand = new DelegateCommand(BrowseSamples);
            _exitCommand = new DelegateCommand(Exit);
            _changeFormatCommand = new DelegateCommand(SetStereoInput);
            _changeProjectionCommand = new DelegateCommand(SetProjection);
            _changeEffectCommand = new DelegateCommand(SetEffect);
            _changeLayoutCommand = new DelegateCommand(SetStereoOutput);
            _changeDistortionCommand = new DelegateCommand(SetDistortion);
            _changeTrackerCommand = new DelegateCommand(SetTracker);
            _saveMediaPresetCommand = new DelegateCommand(SaveMediaPreset);
            _saveDevicePresetCommand = new DelegateCommand(SaveDevicePreset);
            _saveAllPresetCommand = new DelegateCommand(SaveAllPreset);
            _loadMediaPresetCommand = new DelegateCommand(LoadMediaPreset);
            _resetPresetCommand = new DelegateCommand(ResetPreset);
            _settingsCommand = new DelegateCommand(ShowSettings);
            _launchWebBrowserCommand = new DelegateCommand(LaunchWebBrowser);
            _aboutCommand = new DelegateCommand(ShowAbout);
        }

        #region Logic

        private void OpenFile(object o)
        {
            var mediaPlugin = (IPlugin<IMedia>)o;
            if (_state.MediaPlugin == null || _state.MediaPlugin.Content == null) return;
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = FileFilterHelper.GetFilter();
            if (openFileDialog.ShowDialog().Value)
            {
                _state.MediaPlugin = mediaPlugin;
                _state.MediaPlugin.Content.OpenFileCommand.Execute(openFileDialog.FileName);
                LoadSideCarPreset(openFileDialog.FileName);
            }
        }

	    private void OpenStream(object o)
        {
            var mediaPlugin = (IPlugin<IMedia>)o;
            if (_state.MediaPlugin == null || _state.MediaPlugin.Content == null) return;
            var dialog = new StreamInputDialog();
            if (dialog.ShowDialog() == true)
            {
                _state.MediaPlugin = mediaPlugin;
                _state.MediaPlugin.Content.OpenStreamCommand.Execute(dialog.Url);
            }
        }

        private void OpenDevice(object o)
        {
            var mediaPlugin = (IPlugin<IMedia>)o;
            if (_state.MediaPlugin == null || _state.MediaPlugin.Content == null) return;
            //Todo: Call device selection dialog
            _state.MediaPlugin = mediaPlugin;
        }

        private void OpenDisc(object o)
        {
            var mediaPlugin = (IPlugin<IMedia>)o;
            if (_state.MediaPlugin == null || _state.MediaPlugin.Content == null) return;
            var dialog = new DiscInputDialog();
            if (dialog.ShowDialog() == true)
            {
                _state.MediaPlugin = mediaPlugin;
                _state.MediaPlugin.Content.OpenDiscCommand.Execute(dialog.Drive);
            }
        }

        private void OpenProcess(object o)
        {
            var mediaPlugin = (IPlugin<IMedia>)o;
            if (_state.MediaPlugin == null || _state.MediaPlugin.Content == null) return;
            var dialog = new ProcessInputDialog();
            if (dialog.ShowDialog() == true)
            {
                _state.MediaPlugin = mediaPlugin;
                _state.MediaPlugin.Content.OpenProcessCommand.Execute(dialog.Process);
            }
        }
        
	    private void Open(object o)
		{
			var filePath = (string)o;
	        LoadDefaultMedia();
            
            try
            {
                if (_state.MediaPlugin != null && _state.MediaPlugin.Content.OpenFileCommand.CanExecute(filePath))
                {
                    _state.MediaPlugin.Content.OpenFileCommand.Execute(filePath);
                    LoadSideCarPreset(filePath);
                }
            }
            catch (Exception exc)
            {
                var message = String.Format("Unable to load '{0}'.", filePath);
                Logger.Instance.Warn(message, exc);
                MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BrowseSamples(object o)
		{   
            var dirInfo = new DirectoryInfo(_config.SamplesFolder);
            if (Directory.Exists(dirInfo.FullName))
            {
                Process.Start(dirInfo.FullName);
            }
            else
            {
                MessageBox.Show(string.Format("Invalid samples directory: '{0}'.", _config.SamplesFolder), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LaunchWebBrowser(object o)
		{
            Process.Start("http://www.vrplayer.tv");
        }

        private void Exit(object o)
        {
            Application.Current.Shutdown();
        }

        private void SetEffect(object o)
        {
            _state.EffectPlugin = (IPlugin<EffectBase>)o;
        }

        private void SetStereoInput(object o)
        {
            _state.StereoInput = (StereoMode)o;
        }

        private void SetStereoOutput(object o)
        {
            _state.StereoOutput = (LayoutMode)o;
        }

        private void SetProjection(object o)
        {
            _state.ProjectionPlugin = (IPlugin<IProjection>)o;
        }

        private void SetTracker(object o)
        {
            _state.TrackerPlugin = (IPlugin<ITracker>)o;
        }

        private void SetDistortion(object o)
        {
            _state.DistortionPlugin = (IPlugin<DistortionBase>)o;
        }

        private void SaveMediaPreset(object o)
        {
            try
            {
                var filename = o.ToString();
                _presetsManager.SaveMediaToFile(filename);
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while saving media preset file.'", exc);
            }
        }

        private void SaveDevicePreset(object o)
        {
            try
            {
                var filename = o.ToString();
                _presetsManager.SaveDeviceToFile(filename);
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while saving device preset file.'", exc);
            }
        }

        private void SaveAllPreset(object o)
        {
            try
            {
                var filename = o.ToString();
                _presetsManager.SaveAllToFile(filename);
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while saving preset file.'", exc);
            }
        }

        private void LoadMediaPreset(object o)
        {
            try
            {
                var filename = o.ToString();
                _presetsManager.LoadFromUri(filename);
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while loading preset file.'", exc);
            }
        }

        private void ResetPreset(object o)
        {
            try
            {
                _presetsManager.LoadFromUri(Path.GetFullPath("DefaultPreset.json"));
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while resetting preset: Could not load 'DefaultPreset.json'", exc);
            }            
        }

        private void ShowSettings(object o)
        {
            SettingsWindow.ShowSingle();
        }

        private void ShowAbout(object o)
        {
            //Todo: Extract to UI layer
            MessageBox.Show(
                string.Format("VRPlayer ({0})", Assembly.GetExecutingAssembly().GetName().Version) +
                Environment.NewLine +
                Environment.NewLine +
                "(c)Stephane Levesque 2012-2014",    
                "About", 
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        public bool LoadMediaPresetFromMetadata()
        {
            try
            {
                if (!string.IsNullOrEmpty(_state.MediaPlugin.Content.FileName))
                    return _presetsManager.LoadFromMetadata(_state.MediaPlugin.Content.FileName);
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while loading preset file from meta data.'", exc);
            }
            return false;
        }

        #endregion

        #region Helpers

        private void LoadDefaultMedia()
        {
            try
            {
                _state.MediaPlugin = _pluginManager.Medias
                            .Where(plugin => plugin.Content != null)
                            .FirstOrDefault(plugin => plugin.Content.GetType().Namespace == _config.DefaultMedia);
            }
            catch (Exception exc)
            {
                var message = String.Format("Unable to load default media engine '{0}'.", _config.DefaultMedia);
                Logger.Instance.Warn(message, exc);
            }
        }

        private void LoadSideCarPreset(string mediaFile)
        {
            if (!_config.ReadSideCarPresets)
                return;

            try
            {
                var presetFile = Path.GetDirectoryName(mediaFile) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(mediaFile) + ".json";
                if(File.Exists(presetFile))
                    LoadMediaPreset(presetFile);
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while loading side-car preset file.'", exc);
            }
            
        }

        #endregion

	}
}
