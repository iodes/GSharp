using System;
using System.Linq;
using GSharp.Base.Cores;
using GSharp.Extension;

namespace GSharp.Base.Objects.Strings
{
    [Serializable]
    public class GStringCat : GString, ICall
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

        public override string ToStringSource()
        {
            return string.Format("new GString({0}.ToGString().String + {1}.ToGstring().String)", FirstPart?.ToSource(), SecondPart?.ToSource());
        }
    }
}
