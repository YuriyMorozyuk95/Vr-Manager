using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Projections;
using VrPlayer.Helpers;

namespace VrPlayer.Projections.Sphere
{
    [Export(typeof(IPlugin<IProjection>))]
    public class SpherePlugin : PluginBase<IProjection>
    {
        public SpherePlugin()
        {
            try
            {
                Name = "Sphere";
                var projection = new SphereProjection();
                Content = projection;
                Panel = new SpherePanel(projection);
                InjectConfig(PluginConfig.FromSettings(ConfigHelper.LoadConfig().AppSettings.Settings));
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(string.Format("Error while loading '{0}'", GetType().FullName), exc);
            }
        }
    }
}