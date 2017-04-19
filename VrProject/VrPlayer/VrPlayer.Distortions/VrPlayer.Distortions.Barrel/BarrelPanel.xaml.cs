using System;
using System.Windows.Controls;

namespace VrPlayer.Distortions.Barrel
{
    public partial class BarrelPanel : UserControl
    {
        public BarrelPanel(BarrelEffect effect)
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
