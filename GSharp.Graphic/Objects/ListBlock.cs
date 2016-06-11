using GSharp.Base.Cores;
using GSharp.Base.Objects;
using GSharp.Graphic.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Graphic.Objects
{
    public abstract class ListBlock : ObjectBlock
    {
        public override GObject GObject
        {
            get
            {
                return GList;
            }
        }

        public abstract GList GList { get; }

        public ListBlock()
        {
        }
    }
}
