using System.Windows.Media.Media3D;

namespace VrPlayer.Stabilizers.Csv
{
    public class CsvFrame
    {
        public int FrameNumber { get; set; }
        public Vector3D Translation { get; set; }
        public Quaternion Rotation { get; set; }
    }
}
