using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Medias;

namespace VrPlayer.Medias.Gdi
{
    [Export(typeof (IPlugin<IMedia>))]
    public class GdiPlugin : PluginBase<IMedia>
    {
        public GdiPlugin()
        {
            Name = "GDI";
            var media = new GdiMedia();
            Content = media;
            Panel = new GdiPanel(media);
        }
    }
}
