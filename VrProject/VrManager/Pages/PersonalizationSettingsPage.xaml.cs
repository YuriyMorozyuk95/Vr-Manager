
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VrManager.Helpers;
using VrManager.ProgramSetting;
using MahApps.Metro.Controls;
namespace VrManager.Pages
{
    /// <summary>
    /// Interaction logic for PersonalizationSettingsPage.xaml
    /// </summary>
    public partial class PersonalizationSettingsPage : Page
    {
        public PersonalizationSettingsPage()
        {
            InitializeComponent();
        }

        private void RedTheme_Click(object sender, RoutedEventArgs e)
        {
            App.Setting.CurrentAppAccent = AccentTheme.Red;
            SaveSetting();
        }

        private void SaveSetting()
        {
           // throw new NotImplementedException();
        }

        private void GreenTheme_Click(object sender, RoutedEventArgs e)
        {
            App.Setting.CurrentAppAccent = AccentTheme.Green;
            SaveSetting();
        }
        private void BlueTheme_Click(object sender, RoutedEventArgs e)
        {
            App.Setting.CurrentAppAccent = AccentTheme.Blue;
            SaveSetting();
        }
        private void PurpleTheme_Click(object sender, RoutedEventArgs e)
        {
            App.Setting.CurrentAppAccent = AccentTheme.Purple;
            SaveSetting();
        }
        private void OrangeTheme_Click(object sender, RoutedEventArgs e)
        {
            App.Setting.CurrentAppAccent = AccentTheme.Orange;
            SaveSetting();
        }
        private void LimeTheme_Click(object sender, RoutedEventArgs e)
        {
            App.Setting.CurrentAppAccent = AccentTheme.Lime;
            SaveSetting();
        }
        private void TealTheme_Click(object sender, RoutedEventArgs e)
        {
            App.Setting.CurrentAppAccent = AccentTheme.Teal;
            SaveSetting();
        }
        private void CyanTheme_Click(object sender, RoutedEventArgs e)
        {
            App.Setting.CurrentAppAccent = AccentTheme.Cyan;
            SaveSetting();
        }
        private void IndigoTheme_Click(object sender, RoutedEventArgs e)
        {
            App.Setting.CurrentAppAccent = AccentTheme.Indigo;
            SaveSetting();
        }
        private void VioletTheme_Click(object sender, RoutedEventArgs e)
        {
            App.Setting.CurrentAppAccent = AccentTheme.Violet;
            SaveSetting();
        }
        private void PinkTheme_Click(object sender, RoutedEventArgs e)
        {
            App.Setting.CurrentAppAccent = AccentTheme.Pink;
            SaveSetting();
        }
        private void MagentaTheme_Click(object sender, RoutedEventArgs e)
        {
            App.Setting.CurrentAppAccent = AccentTheme.Magenta;
            SaveSetting();
        }
        private void CrimsonTheme_Click(object sender, RoutedEventArgs e)
        {
            App.Setting.CurrentAppAccent = AccentTheme.Crimson;
            SaveSetting();
        }
        private void YellowTheme_Click(object sender, RoutedEventArgs e)
        {
            App.Setting.CurrentAppAccent = AccentTheme.Yellow;
            SaveSetting();
        }
        private void BrownTheme_Click(object sender, RoutedEventArgs e)
        {
            App.Setting.CurrentAppAccent = AccentTheme.Brown;
            SaveSetting();
        }
        private void OliveTheme_Click(object sender, RoutedEventArgs e)
        {
            App.Setting.CurrentAppAccent = AccentTheme.Olive;
            SaveSetting();
        }
        private void SiennaTheme_Click(object sender, RoutedEventArgs e)
        {
            App.Setting.CurrentAppAccent = AccentTheme.Sienna;
            SaveSetting();
        }
        private void ToggleSwitch_Checked(object sender, RoutedEventArgs e)
        {
            App.Setting.CurrentAppTheme = AppTheme.BaseDark;
            SaveSetting();
        }
        private void ToggleSwitch_Unchecked(object sender, RoutedEventArgs e)
        {
            App.Setting.CurrentAppTheme = AppTheme.BaseLight;
            SaveSetting();
        }

        private void Btn_OpenFileImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Выберете фоновые изображение";
            dialog.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                        "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                        "Portable Network Graphic (*.png)|*.png";
            if (dialog.ShowDialog() == true)
            {
                TB_OpenFileImage.Text = dialog.FileName;
            }
            
        }


        private void Btn_SaveChanges_Click(object sender, RoutedEventArgs e)
        {

            if ((bool)BackgroundImageToggle.IsChecked)
            {
                if (TB_OpenFileImage.Text != string.Empty)
                {
                    ChangeBackgroundToNewImage(TB_OpenFileImage.Text);
                }
                else if(File.Exists(App.PathToBackgroundImage))
                {
                    ChangeBackgroundToOldImage();
                }
                else
                {
                    MessageBox.Show("Выберете изображение");
                }
            }
            else
            {
                App.Setting.IsBackgroundImage = false;
                App.MainWnd.Background = App.Current.Resources["ColorBackground"] as SolidColorBrush;
            }

            App.Setting.IsTransperentTile = (bool)ThemeTile.IsChecked;

            PersonalizationHelper.SetStyleTile(App.Setting.IsTransperentTile);

            App.Setting.Export();
            App.Frame.Navigate(new StartUpPage());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            App.MainWnd.ChangeTitle(Title);
            if (App.Setting.CurrentAppTheme == AppTheme.BaseDark)
            {
                ThemeToggle.IsChecked = true;
            }
            else
            {
                ThemeToggle.IsChecked = false;
            }

            BackgroundImageToggle.IsChecked = App.Setting.IsBackgroundImage;
            ThemeTile.IsChecked = App.Setting.IsTransperentTile;
        }
        public void ChangeBackgroundToNewImage(string pathToImage)
        {
            DirectoryInfo dir = new DirectoryInfo(pathToImage);
            if (dir.Parent.FullName == App.Setting.PathToFolderFiles + @"\Config\Background" && new FileInfo(pathToImage).Name == "Background.png")
            {
                ChangeBackgroundToOldImage();
                return;
            }

            if (File.Exists(App.PathToBackgroundImage))
            {
                File.Delete(App.PathToBackgroundImage);
            }

            var file = new FileInfo(pathToImage);        
            file.CopyTo(App.PathToBackgroundImage);
            ChangeBackgroundToOldImage();
        }

        public void ChangeBackgroundToOldImage()
        {
            
            App.MainWnd.Background =  PersonalizationHelper.BitmapFromPath(App.PathToBackgroundImage);
            App.Setting.IsBackgroundImage = true;
        }

      
        public static Style CreateStyleTile()
        {
            Style style = new Style
            {
                TargetType = typeof(Tile)
            };
            style.Setters.Add(new Setter(Tile.ForegroundProperty, new DynamicResourceExtension("ColorForeground")));
            style.Setters.Add(new Setter(Tile.BorderBrushProperty, new DynamicResourceExtension("AccentColorBrush")));
            style.Setters.Add(new Setter(Tile.BorderThicknessProperty, new Thickness(2)));
            style.Setters.Add(new Setter(Tile.MarginProperty, new Thickness(15)));
            style.Setters.Add(new Setter(Tile.HeightProperty,(double)200));
            style.Setters.Add(new Setter(Tile.WidthProperty, (double)200));
            return style;
        }
    }
}
