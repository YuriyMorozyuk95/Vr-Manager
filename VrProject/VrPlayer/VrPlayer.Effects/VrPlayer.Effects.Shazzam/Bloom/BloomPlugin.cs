using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Effects;
using VrPlayer.Helpers;

namespace VrPlayer.Effects.Shazzam.Bloom
{
    [Export(typeof(IPlugin<EffectBase>))]
    public class BloomPlugin : PluginBase<EffectBase>
    {
        public BloomPlugin()
        {
            try
            {
                Name = "Shazzam / Bloom";
                var effect = new BloomEffect();
                Content = effect;
                Panel = new BloomPanel(effect);
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(string.Format("Error while loading '{0}'", GetType().FullName), exc);
            }
        }
    }
}
