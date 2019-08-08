using GSharp.Base.Cores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Base.Objects.Strings
{
    public class GNumberToString : GString
    {
        #region Properties
        public GObject TargetNumber { get; }
        #endregion

        #region Initializer
        public GNumberToString(GObject number)
        {
            TargetNumber = number;
        }
        #endregion

        public override string ToSource()
        {
            return $"{TargetNumber?.ToSource()}.ToText()";
        }
    }
}
