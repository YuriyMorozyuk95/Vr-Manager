using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Distortions;
using VrPlayer.Helpers;

namespace VrPlayer.Distortions.Pincushion
{
    [Export(typeof(IPlugin<DistortionBase>))]
    public class PincushionPlugin : PluginBase<DistortionBase>
    {
        public PincushionPlugin()
        {
            try
            {
                Name = "Pincushion";
                var effect = new PincushionEffect();
                Content = effect;
                Panel = new PincushionPanel(effect);
                InjectConfig(PluginConfig.FromSettings(ConfigHelper.LoadConfig().AppSettings.Settings));
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(string.Format("Error while loading '{0}'", GetType().FullName), exc);
            }
        }
    }
}