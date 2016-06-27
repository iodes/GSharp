using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Collections.Generic;
using GSharp.Base.Cores;
using GSharp.Base.Scopes;
using GSharp.Base.Singles;
using GSharp.Graphic.Holes;
using GSharp.Graphic.Blocks;
using GSharp.Graphic.Scopes;
using GSharp.Graphic.Objects;
using GSharp.Graphic.Statements;
using GSharp.Extension;
using System.ComponentModel;
using System.Windows.Threading;
using GSharp.Compile;

namespace GSharp.Graphic.Controls
{
    /// <summary>
    /// BlockEditor.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class BlockEditor : UserControl, INotifyPropertyChanged
    {
        #region 속성
        /// <summary>
        /// 모든 블럭 객체를 포함하는 최상위 객체를 가져옵니다.
        /// </summary>
        public Grid Master
        {
            get
            {
                return BlockGrid;
            }
        }

        public Dictionary<string, GControl> GControlList
        {
            get
            {
                return _GControlList;
            }
            set
            {
                _GControlList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GControlList"));
            }
        }
        private Dictionary<string, GControl> _GControlList = new Dictionary<string, GControl>();
        #endregion

        #region 객체
        // 선택한 블럭
        private BaseBlock SelectedBlock;

        // 마지막 선택 블럭
        private BaseBlock LastSelectedBlock;

        // 블럭을 잡은 마우스 위치
        private Point SelectedPosition;

        // 자석 효과 대상
        private BaseHole MargnetTarget;

        // 자석 효과 대상 하이라이팅 효과
        private Rectangle Highlighter = new Rectangle
        {
            HorizontalAlignment = HorizontalAlignment.Left,
            VerticalAlignment = VerticalAlignment.Top,
            Width = 10,
            Height = 10,
            Fill = Brushes.Green
        };

        // Hole 목록
        // Statement Hole
        private List<NextConnectHole> NextConnectHoleList = new List<NextConnectHole>();

        // Object Hole
        private List<BaseObjectHole> ObjectHoleList = new List<BaseObjectHole>();
        private List<SettableObjectHole> SettableObjectHoleList = new List<SettableObjectHole>();

        // 선택된 대상이 붙을 수 있는 Hole 목록
        private List<BaseHole> CanAttachHoleList = new List<BaseHole>();
        
        // 선택된 대상이 움직였는지 체크
        private bool IsSelectedBlockMoved = false;

        // 변수 목록
        public Dictionary<string, VariableBlock> GlobalVariableBlockList { get; } = new Dictionary<string, VariableBlock>();
        
        // 함수 목록
        private Dictionary<string, FunctionBlock> FunctionList = new Dictionary<string, FunctionBlock>();
        #endregion

        #region 생성자
        public BlockEditor()
        {
            InitializeComponent();

            ContextDelete.Click += ContextDelete_Click;
            ContextVariable.Click += ContextVariable_Click;
            ContextFunction.Click += ContextFunction_Click;

            BlockViewer.PreviewMouseMove += BlockViewer_PreviewMouseMove;
            BlockViewer.PreviewMouseDown += BlockViewer_PreviewMouseDown;
            BlockViewer.PreviewMouseUp += BlockViewer_PreviewMouseUp;
            BlockViewer.ContextMenuOpening += BlockViewer_ContextMenuOpening;

            Panel.SetZIndex(Highlighter, int.MaxValue - 1);
            Master.Children.Add(Highlighter);

            DispatcherTimer eventTimer = new DispatcherTimer();
            eventTimer.Interval = TimeSpan.FromMilliseconds(100);
            eventTimer.Tick += EventTimer_Tick;
            eventTimer.Start();
        }

        string backSource;
        private void EventTimer_Tick(object sender, EventArgs e)
        {
            string source = GetSource();
            if (backSource != source)
            {
                backSource = source;
                BlockChanged?.Invoke();
            }
        }

        private void BlockViewer_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (MouseOnBlock())
            {
                ContextDelete.Visibility = Visibility.Visible;
                ContextSeparator.Visibility = Visibility.Visible;

            }
            else
            {
                ContextDelete.Visibility = Visibility.Collapsed;
                ContextSeparator.Visibility = Visibility.Collapsed;
            }
        }
        #endregion

