using System;
using GSharp.Base.Cores;
using GSharp.Base.Utilities;
using GSharp.Common.Objects;

namespace GSharp.Base.Objects.Logics
{
    [Serializable]
    public class GCompare : GLogic
    {
        #region Properties
        public GObject FirstPart { get; }

        public GObject SecondPart { get; }

        public ConditionType Condition { get; }

        public string ConditionText
        {
            get
            {
                switch (Condition)
                {
                    case ConditionType.Equal:
                        return "Equal";
                    
                    case ConditionType.NotEqual:
                        return "NotEqual";
                    
                    case ConditionType.LessThen:
                        return "Less";
                    
                    case ConditionType.LessThenOrEqual:
                        return "LessEqual";
                    
                    case ConditionType.GreaterThen:
                        return "Greater";
                    
                    case ConditionType.GreaterThenOrEqual:
                        return "GreaterEqual";
                    
                    default:
                        return string.Empty;
                }
            }
        }
        #endregion
        
        #region Initializer
        public GCompare(GObject firstPart, ConditionType conditionType, GObject secondPart)
        {
            FirstPart = firstPart;
            Condition = conditionType;
            SecondPart = secondPart;
        }
        #endregion

        public override string ToSource()
        {
            return $"({FirstPart.ToSource()}.Is{ConditionText}Than({SecondPart.ToSource()}))";
        }
    }
}
