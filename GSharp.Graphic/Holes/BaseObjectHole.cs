using GSharp.Base.Cores;
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
        public abstract ObjectBlock BaseObjectBlock { get; }

        public override BaseBlock AttachedBlock
        {
            get
            {
                return BaseObjectBlock;
            }
        }

        public BaseObjectHole()
        {

        }
    }
}
