using GSharp.Cores;

namespace GSharp.Objects
{
    public class GString : GObject
    {
        #region 속성
        public string String { get; set; }
        #endregion

        #region 생성자
        public GString(string valueString)
        {
            String = valueString;
        }
        #endregion

        public override string ToSource()
        {
            return string.Format
                (
                    "\"{0}\"",
                    String
                );
        }
    }
}
