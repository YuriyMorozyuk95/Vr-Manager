using System;

namespace ComPortPackages.Core.Model
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [Serializable]
    public class EffectStreamAetehMetaDescriptionDescriptionText
    {
        [System.Xml.Serialization.XmlAttributeAttribute("title")]
        public string Title { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute("value")]
        public string Value { get; set; }
    }
}
