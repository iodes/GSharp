using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Extension.DataTypes
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


        #region bool, string 비교
        // 이미 StringExtension에서 정의함
        public static bool IsEqualThan(this bool target1, string target2)
        {
            return target2.IsEqualThan(target1);
        }

        public static bool IsNotEqualThan(this bool target1, string target2)
        {
            return target2.IsNotEqualThan(target1);
        }

        public static bool IsLessEqualThan(this bool target1, string target2)
        {
            return target2.IsLessEqualThan(target1);
        }

        public static bool IsGreaterEqualThan(this bool target1, string target2)
        {
            return target2.IsGreaterEqualThan(target1);
        }

        public static bool IsLessThan(this bool target1, string target2)
        {
            return target2.IsLessThan(target1);
        }

        public static bool IsGreaterThan(this bool target1, string target2)
        {
            return target2.IsGreaterThan(target1);
        }
        #endregion

        #region bool, bool 비교
        public static bool IsEqualThan(this bool target1, bool target2)
        {
            return target1 == target2;
        }

        public static bool IsNotEqualThan(this bool target1, bool target2)
        {
            return target1 != target2;
        }

        public static bool IsLessEqualThan(this bool target1, bool target2)
        {
            return target1.IsEqualThan(target2);
        }

        public static bool IsGreaterEqualThan(this bool target1, bool target2)
        {
            return target1.IsEqualThan(target2);
        }

        public static bool IsLessThan(this bool target1, bool target2)
        {
            return target1.IsNotEqualThan(target2);
        }

        public static bool IsGreaterThan(this bool target1, bool target2)
        {
            return target1.IsNotEqualThan(target2);
        }
        #endregion

        #region bool, number 비교
        // 이미 NumberExtension에서 정의함
        public static bool IsEqualThan(this bool target1, double target2)
        {
            return target2.IsEqualThan(target1);
        }

        public static bool IsNotEqualThan(this bool target1, double target2)
        {
            return target2.IsNotEqualThan(target1);
        }

        public static bool IsLessEqualThan(this bool target1, double target2)
        {
            return target2.IsLessEqualThan(target1);
        }

        public static bool IsGreaterEqualThan(this bool target1, double target2)
        {
            return target2.IsGreaterEqualThan(target1);
        }

        public static bool IsLessThan(this bool target1, double target2)
        {
            return target2.IsLessThan(target1);
        }

        public static bool IsGreaterThan(this bool target1, double target2)
        {
            return target2.IsGreaterThan(target1);
        }
        #endregion

        #region bool, list 비교
        public static bool IsEqualThan(this bool target1, List<object> target2)
        {
            return target1 == target2.ToBool();
        }

        public static bool IsNotEqualThan(this bool target1, List<object> target2)
        {
            return target1 == target2.ToBool();
        }

        public static bool IsLessEqualThan(this bool target1, List<object> target2)
        {
            return target1.IsEqualThan(target2);
        }

        public static bool IsGreaterEqualThan(this bool target1, List<object> target2)
        {
            return target1.IsEqualThan(target2);
        }

        public static bool IsLessThan(this bool target1, List<object> target2)
        {
            return target1.IsNotEqualThan(target2);
        }

        public static bool IsGreaterThan(this bool target1, List<object> target2)
        {
            return target1.IsNotEqualThan(target2);
        }
        #endregion

        #region bool, object 비교
        public static bool IsEqualThan(this bool target1, object target2)
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

        public static bool IsNotEqualThan(this bool target1, object target2)
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

        public static bool IsLessEqualThan(this bool target1, object target2)
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

        public static bool IsGreaterEqualThan(this bool target1, object target2)
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

        public static bool IsLessThan(this bool target1, object target2)
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

        public static bool IsGreaterThan(this bool target1, object target2)
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
