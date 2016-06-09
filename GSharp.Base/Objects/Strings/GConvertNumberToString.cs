using GSharp.Base.Cores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Base.Objects.Strings
{
    public class GConvertNumberToString : GString
    {
        public GObject TargetNumber { get; set; }

        public GConvertNumberToString(GObject number)
        {
            TargetNumber = number;
        }

        public override string ToStringSource()
        {
            return string.Format("{0}.ToGString()", TargetNumber?.ToSource());
        }
    }
}
