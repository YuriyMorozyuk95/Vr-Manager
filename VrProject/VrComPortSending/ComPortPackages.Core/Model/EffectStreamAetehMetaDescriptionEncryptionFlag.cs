
using System;

namespace ComPortPackages.Core.Model
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [Serializable]
    public class EffectStreamAetehMetaDescriptionEncryptionFlag
    {
        [System.Xml.Serialization.XmlAttributeAttribute("value")]
        public bool Value { get; set; }
    }
}
