using System;
using System.Windows.Controls;

namespace VrPlayer.Projections.DualFullDome
{
    public partial class DualFullDomePanel : UserControl
    {
        public DualFullDomePanel(DualFullDomeProjection projection)
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
