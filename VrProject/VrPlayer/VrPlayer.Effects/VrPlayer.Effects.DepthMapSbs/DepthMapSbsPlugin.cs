using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Effects;
using VrPlayer.Helpers;

namespace VrPlayer.Effects.DepthMapSbs
{
    [Export(typeof(IPlugin<EffectBase>))]
    public class DepthMapSbsPlugin : PluginBase<EffectBase>
    {
        public DepthMapSbsPlugin()
        {
            try
            {
                Name = "Depthmap Side by side";
                var effect = new DepthMapSbsEffect();
                Content = effect;
                Panel = new DepthMapSbsPanel(effect);
                InjectConfig(PluginConfig.FromSettings(ConfigHelper.LoadConfig().AppSettings.Settings));
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(string.Format("Error while loading '{0}'", GetType().FullName), exc);
            }
        }
    }
}