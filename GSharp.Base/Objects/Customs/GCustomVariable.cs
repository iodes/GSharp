using System;
using GSharp.Base.Cores;

namespace GSharp.Base.Objects.Customs
{
    [Serializable]
    public class GCustomVariable : GCustom, IVariable
    {
        #region 속성
        public override Type Type
        {
            get
            {
                return _Type;
            }
        }
        private Type _Type;

        public string Name { get; set; }
        #endregion

        #region 생성자
        public GCustomVariable(Type type, string valueName)
        {
            _Type = type;
            Name = valueName;
        }
        #endregion

        public override string ToSource()
        {
            return Name;
        }
    }
}
