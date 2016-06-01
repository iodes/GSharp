
using GSharp.Base.Objects;
using GSharp.Graphic.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Graphic.Objects
{
    public interface IVariableBlock
    {
        string FriendlyName { get; set; }
        IVariable IVariable { get; }
    }
}
