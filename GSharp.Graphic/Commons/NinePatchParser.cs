using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GSharp.Graphic.Commons
{
    public class NinePatchParser
    {
        #region 변수
        private BitmapSource _source;
        #endregion

        #region 속성
        public bool IsUpdated { get; private set; } = false;

        public Thickness Padding { get; set; } = new Thickness();

        public CroppedBitmap TopLeft { get; private set; }

        public CroppedBitmap TopCenter { get; private set; }

        public CroppedBitmap TopRight { get; private set; }

        public CroppedBitmap MiddleLeft { get; private set; }

        public CroppedBitmap MiddleCenter { get; private set; }

        public CroppedBitmap MiddleRight { get; private set; }

        public CroppedBitmap BottomLeft { get; private set; }

        public CroppedBitmap BottomCenter { get; private set; }

        public CroppedBitmap BottomRight { get; private set; }

        public CroppedBitmap Original { get; private set; }

        public BitmapSource Source
        {
            get => _source;
            set
            {
                _source = value;
                UpdateBitmap();
            }
        }
        #endregion

        #region 생성자
        public NinePatchParser()
        {

        }

        public NinePatchParser(BitmapSource source) : base()
        {
            Source = source;
        }
        #endregion

        #region 내부 함수
        private bool TryGetFillX(BitmapSource bitmap, out Int32Rect result)
        {
            var fillsX = new List<int>();

            for (int x = 0; x < bitmap.PixelWidth; x++)
            {
                if (GetPixel(bitmap, x, bitmap.PixelHeight - 1) == Colors.Black)
                {
                    fillsX.Add(x);
                }
            }

            if (fillsX.Any())
            {
                result = new Int32Rect(fillsX.First(), 0, fillsX.Last() - fillsX.First() + 1, 1);
                return true;
            }

            result = default;
            return false;
        }

        private bool TryGetFillY(BitmapSource bitmap, out Int32Rect result)
        {
            var fillsY = new List<int>();

            for (int y = 0; y < bitmap.PixelHeight; y++)
            {
                if (GetPixel(bitmap, bitmap.PixelWidth - 1, y) == Colors.Black)
                {
                    fillsY.Add(y);
                }
            }

            if (fillsY.Any())
            {
                result = new Int32Rect(0, fillsY.First(), 1, fillsY.Last() - fillsY.First() + 1);
                return true;
            }

            result = default;
            return false;
        }

        private bool TryGetScalableX(BitmapSource bitmap, out Int32Rect result)
        {
            var scalablesX = new List<int>();

            for (int x = 0; x < bitmap.PixelWidth; x++)
            {
                if (GetPixel(bitmap, x, 0) == Colors.Black)
                {
                    scalablesX.Add(x);
                }
            }

            if (scalablesX.Any())
            {
                result = new Int32Rect(scalablesX.First(), 0, scalablesX.Last() - scalablesX.First() + 1, 1);
                return true;
            }

            result = default;
            return false;
        }

        private bool TryGetScalableY(BitmapSource bitmap, out Int32Rect result)
        {
            var scalablesY = new List<int>();

            for (int y = 0; y < bitmap.PixelHeight; y++)
            {
                if (GetPixel(bitmap, 0, y) == Colors.Black)
                {
                    scalablesY.Add(y);
                }
            }

            if (scalablesY.Any())
            {
                result = new Int32Rect(0, scalablesY.First(), 1, scalablesY.Last() - scalablesY.First() + 1);
                return true;
            }

            result = default;
            return false;
        }

        private Color GetPixel(BitmapSource bitmap, int x, int y)
        {
            if (x <= bitmap.PixelWidth && y <= bitmap.PixelHeight)
            {
                var pixels = new byte[4];
                var cropped = new CroppedBitmap(bitmap, new Int32Rect(x, y, 1, 1));
                cropped.CopyPixels(pixels, 4, 0);

                return Color.FromArgb(pixels[3], pixels[2], pixels[1], pixels[0]);
            }

            return Colors.Transparent;
        }
        #endregion

        #region 사용자 함수
        public void UpdateBitmap()
        {
            if (Source != null)
            {
                var resultScalableX = TryGetScalableX(Source, out Int32Rect scalableX);
                var resultScalableY = TryGetScalableY(Source, out Int32Rect scalableY);

                if (!resultScalableX || !resultScalableY)
                {
                    IsUpdated = false;
                    return;
                }

                var scalableXRight = scalableX.X + scalableX.Width;
                var scalableYBottom = scalableY.Y + scalableY.Height;

                // Top Area
                TopLeft = new CroppedBitmap(Source, new Int32Rect(1, 1, scalableX.X - 1, scalableY.Y - 1));
                TopCenter = new CroppedBitmap(Source, new Int32Rect(scalableX.X, 1, scalableX.Width, scalableY.Y - 1));
                TopRight = new CroppedBitmap(Source, new Int32Rect(scalableXRight, 1, Source.PixelWidth - scalableXRight - 1, scalableY.Y - 1));

                // Middle Area
                MiddleLeft = new CroppedBitmap(Source, new Int32Rect(1, scalableY.Y, TopLeft.PixelWidth, scalableY.Height));
                MiddleCenter = new CroppedBitmap(Source, new Int32Rect(scalableX.X, scalableY.Y, scalableX.Width, scalableY.Height));
                MiddleRight = new CroppedBitmap(Source, new Int32Rect(scalableXRight, scalableY.Y, TopRight.PixelWidth, scalableY.Height));

                // Bottom Area
                BottomLeft = new CroppedBitmap(Source, new Int32Rect(1, scalableYBottom, MiddleLeft.PixelWidth, Source.PixelHeight - scalableYBottom - 1));
                BottomCenter = new CroppedBitmap(Source, new Int32Rect(scalableX.X, scalableYBottom, scalableX.Width, BottomLeft.PixelHeight));
                BottomRight = new CroppedBitmap(Source, new Int32Rect(scalableXRight, scalableYBottom, MiddleRight.PixelWidth, BottomLeft.PixelHeight));

                // Full Area
                Original = new CroppedBitmap(Source, new Int32Rect(1, 1, Source.PixelWidth - 2, Source.PixelHeight - 2));

                // Padding
                var resultFillX = TryGetFillX(Source, out Int32Rect fillX);
                var resultFillY = TryGetFillY(Source, out Int32Rect fillY);
                if (resultFillX && resultFillY)
                {
                    Padding = new Thickness(fillX.X - 1, fillY.Y - 1, Source.PixelWidth - (fillX.X + fillX.Width) - 1, Source.PixelHeight - (fillY.Y + fillY.Height) - 1);
                }

                IsUpdated = true;
            }
        }
        #endregion
    }
}
