using System;
using GSharp.Base.Cores;
using GSharp.Base.Utilities;

namespace GSharp.Base.Objects.Logics
{
    [Serializable]
    public class GNot : GLogic
    {
        public GObject Target { get; set; }

        #region 생성자
        public GNot(GObject logic)
        {
            Target = logic;
        }
        #endregion

        public override string ToSource()
        {
            string targetStr = Target.ToSource();
            if (!(Target is GLogic))
            {
                targetStr += ".ToBool()";
            }

            return string.Format("(!{0})", targetStr);
        }
    }
}
