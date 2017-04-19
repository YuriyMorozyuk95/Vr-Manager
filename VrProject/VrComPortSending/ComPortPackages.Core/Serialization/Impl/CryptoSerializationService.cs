using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using ComPortPackages.Core.Serialization.Abstract;

namespace ComPortPackages.Core.Serialization.Impl
{
    public class CryptoSerializationService : ISerializationService
    {
        private  byte[] _key = new byte[] {164, 197,195,123,93, 64, 10, 92};
        private byte[] _iv = new byte[] {16, 129, 86, 55,93, 64, 10, 92};

        //private readonly Rfc2898DeriveBytes rfcBytes = new Rfc2898DeriveBytes("Rfc2898DeriveBytes", 8);

        public CryptoSerializationService()
        {
            //_key = rfcBytes.GetBytes(8);
            //_iv = rfcBytes.GetBytes(8);
        }

        public Stream Serialize<TObject>(TObject @object, SerializationType serializationType) where TObject : class
        {
            switch (serializationType)
            {
                //case SerializationType.Xml:
                //    return XmlSerialize(@object);
                case SerializationType.Binary:
                    return BinarySerialize(@object);
                default:
                    throw new ArgumentOutOfRangeException(nameof(serializationType), serializationType, null);
            }
        }

        private Stream BinarySerialize<TObject>(TObject @object) where TObject : class
        {
            byte[] buffer;

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Key = _key;
            des.IV = _iv;

            ICryptoTransform desEncrypt = des.CreateEncryptor();
          
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, desEncrypt, CryptoStreamMode.Write))
                {
                    var serializer = new BinaryFormatter();
                    serializer.Serialize(csEncrypt, @object);
                    csEncrypt.FlushFinalBlock();
                    buffer = msEncrypt.ToArray();
                }
            }

            return new MemoryStream(buffer);
        }

        public TObject Deserialize<TObject>(Stream stream, SerializationType serializationType) where TObject : class
        {
            switch (serializationType)
            {
                case SerializationType.Xml:
                    return XmlDeserialize<TObject>(stream);
                case SerializationType.Binary:
                    return BinaryDeserialize<TObject>(stream);
                default:
                    throw new ArgumentOutOfRangeException(nameof(serializationType), serializationType, null);
            }
        }

        private TObject XmlDeserialize<TObject>(Stream stream) where TObject : class
        {
            var serializer = new XmlSerializer(typeof(TObject));
            TObject result;

            stream.Position = 0;

            var xml = string.Empty;

            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                xml = reader.ReadToEnd();
            }
            using (var reader = XmlReader.Create(new StringReader(xml)))
            {
                result = (TObject)serializer.Deserialize(reader);
            }
            return result;
        }

        private TObject BinaryDeserialize<TObject>(Stream stream) where TObject : class
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            des.Key = _key;
            des.IV = _iv;

            stream.Position = 0;

            ICryptoTransform decryptor = des.CreateDecryptor();

            using (CryptoStream csDecrypt = new CryptoStream(stream, decryptor, CryptoStreamMode.Read))
            {
                var serializer = new BinaryFormatter();
                var result = (TObject)serializer.Deserialize(csDecrypt);
                return result;
            }

        }
    }
}
