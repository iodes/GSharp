
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
using System.Collections;
using GSharp.Graphic.Objects;

namespace GSharp.Graphic.Controls
{
    /// <summary>
    /// BlockEditor.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class BlockEditor : UserControl
    {

        #region 속성
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
        // Statement Hole
        private List<NextConnectHole> NextConnectHoleList = new List<NextConnectHole>();

        // Logic Hole
        private List<LogicHole> LogicHoleList = new List<LogicHole>();

        // Object Hole
        private List<ObjectHole> ObjectHoleList = new List<ObjectHole>();
        private List<NumberHole> NumberHoleList = new List<NumberHole>();
        private List<StringHole> StringHoleList = new List<StringHole>();
        private List<CustomHole> CustomHoleList = new List<CustomHole>();

        // Variable Hole
        //private List<VariableHole> VariableHoleList = new List<VariableHole>();

        // 선택된 대상이 움직였는지 체크
        private bool IsSelectedBlockMoved = false;

        // 변수 목록
        private Dictionary<string, GDefine> VariableList = new Dictionary<string, GDefine>();

        // 함수 목록
        private Dictionary<string, GFunction> FunctionList = new Dictionary<string, GFunction>();
        #endregion

        #region 생성자
        public BlockEditor()
        {
            InitializeComponent();

            Panel.SetZIndex(Highlighter, int.MaxValue - 1);
            BlockGrid.Children.Add(Highlighter);
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

            Panel.SetZIndex(SelectedBlock, int.MaxValue - 2);
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

            Panel.SetZIndex(SelectedBlock, 1);

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
            VariableList[varName] = new GDefine(varName);
        }

        // 변순 선언 해제
        public void UnDefineVariable(string varName)
        {
            VariableList.Remove(varName);
        }

        // 변수 목록
        public List<string> GetVariableNameList()
        {
            return VariableList.Keys.ToList();
        }
        
        public List<GDefine> GetDefineList()
        {
            return VariableList.Values.ToList();
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
                if (block.HoleList.Contains(hole))
                {
                    continue;
                }

                if (block.Type != hole.Type)
                {
                    continue;
                }

                var position = hole.TranslatePoint(new Point(0, 0), BlockGrid);

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
                if (block.HoleList.Contains(hole))
                {
                    continue;
                }

                var position = hole.TranslatePoint(new Point(0, 0), BlockGrid);

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
            else */if (MargnetTarget is CustomHole)
            {
                var customHole = MargnetTarget as CustomHole;

                // Type이 다르면 연결할 수 없음
                if (customHole.Type != block.Type)
                {
                    return;
                }

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
            BlockGrid.Children.Remove(block);
        }

        private void AttachToCanvas(BaseBlock block)
        {
            BlockGrid.Children.Add(block);
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

                foreach (GDefine define in VariableList.Values)
                {
                    entry.Append(define);
                }

                foreach (var block in BlockGrid.Children)
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
        #endregion
    }
}
