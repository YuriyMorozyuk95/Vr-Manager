using System;
using System.Windows.Controls;

namespace VrPlayer.Trackers.YEI3SpaceTracker
{
    public partial class YEI3SpacePanel : UserControl
    {
        public YEI3SpacePanel(YEI3SpaceTracker tracker)
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
