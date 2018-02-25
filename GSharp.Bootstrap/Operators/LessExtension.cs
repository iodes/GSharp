using GSharp.Bootstrap.DataTypes;
using GSharp.Bootstrap.Extensions;
using System.Collections.Generic;

namespace GSharp.Bootstrap.Operators
{
    public static class LessExtension
    {
        public static bool IsLessThan(this object objA, object objB)
        {
            if (objA.IsNumeric() || objB.IsNumeric())
                return objA.ToNumber() < objB.ToNumber();

            if (objA is bool)
                return objA.ToBool() == objB.ToBool();

            if (objA is string)
                return objA.ToText().Length < objB.ToText().Length;

            if (objA is List<object>)
                return objA.ToList().Count < objB.ToList().Count;

            return false;
        }
    }
}
