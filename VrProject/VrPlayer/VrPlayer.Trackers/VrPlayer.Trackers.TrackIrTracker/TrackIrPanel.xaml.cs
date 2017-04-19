using System;
using System.Windows.Controls;

namespace VrPlayer.Trackers.TrackIrTracker
{
    public partial class TrackIrPanel : UserControl
    {
        public TrackIrPanel(TrackIrTracker tracker)
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
