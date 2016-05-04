using GSharp.Graphic.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Graphic.Objects
{
    public abstract class CustomBlock : ObjectBlock
    {
        public Type Type
        {
            get
            {
                return _Type;
            }
        }
        private Type _Type;

        public CustomBlock(Type type)
        {
            _Type = type;
        }
    }
}
