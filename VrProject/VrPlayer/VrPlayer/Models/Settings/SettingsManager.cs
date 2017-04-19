using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Projections;
using VrPlayer.Helpers;
using VrPlayer.Models.Config;
using VrPlayer.Models.Plugins;
using VrPlayer.Models.State;

namespace VrPlayer.Models.Settings
{
    public class SettingsManager : ISettingsManager
    {
        private readonly IApplicationState _state;
        private readonly IPluginManager _pluginManager;
        private readonly IApplicationConfig _config;
        private readonly ApplicationSettingsBase _settings;
        
        public SettingsManager(IApplicationState state, IPluginManager pluginManager, IApplicationConfig config, ApplicationSettingsBase settings)
        {
            _settings = settings;
            _state = state;
            _pluginManager = pluginManager;
            _config = config;
        }

        public void Save()
        {
            SavePluginsSettings();
            SaveStateSettings();
            SaveConfigsSettings();
            _settings.Save();
        }

        public void Load()
        {
            LoadPluginsSettings();
            LoadStateSettings();
            LoadConfigsSettings();
        }

        private void SavePluginsSettings()
        {
            try
            {
                //Medias
                var mediasConfigs = new ConfigsList();
                if (_pluginManager.Medias != null)
                {
                    mediasConfigs.AddRange(_pluginManager.Medias.Select(plugin => plugin.ExtractConfig()));
                }
                _settings["Medias"] = SerializationHelper.SerializeJson<ConfigsList>(mediasConfigs);

                //Projections
                var projectionsConfigs = new ConfigsList();
                if (_pluginManager.Projections != null)
                {
                    projectionsConfigs.AddRange(_pluginManager.Projections.Select(plugin => plugin.ExtractConfig()));
                }
                _settings["Projections"] = SerializationHelper.SerializeJson<ConfigsList>(projectionsConfigs);

                //Effects
                var effectsConfigs = new ConfigsList();
                if (_pluginManager.Effects != null)
                {
                    effectsConfigs.AddRange(_pluginManager.Effects.Select(plugin => plugin.ExtractConfig()));
                }
                _settings["Effects"] = SerializationHelper.SerializeJson<ConfigsList>(effectsConfigs);

                //Distortions
                var distortionsConfigs = new ConfigsList();
                if (_pluginManager.Distortions != null)
                {
                    distortionsConfigs.AddRange(_pluginManager.Distortions.Select(plugin => plugin.ExtractConfig()));
                }
                _settings["Distortions"] = SerializationHelper.SerializeJson<ConfigsList>(distortionsConfigs);

                //Trackers
                var trackersConfigs = new ConfigsList();
                if (_pluginManager.Trackers != null)
                {
                    trackersConfigs.AddRange(_pluginManager.Trackers.Select(plugin => plugin.ExtractConfig()));
                }
                _settings["Trackers"] = SerializationHelper.SerializeJson<ConfigsList>(trackersConfigs);

                //Stabilizers
                var stabilizersConfigs = new ConfigsList();
                if (_pluginManager.Stabilizers != null)
                {
                    stabilizersConfigs.AddRange(_pluginManager.Stabilizers.Select(plugin => plugin.ExtractConfig()));
                }
                _settings["Stabilizers"] = SerializationHelper.SerializeJson<ConfigsList>(stabilizersConfigs);
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while saving plugins settings.", exc);
            }
        }

        private void SaveStateSettings()
        {
            try
            {

                //if (_state.MediaPlugin != null)
                //    _settings["CurrentMedia"] = _state.MediaPlugin.GetType().FullName;
                if (_state.ProjectionPlugin != null)
                    _settings["CurrentProjection"] = _state.ProjectionPlugin.GetType().FullName;
                if (_state.EffectPlugin != null)
                    _settings["CurrentEffect"] = _state.EffectPlugin.GetType().FullName;
                if (_state.DistortionPlugin != null)
                    _settings["CurrentDistortion"] = _state.DistortionPlugin.GetType().FullName;
                if (_state.TrackerPlugin != null)
                    _settings["CurrentTracker"] = _state.TrackerPlugin.GetType().FullName;
                if (_state.StabilizerPlugin != null)
                    _settings["CurrentStabilizer"] = _state.StabilizerPlugin.GetType().FullName;

                _settings["StereoMode"] = _state.StereoInput.ToString();
                _settings["LayoutMode"] = _state.StereoOutput.ToString();
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while saving state settings.", exc);
            }
        }

