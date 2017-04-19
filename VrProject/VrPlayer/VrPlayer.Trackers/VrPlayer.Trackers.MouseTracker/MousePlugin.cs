using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Trackers;
using VrPlayer.Helpers;

namespace VrPlayer.Trackers.MouseTracker
{
    [Export(typeof(IPlugin<ITracker>))]
    public class OculusRiftPlugin : PluginBase<ITracker>
    {
        public OculusRiftPlugin()
        {
            try
            {
                Name = "Mouse";
                var tracker = new MouseTracker();
                Content = tracker;
                Panel = new MousePanel(tracker);
                InjectConfig(PluginConfig.FromSettings(ConfigHelper.LoadConfig().AppSettings.Settings));
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(string.Format("Error while loading '{0}'", GetType().FullName), exc);
            }
        }
    }
}