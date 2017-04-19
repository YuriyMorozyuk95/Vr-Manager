using System;
using System.Windows.Controls;

namespace VrPlayer.Distortions.Pincushion
{
    public partial class PincushionPanel : UserControl
    {
        public PincushionPanel(PincushionEffect effect)
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
