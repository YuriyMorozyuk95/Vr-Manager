using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using VrPlayer.Contracts.Medias;
using VrPlayer.Helpers.Mvvm;
using Brush = System.Windows.Media.Brush;
using Image = System.Windows.Controls.Image;

namespace VrPlayer.Medias.Gdi
{
    [DataContract]
    public class GdiMedia : MediaBase
    {
        private readonly Image _media;
        private readonly DispatcherTimer _timer;

        public override FrameworkElement Media
        {
            get
            {
                return _media;
            }
        }

        public static readonly DependencyProperty ProcessProperty =
            DependencyProperty.Register("Process", typeof(Process),
            typeof(GdiMedia), new FrameworkPropertyMetadata(null));
        public Process Process
        {
            get { return (Process)GetValue(ProcessProperty); }
            set { SetValue(ProcessProperty, value); }
        }

        public static Brush CreateBrushFromBitmap(Bitmap bmp)
        {
            return new ImageBrush(Imaging.CreateBitmapSourceFromHBitmap(bmp.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()));
        }

        public GdiMedia()
        {
            //Commands
            OpenFileCommand = new RelayCommand(o => { }, o => false);
            OpenDiscCommand = new RelayCommand(o => { }, o => false);
            OpenStreamCommand = new RelayCommand(o => { }, o => false);
            OpenDeviceCommand = new RelayCommand(o => { }, o => false);
            OpenProcessCommand = new RelayCommand(OpenProcess);

            _media = new Image();
            _timer = new DispatcherTimer(DispatcherPriority.Send);
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 125);
            _timer.Tick += TimerOnTick;
        }

        private void TimerOnTick(object sender, EventArgs eventArgs)
        {
            if (Process == null || Process.MainWindowHandle == IntPtr.Zero) return;
            try
            {
                _media.Source = WindowsCapture.CaptureWindow(Process.MainWindowHandle).ToImageSource();
            }
            catch (Exception exc)
            {
                _timer.Stop();
            }
            OnPropertyChanged("Media");
        }

        public override void Load()
        {
        }

        public override void Unload()
        {
            _timer.Stop();
        }

        private void OpenProcess(object o)
        {
            if (o == null) return;
            Process = (Process)o;
            _timer.Start();
            OnPropertyChanged("Media");
        }
    }

    public static class BitmapExtensions
    {
        public static ImageSource ToImageSource(this Bitmap bitmap)
        {
            var hbitmap = bitmap.GetHbitmap();
            try
            {
                var imageSource = Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(bitmap.Width, bitmap.Height));

                return imageSource;
            }
            finally
            {
                NativeMethods.DeleteObject(hbitmap);
            }
        }
    }
}
