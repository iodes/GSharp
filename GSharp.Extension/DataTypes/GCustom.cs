using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Extension.DataTypes
{
    public class GCustom : GObject
    {
        public override Type ObjectType
        {
            get
            {
                return _ObjectType;
            }
        }
        private Type _ObjectType;

        public object Custom {
            get
            {
                return _Custom;
            }
            set
            {
                if (value == null)
                {
                    _Custom = null;
                    _ObjectType = typeof(void);
                }
                else
                {
                    _Custom = value;
                    _ObjectType = value.GetType();
                }
            }
        }
        private object _Custom;

        public GCustom(object custom = null)
        {
            Custom = custom;
        }

        public override GCustom ToGCustom()
        {
            return this;
        }

        public override GLogic ToGLogic()
        {
            if (Custom == null)
            {
                return new GLogic(false);
            }

            return new GLogic(true);
        }

        public override GNumber ToGNumber()
        {
            return new GNumber();
        }

        public override GString ToGString()
        {
            return new GString();
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

        #region GCustom과 GCustom 비교 연산자 오버로딩
        public static GLogic operator ==(GCustom target1, GCustom target2)
        {
            return new GLogic(target1.Custom.Equals(target2.Custom));
        }

        public static GLogic operator !=(GCustom target1, GCustom target2)
        {
            return new GLogic(!target1.Custom.Equals(target2.Custom));
        }

        public static GLogic operator <=(GCustom target1, GCustom target2)
        {
            return target1 == target2;
        }

        public static GLogic operator >=(GCustom target1, GCustom target2)
        {
            return target1 == target2;
        }

        public static GLogic operator <(GCustom target1, GCustom target2)
        {
            return target1 != target2;
        }

        public static GLogic operator >(GCustom target1, GCustom target2)
        {
            return target1 != target2;
        }
        #endregion

        #region GCustom과 GString 비교 연산자 오버로딩
        // GString에 이미 정의됨, 역방향 오버로딩만 정의
        public static GLogic operator ==(GCustom target1, GString target2)
        {
            return target2 == target1;
        }

        public static GLogic operator !=(GCustom target1, GString target2)
        {
            return target2 != target1;
        }

        public static GLogic operator <=(GCustom target1, GString target2)
        {
            return target2 <= target1;
        }

        public static GLogic operator >=(GCustom target1, GString target2)
        {
            return target2 >= target1;
        }

        public static GLogic operator <(GCustom target1, GString target2)
        {
            return target2 < target1;
        }

        public static GLogic operator >(GCustom target1, GString target2)
        {
            return target2 > target1;
        }
        #endregion

        #region GCustom과 GLogic 비교 연산자 오버로딩
        // 이미 GLogic에 정의되어 역방향만 오버로딩
        public static GLogic operator ==(GCustom target1, GLogic target2)
        {
            return target2 == target1;
        }

        public static GLogic operator !=(GCustom target1, GLogic target2)
        {
            return target2 != target1;
        }

        public static GLogic operator <=(GCustom target1, GLogic target2)
        {
            return target2 <= target1;
        }

        public static GLogic operator >=(GCustom target1, GLogic target2)
        {
            return target2 >= target1;
        }

        public static GLogic operator <(GCustom target1, GLogic target2)
        {
            return target2 < target1;
        }

        public static GLogic operator >(GCustom target1, GLogic target2)
        {
            return target2 > target1;
        }
        #endregion

        #region GCustom과 GNumber 비교 연산자 오버로딩
        // 이미 GNumber에 정의되어 역방향만 오버로딩
        public static GLogic operator ==(GCustom target1, GNumber target2)
        {
            return target1 == target2;
        }

        public static GLogic operator !=(GCustom target1, GNumber target2)
        {
            return target1 != target2;
        }

        public static GLogic operator <=(GCustom target1, GNumber target2)
        {
            return target1 <= target2;
        }

        public static GLogic operator >=(GCustom target1, GNumber target2)
        {
            return target1 >= target2;
        }

        public static GLogic operator <(GCustom target1, GNumber target2)
        {
            return target1 < target2;
        }

        public static GLogic operator >(GCustom target1, GNumber target2)
        {
            return target1 > target2;
        }
        #endregion

        #region GCustom과 GList 비교 연산자 오버로딩
        // GList에 이미 정의됨, 역방향 오버로딩만 구현
        public static GLogic operator ==(GCustom target1, GList target2)
        {
            return target2 == target1;
        }

        public static GLogic operator !=(GCustom target1, GList target2)
        {
            return target2 != target1;
        }

        public static GLogic operator <=(GCustom target1, GList target2)
        {
            return target2 <= target1;
        }

        public static GLogic operator >=(GCustom target1, GList target2)
        {
            return target2 >= target1;
        }

        public static GLogic operator <(GCustom target1, GList target2)
        {
            return target2 < target1;
        }

        public static GLogic operator >(GCustom target1, GList target2)
        {
            return target2 > target1;
        }
        #endregion
    }
}
