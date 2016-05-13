using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Base.Objects.Strings
{
    public class GConvertNumberToString : GString
    {
        public GNumber TargetNumber { get; set; }

        public GConvertNumberToString(GNumber number)
        {
            TargetNumber = number;
        }

        public override string ToSource()
        {
            return string.Format("{0}.ToString()", TargetNumber?.ToSource());
        }
    }
}
