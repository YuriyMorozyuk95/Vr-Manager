using System;
using System.Xml.Serialization;

namespace ComPortPackages.Core.Model
{
    [XmlType(AnonymousType = true)]
    [Serializable]
    public class EffectStreamAetehMetaDescription
    {
        public EffectStreamAetehMetaDescriptionDescriptionText DescriptionText { get; set; }
        public EffectStreamAetehMetaDescriptionViewingMethod ViewingMethod { get; set; }
        public EffectStreamAetehMetaDescriptionDescriptionImage DescriptionImage { get; set; }
        public EffectStreamAetehMetaDescriptionLicenseExpDate LicenseExpDate { get; set; }
        public EffectStreamAetehMetaDescriptionEncryptionFlag EncryptionFlag { get; set; }
        public EffectStreamAetehMetaDescriptionCustomerID CustomerID { get; set; }
    }
}