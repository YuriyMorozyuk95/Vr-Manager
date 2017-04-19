using System;
using System.Windows;
using System.Windows.Forms;
using UserControl = System.Windows.Controls.UserControl;

namespace VrPlayer.Stabilizers.Csv
{
    public partial class CsvPanel : UserControl
    {
        public CsvPanel()
        {
            InitializeComponent();
        }

        private readonly CsvStabilizer _stabilizer;

        public CsvPanel(CsvStabilizer stabilizer)
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
            dialog.Filter = "CSV Files|*.csv|All Files|*";
            var result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                _stabilizer.FilePath = dialog.FileName;
            }
        }
    }


}
