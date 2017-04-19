using System;
using System.Windows.Controls;

namespace VrPlayer.Effects.UnwrapFishEyeStereo
{
    public partial class UnwrapFishEyeStereoPanel : UserControl
    {
        public UnwrapFishEyeStereoPanel(UnwrapFishEyeStereoEffect effect)
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
