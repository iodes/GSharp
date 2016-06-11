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
            if (target2 is double)
            {
                return target1.IsEqualThan((double)target2);
            }
            else if (target2 is string)
            {
                return target1.IsEqualThan((string)target2);
            }
            else if (target2 is bool)
            {
                return target1.IsEqualThan((bool)target2);
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
            if (target2 is double)
            {
                return target1.IsNotEqualThan((double)target2);
            }
            else if (target2 is string)
            {
                return target1.IsNotEqualThan((string)target2);
            }
            else if (target2 is bool)
            {
                return target1.IsNotEqualThan((bool)target2);
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
            if (target2 is double)
            {
                return target1.IsLessEqualThan((double)target2);
            }
            else if (target2 is string)
            {
                return target1.IsLessEqualThan((string)target2);
            }
            else if (target2 is bool)
            {
                return target1.IsLessEqualThan((bool)target2);
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
            if (target2 is double)
            {
                return target1.IsGreaterEqualThan((double)target2);
            }
            else if (target2 is string)
            {
                return target1.IsGreaterEqualThan((string)target2);
            }
            else if (target2 is bool)
            {
                return target1.IsGreaterEqualThan((bool)target2);
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
            if (target2 is double)
            {
                return target1.IsLessThan((double)target2);
            }
            else if (target2 is string)
            {
                return target1.IsLessThan((string)target2);
            }
            else if (target2 is bool)
            {
                return target1.IsLessThan((bool)target2);
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
            if (target2 is double)
            {
                return target1.IsGreaterThan((double)target2);
            }
            else if (target2 is string)
            {
                return target1.IsGreaterThan((string)target2);
            }
            else if (target2 is bool)
            {
                return target1.IsGreaterThan((bool)target2);
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
