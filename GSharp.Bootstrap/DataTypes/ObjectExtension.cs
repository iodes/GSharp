using GSharp.Bootstrap.Extensions;
using System;
using System.Collections.Generic;

namespace GSharp.Bootstrap.DataTypes
{
    public static class ObjectExtension
    {
        public static double ToNumber(this object obj)
        {
            if (obj is string)
                return ((string)obj).ToNumber();

            if (obj is bool)
                return ((bool)obj).ToNumber();

            if (obj is List<object>)
                return ((List<object>)obj).ToNumber();

            if (obj.IsNumeric())
                return Convert.ToDouble(obj);

            return 0;
        }

        public static string ToText(this object obj)
        {
            if (obj is string)
                return ((string)obj).ToText();

            if (obj is double)
                return ((double)obj).ToText();

            if (obj is bool)
                return ((bool)obj).ToText();

            if (obj is List<object>)
                return ((List<object>)obj).ToText();

            return obj.ToString();
        }

        public static bool ToBool(this object obj)
        {
            if (obj is string)
                return ((string)obj).ToBool();

            if (obj is bool)
                return ((bool)obj).ToBool();

            if (obj is List<object>)
                return ((List<object>)obj).ToBool();

            if (obj.IsNumeric())
                return Convert.ToBoolean(obj);

            return true;
        }

        public static T ToCustom<T>(this object obj)
        {
            try
            {
                return (T)Convert.ChangeType(obj, typeof(T));
            }
            catch
            {
                return default(T);
            }
        }

        public static List<object> ToList(this object obj)
        {
            if (obj is string)
                return ((string)obj).ToList();

            if (obj is double)
                return ((double)obj).ToList();

            if (obj is bool)
                return ((bool)obj).ToList();

            if (obj is List<object>)
                return ((List<object>)obj).ToList();

            return new List<object>() { obj };
        }
    }
}
