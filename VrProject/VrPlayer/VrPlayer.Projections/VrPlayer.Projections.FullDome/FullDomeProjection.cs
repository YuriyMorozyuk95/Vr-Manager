using System;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using VrPlayer.Contracts.Projections;
using Point = System.Windows.Point;

namespace VrPlayer.Projections.FullDome
{
    [DataContract]
    public class FullDomeProjection : ProjectionBase, IProjection
    {
        private Point3D _center;
        private double _radius = 1;
        private const double Distance = 1000;

        public static readonly DependencyProperty SlicesProperty =
            DependencyProperty.Register("Slices", typeof(int),
            typeof(FullDomeProjection), new FrameworkPropertyMetadata(16));
        [DataMember]
        public int Slices
        {
            get { return (int)GetValue(SlicesProperty); }
            set { SetValue(SlicesProperty, value); }
        }

        public static readonly DependencyProperty StacksProperty =
             DependencyProperty.Register("Stacks", typeof(int),
             typeof(FullDomeProjection), new FrameworkPropertyMetadata(16));
        [DataMember]
        public int Stacks
        {
            get { return (int)GetValue(StacksProperty); }
            set { SetValue(StacksProperty, value); }
        }

        public static readonly DependencyProperty CoverageProperty =
            DependencyProperty.Register("Coverage", typeof(int),
            typeof(FullDomeProjection), new FrameworkPropertyMetadata(180));
        [DataMember]
        public int Coverage
        {
            get { return (int)GetValue(CoverageProperty); }
            set { SetValue(CoverageProperty, value); }
        }

        public static readonly DependencyProperty HeadingProperty =
            DependencyProperty.Register("Heading", typeof(int),
            typeof(FullDomeProjection), new FrameworkPropertyMetadata(0));
        [DataMember]
        public int Heading
        {
            get { return (int)GetValue(HeadingProperty); }
            set { SetValue(HeadingProperty, value); }
        }

        public static readonly DependencyProperty TiltProperty =
            DependencyProperty.Register("Tilt", typeof(int),
            typeof(FullDomeProjection), new FrameworkPropertyMetadata(30));
        [DataMember]
        public int Tilt
        {
            get { return (int)GetValue(TiltProperty); }
            set { SetValue(TiltProperty, value); }
        }

        public Point3D Center
        {
            get { return _center; }
            set
            {
                _center = value;
                OnPropertyChanged("Center");
            }
        }

        public double Radius
        {
            get { return _radius; }
            set
            {
                _radius = value;
                OnPropertyChanged("Radius");
            }
        }

        public new Vector3D CameraLeftPosition
        {
            get
            {
                return new Vector3D(Distance + _radius, 0, 0);
            }
        }

        public new Vector3D CameraRightPosition
        {
            get
            {
                return new Vector3D(-Distance - _radius, 0, 0);
            }
        }

        public override Point3DCollection Positions
        {
            get
            {
                var positions = new Point3DCollection();

                //LEFT
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    double phi = Math.PI / 2 - stack * Math.PI / Stacks * (Coverage / 360D);
                    double y = Radius * Math.Sin(phi);
                    double scale = -Radius * Math.Cos(phi);

                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        double theta = slice * 2 * Math.PI / Slices;
                        double x = scale * Math.Sin(theta) + Radius;
                        double z = scale * Math.Cos(theta);

                        var normal = new Vector3D(x + Distance, y, z) + Center;

                        var axis = new AxisAngleRotation3D(new Vector3D(1,0,0), Tilt);
                        var trans = new RotateTransform3D(axis, new Point3D(0,0,0));
                        var vector = Vector3D.Multiply(new Vector3D(normal.X, normal.Y, normal.Z), trans.Value);
                        positions.Add(new Point3D(vector.X,vector.Y,vector.Z));
                    }
                }

                //RIGH
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    double phi = Math.PI / 2 - stack * Math.PI / Stacks * (Coverage / 360D);
                    double y = Radius * Math.Sin(phi);
                    double scale = -Radius * Math.Cos(phi);

                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        double theta = slice * 2 * Math.PI / Slices;
                        double x = scale * Math.Sin(theta) - Radius;
                        double z = scale * Math.Cos(theta);

                        var normal = new Vector3D(x - Distance, y, z) + Center;
                    
