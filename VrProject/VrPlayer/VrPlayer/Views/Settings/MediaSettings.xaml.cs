using System.Linq;
using System.Windows;
using System.Windows.Controls;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Medias;

namespace VrPlayer.Views.Settings
{
    public partial class MediaSettings : UserControl
    {
        public MediaSettings()
        {
            InitializeComponent();
        }

        private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
            var tabControl = ((TabControl)sender);
            tabControl.SelectedItem = tabControl.Items.Cast<IPlugin<IMedia>>().FirstOrDefault(plugin => plugin.Panel != null);
        }
    }
}
