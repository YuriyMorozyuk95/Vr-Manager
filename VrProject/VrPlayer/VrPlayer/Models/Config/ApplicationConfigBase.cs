using System.Windows.Input;
using VrPlayer.Helpers.Mvvm;

namespace VrPlayer.Models.Config
{
    public abstract class ApplicationConfigBase : ViewModelBase, IApplicationConfig
    {
        protected ApplicationConfigBase()
        {
            KeyPlayPause = Key.Space;
            KeyStop = Key.S;
            KeyPrevious = Key.A;
            KeyNext = Key.D;
            KeyLoop = Key.F;
            KeyMoveLeft = Key.Left;
            KeyMoveRight = Key.Right;
            KeyMoveForward = Key.Up;
            KeyMoveBackward = Key.Down;
            KeyMoveUp = Key.PageUp;
            KeyMoveDown = Key.PageDown;
            KeyTrackerCalibrate = Key.F1;
            KeyTrackerReset = Key.F2;
            KeyFieldOfViewMinus = Key.O;
            KeyFieldOfViewPlus = Key.P;
            KeyHorizontalOffsetMinus = Key.K;
            KeyHorizontalOffsetPlus = Key.L;
            KeyVerticalOffsetMinus = Key.N;
            KeyVerticalOffsetPlus = Key.M;
        }

        private string _defaultMediaFile;
        public string DefaultMediaFile
        {
            get { return _defaultMediaFile; }
            set
            {
                _defaultMediaFile = value;
                OnPropertyChanged("DefaultMediaFile");
            }
        }

        private string _samplesFolder;
        public string SamplesFolder
        {
            get { return _samplesFolder; }
            set 
            { 
                _samplesFolder = value;
                OnPropertyChanged("SamplesFolder");
            }
        }
        
        private int _cameraFieldOfView;
        public int CameraFieldOfView
        {
            get { return _cameraFieldOfView; }
            set
            {
                _cameraFieldOfView = value;
                OnPropertyChanged("CameraFieldOfView");
            }
        }

        private int _viewportsHorizontalOffset;
        public int ViewportsHorizontalOffset
        {
            get { return _viewportsHorizontalOffset; }
            set
            {
                _viewportsHorizontalOffset = value;
                OnPropertyChanged("ViewportsHorizontalOffset");
            }
        }

        private int _viewportsVerticalOffset;
        public int ViewportsVerticalOffset
        {
            get { return _viewportsVerticalOffset; }
            set
            {
                _viewportsVerticalOffset = value;
                OnPropertyChanged("ViewportsVerticalOffset");
            }
        }

        private double _neckHeight;
        public double NeckHeight
        {
            get { return _neckHeight; }
            set
            {
                _neckHeight = value;
                OnPropertyChanged("NeckHeight");
            }
        }

        private bool _readSideCarPresets;
        public bool ReadSideCarPresets
        {
            get { return _readSideCarPresets; }
            set
            {
                _readSideCarPresets = value;
                OnPropertyChanged("ReadSideCarPresets");
            }
        }

        private string _defaultMedia;
        public string DefaultMedia
        {
            get { return _defaultMedia; }
            set
            {
                _defaultMedia = value;
                OnPropertyChanged("DefaultMedia");
            }
        }

        private string _defaultEffect;
        public string DefaultEffect
        {
            get { return _defaultEffect; }
            set
            {
                _defaultEffect = value;
                OnPropertyChanged("DefaultEffect");
            }
        }

        private string _defaultDistortion;
        public string DefaultDistortion
        {
            get { return _defaultDistortion; }
            set
            {
                _defaultDistortion = value;
                OnPropertyChanged("DefaultDistortion");
            }
        }

        private string _defaultProjection;
        public string DefaultProjection
        {
            get { return _defaultProjection; }
            set
            {
                _defaultProjection = value;
                OnPropertyChanged("DefaultProjection");
            }
        }
        
        private string _defaultTracker;
        public string DefaultTracker
        {
            get { return _defaultTracker; }
            set
            {
                _defaultTracker = value;
                OnPropertyChanged("DefaultTracker");
            }
        }

        private string _defaultStabilizer;
        public string DefaultStabilizer
        {
            get { return _defaultStabilizer; }
            set
            {
                _defaultStabilizer = value;
                OnPropertyChanged("DefaultStabilizer");
            }
        }

        #region Keys

