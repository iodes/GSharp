using System;
using System.Linq;
using GSharp.Base.Cores;
using GSharp.Extension;

namespace GSharp.Base.Objects.Strings
{
    [Serializable]
    public class GStringCat : GString, ICall
    {
        public GString FirstPart { get; set; }

        public GString SecondPart { get; set; }

        #region 생성자
        public GStringCat(GString first, GString second)
        {
            FirstPart = first;
            SecondPart = second;
        }
        #endregion

        public override string ToSource()
        {
            return string.Format("({0} + {1})", FirstPart?.ToSource(), SecondPart?.ToSource());
        }
    }
}
