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

            return string.Format("{0} = {1};\n", GSettableObject?.ToSource(), GSharpUtils.CastParameterString(Value, GSettableObject?.SettableType));
        }
    }
}
