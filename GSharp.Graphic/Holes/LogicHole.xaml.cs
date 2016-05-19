using GSharp.Base.Cores;
using GSharp.Graphic.Blocks;
using GSharp.Graphic.Objects;
using System;

namespace GSharp.Graphic.Holes
{
    /// <summary>
    /// LogicHole.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LogicHole : BaseObjectHole
    {
        public override event HoleEventArgs BlockAttached;
        public override event HoleEventArgs BlockDetached;

        public override BaseBlock AttachedBlock {
            get
            {
                return LogicBlock;
            }
        }

        public override ObjectBlock BaseObjectBlock
        {
            get
            {
                return LogicBlock;
            }
        }

        public LogicBlock LogicBlock
        {
            get
            {
                return BlockHole.Child as LogicBlock;
            }
            set
            {
                // 같은 블럭을 연결하려고 한 경우 무시
                if (value == BlockHole.Child) return;

                // 이미 블럭이 존재하는 경우
                if (BlockHole.Child != null)
                {
                    throw new Exception("이미 블럭이 존재합니다.");
                }

                // 연결하려는 블럭을 부모에서 제거
                value?.ParentHole?.DetachBlock();

                // 블럭 연결
                value.ParentHole = this;
                BlockAttached?.Invoke(this, value);
                BlockHole.Child = value;
            }
        }

        public LogicHole()
        {
            InitializeComponent();
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
            if (!(block is LogicBlock))
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
