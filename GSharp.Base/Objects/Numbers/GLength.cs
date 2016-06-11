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

        public override string ToSource()
        {
            string targetStr = Target?.ToSource();
            if (!(Target is GString))
            {
                targetStr += ".ToNumber()";
            }
            
            return string.Format("{0}.Length", targetStr);
        }
    }
}
