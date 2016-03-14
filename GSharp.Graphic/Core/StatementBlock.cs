using GSharp.Cores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Graphic.Core
{
    public abstract class StatementBlock : BaseBlock
    {
        public GStatement GStatement;

        public StatementBlock()
        {
        }

        public abstract StatementBlock GetLastBlock();
    }
}