        private void SaveConfigsSettings()
        {
            try
            {
                _settings["Samples"] = _config.SamplesFolder;
                _settings["FieldOfView"] = _config.CameraFieldOfView.ToString(CultureInfo.InvariantCulture);
                _settings["HorizontalOffset"] = _config.ViewportsHorizontalOffset.ToString(CultureInfo.InvariantCulture);
                _settings["VerticalOffset"] = _config.ViewportsVerticalOffset.ToString(CultureInfo.InvariantCulture);
                _settings["NeckHeight"] = _config.NeckHeight.ToString(CultureInfo.InvariantCulture);
                _settings["ReadSideCarPresets"] = _config.NeckHeight.ToString(CultureInfo.InvariantCulture);

                //Todo: Refactor!
                _settings["Shortcuts"] =
                    _config.KeyPlayPause + "," +
                    _config.KeyStop + "," +
                    _config.KeyNext + "," +
                    _config.KeyPrevious + "," +
                    _config.KeyLoop + "," +
                    _config.KeyMoveLeft + "," +
                    _config.KeyMoveRight + "," +
                    _config.KeyMoveForward + "," +
                    _config.KeyMoveBackward + "," +
                    _config.KeyMoveUp + "," +
                    _config.KeyMoveDown + "," +
                    _config.KeyTrackerCalibrate + "," +
                    _config.KeyTrackerReset + "," +
                    _config.KeyFieldOfViewMinus + "," +
                    _config.KeyFieldOfViewPlus + "," +
                    _config.KeyHorizontalOffsetMinus + "," +
                    _config.KeyHorizontalOffsetPlus + "," +
                    _config.KeyVerticalOffsetMinus + "," +
                    _config.KeyVerticalOffsetPlus;
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while saving configs settings.", exc);
            }
        }

        private void LoadPluginsSettings()
        {
            try
            {
                //Medias
                var mediasConfig = SerializationHelper.DeserializeJson<ConfigsList>(_settings["Medias"].ToString());
                foreach (var config in mediasConfig)
                {
                    var media = _pluginManager.Medias.FirstOrDefault(plugin => plugin.GetType().FullName == config.Type);
                    if (media != null)
                        media.InjectConfig(config);
                }

                //Projections
                var projectionsConfig = SerializationHelper.DeserializeJson<ConfigsList>(_settings["Projections"].ToString());
                foreach (var config in projectionsConfig)
                {
                    var projection = _pluginManager.Projections.FirstOrDefault(plugin => plugin.GetType().FullName == config.Type);
                    if (projection != null)
                        projection.InjectConfig(config);
                }

                //Effects
                var effectsConfig = SerializationHelper.DeserializeJson<ConfigsList>(_settings["Effects"].ToString());
                foreach (var config in effectsConfig)
                {
                    var effects = _pluginManager.Effects.FirstOrDefault(plugin => plugin.GetType().FullName == config.Type);
                    if (effects != null)
                        effects.InjectConfig(config);
                }

                //Distortions
                var distortionsConfig = SerializationHelper.DeserializeJson<ConfigsList>(_settings["Distortions"].ToString());
                foreach (var config in distortionsConfig)
                {
                    var distortions = _pluginManager.Distortions.FirstOrDefault(plugin => plugin.GetType().FullName == config.Type);
                    if (distortions != null)
                        distortions.InjectConfig(config);
                }

                //Trackers
                var trackersConfig = SerializationHelper.DeserializeJson<ConfigsList>(_settings["Trackers"].ToString());
                foreach (var config in trackersConfig)
                {
                    var trackers = _pluginManager.Trackers.FirstOrDefault(plugin => plugin.GetType().FullName == config.Type);
                    if (trackers != null)
                        trackers.InjectConfig(config);
                }

                //Stabilizers
                var stabilizersConfig = SerializationHelper.DeserializeJson<ConfigsList>(_settings["Stabilizers"].ToString());
                foreach (var config in stabilizersConfig)
                {
                    var stabilizers = _pluginManager.Stabilizers.FirstOrDefault(plugin => plugin.GetType().FullName == config.Type);
                    if (stabilizers != null)
                        stabilizers.InjectConfig(config);
                }
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while loading plugins settings.", exc);
            }
        }

