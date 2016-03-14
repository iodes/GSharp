using GSharp.Base.Cores;
using GSharp.Base.Utilities;

namespace GSharp.Base.Logics
{
    public class GCompare : GLogic
    {
        #region 속성
        public GObject FirstPart { get; set; }

        public GObject SecondPart { get; set; }

        public ConditionType Condition { get; set; }
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
        public GCompare(GObject firstPart, GObject secondPart, ConditionType conditionType)
        {
            FirstPart = firstPart;
            SecondPart = secondPart;
            Condition = conditionType;
        }
        #endregion

        public override string ToSource()
        {
            string conditionText = string.Empty;
            switch (Condition)
            {
                case ConditionType.EQUAL:
                    conditionText = "==";
                    break;

                case ConditionType.NOT_EQUAL:
                    conditionText = "!=";
                    break;

                case ConditionType.LESS_THEN:
                    conditionText = "<";
                    break;

                case ConditionType.LESS_THEN_OR_EQUAL:
                    conditionText = "<=";
                    break;

                case ConditionType.GREATER_THEN:
                    conditionText = ">";
                    break;

                case ConditionType.GREATER_THEN_OR_EQUAL:
                    conditionText = "<=";
                    break;
            }

            if (Condition == ConditionType.EQUAL)
            {
                return string.Format
                    (
                        "({0}.ToString() {1} {2}.ToString())",
                        FirstPart.ToSource(),
                        conditionText,
                        SecondPart.ToSource()
                    );
            }
            else
            {
                return string.Format
                    (
                        "(({0}){1} {2} ({3}){4})",
                        ConvertAssistant.ResolveType(FirstPart),
                        FirstPart.ToSource(),
                        conditionText,
                        ConvertAssistant.ResolveType(SecondPart),
                        SecondPart.ToSource()
                    );
            }
        }
    }
}
