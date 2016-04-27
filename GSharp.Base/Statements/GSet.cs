using System;
using GSharp.Base.Cores;
using GSharp.Base.Objects;
using GSharp.Base.Utilities;

namespace GSharp.Base.Statements
{
    [Serializable]
    public class GSet : GStatement
    {
        #region 속성
        public GVariable Variable { get; set; }

        public GObject Value { get; set; }
        #endregion

        #region 생성자
        public GSet(ref GVariable valueVariable, GObject valueObject)
        {
            Variable = valueVariable;
            Value = valueObject;

            if (valueObject.GetType() == typeof(GString))
            {
                valueVariable.CurrentType = GVariable.VariableType.STRING;
            }
            else if (valueObject.GetType() == typeof(GNumber))
            {
                valueVariable.CurrentType = GVariable.VariableType.NUMBER;
            }
        }
        #endregion

        public override string ToSource()
        {
            return string.Format
                (
                    "{0} = ({1}){2};",
                    Variable.ToSource(),
                    ConvertAssistant.ResolveType(Value),
                    Value.ToSource()
                );
        }
    }
}
