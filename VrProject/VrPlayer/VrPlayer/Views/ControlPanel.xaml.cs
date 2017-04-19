using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using VrPlayer.Helpers;
using VrPlayer.ViewModels;

namespace VrPlayer.Views
{
    public partial class ControlPanel : UserControl
    {
        private readonly MediaViewModel _viewModel;

        public ControlPanel()
        {
            InitializeComponent();
            try
            {
                _viewModel = ((App) Application.Current).ViewModelFactory.CreateMediaViewModel();
                DataContext = _viewModel;
                VrPlayerCommander.ControlPanel = this;
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while initilizing ControlPanel view.", exc);
            }
        }

        #region Events

        private void ProgressBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ExecuteSeekCommand(sender, e);
        }

        #endregion

        #region Helpers

        private void ExecuteSeekCommand(object sender, MouseEventArgs e)
        {
            if (_viewModel.State.MediaPlugin == null || _viewModel.State.MediaPlugin.Content == null)
                return;

            if (!_viewModel.State.MediaPlugin.Content.SeekCommand.CanExecute(null)) 
                return;
            
            var seekControl = (ProgressBar)sender;
            var position = e.GetPosition(seekControl).X;
            var percent = position / seekControl.ActualWidth;
            _viewModel.State.MediaPlugin.Content.SeekCommand.Execute(percent);
        }

        #endregion

        #region RemoteCommand
        public void Play()
        {
            _viewModel.State.MediaPlugin.Content.PlayCommand.Execute(new object());
        }

        public void Pause()
        {
            _viewModel.State.MediaPlugin.Content.PauseCommand.Execute(new object());
        }
        public void Stop()
        {
            _viewModel.State.MediaPlugin.Content.StopCommand.Execute(new object());
        }
        #endregion

    }

    public class VrPlayerCommander
    {
        public static ControlPanel ControlPanel { get; set; }

        public static void Play()
        {
            ControlPanel.Play();
        }
        public static void Pause()
        {
            ControlPanel.Pause();
        }
        public static void Stop()
        {
            ControlPanel.Stop();
        }
    }
}
