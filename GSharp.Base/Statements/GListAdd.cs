using GSharp.Base.Cores;
using GSharp.Base.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Base.Statements
{
    public class GListAdd : GStatement
    {
        public GObject List { get; set; }
        public GObject Item { get; set; }

        public GListAdd(GObject list, GObject item)
        {
            List = list;
            Item = item;
        }

        public override string ToSource()
        {
            string listStr = List.ToSource();
            if (!(List is GList))
            {
                listStr += ".ToList()";
            }

            return string.Format("{0}.Add({1})", listStr, Item.ToSource());
        }
    }
}
