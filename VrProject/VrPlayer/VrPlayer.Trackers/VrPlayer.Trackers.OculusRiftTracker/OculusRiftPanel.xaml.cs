using System;
using System.Windows.Controls;

namespace VrPlayer.Trackers.OculusRiftTracker
{
    public partial class OculusRiftPanel : UserControl
    {
        public OculusRiftPanel(OculusRiftTracker tracker)
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
