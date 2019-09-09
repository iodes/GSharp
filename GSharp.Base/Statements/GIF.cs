using System;
using System.Text;
using System.Collections.Generic;
using GSharp.Base.Utilities;
using GSharp.Base.Cores;

namespace GSharp.Base.Statements
{
    [Serializable]
    public class GIf : GStatement
    {
        #region Constants
        private readonly List<GStatement> listStatement = new List<GStatement>();
        #endregion

        #region Properties
        public GObject Logic { get; set; }
        #endregion

        #region Initializer
        public GIf(GObject logicValue)
        {
            Logic = logicValue;
        }
        #endregion

        public void Append(GStatement obj)
        {
            listStatement.Add(obj);
        }

        public override string ToSource()
        {
            var builderCode = new StringBuilder();
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
