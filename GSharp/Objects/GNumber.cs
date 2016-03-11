using GSharp.Cores;

namespace GSharp.Objects
{
    public class GNumber : GObject
    {
        #region 속성
        public long Number { get; set; }
        #endregion

        #region 생성자
        public GNumber(long valueNumber)
        {
            Number = valueNumber;
        }
        #endregion

        public override string ToSource()
        {
            return Number.ToString();
        }
    }
}