        private void LoadStateSettings()
        {
            try
            {
                var layoutMode = _settings["LayoutMode"].ToString();
                if (!string.IsNullOrEmpty(layoutMode))
                    _state.StereoOutput = (LayoutMode)Enum.Parse(typeof(LayoutMode), layoutMode);

                var stereoMode = _settings["StereoMode"].ToString();
                if (!string.IsNullOrEmpty(stereoMode))
                    _state.StereoInput = (StereoMode)Enum.Parse(typeof(StereoMode), stereoMode);

                //var currentMedia = _settings["CurrentMedia"].ToString();
                //if(!string.IsNullOrEmpty(currentMedia))
                //    _state.MediaPlugin = _pluginManager.Medias.FirstOrDefault(plugin => plugin.GetType().FullName == currentMedia);

                var currentProjection = _settings["CurrentProjection"].ToString();
                if (!string.IsNullOrEmpty(currentProjection))
                    _state.ProjectionPlugin = _pluginManager.Projections.FirstOrDefault(plugin => plugin.GetType().FullName == currentProjection);

                var currentEffect = _settings["CurrentEffect"].ToString();
                if (!string.IsNullOrEmpty(currentEffect))
                    _state.EffectPlugin = _pluginManager.Effects.FirstOrDefault(plugin => plugin.GetType().FullName == currentEffect);

                var currentDistortion = _settings["CurrentDistortion"].ToString();
                if (!string.IsNullOrEmpty(currentDistortion))
                    _state.DistortionPlugin = _pluginManager.Distortions.FirstOrDefault(plugin => plugin.GetType().FullName == currentDistortion);

                var currentTracker = _settings["CurrentTracker"].ToString();
                if (!string.IsNullOrEmpty(currentTracker))
                    _state.TrackerPlugin = _pluginManager.Trackers.FirstOrDefault(plugin => plugin.GetType().FullName == currentTracker);

                var currentStabilizer = _settings["CurrentStabilizer"].ToString();
                if (!string.IsNullOrEmpty(currentStabilizer))
                    _state.StabilizerPlugin = _pluginManager.Stabilizers.FirstOrDefault(plugin => plugin.GetType().FullName == currentStabilizer);

                //Todo: Refactor!
                var shortcuts = _settings["Shortcuts"].ToString();
                if (!string.IsNullOrEmpty(shortcuts))
                {
                    var i = 0;
                    var keys = shortcuts.Split(',');
                    _config.KeyPlayPause = (Key) Enum.Parse(typeof(Key), keys[i++]);
                    _config.KeyStop = (Key) Enum.Parse(typeof(Key), keys[i++]);
                    _config.KeyNext = (Key) Enum.Parse(typeof(Key), keys[i++]);
                    _config.KeyPrevious = (Key) Enum.Parse(typeof(Key), keys[i++]);
                    _config.KeyLoop = (Key) Enum.Parse(typeof(Key), keys[i++]);
                    _config.KeyMoveLeft = (Key) Enum.Parse(typeof(Key), keys[i++]);
                    _config.KeyMoveRight = (Key) Enum.Parse(typeof(Key), keys[i++]);
                    _config.KeyMoveForward = (Key) Enum.Parse(typeof(Key), keys[i++]);
                    _config.KeyMoveBackward = (Key) Enum.Parse(typeof(Key), keys[i++]);
                    _config.KeyMoveUp = (Key) Enum.Parse(typeof(Key), keys[i++]);
                    _config.KeyMoveDown = (Key) Enum.Parse(typeof(Key), keys[i++]);
                    _config.KeyTrackerCalibrate = (Key) Enum.Parse(typeof(Key), keys[i++]);
                    _config.KeyTrackerReset = (Key) Enum.Parse(typeof(Key), keys[i++]);
                    _config.KeyFieldOfViewMinus = (Key) Enum.Parse(typeof(Key), keys[i++]);
                    _config.KeyFieldOfViewPlus = (Key) Enum.Parse(typeof(Key), keys[i++]);
                    _config.KeyHorizontalOffsetMinus = (Key) Enum.Parse(typeof(Key), keys[i++]);
                    _config.KeyHorizontalOffsetPlus = (Key) Enum.Parse(typeof(Key), keys[i++]);
                    _config.KeyVerticalOffsetMinus = (Key) Enum.Parse(typeof(Key), keys[i++]);
                    _config.KeyVerticalOffsetPlus = (Key)Enum.Parse(typeof(Key), keys[i++]);
                }
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while loading state settings.", exc);
            }
        }

        private void LoadConfigsSettings()
        {
            try
            {
                var samples = _settings["Samples"].ToString();
                if (!string.IsNullOrEmpty(samples))
                    _config.SamplesFolder = samples;

                var fieldOfView = _settings["FieldOfView"].ToString();
                if (!string.IsNullOrEmpty(fieldOfView))
                    _config.CameraFieldOfView = int.Parse(fieldOfView);

                var horizontalOffset = _settings["HorizontalOffset"].ToString();
                if (!string.IsNullOrEmpty(horizontalOffset))
                    _config.ViewportsHorizontalOffset = int.Parse(horizontalOffset);

                var verticalOffset = _settings["VerticalOffset"].ToString();
                if (!string.IsNullOrEmpty(verticalOffset))
                    _config.ViewportsVerticalOffset = int.Parse(verticalOffset);

                var neckHeight = _settings["NeckHeight"].ToString();
                if (!string.IsNullOrEmpty(neckHeight))
                    _config.NeckHeight = ConfigHelper.ParseDouble(neckHeight);

                var readSideCarPresets = _settings["ReadSideCarPresets"].ToString();
                if (!string.IsNullOrEmpty(readSideCarPresets))
                    _config.ReadSideCarPresets = bool.Parse(readSideCarPresets);
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while loading configs settings.", exc);
            }
        }
    }
}