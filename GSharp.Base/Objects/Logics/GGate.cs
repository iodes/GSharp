using System;
using GSharp.Base.Cores;

namespace GSharp.Base.Objects.Logics
{
    [Serializable]
    public class GGate : GLogic
    {
        #region 속성
        public GObject FirstPart { get; set; }

        public GObject SecondPart { get; set; }

        public GateType Gate { get; set; }

        public string GateText
        {
            get
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

                return gateText;
            }
        }
        #endregion

        #region 열거형
        public enum GateType
        {
            OR,
            AND
        }
        #endregion

        #region 생성자
        public GGate(GObject firstPart, GateType gateType, GObject secondPart)
        {
            FirstPart = firstPart;
            SecondPart = secondPart;
            Gate = gateType;
        }
        #endregion

        public override string ToLogicSource()
        {

            return string.Format
                (
                    "({0}.ToGLogic().Logic {1} {2}.ToGLogic().Logic)",
                    FirstPart.ToSource(),
                    GateText,
                    SecondPart.ToSource()
                );
        }
    }
}
