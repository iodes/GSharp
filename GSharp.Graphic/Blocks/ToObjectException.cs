using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Graphic.Blocks
{
    class ToObjectException : Exception
    {
        private BaseBlock where;

        public BaseBlock Where
        {
            get
            {
                return where;
            }
        }

        public ToObjectException(string message, BaseBlock block) : base(message)
        {
            where = block;
        }
    }
}
