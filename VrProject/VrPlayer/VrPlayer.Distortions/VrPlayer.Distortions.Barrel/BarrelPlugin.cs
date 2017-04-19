using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Distortions;
using VrPlayer.Helpers;

namespace VrPlayer.Distortions.Barrel
{
    [Export(typeof(IPlugin<DistortionBase>))]
    public class BarrelPlugin : PluginBase<DistortionBase>
    {
        public BarrelPlugin()
        {
            try
            {
                Name = "Barrel";
                var effect = new BarrelEffect();
                Content = effect;
                Panel = new BarrelPanel(effect);
                InjectConfig(PluginConfig.FromSettings(ConfigHelper.LoadConfig().AppSettings.Settings));
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(string.Format("Error while loading '{0}'", GetType().FullName), exc);
            }
        }
    }
}
