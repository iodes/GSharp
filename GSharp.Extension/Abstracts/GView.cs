using System.Windows.Input;
using System.Windows.Controls;
using GSharp.Extension.Attributes;

namespace GSharp.Extension.Abstracts
{
    public abstract class GView : UserControl
    {
        #region 이벤트
        [GControl("클릭")]
        public new event ClickEventHandler Click;
        public delegate void ClickEventHandler();

        [GControl("마우스 이동")]
        public new event MouseMoveEventHandler MouseMove;
        public delegate void MouseMoveEventHandler();

        [GControl("마우스 이탈")]
        public new event MouseLeaveEventHandler MouseLeave;
        public delegate void MouseLeaveEventHandler();

        [GControl("마우스 진입")]
        public new event MouseEnterEventHandler MouseEnter;
        public delegate void MouseEnterEventHandler();
        #endregion

        #region 생성자
        public GView()
        {
            base.MouseEnter += GView_MouseEnter;
            base.MouseLeave += GView_MouseLeave;
            PreviewMouseMove += GView_PreviewMouseMove;
            PreviewMouseLeftButtonUp += GView_PreviewMouseLeftButtonUp;
        }
        #endregion

        #region 기본 이벤트
        private void GView_MouseEnter(object sender, MouseEventArgs e)
        {
            MouseEnter?.Invoke();
        }

        private void GView_MouseLeave(object sender, MouseEventArgs e)
        {
            MouseLeave?.Invoke();
        }

        private void GView_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            MouseMove?.Invoke();
        }

        private void GView_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Click?.Invoke();
        }
        #endregion
    }
}
