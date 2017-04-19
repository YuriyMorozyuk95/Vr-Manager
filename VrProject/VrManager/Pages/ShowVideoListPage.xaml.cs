using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using VrManager.Controls;
using VrManager.Data.Abstract;
using VrManager.Data.Entity;
using VrManager.Helpers;

namespace VrManager.Pages
{
    /// <summary>
    /// Interaction logic for ShowVideoPage.xaml
    /// </summary>
    public partial class ShowVideoListPage : Page
    {
        public ShowVideoListPage(IEnumerable<BaseContentEntity> items,TypeItem typeVideo)
        {
            InitializeComponent();

            if(typeVideo == TypeItem.Video360)
            {
                Title = "Видео 360";
            }
            if(typeVideo == TypeItem.Video5D)
            {
                Title = "Видео 5D";
            }
            if (typeVideo == TypeItem.Game)
            {
                Title = "Игры";
            }

            App.MainWnd.ChangeTitle(Title);
            VideoViewer.SetSourse(items);
            Paggination.SetViewer(VideoViewer);
            Paggination.GenerateListPagginationButtons();
            Paggination.CurrentPage = 1;

        }

        private void VideoViewer_TileClick(object sender, EventArgs e)
        {
            BaseContentEntity content;
            VideoControlerPage controller;
            VideoViewer.FreeResourses();

            if (sender is ModelVideo)
            {
                content = sender as ModelVideo;
                controller = new VideoControlerPage(content as ModelVideo);       
            }
            else
            {
                content = sender as ModelGame;
                controller = new VideoControlerPage(content as ModelGame);
            }
            App.Frame.Navigate(controller);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            VideoViewer.FreeResourses();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

    }
}
