using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VrManager
{
    [Serializable]
    public class OptionData
    {
        public  string Email { get; set; }
       
    }
   public static class Serializer
    {
        static XmlSerializer  formatter = new XmlSerializer(typeof(OptionData));

        public static void Serilize (OptionData serializeObject)
        { // получаем поток, куда будем записывать сериализованный объект
            using (FileStream fs = new FileStream("OptionData.xml", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, serializeObject);
            }
        }

        public static OptionData Desirialize()
        {

             using (FileStream fs = new FileStream("OptionData.xml", FileMode.OpenOrCreate))
            {
                OptionData email = (OptionData)formatter.Deserialize(fs);
                return email;
            }
        }
    }
}
