using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Extension.DataTypes
{
    public class GLogic : GObject
    {
        public override Type ObjectType { get; } = typeof(bool);

        public bool Logic { get; set; }

        public GLogic(bool logic = true)
        {
            Logic = logic;
        }

        public override GCustom ToGCustom()
        {
            return new GCustom(null);
        }

        public override GLogic ToGLogic()
        {
            return this;
        }

        public override GNumber ToGNumber()
        {
            if (Logic == true)
            {
                return new GNumber(1);
            }
            
            return new GNumber(0);
        }

        public override GString ToGString()
        {
            if (Logic == true)
            {
                return new GString("참");
            }

            return new GString("거짓");
        }

        public static explicit operator GLogic(bool logic)
        {
            return new GLogic(logic);
        }

        public static implicit operator bool(GLogic logic)
        {
            return logic.Logic;
        }

        public static GLogic operator !(GLogic logic)
        {
            return new GLogic(!logic.Logic);
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

        #region GLogic과 GLogic 비교 연산자 오버로딩
        public static GLogic operator ==(GLogic target1, GLogic target2)
        {
            return new GLogic(target1.Logic == target2.Logic);
        }

        public static GLogic operator !=(GLogic target1, GLogic target2)
        {
            return new GLogic(target1.Logic != target2.Logic);
        }

        public static GLogic operator <=(GLogic target1, GLogic target2)
        {
            return target1 == target2;
        }

        public static GLogic operator >=(GLogic target1, GLogic target2)
        {
            return target1 == target2;
        }

        public static GLogic operator <(GLogic target1, GLogic target2)
        {
            return target1 != target2;
        }

        public static GLogic operator >(GLogic target1, GLogic target2)
        {
            return target1 != target2;
        }
        #endregion

        #region GLogic과 GString 비교 연산자 오버로딩
        public static GLogic operator ==(GLogic target1, GString target2)
        {
            return new GLogic(target1.ToGNumber() == target2);
        }

        public static GLogic operator !=(GLogic target1, GString target2)
        {
            return new GLogic(target1.ToGNumber() != target2);
        }

        public static GLogic operator <=(GLogic target1, GString target2)
        {
            return target1 == target2;
        }

        public static GLogic operator >=(GLogic target1, GString target2)
        {
            return target1 == target2;
        }

        public static GLogic operator <(GLogic target1, GString target2)
        {
            return target1 != target2;
        }

        public static GLogic operator >(GLogic target1, GString target2)
        {
            return target1 != target2;
        }
        #endregion

        #region GLogic과 GNumber 비교 연산자 오버로딩
        public static GLogic operator ==(GLogic target1, GNumber target2)
        {
            return new GLogic(target1.Logic == target2.ToGLogic().Logic);
        }

        public static GLogic operator !=(GLogic target1, GNumber target2)
        {
            return new GLogic(target1.Logic != target2.ToGLogic().Logic);
        }

        public static GLogic operator <=(GLogic target1, GNumber target2)
        {
            return target1 == target2;
        }

        public static GLogic operator >=(GLogic target1, GNumber target2)
        {
            return target1 == target2;
        }

        public static GLogic operator <(GLogic target1, GNumber target2)
        {
            return target1 != target2;
        }

        public static GLogic operator >(GLogic target1, GNumber target2)
        {
            return target1 != target2;
        }
        #endregion

        #region GLogic과 GCustom과 비교 연산자 오버로딩
        public static GLogic operator ==(GLogic target1, GCustom target2)
        {
            return new GLogic(target1.Logic == target2.ToGLogic().Logic);
        }

        public static GLogic operator !=(GLogic target1, GCustom target2)
        {
            return new GLogic(target1.Logic != target2.ToGLogic().Logic);
        }

        public static GLogic operator <=(GLogic target1, GCustom target2)
        {
            return target1 == target2;
        }

        public static GLogic operator >=(GLogic target1, GCustom target2)
        {
            return target1 == target2;
        }

        public static GLogic operator <(GLogic target1, GCustom target2)
        {
            return target1 != target2;
        }

        public static GLogic operator >(GLogic target1, GCustom target2)
        {
            return target1 != target2;
        }

        #endregion
        
        #region GLogic와 GList 비교 연산자 오버로딩
        // 이미 GList에 정의됨, 역방향 오버로딩만 정의
        public static GLogic operator ==(GLogic target1, GList target2)
        {
            return target2 == target1;
        }

        public static GLogic operator !=(GLogic target1, GList target2)
        {
            return target2 != target1;
        }

        public static GLogic operator <=(GLogic target1, GList target2)
        {
            return target2 <= target1;
        }

        public static GLogic operator >=(GLogic target1, GList target2)
        {
            return target2 >= target1;
        }

        public static GLogic operator <(GLogic target1, GList target2)
        {
            return target2 < target1;
        }

        public static GLogic operator >(GLogic target1, GList target2)
        {
            return target2 > target1;
        }
        #endregion
    }
}