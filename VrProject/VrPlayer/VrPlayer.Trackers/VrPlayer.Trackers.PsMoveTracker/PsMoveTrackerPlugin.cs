using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Trackers;
using VrPlayer.Helpers;

namespace VrPlayer.Trackers.PsMoveTracker
{
    [Export(typeof(IPlugin<ITracker>))]
    public class PsMoveTrackerPlugin : PluginBase<ITracker>
    {
        public PsMoveTrackerPlugin()
        {
            try
            {
                Name = "Playstation Move";
                var tracker = new PsMoveTracker();
                Content = tracker;
                Panel = new PsMovePanel(tracker);
                InjectConfig(PluginConfig.FromSettings(ConfigHelper.LoadConfig().AppSettings.Settings));
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(string.Format("Error while loading '{0}'", GetType().FullName), exc);
            }
        }
    }
}