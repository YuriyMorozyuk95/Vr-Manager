using System;
using System.Windows;
using System.Windows.Media.Media3D;
using VrPlayer.Helpers;
using VrPlayer.ViewModels;

namespace VrPlayer.Views.ViewPorts
{
    public partial class ExternalViewPort : FullScreenWindow
    {
        private readonly ViewPortViewModel _viewModel;
        public GeometryModel3D Geometry { get; set; }

        public ExternalViewPort(GeometryModel3D geometry)
        {
            Geometry = geometry;
            InitializeComponent();
            try
            {
                _viewModel = ((App) Application.Current).ViewModelFactory.CreateViewPortViewModel();
                DataContext = _viewModel;
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while initilizing ExternalViewPort view.", exc);
            }
        }
    }
}
