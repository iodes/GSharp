using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Base.Objects
{
    public interface IVariable
    {
        string Name { get; set; }

        Type VariableType { get; }
    }
}
