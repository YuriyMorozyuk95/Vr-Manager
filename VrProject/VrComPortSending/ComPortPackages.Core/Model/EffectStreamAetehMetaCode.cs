using System;

namespace ComPortPackages.Core.Model
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [Serializable]
    public class EffectStreamAetehMetaCode
    {
        [System.Xml.Serialization.XmlAttributeAttribute("base64")]
        public string Base64 { get; set; }
    }
}
