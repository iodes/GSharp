using GSharp.Base.Cores;
using GSharp.Base.Objects;
using GSharp.Graphic.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Graphic.Objects
{
    public abstract class StringBlock : ObjectBlock
    {
        public abstract GString GString { get; }

        public StringBlock()
        {
        }
    }
}
