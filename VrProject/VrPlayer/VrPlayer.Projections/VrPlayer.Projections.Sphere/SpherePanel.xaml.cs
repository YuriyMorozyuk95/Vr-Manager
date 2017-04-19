using System;
using System.Windows.Controls;

namespace VrPlayer.Projections.Sphere
{
    public partial class SpherePanel : UserControl
    {
        public SpherePanel(SphereProjection projection)
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
