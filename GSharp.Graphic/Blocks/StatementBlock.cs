using GSharp.Base.Cores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Graphic.Blocks
{
    public abstract class StatementBlock : BaseBlock
    {
        public abstract GStatement GStatement { get; }

        public abstract StatementBlock GetLastBlock();
    }
}
