using GSharp.Base.Cores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Base.Objects
{
    public abstract class GSettableObject : GObject
    {
        public abstract Type SettableType { get; }
    }
}
