using System.Collections.Generic;

namespace GSharp.Bootstrap.Operators
{
    public static class LessExtension
    {
        public static bool IsLessThan(this bool objA, dynamic objB)
        {
            return objA == objB.ToBool();
        }

        public static bool IsLessThan(this double objA, dynamic objB)
        {
            return objA < objB.ToNumber();
        }

        public static bool IsLessThan(this object objA, dynamic objB)
        {
            return objA < objB.ToCustom();
        }

        public static bool IsLessThan(this string objA, dynamic objB)
        {
            return objA.Length < objB.ToText().Length;
        }

        public static bool IsLessThan(this List<object> objA, dynamic objB)
        {
            return objA.Count < objB.ToList().Count;
        }
    }
}
