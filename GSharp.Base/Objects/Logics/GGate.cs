using System;
using GSharp.Base.Cores;

namespace GSharp.Base.Objects.Logics
{
    [Serializable]
    public class GGate : GLogic
    {
        #region 속성
        public GLogic FirstPart { get; set; }

        public GLogic SecondPart { get; set; }

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
        public GGate(GLogic firstPart, GateType gateType, GLogic secondPart)
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
