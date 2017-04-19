using System;
using System.Windows.Controls;

namespace VrPlayer.Trackers.WiimoteTracker
{
    public partial class WiimotePanel : UserControl
    {
        public WiimotePanel(WiimoteTracker tracker)
        {
            InitializeComponent();
            try
            {
                DataContext = tracker;
            }
            catch (Exception exc)
            {
            }
        }
    }
}