using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using Newtonsoft.Json.Linq;
using SE.Halligang.CsXmpToolkit;
using SE.Halligang.CsXmpToolkit.Schemas;
using VrPlayer.Contracts.Projections;
using VrPlayer.Helpers;
using VrPlayer.Models.Config;
using VrPlayer.Models.Plugins;
using VrPlayer.Models.State;

namespace VrPlayer.Models.Presets
{
    public class PresetsManager : IPresetsManager
    {
        public const string XmpNamespace = "http://ns.vrplayer.tv/";
        public const string XmpField = "Preset";

        private readonly IApplicationState _state;
        private readonly IPluginManager _pluginManager;
        private readonly IApplicationConfig _config;

        public PresetsManager(IApplicationConfig config, IApplicationState state, IPluginManager pluginManager)
        {
            _config = config;
            _state = state;
            _pluginManager = pluginManager;
        }

        public void SaveMediaToFile(string fileName)
        {
            var o = new JObject();
            o.Add("Media", GenerateMediaPresets());
            File.WriteAllText(fileName, o.ToString());
        }

        public void SaveDeviceToFile(string fileName)
        {
            var o = new JObject();
            o.Add("Device", GenerateDevicePresets());
            File.WriteAllText(fileName, o.ToString());
        }

        public void SaveAllToFile(string fileName)
        {
            var o = new JObject();
            o.Add("Media", GenerateMediaPresets());
            o.Add("Device", GenerateDevicePresets());
            File.WriteAllText(fileName, o.ToString());
        }

        private JObject GenerateMediaPresets()
        {
            var o = new JObject();
            try
            {
                //Media
                /*
                if (_state.MediaPlugin != null)
                {
                    var mediasConfigs = new JObject();
                    if (_pluginManager.Medias != null)
                    {
                        foreach (var plugin in _pluginManager.Medias
                            .Where(p => p.Content != null)
                            .Where(p => p == _state.MediaPlugin))
                        {
                            mediasConfigs.Add("Type", plugin.Content.GetType().FullName);
                            mediasConfigs.Add("Params", JObject.FromObject(plugin.Content));
                        }
                    }
                    if (mediasConfigs.HasValues)
                        o.Add("Media", mediasConfigs);
                }
                */
                //Projection
                if (_state.ProjectionPlugin != null)
                {
                    var projectionConfigs = new JObject();
                    if (_pluginManager.Projections != null)
                    {
                        foreach (var plugin in _pluginManager.Projections
                                                             .Where(p => p.Content != null)
                                                             .Where(p => p == _state.ProjectionPlugin))
                        {
                            projectionConfigs.Add("Type", plugin.Content.GetType().FullName);
                            projectionConfigs.Add("Params", JObject.FromObject(plugin.Content));
                        }
                    }
                    if (projectionConfigs.HasValues)
                        o.Add("Projection", projectionConfigs);
                }
                //Effect
                if (_state.EffectPlugin != null)
                {
                    var effectConfigs = new JObject();
                    if (_pluginManager.Effects != null)
                    {
                        foreach (var plugin in _pluginManager.Effects
                                                             .Where(p => p.Content != null)
                                                             .Where(p => p == _state.EffectPlugin))
                        {
                            effectConfigs.Add("Type", plugin.Content.GetType().FullName);
                            effectConfigs.Add("Params", JObject.FromObject(plugin.Content));
                        }
                    }
                    if (effectConfigs.HasValues)
                        o.Add("Effect", effectConfigs);
                }
                //Stabilizer
                if (_state.StabilizerPlugin != null)
                {
                    var stabilizerConfigs = new JObject();
                    if (_pluginManager.Stabilizers != null)
                    {
                        foreach (var plugin in _pluginManager.Stabilizers
                                                             .Where(p => p.Content != null)
                                                             .Where(p => p == _state.StabilizerPlugin))
                        {
                            stabilizerConfigs.Add("Type", plugin.Content.GetType().FullName);
                            stabilizerConfigs.Add("Params", JObject.FromObject(plugin.Content));
                        }
                    }
                    if (stabilizerConfigs.HasValues)
                        o.Add("Stabilizer", stabilizerConfigs);
                }
                //Stereo mode
                o.Add("StereoMode", new JValue(_state.StereoInput.ToString()));
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while saving media preset.", exc);
            }
            return o;
        }

