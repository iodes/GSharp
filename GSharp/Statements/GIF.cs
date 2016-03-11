using System.Text;
using System.Collections.Generic;
using GSharp.Cores;
using GSharp.Utilities;

namespace GSharp.Statements
{
    public class GIF : GStatement
    {
        #region 속성
        public GLogic Logic { get; set; }
        #endregion

        #region 객체
        private List<GStatement> listStatement = new List<GStatement>();
        #endregion

        #region 생성자
        public GIF(GLogic logicValue)
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
