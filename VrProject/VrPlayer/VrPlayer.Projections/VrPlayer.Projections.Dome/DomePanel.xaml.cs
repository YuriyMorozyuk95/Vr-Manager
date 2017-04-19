using System;
using System.Windows.Controls;

namespace VrPlayer.Projections.Dome
{
    public partial class DomePanel : UserControl
    {
        public DomePanel(DomeProjection projection)
        {
            InitializeComponent();
            try
            {
                DataContext = projection;
            }
            catch (Exception exc)
            {
            }
        }
    }
}
