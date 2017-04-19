using System;

namespace ComPortPackages.Core.Model
{
    [Serializable]
    public class Package
    {
        public string ComPort { get; set; }
        public EffectStream Effect { get; set; }
        public string ProccessName { get; set; }
    }
}
