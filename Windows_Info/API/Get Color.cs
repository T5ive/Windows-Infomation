using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace TFive_Windows_Information
{
   public class GetColor_
    {
        public static bool CkStatus;
        public static string CkColor = "";
        [DllImport("user32.dll", SetLastError = true)] public static extern int GetWindowDC(int window);
        [DllImport("gdi32.dll", SetLastError = true)] public static extern uint GetPixel(int dc, int x, int y);
        [DllImport("user32.dll", SetLastError = true)] public static extern int ReleaseDC(int window, int dc);
       
        public static int PositionX;
        public static int PositionY;

        public static Color GetColorAt(int hWnd, int x, int y)
        {
            PositionX = x;
            PositionY = y;
            var dc = GetWindowDC(hWnd);
            var a = (int)GetPixel(dc, x, y);
            ReleaseDC(hWnd, dc);
            return Color.FromArgb(255, (a >> 0) & 0xff, (a >> 8) & 0xff, (a >> 16) & 0xff);
        }

        public static string GetColorString(int x, int y)
        {
            var appHandle = GetAppName.AppName;
            return HexConverterOld(GetColorAt(appHandle.ToInt32(), x, y));
        }

        private static string HexConverterOld(Color c) 
        {
            return $"0x{c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2")}";
        }

        public static int StringColor(string color)
        {
            var colorCut0X = color.Replace("0x", "");
            var intValue = int.Parse(colorCut0X, System.Globalization.NumberStyles.HexNumber);
            return intValue;
        }
        public static bool GetColorFast(IntPtr iHandle, int x, int y, int pixelColorX, int shadeVariation)
        {

            var appHandle = iHandle;
            var hexStr = $"{pixelColorX:x}";
            hexStr = hexStr.ToUpper();
            if (hexStr.Length == 5)
            {
                hexStr = "0x0" + hexStr;
            }
            else
            {
                hexStr = "0x" + hexStr;
            }
            if (HexConverterOld(GetColorAt(appHandle.ToInt32(), x, y)) == hexStr)
            {

                CkStatus = true;
                CkColor = HexConverterOld(GetColorAt(appHandle.ToInt32(), x, y));
            }
            else
            {
                CkStatus = false;
                CkColor = HexConverterOld(GetColorAt(appHandle.ToInt32(), x, y));
            }
            return CkStatus;
        }

    }
}
