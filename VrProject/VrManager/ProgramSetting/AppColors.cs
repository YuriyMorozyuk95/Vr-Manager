using MahApps.Metro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using VrManager;
using VrManager.Data.Entity;

namespace VrManager.ProgramSetting
{
    public static class AppColors
    {
        private static SolidColorBrush _colorBackground;
        private static SolidColorBrush _colorForeground;
        private static SolidColorBrush _colorRegular;
        private static AppTheme _currentAppTheme;
        private static AccentTheme _currentAppAccent;
        private static SolidColorBrush _colorHover;
        private static SolidColorBrush _colorPressed;


        public static AppTheme CurrentAppTheme
        {
            get
            {
                return _currentAppTheme;
            }
            set
            {
                _currentAppTheme = value;
                if (value == AppTheme.BaseLight)
                {
                    _colorBackground = new SolidColorBrush(Colors.White);
                    _colorForeground = new SolidColorBrush(Colors.Black);
                    _colorRegular = new SolidColorBrush(Color.FromRgb(230, 230, 230));
                    _colorHover = new SolidColorBrush(Color.FromRgb(179, 179, 179));
                    _colorPressed = new SolidColorBrush(Color.FromRgb(159, 159, 159));
                }
                else
                {
                    _colorBackground = new SolidColorBrush(Color.FromRgb(43, 43, 43));
                    _colorForeground = new SolidColorBrush(Colors.White);
                    _colorRegular = new SolidColorBrush(Color.FromRgb(23, 23, 23));
                    _colorHover = new SolidColorBrush(Color.FromRgb(46, 46, 46));
                    _colorPressed = new SolidColorBrush(Color.FromRgb(68, 68, 68));

                }
                changeTheme(_currentAppTheme);
                changeResourses();
            }
        }
        public static AccentTheme CurrentAppAccent
        {
            get
            {
                return _currentAppAccent;
            }
            set
            {
                _currentAppAccent = value;
                changeAccent(_currentAppAccent);
            }
        }
        public static SolidColorBrush ColorBackground
        {
            get
            {
                return _colorBackground;
            }
        }
        public static SolidColorBrush ColorForeground
        {
            get
            {
                return _colorForeground;
            }
        }
        public static SolidColorBrush ColorRegular
        {
            get
            {
                return _colorRegular;
            }
        }
        public static SolidColorBrush ColorHover
        {
            get
            {
                return _colorHover;
            }
        }
        public static SolidColorBrush ColorPressed
        {
            get
            {
                return _colorPressed;
            }
        }

        private static void changeResourses()
        {
            ResourceDictionary dict = App.Current.Resources;
            dict["ColorBackground"] = ColorBackground;
            dict["ColorForeground"] = ColorForeground;
            dict["ColorRegular"] = ColorRegular;
            dict["ColorHover"] = ColorHover;
            dict["ColorPressed"] = ColorPressed;
        }
        private static void changeAccent(AccentTheme accent)
        {
            ThemeManager.ChangeAppStyle(Application.Current,
                                              ThemeManager
                                                .GetAccent(accent.ToString()),
                                              ThemeManager
                                                .GetAppTheme(AppColors
                                                .CurrentAppTheme.ToString()));
        }
        private static void changeTheme(AppTheme theme)
        {
            ThemeManager.ChangeAppStyle(Application.Current,
                                              ThemeManager
                                                .GetAccent(AppColors
                                                .CurrentAppAccent.ToString()),
                                              ThemeManager
                                                .GetAppTheme(theme.ToString()));
        }
    }
}