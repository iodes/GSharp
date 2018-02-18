using System.Collections.Generic;

namespace GSharp.Bootstrap.Operators
{
    public static class NotEqualExtension
    {
        public static bool IsNotEqualThan(this bool objA, dynamic objB)
        {
            return objA != objB.ToBool();
        }

        public static bool IsNotEqualThan(this double objA, dynamic objB)
        {
            return objA != objB.ToNumber();
        }

        public static bool IsNotEqualThan(this object objA, dynamic objB)
        {
            return objA != objB.ToCustom();
        }

        public static bool IsNotEqualThan(this string objA, dynamic objB)
        {
            return objA != objB.ToText();
        }

        public static bool IsNotEqualThan(this List<object> objA, dynamic objB)
        {
            return objA != objB.ToList();
        }
    }
}
