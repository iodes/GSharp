using GSharp.Base.Cores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Graphic.Core
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
