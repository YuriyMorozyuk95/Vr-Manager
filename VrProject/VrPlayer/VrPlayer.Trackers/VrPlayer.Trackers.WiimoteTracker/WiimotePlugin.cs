using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Trackers;
using VrPlayer.Helpers;

namespace VrPlayer.Trackers.WiimoteTracker
{
    [Export(typeof(IPlugin<ITracker>))]
    public class WiimotePlugin : PluginBase<ITracker>
    {
        public WiimotePlugin()
        {
            try
            {
                Name = "Nintendo Wiimote";
                var tracker = new WiimoteTracker();
                Content = tracker;
                Panel = null;
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(string.Format("Error while loading '{0}'", GetType().FullName), exc);
            }
        }
    }
}