        private JObject GenerateDevicePresets()
        {
            var o = new JObject();
            try
            {
                //Distortion
                if (_state.DistortionPlugin != null)
                {
                    var distortionConfigs = new JObject();
                    if (_pluginManager.Distortions != null)
                    {
                        foreach (var plugin in _pluginManager.Distortions
                                                             .Where(p => p.Content != null)
                                                             .Where(p => p == _state.DistortionPlugin))
                        {
                            distortionConfigs.Add("Type", plugin.Content.GetType().FullName);
                            distortionConfigs.Add("Params", JObject.FromObject(plugin.Content));
                        }
                    }
                    if (distortionConfigs.HasValues)
                        o.Add("Distortion", distortionConfigs);
                }
                //Tracker
                if (_state.TrackerPlugin != null)
                {
                    var trackerConfigs = new JObject();
                    if (_pluginManager.Trackers != null)
                    {
                        foreach (var plugin in _pluginManager.Trackers
                                                             .Where(p => p.Content != null)
                                                             .Where(p => p == _state.TrackerPlugin))
                        {
                            trackerConfigs.Add("Type", plugin.Content.GetType().FullName);
                            trackerConfigs.Add("Params", JObject.FromObject(plugin.Content));
                        }
                    }
                    if (trackerConfigs.HasValues)
                        o.Add("Tracker", trackerConfigs);
                }
                //Stereo mode
                o.Add("DisplayMode", new JValue(_state.StereoOutput.ToString()));
                //Fov
                o.Add("FieldOfView", new JValue(_config.CameraFieldOfView));
                //Viewport Offsets
                o.Add("ViewportsHorizontalOffset", new JValue(_config.ViewportsHorizontalOffset));
                o.Add("ViewportsVerticalOffset", new JValue(_config.ViewportsVerticalOffset));
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while saving device preset.", exc);
            }
            return o;
        }

        public void LoadFromUri(string path)
        {
            if (string.IsNullOrEmpty(path))
                return;

            try
            {
                var uri = new Uri(path);

                if (uri.IsFile)
                {
                    if (string.IsNullOrEmpty(uri.LocalPath) || !File.Exists(uri.LocalPath))
                        return;

                    using (var reader = File.OpenText(uri.LocalPath))
                    {
                        Load(reader.ReadToEnd());
                    }
                }
                else
                {
                    var json = new WebClient().DownloadString(uri.AbsoluteUri);
                    Load(json);
                }
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while loading preset from URI", exc);
            }
        }

        public bool LoadFromMetadata(string fileName)
        {
            try
            {
                using (var xmp = Xmp.FromFile(fileName, XmpFileMode.ReadOnly))
                {
                    string propValue;
                    PropertyFlags flags;
                    xmp.XmpCore.GetProperty(XmpNamespace, XmpField, out propValue, out flags);
                    if (!string.IsNullOrEmpty(propValue))
                    {
                        Load(propValue);
                        return true;
                    }
                }
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while parsing XMP metadata.", exc);
            }
            return false;
        }
        
        public void Load(string json)
        {
            if (string.IsNullOrEmpty(json)) 
                return;

            try
            {
                var o = JObject.Parse(json);
                LoadMediaSettings((JObject)o["Media"]);
                LoadDeviceSettings((JObject)o["Device"]);
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while loading json data.", exc);
            }
        }

