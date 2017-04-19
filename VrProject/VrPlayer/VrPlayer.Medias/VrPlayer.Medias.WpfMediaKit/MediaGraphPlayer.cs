using System;
using System.Runtime.InteropServices;

using WPFMediaKit.DirectShow.MediaPlayers;
using WPFMediaKit.DirectShow.Interop;

namespace VrPlayer.Medias.WpfMediaKit
{
    public class MediaGraphPlayer : MediaUriPlayer, ISampleGrabberCB
    {
        private const string DEFAULT_AUDIO_RENDERER_NAME = "Default DirectSound Device";

        private IAudioEngine _audioEngine;
        public IAudioEngine AudioEngine
        {
            get { return _audioEngine; }
        }

        private IGraphBuilder _graph;
        public IGraphBuilder Graph
        {
            get { return _graph; }
        }

        protected override void InsertAudioRenderer(string audioDeviceName)
        {
            //base.InsertAudioRenderer(audioDeviceName);
        }

        protected override void SetupFilterGraph(IFilterGraph graph)
        {
            _audioEngine = new X3DAudioEngine();
            _graph = graph as IGraphBuilder;
            base.SetupFilterGraph(graph);
            SetupAudio();
        }

        protected virtual void SetupAudio()
        {
            int hr;

            IEnumFilters enumFilters;
            hr = _graph.EnumFilters(out enumFilters);
            DsError.ThrowExceptionForHR(hr);

            IBaseFilter[] filters = new IBaseFilter[1];
            IntPtr fetched = new IntPtr();

            while (enumFilters.Next(1, filters, fetched) == 0)
            {
                IBaseFilter filter = filters[0] as IBaseFilter;
                IPin unconnectedPin = DsFindPin.ByConnectionStatus((IBaseFilter)filter, PinConnectedStatus.Unconnected, 0);
                if (unconnectedPin != null)
                {
                    PinDirection direction;
                    hr = unconnectedPin.QueryDirection(out direction);
                    DsError.ThrowExceptionForHR(hr);

                    if (direction == PinDirection.Output)
                    {
                        hr = _graph.Render(unconnectedPin);
                        DsError.ThrowExceptionForHR(hr);

                        SetupSampleGrabber();
                    }
                }
            }
        }

        private void SetupSampleGrabber()
        {
            if (_graph == null)
                return;

            int hr;

            //Get directsound filter
            IBaseFilter directSoundFilter;
            hr = _graph.FindFilterByName(DEFAULT_AUDIO_RENDERER_NAME, out directSoundFilter);
            DsError.ThrowExceptionForHR(hr);

            IPin rendererPinIn = DsFindPin.ByConnectionStatus(directSoundFilter, PinConnectedStatus.Connected, 0);

            if (rendererPinIn != null)
            {
                IPin audioPinOut;
                hr = rendererPinIn.ConnectedTo(out audioPinOut);
                DsError.ThrowExceptionForHR(hr);

                if (audioPinOut != null)
                {
                    //Disconect audio decoder to directsound renderer
                    hr = audioPinOut.Disconnect();
                    DsError.ThrowExceptionForHR(hr);

                    hr = _graph.RemoveFilter(directSoundFilter);
                    DsError.ThrowExceptionForHR(hr);

                    //Add Sample Grabber
                    ISampleGrabber sampleGrabber = new SampleGrabber() as ISampleGrabber;
                    hr = sampleGrabber.SetCallback(this, 1);
                    DsError.ThrowExceptionForHR(hr);

                    AMMediaType media;
                    media = new AMMediaType();
                    media.majorType = MediaType.Audio;
                    media.subType = MediaSubType.PCM;
                    media.formatType = FormatType.WaveEx;
                    hr = sampleGrabber.SetMediaType(media);
                    DsError.ThrowExceptionForHR(hr);

                    IPin sampleGrabberPinIn = DsFindPin.ByDirection((IBaseFilter)sampleGrabber, PinDirection.Input, 0);
                    IPin sampleGrabberPinOut = DsFindPin.ByDirection((IBaseFilter)sampleGrabber, PinDirection.Output, 0);
                    hr = _graph.AddFilter((IBaseFilter)sampleGrabber, "SampleGrabber");
                    DsError.ThrowExceptionForHR(hr);

                    PinInfo pinInfo;
                    hr = audioPinOut.QueryPinInfo(out pinInfo);
                    DsError.ThrowExceptionForHR(hr);

                    FilterInfo filterInfo;
                    hr = pinInfo.filter.QueryFilterInfo(out filterInfo);
                    DsError.ThrowExceptionForHR(hr);

                    hr = _graph.Connect(audioPinOut, sampleGrabberPinIn);
                    DsError.ThrowExceptionForHR(hr);

                    //Add null renderer
                    NullRenderer nullRenderer = new NullRenderer();
                    hr = _graph.AddFilter((IBaseFilter)nullRenderer, "NullRenderer");
                    DsError.ThrowExceptionForHR(hr);

                    IPin nullRendererPinIn = DsFindPin.ByDirection((IBaseFilter)nullRenderer, PinDirection.Input, 0);
                    hr = _graph.Connect(sampleGrabberPinOut, nullRendererPinIn);
                    DsError.ThrowExceptionForHR(hr);

                    _audioEngine.Setup(this.GetSampleGrabberFormat(sampleGrabber));
                }
            }
        }

        public WaveFormatEx GetSampleGrabberFormat(ISampleGrabber sampleGrabber)
        {
            int hr;

            AMMediaType mediaInfo = new AMMediaType();
            hr = sampleGrabber.GetConnectedMediaType(mediaInfo);
            DsError.ThrowExceptionForHR(hr);

            if ((mediaInfo.formatType != FormatType.WaveEx) || (mediaInfo.formatPtr == IntPtr.Zero))
            {
                throw new NotSupportedException("Unknown Grabber Media Format");
            }

            WaveFormatEx format = new WaveFormatEx();
            format = (WaveFormatEx)Marshal.PtrToStructure(mediaInfo.formatPtr, typeof(WaveFormatEx));
            Marshal.FreeCoTaskMem(mediaInfo.formatPtr);
            mediaInfo.formatPtr = IntPtr.Zero;

            return format;
        }

        #region Grabber

        public int SampleCB(double sampleTime, IMediaSample mediaSample)
        {
            Marshal.ReleaseComObject(mediaSample);
            return 0;
        }

        public int BufferCB(double SampleTime, IntPtr pBuffer, int BufferLen)
        {
            byte[] buffer = new byte[BufferLen];
            Marshal.Copy(pBuffer, buffer, 0, BufferLen);
            _audioEngine.PlayBuffer(buffer);
            return 0;
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (_audioEngine != null)
                _audioEngine.Dispose();
            base.Dispose(disposing);
        }
    }
}
