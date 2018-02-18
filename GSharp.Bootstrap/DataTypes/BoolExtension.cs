using System.Collections.Generic;

namespace GSharp.Bootstrap.DataTypes
{
    public static class BoolExtension
    {
        public static double ToNumber(this bool logic)
        {
            return logic ? 1 : 0;
        }

        public static string ToText(this bool logic)
        {
            return logic ? "참" : "거짓";
        }

        public static bool ToBool(this bool logic)
        {
            return logic;
        }

        public static object ToCustom(this bool logic)
        {
            return null;
        }

        public static List<object> ToList(this bool logic)
        {
            return new List<object>() { logic };
        }
    }
}
