using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Distortions;
using VrPlayer.Contracts.Effects;
using VrPlayer.Contracts.Medias;
using VrPlayer.Contracts.Projections;
using VrPlayer.Contracts.Stabilizers;
using VrPlayer.Contracts.Trackers;

namespace VrPlayer.Models.Plugins
{
    public class DynamicPluginManager : IPluginManager
    {
        [ImportMany]
        private IEnumerable<IPlugin<IMedia>> _medias;
        public IEnumerable<IPlugin<IMedia>> Medias
        {
            get
            {
                if (_medias == null)
                    return new BindingList<IPlugin<IMedia>>();
                return _medias.Where(media => media.Content == null)
                    .Concat(_medias.Where(media => media.Content != null));
            }
        }

        [ImportMany]
        private IEnumerable<IPlugin<EffectBase>> _effects;
        public IEnumerable<IPlugin<EffectBase>> Effects
        {
            get
            {
                if (_effects == null)
                    return new BindingList<IPlugin<EffectBase>>();
                return _effects.Where(effect => effect.Content == null)
                    .Concat(_effects.Where(effect => effect.Content != null));
            }
        }
        
        [ImportMany]
        private IEnumerable<IPlugin<DistortionBase>> _distortions;
        public IEnumerable<IPlugin<DistortionBase>> Distortions
        {
            get
            {
                if (_distortions == null)
                    return new BindingList<IPlugin<DistortionBase>>();
                return _distortions.Where(distortion => distortion.Content == null)
                    .Concat(_distortions.Where(distortion => distortion.Content != null));
            }
        }

        [ImportMany]
        private IEnumerable<IPlugin<IProjection>> _projections;
        public IEnumerable<IPlugin<IProjection>> Projections
        {
            get
            {
                if (_projections == null)
                    return new BindingList<IPlugin<IProjection>>();
                return _projections.Where(projection => projection.Content == null)
                    .Concat(_projections.Where(projection => projection.Content != null));
            }
        }

        [ImportMany]
        private IEnumerable<IPlugin<ITracker>> _trackers;
        public IEnumerable<IPlugin<ITracker>> Trackers
        {
            get
            {
                if (_trackers == null)
                    return new BindingList<IPlugin<ITracker>>();
                return _trackers.Where(trackers => trackers.Content == null)
                    .Concat(_trackers.Where(trackers => trackers.Content != null));
            }
        }

        [ImportMany]
        private IEnumerable<IPlugin<IStabilizer>> _stabilizers;
        public IEnumerable<IPlugin<IStabilizer>> Stabilizers
        {
            get
            {

                if (_stabilizers == null)
                    return new BindingList<IPlugin<IStabilizer>>();
                return _stabilizers.Where(stabilizers => stabilizers.Content == null)
                    .Concat(_stabilizers.Where(stabilizers => stabilizers.Content != null));
            }
        }

        public DynamicPluginManager(string path, IEnumerable<string> folders)
        {
            var catalog = new AggregateCatalog();
            
            foreach (var dir in from folder in folders 
                select new DirectoryInfo(Path.Combine(path, folder)) into info 
                where info.Exists from dir in info.GetDirectories() select dir)
            {
                catalog.Catalogs.Add(new DirectoryCatalog(dir.FullName));
            }
            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);
        }

        public void Dispose()
        {
        }
    }
}