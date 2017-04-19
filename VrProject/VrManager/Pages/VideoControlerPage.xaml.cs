using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VrManager.Data.Entity;
using VrManager.Controls;
using VrManager.Service;
using VrManager.Helpers;
using System.Diagnostics;
using ComPortPackages.Console;
using System.Windows.Threading;
using System.Threading;
using VrManager.Data.Abstract;

namespace VrManager.Pages
{
    /// <summary>
    /// Interaction logic for VideoControlerPage.xaml
    /// </summary>
    public partial class VideoControlerPage : Page
    {
        private StatusVideoEnum _statusVideo;
        private StatusGameEnum _statusGame;
        private ClientService _service;
        private OpenVrPleyerHelper _videoProcessor;
        private ModelVideo _currentVideo;
        private ModelGame _currentGame;
        private DispatcherTimer _timerEnd;
        private DispatcherTimer _timerStart;
        private DispatcherTimer _timerShift;
        private TimeSpan _timeOut;
        private TimeSpan _timeStart;
        private TimeSpan? _timeShift;
        private ModelObserve _observer = new ModelObserve() { User = App.AuthorizedUser };

        private Action Launch;
        private Action Start;
        private Action Stop;
        private Action Pause;
        private GameProcessor _gameProcessor;
        private TypeItem _typeItem;
        private bool _firstExecuteStart;
        private bool _stopWithTimer;
        private Stopwatch _watcher = new Stopwatch();
        private static object obj = new object();

        public VideoControlerPage()
        {

        }

        public VideoControlerPage(ModelVideo video) : this()
        {
            InitializeComponent();
            _currentVideo = video;

            VideoBanner.VideoCollection = new List<Uri>()
            {
                new Uri(video.PathToBannerVideo)
            };

            _typeItem = TypeItem.Video360;
            Start = ResumeVideo;
            Stop = StopVideo;
            Pause = PauseVideo;
            Launch = LaunchingVideo;

            Btn_Start.IsEnabled = false;
            Btn_Stop.IsEnabled = false;
            Btn_Pause.IsEnabled = false;

            NameVideo.Text = video.Name;

            if (video.TimeOut != null)
            {
                Hour.Text = video.TimeOut.Value.Hour.ToString();
                Minutes.Text = video.TimeOut.Value.Minute.ToString();
                Second.Text = video.TimeOut.Value.Second.ToString();
            }
            else
            {
                TimerPanel.Visibility = Visibility.Hidden;
            }

            _videoProcessor = new OpenVrPleyerHelper(video);
            _timeOut = video.TimeOut.Value.TimeOfDay;
            if (video.MonitorNumber != null)
            {
                _videoProcessor.MonitorNumber = video.MonitorNumber.Value;

            }
            else
            {
                _videoProcessor.MonitorNumber = 2;
            }
            _service = new ClientService();


            _timerEnd = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, 1)
            };
            _timerEnd.Tick += _timerStop_Tick;

