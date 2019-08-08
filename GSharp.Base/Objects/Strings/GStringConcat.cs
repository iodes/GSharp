using System;
using GSharp.Base.Cores;

namespace GSharp.Base.Objects.Strings
{
    [Serializable]
    public class GStringConcat : GString
    {
        #region Properties
        public GObject FirstPart { get; }

        public GObject SecondPart { get; }
        #endregion

        #region Initializer
        public GStringConcat(GObject first, GObject second)
        {
            FirstPart = first;
            SecondPart = second;
        }
        #endregion

        public override string ToSource()
        {
            var firstPart = FirstPart?.ToSource() + (!(FirstPart is GString) ? ".ToText()" : string.Empty);
            var secondPart = SecondPart?.ToSource() + (!(SecondPart is GString) ? ".ToText()" : string.Empty);
            
            return $"({firstPart} + {secondPart})";
        }
    }
}
