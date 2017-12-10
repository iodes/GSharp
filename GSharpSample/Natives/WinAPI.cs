using System;
using System.Runtime.InteropServices;

namespace GSharpSample.Natives
{
    public static class WinAPI
    {
        public const int GWL_EXSTYLE = (-20);
        public const int WS_EX_TRANSPARENT = 0x00000020;

        [StructLayout(LayoutKind.Sequential)]
        public struct Win32Point
        {
            public int X;
            public int Y;
        };

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(ref Win32Point pt);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);
    }
}
