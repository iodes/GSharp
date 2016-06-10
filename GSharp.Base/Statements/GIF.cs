using System;
using System.Text;
using System.Collections.Generic;
using GSharp.Base.Objects;
using GSharp.Base.Utilities;
using GSharp.Base.Cores;

namespace GSharp.Base.Statements
{
    [Serializable]
    public class GIf : GStatement
    {
        #region 속성
        public GObject Logic { get; set; }
        #endregion

        #region 객체
        private List<GStatement> listStatement = new List<GStatement>();
        #endregion

        #region 생성자
        public GIf(GObject logicValue)
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
                    "if ({0}.ToBool())\n{{\n",
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
