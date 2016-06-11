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
        public GSettableObject Variable { get; set; }
        public GObject Value { get; set; }
        #endregion

        #region 생성자
        public GSet(GSettableObject variable, GObject value)
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
                valueName = "Bool";
            }
            else if (Value is GNumber)
            {
                valueName = "Number";
            }
            else if (Value is GString)
            {
                valueName = "Text";
            }
            else
            {
                builder.AppendFormat("{0} = {1};\n", Variable.ToSource(), Value.ToSource());
                return builder.ToString();
            }
            
            builder.AppendFormat("{0} = {1}.To{2}();\n", Variable.ToSource(), Value.ToSource(), valueName);

            return builder.ToString();
        }
    }
}
