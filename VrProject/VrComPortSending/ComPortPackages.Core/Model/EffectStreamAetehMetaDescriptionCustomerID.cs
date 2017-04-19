using System;

namespace ComPortPackages.Core.Model
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [Serializable]
    public class EffectStreamAetehMetaDescriptionCustomerID
    {
        [System.Xml.Serialization.XmlAttributeAttribute("value")]
        public int Value { get; set; }
    }
}
