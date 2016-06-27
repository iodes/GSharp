using GSharp.Base.Cores;
using GSharp.Base.Objects;
using GSharp.Graphic.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace GSharp.Graphic.Objects
{
    public interface ICustomBlock
    {
        Type CustomType { get; }
    }
}
