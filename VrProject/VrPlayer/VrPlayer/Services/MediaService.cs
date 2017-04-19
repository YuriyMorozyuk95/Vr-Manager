using System;
using System.IO;
using System.Linq;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Medias;
using VrPlayer.Helpers;
using VrPlayer.Models.Plugins;
using VrPlayer.Models.Presets;
using VrPlayer.Models.State;

namespace VrPlayer.Services
{
    internal class MediaService : IMediaService
    {
        private readonly IApplicationState _state;
        private readonly IPluginManager _pluginManager;
        private readonly IPresetsManager _presetsManager;
        public StartUpConfig StartUpConfig { get; set; }

        public MediaService(IApplicationState state, IPluginManager pluginManager, IPresetsManager presetsManager)
        {
            _state = state;
            _pluginManager = pluginManager;
            _presetsManager = presetsManager;
          
        }

        public void Load(string source)
        {
            try
            {
                var uri = new Uri(source);
                if (uri.IsFile)
                {
                    loadFile(uri.LocalPath);
                    _presetsManager.LoadFromUri(Path.GetDirectoryName(uri.LocalPath) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(uri.LocalPath) + ".json");
                }
                else
                {
                    loadStream(uri);
                }
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(string.Format("Error while loading media '{0}'", source), exc);
            }
        }

        private void loadFile(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
                return;
            
            IMedia media = null;
            if (_state.MediaPlugin != null && _state.MediaPlugin.Content != null && _state.MediaPlugin.Content.OpenFileCommand.CanExecute(null))
            {
                media = _state.MediaPlugin.Content;
                media.PauseOnLoaded = StartUpConfig.PauseOnStart;
            }
            else
            {
                foreach (var mediaPlugin in _pluginManager.Medias
                    .Where(mediaPlugin => mediaPlugin != _state.MediaPlugin)
                    .Where(mediaPlugin => mediaPlugin.Content != null && mediaPlugin.Content.OpenFileCommand.CanExecute(null)))
                {
                    media = mediaPlugin.Content;
                }
            }
            
            if(media != null)
                media.OpenFileCommand.Execute(path);
        }

        private void loadStream(Uri uri)
        {
            IPlugin<IMedia> mediaPlugin = null;
            if (_state.MediaPlugin != null && _state.MediaPlugin.Content != null && _state.MediaPlugin.Content.OpenStreamCommand.CanExecute(uri))
            {
                mediaPlugin = _state.MediaPlugin;
            }
            else
            {
                foreach (var plugin in _pluginManager.Medias
                    .Where(p => p != _state.MediaPlugin)
                    .Where(p => p.Content != null && p.Content.OpenStreamCommand.CanExecute(uri)))
                {
                    mediaPlugin = plugin;
                }
            }
            if (mediaPlugin != null)
            {
                _state.MediaPlugin = mediaPlugin;
                mediaPlugin.Content.OpenStreamCommand.Execute(uri);
            }
        }
    }
}