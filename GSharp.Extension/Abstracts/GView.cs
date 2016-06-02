using System.Windows.Controls;

namespace GSharp.Extension.Abstracts
{
    public abstract class GView : UserControl
    {
        #region 이벤트
        public event ClickEventHandler Click;
        public delegate void ClickEventHandler();

        public new event MouseMoveEventHandler MouseMove;
        public delegate void MouseMoveEventHandler();

        public new event MouseLeaveEventHandler MouseLeave;
        public delegate void MouseLeaveEventHandler();

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
