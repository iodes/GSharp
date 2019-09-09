using System;
using GSharp.Base.Cores;
using GSharp.Base.Objects;
using GSharp.Base.Utilities;

namespace GSharp.Base.Statements
{
    [Serializable]
    public class GSet : GStatement
    {
        #region Properties
        public GSettableObject GSettableObject { get; set; }

        public GObject Value { get; set; }
        #endregion

        #region Initializer
        public GSet(GSettableObject settableObject, GObject value)
        {
            GSettableObject = settableObject;
            Value = value;
        }
        #endregion

        public override string ToSource()
        {
            return string.Format("{0} = {1};\n", GSettableObject?.ToSource(), GSharpUtils.CastParameterString(Value, GSettableObject?.SettableType));
        }
    }
}
