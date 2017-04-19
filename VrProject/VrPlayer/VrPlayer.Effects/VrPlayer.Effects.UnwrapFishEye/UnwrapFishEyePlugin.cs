using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Effects;
using VrPlayer.Helpers;

namespace VrPlayer.Effects.UnwrapFishEye
{
    [Export(typeof(IPlugin<EffectBase>))]
    public class UnwrapFishEyePlugin : PluginBase<EffectBase>
    {
        public UnwrapFishEyePlugin()
        {
            try
            {
                Name = "Unwrap Fisheye";
                var effect = new UnwrapFishEyeEffect();
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
