using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Extension.DataTypes
{
    public class GString : GObject
    {
        public override Type ObjectType { get; } = typeof(string);

        public string String { get; set; }

        public GString(string str = "")
        {
            String = str;
        }

        public override GCustom ToGCustom()
        {
            return new GCustom(null);
        }

        public override GLogic ToGLogic()
        {
            if (String == "" || String == null)
            {
                return new GLogic(false);
            }

            return new GLogic(true);
        }

        public override GNumber ToGNumber()
        {
            double result;
            if (double.TryParse(String, out result))
            {
                return new GNumber(result);
            }

            return new GNumber(0);
        }

        public override GString ToGString()
        {
            return this;
        }

        public static explicit operator GString(string str)
        {
            return new GString(str);
        }

        public static implicit operator string(GString str)
        {
            return str.String;
        }

        #region 비교 메소드 오버라이딩
        public override GLogic EqualOperator(GObject target)
        {
            if (target is GLogic)
            {
                return this == (target as GLogic);
            }
            else if (target is GNumber)
            {
                return this == (target as GNumber);
            }
            else if (target is GString)
            {
                return this == (target as GString);
            }
            else if (target is GCustom)
            {
                return this == (target as GCustom);
            }
            else if (target is GList)
            {
                return this == (target as GList);
            }

            return new GLogic(false);
        }

        public override GLogic NotEqualOperator(GObject target)
        {
            if (target is GLogic)
            {
                return this != (target as GLogic);
            }
            else if (target is GNumber)
            {
                return this != (target as GNumber);
            }
            else if (target is GString)
            {
                return this != (target as GString);
            }
            else if (target is GCustom)
            {
                return this != (target as GCustom);
            }
            else if (target is GList)
            {
                return this != (target as GList);
            }

            return new GLogic(false);
        }

        public override GLogic LessEqualOperator(GObject target)
        {
            if (target is GLogic)
            {
                return this <= (target as GLogic);
            }
            else if (target is GNumber)
            {
                return this <= (target as GNumber);
            }
            else if (target is GString)
            {
                return this <= (target as GString);
            }
            else if (target is GCustom)
            {
                return this <= (target as GCustom);
            }
            else if (target is GList)
            {
                return this <= (target as GList);
            }

            return new GLogic(false);
        }

        public override GLogic MoreEqualOperator(GObject target)
        {
            if (target is GLogic)
            {
                return this >= (target as GLogic);
            }
            else if (target is GNumber)
            {
                return this >= (target as GNumber);
            }
            else if (target is GString)
            {
                return this >= (target as GString);
            }
            else if (target is GCustom)
            {
                return this >= (target as GCustom);
            }
            else if (target is GList)
            {
                return this >= (target as GList);
            }

            return new GLogic(false);
        }

        public override GLogic LessOperator(GObject target)
        {
            if (target is GLogic)
            {
                return this < (target as GLogic);
            }
            else if (target is GNumber)
            {
                return this < (target as GNumber);
            }
            else if (target is GString)
            {
                return this < (target as GString);
            }
            else if (target is GCustom)
            {
                return this < (target as GCustom);
            }
            else if (target is GList)
            {
                return this < (target as GList);
            }

            return new GLogic(false);
        }

        public override GLogic MoreOperator(GObject target)
        {
            if (target is GLogic)
            {
                return this > (target as GLogic);
            }
            else if (target is GNumber)
            {
                return this > (target as GNumber);
            }
            else if (target is GString)
            {
                return this > (target as GString);
            }
            else if (target is GCustom)
            {
                return this > (target as GCustom);
            }
            else if (target is GList)
            {
                return this > (target as GList);
            }

            return new GLogic(false);
        }
        #endregion

        #region GString과 GString 비교 연산자 오버로딩
        public static GLogic operator ==(GString target1, GString target2)
        {
            return new GLogic(string.Compare(target1.String, target2.String) == 0);
        }

        public static GLogic operator !=(GString target1, GString target2)
        {
            return new GLogic(string.Compare(target1.String, target2.String) != 0);
        }

        public static GLogic operator <=(GString target1, GString target2)
        {
            return new GLogic(string.Compare(target1.String, target2.String) <= 0);
        }

        public static GLogic operator >=(GString target1, GString target2)
        {
            return new GLogic(string.Compare(target1.String, target2.String) >= 0);
        }

        public static GLogic operator <(GString target1, GString target2)
        {
            return new GLogic(string.Compare(target1.String, target2.String) < 0);
        }

        public static GLogic operator >(GString target1, GString target2)
        {
            return new GLogic(string.Compare(target1.String, target2.String) > 0);
        }
        #endregion

        #region GString과 GLogic 비교 연산자 오버로딩
        // 이미 GLogic에 정의되어 역방향만 오버로딩
        public static GLogic operator ==(GString target1, GLogic target2)
        {
            return target2 == target1;
        }

        public static GLogic operator !=(GString target1, GLogic target2)
        {
            return target2 != target1;
        }

        public static GLogic operator <=(GString target1, GLogic target2)
        {
            return target2 <= target1;
        }

        public static GLogic operator >=(GString target1, GLogic target2)
        {
            return target2 >= target1;
        }

        public static GLogic operator <(GString target1, GLogic target2)
        {
            return target2 < target1;
        }

        public static GLogic operator >(GString target1, GLogic target2)
        {
            return target2 > target1;
        }
        #endregion

        #region GString과 GNumber 비교 연산자 오버로딩
        public static GLogic operator ==(GString target1, GNumber target2)
        {
            return target1 == target2.ToGString();
        }

        public static GLogic operator !=(GString target1, GNumber target2)
        {
            return target1 != target2.ToGString();
        }

        public static GLogic operator <=(GString target1, GNumber target2)
        {
            return target1 <= target2.ToGString();
        }

        public static GLogic operator >=(GString target1, GNumber target2)
        {
            return target1 >= target2.ToGString();
        }

        public static GLogic operator <(GString target1, GNumber target2)
        {
            return target1 < target2.ToGString();
        }

        public static GLogic operator >(GString target1, GNumber target2)
        {
            return target1 > target2.ToGString();
        }
        #endregion

        #region GString과 GCustom 비교 연산자 오버로딩
        public static GLogic operator ==(GString target1, GCustom target2)
        {
            return new GLogic(false);
        }

        public static GLogic operator !=(GString target1, GCustom target2)
        {
            return new GLogic(true);
        }

        public static GLogic operator <=(GString target1, GCustom target2)
        {
            return new GLogic(false);
        }

        public static GLogic operator >=(GString target1, GCustom target2)
        {
            return new GLogic(false);
        }

        public static GLogic operator <(GString target1, GCustom target2)
        {
            return new GLogic(true);
        }

        public static GLogic operator >(GString target1, GCustom target2)
        {
            return new GLogic(true);
        }
        #endregion

        #region GString과 GList 비교 연산자 오버로딩
        // 이미 GList에 정의됨, 역방향 오버로딩만 정의
        public static GLogic operator ==(GString target1, GList target2)
        {
            return target2 == target1;
        }

        public static GLogic operator !=(GString target1, GList target2)
        {
            return target2 != target1;
        }

        public static GLogic operator <=(GString target1, GList target2)
        {
            return target2 <= target1;
        }

        public static GLogic operator >=(GString target1, GList target2)
        {
            return target2 >= target1;
        }

        public static GLogic operator <(GString target1, GList target2)
        {
            return target2 < target1;
        }

        public static GLogic operator >(GString target1, GList target2)
        {
            return target2 > target1;
        }
        #endregion
    }
}
