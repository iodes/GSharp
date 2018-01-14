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
using GSharp.Manager;
using GSharpSample.Natives;
using GSharpSample.Utilities;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GSharpSample.Pages
{
    /// <summary>
    /// PageWorkspace.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PageWorkspace : UserControl
    {
        #region 변수
        private bool dragReady = false;
        private bool dropComplete = false;
        private Window dragEffectWindow;
        private BaseBlock lastDragBlock;
        private Point startPoint;
        private GCompiler compiler = new GCompiler();
        private GCompilerConfig config = new GCompilerConfig();
        #endregion

        #region 속성
        public bool IsDebugging { get; set; } = false;
        #endregion

        #region 생성자
        public PageWorkspace()
        {
            InitializeComponent();
            Loaded += PageWorkspace_Loaded;
        }
        #endregion

        #region 이벤트
        private void PageWorkspace_Loaded(object sender, RoutedEventArgs e)
        {
            // 기본 블럭 추가
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

            // 확장 블럭 추가
            if (Directory.Exists("Extensions"))
            {
                var extension = new ExtensionManager("Extensions");
                foreach (var target in extension.Extensions)
                {
                    foreach (BaseBlock block in extension.ConvertToBlocks(target))
                    {
                        block.IsPreview = true;
                        listBlock.Items.Add(block);
                    }
                }
            }

            // 간격 및 이벤트 설정
            foreach (BaseBlock block in listBlock.Items)
            {
                block.Margin = new Thickness(0, 0, 0, 10);
                SetBlockEvent(block);
            }
        }
        #endregion

        #region 내부 함수
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

            if (value.GetType().GetInterfaces().Contains(typeof(IModuleBlock)))
            {
                var moduleBlock = value as IModuleBlock;
                if (moduleBlock.GCommand.Parent != null)
                {
                    Task.Run(async () =>
                    {
                        await compiler.LoadReferenceAsync(moduleBlock.GCommand.Parent.Path);
                        await compiler.LoadDependenciesAsync(moduleBlock.GCommand.Parent.Dependencies);
                    });
                }
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

        private bool IsDragMove(MouseEventArgs e)
        {
            var diff = startPoint - e.GetPosition(this);
            return Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                   Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance;
        }
        #endregion

        #region 사용자 함수
        public void Build()
        {
            var saveDialog = new SaveFileDialog
            {
                Filter = "응용 프로그램 (*.exe)|*.exe"
            };

            if (saveDialog.ShowDialog().Value && Compile(saveDialog.FileName))
            {
                MessageBox.Show("컴파일을 성공하였습니다.", "성공", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void Debug()
        {
            string tempFile = System.IO.Path.GetTempFileName() + ".exe";
            
            if (Compile(tempFile))
            {
                IsDebugging = true;

                var process = Process.Start(tempFile);
                process.EnableRaisingEvents = true;

                process.Exited += delegate
                {
                    IsDebugging = false;
                };
            }
        }

        public bool Compile(string path)
        {
            bool isSuccess = false;

            try
            {
                compiler.Source = blockEditor.GetSource();

                var result = compiler.Build(path, config);
                isSuccess = result.IsSuccess;

                if (!result.IsSuccess)
                {
                    MessageBox.Show(result.Results.Errors[0].ErrorText, "오류", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return isSuccess;
        }

        public void ShowConfig()
        {
            var configDialog = new ConfigDialog
            {
                Owner = Application.Current.MainWindow,
                Config = config
            };

            configDialog.ShowDialog();
        }

        public string GetSource()
        {
            return blockEditor.GetSource();
        }
        #endregion

        #region 블럭 드래그 처리
        private void BlockEditor_Drop(object sender, DragEventArgs e)
        {
            dragReady = false;
            dropComplete = false;
            lastDragBlock = null;
        }

        private void BlockEditor_DragEnter(object sender, DragEventArgs e)
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

                        var mouse = PositionUtility.GetMousePosition();
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
                        WinAPI.SetWindowLong(hwnd, WinAPI.GWL_EXSTYLE, WinAPI.GetWindowLong(hwnd, WinAPI.GWL_EXSTYLE) | WinAPI.WS_EX_TRANSPARENT);

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
                var mouse = PositionUtility.GetMousePosition();
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
