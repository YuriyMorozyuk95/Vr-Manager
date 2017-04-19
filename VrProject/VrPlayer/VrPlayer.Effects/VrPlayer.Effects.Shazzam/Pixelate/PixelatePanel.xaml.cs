using System;
using System.Windows.Controls;

namespace VrPlayer.Effects.Shazzam.Pixelate
{
    public partial class PixelatePanel : UserControl
    {
        public PixelatePanel(PixelateEffect effect)
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
