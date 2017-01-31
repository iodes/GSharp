using System;
using System.Collections.Generic;
using System.Linq;

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

        public static bool IsNotEqualThan(this bool target1, object target2)
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

        public static bool IsLessEqualThan(this bool target1, object target2)
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

        public static bool IsGreaterEqualThan(this bool target1, object target2)
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

        public static bool IsLessThan(this bool target1, object target2)
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

        public static bool IsGreaterThan(this bool target1, object target2)
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
