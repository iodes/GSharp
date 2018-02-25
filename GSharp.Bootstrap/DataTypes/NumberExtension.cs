using System;
using System.Collections.Generic;

namespace GSharp.Bootstrap.DataTypes
{
    public static class NumberExtension
    {
        public static double ToNumber(this double number)
        {
            return number;
        }

        public static string ToText(this double number)
        {
            return number.ToString();
        }

        public static bool ToBool(this double number)
        {
            return number != 0;
        }

        public static object ToCustom(this double number)
        {
            return null;
        }

        public static List<object> ToList(this double number)
        {
            return new List<object>() { number };
        }
    }
}
