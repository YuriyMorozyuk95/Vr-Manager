using System;

namespace ComPortPackages.Core.Model
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [Serializable]
    public partial class EffectStreamAetehMeta
    {
        public EffectStreamAetehMetaDescription Description { get; set; }

        public EffectStreamAetehMetaCode Code { get; set; }
    }
}
