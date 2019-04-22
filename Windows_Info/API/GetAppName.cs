using System;
using System.Runtime.InteropServices;

namespace TFive_Windows_Information
{
     public  class GetAppName
    {
        [DllImport("User32.dll")] public static extern IntPtr FindWindow(string strClassName, string strWindowName);
        public static string App = ""; 
        public static string Class = "";
        public static IntPtr AppName;// = FindWindow(CLASS, APP);

        public static void GetWindow()
        {
            AppName = FindWindow(Class, App);
        }
    }
}
