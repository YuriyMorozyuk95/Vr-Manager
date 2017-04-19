using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using VrPlayer.Helpers;
using VrPlayer.ViewModels;
using VrPlayer.Views.Dialogs;

namespace VrPlayer.Views.Settings
{
    public partial class ShortcutsSettings : UserControl
    {
        private readonly SettingsWindowViewModel _viewModel;

        public ShortcutsSettings()
        {
            InitializeComponent();
            try
            {
                _viewModel = ((App)Application.Current).ViewModelFactory.CreateSettingsWindowViewModel();
                DataContext = _viewModel;
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while initilizing ShortcutsSettings view.", exc);
            }
        }

        private void UIElement_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var textBox = (TextBox)sender;
            SelectKey(textBox);
        }

        private void SelectKey(TextBox textBox)
        {
            textBox.Background = Brushes.Yellow;
            var dialog = new KeyInputDialog();
            if (dialog.ShowDialog() == true)
            {
                AssignKey(textBox, dialog.Key);
            }
            textBox.Background = Brushes.White;
        }

        private void AssignKey(TextBox textBox, Key key)
        {
            if (textBox.Text == key.ToString())
                return;

            if (key == Key.Escape)
                return;

            if (_viewModel.State.Shortcuts.Contains(key))
            {
                var response = MessageBox.Show(
                    string.Format("The key '{0}' is already assigned to another shortcut.\n Please select any other key.", key), 
                    "Duplicated key", 
                    MessageBoxButton.OKCancel, 
                    MessageBoxImage.Warning);
                if (response == MessageBoxResult.OK)
                    SelectKey(textBox);
            }
            else
            {
                textBox.Text = key.ToString();
            }
        }
    }
}
