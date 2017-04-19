using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace VrManager.Pages
{
    /// <summary>
    /// Interaction logic for InstallAdditionalComponentPage.xaml
    /// </summary>
    public partial class InstallAdditionalComponentPage : Page
    {
        private Process installerSQL;
        private Process installerFont;
        private Process installerKLite;
        private Process installerVCL;

        private string pathToFolderInstaller;

        public InstallAdditionalComponentPage()
        {
            InitializeComponent();
            SetPathForInstaller();
            SetInstallerProcesses();
        }

        private void SetPathForInstaller()
        {
            string nameFolder = @"\AdditionalInstaller";
#if (DEBUG)
            pathToFolderInstaller = Directory.GetParent(Directory.GetCurrentDirectory())
            .Parent
            .FullName + nameFolder;
#else
            pathToFolderInstaller = Directory.GetParent(Directory.GetCurrentDirectory()).FullName + nameFolder;
#endif
        }
        private void SetInstallerProcesses()
        {

            installerSQL = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WorkingDirectory = pathToFolderInstaller,
                    Verb = "runas",
                    FileName = "SSCERuntime_x64-ENU.exe"
                }
            };

            installerFont = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WorkingDirectory = pathToFolderInstaller,
                    Verb = "runas",
                    FileName = "segoe-mdl2-assets.ttf"
                }
            };

            installerKLite = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WorkingDirectory = pathToFolderInstaller,
                    Verb = "runas",
                    FileName = "K-Lite_Codec_Pack_1275_Basic.exe"
                }
            };


            installerVCL = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WorkingDirectory = pathToFolderInstaller,
                    Verb = "runas",
                    FileName = "vlc-2.2.4-win32.exe"
                }
            };

        }



        private void Btn_InstallSql_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                installerSQL.Start();
            }
            catch
            {

            }
        }

        private void Btn_InstallFont_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Shell32.Shell shell = new Shell32.Shell();
                Shell32.Folder fontFolder = shell.NameSpace(0x14);
                fontFolder.CopyHere(pathToFolderInstaller + @"\segoe-mdl2-assets.ttf");
            }
            catch
            {
                MessageBox.Show("у вас уже установлен шрифт");
            }
        }

        private void Btn_InstallCodec_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                installerKLite.Start();
            }
            catch
            {

            }
        }

        private void Btn_Vcl_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                installerVCL.Start();
            }
            catch
            {

            }
        }

        private void thisPage_Unloaded(object sender, RoutedEventArgs e)
        {
            installerSQL.Dispose();
            installerFont.Dispose();
            installerKLite.Dispose();
            installerVCL.Dispose();
        }
    }
}
