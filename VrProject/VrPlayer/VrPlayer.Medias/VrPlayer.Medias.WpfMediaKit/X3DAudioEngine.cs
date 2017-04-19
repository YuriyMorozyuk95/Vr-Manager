using System;
using System.Windows.Media.Media3D;

using SharpDX;
using SharpDX.XAudio2;
using SharpDX.X3DAudio;
using SharpDX.Multimedia;

using WPFMediaKit.DirectShow.Interop;

using VrPlayer.Helpers;

namespace VrPlayer.Medias.WpfMediaKit
{
    public class X3DAudioEngine : AudioEngineBase
    {
        private XAudio2 _xaudio2;
        private X3DAudio _x3dAudio;
        private MasteringVoice _masteringVoice;
        private WaveFormatExtensible _deviceFormat;
        private WaveFormatExtensible _format;
        private SourceVoice _voice;
        private Emitter _emitter;
        private bool _isPlaying;

        public X3DAudioEngine()
        {
            _xaudio2 = new XAudio2();
            _masteringVoice = new MasteringVoice(_xaudio2);

            _deviceFormat = _xaudio2.GetDeviceDetails(0).OutputFormat;
            _x3dAudio = new X3DAudio(_deviceFormat.ChannelMask);

            Position = new Vector3D(0, 0, 0);
            Rotation = System.Windows.Media.Media3D.Quaternion.Identity;
        }

        public override void Setup(WaveFormatEx format)
        {
            _format = new WaveFormatExtensible(format.nSamplesPerSec, format.wBitsPerSample, format.nChannels);
            _voice = new SourceVoice(_xaudio2, _format);
            _voice.StreamEnd += _voice_StreamEnd;
            _voice.VoiceError += _voice_VoiceError;
            _emitter = new Emitter
            {
                ChannelAzimuths = GetAzimuths(_format.Channels),
                ChannelCount = _format.Channels,
                ChannelRadius = 10,
                CurveDistanceScaler = float.MinValue,
                OrientFront = new Vector3(0, 0, 1),
                OrientTop = new Vector3(0, 1, 0),
                Position = new Vector3(0, 0, 0),
                Velocity = new Vector3(0, 0, 0)
            };
        }

        public override void PlayBuffer(byte[] buffer)
        {
            if (_voice.IsDisposed)
                return;

            var dataStream = new DataStream(buffer.Length, true, true);
            dataStream.Write(buffer, 0, buffer.Length);
            dataStream.Position = 0;
            var audioBuffer = new AudioBuffer
            {
                Stream = dataStream,
                AudioBytes = buffer.Length,
                Flags = BufferFlags.EndOfStream
            };
            _voice.SubmitSourceBuffer(audioBuffer, null);

            Update();

            if (!_isPlaying)
               Play();
        }

        private void Play()
        {
            _isPlaying = true;
            _voice.Start();
        }

        private void Update()
        {
            if (_voice.IsDisposed)
                return;

            var listener = new Listener
            {
                OrientFront = Vector3DToVector3(QuaternionHelper.FrontVectorFromQuaternion(Rotation)),
                OrientTop = Vector3DToVector3(QuaternionHelper.UpVectorFromQuaternion(Rotation)),
                Position = Vector3DToVector3(Position),
                Velocity = new Vector3(0, 0, 0)
            };

            var dspSettings = _x3dAudio.Calculate(listener, _emitter, CalculateFlags.Matrix, _format.Channels, _deviceFormat.Channels);
            _voice.SetOutputMatrix(_format.Channels, _deviceFormat.Channels, dspSettings.MatrixCoefficients);
        }

        private Vector3 Vector3DToVector3(Vector3D vector)
        {
            return new Vector3((float)vector.X, (float)vector.Y, (float)vector.Z);
        }

        void _voice_VoiceError(SourceVoice.VoiceErrorArgs obj)
        {
            Logger.Instance.Error(obj.Result.ToString(), null);
            _voice_StreamEnd();
        }

        void _voice_StreamEnd()
        {
            _isPlaying = false;
            Dispose();
        }

        public override void Dispose()
        {
            if(_voice != null)
                _voice.Dispose();
            if (_masteringVoice != null)
                _masteringVoice.Dispose();
            if(_xaudio2 != null)
                _xaudio2.Dispose();
        }

        #region Azimuths

        //Todo: Use standard positions.
        private float[] GetAzimuths(int channels)
        {
            if (channels <= 0 || channels > XAudio2.MaximumAudioChannels)
            {
                throw new Exception("Invalid channels count for azimuths generation.");
            }
            if (channels == 1)//Mono
            {
                return new float[] { 0 };
            }
            else if (channels == 2)//Stereo
            {
                return new float[] { 
                    DegreeToRadian(270),    //Left	SPEAKER_FRONT_LEFT		0
                    DegreeToRadian(90)      //Right	SPEAKER_FRONT_RIGHT		1
                };
            }
            else if (channels == 3)//Stereo 2.1
            {
                return new float[] { 
                    DegreeToRadian(270),   //Left	SPEAKER_FRONT_LEFT		0
                    DegreeToRadian(90),    //Right	SPEAKER_FRONT_RIGHT		1
                    DegreeToRadian(0)      //Sub?		    2
                };
            }
            else if (channels == 4)//Surround 4.0 (Quadraphonic) 
            {
                return new float[] { 
                    DegreeToRadian(270),    //Front Left	SPEAKER_FRONT_LEFT		0
                    DegreeToRadian(45),     //Front Right	SPEAKER_FRONT_RIGHT		1
                    DegreeToRadian(225),    //Back Left	    SPEAKER_BACK_LEFT	    2
                    DegreeToRadian(135)     //Back Right	SPEAKER_BACK_RIGHT		3
                };
            }
            else if (channels == 5)//Surround 4.1 
            {
                return new float[] { 
                    DegreeToRadian(270),    //Front Left	SPEAKER_FRONT_LEFT		0
                    DegreeToRadian(45),     //Front Right	SPEAKER_FRONT_RIGHT		1
                    DegreeToRadian(225),    //Back Left	    SPEAKER_BACK_LEFT	    2
                    DegreeToRadian(135),    //Back Right	SPEAKER_BACK_RIGHT		3
                    DegreeToRadian(0)       //Sub? 									4
                };
            }
            else if (channels == 6)//Surround 5.1
            {
                return new float[] { 
                    DegreeToRadian(270),    //Front Left	SPEAKER_FRONT_LEFT		0
                    DegreeToRadian(45),     //Front Right	SPEAKER_FRONT_RIGHT		1
                    DegreeToRadian(0),      //Front Center	SPEAKER_FRONT_CENTER	2
                    DegreeToRadian(0),      //Sub? 									3
                    DegreeToRadian(225),    //Back Left	    SPEAKER_BACK_LEFT	    4
                    DegreeToRadian(135)     //Back Right	SPEAKER_BACK_RIGHT		5
                };
            }
            else
            {
                //Other configurations are not supported yet.
                return new float[channels];
            }
        }

        private float DegreeToRadian(float angle)
        {
            return (float)Math.PI * angle / 180.0f;
        }

        #endregion

    }
}