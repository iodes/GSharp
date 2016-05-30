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
using GSharp.Graphic.Objects.Logics;
using GSharp.Graphic.Objects.Strings;
using GSharp.Graphic.Objects.Numbers;
using GSharp.Extension;
using GSharp.Extension.Optionals;

namespace GSharp.Graphic.Controls
{
    /// <summary>
    /// BlockEditor.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class BlockEditor : UserControl
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

        // Logic Hole
        private List<LogicHole> LogicHoleList = new List<LogicHole>();

        // Object Hole
        private List<ObjectHole> ObjectHoleList = new List<ObjectHole>();
        private List<NumberHole> NumberHoleList = new List<NumberHole>();
        private List<StringHole> StringHoleList = new List<StringHole>();
        private List<CustomHole> CustomHoleList = new List<CustomHole>();

        private List<BaseHole> CanAttachHoleList = new List<BaseHole>();

        // Variable Hole
        //private list<variablehole> variableholelist = new list<variablehole>();

        // 선택된 대상이 움직였는지 체크
        private bool IsSelectedBlockMoved = false;

        // 변수 목록
        public Dictionary<string, IVariableBlock> GlobalVariableBlockList { get; } = new Dictionary<string, IVariableBlock>();
        //private Dictionary<string, GStringVariable> StringVariableList = new Dictionary<string, GStringVariable>();
        //private Dictionary<string, GNumberVariable> NumberVariableList = new Dictionary<string, GNumberVariable>();
        //private Dictionary<string, GLogicVariable> LogicVariableList = new Dictionary<string, GLogicVariable>();
        //private Dictionary<string, GCustomVariable> CustomVariableList = new Dictionary<string, GCustomVariable>();

        // 함수 목록
        private Dictionary<string, GFunction> FunctionList = new Dictionary<string, GFunction>();
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

        public event CreateVariableRequestedHandler CreateVariableRequested;
        public delegate void CreateVariableRequestedHandler();

        public event CreateFunctionRequestedHandler CreateFunctionRequested;
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

