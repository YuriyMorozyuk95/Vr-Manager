using System;
using System.Windows.Controls;

namespace VrPlayer.Projections.Plane
{
    public partial class PlanePanel : UserControl
    {
        public PlanePanel(PlaneProjection projection)
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
