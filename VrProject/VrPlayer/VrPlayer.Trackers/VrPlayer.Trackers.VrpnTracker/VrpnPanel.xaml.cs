using System;
using System.Windows.Controls;

namespace VrPlayer.Trackers.VrpnTracker
{
    public partial class VrpnPanel : UserControl
    {
        public VrpnPanel(VrpnTracker tracker)
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
