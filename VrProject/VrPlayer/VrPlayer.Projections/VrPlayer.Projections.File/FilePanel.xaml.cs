using System;
using System.Windows;
using System.Windows.Forms;
using UserControl = System.Windows.Controls.UserControl;

namespace VrPlayer.Projections.File
{
    public partial class FilePanel : UserControl
    {
        private FileProjection _projection;

        public FilePanel(FileProjection projection)
        {
            InitializeComponent();
            try
            {
                _projection = projection;
                DataContext = projection;
            }
            catch (Exception exc)
            {
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "3D Files|*.obj;*.3ds|All Files|*";
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
                _projection.FilePath = dialog.FileName;
        }
    }
}
