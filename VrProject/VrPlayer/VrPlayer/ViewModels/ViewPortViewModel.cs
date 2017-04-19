using System.Windows.Input;
using VrPlayer.Helpers.Mvvm;
using VrPlayer.Models.Config;
using VrPlayer.Models.State;

namespace VrPlayer.ViewModels
{
    public class ViewPortViewModel: ViewModelBase
	{
        private readonly IApplicationState _state;
        public IApplicationState State
        {
            get { return _state; }
        }

        private readonly IApplicationConfig _config;
        public IApplicationConfig Config
        {
            get { return _config; }
        }

        #region Commands

        private readonly ICommand _toggleNavigationCommand;
        public ICommand ToggleNavigationCommand
        {
            get { return _toggleNavigationCommand; }
        }

        #endregion
        
        public ViewPortViewModel(IApplicationState state, IApplicationConfig config)
        {
            _state = state;
            _config = config;

            //Commands
            _toggleNavigationCommand = new RelayCommand(ToggleNavigation);
            
            State.PropertyChanged += State_PropertyChanged;
            State.StereoOutput = State.StereoOutput;//Force refresh
        }

        #region Logic

        private void ToggleNavigation(object o)
        {
            if (_state.TrackerPlugin == null || _state.TrackerPlugin.Content == null)
                return;

            _state.TrackerPlugin.Content.IsActive = !_state.TrackerPlugin.Content.IsActive;
        }

        #endregion

        #region Layouts

        void State_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "StereoOutput")
            {
                switch (_state.StereoOutput)
                {
                    case LayoutMode.MonoLeft:
                    case LayoutMode.MonoRight:
                    case LayoutMode.DualScreen:
                        Viewport1Col = 0;
                        Viewport1ColSpan = 2;
                        Viewport1Row = 0;
                        Viewport1RowSpan = 2;
                        Viewport2Col = 0;
                        Viewport2ColSpan = 2;
                        Viewport2Row = 0;
                        Viewport2RowSpan = 2;
                        break;
                    case LayoutMode.OverUnder:
                        Viewport1Col = 0;
                        Viewport1ColSpan = 2;
                        Viewport1Row = 0;
                        Viewport1RowSpan = 1;
                        Viewport2Col = 0;
                        Viewport2ColSpan = 2;
                        Viewport2Row = 1;
                        Viewport2RowSpan = 1;
                        break;
                    case LayoutMode.SideBySide:
                        Viewport1Col = 0;
                        Viewport1ColSpan = 1;
                        Viewport1Row = 0;
                        Viewport1RowSpan = 2;
                        Viewport2Col = 1;
                        Viewport2ColSpan = 1;
                        Viewport2Row = 0;
                        Viewport2RowSpan = 2;
                        break;
                    case LayoutMode.CrossEyed:
                        Viewport1Col = 1;
                        Viewport1ColSpan = 1;
                        Viewport1Row = 0;
                        Viewport1RowSpan = 2;
                        Viewport2Col = 0;
                        Viewport2ColSpan = 1;
                        Viewport2Row = 0;
                        Viewport2RowSpan = 2;
                        break;
                }
            }
        }

        private int _viewport1Col;
        public int Viewport1Col
        {
            get { return _viewport1Col; }
            set
            {
                _viewport1Col = value;
                OnPropertyChanged("Viewport1Col");
            }
        }

        private int _viewport1ColSpan;
        public int Viewport1ColSpan
        {
            get { return _viewport1ColSpan; }
            set
            {
                _viewport1ColSpan = value;
                OnPropertyChanged("Viewport1ColSpan");
            }
        }

        private int _viewport1Row;
        public int Viewport1Row
        {
            get { return _viewport1Row; }
            set
            {
                _viewport1Row = value;
                OnPropertyChanged("Viewport1Row");
            }
        }

        private int _viewport1RowSpan;
        public int Viewport1RowSpan
        {
            get { return _viewport1RowSpan; }
            set
            {
                _viewport1RowSpan = value;
                OnPropertyChanged("Viewport1RowSpan");
            }
        }

        private int _viewport2Col;
        public int Viewport2Col
        {
            get { return _viewport2Col; }
            set
            {
                _viewport2Col = value;
                OnPropertyChanged("Viewport2Col");
            }
        }

        private int _viewport2ColSpan;
        public int Viewport2ColSpan
        {
            get { return _viewport2ColSpan; }
            set
            {
                _viewport2ColSpan = value;
                OnPropertyChanged("Viewport2ColSpan");
            }
        }

        private int _viewport2Row;
        public int Viewport2Row
        {
            get { return _viewport2Row; }
            set
            {
                _viewport2Row = value;
                OnPropertyChanged("Viewport2Row");
            }
        }

        private int _viewport2RowSpan;
        public int Viewport2RowSpan
        {
            get { return _viewport2RowSpan; }
            set
            {
                _viewport2RowSpan = value;
                OnPropertyChanged("Viewport2RowSpan");
            }
        }

        #endregion
    }
}
