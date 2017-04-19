using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Trackers;
using VrPlayer.Helpers;

namespace VrPlayer.Trackers.OculusRiftTracker
{
    [Export(typeof(IPlugin<ITracker>))]
    public class OculusRiftPlugin : PluginBase<ITracker>
    {
        public OculusRiftPlugin()
        {
            try
            {
                Name = "Oculus Rift";
                Content = new OculusRiftTracker();
                Panel = null;
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(string.Format("Error while loading '{0}'", GetType().FullName), exc);
            }
        }
    }
}
