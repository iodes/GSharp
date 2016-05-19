using GSharp.Base.Cores;
using GSharp.Extension;
using GSharp.Graphic.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Graphic.Blocks
{
    public interface ICallBlock : IModuleBlock
    {
        ICall ICall { get; }
    }
}
