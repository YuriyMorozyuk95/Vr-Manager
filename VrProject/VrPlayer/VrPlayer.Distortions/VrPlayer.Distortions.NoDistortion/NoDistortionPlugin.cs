using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Distortions;
using VrPlayer.Helpers;

namespace VrPlayer.Distortions.NoDistortion
{
    [Export(typeof(IPlugin<DistortionBase>))]
    public class NoDistortionPlugin : PluginBase<DistortionBase>
    {
        public NoDistortionPlugin()
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