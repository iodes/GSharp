using GSharp.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Base.Cores
{
    public interface IModule
    {
        GCommand GCommand { get; }
    }
}
