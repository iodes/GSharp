using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Base.Objects.Strings
{
    [Serializable]
    public class GStringConst : GString
    {
        #region Properties
        public string Value { get; }
        #endregion

        #region Initializer
        public GStringConst(string value)
        {
            Value = value;
        }
        #endregion

        public override string ToSource()
        {
            return $"\"{Value}\"";
        }
    }
}
