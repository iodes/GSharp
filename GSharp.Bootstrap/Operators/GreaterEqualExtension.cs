using System.Collections.Generic;

namespace GSharp.Bootstrap.Operators
{
    public static class GreaterEqualExtension
    {
        public static bool IsGreaterEqualThan(this bool objA, dynamic objB)
        {
            return objA == objB.ToBool();
        }

        public static bool IsGreaterEqualThan(this double objA, dynamic objB)
        {
            return objA >= objB.ToNumber();
        }

        public static bool IsGreaterEqualThan(this object objA, dynamic objB)
        {
            return objA >= objB.ToCustom();
        }

        public static bool IsGreaterEqualThan(this string objA, dynamic objB)
        {
            return objA.Length >= objB.ToText().Length;
        }

        public static bool IsGreaterEqualThan(this List<object> objA, dynamic objB)
        {
            return objA.Count >= objB.ToList().Count;
        }
    }
}
