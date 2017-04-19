using System;
using System.Windows;
using System.Windows.Forms;
using UserControl = System.Windows.Controls.UserControl;

namespace VrPlayer.Stabilizers.Deshaker
{
    public partial class DeshakerPanel : UserControl
    {
        private readonly DeshakerStabilizer _stabilizer;

        public DeshakerPanel(DeshakerStabilizer stabilizer)
        {
            InitializeComponent();
            try
            {
                _stabilizer = stabilizer;
                DataContext = stabilizer;
            }
            catch (Exception exc)
            {
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Deshaker Files|*.log|All Files|*";
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                _stabilizer.FilePath = dialog.FileName;
            }
        }
    }
}
