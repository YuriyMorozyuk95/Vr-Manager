using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Trackers;
using VrPlayer.Helpers;

namespace VrPlayer.Trackers.TrackIrTracker
{
    [Export(typeof(IPlugin<ITracker>))]
    public class TrackIrPlugin : PluginBase<ITracker>
    {   
        public TrackIrPlugin()
        {
            try
            {
                Name = "NaturalPoint TrackIR";
                var tracker = new TrackIrTracker();
                Content = tracker;
                Panel = new TrackIrPanel(tracker);
                InjectConfig(PluginConfig.FromSettings(ConfigHelper.LoadConfig().AppSettings.Settings));
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(string.Format("Error while loading '{0}'", GetType().FullName), exc);
            }
        }
    }
}