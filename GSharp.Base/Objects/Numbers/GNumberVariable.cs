using System;
using GSharp.Base.Cores;

namespace GSharp.Base.Objects.Numbers
{
    [Serializable]
    public class GNumberVariable : GNumber, IVariable
    {
        #region 속성
        public string Name { get; set; }
        #endregion

        #region 생성자
        public GNumberVariable(string valueName)
        {
            Name = valueName;
        }
        #endregion

        public override string ToSource()
        {
            return Name;
        }
    }
}
