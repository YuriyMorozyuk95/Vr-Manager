using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VrManager.Pages;

namespace VrManager.Helpers
{
    public class PersonalizationHelper
    {
        public static ImageBrush BitmapFromPath(string pathToSource)
        {
            MemoryStream ms = new MemoryStream();
            FileStream stream = new FileStream(pathToSource, FileMode.Open, FileAccess.Read);
            ms.SetLength(stream.Length);
            stream.Read(ms.GetBuffer(), 0, (int)stream.Length);
            ms.Flush();
            stream.Close();
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.StreamSource = ms;
            src.EndInit();
            return new ImageBrush(src);
        }

        public static void SetStyleTile(bool IBackgroundTransperent)
        {
            if (App.Setting.IsTransperentTile)
            {
                Style style = PersonalizationSettingsPage.CreateStyleTile();
                style.Setters.Add(new Setter(Tile.BackgroundProperty, Brushes.Transparent));
                Application.Current.Resources["TileStyle"] = style;
            }
            else
            {
                Style style = PersonalizationSettingsPage.CreateStyleTile();
                style.Setters.Add(new Setter(Tile.BackgroundProperty, new DynamicResourceExtension("ColorBackground")));
                Application.Current.Resources["TileStyle"] = style;
            }
        }
    }
}
