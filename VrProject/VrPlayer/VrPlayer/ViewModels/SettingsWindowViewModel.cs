using System.Windows.Forms;
using System.Windows.Input;
using VrPlayer.Helpers.Mvvm;
using VrPlayer.Models.Plugins;
using VrPlayer.Models.State;
using VrPlayer.Models.Config;

namespace VrPlayer.ViewModels
{
    public class SettingsWindowViewModel: ViewModelBase
	{
        private readonly IApplicationState _state;
        public IApplicationState State
        {
            get { return _state; }
        }

        private readonly IApplicationConfig _config;
        public IApplicationConfig Config
        {
            get { return _config; }
        }

        private readonly IPluginManager _pluginManager;
        public IPluginManager PluginManager
        {
            get { return _pluginManager; }
        }

        private readonly ICommand _changeSamplePathCommand;
        public ICommand ChangeSamplePathCommand
        {
            get { return _changeSamplePathCommand; }
        }

        //Todo: This should be removed when stabilizers will behave
        public bool HideStabilizers
        {
            get 
            { 
                return string.IsNullOrEmpty(_config.DefaultStabilizer) ||
                _config.DefaultStabilizer == "VrPlayer.Stabilizers.NoStabilizer"; 
            }
        }

        public SettingsWindowViewModel(IApplicationState state, IApplicationConfig config, IPluginManager pluginManager)
        {
            _state = state;
            _config = config;
            _pluginManager = pluginManager;

            _changeSamplePathCommand = new DelegateCommand(ChangeSamplePath);
        }

        private void ChangeSamplePath(object o)
        {
            var dialog = new FolderBrowserDialog();
            var result = dialog.ShowDialog();
            if(result == DialogResult.OK)
                _config.SamplesFolder = dialog.SelectedPath;
        }
    }
}