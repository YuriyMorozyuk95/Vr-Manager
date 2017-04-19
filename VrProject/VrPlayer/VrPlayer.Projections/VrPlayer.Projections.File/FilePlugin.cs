using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Projections;
using VrPlayer.Helpers;

namespace VrPlayer.Projections.File
{
    [Export(typeof(IPlugin<IProjection>))]
    public class FilePlugin : PluginBase<IProjection>
    {
        public FilePlugin()
        {
            try
            {
                Name = "File";
                var projection = new FileProjection();
                Content = projection;
                Panel = new FilePanel(projection);
                InjectConfig(PluginConfig.FromSettings(ConfigHelper.LoadConfig().AppSettings.Settings));
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(string.Format("Error while loading '{0}'", GetType().FullName), exc);
            }
        }
    }
}