using GSharp.Graphic.Blocks;
using GSharp.Graphic.Objects;
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
    /// CustomHole.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CustomHole : BaseObjectHole
    {
        public override event HoleEventArgs BlockAttached;
        public override event HoleEventArgs BlockDetached;

        public override BaseBlock AttachedBlock
        {
            get
            {
                return CustomBlock;
            }
        }

        public override ObjectBlock BaseObjectBlock
        {
            get
            {
                return CustomBlock;
            }
        }

        public Type CustomType { get; set; }

        public CustomBlock CustomBlock
        {
            get
            {
                return BlockHole.Child as CustomBlock;
            }
            set
            {
                // 제거는 DetachBlock으로
                if (value == null)
                {
                    DetachBlock();
                    return;
                }

                // 같은 블럭일 경우 무시
                if (value == BlockHole.Child) return;

                // 이미 블럭이 존재하는 경우
                if (BlockHole.Child != null)
                {
                    throw new Exception("이미 블럭이 존재합니다.");
                }

                // 붙일 수 없는 블럭이면
                if (!CanAttachBlock(value))
                {
                    throw new Exception("연결할 수 없는 블럭입니다.");
                }

                // 연결하려는 블럭을 부모에서 제거
                value?.ParentHole?.DetachBlock();

                // 블럭 연결
                value.ParentHole = this;
                BlockAttached?.Invoke(this, value);
                BlockHole.Child = value;
            }
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

        public override bool CanAttachBlock(BaseBlock block)
        {
            if (!(block is CustomBlock))
            { 
                return false;
            }

            var customBlock = block as CustomBlock;
            if (!CustomType.IsAssignableFrom(CustomBlock.Type))
            {
                return false;
            }

            if (block.AllHoleList.Contains(this))
            {
                return false;
            }

            if (block is IVariableBlock && ParentBlock.AllowVariableList.Contains(block as IVariableBlock))
            {
                return false;
            }

            return true;
        }

        public CustomHole()
        {
            InitializeComponent();
        }

        public CustomHole(Type type) : this()
        {
            CustomType = type;
        }
    }
}
