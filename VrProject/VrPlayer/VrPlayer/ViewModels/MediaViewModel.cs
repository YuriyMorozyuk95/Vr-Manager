using VrPlayer.Helpers.Mvvm;
using VrPlayer.Models.State;

namespace VrPlayer.ViewModels
{
	public class MediaViewModel: ViewModelBase
	{
        private readonly IApplicationState _state;
        public IApplicationState State
        {
            get { return _state; }
        }

        public MediaViewModel(IApplicationState state)
        {
            _state = state;
		}
    }
}
