using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Trackers;
using VrPlayer.Helpers;

namespace VrPlayer.Trackers.KinectTracker
{
    [Export(typeof(IPlugin<ITracker>))]
    public class KinectPlugin : PluginBase<ITracker>
    {
        public KinectPlugin()
        {
            try
            {
                Name = "Microsoft Kinect";
                var tracker = new KinectTracker();
                Content = tracker;
                Panel = new KinectPanel(tracker);
                InjectConfig(PluginConfig.FromSettings(ConfigHelper.LoadConfig().AppSettings.Settings));
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(string.Format("Error while loading '{0}'", GetType().FullName), exc);
            }
        }
    }
}