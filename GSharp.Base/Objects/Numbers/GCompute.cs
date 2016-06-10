using System;
using GSharp.Base.Cores;

namespace GSharp.Base.Objects.Numbers
{
    [Serializable]
    public class GCompute : GNumber
    {
        #region 속성
        public GObject FirstPart { get; set; }

        public GObject SecondPart { get; set; }

        public OperatorType Operator { get; set; }

        public string OperatorText
        {
            get
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

                    case OperatorType.MODULO:
                        operatorText = "%";
                        break;
                }

                return operatorText;
            }
        }
        #endregion

        #region 열거형
        public enum OperatorType
        {
            PLUS,
            MINUS,
            MULTIPLY,
            DIVISION,
            MODULO
        }
        #endregion

        #region 생성자
        public GCompute(GObject firstPart, OperatorType operatorType, GObject secondPart)
        {
            FirstPart = firstPart;
            SecondPart = secondPart;
            Operator = operatorType;
        }
        #endregion

        public override string ToSource()
        {
            return string.Format("({0}.ToNumber() {1} {2}.ToNumber())", FirstPart?.ToSource(), OperatorText, SecondPart?.ToSource());
        }
    }
}
