
using GSharp.Base.Objects;
using GSharp.Extension;
using GSharp.Graphic.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Graphic.Objects
{
    public interface ICallBlock : IModuleBlock
    {
        ICall ICall { get; }
    }
}
