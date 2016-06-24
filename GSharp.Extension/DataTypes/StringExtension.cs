using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Extension.DataTypes
{
    public static class StringExtension
    {
        public static double ToNumber(this string str)
        {
            double result;
            if (double.TryParse(str, out result))
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

        #region string, string 비교
        public static bool IsEqualThan(this string target1, string target2)
        {
            return string.Compare(target1, target2) == 0;
        }

        public static bool IsNotEqualThan(this string target1, string target2)
        {
            return string.Compare(target1, target2) != 0;
        }

        public static bool IsLessEqualThan(this string target1, string target2)
        {
            return string.Compare(target1, target2) <= 0;
        }

        public static bool IsGreaterEqualThan(this string target1, string target2)
        {
            return string.Compare(target1, target2) >= 0;
        }

        public static bool IsLessThan(this string target1, string target2)
        {
            return string.Compare(target1, target2) < 0;
        }

        public static bool IsGreaterThan(this string target1, string target2)
        {
            return string.Compare(target1, target2) > 0;
        }
        #endregion

        #region string, bool 비교
        public static bool IsEqualThan(this string target1, bool target2)
        {
            return target1.ToNumber() == target2.ToNumber();
        }

        public static bool IsNotEqualThan(this string target1, bool target2)
        {
            return target1.ToNumber() != target2.ToNumber();
        }

        public static bool IsLessEqualThan(this string target1, bool target2)
        {
            return target1.IsEqualThan(target2);
        }

        public static bool IsGreaterEqualThan(this string target1, bool target2)
        {
            return target1.IsEqualThan(target2);
        }

        public static bool IsLessThan(this string target1, bool target2)
        {
            return target1.IsNotEqualThan(target2);
        }

        public static bool IsGreaterThan(this string target1, bool target2)
        {
            return target1.IsNotEqualThan(target2);
        }
        #endregion
        
        #region string, number 비교
        public static bool IsEqualThan(this string target1, double target2)
        {
            return target1.ToNumber() == target2;
        }

        public static bool IsNotEqualThan(this string target1, double target2)
        {
            return target1.ToNumber() != target2;
        }

        public static bool IsLessEqualThan(this string target1, double target2)
        {
            return target1.ToNumber() <= target2;
        }

        public static bool IsGreaterEqualThan(this string target1, double target2)
        {
            return target1.ToNumber() >= target2;
        }

        public static bool IsLessThan(this string target1, double target2)
        {
            return target1.ToNumber() < target2;
        }

        public static bool IsGreaterThan(this string target1, double target2)
        {
            return target1.ToNumber() > target2;
        }
        #endregion

        #region string, list 비교
        public static bool IsEqualThan(this string target1, List<object> target2)
        {
            return target1.IsEqualThan(target2.ToText());
        }

        public static bool IsNotEqualThan(this string target1, List<object> target2)
        {
            return target1.IsNotEqualThan(target2.ToText());
        }

        public static bool IsLessEqualThan(this string target1, List<object> target2)
        {
            return target1.IsLessEqualThan(target2.ToText());
        }

        public static bool IsGreaterEqualThan(this string target1, List<object> target2)
        {
            return target1.IsGreaterEqualThan(target2.ToText());
        }

        public static bool IsLessThan(this string target1, List<object> target2)
        {
            return target1.IsLessThan(target2.ToText());
        }

        public static bool IsGreaterThan(this string target1, List<object> target2)
        {
            return target1.IsGreaterThan(target2.ToText());
        }
        #endregion

        #region string, object 비교
        public static bool IsEqualThan(this string target1, object target2)
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
                return false;
            }
        }

        public static bool IsNotEqualThan(this string target1, object target2)
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
                return true;
            }
        }

        public static bool IsLessEqualThan(this string target1, object target2)
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
                return false;
            }
        }

        public static bool IsGreaterEqualThan(this string target1, object target2)
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
                return false;
            }
        }

        public static bool IsLessThan(this string target1, object target2)
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
                return true;
            }
        }

        public static bool IsGreaterThan(this string target1, object target2)
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
                return true;
            }
        }
        #endregion
    }
}
