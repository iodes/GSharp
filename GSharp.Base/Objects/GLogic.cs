using GSharp.Base.Cores;
using System;

namespace GSharp.Base.Objects
{
    [Serializable]
    public abstract class GLogic : GObject
    {
        public abstract string ToLogicSource();

        public override sealed string ToSource()
        {
            return string.Format("((GLogic){0})", ToLogicSource());
        }


    }
}
