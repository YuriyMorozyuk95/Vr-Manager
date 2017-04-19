using System.Windows;
using System.Windows.Input;

namespace VrPlayer.Views.Dialogs
{
    public partial class KeyInputDialog : Window
    {
        public KeyInputDialog()
        {
            InitializeComponent();
            DataContext = this;
        }

        private Key _key;
        public Key Key
        {
            get { return _key; }
        }

        private void KeyInputDialog_OnKeyDown(object sender, KeyEventArgs e)
        {
            _key = e.Key;
            DialogResult = true;
        }
    }
}
