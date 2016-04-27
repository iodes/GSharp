using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using GSharp.Base.Cores;
using GSharp.Base.Singles;
using GSharp.Graphic.Core;
using GSharp.Graphic.Holes;
using GSharp.Base;
using GSharp.Base.Scopes;
using GSharp.Graphic.Scopes;
using System.IO;
using System.Xml;
using System.Text;
using GSharp.Graphic.Statements;
using GSharp.Graphic.Logics;
using System.Runtime.Serialization.Formatters.Binary;
using GSharp.Extension;
using System.Reflection;

namespace GSharp.Graphic.Controls
{
    /// <summary>
    /// BlockEditor.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class BlockEditor : UserControl
    {
        #region 객체
        // 선택한 블럭
        private BaseBlock SelectedBlock;

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

        // 자석 효과 대상 목록
        //private List<BaseHole> HoleList = new List<BaseHole>();

        // Hole 목록
        private List<NextConnectHole> NextConnectHoleList = new List<NextConnectHole>();
        private List<LogicHole> LogicHoleList = new List<LogicHole>();
        private List<ObjectHole> ObjectHoleList = new List<ObjectHole>();
        private List<VariableHole> VariableHoleList = new List<VariableHole>();

        //private Dictionary<Type, List<BaseHole>> HoleList = new Dictionary<Type, List<BaseHole>>();

        // 선택된 대상이 움직였는지 체크
        private bool IsSelectedBlockMoved = false;

        // 변수 목록
        private Dictionary<string, GDefine> DefineList = new Dictionary<string, GDefine>();

        // 함수 목록
        private Dictionary<string, GFunction> FunctionList = new Dictionary<string, GFunction>();
        #endregion

        #region 생성자
        public BlockEditor()
        {
            InitializeComponent();

            Panel.SetZIndex(Highlighter, int.MaxValue - 1);
            BlockCanvas.Children.Add(Highlighter);
        }
        #endregion

        #region 이벤트
        public event BlockChangedEventHandler BlockChanged;
        public delegate void BlockChangedEventHandler();

        private void Block_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CaptureMouse();

            SelectedBlock = (BaseBlock)sender;
            SelectedPosition = e.GetPosition(SelectedBlock);

            // 부모 블럭 클릭되지 않도록
            e.Handled = true;
            IsSelectedBlockMoved = false;
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            // 선택한 블럭이 없으면 안됨
            if (SelectedBlock == null)
            {
                return;
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

                    //Point pos = e.GetPosition(this);
                    //SelectedBlock.Position = new Point(pos.X - SelectedPosition.X, pos.Y - SelectedPosition.Y);
                }

                IsSelectedBlockMoved = true;
            }

            // 마우스 좌표로 블럭 이동
            Point position = e.GetPosition(this);
            SelectedBlock.Position = new Point(position.X - SelectedPosition.X, position.Y - SelectedPosition.Y);

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
                Point position = e.GetPosition(this);
                SelectedBlock.Position = new Point(position.X - SelectedPosition.X, position.Y - SelectedPosition.Y);

