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
using VrManager.Helpers;

namespace VrManager.Pages
{
    /// <summary>
    /// Interaction logic for SettingMenu.xaml
    /// </summary>
    public partial class SettingMenu : Page
    {
        public SettingMenu()
        {
            InitializeComponent();
        }

        private void MainSettingTile_Click(object sender, RoutedEventArgs e)
        {
            App.Frame.Navigate(new MainSettingPage());
        }

        private void PersonalizationSettingTile_Click(object sender, RoutedEventArgs e)
        {
            App.Frame.Navigate(new PersonalizationSettingsPage());
        }

        private void thisPage_Loaded(object sender, RoutedEventArgs e)
        {
            App.MainWnd.ChangeTitle(Title);
            PermissionsHelper.PermissionsLicense(MainSettingTile, PersonalizationSettingTile, ChangePasswordTile , OpenSettingPlayer);
            PermissionsHelper.PermissionsAdmin(MainSettingTile, PersonalizationSettingTile, ChangePasswordTile, OpenSettingPlayer);
        }

        private void ChangePasswordTile_Click(object sender, RoutedEventArgs e)
        {
            App.Frame.Navigate(new ChangePasswordPage());
        }

        private void ChangeFileLicense_Click(object sender, RoutedEventArgs e)
        {
            App.Frame.Navigate(new CheckLicensePage());
        }

        private void OpenSettingPlayer_Click(object sender, RoutedEventArgs e)
        {
            OpenVrPleyerHelper.OpenVrPlayerForSetting();
           
        }

        private void thisPage_Unloaded(object sender, RoutedEventArgs e)
        {
            OpenVrPleyerHelper.CloseVrPlayerForSetting();
            App.MainWnd.Topmost = App.Setting.IsKioskMode;
            App.LockDisplayWindow.Topmost = App.Setting.IsKioskMode;
        }
    }
}
