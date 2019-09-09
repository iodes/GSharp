using GSharp.Base.Cores;

namespace GSharp.Base.Objects.Strings
{
    public class GNumberToString : GString
    {
        #region Properties
        public GObject TargetNumber { get; }
        #endregion

        #region Initializer
        public GNumberToString(GObject number)
        {
            TargetNumber = number;
        }
        #endregion

        public override string ToSource()
        {
            return $"{TargetNumber?.ToSource()}.ToText()";
        }
    }
}
