using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrManager.Data.Entity;

namespace VrManager.Data.Concrete
{
    public class DataBaseInitializer : DropCreateDatabaseIfModelChanges<DataBaseContext>
    {
        protected override void Seed(DataBaseContext context)
        {
            base.Seed(context);
            ModelSetting instance = new ModelSetting()
            {
                CurrentAppTheme = AppTheme.BaseDark,
                CurrentAppAccent = AccentTheme.Blue,
                AllTimeOff = new DateTime(2000, 12, 12, 0, 0, 30),
                TimeAdvertising = new DateTime(2000,12,12,0,0,10)
            };
            context.Setting.Add(instance);

            ModelUser user = new ModelUser
            {
                Login = "Admin",
                Password = Hesher.MD5Hash("123")
            };

            context.Users.Add(user);
            context.SaveChanges();
        }
    }
}
