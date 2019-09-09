using System;
using GSharp.Base.Cores;

namespace GSharp.Base.Objects.Logics
{
    [Serializable]
    public class GNot : GLogic
    {
        #region Properties
        public GObject Target { get; }
        #endregion

        #region Initializer
        public GNot(GObject logic)
        {
            Target = logic;
        }
        #endregion

        public override string ToSource()
        {
            var target = Target?.ToSource() + (!(Target is GLogic) ? ".ToBool()" : string.Empty);
            
            return $"(!{target})";
        }
    }
}
