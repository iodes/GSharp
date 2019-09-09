using System;
using System.Text;
using System.Collections.Generic;
using GSharp.Base.Utilities;
using GSharp.Base.Cores;

namespace GSharp.Base.Statements
{
    [Serializable]
    public class GIfElse : GStatement
    {
        #region Properties
        public GObject Logic { get; set; }

        public List<GStatement> IfChild { get; } = new List<GStatement>();

        public List<GStatement> ElseChild { get; } = new List<GStatement>();
        #endregion

        #region Initializer
        public GIfElse(GObject logicValue)
        {
            Logic = logicValue;
        }
        #endregion

        public void AppendToIf(GStatement statement)
        {
            IfChild.Add(statement);
        }

        public void AppendToElse(GStatement statement)
        {
            ElseChild.Add(statement);
        }

        public override string ToSource()
        {
            var builderCode = new StringBuilder();
            builderCode.AppendFormat("if ({0}.ToBool())\n{{\n", Logic.ToSource());

            foreach (GStatement statement in IfChild)
            {
                builderCode.AppendFormat("{0}", ConvertAssistant.Indentation(statement.ToSource()));
            };

            builderCode.Append("}\nelse\n{\n");

            foreach (GStatement statement in ElseChild)
            {
                builderCode.AppendFormat("{0}", ConvertAssistant.Indentation(statement.ToSource()));
            };

            builderCode.Append("}\n");

            return builderCode.ToString();
        }
    }
}
