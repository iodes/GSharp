using GSharp.Base.Cores;
using GSharp.Base.Objects;

namespace GSharp.Base.Statements
{
    public class GListAdd : GStatement
    {
        #region Properties
        public GSettableObject List { get; set; }

        public GObject Item { get; set; }
        #endregion

        public GListAdd(GSettableObject list, GObject item)
        {
            List = list;
            Item = item;
        }

        public override string ToSource()
        {
            return string.Format("{0} = {0}.ToList();\n" +
                                 "{0}.ToList().Add({1});\n", List.ToSource(), Item.ToSource());
        }
    }
}
