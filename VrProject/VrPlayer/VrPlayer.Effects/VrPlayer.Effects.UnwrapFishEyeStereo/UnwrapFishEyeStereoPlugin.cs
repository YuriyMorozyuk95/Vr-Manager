using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Effects;
using VrPlayer.Helpers;

namespace VrPlayer.Effects.UnwrapFishEyeStereo
{
    [Export(typeof(IPlugin<EffectBase>))]
    public class UnwrapFishEyeStereoPlugin : PluginBase<EffectBase>
    {
        public UnwrapFishEyeStereoPlugin()
        {
            try
            {
                Name = "Unwrap Fisheye Stereo";
                var effect = new UnwrapFishEyeStereoEffect();
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
