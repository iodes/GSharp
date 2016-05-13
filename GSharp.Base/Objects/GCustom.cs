using GSharp.Extension;
using System;

namespace GSharp.Base.Cores
{
    [Serializable]
    public abstract class GCustom : GObject
    {
        public abstract Type Type { get; }
    }
}
