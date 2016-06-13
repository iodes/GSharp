using System;
using GSharp.Base.Cores;
using GSharp.Base.Utilities;

namespace GSharp.Base.Objects.Logics
{
    [Serializable]
    public class GCompare : GLogic
    {
        #region 속성
        public GObject FirstPart { get; set; }

        public GObject SecondPart { get; set; }

        public ConditionType Condition { get; set; }

        public string ConditionText
        {
            get
            {
                string conditionText = string.Empty;
                switch (Condition)
                {
                    case ConditionType.EQUAL:
                        conditionText = "Equal";
                        break;

                    case ConditionType.NOT_EQUAL:
                        conditionText = "NotEqual";
                        break;

                    case ConditionType.LESS_THEN:
                        conditionText = "Less";
                        break;

                    case ConditionType.LESS_THEN_OR_EQUAL:
                        conditionText = "LessEqual";
                        break;

                    case ConditionType.GREATER_THEN:
                        conditionText = "Greater";
                        break;

                    case ConditionType.GREATER_THEN_OR_EQUAL:
                        conditionText = "GreaterEqual";
                        break;
                }

                return conditionText;
            }
        }
        #endregion

        #region 열거형
        public enum ConditionType
        {
            EQUAL,
            NOT_EQUAL,
            LESS_THEN,
            LESS_THEN_OR_EQUAL,
            GREATER_THEN,
            GREATER_THEN_OR_EQUAL
        }
        #endregion

        #region 생성자
        public GCompare(GObject firstPart, ConditionType conditionType, GObject secondPart)
        {
            FirstPart = firstPart;
            Condition = conditionType;
            SecondPart = secondPart;
        }
        #endregion

        public override string ToSource()
        {
            return string.Format
            (
                "({0}.Is{1}Than({2}))",
                FirstPart.ToSource(),
                ConditionText,
                SecondPart.ToSource()
            );
        }
    }
}
