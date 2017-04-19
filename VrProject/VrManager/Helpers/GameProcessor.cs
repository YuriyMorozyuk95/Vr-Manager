using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using VrManager.Data.Abstract;
using VrManager.Data.Entity;

namespace VrManager.Helpers
{
    public class GameProcessor : ProcessorHelper
    {
        public ModelGame Game;

        public string PathToVideo { get; set; }

        public GameProcessor(ModelGame game)
        {
            Game = game;
            NameProcess = Game.NameProcess;
            MouseHelper.X = (int)Game.MouseClickCordX;
            MouseHelper.Y = (int)Game.MouseClickCordY;
            ProcessStartInfo proc = new ProcessStartInfo();
           // proc.UseShellExecute = true;
            proc.Arguments = Game.StartUpParams;
            proc.WorkingDirectory = new DirectoryInfo(Game.ItemPath).Parent.FullName.ToString();
            proc.FileName = new DirectoryInfo(Game.ItemPath).Name;
            proc.Verb = "runas";

            _currentProcess = new Process()
            {
                StartInfo = proc
            };
        }
      
        public override void LaunchProcess()
        {
            App.MainWnd.Topmost = false;

            if (!IsProcessOpened) 
            {
                _currentProcess.Start();
                _currentProcess.WaitForInputIdle();

                Thread.Sleep(500); // Wait for open window config
                if (Game.MouseClickCordX != 0 || Game.MouseClickCordY != 0)          
                {
                   MouseHelper.Click();
                }

                base.LaunchProcess();
            }

             
        }
        public void WaitForProgram()
        {
            _currentProcess.WaitForInputIdle();
        }
       
    }
}