                        var axis = new AxisAngleRotation3D(new Vector3D(1, 0, 0), Tilt);
                        var trans = new RotateTransform3D(axis, new Point3D(0, 0, 0));
                        var vector = Vector3D.Multiply(new Vector3D(normal.X, normal.Y, normal.Z), trans.Value);
                        positions.Add(new Point3D(vector.X, vector.Y, vector.Z));
                    }
                }

                return positions;
            }
        }

        public override Int32Collection TriangleIndices
        {
            get
            {
                var triangleIndices = new Int32Collection();

                //LEFT
                for (int stack = 0; stack < Stacks; stack++)
                {
                    int top = (stack + 0) * (Slices + 1);
                    int bot = (stack + 1) * (Slices + 1);

                    for (int slice = 0; slice < Slices; slice++)
                    {
                        triangleIndices.Add(top + slice);
                        triangleIndices.Add(bot + slice);
                        triangleIndices.Add(top + slice + 1);
                        triangleIndices.Add(top + slice + 1);
                        triangleIndices.Add(bot + slice);
                        triangleIndices.Add(bot + slice + 1);
                    }
                }

                // RIGHT
                for (int stack = Stacks + 1; stack <= (Stacks * 2); stack++)
                {
                    int top = (stack + 0) * (Slices + 1);
                    int bot = (stack + 1) * (Slices + 1);

                    for (int slice = 0; slice < Slices; slice++)
                    {
                        triangleIndices.Add(top + slice);
                        triangleIndices.Add(bot + slice);
                        triangleIndices.Add(top + slice + 1);
                        triangleIndices.Add(top + slice + 1);
                        triangleIndices.Add(bot + slice);
                        triangleIndices.Add(bot + slice + 1);
                    }
                }

                return triangleIndices;
            }
        }

        public override PointCollection MonoTextureCoordinates
        {
            get
            {
                var textureCoordinates = new PointCollection();

                //LEFT
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        var p = new Point((double)slice / Slices, (double)stack / Stacks);
                        var u = 0.5 + (p.Y / 2) * Math.Cos(p.X * 2 * Math.PI - Math.PI / 2 + 2 * Math.PI * Heading / 360);
                        var v = 0.5 + (p.Y / 2) * Math.Sin(p.X * 2 * Math.PI - Math.PI / 2 + 2 * Math.PI * Heading / 360);
                        textureCoordinates.Add(new Point(u, v));
                    }
                }

                //RIGHT
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        var p = new Point((double)slice / Slices, (double)stack / Stacks);
                        var u = 0.5 + (p.Y / 2) * Math.Cos(p.X * 2 * Math.PI - Math.PI / 2 + 2 * Math.PI * Heading / 360);
                        var v = 0.5 + (p.Y / 2) * Math.Sin(p.X * 2 * Math.PI - Math.PI / 2 + 2 * Math.PI * Heading / 360);
                        textureCoordinates.Add(new Point(u, v));
                    }
                }

                return textureCoordinates;
            }
        }

        public override PointCollection OverUnderTextureCoordinates
        {
            get
            {
                var textureCoordinates = new PointCollection();

                //LEFT
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        var p = new Point((double)slice / Slices, (double)stack / Stacks);
                        var u = 0.5 + (p.Y / 2) * Math.Cos(p.X * 2 * Math.PI - Math.PI / 2 + 2 * Math.PI * Heading / 360);
                        var v = 0.25 + (p.Y / 4) * Math.Sin(p.X * 2 * Math.PI - Math.PI / 2 + 2 * Math.PI * Heading / 360);
                        textureCoordinates.Add(new Point(u, v));
                    }
                }

                //RIGHT
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        var p = new Point((double)slice / Slices, (double)stack / Stacks);
                        var u = 0.5 + (p.Y / 2) * Math.Cos(p.X * 2 * Math.PI - Math.PI / 2 + 2 * Math.PI * Heading / 360);
                        var v = 0.75 + (p.Y / 4) * Math.Sin(p.X * 2 * Math.PI - Math.PI / 2 + 2 * Math.PI * Heading / 360);
                        textureCoordinates.Add(new Point(u, v));
                    }
                }

                return textureCoordinates;
            }
        }

        public override PointCollection SideBySideTextureCoordinates
        {
            get
            {
                var textureCoordinates = new PointCollection();

                //LEFT
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        var p = new Point((double)slice / Slices, (double)stack / Stacks);
                        var u = 0.25 + (p.Y / 4) * Math.Cos(p.X * 2 * Math.PI - Math.PI / 2 + 2 * Math.PI * Heading / 360);
                        var v = 0.5 + (p.Y / 2) * Math.Sin(p.X * 2 * Math.PI - Math.PI / 2 + 2 * Math.PI * Heading / 360);
                        textureCoordinates.Add(new Point(u, v));
                    }
                }

                //RIGHT
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        var p = new Point((double)slice / Slices, (double)stack / Stacks);
                        var u = 0.75 + (p.Y / 4) * Math.Cos(p.X * 2 * Math.PI - Math.PI / 2 + 2 * Math.PI * Heading / 360);
                        var v = 0.5 + (p.Y / 2) * Math.Sin(p.X * 2 * Math.PI - Math.PI / 2 + 2 * Math.PI * Heading / 360);
                        textureCoordinates.Add(new Point(u, v));
                    }
                }

                return textureCoordinates;
            }
        }
    }
}