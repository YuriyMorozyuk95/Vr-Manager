using System.Windows.Media.Media3D;

using WPFMediaKit.DirectShow.Interop;

namespace VrPlayer.Medias.WpfMediaKit
{
    public abstract class AudioEngineBase: IAudioEngine
    {
        private Vector3D _position;
        public Vector3D Position
        {
            get { return _position; }
            set { _position = value; }
        }

        private Quaternion _rotation;
        public Quaternion Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public abstract void PlayBuffer(byte[] buffer);
        public abstract void Setup(WaveFormatEx format);
        public abstract void Dispose();
    }
}