        private void LoadMediaSettings(JObject o)
        {
            try
            {
                //Stereo mode
                if (o["StereoMode"] != null)
                {
                    var stereoMode = o["StereoMode"].ToString();
                    if (!string.IsNullOrEmpty(stereoMode))
                        _state.StereoInput = (StereoMode)Enum.Parse(typeof(StereoMode), stereoMode);
                }
                
                //Medias
                /*
                var media = (JObject)o["Media"];
                if (media["Type"] != null)
                {
                    var plugin = _pluginManager.Medias.FirstOrDefault(p => p.Content != null && p.Content.GetType().FullName == media["Type"].ToString());
                    if (plugin != null)
                    {
                        _state.MediaPlugin = plugin;
                        var token = media["Params"];
                        if (token is JProperty)
                        {
                            var prop = token as JProperty;
                            Inject(plugin.Content, prop.Value);
                        }
                    }
                }
                */
                //Projection
                var projection = (JObject)o["Projection"];
                if (projection["Type"] != null)
                {
                    var plugin = _pluginManager.Projections.FirstOrDefault(p => p.Content != null && p.Content.GetType().FullName == projection["Type"].ToString());
                    if (plugin != null)
                    {
                        _state.ProjectionPlugin = plugin;
                        var token = projection["Params"];
                        if (token is JProperty)
                        {
                            var prop = token as JProperty;
                            Inject(plugin.Content, prop.Value);
                        }
                    }
                }
                //Effect
                var effect = (JObject)o["Effect"];
                if (effect != null && effect["Type"] != null)
                {
                    var plugin = _pluginManager.Effects.FirstOrDefault(p => p.Content != null && p.Content.GetType().FullName == effect["Type"].ToString());
                    if (plugin != null)
                    {
                        _state.EffectPlugin = plugin;
                        var token = effect["Params"];
                        if (token is JProperty)
                        {
                            var prop = token as JProperty;
                            Inject(plugin.Content, prop.Value);
                        }
                    }
                }
                //Stabilizer
                var stabilizer = (JObject)o["Stabilizer"];
                if (stabilizer!= null && stabilizer["Type"] != null)
                {
                    var plugin = _pluginManager.Stabilizers.FirstOrDefault(p => p.Content != null && p.Content.GetType().FullName == stabilizer["Type"].ToString());
                    if (plugin != null)
                    {
                        _state.StabilizerPlugin = plugin;
                        var token = stabilizer["Params"];
                        if (token is JProperty)
                        {
                            var prop = token as JProperty;
                            Inject(plugin.Content, prop.Value);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while loading media preset.", exc);
            }
        }

        private void LoadDeviceSettings(JObject o)
        {
            try
            {
                //Stereo mode
                if (o["DisplayMode"] != null)
                {
                    var displayMode = o["DisplayMode"].ToString();
                    if (!string.IsNullOrEmpty(displayMode))
                        _state.StereoOutput = (LayoutMode)Enum.Parse(typeof(LayoutMode), displayMode);
                }
                //Fov
                if (o["FieldOfView"] != null)
                {
                    var fov = o["FieldOfView"].ToObject<int>();
                    _config.CameraFieldOfView = fov;
                }
                //Viewport Offsets
                if (o["ViewportsHorizontalOffset"] != null)
                {
                    var hOffset = o["ViewportsHorizontalOffset"].ToObject<int>();
                    _config.ViewportsHorizontalOffset = hOffset;
                }
                if (o["ViewportsVerticalOffset"] != null)
                {
                    var vOffset = o["ViewportsVerticalOffset"].ToObject<int>();
                    _config.ViewportsVerticalOffset = vOffset;
                }
                //Distortion
                var distortion = (JObject)o["Distortion"];
                if (distortion["Type"] != null)
                {
                    var plugin = _pluginManager.Distortions.FirstOrDefault(p => p.Content != null && p.Content.GetType().FullName == distortion["Type"].ToString());
                    if (plugin != null)
                    {
                        _state.DistortionPlugin = plugin;
                        var token = distortion["Params"];
                        if (token is JProperty)
                        {
                            var prop = token as JProperty;
                            Inject(plugin.Content, prop.Value);
                        }
                    }
                }
                //Tracker
                var tracker = (JObject)o["Tracker"];
                if (tracker["Type"] != null)
                {
                    var plugin = _pluginManager.Trackers.FirstOrDefault(p => p.Content != null && p.Content.GetType().FullName == tracker["Type"].ToString());
                    if (plugin != null)
                    {
                        _state.TrackerPlugin = plugin;
                        var token = tracker["Params"];
                        if (token is JProperty)
                        {
                            var prop = token as JProperty;
                            Inject(plugin.Content, prop.Value);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while loading device preset.", exc);
            }
        }

        #region Helpers

        private static void Inject(object target, JToken properties)
        {
            if (target == null || properties == null) return;

            foreach (var token in properties.Children())
            {
                var property = token as JProperty;
                if (property == null) continue;

                var propertyInfo = target.GetType().GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo == null || !propertyInfo.CanWrite) continue;

                try
                {
                    var value = property.ToObject(propertyInfo.PropertyType);
                    propertyInfo.SetValue(target, value, null);
                }
                catch (Exception exc)
                {
                }
            }
        }

        #endregion
    }
}