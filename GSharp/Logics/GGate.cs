using GSharp.Cores;

namespace GSharp.Logics
{
    public class GGate : GLogic
    {
        #region 속성
        public GCompare FirstPart { get; set; }

        public GCompare SecondPart { get; set; }

        public GateType Gate { get; set; }
        #endregion

        #region 열거형
        public enum GateType
        {
            OR,
            AND
        }
        #endregion

        #region 생성자
        public GGate(GCompare firstPart, GCompare secondPart, GateType gateType)
        {
            FirstPart = firstPart;
            SecondPart = secondPart;
            Gate = gateType;
        }
        #endregion

        public override string ToSource()
        {
            string gateText = string.Empty;
            switch (Gate)
            {
                case GateType.OR:
                    gateText = "||";
                    break;

                case GateType.AND:
                    gateText = "&&";
                    break;
            }

            return string.Format
                (
                    "({0} {1} {2})",
                    FirstPart.ToSource(),
                    gateText,
                    SecondPart.ToSource()
                );
        }
    }
}
