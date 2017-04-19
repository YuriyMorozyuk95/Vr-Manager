using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VrManager.Helpers
{
    class CreateDictonaryHelper
    {
        public void CreateFolders(string path)
        {
            string config = path + @"\Config";
            string games = path + @"\Games";
            string moution = path + @"\Moution";
            string video = path + @"\Video";
            string configBackground = config + @"\Background";
            string configDataBase = config + @"\DataBase";
            string configIcon = config + @"\Icon";
            string configSettingPlayer = config + @"\SettingPlayer";
            string configImageIcon = configIcon + @"\ImageIcon";
            string configVidoIcon = configIcon + @"\VidoIcon";
            string moutionGames = moution + @"\Games";
            string moutionVideo5D = moution + @"\Video5D";
            string moutionVideo360 = moution + @"\Video360";
            string video5D = video + @"\Video5D";
            string video360 = video + @"\Video360";
            string bannerVideo = video + @"\BannerVideo";
            string advertisingVideo = video + @"\AdvertisingVideo";

            CreateIfNotExist(config);
            CreateIfNotExist(games);
            CreateIfNotExist(moution);
            CreateIfNotExist(video);
            CreateIfNotExist(configBackground);
            CreateIfNotExist(configDataBase);
            CreateIfNotExist(configIcon);
            CreateIfNotExist(configSettingPlayer);
            CreateIfNotExist(configImageIcon);
            CreateIfNotExist(configVidoIcon);
            CreateIfNotExist(moutionGames);
            CreateIfNotExist(moutionVideo5D);
            CreateIfNotExist(moutionVideo360);
            CreateIfNotExist(video5D);
            CreateIfNotExist(video360);
            CreateIfNotExist(bannerVideo);
            CreateIfNotExist(advertisingVideo);
        }

        private void CreateIfNotExist(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
