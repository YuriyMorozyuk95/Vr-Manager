using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Effects;
using VrPlayer.Helpers;

namespace VrPlayer.Effects.Shazzam.ParametricEdgeDetection
{
    [Export(typeof(IPlugin<EffectBase>))]
    public class ParametricEdgeDetectionPlugin : PluginBase<EffectBase>
    {
        public ParametricEdgeDetectionPlugin()
        {
            try
            {
                Name = "Shazzam / Parametric Edge Detection";
                var effect = new ParametricEdgeDetectionEffect();
                Content = effect;
                Panel = new ParametricEdgeDetectionPanel(effect);
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(string.Format("Error while loading '{0}'", GetType().FullName), exc);
            }
        }
    }
}
