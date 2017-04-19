using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Projections;
using VrPlayer.Helpers;

namespace VrPlayer.Projections.FullDome
{
    [Export(typeof(IPlugin<IProjection>))]
    public class FullDomePlugin : PluginBase<IProjection>
    {
        public FullDomePlugin()
        {
            try
            {
                Name = "Full Dome";
                var projection = new FullDomeProjection();
                Content = projection;
                Panel = new FullDomePanel(projection);
                InjectConfig(PluginConfig.FromSettings(ConfigHelper.LoadConfig().AppSettings.Settings));
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(string.Format("Error while loading '{0}'", GetType().FullName), exc);
            }
        }
    }
}