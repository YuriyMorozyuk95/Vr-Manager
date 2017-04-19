using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Projections;
using VrPlayer.Helpers;

namespace VrPlayer.Projections.Dome
{
    [Export(typeof(IPlugin<IProjection>))]
    public class DomePlugin : PluginBase<IProjection>
    {
        public DomePlugin()
        {
            try
            {
                Name = "Dome";
                var projection = new DomeProjection();
                Content = projection;
                Panel = new DomePanel(projection);
                InjectConfig(PluginConfig.FromSettings(ConfigHelper.LoadConfig().AppSettings.Settings));
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(string.Format("Error while loading '{0}'", GetType().FullName), exc);
            }
        }
    }
}