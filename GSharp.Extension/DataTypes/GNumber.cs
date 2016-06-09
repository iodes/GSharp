using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Extension.DataTypes
{
    public class GNumber : GObject
    {
        public static Type[] numberTypes = {
            typeof(sbyte), // SByte
            typeof(byte), // Byte
            typeof(short), // Int16
            typeof(ushort), // UInt16
            typeof(int), // Int32
            typeof(uint), // UInt32
            typeof(long), // Int64
            typeof(ulong), // UInt64
            typeof(float), // Single
            typeof(double), // Double
        };

        public override Type ObjectType { get; } = typeof(double);

        public double Number { get; set; }

        public GNumber(double number = 0)
        {
            Number = number;
        }

        public override GCustom ToGCustom()
        {
            return new GCustom(null);
        }

        public override GLogic ToGLogic()
        {
            if (Number == 0)
            {
                return new GLogic(false);
            }

            return new GLogic(true);
        }

        public override GNumber ToGNumber()
        {
            return this;
        }

        public override GString ToGString()
        {
            return new GString(Number.ToString());
        }

        public static GNumber operator +(GNumber number1, GNumber number2)
        {
            return new GNumber(number1.Number + number2.Number);
        }

        public static GNumber operator -(GNumber number1, GNumber number2)
        {
            return new GNumber(number1.Number - number2.Number);
        }

        public static GNumber operator *(GNumber number1, GNumber number2)
        {
            return new GNumber(number1.Number * number2.Number);
        }

        public static GNumber operator /(GNumber number1, GNumber number2)
        {
            return new GNumber(number1.Number / number2.Number);
        }

        public static GNumber operator %(GNumber number1, GNumber number2)
        {
            return new GNumber(number1.Number % number2.Number);
        }

        public static GNumber operator ++(GNumber target)
        {
            target.Number++;
            return target;
        }

        public static GNumber operator --(GNumber target)
        {
            target.Number--;
            return target;
        }

        #region 기존 자료형에서 GNumber로 변환
        // double to GNumber
        // sbyte, byte, short, ushort, int, uint, float에서 double로 자동변환됨
        public static explicit operator GNumber(double number)
        {
            return new GNumber(number);
        }
        #endregion

        #region GNumber에서 기존 자료형으로 변환
        // GNumber to sbyte
        public static implicit operator sbyte(GNumber number)
        {
            return (sbyte)number.Number;
        }

        // GNumber to byte
        public static implicit operator byte(GNumber number)
        {
            return (byte)number.Number;
        }

        // GNumber to short
        public static implicit operator short(GNumber number)
        {
            return (short)number.Number;
        }

        // GNumber to ushort
        public static implicit operator ushort(GNumber number)
        {
            return (ushort)number.Number;
        }

        // GNumber to int
        public static implicit operator int(GNumber number)
        {
            return (int)number.Number;
        }

        // GNumber to uint
        public static implicit operator uint(GNumber number)
        {
            return (uint)number.Number;
        }

        // GNumber to float
        public static implicit operator float(GNumber number)
        {
            return (float)number.Number;
        }

        // GNumber to double
        public static implicit operator double(GNumber number)
        {
            return number.Number;
        }
        #endregion

        // 비교 연산자 오버로딩
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

        #region GNumber와 GNumber 비교 연산자 오버로딩
        public static GLogic operator ==(GNumber target1, GNumber target2)
        {
            return new GLogic(target1.Number == target2.Number);
        }

        public static GLogic operator !=(GNumber target1, GNumber target2)
        {
            return new GLogic(target1.Number != target2.Number);
        }

        public static GLogic operator <=(GNumber target1, GNumber target2)
        {
            return new GLogic(target1.Number <= target2.Number);
        }

        public static GLogic operator >=(GNumber target1, GNumber target2)
        {
            return new GLogic(target1.Number >= target2.Number);
        }

        public static GLogic operator <(GNumber target1, GNumber target2)
        {
            return new GLogic(target1.Number < target2.Number);
        }

        public static GLogic operator >(GNumber target1, GNumber target2)
        {
            return new GLogic(target1.Number > target2.Number);
        }
        #endregion

        #region GNumber와 GString 비교 연산자 오버로딩
        // 이미 GString에서 정의됨, 역방향 오버로딩만 정의
        public static GLogic operator ==(GNumber target1, GString target2)
        {
            return target2 == target1;
        }

        public static GLogic operator !=(GNumber target1, GString target2)
        {
            return target2 != target1;
        }

        public static GLogic operator <=(GNumber target1, GString target2)
        {
            return target2 <= target1;
        }

        public static GLogic operator >=(GNumber target1, GString target2)
        {
            return target2 >= target1;
        }

        public static GLogic operator <(GNumber target1, GString target2)
        {
            return target2 < target1;
        }

        public static GLogic operator >(GNumber target1, GString target2)
        {
            return target2 > target1;
        }
        #endregion

        #region GNumber와 GLogic 비교 연산자 오버로딩
        // GLogic에 이미 정의됨, 역방향 오버로딩만 정의
        public static GLogic operator ==(GNumber target1, GLogic target2)
        {
            return target2 == target1;
        }

        public static GLogic operator !=(GNumber target1, GLogic target2)
        {
            return target2 != target1;
        }


        public static GLogic operator <=(GNumber target1, GLogic target2)
        {
            return target2 <= target1;
        }

        public static GLogic operator >=(GNumber target1, GLogic target2)
        {
            return target2 >= target1;
        }

        public static GLogic operator <(GNumber target1, GLogic target2)
        {
            return target2 < target1;
        }

        public static GLogic operator >(GNumber target1, GLogic target2)
        {
            return target2 > target1;
        }
        #endregion

        #region GNumber와 GCustom 비교 연산자 오버로딩
        public static GLogic operator ==(GNumber target1, GCustom target2)
        {
            return new GLogic(false);
        }

        public static GLogic operator !=(GNumber target1, GCustom target2)
        {
            return new GLogic(true);
        }


        public static GLogic operator <=(GNumber target1, GCustom target2)
        {
            return new GLogic(false);
        }

        public static GLogic operator >=(GNumber target1, GCustom target2)
        {
            return new GLogic(false);
        }

        public static GLogic operator <(GNumber target1, GCustom target2)
        {
            return new GLogic(true);
        }

        public static GLogic operator >(GNumber target1, GCustom target2)
        {
            return new GLogic(true);
        }
        #endregion
        
        #region GNumber와 GList 비교 연산자 오버로딩
        // 이미 GList에 정의됨, 역방향 오버로딩만 정의
        public static GLogic operator ==(GNumber target1, GList target2)
        {
            return target2 == target1;
        }

        public static GLogic operator !=(GNumber target1, GList target2)
        {
            return target2 != target1;
        }

        public static GLogic operator <=(GNumber target1, GList target2)
        {
            return target2 <= target1;
        }

        public static GLogic operator >=(GNumber target1, GList target2)
        {
            return target2 >= target1;
        }

        public static GLogic operator <(GNumber target1, GList target2)
        {
            return target2 < target1;
        }

        public static GLogic operator >(GNumber target1, GList target2)
        {
            return target2 > target1;
        }
        #endregion
    }
}
