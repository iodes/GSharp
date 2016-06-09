using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSharp.Extension.DataTypes
{
    public abstract class GObject
    {
        public virtual Type ObjectType { get; } = typeof(object);

        public virtual GCustom ToGCustom()
        {
            return new GCustom();
        }

        public virtual GLogic ToGLogic()
        {
            return new GLogic();
        }

        public virtual GNumber ToGNumber()
        {
            return new GNumber();
        }

        public virtual GString ToGString()
        {
            return new GString();
        }

        public virtual GList ToGList()
        {
            return new GList(this);
        }

        public abstract GLogic EqualOperator(GObject target);
        public abstract GLogic NotEqualOperator(GObject target);
        public abstract GLogic LessEqualOperator(GObject target);
        public abstract GLogic MoreEqualOperator(GObject target);
        public abstract GLogic LessOperator(GObject target);
        public abstract GLogic MoreOperator(GObject target);

        public static GLogic operator ==(GObject target1, GObject target2)
        {
            return target1.EqualOperator(target2);
        }

        public static GLogic operator !=(GObject target1, GObject target2)
        {
            return target1.NotEqualOperator(target2);
        }

        public static GLogic operator <=(GObject target1, GObject target2)
        {
            return target1.LessEqualOperator(target2);
        }

        public static GLogic operator >=(GObject target1, GObject target2)
        {
            return target1.MoreEqualOperator(target2);
        }

        public static GLogic operator <(GObject target1, GObject target2)
        {
            return target1.LessOperator(target2);
        }

        public static GLogic operator >(GObject target1, GObject target2)
        {
            return target1.MoreOperator(target2);
        }
    }
}