        private Key _keyPlayPause;
        public Key KeyPlayPause
        {
            get { return _keyPlayPause; }
            set
            {
                _keyPlayPause = value;
                OnPropertyChanged("Keys");
            }
        }

        private Key _keyStop;
        public Key KeyStop
        {
            get { return _keyStop; }
            set
            {
                _keyStop = value;
                OnPropertyChanged("Keys");
            }
        }

        private Key _keyNext;
        public Key KeyNext
        {
            get { return _keyNext; }
            set
            {
                _keyNext = value;
                OnPropertyChanged("Keys");
            }
        }

        private Key _keyPrevious;
        public Key KeyPrevious
        {
            get { return _keyPrevious; }
            set
            {
                _keyPrevious = value;
                OnPropertyChanged("Keys");
            }
        }

        private Key _keyLoop;
        public Key KeyLoop
        {
            get { return _keyLoop; }
            set
            {
                _keyLoop = value;
                OnPropertyChanged("Keys");
            }
        }

        private Key _keyMoveLeft;
        public Key KeyMoveLeft
        {
            get { return _keyMoveLeft; }
            set
            {
                _keyMoveLeft = value;
                OnPropertyChanged("Keys");
            }
        }

        private Key _keyMoveRight;
        public Key KeyMoveRight
        {
            get { return _keyMoveRight; }
            set
            {
                _keyMoveRight = value;
                OnPropertyChanged("Keys");
            }
        }

        private Key _keyMoveForward;
         public Key KeyMoveForward
        {
            get { return _keyMoveForward; }
            set
            {
                _keyMoveForward = value;
                OnPropertyChanged("Keys");
            }
        }

         private Key _keyMoveBackward;
         public Key KeyMoveBackward
        {
            get { return _keyMoveBackward; }
            set
            {
                _keyMoveBackward = value;
                OnPropertyChanged("Keys");
            }
        }

        private Key _keyMoveUp;
        public Key KeyMoveUp
        {
            get { return _keyMoveUp; }
            set
            {
                _keyMoveUp = value;
                OnPropertyChanged("Keys");
            }
        }

        private Key _keyMoveDown;
        public Key KeyMoveDown
        {
            get { return _keyMoveDown; }
            set
            {
                _keyMoveDown = value;
                OnPropertyChanged("Keys");
            }
        }

        private Key _keyTrackerCalibrate;
        public Key KeyTrackerCalibrate
        {
            get { return _keyTrackerCalibrate; }
            set
            {
                _keyTrackerCalibrate = value;
                OnPropertyChanged("Keys");
            }
        }

        private Key _keyTrackerReset;
        public Key KeyTrackerReset
        {
            get { return _keyTrackerReset; }
            set
            {
                _keyTrackerReset = value;
                OnPropertyChanged("Keys");
            }
        }

        private Key _keyFieldOfViewMinus;
        public Key KeyFieldOfViewMinus
        {
            get { return _keyFieldOfViewMinus; }
            set
            {
                _keyFieldOfViewMinus = value;
                OnPropertyChanged("Keys");
            }
        }

        private Key _keyFieldOfViewPlus;
        public Key KeyFieldOfViewPlus
        {
            get { return _keyFieldOfViewPlus; }
            set
            {
                _keyFieldOfViewPlus = value;
                OnPropertyChanged("Keys");
            }
        }

        private Key _keyHorizontalOffsetMinus;
        public Key KeyHorizontalOffsetMinus
        {
            get { return _keyHorizontalOffsetMinus; }
            set
            {
                _keyHorizontalOffsetMinus = value;
                OnPropertyChanged("Keys");
            }
        }

        private Key _keyHorizontalOffsetPlus;
        public Key KeyHorizontalOffsetPlus
        {
            get { return _keyHorizontalOffsetPlus; }
            set
            {
                _keyHorizontalOffsetPlus = value;
                OnPropertyChanged("Keys");
            }
        }

        private Key _keyVerticalOffsetMinus;
        public Key KeyVerticalOffsetMinus
        {
            get { return _keyVerticalOffsetMinus; }
            set
            {
                _keyVerticalOffsetMinus = value;
                OnPropertyChanged("Keys");
            }
        }

        private Key _keyVerticalOffsetPlus;
        public Key KeyVerticalOffsetPlus
        {
            get { return _keyVerticalOffsetPlus; }
            set
            {
                _keyVerticalOffsetPlus = value;
                OnPropertyChanged("Keys");
            }
        }

        #endregion

    }
}
