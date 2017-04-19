using System.Windows;

namespace VrPlayer.Views.Dialogs
{
    public partial class StreamInputDialog : Window
    {
        public StreamInputDialog()
        {
            InitializeComponent();
        }

        public string Url
        {
            get { return ResponseTextBox.Text; }
            set { ResponseTextBox.Text = value; }
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
            ResponseTextBox.Focus();
        }
    }
}
