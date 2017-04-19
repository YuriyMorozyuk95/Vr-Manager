using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Projections;
using VrPlayer.Helpers;

namespace VrPlayer.Projections.Cylinder
{
    [Export(typeof(IPlugin<IProjection>))]
    public class CylinderPlugin : PluginBase<IProjection>
    {
        public CylinderPlugin()
        {
            try
            {
                Name = "Cylinder";
                var projection = new CylinderProjection();
                Content = projection;
                Panel = new CylinderPanel(projection);
                InjectConfig(PluginConfig.FromSettings(ConfigHelper.LoadConfig().AppSettings.Settings));
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(string.Format("Error while loading '{0}'", GetType().FullName), exc);
            }
        }
    }
}