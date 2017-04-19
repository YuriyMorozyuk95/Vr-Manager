using System.ComponentModel.Composition;
using System.Configuration;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Trackers;
using VrPlayer.Helpers;

namespace VrPlayer.Trackers.RazerHydraTracker
{
    [Export(typeof(IPlugin<ITracker>))]
    public class RazerHydraPlugin : PluginBase<ITracker>
    {
        private static readonly Configuration Config = ConfigHelper.LoadConfig();

        public RazerHydraPlugin()
        {
            Name = "Razer Hydra";
            var tracker = new RazerHydraTracker
                {
                    PositionScaleFactor = ConfigHelper.ParseDouble(Config.AppSettings.Settings["PositionScaleFactor"].Value),
                    RotationOffset = QuaternionHelper.EulerAnglesInDegToQuaternion(ConfigHelper.ParseVector3D(Config.AppSettings.Settings["RotationOffset"].Value)),
                    FilterEnabled = bool.Parse(Config.AppSettings.Settings["FilterEnabled"].Value),
                };
            Content = tracker;
            Panel = new RazerHydraPanel(tracker);
        }
    }
}