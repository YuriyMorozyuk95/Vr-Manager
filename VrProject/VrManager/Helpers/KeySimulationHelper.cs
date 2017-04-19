using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using VrManager.Data.Abstract;
using VrManager.Data.Entity;

namespace VrManager.Helpers
{
    public class KeySimulationHelper
    {
        private static string _stringButtonStart;
        private static string _stringButtonAdditional;
        public static Key _buttonStart;


        public static Key ButtonStart
        {
            get
            {
                return _buttonStart;
            }
            set
            {
                _buttonStart = value;
                if(_buttonStart == Key.Space)
                {
                    _stringButtonStart = " ";
                }
                if(_buttonStart == Key.Return)
                {
                    _stringButtonStart = "{ENTER}";
                }
            }
        }
        public static ModelGame CurrentGame { get; set; }
        public static Key? ButtonAdditional
        {
            set
            {
                if (value != null)
                {
                    _stringButtonAdditional = GetStringButton(value.Value);
                }
            }
        }

        public static string GetStringButton(Key button)
        {
            string buttonString = "";

            if(button.ToString().Count() == 1)
            {
                return button.ToString();
            }

            switch (button)
            {
                case Key.Back:
                        buttonString = "{BACKSPACE}";
                    break;
                case Key.Pause:
                    buttonString = "{BREAK}";
                    break;
                case Key.CapsLock:
                    buttonString = "{CAPSLOCK}";
                    break;
                case Key.Return:
                    buttonString = "{ENTER}";
                    break;
                case Key.Escape:
                    buttonString = "{ESC}";
                    break;
                case Key.System:
                    buttonString = "{F10}";
                    break;
                case Key.Space:
                    buttonString = " ";
                    break;
                case Key.Next:
                    buttonString = "{PGDN}";
                    break;
                case Key.PageUp:
                    buttonString = "{PGUP}"; 
                    break;
                case Key.Snapshot:
                    buttonString = "{PRTSC}"; 
                    break;
                case Key.LeftShift:
                    buttonString = "+";
                    break;
                case Key.RightShift:
                    buttonString = "+";
                    break;

                default:
                    buttonString = "{" + button.ToString().ToUpper() + "}";
                    break;
            }
            return buttonString;
        }

        public static void Simulation(GameProcessor gameProcessor)
        {
            IntPtr gameHandl = WinAPI.FindWindow(null, gameProcessor.Game.NameProcess);

            if (gameHandl == IntPtr.Zero)
            {
                return;
            }

            
            WinAPI.SetForegroundWindow(gameHandl);
            try
            {
                SendKeys.SendWait(_stringButtonStart);
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }


            if (_stringButtonAdditional != null || _stringButtonAdditional != string.Empty) 
            {
                Thread.Sleep(1000);
                SendKeys.SendWait(_stringButtonAdditional);
            }

            if (CurrentGame.TypeStartFocus == TypeStartFocus.FocusToMainWnd)
            {
                SwitchToMainWindow();
            }
            else if (gameProcessor.Game.TypeStartFocus == TypeStartFocus.FocusedInFullScreen)
            {
                WinAPI.ShowWindow(gameHandl, 3);
            }

        }

        public static void SwitchToMainWindow()
        {
            IntPtr calculatorHandle = WinAPI.FindWindow(null, "VrManager");
            WinAPI.SwitchToThisWindow(calculatorHandle, true);
        }

        internal static void ShiftClick(GameProcessor gameProcessor)
        {
            IntPtr gameHandl = WinAPI.FindWindow(null, gameProcessor.Game.NameProcess);

            if (gameHandl == IntPtr.Zero)
            {
                return;
            }

            WinAPI.SendMessage(gameHandl, 0x100, (int)Key.RightShift,1);
            App.Logger.Error("Key Shift is preesed");

            if (CurrentGame.TypeStartFocus == TypeStartFocus.FocusToMainWnd)
            {
                SwitchToMainWindow();
            }

        }
    }
}
