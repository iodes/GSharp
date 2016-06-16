using GSharp.Base.Cores;
using GSharp.Graphic.Holes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GSharp.Graphic.Blocks
{
    public abstract class ScopeBlock : BaseBlock
    {
        public abstract GScope GScope { get; }

        public abstract NextConnectHole NextConnectHole { get; }

        public ScopeBlock()
        {
        }

        protected override sealed void SaveNextBlock(XmlWriter writer)
        {
            NextConnectHole.AttachedBlock?.SaveXML(writer);
        }
    }
}
