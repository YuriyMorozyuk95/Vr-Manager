using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrManager.Data.Entity;

namespace VrManager.Data.Concrete
{
    public class DataBaseContext: DbContext
    {
        public DataBaseContext() : base("VrManagerDataBase")
        {
            string pathToBd = "pathToBd.txt";
            if (File.Exists(pathToBd))
            {
                using (StreamReader sr = File.OpenText(pathToBd))
                {
                    string path = "";
                    path = sr.ReadLine();
                    Database.Connection.ConnectionString = $"Data Source={path}";
                }
            }
            
            Database.SetInitializer<DataBaseContext>(new DataBaseInitializer());
        }



        public DbSet<ModelGame> Games { get; set; }
        public DbSet<ModelVideo> Videos { get; set; }
        public DbSet<ModelSetting> Setting { get; set; }
        public DbSet<ModelUser> Users { get; set; }

        public DbSet<ModelObserve> Observers { get; set; }

    }
}
