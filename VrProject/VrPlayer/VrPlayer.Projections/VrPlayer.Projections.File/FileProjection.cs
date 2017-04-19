using System;
using System.IO;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using VrPlayer.Contracts.Projections;
using VrPlayer.Helpers;

namespace VrPlayer.Projections.File
{
    [DataContract]
    public class FileProjection : ProjectionBase, IProjection
    {
        private const double _distance = 1000;

        private string _filePath;
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                OnPropertyChanged("FilePath");
                OnPropertyChanged("Geometry");
            }
        }

        public override MeshGeometry3D Geometry 
        {
            get
            {
                ReadGeometryFromFile(_filePath);
                return base.Geometry;
            }
        }

        private void ReadGeometryFromFile(string path)
        {
            if (string.IsNullOrEmpty(path))
                return;

            try
            {
                _positions = new Point3DCollection();
                _triangleIndices = new Int32Collection();
                _monoTextureCoordinates = new PointCollection();
                _overUnderTextureCoordinates = new PointCollection();
                _sideBySideTextureCoordinates = new PointCollection();

                var geometry = new MeshGeometry3D();
                var models = new Model3DGroup();
                var fileInfo = new FileInfo(path);

                if (fileInfo.Extension == ".obj")
                {
                    var reader = new ObjReader();
                    try
                    {
                        models = reader.Read(fileInfo.FullName);
                    }
                    catch (Exception exc)
                    {
                        Logger.Instance.Error(string.Format("Error while loading obj file '{0}'.", path), exc);
                    }
                }

                if (fileInfo.Extension == ".3ds")
                {
                    var reader = new StudioReader();
                    try
                    {
                        models = reader.Read(fileInfo.FullName);
                    }
                    catch (Exception exc)
                    {
                        Logger.Instance.Error(string.Format("Error while loading 3ds file '{0}'.", path), exc);
                    }
                }

                if (models.Children.Count > 0)
                {
                    var model = models.Children[0] as GeometryModel3D;
                    if (model != null) geometry = model.Geometry as MeshGeometry3D;
                }

                if (geometry == null)
                    return;

                //Positions
                var leftPositions = geometry.Positions.Clone();
                var rightPositions = geometry.Positions.Clone();

                foreach (var leftPosition in leftPositions)
                {
                    leftPosition.Offset(-_distance, 0, 0);
                    _positions.Add(leftPosition);
                }

                foreach (var rightPosition in rightPositions)
                {
                    rightPosition.Offset(_distance, 0, 0);
                    _positions.Add(rightPosition);
                }

                if (geometry.TextureCoordinates == null)
                    return;

                //Textures
                var leftTextureCoordinates = geometry.TextureCoordinates.Clone();
                var rightTextureCoordinates = geometry.TextureCoordinates.Clone();

                foreach (var leftTextureCoordinate in leftTextureCoordinates)
                {
                    _monoTextureCoordinates.Add(leftTextureCoordinate);
                    _overUnderTextureCoordinates.Add(new Point(leftTextureCoordinate.X, leftTextureCoordinate.Y / 2));
                    _sideBySideTextureCoordinates.Add(new Point(leftTextureCoordinate.X / 2, leftTextureCoordinate.Y));
                }

                foreach (var rightTextureCoordinate in rightTextureCoordinates)
                {
                    _monoTextureCoordinates.Add(rightTextureCoordinate);
                    _overUnderTextureCoordinates.Add(new Point(rightTextureCoordinate.X, rightTextureCoordinate.Y / 2 + 0.5));
                    _sideBySideTextureCoordinates.Add(new Point(rightTextureCoordinate.X / 2 + 0.5, rightTextureCoordinate.Y));
                }

                //TriangleIndices
                var leftTriangleIndices = geometry.TriangleIndices.Clone();
                var rightTriangleIndices = geometry.TriangleIndices.Clone();
                int t = 0;
                foreach (var leftTriangleIndice in leftTriangleIndices)
                {
                    if (leftTriangleIndice > t) t = leftTriangleIndice;
                    _triangleIndices.Add(leftTriangleIndice);
                }

                foreach (var rightTriangleIndice in rightTriangleIndices)
                {
                    if (rightTriangleIndice > t) t = rightTriangleIndice;
                    _triangleIndices.Add(rightTriangleIndice + geometry.Positions.Count);
                }
            }
            catch (Exception exc)
            {
                Logger.Instance.Error("Error while generating file projection.", exc);
            }
        }

        public new Vector3D CameraLeftPosition
        {
            get { return new Vector3D(-_distance, 0, 0); }
        }

        public new Vector3D CameraRightPosition
        {
            get { return new Vector3D(_distance, 0, 0); }
        }

        private Int32Collection _triangleIndices;
        public override Int32Collection TriangleIndices
        {
            get { return _triangleIndices; }
        }

        private PointCollection _monoTextureCoordinates;
        public override PointCollection MonoTextureCoordinates
        {
            get { return _monoTextureCoordinates; }
        }

        private PointCollection _overUnderTextureCoordinates;
        public override PointCollection OverUnderTextureCoordinates
        {
            get { return _overUnderTextureCoordinates; }
        }

        private PointCollection _sideBySideTextureCoordinates;
        public override PointCollection SideBySideTextureCoordinates
        {
            get { return _sideBySideTextureCoordinates; }
        }

        private Point3DCollection _positions;
        public override Point3DCollection Positions
        {
            get { return _positions; }
        }
    }
}