using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Extension.DataTypes
{
    public class GList : GObject
    {
        public List<GObject> List { get; }

        public GList()
        {
            List = new List<GObject>();
        }

        public GList(GObject obj) : this()
        {
            List.Add(obj);
        }

        public GList(List<GObject> list)
        {
            List = list;
        }

        public override GCustom ToGCustom()
        {
            return new GCustom();
        }

        public override GLogic ToGLogic()
        {
            return new GLogic(List.Any());
        }

        public override GNumber ToGNumber()
        {
            return new GNumber(List.Count);
        }

        public override GString ToGString()
        {
            IEnumerable<string> arr = from item in List select item.ToGString().String;
            return new GString("[" + string.Join(", ", arr) + "]");
        }

        public override GList ToGList()
        {
            return this;
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

        #region GList와 GList 비교 연산자 오버로딩
        public static GLogic operator ==(GList target1, GList target2)
        {
            var count = target1.List.Count;
            if (count != target2.List.Count)
            {
                return new GLogic(false);
            }

            for(int i=0; i<count; i++)
            {
                if (target1.List.ElementAt(i) != target2.List.ElementAt(i))
                {
                    return new GLogic(false);
                }
            }

            return new GLogic(true);
        }

        public static GLogic operator !=(GList target1, GList target2)
        {
            GLogic logic = target1 == target2;
            logic.Logic = !logic.Logic;
            return logic;
        }

        public static GLogic operator <=(GList target1, GList target2)
        {
            return new GLogic(target1.List.Count <= target2.List.Count);
        }

        public static GLogic operator >=(GList target1, GList target2)
        {
            return new GLogic(target1.List.Count >= target2.List.Count);
        }

        public static GLogic operator <(GList target1, GList target2)
        {
            return new GLogic(target1.List.Count < target2.List.Count);
        }

        public static GLogic operator >(GList target1, GList target2)
        {
            return new GLogic(target1.List.Count > target2.List.Count);
        }
        #endregion

        #region GList와 GNumber 비교 연산자 오버로딩
        public static GLogic operator ==(GList target1, GNumber target2)
        {
            return target1.ToGNumber() == target2;
        }

        public static GLogic operator !=(GList target1, GNumber target2)
        {
            return target1.ToGNumber() != target2;
        }

        public static GLogic operator <=(GList target1, GNumber target2)
        {
            return target1.ToGNumber() <= target2;
        }

        public static GLogic operator >=(GList target1, GNumber target2)
        {
            return target1.ToGNumber() >= target2;
        }

        public static GLogic operator <(GList target1, GNumber target2)
        {
            return target1.ToGNumber() < target2;
        }

        public static GLogic operator >(GList target1, GNumber target2)
        {
            return target1.ToGNumber() > target2;
        }
        #endregion

        #region GList와 GString 비교 연산자 오버로딩
        public static GLogic operator ==(GList target1, GString target2)
        {
            return target1.ToGString() == target2;
        }

        public static GLogic operator !=(GList target1, GString target2)
        {
            return target1.ToGString() != target2;
        }

        public static GLogic operator <=(GList target1, GString target2)
        {
            return target1.ToGString() <= target2;
        }

        public static GLogic operator >=(GList target1, GString target2)
        {
            return target1.ToGString() >= target2;
        }

        public static GLogic operator <(GList target1, GString target2)
        {
            return target1.ToGString() < target2;
        }

        public static GLogic operator >(GList target1, GString target2)
        {
            return target1.ToGString() > target2;
        }
        #endregion

        #region GList와 GLogic 비교 연산자 오버로딩
        public static GLogic operator ==(GList target1, GLogic target2)
        {
            return new GLogic(target1.List.Any() == target2);
        }

        public static GLogic operator !=(GList target1, GLogic target2)
        {
            return new GLogic(target1.List.Any() != target2);
        }

        public static GLogic operator <=(GList target1, GLogic target2)
        {
            return target1 == target2;
        }

        public static GLogic operator >=(GList target1, GLogic target2)
        {
            return target1 == target2;
        }

        public static GLogic operator <(GList target1, GLogic target2)
        {
            return target1 != target2;
        }

        public static GLogic operator >(GList target1, GLogic target2)
        {
            return target1 != target2;
        }
        #endregion

        #region GList와 GCustom 비교 연산자 오버로딩
        public static GLogic operator ==(GList target1, GCustom target2)
        {
            return new GLogic(false);
        }

        public static GLogic operator !=(GList target1, GCustom target2)
        {
            return new GLogic(true);
        }

        public static GLogic operator <=(GList target1, GCustom target2)
        {
            return new GLogic(false);
        }

        public static GLogic operator >=(GList target1, GCustom target2)
        {
            return new GLogic(false);
        }

        public static GLogic operator <(GList target1, GCustom target2)
        {
            return new GLogic(true);
        }

        public static GLogic operator >(GList target1, GCustom target2)
        {
            return new GLogic(true);
        }
        #endregion
    }
}