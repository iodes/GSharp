using System;
using GSharp.Base.Cores;
using GSharp.Base.Objects;
using GSharp.Base.Utilities;
using System.Text;

namespace GSharp.Base.Statements
{
    [Serializable]
    public class GSet : GStatement
    {
        #region 속성
        public GSettableObject GSettableObject { get; set; }
        public GObject Value { get; set; }
        #endregion

        #region 생성자
        public GSet(GSettableObject settableObject, GObject value)
        {
            GSettableObject = settableObject;
            Value = value;
        }
        #endregion

        public override string ToSource()
        {
            var builder = new StringBuilder();

            if (Variable.SettableType == typeof(object))
            {
                return string.Format("{0} = {1};\n", Variable.ToSource(), Value.ToSource());
            }

            else if (Variable.SettableType == typeof(bool))
            {
                return string.Format("{0} = {1}.ToBool();\n", Variable.ToSource(), Value.ToSource());
            }

            else if (Variable.SettableType == typeof(string))
            {
                return string.Format("{0} = {1}.ToText();\n", Variable.ToSource(), Value.ToSource());
            }

            else if (Variable.SettableType == typeof(double))
            {
                return string.Format("{0} = {1}.ToNumber();\n", Variable.ToSource(), Value.ToSource());
            }

            else if (GSharpUtils.IsNumberType(Variable.SettableType))
            {
                return string.Format("{0} = ({2}){1}.ToNumber();\n", Variable.ToSource(), Value.ToSource(), Variable.SettableType.FullName);
            }

            return string.Format("{0} = ({2}){1}.ToCustom<{2}>();\n", Variable.ToSource(), Value.ToSource(), Variable.SettableType.FullName);
        }
    }
}
