using System;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;

namespace VrPlayer.Medias.WpfMediaKit
{
    public partial class WpfMediaKitPanel : UserControl
    {
        public WpfMediaKitPanel(WpfMediaKitMedia media)
        {
            InitializeComponent();
            try
            {
                DataContext = media;
            }
            catch (Exception exc)
            {
            }
        }

        private void DownloadKLite_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("http://codecguide.com/download_kl.htm");
        }
    }
}
