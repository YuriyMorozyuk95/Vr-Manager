using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Effects;
using VrPlayer.Helpers;

namespace VrPlayer.Effects.NoEffect
{
    [Export(typeof(IPlugin<EffectBase>))]
    public class NoEffectPlugin : PluginBase<EffectBase>
    {
        public NoEffectPlugin()
        {
            try
            {
                Name = "None";
                Content = null;
                Panel = null;
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(string.Format("Error while loading '{0}'", GetType().FullName), exc);
            }
        }
    }
}
