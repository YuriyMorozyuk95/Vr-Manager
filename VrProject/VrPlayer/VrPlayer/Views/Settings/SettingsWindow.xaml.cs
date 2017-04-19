using System;
using System.Linq;
using System.Windows;
using VrPlayer.Helpers;

namespace VrPlayer.Views.Settings
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            try
            {
                DataContext = ((App)Application.Current).ViewModelFactory.CreateSettingsWindowViewModel();
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while initilizing Settings window.", exc);
            }
        }

        public static void ShowSingle()
        {
            var window = Application.Current.Windows.Cast<Window>().SingleOrDefault(w => w.GetType() == typeof(SettingsWindow));
            if (window != null)
            {
                window.Activate();
            }
            else
            {
                window = new SettingsWindow();
                window.Show();
            }
        }

        private void SettingsWindow_OnClosed(object sender, EventArgs e)
        {
            Content = null;
        }
    }
}
