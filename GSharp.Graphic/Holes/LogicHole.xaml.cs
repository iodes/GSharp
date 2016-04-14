using GSharp.Base.Cores;
using GSharp.Graphic.Core;
using System;

namespace GSharp.Graphic.Holes
{
    /// <summary>
    /// LogicHole.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LogicHole : BaseObjectHole
    {
        public GLogic Logic;

        public override event HoleEventArgs BlockChanged;

        public override BaseBlock Block {
            get
            {
                return LogicBlock;
            }
        }

        public LogicBlock LogicBlock
        {
            get
            {
                return RealLogicBlock.Child as LogicBlock;
            }
            set
            {
                if (value == RealLogicBlock.Child) return;

                if (RealLogicBlock.Child != null)
                {
                    throw new Exception("이미 블럭이 존재합니다.");
                }

                BlockChanged?.Invoke(Block, value);
                RealLogicBlock.Child = value;
            }
        }

        public override ObjectBlock BaseObjectBlock {
            get
            {
                return LogicBlock;
            }
        }

        public LogicHole()
        {
            InitializeComponent();
        }
    }
}
