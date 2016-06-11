using GSharp.Base.Cores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Base.Objects.Lists
{
    public class GItem : GObject
    {
        private GObject Target;
        private GObject Index;

        public GItem(GObject target, GObject idx)
        {
            Target = target;
            Index = idx;
        }

        public override string ToSource()
        {
            string targetStr = Target.ToSource();
            string indexStr = Index.ToSource();

            if (!(Target is GList))
            {
                targetStr += ".ToList()";
            }

            if (!(Index is GNumber))
            {
                indexStr += ".ToNumber()";
            }

            return string.Format("{0}[{1}]", targetStr, indexStr);
        }
    }
}
