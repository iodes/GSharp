
using GSharp.Base.Objects;
using GSharp.Extension;
using GSharp.Graphic.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Graphic.Objects
{
    public interface IPropertyBlock : IModuleBlock
    {
        IProperty IProperty { get; }
    }
}
