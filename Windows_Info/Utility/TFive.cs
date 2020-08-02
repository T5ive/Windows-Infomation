using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;

namespace TFive_Windows_Information
{
    public class TFive
    {
        #region Get App Name

        #region Dll Import

        [DllImport("User32.dll")] private static extern IntPtr FindWindow(string strClassName, string strWindowName);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)] private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)] private static extern int GetClassName(IntPtr hWnd, StringBuilder text, int count);

        #endregion

        #region Var

        public static string App = "";
        public static string Class = "";
        public static IntPtr AppName;

        #endregion Var

        public static void GetAppName()
        {
            AppName = FindWindow(Class, App);
        }

        public static string GetWindowsTitle(IntPtr iHandle)
        {
            const int nChars = 256;
            var buff = new StringBuilder(nChars);
            return GetWindowText(iHandle, buff, nChars) > 0 ? buff.ToString() : null;
        }
        public static string GetWindowsClassName(IntPtr iHandle)
        {
            const int nChars = 256;
            var buff = new StringBuilder(nChars);
            return GetClassName(iHandle, buff, nChars) > 0 ? buff.ToString() : null;
        }

        #endregion

        #region Get Colours

        #region DllImport
        [DllImport("user32.dll", EntryPoint = "GetDC")] private static extern IntPtr GetDC(IntPtr hWnd);
        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC")] private static extern IntPtr CreateCompatibleDC(IntPtr hdc);
        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap")] private static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);
        [DllImport("gdi32.dll", EntryPoint = "DeleteDC")] private static extern IntPtr DeleteDC(IntPtr hDc);
        [DllImport("user32.dll", EntryPoint = "ReleaseDC")] private static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);
        [DllImport("gdi32.dll", EntryPoint = "BitBlt")] private static extern bool BitBlt(IntPtr hdcDest, int xDest, int yDest, int wDest, int hDest, IntPtr hdcSource, int xSrc, int ySrc, int rasterOp);
        [DllImport("gdi32.dll", EntryPoint = "SelectObject")] private static extern IntPtr SelectObject(IntPtr hdc, IntPtr bmp);
        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")] private static extern IntPtr DeleteObject(IntPtr hDc);
        [DllImport("user32.dll", SetLastError = true)] private static extern int GetWindowDC(int window);
        [DllImport("gdi32.dll", SetLastError = true)] private static extern uint GetPixel(int dc, int x, int y);
        [DllImport("user32.dll", SetLastError = true)] private static extern int ReleaseDC(int window, int dc);
        [DllImport("user32.dll")] private static extern bool GetWindowRect(IntPtr handle, ref Rectangle rect);

        #endregion

        #region Var

        private static Size _winSize;

        #endregion

        private static Color GetColorAt(int hWnd, int x, int y)
        {
            var dc = GetWindowDC(hWnd);
            var a = (int)GetPixel(dc, x, y);
            ReleaseDC(hWnd, dc);
            return Color.FromArgb(255, (a >> 0) & 0xff, (a >> 8) & 0xff, (a >> 16) & 0xff);
        }
        public static Color GetColorAt(int x, int y)
        {
            var dc = GetWindowDC(AppName.ToInt32());
            var a = (int)GetPixel(dc, x, y);
            ReleaseDC(AppName.ToInt32(), dc);
            return Color.FromArgb(255, (a >> 0) & 0xff, (a >> 8) & 0xff, (a >> 16) & 0xff);
        }

        public static Size GetControlSize(IntPtr iHandle)
        {
            var pRect = new Rectangle();
            GetWindowRect(iHandle, ref pRect);
            _winSize.Width = pRect.Right - pRect.Left;
            _winSize.Height = pRect.Bottom - pRect.Top;
            return _winSize;
        }
        public static string GetHexColor(int x, int y) => Color2Hex(GetColorAt(AppName.ToInt32(), x, y));
        private static string Color2Hex(Color c) => $"0x{c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2")}";

        public static bool GetColor(IntPtr iHandle, int x, int y, int pixelColor)
        {
            var appHandle = iHandle.ToInt32();
            var hexStr = $"{pixelColor:x}".ToUpper();
            hexStr = hexStr.Length == 5 ? "0x0" + hexStr : "0x" + hexStr;
            return Color2Hex(GetColorAt(appHandle, x, y)) == hexStr;
        }

        public static int StringColor(string color) => int.Parse(color.Replace("0x", ""), System.Globalization.NumberStyles.HexNumber);

        public static Color GetColorToBg(string color) => ColorTranslator.FromHtml(color);

        #endregion

        #region Pixel Search

     
        #endregion

        #region Images Search


        #endregion

        #region Utility

      
        #endregion

        #region Mouse



        #endregion

        #region Keyboard



        #endregion

    }
}