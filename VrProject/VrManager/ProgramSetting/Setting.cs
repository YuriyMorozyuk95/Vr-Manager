using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VrManager.Data.Concrete;
using VrManager.Data.Abstract;
using VrManager.Data.Entity;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VrManager.Helpers;

namespace VrManager.ProgramSetting
{
    public class Setting
    {
        public Setting()
        {
            _rep = App.Repository;
        }

        public int NumberRightMonitor = 1;
        public int NumberLeftMonitor = 0;

        private TimeSpan _allTimeOff;
        private EntityRepository _rep;
        private bool _isBackgroundImage;
        private bool _firstExecute = false;
        private LicenseProvider license = new LicenseProvider();

        public bool IsBackgroundImage
        {
            get
            {
                return _isBackgroundImage;
            }
            set
            {
                _isBackgroundImage = value;
            }

        }
        public AppTheme CurrentAppTheme
        {
            get
            {
                return AppColors.CurrentAppTheme;
            }
            set
            {
                AppColors.CurrentAppTheme = value;
            }
            
        }
        public AccentTheme CurrentAppAccent
        {
            get
            {
                return AppColors.CurrentAppAccent;
            }
            set
            {
                AppColors.CurrentAppAccent = value;
            }
        }
        public string PathToFolderFiles { get; set; }
        public TimeSpan AllTimeOff
        {
            get
            {
                return _allTimeOff;
            }
            set
            {
                _allTimeOff = value;
                if (_firstExecute)
                {
                    foreach (ModelGame game in _rep.Games)
                    {
                        game.TimeOut = new DateTime(2000, 12, 12, _allTimeOff.Hours, _allTimeOff.Minutes, _allTimeOff.Seconds);
                    }
                    foreach (ModelVideo video in _rep.Videos)
                    {
                        video.TimeOut = new DateTime(2000, 12, 12, _allTimeOff.Hours, _allTimeOff.Minutes, _allTimeOff.Seconds);
                    }
                }
                else
                {
                    _firstExecute = true;
                }
                _rep.SaveChanges();
            }
        }

        public bool IsTransperentTile { get; internal set; }
        public bool IsKioskMode { get; internal set; }
        public string PathToLicense
        {
            get
            {
                return license.FullPathLicense;
            }
            set
            {
                license.FullPathLicense = value;
                App.ІsLicensed = license.CheckLicense();
            }

        }

        private TimeSpan _timeAdvertising;
        public TimeSpan TimeAdvertising
        {
            get
            {
                return _timeAdvertising;
            }
            internal set
            {
                _timeAdvertising = value;
                ObserverUserActivity.TimeForCheck = value;
            }
        }

        public void Import()
        {
            ModelSetting importingSetting = _rep.Setting.SingleOrDefault();
            CurrentAppTheme = importingSetting.CurrentAppTheme;
            CurrentAppAccent = importingSetting.CurrentAppAccent;
            PathToFolderFiles = importingSetting.PathToFolderFiles;
            AllTimeOff = importingSetting.AllTimeOff.Value.TimeOfDay;
            IsBackgroundImage = importingSetting.IsBackgroundImage;
            IsTransperentTile = importingSetting.IsTransperentTile;
            IsKioskMode = importingSetting.IsKioskMode;
            PathToLicense = importingSetting.PathToLicense;
            NumberLeftMonitor = importingSetting.NumberLeftMonitor;
            NumberRightMonitor = importingSetting.NumberRightMonitor;
            TimeAdvertising = importingSetting.TimeAdvertising.Value.TimeOfDay;

            if (IsBackgroundImage)
            {
                App.MainWnd.Background = PersonalizationHelper.BitmapFromPath(App.PathToBackgroundImage);
            }

            PersonalizationHelper.SetStyleTile(IsTransperentTile);


        }
        public void Export()
        {
            ModelSetting importingSetting;
            _rep = App.Repository;
            try
            {
                importingSetting = _rep.Setting.SingleOrDefault();
            }
            catch
            {
                _rep.FirstExecute();
                importingSetting = _rep.Setting.SingleOrDefault();
            }
            importingSetting.CurrentAppTheme = CurrentAppTheme;
            importingSetting.CurrentAppAccent = CurrentAppAccent;
            importingSetting.PathToFolderFiles = PathToFolderFiles;
            importingSetting.IsBackgroundImage = IsBackgroundImage;
            importingSetting.IsTransperentTile = IsTransperentTile;
            importingSetting.IsKioskMode = IsKioskMode;
            importingSetting.PathToLicense = PathToLicense;
            importingSetting.AllTimeOff = new DateTime(2000, 12, 12, AllTimeOff.Hours, AllTimeOff.Minutes, AllTimeOff.Seconds);
            importingSetting.NumberLeftMonitor = NumberLeftMonitor;
            importingSetting.NumberRightMonitor = NumberRightMonitor;
            importingSetting.TimeAdvertising = new DateTime(2000, 12, 12, TimeAdvertising.Hours, TimeAdvertising.Minutes, TimeAdvertising.Seconds);
            _rep.SaveChanges();

        }
        public bool CheckLicense()
        {
           return license.CheckLicense();
        }
       
    }
}
