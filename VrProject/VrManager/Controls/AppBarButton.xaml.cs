using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for AppBarButton.xaml
    /// </summary>
    public partial class AppBarButton : UserControl, INotifyPropertyChanged
    {
        private string _icon;
        private string _text;
        private double _height; /*= 48;*/
        private double _width; /*= 48;*/
        private double _size;
        private double _fontSizeIcon;/*=16;*/
        private double _fontSizeText;
        private FontFamily _fontIcon = Application.Current.Resources["IconFont"] as FontFamily;
        private Brush _foregroundColor = App.Current.Resources["ColorForeground"] as Brush;

        public AppBarButton()
        {
            InitializeComponent();
            DataContext = this;
        }

        public string Icon
        {
            get
            {
                return _icon;
            }
            set
            {
                _icon = value;
                OnChanged("Icon");
            }
        }
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {     
                _text = value;
                OnChanged("Text");
            }
        }
        public double Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
                ButtonHeight = _size;
                ButtonWidth = _size;
                FontSizeIcon = _size / 3;
                FontSizeText = _size / 4;
            }
        }
        public double ButtonHeight
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
                OnChanged("ButtonHeight");
            }
        }
        public double ButtonWidth
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
                OnChanged("ButtonWidth");
            }
        }
        public double FontSizeIcon
        {
            get
            {
                return _fontSizeIcon;
            }
            set
            {
                _fontSizeIcon = value;
                OnChanged("FontSizeIcon");
            }
        }
        public double FontSizeText
        {
            get
            {
                return _fontSizeText;
            }
            set
            {
                _fontSizeText = value;
                OnChanged("FontSizeText");
            }
        }
        public FontFamily FontIcon
        {
            get
            {
                return _fontIcon;
            }
            set
            {
                _fontIcon = value;
                OnChanged("FontIcon");
            }
        }
        public bool Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                _selected = value;
                if (_selected)
                {
                    ForegroundColor = App.Current.Resources["AccentColorBrush"] as Brush;
                }
                else
                {
                    ForegroundColor = App.Current.Resources["ColorForeground"] as Brush;
                }


            }
        }
        public Brush ForegroundColor
        {
            get
            {
                return _foregroundColor;
            }
            set
            {
                _foregroundColor = value;
                OnChanged("ForegroundColor");
            }
        }
      


        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click",
            RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(AppBarButton));
        private bool _selected;
     

        public event RoutedEventHandler Click
        {
            add
            {
                base.AddHandler(AppBarButton.ClickEvent, value);
            }
            remove
            {
                base.RemoveHandler(AppBarButton.ClickEvent, value);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnChanged(string prop)
        {
            if(PropertyChanged !=null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(prop));
            }
        }

        private void Shell_Click(object sender, RoutedEventArgs e)
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(AppBarButton.ClickEvent);
            RaiseEvent(newEventArgs);
        }
    }
}
