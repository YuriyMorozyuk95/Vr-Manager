using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Medias;
using VrPlayer.Helpers;

namespace VrPlayer.Medias.VlcDotNet
{
    [Export(typeof(IPlugin<IMedia>))]
    public class VlcDotNetPlugin : PluginBase<IMedia>
    {
        public VlcDotNetPlugin()
        {
            try
            {
                Name = "VLC";
                var media = new VlcDotNetMedia();
                Content = media;
                Panel = new VlcDotNetPanel(media);
                InjectConfig(PluginConfig.FromSettings(ConfigHelper.LoadConfig().AppSettings.Settings));
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(string.Format("Error while loading '{0}'", GetType().FullName), exc);
            }
        }
    }
}