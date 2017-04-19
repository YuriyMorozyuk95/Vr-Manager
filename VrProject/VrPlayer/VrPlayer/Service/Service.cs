using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VrPlayer.Views;
using VrManager.Data.Abstract;
using System.Windows;

namespace VrPlayer.Service
{
    class Service : IRemoteVideoCommand
    {
        public void ChangeSize()
        {
            App.MainWindowPlayer.WindowState = System.Windows.WindowState.Normal;
            App.MainWindowPlayer.WindowState = System.Windows.WindowState.Maximized;
        }


        public void ChangeToampostMode(bool mode)
        {
            App.MainWindowPlayer.Topmost = mode;
        }

        public void Pause()
        {
            VrPlayerCommander.Pause();
        }
        public void Play()
        {
           // MessageBox.Show("Is play");
            VrPlayerCommander.Play();
        }
        public void Stop()
        {
            VrPlayerCommander.Stop();
        }
    }
}
