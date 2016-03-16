using GSharp.Graphic.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Graphic.Holes
{
    public abstract class BaseObjectHole : BaseHole
    {
        public abstract ObjectBlock ObjBlock { get; }
    }
}
