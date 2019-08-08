using System;
using System.Globalization;
using GSharp.Base.Cores;

namespace GSharp.Base.Objects.Numbers
{
    [Serializable]
    public class GNumberConst : GNumber
    {
        #region Properties
        public double Number { get; }
        #endregion

        #region Initializer
        public GNumberConst(double number)
        {
            Number = number;
        }
        #endregion

        public override string ToSource()
        {
            return Number.ToString(CultureInfo.InvariantCulture);
        }
    }
}
