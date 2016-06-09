using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Base.Objects.Strings
{
    [Serializable]
    public class GStringConst : GString
    {
        #region 속성
        public string String { get; set; }
        #endregion

        #region 생성자
        public GStringConst(string valueString)
        {
            String = valueString;
        }
        #endregion

        public override string ToStringSource()
        {
            return string.Format
                (
                    "\"{0}\"",
                    String
                );
        }
    }
}
