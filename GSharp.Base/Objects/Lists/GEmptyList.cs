using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Base.Objects.Lists
{
    public class GEmptyList : GList
    {
        public override string ToSource()
        {
            return "new List<object>()";
        }
    }
}
