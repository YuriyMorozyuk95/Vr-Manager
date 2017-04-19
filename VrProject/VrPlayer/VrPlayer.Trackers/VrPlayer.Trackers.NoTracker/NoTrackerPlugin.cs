using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Trackers;
using VrPlayer.Helpers;

namespace VrPlayer.Trackers.NoTracker
{
    [Export(typeof(IPlugin<ITracker>))]
    public class NoTrackerPlugin: PluginBase<ITracker>
    {
        public NoTrackerPlugin()
        {
            try
            {
                Name = "None";
                Content = null;
                Panel = null;
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(string.Format("Error while loading '{0}'", GetType().FullName), exc);
            }
        }
    }
}
