using System;
using GSharp.Base.Cores;
using GSharp.Base.Utilities;

namespace GSharp.Base.Objects.Logics
{
    [Serializable]
    public class GNot : GLogic
    {
        public GLogic TargetLogic { get; set; }

        #region 생성자
        public GNot(GLogic logic)
        {
            TargetLogic = logic;
        }
        #endregion

        public override string ToSource()
        {
            return string.Format("!({0})",TargetLogic.ToSource());
        }
    }
}
