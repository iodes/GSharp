using GSharp.Cores;

namespace GSharp.Objects
{
    public class GCompute : GObject
    {
        #region 속성
        public GObject FirstPart { get; set; }

        public GObject SecondPart { get; set; }

        public OperatorType Operator { get; set; }
        #endregion

        #region 열거형
        public enum OperatorType
        {
            PLUS,
            MINUS,
            MULTIPLY,
            DIVISION
        }
        #endregion

        #region 생성자
        public GCompute(GObject firstPart, GObject secondPart, OperatorType operatorType)
        {
            FirstPart = firstPart;
            SecondPart = secondPart;
            Operator = operatorType;
        }
        #endregion

        public override string ToSource()
        {
            string operatorText = string.Empty;
            switch (Operator)
            {
                case OperatorType.PLUS:
                    operatorText = "+";
                    break;

                case OperatorType.MINUS:
                    operatorText = "-";
                    break;

                case OperatorType.MULTIPLY:
                    operatorText = "*";
                    break;

                case OperatorType.DIVISION:
                    operatorText = "/";
                    break;
            }

            return string.Format
                (
                    "({0} {1} {2})",
                    FirstPart.ToSource(),
                    operatorText,
                    SecondPart.ToSource()
                );
        }
    }
}
