using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Projections;
using VrPlayer.Helpers;

namespace VrPlayer.Projections.Plane
{
    [Export(typeof(IPlugin<IProjection>))]
    public class PlanePlugin : PluginBase<IProjection>
    {
        public PlanePlugin()
        {
            try
            {
                Name = "Plane";
                var projection = new PlaneProjection();
                Content = projection;
                Panel = new PlanePanel(projection);
                InjectConfig(PluginConfig.FromSettings(ConfigHelper.LoadConfig().AppSettings.Settings));
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(string.Format("Error while loading '{0}'", GetType().FullName), exc);
            }
        }
    }
}
