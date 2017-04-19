using System;
using System.Windows.Controls;

namespace VrPlayer.Projections.DualDome
{
    public partial class DualDomePanel : UserControl
    {
        public DualDomePanel(DualDomeProjection projection)
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
