using GSharp.Graphic.Blocks;
using GSharp.Graphic.Holes;
using System.Xml;

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

        protected override sealed void SaveNextBlock(XmlWriter writer)
        {
            NextConnectHole.AttachedBlock?.SaveXML(writer);
        }
    }
}
