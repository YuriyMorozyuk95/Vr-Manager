using System;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Effects;
using VrPlayer.Helpers;

namespace VrPlayer.Effects.Anaglyph
{
    [Export(typeof(IPlugin<EffectBase>))]
    class AnaglyphPlugin : PluginBase<EffectBase>
    {
        public AnaglyphPlugin()
        {
            try
            {
                Name = "Anaglyph";
                var effect = new AnaglyphEffect();
                Content = effect;
                Panel = new AnaglyphPanel(effect);
                InjectConfig(PluginConfig.FromSettings(ConfigHelper.LoadConfig().AppSettings.Settings));
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(string.Format("Error while loading '{0}'", GetType().FullName), exc);
            }
        }
    }
}