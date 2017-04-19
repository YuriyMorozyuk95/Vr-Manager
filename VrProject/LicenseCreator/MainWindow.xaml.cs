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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LicenseCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Btn_FilePicker_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "(*.lic) | *.lic";
            dialog.FileName = "VrLicense";
            if (dialog.ShowDialog() == true)
            {
                TB_LicensePath.Text = dialog.FileName;
            }
        }

        private void Btn_LicenseCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LicenseProvider lic = new LicenseProvider
                {
                    FullPathLicense = TB_LicensePath.Text
                };
                lic.OpenFileClientInfo(TB_ClientInfoPath.Text);
                lic.CreateLicense();
                MessageBox.Show("Лицензия создана");
                this.Close();
            }
            catch
            {
                MessageBox.Show("Выбирите путь где создать файл лицензии");
            }
        }

        private void Btn_ClientInfo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "(*.info) | *.info";
            dialog.FileName = "Client";
            if (dialog.ShowDialog() == true)
            {
                TB_ClientInfoPath.Text = dialog.FileName;
            }
        }
    }
}
