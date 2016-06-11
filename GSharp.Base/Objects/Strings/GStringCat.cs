using System;
using System.Linq;
using GSharp.Base.Cores;
using GSharp.Extension;

namespace GSharp.Base.Objects.Strings
{
    [Serializable]
    public class GStringCat : GString
    {
        public GObject FirstPart { get; set; }

        public GObject SecondPart { get; set; }

        #region 생성자
        public GStringCat(GObject first, GObject second)
        {
            FirstPart = first;
            SecondPart = second;
        }
        #endregion

        public override string ToSource()
        {
            string firstPartStr = FirstPart?.ToSource();
            string secondPartStr = SecondPart?.ToSource();
            if (!(FirstPart is GString))
            {
                firstPartStr += ".ToText()";
            }

            if (!(SecondPart is GString))
            {
                secondPartStr += ".ToText()";
            }

            return string.Format("({0} + {1})", firstPartStr, secondPartStr);
        }
    }
}
