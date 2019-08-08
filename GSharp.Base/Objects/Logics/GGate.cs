using System;
using GSharp.Base.Cores;
using GSharp.Common.Objects;

namespace GSharp.Base.Objects.Logics
{
    [Serializable]
    public class GGate : GLogic
    {
        #region Properties
        public GObject FirstPart { get; }

        public GObject SecondPart { get; }

        public GateType Gate { get; }

        public string GateText
        {
            get
            {
                switch (Gate)
                {
                    case GateType.Or:
                        return "||";

                    case GateType.And:
                        return "&&";
                    
                    default:
                        return string.Empty;
                }
            }
        }
        #endregion

        #region Initializer
        public GGate(GObject firstPart, GateType gateType, GObject secondPart)
        {
            FirstPart = firstPart;
            SecondPart = secondPart;
            Gate = gateType;
        }
        #endregion

        public override string ToSource()
        {
            var firstPart = FirstPart?.ToSource() + (!(FirstPart is GLogic) ? ".ToBool()" : string.Empty);
            var secondPart = SecondPart?.ToSource() + (!(SecondPart is GLogic) ? ".ToBool()" : string.Empty);
            
            return $"({firstPart} {GateText} {secondPart})";
        }
    }
}
