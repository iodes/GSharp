using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Extension.DataTypes
{
    public static class ObjectExtension
    {
        public static double ToNumber(this object obj)
        {
            return 0;
        }

        public static string ToText(this object obj)
        {
            return "";
        }

        public static bool ToBool(this object obj)
        {
            return true;
        }

        public static object ToCustom(this object obj)
        {
            return obj;
        }

        public static List<object> ToList(this object obj)
        {
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
            return target2.IsLessEqualThan(target1);
        }

        public static bool IsGreaterEqualThan(this object target1, string target2)
        {
            return target2.IsGreaterEqualThan(target1);
        }

        public static bool IsLessThan(this object target1, string target2)
        {
            return target2.IsLessThan(target1);
        }

        public static bool IsGreaterThan(this object target1, string target2)
        {
            return target2.IsGreaterThan(target1);
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
            return target2.IsLessEqualThan(target1);
        }

        public static bool IsGreaterEqualThan(this object target1, bool target2)
        {
            return target2.IsGreaterEqualThan(target1);
        }

        public static bool IsLessThan(this object target1, bool target2)
        {
            return target2.IsLessThan(target1);
        }

        public static bool IsGreaterThan(this object target1, bool target2)
        {
            return target2.IsGreaterThan(target1);
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
            return target2.IsLessEqualThan(target1);
        }

        public static bool IsGreaterEqualThan(this object target1, double target2)
        {
            return target2.IsGreaterEqualThan(target1);
        }

        public static bool IsLessThan(this object target1, double target2)
        {
            return target2.IsLessThan(target1);
        }

        public static bool IsGreaterThan(this object target1, double target2)
        {
            return target2.IsGreaterThan(target1);
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
            return target2.IsLessEqualThan(target1);
        }

        public static bool IsGreaterEqualThan(this object target1, List<object> target2)
        {
            return target2.IsGreaterEqualThan(target1);
        }

        public static bool IsLessThan(this object target1, List<object> target2)
        {
            return target2.IsLessThan(target1);
        }

        public static bool IsGreaterThan(this object target1, List<object> target2)
        {
            return target2.IsGreaterThan(target1);
        }
        #endregion

        #region object, object 비교
        public static bool IsEqualThan(this object target1, object target2)
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
                return target1 == target2;
            }
        }

        public static bool IsNotEqualThan(this object target1, object target2)
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
                return target1 != target2;
            }
        }

        public static bool IsLessEqualThan(this object target1, object target2)
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
                return target1 == target2;
            }
        }

        public static bool IsGreaterEqualThan(this object target1, object target2)
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
                return target1 == target2;
            }
        }

        public static bool IsLessThan(this object target1, object target2)
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
                return target1 != target2;
            }
        }

        public static bool IsGreaterThan(this object target1, object target2)
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
                return target1 != target2;
            }
        }
        #endregion
    }
}
