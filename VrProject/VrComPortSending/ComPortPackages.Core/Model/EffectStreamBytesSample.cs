using System;

namespace ComPortPackages.Core.Model
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [Serializable]
    public class EffectStreamBytesSample
    {
        [System.Xml.Serialization.XmlAttributeAttribute(AttributeName = "sampleTime",  DataType = "time")]
        public System.DateTime SampleTime { get; set; }

        [System.Xml.Serialization.XmlAttributeAttribute("data")]
        public string Data { get; set; }
    }
}
