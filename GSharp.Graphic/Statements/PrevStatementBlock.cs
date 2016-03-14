using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GSharp.Graphic.Core;
using GSharp.Graphic.Holes;
using GSharp.Cores;

namespace GSharp.Graphic.Statements
{
    public abstract class PrevStatementBlock : StatementBlock
    {
        public abstract NextConnectHole NextConnectHole { get; }

        public PrevStatementBlock()
        {
        }

        public override StatementBlock GetLastBlock()
        {
            if (NextConnectHole.StatementBlock == null)
            {
                return this;
            }

            return NextConnectHole.StatementBlock.GetLastBlock();
        }
    }
}
