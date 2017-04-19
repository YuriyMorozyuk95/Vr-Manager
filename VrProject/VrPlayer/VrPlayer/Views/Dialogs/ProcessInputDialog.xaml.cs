using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace VrPlayer.Views.Dialogs
{
    public partial class ProcessInputDialog : Window
    {
        public ProcessInputDialog()
        {
            InitializeComponent();
            DataContext = this;
        }

        public Process Process
        {
            get { return (Process)ResponseComboBox.SelectedValue; }
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

        public IEnumerable<Process> Processes
        {
            get
            {
                var processlist = Process.GetProcesses();
                return processlist.Where(process => !String.IsNullOrEmpty(process.MainWindowTitle)).ToList();
            }
        }
    }
}
