using System;
using System.Text;
using System.Collections.Generic;
using GSharp.Base.Objects;
using GSharp.Base.Utilities;
using GSharp.Base.Cores;

namespace GSharp.Base.Statements
{
    [Serializable]
    public class GIfElse : GStatement
    {
        #region 속성
        public GLogic Logic { get; set; }
        #endregion

        #region 객체
        public List<GStatement> IfChild
        {
            get
            {
                return ifChild;
            }
        }
        private List<GStatement> ifChild = new List<GStatement>();

        public List<GStatement> ElseChild
        {
            get
            {
                return elseChild;
            }
        }
        private List<GStatement> elseChild = new List<GStatement>();
        #endregion

        #region 생성자
        public GIfElse(GLogic logicValue)
        {
            Logic = logicValue;
        }
        #endregion

        #region 사용자 함수
        public void AppendToIf(GStatement statement)
        {
            ifChild.Add(statement);
        }

        public void AppendToElse(GStatement statement)
        {
            elseChild.Add(statement);
        }
        #endregion

        public override string ToSource()
        {
            StringBuilder builderCode = new StringBuilder();
            builderCode.AppendFormat("if ({0})\n{{\n", Logic.ToSource());

            foreach (GStatement statement in ifChild)
            {
                builderCode.AppendFormat("{0}", ConvertAssistant.Indentation(statement.ToSource()));
            };

            builderCode.Append("}\nelse\n{\n");

            foreach (GStatement statement in elseChild)
            {
                builderCode.AppendFormat("{0}", ConvertAssistant.Indentation(statement.ToSource()));
            };

            builderCode.Append("}\n");

            return builderCode.ToString();
        }
    }
}
