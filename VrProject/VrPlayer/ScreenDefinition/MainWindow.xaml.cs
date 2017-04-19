using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MessageBox = System.Windows.MessageBox;
using Rectangle = System.Drawing.Rectangle;

namespace ScreenDefinition
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            int screenCount = Screen.AllScreens.Length;
            _infoListBox.Items.Add(string.Format("Количество экранов:{0}", screenCount));
            _infoListBox.Items.Add(string.Format("Имя экрана по умолчанию:{0}", Screen.PrimaryScreen.DeviceName));
            _infoListBox.Items.Add(string.Format("Список экранов:"));

            foreach (var s in Screen.AllScreens)
            {
                _infoListBox.Items.Add(string.Format("{0}||B:{1}||WA:{2}", s.DeviceName, s.Bounds, s.WorkingArea));
            }

            //if (screenCount > 1)
            //{
            //    Screen secondScreen = Screen.AllScreens[1];
            //    Rectangle secondRectangle = secondScreen.WorkingArea;
            //    Top = secondRectangle.Top;
            //    Left = secondRectangle.Left;
            //}

            //WindowState = WindowState.Maximized;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            uint displayNumber = 0;
            if (string.IsNullOrWhiteSpace(_displayNumber.Text)
                || uint.TryParse(_displayNumber.Text, out displayNumber) == false)
            {
                MessageBox.Show("Номер дисплея должен быть целым положительным числом");
                return;
            }

            int screenCount = Screen.AllScreens.Length;
            if (screenCount <= displayNumber)
            {
                MessageBox.Show("Номер дисплея превышает количество дисплеев");
                return;
            }

            Screen screen = Screen.AllScreens[displayNumber];
            Rectangle rectangle = screen.WorkingArea;
            Top = rectangle.Top;
            Left = rectangle.Left;
            //WindowState = WindowState.Maximized;
        }
    }
}
