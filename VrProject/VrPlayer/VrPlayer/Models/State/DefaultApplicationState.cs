using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Distortions;
using VrPlayer.Contracts.Effects;
using VrPlayer.Contracts.Medias;
using VrPlayer.Contracts.Stabilizers;
using VrPlayer.Contracts.Trackers;
using VrPlayer.Helpers;
using VrPlayer.Models.Plugins;
using VrPlayer.Helpers.Mvvm;
using VrPlayer.Models.Config;
using VrPlayer.Contracts.Projections;
using VrPlayer.Models.Settings;

namespace VrPlayer.Models.State
{
    public class DefaultApplicationState : ViewModelBase, IApplicationState
    {
        #region Properties

        public static readonly DependencyProperty MediaPluginProperty =
            DependencyProperty.Register("MediaPlugin", typeof(IPlugin<IMedia>), typeof(DefaultApplicationState),
            new FrameworkPropertyMetadata(OnMediaPluginChanged));
        public IPlugin<IMedia> MediaPlugin
        {
            get { return (IPlugin<IMedia>)GetValue(MediaPluginProperty); }
            set { SetValue(MediaPluginProperty, value); }
        }

        private static void OnMediaPluginChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (IPlugin<IMedia>)args.OldValue;
            if (oldValue != null)
                oldValue.Unload();

            var newValue = (IPlugin<IMedia>)args.NewValue;
            if (newValue != null)
                newValue.Load();

            ((DefaultApplicationState)obj).OnPropertyChanged("MediaPlugin");
        }
        
        public static readonly DependencyProperty EffectPluginProperty =
            DependencyProperty.Register("EffectPlugin", typeof(IPlugin<EffectBase>), typeof(DefaultApplicationState),
            new FrameworkPropertyMetadata(OnEffectPluginChanged));
        public IPlugin<EffectBase> EffectPlugin
        {
            get { return (IPlugin<EffectBase>)GetValue(EffectPluginProperty); }
            set { SetValue(EffectPluginProperty, value); }
        }

        private static void OnEffectPluginChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (IPlugin<EffectBase>)args.OldValue;
            if (oldValue != null)
                oldValue.Unload();

            var newValue = (IPlugin<EffectBase>)args.NewValue;
            if (newValue != null)
                newValue.Load();

