using System;
using System.Windows.Controls;

namespace VrPlayer.Projections.Cube
{
    public partial class CubePanel : UserControl
    {
        public CubePanel(CubeProjection projection)
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