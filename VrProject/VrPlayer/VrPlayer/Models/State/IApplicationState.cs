using System.ComponentModel;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Distortions;
using VrPlayer.Contracts.Effects;
using VrPlayer.Contracts.Medias;
using VrPlayer.Contracts.Stabilizers;
using VrPlayer.Contracts.Trackers;
using VrPlayer.Contracts.Projections;
using VrPlayer.Models.Settings;

namespace VrPlayer.Models.State
{
    public interface IApplicationState : INotifyPropertyChanged
    {
        IPlugin<IMedia> MediaPlugin { get; set; }
        IPlugin<EffectBase> EffectPlugin { get; set; }
        IPlugin<IProjection> ProjectionPlugin { get; set; }
        IPlugin<ITracker> TrackerPlugin { get; set; }
        IPlugin<DistortionBase> DistortionPlugin { get; set; }
        IPlugin<IStabilizer> StabilizerPlugin { get; set; }
        StereoMode StereoInput { get; set; }
        LayoutMode StereoOutput { get; set; }
        ShortcutsManager Shortcuts { get; set; }
    }
}