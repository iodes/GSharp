using GSharp.Base.Cores;
using GSharp.Graphic.Holes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Graphic.Core
{
    public abstract class ScopeBlock : BaseBlock
    {
        public abstract GScope GScope { get; }

        public abstract NextConnectHole NextConnectHole { get; }

        public ScopeBlock()
        {
        }
    }
}
