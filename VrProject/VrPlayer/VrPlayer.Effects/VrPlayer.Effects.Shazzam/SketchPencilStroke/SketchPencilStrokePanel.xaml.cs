using System;
using System.Windows.Controls;

namespace VrPlayer.Effects.Shazzam.SketchPencilStroke
{
    public partial class SketchPencilStrokePanel : UserControl
    {
        public SketchPencilStrokePanel(SketchPencilStrokeEffect effect)
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
