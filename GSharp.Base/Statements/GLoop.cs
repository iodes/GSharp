using System;
using System.Text;
using System.Collections.Generic;
using GSharp.Base.Cores;
using GSharp.Base.Utilities;
using GSharp.Base.Objects;

namespace GSharp.Base.Statements
{
    [Serializable]
    public class GLoop : GStatement
    {
        #region 반복 번호
        private static int loopNo = 0;

        public static void InitLoopNo()
        {
            loopNo = 0;
        }
        #endregion

        #region 속성
        public GObject GNumber { get; set; }
        #endregion

        #region 객체
        private List<GStatement> listStatement = new List<GStatement>();
        #endregion

        #region 생성자
        public GLoop()
        {

        }

        public GLoop(GObject number)
        {
            GNumber = number;
        }
        #endregion

        #region 사용자 함수
        public void Append(GStatement obj)
        {
            listStatement.Add(obj);
        }
        #endregion

        public override string ToSource()
        {
            StringBuilder builderCode = new StringBuilder();

            if (GNumber != null)
            {
                string varName = "_" + loopNo++;
                builderCode.AppendFormat("for (int {0} = 0; {0} < {1}.ToNumber(); {0}++)\n{{\n", varName, GNumber.ToSource());
            }
            else
            {
                builderCode.AppendLine("while (true)\n{\n");
            }

            foreach (GStatement statement in listStatement)
            {
                builderCode.Append(ConvertAssistant.Indentation(statement.ToSource()));
            };

            builderCode.Append("};");

            return builderCode.ToString();
        }
    }
}
