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
            return string.Format("({0}.ToText() + {1}.ToText())", FirstPart?.ToSource(), SecondPart?.ToSource());
        }
    }
}
