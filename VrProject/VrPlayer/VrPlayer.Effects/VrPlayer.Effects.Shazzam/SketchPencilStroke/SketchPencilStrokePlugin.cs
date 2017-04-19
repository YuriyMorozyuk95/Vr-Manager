using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Effects;
using VrPlayer.Helpers;

namespace VrPlayer.Effects.Shazzam.SketchPencilStroke
{
    [Export(typeof(IPlugin<EffectBase>))]
    public class SketchPencilStrokePlugin : PluginBase<EffectBase>
    {
        public SketchPencilStrokePlugin()
        {
            try
            {
                Name = "Shazzam / Sketch Pencil Stroke";
                var effect = new SketchPencilStrokeEffect();
                Content = effect;
                Panel = new SketchPencilStrokePanel(effect);
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(string.Format("Error while loading '{0}'", GetType().FullName), exc);
            }
        }
    }
}
