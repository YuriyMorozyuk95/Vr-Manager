using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using VrManager.Helpers;
using MahApps.Metro.Controls;

namespace VrManager.Pages
{
    /// <summary>
    /// Interaction logic for MainSettingPage.xaml
    /// </summary>
    public partial class MainSettingPage : Page
    {
        private bool _isFirstExecute = false;

        public MainSettingPage()
        {
            InitializeComponent();
            FirstMessage.Visibility = Visibility.Collapsed;
            Btn_ChangeAllTimeOff.IsEnabled = true;
            Btn_ChangeKioskMode.IsEnabled = true;
            TP_TimeOut.SelectedTime = new TimeSpan(0, 0, 30);
            TP_TimeAdvertising.SelectedTime = new TimeSpan(0, 0, 10);
            MonitorManager m = new MonitorManager();                      
        }

        

        public MainSettingPage(bool firstExecute) : base()
        {
            InitializeComponent();
            App.MainWnd.ShowHomeButton(false);
            _isFirstExecute = firstExecute;
            FirstMessage.Visibility = Visibility.Visible;
            Btn_ChangeAllTimeOff.IsEnabled = false;
            Btn_ChangeKioskMode.IsEnabled = false;
            hideControls();
        }

        private void hideControls()
        {
            L2.Visibility = Visibility.Collapsed;
            L3.Visibility = Visibility.Collapsed;
            L4.Visibility = Visibility.Collapsed;

            TP_TimeOut.Visibility = Visibility.Collapsed;
            CB_Monit.Visibility = Visibility.Collapsed;
            TP_TimeAdvertising.Visibility = Visibility.Collapsed;
            
            Btn_ChangeAllTimeOff.Visibility = Visibility.Collapsed;
            Btn_ChangeKioskMode.Visibility = Visibility.Collapsed;
            Btn_ChangeMonitor.Visibility = Visibility.Collapsed;
            Btn_TimeAdvertising.Visibility = Visibility.Collapsed;
            TS_KioskMode.Visibility = Visibility.Collapsed;
        }

        private void Btn_OpenFileDialog_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                TB_OpenFile.Text = dialog.SelectedPath;
            }
        }

        private void Btn_SaveNewFolderToFile_Click(object sender, RoutedEventArgs e)
        {
            CreateDictonaryHelper directoryCreator = new CreateDictonaryHelper();

           if (TB_OpenFile.Text == string.Empty)
            { 

                System.Windows.MessageBox.Show("Выберите путь к папке где будет сохранятся все файлы");
                return;
            }

            App.Setting.PathToFolderFiles = TB_OpenFile.Text;
            directoryCreator.CreateFolders(App.Setting.PathToFolderFiles);

            try
            {
                App.Repository.ChangePathToBd(App.Setting.PathToFolderFiles + @"\Config\DataBase\VrManagerData.sdf");
            }
            catch
            {
                System.Windows.MessageBox.Show("Пожалуйста установите Microsoft SQL Server Compact 4.0");
            }

            App.Setting.Export();

            ObserverUserActivity.StartActivityObserv();

            if (_isFirstExecute)
            {
                FirstMessage.Visibility = Visibility.Collapsed;
                App.MainWnd.Close();
                App.RestartApp();
            }
        }

        private void Btn_ChangeAllTimeOff_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBoxResult.Yes == System.Windows.MessageBox.Show("Вы уверены что хотите изменить время выключения всех видео", "", MessageBoxButton.YesNo))
            {
                App.Setting.AllTimeOff = TP_TimeOut.SelectedTime.Value;
                App.Setting.Export();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            App.MainWnd.ChangeTitle(Title);
            TB_OpenFile.Text = App.Setting.PathToFolderFiles;
            TP_TimeOut.SelectedTime = App.Setting.AllTimeOff;
            TS_KioskMode.IsChecked = App.Setting.IsKioskMode;
            CB_Monit.SelectedIndex = App.Setting.NumberRightMonitor;
            TP_TimeAdvertising.SelectedTime = App.Setting?.TimeAdvertising;
        }

        private void Btn_ChangeKioskMode_Click(object sender, RoutedEventArgs e)
        {
            App.Setting.IsKioskMode = (bool)TS_KioskMode.IsChecked;

            if (App.Setting.IsKioskMode)
            {
                Taskbar.Hide();
                App.MainWnd.Topmost = true;
            }
            else
            {
                App.MainWnd.Topmost = false;
                Taskbar.Show();
            }

            App.Setting.Export();
        }

        private void Btn_ChangeMonitor_Click(object sender, RoutedEventArgs e)
        {
            if (CB_Monit.SelectedIndex == 0)
            {
                App.Setting.NumberLeftMonitor = 1;
                App.Setting.NumberRightMonitor = 0;
            }
            else
            {
                App.Setting.NumberLeftMonitor = 0;
                App.Setting.NumberRightMonitor = 1;
            }

            MonitorManager.RestartPositionWindows();
            App.Setting.Export();
        }




        private void TextBlock_MouseRightButtonUp(object sender, RoutedEventArgs e)
        {
            App.Frame.Navigate(new InstallAdditionalComponentPage());
        }

        private void Btn_TimeAdvertising_Click(object sender, RoutedEventArgs e)
        {
            App.Setting.TimeAdvertising = TP_TimeAdvertising.SelectedTime.Value;
            App.Setting.Export();
        }
    }
}
