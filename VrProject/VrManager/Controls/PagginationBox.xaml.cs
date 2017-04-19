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
using VrManager.Data.Abstract;
using VrManager.Helpers;

namespace VrManager.Controls
{
    /// <summary>
    /// Interaction logic for PagginationBox.xaml
    /// </summary>
    public partial class PagginationBox : UserControl
    {
        public PagginationBox()
        {
            InitializeComponent();
        }
        public PagginationBox(VideoBoard board)
        {
            InitializeComponent();
            _board = board;
            CurrentPage = 1;
        }

        private int _currentPage;
        private VideoBoard _board;

        public void GenerateListPagginationButtons()
        {
            ContainerButtons.Children.Clear();
            if (_board.Count > 9)
            {
                int totalPages = (int)Math.Ceiling((decimal)_board.Count / _board.PageSize);
                for (int i = 1; i <= totalPages; i++)
                {
                    AppBarButton paggButton = new AppBarButton
                    {
                        Icon = i.ToString(),
                        Tag = i,
                        Size = 125,
                        FontIcon = Fonts.GetFontFamilies(new Uri("pack://application:,,,/Resources/Fonts/#")).FirstOrDefault()

                };
                    paggButton.Click += PaggButton_Click;
                    ContainerButtons.Children.Add(paggButton);
                }
            }
            else
            {
                this.Visibility = Visibility.Collapsed;
            }
        }
        public int CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
               
                _currentPage = value;
              

                if (_currentPage+1 <= Math.Ceiling((decimal)_board.Count/_board.PageSize) )
                { 
                     Next.IsEnabled = true;

                }
                else
                {
                    Next.IsEnabled = false;
                }

                if (_currentPage > 1)
                {
                    Back.IsEnabled = true;
                }
                else
                {
                    Back.IsEnabled = false;
                }

                _board.ChangeVideoList(_currentPage);
                CurrentNumber.Icon = _currentPage.ToString();
                selectPaggButton();
            }
        }
        public void SetViewer(VideoBoard videoViewer)
        {
            _board = videoViewer;
        }

        private void PaggButton_Click(object sender, RoutedEventArgs e)
        {
            AppBarButton paggButton = sender as AppBarButton;
            CurrentPage = (int)paggButton.Tag;
            selectPaggButton();
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage--;
            selectPaggButton();
        }          
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage++;
            selectPaggButton();
        }

        private void selectPaggButton()
        {
            foreach (AppBarButton button in ContainerButtons.Children)
            {
                button.Selected = false;
                if ((int)button.Tag == CurrentPage)
                {
                    button.Selected = true;
                }
                
            }
        }
    }
}
