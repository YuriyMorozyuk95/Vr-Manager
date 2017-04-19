using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Effects;
using VrPlayer.Helpers;

namespace VrPlayer.Effects.Shazzam.OldMovie
{
    [Export(typeof(IPlugin<EffectBase>))]
    public class OldMoviePlugin : PluginBase<EffectBase>
    {
        public OldMoviePlugin()
        {
            try
            {
                Name = "Shazzam / Old Movie";
                var effect = new OldMovieEffect();
                Content = effect;
                Panel = null;
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(string.Format("Error while loading '{0}'", GetType().FullName), exc);
            }
        }
    }
}
