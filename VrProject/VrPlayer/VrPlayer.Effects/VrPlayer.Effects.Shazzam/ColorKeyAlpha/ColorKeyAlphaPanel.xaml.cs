using System;
using System.Windows.Controls;

namespace VrPlayer.Effects.Shazzam.ColorKeyAlpha
{
    public partial class ColorKeyAlphaPanel : UserControl
    {
        public ColorKeyAlphaPanel(ColorKeyAlphaEffect effect)
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
