using System;
using System.Windows.Controls;

namespace VrPlayer.Trackers.RazerHydraTracker
{
    public partial class RazerHydraPanel : UserControl
    {
        public RazerHydraPanel(RazerHydraTracker tracker)
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