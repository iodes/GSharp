using System;
using System.Collections.Generic;

namespace GSharp.Bootstrap.DataTypes
{
    public static class ObjectExtension
    {
        public static double ToNumber(this object obj)
        {
            if (obj is string)
            {
                return ((string)obj).ToNumber();
            }

            if (obj is bool)
            {
                return ((bool)obj).ToNumber();
            }

            if (obj is List<object>)
            {
                return ((List<object>)obj).ToNumber();
            }

            if (NumberExtension.NumberTypes.Contains(obj.GetType()))
            {
                return Convert.ToDouble(obj);
            }

            return 0;
        }

        public static string ToText(this object obj)
        {
            if (obj is string)
            {
                return ((string)obj).ToText();
            }

            if (obj is double)
            {
                return ((double)obj).ToText();
            }

            if (obj is bool)
            {
                return ((bool)obj).ToText();
            }

            if (obj is List<object>)
            {
                return ((List<object>)obj).ToText();
            }

            return obj.ToString();
        }

        public static bool ToBool(this object obj)
        {
            if (obj is string)
            {
                return ((string)obj).ToBool();
            }

            if (obj is bool)
            {
                return ((bool)obj).ToBool();
            }

            if (obj is List<object>)
            {
                return ((List<object>)obj).ToBool();
            }

            if (NumberExtension.NumberTypes.Contains(obj.GetType()))
            {
                return Convert.ToBoolean(obj);
            }

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
            {
                return ((string)obj).ToList();
            }

            if (obj is double)
            {
                return ((double)obj).ToList();
            }

            if (obj is bool)
            {
                return ((bool)obj).ToList();
            }

            if (obj is List<object>)
            {
                return ((List<object>)obj).ToList();
            }

            return new List<object>() { obj };
        }

        #region object, string 비교
        // 이미 StringExtension에 정의됨
        public static bool IsEqualThan(this object target1, string target2)
        {
            return target2.IsEqualThan(target1);
        }

        public static bool IsNotEqualThan(this object target1, string target2)
        {
            return target2.IsNotEqualThan(target1);
        }

        public static bool IsLessEqualThan(this object target1, string target2)
        {
            return target2.IsGreaterEqualThan(target1);
        }

        public static bool IsGreaterEqualThan(this object target1, string target2)
        {
            return target2.IsLessEqualThan(target1);
        }

        public static bool IsLessThan(this object target1, string target2)
        {
            return target2.IsGreaterThan(target1);
        }

        public static bool IsGreaterThan(this object target1, string target2)
        {
            return target2.IsLessThan(target1);
        }
        #endregion

        #region object, bool 비교
        // 이미 BoolExtension에 정의됨
        public static bool IsEqualThan(this object target1, bool target2)
        {
            return target2.IsEqualThan(target1);
        }

        public static bool IsNotEqualThan(this object target1, bool target2)
        {
            return target2.IsNotEqualThan(target1);
        }

        public static bool IsLessEqualThan(this object target1, bool target2)
        {
            return target2.IsGreaterEqualThan(target1);
        }

        public static bool IsGreaterEqualThan(this object target1, bool target2)
        {
            return target2.IsLessEqualThan(target1);
        }

        public static bool IsLessThan(this object target1, bool target2)
        {
            return target2.IsGreaterThan(target1);
        }

        public static bool IsGreaterThan(this object target1, bool target2)
        {
            return target2.IsLessThan(target1);
        }
        #endregion

        #region object, number 비교
        // 이미 NumberExtension에 정의됨
        public static bool IsEqualThan(this object target1, double target2)
        {
            return target2.IsEqualThan(target1);
        }

        public static bool IsNotEqualThan(this object target1, double target2)
        {
            return target2.IsNotEqualThan(target1);
        }

        public static bool IsLessEqualThan(this object target1, double target2)
        {
            return target2.IsGreaterEqualThan(target1);
        }

        public static bool IsGreaterEqualThan(this object target1, double target2)
        {
            return target2.IsLessEqualThan(target1);
        }

        public static bool IsLessThan(this object target1, double target2)
        {
            return target2.IsGreaterThan(target1);
        }

        public static bool IsGreaterThan(this object target1, double target2)
        {
            return target2.IsLessThan(target1);
        }
        #endregion

        #region object, list 비교
        // 이미 ListExtension에 정의됨
        public static bool IsEqualThan(this object target1, List<object> target2)
        {
            return target2.IsEqualThan(target1);
        }

