using System;
using GSharp.Base.Cores;

namespace GSharp.Base.Objects.Numbers
{
    [Serializable]
    public class GNumberConst : GNumber
    {
        #region 속성
        public double Number { get; set; }
        #endregion

        #region 생성자
        public GNumberConst(double number)
        {
            Number = number;
        }
        #endregion

        public override string ToSource()
        {
            return Number.ToString();
        }
    }
}
