using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Effects;
using VrPlayer.Helpers;

namespace VrPlayer.Effects.Shazzam.Pixelate
{
    [Export(typeof(IPlugin<EffectBase>))]
    public class PixelatePlugin : PluginBase<EffectBase>
    {
        public PixelatePlugin()
        {
            try
            {
                Name = "Shazzam / Pixelate";
                var effect = new PixelateEffect();
                Content = effect;
                Panel = new PixelatePanel(effect);
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(string.Format("Error while loading '{0}'", GetType().FullName), exc);
            }
        }
    }
}
