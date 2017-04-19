using System;

namespace ComPortPackages.Core.Model
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [Serializable]
    public partial class EffectStreamAetehMetaDescriptionViewingMethod
    {
        [System.Xml.Serialization.XmlAttributeAttribute("size")]
        public string Size { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute("input")]
        public string Input { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("value")]
        public string Value { get; set; }
    }
}
