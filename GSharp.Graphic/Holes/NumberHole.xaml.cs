using GSharp.Graphic.Blocks;
using GSharp.Graphic.Objects;
using GSharp.Graphic.Objects.Numbers;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// VariableHole.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class NumberHole : BaseObjectHole
    {
        public override event HoleEventArgs BlockAttached;
        public override event HoleEventArgs BlockDetached;

        public override BaseBlock AttachedBlock
        {
            get
            {
                return NumberBlock;
            }
        }

        public override ObjectBlock BaseObjectBlock
        {
            get
            {
                return NumberBlock;
            }
            set
            {
                NumberBlock = value;
            }
        }

        public ObjectBlock NumberBlock
        {
            get
            {
                if (BlockHole.Child != null)
                {
                    return BlockHole.Child as ObjectBlock;
                }
                else
                {
                    return BlockNumber;
                }
            }
            set
            {
                var prevBlock = NumberBlock;

                // 제거는 DetachBlock으로
                if (value == null)
                {
                    DetachBlock();
                    return;
                }

                // 같은 블럭을 연결하려고 한 경우 무시
                if (value == prevBlock) return;

                // 기존 블럭이 존재할 경우
                if (prevBlock != null && !(prevBlock is NumberConstBlock))
                {
                    throw new Exception("이미 블럭이 존재합니다.");
                }

                // 연결하려는 블럭을 부모에서 제거
                value?.ParentHole?.DetachBlock();

                // 블럭 연결
                value.ParentHole = this;
                BlockAttached?.Invoke(this, value);
                BlockHole.Child = value;
                
                // 상수 블럭을 보이지 않도록 변경
                BlockNumber.Visibility = Visibility.Hidden;
            }
        }

        private Type NumberType = typeof(double);

        public NumberHole()
        {
            InitializeComponent();
        }

        public NumberHole(double initialValue) : this()
        {
            BlockNumber.Number = initialValue;
        }

        public NumberHole(Type type) : this()
        {
            NumberType = type;
        }

        public NumberHole(Type type, double initialValue) :this()
        {
            BlockNumber.Number = initialValue;
            NumberType = type;
        }

        public override BaseBlock DetachBlock()
        {
            var block = AttachedBlock;

            if (block is NumberConstBlock)
            { 
                return null;
            }

            // Detach 이벤트 호출
            BlockDetached?.Invoke(this, block);

            // 블럭 연결 해제
            BlockHole.Child = null;

            // 상수 블럭을 보이도록 변경
            BlockNumber.Visibility = Visibility.Visible;

            return block;
        }
    }
}
