using System;
using System.Windows.Controls;

namespace VrPlayer.Effects.Shazzam.Bloom
{
    public partial class BloomPanel : UserControl
    {
        public BloomPanel(BloomEffect effect)
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
