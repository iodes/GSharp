using GSharp.Support.Utilities;
using GSharpSample.Pages;
using System.Windows;

namespace GSharpSample
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        #region 변수
        private LayoutMac layoutMac = new LayoutMac();
        private LayoutWindows layoutWindows = new LayoutWindows();
        #endregion

        #region 생성자
        public MainWindow()
        {
            InitializeComponent();

            switch (EnvironmentUtility.Type)
            {
                case EnvironmentUtility.EnvironmentType.Native:
                    presenter.Content = layoutWindows;
                    break;

                case EnvironmentUtility.EnvironmentType.Wine:
                    presenter.Content = layoutMac;
                    break;
            }
        }
        #endregion
    }
}
