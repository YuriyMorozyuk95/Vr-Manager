using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace VrManager
{
    /// <summary>
    /// Interaction logic for LockScreenWindow.xaml
    /// </summary>
    public partial class LockScreenWindow : Window
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(LockScreenWindow));
        public string LockScreenWindowName { get; set; } = "LockScreenWindow";
        public LockScreenWindow()
        {
            InitializeComponent();
        }

        public void ChangeDisplay(int numberMonitor)
        {
            MonitorManager.MoveWindow(LockScreenWindowName, numberMonitor);
            //WindowState = WindowState.Normal;
            //WindowState = WindowState.Maximized;
            Topmost = false;
            //Hide();
            _log.Error("hide");
        }
        public void PlayerClosed()
        {
            _log.Error("show");
            Topmost = true;
           // Show();
        }
    }
}
