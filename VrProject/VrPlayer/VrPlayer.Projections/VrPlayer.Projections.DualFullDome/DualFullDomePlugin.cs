using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Projections;
using VrPlayer.Helpers;

namespace VrPlayer.Projections.DualFullDome
{
    [Export(typeof(IPlugin<IProjection>))]
    public class DualFullDomePlugin : PluginBase<IProjection>
    {
        public DualFullDomePlugin()
        {
            try
            {
                Name = "Dual full dome";
                var projection = new DualFullDomeProjection();
                Content = projection;
                Panel = new DualFullDomePanel(projection);
                InjectConfig(PluginConfig.FromSettings(ConfigHelper.LoadConfig().AppSettings.Settings));
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(string.Format("Error while loading '{0}'", GetType().FullName), exc);
            }
        }
    }
}