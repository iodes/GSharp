using System.Collections.Generic;

namespace GSharp.Bootstrap.Operators
{
    public static class LessEqualExtension
    {
        public static bool IsLessEqualThan(this bool objA, dynamic objB)
        {
            return objA == objB.ToBool();
        }

        public static bool IsLessEqualThan(this double objA, dynamic objB)
        {
            return objA <= objB.ToNumber();
        }

        public static bool IsLessEqualThan(this object objA, dynamic objB)
        {
            return objA <= objB.ToCustom();
        }

        public static bool IsLessEqualThan(this string objA, dynamic objB)
        {
            return objA.Length <= objB.ToText().Length;
        }

        public static bool IsLessEqualThan(this List<object> objA, dynamic objB)
        {
            return objA.Count <= objB.ToList().Count;
        }
    }
}
