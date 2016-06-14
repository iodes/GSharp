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
using System.ComponentModel;
using System.Windows.Threading;

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
            if (GlobalVariableBlockList.Keys.Contains(varName))
            {
                return null;
            }

            GlobalVariableBlockList[varName] = BlockUtils.CreateVariableBlock(varName);
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

            if (target is CompareBlock)
            {
                writer.WriteAttributeString("Operator", (target as CompareBlock).GetConditionString());
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

            if (target is VariableBlock)
            {
                writer.WriteAttributeString("Variable", (target as VariableBlock).GVariable.Name);
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
