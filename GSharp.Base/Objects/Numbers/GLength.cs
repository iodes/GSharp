using System;
using GSharp.Base.Cores;

namespace GSharp.Base.Objects.Numbers
{
    [Serializable]
    public class GLength : GNumber
    {
        #region 속성
        public GObject Target { get; set; }
        #endregion

        #region 생성자
        public GLength(GObject target)
        {
            Target = target;
        }
        #endregion

        public override string ToNumberSource()
        {
            return string.Format("{0}.ToGString().String.Length", Target?.ToSource());
        }
    }
}
