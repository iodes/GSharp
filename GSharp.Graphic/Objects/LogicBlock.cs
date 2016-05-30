using GSharp.Base.Cores;
using GSharp.Graphic.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Graphic.Objects
{
    public abstract class LogicBlock : ObjectBlock
    {
        public override GObject GObject
        {
            get
            {
                return GLogic;
            }
        }

        public abstract GLogic GLogic { get; }

        public LogicBlock()
        {
        }
    }
}
