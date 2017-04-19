using System;
using System.Windows.Controls;

namespace VrPlayer.Trackers.KinectTracker
{
    public partial class KinectPanel : UserControl
    {
        public KinectPanel(KinectTracker tracker)
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
