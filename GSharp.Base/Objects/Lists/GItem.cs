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
        private int index;

        public GItem(GObject target, int idx)
        {
            Target = target;
            index = idx;
        }

        public override string ToSource()
        {
            string targetStr = Target.ToSource();
            if (!(Target is GList))
            {
                targetStr += ".ToList()";
            }

            return string.Format("{0}[{1}]", targetStr, index);
        }
    }
}
