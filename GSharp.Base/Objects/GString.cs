using System;
using GSharp.Base.Cores;

namespace GSharp.Base.Objects
{
    [Serializable]
    public abstract class GString : GObject
    {
        public abstract string ToStringSource();

        public override sealed string ToSource()
        {
            return string.Format("((GString){0})", ToStringSource());
        }
    }
}
