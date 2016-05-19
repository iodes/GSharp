using GSharp.Graphic.Blocks;
using GSharp.Graphic.Objects;
using GSharp.Graphic.Statements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GSharp.Graphic.Holes
{
    /// <summary>
    /// NextConnectHole.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class NextConnectHole : BaseStatementHole, INotifyPropertyChanged
    {
        public Brush BodyColor
        {
            get
            {
                return _BodyColor;
            }
            set
            {
                _BodyColor = value;
                OnPropertyChanged();
            }
        }
        private Brush _BodyColor = Brushes.White;

        public Brush BorderColor
        {
            get
            {
                return _BorderColor;
            }
            set
            {
                _BorderColor = value;
                OnPropertyChanged();
            }
        }
        private Brush _BorderColor = Brushes.DarkOrange;

        public override event HoleEventArgs BlockAttached;
        public override event HoleEventArgs BlockDetached;
        public event PropertyChangedEventHandler PropertyChanged;

        public override BaseBlock AttachedBlock
        {
            get
            {
                return StatementBlock;
            }
        }

        public StatementBlock StatementBlock
        {
            get
            {
                return BlockHole.Child as StatementBlock;
            }
            set
            {
                var prevBlock = StatementBlock;

                // 같은 블럭을 연결하려고 한 경우 무시
                if (value == prevBlock) return;

                // 기존 블럭이 존재할 경우
                if (prevBlock != null)
                {
                    var lastBlock = value.GetLastBlock();
                    
                    // 연결하려는 블럭이 끝 블럭인 경우 중단
                    if (!(lastBlock is PrevStatementBlock))
                    {
                        throw new Exception("끝 블럭이 중간에 들어갈 수 없습니다.");
                    }
                    
                    // 연결하려는 블럭 맨 뒤에 기존 블럭을 붙임
                    // 자동으로 기존 부모와 떨어짐
                    (lastBlock as PrevStatementBlock).NextConnectHole.StatementBlock = prevBlock;
                }

                // 연결하려는 블럭을 부모에서 제거
                value?.ParentHole?.DetachBlock();

                // 블럭 연결
                value.ParentHole = this;
                BlockAttached?.Invoke(this, value);
                BlockHole.Child = value;
            }
        }

        public NextConnectHole()
        {
            InitializeComponent();
            DataContext = this;
        }

        // 연결된 블럭을 제거
        public override BaseBlock DetachBlock()
        {
            var block = AttachedBlock;

            // Detach 이벤트 호출
            BlockDetached?.Invoke(this, block);

            // 블럭 연결 해제
            BlockHole.Child = null;

            return block;
        }

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public override bool CanAttachBlock(BaseBlock block)
        {
            if (!(block is StatementBlock))
            {
                return false;
            }

            if (block.AllHoleList.Contains(this))
            {
                return false;
            }

            if (block is IVariableBlock && !ParentBlock.AllowVariableList.Contains(block as IVariableBlock))
            {
                return false;
            }

            return true;
        }
    }
}
