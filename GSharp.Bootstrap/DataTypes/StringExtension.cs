using System.Collections.Generic;

namespace GSharp.Bootstrap.DataTypes
{
    public static class StringExtension
    {
        public static double ToNumber(this string str)
        {
            if (double.TryParse(str, out double result))
            {
                return result;
            }

            return 0;
        }

        public static string ToText(this string str)
        {
            return str;
        }

        public static bool ToBool(this string str)
        {
            if (str == "" || str == null)
            {
                return false;
            }

            return true;
        }

        public static object ToCustom(this string str)
        {
            return null;
        }

        public static List<object> ToList(this string str)
        {
            return new List<object>() { str };
        }
    }
}
