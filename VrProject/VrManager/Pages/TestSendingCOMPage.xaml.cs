using ComPortPackages.Console;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for SendingCOM.xaml
    /// </summary>
    public partial class SendingCOMTestPage : Page
    {
        private bool flagIsStarted = false;
        public SendingCOMTestPage()
        {
            InitializeComponent();
        }

        private void startSend_Click(object sender, RoutedEventArgs e)
        {
            if (!flagIsStarted)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                if (dialog.ShowDialog() == true)
                {
                    ComPrortSender.Send(dialog.FileName, Terminal, App.Current);
                }
                flagIsStarted = true;
            }
        }

        private void BtnPause_Click(object sender, RoutedEventArgs e)
        {
            if (flagIsStarted)
            {
                ComPortPackagesService.Pause();
            }
        }

        private void BtnReusume_Click(object sender, RoutedEventArgs e)
        {
            if (flagIsStarted)
            {
                ComPortPackagesService.Resume();
            }
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            if (flagIsStarted)
            {
                ComPortPackagesService.Stop();
                flagIsStarted = false;
            }
        }
    }
}
