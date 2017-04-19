using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using VrPlayer.Helpers;
using VrPlayer.Models.State;
using VrPlayer.ViewModels;
using Application = System.Windows.Application;

namespace VrPlayer.Views.ViewPorts
{
    public partial class MainViewPort
    {
        private static ViewPortViewModel _viewModel;
        private readonly ExternalViewPort _externalViewPort;
        private static DispatcherTimer _clickWaitTimer;

        public MainViewPort()
        {
            InitializeComponent();
            try
            {
                _viewModel = ((App) Application.Current).ViewModelFactory.CreateViewPortViewModel();
                DataContext = _viewModel;

                //Todo: Extract to view model
                _externalViewPort = new ExternalViewPort(Resources["Geometry"] as GeometryModel3D);
                _externalViewPort.Closing += ExternalViewPortOnClosing;
                _viewModel.State.PropertyChanged += StateOnPropertyChanged;
                _viewModel.State.StereoOutput = _viewModel.State.StereoOutput;

                _clickWaitTimer = new DispatcherTimer(
                    new TimeSpan(0, 0, 0, 0, 200),
                    DispatcherPriority.Background,
                    mouseWaitTimer_Tick,
                    Dispatcher.CurrentDispatcher);
                _clickWaitTimer.Stop();
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while initilizing MainViewPort view.", exc);
            }
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //switch (e.ClickCount)
            //{
            //    case 1:
            //        _clickWaitTimer.Start();
            //        break;
            //    case 2:
            //        _clickWaitTimer.Stop();
            //        //System.Windows.MessageBox.Show("dsdsds");
            //        //((FullScreenWindow)Application.Current.MainWindow).ToggleFullScreen();
            //        e.Handled = true;
            //        break;
            //}
        }

        private static void mouseWaitTimer_Tick(object sender, EventArgs e)
	    {
		    _clickWaitTimer.Stop();
            _viewModel.ToggleNavigationCommand.Execute(null);
	    }

        private void ExternalViewPortOnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            if (!(sender is ExternalViewPort)) return;
            if (_viewModel.State.StereoOutput != LayoutMode.DualScreen) return;

            cancelEventArgs.Cancel = true;
            ((Window)sender).Hide();
            _viewModel.State.StereoOutput = LayoutMode.MonoLeft;
        }

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta == 0)
                return;

            if (e.Delta > 0 && _viewModel.Config.CameraFieldOfView > 0)
                _viewModel.Config.CameraFieldOfView--;

            if (e.Delta < 0 && _viewModel.Config.CameraFieldOfView < 360)
                _viewModel.Config.CameraFieldOfView++;
        }

        private void StateOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName != "StereoOutput") return;

            ViewPortLeft.Visibility = Visibility.Visible;
            if (_viewModel.State.StereoOutput == LayoutMode.MonoRight)
            {
                ViewPortLeft.Visibility = Visibility.Hidden;
            }

            ViewPortRight.Visibility = Visibility.Visible;
            if (_viewModel.State.StereoOutput == LayoutMode.MonoLeft ||
                _viewModel.State.StereoOutput == LayoutMode.DualScreen)
            {
                ViewPortRight.Visibility = Visibility.Hidden;
            }
            
            if (_viewModel.State.StereoOutput == LayoutMode.DualScreen)
            {
                var secondScreenIndex = (SystemInformation.MonitorCount >= 2) ? 1 : 0;
                PositionWindowToScreen(Application.Current.MainWindow, Screen.AllScreens[0]);
                PositionWindowToScreen(_externalViewPort, Screen.AllScreens[secondScreenIndex]);
            }
            else
            {
                if(_externalViewPort != null)
                    _externalViewPort.Hide();
            }
        }

        private void PositionWindowToScreen(Window window, Screen screen)
        {
            window.WindowStartupLocation = WindowStartupLocation.Manual;
            window.Left = screen.WorkingArea.Left;
            window.Top = screen.WorkingArea.Top;
            window.Width = screen.WorkingArea.Width;
            window.Height = screen.WorkingArea.Height;
            window.Show();
            window.WindowState = WindowState.Maximized;
            window.Focus();
        }
    }
}
