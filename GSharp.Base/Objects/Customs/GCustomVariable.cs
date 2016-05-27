using System;
using GSharp.Base.Cores;

namespace GSharp.Base.Objects.Customs
{
    [Serializable]
    public class GCustomVariable : GCustom, IVariable
    {
        #region 속성
        public override Type CustomType
        {
            get
            {
                return VariableType;
            }
        }

        public Type VariableType
        {
            get
            {
                return _VariableType;
            }
        }
        private Type _VariableType;

        public string Name { get; set; }
        #endregion

        #region 생성자
        public GCustomVariable(Type variableType, string valueName)
        {
            _VariableType = variableType;
            Name = valueName;
        }
        #endregion

        public override string ToSource()
        {
            return Name;
        }
    }
}
