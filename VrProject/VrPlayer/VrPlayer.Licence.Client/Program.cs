using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Vr.License;

namespace VrPlayer.Licence.Client
{
    public class Program
    {
        public   static void Main(string[] args)
        {
            try
            {
                
                LicenseProvider licenseProvider = new LicenseProvider();
                var info = licenseProvider.GetInfoString();
                File.WriteAllText(GetPathLicense(), info);
                Console.WriteLine(string.Format("Файл информации о ПК {0} создан в текущей директории, нажмите Enter для выхода", "Client.info"));
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
            int indexSubString = res.IndexOf("VrPlayer.Licence.Client\\bin\\Debug\\");
            res = res.Remove(indexSubString);
            res = res + "Vr.Licence.Generate\\bin\\Debug\\Client.info";
            return res;
        }
    }
}
