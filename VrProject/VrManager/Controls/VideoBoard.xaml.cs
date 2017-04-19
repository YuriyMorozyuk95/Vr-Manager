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
using VrManager.Data.Entity;
using MahApps.Metro.Controls;
using VrManager.Data.Abstract;

namespace VrManager.Controls
{
    /// <summary>
    /// Interaction logic for VideoBoard.xaml
    /// </summary>
    public partial class VideoBoard : UserControl
    {
        private int _widthTiles;
        private int _heightTiles;
        private IEnumerable<BaseContentEntity> _currentItemsList;
        public int PageSize { get; set; } = 9;

        public VideoBoard()
        {
            InitializeComponent();
        }


        public int Count
        {
            get
            {
                return _currentItemsList.Count();
            }
        }
        public int HeightTiles
        {
            get
            {
                return _heightTiles;
            }
            set
            {
                foreach (Tile tile in Board.Children)
                {
                    tile.Height = value;
                }
                _heightTiles = value;
            }
        }
        public int WidthTiles
        {
            get
            {
                return _widthTiles;
            }
            set
            {
                foreach (Tile tile in Board.Children)
                {
                    tile.Width = value;
                }
                _widthTiles = value;
            }
        }


        public event EventHandler TileClick;

        public void AddItem(BaseContentEntity video)
        {
            FrameworkElement content = null;

            if (video.IconType == Data.Abstract.IconType.Image)
            {
                content = new Image()
                {
                    Source = new BitmapImage(new Uri(video.PathIcon, UriKind.Absolute))
                };
            }
            else if (video.IconType == Data.Abstract.IconType.Video)
            {
                content = new MediaElement()
                {
                    LoadedBehavior = MediaState.Manual,
                    Volume = 0,
                    Source = new Uri(video.PathIcon, UriKind.Absolute),
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch
                };

               ((MediaElement)content).MediaEnded += VideoEnding;
               ((MediaElement)content).Play();
            }
            content.VerticalAlignment = VerticalAlignment.Top;

            Tile newVideoBox = new Tile
            {
                Title = video.Name,
                Content = content,
                Tag = video,
            };
            newVideoBox.Height = 240;
            newVideoBox.Width = 475;
            newVideoBox.Margin = new Thickness(25,15, 25, 15);
            newVideoBox.Click += NewVideoBox_Click;
            Board.Children.Add(newVideoBox);

        }

        internal void ChangeVideoList(int _currentPage)
        {
           IEnumerable<BaseContentEntity> showingItems = _currentItemsList.Skip((_currentPage-1)*PageSize).Take(PageSize).ToList();
           FreeResourses();
           ClearItems();
           AddingItems(showingItems);
        }

        private void ClearItems()
        {
            Board.Children.Clear();
        }

        public void StartShowing()
        {
            foreach (Tile tile in Board.Children)
            {
                if (tile.Content is MediaElement)
                {
                    MediaElement videoPlayer = tile.Content as MediaElement;
                    videoPlayer.Play();
                }
            }
        }
        public void SetSourse(IEnumerable<BaseContentEntity> items)
        {
            _currentItemsList = items;
        }
        public void AddingItems(IEnumerable<BaseContentEntity> items)
        {
            foreach (BaseContentEntity item in items)
            {
                AddItem(item);
                Message.Visibility = Visibility.Collapsed;
            }
           
        }     
        public void FreeResourses()
        {
            foreach(UIElement element in Board.Children)
            {
                if(element is MediaElement)
                {
                    var el = element as MediaElement;
                    el.Stop();
                    el.Close();
                    
                }
            }
        }

        private void NewVideoBox_Click(object sender, RoutedEventArgs e)
        {
            Tile videoBox = sender as Tile;
            if (TileClick != null)
            {
                TileClick.Invoke(videoBox.Tag, new EventArgs());
            }
        }
        private void VideoEnding(object sender, RoutedEventArgs e)
        {
            MediaElement el = sender as MediaElement;
            el.Position = TimeSpan.Zero;
            el.Play();
        }

    }
}