        public static bool IsNotEqualThan(this object target1, List<object> target2)
        {
            return target2.IsNotEqualThan(target1);
        }

        public static bool IsLessEqualThan(this object target1, List<object> target2)
        {
            return target2.IsGreaterEqualThan(target1);
        }

        public static bool IsGreaterEqualThan(this object target1, List<object> target2)
        {
            return target2.IsLessEqualThan(target1);
        }

        public static bool IsLessThan(this object target1, List<object> target2)
        {
            return target2.IsGreaterThan(target1);
        }

        public static bool IsGreaterThan(this object target1, List<object> target2)
        {
            return target2.IsLessThan(target1);
        }
        #endregion

        #region object, object 비교
        public static bool IsEqualThan(this object target1, object target2)
        {
            if (NumberExtension.NumberTypes.Contains(target2.GetType()))
            {
                return target1.IsEqualThan(Convert.ToDouble(target2));
            }
            else if (target2 is string)
            {
                return target1.IsEqualThan(Convert.ToString(target2));
            }
            else if (target2 is bool)
            {
                return target1.IsEqualThan(Convert.ToBoolean(target2));
            }
            else if (target2 is List<object>)
            {
                return target1.IsEqualThan(target2 as List<object>);
            }
            else
            {
                return target1.Equals(target2);
            }
        }

        public static bool IsNotEqualThan(this object target1, object target2)
        {
            if (NumberExtension.NumberTypes.Contains(target2.GetType()))
            {
                return target1.IsNotEqualThan(Convert.ToDouble(target2));
            }
            else if (target2 is string)
            {
                return target1.IsNotEqualThan(Convert.ToString(target2));
            }
            else if (target2 is bool)
            {
                return target1.IsNotEqualThan(Convert.ToBoolean(target2));
            }
            else if (target2 is List<object>)
            {
                return target1.IsNotEqualThan(target2 as List<object>);
            }
            else
            {
                return !target1.Equals(target2);
            }
        }

        public static bool IsLessEqualThan(this object target1, object target2)
        {
            if (NumberExtension.NumberTypes.Contains(target2.GetType()))
            {
                return target1.IsLessEqualThan(Convert.ToDouble(target2));
            }
            else if (target2 is string)
            {
                return target1.IsLessEqualThan(Convert.ToString(target2));
            }
            else if (target2 is bool)
            {
                return target1.IsLessEqualThan(Convert.ToBoolean(target2));
            }
            else if (target2 is List<object>)
            {
                return target1.IsLessEqualThan(target2 as List<object>);
            }
            else
            {
                return target1.IsEqualThan(target2);
            }
        }

        public static bool IsGreaterEqualThan(this object target1, object target2)
        {
            if (NumberExtension.NumberTypes.Contains(target2.GetType()))
            {
                return target1.IsGreaterEqualThan(Convert.ToDouble(target2));
            }
            else if (target2 is string)
            {
                return target1.IsGreaterEqualThan(Convert.ToString(target2));
            }
            else if (target2 is bool)
            {
                return target1.IsGreaterEqualThan(Convert.ToBoolean(target2));
            }
            else if (target2 is List<object>)
            {
                return target1.IsGreaterEqualThan(target2 as List<object>);
            }
            else
            {
                return target1.IsEqualThan(target2);
            }
        }

        public static bool IsLessThan(this object target1, object target2)
        {
            if (NumberExtension.NumberTypes.Contains(target2.GetType()))
            {
                return target1.IsLessThan(Convert.ToDouble(target2));
            }
            else if (target2 is string)
            {
                return target1.IsLessThan(Convert.ToString(target2));
            }
            else if (target2 is bool)
            {
                return target1.IsLessThan(Convert.ToBoolean(target2));
            }
            else if (target2 is List<object>)
            {
                return target1.IsLessThan(target2 as List<object>);
            }
            else
            {
                return target1.IsNotEqualThan(target2);
            }
        }

        public static bool IsGreaterThan(this object target1, object target2)
        {
            if (NumberExtension.NumberTypes.Contains(target2.GetType()))
            {
                return target1.IsGreaterThan(Convert.ToDouble(target2));
            }
            else if (target2 is string)
            {
                return target1.IsGreaterThan(Convert.ToString(target2));
            }
            else if (target2 is bool)
            {
                return target1.IsGreaterThan(Convert.ToBoolean(target2));
            }
            else if (target2 is List<object>)
            {
                return target1.IsGreaterThan(target2 as List<object>);
            }
            else
            {
                return target1.IsNotEqualThan(target2);
            }
        }
        #endregion
    }
}
