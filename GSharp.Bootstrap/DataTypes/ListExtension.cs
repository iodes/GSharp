using System.Collections.Generic;
using System.Linq;

namespace GSharp.Bootstrap.DataTypes
{
    public static class ListExtension
    {
        public static double ToNumber(this List<object> list)
        {
            return list.Count;
        }

        public static string ToText(this List<object> list)
        {
            var strList = from item in list select item.ToText();

            return "[" + string.Join(", ", strList) + "]";
        }

        public static bool ToBool(this List<object> list)
        {
            return list != null && list.Any();
        }

        public static object ToCustom(this List<object> list)
        {
            return null;
        }

        public static List<object> ToList(this List<object> list)
        {
            return list;
        }
    }
}
