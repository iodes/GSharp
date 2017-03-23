using GSharp.Base.Objects.Numbers;
using GSharp.Compile;
using GSharp.Extension;
using GSharp.Graphic.Blocks;
using GSharp.Graphic.Objects;
using GSharp.Graphic.Objects.Lists;
using GSharp.Graphic.Objects.Logics;
using GSharp.Graphic.Objects.Numbers;
using GSharp.Graphic.Objects.Strings;
using GSharp.Graphic.Scopes;
using GSharp.Graphic.Statements;
using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GSharpSample
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        #region 변수
        private bool dragReady = false;
        private bool dropComplete = false;
        private Window dragEffectWindow;
        private BaseBlock lastDragBlock;
        private Point startPoint;
        #endregion

        #region 생성자
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }
        #endregion

        #region 이벤트
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            listBlock.Items.Add(new EventBlock(new GCommand("this", "Loaded", "프로그램이 시작될 때", typeof(void), GCommand.CommandType.Event)));
            listBlock.Items.Add(new EventBlock(new GCommand("this", "Closing", "프로그램이 종료될 때", typeof(void), GCommand.CommandType.Event)));

            listBlock.Items.Add(new IfBlock());
            listBlock.Items.Add(new IfElseBlock());
            listBlock.Items.Add(new SetBlock());

            listBlock.Items.Add(new GateBlock());
            listBlock.Items.Add(new CompareBlock());

            listBlock.Items.Add(new EmptyListBlock());
            listBlock.Items.Add(new ItemBlock());
            listBlock.Items.Add(new ListAddBlock());

            listBlock.Items.Add(new ComputeBlock(GCompute.OperatorType.PLUS));
            listBlock.Items.Add(new ComputeBlock(GCompute.OperatorType.MINUS));
            listBlock.Items.Add(new ComputeBlock(GCompute.OperatorType.MULTIPLY));
            listBlock.Items.Add(new ComputeBlock(GCompute.OperatorType.DIVISION));
            listBlock.Items.Add(new ComputeBlock(GCompute.OperatorType.MODULO));

            listBlock.Items.Add(new StringCatBlock());
            listBlock.Items.Add(new LengthBlock());

            listBlock.Items.Add(new LoopInfinityBlock());
            listBlock.Items.Add(new LoopNBlock());

            foreach (BaseBlock block in listBlock.Items)
            {
                block.Margin = new Thickness(0, 0, 0, 10);
                SetBlockEvent(block);
            }
        }

        private void btnConvert_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(blockEditor.GetSource());
        }

        private void btnStdCompile_Click(object sender, RoutedEventArgs e)
        {
            Compile(false);
        }

        private void btnCompCompile_Click(object sender, RoutedEventArgs e)
        {
            Compile(true);
        }
        #endregion

        #region 내부 함수
        private void Compile(bool isCompressed)
        {
            var compiler = new GCompiler
            {
                Source = blockEditor.GetSource()
            };

            var saveDialog = new SaveFileDialog
            {
                Filter = "응용 프로그램 (*.exe)|*.exe"
            };

            if (saveDialog.ShowDialog().Value)
            {
                var result = compiler.Build(saveDialog.FileName, isCompressed, isCompressed);
                MessageBox.Show(result.IsSuccess ? "컴파일 성공" : "컴파일 실패", "결과", MessageBoxButton.OK, result.IsSuccess ? MessageBoxImage.Information : MessageBoxImage.Error);
            }
        }

        private BaseBlock CopyBlock(object value)
        {
            object[] args = null;

            if (value is IModuleBlock)
            {
                var oldBlock = value as IModuleBlock;
                args = new object[] { oldBlock.GCommand };
            }
            else if (value is VariableBlock)
            {
                var oldBlock = value as VariableBlock;
                args = new object[] { oldBlock.FriendlyName, oldBlock.GVariable };
            }
            else if (value is FunctionBlock)
            {
                var oldBlock = value as FunctionBlock;
                args = new object[] { oldBlock.GFunction };
            }
            else if (value is ComputeBlock)
            {
                args = new object[] { (value as ComputeBlock).OperatorType };
            }

            return Activator.CreateInstance(value.GetType(), args) as BaseBlock;
        }

        private BaseBlock SetBlockEvent(BaseBlock block)
        {
            block.GiveFeedback += Block_GiveFeedback;
            block.PreviewMouseMove += Block_PreviewMouseMove;
            block.QueryContinueDrag += Block_QueryContinueDrag;
            block.PreviewMouseLeftButtonUp += Block_PreviewMouseLeftButtonUp;
            block.PreviewMouseLeftButtonDown += Block_PreviewMouseLeftButtonDown;

            return block;
        }

        private Point GetDPI()
        {
            var deviceMatrix = PresentationSource.FromVisual(Application.Current.MainWindow).CompositionTarget.TransformToDevice;
            return new Point(deviceMatrix.M11, deviceMatrix.M22);
        }

        private Point GetMousePosition()
        {
            var displayDPI = GetDPI();

            Win32Point mousePoint = new Win32Point();
            GetCursorPos(ref mousePoint);

            return new Point(mousePoint.X / displayDPI.X, mousePoint.Y / displayDPI.Y);
        }

        private bool IsDragMove(MouseEventArgs e)
        {
            var diff = startPoint - e.GetPosition(this);
            return Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance || Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance;
        }
        #endregion

        #region 네이티브 함수
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Win32Point
        {
            public int X;
            public int Y;
        };

        public const int GWL_EXSTYLE = (-20);
        public const int WS_EX_TRANSPARENT = 0x00000020;

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);
        #endregion

        #region 블럭 드래그 처리
        private void blockEditor_Drop(object sender, DragEventArgs e)
        {
            dragReady = false;
            dropComplete = false;
            lastDragBlock = null;
        }

        private void blockEditor_DragEnter(object sender, DragEventArgs e)
        {
            if (!dropComplete)
            {
                var originalBlock = e.Data.GetData("DragSource") as BaseBlock;
                if (originalBlock != null)
                {
                    lastDragBlock = CopyBlock(originalBlock);
                    lastDragBlock.Position = e.GetPosition(blockEditor.Master);

                    blockEditor.AddBlock(lastDragBlock);
                    blockEditor.StartBlockMove(lastDragBlock);
                }

                dropComplete = true;
            }
        }

        private void Block_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() =>
            {
                if (e.LeftButton == MouseButtonState.Pressed && dragReady)
                {
                    if (IsDragMove(e))
                    {
                        var target = sender as BaseBlock;

                        var visulSource = new Rectangle
                        {
                            Width = target.ActualWidth,
                            Height = target.ActualHeight,
                            Fill = new VisualBrush(target)
                        };

                        var mouse = GetMousePosition();
                        dragEffectWindow = new Window
                        {
                            Top = mouse.Y,
                            Left = mouse.X,
                            Opacity = 0.7,
                            Background = null,
                            Width = target.ActualWidth,
                            Height = target.ActualHeight,
                            WindowStyle = WindowStyle.None,
                            AllowsTransparency = true,
                            IsHitTestVisible = false,
                            ShowActivated = false,
                            ShowInTaskbar = false,
                            Topmost = true,
                            Content = visulSource
                        };
                        dragEffectWindow.Show();

                        var hwnd = new WindowInteropHelper(dragEffectWindow).Handle;
                        SetWindowLong(hwnd, GWL_EXSTYLE, GetWindowLong(hwnd, GWL_EXSTYLE) | WS_EX_TRANSPARENT);

                        var dataObj = new DataObject();
                        dataObj.SetData("DragSource", sender);
                        DragDrop.DoDragDrop(target, dataObj, DragDropEffects.Move);

                        dragEffectWindow?.Close();
                        dragEffectWindow = null;
                    }
                }
            }));
        }

        private void Block_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (dragEffectWindow != null)
            {
                var mouse = GetMousePosition();
                dragEffectWindow.Top = mouse.Y;
                dragEffectWindow.Left = mouse.X;
            }
        }

        private void Block_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            if (dropComplete)
            {
                e.Handled = true;
                e.Action = DragAction.Drop;
            }
        }

        private void Block_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            dragReady = false;
        }

        private void Block_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            dragReady = true;
            startPoint = e.GetPosition(this);
        }
        #endregion
    }
}
