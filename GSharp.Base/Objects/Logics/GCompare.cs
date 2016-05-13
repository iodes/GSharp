using System;
using GSharp.Base.Cores;
using GSharp.Base.Utilities;

namespace GSharp.Base.Objects.Logics
{
    [Serializable]
    public class GCompare<T> : GLogic where T : GObject
    {
        #region 속성
        public T FirstPart { get; set; }

        public T SecondPart { get; set; }

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
        public GCompare(T firstPart, ConditionType conditionType, T secondPart)
        {
            FirstPart = firstPart;
            Condition = conditionType;
            SecondPart = secondPart;
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
                    conditionText = ">=";
                    break;
            }

            return string.Format
            (
                "({0} {1} {2})",
                FirstPart.ToSource(),
                conditionText,
                SecondPart.ToSource()
            );
        }
    }
}
