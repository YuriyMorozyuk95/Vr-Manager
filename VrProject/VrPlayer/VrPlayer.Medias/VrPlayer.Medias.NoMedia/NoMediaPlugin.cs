using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Medias;
using VrPlayer.Helpers;

namespace VrPlayer.Medias.NoMedia
{
    [Export(typeof(IPlugin<IMedia>))]
    public class NoMediaPlugin : PluginBase<IMedia>
    {
        public NoMediaPlugin()
        {
            try
            {
                Name = "None";
                Content = null;
                Panel = null;
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(string.Format("Error while loading '{0}'", GetType().FullName), exc);
            }
        }
    }
}
