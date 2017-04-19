using System;
using System.Windows.Controls;

namespace VrPlayer.Medias.Gdi
{
    public partial class GdiPanel : UserControl
    {
        public GdiPanel(GdiMedia media)
        {
            InitializeComponent();
            try
            {
                DataContext = media;
            }
            catch (Exception exc)
            {
            }
        }
    }
}
