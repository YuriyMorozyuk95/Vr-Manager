using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Effects;
using VrPlayer.Helpers;

namespace VrPlayer.Effects.Shazzam.ToonShader
{
    [Export(typeof(IPlugin<EffectBase>))]
    public class ToonShaderPlugin : PluginBase<EffectBase>
    {
        public ToonShaderPlugin()
        {
            try
            {
                Name = "Shazzam / Toon Shader";
                var effect = new ToonShaderEffect();
                Content = effect;
                Panel = new ToonShaderPanel(effect);
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(string.Format("Error while loading '{0}'", GetType().FullName), exc);
            }
        }
    }
}
