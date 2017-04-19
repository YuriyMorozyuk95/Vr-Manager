using System;
using System.Windows.Controls;

namespace VrPlayer.Trackers.PsMoveTracker
{
    public partial class PsMovePanel : UserControl
    {
        public PsMovePanel(PsMoveTracker tracker)
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
