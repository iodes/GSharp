using System;
using GSharp.Base.Cores;
using GSharp.Common.Objects;

namespace GSharp.Base.Objects.Numbers
{
    [Serializable]
    public class GCompute : GNumber
    {
        #region Properties
        public GObject FirstPart { get; }

        public GObject SecondPart { get; }

        public OperatorType Operator { get; }

        public string OperatorText
        {
            get
            {
                switch (Operator)
                {
                    case OperatorType.Plus:
                        return "+";

                    case OperatorType.Minus:
                        return "-";

                    case OperatorType.Multiply:
                        return "*";

                    case OperatorType.Division:
                        return "/";

                    case OperatorType.Modulo:
                        return "%";
                    
                    default:
                        return string.Empty;
                }
            }
        }
        #endregion
        
        #region Initializer
        public GCompute(GObject firstPart, OperatorType operatorType, GObject secondPart)
        {
            FirstPart = firstPart;
            SecondPart = secondPart;
            Operator = operatorType;
        }
        #endregion

        public override string ToSource()
        {
            var firstPart = FirstPart?.ToSource() + (!(FirstPart is GNumber) ? ".ToNumber()" : string.Empty);
            var secondPart = SecondPart?.ToSource() + (!(SecondPart is GNumber) ? ".ToNumber()" : string.Empty);
            
            return $"({firstPart} {OperatorText} {secondPart})";
        }
    }
}
