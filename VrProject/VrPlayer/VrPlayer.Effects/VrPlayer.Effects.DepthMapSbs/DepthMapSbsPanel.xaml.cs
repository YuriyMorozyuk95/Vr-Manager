using System;
using System.Windows.Controls;

namespace VrPlayer.Effects.DepthMapSbs
{
    public partial class DepthMapSbsPanel : UserControl
    {
        public DepthMapSbsPanel(DepthMapSbsEffect effect)
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
