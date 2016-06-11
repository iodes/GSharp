using GSharp.Base.Cores;
using GSharp.Extension;
using System;

namespace GSharp.Base.Objects
{
    [Serializable]
    public abstract class GCustom : GObject
    {
        public abstract Type CustomType { get; }
    }
}
