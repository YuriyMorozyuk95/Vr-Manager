using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Projections;
using VrPlayer.Helpers;

namespace VrPlayer.Projections.DualDome
{
    [Export(typeof(IPlugin<IProjection>))]
    public class DualDomePlugin : PluginBase<IProjection>
    {
        public DualDomePlugin()
        {
            try
            {
                Name = "Dual dome";
                var projection = new DualDomeProjection();
                Content = projection;
                Panel = new DualDomePanel(projection);
                InjectConfig(PluginConfig.FromSettings(ConfigHelper.LoadConfig().AppSettings.Settings));
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(string.Format("Error while loading '{0}'", GetType().FullName), exc);
            }
        }
    }
}