using System;
using GSharp.Base.Cores;

namespace GSharp.Base.Objects
{
    [Serializable]
    public abstract class GNumber : GObject
    {
        public abstract string ToNumberSource();

        public override sealed string ToSource()
        {
            return string.Format("((GNumber){0})", ToNumberSource());
        }
    }
}
