using System;
using System.Windows.Controls;

namespace VrPlayer.Effects.DepthMapOverUnder
{
    public partial class DepthMapOverUnderPanel : UserControl
    {
        public DepthMapOverUnderPanel(DepthMapOverUnderEffect effect)
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
