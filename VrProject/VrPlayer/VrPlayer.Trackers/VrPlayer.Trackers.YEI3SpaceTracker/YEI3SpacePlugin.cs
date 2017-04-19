using System.ComponentModel.Composition;
using System.Configuration;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Trackers;
using VrPlayer.Helpers;

namespace VrPlayer.Trackers.YEI3SpaceTracker
{
    [Export(typeof(IPlugin<ITracker>))]
    public class YEI3SpacePlugin : PluginBase<ITracker>
    {
        private static readonly Configuration Config = ConfigHelper.LoadConfig();

        public YEI3SpacePlugin()
        {
            Name = "YEI 3-Space";
            var tracker = new YEI3SpaceTracker()
                {
                    PositionScaleFactor = ConfigHelper.ParseDouble(Config.AppSettings.Settings["PositionScaleFactor"].Value),
                    RotationOffset = QuaternionHelper.EulerAnglesInDegToQuaternion(ConfigHelper.ParseVector3D(Config.AppSettings.Settings["RotationOffset"].Value)),
                };
            Content = tracker;
            Panel = new YEI3SpacePanel(tracker);
        }
    }
}