            ((DefaultApplicationState)obj).OnPropertyChanged("EffectPlugin");
        }

        public static readonly DependencyProperty ProjectionPluginProperty =
            DependencyProperty.Register("ProjectionPlugin", typeof(IPlugin<IProjection>), typeof(DefaultApplicationState),
            new FrameworkPropertyMetadata(OnProjectionPluginChanged));
        public IPlugin<IProjection> ProjectionPlugin
        {
            get { return (IPlugin<IProjection>)GetValue(ProjectionPluginProperty); }
            set { SetValue(ProjectionPluginProperty, value); }
        }

        private static void OnProjectionPluginChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var state = (DefaultApplicationState) obj;

            var oldValue = (IPlugin<IProjection>)args.OldValue;
            if (oldValue != null)
            {
                oldValue.Unload();
            }

            var newValue = (IPlugin<IProjection>)args.NewValue;
            if (newValue != null)
            {
                newValue.Load();
                newValue.Content.StereoMode = state.StereoInput;
            }

            ((DefaultApplicationState)obj).OnPropertyChanged("ProjectionPlugin");
        }

        public static readonly DependencyProperty TrackerPluginProperty =
            DependencyProperty.Register("TrackerPlugin", typeof(IPlugin<ITracker>), typeof(DefaultApplicationState),
            new FrameworkPropertyMetadata(OnTrackerPluginChanged));

        public IPlugin<ITracker> TrackerPlugin
        {
            get { return (IPlugin<ITracker>)GetValue(TrackerPluginProperty); }
            set { SetValue(TrackerPluginProperty, value); }
        }

        private static void OnTrackerPluginChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (IPlugin<ITracker>)args.OldValue;
            if (oldValue != null)
                oldValue.Unload();

            var newValue = (IPlugin<ITracker>)args.NewValue;
            if (newValue != null)
                newValue.Load();

            ((DefaultApplicationState)obj).OnPropertyChanged("TrackerPlugin");
        }

        public static readonly DependencyProperty DistortionPluginProperty =
            DependencyProperty.Register("DistortionPlugin", typeof(IPlugin<DistortionBase>), typeof(DefaultApplicationState),
            new FrameworkPropertyMetadata(OnDistortionPluginChanged));
        public IPlugin<DistortionBase> DistortionPlugin
        {
            get { return (IPlugin<DistortionBase>)GetValue(DistortionPluginProperty); }
            set { SetValue(DistortionPluginProperty, value); }
        }

        private static void OnDistortionPluginChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (IPlugin<DistortionBase>)args.OldValue;
            if (oldValue != null)
                oldValue.Unload();

            var newValue = (IPlugin<DistortionBase>)args.NewValue;
            if (newValue != null)
                newValue.Load();

            ((DefaultApplicationState)obj).OnPropertyChanged("DistortionPlugin");
        }

        public static readonly DependencyProperty StabilizerPluginProperty =
            DependencyProperty.Register("StabilizerPlugin", typeof(IPlugin<IStabilizer>), typeof(DefaultApplicationState),
            new FrameworkPropertyMetadata(OnStabilizerPluginChanged));
        public IPlugin<IStabilizer> StabilizerPlugin
        {
            get { return (IPlugin<IStabilizer>)GetValue(StabilizerPluginProperty); }
            set { SetValue(StabilizerPluginProperty, value); }
        }

        private static void OnStabilizerPluginChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldValue = (IPlugin<IStabilizer>)args.OldValue;
            if (oldValue != null)
                oldValue.Unload();

            var newValue = (IPlugin<IStabilizer>)args.NewValue;
            if (newValue != null)
                newValue.Load();

            ((DefaultApplicationState)obj).OnPropertyChanged("StabilizerPlugin");
        }

        public static readonly DependencyProperty StereoInputProperty =
            DependencyProperty.Register("StereoInput", typeof(StereoMode), typeof(DefaultApplicationState),
            new FrameworkPropertyMetadata(OnStereoInputChanged));
        public StereoMode StereoInput
        {
            get { return (StereoMode)GetValue(StereoInputProperty); }
            set { SetValue(StereoInputProperty, value); }
        }

        private static void OnStereoInputChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var projectionPlugin = ((DefaultApplicationState) obj).ProjectionPlugin;
            if (projectionPlugin != null && projectionPlugin.Content != null)
            {
                projectionPlugin.Content.StereoMode = (StereoMode)args.NewValue;
            }
        }

        private LayoutMode _stereoOutput;
        public LayoutMode StereoOutput
        {
            get
            {
                return _stereoOutput;
            }
            set
            {
                _stereoOutput = value;
                OnPropertyChanged("StereoOutput");
            }
        }

        private ShortcutsManager _shortcuts;
        public ShortcutsManager Shortcuts
        {
            get
            {
                return _shortcuts;
            }
            set
            {
                _shortcuts = value;
                OnPropertyChanged("Shortcuts");
            }
        }

        #endregion

        public DefaultApplicationState(IApplicationConfig config, IPluginManager pluginManager)
        {
            //Set plugins
            MediaPlugin = pluginManager.Medias
                .Where(m => m.GetType().FullName.Contains(config.DefaultMedia))
                .DefaultIfEmpty(pluginManager.Medias.FirstOrDefault())
                .First();

            EffectPlugin = pluginManager.Effects
                .Where(e => e.GetType().FullName.Contains(config.DefaultEffect))
                .DefaultIfEmpty(pluginManager.Effects.FirstOrDefault())
                .First();

            DistortionPlugin = pluginManager.Distortions
                .Where(d => d.GetType().FullName.Contains(config.DefaultDistortion))
                .DefaultIfEmpty(pluginManager.Distortions.FirstOrDefault())
                .First();
            
            ProjectionPlugin = pluginManager.Projections
                .Where(p => p.GetType().FullName.Contains(config.DefaultProjection))
                .DefaultIfEmpty(pluginManager.Projections.FirstOrDefault())
                .First();
            
            TrackerPlugin = pluginManager.Trackers
                .Where(t => t.GetType().FullName.Contains(config.DefaultTracker))
                .DefaultIfEmpty(pluginManager.Trackers.FirstOrDefault())
                .First();

            StabilizerPlugin = pluginManager.Stabilizers
                .Where(s => s.GetType().FullName.Contains(config.DefaultStabilizer))
                .DefaultIfEmpty(pluginManager.Stabilizers.FirstOrDefault())
                .First();

            Shortcuts = new ShortcutsManager();
            
            //Todo: Use binding instead of a timer
            var timer = new DispatcherTimer(DispatcherPriority.Render);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            timer.Tick += TimerOnTick;
            timer.Start();
        }

        //Todo: Use data binding for inter-plugins dependancies.
        private void TimerOnTick(object sender, EventArgs eventArgs)
        {
            if (MediaPlugin == null || MediaPlugin.Content == null)
                return;
            
            if (StabilizerPlugin != null && StabilizerPlugin.Content != null && 
                StabilizerPlugin.Content.GetFramesCount() > 0)
            {
                var frame = (int) Math.Round(StabilizerPlugin.Content.GetFramesCount()*MediaPlugin.Content.Progress/100);
                StabilizerPlugin.Content.UpdateCurrentFrame(frame);
            }

            if (TrackerPlugin != null && TrackerPlugin.Content != null)
            {
                MediaPlugin.Content.AudioPosition = TrackerPlugin.Content.Position;// +StabilizerPlugin.Content.Translation;
                MediaPlugin.Content.AudioRotation = TrackerPlugin.Content.Rotation;// *StabilizerPlugin.Content.Rotation;
            }
        }
    }
}
