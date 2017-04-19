using System;
using System.Windows.Controls;

namespace VrPlayer.Effects.UnwrapFishEye
{
    public partial class UnwrapFishEyePanel : UserControl
    {
        public UnwrapFishEyePanel(UnwrapFishEyeEffect effect)
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
