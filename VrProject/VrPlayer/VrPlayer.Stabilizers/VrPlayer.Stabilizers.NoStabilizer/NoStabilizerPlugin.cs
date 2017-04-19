using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Stabilizers;
using VrPlayer.Helpers;

namespace VrPlayer.Stabilizers.NoStabilizer
{
    [Export(typeof(IPlugin<IStabilizer>))]
    public class NoStabilizerPlugin : PluginBase<IStabilizer>
    {
        public NoStabilizerPlugin()
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