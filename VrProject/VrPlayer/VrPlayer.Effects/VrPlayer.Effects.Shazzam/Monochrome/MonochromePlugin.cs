using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Effects;
using VrPlayer.Helpers;

namespace VrPlayer.Effects.Shazzam.Monochrome
{
    [Export(typeof(IPlugin<EffectBase>))]
    public class MonochromePlugin : PluginBase<EffectBase>
    {
        public MonochromePlugin()
        {
            try
            {
                Name = "Shazzam / Monochrome";
                var effect = new MonochromeEffect();
                Content = effect;
                Panel = new MonochromePanel(effect);
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(string.Format("Error while loading '{0}'", GetType().FullName), exc);
            }
        }
    }
}
