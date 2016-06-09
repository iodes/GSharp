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
        public ISettable Variable { get; set; }
        public GObject Value { get; set; }
        #endregion

        #region 생성자
        public GSet(ISettable variable, GObject value)
        {
            Variable = variable;
            Value = value;
        }
        #endregion

        public override string ToSource()
        {
            var builder = new StringBuilder();

            string valueName;
            if (Value is GLogic)
            {
                valueName = "Logic";
            }
            else if (Value is GNumber)
            {
                valueName = "Number";
            }
            else if (Value is GString)
            {
                valueName = "String";
            }
            else
            {
                valueName = "Custom";
            }

            builder.AppendFormat("{0} = {0}.ToG{1}();\n", Variable.ToSource(), valueName);
            builder.AppendFormat("{0}.{1} = {2}.ToG{1}().{1};\n", Variable.ToSource(), valueName, Value.ToSource());

            return builder.ToString();
        }
    }
}
