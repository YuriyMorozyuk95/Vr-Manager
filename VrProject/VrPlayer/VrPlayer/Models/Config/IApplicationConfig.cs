using System.ComponentModel;
using System.Windows.Input;

namespace VrPlayer.Models.Config
{
    public interface IApplicationConfig : INotifyPropertyChanged
    {
        string DefaultMediaFile { get; set; }
        int CameraFieldOfView { get; set; }
        int ViewportsHorizontalOffset { get; set; }
        int ViewportsVerticalOffset { get; set; }
        string SamplesFolder { get; set; }
        double NeckHeight { get; set; }
        bool ReadSideCarPresets { get; set; }

        string DefaultMedia { get; set; }
        string DefaultEffect { get; set; }
        string DefaultDistortion { get; set; }
        string DefaultProjection { get; set; }
        string DefaultTracker { get; set; }
        string DefaultStabilizer { get; set; }

        //Todo: extract keys
        Key KeyPlayPause { get; set; }
        Key KeyStop { get; set; }
        Key KeyNext { get; set; }
        Key KeyPrevious { get; set; }
        Key KeyLoop { get; set; }
        Key KeyMoveLeft { get; set; }
        Key KeyMoveRight { get; set; }
        Key KeyMoveForward { get; set; }
        Key KeyMoveBackward { get; set; }
        Key KeyMoveUp { get; set; }
        Key KeyMoveDown { get; set; }
        Key KeyTrackerCalibrate { get; set; }
        Key KeyTrackerReset { get; set; }
        Key KeyFieldOfViewMinus { get; set; }
        Key KeyFieldOfViewPlus { get; set; }
        Key KeyHorizontalOffsetMinus { get; set; }
        Key KeyHorizontalOffsetPlus { get; set; }
        Key KeyVerticalOffsetMinus { get; set; }
        Key KeyVerticalOffsetPlus { get; set; }
    }
}