            App.IsVideoMod = true;
            Observer.SetItem(_videoProcessor);
        }
        public VideoControlerPage(ModelGame game) : this()
        {
            InitializeComponent();

            _typeItem = TypeItem.Game;
            _currentGame = game;
            Start = StartGame;
            Stop = StopGame;
            Launch = LaunchingGame;

            try
            {
                VideoBanner.VideoCollection = new List<Uri>()
                {
                    new Uri(game.PathToBannerVideo)
                };
            }
            catch
            {

            }

            KeySimulationHelper.CurrentGame = _currentGame;
            KeySimulationHelper.ButtonStart = _currentGame.RunKey.Value;

            Btn_Start.IsEnabled = false;
            Btn_Stop.IsEnabled = false;
            Btn_Pause.Visibility = Visibility.Collapsed;
            Btn_Play.Icon = Char.ConvertFromUtf32(0xE7FC);
            Btn_Play.Text = "Запустить игру";

            NameVideo.Text = _currentGame.Name;
            KeySimulationHelper.ButtonAdditional = _currentGame.AdditionalKey;
            //visibly timer on
            // GameStartTimer.Visibility = Visibility.Collapsed;
            TimerPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
            TitleTimerOff.Text = "Обратный отсчет выключения игры";
            _timeOut = _currentGame.TimeOut.Value.TimeOfDay;

            if (_timeOut != null)
            {
                Hour.Text = _timeOut.Hours.ToString();
                Minutes.Text = _timeOut.Minutes.ToString();
                Second.Text = _timeOut.Seconds.ToString();
            }
            else
            {
                TimerPanel.Visibility = Visibility.Hidden;
            }

            _gameProcessor = new GameProcessor(_currentGame);

            // GameStartTimer.Visibility = Visibility.Visible;
            _timeStart = TimeSpan.FromSeconds(game.StartTime.Value);

            _timerStart = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            _timerStart.Tick += _timerStartGame_Tick;

            _timerEnd = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, 1)
            };

            _timerEnd.Tick += _timerStop_Tick;

            // StarGame_Hour.Text = _timeStart.Hours.ToString();
            //  StarGame_Minutes.Text = _timeStart.Minutes.ToString();
            // StarGame_Second.Text = _timeStart.Seconds.ToString();

            if (_gameProcessor.Game.ShiftPressTime != null)
            {
                _timeShift = TimeSpan.FromSeconds(_gameProcessor.Game.ShiftPressTime.Value);
                _timerShift = new DispatcherTimer()
                {
                    Interval = TimeSpan.FromSeconds(1)
                };

                _timerShift.Tick += _timerShift_Tick;
            }

            if (_timeShift != null)
            {
                //    TimerShift.Visibility = Visibility.Visible;
                //   ShiftTime_Hour.Text = _timeShift.Value.Hours.ToString();
                //  ShiftTime_Minutes.Text = _timeShift.Value.Minutes.ToString();
                //   ShiftTime_Second.Text = _timeShift.Value.Seconds.ToString();
            }

            App.IsVideoMod = false;
            Observer.SetItem(_gameProcessor);
        }

        #region ControllerItemFunction


        public void LaunchingGame()
        {
            _timeStart = TimeSpan.FromSeconds(_currentGame.StartTime.Value);
            _timerStart.Start();
        }
        public void LunchAndClickGame()
        {

            _gameProcessor.LaunchProcess();
            _gameProcessor.WaitForProgram();

            if (_gameProcessor.Game.TypeStartFocus == TypeStartFocus.FocusToMainWnd)
            {
                WinAPI.ShowWindow(WinAPI.FindWindow(null, _gameProcessor.NameProcess), 22);
                Thread.Sleep(3000);
            }

            MonitorManager.MoveWindow(_currentGame.NameProcess, App.Setting.NumberLeftMonitor);

            if (_gameProcessor.Game.TypeStartFocus == TypeStartFocus.FocusToMainWnd)
            {
                KeySimulationHelper.SwitchToMainWindow();
            }

            Btn_Start.IsEnabled = true;
            Observer.StartObserv();
            _statusGame = StatusGameEnum.Lunched;
        }
        public void StartGame()
        {
            if (_statusGame == StatusGameEnum.Lunched)
            {
                KeySimulationHelper.Simulation(_gameProcessor);
                ComPrortSender.Send(_currentGame.FileMotion, Terminal, App.Current);

                _timerEnd.Start();

                if (_gameProcessor.Game.ShiftPressTime != null)
                {
                    _timerShift.Start();
                }
                _statusGame = StatusGameEnum.Started;
            }
            else
            {
                MessageBox.Show("Запустите сначала игру");
            }
        }
        private void ExecuteShiftClick()
        {
            KeySimulationHelper.ShiftClick(_gameProcessor);
        }
        public void StopGame()
        {
            try
            {
                if (_statusGame == StatusGameEnum.Started || _statusGame == StatusGameEnum.Lunched)
                {
                    _gameProcessor.StopProcess();
                    ComPortPackagesService.Stop();
                    Thread.Sleep(250);
                    ComPrortSender.StartMoutionPosition();
                    _gameProcessor.Dispose();
                }
                else
                {
                    MessageBox.Show("Игра не запущена");
                }


            }
            catch (ObjectDisposedException e)
            {

            }
        }

        public void LaunchingVideo()
        {
            _statusVideo = StatusVideoEnum.Lunched;
            _videoProcessor.LaunchProcess();
            Btn_Start.IsEnabled = true;
            Btn_Stop.IsEnabled = true;
           // Btn_Play.IsEnabled = true;
        }
        public void PauseVideo()
        {
            if (_statusVideo == StatusVideoEnum.Started)
            {
                _service.Pause();
                VideoBanner.PausePlayer();
                ComPortPackagesService.Pause();
                _timerEnd.Stop();
                Thread.Sleep(300);
                _statusVideo = StatusVideoEnum.Pause;
            }
            else
            {
                MessageBox.Show("Видео не началось производиться");
            }
        }
        public void ResumeVideo()
        {
            try
            {
                if (_statusVideo == StatusVideoEnum.Lunched)
                {
                    _service.Play();
                    ComPrortSender.Send(_currentVideo.FileMotion, Terminal, App.Current);
                    _timerEnd.Start();
                }
                else
                {
                    ComPortPackagesService.Resume();
                    _timerEnd.Start();
                    _service.Play();
                    VideoBanner.StartPlayer();
                }

                Observer.StartObserv();
                _statusVideo = StatusVideoEnum.Started;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Video not runned please wait. | {ex.Message}");
            }
        }
        public void StopVideo()
        {
            try
            {
                _timerEnd.Stop();
                _videoProcessor.StopProcess();

                if (_statusVideo == StatusVideoEnum.Started || _statusVideo == StatusVideoEnum.Pause)
                {
                    ComPortPackagesService.Stop();
                    Thread.Sleep(250);
                    ComPrortSender.StartMoutionPosition();
                }
                _service.Stop();
                _videoProcessor.Dispose();
                _statusVideo = StatusVideoEnum.NotLunched;

            }
            catch (Exception ex)
            {

            }

        }
        #endregion

        #region ButtonEventClick

        private void Btn_OneVisitor_Click(object sender, RoutedEventArgs e)
        {
            Btn_Play.IsEnabled = true;
            Btn_OneVisitor.IsEnabled = false;
            Btn_TwoVisitor.IsEnabled = false;
            SetObserverName();

        }
        private void SetObserverName()
        {
            _observer.PC = PC.PC1;
            BaseContentEntity item = _currentVideo ?? (BaseContentEntity)_currentGame;
            _observer.Name = item.Name;
            _observer.TypeItem = _typeItem;
        }
        private void Btn_Launch_Click(object sender, RoutedEventArgs e)
        {
            Btn_Play.IsEnabled = false;
            if (_typeItem != TypeItem.Game)
            {
                Btn_Start.IsEnabled = true;
            }
            App.LaunchMedia = null;
            App.StartMedia = null;
            App.PauseMedia = null;
            App.StopMedia = null;
            //App.MainWnd.FirstExecuteStart = true;
            //App.MainWnd.firstExecutePause = true;
            
            App.LaunchMedia += Btn_Launch_Click;
            App.StartMedia += Btn_Start_Click;
            App.PauseMedia += Btn_Pause_Click;
            App.StopMedia += Btn_Stop_Click;
            Launch();
            Thread.Sleep(250);
            Observer.ObservIteration();
            Btn_Play.IsEnabled = false;

        }
        private void Btn_Start_Click(object sender, RoutedEventArgs e)
        {

            if (_statusVideo == StatusVideoEnum.Pause || _statusVideo == StatusVideoEnum.Lunched || _statusGame == StatusGameEnum.Lunched)
            {
                _watcher.Start();
                Start();
                Btn_Pause.IsEnabled = true;
                Btn_Start.IsEnabled = false;

                if (_typeItem != TypeItem.Game)
                {
                    Observer.ObservIteration();
                }

                SetObserverStart();

                if (VideoBanner.VideoCollection != null)
                {
                    VideoBanner.Visibility = Visibility.Visible;
                    Btn_Play.Visibility = Visibility.Collapsed;
                    PC_Bar.Visibility = Visibility.Collapsed;
                    VideoBanner.StartPlayer();
                }
            }
            else
            {
                //System.Windows.MessageBox.Show("Запустите видео или игру");
            }

        }
        private void SetObserverStart()
        {
            if (_firstExecuteStart)
            {
                _observer.TimeStart = DateTime.Now;
                _firstExecuteStart = false;
            }

            if (_observer.PressStartCouner != null)
            {
                _observer.PressStartCouner++;
            }
            else
            {
                _observer.PressStartCouner = 1;
            }
        }
        private void Btn_Pause_Click(object sender, RoutedEventArgs e)
        {
            if (_statusVideo == StatusVideoEnum.Started)
            {
                Pause();
                _statusVideo = StatusVideoEnum.Pause;
                Thread.Sleep(500);
                Btn_Pause.IsEnabled = false;
                Btn_Start.IsEnabled = true;
                SetObservPause();
                Observer.ObservIteration();
            }
            else
            {
                //System.Windows.MessageBox.Show("Видео не запушено");
            }


        }
        private void SetObservPause()
        {
            _observer.TimePause = DateTime.Now;
            _watcher.Stop();
            if (_observer.PressPauseCouner != null)
            {
                _observer.PressPauseCouner++;
            }
            else
            {
                _observer.PressPauseCouner = 1;
            }
        }
        private void Btn_Stop_Click(object sender, RoutedEventArgs e)
        {
            _observer.TimeStop = DateTime.Now;
            _watcher.Stop();
            _observer.Duration = new DateTime(2000, 01, 01, 0, 0, 0, 0) + _watcher.Elapsed;

            if (_stopWithTimer)
            {
                _observer.Halted = Halted.AfterTime;
            }
            else
            {
                _observer.Halted = Halted.Manual;
            }

            App.Frame.Navigate(new StartUpPage());
        }

        #endregion

        #region LoadAndUnload

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            App.MainWnd.StartOrPause = true;

            App.LaunchMedia -= Btn_Launch_Click;
            App.StartMedia -= Btn_Start_Click;
            App.PauseMedia -= Btn_Pause_Click;
            App.StopMedia -= Btn_Stop_Click;
            try
            {
                ObserverUserActivity.StartActivityObserv();

                Action<DispatcherTimer> StopTimer = (t) =>
                {
                    if (t != null)
                    {
                        t.Stop();
                    }
                };

                Stop();

                StopTimer(_timerStart);
                StopTimer(_timerShift);
                StopTimer(_timerEnd);

                MonitorManager.RestartPositionWindows();
                App.LockDisplayWindow.Topmost = true;

                Observer.EndObserv();
                App.Repository.AddObserver(_observer);
            }
            catch (Exception ex)
            {
                App.Logger.Error(DateTime.Now.ToString() + " In Page_Unloaded VideoControllerPage" + ex.Message);
            }
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            App.MainWnd.ChangeTitle(Title);
            App.MainWnd.StartOrPause = true;

            try
            {
                App.StopMedia -= Btn_Stop_Click;
                App.StartMedia -= Btn_Start_Click;
                App.PauseMedia -= Btn_Pause_Click;

                Btn_OneVisitor.IsEnabled = true;
                Btn_TwoVisitor.IsEnabled = true;
                _firstExecuteStart = true;
                App.LockDisplayWindow.Topmost = false;

                ObserverUserActivity.EndActivityObserv();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Подключите второй монитор");
            }
        }

        #endregion

        #region TickEvents

        private void _timerStartGame_Tick(object sender, EventArgs e)
        {
            if (_timeStart == TimeSpan.Zero)
            {
                _timerStart.Stop();
                LunchAndClickGame();

                Thread.Sleep(200);
                Btn_Stop.IsEnabled = true;
                Btn_Play.IsEnabled = true;
                return;
            }

            _timeStart -= TimeSpan.FromSeconds(1);
            //StarGame_Hour.Text = _timeStart.Hours.ToString();
            //StarGame_Minutes.Text = _timeStart.Minutes.ToString();
            //StarGame_Second.Text = _timeStart.Seconds.ToString();


        }
        private void _timerStop_Tick(object sender, EventArgs e)
        {
            try
            {
                if (_timeOut == TimeSpan.Zero)
                {
                    _timerEnd.Stop();
                    _stopWithTimer = true;
                    Btn_Stop_Click(null, null);

                    return;
                }

                _timeOut -= TimeSpan.FromSeconds(1);
                Hour.Text = _timeOut.Hours.ToString();
                Minutes.Text = _timeOut.Minutes.ToString();
                Second.Text = _timeOut.Seconds.ToString();
            }
            catch
            {

            }
        }
        private void _timerShift_Tick(object sender, EventArgs e)
        {
            if (_timeShift == TimeSpan.Zero)
            {
                _timerShift.Stop();
                ExecuteShiftClick();
                return;
            }
            _timeShift -= TimeSpan.FromSeconds(1);

            //ShiftTime_Hour.Text = _timeShift.Value.Hours.ToString();
            //ShiftTime_Minutes.Text = _timeShift.Value.Minutes.ToString();
            //ShiftTime_Second.Text = _timeShift.Value.Seconds.ToString();

        }



        #endregion

        private void Btn_TwoVisitor_Click(object sender, RoutedEventArgs e)
        {
            Btn_OneVisitor_Click(null, null);
            _observer.PC = PC.PC2;
        }
    }
    public enum StatusVideoEnum
    {
        Lunched,
        NotLunched,
        Started,
        Pause
    }
    public enum StatusGameEnum
    {
        NotLunched,
        Lunched,
        Started,
        Work
    }
}
