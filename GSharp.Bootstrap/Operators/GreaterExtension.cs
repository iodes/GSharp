using System.Collections.Generic;

namespace GSharp.Bootstrap.Operators
{
    public static class GreaterExtension
    {
        public static bool IsGreaterThan(this bool objA, dynamic objB)
        {
            return objA == objB.ToBool();
        }

        public static bool IsGreaterThan(this double objA, dynamic objB)
        {
            return objA > objB.ToNumber();
        }

        public static bool IsGreaterThan(this object objA, dynamic objB)
        {
            return objA > objB.ToCustom();
        }

        public static bool IsGreaterThan(this string objA, dynamic objB)
        {
            return objA.Length > objB.ToText().Length;
        }

        public static bool IsGreaterThan(this List<object> objA, dynamic objB)
        {
            return objA.Count > objB.ToList().Count;
        }
    }
}
