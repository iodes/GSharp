using GSharp.Base.Cores;

namespace GSharp.Base.Objects
{
    public class GNumber : GObject
    {
        #region 속성
        public double Number { get; set; }
        #endregion

        #region 생성자
        public GNumber(double number)
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
