using System;
using System.IO;
using System.Runtime.Serialization;

namespace ComPortPackages.Core.Serialization.Abstract
{
    public interface ISerializationService
    {
        Stream Serialize<TObject>(TObject @object, SerializationType serializationType) where TObject : class;
        TObject Deserialize<TObject>(Stream stream, SerializationType serializationType) where TObject : class;
    }
}
