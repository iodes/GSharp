using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Extension.DataTypes
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

        public static bool IsNotEqualThan(this double target1, object target2)
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

        public static bool IsLessEqualThan(this double target1, object target2)
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

        public static bool IsGreaterEqualThan(this double target1, object target2)
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

        public static bool IsLessThan(this double target1, object target2)
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

        public static bool IsGreaterThan(this double target1, object target2)
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
