using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WPFExtension;

namespace GSharp.Graphic.Commons
{
    public class NinePatchPanel : ContentControl
    {
        #region 변수
        Rect _drawTopLeft;
        Rect _drawTopCenter;
        Rect _drawTopRight;

        Rect _drawMiddleLeft;
        Rect _drawMiddleCenter;
        Rect _drawMiddleRight;

        Rect _drawBottomLeft;
        Rect _drawBottomCenter;
        Rect _drawBottomRight;
        #endregion

        #region 속성
        [Category("Common Properties")]
        public NinePatchParser Parser
        {
            get { return (NinePatchParser)GetValue(ParserProperty); }
            set { SetValue(ParserProperty, value); }
        }

        public static readonly DependencyProperty ParserProperty =
            DependencyHelper.Register(new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnSourceChanged)));
        #endregion

        #region 이벤트
        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as NinePatchPanel).InvalidateVisual();
        }
        #endregion

        #region 부모 함수
        protected override Size MeasureOverride(Size availableSize)
        {
            if (Parser?.IsUpdated == true)
            {
                _drawTopLeft = new Rect(-Parser.Padding.Left, -Parser.Padding.Top, Parser.TopLeft.PixelWidth, Parser.TopLeft.PixelHeight);

                var availableWidth = Math.Max(availableSize.Width - _drawTopLeft.Width - Parser.TopRight.PixelWidth, Parser.TopCenter.PixelWidth);
                var availablePaddingWidth = availableWidth + Parser.Padding.Left + Parser.Padding.Right;

                _drawTopCenter = new Rect(_drawTopLeft.Right, -Parser.Padding.Top, availablePaddingWidth, Parser.TopCenter.PixelHeight);
                _drawTopRight = new Rect(_drawTopCenter.Right, -Parser.Padding.Top, Parser.TopRight.PixelWidth, Parser.TopRight.PixelHeight);

                var availableHeight = Math.Max(availableSize.Height - _drawTopLeft.Height - Parser.BottomLeft.PixelHeight, Parser.MiddleLeft.PixelHeight);
                var availablePaddingHeight = availableHeight + Parser.Padding.Top + Parser.Padding.Bottom;

                _drawMiddleLeft = new Rect(_drawTopLeft.Left, _drawTopLeft.Bottom, Parser.MiddleLeft.PixelWidth, availablePaddingHeight);
                _drawMiddleCenter = new Rect(_drawMiddleLeft.Right, _drawTopCenter.Bottom, _drawTopCenter.Width, _drawMiddleLeft.Height);
                _drawMiddleRight = new Rect(_drawMiddleCenter.Right, _drawTopRight.Bottom, Parser.MiddleRight.PixelWidth, _drawMiddleLeft.Height);

                _drawBottomLeft = new Rect(_drawMiddleLeft.Left, _drawMiddleLeft.Bottom, Parser.BottomLeft.PixelWidth, Parser.BottomLeft.PixelHeight);
                _drawBottomCenter = new Rect(_drawBottomLeft.Right, _drawMiddleCenter.Bottom, _drawMiddleCenter.Width, _drawBottomLeft.Height);
                _drawBottomRight = new Rect(_drawBottomCenter.Right, _drawMiddleRight.Bottom, Parser.BottomRight.PixelWidth, _drawBottomLeft.Height);
            }

            return base.MeasureOverride(availableSize);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (Parser?.IsUpdated == true)
            {
                drawingContext.DrawImage(Parser.TopLeft, _drawTopLeft);
                drawingContext.DrawImage(Parser.TopCenter, _drawTopCenter);
                drawingContext.DrawImage(Parser.TopRight, _drawTopRight);

                drawingContext.DrawImage(Parser.MiddleLeft, _drawMiddleLeft);
                drawingContext.DrawImage(Parser.MiddleCenter, _drawMiddleCenter);
                drawingContext.DrawImage(Parser.MiddleRight, _drawMiddleRight);

                drawingContext.DrawImage(Parser.BottomLeft, _drawBottomLeft);
                drawingContext.DrawImage(Parser.BottomCenter, _drawBottomCenter);
                drawingContext.DrawImage(Parser.BottomRight, _drawBottomRight);
            }
        }

        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
        {
            return null;
        }
        #endregion
    }
}
