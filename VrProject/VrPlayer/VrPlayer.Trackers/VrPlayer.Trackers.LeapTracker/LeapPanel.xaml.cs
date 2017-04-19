using System;
using System.Windows.Controls;

namespace VrPlayer.Trackers.LeapTracker
{
    public partial class LeapPanel : UserControl
    {
        public LeapPanel(LeapTracker tracker)
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
