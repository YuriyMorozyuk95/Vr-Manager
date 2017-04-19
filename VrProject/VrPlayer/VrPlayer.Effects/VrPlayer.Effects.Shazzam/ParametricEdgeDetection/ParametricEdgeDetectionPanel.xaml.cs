using System;
using System.Windows.Controls;

namespace VrPlayer.Effects.Shazzam.ParametricEdgeDetection
{
    public partial class ParametricEdgeDetectionPanel : UserControl
    {
        public ParametricEdgeDetectionPanel(ParametricEdgeDetectionEffect effect)
        {
            InitializeComponent();
            try
            {
                DataContext = effect;
            }
            catch (Exception exc)
            {
            }
        }
    }
}
