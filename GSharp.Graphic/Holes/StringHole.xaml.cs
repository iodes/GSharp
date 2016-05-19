using GSharp.Graphic.Blocks;
using GSharp.Graphic.Objects;
using GSharp.Graphic.Objects.Strings;
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
    /// StringHole.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class StringHole : BaseObjectHole
    {
        public override event HoleEventArgs BlockAttached;
        public override event HoleEventArgs BlockDetached;

        public override BaseBlock AttachedBlock
        {
            get
            {
                return StringBlock;
            }
        }

        public override ObjectBlock BaseObjectBlock
        {
            get
            {
                return StringBlock;
            }
        }

        public StringBlock StringBlock
        {
            get
            {
                if (BlockHole.Child != null)
                {
                    return BlockHole.Child as StringBlock;
                }
                else
                {
                    return BlockString;
                }
            }
            set
            {
                var prevBlock = StringBlock;

                // 제거는 DetachBlock으로
                if (value == null)
                {
                    DetachBlock();
                    return;
                }

                // 같은 블럭을 연결하려고 한 경우 무시
                if (value == prevBlock) return;

                // 기존 블럭이 존재할 경우
                if (prevBlock != null && !(prevBlock is StringConstBlock))
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
                BlockString.Visibility = Visibility.Hidden;
            }
        }

        public StringHole()
        {
            InitializeComponent();
        }

        public override BaseBlock DetachBlock()
        {
            var block = AttachedBlock;

            if (block is StringConstBlock)
            {
                return null;
            }

            // Detach 이벤트 호출
            BlockDetached?.Invoke(this, block);

            // 블럭 연결 해제
            BlockHole.Child = null;

            // 상수 블럭을 보이도록 변경
            BlockString.Visibility = Visibility.Visible;

            return block;
        }

        public override bool CanAttachBlock(BaseBlock block)
        {
            if (!(block is StringBlock))
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
