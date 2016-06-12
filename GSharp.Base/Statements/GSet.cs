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

            if (GSettableObject.SettableType == typeof(object))
            {
                return string.Format("{0} = {1};\n", GSettableObject.ToSource(), Value.ToSource());
            }

            else if (GSettableObject.SettableType == typeof(bool))
            {
                return string.Format("{0} = {1}.ToBool();\n", GSettableObject.ToSource(), Value.ToSource());
            }

            else if (GSettableObject.SettableType == typeof(string))
            {
                return string.Format("{0} = {1}.ToText();\n", GSettableObject.ToSource(), Value.ToSource());
            }

            else if (GSettableObject.SettableType == typeof(double))
            {
                return string.Format("{0} = {1}.ToNumber();\n", GSettableObject.ToSource(), Value.ToSource());
            }

            else if (GSharpUtils.IsNumberType(GSettableObject.SettableType))
            {
                return string.Format("{0} = ({2}){1}.ToNumber();\n", GSettableObject.ToSource(), Value.ToSource(), GSettableObject.SettableType.FullName);
            }

            return string.Format("{0} = ({2}){1}.ToCustom<{2}>();\n", GSettableObject.ToSource(), Value.ToSource(), GSettableObject.SettableType.FullName);
        }
    }
}
