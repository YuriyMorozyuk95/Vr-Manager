using System;
using System.Windows.Controls;

namespace VrPlayer.Effects.Shazzam.ToonShader
{
    public partial class ToonShaderPanel : UserControl
    {
        public ToonShaderPanel(ToonShaderEffect effect)
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
