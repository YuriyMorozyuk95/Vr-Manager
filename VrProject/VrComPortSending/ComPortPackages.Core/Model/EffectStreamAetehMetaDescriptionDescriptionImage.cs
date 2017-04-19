using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComPortPackages.Core.Model
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [Serializable]
    public class EffectStreamAetehMetaDescriptionDescriptionImage
    {
        [System.Xml.Serialization.XmlAttributeAttribute("base64")]
        public string Base64 { get; set; }
    }
}
