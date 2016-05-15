using System;
using GSharp.Base.Cores;
using GSharp.Base.Objects;
using GSharp.Base.Utilities;

namespace GSharp.Base.Statements
{
    [Serializable]
    public class GSet<T1, T2> : GStatement where T2 : GObject where T1 : T2, IVariable
    {
        #region 속성
        public T1 Variable { get; set; }
        public T2 Value { get; set; }
        #endregion

        #region 생성자
        public GSet(T1 variable, T2 value)
        {
            Variable = variable;
            Value = value;
        }
        #endregion

        public override string ToSource()
        {
            return string.Format("{0} = {1};", Variable.ToSource(), Value.ToSource());
        }
    }
}
