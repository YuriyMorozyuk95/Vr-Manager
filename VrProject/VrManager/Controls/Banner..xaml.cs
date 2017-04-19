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

namespace VrManager.Controls
{
    /// <summary>
    /// Interaction logic for Banner.xaml
    /// </summary>
    public partial class Banner : UserControl
    {
        public Banner()
        {
            InitializeComponent();
        }

        public event EventHandler TileClick;

        private List<Uri> _videoCollection;
        public List<Uri> VideoCollection { get
            {
                return _videoCollection;
            }
            set
            {
                _videoCollection = value;
                CurrentVideoIndex = 0;
            }
        }

        private int _currentVideoIndex;
        public int CurrentVideoIndex
        {
            get
            {
                return _currentVideoIndex;
            }
            set
            {
                try
                {
                    if (VideoCollection == null)
                    {
                        throw new NullReferenceException();
                    }

                    if (VideoCollection.Count == 0)
                    {
                        throw new IndexOutOfRangeException();
                    }

                    if (value >= VideoCollection.Count)
                    {
                        value = 0;
                    }

                    _currentVideoIndex = value;
                    Player.Stop();
                    Player.Source = VideoCollection[_currentVideoIndex];
                    Player.Position = TimeSpan.Zero;
                    Player.Play();
                }
                catch
                {

                }
            }
        }

        public void StopPlayer()
        {
            Player.Stop();
        }
        public void StartPlayer()
        {
            Player.Play();
        }

        public void PausePlayer()
        {
            Player.Pause();
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            CurrentVideoIndex++;
        }
        private void This_Loaded(object sender, RoutedEventArgs e)
        {
            Player.Play();
        }
        private void TileConteiner_Click(object sender, RoutedEventArgs e)
        {
            if(TileClick != null)
            {
                TileClick.Invoke(sender, e);
            }
        }


    }
}