        #region 이벤트
        public event BlockChangedEventHandler BlockChanged;
        public delegate void BlockChangedEventHandler();

        public event LoadRequestedHandler LoadRequested;
        public delegate void LoadRequestedHandler(string xaml);

        public event LoadCompletedHandler LoadCompleted;
        public delegate void LoadCompletedHandler(string[] references, string[] dependencies);

        public event CreateVariableRequestedHandler CreateVariableRequested;
        public delegate void CreateVariableRequestedHandler();

        public event CreateFunctionRequestedHandler CreateFunctionRequested;
        public event PropertyChangedEventHandler PropertyChanged;

        public delegate void CreateFunctionRequestedHandler();

        private void Block_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;

            BaseBlock target = sender as BaseBlock;
            LastSelectedBlock = target;
            StartBlockMove(target, e.GetPosition(target));
        }

        private void Block_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            LastSelectedBlock = sender as BaseBlock;
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            // 선택한 블럭이 없으면 안됨
            if (SelectedBlock == null)
            {
                return;
            }

            // 마우스 캡쳐 상태 처리
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (!IsMouseCaptured)
                {
                    CaptureMouse();
                }
            }
            else
            {
                ReleaseMouseCapture();
            }

            if (!IsSelectedBlockMoved)
            {
                // 부모가 캔버스가 아닐때만 (null -> canvas)
                if (SelectedBlock.ParentHole != null)
                {
                    // 선택 블럭을 부모와 연결 해제
                    SelectedBlock.ParentHole.DetachBlock();

                    // 선택 블럭을 캔버스에 추가
                    AttachToCanvas(SelectedBlock);
                }

                IsSelectedBlockMoved = true;
            }

            // 마우스 좌표로 블럭 이동
            Point position = e.GetPosition(Master);
            SelectedBlock.Position = new Point(position.X - SelectedPosition.X, position.Y - SelectedPosition.Y);
            if (SelectedBlock.Position.X < 0)
            {
                SelectedBlock.Position = new Point(0, SelectedBlock.Position.Y);
            }

            if (SelectedBlock.Position.Y < 0)
            {
                SelectedBlock.Position = new Point(SelectedBlock.Position.X, 0);
            }

            // 연결할 대상을 찾고 하이라이팅
            MargnetBlock(SelectedBlock, e);
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // 선택한 블럭이 없으면 안됨
            if (SelectedBlock == null)
            {
                return;
            }

            // 마우스를 움직인 경우에만
            if (IsSelectedBlockMoved)
            {
                // 마우스 좌표로 블럭 이동
                Point position = e.GetPosition(Master);
                SelectedBlock.Position = new Point(position.X - SelectedPosition.X, position.Y - SelectedPosition.Y);
                if (SelectedBlock.Position.X < 0)
                {
                    SelectedBlock.Position = new Point(0, SelectedBlock.Position.Y);
                }

                if (SelectedBlock.Position.Y < 0)
                {
                    SelectedBlock.Position = new Point(SelectedBlock.Position.X, 0);
                }

                // 연결할 대상이 있다면 연결
                if (MargnetTarget != null)
                {
                    ConnectBlock(SelectedBlock, e);
                }
            }

            Panel.SetZIndex(SelectedBlock, 1);

            // Selected 해제
            SelectedBlock = null;
            MargnetTarget = null;
            Highlighter.Visibility = Visibility.Hidden;
            CanAttachHoleList.Clear();

            ReleaseMouseCapture();
        }
        #endregion

        #region 변수 선언
        // 변수 선언
        public VariableBlock DefineGlobalVariable(string varName)
        {
            string input = varName.Replace(" ", "_");

            if (GlobalVariableBlockList.Keys.Contains(input))
            {
                return null;
            }

            GlobalVariableBlockList[input] = BlockUtils.CreateVariableBlock(input, varName);
            return GlobalVariableBlockList[input];
        }

        // 변순 선언 해제
        public void UndefineGlobalVariable(string varName)
        {
            GlobalVariableBlockList.Remove(varName);
        }

        // 변수 목록
        public List<string> GetGlobalVariableNameList()
        {
            return GlobalVariableBlockList.Keys.ToList();
        }

        public List<VariableBlock> GetGlobalVariableBlockList()
        {
            return GlobalVariableBlockList.Values.ToList();
        }
        #endregion

        #region 함수 선언
        // 함수 선언
        public FunctionBlock DefineFunction(string funcName)
        {
            var function = new GFunction(funcName);
            return FunctionList[funcName] = new FunctionBlock(function);
        }

        // 함수 선언 해제
        public void UnDefineFunction(string funcName)
        {
            FunctionList.Remove(funcName);
        }

        // 함수 이름 목록
        public List<string> GetFunctionNameList()
        {
            return FunctionList.Keys.ToList();
        }

        // 함수 목록
        public List<FunctionBlock> GetFunctionBlockList()
        {
            return FunctionList.Values.ToList();
        }

        // 함수 가져오기
        public FunctionBlock GetFunction(string funcName)
        {
            return FunctionList[funcName];
        }
        #endregion

        #region 블럭 자석 효과
        // 각자 맞는 블럭으로 오버로딩
        private void MargnetBlock(BaseBlock block, MouseEventArgs e)
        {
            MargnetTarget = null;
            Highlighter.Visibility = Visibility.Hidden;
            
            foreach (var hole in CanAttachHoleList)
            {
                if (block.AllHoleList.Contains(hole))
                {
                    continue;
                }

                var position = hole.TranslatePoint(new Point(0, 0), Master);

                if (GetDistance(position, block.Position) > 20)
                {
                    continue;
                }

                MargnetTarget = hole;
                Highlighter.Margin = new Thickness(position.X, position.Y, 0, 0);
                Highlighter.Visibility = Visibility.Visible;
            }
        }
        #endregion

        #region 블럭 연결
        // 각자 맞는 블럭으로 오버로딩
        private void ConnectBlock(BaseBlock block, MouseEventArgs e)
        {
            if (SelectedBlock is StatementBlock)
            {
                ConnectBlock(SelectedBlock as StatementBlock, e);
            }
            else if (SelectedBlock is ObjectBlock)
            {
                ConnectBlock(SelectedBlock as ObjectBlock, e);
            }
        }

        // StatementBlock일때
        private void ConnectBlock(StatementBlock block, MouseEventArgs e)
        {
            // NextConnectHole이 아니면 연결할 수 없음
            if (!(MargnetTarget is NextConnectHole))
            {
                return;
            }

            // 블럭을 캔버스에서 연결 대상으로 이동
            DetachFromCanvas(block);
            (MargnetTarget as NextConnectHole).StatementBlock = block;

            // 블럭 위치 재조정
            block.Position = new Point(0, 0);
        }

        // LogicBlock일때
        private void ConnectBlock(ObjectBlock block, MouseEventArgs e)
        {
            if (SelectedBlock is SettableObjectBlock && MargnetTarget is SettableObjectHole)
            {
                var settableObjectBlock = block as SettableObjectBlock;
                var settableObjectHole = MargnetTarget as SettableObjectHole;

                // 연결 대상에 이미 블럭이 존재하는 경우
                if (settableObjectHole.SettableObjectBlock != null)
                {
                    // 기존 블럭을 캔버스로 이동
                    var detachedBlock = settableObjectHole.DetachBlock();

                    if (detachedBlock != null)
                    {
                        AttachToCanvas(detachedBlock);

                        detachedBlock.Position = block.Position;
                    }
                }

                // 블럭을 캔버스에서 연결 대상으로 이동
                DetachFromCanvas(block);
                settableObjectHole.SettableObjectBlock = settableObjectBlock;
                
                // 블럭 위치 재조정
                block.Position = new Point(0, 0);
            }
            else if (MargnetTarget is BaseObjectHole)
            {
                var objectHole = MargnetTarget as BaseObjectHole;

                // 연결 대상에 이미 블럭이 존재하는 경우
                if (objectHole.BaseObjectBlock != null)
                {
                    // 기존 블럭을 캔버스로 이동
                    var detachedBlock = objectHole.DetachBlock();

                    if (detachedBlock != null)
                    {
                        AttachToCanvas(detachedBlock);

                        detachedBlock.Position = block.Position;
                    }
                }

                // 블럭을 캔버스에서 연결 대상으로 이동
                DetachFromCanvas(block);
                objectHole.BaseObjectBlock = block;

                // 블럭 위치 재조정
                block.Position = new Point(0, 0);
            }
        }
        #endregion

        #region 내부 함수
        private bool MouseOnBlock()
        {
            return FindAncestorOrSelf<BaseBlock>((DependencyObject)Mouse.DirectlyOver) != null;
        }

        private double GetDistance2(Point a, Point b)
        {
            return Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2);
        }

        private double GetDistance(Point a, Point b)
        {
            return Math.Sqrt(GetDistance2(a, b));
        }

        // 부모에서 선택한 요소를 제거
        public void DetachFromCanvas(BaseBlock block)
        {
            Master.Children.Remove(block);
        }

        private void AttachToCanvas(BaseBlock block)
        {
            Master.Children.Add(block);
            block.ParentHole = null;
        }

        private DependencyObject GetParent(DependencyObject obj)
        {
            if (obj == null)
            {
                return null;
            }

            ContentElement ce = obj as ContentElement;

            if (ce != null)
            {
                DependencyObject parent = ContentOperations.GetParent(ce);

                if (parent != null)
                {
                    return parent;
                }

                FrameworkContentElement fce = ce as FrameworkContentElement;
                return fce != null ? fce.Parent : null;
            }

            return VisualTreeHelper.GetParent(obj);
        }

        private T FindAncestorOrSelf<T>(DependencyObject obj) where T : DependencyObject
        {
            while (obj != null)
            {
                T objTest = obj as T;
                if (objTest != null)
                {
                    return objTest;
                }

                obj = GetParent(obj);
            }

            return null;
        }
        #endregion

        #region 컨트롤 함수
        /// <summary>
        /// 블럭 에디터에 블럭을 추가하고 필요한 내용을 추가합니다.
        /// </summary>
        /// <param name="block">추가할 블럭</param>
        public void AddBlock(BaseBlock block)
        {
            // 블럭 클릭 이벤트 추가
            block.MouseLeftButtonDown += Block_MouseLeftButtonDown;
            block.MouseRightButtonDown += Block_MouseRightButtonDown;

            // 블럭을 캔버스에 추가
            AttachToCanvas(block);

            block.BlockEditor = this;

            // 블럭의 HoleList를 가져와 BlockEditor에 추가
            List<BaseHole> holeList = block.HoleList;

            foreach (BaseHole hole in holeList)
            {
                Type type = hole.GetType();

                if (hole is NextConnectHole)
                {
                    NextConnectHoleList.Add(hole as NextConnectHole);
                }
                else if (hole is SettableObjectHole)
                {
                    SettableObjectHoleList.Add(hole as SettableObjectHole);
                }
                else if (hole is BaseObjectHole)
                {
                    ObjectHoleList.Add(hole as BaseObjectHole);
                }
            }
        }

        // 블럭 삭제
        public void RemoveBlock(BaseBlock block)
        {
            if (block == null) return;

            if (block.ParentHole == null)
            {
                DetachFromCanvas(block);
            }
            else
            {
                block.ParentHole.DetachBlock();
            }

            List<BaseHole> holeList = block.HoleList;
            foreach (BaseHole hole in holeList)
            {
                if (hole is NextConnectHole)
                {
                    NextConnectHoleList.Remove(hole as NextConnectHole);
                }
                else if (hole is SettableObjectHole)
                {
                    SettableObjectHoleList.Remove(hole as SettableObjectHole);
                }
                else if (hole is BaseObjectHole)
                {
                    ObjectHoleList.Remove(hole as BaseObjectHole);
                }

                RemoveBlock(hole.AttachedBlock);
            }
        }

        // 컴파일
        public string GetSource()
        {
            try
            {
                GEntry entry = new GEntry();
                List<GBase> list = new List<GBase>();

                foreach (VariableBlock variableBlock in GetGlobalVariableBlockList())
                {
                    entry.Append(new GDefine(variableBlock.GVariable));
                }

                foreach (var block in Master.Children)
                {
                    if (block is ScopeBlock)
                    {
                        ScopeBlock scopeBlock = block as ScopeBlock;
                        entry.Append(scopeBlock.GScope);
                    }
                }

                return entry.ToSource().TrimStart();
            }
            catch (ToObjectException e)
            {
                return e.Message;
            }
        }

        public void StartBlockMove(BaseBlock target, Point position = new Point())
        {
            CaptureMouse();

            SelectedBlock = target;
            SelectedPosition = position;
            IsSelectedBlockMoved = false;

            CanAttachHoleList.Clear();
            if (SelectedBlock is ObjectBlock)
            {
                if (SelectedBlock is SettableObjectBlock)
                {
                    CanAttachHoleList.AddRange(SettableObjectHoleList.Where(e => e.CanAttachBlock(SelectedBlock)));
                }

                CanAttachHoleList.AddRange(ObjectHoleList.Where((e) => e.CanAttachBlock(SelectedBlock)));
            }
            else if (SelectedBlock is StatementBlock)
            {
                CanAttachHoleList.AddRange(NextConnectHoleList.Where((e) => e.CanAttachBlock(SelectedBlock)));
            }

            Panel.SetZIndex(SelectedBlock, int.MaxValue - 2);
        }

        public void Save(string path, string designXML, GCompiler compiler)
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            FileStream stream = new FileStream(path, FileMode.Create);
            XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8)
            {
                Formatting = Formatting.Indented,
                Indentation = 4
            };

            writer.WriteStartDocument();
            writer.WriteStartElement("Canvas");

            writer.WriteStartElement("Code");

            foreach (var block in Master.Children)
            {
                if (block is BaseBlock)
                {
                    writer.WriteStartElement("Blocks");
                    writer.WriteAttributeString("Position", (block as BaseBlock).Position.ToString());
                    (block as BaseBlock).SaveXML(writer);
                    writer.WriteEndElement();
                } 
            }

            writer.WriteEndElement(); // End Code

            writer.WriteStartElement("Design");

            byte[] base64 = Encoding.UTF8.GetBytes(designXML);
            writer.WriteBase64(base64, 0, base64.Length);

            writer.WriteEndElement(); // End Design

            writer.WriteStartElement("Extension");

            foreach (string reference in compiler.LoadedReferences)
            {
                if (reference != null && reference.Length > 0 && File.Exists(reference))
                {
                    writer.WriteStartElement("Reference");
                    writer.WriteAttributeString("Path", reference.Replace(appData, "%APPDATA%"));
                    writer.WriteEndElement();
                }
            }

            foreach (string dependencies in compiler.Dependencies)
            {
                if (dependencies != null && dependencies.Length > 0 && Directory.Exists(dependencies))
                {
                    writer.WriteStartElement("Dependencies");
                    writer.WriteAttributeString("Path", dependencies.Replace(appData, "%APPDATA%"));
                    writer.WriteEndElement();
                }
            }

            writer.WriteEndElement(); // End Extension

            writer.WriteEndElement(); // End Canvas

            writer.WriteEndDocument();

            writer.Flush();
            stream.Close();
        }

        public void Load(string path)
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            var document = new XmlDocument();
            document.Load(path);

            var design = document.SelectSingleNode("/Canvas/Design");
            byte[] arr = Convert.FromBase64String(design.InnerText);
            LoadRequested?.Invoke(Encoding.UTF8.GetString(arr));

            foreach (XmlElement element in document.SelectNodes("/Canvas/Code/Blocks"))
            {
                var position = Point.Parse(element.GetAttribute("Position"));
                var block = BlocksFromXML(element);
                AttachToCanvas(block);
                block.Position = position;
            }

            List<string> references = new List<string>();
            foreach (XmlElement element in document.SelectNodes("/Canvas/Extension/Reference"))
            {
                references.Add(element.GetAttribute("Path").Replace("%APPDATA%", appData));
            }

            List<string> dependencies = new List<string>();
            foreach (XmlElement element in document.SelectNodes("/Canvas/Extension/Dependencies"))
            {
                dependencies.Add(element.GetAttribute("Path").Replace("%APPDATA%", appData));
            }

            LoadCompleted?.Invoke(references.ToArray(), dependencies.ToArray());
        }

        private BaseBlock BlocksFromXML(XmlElement element)
        {
            var blockNodeList = element.SelectNodes("Block");
            
            var firstBlock = BaseBlock.LoadBlock(blockNodeList[0] as XmlElement, this);
            var prevBlock = firstBlock;

            for (int i=1; i<blockNodeList.Count; i++)
            {
                var block = BaseBlock.LoadBlock(blockNodeList[i] as XmlElement, this) as StatementBlock;
                if (block == null)
                {
                    throw new Exception("Statement가 아니면 붙일 수 없습니다.");
                }

                if (prevBlock is ScopeBlock)
                {
                    (prevBlock as ScopeBlock).NextConnectHole.StatementBlock = block;
                }
                else if (prevBlock is PrevStatementBlock)
                {
                    (prevBlock as PrevStatementBlock).NextConnectHole.StatementBlock = block;
                }
                else
                {
                    throw new Exception("Statement를 붙일 수 없습니다.");
                }

                prevBlock = block;
            }

            return firstBlock;
        }
        #endregion

        #region 컨텍스트 메뉴
        private void ContextDelete_Click(object sender, RoutedEventArgs e)
        {
            foreach (BaseHole hole in LastSelectedBlock?.AllHoleList)
            {
                if (hole.AttachedBlock != null)
                {
                    if (MessageBox.Show("연결된 모든 블럭 및 내용이 삭제됩니다.\n정말로 해당 블럭을 삭제하시겠습니까?", "확인", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.No)
                    {
                        return;
                    }

                    break;
                }
            }

            RemoveBlock(LastSelectedBlock);
        }

        private void ContextVariable_Click(object sender, RoutedEventArgs e)
        {
            CreateVariableRequested?.Invoke();
        }

        private void ContextFunction_Click(object sender, RoutedEventArgs e)
        {
            CreateFunctionRequested?.Invoke();
        }
        #endregion

        #region 스크롤 뷰어 드래그
        double vOffset = 0;
        double hOffset = 0;
        Point scrollMousePoint = new Point();

        private void BlockViewer_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            BlockViewer.ReleaseMouseCapture();
        }

        private void BlockViewer_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed && !MouseOnBlock())
            {
                BlockViewer.CaptureMouse();
                vOffset = BlockViewer.VerticalOffset;
                hOffset = BlockViewer.HorizontalOffset;
                scrollMousePoint = e.GetPosition(BlockViewer);
            }
        }

        private void BlockViewer_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (BlockViewer.IsMouseCaptured)
            {
                BlockViewer.ScrollToVerticalOffset(vOffset + (scrollMousePoint.Y - e.GetPosition(BlockViewer).Y));
                BlockViewer.ScrollToHorizontalOffset(hOffset + (scrollMousePoint.X - e.GetPosition(BlockViewer).X));
            }
        }
        #endregion
    }
}
