using GSharp.Base.Cores;
using GSharp.Extension;
using GSharp.Graphic.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Graphic.Core
{
    public interface ICallBlock : IModuleBlock
    {
        ICall ICall { get; }
    }
}
