using System.Text;
using System.Collections.Generic;
using GSharp.Cores;
using GSharp.Utilities;

namespace GSharp.Scopes
{
    public class GVoid : GScope
    {
        #region 속성
        public string Name { get; set; }
        #endregion

        #region 객체
        private List<GStatement> listStatement = new List<GStatement>();
        #endregion

        #region 생성자
        public GVoid(string valueName)
        {
            Name = valueName;
        }
        #endregion

        #region 사용자 함수
        public void Append(GStatement valueStatement)
        {
            listStatement.Add(valueStatement);
        }
        #endregion

        public override string ToSource()
        {
            StringBuilder builderCode = new StringBuilder();
            builderCode.AppendFormat
                (
                    "public void {0}()\n{{\n",
                    Name
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
                    "}"
                );

            return builderCode.ToString();
        }
    }
}
