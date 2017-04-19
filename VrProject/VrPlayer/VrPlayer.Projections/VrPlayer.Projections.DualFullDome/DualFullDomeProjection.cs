using System;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using VrPlayer.Contracts.Projections;

namespace VrPlayer.Projections.DualFullDome
{
    [DataContract]
    public class DualFullDomeProjection : ProjectionBase, IProjection
    {
        private Point3D _center;
        private double _radius = 1;
        private const double Distance = 1000;
        private const double HorizontalCoverage = 0.5D;
        
        public static readonly DependencyProperty SlicesProperty =
            DependencyProperty.Register("Slices", typeof(int),
            typeof(DualFullDomeProjection), new FrameworkPropertyMetadata(16));
        [DataMember]
        public int Slices
        {
            get { return (int)GetValue(SlicesProperty); }
            set { SetValue(SlicesProperty, value); }
        }

        public static readonly DependencyProperty StacksProperty =
             DependencyProperty.Register("Stacks", typeof(int),
             typeof(DualFullDomeProjection), new FrameworkPropertyMetadata(16));
        [DataMember]
        public int Stacks
        {
            get { return (int)GetValue(StacksProperty); }
            set { SetValue(StacksProperty, value); }
        }

        public static readonly DependencyProperty CoverageProperty =
            DependencyProperty.Register("Coverage", typeof(double),
            typeof(DualFullDomeProjection), new FrameworkPropertyMetadata(0.5D));
        [DataMember]
        public double Coverage
        {
            get { return (double)GetValue(CoverageProperty); }
            set { SetValue(CoverageProperty, value); }
        }

        public static readonly DependencyProperty FishEyeFOVProperty =
            DependencyProperty.Register("FishEyeFOV", typeof(double),
            typeof(DualFullDomeProjection), new FrameworkPropertyMetadata(180D));
        [DataMember]
        public double FishEyeFOV
        {
            get { return (double)GetValue(FishEyeFOVProperty); }
            set { SetValue(FishEyeFOVProperty, value); }
        }

        public static readonly DependencyProperty HeadingProperty =
            DependencyProperty.Register("Heading", typeof(int),
            typeof(DualFullDomeProjection), new FrameworkPropertyMetadata(0));
        [DataMember]
        public int Heading
        {
            get { return (int)GetValue(HeadingProperty); }
            set { SetValue(HeadingProperty, value); }
        }

        public static readonly DependencyProperty TiltProperty =
            DependencyProperty.Register("Tilt", typeof(int),
            typeof(DualFullDomeProjection), new FrameworkPropertyMetadata(0));
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

                //FRONT LEFT
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    double phi = Math.PI / 2 - stack * Math.PI / Stacks * Coverage;
                    double y = Radius * Math.Sin(phi);
                    double scale = -Radius * Math.Cos(phi);

                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        double theta = HorizontalCoverage * slice * 4 * Math.PI / Slices + Math.PI * (0.5 - HorizontalCoverage);
                        //double theta = (slice * 2 * Math.PI / Slices);
                        double x = scale * Math.Sin(theta) + Radius;
                        double z = scale * Math.Cos(theta);

                        var normal = new Vector3D(x + Distance, y, z) + Center;

