using GSharpSample.Natives;
using System.Windows;

namespace GSharpSample.Utilities
{
    public static class PositionUtility
    {
        public static Point GetDPI()
        {
            var deviceMatrix = PresentationSource.FromVisual(Application.Current.MainWindow).CompositionTarget.TransformToDevice;
            return new Point(deviceMatrix.M11, deviceMatrix.M22);
        }

        public static Point GetMousePosition()
        {
            var displayDPI = GetDPI();

            var mousePoint = new WinAPI.Win32Point();
            WinAPI.GetCursorPos(ref mousePoint);

            return new Point(mousePoint.X / displayDPI.X, mousePoint.Y / displayDPI.Y);
        }
    }
}
