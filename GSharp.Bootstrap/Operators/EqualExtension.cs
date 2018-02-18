using System.Collections.Generic;

namespace GSharp.Bootstrap.Operators
{
    public static class EqualExtension
    {
        public static bool IsEqualThan(this bool objA, dynamic objB)
        {
            return objA == objB.ToBool();
        }

        public static bool IsEqualThan(this double objA, dynamic objB)
        {
            return objA == objB.ToNumber();
        }

        public static bool IsEqualThan(this object objA, dynamic objB)
        {
            return objA == objB.ToCustom();
        }

        public static bool IsEqualThan(this string objA, dynamic objB)
        {
            return objA == objB.ToText();
        }

        public static bool IsEqualThan(this List<object> objA, dynamic objB)
        {
            return objA == objB.ToList();
        }
    }
}
