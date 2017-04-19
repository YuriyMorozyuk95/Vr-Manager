using System;
using System.Windows;
using System.Windows.Controls;

namespace VrPlayer.Effects.Anaglyph
{
    public partial class AnaglyphPanel : UserControl
    {
        private AnaglyphEffect _effect;

        public AnaglyphPanel(AnaglyphEffect effect)
        {
            _effect = effect;
            InitializeComponent();
            try
            {
                DataContext = effect;
            }
            catch (Exception exc)
            {
            }
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
