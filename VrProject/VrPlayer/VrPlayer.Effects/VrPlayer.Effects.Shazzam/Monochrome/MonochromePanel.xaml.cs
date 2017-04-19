using System;
using System.Windows.Controls;

namespace VrPlayer.Effects.Shazzam.Monochrome
{
    public partial class MonochromePanel : UserControl
    {
        public MonochromePanel(MonochromeEffect effect)
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
