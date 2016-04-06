using System;
using GSharp.Base.Cores;
using System.Collections.Generic;
using System.Text;
using GSharp.Base.Utilities;

namespace GSharp.Base.Statements
{
    public class GLoop : GStatement
    {
        #region 속성
        public GObject Object { get; set; }
        #endregion

        #region 객체
        private List<GStatement> listStatement = new List<GStatement>();
        #endregion

        #region 생성자
        public GLoop(){}

        public GLoop(GObject obj)
        {
            Object = obj;
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

            if (Object != null)
            {
                String varName = "i";

                builderCode.AppendFormat("for (int {0} = 0; {0} < {1}; {0}++)\n{{\n", varName, Object.ToSource());
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
