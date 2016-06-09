using GSharp.Base.Cores;
using GSharp.Extension;
using System;

namespace GSharp.Base.Objects
{
    [Serializable]
    public abstract class GCustom : GObject
    {
        public abstract Type CustomType { get; }

        public abstract string ToCustomSource();

        public override string ToSource()
        {
            return string.Format("((GCustom){0})", ToCustomSource());
        }
    }
}
