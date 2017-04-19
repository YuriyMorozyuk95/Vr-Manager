using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Projections;
using VrPlayer.Helpers;

namespace VrPlayer.Projections.Cube
{
    [Export(typeof(IPlugin<IProjection>))]
    public class CubePlugin : PluginBase<IProjection>
    {
        public CubePlugin()
        {
            try
            {
                Name = "Cube";
                var projection = new CubeProjection();
                Content = projection;
                Panel = null;
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(string.Format("Error while loading '{0}'", GetType().FullName), exc);
            }
        }
    }
}
