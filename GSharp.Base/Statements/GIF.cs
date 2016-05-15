using System;
using System.Text;
using System.Collections.Generic;
using GSharp.Base.Cores;
using GSharp.Base.Utilities;

namespace GSharp.Base.Statements
{
    [Serializable]
    public class GIf : GStatement
    {
        #region 속성
        public GLogic Logic { get; set; }
        #endregion

        #region 객체
        private List<GStatement> listStatement = new List<GStatement>();
        #endregion

        #region 생성자
        public GIf(GLogic logicValue)
        {
            Logic = logicValue;
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
            builderCode.AppendFormat
                (
                    "if ({0})\n{{\n",
                    Logic.ToSource()
                );

            foreach (GStatement statement in listStatement)
            {
                builderCode.AppendFormat
                    (
                        "{0}",
                        ConvertAssistant.Indentation(statement.ToSource())
                    );
            };

            builderCode.Append
                (
                    "};"
                );

            return builderCode.ToString();
        }
    }
}
