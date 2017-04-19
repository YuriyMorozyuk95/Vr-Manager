using System;
using System.Windows.Controls;

namespace VrPlayer.Trackers.MouseTracker
{
    public partial class MousePanel : UserControl
    {
        public MousePanel(MouseTracker tracker)
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
