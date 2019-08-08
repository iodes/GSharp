using System;
using GSharp.Base.Cores;

namespace GSharp.Base.Objects.Numbers
{
    [Serializable]
    public class GLength : GNumber
    {
        #region Properties
        public GObject Target { get; }
        #endregion

        #region Initializer
        public GLength(GObject target)
        {
            Target = target;
        }
        #endregion

        public override string ToSource()
        {
            var target = Target?.ToSource() + (!(Target is GString) ? ".ToText()" : string.Empty);
            
            return $"{target}.Length";
        }
    }
}
