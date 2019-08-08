using System;

namespace GSharp.Base.Cores
{
    [Serializable]
    public abstract class GBase
    {
        /// <summary>
        /// Convert object to native source.
        /// </summary>
        public abstract string ToSource();
    }
}
