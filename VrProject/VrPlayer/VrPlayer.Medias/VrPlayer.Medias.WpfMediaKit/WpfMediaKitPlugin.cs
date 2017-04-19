using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Medias;
using VrPlayer.Helpers;

namespace VrPlayer.Medias.WpfMediaKit
{
    [Export(typeof(IPlugin<IMedia>))]
    public class WpfMediaKitPlugin : PluginBase<IMedia>
    {
        public WpfMediaKitPlugin()
        {
            try
            {
                Name = "Direct Show";
                var media = new WpfMediaKitMedia();
                Content = media;
                Panel = new WpfMediaKitPanel(media);
                InjectConfig(PluginConfig.FromSettings(ConfigHelper.LoadConfig().AppSettings.Settings));
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(string.Format("Error while loading '{0}'", GetType().FullName), exc);
            }
        }
    }
}
