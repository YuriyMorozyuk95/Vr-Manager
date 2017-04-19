using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Stabilizers;
using VrPlayer.Helpers;

namespace VrPlayer.Stabilizers.Csv
{
    [Export(typeof(IPlugin<IStabilizer>))]
    public class CsvPlugin : PluginBase<IStabilizer>
    {
        public CsvPlugin()
        {
            try
            {
                Name = "CSV";
                var stabilizer = new CsvStabilizer();
                Content = stabilizer;
                Panel = new CsvPanel(stabilizer);
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(string.Format("Error while loading '{0}'", GetType().FullName), exc);
            }
        }
    }
}
