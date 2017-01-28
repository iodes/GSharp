using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GSharp.Extension.DataTypes
{
    public static class NumberExtension
    {
        public static readonly List<Type> NumberTypes = new List<Type>()
        {
            typeof(sbyte),
            typeof(byte),
            typeof(short),
            typeof(ushort),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(float),
            typeof(double)
        };

        public static bool IsNumeric(object obj)
        {
            return NumberTypes.Contains(obj.GetType());
        }

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

        #region number, string 비교
        // 이미 StringExtension에서 정의함
        public static bool IsEqualThan(this double target1, string target2)
        {
            return target2.IsEqualThan(target1);
        }

        public static bool IsNotEqualThan(this double target1, string target2)
        {
            return target2.IsNotEqualThan(target1);
        }

        public static bool IsLessEqualThan(this double target1, string target2)
        {
            return target2.IsLessEqualThan(target1);
        }

        public static bool IsGreaterEqualThan(this double target1, string target2)
        {
            return target2.IsGreaterEqualThan(target1);
        }

        public static bool IsLessThan(this double target1, string target2)
        {
            return target2.IsLessThan(target1);
        }

        public static bool IsGreaterThan(this double target1, string target2)
        {
            return target2.IsGreaterThan(target1);
        }
        #endregion

        #region number, bool 비교
        public static bool IsEqualThan(this double target1, bool target2)
        {
            return target1.ToBool() == target2;
        }

        public static bool IsNotEqualThan(this double target1, bool target2)
        {
            return target1.ToBool() != target2;
        }

        public static bool IsLessEqualThan(this double target1, bool target2)
        {
            return target1.IsEqualThan(target2);
        }

        public static bool IsGreaterEqualThan(this double target1, bool target2)
        {
            return target1.IsEqualThan(target2);
        }

        public static bool IsLessThan(this double target1, bool target2)
        {
            return target1.IsNotEqualThan(target2);
        }

        public static bool IsGreaterThan(this double target1, bool target2)
        {
            return target1.IsNotEqualThan(target2);
        }
        #endregion

        #region number, number 비교
        public static bool IsEqualThan(this double target1, double target2)
        {
            return target1 == target2;
        }

        public static bool IsNotEqualThan(this double target1, double target2)
        {
            return target1 != target2;
        }

        public static bool IsLessEqualThan(this double target1, double target2)
        {
            return target1 <= target2;
        }

        public static bool IsGreaterEqualThan(this double target1, double target2)
        {
            return target1 >= target2;
        }

        public static bool IsLessThan(this double target1, double target2)
        {
            return target1 < target2;
        }

        public static bool IsGreaterThan(this double target1, double target2)
        {
            return target1 > target2;
        }
        #endregion

        #region number, list 비교
        public static bool IsEqualThan(this double target1, List<object> target2)
        {
            return target1 == target2.Count;
        }

        public static bool IsNotEqualThan(this double target1, List<object> target2)
        {
            return target1 != target2.Count;
        }

        public static bool IsLessEqualThan(this double target1, List<object> target2)
        {
            return target1 <= target2.Count;
        }

        public static bool IsGreaterEqualThan(this double target1, List<object> target2)
        {
            return target1 >= target2.Count;
        }

        public static bool IsLessThan(this double target1, List<object> target2)
        {
            return target1 < target2.Count;
        }

        public static bool IsGreaterThan(this double target1, List<object> target2)
        {
            return target1 > target2.Count;
        }
        #endregion

        #region number, object 비교
        public static bool IsEqualThan(this double target1, object target2)
        {
            if (IsNumeric(target2))
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
                return false;
            }
        }

        public static bool IsNotEqualThan(this double target1, object target2)
        {
            if (IsNumeric(target2))
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
                return true;
            }
        }

        public static bool IsLessEqualThan(this double target1, object target2)
        {
            if (IsNumeric(target2))
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
                return false;
            }
        }

        public static bool IsGreaterEqualThan(this double target1, object target2)
        {
            if (IsNumeric(target2))
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
                return false;
            }
        }

        public static bool IsLessThan(this double target1, object target2)
        {
            if (IsNumeric(target2))
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
                return true;
            }
        }

        public static bool IsGreaterThan(this double target1, object target2)
        {
            if (IsNumeric(target2))
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
                return true;
            }
        }
        #endregion
    }
}
