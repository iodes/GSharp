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
            PreviewMouseLeftButtonUp += GView_PreviewMouseLeftButtonUp;
            PreviewMouseMove += GView_PreviewMouseMove;
            MouseLeave += GView_MouseLeave;
            MouseEnter += GView_MouseEnter;
        }
        #endregion

        #region 기본 이벤트
        private void GView_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Click?.Invoke();
        }

        private void GView_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            MouseMove?.Invoke();
        }

        private void GView_MouseLeave()
        {
            MouseLeave?.Invoke();
        }

        private void GView_MouseEnter()
        {
            MouseEnter?.Invoke();
        }
        #endregion
    }
}
