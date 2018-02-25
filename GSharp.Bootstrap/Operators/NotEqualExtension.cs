using GSharp.Bootstrap.DataTypes;
using GSharp.Bootstrap.Extensions;
using System.Collections.Generic;

namespace GSharp.Bootstrap.Operators
{
    public static class NotEqualExtension
    {
        public static bool IsNotEqualThan(this object objA, object objB)
        {
            if (objA.IsNumeric() || objB.IsNumeric())
                return objA.ToNumber() != objB.ToNumber();

            if (objA is bool)
                return objA.ToBool() != objB.ToBool();

            if (objA is string)
                return objA.ToText() != objB.ToText();

            if (objA is List<object>)
                return objA.ToList() != objB.ToList();

            return false;
        }
    }
}
