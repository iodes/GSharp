using GSharp.Graphic.Holes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Graphic.Statements
{
    public interface IContainChildBlock
    {
        NextConnectHole ChildConnectHole { get; }
    }
}
