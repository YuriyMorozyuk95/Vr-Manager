using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Stabilizers;
using VrPlayer.Helpers;

namespace VrPlayer.Stabilizers.Deshaker
{
    [Export(typeof(IPlugin<IStabilizer>))]
    public class DeshakerPlugin : PluginBase<IStabilizer>
    {
        public DeshakerPlugin()
        {
            try
            {
                Name = "Deshaker";
                var stabilizer = new DeshakerStabilizer();
                Content = stabilizer;
                Panel = new DeshakerPanel(stabilizer);
                InjectConfig(PluginConfig.FromSettings(ConfigHelper.LoadConfig().AppSettings.Settings));
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(string.Format("Error while loading '{0}'", GetType().FullName), exc);
            }
        }
    }
}