                // 연결할 대상이 있다면 연결
                if (MargnetTarget != null)
                {
                    ConnectBlock(SelectedBlock, e);
                }
            }

            // Selected 해제
            SelectedBlock = null;
            MargnetTarget = null;
            Highlighter.Visibility = Visibility.Hidden;

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
        public void DefineVariable(string varName)
        {
            DefineList[varName] = new GDefine(varName);
        }

        // 변순 선언 해제
        public void UnDefineVariable(string varName)
        {
            DefineList.Remove(varName);
        }

        // 변수 목록
        public List<string> GetVariableNameList()
        {
            return DefineList.Keys.ToList();
        }
        
        public List<GDefine> GetDefineList()
        {
            return DefineList.Values.ToList();
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
                MargnetBlock((StatementBlock)SelectedBlock, e);
            }
            else if (SelectedBlock is LogicBlock)
            {
                MargnetBlock((LogicBlock)SelectedBlock, e);
            }
            else if (SelectedBlock is ObjectBlock)
            {
                MargnetBlock((ObjectBlock)SelectedBlock, e);
            }
        }

        // StatementBlock일때
        private void MargnetBlock(StatementBlock block, MouseEventArgs e)
        {
            foreach (var hole in NextConnectHoleList)
            {
                if (block.HoleList.Contains(hole))
                {
                    continue;
                }

                var position = hole.TranslatePoint(new Point(0, 0), BlockCanvas);
                if (GetDistance(position, block.Position) > 20)
                {
                    continue;
                }

                MargnetTarget = hole;
                Highlighter.Margin = new Thickness(position.X, position.Y, 0, 0);
                Highlighter.Visibility = Visibility.Visible;
            }
        }

        // LogicBlock일때
        private void MargnetBlock(LogicBlock block, MouseEventArgs e)
        {
            foreach (LogicHole hole in LogicHoleList)
            {
                if (block.HoleList.Contains(hole))
                {
                    continue;
                }

                var position = hole.TranslatePoint(new Point(0, 0), BlockCanvas);

                if (GetDistance(position, block.Position) > 20)
                {
                    continue;
                }

                MargnetTarget = hole;
                Highlighter.Margin = new Thickness(position.X, position.Y, 0, 0);
                Highlighter.Visibility = Visibility.Visible;
            }
        }

        // ObjectBlock일때
        private void MargnetBlock(ObjectBlock block, MouseEventArgs e)
        {
            foreach (ObjectHole hole in ObjectHoleList)
            {
                if (block.HoleList.Contains(hole))
                {
                    continue;
                }

                var position = hole.TranslatePoint(new Point(0, 0), BlockCanvas);

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

        // ObjectBlock일때
        private void ConnectBlock(ObjectBlock block, MouseEventArgs e)
        {
            // ObjectHole 아니면 연결할 수 없음
            if (!(MargnetTarget is ObjectHole))
            {
                return;
            }

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
        #endregion

        #region 내부 함수
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
            BlockCanvas.Children.Remove(block);
        }

        private void AttachToCanvas(BaseBlock block)
        {
            BlockCanvas.Children.Add(block);
            block.ParentHole = null;
        }
        #endregion

        #region 컨트롤 함수
        // 블럭 추가
        public void AddBlock(BaseBlock block)
        {
            // 블럭 클린 이벤트 추가
            block.MouseLeftButtonDown += Block_MouseLeftButtonDown;

            // 블럭을 캔버스에 추가
            AttachToCanvas(block);

            // 블럭의 HoleList를 가져와 BlockEditor에 추가
            List<BaseHole> holeList = block.HoleList;
            foreach(BaseHole hole in holeList)
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
                else if (hole is ObjectHole)
                {
                    ObjectHoleList.Add(hole as ObjectHole);
                }
                else if (hole is VariableHole)
                {
                    var vHole = hole as VariableHole;
                    VariableHoleList.Add(vHole);
                    vHole.SetItemList(GetVariableNameList());
                }
            }

            // 블럭 변경 이벤트 호출
            BlockChanged?.Invoke();
        }

        // 블럭 삭제
        public void RemoveBlock(BaseBlock block)
        {
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
                else if (hole is VariableHole)
                {
                    VariableHoleList.Remove(hole as VariableHole);
                }
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

                foreach (GDefine define in DefineList.Values)
                {
                    entry.Append(define);
                }

                foreach (var block in BlockCanvas.Children)
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

        public void Save(string path)
        {
            FileStream stream = new FileStream(path, FileMode.Create);
            XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8)
            {
                Formatting = Formatting.Indented,
                Indentation = 4
            };

            writer.WriteStartDocument();
            writer.WriteStartElement("Contents");
            foreach (var target in BlockCanvas.Children)
            {
                if (target is BaseBlock)
                {
                    ResolveBlockToXML(writer, target as BaseBlock, path);
                }
            }
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();

            stream.Close();
        }

        public void ResolveBlockToXML(XmlTextWriter writer, BaseBlock target, string path)
        {
            writer.WriteStartElement("Block");
            writer.WriteAttributeString("Type", target.GetType().ToString());

            if (target.Position.X != 0 && target.Position.Y != 0)
            {
                writer.WriteAttributeString("Position", target.Position.ToString());
            }
            if (target is CompareBlock)
            {
                writer.WriteAttributeString("Option", (target as CompareBlock).Operator.SelectedIndex.ToString());
            }

            if (target is ObjectBlock)
            {
                writer.WriteAttributeString("Content", target.GetHashCode().ToString());
                Serialize(System.IO.Path.GetDirectoryName(path) + "\\" + target.GetHashCode().ToString(), (target as ObjectBlock).GObject);
            }
            else if (target is IModuleBlock)
            {
                writer.WriteAttributeString("Content", target.GetHashCode().ToString());
                Serialize(System.IO.Path.GetDirectoryName(path) + "\\" + target.GetHashCode().ToString(), (target as IModuleBlock).Command);
            }

            NextConnectHole realNextConnectHole = null;
            foreach (BaseHole hole in target.HoleList)
            {
                if (hole?.AttachedBlock != null)
                {
                    if (hole is NextConnectHole)
                    {
                        if (hole.Name == "RealNextConnectHole")
                        {
                            realNextConnectHole = hole as NextConnectHole;
                        }
                        else
                        {
                            ResolveBlockToXML(writer, hole.AttachedBlock, path);
                        }
                    }
                    else
                    {
                        writer.WriteStartElement("Hole");
                        writer.WriteAttributeString("Type", hole.GetType().ToString());
                        ResolveBlockToXML(writer, hole.AttachedBlock, path);
                        writer.WriteEndElement();
                    }
                }
            }
            writer.WriteEndElement();

            if (realNextConnectHole != null)
            {
                ResolveBlockToXML(writer, realNextConnectHole.AttachedBlock, path);
            }
        }

        public void Load(string path)
        {
            XmlDocument document = new XmlDocument();
            document.Load(path);

            BaseBlock prevBlock = null;
            foreach (XmlElement element in document.GetElementsByTagName("Block"))
            {
                Type blockType = Type.GetType(element.GetAttribute("Type"));
                ParameterInfo[] blockParams = blockType.GetConstructors()[0].GetParameters();

                BaseBlock block = null;
                if (blockParams.Any())
                {
                    // 블럭 생성자에 인자가 필요함
                    block = Activator.CreateInstance(blockType, Deserialize(System.IO.Path.GetDirectoryName(path) + "\\" + element.GetAttribute("Content"))) as BaseBlock;
                }
                else
                {
                    // 인자 없이 즉시 블럭 생성이 가능
                    block = Activator.CreateInstance(blockType) as BaseBlock;
                }
                block.Position = new Point(50, 50);

                if (prevBlock is EventBlock)
                {
                    (prevBlock as EventBlock).NextConnectHole.StatementBlock = block as StatementBlock;
                    block.Position = new Point(0, 0);
                    AddBlock(prevBlock);
                }

                prevBlock = block;
            }
        }

        private void Serialize(string path, object target)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                formatter.Serialize(stream, target);
            }
        }

        private object Deserialize(string path)
        {
            object result;

            BinaryFormatter formatter = new BinaryFormatter(); ;
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                result = formatter.Deserialize(stream);
            }
            return result;
        }
        #endregion
    }
}
