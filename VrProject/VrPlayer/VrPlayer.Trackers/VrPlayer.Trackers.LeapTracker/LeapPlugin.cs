using System;
using System.ComponentModel.Composition;
using System.Configuration;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Trackers;
using VrPlayer.Helpers;

namespace VrPlayer.Trackers.LeapTracker
{
    [Export(typeof(IPlugin<ITracker>))]
    public class LeapPlugin : PluginBase<ITracker>
    {
        private static readonly Configuration Config = ConfigHelper.LoadConfig();

        public LeapPlugin()
        {
            try
            {
                Name = "Leap";
                var tracker = new LeapTracker()
                {
                    PositionScaleFactor = ConfigHelper.ParseDouble(Config.AppSettings.Settings["PositionScaleFactor"].Value),
                    RotationFactor = ConfigHelper.ParseDouble(Config.AppSettings.Settings["RotationFactor"].Value)
                };
                Content = tracker;
                Panel = new LeapPanel(tracker);
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(string.Format("Error while loading '{0}'", GetType().FullName), exc);
            }
        }
    }
}
