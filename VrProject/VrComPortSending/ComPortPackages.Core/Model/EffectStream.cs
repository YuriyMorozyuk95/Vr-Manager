using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ComPortPackages.Core.Model
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    [Serializable]
    public class EffectStream
    {
        private EffectStreamAetehMeta aetehMetaField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("BytesSample", IsNullable = false)]
        public List<EffectStreamBytesSample> BytesSamples { get; set; }

        /// <remarks/>
        public EffectStreamAetehMeta AetehMeta
        {
            get
            {
                return this.aetehMetaField;
            }
            set
            {
                this.aetehMetaField = value;
            }
        }
    }
}
