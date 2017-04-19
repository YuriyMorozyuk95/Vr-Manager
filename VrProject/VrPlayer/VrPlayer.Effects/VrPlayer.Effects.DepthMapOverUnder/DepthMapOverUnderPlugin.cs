using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Effects;
using VrPlayer.Helpers;

namespace VrPlayer.Effects.DepthMapOverUnder
{
    [Export(typeof(IPlugin<EffectBase>))]
    public class DepthMapOverUnderPlugin : PluginBase<EffectBase>
    {
        public DepthMapOverUnderPlugin()
        {
            try
            {
                Name = "Depthmap Over/Under";
                var effect = new DepthMapOverUnderEffect();
                Content = effect;
                Panel = new DepthMapOverUnderPanel(effect);
                InjectConfig(PluginConfig.FromSettings(ConfigHelper.LoadConfig().AppSettings.Settings));
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(string.Format("Error while loading '{0}'", GetType().FullName), exc);
            }
        }
    }
}
