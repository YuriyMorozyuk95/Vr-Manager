//Based on: http://stackoverflow.com/questions/891345/get-a-screenshot-of-a-specific-application
using System;
using System.Drawing;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace VrPlayer.Medias.Gdi
{
    public class WindowsCapture
    {
        public static Bitmap CaptureWindow(IntPtr hWnd)  
        {       
            Rect rc;
            NativeMethods.GetWindowRect(hWnd, out rc);

            var bmp = new Bitmap(rc.Width, rc.Height, PixelFormat.Format32bppArgb);        
            var gfxBmp = Graphics.FromImage(bmp);        
            var hdcBitmap = gfxBmp.GetHdc();

            NativeMethods.PrintWindow(hWnd, hdcBitmap, 0);  

            gfxBmp.ReleaseHdc(hdcBitmap);               
            gfxBmp.Dispose(); 

            return bmp;   
        }
    }
}
