using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Effects;
using VrPlayer.Helpers;

namespace VrPlayer.Effects.Shazzam.InvertColor
{
    [Export(typeof(IPlugin<EffectBase>))]
    public class InvertColorPlugin : PluginBase<EffectBase>
    {
        public InvertColorPlugin()
        {
            try
            {
                Name = "Shazzam / Invert Color";
                var effect = new InvertColorEffect();
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
