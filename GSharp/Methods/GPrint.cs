using GSharp.Cores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Methods
{
    public class GPrint : GMethod
    {
        public override string ToSource()
        {
            return "Console.WriteLine";
        }
    }
}
