using System;
using System.Windows.Controls;

namespace VrPlayer.Projections.FullDome
{
    public partial class FullDomePanel : UserControl
    {
        public FullDomePanel(FullDomeProjection projection)
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
