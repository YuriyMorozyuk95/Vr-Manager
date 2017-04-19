using System;
using System.Windows.Media.Media3D;

using WPFMediaKit.DirectShow.Interop;

namespace VrPlayer.Medias.WpfMediaKit
{
    public interface IAudioEngine: IDisposable
    {
        void PlayBuffer(byte[] buffer);
        void Setup(WaveFormatEx format);
        Vector3D Position { get; set; }
        Quaternion Rotation { get; set; }
    }
}