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
using VrManager.Data.Concrete;

namespace VrManager.Pages
{
    /// <summary>
    /// Interaction logic for ChangePasswordPage.xaml
    /// </summary>
    public partial class ChangePasswordPage : Page
    {
        public ChangePasswordPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            App.MainWnd.ChangeTitle(Title);
            ValPasswordWrong.Visibility = Visibility.Collapsed;
            ValPasswordMach.Visibility = Visibility.Collapsed;
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            string oldPass = Hesher.MD5Hash(TB_OldPassword.Password);
            string newPass1 = TB_NewPassword1.Password;
            string newPass2 = TB_NewPassword2.Password;

            if(oldPass != App.Repository.Users.FirstOrDefault().Password)
            {
                ValPasswordWrong.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                ValPasswordWrong.Visibility = Visibility.Collapsed;
            }

            if (newPass1 != newPass2)
            {
                ValPasswordMach.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                ValPasswordMach.Visibility = Visibility.Collapsed;
            }

            string password = Hesher.MD5Hash(TB_NewPassword1.Password);
            App.Repository.ChangePassword(password);
            App.Frame.Navigate(new StartUpPage());

        }
    }
}