            // 이벤트 발동
            BlockChanged?.Invoke();
        }

        private void UserControl_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BlockChanged?.Invoke();
            }
        }

        private void UserControl_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            BlockChanged?.Invoke();
        }
        #endregion

        #region 변수 선언
        // 변수 선언
        public IVariableBlock DefineGlobalVariable(string varName, Type variableType)
        {
            if (GlobalVariableBlockList.Keys.Contains(varName))
            {
                return null;
            }

            GlobalVariableBlockList[varName] = BlockUtils.CreateVariableBlock(varName, variableType);
            return GlobalVariableBlockList[varName];
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

        public List<IVariableBlock> GetGlobalVariableBlockList()
        {
            return GlobalVariableBlockList.Values.ToList();
        }
        #endregion

        #region 함수 선언
        // 함수 선언
        public void DefineFunction(string funcName)
        {
            FunctionList[funcName] = new GFunction(funcName);
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
        public List<GFunction> GetFunctionList()
        {
            return FunctionList.Values.ToList();
        }

        // 함수 가져오기
        public GFunction GetFunction(string funcName)
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

            if (SelectedBlock is StatementBlock)
            {
                MargnetBlock(SelectedBlock as StatementBlock, e);
            }
            else if (SelectedBlock is LogicBlock)
            {
                MargnetBlock(SelectedBlock as LogicBlock, e);
            }
            else if (SelectedBlock is ObjectBlock)
            {
                MargnetBlock(SelectedBlock as ObjectBlock, e);

                if (SelectedBlock is NumberBlock)
                {
                    MargnetBlock(SelectedBlock as NumberBlock, e);
                }
                else if (SelectedBlock is StringBlock)
                {
                    MargnetBlock(SelectedBlock as StringBlock, e);
                }
                else if (SelectedBlock is CustomBlock)
                {
                    MargnetBlock(SelectedBlock as CustomBlock, e);
                }
            }
        }

        // StatementBlock일 때
        private void MargnetBlock(StatementBlock block, MouseEventArgs e)
        {
            _MargnetBlock(block, NextConnectHoleList.ToList<BaseHole>(), e);
        }

        // LogicBlock일 때
        private void MargnetBlock(LogicBlock block, MouseEventArgs e)
        {
            _MargnetBlock(block, LogicHoleList.ToList<BaseHole>(), e);
        }

        // ObjectBlock일 때
        private void MargnetBlock(ObjectBlock block, MouseEventArgs e)
        {
            _MargnetBlock(block, ObjectHoleList.ToList<BaseHole>(), e);
        }

        // NumberBlock일 때
        private void MargnetBlock(NumberBlock block, MouseEventArgs e)
        {
            _MargnetBlock(block, NumberHoleList.ToList<BaseHole>(), e);
        }

        // StringBlock일 때
        private void MargnetBlock(StringBlock block, MouseEventArgs e)
        {
            _MargnetBlock(block, StringHoleList.ToList<BaseHole>(), e);
        }

        // CustomBlock일 때
        private void MargnetBlock(CustomBlock block, MouseEventArgs e)
        {
            foreach (var hole in CustomHoleList)
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

        // 공통 부분 통일
        private void _MargnetBlock(BaseBlock block, List<BaseHole> holeList, MouseEventArgs e)
        {
            foreach (var hole in holeList)
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
            else if (SelectedBlock is LogicBlock)
            {
                ConnectBlock(SelectedBlock as LogicBlock, e);
            }
            else if (SelectedBlock is ObjectBlock)
            {
                if (SelectedBlock is NumberBlock)
                {
                    ConnectBlock(SelectedBlock as NumberBlock, e);
                }
                else if (SelectedBlock is StringBlock)
                {
                    ConnectBlock(SelectedBlock as StringBlock, e);
                }
                else if (SelectedBlock is CustomBlock)
                {
                    ConnectBlock(SelectedBlock as CustomBlock, e);
                }
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
        private void ConnectBlock(LogicBlock block, MouseEventArgs e)
        {
            // LogicHole 아니면 연결할 수 없음
            if (!(MargnetTarget is LogicHole))
            {
                return;
            }

            var logicHole = MargnetTarget as LogicHole;

            // 연결 대상에 이미 블럭이 존재하는 경우
            if (logicHole.LogicBlock != null)
            {
                // 기존 블럭을 캔버스로 이동
                var detachedBlock = logicHole.DetachBlock();
                AttachToCanvas(detachedBlock);

                detachedBlock.Position = block.Position;
            }

            // 블럭을 캔버스에서 연결 대상으로 이동
            DetachFromCanvas(block);
            logicHole.LogicBlock = block;

            // 블럭 위치 재조정
            block.Position = new Point(0, 0);
        }

        // NumberBlock일때
        private void ConnectBlock(NumberBlock block, MouseEventArgs e)
        {
            // ObjectHole인 경우 연결
            if (MargnetTarget is ObjectHole)
            {
                var objectHole = MargnetTarget as ObjectHole;

                // 연결 대상에 이미 블럭이 존재하는 경우
                if (objectHole.ObjectBlock != null)
                {
                    // 기존 블럭을 Canvas로 이동
                    var detachedBlock = objectHole.DetachBlock();
                    AttachToCanvas(detachedBlock);

                    detachedBlock.Position = block.Position;
                }

                // 블럭을 캔버스에서 연결 대상으로 이동
                DetachFromCanvas(block);
                objectHole.ObjectBlock = block;

                // 블럭 위치 재조정
                block.Position = new Point(0, 0);
            }
            // NumberHole인 경우 연결
            else if (MargnetTarget is NumberHole)
            {
                var numberHole = MargnetTarget as NumberHole;

                // 연결 대상에 이미 블럭이 존재하는 경우
                if (numberHole.NumberBlock != null)
                {
                    // 기존 블럭을 Canvas로 이동
                    var detachedBlock = numberHole.DetachBlock();

                    if (detachedBlock != null)
                    {
                        AttachToCanvas(detachedBlock);
                        detachedBlock.Position = block.Position;
                    }
                }

                // 블럭을 캔버스에서 연결 대상으로 이동
                DetachFromCanvas(block);
                numberHole.NumberBlock = block;

                // 블럭 위치 재조정
                block.Position = new Point(0, 0);
            }
        }

        // StringBlock일 때
        private void ConnectBlock(StringBlock block, MouseEventArgs e)
        {
            // ObjectHole인 경우 연결
            if (MargnetTarget is ObjectHole)
            {
                var objectHole = MargnetTarget as ObjectHole;

                // 연결 대상에 이미 블럭이 존재하는 경우
                if (objectHole.ObjectBlock != null)
                {
                    // 기존 블럭을 Canvas로 이동
                    var detachedBlock = objectHole.DetachBlock();
                    AttachToCanvas(detachedBlock);

                    detachedBlock.Position = block.Position;
                }

                // 블럭을 캔버스에서 연결 대상으로 이동
                DetachFromCanvas(block);
                objectHole.ObjectBlock = block;

                // 블럭 위치 재조정
                block.Position = new Point(0, 0);
            }
            // StringHole인 경우 연결
            else if (MargnetTarget is StringHole)
            {
                var stringHole = MargnetTarget as StringHole;

                // 연결 대상에 이미 블럭이 존재하는 경우
                if (stringHole.StringBlock != null)
                {
                    // 기존 블럭을 Canvas로 이동
                    var detachedBlock = stringHole.DetachBlock();

                    if (detachedBlock != null)
                    {
                        AttachToCanvas(detachedBlock);
                        detachedBlock.Position = block.Position;
                    }
                }

                // 블럭을 캔버스에서 연결 대상으로 이동
                DetachFromCanvas(block);
                stringHole.StringBlock = block;

                // 블럭 위치 재조정
                block.Position = new Point(0, 0);
            }
        }

        // CustomBlock인 경우
        private void ConnectBlock(CustomBlock block, MouseEventArgs e)
        {
            /* 일단 CustomBlock은 ObjectHole에 끼울 수 없음
            // ObjectHole인 경우 연결
            if (MargnetTarget is ObjectHole)
            {
                var objectHole = MargnetTarget as ObjectHole;

                // 연결 대상에 이미 블럭이 존재하는 경우
                if (objectHole.ObjectBlock != null)
                {
                    // 기존 블럭을 Canvas로 이동
                    var detachedBlock = objectHole.DetachBlock();
                    AttachToCanvas(detachedBlock);

                    detachedBlock.Position = block.Position;
                }

                // 블럭을 캔버스에서 연결 대상으로 이동
                DetachFromCanvas(block);
                objectHole.ObjectBlock = block;

                // 블럭 위치 재조정
                block.Position = new Point(0, 0);
            }

            // StringHole인 경우 연결
            else */
            if (MargnetTarget is CustomHole)
            {
                var customHole = MargnetTarget as CustomHole;

                // 연결 대상에 이미 블럭이 존재하는 경우
                if (customHole.CustomBlock != null)
                {
                    // 기존 블럭을 Canvas로 이동
                    var detachedBlock = customHole.DetachBlock();

                    AttachToCanvas(detachedBlock);
                    detachedBlock.Position = block.Position;
                }

                // 블럭을 캔버스에서 연결 대상으로 이동
                DetachFromCanvas(block);
                customHole.CustomBlock = block;

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
        private void DetachFromCanvas(BaseBlock block)
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

            // 블럭에 에디터 정보 저장
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
                else if (hole is LogicHole)
                {
                    LogicHoleList.Add(hole as LogicHole);
                }
                else if (hole is NumberHole)
                {
                    NumberHoleList.Add(hole as NumberHole);
                }
                else if (hole is StringHole)
                {
                    StringHoleList.Add(hole as StringHole);
                }
                else if (hole is CustomHole)
                {
                    CustomHoleList.Add(hole as CustomHole);
                }
                else if (hole is ObjectHole)
                {
                    ObjectHoleList.Add(hole as ObjectHole);
                }
            }

            // 블럭 변경 이벤트 호출
            BlockChanged?.Invoke();
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
                Type type = hole.GetType();

                if (hole is NextConnectHole)
                {
                    NextConnectHoleList.Remove(hole as NextConnectHole);
                }
                else if (hole is LogicHole)
                {
                    LogicHoleList.Remove(hole as LogicHole);
                }
                else if (hole is ObjectHole)
                {
                    ObjectHoleList.Remove(hole as ObjectHole);
                }
                else if (hole is NumberHole)
                {
                    NumberHoleList.Remove(hole as NumberHole);
                }
                else if (hole is StringHole)
                {
                    StringHoleList.Remove(hole as StringHole);
                }
                else if (hole is CustomHole)
                {
                    CustomHoleList.Remove(hole as CustomHole);
                }

                RemoveBlock(hole.AttachedBlock);
            }

            BlockChanged?.Invoke();
        }

        // 컴파일
        public string GetSource()
        {
            try
            {
                GEntry entry = new GEntry();
                List<GBase> list = new List<GBase>();

                foreach (IVariableBlock variableBlock in GetGlobalVariableBlockList())
                {
                    entry.Append(new GDefine(variableBlock.IVariable));
                }

                foreach (var block in Master.Children)
                {
                    if (block is EventBlock)
                    {
                        EventBlock scopeBlock = block as EventBlock;
                        entry.Append(scopeBlock.GEvent);
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

            if (target is NumberBlock)
            {
                CanAttachHoleList.AddRange(NumberHoleList.Where((e) => { return e.CanAttachBlock(SelectedBlock); }));
            }
            else if (target is LogicBlock)
            {
                CanAttachHoleList.AddRange(LogicHoleList.Where((e) => { return e.CanAttachBlock(SelectedBlock); }));
            }
            else if (target is StringBlock)
            {
                CanAttachHoleList.AddRange(StringHoleList.Where((e) => { return e.CanAttachBlock(SelectedBlock); }));
            }
            else if (target is ObjectBlock)
            {
                CanAttachHoleList.AddRange(ObjectHoleList.Where((e) => { return e.CanAttachBlock(SelectedBlock); }));
            }
            else if (target is CustomBlock)
            {
                CanAttachHoleList.AddRange(CustomHoleList.Where((e) => { return e.CanAttachBlock(SelectedBlock); }));
            }
            else if (target is StatementBlock)
            {
                CanAttachHoleList.AddRange(NextConnectHoleList.Where((e) => { return e.CanAttachBlock(SelectedBlock); }));
            }

            Panel.SetZIndex(SelectedBlock, int.MaxValue - 2);
        }

        public void Save(string path)
        {
            FileStream stream = new FileStream(path, FileMode.Create);
            XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8)
            {
                Formatting = Formatting.Indented,
                Indentation = 4
            };

            writer.WriteStartDocument();
            writer.WriteStartElement("Canvas");

            foreach (var target in Master.Children)
            {
                if (target is BaseBlock)
                {
                    var baseBlock = target as BaseBlock;

                    writer.WriteStartElement("Blocks");
                    writer.WriteAttributeString("Position", baseBlock.Position.ToString());

                    BlockToXML(writer, baseBlock);

                    writer.WriteEndElement();
                }
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();

            stream.Close();
        }

        public void BlockToXML(XmlTextWriter writer, BaseBlock target)
        {
            if (target == null) return;

            writer.WriteStartElement("Block");
            writer.WriteAttributeString("Type", target.GetType().ToString());

            if (target is ICompareBlock)
            {
                writer.WriteAttributeString("Operator", (target as ICompareBlock).GetConditionString());
            }
            else if (target is GateBlock)
            {
                writer.WriteAttributeString("Operator", (target as GateBlock).GetGateType().ToString());
            }
            else if (target is NumberConstBlock)
            {
                writer.WriteAttributeString("Number", (target as NumberConstBlock).Number.ToString());
            }
            else if (target is StringConstBlock)
            {
                writer.WriteAttributeString("String", (target as StringConstBlock).String.ToString());
            }

            if (target is IVariableBlock)
            {
                writer.WriteAttributeString("Variable", (target as IVariableBlock).IVariable.Name);
            }

            if (target is IModuleBlock)
            {
                var moduleBlock = target as IModuleBlock;

                writer.WriteAttributeString("FriendlyName", moduleBlock.GCommand.FriendlyName);
                writer.WriteAttributeString("NamespaceName", moduleBlock.GCommand.NamespaceName);
                writer.WriteAttributeString("MethodName", moduleBlock.GCommand.MethodName);
                writer.WriteAttributeString("MethodType", moduleBlock.GCommand.MethodType.ToString());
                writer.WriteAttributeString("ObjectType", moduleBlock.GCommand.ObjectType.ToString());

                if (moduleBlock.GCommand.Optionals?.Length > 0)
                {
                    writer.WriteStartElement("Arguments");
                    foreach (GOptional arg in moduleBlock.GCommand.Optionals)
                    {
                        writer.WriteStartElement("Argument");
                        writer.WriteAttributeString("Type", arg.ObjectType.FullName);
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }
            }

            if (target.HoleList.Any())
            {
                writer.WriteStartElement("Holes");
                foreach (BaseHole hole in target.HoleList)
                {
                    if (hole is NextConnectHole)
                    {
                        continue;
                    }

                    writer.WriteStartElement("Hole");
                    writer.WriteAttributeString("Type", hole.GetType().ToString());
                    BlockToXML(writer, hole.AttachedBlock);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }

            if (target is IContainChildBlock && (target as IContainChildBlock).ChildConnectHole.AttachedBlock != null)
            {
                writer.WriteStartElement("Blocks");
                BlockToXML(writer, (target as IContainChildBlock).ChildConnectHole.AttachedBlock);
                writer.WriteEndElement();
            }

            writer.WriteEndElement();

            if (target is ScopeBlock)
            {
                BlockToXML(writer, (target as ScopeBlock).NextConnectHole.AttachedBlock);
            }
            else if (target is PrevStatementBlock)
            {
                BlockToXML(writer, (target as PrevStatementBlock).NextConnectHole.AttachedBlock);
            }
        }

        public void Load(string path)
        {
            var document = new XmlDocument();
            document.Load(path);

            foreach (XmlElement element in document.SelectNodes("/Canvas/Blocks"))
            {
                var position = Point.Parse(element.GetAttribute("Position"));
                var block = BlocksFromXML(element);
                block.Position = position;
            }
        }

        private BaseBlock BlocksFromXML(XmlElement parent)
        {
            if (parent == null) return null;

            BaseBlock prevBlock = null;
            BaseBlock firstBlock = null;

            foreach (XmlElement element in parent.ChildNodes)
            {
                var block = BlockFromXML(element);

                if (prevBlock == null)
                {
                    firstBlock = block;
                }

                if (block is StatementBlock)
                {
                    var statement = block as StatementBlock;
                    if (prevBlock is PrevStatementBlock)
                    {
                        DetachFromCanvas(statement);
                        (prevBlock as PrevStatementBlock).NextConnectHole.StatementBlock = statement;
                    }
                    else if (prevBlock is ScopeBlock)
                    {
                        DetachFromCanvas(statement);
                        (prevBlock as ScopeBlock).NextConnectHole.StatementBlock = statement;
                    }
                }

                prevBlock = block;
            }

            return firstBlock;
        }

        private BaseBlock BlockFromXML(XmlElement element)
        {
            Type blockType = Type.GetType(element.GetAttribute("Type"));
            BaseBlock block;

            var constructorInfo = blockType.GetConstructor(Type.EmptyTypes);

            if (constructorInfo != null)
            {
                // 블럭 생성에 인자가 필요 없음
                block = Activator.CreateInstance(blockType) as BaseBlock;
            }
            else
            {
                var argList = new List<object>();

                // IModuleBlock은 GCommand를 생성자로 가짐
                if (typeof(IModuleBlock).IsAssignableFrom(blockType))
                {
                    var friendlyName = element.GetAttribute("FriendlyName");
                    var namespaceName = element.GetAttribute("NamespaceName");
                    var methodName = element.GetAttribute("MethodName");
                    var methodType = (GCommand.CommandType)Enum.Parse(typeof(GCommand.CommandType), element.GetAttribute("MethodType"));
                    var objectType = Type.GetType(element.GetAttribute("ObjectType"));

                    var commandArgList = new List<GOptional>();

                    XmlNodeList argNodeList;
                    if ((argNodeList = element.SelectSingleNode("Arguments")?.ChildNodes) != null)
                    {
                        foreach (XmlElement argElem in argNodeList)
                        {
                            var argType = Type.GetType(argElem.GetAttribute("Type"));
                            commandArgList.Add(new GOptional(string.Empty, string.Empty, string.Empty, argType));
                        }
                    }

                    var command = new GCommand(namespaceName, methodName, friendlyName, objectType, methodType, commandArgList.ToArray());
                    argList.Add(command);
                }

                // 블럭 생성자에 인자가 필요함
                block = Activator.CreateInstance(blockType, argList.ToArray()) as BaseBlock;
            }

            int idx = 0;

            XmlNodeList holeNodeList;
            if ((holeNodeList = element.SelectSingleNode("Holes")?.ChildNodes) != null)
            {
                foreach (XmlElement holeElem in holeNodeList)
                {
                    Type holeType = Type.GetType(holeElem.GetAttribute("Type"));

                    // 불러온 Hole 정보가 현재 Block의 Hole 정보와 일치하지 않음
                    if (holeType != block.HoleList[idx].GetType())
                    {
                        throw new Exception();
                    }

                    // 불러온 Hole에 연결된 블럭 불러오기
                    XmlElement holeBlockElem = holeElem.SelectSingleNode("Block") as XmlElement;
                    if (holeBlockElem != null)
                    {
                        BaseBlock holeBlock = BlockFromXML(holeBlockElem);

                        if (block.HoleList[idx] is StringHole)
                        {
                            DetachFromCanvas(holeBlock);
                            (block.HoleList[idx] as StringHole).StringBlock = holeBlock as StringBlock;
                        }
                        else if (block.HoleList[idx] is NumberHole)
                        {
                            DetachFromCanvas(holeBlock);
                            (block.HoleList[idx] as NumberHole).NumberBlock = holeBlock as NumberBlock;
                        }
                        else if (block.HoleList[idx] is LogicHole)
                        {
                            DetachFromCanvas(holeBlock);
                            (block.HoleList[idx] as LogicHole).LogicBlock = holeBlock as LogicBlock;
                        }
                        else if (block.HoleList[idx] is CustomHole)
                        {
                            DetachFromCanvas(holeBlock);
                            (block.HoleList[idx] as CustomHole).CustomBlock = holeBlock as CustomBlock;
                        }
                        else if (block.HoleList[idx] is ObjectHole)
                        {
                            DetachFromCanvas(holeBlock);
                            (block.HoleList[idx] as ObjectHole).ObjectBlock = holeBlock as ObjectBlock;
                        }
                    }

                    idx++;
                }
            }

            // IContainChild 블럭은 자식 요소를 포함함
            if (block is IContainChildBlock)
            {
                var childBlocksElem = element.SelectSingleNode("Blocks") as XmlElement;

                if (childBlocksElem != null)
                {
                    var firstChildBlock = BlocksFromXML(childBlocksElem);
                    DetachFromCanvas(firstChildBlock);
                    (block as IContainChildBlock).ChildConnectHole.StatementBlock = firstChildBlock as StatementBlock;
                }
            }

            AddBlock(block);

            return block;
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
            if (e.LeftButton == MouseButtonState.Pressed && !MouseOnBlock())
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
