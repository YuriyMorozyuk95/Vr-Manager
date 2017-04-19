using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Vr.License;

namespace Vr.Licence.Generate
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                
                if (File.Exists("Client.info") == false)
                {
                    throw new VrLicenseException("Не найден файл информации о ПК клиента!");
                }
               
                LicenseProvider licenseProvider = new LicenseProvider();


                var licese = licenseProvider.GetLicenseValueByFile("Client.info");
                File.WriteAllText(GetPathLicense(), licese);
                Console.WriteLine(string.Format("Файл лицензии {0} создан в текущей директории, нажмите Enter для выхода", "Vr.lic"));
            }
            catch (Exception le)
            {
                Console.WriteLine(le);
            }
            
            Console.ReadLine();
        }
        public static string GetPathLicense()
        {
            string res = AppDomain.CurrentDomain.BaseDirectory;
            int indexSubString = res.IndexOf("Vr.Licence.Generate\\bin\\Debug\\");
            res = res.Remove(indexSubString);
            res = res + "VrPlayer\\bin\\Debug\\Vr.lic";
            return res;
        }
    }
}
