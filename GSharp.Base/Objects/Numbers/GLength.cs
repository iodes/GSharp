using System;
using GSharp.Base.Cores;

namespace GSharp.Base.Objects.Numbers
{
    [Serializable]
    public class GLength : GNumber
    {
        #region 속성
        public GString TargetString { get; set; }
        #endregion

        #region 생성자
        public GLength(GString target)
        {
            TargetString = target;
        }
        #endregion

        public override string ToSource()
        {
            return string.Format
                (
                    "({0}).Length()",
                    TargetString?.ToSource()
                );
        }
    }
}
