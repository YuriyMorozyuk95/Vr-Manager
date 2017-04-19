using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrManager.Data.Abstract;
using VrManager.Data.Entity;

namespace VrManager.Data.Concrete
{
    public class EntityRepository : IRepository,IDisposable
    {
        DataBaseContext context;
        
        public EntityRepository()
        {
            context = new DataBaseContext();
        }

        public IEnumerable<ModelGame> Games
        {
            get
            {
                return context.Games;
            }
        }
        public IEnumerable<ModelVideo> Videos
        {
            get
            {
                return context.Videos;
            }
        }
        public IEnumerable<ModelSetting> Setting
        {
            get
            {
                return context.Setting;
            }
        }
        public IEnumerable<ModelUser> Users
        {
            get
            {
                return context.Users;
            }
        }

        public IEnumerable<ModelObserve> Observes
        {
            get
            {
                return context.Observers.Include("User");
            }
        }

        public void AddObserver(ModelObserve observer)
        {
            if (observer.Duration.TimeOfDay >= TimeSpan.FromSeconds(30))
            {
                context.Entry(observer).State = System.Data.Entity.EntityState.Added;
                context.SaveChanges();
            }
        }

        public event EventHandler RefreshTableVideo360;
        public event EventHandler RefreshTableVideo5D;
        public event EventHandler RefreshTableGames;

        public void AddGame(ModelGame game)
        {
            context.Games.Add(game);
            context.SaveChanges();

            if (RefreshTableGames != null)
            {
                RefreshTableGames.Invoke(this, null);
            }
        }
        public void AddVideo(ModelVideo video)
        {
            context.Videos.Add(video);
            context.SaveChanges();

            if(video.TypeItem == TypeItem.Video360)
            {
                if (RefreshTableVideo360 != null)
                {
                    RefreshTableVideo360.Invoke(this, null);
                }
            }
            if(video.TypeItem == TypeItem.Video5D)
            {
                if (RefreshTableVideo5D != null)
                {
                    RefreshTableVideo5D.Invoke(this, null);
                }
            }
        }
        public void DeleteGame(ModelGame game)
        {
            if(game != null)
            {
                context.Games.Remove(game);
                context.SaveChanges();

                if (RefreshTableGames != null)
                {
                    RefreshTableGames.Invoke(this, null);
                }
                return;
            }
            throw new Exception("Game not found");
        }
        public void DeleteVideo(ModelVideo video)
        {
            if (video != null)
            {
                context.Videos.Remove(video);
                context.SaveChanges();

                if (video.TypeItem == TypeItem.Video360)
                {
                    if (RefreshTableVideo360 != null)
                    {
                        RefreshTableVideo360.Invoke(this, null);
                    }
                }
                if (video.TypeItem == TypeItem.Video5D)
                {
                    if (RefreshTableVideo5D != null)
                    {
                        RefreshTableVideo5D.Invoke(this, null);
                    }
                }
                return;
            }
            throw new Exception("Виедо не выбрано");
        }
        public void ChangeGame(ModelGame oldGame,ModelGame newGame)
        {
            if (oldGame != null && newGame!=null)
            {
                oldGame.Name = newGame.Name;
                oldGame.IconType = newGame.IconType;
                oldGame.PathIcon = newGame.PathIcon;
                oldGame.ItemPath = newGame.ItemPath;
                oldGame.StartUpParams = newGame.StartUpParams;
                oldGame.MouseClickCordX = newGame.MouseClickCordX;
                oldGame.MouseClickCordY = newGame.MouseClickCordY;
                oldGame.NameProcess = newGame.NameProcess;
                oldGame.FileMotion = newGame.FileMotion;
                oldGame.RunKey = newGame.RunKey;
                oldGame.AdditionalKey = newGame.AdditionalKey;
                oldGame.TimeOut = newGame.TimeOut;
                oldGame.StartTime = newGame.StartTime;
                oldGame.TypeStartFocus = newGame.TypeStartFocus;
                oldGame.ShiftPressTime = newGame.ShiftPressTime;
                oldGame.PathToBannerVideo = newGame.PathToBannerVideo;
                context.SaveChanges();

                if (RefreshTableGames != null)
                {
                    RefreshTableGames.Invoke(this, null);
                }
                return;
            }
            throw new Exception("Game not found");
        }
        public void ChangeVideo(ModelVideo oldVideo,ModelVideo newVideo)
        {
            if (oldVideo != null && newVideo!=null)
            {
                oldVideo.Name = newVideo.Name;
                oldVideo.IconType = newVideo.IconType;
                oldVideo.PathIcon = newVideo.PathIcon;
                oldVideo.ItemPath = newVideo.ItemPath;
                oldVideo.TypeItem = newVideo.TypeItem;
                oldVideo.MonitorNumber = newVideo.MonitorNumber;
                oldVideo.VrSettingPath = newVideo.VrSettingPath;
                oldVideo.FileMotion = newVideo.FileMotion;
                oldVideo.TimeOut = newVideo.TimeOut;
                oldVideo.PathToBannerVideo = newVideo.PathToBannerVideo;
                context.SaveChanges();

                if (oldVideo.TypeItem == TypeItem.Video360)
                {
                    if (RefreshTableVideo360 != null)
                    {
                        RefreshTableVideo360.Invoke(this, null);
                    }
                }
                if (oldVideo.TypeItem == TypeItem.Video5D)
                {
                    if (RefreshTableVideo5D != null)
                    {
                        RefreshTableVideo5D.Invoke(this, null);
                    }
                }
                return;
            }
            throw new Exception("Video not found");
        }
        public ModelGame FindGame(int gameId)
        {
            return context.Games.Find(gameId);
        }
        public ModelVideo FindVideo(int videoId)
        {
            return context.Videos.Find(videoId);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
        public void FirstExecute()
        {
           
            ModelSetting instance = new ModelSetting();
            context.Setting.Add(instance);
            context.SaveChanges();
        }

        public void Dispose()
        {
            try
            {
                context.Dispose();
            }
            catch
            {

            }
        }

        public void ChangePathToBd(string newPath)
        {
            string pathToBd = "pathToBd.txt";

            if (File.Exists(pathToBd))
            {
                File.Delete(pathToBd);
            }

            using (FileStream fs = File.Create(pathToBd))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes(newPath);
                fs.Write(info, 0, info.Length);
            }

            context.Database.Connection.ConnectionString = $"Data Source={newPath}";
            context = new DataBaseContext();
            context.SaveChanges();
        }
        public void ChangePassword(string newPassword)
        {
            ModelUser user = context.Users.FirstOrDefault();
            user.Password = newPassword;
            context.SaveChanges();
        }
    }
}
