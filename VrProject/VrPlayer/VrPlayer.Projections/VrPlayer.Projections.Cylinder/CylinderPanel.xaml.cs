using System;
using System.Windows.Controls;

namespace VrPlayer.Projections.Cylinder
{
    public partial class CylinderPanel : UserControl
    {
        public CylinderPanel(CylinderProjection projection)
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
