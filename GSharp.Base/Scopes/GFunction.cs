using System.Text;
using System.Collections.Generic;
using GSharp.Base.Cores;
using GSharp.Base.Utilities;
using System;

namespace GSharp.Base.Scopes
{
    public class GFunction : GScope
    {
        public string Name { get; set; }

        public GFunction(string name)
        {
            Name = name;
        }

        public override string ToSource()
        {
            return "";
        }
    }
}
