using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace VrPlayer.Views.Dialogs
{
    public partial class DiscInputDialog : Window
    {
        public DiscInputDialog()
        {
            InitializeComponent();
            DataContext = this;
        }

        public DriveInfo Drive
        {
            get { return (DriveInfo) ResponseComboBox.SelectedValue; }
            set { ResponseComboBox.SelectedValue = value; }
        }

        private void OnOkButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void OnCancelButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void UrlInputDialog_OnLoaded(object sender, RoutedEventArgs e)
        {
            ResponseComboBox.Focus();
        }

        public IEnumerable<DriveInfo> Drives
        {
            get
            {
                var drives = DriveInfo.GetDrives();
                return drives.Where(drive => drive.DriveType == DriveType.CDRom).ToList();
            }
        }
    }
}