                        var axis = new AxisAngleRotation3D(new Vector3D(1, 0, 0), Tilt + 90);
                        var trans = new RotateTransform3D(axis, new Point3D(0, 0, 0));
                        var vector = Vector3D.Multiply(new Vector3D(normal.X, normal.Y, normal.Z), trans.Value);
                        positions.Add(new Point3D(vector.X, vector.Y, vector.Z));
                    }
                }

                //FRONT RIGHT
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    double phi = Math.PI / 2 - stack * Math.PI / Stacks * Coverage;
                    double y = Radius * Math.Sin(phi);
                    double scale = -Radius * Math.Cos(phi);

                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        double theta = HorizontalCoverage * slice * 4 * Math.PI / Slices + Math.PI * (0.5 - HorizontalCoverage);
                        //double theta = slice * 2 * Math.PI / Slices;
                        double x = scale * Math.Sin(theta) - Radius;
                        double z = scale * Math.Cos(theta);

                        var normal = new Vector3D(x - Distance, y, z) + Center;

                        var axis = new AxisAngleRotation3D(new Vector3D(1, 0, 0), Tilt + 90);
                        var trans = new RotateTransform3D(axis, new Point3D(0, 0, 0));
                        var vector = Vector3D.Multiply(new Vector3D(normal.X, normal.Y, normal.Z), trans.Value);
                        positions.Add(new Point3D(vector.X, vector.Y, vector.Z));
                    }
                }

                //REAR LEFT
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    double phi = Math.PI / 2 - stack * Math.PI / Stacks * Coverage;
                    double y = Radius * Math.Sin(phi);
                    double scale = -Radius * Math.Cos(phi);

                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        double theta = HorizontalCoverage * slice * 4 * Math.PI / Slices + Math.PI * (0.5 - HorizontalCoverage);
                        //double theta = slice * 2 * Math.PI / Slices;
                        double x = scale * Math.Sin(theta) + Radius;
                        double z = scale * Math.Cos(theta);

                        var normal = new Vector3D(x + Distance, y, z) + Center;

                        var axis = new AxisAngleRotation3D(new Vector3D(1, 0, 0), Tilt - 90);
                        var trans = new RotateTransform3D(axis, new Point3D(0, 0, 0));
                        var vector = Vector3D.Multiply(new Vector3D(normal.X, normal.Y, normal.Z), trans.Value);
                        positions.Add(new Point3D(vector.X, vector.Y, vector.Z));
                    }
                }

                //REAR RIGHT
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    double phi = Math.PI / 2 - stack * Math.PI / Stacks * Coverage;
                    double y = Radius * Math.Sin(phi);
                    double scale = -Radius * Math.Cos(phi);

                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        double theta = HorizontalCoverage * slice * 4 * Math.PI / Slices + Math.PI * (0.5 - HorizontalCoverage);
                        //double theta = slice * 2 * Math.PI / Slices;
                        double x = scale * Math.Sin(theta) - Radius;
                        double z = scale * Math.Cos(theta);

                        var normal = new Vector3D(x - Distance, y, z) + Center;

                        var axis = new AxisAngleRotation3D(new Vector3D(1, 0, 0), Tilt - 90);
                        var trans = new RotateTransform3D(axis, new Point3D(0, 0, 0));
                        var vector = Vector3D.Multiply(new Vector3D(normal.X, normal.Y, normal.Z), trans.Value);
                        positions.Add(new Point3D(vector.X, vector.Y, vector.Z));
                    }
                }
                // Create triangle that will never be seen. It is a work around for automatic texture scaling in WPF.
                //  It is needed so we can scale for Fisheye lens FOV greater than 180.
                positions.Add(new Point3D(10000, 10000, 10000));
                positions.Add(new Point3D(10000, 10001, 10000));
                positions.Add(new Point3D(10000, 10000, 10001));
                return positions;
            }
        }

        public override Int32Collection TriangleIndices
        {
            get
            {
                var triangleIndices = new Int32Collection();

                //FRONT LEFT
                for (int stack = 0; stack < Stacks; stack++)
                {
                    int top = (stack + 0) * (Slices + 1);
                    int bot = (stack + 1) * (Slices + 1);

                    for (int slice = 0; slice < Slices; slice++)
                    {
                        if (stack != 0)
                        {
                            triangleIndices.Add(top + slice);
                            triangleIndices.Add(bot + slice);
                            triangleIndices.Add(top + slice + 1);
                        }
                        triangleIndices.Add(top + slice + 1);
                        triangleIndices.Add(bot + slice);
                        triangleIndices.Add(bot + slice + 1);
                    }
                }

                //FRONT RIGHT
                for (int stack = Stacks + 1; stack <= (Stacks * 2); stack++)
                {
                    int top = (stack + 0) * (Slices + 1);
                    int bot = (stack + 1) * (Slices + 1);

                    for (int slice = 0; slice < Slices; slice++)
                    {
                        if (stack != Stacks + 1)
                        {
                            triangleIndices.Add(top + slice);
                            triangleIndices.Add(bot + slice);
                            triangleIndices.Add(top + slice + 1);
                        }
                        triangleIndices.Add(top + slice + 1);
                        triangleIndices.Add(bot + slice);
                        triangleIndices.Add(bot + slice + 1);
                    }
                }

                //REAR LEFT
                for (int stack = (Stacks * 2) + 2; stack <= (Stacks * 3) + 1; stack++)
                {
                    int top = (stack + 0) * (Slices + 1);
                    int bot = (stack + 1) * (Slices + 1);

                    for (int slice = 0; slice < Slices; slice++)
                    {
                        if (stack != (Stacks * 2) + 2)
                        {
                            triangleIndices.Add(top + slice);
                            triangleIndices.Add(bot + slice);
                            triangleIndices.Add(top + slice + 1);
                        }
                        triangleIndices.Add(top + slice + 1);
                        triangleIndices.Add(bot + slice);
                        triangleIndices.Add(bot + slice + 1);
                    }
                }

                //REAR RIGHT
                for (int stack = (Stacks * 3) + 3; stack <= (Stacks * 4) + 2; stack++)
                {
                    int top = (stack + 0) * (Slices + 1);
                    int bot = (stack + 1) * (Slices + 1);

                    for (int slice = 0; slice < Slices; slice++)
                    {
                        if (stack != (Stacks * 3) + 3)
                        {
                            triangleIndices.Add(top + slice);
                            triangleIndices.Add(bot + slice);
                            triangleIndices.Add(top + slice + 1);
                        }
                        triangleIndices.Add(top + slice + 1);
                        triangleIndices.Add(bot + slice);
                        triangleIndices.Add(bot + slice + 1);
                    }
                }
                // Create triangle that will never be seen. It is a work around for automatic texture scaling in WPF.
                //  It is needed so we can scale for Fisheye lens FOV greater than 180.
                triangleIndices.Add((Slices+1)*(Stacks+1)+1);
                triangleIndices.Add((Slices + 1) * (Stacks + 1) + 2);
                triangleIndices.Add((Slices + 1) * (Stacks + 1) + 3);
                return triangleIndices;
            }
        }

        public override PointCollection MonoTextureCoordinates
        {
            get
            {
                double fisheyeScale = 1.0;
                if (FishEyeFOV > 180.0) fisheyeScale -= (FishEyeFOV - 180.0) / 180.0;
                var textureCoordinates = new PointCollection();

                //FRONT LEFT
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        var p = new Point((double)slice / Slices, (double)stack / Stacks);
                        var u = 0.5 + fisheyeScale * (p.Y / 2) * Math.Cos(p.X * 2 * Math.PI - Math.PI / 2 - 2 * Math.PI * Heading / 360);
                        var v = 0.5 + fisheyeScale * (p.Y / 2) * Math.Sin(p.X * 2 * Math.PI - Math.PI / 2 - 2 * Math.PI * Heading / 360);
                        textureCoordinates.Add(new Point(u, v));
                    }
                }

                //FRONT RIGHT
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        var p = new Point((double)slice / Slices, (double)stack / Stacks);
                        var u = 0.5 + fisheyeScale * (p.Y / 2) * Math.Cos(p.X * 2 * Math.PI - Math.PI / 2 - 2 * Math.PI * Heading / 360);
                        var v = 0.5 + fisheyeScale * (p.Y / 2) * Math.Sin(p.X * 2 * Math.PI - Math.PI / 2 - 2 * Math.PI * Heading / 360);
                        textureCoordinates.Add(new Point(u, v));
                    }
                }

                //REAR LEFT
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        var p = new Point((double)slice / Slices, (double)stack / Stacks);
                        var u = 0.5 + fisheyeScale * (p.Y / 2) * Math.Cos(p.X * 2 * Math.PI - Math.PI / 2 + 2 * Math.PI * Heading / 360);
                        var v = 0.5 + fisheyeScale * (p.Y / 2) * Math.Sin(p.X * 2 * Math.PI - Math.PI / 2 + 2 * Math.PI * Heading / 360);
                        textureCoordinates.Add(new Point(u, v));
                    }
                }

                //REAR RIGHT
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        var p = new Point((double)slice / Slices, (double)stack / Stacks);
                        var u = 0.5 + fisheyeScale * (p.Y / 2) * Math.Cos(p.X * 2 * Math.PI - Math.PI / 2 + 2 * Math.PI * Heading / 360);
                        var v = 0.5 + fisheyeScale * (p.Y / 2) * Math.Sin(p.X * 2 * Math.PI - Math.PI / 2 + 2 * Math.PI * Heading / 360);
                        textureCoordinates.Add(new Point(u, v));
                    }
                }

                // The extra triangle that will never be seen must have texture coords of 0,0 and 0,1 for two of the positions. It is a work around for automatic texture scaling in WPF.
                //  It will keep WPF from scaling the texture after cropping for fisheye FOV setting greater than 180 degrees.
                textureCoordinates.Add(new Point(0, 0));
                textureCoordinates.Add(new Point(0, 1));
                textureCoordinates.Add(new Point(1, 1));
                return textureCoordinates;
            }
        }

        public override PointCollection OverUnderTextureCoordinates
        {
            get
            {
                double fisheyeScale = 1.0;
                if (FishEyeFOV > 180.0) fisheyeScale -= (FishEyeFOV - 180.0) / 180.0;
                // Divide by 2 because each Fisheye is half of width and height.
                var textureCoordinates = new PointCollection();

                //FRONT LEFT
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        var p = new Point((double)slice / Slices, (double)stack / Stacks);
                        var u = 0.25 + fisheyeScale * (p.Y / 4) * Math.Cos(p.X * 2 * Math.PI - Math.PI / 2 - 2 * Math.PI * Heading / 360);
                        var v = 0.25 + fisheyeScale * (p.Y / 4) * Math.Sin(p.X * 2 * Math.PI - Math.PI / 2 - 2 * Math.PI * Heading / 360);
                        textureCoordinates.Add(new Point(u, v));
                    }
                }

                //FRONT RIGHT
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        var p = new Point((double)slice / Slices, (double)stack / Stacks);
                        var u = 0.25 + fisheyeScale * (p.Y / 4) * Math.Cos(p.X * 2 * Math.PI - Math.PI / 2 - 2 * Math.PI * Heading / 360);
                        var v = 0.75 + fisheyeScale * (p.Y / 4) * Math.Sin(p.X * 2 * Math.PI - Math.PI / 2 - 2 * Math.PI * Heading / 360);
                        textureCoordinates.Add(new Point(u, v));
                    }
                }

                //REAR LEFT
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        var p = new Point((double)slice / Slices, (double)stack / Stacks);
                        var u = 0.75 + fisheyeScale * (p.Y / 4) * Math.Cos(p.X * 2 * Math.PI + Math.PI / 2 + 2 * Math.PI * Heading / 360);
                        var v = 0.25 + fisheyeScale * (p.Y / 4) * Math.Sin(p.X * 2 * Math.PI + Math.PI / 2 + 2 * Math.PI * Heading / 360);
                        textureCoordinates.Add(new Point(u, v));
                    }
                }

                //REAR RIGHT
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        var p = new Point((double)slice / Slices, (double)stack / Stacks);
                        var u = 0.75 + fisheyeScale * (p.Y / 4) * Math.Cos(p.X * 2 * Math.PI + Math.PI / 2 + 2 * Math.PI * Heading / 360);
                        var v = 0.75 + fisheyeScale * (p.Y / 4) * Math.Sin(p.X * 2 * Math.PI + Math.PI / 2 + 2 * Math.PI * Heading / 360);
                        textureCoordinates.Add(new Point(u, v));
                    }
                }

                // The extra triangle that will never be seen must have texture coords of 0,0 and 0,1 for two of the positions. It is a work around for automatic texture scaling in WPF.
                //  It will keep WPF from scaling the texture after cropping for fisheye FOV setting greater than 180 degrees.
                textureCoordinates.Add(new Point(0, 0));
                textureCoordinates.Add(new Point(0, 1));
                textureCoordinates.Add(new Point(1, 1));
                return textureCoordinates;
            }

        }

        public override PointCollection SideBySideTextureCoordinates
        {
            get
            {
                double fisheyeScale = 1.0;
                if (FishEyeFOV > 180.0) fisheyeScale -= (FishEyeFOV - 180.0) / 180.0;
                var textureCoordinates = new PointCollection();

                //FRONT LEFT
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        var p = new Point((double)slice / Slices, (double)stack / Stacks);
                        var u = 0.25 + fisheyeScale*(p.Y / 4) * Math.Cos(p.X * 2 * Math.PI - Math.PI / 2 - 2 * Math.PI * Heading / 360);
                        var v = 0.25 + fisheyeScale*(p.Y / 4) * Math.Sin(p.X * 2 * Math.PI - Math.PI / 2 - 2 * Math.PI * Heading / 360);
                        textureCoordinates.Add(new Point(u, v));
                    }
                }

                //FRONT RIGHT
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        var p = new Point((double)slice / Slices, (double)stack / Stacks);
                        var u = 0.75 + fisheyeScale*(p.Y / 4) * Math.Cos(p.X * 2 * Math.PI - Math.PI / 2 - 2 * Math.PI * Heading / 360);
                        var v = 0.25 + fisheyeScale*(p.Y / 4) * Math.Sin(p.X * 2 * Math.PI - Math.PI / 2 - 2 * Math.PI * Heading / 360);
                        textureCoordinates.Add(new Point(u, v));
                    }
                }

                //REAR LEFT
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        var p = new Point((double)slice / Slices, (double)stack / Stacks);
                        var u = 0.25 + fisheyeScale*(p.Y / 4) * Math.Cos(p.X * 2 * Math.PI + Math.PI / 2 + 2 * Math.PI * Heading / 360);
                        var v = 0.75 + fisheyeScale*(p.Y / 4) * Math.Sin(p.X * 2 * Math.PI + Math.PI / 2 + 2 * Math.PI * Heading / 360);
                        textureCoordinates.Add(new Point(u, v));
                    }
                }

                //REAR RIGHT
                for (int stack = 0; stack <= Stacks; stack++)
                {
                    for (int slice = 0; slice <= Slices; slice++)
                    {
                        var p = new Point((double)slice / Slices, (double)stack / Stacks);
                        var u = 0.75 + fisheyeScale*(p.Y / 4) * Math.Cos(p.X * 2 * Math.PI + Math.PI / 2 + 2 * Math.PI * Heading / 360);
                        var v = 0.75 + fisheyeScale*(p.Y / 4) * Math.Sin(p.X * 2 * Math.PI + Math.PI / 2 + 2 * Math.PI * Heading / 360);
                        textureCoordinates.Add(new Point(u, v));
                    }
                }

                // The extra triangle that will never be seen must have texture coords of 0,0 and 0,1 for two of the positions. It is a work around for automatic texture scaling in WPF.
                //  It will keep WPF from scaling the texture after cropping for fisheye FOV setting greater than 180 degrees.
                textureCoordinates.Add(new Point(0, 0));
                textureCoordinates.Add(new Point(0, 1));
                textureCoordinates.Add(new Point(1, 1));

                return textureCoordinates;
            }
        }

        private double DegToRad(double angle)
        {
            return Math.PI * angle / 180.0;
        }
    }
}
